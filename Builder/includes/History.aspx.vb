Imports UtilityWizards.CommonCore.Shared.Common

Public Class History
    Inherits System.Web.UI.Page

#Region "Properties"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LoadHistory()
    End Sub

    Private Sub LoadHistory()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Me.tblHistory.Rows.Clear()
            Dim hr As New TableRow
            Dim hc1 As New TableCell
            hc1.Font.Bold = True
            hc1.Text = "User"
            Dim hc2 As New TableCell
            hc2.Width = New Unit(10, UnitType.Pixel)
            hc2.Text = "&nbsp;"
            Dim hc3 As New TableCell
            hc3.Font.Bold = True
            hc3.Text = "Action"
            hr.Cells.Add(hc1)
            hr.Cells.Add(hc2)
            hr.Cells.Add(hc3)
            Me.tblHistory.Rows.Add(hr)

            Dim rowCount As Integer = 0
            Dim cmd As New SqlClient.SqlCommand("SELECT [itemText], [UserName] FROM [vwHistory] WHERE DATEADD(dd, DATEDIFF(dd,0,[dtInserted]), 0) = DATEADD(dd, DATEDIFF(dd,0,GETDATE()), 0) AND [ClientID] = " & App.CurrentClient.ID & " ORDER BY [dtInserted] DESC", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim tr As New TableRow
                Dim tc1 As New TableCell
                tc1.VerticalAlign = VerticalAlign.Top
                tc1.Wrap = False
                tc1.Text = rs("UserName").ToString
                Dim tc2 As New TableCell
                tc2.Width = New Unit(10, UnitType.Pixel)
                tc2.Text = "&nbsp;"
                Dim tc3 As New TableCell
                tc3.VerticalAlign = VerticalAlign.Top
                tc3.Text = rs("itemText").ToString
                tr.Cells.Add(tc1)
                tr.Cells.Add(tc2)
                tr.Cells.Add(tc3)
                Me.tblHistory.Rows.Add(tr)

                rowCount += 1
            Loop

            If rowCount = 0 Then
                Dim tr As New TableRow
                Dim tc1 As New TableCell
                tc1.ColumnSpan = 3
                tc1.Text = "No records yet to show for today."
                tr.Cells.Add(tc1)
                Me.tblHistory.Rows.Add(tr)
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub
End Class