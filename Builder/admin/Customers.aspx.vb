Imports Telerik.Web.UI

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
            cmd.Parameters.AddWithValue("@ServiceAddress", "")
            cmd.Parameters.AddWithValue("@CustName", "")

            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader

            Me.RadCustomerGrid.DataSource = rs

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            'cn.Close()
        End Try
    End Sub


End Class