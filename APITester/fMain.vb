Public Class fMain
    Private Sub btnAWSUpload_Click(sender As Object, e As EventArgs) Handles btnAWSUpload.Click
        'https://westonapi.utilitywizards.com/inputcontroller.asmx
        'http://localhost:64940/InputController.asmx

        Me.OpenFileDialog.ShowDialog()

        Dim apiKey As String = "eb1JAbFE6aXKVGVTStx7DwUWV1otw+twAAXfuJ+8BaoR/paPJyfyykXIFbHdtWf3PtYiOApZBDUKNSWNFD5zAw=="

        Dim svc As New UWAPI.InputController
        Dim req As New UWAPI.ApiRequest() With {.apiKey = apiKey, .UseSandboxDb = False}

        For Each n As String In Me.OpenFileDialog.FileNames
            Dim fInfo As New IO.FileInfo(n)
            Dim fileData As Byte() = IO.File.ReadAllBytes(n)
            Dim res As UWAPI.ApiResponse = svc.UploadFile(req, fileData, fInfo.Name)

            If res.responseCode = UWAPI.ApiResultCode.failed Then
                MsgBox(res.responseMessage)
            End If
        Next
    End Sub
End Class
