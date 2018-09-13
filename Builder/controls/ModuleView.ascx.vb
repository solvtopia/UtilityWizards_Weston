﻿Imports Telerik.Web.UI
Imports UtilityWizards.CommonCore.Shared.Common

Public Class ModuleView
    Inherits System.Web.UI.UserControl

#Region "Enums"

    Public Enum ControlMode
        View
        Edit
    End Enum

#End Region

#Region "Properties"

    Public Property CurrentModule As SystemModule
        Get
            If Session("PreviewModule") Is Nothing Then Session("PreviewModule") = New SystemModule
            Return CType(Session("PreviewModule"), SystemModule)
        End Get
        Set(value As SystemModule)
            Session("PreviewModule") = value
        End Set
    End Property

    Public Property ModuleQuestions As List(Of SystemQuestion)
        Get
            If Session("PreviewQuestions") Is Nothing Then Session("PreviewQuestions") = New List(Of SystemQuestion)
            Return CType(Session("PreviewQuestions"), List(Of SystemQuestion))
        End Get
        Set(value As List(Of SystemQuestion))
            Session("PreviewQuestions") = value
        End Set
    End Property

    Public Property Mode As ControlMode
        Get
            If Session("PreviewControlMode") Is Nothing Then Session("PreviewControlMode") = ControlMode.View
            Return CType(Session("PreviewControlMode"), ControlMode)
        End Get
        Set(value As ControlMode)
            Session("PreviewControlMode") = value
        End Set
    End Property

    Public Property TopLeftTitle As String
        Get
            Return Me.txtTopLeftTitle.Text
        End Get
        Set(value As String)
            Me.txtTopLeftTitle.Text = value
            Me.lblTopLeftTitle.Text = value
        End Set
    End Property

    Public Property TopMiddleTitle As String
        Get
            Return Me.txtTopMiddleTitle.Text
        End Get
        Set(value As String)
            Me.txtTopMiddleTitle.Text = value
            Me.lblTopMiddleTitle.Text = value
        End Set
    End Property

    Public Property TopRightTitle As String
        Get
            Return Me.txtTopRightTitle.Text
        End Get
        Set(value As String)
            Me.txtTopRightTitle.Text = value
            Me.lblTopRightTitle.Text = value
        End Set
    End Property

    Public Property FullPageTitle As String
        Get
            Return Me.txtFullPageTitle.Text
        End Get
        Set(value As String)
            Me.txtFullPageTitle.Text = value
            Me.lblFullPageTitle.Text = value
        End Set
    End Property

    Public Property BottomLeftTitle As String
        Get
            Return Me.txtBottomLeftTitle.Text
        End Get
        Set(value As String)
            Me.txtBottomLeftTitle.Text = value
            Me.lblBottomLeftTitle.Text = value
        End Set
    End Property

    Public Property BottomMiddleTitle As String
        Get
            Return Me.txtBottomMiddleTitle.Text
        End Get
        Set(value As String)
            Me.txtBottomMiddleTitle.Text = value
            Me.lblBottomMiddleTitle.Text = value
        End Set
    End Property

    Public Property BottomRightTitle As String
        Get
            Return Me.txtBottomRightTitle.Text
        End Get
        Set(value As String)
            Me.txtBottomRightTitle.Text = value
            Me.lblBottomRightTitle.Text = value
        End Set
    End Property

#End Region

