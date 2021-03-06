﻿Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Web

Public Class Common
    Private Const AspNetNamespace As String = "ASP"

    Public Shared Function GetApplicationAssembly(ctx As HttpContext) As Assembly
        ' Try the EntryAssembly, this doesn't work for ASP.NET classic pipeline (untested on integrated)
        Dim ass As Assembly = Assembly.GetEntryAssembly()

        ' Look for web application assembly
        If ctx IsNot Nothing Then
            ass = GetWebApplicationAssembly(ctx)
        End If

        ' Fallback to executing assembly
        Return If(ass, (Assembly.GetExecutingAssembly()))
    End Function

    Private Shared Function GetWebApplicationAssembly(context As HttpContext) As Assembly
        'Guard.AgainstNullArgument(context)

        Dim app As Object = context.ApplicationInstance
        If app Is Nothing Then
            Return Nothing
        End If

        Dim type As Type = app.[GetType]()
        While type IsNot Nothing AndAlso type <> GetType(Object) AndAlso type.[Namespace] = AspNetNamespace
            type = type.BaseType
        End While

        Return type.Assembly
    End Function

    'Public Shared Function ConnectionString() As String
    '    Return ConnectionString(False)
    'End Function
    Public Shared Function ConnectionString(ByVal UseSandbox As Boolean) As String
        ' override the sandbox setting to force live database
        UseSandbox = False

        If Not UseSandbox Then
            Return ConnectionString(Enums.DBName.UtilityWizards)
        Else Return ConnectionString(Enums.DBName.UtilityWizards_Sandbox)
        End If
    End Function
    Public Shared Function ConnectionString(ByVal DbName As Enums.DBName) As String
        Dim retVal As String = ""

        Dim db As String = ""

        Select Case DbName
            Case Enums.DBName.UtilityWizards
                db = "UtilityWizards"

            Case Enums.DBName.UtilityWizards_Sandbox
                db = "UtilityWizards_Sandbox"

            Case Else
                'If System.Configuration.ConfigurationManager.ConnectionStrings(DbName) IsNot Nothing Then
                '    retVal = System.Configuration.ConfigurationManager.ConnectionStrings(DbName).ConnectionString
                '    Return retVal
                'End If

        End Select

        retVal = My.Settings.DbConnection.Replace("[DataBaseName]", db)

        Return retVal
    End Function

    Public Shared Function SearchDir(ByVal sDir As String, ByVal searchPattern As String, ByVal sort As Enums.FileSort) As List(Of IO.FileInfo)
        Dim retVal As New List(Of IO.FileInfo)

        Dim d As String
        Dim f As String

        Try
            ' recursively search any sub directories
            ' add files for the current directory
            For Each f In Directory.GetFiles(sDir, searchPattern)
                Dim fi As New FileInfo(f)
                retVal.Add(fi)
            Next

            ' add any sub directories
            Dim directories() As String = Directory.GetDirectories(sDir)
            For Each d In directories
                retVal.AddRange(SearchDir(d, searchPattern, sort))
            Next

            Select Case sort
                Case Enums.FileSort.Name
                    retVal.Sort(Function(p1, p2) p1.Name.CompareTo(p2.Name))
                Case Enums.FileSort.Size
                    retVal.Sort(Function(p1, p2) p1.Length.CompareTo(p2.Length))
            End Select

        Catch ex As System.Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared, False))
        End Try

        Return retVal
    End Function

    Public Shared Function ScrapeUrl(ByVal url As String, ByVal type As Enums.ScrapeType) As String
        Dim retVal As String = ""

        Try
            Dim strOutput As String = ""

            Dim wrResponse As WebResponse
            Dim wrRequest As WebRequest = HttpWebRequest.Create(url)

            wrResponse = wrRequest.GetResponse()

            Using sr As New StreamReader(wrResponse.GetResponseStream())
                strOutput = sr.ReadToEnd()
                sr.Close()
            End Using

            'Formatting Techniques
            If Not type = Enums.ScrapeType.ReturnAll Then
                ' Remove Doctype ( HTML 5 )
                strOutput = Regex.Replace(strOutput, "<!(.|\s)*?>", "")

                If Not type = Enums.ScrapeType.KeepTags Then
                    ' Replace HTML Tags with a pipe (|) so we can keep values separate
                    strOutput = Regex.Replace(strOutput, "</?[a-z][a-z0-9]*[^<>]*>", "|")
                End If

                ' Remove HTML Comments
                strOutput = Regex.Replace(strOutput, "<!--(.|\s)*?-->", "")

                ' Remove Script Tags
                strOutput = Regex.Replace(strOutput, "<script.*?</script>", "", RegexOptions.Singleline Or RegexOptions.IgnoreCase)

                ' Remove Stylesheets
                strOutput = Regex.Replace(strOutput, "<style.*?</style>", "", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
            End If

            retVal = strOutput

        Catch ex As Exception
            Console.WriteLine(ex.Message, "Error")
        End Try

        Return retVal
    End Function

    Public Shared Sub LogHistory(ByVal itemText As String, ByVal userId As Integer, ByVal UseSandboxDb As Boolean)
        Dim cn As New SqlClient.SqlConnection(ConnectionString(UseSandboxDb))

        Try
            Dim cmd As New SqlClient.SqlCommand("INSERT INTO [Sys_History] ([itemText], [dtInserted], [insertedBy]) VALUES ('" & itemText & "', '" & Now.ToString & "', '" & userId & "');SELECT @@Identity AS SCOPEIDENTITY;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(userId, 0, Enums.ProjectName.Builder, UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    Public Shared Function GetFieldExtras(ByVal UseSandboxDb As Boolean) As List(Of FieldExtras)
        Dim retVal As New List(Of FieldExtras)

        Dim cn As New SqlClient.SqlConnection([Shared].Common.ConnectionString(UseSandboxDb))

        Try
            Dim cmd As New SqlClient.SqlCommand("procGetFieldExtras", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim fe As New FieldExtras
                fe.MasterFeedFieldName = rs("FieldName").ToString
                If Not IsDBNull(rs("DisplayText")) Then
                    fe.DisplayText = rs("DisplayText").ToString
                Else fe.DisplayText = rs("FieldName").ToString
                End If
                If Not IsDBNull(rs("ImportTableName")) Then
                    fe.ImportTableName = rs("ImportTableName").ToString
                End If
                If Not IsDBNull(rs("ImportFieldName")) Then
                    fe.ImportFieldName = rs("ImportFieldName").ToString
                End If
                If Not IsDBNull(rs("Description")) Then
                    fe.Description = rs("Description").ToString
                End If
                If Not IsDBNull(rs("DataType")) Then
                    fe.DataType = rs("DataType").ToString
                End If

                retVal.Add(fe)
            Loop
            cmd.Cancel()
            rs.Close()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared, UseSandboxDb))
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Public Shared Function UploadFile(ByVal fName As String, ByVal fData As Byte(), ByVal UseSandboxDb As Boolean) As String
        Dim retVal As String = fName

        Try
            ' save the file data
            File.WriteAllBytes(fName, fData)

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared, UseSandboxDb))
            retVal = ex.ToString
        End Try

        Return retVal
    End Function

    Public Shared Function UploadFileToAWS(ByVal fName As String, ByVal fData As Byte(), ByVal UseSandboxDb As Boolean) As String
        Dim retVal As String = ""

        Try
            ' save the file data
            Dim tmpPath As String = UploadFile(fName, fData, UseSandboxDb)
            Dim fi As New FileInfo(tmpPath)

            ' move the file to aws
            Dim aws As New AWSHelper
            Dim awsMsg As Boolean = aws.AddFileToFolder(fi.FullName, "")

            If awsMsg Then
                Dim FoundIt As Boolean = False

                ' pull a directory from aws to make sure the file is in the bucket
                Dim dir As List(Of AWSHelper.AwsFileInfo) = aws.ListFiles()
                For Each afi As AWSHelper.AwsFileInfo In dir
                    If afi.Name.ToLower = fi.Name.ToLower Then
                        FoundIt = True
                        Exit For
                    End If
                Next

                If FoundIt Then
                    ' delete the local file on success
                    My.Computer.FileSystem.DeleteFile(tmpPath)
                End If
            Else retVal = "Error uploading to AWS"
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Enums.ProjectName.CommonCoreShared, UseSandboxDb))
            retVal = ex.ToString
        End Try

        Return retVal
    End Function
End Class
