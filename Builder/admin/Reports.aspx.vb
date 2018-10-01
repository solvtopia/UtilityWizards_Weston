Imports System.Xml
Imports UtilityWizards.CommonCore.Shared.Common
Imports UtilityWizards.CommonCore.Shared.Xml
Imports Telerik.Web.UI

Public Class Reports
    Inherits builderPage

#Region "Properties"

    Public Property EditId As Integer
        Get
            If Not IsNumeric(Me.lblEditID.Text) Then Me.lblEditID.Text = "0"
            Return Me.lblEditID.Text.ToInteger
        End Get
        Set(value As Integer)
            Me.lblEditID.Text = value.ToString
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LoadReports()

        If Not IsPostBack Then
            Me.EditId = 0
        End If
    End Sub

    Private Sub LoadReports()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Dim lst As New List(Of SystemReport)

            Dim cmd As New SqlClient.SqlCommand("EXEC [procRefreshReportIDs]", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

            Dim sql As String = "SELECT [ID] FROM [Reports] WHERE [xClientID] = " & App.CurrentClient.ID & " ORDER BY [xName]"
            cmd = New SqlClient.SqlCommand(sql, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                lst.Add(New SystemReport(rs("ID").ToString.ToInteger, App.UseSandboxDb))
            Loop
            rs.Close()
            cmd.Cancel()

            Me.tblReports.Rows.Clear()

            Dim iconSize As Integer = 80
            If App.CurrentClient.IconSize = Enums.IconSize.Large Then
                iconSize = 128
            End If

            For Each rpt As SystemReport In lst
                Dim tr As New TableRow
                'If Me.EditId = 0 Then Me.EditId = u.ID

                Dim tc As New TableCell
                tc.Attributes.Add("style", "display: inline-block;")
                tc.VerticalAlign = VerticalAlign.Top
                tc.HorizontalAlign = HorizontalAlign.Center
                tc.RowSpan = 2
                tc.Width = New Unit(iconSize + 10, UnitType.Pixel)

                Dim ibtn As New Telerik.Web.UI.RadButton
                ibtn.ID = "ibtnReport_" & rpt.ID
                ibtn.Image.ImageUrl = "~/images/icon_report.png"
                ibtn.Image.IsBackgroundImage = True
                ibtn.Height = New Unit(iconSize, UnitType.Pixel)
                ibtn.Width = New Unit(iconSize, UnitType.Pixel)
                AddHandler ibtn.Click, AddressOf ibtnReport_Click

                Dim tc1 As New TableCell
                tc1.Attributes.Add("style", "display: inline-block;")
                tc1.VerticalAlign = VerticalAlign.Top
                tc1.Text = rpt.Name

                Dim tc2 As New TableCell
                tc2.Attributes.Add("style", "display: inline-block;")
                tc2.VerticalAlign = VerticalAlign.Top
                tc2.Text = rpt.Description

                Dim lit As New Literal
                lit.ID = "litReport_" & rpt.ID
                lit.Text = "<br/>"

                Dim lbl As New Label
                lbl.ID = "lblReport_" & rpt.ID
                lbl.Text = rpt.Name

                tc.Controls.Add(lit)
                tc.Controls.Add(ibtn)

                tr.Cells.Add(tc)
                tr.Cells.Add(tc1)
                tr.Cells.Add(tc2)

                Me.tblReports.Rows.Add(tr)
            Next

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub ibtnReport_Click(sender As Object, e As EventArgs)
        Dim ibtn As Telerik.Web.UI.RadButton = CType(sender, Telerik.Web.UI.RadButton)
        Dim rptId As Integer = ibtn.ID.Split("_"c)(1).ToInteger

        Dim url As String = "~/admin/ReportEditor.aspx?t=" & CStr(Enums.TransactionType.Existing) & "&id=" & rptId
        If rptId = 4 Then url = "~/admin/DebrisTally.aspx"
        Response.Redirect(url, False)
    End Sub

    Private Sub lnkNewReport_Click(sender As Object, e As EventArgs) Handles lnkNewReport.Click
        Response.Redirect("~/admin/ReportEditor.aspx?t=" & CStr(Enums.TransactionType.New) & "&id=0", False)
    End Sub

    'Protected Sub rbReport_CheckedChanged(sender As Object, e As EventArgs)
    '    Dim rb As RadioButton = CType(sender, RadioButton)
    '    Me.EditId = rb.ID.Split("_"c)(1).ToString.ToInteger
    '    Me.SetupToolbar()
    'End Sub
End Class