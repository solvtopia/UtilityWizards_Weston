Imports UtilityWizards.CommonCore.Shared.Common
Imports System.IO

Public Class fMain
    Private Sub fMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.lblProgress.Text = ""
        Me.lblStatus.Text = ""
        Me.tmrTimer.Enabled = True
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        Me.tmrTimer.Enabled = False

        Me.ProcessSouthernImport()

        Me.tmrTimer.Enabled = True
    End Sub

    Private Sub ProcessSouthernImport()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim files_tmp As String = "C:\inetpub\access.utilitywizards.com\wwwroot\import\"
            Dim fPath As String = My.Computer.FileSystem.CombinePath(files_tmp, "accountdata.txt")

            'If My.Computer.FileSystem.FileExists(fPath) Then My.Computer.FileSystem.DeleteFile(fPath)
            'My.Computer.Network.DownloadFile("https://access.utilitywizards.com/import/accountdata.txt", fPath)

            Dim cmd As New SqlClient.SqlCommand

            ' turn the file into a table
            Me.lblStatus.Text = "Processing File to Table ..."
            My.Application.DoEvents()

            Dim tbl As DataTable = Me.ProcessFileToTable(fPath, "Customers_New")

            ' add the records to the database
            If Me.chkCustomers.Checked Then
                Me.lblStatus.Text = "Saving Customers to Database ..."
                My.Application.DoEvents()

                cmd = New SqlClient.SqlCommand("TRUNCATE TABLE [Customers_New]", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()

                Using bulkCopy As SqlClient.SqlBulkCopy = New SqlClient.SqlBulkCopy(cn)
                    bulkCopy.BulkCopyTimeout = 2000
                    bulkCopy.DestinationTableName = "Customers_New"
                    Try
                        ' Write from the source to the destination.
                        bulkCopy.WriteToServer(tbl)

                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                End Using
            End If

            If Me.rbAllDupllicates.Checked Or (Me.chkCustomers.Checked And Me.rbSelectedDuplicates.Checked) Then
                Me.lblStatus.Text = "Removing Duplicate Customer Records ..."
                My.Application.DoEvents()
                Me.FixDuplicates("customers_new")
            End If

            If Me.chkLocations.Checked Then
                Me.lblStatus.Text = "Saving Locations to Database ..."
                My.Application.DoEvents()

                cmd = New SqlClient.SqlCommand("TRUNCATE TABLE [Locations_New]", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()

                ' remove the clientid field
                For x As Integer = 0 To tbl.Columns.Count - 1
                    If tbl.Columns(x).ColumnName.ToLower = "clientid" Then
                        tbl.Columns.RemoveAt(x)
                        Exit For
                    End If
                Next

                Using bulkCopy As SqlClient.SqlBulkCopy = New SqlClient.SqlBulkCopy(cn)
                    ' map the columns for this table from the file table
                    For Each c As DataColumn In tbl.Columns
                        bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName)
                    Next

                    bulkCopy.BulkCopyTimeout = 2000
                    bulkCopy.DestinationTableName = "Locations_New"
                    Try
                        ' Write from the source to the destination.
                        bulkCopy.WriteToServer(tbl)

                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                End Using
            End If

            If Me.rbAllDupllicates.Checked Or (Me.chkLocations.Checked And Me.rbSelectedDuplicates.Checked) Then
                Me.lblStatus.Text = "Removing Duplicate Location Records ..."
                My.Application.DoEvents()
                Me.FixDuplicates("locations_new")
            End If

            ' finish everything up
            Me.lblStatus.Text = "Cleaning Up ..."
            My.Application.DoEvents()

            My.Computer.FileSystem.RenameFile(fPath, "accountdata." & Now.ToString.Replace("/", "").Replace(":", "").Replace("AM", "").Replace("PM", "").Replace(" ", "") & ".txt.processed")

            Me.lblStatus.Text = "Finished!"
            Me.lblProgress.Text = ""
            My.Application.DoEvents()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.FileProcessor))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Function ProcessFileToTable(ByVal fPath As String, ByVal tblName As String) As DataTable
        Dim retVal As New DataTable
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            ' build a table to store the records in
            Dim cmd As New SqlClient.SqlCommand("SELECT * FROM [" & tblName & "]", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader(CommandBehavior.SchemaOnly)
            For x As Integer = 0 To rs.FieldCount - 1
                Dim col As New DataColumn
                col.ColumnName = rs.GetName(x)
                col.DataType = rs.GetProviderSpecificFieldType(x)

                retVal.Columns.Add(col)
            Next
            rs.Close()
            cmd.Cancel()

            ' first open the file and read in all the records
            Dim htFields As New Hashtable
            If My.Computer.FileSystem.FileExists(fPath) Then
                Dim lineCount As Integer = File.ReadAllLines(fPath).Length
                Me.pbrProgress.Value = 0
                Me.pbrProgress.Maximum = lineCount

                Dim rdr As New System.IO.StreamReader(fPath)
                Do While rdr.Peek() <> -1
                    Dim s As String = rdr.ReadLine
                    Dim acctNum As String = ""

                    If Not IsNumeric(s.Substring(0, 3)) Then
                        ' this is the header line so read all the fields and create new items in the hashtable
                        Dim z As Integer = 0
                        For Each f As String In s.Split("|"c)
                            htFields.Add(f, z)
                            z += 1
                            My.Application.DoEvents()
                        Next
                    Else
                        ' this is a data line so read all the values and assign them to their fields
                        Dim rec As List(Of String) = s.Split("|"c).ToList
                        Dim r As DataRow = retVal.NewRow
                        For Each key As String In htFields.Keys
                            Select Case True
                                Case r(key).GetType Is GetType(SqlTypes.SqlInt32)
                                    r(key) = rec(htFields(key)).ToInteger
                                Case r(key).GetType Is GetType(SqlTypes.SqlDateTime)
                                    r(key) = rec(htFields(key)).ToDate("1/1/1900")
                                Case r(key).GetType Is GetType(SqlTypes.SqlBoolean)
                                    r(key) = rec(htFields(key)).ToBoolean
                                Case r(key).GetType Is GetType(SqlTypes.SqlDouble)
                                    r(key) = rec(htFields(key)).ToDouble
                                Case Else
                                    r(key) = rec(htFields(key)).ToString
                            End Select
                            My.Application.DoEvents()
                        Next

                        ' add the record to the "table" and reset the record hashtable
                        r("ID") = Me.pbrProgress.Value
                        If tblName.ToLower = "customers_new" Then r("ClientID") = 4
                        r("dtInserted") = Now
                        r("dtUpdated") = Now
                        r("Active") = True
                        retVal.Rows.Add(r)
                        retVal.AcceptChanges()
                    End If

                    Me.pbrProgress.Value += 1
                    Me.lblProgress.Text = "Record " & Me.pbrProgress.Value & "/" & Me.pbrProgress.Maximum & " (" & FormatPercent(Me.pbrProgress.Value / Me.pbrProgress.Maximum, 1) & ")"
                    My.Application.DoEvents()
                Loop
                rdr.Close()
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.FileProcessor))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Private Sub FixDuplicates(ByVal tbl As String)
        Dim fld As String = If(tbl.ToLower = "customers_new", "AccountNum", "LocationNum")

        Dim cn As New SqlClient.SqlConnection(ConnectionString)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim sql As String = ""
            Dim fieldNames As String = ""
            Dim firstRun As Boolean = True

            ' setup the progress bar
            Dim cmd1 As New SqlClient.SqlCommand("SELECT COUNT(DISTINCT [" & fld & "]) AS [TotalCount] FROM [" & tbl & "];", cn1)
            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
            Dim rs1 As SqlClient.SqlDataReader = cmd1.ExecuteReader
            If rs1.Read Then
                Me.pbrProgress.Value = 0
                Me.pbrProgress.Maximum = rs1("TotalCount").ToString.ToInteger
            End If

            cmd1 = New SqlClient.SqlCommand("SELECT DISTINCT [" & fld & "] FROM [" & tbl & "];", cn1)
            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
            rs1 = cmd1.ExecuteReader
            Do While rs1.Read
                Dim cmd As SqlClient.SqlCommand
                Dim rs As SqlClient.SqlDataReader
                Dim fieldList As New Hashtable
                sql = ""

                ' make sure we only have 1 record for the customer
                Dim recordIds As New List(Of Integer)
                sql = "SELECT * FROM [" & tbl & "] WHERE [" & fld & "] LIKE '" & rs1(fld).ToString & "' ORDER BY [ID];"
                cmd = New SqlClient.SqlCommand(sql, cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader

                Do While rs.Read
                    If Not recordIds.Contains(rs("ID").ToString.ToInteger) Then recordIds.Add(rs("ID").ToString.ToInteger)

                    ' get a list of all the fields in the customer table
                    For x As Integer = 0 To rs.FieldCount - 1
                        If Not fieldList.Contains(rs.GetName(x)) Then fieldList.Add(rs.GetName(x), Nothing)
                        If firstRun Then
                            If rs.GetName(x).ToLower <> "addlvalues" And rs.GetName(x).ToLower <> "id" And rs.GetName(x).ToLower <> "locationnum" And rs.GetName(x).ToLower <> "searchaddress" Then
                                If fieldNames = "" Then fieldNames = "[" & rs.GetName(x) & "]" Else fieldNames &= ", [" & rs.GetName(x) & "]"
                            End If
                        End If

                        My.Application.DoEvents()
                    Next

                    My.Application.DoEvents()
                Loop
                cmd.Cancel()
                rs.Close()

                ' now combine the records into a single record if we have more than 1
                If recordIds.Count > 1 Then
                    cmd = New SqlClient.SqlCommand(sql, cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    rs = cmd.ExecuteReader
                    Do While rs.Read
                        For x As Integer = 0 To fieldList.Keys.Count - 1
                            Dim f As String = fieldList.Keys(x).ToString

                            ' only update the record if the value isn't null
                            If Not IsDBNull(rs(f)) Then
                                ' make sure the fieldlist value is different
                                If fieldList(f) Is Nothing OrElse fieldList(f).ToString.ToLower <> rs(f).ToString.ToLower Then
                                    If rs(f).ToString.ToLower <> "null" Then
                                        fieldList(f) = rs(f)
                                    End If
                                End If
                            End If

                            My.Application.DoEvents()
                        Next

                        My.Application.DoEvents()
                    Loop
                    cmd.Cancel()
                    rs.Close()

                    ' update the customers and locations table
                    Dim fields As String = ""
                    For x As Integer = 0 To fieldList.Keys.Count - 1
                        Dim f As String = fieldList.Keys(x).ToString

                        If f.ToLower <> "id" And f.ToLower <> "searchaddress" And f.ToLower <> "locationnum" Then
                            If fieldList(f) Is Nothing Then
                                ' null values
                                If fields = "" Then fields = "[" & f & "] = NULL" Else fields &= ", [" & f & "] = NULL"

                            ElseIf f.ToLower = "dtupdated" Then
                                ' date updated
                                If fields = "" Then fields = "[" & f & "] = '" & fieldList(f).ToString & "'" Else fields &= ", [" & f & "] = '" & fieldList(f).ToString & "'"

                            ElseIf f.ToLower = "active" Then
                                ' active flag
                                If fields = "" Then fields = "[" & f & "] = 1" Else fields &= ", [" & f & "] = 1"

                            ElseIf f.ToLower = "zipcode" Then
                                ' zip codes are strings
                                If fields = "" Then fields = "[" & f & "] = '" & fieldList(f).ToString.Trim & "'" Else fields &= ", [" & f & "] = '" & fieldList(f).ToString.Trim & "'"

                            ElseIf f.ToLower.Contains("serial00") Then
                                ' trash can serial numbers are strings
                                If fields = "" Then fields = "[" & f & "] = '" & fieldList(f).ToString & "'" Else fields &= ", [" & f & "] = '" & fieldList(f).ToString & "'"

                            ElseIf IsNumeric(fieldList(f)) Then
                                ' numbers and bits
                                If fields = "" Then fields = "[" & f & "] = " & fieldList(f).ToString Else fields &= ", [" & f & "] = " & fieldList(f).ToString

                            ElseIf IsDate(fieldList(f)) Then
                                ' dates
                                If fields = "" Then fields = "[" & f & "] = '" & fieldList(f).ToString & "'" Else fields &= ", [" & f & "] = '" & fieldList(f).ToString & "'"

                            End If
                        End If

                        My.Application.DoEvents()
                    Next

                    sql = "UPDATE [" & tbl & "] SET " & fields & " WHERE [" & fld & "] LIKE '" & rs1(fld).ToString & "';"
                    cmd = New SqlClient.SqlCommand(sql, cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    cmd.ExecuteNonQuery()
                End If

                firstRun = False

                Me.lblProgress.Text = "Combining " & recordIds.Count & " Record(s) for " & fld & " " & rs1(fld) & " (" & FormatPercent(Me.pbrProgress.Value / Me.pbrProgress.Maximum, 1) & ")"
                Me.pbrProgress.Value += 1
                My.Application.DoEvents()
            Loop
            cmd1.Cancel()
            rs1.Close()

            ' delete duplicates
            sql = "DELETE FROM [" & tbl & "] WHERE ID NOT IN (SELECT MIN(ID) FROM [" & tbl & "] GROUP BY " & fieldNames & ")"
            cmd1 = New SqlClient.SqlCommand(sql, cn1)
            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
            cmd1.ExecuteNonQuery()

            Me.lblProgress.Text = ""
            My.Application.DoEvents()

        Catch ex As Exception
            Dim s As String = ex.ToString
        Finally
            cn.Close()
            cn1.Close()
        End Try
    End Sub

    Private Sub tmrTimer_Tick(sender As Object, e As EventArgs) Handles tmrTimer.Tick
        If Now.Hour = 0 And Now.Minute = 0 Then
            ' autorun at midnight
            btnProcess_Click(Nothing, New EventArgs)
        End If
    End Sub
End Class
