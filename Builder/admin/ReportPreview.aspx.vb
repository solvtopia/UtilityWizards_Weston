Imports Telerik.Web.UI

Public Class ReportPreview
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property EditId As Integer
        Get
            Return Request.QueryString("id").ToInteger
        End Get
    End Property
    Private ReadOnly Property Report As SystemReport
        Get
            Return New SystemReport(Me.EditId, App.UseSandboxDb)
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblModule.Text = Me.Report.ModuleId.ToString
        Me.lblFields.Text = Me.LoadSearchFields
        Me.RadReportGrid.DataBind()
    End Sub

    Private Function LoadSearchFields() As String
        Dim retVal As String = ""

        For Each itm As String In Me.Report.Fields
            If retVal = "" Then
                retVal = "ISNULL(xmlData.value('(/Data/" & itm.Split("|"c)(1).Replace("~", "/") & "/text())[1]', 'varchar(255)'), '') AS [" & itm.Split("|"c)(0) & "]"
            Else retVal &= ", ISNULL(xmlData.value('(/Data/" & itm.Split("|"c)(1).Replace("~", "/") & "/text())[1]', 'varchar(255)'), '') AS [" & itm.Split("|"c)(0) & "]"
            End If
        Next

        Return retVal
    End Function
End Class