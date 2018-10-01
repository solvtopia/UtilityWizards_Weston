Imports System.Xml
Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Shared.Common
Imports UtilityWizards.CommonCore.Shared.Xml
Imports Telerik.Web.UI
Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser

Public Class DebrisTally
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property ModId As Integer
        Get
            Return 109
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If App.ActiveModule.ID <> Me.ModId Then
            App.ActiveModule = New SystemModule(Me.ModId, App.UseSandboxDb)
            App.ActiveFolderID = App.ActiveModule.FolderID
        End If

        Me.LoadQuestions()

        If Not IsPostBack Then
            Me.txtUserEmail.Text = App.CurrentUser.Email
            Me.txtEmailRecipients.Text = "wadesboromanager@windstream.net"
        End If
    End Sub

    Private Sub LoadQuestions()
        If App.RootFolderQuestions.Count > 0 Then
            Dim folderName As String = GetFolderName(App.ActiveModule.FolderID)
            Me.LoadQuestions(App.RootFolderQuestions, tblFolderQuestions, folderName.Replace(" ", "_"))
        Else
            Me.tblFolderQuestions.Visible = False
        End If
        Me.LoadQuestions(App.ActiveModuleQuestions, tblModuleQuestions, "")
    End Sub
    Private Sub LoadQuestions(ByVal qList As List(Of SystemQuestion), ByVal tbl As Table, ByVal xmlPath As String)
        tbl.Rows.Clear()

        If qList.Count = 0 Then
            Dim tr As New TableRow
            Dim tc1 As New TableCell
            tc1.Text = "There are no questions To show In this view."
            tc1.VerticalAlign = VerticalAlign.Top
            tr.Cells.Add(tc1)
            tbl.Rows.Add(tr)
        Else
            For Each q In qList
                Dim tr1 As New TableRow
                Dim tc1 As New TableCell
                tc1.Text = q.Question.XmlDecode
                tc1.VerticalAlign = VerticalAlign.Top
                tr1.Cells.Add(tc1)
                tbl.Rows.Add(tr1)

                Dim tr2 As New TableRow
                Dim tc2 As New TableCell
                Select Case q.Type
                    Case Enums.SystemQuestionType.CheckBox
                        Dim chk As New Controls.CheckBoxes.CheckBox
                        chk.ID = "chk_" & q.ID
                        chk.Required = q.Required
                        chk.XmlPath = xmlPath
                        chk.DataFieldName = q.DataFieldName
                        chk.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler chk.CheckedChanged, AddressOf CheckBox_CheckedChanged
                        tc2.Controls.Add(chk)

                    Case Enums.SystemQuestionType.DropDownList
                        Dim ddl As New Controls.DropDownLists.DropDownList
                        ddl.ID = "ddl_" & q.ID
                        ddl.Width = New Unit(100, UnitType.Percentage)
                        For Each itm As String In q.DropDownValues
                            ddl.Items.Add(New RadComboBoxItem(itm, itm))
                        Next
                        ddl.Required = q.Required
                        ddl.XmlPath = xmlPath
                        ddl.DataFieldName = q.DataFieldName
                        ddl.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler ddl.SelectedIndexChanged, AddressOf DropDownList_SelectedIndexChanged
                        tc2.Controls.Add(ddl)

                    Case Enums.SystemQuestionType.MemoField
                        Dim txt As New Controls.TextBoxes.TextBox
                        txt.ID = "txt_" & q.ID
                        txt.Width = New Unit(100, UnitType.Percentage)
                        txt.Rows = 3
                        txt.TextMode = InputMode.MultiLine
                        txt.Required = q.Required
                        txt.XmlPath = xmlPath
                        txt.DataFieldName = q.DataFieldName
                        txt.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler txt.TextChanged, AddressOf TextBox_TextChanged
                        tc2.Controls.Add(txt)

                    Case Enums.SystemQuestionType.TextBox
                        Dim txt As New Controls.TextBoxes.TextBox
                        txt.ID = "txt_" & q.ID
                        txt.Width = New Unit(100, UnitType.Percentage)
                        txt.MaxLength = 255
                        txt.Required = q.Required
                        txt.XmlPath = xmlPath
                        txt.DataFieldName = q.DataFieldName
                        txt.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler txt.TextChanged, AddressOf TextBox_TextChanged
                        tc2.Controls.Add(txt)

                    Case Enums.SystemQuestionType.NumericTextBox
                        Dim txt As New Controls.TextBoxes.NumericTextBox
                        txt.ID = "txt_" & q.ID
                        txt.Width = New Unit(100, UnitType.Percentage)
                        txt.Required = q.Required
                        txt.XmlPath = xmlPath
                        txt.DataFieldName = q.DataFieldName
                        txt.NumberFormat.DecimalDigits = 0
                        txt.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler txt.TextChanged, AddressOf TextBox_TextChanged
                        tc2.Controls.Add(txt)
                End Select
                tc2.VerticalAlign = VerticalAlign.Top
                tr2.Cells.Add(tc2)

                tbl.Rows.Add(tr2)
            Next
        End If

        If Me.OnMobile Then
            Me.SetSkin(tbl, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Mobile"))
        Else Me.SetSkin(tbl, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Desktop"))
        End If
    End Sub

    Protected Sub TextBox_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub CheckBox_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub DropDownList_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)

    End Sub

    'Private Sub lnkPrint_Click(sender As Object, e As EventArgs) Handles lnkPrint.Click
    '    Dim fName As String = Me.PrintPdf("window")
    '    'Response.Redirect("~/account/PrintPreview.aspx?modid=" & Me.ModId & "&id=" & Me.RecordId, False)
    '    Me.RunClientScript("window.open('" & fName & "','_blank')")
    'End Sub

    'Private Function PrintPdf(ByVal dest As String) As String
    '    Dim retVal As String = ""

    '    Dim HTMLContent As String = ScrapeUrl(Left(Request.Url.OriginalString, Request.Url.OriginalString.Length - Request.Url.PathAndQuery.Length) & "/account/Print.aspx?modid=" & Me.ModId & "&id=" & Me.RecordId & "&usr=" & App.CurrentUser.ID, Enums.ScrapeType.KeepTags)
    '    Dim ba() As Byte = GetPDF(HTMLContent)

    '    Select Case dest
    '        Case "download"
    '            ' download the pdf
    '            Response.Clear()
    '            Response.ContentType = "application/pdf"
    '            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf")
    '            Response.Cache.SetCacheability(HttpCacheability.NoCache)
    '            Response.BinaryWrite(ba)
    '            Response.[End]()

    '        Case "window", "attachment"
    '            ' save the file
    '            Dim fName As String = "WO_" & Me.RecordId & "_" & Now.ToString.Replace("/", "").Replace(":", "").Replace("AM", "").Replace("PM", "").Replace(" ", "") & ".pdf"
    '            File.WriteAllBytes(Server.MapPath("~/temp/" & fName), ba)
    '            retVal = "../temp/" & fName
    '    End Select

    '    Return retVal
    'End Function

    Private Function GetPDF(pHTML As String) As Byte()
        Dim bPDF As Byte() = Nothing

        Dim ms As New MemoryStream()
        Dim txtReader As TextReader = New StringReader(pHTML)

        ' 1: create object of a itextsharp document class
        Dim doc As New Document(PageSize.A4, 25, 25, 25, 25)

        ' 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
        Dim oPdfWriter As PdfWriter = PdfWriter.GetInstance(doc, ms)

        ' 3: we create a worker parse the document
        Dim htmlWorker As New HTMLWorker(doc)

        ' 4: we open document and start the worker on the document
        doc.Open()
        htmlWorker.StartDocument()

        ' 5: parse the html into the document
        htmlWorker.Parse(txtReader)

        ' 6: close the document and the worker
        htmlWorker.EndDocument()
        htmlWorker.Close()
        doc.Close()

        bPDF = ms.ToArray()

        Return bPDF
    End Function

    Protected Sub btnSendMail_Click(sender As Object, e As EventArgs) Handles btnSendMail.Click
        Try
            ' build the message
            Dim msgtxt As String = ""
            If App.RootFolderQuestions.Count > 0 Then
                For Each q In App.RootFolderQuestions
                    Dim l As String = q.Question.XmlDecode & ": "
                    Select Case q.Type
                        Case Enums.SystemQuestionType.CheckBox
                            Dim chk As Controls.CheckBoxes.CheckBox = CType(Me.FindInControl("chk_" & q.ID), Controls.CheckBoxes.CheckBox)
                            l &= chk.Checked

                        Case Enums.SystemQuestionType.DropDownList
                            Dim ddl As Controls.DropDownLists.DropDownList = CType(Me.FindInControl("ddl_" & q.ID), Controls.DropDownLists.DropDownList)
                            l &= ddl.SelectedValue

                        Case Enums.SystemQuestionType.MemoField
                            Dim txt As Controls.TextBoxes.TextBox = CType(Me.FindInControl("txt_" & q.ID), Controls.TextBoxes.TextBox)
                            l &= txt.Text.Replace(vbCrLf, "<br/>")

                        Case Enums.SystemQuestionType.TextBox
                            Dim txt As Controls.TextBoxes.TextBox = CType(Me.FindInControl("txt_" & q.ID), Controls.TextBoxes.TextBox)
                            l &= txt.Text

                        Case Enums.SystemQuestionType.NumericTextBox
                            Dim txt As Controls.TextBoxes.TextBox = CType(Me.FindInControl("txt_" & q.ID), Controls.TextBoxes.TextBox)
                            l &= txt.Text

                    End Select
                    If msgtxt = "" Then msgtxt = l Else msgtxt &= "<br/>" & l
                Next
            End If

            ' module questions
            For Each q In App.ActiveModuleQuestions
                Dim l As String = q.Question.XmlDecode & ": "
                Select Case q.Type
                    Case Enums.SystemQuestionType.CheckBox
                        Dim chk As Controls.CheckBoxes.CheckBox = CType(Me.FindInControl("chk_" & q.ID), Controls.CheckBoxes.CheckBox)
                        l &= chk.Checked

                    Case Enums.SystemQuestionType.DropDownList
                        Dim ddl As Controls.DropDownLists.DropDownList = CType(Me.FindInControl("ddl_" & q.ID), Controls.DropDownLists.DropDownList)
                        l &= ddl.SelectedValue

                    Case Enums.SystemQuestionType.MemoField
                        Dim txt As Controls.TextBoxes.TextBox = CType(Me.FindInControl("txt_" & q.ID), Controls.TextBoxes.TextBox)
                        l &= txt.Text.Replace(vbCrLf, "<br/>")

                    Case Enums.SystemQuestionType.TextBox
                        Dim txt As Controls.TextBoxes.TextBox = CType(Me.FindInControl("txt_" & q.ID), Controls.TextBoxes.TextBox)
                        l &= txt.Text

                    Case Enums.SystemQuestionType.NumericTextBox
                        Dim txt As Controls.TextBoxes.TextBox = CType(Me.FindInControl("txt_" & q.ID), Controls.TextBoxes.TextBox)
                        l &= txt.Text

                End Select
                If msgtxt = "" Then msgtxt = l Else msgtxt &= "<br/>" & l
            Next

            Dim msg As New Mailer
            msg.HostServer = AppSettings("MailHost")
            msg.UserName = AppSettings("MailUser")
            msg.Password = AppSettings("MailPassword")
            For Each addr As String In Me.txtEmailRecipients.Text.Split(CChar(vbCrLf))
                If addr.Trim <> "" Then msg.To.Add(addr.Trim)
            Next
            msg.Body = "<html>" & msgtxt & "</html>"
            msg.Subject = "Debris Tally Report"
            msg.From = "noreply@utilitywizards.com"
            msg.HtmlBody = True
            msg.Send()

            Response.Redirect("~/Default.aspx", False)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        End Try
    End Sub
End Class