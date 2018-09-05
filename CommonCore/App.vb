Imports System.Web

Public Class App
    Public Shared Property CurrentUser As SystemUser
        Get
            Dim retVal As New SystemUser
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("CurrentUser") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("CurrentUser"), SystemUser)
            Return retVal
        End Get
        Set(value As SystemUser)
            HttpContext.Current.Session("CurrentUser") = value
        End Set
    End Property

    Public Shared Property CurrentClient As SystemClient
        Get
            Dim retVal As New SystemClient
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("CurrentClient") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("CurrentClient"), SystemClient)
            Return retVal
        End Get
        Set(value As SystemClient)
            HttpContext.Current.Session("CurrentClient") = value
        End Set
    End Property

    Public Shared Property ActiveFolderID As Integer
        Get
            Dim retVal As Integer = 0
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("ActiveFolderID") IsNot Nothing Then retVal = HttpContext.Current.Session("ActiveFolderID").ToString.ToInteger
            Return retVal
        End Get
        Set(value As Integer)
            HttpContext.Current.Session("ActiveFolderID") = value
        End Set
    End Property

    Public Shared Property ActiveModule As SystemModule
        Get
            Dim retVal As New SystemModule
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("ActiveModule") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("ActiveModule"), SystemModule)
            Return retVal
        End Get
        Set(value As SystemModule)
            HttpContext.Current.Session("ActiveModule") = value
        End Set
    End Property

    Public Shared ReadOnly Property RootFolderQuestions() As List(Of SystemQuestion)
        Get
            Return LoadModuleQuestions(App.ActiveFolderID)
        End Get
    End Property

    Public Shared ReadOnly Property ActiveModuleQuestions() As List(Of SystemQuestion)
        Get
            Return LoadModuleQuestions(App.ActiveModule.ID)
        End Get
    End Property

    Public Shared Property Mobile_Questions() As DataTable
        Get
            Dim retVal As New DataTable
            retVal.Columns.Add("ID", GetType(Integer))
            retVal.Columns.Add("Question", GetType(String))
            retVal.Columns.Add("DataFieldName", GetType(String))
            retVal.Columns.Add("Type", GetType(Enums.SystemQuestionType))
            retVal.Columns.Add("Values", GetType(String))
            retVal.Columns.Add("XmlPath", GetType(String))
            retVal.Columns.Add("Locked", GetType(Boolean))

            If HttpContext.Current.Session("Mobile_Questions") Is Nothing Then HttpContext.Current.Session("Mobile_Questions") = retVal Else retVal = CType(HttpContext.Current.Session("Mobile_Questions"), DataTable)

            Return retVal
        End Get
        Set(value As DataTable)
            If value Is Nothing Then
                value = New DataTable
                value.Columns.Add("ID", GetType(Integer))
                value.Columns.Add("Question", GetType(String))
                value.Columns.Add("DataFieldName", GetType(String))
                value.Columns.Add("Type", GetType(Enums.SystemQuestionType))
                value.Columns.Add("Values", GetType(String))
                value.Columns.Add("XmlPath", GetType(String))
                value.Columns.Add("Locked", GetType(Boolean))
            End If
            HttpContext.Current.Session("Mobile_Questions") = value
        End Set
    End Property

    Public Shared Property Mobile_SupervisorID As Integer
        Get
            Dim retVal As Integer = 0
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("Mobile_SupervisorID") IsNot Nothing Then retVal = HttpContext.Current.Session("Mobile_SupervisorID").ToString.ToInteger
            Return retVal
        End Get
        Set(value As Integer)
            HttpContext.Current.Session("Mobile_SupervisorID") = value
        End Set
    End Property

    Public Shared Property Mobile_TechnicianID As Integer
        Get
            Dim retVal As Integer = 0
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("Mobile_TechnicianID") IsNot Nothing Then retVal = HttpContext.Current.Session("Mobile_TechnicianID").ToString.ToInteger
            Return retVal
        End Get
        Set(value As Integer)
            HttpContext.Current.Session("Mobile_TechnicianID") = value
        End Set
    End Property

    Public Shared Property CurrentAccountNumber As String
        Get
            Dim retVal As String = ""
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("CurrentAccountNumber") IsNot Nothing Then retVal = HttpContext.Current.Session("CurrentAccountNumber").ToString
            Return retVal
        End Get
        Set(value As String)
            HttpContext.Current.Session("CurrentAccountNumber") = value
        End Set
    End Property

    Public Shared ReadOnly Property FieldInfoLookup As Hashtable
        Get
            If HttpContext.Current.Session("FieldInfoLookup") Is Nothing Then
                Dim ht As New Hashtable

                Dim cn As New SqlClient.SqlConnection(CommonCore.Shared.Common.ConnectionString)

                Dim retVal As Boolean = True

                Try
                    Dim cmd As New SqlClient.SqlCommand("SELECT [Table], [Key], [Value], [Description] FROM [FieldInfoLookup] ORDER BY [Table], [Key]", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    Do While rs.Read
                        Dim key As String = rs("Table").ToString & "|" & rs("Key").ToString
                        Dim value As String = ""
                        If Not IsDBNull(rs("Value")) Then value = rs("Value").ToString
                        If Not IsDBNull(rs("Description")) Then value &= "|" & rs("Description").ToString Else value &= "|"
                        If Not ht.ContainsKey(key) Then
                            ht.Add(key, value)
                        End If
                    Loop
                    cmd.Cancel()
                    rs.Close()

                Catch ex As Exception
                    retVal = False
                    ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared))
                Finally
                    cn.Close()
                End Try

                HttpContext.Current.Session("FieldInfoLookup") = ht
            End If

            Return CType(HttpContext.Current.Session("FieldInfoLookup"), Hashtable)
        End Get
    End Property
End Class
