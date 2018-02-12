Imports System.Xml
Imports Ionic.Zip
Imports UtilityWizards.CommonCore.Shared.Common

Public Class fMain
    Dim dtStartTime As DateTime

    Dim lastRunTime As DateTime
    Dim processRunning As Boolean

    'Dim oFtp As UtilityWizards.CommonCore.Shared.Ftp

    Private Sub Form1_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.lstLog.Items.Clear()
        Me.lblTotalTime.Text = "0"
        Me.txtError.Text = ""

        ' setup the ftp server
        'oFtp = New Ftp("ftp://ftp.dailycourtdocket.com/access.dailycourtdocket.com/wwwroot/", "wdivzylm_root", "9dR00g326d!")

        processRunning = False

        'Me.lblVersion.Text = "Version: " & GetApplicationAssembly(Nothing).GetName.Version.ToString
        My.Application.DoEvents()

        ' autorun on load if we are on the server
        If My.Computer.Name.ToUpper.Contains("WIN-CQJRSQ0R80J") Then
            'Me.lstLog.LogFileProcessorHistory("Auto Run: " & Now.ToString)
            'Me.Process(True)
        End If
    End Sub

    Private Sub Process(ByVal fetchEmails As Boolean)
        Dim retry_location As String = ""
        processRunning = True

        lastRunTime = Now

        dtStartTime = Now
        Me.tmrTimer.Enabled = True

        Me.lblTotalTime.Text = "0"
        My.Application.DoEvents()

        Me.lblPrimary.Text = "0/0 (0.0%)"
        Me.lblSecondary.Text = "0/0 (0.0%)"

        files_tmp = "C:\inetpub\access.utilitywizards.com\wwwroot\811_tmp\"
        If Not My.Computer.FileSystem.DirectoryExists(files_tmp) Then My.Computer.FileSystem.CreateDirectory(files_tmp)

        Dim cn As New SqlClient.SqlConnection(ConnectionString)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

        Dim retryCount As Integer = 0

        Try
            Dim q As Integer = cn.PacketSize

            retryCount = 0
