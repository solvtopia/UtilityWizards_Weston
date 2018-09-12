Imports Telerik.Web.UI
Imports UtilityWizards.CommonCore.Shared.Common

Public Class Modules
    Inherits builderPage

#Region "Properties"

    Public ReadOnly Property FolderId As Integer
        Get
            Return Request.QueryString("fid").ToInteger
        End Get
    End Property
    Public ModId As Integer = 0

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim FolderName As String = "Dashboard"
            If Me.FolderId <> 0 Then FolderName = GetFolderName(Me.FolderId)
            Me.Master.TitleText = FolderName & " > Account Record"
        End If

        Me.LoadModules()
        If Not IsPostBack Then
            Me.LoadModule(Me.ModId)
        End If

        Me.ShowOptions()
    End Sub

    Private Sub tabModules_TabClick(sender As Object, e As RadTabStripEventArgs) Handles tabModules.TabClick
        Me.ModId = e.Tab.Value.ToInteger
        Me.LoadModule(Me.ModId)
    End Sub

    Private Sub ShowOptions()
        If Me.OnPhone Then
            Me.pnlRootOptions.Visible = False
            Me.pnlRecordOptions.Visible = (App.CurrentAccountNumber <> "")
            'Me.pnlModuleOptions.Visible = False

        ElseIf Me.OnTablet Then
            Me.pnlRootOptions.Visible = False

            Me.pnlRecordOptions.Visible = (App.CurrentAccountNumber <> "")
            'Me.pnlModuleOptions.Visible = (App.ActiveModule.ID > 0)

        Else ' desktop
            Me.pnlRootOptions.Visible = (App.CurrentUser.IsSysAdmin)

            If App.CurrentUser.Permissions = Enums.SystemUserPermissions.User Then
                Me.pnlRootOptions.Visible = False
            End If

            Me.pnlRecordOptions.Visible = (App.CurrentAccountNumber <> "")
            'Me.pnlModuleOptions.Visible = (App.ActiveModule.ID > 0)
        End If

        Me.Master.MenuAjaxPanel.RaisePostBackEvent("")
    End Sub

    Private Sub LoadModules()
        Me.tabModules.Tabs.Clear()

        If App.CurrentUser.ApprovedModules.Count = App.CurrentClient.Modules.Count Or App.CurrentUser.ApprovedModules.Count = 0 Then
            If App.CurrentClient.Modules.Count > 0 Then
                For Each m As SystemModule In App.CurrentClient.Modules
                    If m.FolderID = Me.FolderId Then
                        Dim itm As New RadTab(m.Name, m.ID.ToString)
                        If Me.ModId = 0 Then Me.ModId = m.ID
                        Me.tabModules.Tabs.Add(itm)
                    End If
                Next
            End If
        ElseIf App.CurrentUser.ApprovedModules.Count > 0 Then
            For Each m As SystemModule In App.CurrentClient.Modules
                ' add modules only for the active folder
                Dim addModule As Boolean = App.CurrentUser.ApprovedModules.Contains(m.ID) And m.ID <> 109

                If addModule Then
                    Dim itm As New RadTab(m.Name, m.ID.ToString)
                    If Me.ModId = 0 Then Me.ModId = m.ID
                    Me.tabModules.Tabs.Add(itm)
                End If
            Next
        End If
    End Sub

    Public Sub LoadModule(ByVal id As Integer)
        ' set the selected tab based on module id
        Dim tab As RadTab = Me.tabModules.FindTabByValue(id.ToString)
        tab.Selected = True

        ' load the content page
        App.ActiveModule = New SystemModule(id)
        Me.pvModules.ContentUrl = "~/account/ModuleTab.aspx?modid=" & id & "&custacctnum=" & App.CurrentAccountNumber
    End Sub

    Private Sub lnkModules_Click(sender As Object, e As EventArgs) Handles lnkModules.Click
        Layout_ManageModulesClicked(App.ActiveFolderID)
    End Sub

End Class