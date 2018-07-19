Imports System.Security.Cryptography
Imports System.Text
Imports System.Xml
Imports UtilityWizards.CommonCore.Shared.Enums
Imports System.Reflection
Imports UtilityWizards.CommonCore.Shared.Xml
Imports UtilityWizards.CommonCore.Shared.Extensions


Public Structure ErrorLogEntry
    Sub New(ByVal userId As Integer, ByVal clientId As Integer, ByVal project As Enums.ProjectName)
        Me.userId = userId
        Me.clientId = clientId
        Me.project = project
    End Sub
    Sub New(ByVal project As Enums.ProjectName)
        Me.userId = 0
        Me.clientId = 0
        Me.project = project
    End Sub

    Public userId As Integer
    Public clientId As Integer
    Public project As Enums.ProjectName
End Structure


Public Class SystemUser
    Public Sub New()
        Me.ID = 0
        Me.MobileDeviceIds = New List(Of String)
        Me.ApprovedModules = New List(Of Integer)
    End Sub
    Public Sub New(ByVal userEmail As String, ByVal userPassword As String, ByVal apiKey As String)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cl As New SystemClient
            Dim cmd As New SqlClient.SqlCommand()
            Dim rs As SqlClient.SqlDataReader

            Dim mobileUserFound As Boolean = False

            If apiKey = "" Then
                ' check login based on xMobileUsername and xPassword first
                cmd = New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData] FROM [vwUserInfo] WHERE [xMobileUsername] LIKE @Email AND [xPassword] LIKE @Password AND [xActive] = 1;", cn)
                cmd.Parameters.AddWithValue("@Email", userEmail)
                cmd.Parameters.AddWithValue("@Password", userPassword)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                If rs.Read Then
                    Me.Initialize(rs("UserXmlData").ToString)
                    Me.ID = rs("id").ToString.ToInteger
                    cl.Initialize(rs("ClientXmlData").ToString)

                    mobileUserFound = True
                End If
                rs.Close()
                cmd.Cancel()

                If Not mobileUserFound Then
                    ' no mobile user found so check login based on xEmail and xPassword
                    cmd = New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData] FROM [vwUserInfo] WHERE [xEmail] LIKE @Email AND [xPassword] LIKE @Password AND [xActive] = 1;", cn)
                    cmd.Parameters.AddWithValue("@Email", userEmail)
                    cmd.Parameters.AddWithValue("@Password", userPassword)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    rs = cmd.ExecuteReader
                    If rs.Read Then
                        Me.Initialize(rs("UserXmlData").ToString)
                        Me.ID = rs("id").ToString.ToInteger
                        cl.Initialize(rs("ClientXmlData").ToString)
                    End If
                    rs.Close()
                    cmd.Cancel()
                End If
            Else
                ' we have an apiKey so lookup the user based on that
                cmd = New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData], [xEmail], [xPassword] FROM [vwUserInfo] WHERE [xApiEnabled] = 1 AND [xActive] = 1;", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                Do While rs.Read
                    Dim convertedToBytes As Byte() = Encoding.UTF8.GetBytes(rs("xPassword").ToString & rs("xEmail").ToString)
                    Dim hashType As HashAlgorithm = New SHA512Managed()
                    Dim hashBytes As Byte() = hashType.ComputeHash(convertedToBytes)
                    Dim hashedResult As String = Convert.ToBase64String(hashBytes)

                    If apiKey = hashedResult Then
                        Me.Initialize(rs("UserXmlData").ToString)
                        Me.ID = rs("id").ToString.ToInteger
                        cl.Initialize(rs("ClientXmlData").ToString)
                        Exit Do
                    End If
                Loop
                rs.Close()
                cmd.Cancel()
            End If

            If Me.ID > 0 Then
                If cl.Approved = SystemMode.Demo And DateDiff(DateInterval.Day, cl.DemoStartDate, Now.Date) > cl.DemoDuration Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your system demo period has expired."
                ElseIf Not cl.Active Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your client profile is currently disabled."
                Else
                    Me.APIResponseCode = Enums.ApiResultCode.success
                    Me.APIResponseMessage = ""
                End If
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Public Sub New(ByVal deviceId As String)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cl As New SystemClient

            ' pull the last user that logged in with this device id
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData] FROM [vwUserInfo] WHERE [UserXmlData].exist('/SystemUser/MobileDeviceIds [contains(.,""" & deviceId & """)]') = 1 AND [ID] > 1 ORDER BY [dtUpdated] DESC;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("UserXmlData").ToString)

                ' save the device id to the list
                If Not Me.MobileDeviceIds.Contains(deviceId) Then Me.MobileDeviceIds.Add(deviceId)

                'save any changes
                Me.Save()

                Me.ID = rs("id").ToString.ToInteger
                cl.Initialize(rs("ClientXmlData").ToString)
            End If
            rs.Close()
            cmd.Cancel()

            If Me.ID > 0 Then
                If cl.Approved = SystemMode.Demo And DateDiff(DateInterval.Day, cl.DemoStartDate, Now.Date) > cl.DemoDuration Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your system demo period has expired."
                ElseIf Not cl.Active Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your client profile is currently disabled."
                Else
                    Me.APIResponseCode = Enums.ApiResultCode.success
                    Me.APIResponseMessage = ""
                End If
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Public Sub New(ByVal userId As Integer)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cl As New SystemClient

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [UserXmlData], [ClientXmlData] FROM [vwUserInfo] WHERE [ID] = " & userId & " AND [xActive] = 1;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("UserXmlData").ToString)
                Me.ID = rs("id").ToString.ToInteger
                cl.Initialize(rs("ClientXmlData").ToString)
            End If
            rs.Close()
            cmd.Cancel()

            If Me.ID > 0 Then
                If cl.Approved = SystemMode.Demo And DateDiff(DateInterval.Day, cl.DemoStartDate, Now.Date) > cl.DemoDuration Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your system demo period has expired."
                ElseIf Not cl.Active Then
                    Me.APIResponseCode = Enums.ApiResultCode.failed
                    Me.APIResponseMessage = "Your client profile is currently disabled."
                Else
                    Me.APIResponseCode = Enums.ApiResultCode.success
                    Me.APIResponseMessage = ""
                End If
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public ID As Integer
    Public Name As String
    Public Email As String
    Public Password As String
    Public Permissions As SystemUserPermissions
    Public Property ImageUrl As String
        Get
            Dim retVal As String = ""

            Select Case Me.Permissions
                Case Enums.SystemUserPermissions.Administrator, Enums.SystemUserPermissions.SystemAdministrator
                    retVal = "~/images/icon_administrator_avatar.png"
                Case Enums.SystemUserPermissions.Technician
                    retVal = "~/images/icon_technician_avatar.png"
                Case Enums.SystemUserPermissions.Solvtopia
                    retVal = "~/images/icon.png"
                Case Enums.SystemUserPermissions.Supervisor
                    retVal = "~/images/icon_supervisor_avatar.png"
                Case Else
                    retVal = "~/images/icon_user_avatar.png"
            End Select

            Return retVal
        End Get
        Set(value As String)
            ' Property is ReadOnly. Set is for API Compatibility Only
        End Set
    End Property
    Public ReadOnly Property Hash As String
        Get
            Dim retVal As String = ""

            Dim convertedToBytes As Byte() = Encoding.UTF8.GetBytes(Me.Password & Me.Email)
            Dim hashType As HashAlgorithm = New SHA512Managed()
            Dim hashBytes As Byte() = hashType.ComputeHash(convertedToBytes)
            retVal = Convert.ToBase64String(hashBytes)

            Return retVal
        End Get
    End Property
    Public Property MobileImageUrl As String
        Get
            Dim retVal As String = Me.ImageUrl

            retVal = retVal.Replace("~/", "https://weston.utilitywizards.com/")
            'retVal = retVal.Replace("avatar.png", "mobile_avatar.png")

            Return retVal
        End Get
        Set(value As String)
            ' Property is ReadOnly. Set is for API Compatibility Only
        End Set
    End Property
    Public MobileDeviceId As String
    Public MobileDeviceIds As List(Of String)
    Public MobileUsername As String
    Public MobileNumber As String
    'Public MobilePassword As String
    Public Active As Boolean
    Public ClientID As Integer
    Public ApiEnabled As Boolean
    Public WebEnabled As Boolean
    Public Property IsAdminUser As Boolean
        Get
            Return (Me.Permissions = SystemUserPermissions.Administrator Or
                    Me.Permissions = SystemUserPermissions.SystemAdministrator Or
                    Me.Permissions = SystemUserPermissions.Solvtopia)
        End Get
        Set(value As Boolean)
            ' Property is ReadOnly. Set is for API Compatibility Only
        End Set
    End Property
    Public Property IsSysAdmin As Boolean
        Get
            Return (Me.Permissions = SystemUserPermissions.SystemAdministrator Or
                    Me.Permissions = SystemUserPermissions.Solvtopia)
        End Get
        Set(value As Boolean)
            ' Property is ReadOnly. Set is for API Compatibility Only
        End Set
    End Property
    Public SupervisorID As Integer
    Public MobileDeviceType As Enums.UserPlatform
    Public OneSignalUserID As String
    Public OneSignalPushToken As String
    Public ApprovedModules As List(Of Integer)

    Public APIResponseCode As ApiResultCode
    Public APIResponseMessage As String

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New SystemUser
            source = CType(source.DeserializeFromXml(xmlData), SystemUser)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As SystemUser
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As SystemUser
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As New SystemUser

        Try
            If Me.ID = 0 Then
                Me.Active = True

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Users] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '" & Me.ID & "', '" & Me.ID & "', '1');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim cmd As New SqlClient.SqlCommand("UPDATE [Users] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else : cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New SystemUser
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As Boolean = True

        Try
            Me.Active = False

            Dim cmd As New SqlClient.SqlCommand("UPDATE [Users] SET [xmlData] = @xmlData, [Active] = 0, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
            cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ID, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Class SystemClient
    Public Sub New()
        Me.ID = 0
        Me.IconSize = IconSize.Small
    End Sub
    Public Sub New(ByVal clientId As Integer)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Clients] WHERE [ID] = " & clientId, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("xmlData").ToString)
                Me.ID = rs("id").ToString.ToInteger
            End If
            rs.Close()
            cmd.Cancel()

            If Me.Approved = SystemMode.Demo And DateDiff(DateInterval.Day, Me.DemoStartDate, Now.Date) > Me.DemoDuration Then
                Me.APIResponseCode = Enums.ApiResultCode.failed
                Me.APIResponseMessage = "Your system demo period has expired."
            ElseIf Not Me.Active Then
                Me.APIResponseCode = Enums.ApiResultCode.failed
                Me.APIResponseMessage = "Your client profile is currently disabled."
            Else
                Me.APIResponseCode = Enums.ApiResultCode.success
                Me.APIResponseMessage = ""
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public ID As Integer
    Public Name As String
    Public Address1 As String
    Public Address2 As String
    Public City As String
    Public State As String
    Public ZipCode As String
    Public ContactName As String
    Public ContactPhone As String
    Public ContactEmail As String
    Public Property Modules As List(Of SystemModule)
        Get
            If Me.ID = 0 Then Return New List(Of SystemModule) Else Return Me.LoadModules()
        End Get
        Set(value As List(Of SystemModule))
            ' Property is ReadOnly. Set is for API Compatibility Only
        End Set
    End Property
    Public Active As Boolean
    Public Approved As Enums.SystemMode
    Public IconSize As Enums.IconSize

    Public DemoStartDate As Date
    Public DemoDuration As Integer

    Public ProjectUrl As String

    Public APIResponseCode As ApiResultCode
    Public APIResponseMessage As String

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New SystemClient
            source = CType(source.DeserializeFromXml(xmlData), SystemClient)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As SystemClient
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As SystemClient
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As New SystemClient

        Try
            If Me.ID = 0 Then
                Me.Active = True

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Clients] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '0', '0');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim cmd As New SqlClient.SqlCommand("UPDATE [Clients] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = 0 WHERE [ID] = " & Me.ID, cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New SystemClient
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As Boolean = True

        Try
            Me.Active = False

            Dim cmd As New SqlClient.SqlCommand("UPDATE [Clients] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = 0 WHERE [ID] = " & Me.ID, cn)
            cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Private Function LoadModules() As List(Of SystemModule)
        Dim retVal As New List(Of SystemModule)

        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [xModules] FROM [Clients] WHERE ID = " & Me.ID & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                retVal = LoadModules(rs("xModules").ToString)
            End If
            rs.Close()
            cmd.Cancel()

            retVal.Sort(Function(p1, p2) p1.Type.CompareTo(p2.Type))

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function
    Private Function LoadModules(ByVal xModules As String) As List(Of SystemModule)
        Dim retVal As New List(Of SystemModule)

        Dim xMods As New XmlDocument
        xMods.LoadXml("<Data>" & xModules & "</Data>")

        For Each xMod As XmlNode In xMods.GetElementsByTagName("SystemModule")
            Dim m As New SystemModule("<SystemModule>" & xMod.InnerXml & "</SystemModule>")
            retVal.Add(m)
        Next

        Return retVal
    End Function

#End Region

End Class

Public Class SystemModule
    Public Sub New()
        Me.ID = 0
    End Sub
    Public Sub New(ByVal id As Integer)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Modules] WHERE [ID] = " & id & " AND [Active] = 1", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("xmlData").ToString)
                Me.ID = rs("id").ToString.ToInteger
                Me.LoadSpecial()
            End If
            rs.Close()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Public Sub New(ByVal xData As String)
        Me.Initialize(xData)
        Me.LoadSpecial()
    End Sub

    Public ID As Integer
    Public ClientID As Integer
    Public FolderID As Integer
    Public Name As String
    Public Description As String
    Public Icon As String
    Public Type As SystemModuleType
    Public SupervisorID As Integer

    Public APIResponseCode As ApiResultCode
    Public APIResponseMessage As String

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New SystemModule
            source = CType(source.DeserializeFromXml(xmlData), SystemModule)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As SystemModule
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As SystemModule
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As New SystemModule

        Try
            If Me.ID = 0 Then
                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Modules] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active], [Hidden]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '0', '0', '1', '0');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim cmd As New SqlClient.SqlCommand("UPDATE [Modules] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = 0 WHERE [ID] = " & Me.ID, cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New SystemModule
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As Boolean = True

        Try
            Dim cmd As New SqlClient.SqlCommand("UPDATE [Modules] SET [Active] = 0 WHERE [ID] = " & Me.ID, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

            cmd = New SqlClient.SqlCommand("EXEC [procMoveModule] 0, " & Me.ID & ", 0, 1;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Hide() As Boolean
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As Boolean = True

        Try
            Dim cmd As New SqlClient.SqlCommand("UPDATE [Modules] SET [Hidden] = 1 WHERE [ID] = " & Me.ID, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Move(ByVal newFolderId As Integer) As Boolean
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As Boolean = True

        Try
            Dim cmd As New SqlClient.SqlCommand("EXEC [procMoveModule] " & Me.ID & ", 0, " & newFolderId & ", 0;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(0, Me.ClientID, Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Sub LoadSpecial()
        ' special cases for properties
        Dim iconFolder As String = "Modules"
        'If App.CurrentClient.IconSize = IconSize.Large Then iconFolder = "modules_large"

        If Me.Type = SystemModuleType.Folder Then
            Me.Icon = "~/images/gallery/" & iconFolder & "/folder.png"
        Else
            If Me.Icon = "" Then
                If Me.Name.ToLower.Contains("water") Or Me.Description.ToLower.Contains("water") Then
                    Me.Icon = "~/images/gallery/" & iconFolder & "/water.png"
                ElseIf Me.Name.ToLower.Contains("sewer") Or Me.Description.ToLower.Contains("sewer") Then
                    Me.Icon = "~/images/gallery/" & iconFolder & "/sewer.png"
                ElseIf Me.Name.ToLower.Contains("sanitation") Or Me.Description.ToLower.Contains("sanitation") Or Me.Name.ToLower.Contains("trash") Or Me.Description.ToLower.Contains("trash") Then
                    Me.Icon = "~/images/gallery/" & iconFolder & "/sanitation.png"
                ElseIf Me.Name.ToLower.Contains("811") Then
                    Me.Icon = "~/images/gallery/" & iconFolder & "/811.png"
                Else Me.Icon = "~/images/gallery/" & iconFolder & "/module.png"
                End If
            End If
        End If
    End Sub

#End Region

End Class

Public Class SystemQuestion
    Sub New()
        Me.Type = SystemQuestionType.TextBox
        Me.Values = New List(Of String)
    End Sub
    Public Sub New(ByVal id As Integer)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Questions] WHERE [ID] = " & id & " AND [Active] = 1", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("xmlData").ToString)
                Me.ID = rs("id").ToString.ToInteger
            End If
            rs.Close()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Public Sub New(ByVal xData As String)
        Me.Initialize(xData)
    End Sub

    Public ID As Integer
    Public ModuleID As Integer
    Public Question As String
    Public Property DataFieldName As String
        Get
            Dim retVal As String = StrConv(Me.Question, VbStrConv.ProperCase).Replace(" ", "")

            If Me.DataFieldNameOverride <> "" Then retVal = Me.DataFieldNameOverride

            retVal = retVal.Replace("?", "")
            retVal = retVal.Replace("/", "")
            retVal = retVal.Replace("'", "")
            retVal = retVal.Replace("&", "And")
            retVal = retVal.Replace("#", "Number")
            retVal = retVal.Replace("%", "Percent")
            retVal = retVal.Replace("'", "")

            Return retVal
        End Get
        Set(value As String)
            Me.DataFieldNameOverride = value
        End Set
    End Property
    Public DataFieldNameOverride As String
    Public Rule As String
    Public Type As SystemQuestionType
    Public Required As Boolean
    Public Values As List(Of String)
    Public SearchField As Boolean
    Public ReportField As Boolean
    Public ExportField As Boolean
    Public MobileField As Boolean
    Public MobileData As String
    Public NewMobileData As String
    Public Locked As Boolean
    Public Visible As Boolean

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New SystemQuestion
            source = CType(source.DeserializeFromXml(xmlData), SystemQuestion)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As SystemQuestion
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As SystemQuestion
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As New SystemQuestion

        Try
            If Me.ID = 0 Then
                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Questions] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '0', '0', '1');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim cmd As New SqlClient.SqlCommand("UPDATE [Questions] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = 0 WHERE [ID] = " & Me.ID, cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New SystemQuestion
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As Boolean = True

        Try
            Dim cmd As New SqlClient.SqlCommand("UPDATE [Questions] SET [xmlData] = @xmlData, [Active] = 0, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = 0 WHERE [ID] = " & Me.ID, cn)
            cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Class SystemReport
    Public Sub New()
        Me.ID = 0
        Me.Fields = New List(Of String)
    End Sub
    Public Sub New(ByVal reportId As Integer)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cl As New SystemClient

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Reports] WHERE [ID] = " & reportId & " AND [Active] = 1;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.Initialize(rs("xmlData").ToString)
                Me.ID = rs("id").ToString.ToInteger
            End If
            rs.Close()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public ID As Integer
    Public Name As String
    Public Description As String
    Public Active As Boolean
    Public ClientID As Integer
    Public Fields As List(Of String)
    Public ModuleId As Integer
    Public Url As String

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Dim typeB As Type = Me.[GetType]()

            Dim source As New SystemReport
            source = CType(source.DeserializeFromXml(xmlData), SystemReport)

            ' handle the properties
            For Each [property] As PropertyInfo In source.[GetType]().GetProperties()
                If Not [property].CanRead OrElse ([property].GetIndexParameters().Length > 0) Then
                    Continue For
                End If

                Dim other As PropertyInfo = typeB.GetProperty([property].Name)
                If (other IsNot Nothing) AndAlso (other.CanWrite) Then
                    other.SetValue(Me, [property].GetValue(source, Nothing), Nothing)
                End If
            Next

            ' handle the fields
            For Each [field] As FieldInfo In source.[GetType]().GetFields()
                Dim other As FieldInfo = typeB.GetField([field].Name)
                If (other IsNot Nothing) Then
                    other.SetValue(Me, [field].GetValue(source))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As SystemReport
        Return Me.Save("")
    End Function
    Public Function Save(ByVal xmlData As String) As SystemReport
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As New SystemReport

        Try
            If Me.ID = 0 Then
                Me.Active = True

                Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Reports] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "', '" & Me.ID & "', '" & Me.ID & "', '1');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                cmd.Parameters.AddWithValue("@xmlData", xmlData)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim cmd As New SqlClient.SqlCommand("UPDATE [Reports] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
                If xmlData = "" Then
                    cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
                Else : cmd.Parameters.AddWithValue("@xmlData", xmlData)
                End If
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New SystemReport
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As Boolean = True

        Try
            Me.Active = False

            Dim cmd As New SqlClient.SqlCommand("UPDATE [Reports] SET [xmlData] = @xmlData, [Active] = 0, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = " & Me.ID & " WHERE [ID] = " & Me.ID, cn)
            cmd.Parameters.AddWithValue("@xmlData", Me.SerializeToXml)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Class ApiKeyResult
    Sub New()
    End Sub
    Sub New(ByVal userEmail As String, ByVal userPassword As String)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Me.UserEmail = userEmail
            Me.UserPassword = userPassword

            Dim convertedToBytes As Byte() = Encoding.UTF8.GetBytes(userPassword & userEmail)
            Dim hashType As HashAlgorithm = New SHA512Managed()
            Dim hashBytes As Byte() = hashType.ComputeHash(convertedToBytes)
            Dim hashedResult As String = Convert.ToBase64String(hashBytes)

            Me.ApiKey = hashedResult

            Dim cmd As New SqlClient.SqlCommand("SELECT [xClientID] FROM [Users] WHERE [xEmail] LIKE @Email AND [xPassword] LIKE @Password;", cn)
            cmd.Parameters.AddWithValue("@Email", userEmail)
            cmd.Parameters.AddWithValue("@Password", userPassword)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.ClientID = rs("xClientID").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public UserEmail As String
    Public UserPassword As String
    Public ApiKey As String
    Public ClientID As Integer

    Public responseCode As Enums.ApiResultCode
    Public responseMessage As String
End Class

Public Class ApiResponse
    Sub New()
        Me.responseCode = ApiResultCode.success
        Me.responseMessage = ""
    End Sub

    Public responseCode As ApiResultCode
    Public responseMessage As String
    Public responseObject As Object
End Class

Public Class ApiRequest
    Sub New()
    End Sub
    Sub New(ByVal apiKey As String, ByVal clientID As Integer, ByVal deviceId As String, ByVal deviceType As UserPlatform)
        Me.apiKey = apiKey
        Me.deviceId = deviceId
        Me.deviceType = deviceType
    End Sub

    Public apiKey As String
    Public clientId As Integer
    Public deviceId As String
    Public deviceType As UserPlatform
End Class

Public Class CustomerRecord
    Sub New()
        Me.ID = 0
        Me.RecordData = New List(Of NameValuePair)
        'Me.RecordData.Add(New NameValuePair("ClientID", App.CurrentClient.ID.ToString))
    End Sub
    Sub New(ByVal req As ApiRequest)
        Me.ID = 0
        Me.RecordData = New List(Of NameValuePair)
        Me.RecordData.Add(New NameValuePair("ClientID", req.clientId.ToString))
    End Sub
    Sub New(ByVal id As Integer)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [ClientID], [AccountNum] AS [Account], [FullName] FROM [Customers_new] WHERE [ID] = " & id & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                ' build xml to populate the object
                Dim xDoc As XmlDocument = NewXmlDocument()
                For x As Integer = 0 To rs.FieldCount - 1
                    Dim n As String = rs.GetName(x)
                    Dim v As String = rs.GetString(x)
                    xDoc.NewElement(n, v)
                Next
                Me.RecordData = New List(Of NameValuePair)
                Me.Initialize(xDoc.ToXmlString)
                Me.ID = rs("id").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

            Me.Locations = New List(Of CustomerLocation)
            cmd = New SqlClient.SqlCommand("SELECT [ID] FROM [Locations_new] WHERE [AccountNum] LIKE '" & Me.AccountNumber & "'", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.Locations.Add(New CustomerLocation(rs("id").ToString.ToInteger))
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Sub New(ByVal locationId As String)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("Select [CustomerID] AS [ID], [ClientID], [Account], [FullName] FROM [vwCustomerSearch_new] WHERE [LocationNum] LIKE '" & locationId & "';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                ' build xml to populate the object
                Dim xDoc As XmlDocument = NewXmlDocument()
                For x As Integer = 0 To rs.FieldCount - 1
                    Dim n As String = rs.GetName(x)
                    Dim v As String = rs.GetString(x)
                    xDoc.NewElement(n, v)
                Next
                Me.RecordData = New List(Of NameValuePair)
                Me.Initialize(xDoc.ToXmlString)
                Me.ID = rs("id").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

            Me.Locations = New List(Of CustomerLocation)
            cmd = New SqlClient.SqlCommand("SELECT [ID] FROM [Locations_new] WHERE [AccountNum] LIKE '" & Me.AccountNumber & "'", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.Locations.Add(New CustomerLocation(rs("id").ToString.ToInteger))
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Sub New(ByVal custAcctNum As String, ByVal serviceAddress As String)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader

            If custAcctNum <> "" And serviceAddress = "" Then
                cmd = New SqlClient.SqlCommand("SELECT [ID], [ClientID], [AccountNum] AS [Account], [FullName] FROM [Customers_new] WHERE [AccountNum] LIKE '" & custAcctNum & "';", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                If rs.Read Then
                    ' build xml to populate the object
                    Dim xDoc As XmlDocument = NewXmlDocument()
                    For x As Integer = 0 To rs.FieldCount - 1
                        Dim n As String = rs.GetName(x)
                        Dim v As String = rs.GetString(x)
                        xDoc.NewElement(n, v)
                    Next
                    Me.RecordData = New List(Of NameValuePair)
                    Me.Initialize(xDoc.ToXmlString)
                    Me.ID = rs("id").ToString.ToInteger
                End If
                cmd.Cancel()
                rs.Close()
            ElseIf custAcctNum = "" And serviceAddress <> "" Then
                Dim sWhere As String = ""
                For Each s As String In serviceAddress.Split(" "c)
                    If sWhere = "" Then sWhere = "[SearchAddress] LIKE '%" & s & "%'" Else sWhere &= " AND [SearchAddress] LIKE '%" & s & "%'"
                Next

                cmd = New SqlClient.SqlCommand("SELECT [CustomerID] AS [ID], [ClientID], [Account], [FullName] FROM [vwCustomerSearch_new] WHERE " & sWhere & ";", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                If rs.Read Then
                    ' build xml to populate the object
                    Dim xDoc As XmlDocument = NewXmlDocument()
                    For x As Integer = 0 To rs.FieldCount - 1
                        Dim n As String = rs.GetName(x)
                        Dim v As String = rs.GetString(x)
                        xDoc.NewElement(n, v)
                    Next
                    Me.RecordData = New List(Of NameValuePair)
                    Me.Initialize(xDoc.ToXmlString)
                    Me.ID = rs("id").ToString.ToInteger
                End If
                cmd.Cancel()
                rs.Close()
            End If

            Me.Locations = New List(Of CustomerLocation)
            cmd = New SqlClient.SqlCommand("SELECT [ID] FROM [Locations_new] WHERE [AccountNum] LIKE '" & Me.AccountNumber & "'", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.Locations.Add(New CustomerLocation(rs("id").ToString.ToInteger))
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public ID As Integer
    Public Property Locations As List(Of CustomerLocation)
    Public Property AccountNumber As String
        Get
            Dim retVal As String = ""
            If Me.RecordData Is Nothing Then Me.RecordData = New List(Of NameValuePair)
            For Each v As NameValuePair In Me.RecordData
                If v.Name IsNot Nothing AndAlso v.Name.ToLower = "account" Then
                    retVal = v.value
                    Exit For
                End If
            Next
            Return retVal
        End Get
        Set(value As String)
            Dim i As Integer = Me.RecordData.FindItemIndex("account")
            Dim nvp As New NameValuePair("Account", value)

            If i = -1 Then
                Me.RecordData.Add(nvp)
            Else Me.RecordData(i) = nvp
            End If
        End Set
    End Property
    Public Property FullName As String
        Get
            Dim retVal As String = ""
            If Me.RecordData Is Nothing Then Me.RecordData = New List(Of NameValuePair)
            For Each v As NameValuePair In Me.RecordData
                If v.Name IsNot Nothing AndAlso v.Name.ToLower = "fullname" Then
                    retVal = v.value
                    Exit For
                End If
            Next
            Return retVal
        End Get
        Set(value As String)
            Dim i As Integer = Me.RecordData.FindItemIndex("fullname")
            Dim nvp As New NameValuePair("FullName", value)

            If i = -1 Then
                Me.RecordData.Add(nvp)
            Else Me.RecordData(i) = nvp
            End If
        End Set
    End Property
    Public RecordData As List(Of NameValuePair)

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Me.RecordData = New List(Of NameValuePair)

            Dim xDoc As New XmlDocument
            xDoc.LoadXml(xmlData)

            For Each xElem As XmlElement In xDoc.Item("Data").ChildNodes
                Dim n As String = xElem.Name
                Dim v As String = xElem.InnerText.XmlDecode

                If n.ToLower <> "locations" Then
                    Me.RecordData.Add(New NameValuePair(n, v))
                End If
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As CustomerRecord
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As New CustomerRecord

        Try
            ' save the locations first
            Dim newLocs As New List(Of CustomerLocation)
            For Each loc As CustomerLocation In Me.Locations
                newLocs.Add(loc.Save)
            Next
            Me.Locations = newLocs

            Dim addNew As Boolean = True
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID] FROM [Customers_new] WHERE [AccountNum] LIKE '" & Me.AccountNumber & "' OR [ID] = " & Me.ID & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                addNew = False
            End If

            ' now save the client record
            If addNew Then
                Dim fields As String = ""
                Dim values As String = ""
                For Each itm In Me.RecordData
                    If fields = "" Then fields = "[" & itm.Name & "]" Else fields &= ", [" & itm.Name & "]"
                    If values = "" Then values = "'" & itm.value & "'" Else values &= ", '" & itm.value & "'"
                Next

                fields &= "[dtInserted], [dtUpdated], [Active]"
                values &= "'" & Now.ToString & "', '" & Now.ToString & "', '1'"

                cmd = New SqlClient.SqlCommand("INSERT INTO [Customers_new] (" & fields & ") VALUES (" & values & ");SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim fields As String = ""
                For Each itm In Me.RecordData
                    If fields = "" Then fields = "[" & itm.Name & "] = '" & itm.value & "'" Else fields &= ", [" & itm.Name & "] = '" & itm.value & "'"
                Next

                fields &= "[dtUpdated] = '" & Now.ToString & "'"

                Dim sWhere As String = ""
                cmd = New SqlClient.SqlCommand("UPDATE [Customers_new] SET " & fields & " WHERE [AccountNum] LIKE '" & Me.AccountNumber & "' OR [ID] = " & Me.ID, cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New CustomerRecord
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As Boolean = True

        Try
            Dim cmd As New SqlClient.SqlCommand("UPDATE [Customers_new] SET [Active] = 0, [dtUpdated] = '" & Now.ToString & "' WHERE [ID] = " & Me.ID, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Class CustomerLocation
    Sub New()
        Me.ID = 0
        Me.RecordData = New List(Of NameValuePair)
        'Me.RecordData.Add(New NameValuePair("ClientID", App.CurrentClient.ID.ToString))
    End Sub
    Sub New(ByVal id As Integer)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("SELECT * FROM [Locations_new] WHERE [ID] = " & id & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                ' build xml to populate the object
                Dim xDoc As XmlDocument = NewXmlDocument()
                For x As Integer = 0 To rs.FieldCount - 1
                    Dim n As String = rs.GetName(x)
                    Dim v As String = rs.GetString(x)
                    xDoc.NewElement(n, v)
                Next
                Me.RecordData = New List(Of NameValuePair)
                Me.Initialize(xDoc.ToXmlString)
                Me.ID = rs("id").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub
    Sub New(ByVal locationId As String)
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("Select * FROM [Locations_new] WHERE [LocationNum] LIKE '" & locationId & "';", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                ' build xml to populate the object
                Dim xDoc As XmlDocument = NewXmlDocument()
                For x As Integer = 0 To rs.FieldCount - 1
                    Dim n As String = rs.GetName(x)
                    Dim v As String = rs.GetString(x)
                    xDoc.NewElement(n, v)
                Next
                Me.RecordData = New List(Of NameValuePair)
                Me.Initialize(xDoc.ToXmlString)
                Me.ID = rs("id").ToString.ToInteger
            End If
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try
    End Sub

    Public ID As Integer
    Public Property LocationId As String
        Get
            Dim retVal As String = "000-0000000"
            If Me.RecordData Is Nothing Then Me.RecordData = New List(Of NameValuePair)
            For Each v As NameValuePair In Me.RecordData
                If v.Name IsNot Nothing AndAlso v.Name.ToLower = "locationnum" Then
                    retVal = v.value
                    Exit For
                End If
            Next
            Return retVal
        End Get
        Set(value As String)
            Dim i As Integer = Me.RecordData.FindItemIndex("locationnum")
            Dim nvp As New NameValuePair("LocationNum", value)

            If i = -1 Then
                Me.RecordData.Add(nvp)
            Else Me.RecordData(i) = nvp
            End If
        End Set
    End Property
    Public Property ServiceAddress As String
        Get
            Dim retVal As String = ""
            If Me.RecordData Is Nothing Then Me.RecordData = New List(Of NameValuePair)
            For Each v As NameValuePair In Me.RecordData
                If v.Name IsNot Nothing AndAlso v.Name.ToLower = "serviceaddr" Then
                    retVal = v.value
                    Exit For
                End If
            Next
            Return retVal
        End Get
        Set(value As String)
            Dim i As Integer = Me.RecordData.FindItemIndex("serviceaddr")
            Dim nvp As New NameValuePair("ServiceAddr", value)

            If i = -1 Then
                Me.RecordData.Add(nvp)
            Else Me.RecordData(i) = nvp
            End If
        End Set
    End Property
    Public Property City As String
        Get
            Dim retVal As String = ""
            If Me.RecordData Is Nothing Then Me.RecordData = New List(Of NameValuePair)
            For Each v As NameValuePair In Me.RecordData
                If v.Name IsNot Nothing AndAlso v.Name.ToLower = "city" Then
                    retVal = v.value
                    Exit For
                End If
            Next
            Return retVal
        End Get
        Set(value As String)
            Dim i As Integer = Me.RecordData.FindItemIndex("city")
            Dim nvp As New NameValuePair("City", value)

            If i = -1 Then
                Me.RecordData.Add(nvp)
            Else Me.RecordData(i) = nvp
            End If
        End Set
    End Property
    Public Property State As String
        Get
            Dim retVal As String = ""
            If Me.RecordData Is Nothing Then Me.RecordData = New List(Of NameValuePair)
            For Each v As NameValuePair In Me.RecordData
                If v.Name IsNot Nothing AndAlso v.Name.ToLower = "state" Then
                    retVal = v.value
                    Exit For
                End If
            Next
            Return retVal
        End Get
        Set(value As String)
            Dim i As Integer = Me.RecordData.FindItemIndex("state")
            Dim nvp As New NameValuePair("State", value)

            If i = -1 Then
                Me.RecordData.Add(nvp)
            Else Me.RecordData(i) = nvp
            End If
        End Set
    End Property
    Public Property ZipCode As String
        Get
            Dim retVal As String = ""
            If Me.RecordData Is Nothing Then Me.RecordData = New List(Of NameValuePair)
            For Each v As NameValuePair In Me.RecordData
                If v.Name IsNot Nothing AndAlso v.Name.ToLower = "zipcode" Then
                    retVal = v.value
                    Exit For
                End If
            Next
            Return retVal
        End Get
        Set(value As String)
            Dim i As Integer = Me.RecordData.FindItemIndex("zipcode")
            Dim nvp As New NameValuePair("ZipCode", value)

            If i = -1 Then
                Me.RecordData.Add(nvp)
            Else Me.RecordData(i) = nvp
            End If
        End Set
    End Property
    Public ReadOnly Property SearchAddress As String
        Get
            Return Me.ServiceAddress & " " & Me.City & " " & Me.State & " " & Me.ZipCode
        End Get
    End Property
    Public RecordData As List(Of NameValuePair)

#Region "Workers"

    Public Sub Initialize(ByVal xmlData As String)
        Try
            Me.RecordData = New List(Of NameValuePair)

            Dim xDoc As New XmlDocument
            xDoc.LoadXml(xmlData)

            For Each xElem As XmlElement In xDoc.Item("Data").ChildNodes
                Dim n As String = xElem.Name
                Dim v As String = xElem.InnerText.XmlDecode

                Me.RecordData.Add(New NameValuePair(n, v))
            Next

        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Public Function Save() As CustomerLocation
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As New CustomerLocation

        Try
            Dim addNew As Boolean = True
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID] FROM [Locations_new] WHERE [LocationNum] LIKE '" & Me.LocationId & "' OR [ID] = " & Me.ID & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                addNew = False
            End If

            If addNew Then
                Dim fields As String = ""
                Dim values As String = ""
                For Each itm In Me.RecordData
                    If fields = "" Then fields = "[" & itm.Name & "]" Else fields &= ", [" & itm.Name & "]"
                    If values = "" Then values = "'" & itm.value & "'" Else values &= ", '" & itm.value & "'"
                Next

                fields &= "[dtInserted], [dtUpdated], [Active]"
                values &= "'" & Now.ToString & "', '" & Now.ToString & "', '1'"

                cmd = New SqlClient.SqlCommand("INSERT INTO [Locations_new] (" & fields & ") VALUES (" & values & ");SELECT @@Identity AS SCOPEIDENTITY;", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                If rs.Read Then
                    Me.ID = rs("SCOPEIDENTITY").ToString.ToInteger
                End If
                rs.Close()
                cmd.Cancel()
            Else
                Dim fields As String = ""
                For Each itm In Me.RecordData
                    If fields = "" Then fields = "[" & itm.Name & "] = '" & itm.value & "'" Else fields &= ", [" & itm.Name & "] = '" & itm.value & "'"
                Next

                fields &= "[dtUpdated] = '" & Now.ToString & "'"

                Dim sWhere As String = ""
                cmd = New SqlClient.SqlCommand("UPDATE [Locations_new] SET " & fields & " WHERE [LocationNum] LIKE '" & Me.LocationId & "' OR [ID] = " & Me.ID, cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
            End If

            retVal = Me

        Catch ex As Exception
            retVal = New CustomerLocation
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Function Delete() As Boolean
        Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

        Dim retVal As Boolean = True

        Try
            Dim cmd As New SqlClient.SqlCommand("UPDATE [Locations_new] SET [Active] = 0, [dtUpdated] = '" & Now.ToString & "' WHERE [ID] = " & Me.ID, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

        Catch ex As Exception
            retVal = False
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

#End Region

End Class

Public Structure NameValuePair
    Public Sub New(ByVal n As String, ByVal v As String)
        Me.Name = n
        Me.value = v
    End Sub

    Public Name As String
    Public value As String
End Structure

Public Structure AuditLogEntry
    Public ActionType As String
    Public Description As String
    Public UserID As Integer
    Public ClientID As Integer
    Public IpAddress As String
    Public Project As String
End Structure

Public Structure MaintenanceInfo
    Public Web As Boolean
    Public Android As Boolean
    Public iOS As Boolean
    Public Property Active As Boolean
        Get
            Return Web Or Android Or iOS
        End Get
        Set(value As Boolean)
        End Set
    End Property
    Public Notes As String
End Structure

Public Structure CommonQuestions
    Public Shared Question As New List(Of String)(New String() {"Customer Account Number",
                                                                "Location Number",
                                                                "Customer Name",
                                                                "Service Address",
                                                                "Service Lat",
                                                                "Service Lon",
                                                                "Status",
                                                                "Supervisor",
                                                                "Technician",
                                                                "Technician Comments",
                                                                "Priority"})

    Public Shared DataFieldName As New List(Of String)(New String() {"CustAcctNum",
                                                                     "LocationNum",
                                                                     "CustomerName",
                                                                     "CustomerServiceAddress",
                                                                     "CustomerServiceAddressLat",
                                                                     "CustomerServiceAddressLon",
                                                                     "Status",
                                                                     "Supervisor",
                                                                     "Technician",
                                                                     "TechnicianComments",
                                                                     "Priority"})

    Public Shared [Type] As New List(Of Enums.SystemQuestionType)(New Enums.SystemQuestionType() {Enums.SystemQuestionType.TextBox,
                                                                                                  Enums.SystemQuestionType.TextBox,
                                                                                                  Enums.SystemQuestionType.TextBox,
                                                                                                  Enums.SystemQuestionType.MemoField,
                                                                                                  Enums.SystemQuestionType.TextBox,
                                                                                                  Enums.SystemQuestionType.TextBox,
                                                                                                  Enums.SystemQuestionType.DropDownList,
                                                                                                  Enums.SystemQuestionType.DropDownList,
                                                                                                  Enums.SystemQuestionType.DropDownList,
                                                                                                  Enums.SystemQuestionType.MemoField,
                                                                                                  Enums.SystemQuestionType.DropDownList})
End Structure