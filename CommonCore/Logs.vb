Imports UtilityWizards.CommonCore.Common
Imports System.Runtime.CompilerServices
Imports System.Web

Public Module Logs

    ' writes a new entry to the audit log
    <Extension()> Public Sub WriteToAuditLog(ByVal entry As AuditLogEntry)
        If entry.ActionType <> Enums.LogEntry.QueryRecord.ToString Then
            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Try
                ' add the user's ip address to the entry object before saving
                Dim REMOTE_ADDR As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
                If REMOTE_ADDR = "127.0.0.1" Then REMOTE_ADDR = "96.36.105.18"

                entry.IpAddress = REMOTE_ADDR

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO AuditLog (ClientID, xmlData, InsertedBy, UpdatedBy) VALUES (@ClientID, @xmlData, '" & entry.UserID & "', '" & entry.UserID & "');", cn)
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@ClientID", entry.ClientID)
                cmd.Parameters.AddWithValue("@xmlData", entry.SerializeToXml)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.CommandTimeout = 0
                cmd.ExecuteNonQuery()
                cmd.Cancel()

            Catch ex As Exception
                ex.WriteToErrorLog(entry.ClientID, entry.Project)
            Finally
                cn.Close()
            End Try
        End If
    End Sub

    ' writes a new entry to the error log
    <Extension()> Public Sub WriteToErrorLog(ByVal exp As Exception)
        exp.WriteToErrorLog(App.CurrentClient.ID, "System")
    End Sub
    <Extension()> Public Sub WriteToErrorLog(ByVal exp As Exception, ByVal clientID As Integer, ByVal [module] As String)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            If exp IsNot Nothing Then
                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Sys_ErrorLog] (ClientID, xmlData, dtInserted, dtUpdated, insertedBy, updatedBy) VALUES (@ClientID, @xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '" & App.CurrentUser.ID & "', '" & App.CurrentUser.ID & "');", cn)
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@ClientID", clientID)
                cmd.Parameters.AddWithValue("@xmlData", exp.SerializeToXml)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.CommandTimeout = 0
                cmd.ExecuteNonQuery()
                cmd.Cancel()

                'Dim msg As New Mailer
                'msg.MailTo.Add("james@nk5.co")
                'msg.MailFrom = "noreply@nk5.co"
                'msg.Password = "nk5coNoReply!"
                'msg.Subject = "Error from Utility Wizards Builder - " & project & " Module"
                'msg.Body = exp.ToString
                'msg.Send()
            End If

        Catch ex As Exception
            ex.WriteToEventLog
            exp.WriteToEventLog
        Finally
            cn.Close()
        End Try
    End Sub

    ' writes a new entry to the servers event log
    <Extension()> Public Function WriteToEventLog(ByVal exp As Exception) As Boolean
        Return exp.WriteToEventLog
    End Function
    <Extension()> Public Function WriteToEventLog(ByVal exp As Exception, ByVal userID As Integer) As Boolean
        Try
            ''stop

            'Dim logName As String = System.Configuration.ConfigurationManager.AppSettings("EventLog")

            '' Create the source, if it does not already exist.
            'If Not EventLog.SourceExists("NK5" & logName) Then
            '    EventLog.CreateEventSource("NK5" & logName, logName)
            'End If

            '' Create an EventLog instance and assign its source.
            'Dim myLog As New EventLog()
            'myLog.Source = "NK5" & logName

            '' Write an informational entry to the event log.
            'Dim usr As SystemUser = GetUser(userID)
            'Dim strMsg As String = "Error occurred at " & Date.Now.ToString & ControlChars.NewLine

            'If (usr.ID > 0) Then
            '    strMsg &= "Current User: " & usr.Email & ControlChars.NewLine
            'End If
            'strMsg &= "Exception: " & exp.ToString

            'myLog.WriteEntry(strMsg, EventLogEntryType.Error)

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

End Module
