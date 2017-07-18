Imports System.Xml

Public Class XmlUpgrade
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.UpdateCustomerRecords()
        'Me.UpdateClientRecords(0)
        'Me.UpdateUserRecords(0)
        'Me.UpdateModuleRecords(0)
        'Me.UpdateQuestionRecords(0)
        'Me.UpdateReportRecords(0)
    End Sub

    Private Sub UpdateCustomerRecords()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim lst As New List(Of XmlDocument)

            Dim cmd As New SqlClient.SqlCommand("SELECT * FROM [Customers];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim xDoc As New XmlDocument
                xDoc = NewXmlDocument("Data")

                Dim e As XmlElement = xDoc.Item("Data")

                For x As Integer = 0 To rs.FieldCount - 1
                    Dim n As String = rs.GetName(x).Replace(" ", "")
                    Dim v As Object = rs.GetValue(x)
                    If v Is Nothing OrElse IsDBNull(v) Then v = ""

                    e.AppendChild(xDoc.NewElement(n, v.ToString))
                Next

                lst.Add(xDoc)
            Loop
            cmd.Cancel()
            rs.Close()

            For Each itm In lst
                cmd = New SqlClient.SqlCommand("INSERT INTO [CustomersNew] ([xmlData], [dtInserted], [dtUpdated]) VALUES (@xmlData, '" & Now.ToString & "', '" & Now.ToString & "');", cn)
                cmd.Parameters.AddWithValue("@xmlData", itm.ToXmlString)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            Next

        Catch ex As Exception
            Dim s As String = ""
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub UpdateClientRecords(ByVal startId As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim ht As New Hashtable

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Clients] WHERE [ID] >= " & startId & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim xDoc As New XmlDocument
                xDoc.LoadXml(rs("xmlData").ToString)

                Dim obj As New SystemClient()
                obj.ID = rs("ID").ToString.ToInteger
                If xDoc.Item("SystemClient").Item("Name") IsNot Nothing Then obj.Name = xDoc.Item("SystemClient").Item("Name").InnerText
                If xDoc.Item("SystemClient").Item("Address1") IsNot Nothing Then obj.Address1 = xDoc.Item("SystemClient").Item("Address1").InnerText
                If xDoc.Item("SystemClient").Item("Address2") IsNot Nothing Then obj.Address2 = xDoc.Item("SystemClient").Item("Address2").InnerText
                If xDoc.Item("SystemClient").Item("City") IsNot Nothing Then obj.City = xDoc.Item("SystemClient").Item("City").InnerText
                If xDoc.Item("SystemClient").Item("State") IsNot Nothing Then obj.State = xDoc.Item("SystemClient").Item("State").InnerText
                If xDoc.Item("SystemClient").Item("ZipCode") IsNot Nothing Then obj.ZipCode = xDoc.Item("SystemClient").Item("ZipCode").InnerText
                If xDoc.Item("SystemClient").Item("ContactName") IsNot Nothing Then obj.ContactName = xDoc.Item("SystemClient").Item("ContactName").InnerText
                If xDoc.Item("SystemClient").Item("ContactPhone") IsNot Nothing Then obj.ContactPhone = xDoc.Item("SystemClient").Item("ContactPhone").InnerText
                If xDoc.Item("SystemClient").Item("ContactEmail") IsNot Nothing Then obj.ContactEmail = xDoc.Item("SystemClient").Item("ContactEmail").InnerText
                If xDoc.Item("SystemClient").Item("Active") IsNot Nothing Then obj.Active = xDoc.Item("SystemClient").Item("Active").InnerText.ToBoolean
                If xDoc.Item("SystemClient").Item("Approved") IsNot Nothing Then
                    If xDoc.Item("SystemClient").Item("Approved").InnerText = Enums.SystemMode.Demo.ToString Then
                        obj.Approved = Enums.SystemMode.Demo
                    ElseIf xDoc.Item("SystemClient").Item("Approved").InnerText = Enums.SystemMode.Live.ToString Then
                        obj.Approved = Enums.SystemMode.Live
                    End If
                End If
                If xDoc.Item("SystemClient").Item("DemoStartDate") IsNot Nothing Then obj.DemoStartDate = xDoc.Item("SystemClient").Item("DemoStartDate").InnerText.ToDate(Now)
                If xDoc.Item("SystemClient").Item("DemoDuration") IsNot Nothing Then obj.DemoDuration = xDoc.Item("SystemClient").Item("DemoDuration").InnerText.ToInteger

                ht.Add("_" & rs("ID").ToString, obj)
            Loop
            cmd.Cancel()
            rs.Close()

            For Each itm In ht.Keys
                Dim recordId As Integer = itm.ToString.Replace("_", "").ToInteger
                Dim xmlData As String = CType(ht(itm), SystemClient).SerializeToXml
                cmd = New SqlClient.SqlCommand("UPDATE [Clients] SET [xmlData] = @xmlData WHERE [ID] = " & recordId, cn)
                cmd.Parameters.AddWithValue("@xmlData", xmlData)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            Next

        Catch ex As Exception
            Dim s As String = ""
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub UpdateUserRecords(ByVal startId As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim ht As New Hashtable

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Users] WHERE [ID] >= " & startId & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim xDoc As New XmlDocument
                xDoc.LoadXml(rs("xmlData").ToString)

                Dim obj As New SystemUser()
                obj.ID = rs("ID").ToString.ToInteger
                If xDoc.Item("Data").Item("Name") IsNot Nothing Then obj.Name = xDoc.Item("Data").Item("Name").InnerText
                If xDoc.Item("Data").Item("Email") IsNot Nothing Then obj.Email = xDoc.Item("Data").Item("Email").InnerText
                If xDoc.Item("Data").Item("Password") IsNot Nothing Then obj.Password = xDoc.Item("Data").Item("Password").InnerText
                If xDoc.Item("Data").Item("Permissions") IsNot Nothing Then obj.Permissions = CType(xDoc.Item("Data").Item("Permissions").InnerText, Enums.SystemUserPermissions)
                If xDoc.Item("Data").Item("MobileDeviceId") IsNot Nothing Then obj.MobileDeviceId = xDoc.Item("Data").Item("MobileDeviceId").InnerText
                If xDoc.Item("Data").Item("MobileUsername") IsNot Nothing Then obj.MobileUsername = xDoc.Item("Data").Item("MobileUsername").InnerText
                If xDoc.Item("Data").Item("MobileNumber") IsNot Nothing Then obj.MobileNumber = xDoc.Item("Data").Item("MobileNumber").InnerText
                If xDoc.Item("Data").Item("Active") IsNot Nothing Then obj.Active = xDoc.Item("Data").Item("Active").InnerText.ToBoolean
                If xDoc.Item("Data").Item("ClientID") IsNot Nothing Then obj.ClientID = xDoc.Item("Data").Item("ClientID").InnerText.ToInteger
                If xDoc.Item("Data").Item("SupervisorID") IsNot Nothing Then obj.SupervisorID = xDoc.Item("Data").Item("SupervisorID").InnerText.ToInteger
                If xDoc.Item("Data").Item("MobileDeviceType") IsNot Nothing Then obj.MobileDeviceType = CType(xDoc.Item("Data").Item("MobileDeviceType").InnerText, Enums.UserPlatform)
                If xDoc.Item("Data").Item("APIResponseCode") IsNot Nothing Then obj.APIResponseCode = CType(xDoc.Item("Data").Item("APIResponseCode").InnerText, Enums.ApiResultCode)
                If xDoc.Item("Data").Item("APIResponseMessage") IsNot Nothing Then obj.APIResponseMessage = xDoc.Item("Data").Item("APIResponseMessage").InnerText
                If xDoc.Item("Data").Item("ApiEnabled") IsNot Nothing Then obj.ApiEnabled = xDoc.Item("Data").Item("ApiEnabled").InnerText.ToBoolean
                If xDoc.Item("Data").Item("WebEnabled") IsNot Nothing Then obj.WebEnabled = xDoc.Item("Data").Item("WebEnabled").InnerText.ToBoolean
                If xDoc.Item("Data").Item("OneSignalUserID") IsNot Nothing Then obj.OneSignalUserID = xDoc.Item("Data").Item("OneSignalUserID").InnerText
                If xDoc.Item("Data").Item("OneSignalPushToken") IsNot Nothing Then obj.OneSignalPushToken = xDoc.Item("Data").Item("OneSignalPushToken").InnerText

                ht.Add("_" & rs("ID").ToString, obj)
            Loop
            cmd.Cancel()
            rs.Close()

            For Each itm In ht.Keys
                Dim recordId As Integer = itm.ToString.Replace("_", "").ToInteger
                Dim xmlData As String = CType(ht(itm), SystemUser).SerializeToXml
                cmd = New SqlClient.SqlCommand("UPDATE [Users] SET [xmlData] = @xmlData WHERE [ID] = " & recordId, cn)
                cmd.Parameters.AddWithValue("@xmlData", xmlData)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            Next

        Catch ex As Exception
            Dim s As String = ""
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub UpdateModuleRecords(ByVal startId As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim ht As New Hashtable

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Modules] WHERE [ID] >= " & startId & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim xDoc As New XmlDocument
                xDoc.LoadXml(rs("xmlData").ToString)

                Dim obj As New SystemModule()
                obj.ID = rs("ID").ToString.ToInteger
                If xDoc.Item("Data").Item("ClientID") IsNot Nothing Then obj.ClientID = xDoc.Item("Data").Item("ClientID").InnerText.ToInteger
                If xDoc.Item("Data").Item("FolderID") IsNot Nothing Then obj.FolderID = xDoc.Item("Data").Item("FolderID").InnerText.ToInteger
                If xDoc.Item("Data").Item("Name") IsNot Nothing Then obj.Name = xDoc.Item("Data").Item("Name").InnerText
                If xDoc.Item("Data").Item("Description") IsNot Nothing Then obj.Description = xDoc.Item("Data").Item("Description").InnerText
                If xDoc.Item("Data").Item("Icon") IsNot Nothing Then obj.Icon = xDoc.Item("Data").Item("Icon").InnerText
                If xDoc.Item("Data").Item("Type") IsNot Nothing Then obj.Type = CType(xDoc.Item("Data").Item("Type").InnerText, Enums.SystemModuleType)
                If xDoc.Item("Data").Item("SupervisorID") IsNot Nothing Then obj.SupervisorID = xDoc.Item("Data").Item("SupervisorID").InnerText.ToInteger
                If xDoc.Item("Data").Item("APIResponseCode") IsNot Nothing Then obj.APIResponseCode = CType(xDoc.Item("Data").Item("APIResponseCode").InnerText, Enums.ApiResultCode)
                If xDoc.Item("Data").Item("APIResponseMessage") IsNot Nothing Then obj.APIResponseMessage = xDoc.Item("Data").Item("APIResponseMessage").InnerText

                ht.Add("_" & rs("ID").ToString, obj)
            Loop
            cmd.Cancel()
            rs.Close()

            For Each itm In ht.Keys
                Dim recordId As Integer = itm.ToString.Replace("_", "").ToInteger
                Dim xmlData As String = CType(ht(itm), SystemModule).SerializeToXml
                cmd = New SqlClient.SqlCommand("UPDATE [Modules] SET [xmlData] = @xmlData WHERE [ID] = " & recordId, cn)
                cmd.Parameters.AddWithValue("@xmlData", xmlData)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            Next

        Catch ex As Exception
            Dim s As String = ""
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub UpdateQuestionRecords(ByVal startId As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim ht As New Hashtable

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Questions] WHERE [ID] >= " & startId & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim xDoc As New XmlDocument
                xDoc.LoadXml(rs("xmlData").ToString)

                Dim obj As New SystemQuestion()
                obj.ID = rs("ID").ToString.ToInteger
                If xDoc.Item("Data").Item("ModuleID") IsNot Nothing Then obj.ModuleID = xDoc.Item("Data").Item("ModuleID").InnerText.ToInteger
                If xDoc.Item("Data").Item("Question") IsNot Nothing Then obj.Question = xDoc.Item("Data").Item("Question").InnerText
                If xDoc.Item("Data").Item("DataFieldName") IsNot Nothing Then obj.DataFieldName = xDoc.Item("Data").Item("DataFieldName").InnerText
                If xDoc.Item("Data").Item("DataFieldNameOverride") IsNot Nothing Then obj.DataFieldNameOverride = xDoc.Item("Data").Item("DataFieldNameOverride").InnerText
                If xDoc.Item("Data").Item("Rule") IsNot Nothing Then obj.Rule = xDoc.Item("Data").Item("Rule").InnerText
                If xDoc.Item("Data").Item("Type") IsNot Nothing Then obj.Type = CType(xDoc.Item("Data").Item("Type").InnerText, Enums.SystemQuestionType)
                If xDoc.Item("Data").Item("Required") IsNot Nothing Then obj.Required = xDoc.Item("Data").Item("Required").InnerText.ToBoolean
                If xDoc.Item("Data").Item("Values") IsNot Nothing Then
                    For Each s As String In xDoc.Item("Data").Item("Values").InnerText.Split("|"c)
                        If s.Trim <> "" Then obj.Values.Add(s.Trim.XmlDecode)
                    Next
                End If
                If xDoc.Item("Data").Item("SearchField") IsNot Nothing Then obj.SearchField = xDoc.Item("Data").Item("SearchField").InnerText.ToBoolean
                If xDoc.Item("Data").Item("ReportField") IsNot Nothing Then obj.ReportField = xDoc.Item("Data").Item("ReportField").InnerText.ToBoolean
                If xDoc.Item("Data").Item("ExportField") IsNot Nothing Then obj.ExportField = xDoc.Item("Data").Item("ExportField").InnerText.ToBoolean
                If xDoc.Item("Data").Item("MobileField") IsNot Nothing Then obj.MobileField = xDoc.Item("Data").Item("MobileField").InnerText.ToBoolean
                If xDoc.Item("Data").Item("MobileData") IsNot Nothing Then obj.MobileData = xDoc.Item("Data").Item("MobileData").InnerText
                If xDoc.Item("Data").Item("NewMobileData") IsNot Nothing Then obj.NewMobileData = xDoc.Item("Data").Item("NewMobileData").InnerText
                If xDoc.Item("Data").Item("Locked") IsNot Nothing Then obj.Locked = xDoc.Item("Data").Item("Locked").InnerText.ToBoolean
                If xDoc.Item("Data").Item("Visible") IsNot Nothing Then obj.Visible = xDoc.Item("Data").Item("Visible").InnerText.ToBoolean

                ht.Add("_" & rs("ID").ToString, obj)
            Loop
            cmd.Cancel()
            rs.Close()

            For Each itm In ht.Keys
                Dim recordId As Integer = itm.ToString.Replace("_", "").ToInteger
                Dim xmlData As String = CType(ht(itm), SystemQuestion).SerializeToXml
                cmd = New SqlClient.SqlCommand("UPDATE [Questions] SET [xmlData] = @xmlData WHERE [ID] = " & recordId, cn)
                cmd.Parameters.AddWithValue("@xmlData", xmlData)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            Next

        Catch ex As Exception
            Dim s As String = ""
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub UpdateReportRecords(ByVal startId As Integer)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim ht As New Hashtable

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xmlData] FROM [Reports] WHERE [ID] >= " & startId & ";", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim xDoc As New XmlDocument
                xDoc.LoadXml(rs("xmlData").ToString)

                Dim obj As New SystemReport()
                obj.ID = rs("ID").ToString.ToInteger
                If xDoc.Item("Data").Item("Name") IsNot Nothing Then obj.Name = xDoc.Item("Data").Item("Name").InnerText
                If xDoc.Item("Data").Item("Description") IsNot Nothing Then obj.Description = xDoc.Item("Data").Item("Description").InnerText
                If xDoc.Item("Data").Item("Active") IsNot Nothing Then obj.Active = xDoc.Item("Data").Item("Active").InnerText.ToBoolean
                If xDoc.Item("Data").Item("ClientID") IsNot Nothing Then obj.ClientID = xDoc.Item("Data").Item("ClientID").InnerText.ToInteger
                If xDoc.Item("Data").Item("Fields") IsNot Nothing Then
                    For Each s As String In xDoc.Item("Data").Item("Fields").InnerText.Split("^"c)
                        If s.Trim <> "" Then obj.Fields.Add(s.Trim.XmlDecode)
                    Next
                End If
                If xDoc.Item("Data").Item("ModuleId") IsNot Nothing Then obj.ModuleId = xDoc.Item("Data").Item("ModuleId").InnerText.ToInteger

                ht.Add("_" & rs("ID").ToString, obj)
            Loop
            cmd.Cancel()
            rs.Close()

            For Each itm In ht.Keys
                Dim recordId As Integer = itm.ToString.Replace("_", "").ToInteger
                Dim xmlData As String = CType(ht(itm), SystemReport).SerializeToXml
                cmd = New SqlClient.SqlCommand("UPDATE [Reports] SET [xmlData] = @xmlData WHERE [ID] = " & recordId, cn)
                cmd.Parameters.AddWithValue("@xmlData", xmlData)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            Next

        Catch ex As Exception
            Dim s As String = ""
        Finally
            cn.Close()
        End Try
    End Sub

End Class