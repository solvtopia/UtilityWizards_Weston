Imports Telerik.Web.UI

Public Class _Default1
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.UrlReferrer IsNot Nothing Then
            Me.lblComingFrom.Text = Request.UrlReferrer.ToString
            'Me.lblComingFromTitle.Text = Me.lblComingFrom.Text.GetUrlTitle
        End If
    End Sub
End Class