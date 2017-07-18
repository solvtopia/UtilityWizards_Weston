Imports System.Xml

Public Class ServiceTester
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CommonCore.Messaging.SendTwilioNotification(New SystemUser(1), Enums.NotificationType.NormalWorkOrderCreated, "something", 0)

        'Me.TestInput()
        'OneSignal.SendNotification(New SystemUser(1), "test message from utility wizards")
    End Sub

    Private Sub TestInput()
        'Dim iCli As New UWInput.InputController()
        'Dim iClo As New UWOutput.OutputController

        'Dim apiKey As UWOutput.ApiKeyResult = iClo.GetApiKey("api@southernsoftware.com", "9dR00g326d")
        'Dim apiRequest As New UWOutput.ApiRequest() With {.apiKey = apiKey.ApiKey, .clientId = apiKey.ClientID}

        'Dim resp As UWOutput.ApiResponse = iClo.GetCustomerByAcctNum(apiRequest, "001-0000010-1")

        'If resp.responseCode = UWOutput.ApiResultCode.success Then
        '    Dim itm As UWOutput.CustomerRecord = CType(resp.responseObject, UWOutput.CustomerRecord)

        '    Dim s As String = itm.AccountNumber
        'End If

        'Dim rec As New UWInput.CustomerRecord With {
        '    .AccountNumber = "100-0001000-0",
        '    .FullName = "Test Customer",
        '    .Locations = {},
        '    .RecordData = {}
        '}

        ''add the trashcan serial numbers 
        'Dim s1 As New UWInput.NameValuePair() With {.Name = "Serial1", .value = "10101"}
        'Dim s2 As New UWInput.NameValuePair() With {.Name = "Serial2", .value = "20202"}
        'rec.RecordData = {s1, s2}

        ''add the location
        'Dim loc As New UWInput.CustomerLocation With {
        '    .LocationId = "100-0001000",
        '    .City = "Southern Pines",
        '    .State = "NC",
        '    .ServiceAddress = "150 Perry Drive",
        '    .ZipCode = "28713"
        '}
        'rec.Locations = {loc}

        ''call the API
        'Dim response = iCli.CreateCustomerRecord(apiRequest, rec)

        'If response.responseCode = UWInput.ApiResultCode.success Then
        '    'lbLog.Items.Add(" Added Customer record!")
        'Else
        '    'lbLog.Items.Add("Fail to add Customer record!")
        '    'lbLog.Items.Add(response.responseMessage)
        'End If
    End Sub
    'end of testInput
End Class