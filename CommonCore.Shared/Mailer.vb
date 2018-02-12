Imports System.Net.Mail

Public Class Mailer
    Public Sub New()
        Me.To = New List(Of String)
        Me.CC = New List(Of String)
        Me.BCC = New List(Of String)
        Me.Attachments = New List(Of String)
    End Sub
    Public Sub New(ByVal subject As String, ByVal body As String, ByVal [to] As String)
        Me.To = New List(Of String)
        Me.CC = New List(Of String)
        Me.BCC = New List(Of String)
        Me.Attachments = New List(Of String)

        Me.Subject = subject
        Me.Body = body
        Me.To.Add([to])
    End Sub
    Public Sub New(ByVal subject As String, ByVal body As String, ByVal [to] As String, ByVal attachment As String)
        Me.To = New List(Of String)
        Me.CC = New List(Of String)
        Me.BCC = New List(Of String)
        Me.Attachments = New List(Of String)

        Me.Subject = subject
        Me.Body = body
        Me.To.Add([to])
        Me.Attachments.Add(attachment)
    End Sub

    Public Subject As String
    Public Body As String

    Public [To] As List(Of String)
    Public [From] As String
    Public CC As List(Of String)
    Public BCC As List(Of String)

    Public Attachments As List(Of String)
    Public HtmlBody As Boolean

    Public HostServer As String
    Public UserName As String
    Public Password As String

#Region "Workers"

    Public Function Send() As Boolean
        Dim retVal As Boolean = True

        Try
            Dim Smtp_Server As New SmtpClient
            Dim e_mail As New MailMessage()
            Smtp_Server.UseDefaultCredentials = False
            Smtp_Server.Credentials = New Net.NetworkCredential(Me.UserName, Me.Password)
            Smtp_Server.Host = Me.HostServer

            e_mail = New MailMessage()
            e_mail.From = New MailAddress(Me.From)

            For Each toAddress As String In Me.To
                e_mail.To.Add(toAddress)
            Next
            For Each toAddress As String In Me.CC
                e_mail.CC.Add(toAddress)
            Next
            For Each toAddress As String In Me.BCC
                e_mail.Bcc.Add(toAddress)
            Next
            e_mail.Subject = Me.Subject
            e_mail.IsBodyHtml = Me.HtmlBody
            e_mail.Body = Me.Body

            For Each att As String In Me.Attachments
                e_mail.Attachments.Add(New Attachment(att))
            Next
            Smtp_Server.Send(e_mail)

        Catch ex As Exception
            retVal = False
        End Try

        Return retVal
    End Function

#End Region

End Class
