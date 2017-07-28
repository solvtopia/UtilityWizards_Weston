Imports System.Web.Services
Imports System.ComponentModel
Imports UtilityWizards.CommonCore.Common
Imports System.Xml.Serialization
Imports System.Xml

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="https://api.utilitywizards.com/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class OutputController
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

    <WebMethod()> <XmlInclude(GetType(List(Of CustomerRecord)))>
    Public Function GetAllCustomers(ByVal req As ApiRequest) As ApiResponse
        Dim retVal As New ApiResponse
        Dim lst As New List(Of CustomerRecord)

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                Dim cn As New SqlClient.SqlConnection(ConnectionString)

                Try
                    Dim cmd As New SqlClient.SqlCommand("SELECT [ID] FROM [Customers_new];", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    Do While rs.Read
                        lst.Add(New CustomerRecord(rs("id").ToString.ToInteger))
                    Loop
                    cmd.Cancel()
                    rs.Close()

                Catch ex As Exception
                    ex.WriteToErrorLog
                Finally
                    cn.Close()
                End Try

                retVal.responseObject = lst
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = Nothing
        End Try

        Return retVal
    End Function

    <WebMethod()> <XmlInclude(GetType(CustomerRecord))>
    Public Function GetCustomerByAcctNum(ByVal req As ApiRequest, ByVal acctNum As String) As ApiResponse
        Dim retVal As New ApiResponse

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = New CustomerRecord(acctNum, "")
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = Nothing
        End Try

        Return retVal
    End Function

    <WebMethod()> <XmlInclude(GetType(CustomerRecord))>
    Public Function GetCustomerByLocationId(ByVal req As ApiRequest, ByVal locationId As String) As ApiResponse
        Dim retVal As New ApiResponse

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = New CustomerRecord(locationId)
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = Nothing
        End Try

        Return retVal
    End Function

    <WebMethod()> <XmlInclude(GetType(List(Of CustomerRecord)))>
    Public Function GetCustomersNotSynced(ByVal req As ApiRequest, ByVal lastSyncDate As Date) As ApiResponse
        Dim retVal As New ApiResponse
        Dim lst As New List(Of CustomerRecord)

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                ' all customers updated since the lastSyncDate

                retVal.responseObject = lst
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = Nothing
        End Try

        Return retVal
    End Function

    <WebMethod()> <XmlInclude(GetType(CustomerRecord))>
    Public Function GetLocationById(ByVal req As ApiRequest, ByVal locationId As String) As ApiResponse
        Dim retVal As New ApiResponse

        Try
            If req.apiKey <> "" Then
                Dim apiUsr As New SystemUser("", "", req.apiKey)

                retVal.responseObject = New CustomerLocation(locationId)
            Else
                retVal.responseCode = Enums.ApiResultCode.failed
                retVal.responseMessage = "No API Key Provided"
                retVal.responseObject = Nothing
            End If

        Catch ex As Exception
            ex.WriteToErrorLog
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = Nothing
        End Try

        Return retVal
    End Function

End Class