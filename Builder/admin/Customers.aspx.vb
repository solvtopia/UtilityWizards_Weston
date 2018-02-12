Imports Telerik.Web.UI
Imports UtilityWizards.CommonCore.Shared.Common

Public Class Customers
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub RadCustomerGrid_New_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles RadCustomerGrid.NeedDataSource
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("procCustomerSearch_new", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@CustAcctNum", "")
            If Me.ddlSearch.SelectedValue.ToLower = "serviceaddress" Then
                cmd.Parameters.AddWithValue("@ServiceAddress", Me.txtSearch.Text)
            Else cmd.Parameters.AddWithValue("@ServiceAddress", "")
            End If
            cmd.Parameters.AddWithValue("@CustName", "")
            If Me.ddlSearch.SelectedValue.ToLower = "receptacle" Then
                cmd.Parameters.AddWithValue("@Receptacle", Me.txtSearch.Text)
            Else cmd.Parameters.AddWithValue("@Receptacle", "")
            End If

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