#Region "Events"

    Public Event EditQuestion(ByVal q As SystemQuestion, ByVal qid As Integer)

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Me.ModuleQuestions = Nothing
            'Me.CurrentModule = Nothing
        End If
    End Sub

    Public Sub BuildQuestionList()
        Try
            Me.txtTopLeftTitle.Visible = (Me.Mode = ControlMode.Edit)
            Me.txtTopMiddleTitle.Visible = (Me.Mode = ControlMode.Edit)
            Me.txtTopRightTitle.Visible = (Me.Mode = ControlMode.Edit)
            Me.txtFullPageTitle.Visible = (Me.Mode = ControlMode.Edit)
            Me.txtBottomLeftTitle.Visible = (Me.Mode = ControlMode.Edit)
            Me.txtBottomMiddleTitle.Visible = (Me.Mode = ControlMode.Edit)
            Me.txtBottomRightTitle.Visible = (Me.Mode = ControlMode.Edit)

            ' sort the list by the sort order and id
            Me.ModuleQuestions = Me.ModuleQuestions.OrderBy(Function(q) q.Sort).ThenBy(Function(q) q.ID).ToList

            Me.tblModuleQuestions_TopLeft.Rows.Clear()
            Me.tblModuleQuestions_TopMiddle.Rows.Clear()
            Me.tblModuleQuestions_TopRight.Rows.Clear()
            Me.tblModuleQuestions_FullPage.Rows.Clear()
            Me.tblModuleQuestions_BottomLeft.Rows.Clear()
            Me.tblModuleQuestions_BottomMiddle.Rows.Clear()
            Me.tblModuleQuestions_BottomRight.Rows.Clear()

            Dim x As Integer = 0
            For Each q As SystemQuestion In Me.ModuleQuestions
                Dim tr1 As New TableRow

                Dim tc3 As New TableCell
                tc3.Wrap = False
                If Me.Mode = ControlMode.View Then
                    tc3.Text = q.Question
                ElseIf Me.Mode = ControlMode.Edit Then
                    Dim displayText As String = q.Question
                    If displayText = "" Then displayText = "(No Label)"
                    Dim lnk As New LinkButton
                    lnk.ID = "lnk_" & x
                    lnk.Text = displayText
                    tc3.Controls.Add(lnk)
                    AddHandler lnk.Click, AddressOf lnkEdit_Click
                End If
                If Not q.Visible Then tc3.Font.Strikeout = True
                If q.Type = Enums.SystemQuestionType.MemoField Then tc3.VerticalAlign = VerticalAlign.Top

                'Dim tr2 As New TableRow
                Dim tc4 As New TableCell
                tc4.ColumnSpan = 3
                Select Case q.Type
                    Case Enums.SystemQuestionType.CheckBox
                        Dim chk As New Controls.CheckBoxes.CheckBox
                        chk.ID = "chk_" & x
                        chk.DataFieldName = q.DataFieldName
                        chk.ReadOnly = (q.BindingType <> Enums.SystemQuestionBindingType.UserInput)
                        tc4.Controls.Add(chk)

                    Case Enums.SystemQuestionType.DropDownList
                        Dim ddl As New Controls.DropDownLists.DropDownList
                        ddl.ID = "ddl_" & x
                        If q.DropDownSize <> Enums.SystemQuestionDropDownSize.Auto Then
                            ddl.Width = New Unit(CStr(q.DropDownSize).ToInteger, UnitType.Pixel)
                        End If
                        For Each itm As String In q.DropDownValues
                            Dim text As String = itm.Split("="c)(0)
                            Dim value As String = itm.Split("="c)(1)
                            If value = "" Then value = text
                            ddl.Items.Add(New RadComboBoxItem(text, value))
                        Next
                        ddl.DataFieldName = q.DataFieldName
                        ddl.Enabled = (q.BindingType = Enums.SystemQuestionBindingType.UserInput)
                        tc4.Controls.Add(ddl)

                    Case Enums.SystemQuestionType.MemoField
                        Dim txt As New Controls.TextBoxes.TextBox
                        txt.ID = "txt_" & x
                        txt.Width = New Unit(250, UnitType.Pixel)
                        txt.Rows = q.Rows
                        txt.TextMode = InputMode.MultiLine
                        txt.DataFieldName = q.DataFieldName
                        txt.ReadOnly = (q.BindingType <> Enums.SystemQuestionBindingType.UserInput)
                        tc4.Controls.Add(txt)

                    Case Enums.SystemQuestionType.TextBox
                        Dim txt As New Controls.TextBoxes.TextBox
                        txt.ID = "txt_" & x
                        txt.Width = New Unit(CStr(q.TextBoxSize).ToInteger, UnitType.Pixel)
                        txt.MaxLength = 255
                        txt.DataFieldName = q.DataFieldName
                        txt.ReadOnly = (q.BindingType <> Enums.SystemQuestionBindingType.UserInput)
                        tc4.Controls.Add(txt)

                    Case Enums.SystemQuestionType.NumericTextBox
                        Dim txt As New Controls.TextBoxes.NumericTextBox
                        txt.ID = "txt_" & x
                        txt.Width = New Unit(100, UnitType.Pixel)
                        txt.NumberFormat.DecimalDigits = q.DecimalDigits
                        If Not q.ThousandsSeparator Then txt.NumberFormat.GroupSeparator = ""
                        txt.DataFieldName = q.DataFieldName
                        txt.ReadOnly = (q.BindingType <> Enums.SystemQuestionBindingType.UserInput)
                        tc4.Controls.Add(txt)

                    Case Enums.SystemQuestionType.CurrencyTextBox
                        Dim txt As New Controls.TextBoxes.NumericTextBox
                        txt.ID = "txt_" & x
                        txt.Width = New Unit(100, UnitType.Pixel)
                        txt.NumberFormat.DecimalDigits = 2
                        txt.Type = NumericType.Currency
                        txt.DataFieldName = q.DataFieldName
                        txt.ReadOnly = (q.BindingType <> Enums.SystemQuestionBindingType.UserInput)
                        tc4.Controls.Add(txt)
                End Select
                tc4.VerticalAlign = VerticalAlign.Top

                If (Me.Mode = ControlMode.View And q.Visible) Or (Me.Mode = ControlMode.Edit) Then
                    tr1.Cells.Add(tc3)
                    tr1.Cells.Add(tc4)
                End If

                'Dim tr3 As New TableRow
                'Dim tc5 As New TableCell
                'tc5.Width = New Unit(20, UnitType.Pixel)
                'Dim imgRequired As New Image
                'imgRequired.ID = "imgRequired" & x
                'imgRequired.ImageUrl = "~/images/toolbar/icon_error.png"
                'imgRequired.ToolTip = "Required Field"
                'If q.Required Then tc5.Controls.Add(imgRequired)
                'tc5.VerticalAlign = VerticalAlign.Top
                'tr1.Cells.Add(tc5)

                'Dim tc7 As New TableCell
                'tc7.Width = New Unit(20, UnitType.Pixel)
                'Dim imgReporting As New Image
                'imgReporting.ID = "imgReporting" & x
                'imgReporting.ImageUrl = "~/images/toolbar/icon_report.png"
                'imgReporting.ToolTip = "Allow Reporting"
                'If q.ReportField Then tc7.Controls.Add(imgReporting)
                'tc7.VerticalAlign = VerticalAlign.Top
                'tr1.Cells.Add(tc7)

                'Dim tc8 As New TableCell
                'tc8.Width = New Unit(20, UnitType.Pixel)
                'Dim imgExport As New Image
                'imgExport.ID = "imgExport" & x
                'imgExport.ImageUrl = "~/images/toolbar/icon_saveweb.png"
                'imgExport.ToolTip = "Can be Exported"
                'If q.ExportField Then tc8.Controls.Add(imgExport)
                'tc8.VerticalAlign = VerticalAlign.Top
                'tr1.Cells.Add(tc8)

                Dim addRow As Boolean = (tr1.Cells.Count > 0)

                Select Case True
                    Case addRow And q.Location = Enums.SystemQuestionLocation.TopLeft
                        Me.tblModuleQuestions_TopLeft.Rows.Add(tr1)
                    Case addRow And q.Location = Enums.SystemQuestionLocation.TopMiddle
                        Me.tblModuleQuestions_TopMiddle.Rows.Add(tr1)
                    Case addRow And q.Location = Enums.SystemQuestionLocation.TopRight
                        Me.tblModuleQuestions_TopRight.Rows.Add(tr1)
                    Case addRow And q.Location = Enums.SystemQuestionLocation.FullPage
                        Me.tblModuleQuestions_FullPage.Rows.Add(tr1)
                    Case addRow And q.Location = Enums.SystemQuestionLocation.BottomLeft
                        Me.tblModuleQuestions_BottomLeft.Rows.Add(tr1)
                    Case addRow And q.Location = Enums.SystemQuestionLocation.BottomMiddle
                        Me.tblModuleQuestions_BottomMiddle.Rows.Add(tr1)
                    Case addRow And q.Location = Enums.SystemQuestionLocation.BottomRight
                        Me.tblModuleQuestions_BottomRight.Rows.Add(tr1)
                End Select

                x += 1
            Next

            Me.boxTopLeft.Visible = (Me.tblModuleQuestions_TopLeft.Rows.Count > 0)
            Me.boxTopMiddle.Visible = (Me.tblModuleQuestions_TopMiddle.Rows.Count > 0)
            Me.boxTopRight.Visible = (Me.tblModuleQuestions_TopRight.Rows.Count > 0)
            Me.boxFullPage.Visible = (Me.tblModuleQuestions_FullPage.Rows.Count > 0)
            Me.boxBottomLeft.Visible = (Me.tblModuleQuestions_BottomLeft.Rows.Count > 0)
            Me.boxBottomMiddle.Visible = (Me.tblModuleQuestions_BottomMiddle.Rows.Count > 0)
            Me.boxBottomRight.Visible = (Me.tblModuleQuestions_BottomRight.Rows.Count > 0)

            If Me.Mode = ControlMode.View Then
                Me.boxTopLeftHeader.Visible = (Me.TopLeftTitle <> "")
                Me.boxTopMiddleHeader.Visible = (Me.TopMiddleTitle <> "")
                Me.boxTopRightHeader.Visible = (Me.TopRightTitle <> "")
                Me.boxFullPageHeader.Visible = (Me.FullPageTitle <> "")
                Me.boxBottomLeftHeader.Visible = (Me.BottomLeftTitle <> "")
                Me.boxBottomMiddleHeader.Visible = (Me.BottomMiddleTitle <> "")
                Me.boxBottomRightHeader.Visible = (Me.BottomRightTitle <> "")
            End If

            'Me.SetSkin(Me, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Desktop"))

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        End Try
    End Sub

    Public Sub Refresh()
        Me.BuildQuestionList()
    End Sub

    Protected Sub ibtnEdit_Click(sender As Object, e As EventArgs)
        Dim ibtn As Telerik.Web.UI.RadButton = CType(sender, Telerik.Web.UI.RadButton)
        Dim qId As Integer = ibtn.ID.Split("_"c)(1).ToInteger

        RaiseEvent EditQuestion(Me.ModuleQuestions.Item(qId), qId)
    End Sub

    Protected Sub ibtnDelete_Click(sender As Object, e As EventArgs)
        Dim ibtn As Telerik.Web.UI.RadButton = CType(sender, Telerik.Web.UI.RadButton)
        Dim qId As Integer = ibtn.ID.Split("_"c)(1).ToInteger

        Me.ModuleQuestions.RemoveAt(qId)
        Me.Refresh()
    End Sub

    Protected Sub mnuActions_ItemClick(sender As Object, e As RadMenuEventArgs)
        Dim qId As Integer = e.Item.Value.ToInteger
        Select Case e.Item.Text.ToLower
            Case "edit"
                RaiseEvent EditQuestion(Me.ModuleQuestions.Item(qId), qId)
            Case "delete"
                Me.ModuleQuestions.RemoveAt(qId)
                Me.Refresh()
        End Select
    End Sub

    Private Sub lnkEdit_Click(sender As Object, e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Dim qId As Integer = lnk.ID.Split("_"c)(1).ToInteger

        RaiseEvent EditQuestion(Me.ModuleQuestions.Item(qId), qId)
    End Sub
End Class