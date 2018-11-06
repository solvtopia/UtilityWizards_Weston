Imports UtilityWizards.CommonCore.Shared.Common
Imports System.IO
Imports System.Collections.ObjectModel

Public Class fMain

    Private FirstRunProcessed As List(Of String)

    Private Sub fMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.txtLog.Text = ""
        Me.lblProgress.Text = ""
        Me.lblStatus.Text = ""
        Me.lblStatus2.Text = ""
        Me.FirstRunProcessed = New List(Of String)
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
        If Not My.Computer.FileSystem.DirectoryExists(My.Settings.LocalTempDir) Then My.Computer.FileSystem.CreateDirectory(My.Settings.LocalTempDir)

        ' download the files from the S3 bucket to the temp folder
        'Dim lstS3Files As ObservableCollection(Of String) = aws.ListingFiles()
        'For Each f As String In lstS3Files
        '    aws.DownloadFile("", f, My.Settings.LocalTempDir)
        'Next

        Dim dir As List(Of FileInfo) = SearchDir(My.Settings.LocalTempDir, "*.txt", Enums.FileSort.Name)

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

                Me.lblStatus.Text = "Processing " & f.Name & " into [" & tblName & "] ..."
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
                    ' keep track of the tables that have been processed so we only truncate on the first run
                    If Not Me.FirstRunProcessed.Contains(tblName) Then Me.FirstRunProcessed.Add(tblName)

                    ' rename the file as processed so we don't import the same file again
                    My.Computer.FileSystem.RenameFile(f.FullName, f.Name & ".processed")
                    Me.txtLog.AppendText(Now.ToString & ": " & "Finished Processing " & f.Name & vbCrLf & vbCrLf)
                    Me.txtLog.AppendText(Now.ToString & ": " & "Waiting 10 Seconds ..." & vbCrLf & vbCrLf)
                    My.Application.DoEvents()

                    Threading.Thread.Sleep(10000)
                Else
                    Me.txtLog.AppendText(Now.ToString & ": " & "Error Processing " & f.Name & "!" & vbCrLf & vbCrLf)
                    Me.txtLog.AppendText(Now.ToString & ": " & "Waiting 10 Seconds ..." & vbCrLf & vbCrLf)
                    My.Application.DoEvents()

                    Threading.Thread.Sleep(10000)
                End If
            End If
        Next

        Me.lblStatus.Text = "Finished Processing Files."
        Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus.Text & vbCrLf & vbCrLf & vbCrLf)
        Me.lblStatus2.Text = ""
        Me.lblProgress.Text = ""


        Dim strFile As String = "Processor_" & Now.Date.ToString("MM/dd/yyyy").Replace("/", "") & ".log"
        My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.CombinePath(My.Settings.LocalTempDir, strFile), Me.txtLog.Text, True)

        My.Application.DoEvents()

    End Sub

    Private Function ProcessFileToTable(ByVal fPath As String, ByVal tblName As String) As DataTable
        Dim retVal As New DataTable
        Dim cnLive As New SqlClient.SqlConnection(ConnectionString(False))
        Dim cnSandbox As New SqlClient.SqlConnection(ConnectionString(True))

        Try
            Me.lblStatus2.Text = "Loading Table Structure ..."
            Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
            My.Application.DoEvents()

            ' build a table to store the records in
            Dim cmd As New SqlClient.SqlCommand("select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH from information_schema.columns where TABLE_NAME like '" & tblName & "'", cnLive)
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
TrySplitAgain:
                            Dim rec As List(Of String) = s.Split(lineDelimiter).ToList
                            If rec.Count < header.Count Then
                                ' the record field count doesn't match the header count so add blanks until we match
                                s &= lineDelimiter
                                My.Application.DoEvents()
                                GoTo TrySplitAgain
                            End If
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
            cnLive.Close()
            cnSandbox.Close()
        End Try

        Return retVal
    End Function

    Private Function ImportTable(ByVal tbl As DataTable, ByVal tblName As String) As Boolean
        Dim retVal As Boolean = True

        Dim cnLive As New SqlClient.SqlConnection(ConnectionString(False))
        Dim cnSandbox As New SqlClient.SqlConnection(ConnectionString(True))

        Dim tblNewLive As DataTable = tbl.Copy
        Dim tblNewSandbox As DataTable = tbl.Copy

        Try
            Dim GoodToTruncate As Boolean = False

            ' truncate if the radio is selected under settings or if this is the standard table
            GoodToTruncate = (Me.rbTruncate.Checked Or tblName.ToLower = "_import_standard")
            ' check if we have rows in the import table
            If GoodToTruncate Then GoodToTruncate = (tbl.Rows.Count > 0)
            ' the board table is never truncated
            If GoodToTruncate Then GoodToTruncate = (tblName.ToLower <> "_import_board")
            ' we only truncate on the first run of a table
            If GoodToTruncate Then GoodToTruncate = (Not Me.FirstRunProcessed.Contains(tblName))

            If GoodToTruncate Then
                ' we've passed all the checks so go ahead and truncate the table

                Dim cmd As SqlClient.SqlCommand
                If Not Me.chkUseSandboxDb.Checked Then
                    ' update the live site if the check to use the sandbox only is not checked
                    Me.lblStatus2.Text = "Truncating LIVE Table ..."
                    Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
                    My.Application.DoEvents()

                    cmd = New SqlClient.SqlCommand("TRUNCATE TABLE [" & tblName & "]", cnLive)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    cmd.CommandTimeout = 0
                    cmd.ExecuteNonQuery()

                    Threading.Thread.Sleep(1000)
                End If

                ' sandbox always gets updated
                Me.lblStatus2.Text = "Truncating SANDBOX Table ..."
                Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
                My.Application.DoEvents()

                cmd = New SqlClient.SqlCommand("TRUNCATE TABLE [" & tblName & "]", cnSandbox)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.CommandTimeout = 0
                cmd.ExecuteNonQuery()
            End If

            If tblName.ToLower = "_import_board" Then
                ' board table only gets new records for accounts that are in the standard but not already
                ' in the board

                Dim lstNewLiveAccounts As New List(Of String)
                Dim lstNewSandboxAccounts As New List(Of String)

                ' get a list of accounts that exist in standard but not in board
                Dim cmd As SqlClient.SqlCommand
                Dim rs As SqlClient.SqlDataReader
                If Not Me.chkUseSandboxDb.Checked Then
                    ' update the live site if the check to use the sandbox only is not checked
                    Me.lblStatus2.Text = "Searching For New LIVE Board Records ..."
                    Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
                    My.Application.DoEvents()

                    cmd = New SqlClient.SqlCommand("SELECT t1.[ACCOUNT_NUMBER] FROM [_import_Standard] t1 LEFT JOIN [_import_Board] t2 ON t2.[ACCOUNT_NUMBER] = t1.[ACCOUNT_NUMBER] WHERE t2.[ACCOUNT_NUMBER] IS NULL", cnLive)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    rs = cmd.ExecuteReader
                    Do While rs.Read
                        If Not lstNewLiveAccounts.Contains(rs("ACCOUNT_NUMBER").ToString) Then lstNewLiveAccounts.Add(rs("ACCOUNT_NUMBER").ToString)
                        My.Application.DoEvents()
                    Loop
                    cmd.Cancel()
                    rs.Close()

                    ' remove all the rows from the table that aren't in the list of new accounts
                    ' update the live site if the check to use the sandbox only is not checked
                    tblNewLive.Rows.Clear()
                    For Each r As DataRow In tbl.Rows
                        If lstNewLiveAccounts.Contains(r("ACCOUNT_NUMBER").ToString) Then
                            tblNewLive.ImportRow(r)
                        End If
                        My.Application.DoEvents()
                    Next

                    Threading.Thread.Sleep(1000)
                End If

                ' sandbox always gets updated
                Me.lblStatus2.Text = "Searching For New SANDBOX Board Records ..."
                Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
                My.Application.DoEvents()

                cmd = New SqlClient.SqlCommand("SELECT t1.[ACCOUNT_NUMBER] FROM [_import_Standard] t1 LEFT JOIN [_import_Board] t2 ON t2.[ACCOUNT_NUMBER] = t1.[ACCOUNT_NUMBER] WHERE t2.[ACCOUNT_NUMBER] IS NULL", cnSandbox)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                Do While rs.Read
                    If Not lstNewSandboxAccounts.Contains(rs("ACCOUNT_NUMBER").ToString) Then lstNewSandboxAccounts.Add(rs("ACCOUNT_NUMBER").ToString)
                    My.Application.DoEvents()
                Loop
                cmd.Cancel()
                rs.Close()

                ' sandbox always gets updated
                tblNewSandbox.Rows.Clear()
                For Each r As DataRow In tbl.Rows
                    If lstNewSandboxAccounts.Contains(r("ACCOUNT_NUMBER").ToString) Then
                        tblNewSandbox.ImportRow(r)
                    End If
                    My.Application.DoEvents()
                Next
            End If

            If tblNewLive.Rows.Count > 0 Or tblNewSandbox.Rows.Count > 0 Then
                ' use the bulk copy object to insert the records
                Dim bulkCopy As SqlClient.SqlBulkCopy
                If Not Me.chkUseSandboxDb.Checked Then
                    ' update the live site if the check to use the sandbox only is not checked
                    Me.lblStatus2.Text = "Copying " & FormatNumber(tblNewLive.Rows.Count, 0) & " Records to LIVE Table ..."
                    Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
                    Me.pbrProgress.Value = 0
                    Me.pbrProgress.Maximum = tblNewLive.Rows.Count
                    My.Application.DoEvents()

                    bulkCopy = New SqlClient.SqlBulkCopy(cnLive)

                    ' clear the timeout
                    bulkCopy.BulkCopyTimeout = 0
                    bulkCopy.DestinationTableName = tblName

                    ' setup the progress indicator
                    bulkCopy.NotifyAfter = CInt(tblNewLive.Rows.Count / 200)
                    AddHandler bulkCopy.SqlRowsCopied, AddressOf OnSqlRowsCopied

                    Using bulkCopy
                        If cnLive.State = ConnectionState.Closed Then cnLive.Open()

                        Try
                            bulkCopy.WriteToServer(tblNewLive, DataRowState.Unchanged)
                            My.Application.DoEvents()
                        Catch ex As Exception
                            Console.WriteLine(ex.Message)
                            retVal = False
                        End Try
                    End Using

                    Threading.Thread.Sleep(5000)
                End If

                ' sandbox always gets updated
                Me.lblStatus2.Text = "Copying " & FormatNumber(tblNewSandbox.Rows.Count, 0) & " Records to SANDBOX Table ..."
                Me.txtLog.AppendText(Now.ToString & ": " & Me.lblStatus2.Text & vbCrLf)
                Me.pbrProgress.Value = 0
                Me.pbrProgress.Maximum = tblNewSandbox.Rows.Count
                My.Application.DoEvents()

                bulkCopy = New SqlClient.SqlBulkCopy(cnSandbox)

                ' clear the timeout
                bulkCopy.BulkCopyTimeout = 0
                bulkCopy.DestinationTableName = tblName

                ' setup the progress indicator
                bulkCopy.NotifyAfter = CInt(tblNewSandbox.Rows.Count / 200)
                AddHandler bulkCopy.SqlRowsCopied, AddressOf OnSqlRowsCopied

                Using bulkCopy
                    If cnSandbox.State = ConnectionState.Closed Then cnSandbox.Open()

                    Try
                        bulkCopy.WriteToServer(tblNewSandbox, DataRowState.Unchanged)
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
            cnLive.Close()
            cnSandbox.Close()
        End Try

        Return retVal
    End Function

    Private Sub tmrTimer_Tick(sender As Object, e As EventArgs) Handles tmrTimer.Tick
        If Now.Minute >= 0 And Now.Minute <= 5 Then
            ' autorun every hour
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

    Private Sub btnRenameProcessed_Click(sender As Object, e As EventArgs) Handles btnRenameProcessed.Click
        Dim dir As List(Of FileInfo) = SearchDir(My.Settings.LocalTempDir, "*.processed", Enums.FileSort.Name)

        For Each f As FileInfo In dir
            My.Computer.FileSystem.RenameFile(f.FullName, f.Name.Replace(".processed", ""))
        Next

        MsgBox("All files have been reset to .txt")
    End Sub

    Private Sub btnPresignedUrls_Click(sender As Object, e As EventArgs) Handles btnPresignedUrls.Click
        Dim aws As New AWSHelper

        Me.txtExtras.Text = ""

        Dim lst As List(Of AWSHelper.AwsFileInfo) = aws.ListFiles()
        For Each f As AWSHelper.AwsFileInfo In lst
            Me.txtExtras.AppendText(f.FullName & " > Expires " & f.PutUrlExpires & vbCrLf & f.PutUrl & vbCrLf & vbCrLf)
        Next
    End Sub
End Class
