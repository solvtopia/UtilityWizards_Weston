Imports UtilityWizards.CommonCore.Shared.Common
Imports System.IO
Imports System.Collections.ObjectModel

Public Class fMain

    Private Sub fMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.txtLog.Text = ""
        Me.lblProgress.Text = ""
        Me.lblStatus.Text = ""
        Me.lblStatus2.Text = ""
        Me.tmrTimer.Enabled = True
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        Me.tmrTimer.Enabled = False

        Me.txtLog.Text = ""
        Me.lblProgress.Text = ""
        Me.lblStatus.Text = ""
        Me.lblStatus2.Text = ""

        Me.ProcessS3Files()

        Me.tmrTimer.Enabled = True
    End Sub

    Private Sub ProcessS3Files()
        Dim aws As New AWSHelper

        Me.lblStatus.Text = "Downloading Files ..."
        Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus.Text & vbCrLf)
        My.Application.DoEvents()

        ' make sure the temp folder exists
        Dim tmpS3Path As String = "C:\Users\James\Google Drive\Projects\UtilityWizards_Weston\FileProcessor\S3Temp"
        'Dim tmpS3Path As String = My.Computer.FileSystem.CombinePath(My.Application.Info.DirectoryPath, "S3Temp")
        'If Not My.Computer.FileSystem.DirectoryExists(tmpS3Path) Then My.Computer.FileSystem.CreateDirectory(tmpS3Path)

        ' delete any files in the folder
        'For Each deleteFile In Directory.GetFiles(tmpS3Path, "*.txt", SearchOption.TopDirectoryOnly)
        '    File.Delete(deleteFile)
        'Next

        ' download the files from the S3 bucket to the temp folder
        'Dim lstS3Files As ObservableCollection(Of String) = aws.ListingFiles()
        'For Each f As String In lstS3Files
        '    aws.DownloadFile("", f, tmpS3Path)
        'Next

        Dim dir As List(Of FileInfo) = SearchDir(tmpS3Path, "*.txt", Enums.FileSort.Name)

        Me.lblStatus.Text = "Found " & dir.Count & " Files To Process ..."
        Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus.Text & vbCrLf & vbCrLf)
        My.Application.DoEvents()

        For Each f As FileInfo In dir
            Dim tblName As String = ""

            ' these tables are in the master feed view
            Select Case True
                Case f.Name.ToLower.Contains("_bkrdaily_") : tblName = "_import_BKRDaily"
                Case f.Name.ToLower.Contains("_crwdaily_") : tblName = "_import_CRWDaily"
                Case f.Name.ToLower.Contains("_diligence_") : tblName = "_import_Diligence"
                Case f.Name.ToLower.Contains("_fcldaily_") : tblName = "_import_FCLDaily"
                Case f.Name.ToLower.Contains("_holddata_") : tblName = "_import_HoldData"
                Case f.Name.ToLower.Contains("_lmmdaily_") : tblName = "_import_LMMDaily"
                Case f.Name.ToLower.Contains("_lps_") : tblName = "_import_LPS"
                Case f.Name.ToLower.Contains("_propinsp_") : tblName = "_import_PropInsp"
                Case f.Name.ToLower.Contains("_protection_") : tblName = "_import_Protection"
                Case f.Name.ToLower.Contains("_reodaily_") : tblName = "_import_REODaily"
                Case f.Name.ToLower.Contains("_payplans_") : tblName = "_import_PayPlans"
                Case f.Name.ToLower.Contains("_standard_") : tblName = "_import_Standard"
                Case f.Name.ToLower.Contains("_contacts_") : tblName = "_import_Contacts"
                Case f.Name.ToLower.Contains("_wffeed_") : tblName = "_import_WF_Feed"
            End Select

            ' these tables are additional data received
            Select Case True
                Case f.Name.ToLower.Contains("_bpodaily_") : tblName = "_import_BPODaily"
                Case f.Name.ToLower.Contains("_cashxxxx_") : tblName = "_import_Cashxxxx"
                Case f.Name.ToLower.Contains("_ccaraddl_") : tblName = "_import_CCARAddl"
                Case f.Name.ToLower.Contains("_comments_") : tblName = "_import_Comments"
                Case f.Name.ToLower.Contains("_escrowxx_") : tblName = "_import_Escrowxx"
                Case f.Name.ToLower.Contains("_hampdata_") : tblName = "_import_HampData"
                Case f.Name.ToLower.Contains("_liqdaily_") : tblName = "_import_LiqDaily"
                Case f.Name.ToLower.Contains("_moddaily_") : tblName = "_import_ModDaily"
                Case f.Name.ToLower.Contains("_transact_") : tblName = "_import_Transact"
            End Select

            If tblName <> "" Then
                Dim hasBeenProcessed As Boolean = False

                Me.lblStatus.Text = "Processing " & f.Name & " into [" & tblName & "] " & If(Me.chkUseSandboxDb.Checked, "on the Sandbox", "") & " ..."
                Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus.Text & vbCrLf)
                My.Application.DoEvents()

                ' get the file as a datatable
                Dim tbl As DataTable = Me.ProcessFileToTable(f.FullName, tblName)
                Threading.Thread.Sleep(1000)
                hasBeenProcessed = Me.ImportTable(tbl, tblName)

                If tblName.ToLower = "_import_standard" And hasBeenProcessed Then
                    Me.lblStatus.Text = "Processing Board Records From [" & tblName & "] Table ..."
                    Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus.Text & vbCrLf)
                    My.Application.DoEvents()

                    ' if this is the standard table we need to include board records
                    Threading.Thread.Sleep(1000)
                    hasBeenProcessed = Me.ImportTable(tbl, "_import_Board")
                End If

                If hasBeenProcessed Then
                    ' rename the file as processed so we don't import the same file again
                    My.Computer.FileSystem.RenameFile(f.FullName, f.Name & ".processed")
                    Me.txtLog.AppendText(Now.ToString & ": " & "Finished Processing " & f.Name & vbCrLf & vbCrLf)
                    My.Application.DoEvents()

                    Me.txtLog.AppendText(Now.ToString & ": " & "Waiting 10 Seconds ..." & vbCrLf & vbCrLf)
                    My.Application.DoEvents()

                    Threading.Thread.Sleep(10000)
                End If
            End If
        Next

        Me.lblStatus.Text = "Finished Processing Files."
        Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus.Text & vbCrLf)
        Me.lblStatus2.Text = ""
        Me.lblProgress.Text = ""


        Dim strFile As String = "Processor_" & Now.Date.ToString("MM/dd/yyyy").Replace("/", "") & ".log"
        'Dim fileExists As Boolean = File.Exists(My.Computer.FileSystem.CombinePath(tmpS3Path, strFile))
        'Using sw As New StreamWriter(File.Open(My.Computer.FileSystem.CombinePath(tmpS3Path, strFile), FileMode.OpenOrCreate))
        '    sw.WriteLine(Me.txtLog.Text)
        'End Using
        My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.CombinePath(tmpS3Path, strFile), Me.txtLog.Text, True)

        My.Application.DoEvents()

    End Sub

    Private Function ProcessFileToTable(ByVal fPath As String, ByVal tblName As String) As DataTable
        Dim retVal As New DataTable
        Dim cn As New SqlClient.SqlConnection(ConnectionString(Me.chkUseSandboxDb.Checked))

        Try
            Me.lblStatus2.Text = "Loading [" & tblName & "] Table Structure ..."
            Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
            My.Application.DoEvents()

            ' build a table to store the records in
            Dim cmd As New SqlClient.SqlCommand("select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH from information_schema.columns where TABLE_NAME like '" & tblName & "'", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader()
            Do While rs.Read
                Dim col As New DataColumn
                col.ColumnName = rs("COLUMN_NAME").ToString
                Select Case rs("DATA_TYPE").ToString.ToLower
                    Case "date"
                        col.DataType = GetType(Date)
                    Case "datetime"
                        col.DataType = GetType(SqlTypes.SqlDateTime)
                    Case "float"
                        col.DataType = GetType(SqlTypes.SqlDouble)
                    Case "int"
                        col.DataType = GetType(SqlTypes.SqlInt32)
                    Case "bigint"
                        col.DataType = GetType(SqlTypes.SqlInt64)
                    Case "bit"
                        col.DataType = GetType(SqlTypes.SqlBoolean)
                    Case Else
                        col.DataType = GetType(SqlTypes.SqlString)
                        Dim iMaxLength As Integer = 0
                        If rs("CHARACTER_MAXIMUM_LENGTH").ToString.ToInteger < 0 Then
                            iMaxLength = 50000
                        Else iMaxLength = rs("CHARACTER_MAXIMUM_LENGTH").ToString.ToInteger
                        End If
                        col.MaxLength = iMaxLength
                End Select

                retVal.Columns.Add(col)
            Loop
            rs.Close()
            cmd.Cancel()

            Me.lblStatus2.Text = "Processing Text Records ..."
            Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
            My.Application.DoEvents()

            ' first open the file and read in all the records
            If My.Computer.FileSystem.FileExists(fPath) Then
                Dim processTable As Boolean = False
                Dim fInfo As New FileInfo(fPath)
                Dim fNameParts As List(Of String) = fInfo.Name.Replace(".txt", "").Split("_"c).ToList
                Dim filePart1 As String = fNameParts(0)
                Dim fileDate As Date = Date.Parse(fNameParts(2).Substring(0, 4) & "-" & fNameParts(2).Substring(4, 2) & "-" & fNameParts(2).Substring(6))
                Dim fileName As String = fInfo.Name

                Dim lineCount As Integer = File.ReadAllLines(fPath).Length
                Me.pbrProgress.Value = 0
                Me.pbrProgress.Maximum = lineCount

                Dim header As New List(Of String)
                Dim lineDelimiter As String = vbTab
                Dim acctNum As String = ""

                Dim rdr As New System.IO.StreamReader(fPath)
                Do While rdr.Peek() <> -1
                    Dim s As String = rdr.ReadLine

                    If header.Count = 0 Then
                        ' first row is the header
                        ' try tab delimited first, then pipe, semicolon, comma
                        header = s.Split(lineDelimiter).ToList
                        If header.Count = 1 Then
                            ' if tabs didn't work, try a pipe
                            lineDelimiter = "|"
                            header = s.Split(lineDelimiter).ToList
                        End If
                        If header.Count = 1 Then
                            ' if pipe didn't work, try a comma
                            lineDelimiter = ";"
                            header = s.Split(lineDelimiter).ToList
                        End If
                        If header.Count = 1 Then
                            ' if pipe didn't work, try a comma
                            lineDelimiter = ","
                            header = s.Split(lineDelimiter).ToList
                        End If
                        processTable = (header.Count > 1)
                    Else
                        If processTable Then
                            ' this is a data line so read all the values and assign them to their fields
                            Dim rec As List(Of String) = s.Split(lineDelimiter).ToList
                            Dim r As DataRow = retVal.NewRow
                            For x As Integer = 0 To header.Count - 1
                                If header(x) <> "" Then
                                    If r.Table.Columns.Contains(header(x)) Then
                                        If rec(x) = "" Then
                                            r(header(x)) = DBNull.Value
                                        Else
                                            Select Case True
                                                Case r(header(x)).GetType Is GetType(Date)
                                                    r(header(x)) = rec(x).ToDate("1/1/1900")
                                                Case r(header(x)).GetType Is GetType(SqlTypes.SqlDateTime)
                                                    r(header(x)) = rec(x).ToDate("1/1/1900")
                                                Case r(header(x)).GetType Is GetType(SqlTypes.SqlDouble)
                                                    r(header(x)) = rec(x).ToDouble
                                                Case r(header(x)).GetType Is GetType(SqlTypes.SqlInt32)
                                                    r(header(x)) = rec(x).ToInteger
                                                Case r(header(x)).GetType Is GetType(SqlTypes.SqlInt64)
                                                    r(header(x)) = rec(x).ToLongInteger
                                                Case r(header(x)).GetType Is GetType(SqlTypes.SqlBoolean)
                                                    r(header(x)) = rec(x).ToBoolean
                                                Case Else
                                                    Dim value As String = rec(x).ToString
                                                    Dim maxLength As Integer = r.Table.Columns.Item(header(x)).MaxLength
                                                    If maxLength <= 0 Then Dim z As String = ""
                                                    If value.Length > maxLength Then
                                                        Me.txtLog.AppendText(Now.ToString & ": " & "Value Truncated for Column [" & tblName & "].[" & header(x) & "] from " & value.Length & " Characters to " & maxLength & " Characters" & vbCrLf)
                                                        My.Application.DoEvents()
                                                        value = value.Substring(0, maxLength)
                                                    End If
                                                    r(header(x)) = value
                                            End Select
                                        End If
                                    Else
                                        If Not Me.txtLog.Text.Contains("Column [" & tblName & "].[" & header(x) & "] Does Not Exist!") Then
                                            Me.txtLog.AppendText(Now.ToString & ": " & "Column [" & tblName & "].[" & header(x) & "] Does Not Exist!" & vbCrLf)
                                        End If
                                        My.Application.DoEvents()
                                        End If
                                    End If
                            Next

                            ' add the record to the table
                            r("FILEPART1") = filePart1
                            r("FILEDATE") = fileDate
                            r("FILENAME") = fileName
                            retVal.Rows.Add(r)
                            retVal.AcceptChanges()
                        End If
                    End If

                    Me.pbrProgress.Value += 1
                    Me.lblProgress.Text = "Record " & Me.pbrProgress.Value & "/" & Me.pbrProgress.Maximum & " (" & FormatPercent(Me.pbrProgress.Value / Me.pbrProgress.Maximum, 1) & ")"
                    My.Application.DoEvents()
                Loop
                rdr.Close()
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.FileProcessor, Me.chkUseSandboxDb.Checked))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Private Function ImportTable(ByVal tbl As DataTable, ByVal tblName As String) As Boolean
        Dim retVal As Boolean = True

        Dim cn As New SqlClient.SqlConnection(ConnectionString(Me.chkUseSandboxDb.Checked))

        Try
            If (Me.rbTruncate.Checked Or tblName.ToLower = "_import_standard") And tbl.Rows.Count > 0 Then
                ' if we are set to truncate, or working with the standard table and we have rows in the import table 
                If tblName.ToLower <> "_import_board" Then
                    Me.lblStatus2.Text = "Truncating Table [" & tblName & "] ..."
                    Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
                    My.Application.DoEvents()

                    ' never truncate the board table
                    Dim cmd As New SqlClient.SqlCommand("TRUNCATE TABLE [" & tblName & "]", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    cmd.CommandTimeout = 0
                    cmd.ExecuteNonQuery()
                End If
            End If

            If tblName.ToLower = "_import_board" Then
                Me.lblStatus2.Text = "Searching For New Board Records ..."
                Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
                My.Application.DoEvents()

                ' board table only gets new records for accounts that are in the standard but not already
                ' in the board

                Dim lstNewAccounts As New List(Of String)

                ' get a list of accounts that exist in standard but not in board
                Dim cmd As New SqlClient.SqlCommand("SELECT t1.[ACCOUNT_NUMBER] FROM [_import_Standard] t1 LEFT JOIN [_import_Board] t2 ON t2.[ACCOUNT_NUMBER] = t1.[ACCOUNT_NUMBER] WHERE t2.[ACCOUNT_NUMBER] IS NULL", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                Do While rs.Read
                    If Not lstNewAccounts.Contains(rs("ACCOUNT_NUMBER").ToString) Then lstNewAccounts.Add(rs("ACCOUNT_NUMBER").ToString)
                    My.Application.DoEvents()
                Loop
                cmd.Cancel()
                rs.Close()

                ' remove all the rows from the table that aren't in the list of new accounts
                Dim tblNew As DataTable = tbl.Copy
                tblNew.Rows.Clear()
                For Each r As DataRow In tbl.Rows
                    If lstNewAccounts.Contains(r("ACCOUNT_NUMBER").ToString) Then
                        tblNew.ImportRow(r)
                    End If
                    My.Application.DoEvents()
                Next

                tbl = tblNew.Copy
            End If

            If tbl.Rows.Count > 0 Then
                Me.lblStatus2.Text = "Copying " & FormatNumber(tbl.Rows.Count, 0) & " Records to SQL Server ..."
                Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
                Me.pbrProgress.Value = 0
                Me.pbrProgress.Maximum = tbl.Rows.Count
                My.Application.DoEvents()

                ' use the bulk copy object to insert the records
                Using bulkCopy As SqlClient.SqlBulkCopy = New SqlClient.SqlBulkCopy(cn)
                    If cn.State = ConnectionState.Closed Then cn.Open()

                    ' clear the timeout
                    bulkCopy.BulkCopyTimeout = 0
                    bulkCopy.DestinationTableName = tblName

                    ' setup the progress indicator
                    bulkCopy.NotifyAfter = CInt(tbl.Rows.Count / 200)
                    AddHandler bulkCopy.SqlRowsCopied, AddressOf OnSqlRowsCopied

                    Try
                        bulkCopy.WriteToServer(tbl, DataRowState.Unchanged)
                        My.Application.DoEvents()
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                        retVal = False
                    End Try
                End Using
            End If

            Me.lblStatus2.Text = ""
            My.Application.DoEvents()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.FileProcessor, Me.chkUseSandboxDb.Checked))
            retVal = False
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Private Sub tmrTimer_Tick(sender As Object, e As EventArgs) Handles tmrTimer.Tick
        If Now.Minute = 0 Then
            ' autorun at midnight
            btnProcess_Click(Nothing, New EventArgs)
        End If
    End Sub

    Private Sub OnSqlRowsCopied(ByVal sender As Object, ByVal e As SqlClient.SqlRowsCopiedEventArgs)
        If e.RowsCopied < Me.pbrProgress.Maximum Then
            Me.pbrProgress.Value = e.RowsCopied
        Else Me.pbrProgress.Value = Me.pbrProgress.Maximum
        End If

        Me.lblProgress.Text = "Record " & Me.pbrProgress.Value & "/" & Me.pbrProgress.Maximum & " (" & FormatPercent(Me.pbrProgress.Value / Me.pbrProgress.Maximum, 1) & ")"
        My.Application.DoEvents()
        'Console.WriteLine("Copied {0} so far...", e.RowsCopied)
    End Sub
End Class
