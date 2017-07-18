Imports System.Xml
Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Xml
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
    End Sub

    Private Sub LoadLists()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
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
            Dim sWhere As String = ""
            If App.CurrentUser.Permissions <> Enums.SystemUserPermissions.Solvtopia Then
                sWhere = " AND [xClientID] = " & App.CurrentClient.ID
            End If
            cmd = New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Users] WHERE [xPermissions] = '" & Enums.SystemUserPermissions.Supervisor.ToString & "'" & sWhere, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlSupervisor.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog
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

            Common.LogHistory("User Information Updated for " & Me.txtName.Text)

        Catch ex As Exception
            ex.WriteToErrorLog()
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