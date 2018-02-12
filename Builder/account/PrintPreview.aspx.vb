Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports UtilityWizards.CommonCore.Shared.Common

Public Class PrintPreview
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property ModId As Integer
        Get
            Return Request.QueryString("modid").ToInteger
        End Get
    End Property
    Private ReadOnly Property RecordId As Integer
        Get
            Return Request.QueryString("id").ToInteger
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PrintPdf()
        End If
    End Sub

    Private Sub PrintPdf()
        Dim HTMLContent As String = ScrapeUrl(Left(Request.Url.OriginalString, Request.Url.OriginalString.Length - Request.Url.PathAndQuery.Length) & "/account/Print.aspx?modid=" & Me.ModId & "&id=" & Me.RecordId & "&usr=" & App.CurrentUser.ID, Enums.ScrapeType.KeepTags)

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf")
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.BinaryWrite(GetPDF(HTMLContent))
        Response.[End]()
    End Sub

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

End Class