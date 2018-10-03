Imports System.Web

Public Class App

    Public Shared Property CurrentUser As SystemUser
        Get
            Dim retVal As New SystemUser(UseSandboxDb)
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("CurrentUser") IsNot Nothing Then retVal = CType(HttpContext.Current.Session("CurrentUser"), SystemUser)
            Return retVal
        End Get
        Set(value As SystemUser)
            HttpContext.Current.Session("CurrentUser") = value
        End Set
    End Property

    Public Shared Property UseSandboxDb As Boolean
        Get
            Dim retVal As Boolean = False
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("UseSandboxDb") IsNot Nothing Then retVal = CBool(HttpContext.Current.Session("UseSandboxDb"))
            Return retVal
        End Get
        Set(value As Boolean)
            HttpContext.Current.Session("UseSandboxDb") = value
        End Set
    End Property

    Public Shared Property CurrentClient As SystemClient
        Get
            Dim retVal As New SystemClient(UseSandboxDb)
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
            Dim retVal As New SystemModule(UseSandboxDb)
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

    Public Shared ReadOnly Property FieldExtras As List(Of FieldExtras)
        Get
            If HttpContext.Current.Session("FieldInfoLookup") Is Nothing Then
                Dim lst As New List(Of FieldExtras)

                Dim cn As New SqlClient.SqlConnection([Shared].Common.ConnectionString(App.UseSandboxDb))

                Try
                    Dim cmd As New SqlClient.SqlCommand("select fe.FieldName, fe.DisplayText, c.Data_Type as DataType from _MasterFeedFieldExtras fe inner join information_schema.columns c on c.COLUMN_NAME = fe.FieldName ORDER BY fe.FieldName", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    Do While rs.Read
                        Dim fe As New FieldExtras
                        fe.FieldName = rs("FieldName").ToString
                        If Not IsDBNull(rs("DisplayText")) Then
                            fe.DisplayText = rs("DisplayText").ToString
                        Else fe.DisplayText = rs("FieldName").ToString
                        End If
                        fe.DataType = rs("DataType").ToString

                        lst.Add(fe)
                    Loop
                    cmd.Cancel()
                    rs.Close()

                Catch ex As Exception
                    ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared, UseSandboxDb))
                Finally
                    cn.Close()
                End Try

                HttpContext.Current.Session("FieldInfoLookup") = lst
            End If

            Return CType(HttpContext.Current.Session("FieldInfoLookup"), List(Of FieldExtras))
        End Get
    End Property
End Class
