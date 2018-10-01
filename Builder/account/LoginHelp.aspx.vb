Imports System.Configuration.ConfigurationManager
Imports UtilityWizards.CommonCore.Shared.Common

Public Class LoginHelp
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblMessage.Text = ""
        Me.Master.Master.UserInfoPanel.Visible = False
        Me.Master.Master.TopRightPanel.Visible = False
        Me.Master.Master.Breadcrumbs.Visible = (Not Me.OnMobile)

        If Not IsPostBack Then
            Me.txtEmail.Focus()
        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [xPassword] FROM [Users] WHERE [xEmail] LIKE @email or [xMobileNumber] LIKE @mobileNumber;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.Parameters.AddWithValue("@email", Me.txtEmail.Text)
            cmd.Parameters.AddWithValue("@mobileNumber", Me.txtMobileNumber.Text.Replace(" ", "").Replace("-", "").Replace("(", ""))
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                ' email the customer
                Dim msg As New Mailer
                msg.HostServer = AppSettings("MailHost")
                msg.UserName = AppSettings("MailUser")
                msg.Password = AppSettings("MailPassword")
                msg.To.Add(Me.txtEmail.Text)
                msg.BCC.Add("james@nk5.co")
                msg.Body = "<html>Below is the password we have on file for your account.<br/><br/>" & rs("xPassword").ToString & "<br/><br/>Please keep this password safe or delete this message once you have successfully logged in!</html>"
                msg.Subject = "Utility Wizards Forgot Password"
                msg.From = "sales@utilitywizards.com"
                msg.HtmlBody = True
                msg.Send()
            Else
                Me.lblMessage.Text = "We could not find any accounts with the email address or mobile number you provided."
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub
End Class