Imports Telerik.Web.UI

Public Class _Default4
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AddHandler Master.NewModuleClicked, AddressOf Layout_NewModuleClicked
        AddHandler Master.FolderClicked, AddressOf Layout_FolderClicked

        If App.CurrentClient.ID > 0 Then Me.LoadModules()

        If Not IsPostBack Then
            ' handles the work order id passed from text messages
            If Me.WoId > 0 Then
                Dim cn As New SqlClient.SqlConnection(ConnectionString)

                Try
                    Dim custNum As String = ""
                    Dim mId As Integer = 0
                    Dim cmd As New SqlClient.SqlCommand("SELECT [xCustAcctNum], [xModuleID] FROM [vwModuleData] WHERE [ID] = " & Me.WoId & ";", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    If rs.Read Then
                        If Not IsDBNull(rs("xCustAcctNum")) Then custNum = rs("xCustAcctNum").ToString
                        mId = rs("xModuleID").ToString.ToInteger
                    End If
                    cmd.Cancel()
                    rs.Close()

                    If mId > 0 And custNum <> "" Then
                        App.ActiveModule = New SystemModule(mId)
                        App.ActiveFolderID = App.ActiveModule.FolderID

                        Dim url As String = "~/account/Module.aspx?modid=" & mId & "&id=" & Me.WoId & "&custacctnum=" & custNum
                        Me.lblAppUrl.Text = url & "<br/>"
                        'Response.Redirect(url, False)
                    End If

                Catch ex As Exception
                    ex.WriteToErrorLog
                Finally
                    cn.Close()
                End Try
            Else
                Me.lblAppUrl.Text = "No Work Order Id Provided<br/>"
            End If
        End If

        Me.ShowOptions()
    End Sub

    Private Sub ShowOptions()
        If Me.OnPhone Then
            Me.pnlRootOptions.Visible = False
            Me.pnlFolderOptions.Visible = False
            Me.pnlBadges.Visible = False
            Me.pnlActivity.Visible = False

        ElseIf Me.OnTablet Then
            Me.pnlRootOptions.Visible = False
            Me.pnlFolderOptions.Visible = False

            Me.pnlBadges.Visible = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Administrator Or
                                      App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia Or
                                      App.CurrentUser.Permissions = Enums.SystemUserPermissions.SystemAdministrator)
            Me.pnlActivity.Visible = Me.pnlBadges.Visible

        Else ' desktop
            Me.pnlRootOptions.Visible = (App.ActiveFolderID = 0 And App.CurrentUser.IsSysAdmin)
            Me.pnlFolderOptions.Visible = (App.ActiveFolderID > 0 And App.CurrentUser.IsSysAdmin)

            If App.CurrentUser.Permissions = Enums.SystemUserPermissions.User Then
                Me.pnlRootOptions.Visible = False
                Me.pnlFolderOptions.Visible = False
            End If

            Me.pnlBadges.Visible = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Administrator Or
                                      App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia Or
                                      App.CurrentUser.Permissions = Enums.SystemUserPermissions.SystemAdministrator)
            Me.pnlActivity.Visible = Me.pnlBadges.Visible
        End If

        Me.Master.MenuAjaxPanel.RaisePostBackEvent("")

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.lblOpenWorkOrders.Text = "0"
            Me.lblCompletedWorkOrders.Text = "0"

            Dim cmd As New SqlClient.SqlCommand("SELECT COUNT([ID]) AS [WOCount] FROM [vwModuleData] WHERE [xStatus] = " & Enums.SystemModuleStatus.Open & " AND [xClientId] = " & App.CurrentClient.ID, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.lblOpenWorkOrders.Text = FormatNumber(rs("WOCount").ToString, 0)
            End If
            cmd.Cancel()
            rs.Close()

            cmd = New SqlClient.SqlCommand("SELECT COUNT([ID]) AS [WOCount] FROM [vwModuleData] WHERE [xStatus] = " & Enums.SystemModuleStatus.Completed & " AND [xClientId] = " & App.CurrentClient.ID, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            If rs.Read Then
                Me.lblCompletedWorkOrders.Text = FormatNumber(rs("WOCount").ToString, 0)
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadModules()
        Me.tblModules.Rows.Clear()

        Dim tr As New TableRow

        Dim goHomeAdded As Boolean = False

        Dim iconFolder As String = "modules"
        Dim iconSize As Integer = 80
        If App.CurrentClient.IconSize = Enums.IconSize.Large Then
            iconFolder = "modules_large"
            iconSize = 128
        End If

        If App.CurrentUser.ApprovedModules.Count = App.CurrentClient.Modules.Count Or App.CurrentUser.ApprovedModules.Count = 0 Then
            If App.ActiveFolderID > 0 Then
                ' inside a folder, show the description if the folder has one
                Dim f As New SystemModule(App.ActiveFolderID)
                If f.Description <> "" AndAlso f.Description.Trim <> "" Then
                    Dim trDesc As New TableRow
                    Dim tcDesc As New TableCell
                    tcDesc.Attributes.Add("style", "display: inline-block;")
                    tcDesc.BorderColor = Drawing.Color.Silver
                    tcDesc.BorderStyle = BorderStyle.Solid
                    tcDesc.BorderWidth = New Unit(1, UnitType.Pixel)
                    tcDesc.BackColor = Drawing.Color.White
                    tcDesc.Text = f.Description
                    trDesc.Cells.Add(tcDesc)
                    Me.tblModules.Rows.Add(trDesc)
                End If
            End If

            If App.CurrentClient.Modules.Count > 0 Then
                If App.ActiveFolderID > 0 And Not goHomeAdded Then
                    ' add the go home button if we are inside a folder
                    Dim tc As New TableCell
                    tc.Attributes.Add("style", "display: inline-block; padding: 10px;")
                    tc.Height = New Unit(80, UnitType.Pixel)
                    tc.VerticalAlign = VerticalAlign.Top
                    tc.HorizontalAlign = HorizontalAlign.Center
                    Dim ibtn As New Telerik.Web.UI.RadButton
                    ibtn.ID = "ibtnModule_0"
                    ibtn.Image.ImageUrl = "~/images/gallery/" & iconFolder & "/up_folder.png"
                    ibtn.Image.IsBackgroundImage = True
                    ibtn.Height = New Unit(iconSize, UnitType.Pixel)
                    ibtn.Width = New Unit(iconSize, UnitType.Pixel)
                    AddHandler ibtn.Click, AddressOf ibtnModule_Click
                    Dim lit As New Literal
                    lit.ID = "litModule_0"
                    lit.Text = "<br/>"
                    Dim lbl As New Label
                    lbl.ID = "lblModule_0"
                    lbl.Text = "Go Home"
                    tc.Controls.Add(ibtn)
                    tc.Controls.Add(lit)
                    'tc.Controls.Add(lbl)
                    tr.Cells.Add(tc)

                    goHomeAdded = True
                End If

                For Each m As SystemModule In App.CurrentClient.Modules
                    If m.FolderID = App.ActiveFolderID Then
                        Dim tc As New TableCell
                        tc.Attributes.Add("style", "display: inline-block; padding: 10px;")
                        tc.VerticalAlign = VerticalAlign.Top
                        tc.HorizontalAlign = HorizontalAlign.Center
                        Dim ibtn As New Telerik.Web.UI.RadButton
                        ibtn.ID = "ibtnModule_" & m.ID
                        ibtn.Image.ImageUrl = m.Icon
                        ibtn.Image.IsBackgroundImage = True
                        ibtn.Height = New Unit(iconSize, UnitType.Pixel)
                        ibtn.Width = New Unit(iconSize, UnitType.Pixel)
                        AddHandler ibtn.Click, AddressOf ibtnModule_Click
                        Dim lit As New Literal
                        lit.ID = "litModule_" & m.ID
                        lit.Text = "<br/>"
                        Dim lbl As New Label
                        lbl.ID = "lblModule_" & m.ID
                        lbl.Text = m.Name
                        tc.Controls.Add(ibtn)
                        tc.Controls.Add(lit)
                        tc.Controls.Add(lbl)
                        tr.Cells.Add(tc)
                    End If
                Next

                Me.tblModules.Rows.Add(tr)
            Else
                Dim tc As New TableCell
                tc.Attributes.Add("style", "display: inline-block;")
                tc.Text = "Click on &lt;Add A New Module&gt; on the left under &lt;" & IIf(App.ActiveFolderID = 0, "Dashboard", "Folder").ToString & " Actions&gt; to begin."
                tr.Cells.Add(tc)
                Me.tblModules.Rows.Add(tr)
            End If
        ElseIf App.CurrentUser.ApprovedModules.Count > 0 Then
            For Each m As SystemModule In App.CurrentClient.Modules
                ' add modules only for the active folder
                Dim addModule As Boolean = App.CurrentUser.ApprovedModules.Contains(m.ID)

                If addModule Then
                    Dim tc As New TableCell
                    tc.Attributes.Add("style", "display: inline-block; padding: 10px;")
                    tc.VerticalAlign = VerticalAlign.Top
                    tc.HorizontalAlign = HorizontalAlign.Center
                    Dim ibtn As New Telerik.Web.UI.RadButton
                    ibtn.ID = "ibtnModule_" & m.ID
                    ibtn.Image.ImageUrl = m.Icon
                    ibtn.Image.IsBackgroundImage = True
                    ibtn.Height = New Unit(iconSize, UnitType.Pixel)
                    ibtn.Width = New Unit(iconSize, UnitType.Pixel)
                    AddHandler ibtn.Click, AddressOf ibtnModule_Click
                    Dim lit As New Literal
                    lit.ID = "litModule_" & m.ID
                    lit.Text = "<br/>"
                    Dim lbl As New Label
                    lbl.ID = "lblModule_" & m.ID
                    If m.FolderID = 0 Then lbl.Text = m.Name Else lbl.Text = GetFolderName(m.FolderID) & " > " & m.Name
                    tc.Controls.Add(ibtn)
                    tc.Controls.Add(lit)
                    tc.Controls.Add(lbl)
                    tr.Cells.Add(tc)
                End If
            Next

            Me.tblModules.Rows.Add(tr)
        End If

        If App.ActiveFolderID = 0 Then
            Me.lblHeader.Text = "System Modules"
        Else Me.lblHeader.Text = "System Modules > " & App.CurrentClient.Modules.GetByID(App.ActiveFolderID).Name
        End If
    End Sub

    Protected Sub ibtnModule_Click(sender As Object, e As EventArgs)
        Dim ibtn As Telerik.Web.UI.RadButton = CType(sender, Telerik.Web.UI.RadButton)
        Dim modId As Integer = ibtn.ID.Split("_"c)(1).ToInteger

        If modId = 0 Then
            App.ActiveFolderID = 0
            Me.LoadModules()
        Else
            If App.CurrentClient.Modules.GetByID(modId).Type = Enums.SystemModuleType.Folder Then
                App.ActiveFolderID = modId
                Me.LoadModules()
            Else
                App.ActiveModule = New SystemModule(modId)
                ShowModule(modId)
            End If
        End If

        Me.ShowOptions()
    End Sub

    Private Sub lnkNewModule_Click(sender As Object, e As EventArgs) Handles lnkNewModule.Click, lnkNewFolderModule.Click
        Layout_NewModuleClicked(App.ActiveFolderID)
    End Sub

    Private Sub lnkNewFolder_Click(sender As Object, e As EventArgs) Handles lnkNewFolder.Click
        Layout_FolderClicked(0)
    End Sub

    Private Sub lnkEditFolder_Click(sender As Object, e As EventArgs) Handles lnkEditFolder.Click
        Layout_FolderClicked(App.ActiveFolderID)
    End Sub

    Private Sub lnkDeleteFolder_Click(sender As Object, e As EventArgs) Handles lnkDeleteFolder.Click
        ShowInformationPopup(Enums.InformationPopupType.DeleteFolder, Enums.InformationPopupButtons.YesNo, App.ActiveFolderID)
    End Sub
End Class