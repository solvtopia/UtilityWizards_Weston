Imports Telerik.Web.UI

Public Class RecentWorkOrders
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Me.OnMobile Then
                Me.RadSearchGrid.Visible = False
                Me.RadSearchGridMobile.Visible = True
                Me.LoadMobileData()
            Else
                Me.RadSearchGrid.Visible = True
                Me.RadSearchGridMobile.Visible = False
            End If
        End If
    End Sub

    Private Sub LoadMobileData()
        If App.CurrentUser.Permissions = Enums.SystemUserPermissions.Supervisor Then

            Me.SqlDataSource1.SelectCommand = "SELECT * FROM [vwMobileWorkOrders] WHERE [SupervisorID] = " & App.CurrentUser.ID
            Me.SqlDataSource1.SelectCommand &= " AND [xClientID] = " & App.CurrentClient.ID & " AND DATEADD(dd, DATEDIFF(dd,0,[dtInserted]), 0) >= DATEADD(dd, DATEDIFF(dd,0,GETDATE()), -7) OR DATEADD(dd, DATEDIFF(dd,0,[dtUpdated]), 0) >= DATEADD(dd, DATEDIFF(dd,0,GETDATE()), -7) ORDER BY [ID] DESC"

        ElseIf (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Administrator Or
                App.CurrentUser.Permissions = Enums.SystemUserPermissions.SystemAdministrator) Then

            Me.SqlDataSource1.SelectCommand = "SELECT * FROM [vwMobileWorkOrders]"
            Me.SqlDataSource1.SelectCommand &= " WHERE [xClientID] = " & App.CurrentClient.ID & " AND DATEADD(dd, DATEDIFF(dd,0,[dtInserted]), 0) >= DATEADD(dd, DATEDIFF(dd,0,GETDATE()), -7) OR DATEADD(dd, DATEDIFF(dd,0,[dtUpdated]), 0) >= DATEADD(dd, DATEDIFF(dd,0,GETDATE()), -7) ORDER BY [ID] DESC"

        Else
            Me.SqlDataSource1.SelectCommand = "SELECT * FROM [vwMobileWorkOrders] WHERE [AssignedToID] = " & App.CurrentUser.ID
            Me.SqlDataSource1.SelectCommand &= " AND [xClientID] = " & App.CurrentClient.ID & " AND DATEADD(dd, DATEDIFF(dd,0,[dtInserted]), 0) >= DATEADD(dd, DATEDIFF(dd,0,GETDATE()), -7) OR DATEADD(dd, DATEDIFF(dd,0,[dtUpdated]), 0) >= DATEADD(dd, DATEDIFF(dd,0,GETDATE()), -7) ORDER BY [ID] DESC"
        End If
    End Sub

    Private Function LoadSearchFields() As String
        Dim retVal As String = ""

        ' add the folder search fields
        If App.ActiveModule.FolderID > 0 Then
            Dim folderName As String = GetFolderName(App.ActiveModule.FolderID)

            For Each q As SystemQuestion In App.RootFolderQuestions
                If q.SearchField Then
                    If retVal = "" Then
                        retVal = "ISNULL(xmlData.value('(/Data/" & folderName & "/" & q.DataFieldName & "/text())[1]', 'varchar(255)'), '') AS [" & q.Question & "]"
                    Else retVal &= ", ISNULL(xmlData.value('(/Data/" & folderName & "/" & q.DataFieldName & "/text())[1]', 'varchar(255)'), '') AS [" & q.Question & "]"
                    End If
                End If
            Next
        End If

        ' add the module search fields
        For Each q As SystemQuestion In App.ActiveModuleQuestions
            If q.SearchField Then
                If retVal = "" Then
                    retVal = "ISNULL(xmlData.value('(/Data/" & q.DataFieldName & "/text())[1]', 'varchar(255)'), '') AS [" & q.Question & "]"
                Else retVal &= ", ISNULL(xmlData.value('(/Data/" & q.DataFieldName & "/text())[1]', 'varchar(255)'), '') AS [" & q.Question & "]"
                End If
            End If
        Next

        Return retVal
    End Function

    Private Sub RadSearchGrid_EditCommand(sender As Object, e As GridCommandEventArgs) Handles RadSearchGrid.EditCommand
        e.Canceled = True

        'Dim dataItem As GridItem = e.Item
        'Dim woId As Integer = dataItem.Cells(3).Text.ToInteger

        App.ActiveModule = New SystemModule(e.Item.Cells(7).Text.ToInteger)
        App.ActiveFolderID = App.ActiveModule.FolderID
        'App.Mobile_SupervisorID = dataItem.Cells(7).Text.ToInteger
        'App.Mobile_TechnicianID = dataItem.Cells(8).Text.ToInteger

        Me.RunClientScript("window.parent.location = '../account/Module.aspx?modid=" & e.Item.Cells(7).Text & "&id=" & e.Item.Cells(3).Text & "&custacctnum=" & e.Item.Cells(4).Text & "';")
        'Response.Redirect("~/account/Module.aspx?modid=" & e.Item.Cells(7).Text & "&id=" & e.Item.Cells(3).Text & "&custacctnum=" & e.Item.Cells(4).Text, False)
    End Sub

    Private Sub RadSearchGrid_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles RadSearchGrid.NeedDataSource
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.RadSearchGrid.Visible = True

            Dim cmd As New SqlClient.SqlCommand("procSearchGrid", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@fields", Me.LoadSearchFields)
            cmd.Parameters.AddWithValue("@clientID", App.CurrentClient.ID)
            cmd.Parameters.AddWithValue("@moduleID", 0)
            cmd.Parameters.AddWithValue("@CustAcctNum", "")
            cmd.Parameters.AddWithValue("@id", 0)

            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader

            Me.RadSearchGrid.DataSource = rs

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            'cn.Close()
        End Try
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
            itm.Cells(5).Text = "<span style='color:#CC0000;'>" & itm.Cells(5).Text & "</span>"
        End If

        ' change out the enum value from status to the text
        itm.Cells(6).Text = CType(itm.Cells(6).Text.ToInteger, Enums.SystemModuleStatus).ToString
    End Sub

    Private Sub RadSearchGrid_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadSearchGrid.ItemDataBound
        Try
            If TypeOf e.Item Is GridItem Then
                Dim dataItem As GridItem = e.Item
                If IsNumeric(dataItem.Cells(3).Text) Then
                    If dataItem.Cells(3).Text = CStr(Enums.SystemModulePriority.Emergency) Then
                        dataItem.BackColor = GetColor("#EF4444")
                    Else dataItem.BackColor = GetColor("#27AAD0")
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub RadSearchGridMobile_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadSearchGridMobile.SelectedIndexChanged
        Dim dataItem As GridItem = RadSearchGridMobile.SelectedItems(0)
        App.ActiveFolderID = dataItem.Cells(6).Text.ToInteger
        App.ActiveModule = New SystemModule(dataItem.Cells(5).Text.ToInteger)
        App.Mobile_SupervisorID = dataItem.Cells(7).Text.ToInteger
        App.Mobile_TechnicianID = dataItem.Cells(8).Text.ToInteger
        Me.RunClientScript("window.parent.location = '../account/Module.aspx?modid=" & App.ActiveModule.ID & "&id=" & dataItem.Cells(4).Text & "&custacctnum=" & dataItem.Cells(9).Text & "&assignments=" & False & "';")
        'Response.Redirect("~/account/Module.aspx?modid=" & App.ActiveModule.ID & "&id=" & dataItem.Cells(4).Text & "&custacctnum=" & dataItem.Cells(9).Text & "&assignments=" & False, False)
    End Sub

    Private Sub RadSearchGridMobile_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadSearchGridMobile.ItemDataBound
        Try
            If TypeOf e.Item Is GridItem Then
                Dim dataItem As GridItem = e.Item
                If IsNumeric(dataItem.Cells(3).Text) Then
                    If dataItem.Cells(3).Text = CStr(Enums.SystemModulePriority.Emergency) Then
                        dataItem.BackColor = GetColor("#EF4444")
                    Else dataItem.BackColor = GetColor("#27AAD0")
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class