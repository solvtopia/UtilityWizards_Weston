Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports UtilityWizards.CommonCore.Shared.Common
Imports System.Xml

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="https://westonapi.utilitywizards.com/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class MobileController
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

#Region "Input Routines"

    <WebMethod()>
    Public Function MarkAsViewed(ByVal req As ApiRequest) As ApiResponse
        Dim retVal As New ApiResponse

        retVal.responseObject = True

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("EXEC [procMarkAsViewed_M] @deviceId;", cn)
            cmd.Parameters.AddWithValue("@deviceId", req.deviceId)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal.responseCode = Enums.ApiResultCode.failed
            retVal.responseMessage = ex.Message
            retVal.responseObject = False
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

#Region "Output Routines"

    <WebMethod()> <XmlInclude(GetType(MaintenanceInfo))>
    Public Function CheckMaintenanceMode() As MaintenanceInfo
        Dim retVal As New MaintenanceInfo

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [Web], [Android], [iOS], [Notes] FROM [Sys_MaintenanceMode] WHERE GETDATE() BETWEEN [dtStart] AND [dtEnd];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                retVal.Android = rs("Android").ToString.ToBoolean
                retVal.iOS = rs("iOS").ToString.ToBoolean
                retVal.Web = rs("Web").ToString.ToBoolean
                retVal.Notes = rs("Notes").ToString
            End If
            rs.Close()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.API))
            retVal = New MaintenanceInfo
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    <WebMethod()>
    Public Function CheckLogin(ByVal userEmail As String, ByVal userPassword As String, ByVal deviceID As String) As Integer
        Dim retVal As Integer = 0

        Dim usr As SystemUser
        If userEmail = "" And userPassword = "" And deviceID <> "" Then
            usr = New SystemUser(deviceID)
        Else usr = New SystemUser(userEmail, userPassword, "")
        End If

        retVal = usr.ID

        Return retVal
    End Function

#End Region

End Class