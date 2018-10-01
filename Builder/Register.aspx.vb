Imports UtilityWizards.CommonCore.Shared.Common
Imports System.Configuration.ConfigurationManager

Public Class Register
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property Duration As Integer
        Get
            If Request.QueryString("duration") = "" Then
                Return 7
            Else Return Request.QueryString("duration").ToInteger
            End If
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.Master.UserInfoPanel.Visible = False
        Me.Master.Master.TopRightPanel.Visible = False
        Me.Master.Master.Breadcrumbs.Visible = (Not Me.OnMobile)

        If Not IsPostBack Then
            Me.pnlStep1.Visible = True
            Me.pnlDone.Visible = False
        End If
    End Sub

    Protected Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            ' save the client record
            Dim cl As New SystemClient(App.UseSandboxDb)
            cl.ID = 0
            cl.Name = Me.txtClientName.Text
            cl.Address1 = Me.txtClientAddress1.Text
            cl.Address2 = Me.txtClientAddress2.Text
            cl.City = Me.txtClientCity.Text
            cl.State = Me.txtClientState.Text
            cl.ZipCode = Me.txtClientZipCode.Text
            cl.ContactName = Me.txtClientContactName.Text
            cl.ContactPhone = Me.txtClientContactPhone.Text
            cl.ContactEmail = Me.txtClientContactEmail.Text
            cl.Active = True
            cl.DemoStartDate = Now.Date
            cl.DemoDuration = Me.Duration
            cl.Approved = Enums.SystemMode.Demo
            cl = cl.Save()

            ' save the user record
            Dim permissions As Enums.SystemUserPermissions = Enums.SystemUserPermissions.SystemAdministrator
            If Me.txtAdminEmail.Text.ToLower.Contains("@nk5.co") Or Me.txtAdminEmail.Text.ToLower.Contains("@solvtopia.com") Then permissions = Enums.SystemUserPermissions.Solvtopia

            Dim usr As New SystemUser(App.UseSandboxDb)
            usr.ID = 0
            usr.ClientID = cl.ID
            usr.Name = Me.txtAdminName.Text
            usr.Email = Me.txtAdminEmail.Text
            usr.Password = Me.txtAdminPasswordConfirm.Text
            usr.Permissions = permissions
            usr.Active = True
            usr.WebEnabled = True
            usr = usr.Save

            If cl.ID > 0 And usr.ID > 0 Then
                Dim cmd As New SqlClient.SqlCommand("EXEC [procRefreshAllIDs]", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()

                Me.pnlStep1.Visible = False
                Me.pnlDone.Visible = True

                ' email the customer
                Dim msg As New Mailer
                msg.HostServer = AppSettings("MailHost")
                msg.UserName = AppSettings("MailUser")
                msg.Password = AppSettings("MailPassword")
                msg.To.Add(Me.txtClientContactEmail.Text)
                msg.BCC.Add("james@nk5.co")
                msg.Body = "<html>Welcome to Utility Wizards!<br/><br/>Your accounts have been created and your information has been sent to our sales department.<br/><br/>We will be in touch in less than 24 hours to approve your access.<br/><br/>Click Login on the menu or visit <a href='https://weston.utilitywizards.com'>Utility Wizards</a> to start using your new account immediately!<br/><br/>Username: " & Me.txtAdminEmail.Text & "<br/>Password: " & Me.txtAdminPasswordConfirm.Text & "</html>"
                msg.Subject = "Welcome to Utility Wizards!"
                msg.From = "sales@utilitywizards.com"
                msg.HtmlBody = True
                msg.Send()

                ' email jose and let him know
                msg = New Mailer
                msg.HostServer = AppSettings("MailHost")
                msg.UserName = AppSettings("MailUser")
                msg.Password = AppSettings("MailPassword")
                msg.To.Add("jose@solvtopia.com")
                msg.Body = "<html>A new customer has registered for a demo on Utility Wizards!<br/><br/>Client Name: " & Me.txtClientName.Text & "<br/>Contact Name: " & Me.txtClientContactName.Text & "<br/>Contact Email: " & Me.txtClientContactEmail.Text & "<br/>Contact Phone: " & Me.txtClientContactPhone.Text & "</html>"
                msg.Subject = "Welcome to Utility Wizards!"
                msg.From = "sales@utilitywizards.com"
                msg.HtmlBody = True
                msg.Send()
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub
End Class