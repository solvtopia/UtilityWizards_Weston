﻿Imports UtilityWizards.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class Common

    Public Shared Sub GetExtraInfo(ByVal tableName As String, ByRef q As SystemQuestion)
        Dim key As String = tableName & "|" & q.Question.Replace(" ", "_")

        Dim ht As Hashtable = App.FieldInfoLookup
        If ht.ContainsKey(key) Then
            q.FriendlyName = ht(key).ToString.Split("|"c)(0)
            q.Description = ht(key).ToString.Split("|"c)(1)
        End If
    End Sub

End Class

Class ContentTemplate
    Implements ITemplate

    Private itemText As String
    Private preText As String

    Public Sub New(ByVal text As String, ByVal content As String)
        preText = text
        itemText = content
    End Sub

    Private Sub label1_DataBinding(sender As Object, e As EventArgs)
        Dim target As Literal = DirectCast(sender, Literal)
        Dim item As RadPanelItem = DirectCast(target.BindingContainer, RadPanelItem)
        'Dim itemText As String = DirectCast(DataBinder.Eval(item, "Text"), String)
        target.Text = itemText
    End Sub

    Private Sub ITemplate_InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
        Dim tbl As New Table
        tbl.Width = New Unit(100, UnitType.Percentage)
        tbl.Style.Add("margin", "10px")
        tbl.CellPadding = 1
        tbl.CellSpacing = 2
        Dim tr As New TableRow
        Dim tc1 As New TableCell
        tc1.Text = preText
        tc1.VerticalAlign = VerticalAlign.Top
        tr.Cells.Add(tc1)
        Dim tc2 As New TableCell
        Dim lit As New Label
        lit.ID = "litContent"
        lit.Text = itemText
        tc2.Controls.Add(lit)
        tr.Cells.Add(tc2)
        tbl.Rows.Add(tr)
        container.Controls.Add(tbl)
    End Sub

End Class
