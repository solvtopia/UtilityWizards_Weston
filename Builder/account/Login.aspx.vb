Imports UtilityWizards.CommonCore.Shared.Common

Public Class Login
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblMessage.Text = ""
        Me.Master.Master.UserInfoPanel.Visible = False
        Me.Master.Master.TopRightPanel.Visible = False
        Me.Master.Master.Breadcrumbs.Visible = (Not Me.OnMobile)
        Me.imgLogo.Visible = Not Me.OnMobile
        Me.chkRememberMe.Visible = Not Me.OnMobile

        If Not IsPostBack Then
            Me.txtEmail.Focus()
        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim usr As New SystemUser(Me.txtEmail.Text, Me.txtPassword.Text, "")

            ' get the device type
            If Me.UserPlatform <> Enums.UserPlatform.Desktop Then usr.MobileDeviceType = Me.UserPlatform

            ' save the device id to the list
            If Me.DeviceId <> "" And Not usr.MobileDeviceIds.Contains(Me.DeviceId) Then usr.MobileDeviceIds.Add(Me.DeviceId)

            ' save any changes
            If usr.ID > 0 Then usr = usr.Save

            Dim cl As New SystemClient(usr.ClientID)

            App.CurrentUser = usr
            App.CurrentClient = cl

            If usr.ID = 0 Then
                Me.lblMessage.Text = "We could not find a user matching the credentials you specified.<br/><br/>If you have not registered your account yet please visit http://www.utilitywizards.com/registration to setup your profile."
            Else
                If Me.chkRememberMe.Checked And Not Me.OnLocal Then
                    Dim myCookie As New HttpCookie("UtilityWizards")
                    myCookie.Values.Add("userid", usr.ID.ToString())
                    myCookie.Expires = DateTime.Now.AddYears(1)
                    Response.Cookies.Add(myCookie)
                End If

                If cl.Approved = Enums.SystemMode.Demo And DateDiff(DateInterval.Day, cl.DemoStartDate, Now.Date) > cl.DemoDuration Then
                    Me.lblMessage.Text = "Your system demo period has expired.<br/><br/>Please contact sales@solvtopia.com for more information."
                ElseIf Not App.CurrentClient.Active Then
                    Me.lblMessage.Text = "Your client profile is currently disabled.<br/><br/>Please contact sales@solvtopia.com for more information."
                Else
                    If Me.OnPhone Or Me.OnTablet Then
                        Response.Redirect("~/account/MobileLandingPage.aspx", False)
                    Else Response.Redirect("~/Default.aspx", False)
                    End If
                End If
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub
End Class