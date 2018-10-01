Imports System.Xml
Imports UtilityWizards.CommonCore.Shared.Common
Imports UtilityWizards.CommonCore.Shared.Xml
Imports Telerik.Web.UI

Public Class AdminUserEditor
    Inherits builderPage

#Region "Properties"

    Dim maxSteps As Integer = 6
    Dim wizardMaxSteps As Integer = 3
    Public ReadOnly Property EditId As Integer
        Get
            Return CInt(Request.QueryString("id"))
        End Get
    End Property
    Public ReadOnly Property TransactionType As Enums.TransactionType
        Get
            Return CType(Request.QueryString("t"), Enums.TransactionType)
        End Get
    End Property
    Private Property WizardStep As Integer
        Get
            Dim retVal As Integer = 0

            For x As Integer = 1 To maxSteps
                If CType(Me.FindInControl("pnlStep" & x), Panel).Visible Then
                    retVal = x
                    Exit For
                End If
            Next

            Return retVal
        End Get
        Set(value As Integer)
            For x As Integer = 1 To maxSteps
                If x <> value Then
                    CType(Me.FindInControl("pnlStep" & x), Panel).Visible = False
                    CType(Me.FindInControl("pnlStepInfo" & x), Panel).Visible = False
                Else
                    CType(Me.FindInControl("pnlStep" & x), Panel).Visible = True
                    CType(Me.FindInControl("pnlStepInfo" & x), Panel).Visible = True

                    If value < wizardMaxSteps Then
                        Me.btnNext.Text = "Next"
                        Me.btnBack.Visible = (value > 1)
                    Else
                        Me.btnNext.Visible = True
                        Me.btnNext.Text = "Finish"
                    End If
                End If
            Next
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.WizardStep = 1

            Me.LoadLists()
            Me.LoadData()
        End If

        Me.SetupForm()
    End Sub

    Private Sub LoadData()
        Dim usr As New SystemUser(Me.EditId, App.UseSandboxDb)

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
        If Me.TransactionType = Enums.TransactionType.New Then
            Me.chkApiAccess.Checked = True
            Me.chkWebAccess.Checked = True
            Me.ddlDeviceType.SelectedValue = "100"
        Else
            Me.chkApiAccess.Checked = usr.ApiEnabled
            Me.chkWebAccess.Checked = usr.WebEnabled
            Me.ddlDeviceType.SelectedValue = CStr(usr.MobileDeviceType)
        End If

        If usr.ApprovedModules Is Nothing OrElse usr.ApprovedModules.Count = 0 Then
            ' no selected modules so check everything
            For Each itm As RadTreeNode In Me.tvModules.Nodes
                itm.Checked = True
                itm.CheckChildNodes()
            Next
        Else
            ' only check the selected modules
            For Each i As Integer In usr.ApprovedModules
                For Each pi As RadTreeNode In Me.tvModules.Nodes
                    If pi.Nodes.Count > 0 Then
                        For Each itm As RadTreeNode In pi.Nodes
                            If itm.Value.ToInteger = i Then itm.Checked = True : Exit For
                        Next
                    Else
                        If pi.Value.ToInteger = i Then pi.Checked = True : Exit For
                    End If
                Next
            Next
        End If
    End Sub

    Private Sub LoadLists()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

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

            Dim lastFolderId As Integer = 0
            Dim lstNodes As New List(Of RadTreeNode)
            Me.tvModules.Nodes.Clear()
            For Each m As SystemModule In App.CurrentClient.Modules
                If m.Type = Enums.SystemModuleType.Folder Then
                    lastFolderId = m.ID
                    lstNodes.Add(New RadTreeNode(m.Name, m.ID.ToString))
                ElseIf m.Type = Enums.SystemModuleType.Module Then
                    Dim parentNode As RadTreeNode = Nothing
                    If m.FolderID > 0 Then
                        For Each itm As RadTreeNode In lstNodes
                            If itm.Value.ToInteger = m.FolderID Then
                                parentNode = itm
                                Exit For
                            End If
                        Next
                    End If
                    If parentNode IsNot Nothing Then
                        parentNode.Nodes.Add(New RadTreeNode(m.Name, m.ID.ToString))
                    Else lstNodes.Add(New RadTreeNode(m.Name, m.ID.ToString))
                    End If
                End If
            Next
            For Each itm As RadTreeNode In lstNodes
                itm.Expanded = True
                'itm.Checked = True
                'itm.CheckChildNodes()
                Me.tvModules.Nodes.Add(itm)
            Next

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
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
        Me.pnlSupervisor.Visible = Me.ddlSupervisor.Enabled

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

        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Dim usr As New SystemUser(App.UseSandboxDb)
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

            Dim approvedModules As New List(Of Integer)
            For Each pi As RadTreeNode In Me.tvModules.Nodes
                If pi.Nodes.Count > 0 Then
                    For Each itm As RadTreeNode In pi.Nodes
                        If itm.Checked Then approvedModules.Add(itm.Value.ToInteger)
                    Next
                Else
                    If pi.Checked Then approvedModules.Add(pi.Value.ToInteger)
                End If
            Next
            usr.ApprovedModules = approvedModules

            usr = usr.Save

            Dim cmd As New SqlClient.SqlCommand("EXEC [procRefreshUserIDs]", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

            LogHistory("User Information Updated for " & Me.txtName.Text, App.CurrentUser.ID, App.UseSandboxDb)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
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

    Private Sub lnkDeleteUser_Click(sender As Object, e As EventArgs) Handles lnkDeleteUser.Click
        ShowInformationPopup(Enums.InformationPopupType.DeleteUser, Enums.InformationPopupButtons.YesNo, Me.EditId)
    End Sub

    Private Sub lnkClearUser_Click(sender As Object, e As EventArgs) Handles lnkClearUser.Click
        Me.ClearForm()
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.WizardStep -= 1
    End Sub

    Protected Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If Me.WizardStep < Me.wizardMaxSteps Then
            Me.WizardStep += 1
        Else
            If Me.SaveChanges Then
                Response.Redirect("~/admin/Users.aspx", False)
            Else Me.MsgBox("We were unable to save your changes.")
            End If
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("~/admin/Users.aspx", False)
    End Sub

    Private Sub lnkNewUser_Click(sender As Object, e As EventArgs) Handles lnkNewUser.Click
        Response.Redirect("~/admin/UserEditor.aspx?t=" & CStr(Enums.TransactionType.[New]) & "&id=0", False)
    End Sub
End Class