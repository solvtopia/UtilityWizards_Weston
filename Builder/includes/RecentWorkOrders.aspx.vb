Imports Telerik.Web.UI

Public Class RecentWorkOrders
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblClientID.Text = App.CurrentClient.ID.ToString

        If App.CurrentUser.Permissions = Enums.SystemUserPermissions.Administrator Or
                App.CurrentUser.Permissions = Enums.SystemUserPermissions.Solvtopia Or
                App.CurrentUser.Permissions = Enums.SystemUserPermissions.SystemAdministrator Then

            Me.lblUserID.Text = "0"
        Else Me.lblUserID.Text = App.CurrentUser.ID.ToString
        End If

        If Not IsPostBack Then
            If Me.OnMobile Then
                Me.RadSearchGrid.Visible = False
                Me.RadSearchGridMobile.Visible = True
            Else
                Me.RadSearchGrid.Visible = True
                Me.RadSearchGridMobile.Visible = False
            End If
        End If
    End Sub

    Private Sub RadSearchGrid_EditCommand(sender As Object, e As GridCommandEventArgs) Handles RadSearchGrid.EditCommand
        e.Canceled = True

        'Dim dataItem As GridItem = e.Item
        'Dim woId As Integer = dataItem.Cells(3).Text.ToInteger

        App.ActiveModule = New SystemModule(e.Item.Cells(7).Text.ToInteger)
        App.ActiveFolderID = App.ActiveModule.FolderID
        'App.Mobile_SupervisorID = dataItem.Cells(7).Text.ToInteger
        'App.Mobile_TechnicianID = dataItem.Cells(8).Text.ToInteger

        Me.RunClientScript("window.parent.location = '../account/Module.aspx?modid=" & e.Item.Cells(7).Text & "&id=" & e.Item.Cells(2).Text & "&custacctnum=" & e.Item.Cells(3).Text & "';")
        'Response.Redirect("~/account/Module.aspx?modid=" & e.Item.Cells(7).Text & "&id=" & e.Item.Cells(3).Text & "&custacctnum=" & e.Item.Cells(4).Text, False)
    End Sub

    Private Sub RadSearchGrid_ItemCreated(sender As Object, e As GridItemEventArgs) Handles RadSearchGrid.ItemCreated
        If TypeOf e.Item Is GridDataItem Then
            AddHandler e.Item.PreRender, AddressOf RadSearchGrid_ItemPreRender
        End If
    End Sub

    Protected Sub RadSearchGrid_ItemPreRender(ByVal sender As Object, ByVal e As EventArgs)
        ' this event fires when the user checks a box or expands a category
        Dim itm As GridItem = CType(sender, GridDataItem)

        ' change priority to red if emergency
        If itm.Cells(5).Text.ToLower = Enums.SystemModulePriority.Emergency.ToString.ToLower Then
            itm.Cells(5).Text = "<span style='color:#FFFFFF;'>" & itm.Cells(5).Text & "</span>"
        ElseIf itm.Cells(5).Text.ToLower = Enums.SystemModulePriority.Emergency.ToString.ToLower Then
            'itm.Cells(5).Text = "<span style='color:#FF6600;'>" & itm.Cells(5).Text & "</span>"
        End If

        ' change out the enum value from status to the text
        itm.Cells(6).Text = CType(itm.Cells(6).Text.ToInteger, Enums.SystemModuleStatus).ToString
    End Sub

    Private Sub RadSearchGrid_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadSearchGrid.ItemDataBound
        Try
            If TypeOf e.Item Is GridItem Then
                Dim dataItem As GridItem = e.Item
                If dataItem.Cells(5).Text.ToLower = Enums.SystemModulePriority.Emergency.ToString.ToLower Then
                    dataItem.BackColor = GetColor("#CC0000")
                ElseIf dataItem.Cells(5).Text.ToLower = Enums.SystemModulePriority.High.ToString.ToLower Then
                    dataItem.BackColor = GetColor("#FF6600")
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub RadSearchGridMobile_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadSearchGridMobile.SelectedIndexChanged
        ' priority (3), id (4), moduleid (5), folderid (6), supervisorid (7), custacctnum (8)
        Dim dataItem As GridItem = RadSearchGridMobile.SelectedItems(0)
        App.ActiveModule = New SystemModule(dataItem.Cells(5).Text.ToInteger)
        App.ActiveFolderID = App.ActiveModule.FolderID
        App.Mobile_SupervisorID = dataItem.Cells(7).Text.ToInteger
        App.Mobile_TechnicianID = dataItem.Cells(8).Text.ToInteger
        Me.RunClientScript("window.parent.location = '../account/Module.aspx?modid=" & App.ActiveModule.ID & "&id=" & dataItem.Cells(4).Text & "&custacctnum=" & dataItem.Cells(8).Text & "&assignments=" & False & "';")
        'Response.Redirect("~/account/Module.aspx?modid=" & App.ActiveModule.ID & "&id=" & dataItem.Cells(4).Text & "&custacctnum=" & dataItem.Cells(9).Text & "&assignments=" & False, False)
    End Sub

    Private Sub RadSearchGridMobile_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadSearchGridMobile.ItemDataBound
        Try
            If TypeOf e.Item Is GridItem Then
                Dim dataItem As GridItem = e.Item
                If dataItem.Cells(3).Text.ToLower = Enums.SystemModulePriority.Emergency.ToString.ToLower Then
                    dataItem.BackColor = GetColor("#CC0000")
                ElseIf dataItem.Cells(3).Text.ToLower = Enums.SystemModulePriority.High.ToString.ToLower Then
                    dataItem.BackColor = GetColor("#FF6600")
                Else
                    dataItem.BackColor = GetColor("#27AAD0")
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class