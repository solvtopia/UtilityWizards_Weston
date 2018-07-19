Imports System.Web.Services
Imports System.ComponentModel
Imports UtilityWizards.CommonCore.Common
Imports System.Xml
Imports System.Xml.Serialization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="https://westonapi.utilitywizards.com/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class InputController
    Inherits System.Web.Services.WebService

#Region "Common Routines"

    <WebMethod()>
    Public Function GetApiKey(ByVal userEmail As String, ByVal userPassword As String) As ApiKeyResult
        Dim retVal As New ApiKeyResult(userEmail, userPassword)

        If retVal.ApiKey = "" Then
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = "User not authorized to access service"
        End If

        Return retVal
    End Function

#End Region

    <WebMethod()>
    Public Function UpdateCustomerRecord(ByVal req As ApiRequest, ByVal custRecord As CustomerRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                ' fix the record data (all uppercase for standardization)
                For Each nvp As NameValuePair In custRecord.RecordData
                    If nvp.value IsNot Nothing Then nvp.value = nvp.value.ToUpper
                Next

                retVal.responseObject = custRecord.Save
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function CreateCustomerRecord(ByVal req As ApiRequest, ByVal custRecord As CustomerRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                ' make sure we have a client id for the customer and location
                If custRecord.RecordData.FindItem("ClientID").Name <> "" Then
                    Dim itm As NameValuePair = custRecord.RecordData.FindItem("ClientID")
                    itm.value = apiUsr.ClientID.ToString
                    custRecord.RecordData(custRecord.RecordData.FindItemIndex("ClientID")) = itm
                Else custRecord.RecordData.Add(New NameValuePair("ClientID", apiUsr.ClientID.ToString))
                End If

                ' fix the record data (all uppercase for standardization)
                For Each nvp As NameValuePair In custRecord.RecordData
                    If nvp.value IsNot Nothing Then nvp.value = nvp.value.ToUpper
                Next

                For Each loc As CustomerLocation In custRecord.Locations
                    If loc.RecordData.FindItem("ClientID").Name <> "" Then
                        Dim itm As NameValuePair = loc.RecordData.FindItem("ClientID")
                        itm.value = apiUsr.ClientID.ToString
                        loc.RecordData(loc.RecordData.FindItemIndex("ClientID")) = itm
                    Else loc.RecordData.Add(New NameValuePair("ClientID", apiUsr.ClientID.ToString))
                    End If

                    ' fix the record data (all uppercase for standardization)
                    For Each nvp As NameValuePair In loc.RecordData
                        If nvp.value IsNot Nothing Then nvp.value = nvp.value.ToUpper
                    Next
                Next

                Dim cr As CustomerRecord = custRecord.Save
                If cr.ID > 0 Then
                    retVal.responseObject = cr
                Else
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Save New Customer Record"
                    retVal.responseObject = Nothing
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function DeleteCustomerRecord(ByVal req As ApiRequest, ByVal custRecord As CustomerRecord) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = custRecord.Delete
                If Not retVal.responseObject.ToString.ToBoolean Then
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Delete Customer Record"
                    retVal.responseObject = False
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function UploadCustomerRecords(ByVal req As ApiRequest, ByVal fData As Byte()) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                ' save fData to file and import the records to Xml
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function UpdateUserRecord(ByVal req As ApiRequest, ByVal usr As SystemUser) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = usr.Save
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function CreateUserRecord(ByVal req As ApiRequest, ByVal usr As SystemUser) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                Dim obj As SystemUser = usr.Save
                If obj.ID > 0 Then
                    retVal.responseObject = obj
                Else
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Save New User Record"
                    retVal.responseObject = Nothing
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function DeleteUserRecord(ByVal req As ApiRequest, ByVal usr As SystemUser) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = usr.Delete
                If Not retVal.responseObject.ToString.ToBoolean Then
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Delete User Record"
                    retVal.responseObject = False
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function UpdateCustomerLocation(ByVal req As ApiRequest, ByVal loc As CustomerLocation) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = loc.Save
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function CreateCustomerLocation(ByVal req As ApiRequest, ByVal loc As CustomerLocation) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                Dim obj As CustomerLocation = loc.Save
                If obj.ID > 0 Then
                    retVal.responseObject = obj
                Else
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Save New Location Record"
                    retVal.responseObject = Nothing
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function DeleteCustomerLocation(ByVal req As ApiRequest, ByVal loc As CustomerLocation) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = loc.Delete
                If Not retVal.responseObject.ToString.ToBoolean Then
                    retVal.responseCode = Enums.ApiResultCode.failed
                    retVal.responseMessage = "Failed to Delete Location Record"
                    retVal.responseObject = False
                End If
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        End Try

        Return retVal
    End Function

End Class