retry_connection_top:
            retry_location = "top"
            Dim clientId As Integer = 0
            Dim moduleId As Integer = 0
            Dim notifyIds As New List(Of Integer)
            Dim adminUserId As Integer = 0

            ' loop through the email accounts and save the attachments
            Dim cmd As New SqlClient.SqlCommand("SELECT [ClientID], [ModuleID], [NotifyIDs], [AdminUserID], [Email], [Password], [MailServer], [FtpServer], [FtpUser], [FtpPass] FROM [811Settings] ORDER BY [ClientID];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                clientId = rs("ClientID").ToString.ToInteger
                moduleId = rs("ModuleID").ToString.ToInteger
                For Each s As String In rs("NotifyIDs").ToString.Split("|"c)
                    If s.ToInteger > 0 Then notifyIds.Add(s.ToInteger)
                Next
                adminUserId = rs("AdminUserID").ToString.ToInteger

                'Me.lstLog.LogFileProcessorHistory("Checking Messages ...")

                ' create a client folder to store all the attachments in
                Dim client_tmp As String = My.Computer.FileSystem.CombinePath(files_tmp, clientId.ToString)
                If Not My.Computer.FileSystem.DirectoryExists(client_tmp) Then My.Computer.FileSystem.CreateDirectory(client_tmp)

                If fetchEmails Then
                    Dim processedMessages As Integer = 0
                    Dim processedMessageIDs As New List(Of Integer)

                    Dim client As OpenPop.Pop3.Pop3Client = New OpenPop.Pop3.Pop3Client()
                    client.Connect(rs("MailServer").ToString, 110, False)
                    client.Authenticate(rs("Email").ToString, rs("Password").ToString)

                    Dim messageCount As Integer = client.GetMessageCount()
                    Dim allMessages As List(Of OpenPop.Mime.Message) = New List(Of OpenPop.Mime.Message)(messageCount)
                    For count As Integer = 1 To messageCount
                        Dim msg As OpenPop.Mime.Message = client.GetMessage(count)

                        ' make sure we're looking at tickets only
                        If msg.ToMailMessage.From.Address.ToLower.Contains("nc811tkts@nc811.org") And Not msg.ToMailMessage.Subject.ToLower.Contains("eod audit") Then
                            processedMessageIDs.Add(count)

                            Dim attachments = msg.FindAllAttachments
                            For Each att In attachments
                                If att.FileName.ToLower.EndsWith(".xml") Then
                                    Dim thefile = att.FileName
                                    Dim filetype = att.ContentType
                                    Dim contentid = att.ContentId

                                    Dim tmpPath As String = My.Computer.FileSystem.CombinePath(client_tmp, thefile)
                                    If My.Computer.FileSystem.FileExists(tmpPath) Then My.Computer.FileSystem.DeleteFile(tmpPath)
                                    System.IO.File.WriteAllBytes(tmpPath, att.Body)

                                    processedMessages += 1
                                End If

                                My.Application.DoEvents()
                            Next
                        End If

                        Me.pbrPrimary.Maximum = messageCount
                        If count <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value = count
                        Me.lblPrimary.Text = FormatNumber(count, 0) & "/" & FormatNumber(messageCount, 0) & " (" & FormatNumber((count / messageCount) * 100, 1) & "%)"

                        My.Application.DoEvents()
                    Next

                    'Me.lstLog.LogFileProcessorHistory(processedMessages & " Message(s) Downloaded ...")
                    'Me.lstLog.LogFileProcessorHistory("Processing Xml File(s) ...")

                    ' delete all messages we processed from the account after processing
                    For Each i As Integer In processedMessageIDs
                        client.DeleteMessage(i)
                    Next

                    client.Disconnect()
                    client.Dispose()
                End If

                ' process the xml files
                Dim processedTextFiles As Integer = 0
                Dim fileEntries As List(Of IO.FileInfo) = SearchDir(client_tmp, "*.xml", UtilityWizards.CommonCore.Shared.Enums.FileSort.Size)
                For Each fi As IO.FileInfo In fileEntries
                    Me.pbrPrimary.Maximum = fileEntries.Count
                    If processedTextFiles <= Me.pbrPrimary.Maximum Then Me.pbrPrimary.Value = processedTextFiles
                    Me.lblPrimary.Text = FormatNumber(processedTextFiles, 0) & "/" & FormatNumber(fileEntries.Count, 0) & " (" & FormatNumber((processedTextFiles / fileEntries.Count) * 100, 1) & "%)"
                    My.Application.DoEvents()

                    ' create the xml for the work order
                    Dim xDoc As New XmlDocument
                    xDoc.LoadXml(templateXml)

                    ' change out the template fields for the values from the ticket
                    Dim xTicket As New XmlDocument
                    xTicket.Load(fi.FullName)

                    If xTicket.Item("NewDataSet").Item("tickets").Item("duration") IsNot Nothing Then
                        xDoc.Item("Data").Item("Duration").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("duration").InnerText
                    End If
                    If xTicket.Item("NewDataSet").Item("tickets").Item("response_due") IsNot Nothing Then
                        xDoc.Item("Data").Item("ResponseDueDate").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("response_due").InnerText
                    End If
                    If xTicket.Item("NewDataSet").Item("tickets").Item("done_for") IsNot Nothing Then
                        xDoc.Item("Data").Item("CustomerName").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("done_for").InnerText
                        If xTicket.Item("NewDataSet").Item("tickets").Item("name") IsNot Nothing Then
                            xDoc.Item("Data").Item("CustomerName").InnerText &= " - " & xTicket.Item("NewDataSet").Item("tickets").Item("name").InnerText
                        End If
                    End If
                    If xTicket.Item("NewDataSet").Item("tickets").Item("work_type") IsNot Nothing Then
                        xDoc.Item("Data").Item("WorkType").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("work_type").InnerText
                    End If
                    If xTicket.Item("NewDataSet").Item("tickets").Item("remarks") IsNot Nothing Then
                        xDoc.Item("Data").Item("Remarks").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("remarks").InnerText
                    End If

                    ' build the location address
                    Dim fromAddress = "", toAddress = "", street = "", city = "", state As String = ""
                    Dim cross As New List(Of String)
                    If xTicket.Item("NewDataSet").Item("tickets").Item("st_from_address") IsNot Nothing Then fromAddress = xTicket.Item("NewDataSet").Item("tickets").Item("st_from_address").InnerText
                    If xTicket.Item("NewDataSet").Item("tickets").Item("st_to_address") IsNot Nothing Then toAddress = xTicket.Item("NewDataSet").Item("tickets").Item("st_to_address").InnerText
                    If xTicket.Item("NewDataSet").Item("tickets").Item("street") IsNot Nothing Then street = xTicket.Item("NewDataSet").Item("tickets").Item("street").InnerText
                    If xTicket.Item("NewDataSet").Item("tickets").Item("place") IsNot Nothing Then city = xTicket.Item("NewDataSet").Item("tickets").Item("place").InnerText
                    If xTicket.Item("NewDataSet").Item("tickets").Item("state") IsNot Nothing Then state = xTicket.Item("NewDataSet").Item("tickets").Item("state").InnerText
                    For x As Integer = 1 To 10
                        If xTicket.Item("NewDataSet").Item("tickets").Item("cross" & x) IsNot Nothing Then cross.Add(xTicket.Item("NewDataSet").Item("tickets").Item("cross" & x).InnerText)
                    Next
                    Dim serviceAddress As String = ""
                    If fromAddress.Trim.ToLower = toAddress.Trim.ToLower Then
                        serviceAddress = fromAddress & " " & street & " " & city & ", " & state
                    Else serviceAddress = fromAddress & "-" & toAddress & " " & street & " " & city & ", " & state
                    End If
                    ' add any cross streets
                    Dim crossCounter As Integer = 1
                    For Each s As String In cross
                        If s.Trim <> "" Then serviceAddress &= vbCrLf & "Cross Street " & crossCounter & ": " & s.Trim
                        crossCounter += 1
                    Next
                    xDoc.Item("Data").Item("CustomerServiceAddress").InnerText = serviceAddress

                    If xTicket.Item("NewDataSet").Item("tickets").Item("location") IsNot Nothing Then
                        xDoc.Item("Data").Item("LocateDetails").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("location").InnerText
                    End If
                    If xTicket.Item("NewDataSet").Item("tickets").Item("revision") IsNot Nothing Then
                        xDoc.Item("Data").Item("Revision").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("revision").InnerText
                    End If
                    If xTicket.Item("NewDataSet").Item("tickets").Item("original_ticket") IsNot Nothing Then
                        xDoc.Item("Data").Item("OriginalTicketNumber").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("original_ticket").InnerText
                    End If
                    If xTicket.Item("NewDataSet").Item("tickets").Item("ticket") IsNot Nothing Then
                        xDoc.Item("Data").Item("TicketNumber").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("ticket").InnerText
                    End If

                    ' set the priority ... normal is high priority, everything else is emergency for us
                    Dim priority As UtilityWizards.CommonCore.Shared.Enums.SystemModulePriority = UtilityWizards.CommonCore.Shared.Enums.SystemModulePriority.Normal
                    If xTicket.Item("NewDataSet").Item("tickets").Item("priority") IsNot Nothing Then
                        If xTicket.Item("NewDataSet").Item("tickets").Item("priority").InnerText.ToLower = "norm" Then
                            priority = UtilityWizards.CommonCore.Shared.Enums.SystemModulePriority.High
                        Else priority = UtilityWizards.CommonCore.Shared.Enums.SystemModulePriority.Emergency
                        End If
                        xDoc.Item("Data").Item("Priority").InnerText = CStr(priority)
                    End If

                    If xTicket.Item("NewDataSet").Item("tickets").Item("printable_text") IsNot Nothing Then
                        xDoc.Item("Data").Item("printable_text").InnerText = xTicket.Item("NewDataSet").Item("tickets").Item("printable_text").InnerText
                    End If
                    xDoc.Item("Data").Item("ModuleID").InnerText = moduleId.ToString
                    xDoc.Item("Data").Item("ClientID").InnerText = clientId.ToString

                    ' save the xml to the database
                    Dim newRecordId As Integer = 0
                    Dim cmd1 As New SqlClient.SqlCommand("INSERT INTO [ModuleData] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active]) VALUES(@xmlData,'" & Now.ToString & "', '" & Now.ToString & "', '" & adminUserId & "', '" & adminUserId & "', '1');SELECT @@Identity AS SCOPEIDENTITY;", cn1)
                    cmd1.Parameters.AddWithValue("@xmlData", xDoc.ToXmlString)
                    If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                    Dim rs1 As SqlClient.SqlDataReader = cmd1.ExecuteReader()
                    If rs1.Read Then
                        newRecordId = CInt(rs1("SCOPEIDENTITY"))
                    End If
                    cmd1.Cancel()
                    rs1.Close()

                    UtilityWizards.CommonCore.Shared.Common.LogHistory("New Work Order #" & newRecordId & " Created", adminUserId)

                    cmd1 = New SqlClient.SqlCommand("EXEC [procRefreshModuleDataIDs]", cn1)
                    If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                    cmd1.ExecuteNonQuery()
                    cmd1.Cancel()

                    ' text the supervisors
                    Dim txt As String = ""
                    Dim supUsers As String = ""
                    For Each supId As Integer In notifyIds
                        Dim supUser As New SystemUser(supId)
                        If supUsers = "" Then supUsers = supUser.Name Else supUsers &= ", " & supUser.Name
                        Dim technicianUpdated As Boolean = False

                        Dim assigned As String = ""

                        ' text the supervisor that a work order has been created
                        If priority = UtilityWizards.CommonCore.Shared.Enums.SystemModulePriority.High Then
                            txt = "811 Locates Work Order #" & newRecordId & " has been created " & assigned & "and assigned to you as the supervisor(s)."
                            UtilityWizards.CommonCore.Shared.Messaging.SendTwilioNotification(supUser, UtilityWizards.CommonCore.Shared.Enums.NotificationType.Custom, txt, newRecordId)
                        ElseIf priority = UtilityWizards.CommonCore.Shared.Enums.SystemModulePriority.Emergency Then
                            txt = assigned & "EMERGENCY 811 Locates Work Order #" & newRecordId & " has been created " & assigned & "and assigned to you as the supervisor(s)."
                            UtilityWizards.CommonCore.Shared.Messaging.SendTwilioNotification(supUser, UtilityWizards.CommonCore.Shared.Enums.NotificationType.Custom, txt, newRecordId)
                        End If
                    Next

                    ' add the supervisor name to the text
                    txt = txt.Replace(" you ", " " & supUsers & " ")

                    ' email administrators if the notify box is checked
                    cmd1 = New SqlClient.SqlCommand("SELECT [xEmail] FROM [vwUserInfo] WHERE [xClientID] = " & clientId & " AND [xPermissions] LIKE '%administrator%'", cn1)
                    If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
                    rs1 = cmd1.ExecuteReader
                    Do While rs1.Read
                        ' email all administrators that the work order has been created
                        Dim msg As New Mailer
                        msg.HostServer = My.Settings.MailHost
                        msg.UserName = My.Settings.MailUser
                        msg.Password = My.Settings.MailPassword
                        msg.To.Add(rs1("xEmail").ToString)
                        'msg.BCC.Add("james@solvtopia.com")
                        msg.Body = "<html>" & txt & "</html>"
                        msg.Subject = "Utility Wizards Work Order Created"
                        msg.From = "noreply@utilitywizards.com"
                        msg.HtmlBody = True
                        msg.Send()
                    Loop
                    cmd1.Cancel()
                    rs1.Close()

                    ' rename the file
                    If My.Computer.FileSystem.FileExists(fi.FullName & ".processed") Then My.Computer.FileSystem.DeleteFile(fi.FullName & ".processed")
                    My.Computer.FileSystem.RenameFile(fi.FullName, fi.Name & ".processed")

                    My.Application.DoEvents()
                Next

                My.Application.DoEvents()
            Loop
            cmd.Cancel()
            rs.Close()

            Me.tmrTimer.Enabled = False

            processRunning = False

        Catch ex As Exception
            If retryCount <= 10 Then
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
                retryCount += 1
                ' wait 1 second before retrying ...
                'System.Threading.Thread.Sleep(1000)
                Select Case retry_location.ToLower
                    Case "top" : GoTo retry_connection_top
                End Select
            Else
                ex.WriteToErrorLog(New ErrorLogEntry(UtilityWizards.CommonCore.Shared.Enums.ProjectName._811Processor))
                Me.txtError.Text &= ex.Message & vbCrLf
                Me.txtError.Text &= ex.ToString & vbCrLf & vbCrLf & vbCrLf
            End If
        Finally
            cn.Close()
            cn1.Close()
        End Try
    End Sub

    Private Sub ProcessCompleted()

    End Sub

#Region "Buttons & Timers"

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcessFiles.Click
        'Me.lstLog.LogFileProcessorHistory("Manual Run: " & Now.ToString)
        Me.Process(False)
    End Sub

    Private Sub btnProcessAttorneys_Click(sender As Object, e As EventArgs)
        Me.tmrAutoRun.Enabled = False
        Me.tmrAutoRun.Enabled = True
        'LookupAttorney("HUNT,AMY,P", "NC")
    End Sub

    Private Sub btnProcessClients_Click(sender As Object, e As EventArgs)
        Me.tmrAutoRun.Enabled = False
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs)
        Me.tmrAutoRun.Enabled = False

        'Dim lst As New List(Of FileRecord)

        'lst = Processor.NCCivilFileNew("c:\temp\S.__ALAN_Z_THORNBURG.1.10_00_.TRIALS.txt")
        'files_tmp = "C:\temp\"

        'Dim fileEntries As New List(Of String)
        'fileEntries = SearchDir(files_tmp, "*.txt")
        'fileEntries.Sort()
        'For Each fName As String In fileEntries
        '    Dim fi As New System.IO.FileInfo(fName)
        '    Dim fa() As String = fName.Replace(files_tmp, "").Split("\"c)

        '    Dim sCounty As String = fName.Replace(files_tmp, "").Split("\"c)(1)
        '    Dim sType As String = fName.Replace(files_tmp, "").Split("\"c)(2)
        '    Dim sDate As String = ""

        '    If sType.ToLower = "civil" Then
        '        sDate = fName.Replace(files_tmp, "").Split("\"c)(3)
        '    ElseIf sType.ToLower = "criminal" Then
        '        sDate = fi.Name.Split("."c)(2) & "." & fi.Name.Split("."c)(3) & "." & fi.Name.Split("."c)(4)
        '    End If

        '    Dim lst As New List(Of FileRecord)
        '    Select Case True
        '        Case sType.ToLower = "civil"
        '            lst = Processor.NCCivilFile(fName)
        '        Case sType.ToLower = "criminal"
        '            lst = Processor.NCCriminalFile(fName)
        '    End Select

        'Dim x As Integer = lst.Count
        'Next

        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub tmrTimer_Tick(sender As Object, e As EventArgs) Handles tmrTimer.Tick
        Me.tmrTimer.Enabled = False

        Dim time As TimeSpan = TimeSpan.FromSeconds(DateDiff(DateInterval.Second, dtStartTime, Now))
        Dim str As String = time.ToString("hh\:mm\:ss\:fff")

        Me.lblTotalTime.Text = str & " to Process"

        Me.tmrTimer.Enabled = True
        My.Application.DoEvents()
    End Sub

    Private Sub tmrAutoRun_Tick(sender As Object, e As EventArgs) Handles tmrAutoRun.Tick
        If DateDiff(DateInterval.Minute, lastRunTime, Now) >= 120 And Not processRunning Then
            'Me.lstLog.LogFileProcessorHistory("Auto Run: " & Now.ToString)

            Me.Process(True)
        End If
    End Sub

    Private Sub btnProcessAttorneyCases_Click(sender As Object, e As EventArgs) Handles btnProcessAttorneyCases.Click
        Me.tmrAutoRun.Enabled = False
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnAttorneyNames_Click(sender As Object, e As EventArgs) Handles btnAttorneyNames.Click
        Me.tmrAutoRun.Enabled = False
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnManualNotifications_Click(sender As Object, e As EventArgs) Handles btnManualNotifications.Click
        Me.tmrAutoRun.Enabled = False
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnAdminCalendars_Click(sender As Object, e As EventArgs) Handles btnAdminCalendars.Click
        Me.tmrAutoRun.Enabled = False
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnCheckProcessed_Click(sender As Object, e As EventArgs) Handles btnCheckProcessed.Click
        Me.tmrAutoRun.Enabled = False
        Me.tmrAutoRun.Enabled = True
    End Sub

    Private Sub btnArchive_Click(sender As Object, e As EventArgs) Handles btnArchive.Click
        Me.tmrAutoRun.Enabled = False
        Me.tmrAutoRun.Enabled = True
    End Sub

#End Region
End Class
