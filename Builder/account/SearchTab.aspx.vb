Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class SearchTab
    Inherits builderPage

#Region "Properties"


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.RadSearchGrid.Visible = False
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.RadSearchGrid.Visible = True
            Me.RadSearchGrid.DataBind()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            'cn.Close()
        End Try
    End Sub

    Private Sub RadSearchGrid_ColumnCreated(sender As Object, e As GridColumnCreatedEventArgs) Handles RadSearchGrid.ColumnCreated
        If e.Column.ColumnType.ToString = "GridEditCommandColumn" Then
            Dim Col As Telerik.Web.UI.GridEditCommandColumn = CType(Me.RadSearchGrid.Columns(e.Column.EditFormColumnIndex), GridEditCommandColumn)
            Col.EditText = "Select"
            Col = Nothing
        End If
    End Sub

    Private Sub RadSearchGrid_EditCommand(sender As Object, e As GridCommandEventArgs) Handles RadSearchGrid.EditCommand
        e.Canceled = True

        App.CurrentAccountNumber = e.Item.Cells(8).Text
        'Dim modId As Integer = Me.ModId
        'If modId = 0 Then modId = e.Item.Cells(8).Text.ToInteger
        'App.ActiveModule = New SystemModule(modId)
        'App.ActiveFolderID = App.ActiveModule.FolderID

        'Response.Redirect("~/account/Module.aspx?modid=" & Me.ModId & "&id=" & e.Item.Cells(3).Text & "&custacctnum=" & e.Item.Cells(4).Text, False)
    End Sub

    'Private Sub RadSearchGrid_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles RadSearchGrid.NeedDataSource
    '    Dim cn As New SqlClient.SqlConnection(ConnectionString)

    '    Try
    '        Me.RadSearchGrid.Visible = True

    '        Dim cmd As New SqlClient.SqlCommand("procSearchAccounts", cn)
    '        If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
    '        cmd.CommandType = CommandType.StoredProcedure
    '        cmd.Parameters.AddWithValue("@search", Me.txtSearch.Text)

    '        Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader

    '        Me.RadSearchGrid.DataSource = rs

    '    Catch ex As Exception
    '        ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
    '    Finally
    '        'cn.Close()
    '    End Try
    'End Sub

End Class