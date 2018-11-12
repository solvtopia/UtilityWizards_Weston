Imports System.IO
Imports Telerik.Web.UI
Imports UtilityWizards.CommonCore.Shared.Common

Public Class Upload
    Inherits builderPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

    Private Sub RadAsyncUpload_FileUploaded(sender As Object, e As FileUploadedEventArgs) Handles RadAsyncUpload.FileUploaded
        ' once a file is uploaded to the temp folder, move it to the uploads
        Dim uploadPath As String = Server.MapPath("~/Uploads/" & e.File.FileName)
        Dim br As New BinaryReader(e.File.InputStream)
        Dim fData As Byte() = br.ReadBytes(e.File.ContentLength.ToInteger)

        ' upload the file to aws
        UploadFileToAWS(uploadPath, fData, App.UseSandboxDb)

        br.Close()
    End Sub
End Class