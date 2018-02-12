Imports System.Xml
Imports UtilityWizards.CommonCore.Shared.Common
Imports UtilityWizards.CommonCore.Shared.Xml
Imports Telerik.Web.UI

Public Class Profile
    Inherits builderPage

#Region "Properties"

    Public ReadOnly Property EditId As Integer
        Get
            Return App.CurrentUser.ID
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadLists()
            Me.LoadData()
        End If

        Me.SetupForm()
    End Sub

    Private Sub LoadData()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim usr As New SystemUser(Me.EditId)

            If Me.EditId = 0 Then
                Me.ddlClient.SelectedValue = App.CurrentClient.ID.ToString
            Else Me.ddlClient.SelectedValue = usr.ClientID.ToString
            End If
            Me.txtName.Text = usr.Name
            Me.txtEmail.Text = usr.Email
            Me.txtMobileUser.Text = usr.MobileUsername
            Me.txtPassword.Text = usr.Password
            Me.txtMobileNumber.Text = usr.MobileNumber
            Me.ddlPermissions.SelectedValue = CStr(usr.Permissions)
            Me.ddlSupervisor.SelectedValue = CStr(usr.SupervisorID)
            Me.txtDeviceID.Text = usr.MobileDeviceId
            Me.txtOneSignal.Text = usr.OneSignalUserID
            Me.chkApiAccess.Checked = usr.ApiEnabled
            Me.chkWebAccess.Checked = usr.WebEnabled
            Me.ddlDeviceType.SelectedValue = CStr(usr.MobileDeviceType)

            ' get the 811 settings
            Dim cmd As New SqlClient.SqlCommand("SELECT [ModuleID], [NotifyIDs], [AdminUserID], [Email], [Password], [MailServer], [FtpServer], [FtpUser], [FtpPass] FROM [811Settings] WHERE [ClientID] = " & App.CurrentClient.ID.ToString, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.chk811Record.Checked = True
                Me.ddl811Module.SelectedValue = rs("ModuleID").ToString
                Me.ddl811AdminAcount.SelectedValue = rs("AdminUserID").ToString
                Me.txt811EmailAddress.Text = rs("Email").ToString
                Me.txt811EmailPass.Text = rs("Password").ToString
                Me.txt811EmailServer.Text = rs("MailServer").ToString
                Me.txt811FtpServer.Text = rs("FtpServer").ToString
                Me.txt811FtpUser.Text = rs("FtpUser").ToString
                Me.txt811FtpPass.Text = rs("FtpPass").ToString
                Dim notifyIDs As List(Of String) = rs("NotifyIDs").ToString.Split("|"c).ToList
                For Each itm As ListItem In Me.cbl811Notify.Items
                    If notifyIDs.Contains(itm.Value) Then itm.Selected = True
                Next
            Else
                Me.chk811Record.Checked = False
                For Each itm As RadComboBoxItem In Me.ddl811Module.Items
                    If itm.Text.Contains("811") Then itm.Selected = True
                Next
                Me.ddl811AdminAcount.SelectedIndex = 0
                Me.txt811EmailAddress.Text = ""
                Me.txt811EmailPass.Text = ""
                Me.txt811EmailServer.Text = ""
                Me.txt811FtpServer.Text = "resp.nc811.org"
                Me.txt811FtpUser.Text = ""
                Me.txt811FtpPass.Text = ""
                For Each itm As ListItem In Me.cbl811Notify.Items
                    itm.Selected = False
                Next
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadLists()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim sWhere As String = ""
            If App.CurrentUser.Permissions <> Enums.SystemUserPermissions.Solvtopia Then
                sWhere = " AND [xClientID] = " & App.CurrentClient.ID
            End If

            Me.ddlClient.Items.Clear()
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Clients];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlClient.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()

            Me.ddlPermissions.Items.Clear()
            Dim enumValues As Array = System.[Enum].GetValues(GetType(Enums.SystemUserPermissions))
            For Each resource As Enums.SystemUserPermissions In enumValues
                If resource <> Enums.SystemUserPermissions.Solvtopia Then
                    Me.ddlPermissions.Items.Add(New RadComboBoxItem(resource.ToString, CStr(resource)))
                End If
            Next

            Me.ddlDeviceType.Items.Clear()
            enumValues = System.[Enum].GetValues(GetType(Enums.UserPlatform))
            For Each resource As Enums.UserPlatform In enumValues
                Me.ddlDeviceType.Items.Add(New RadComboBoxItem(resource.ToString, CStr(resource)))
            Next

            Me.ddlSupervisor.Items.Clear()
            cmd = New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Users] WHERE [xPermissions] = '" & Enums.SystemUserPermissions.Supervisor.ToString & "'" & sWhere, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlSupervisor.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()

            ' 811 options
            Me.ddl811Module.Items.Clear()
            For Each m As SystemModule In App.CurrentClient.Modules
                Me.ddl811Module.Items.Add(New RadComboBoxItem(m.Name, m.ID.ToString))
            Next

            Me.ddl811AdminAcount.Items.Clear()
            cmd = New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Users] WHERE [xPermissions] = '" & Enums.SystemUserPermissions.SystemAdministrator.ToString & "'" & sWhere, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.ddl811AdminAcount.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()

            Me.cbl811Notify.Items.Clear()
            cmd = New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Users] WHERE [Active] = 1" & sWhere, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.cbl811Notify.Items.Add(New ListItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub SetupForm()
        Me.ddlClient.Enabled = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)

        Me.ddlSupervisor.Enabled = (CType(Me.ddlPermissions.SelectedValue, Enums.SystemUserPermissions) = Enums.SystemUserPermissions.Technician)
        If Not Me.ddlSupervisor.Enabled Then
            Me.ddlSupervisor.SelectedIndex = -1
        End If

        Me.txtDeviceID.Enabled = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)
        Me.txtOneSignal.Enabled = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)
        Me.chkApiAccess.Enabled = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)
        Me.chkWebAccess.Enabled = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia)

        Me.RadTabStrip1.Tabs(1).Visible = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia Or
                                           App.CurrentUser.Permissions = Enums.SystemUserPermissions.SystemAdministrator)

        Select Case CType(Me.ddlPermissions.SelectedValue, Enums.SystemUserPermissions)
            Case Enums.SystemUserPermissions.Administrator, Enums.SystemUserPermissions.SystemAdministrator
                Me.imgAvatar.ImageUrl = "~/images/icon_administrator_avatar.png"
            Case Enums.SystemUserPermissions.Technician
                Me.imgAvatar.ImageUrl = "~/images/icon_technician_avatar.png"
            Case Enums.SystemUserPermissions.Solvtopia
                Me.imgAvatar.ImageUrl = "~/images/icon_72.png"
            Case Enums.SystemUserPermissions.Supervisor
                Me.imgAvatar.ImageUrl = "~/images/icon_supervisor_avatar.png"
            Case Else
                Me.imgAvatar.ImageUrl = "~/images/icon_user_avatar.png"
        End Select

        ' hide the 811 tab if the account doesn't have an 811 module
        Dim _811Found As Boolean = False
        For Each m As SystemModule In App.CurrentClient.Modules
            If m.Name.ToLower.Contains("811") Then
                _811Found = True
                Exit For
            End If
        Next
        Me.RadTabStrip1.Tabs(1).Visible = _811Found
    End Sub

    Private Function SaveChanges() As Boolean
        Dim retVal As Boolean = True

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim usr As New SystemUser
            usr.ID = Me.EditId
            usr.Name = Me.txtName.Text
            usr.Email = Me.txtEmail.Text
            usr.MobileUsername = Me.txtMobileUser.Text
            usr.Password = Me.txtPassword.Text
            usr.MobileNumber = Me.txtMobileNumber.Text.Replace(" ", "").Replace("-", "").Replace("(", "")
            usr.Permissions = CType(Me.ddlPermissions.SelectedValue, Enums.SystemUserPermissions)
            If usr.Permissions = Enums.SystemUserPermissions.Technician Then
                usr.SupervisorID = Me.ddlSupervisor.SelectedValue.ToInteger
            Else usr.SupervisorID = 0
            End If
            usr.ClientID = Me.ddlClient.SelectedValue.ToInteger
            usr.Active = True
            usr.ApiEnabled = Me.chkApiAccess.Checked
            usr.WebEnabled = Me.chkWebAccess.Checked
            usr.MobileDeviceType = CType(Me.ddlDeviceType.SelectedValue, Enums.UserPlatform)
            usr = usr.Save

            Dim cmd As New SqlClient.SqlCommand("EXEC [procRefreshUserIDs]", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

            ' save the 811 information
            Dim notifyIDs As String = ""
            For Each itm As ListItem In Me.cbl811Notify.Items
                If itm.Selected Then If notifyIDs = "" Then notifyIDs = itm.Value Else notifyIDs &= "|" & itm.Value
            Next
            If Me.chk811Record.Checked Then
                cmd = New SqlClient.SqlCommand("UPDATE [811Settings] SET [ModuleID] = " & Me.ddl811Module.SelectedValue &
                                               ", [NotifyIDs] = '" & notifyIDs &
                                               "', [AdminUserID] = " & Me.ddl811AdminAcount.SelectedValue &
                                               ", [Email] = '" & Me.txt811EmailAddress.Text &
                                               "', [Password] = '" & Me.txt811EmailPass.Text &
                                               "', [MailServer] = '" & Me.txt811EmailServer.Text &
                                               "', [FtpServer] = '" & Me.txt811FtpServer.Text &
                                               "', [FtpUser] = '" & Me.txt811FtpUser.Text &
                                               "', [FtpPass] = '" & Me.txt811FtpPass.Text &
                                               "' WHERE [ClientID] = " & App.CurrentClient.ID & ";", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            Else
                cmd = New SqlClient.SqlCommand("INSERT INTO [811Settings] ([ModuleID], [NotifyIDs], [AdminUserID], [Email], [Password], [MailServer], [FtpServer], [FtpUser], [FtpPass])" &
                                               " VALUES ('" & Me.ddl811Module.SelectedValue & "', '" &
                                               notifyIDs & "', '" &
                                               Me.ddl811AdminAcount.SelectedValue & "', '" &
                                               Me.txt811EmailAddress.Text & "', '" &
                                               Me.txt811EmailPass.Text & "', '" &
                                               Me.txt811EmailServer.Text & "', '" &
                                               Me.txt811FtpServer.Text & "', '" &
                                               Me.txt811FtpUser.Text & "', '" &
                                               Me.txt811FtpPass.Text & "');", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            End If

            CommonCore.Shared.Common.LogHistory("User Information Updated for " & Me.txtName.Text, App.CurrentUser.ID)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
            retVal = False
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Private Sub ClearForm()
        Me.ddlClient.SelectedIndex = App.CurrentClient.ID
        Me.txtName.Text = ""
        Me.txtEmail.Text = ""
        Me.txtMobileUser.Text = ""
        Me.txtPassword.Text = ""
        Me.ddlPermissions.SelectedIndex = -1
        Me.ddlSupervisor.SelectedIndex = -1
        Me.ddlDeviceType.SelectedValue = "100"
        Me.txtDeviceID.Text = ""
        Me.txtOneSignal.Text = ""
        Me.chkApiAccess.Checked = False
        Me.chkWebAccess.Checked = False
    End Sub

    Protected Sub txtEmail_TextChanged(sender As Object, e As EventArgs) Handles txtEmail.TextChanged
        If Me.txtEmail.Text.Contains("@") Then
            Me.txtMobileUser.Text = Me.txtEmail.Text.Split("@"c)(0)
        Else Me.txtMobileUser.Text = Me.txtEmail.Text
        End If
    End Sub

    Protected Sub txtMobileUser_TextChanged(sender As Object, e As EventArgs) Handles txtMobileUser.TextChanged
        If Me.txtEmail.Text.Trim = "" Then
            Me.txtEmail.Text = Me.txtMobileUser.Text
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Me.SaveChanges Then
            Response.Redirect("~/admin/Users.aspx", False)
        Else Me.MsgBox("We were unable to save your changes.")
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("~/default.aspx", False)
    End Sub
End Class