Imports Telerik.Web.UI
Imports UtilityWizards.CommonCore.Shared.Common

Public Class Customers
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

    Private Sub RadCustomerGrid_New_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles RadCustomerGrid.NeedDataSource
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim sql As String = ""
            If Me.ddlSearch.SelectedValue.ToLower = "account_number" Then
                sql = "SELECT TRIM(STR([ACCOUNT_NUMBER], 25, 0)) AS [ACCOUNT NUMBER], [BORROWER_PRIMARY_NAME] AS [PRIMARY NAME], [PROPERTY_ADDRESS_1] AS [PROPERTY ADDRESS], [PROPERTY_CITY] AS [PROPERTY CITY], [PROPERTY_STATE] AS [PROPERTY STATE], [PROPERTY_ZIP] AS [PROPERTY ZIP] FROM [_import_Standard] WHERE TRIM(STR([ACCOUNT_NUMBER], 25, 0)) LIKE @search;"
            Else sql = "SELECT TRIM(STR([ACCOUNT_NUMBER], 25, 0)) AS [ACCOUNT NUMBER], [BORROWER_PRIMARY_NAME] AS [PRIMARY NAME], [PROPERTY_ADDRESS_1] AS [PROPERTY ADDRESS], [PROPERTY_CITY] AS [PROPERTY CITY], [PROPERTY_STATE] AS [PROPERTY STATE], [PROPERTY_ZIP] AS [PROPERTY ZIP] FROM [_import_Standard] WHERE [" & Me.ddlSearch.SelectedValue & "] LIKE '%' + @search + '%';"
            End If
            Dim cmd As New SqlClient.SqlCommand(sql, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.Parameters.AddWithValue("@search", Me.txtSearch.Text)

            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader

            Me.RadCustomerGrid.DataSource = rs

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            'cn.Close()
        End Try
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Me.RadCustomerGrid.Rebind()
    End Sub
End Class