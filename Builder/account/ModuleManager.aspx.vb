Public Class ModuleManager
    Inherits builderPage

#Region "Properties"

    Public ReadOnly Property FolderId As Integer
        Get
            Return Request.QueryString("fid").ToInteger
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LoadModules()

        If Not IsPostBack Then
            Dim FolderName As String = "Dashboard"
            If Me.FolderId <> 0 Then FolderName = GetFolderName(Me.FolderId)
            Me.Master.TitleText = FolderName & " > Tabs"
        End If
    End Sub

    Private Sub LoadModules()
        Me.tblModules.Rows.Clear()

        If App.CurrentUser.ApprovedModules.Count = App.CurrentClient.Modules.Count Or App.CurrentUser.ApprovedModules.Count = 0 Then
            If App.CurrentClient.Modules.Count > 0 Then
                For Each m As SystemModule In App.CurrentClient.Modules
                    If m.FolderID = Me.FolderId And m.Name <> "" Then
                        Me.tblModules.Rows.Add(Me.GetModuleRow(m))
                    End If
                Next
            End If
        ElseIf App.CurrentUser.ApprovedModules.Count > 0 Then
            For Each m As SystemModule In App.CurrentClient.Modules
                ' add modules only for the active folder
                Dim addModule As Boolean = (App.CurrentUser.ApprovedModules.Contains(m.ID) And m.Name <> "")

                If addModule Then
                    Me.tblModules.Rows.Add(Me.GetModuleRow(m))
                End If
            Next
        End If
    End Sub

    Public Function GetModuleRow(ByVal m As SystemModule) As TableRow
        Dim tr As New TableRow

        Dim iconFolder As String = "modules"
        Dim iconSize As Integer = 80
        If App.CurrentClient.IconSize = Enums.IconSize.Large Then
            iconFolder = "modules_large"
            iconSize = 128
        End If

        Dim tc1 As New TableCell
        tc1.Width = New Unit(30, UnitType.Pixel)
        Dim ibtnDelete As New ImageButton
        ibtnDelete.ID = "ibtnDelete_" & m.ID
        ibtnDelete.ImageUrl = "~/images/toolbar/icon_delete.png"
        ibtnDelete.ToolTip = "Delete this Tab"
        AddHandler ibtnDelete.Click, AddressOf ibtnDelete_Click
        tc1.Controls.Add(ibtnDelete)
        tr.Cells.Add(tc1)

        Dim tc2 As New TableCell
        tc2.Width = New Unit(30, UnitType.Pixel)
        Dim ibtnEdit As New ImageButton
        ibtnEdit.ID = "ibtnEdit_" & m.ID
        ibtnEdit.ImageUrl = "~/images/toolbar/icon_edit.png"
        ibtnEdit.ToolTip = "Edit this Tab"
        AddHandler ibtnEdit.Click, AddressOf ibtnEdit_Click
        tc2.Controls.Add(ibtnEdit)
        tr.Cells.Add(tc2)

        Dim tc3 As New TableCell
        tc3.Width = New Unit(30, UnitType.Pixel)
        Dim ibtnMove As New ImageButton
        ibtnMove.ID = "ibtnMove_" & m.ID
        ibtnMove.ImageUrl = "~/images/toolbar/icon_folder.png"
        ibtnMove.ToolTip = "Move this Tab to another Folder"
        AddHandler ibtnMove.Click, AddressOf ibtnMove_Click
        tc3.Controls.Add(ibtnMove)
        tr.Cells.Add(tc3)

        Dim tc4 As New TableCell
        tc4.Text = m.Name
        If m.Description <> "" Then tc4.Text &= " > " & m.Description
        tr.Cells.Add(tc4)

        Return tr
    End Function

    Private Sub ibtnDelete_Click(sender As Object, e As ImageClickEventArgs)
        Dim ibtn As ImageButton = CType(sender, ImageButton)
        Dim ModId As Integer = ibtn.ID.Split("_"c)(1).ToInteger

        ShowInformationPopup(Enums.InformationPopupType.DeleteModule, Enums.InformationPopupButtons.YesNo, ModId)
    End Sub

    Private Sub ibtnEdit_Click(sender As Object, e As ImageClickEventArgs)
        Dim ibtn As ImageButton = CType(sender, ImageButton)
        Dim ModId As Integer = ibtn.ID.Split("_"c)(1).ToInteger

        Response.Redirect("~/account/ModuleWizard.aspx?id=" & ModId & "&fid=" & Me.FolderId & "&t=" & CStr(Enums.SystemModuleType.Module), False)
    End Sub

    Private Sub ibtnMove_Click(sender As Object, e As ImageClickEventArgs)
        Dim ibtn As ImageButton = CType(sender, ImageButton)
        Dim ModId As Integer = ibtn.ID.Split("_"c)(1).ToInteger

        ShowInformationPopup(Enums.InformationPopupType.MoveModule, Enums.InformationPopupButtons.OkCancel, ModId)
    End Sub

    Private Sub btnAddModule_Click(sender As Object, e As EventArgs) Handles btnAddModule.Click
        Layout_NewModuleClicked(Me.FolderId)
    End Sub
End Class