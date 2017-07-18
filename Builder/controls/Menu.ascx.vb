Public Class Menu
    Inherits System.Web.UI.UserControl

#Region "Properties"

    Public Property UrlString As String
        Get
            Return Me.lblUrlString.Text
        End Get
        Set(value As String)
            Me.lblUrlString.Text = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.pnlAdminTools.Visible = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Administrator Or
                                      App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia Or
                                      App.CurrentUser.Permissions = Enums.SystemUserPermissions.SystemAdministrator)

        ' hide the help and faq for now
        Me.lnkHelp.Visible = False
        Me.lnkFaq.Visible = False

        '' help topics are not available on phones
        'Me.lnkHelp.Visible = (Not Me.Page.OnPhone)
        'If Me.lnkHelp.Visible Then
        '    Me.lnkHelp.Visible = (Not App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)
        'End If

        Me.pnlLoginOptions.Visible = False
        Me.pnlMainOptions.Visible = False
        Me.pnlReportOptions.Visible = False

        Me.lnkHelpTopics.Visible = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)
        Me.lnkLogs.Visible = Me.lnkHelpTopics.Visible

        ' show specific options depending on the page we are displaying
        Dim currentUrl As String = Request.Url.ToString.ToLower
        Select Case True
            Case currentUrl.Contains("login.aspx")
                Me.pnlLoginOptions.Visible = True
                Me.lnkDashboard.Visible = False
                Me.lnkLogin.Visible = False
            Case currentUrl.Contains("loginhelp.aspx")
                Me.pnlLoginOptions.Visible = True
                Me.lnkDashboard.Visible = False
                Me.lnkForgotPassword.Visible = False
            Case currentUrl.Contains("register.aspx")
                Me.pnlLoginOptions.Visible = True
                Me.lnkDashboard.Visible = False
                Me.lnkForgotPassword.Visible = False
            Case currentUrl.Contains("preview.aspx")
                Me.pnlLoginOptions.Visible = True
                Me.lnkDashboard.Visible = False
                Me.lnkForgotPassword.Visible = False
            Case currentUrl.Contains("reports.aspx")
                Me.pnlReportOptions.Visible = True
            Case Else
                Me.pnlMainOptions.Visible = True
        End Select

        Me.SetActive()
    End Sub

    Private Sub SetActive()
        Dim currentUrl As String = Request.Url.ToString.ToLower
        Select Case True
            Case currentUrl.Contains("documentation/default.aspx")
                Me.liHelp.Attributes.Add("class", "active")
            Case currentUrl.Contains("faq.aspx")
                Me.liFAQ.Attributes.Add("class", "active")
            Case currentUrl.Contains("users.aspx")
                Me.liUsers.Attributes.Add("class", "active")
            Case currentUrl.Contains("logs.aspx")
                Me.liLogs.Attributes.Add("class", "active")
            Case currentUrl.Contains("helptopics.aspx")
                Me.liHelpTopics.Attributes.Add("class", "active")
            Case currentUrl.Contains("login.aspx")
                Me.liLogin.Attributes.Add("class", "active")
            Case currentUrl.Contains("loginhelp.aspx")
                Me.liForgotPassword.Attributes.Add("class", "active")
            Case currentUrl.Contains("register.aspx")
                Me.liRegister.Attributes.Add("class", "active")
            Case currentUrl.Contains("customers.aspx")
                Me.liCustomers.Attributes.Add("class", "active")
            Case Else
                Me.liDashboard.Attributes.Add("class", "active")
        End Select
    End Sub

    Private Sub lnkDashboard_Click(sender As Object, e As EventArgs) Handles lnkDashboard.Click
        Response.Redirect("~/Default.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkHelp_Click(sender As Object, e As EventArgs) Handles lnkHelp.Click
        Response.Redirect("~/documentation/Default.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkUsers_Click(sender As Object, e As EventArgs) Handles lnkUsers.Click
        Response.Redirect("~/admin/Users.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkLogs_Click(sender As Object, e As EventArgs) Handles lnkLogs.Click
        Response.Redirect("~/admin/Logs.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkForgotPassword_Click(sender As Object, e As EventArgs) Handles lnkForgotPassword.Click
        Response.Redirect("~/account/LoginHelp.aspx", False)
    End Sub

    Private Sub lnkLogin_Click(sender As Object, e As EventArgs) Handles lnkLogin.Click
        Response.Redirect("~/account/Login.aspx", False)
    End Sub

    Private Sub lnkRegister_Click(sender As Object, e As EventArgs) Handles lnkRegister.Click
        Response.Redirect("~/Register.aspx?" & Me.UrlString & "&duration=7", False)
    End Sub

    Private Sub lnkFaq_Click(sender As Object, e As EventArgs) Handles lnkFaq.Click
        Response.Redirect("~/documentation/faq.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkHelpTopics_Click(sender As Object, e As EventArgs) Handles lnkHelpTopics.Click
        Response.Redirect("~/admin/HelpTopics.aspx?" & Me.UrlString, False)
    End Sub

    Private Sub lnkReports_Click(sender As Object, e As EventArgs) Handles lnkReports.Click
        Response.Redirect("~/admin/Reports.aspx", False)
    End Sub

    Private Sub lnkNewReport_Click(sender As Object, e As EventArgs) Handles lnkNewReport.Click
        Response.Redirect("~/admin/ReportEditor.aspx?t=" & CStr(Enums.TransactionType.[New]) & "&id=0", False)
    End Sub

    Private Sub lnkCustomers_Click(sender As Object, e As EventArgs) Handles lnkCustomers.Click
        Response.Redirect("~/admin/Customers.aspx", False)
    End Sub
End Class