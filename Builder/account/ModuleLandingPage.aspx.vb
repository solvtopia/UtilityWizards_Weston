Imports UtilityWizards.CommonCore.Common
Imports Telerik.Web.UI

Public Class ModuleLandingPage
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property ModId As Integer
        Get
            Return Request.QueryString("modid").ToInteger
        End Get
    End Property
    Private ReadOnly Property Type As Enums.TransactionType
        Get
            Return CType(Request.QueryString("t").ToInteger, Enums.TransactionType)
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.Master.TitleText = GetFolderName(App.ActiveFolderID) & " > " & App.ActiveModule.Name & " Module"

            Me.RadCustomerGrid_New.Visible = False
            Me.RadCustomerGrid_Existing.Visible = False
            Me.imgHelp_Customers_Existing.Visible = False
            Select Case Me.Type
                Case Enums.TransactionType.Unavailable
                    Me.pnlOptions.Visible = True
                    Me.pnlNew.Visible = False
                    Me.pnlSearch.Visible = False
                Case Enums.TransactionType.New
                    Me.pnlOptions.Visible = False
                    Me.pnlNew.Visible = True
                    Me.pnlSearch.Visible = False
                Case Enums.TransactionType.Existing
                    Me.pnlOptions.Visible = False
                    Me.pnlNew.Visible = False
                    Me.pnlSearch.Visible = True
            End Select
        End If
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Me.pnlNew.Visible = True
        Me.pnlSearch.Visible = False
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Me.pnlNew.Visible = False
        Me.pnlSearch.Visible = True
    End Sub

    Protected Sub btnSearch_New_Click(sender As Object, e As EventArgs) Handles btnSearch_New.Click
        Me.RadCustomerGrid_New.Visible = True
        Me.RadCustomerGrid_New.Rebind()
    End Sub

    Private Sub RadCustomerGrid_New_ColumnCreated(sender As Object, e As GridColumnCreatedEventArgs) Handles RadCustomerGrid_New.ColumnCreated
        If e.Column.ColumnType.ToString = "GridEditCommandColumn" Then
            Dim Col As Telerik.Web.UI.GridEditCommandColumn = CType(Me.RadCustomerGrid_New.Columns(e.Column.EditFormColumnIndex), GridEditCommandColumn)
            Col.EditText = "Select"
            Col = Nothing
        End If
    End Sub

    Private Sub RadCustomerGrid_New_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles RadCustomerGrid_New.NeedDataSource
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.RadCustomerGrid_New.Visible = True

            Dim cmd As New SqlClient.SqlCommand("procCustomerSearch_new", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            Select Case Me.ddlSearchField_New.SelectedValue.ToLower
                Case "name"
                    cmd.Parameters.AddWithValue("@CustAcctNum", "")
                    cmd.Parameters.AddWithValue("@ServiceAddress", "")
                    cmd.Parameters.AddWithValue("@CustName", Me.txtSearch_New.Text)
                Case "acctnum"
                    cmd.Parameters.AddWithValue("@CustAcctNum", Me.txtSearch_New.Text)
                    cmd.Parameters.AddWithValue("@ServiceAddress", "")
                    cmd.Parameters.AddWithValue("@CustName", "")
                Case "address"
                    cmd.Parameters.AddWithValue("@CustAcctNum", "")
                    cmd.Parameters.AddWithValue("@ServiceAddress", Me.txtSearch_New.Text)
                    cmd.Parameters.AddWithValue("@CustName", "")
            End Select

            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader

            Me.RadCustomerGrid_New.DataSource = rs

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            'cn.Close()
        End Try
    End Sub

    Protected Sub btnSearch_Existing_Click(sender As Object, e As EventArgs) Handles btnSearch_Existing.Click
        If Me.ddlSearchField_Existing.SelectedValue.ToLower <> "id" Then
            Me.RadCustomerGrid_Existing.Visible = True
            Me.imgHelp_Customers_Existing.Visible = True
            Me.RadCustomerGrid_Existing.Rebind()
        Else
            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Try
                Dim custNum As String = ""
                Dim cmd As New SqlClient.SqlCommand("SELECT [xCustAcctNum] FROM [vwModuleData] WHERE [ID] = " & Me.txtSearch_Existing.Text & ";", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    If Not IsDBNull(rs("xCustAcctNum")) Then custNum = rs("xCustAcctNum").ToString
                End If
                cmd.Cancel()
                rs.Close()

                Response.Redirect("~/account/Search.aspx?modid=" & Me.ModId & "&custacctnum=" & custNum & "&id=" & Me.txtSearch_Existing.Text, False)

            Catch ex As Exception
                ex.WriteToErrorLog
            Finally
                cn.Close()
            End Try
        End If
    End Sub

    Private Sub RadCustomerGrid_Existing_ColumnCreated(sender As Object, e As GridColumnCreatedEventArgs) Handles RadCustomerGrid_Existing.ColumnCreated
        If e.Column.ColumnType.ToString = "GridEditCommandColumn" Then
            Dim Col As Telerik.Web.UI.GridEditCommandColumn = CType(Me.RadCustomerGrid_Existing.Columns(e.Column.EditFormColumnIndex), GridEditCommandColumn)
            Col.EditText = "Select"
            Col = Nothing
        End If
    End Sub

    Private Sub RadCustomerGrid_Existing_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles RadCustomerGrid_Existing.NeedDataSource
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.RadCustomerGrid_Existing.Visible = True
            Me.imgHelp_Customers_Existing.Visible = True

            Dim cmd As New SqlClient.SqlCommand("procCustomerSearch_new", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            Select Case Me.ddlSearchField_New.SelectedValue.ToLower
                Case "name"
                    cmd.Parameters.AddWithValue("@CustAcctNum", "")
                    cmd.Parameters.AddWithValue("@ServiceAddress", "")
                    cmd.Parameters.AddWithValue("@CustName", Me.txtSearch_Existing.Text)
                Case "acctnum"
                    cmd.Parameters.AddWithValue("@CustAcctNum", Me.txtSearch_Existing.Text)
                    cmd.Parameters.AddWithValue("@ServiceAddress", "")
                    cmd.Parameters.AddWithValue("@CustName", "")
            End Select

            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader

            Me.RadCustomerGrid_Existing.DataSource = rs

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            'cn.Close()
        End Try
    End Sub

    Protected Sub btnShowAll_Click(sender As Object, e As EventArgs) Handles btnShowAll.Click
        Response.Redirect("~/account/Search.aspx?modid=" & Me.ModId, False)
    End Sub

    Private Sub RadCustomerGrid_New_EditCommand(sender As Object, e As GridCommandEventArgs) Handles RadCustomerGrid_New.EditCommand
        e.Canceled = True

        Response.Redirect("~/account/Module.aspx?modid=" & Me.ModId & "&custacctnum=" & e.Item.Cells(3).Text & "&locationnum=" & e.Item.Cells(4).Text, False)
    End Sub

    Private Sub RadCustomerGrid_Existing_EditCommand(sender As Object, e As GridCommandEventArgs) Handles RadCustomerGrid_Existing.EditCommand
        e.Canceled = True

        Response.Redirect("~/account/Search.aspx?modid=" & Me.ModId & "&custacctnum=" & e.Item.Cells(3).Text & "&locationnum=" & e.Item.Cells(4).Text, False)
    End Sub

    Private Sub lnkCopyModule_Click(sender As Object, e As EventArgs) Handles lnkCopyModule.Click

    End Sub

    Private Sub lnkDeleteModule_Click(sender As Object, e As EventArgs) Handles lnkDeleteModule.Click
        ShowInformationPopup(Enums.InformationPopupType.DeleteModule, Enums.InformationPopupButtons.YesNo, Me.ModId)
    End Sub

    Private Sub lnkEditModule_Click(sender As Object, e As EventArgs) Handles lnkEditModule.Click
        Response.Redirect("~/account/ModuleWizard.aspx?id=" & Me.ModId & "&fid=" & App.ActiveFolderID & "&t=" & CStr(Enums.SystemModuleType.Module), False)
    End Sub

    Private Sub lnkMoveModule_Click(sender As Object, e As EventArgs) Handles lnkMoveModule.Click
        ShowInformationPopup(Enums.InformationPopupType.MoveModule, Enums.InformationPopupButtons.OkCancel, Me.ModId)
    End Sub
End Class