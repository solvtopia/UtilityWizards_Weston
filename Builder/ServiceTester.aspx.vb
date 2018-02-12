Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Shared.Common
Imports System.Xml

Public Class ServiceTester
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CommonCore.Shared.Messaging.SendTwilioNotification(New SystemUser(1), Enums.NotificationType.NormalWorkOrderCreated, "something", 0)

        'Me.TestInput()
        'OneSignal.SendNotification(New SystemUser(1), "test message from utility wizards")
        Me.FixCustomers("customers_new")
        Me.FixCustomers("locations_new")
    End Sub

    Private Sub FixCustomers(ByVal tbl As String)
        Dim fld As String = If(tbl.ToLower = "customers_new", "AccountNum", "LocationNum")

        Dim cn As New SqlClient.SqlConnection(ConnectionString)
        Dim cn1 As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim sql As String = ""
            Dim fieldNames As String = ""
            Dim firstRun As Boolean = True

            Dim cmd1 As New SqlClient.SqlCommand("SELECT DISTINCT [" & fld & "] FROM [" & tbl & "];", cn1)
            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
            Dim rs1 As SqlClient.SqlDataReader = cmd1.ExecuteReader
            Do While rs1.Read
                Dim cmd As SqlClient.SqlCommand
                Dim rs As SqlClient.SqlDataReader
                Dim fieldList As New Hashtable
                sql = ""

                ' make sure we only have 1 record for the customer
                Dim recordIds As New List(Of Integer)
                sql = "SELECT * FROM [" & tbl & "] WHERE [" & fld & "] LIKE '" & rs1(fld).ToString & "' ORDER BY [ID];"
                cmd = New SqlClient.SqlCommand(sql, cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                rs = cmd.ExecuteReader
                Do While rs.Read
                    If Not recordIds.Contains(rs("ID").ToString.ToInteger) Then recordIds.Add(rs("ID").ToString.ToInteger)

                    ' get a list of all the fields in the customer table
                    For x As Integer = 0 To rs.FieldCount - 1
                        If Not fieldList.Contains(rs.GetName(x)) Then fieldList.Add(rs.GetName(x), Nothing)
                        If firstRun Then
                            If rs.GetName(x).ToLower <> "addlvalues" And rs.GetName(x).ToLower <> "id" And rs.GetName(x).ToLower <> "locationnum" And rs.GetName(x).ToLower <> "searchaddress" Then
                                If fieldNames = "" Then fieldNames = "[" & rs.GetName(x) & "]" Else fieldNames &= ", [" & rs.GetName(x) & "]"
                            End If
                        End If
                    Next
                Loop
                cmd.Cancel()
                rs.Close()

                ' now combine the records into a single record if we have more than 1
                If recordIds.Count > 1 Then
                    cmd = New SqlClient.SqlCommand(sql, cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    rs = cmd.ExecuteReader
                    Do While rs.Read
                        For x As Integer = 0 To fieldList.Keys.Count - 1
                            Dim f As String = fieldList.Keys(x).ToString

                            ' only update the record if the value isn't null
                            If Not IsDBNull(rs(f)) Then
                                ' make sure the fieldlist value is different
                                If fieldList(f) Is Nothing OrElse fieldList(f).ToString.ToLower <> rs(f).ToString.ToLower Then
                                    If rs(f).ToString.ToLower <> "null" Then
                                        fieldList(f) = rs(f)
                                    End If
                                End If
                            End If
                        Next
                    Loop
                    cmd.Cancel()
                    rs.Close()

                    ' update the customers and locations table
                    Dim fields As String = ""
                    For x As Integer = 0 To fieldList.Keys.Count - 1
                        Dim f As String = fieldList.Keys(x).ToString

                        If f.ToLower <> "id" And f.ToLower <> "searchaddress" And f.ToLower <> "locationnum" Then
                            If fieldList(f) Is Nothing Then
                                ' null values
                                If fields = "" Then fields = "[" & f & "] = NULL" Else fields &= ", [" & f & "] = NULL"

                            ElseIf f.ToLower = "dtupdated" Then
                                ' date updated
                                If fields = "" Then fields = "[" & f & "] = '" & fieldList(f).ToString & "'" Else fields &= ", [" & f & "] = '" & fieldList(f).ToString & "'"

                            ElseIf f.ToLower = "active" Then
                                ' active flag
                                If fields = "" Then fields = "[" & f & "] = 1" Else fields &= ", [" & f & "] = 1"

                            ElseIf f.ToLower = "zipcode" Then
                                ' zip codes are strings
                                If fields = "" Then fields = "[" & f & "] = '" & fieldList(f).ToString & "'" Else fields &= ", [" & f & "] = '" & fieldList(f).ToString & "'"

                            ElseIf IsNumeric(fieldList(f)) Then
                                ' numbers and bits
                                If fields = "" Then fields = "[" & f & "] = " & fieldList(f).ToString Else fields &= ", [" & f & "] = " & fieldList(f).ToString

                            ElseIf IsDate(fieldList(f)) Then
                                ' dates
                                If fields = "" Then fields = "[" & f & "] = '" & fieldList(f).ToString & "'" Else fields &= ", [" & f & "] = '" & fieldList(f).ToString & "'"

                            End If
                        End If
                    Next

                    sql = "UPDATE [" & tbl & "] SET " & fields & " WHERE [" & fld & "] LIKE '" & rs1(fld).ToString & "';"
                    cmd = New SqlClient.SqlCommand(sql, cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    cmd.ExecuteNonQuery()
                End If

                firstRun = False
            Loop
            cmd1.Cancel()
            rs1.Close()

            ' delete duplicates
            sql = "DELETE FROM [" & tbl & "] WHERE ID NOT IN (SELECT MIN(ID) FROM [" & tbl & "] GROUP BY " & fieldNames & ")"
            cmd1 = New SqlClient.SqlCommand(sql, cn1)
            If cmd1.Connection.State = ConnectionState.Closed Then cmd1.Connection.Open()
            cmd1.ExecuteNonQuery()

        Catch ex As Exception
            Dim s As String = ex.ToString
        Finally
            cn.Close()
            cn1.Close()
        End Try
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