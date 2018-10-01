Imports System.Xml
Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Shared.Common
Imports UtilityWizards.CommonCore.Shared.Xml
Imports Telerik.Web.UI
Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser

Public Class ModuleTab
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property ModId As Integer
        Get
            Return Request.QueryString("modid").ToInteger
        End Get
    End Property
    Private Property RecordId As Integer
        Get
            If Me.txtID.Text.ToInteger = 0 Then Me.txtID.Text = Request.QueryString("id").ToInteger.ToString
            Return Me.txtID.Text.ToInteger
        End Get
        Set(value As Integer)
            Me.txtID.Text = value.ToString
        End Set
    End Property
    Private ReadOnly Property CustAcctNum As String
        Get
            Return Request.QueryString("custacctnum")
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.ModuleView1.ModuleQuestions = App.ActiveModuleQuestions
        Me.ModuleView1.CurrentModule = App.ActiveModule

        Me.ModuleView1.TopLeftTitle = App.ActiveModule.TopLeftTitle
        Me.ModuleView1.TopMiddleTitle = App.ActiveModule.TopMiddleTitle
        Me.ModuleView1.TopRightTitle = App.ActiveModule.TopRightTitle
        Me.ModuleView1.FullPageTitle = App.ActiveModule.FullPageTitle
        Me.ModuleView1.BottomLeftTitle = App.ActiveModule.BottomLeftTitle
        Me.ModuleView1.BottomMiddleTitle = App.ActiveModule.BottomMiddleTitle
        Me.ModuleView1.BottomRightTitle = App.ActiveModule.BottomRightTitle

        Me.ModuleView1.BuildQuestionList()
        'Me.LoadQuestions()

        If Not IsPostBack Then
            'Me.lblHeader.Text = GetFolderName(App.ActiveFolderID) & " > " & App.ActiveModule.Name & " Module (Work Order #" & Me.RecordId & ")"

            Me.LoadLists()

            If Me.RecordId > 0 Or Me.CustAcctNum <> "" Then
                Me.LoadData()
                Me.txtUserEmail.Text = App.CurrentUser.Email
                'Me.txtAcctNumber.Text = Me.CustAcctNum
            End If

            Me.LoadCustomerInfo()

            Me.SetupForm()
        End If

        Me.SetSkin(Me.ModuleView1, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Desktop"))
    End Sub

    Private Sub SetupForm()
        Me.pnlScrollModuleView.Visible = (Me.ModuleView1.ModuleQuestions.Count > 0)
        'Me.ddlSupervisor.Enabled = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.User Or
        '                            App.CurrentUser.IsAdminUser Or
        '                            App.CurrentUser.IsSysAdmin)
        'Me.ddlTechnician.Enabled = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Supervisor Or
        '                            App.CurrentUser.IsAdminUser Or
        '                            App.CurrentUser.IsSysAdmin)
        'Me.pnlModuleOptions.Visible = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Supervisor Or
        '                            App.CurrentUser.IsAdminUser Or
        '                            App.CurrentUser.IsSysAdmin)

        'Me.txtTechComments.Enabled = True

        'Me.pnlCustomerDetails.Visible = True
        'Me.RadTabStrip1.Tabs(0).Text = "Header &amp; Customer Details"
        'Me.RadTabStrip1.Tabs(1).Visible = True
        'Me.lnkNew.Visible = True

        'Me.RadTabStrip1.Tabs(1).Visible = (App.ActiveModule.ID <> 108)
        'Me.RadTabStrip1.Tabs(2).Visible = False
        'Me.lblAcctNum.Visible = True
        'Me.txtAcctNumber.Visible = True
        'Me.lblLocationNum.Visible = True
        'Me.txtLocationNum.Visible = True
        'Me.tbl811SignOff.Visible = False
    End Sub

    Private Sub LoadLists()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            'Me.ddlSupervisor.Items.Clear()
            'Me.ddlSupervisor.Items.Add(New RadComboBoxItem("Unassigned", "0"))
            'Dim sql As String = "SELECT [ID], [xName] FROM [vwUserInfo] WHERE [xClientID] = " & App.CurrentClient.ID & " AND ([xPermissions] IN ('" & Enums.SystemUserPermissions.Supervisor.ToString & "', '" & Enums.SystemUserPermissions.Administrator.ToString & "', '" & Enums.SystemUserPermissions.SystemAdministrator.ToString & "')) ORDER BY [xName]"
            'Dim cmd As New SqlClient.SqlCommand(sql, cn)
            'If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            'Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            'Do While rs.Read
            '    Me.ddlSupervisor.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            'Loop
            'rs.Close()
            'cmd.Cancel()
            'If App.ActiveModule.SupervisorID > 0 Then
            '    Me.ddlSupervisor.SelectedValue = App.ActiveModule.SupervisorID.ToString
            'Else
            '    Dim fldr As New SystemModule(App.ActiveFolderID)
            '    If fldr.SupervisorID > 0 Then
            '        Me.ddlSupervisor.SelectedValue = fldr.SupervisorID.ToString
            '    End If
            'End If

            'Me.LoadTechnicians()

            'Me.ddlStatus.Items.Clear()
            'Dim enumValues As Array = System.[Enum].GetValues(GetType(Enums.SystemModuleStatus))
            'For Each resource As Enums.SystemModuleStatus In enumValues
            '    Me.ddlStatus.Items.Add(New RadComboBoxItem(resource.ToString, CStr(resource)))
            'Next

            'Me.ddlPriority.Items.Clear()
            'enumValues = System.[Enum].GetValues(GetType(Enums.SystemModulePriority))
            'For Each resource As Enums.SystemModulePriority In enumValues
            '    Me.ddlPriority.Items.Add(New RadComboBoxItem(resource.ToString, CStr(resource)))
            'Next

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadTechnicians()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            'Dim sql As String = ""
            'Me.ddlTechnician.Items.Clear()
            'Me.ddlTechnician.Items.Add(New RadComboBoxItem("Unassigned", "0"))
            'sql = "SELECT [ID], [xName] FROM [vwUserInfo] WHERE [xClientID] = " & App.CurrentClient.ID & " AND [xPermissions] = '" & Enums.SystemUserPermissions.Technician.ToString & "' AND [xSupervisorID] = " & Me.ddlSupervisor.SelectedValue & " ORDER BY [xName]"
            'Dim cmd As New SqlClient.SqlCommand(sql, cn)
            'If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            'Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            'Do While rs.Read
            '    Me.ddlTechnician.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            'Loop
            'rs.Close()
            'cmd.Cancel()
            'Me.ddlTechnician.SelectedIndex = 0

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadCustomerInfo()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Dim sql As String = ""
            Dim cmd As SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader

            ' now run the query to pull the new record
            cmd = New SqlClient.SqlCommand("procGetMasterFeedRecord", cn)
            cmd.Parameters.AddWithValue("@CustAcctNum", Me.CustAcctNum)
            cmd.CommandType = CommandType.StoredProcedure
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            If rs.Read Then
                'Me.txtCustomerName.Text = rs("PRIMARY NAME").ToString
                'Me.txtCustomerServiceAddress.Text = rs("PROPERTY ADDRESS").ToString & ", " & rs("PROPERTY CITY").ToString & " " & rs("PROPERTY STATE").ToString & " " & rs("PROPERTY ZIP").ToString

                'Dim latLon As List(Of String) = AddressToLatLon(Me.txtCustomerServiceAddress.Text)
                'Me.txtCustomerServiceAddressLat.Text = latLon(0)
                'Me.txtCustomerServiceAddressLon.Text = latLon(1)

                'Dim ds As New DataSet("ServiceLocation")
                'Dim dt As New DataTable("ServiceLocationTable")
                'dt.Columns.Add("Shape", Type.[GetType]("System.String"))
                'dt.Columns.Add("Country", Type.[GetType]("System.String"))
                'dt.Columns.Add("City", Type.[GetType]("System.String"))
                'dt.Columns.Add("Address", Type.[GetType]("System.String"))
                'dt.Columns.Add("Latitude", Type.[GetType]("System.Decimal"))
                'dt.Columns.Add("Longitude", Type.[GetType]("System.Decimal"))
                'dt.Rows.Add("PinTarget", "", rs("PRIMARY NAME").ToString, rs("PROPERTY ADDRESS").ToString & "<br />" & rs("PROPERTY CITY").ToString & " " & rs("PROPERTY ZIP").ToString, latLon(0).ToDecimal, latLon(1).ToDecimal)
                'ds.Tables.Add(dt)

                'Me.RadMap1.CenterSettings.Latitude = latLon(0).ToDecimal
                'RadMap1.CenterSettings.Longitude = latLon(1).ToDecimal

                'RadMap1.DataSource = ds
                'RadMap1.DataBind()
            End If
            rs.Close()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    'Private Sub LoadQuestions()
    '    'If App.RootFolderQuestions.Count > 0 Then
    '    '    Dim folderName As String = GetFolderName(App.ActiveModule.FolderID)
    '    '    Me.LoadQuestions(App.RootFolderQuestions, tblFolderQuestions, folderName.Replace(" ", "_"))
    '    'Else
    '    '    Me.tblFolderQuestions.Visible = False
    '    'End If
    '    Me.LoadQuestions(App.ActiveModuleQuestions, "")
    'End Sub
    'Private Sub LoadQuestions(ByVal qList As List(Of SystemQuestion), ByVal xmlPath As String)
    '    LoadQuestions(qList, Nothing, xmlPath)
    'End Sub
    'Private Sub LoadQuestions(ByVal qList As List(Of SystemQuestion), ByVal tbl As Table, ByVal xmlPath As String)

    'End Sub

    Private Sub LoadData()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand()
            Dim rs As SqlClient.SqlDataReader
            Dim sql As String = ""
            cmd = New SqlClient.SqlCommand("Select [ID], [xmlData], [xUserEmail] FROM [vwModuleData] WHERE [ID] = " & Me.RecordId, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Me.pnlDataNav.Visible = False
            If rs.Read Then
                Me.FromXml(rs("xmlData").ToString)
                '    Me.litNoData.Text = ""
                'Else
                '    Me.litNoData.Text = "No Data available for this customer.<br/>"
            End If
            rs.Close()
            cmd.Cancel()

            'If Me.txtLocationNum.Text = "" Then Me.txtLocationNum.Text = Me.LocationNum

            ' load master feed values
            Me.LoadMasterFeed()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadMasterFeed()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Dim dt As New DataTable()
            Dim lstFields As New List(Of String)

            ' get a list of master fields to add
            For Each q As SystemQuestion In Me.ModuleView1.ModuleQuestions
                If q.BindingType = Enums.SystemQuestionBindingType.MasterFeed Then
                    If Not lstFields.Contains(q.MasterFeedField) Then lstFields.Add(q.MasterFeedField)
                End If
            Next

            ' get a list of fields from the formula fields
            For Each q As SystemQuestion In Me.ModuleView1.ModuleQuestions
                If q.BindingType = Enums.SystemQuestionBindingType.Formula And q.Rule <> "" Then
                    lstFields.AddRange(q.Rule.GetFieldNamesFromFormula(lstFields))
                    dt.Columns.Add(q.DataFieldName)
                End If
            Next

            ' fix the fields into a string
            Dim fields As String = ""
            For Each f As String In lstFields
                If fields = "" Then
                    fields = "[" & f & "]"
                Else fields &= ", [" & f & "]"
                End If

                dt.Columns.Add(f)
            Next

            Dim cmd As New SqlClient.SqlCommand("procGetMasterFeedRecord", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@CustAcctNum", Me.CustAcctNum)
            cmd.Parameters.AddWithValue("@Fields", fields)
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Dim r As DataRow = dt.NewRow
                For Each q As SystemQuestion In Me.ModuleView1.ModuleQuestions
                    If q.BindingType = Enums.SystemQuestionBindingType.MasterFeed Then
                        If q.DisplayAsDate And IsDate(rs(q.MasterFeedField)) Then
                            r(q.MasterFeedField) = FormatDateTime(CDate(rs(q.MasterFeedField)), vbShortDate)
                        Else r(q.MasterFeedField) = rs(q.MasterFeedField)
                        End If
                    ElseIf q.BindingType = Enums.SystemQuestionBindingType.Formula Then
                        If q.Rule <> "" Then r(q.DataFieldName) = q.Rule
                        Dim lstFormulaFields As List(Of String) = q.Rule.GetFieldNamesFromFormula
                        For Each f As String In lstFormulaFields
                            r(f) = rs(f)
                        Next
                    End If
                Next
                dt.Rows.Add(r)
            Loop
            rs.Close()
            cmd.Cancel()

            If dt.Rows.Count > 0 Then
                Me.lblTotalRecords.Text = dt.Rows.Count.ToString
                Me.pnlDataNav.Visible = (dt.Rows.Count > 1)

                Session("ImportDataTable" & Me.ModId) = dt

                lnkFirstRecord_Click(Nothing, New EventArgs)
            Else
                'Me.litNoData.Text = "No Master Feed Data available for this customer.<br/>"
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    'Private Sub UpdateFormulaValues(ByVal data As DataTable, ByVal recordNum As Integer)
    '    Dim row As DataRow = data.Rows(recordNum - 1)

    '    For Each q As SystemQuestion In Me.ModuleView1.ModuleQuestions
    '        If q.BindingType = Enums.SystemQuestionBindingType.Formula Then
    '            If q.Type = Enums.SystemQuestionType.TextBox Or q.Type = Enums.SystemQuestionType.MemoField Then
    '                ' text formulas 
    '                ' replace all the instances of the field names with their actual values
    '                If data.Rows.Count > 0 Then
    '                    For Each dc As DataColumn In data.Columns
    '                        Dim name As String = dc.ColumnName
    '                        Dim value As String = ""
    '                        If Not IsDBNull(row(name)) Then value = row(name).ToString


    '                    Next
    '                End If
    '            ElseIf q.Type = Enums.SystemQuestionType.NumericTextBox Or q.Type = Enums.SystemQuestionType.CurrencyTextBox Then
    '                ' numeric formulas process as math
    '            End If
    '        End If
    '    Next
    'End Sub

    Private Function SaveChanges() As Boolean
        Dim retVal As Boolean = True

        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            ' validate the form
            'If Me.txtTechComments.Text.Trim = "" And Not Me.RecordId = 0 And Me.ddlStatus.SelectedValue = CStr(Enums.SystemModuleStatus.Completed) Then
            '    Me.MsgBox("Technician Comments are required.")
            '    Exit Function
            'End If

            'If Me.PageErrors(wot).Count = 0 Then
            Dim xDoc As New XmlDocument()
            xDoc = Me.ToXml
            xDoc.Item("Data").AppendChild(xDoc.NewElement("ModuleID", Me.ModId.ToString))
            xDoc.Item("Data").AppendChild(xDoc.NewElement("ClientID", App.CurrentClient.ID.ToString))

            Dim cmd As New SqlClient.SqlCommand

            Dim technicianUpdated As Boolean = False
            Dim statusUpdated As Boolean = False
            If Me.RecordId > 0 Then
                cmd = New SqlClient.SqlCommand("SELECT [xmlData] FROM [ModuleData] WHERE [ID] = " & Me.RecordId & ";", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Dim xD As New XmlDocument
                    xD.LoadXml(rs("xmlData").ToString)

                    'If xD.Item("Data").Item("Status") IsNot Nothing Then statusUpdated = (xD.Item("Data").Item("Status").InnerText.ToInteger <> Me.ddlStatus.SelectedValue.ToInteger)
                    'If xD.Item("Data").Item("Technician") IsNot Nothing Then technicianUpdated = (xD.Item("Data").Item("Technician").InnerText.ToInteger <> Me.ddlTechnician.SelectedValue.ToInteger)
                End If
            End If

            Dim newRecord As Boolean = False
            'If Me.NeedToSave Or newRecord Then

            ' grab the trash can serials for Wadesboro
            Dim trashCanSerial1 As String = ""
            Dim trashCanSerial2 As String = ""
            Dim trashCanSerial3 As String = ""
            If App.CurrentClient.ID = 4 And App.ActiveModule.ID = 78 Then
                trashCanSerial1 = xDoc.GetNodeValue("SerialNumber1")
                trashCanSerial2 = xDoc.GetNodeValue("SerialNumber2")
                trashCanSerial3 = xDoc.GetNodeValue("SerialNumber3")
            End If

            ' update the location record with the trash can serials for Wadesboro
            If App.CurrentClient.ID = 4 And App.ActiveModule.ID = 78 Then
                cmd = New SqlClient.SqlCommand("UPDATE [Locations_new] SET [Serial001] = '" & trashCanSerial1 & "', [Serial002] = '" & trashCanSerial2 & "', [Serial003] = '" & trashCanSerial3 & "' WHERE [LocationNum] LIKE '" & xDoc.GetNodeValue("LocationNum") & "';", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            End If

            If Me.RecordId = 0 Then
                xDoc.Item("Data").Item("ViewedOnMobile").InnerText = "0"

                newRecord = True
                Dim newRecordId As Integer = 0

                cmd = New SqlClient.SqlCommand("INSERT INTO [ModuleData] ([xmlData], [dtInserted], [dtUpdated], [insertedBy], [updatedBy], [Active]) VALUES(@xmlData,'" & Now.ToString & "', '" & Now.ToString & "', '" & App.CurrentUser.ID & "', '" & App.CurrentUser.ID & "', '1');SELECT @@Identity AS SCOPEIDENTITY;", cn)
                cmd.Parameters.AddWithValue("@xmlData", xDoc.ToXmlString)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader()
                If rs.Read Then
                    newRecordId = CInt(rs("SCOPEIDENTITY"))
                End If
                cmd.Cancel()
                rs.Close()

                Me.RecordId = newRecordId
            Else
                cmd = New SqlClient.SqlCommand("UPDATE [ModuleData] SET [xmlData] = @xmlData, [dtUpdated] = '" & Now.ToString & "', [updatedBy] = '" & App.CurrentUser.ID & "' WHERE ID = " & Me.RecordId, cn)
                cmd.Parameters.AddWithValue("@xmlData", xDoc.ToXmlString)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Cancel()
            End If

            cmd = New SqlClient.SqlCommand("EXEC [procRefreshModuleDataIDs]", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Cancel()

            If newRecord Then
                technicianUpdated = True

                LogHistory("New Work Order #" & Me.RecordId & " Created", App.CurrentUser.ID, App.UseSandboxDb)

                Dim assigned As String = "with no technician "
                Dim txt As String = ""

                ' text the technician only if it has been assigned
                'If technicianUpdated And Me.ddlTechnician.SelectedValue.ToInteger > 0 Then
                '    txt = App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been created and assigned to you as the technician."
                '    CommonCore.Shared.Messaging.SendTwilioNotification(New SystemUser(Me.ddlTechnician.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)
                '    assigned = ""
                'End If

                ' text the supervisor that a work order has been created
                'If CType(Me.ddlPriority.SelectedValue, Global.UtilityWizards.CommonCore.Shared.Enums.SystemModulePriority) = Enums.SystemModulePriority.Normal Then
                '    txt = App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been created " & assigned & "and assigned to you as the supervisor."
                '    CommonCore.Shared.Messaging.SendTwilioNotification(New SystemUser(Me.ddlSupervisor.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)
                'ElseIf CType(Me.ddlPriority.SelectedValue, Global.UtilityWizards.CommonCore.Shared.Enums.SystemModulePriority) = Enums.SystemModulePriority.Emergency Then
                '    txt = assigned & "EMERGENCY " & App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been created " & assigned & "and assigned to you as the supervisor."
                '    CommonCore.Shared.Messaging.SendTwilioNotification(New SystemUser(Me.ddlSupervisor.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)
                'End If

                ' email administrators if the notify box is checked
                'If Me.chkNotifyAdmins.Checked Then
                '    Dim fName As String = Me.PrintPdf("window")
                '    fName = fName.Replace("..", "~")

                '    ' add the supervisor name to the the text
                '    txt = txt.Replace(" you ", " " & Me.ddlSupervisor.SelectedItem.Text & " ")

                '    cmd = New SqlClient.SqlCommand("SELECT [xEmail] FROM [vwUserInfo] WHERE [xClientID] = " & App.CurrentClient.ID & " AND [xPermissions] LIKE '%administrator%'", cn)
                '    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                '    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                '    Do While rs.Read
                '        ' email all administrators that the work order has been created
                '        Dim msg As New Mailer
                '        msg.HostServer = AppSettings("MailHost")
                '        msg.UserName = AppSettings("MailUser")
                '        msg.Password = AppSettings("MailPassword")
                '        If Me.OnLocal Then
                '            msg.To.Add("james@solvtopia.com")
                '        Else
                '            msg.To.Add(rs("xEmail").ToString)
                '            msg.BCC.Add("james@solvtopia.com")
                '        End If
                '        msg.Body = "<html>" & txt & "</html>"
                '        msg.Subject = "Utility Wizards Work Order Created"
                '        msg.From = "noreply@utilitywizards.com"
                '        msg.HtmlBody = True
                '        msg.Attachments.Add(Server.MapPath(fName))
                '        msg.Send()
                '    Loop
                '    cmd.Cancel()
                '    rs.Close()
                'End If

            Else
                ' text the supervisor if the status has been changed
                If statusUpdated Then
                    'LogHistory("Work Order #" & Me.RecordId & " Status Updated To " & Me.ddlStatus.SelectedItem.Text, App.CurrentUser.ID)

                    'Dim txt As String = "The status for " & App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been updated to " & Me.ddlStatus.SelectedItem.Text & "."
                    'CommonCore.Shared.Messaging.SendTwilioNotification(New SystemUser(Me.ddlSupervisor.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)

                    ' email administrators if the notify box is checked
                    'If Me.chkNotifyAdmins.Checked Then
                    '    Dim fName As String = Me.PrintPdf("window")
                    '    fName = fName.Replace("..", "~")

                    '    cmd = New SqlClient.SqlCommand("SELECT [xEmail] FROM [vwUserInfo] WHERE [xClientID] = " & App.CurrentClient.ID & " AND [xPermissions] LIKE '%administrator%'", cn)
                    '    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    '    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    '    Do While rs.Read
                    '        ' email all administrators that the work order has been created
                    '        Dim msg As New Mailer
                    '        msg.HostServer = AppSettings("MailHost")
                    '        msg.UserName = AppSettings("MailUser")
                    '        msg.Password = AppSettings("MailPassword")
                    '        If Me.OnLocal Then
                    '            msg.To.Add("james@solvtopia.com")
                    '        Else
                    '            msg.To.Add(rs("xEmail").ToString)
                    '            msg.BCC.Add("james@solvtopia.com")
                    '        End If
                    '        msg.Body = "<html>" & txt & "</html>"
                    '        msg.Subject = "Utility Wizards Work Order Updated"
                    '        msg.From = "noreply@utilitywizards.com"
                    '        msg.HtmlBody = True
                    '        msg.Attachments.Add(Server.MapPath(fName))
                    '        msg.Send()
                    '    Loop
                    '    cmd.Cancel()
                    '    rs.Close()
                    'End If
                End If

                ' text the technician if it has been assigned
                'If technicianUpdated And Me.ddlTechnician.SelectedValue.ToInteger > 0 Then
                '    LogHistory("Work Order #" & Me.RecordId & " Assigned To " & Me.ddlTechnician.SelectedItem.Text, App.CurrentUser.ID)

                '    Dim txt As String = App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been assigned to you."
                '    CommonCore.Shared.Messaging.SendTwilioNotification(New SystemUser(Me.ddlTechnician.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)
                'End If
            End If

            '    Me.NeedToSave = False
            'Else
            '    retVal = False
            '    Dim msg As String = "Please provide values for the following questions to continue:" & vbCrLf & vbCrLf
            '    For Each s As String In Me.PageErrors(wot)
            '        msg &= s & vbCrLf
            '    Next
            '    Me.MsgBox(msg)
            'End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
            retVal = False
        Finally
            cn.Close()
        End Try

        Return retVal
    End Function

    Private Sub ClearForm()

    End Sub

    Protected Sub TextBox_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub CheckBox_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub DropDownList_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)

    End Sub

    'Private Sub lnkNew_Click(sender As Object, e As EventArgs) Handles lnkNew.Click
    '    Response.Redirect("~/account/ModuleLandingPage.aspx?t=" & CStr(Enums.TransactionType.[New]) & "&modid=" & Me.ModId, False)
    'End Sub

    'Private Sub lnkCopyModule_Click(sender As Object, e As EventArgs) Handles lnkCopyModule.Click

    'End Sub

    'Private Sub lnkDeleteModule_Click(sender As Object, e As EventArgs) Handles lnkDeleteModule.Click
    '    ShowInformationPopup(Enums.InformationPopupType.DeleteModule, Enums.InformationPopupButtons.YesNo, Me.ModId)
    'End Sub

    'Private Sub lnkEditModule_Click(sender As Object, e As EventArgs) Handles lnkEditModule.Click
    '    Response.Redirect("~/account/ModuleWizard.aspx?id=" & Me.ModId & "&fid=" & App.ActiveFolderID & "&t=" & CStr(Enums.SystemModuleType.Module), False)
    'End Sub

    'Private Sub lnkMoveModule_Click(sender As Object, e As EventArgs) Handles lnkMoveModule.Click
    '    ShowInformationPopup(Enums.InformationPopupType.MoveModule, Enums.InformationPopupButtons.OkCancel, Me.ModId)
    'End Sub

    'Private Sub lnkPrint_Click(sender As Object, e As EventArgs) Handles lnkPrint.Click
    '    Dim fName As String = Me.PrintPdf("window")
    '    'Response.Redirect("~/account/PrintPreview.aspx?modid=" & Me.ModId & "&id=" & Me.RecordId, False)
    '    Me.RunClientScript("window.open('" & fName & "','_blank')")
    'End Sub

    Private Function PrintPdf(ByVal dest As String) As String
        Dim retVal As String = ""

        Dim HTMLContent As String = ScrapeUrl(Left(Request.Url.OriginalString, Request.Url.OriginalString.Length - Request.Url.PathAndQuery.Length) & "/account/Print.aspx?modid=" & Me.ModId & "&id=" & Me.RecordId & "&usr=" & App.CurrentUser.ID, Enums.ScrapeType.KeepTags)
        Dim ba() As Byte = GetPDF(HTMLContent)

        Select Case dest
            Case "download"
                ' download the pdf
                Response.Clear()
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf")
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.BinaryWrite(ba)
                Response.[End]()

            Case "window", "attachment"
                ' save the file
                Dim fName As String = "WO_" & Me.RecordId & "_" & Now.ToString.Replace("/", "").Replace(":", "").Replace("AM", "").Replace("PM", "").Replace(" ", "") & ".pdf"
                If My.Computer.FileSystem.FileExists(Server.MapPath("~/temp/" & fName)) Then My.Computer.FileSystem.DeleteFile(Server.MapPath("~/temp/" & fName))
                File.WriteAllBytes(Server.MapPath("~/temp/" & fName), ba)
                retVal = "../temp/" & fName
        End Select

        Return retVal
    End Function

    Private Function GetPDF(pHTML As String) As Byte()
        Dim bPDF As Byte() = Nothing

        Dim ms As New MemoryStream()
        Dim txtReader As TextReader = New StringReader(pHTML)

        ' 1: create object of a itextsharp document class
        Dim doc As New Document(PageSize.A4, 25, 25, 25, 25)

        ' 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
        Dim oPdfWriter As PdfWriter = PdfWriter.GetInstance(doc, ms)

        ' 3: we create a worker parse the document
        Dim htmlWorker As New HTMLWorker(doc)

        ' 4: we open document and start the worker on the document
        doc.Open()
        htmlWorker.StartDocument()

        ' 5: parse the html into the document
        htmlWorker.Parse(txtReader)

        ' 6: close the document and the worker
        htmlWorker.EndDocument()
        htmlWorker.Close()
        doc.Close()

        bPDF = ms.ToArray()

        Return bPDF
    End Function

    Protected Sub lnkFirstRecord_Click(sender As Object, e As EventArgs) Handles lnkFirstRecord.Click
        Dim data As DataTable = CType(Session("ImportDataTable" & Me.ModId), DataTable)
        Me.txtRecordNum.Text = "1"
        Me.FromDataTable(data, Me.txtRecordNum.Text.ToInteger)
        'Me.UpdateFormulaValues(data, Me.txtRecordNum.Text.ToInteger)
    End Sub

    Protected Sub lnkPreviousRecord_Click(sender As Object, e As EventArgs) Handles lnkPreviousRecord.Click
        If Me.txtRecordNum.Text.ToInteger > 1 Then
            Dim data As DataTable = CType(Session("ImportDataTable" & Me.ModId), DataTable)
            Me.txtRecordNum.Text = (Me.txtRecordNum.Text.ToInteger - 1).ToString
            Me.FromDataTable(data, Me.txtRecordNum.Text.ToInteger)
            'Me.UpdateFormulaValues(data, Me.txtRecordNum.Text.ToInteger)
        End If
    End Sub

    Protected Sub lnkNextRecord_Click(sender As Object, e As EventArgs) Handles lnkNextRecord.Click
        If Me.txtRecordNum.Text.ToInteger < Me.lblTotalRecords.Text.ToInteger Then
            Dim data As DataTable = CType(Session("ImportDataTable" & Me.ModId), DataTable)
            Me.txtRecordNum.Text = (Me.txtRecordNum.Text.ToInteger + 1).ToString
            Me.FromDataTable(data, Me.txtRecordNum.Text.ToInteger)
            'Me.UpdateFormulaValues(data, Me.txtRecordNum.Text.ToInteger)
        End If
    End Sub

    Protected Sub lnkLastRecord_Click(sender As Object, e As EventArgs) Handles lnkLastRecord.Click
        Dim data As DataTable = CType(Session("ImportDataTable" & Me.ModId), DataTable)
        Me.txtRecordNum.Text = Me.lblTotalRecords.Text
        Me.FromDataTable(data, Me.txtRecordNum.Text.ToInteger)
        'Me.UpdateFormulaValues(data, Me.txtRecordNum.Text.ToInteger)
    End Sub


    'Private Sub lnkReset_Click(sender As Object, e As EventArgs) Handles lnkReset.Click
    '    Me.ClearForm()
    'End Sub

    'Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click
    '    If Me.SaveChanges Then
    '        Me.MsgBox("Your changes have been saved.")
    '    Else Me.MsgBox("We were unable to save your changes.")
    '    End If
    'End Sub

    'Private Sub lnkSaveHome_Click(sender As Object, e As EventArgs) Handles lnkSaveHome.Click
    '    If Me.SaveChanges Then
    '        Response.Redirect("~/Default.aspx", False)
    '        Exit Sub
    '    Else Me.MsgBox("We were unable to save your changes.")
    '    End If
    'End Sub

    'Private Sub lnkSearch_Click(sender As Object, e As EventArgs) Handles lnkSearch.Click
    '    Response.Redirect("~/account/ModuleLandingPage.aspx?t=" & CStr(Enums.TransactionType.Existing) & "&modid=" & Me.ModId, False)
    'End Sub

    'Private Sub ddlSupervisor_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlSupervisor.SelectedIndexChanged
    '    Me.LoadTechnicians()
    'End Sub

    'Private Sub ddlPriority_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlPriority.SelectedIndexChanged
    '    If Me.ddlPriority.SelectedValue = CStr(Enums.SystemModulePriority.Emergency) Or Me.ddlPriority.SelectedValue = CStr(Enums.SystemModulePriority.High) Then
    '        Me.ddlStatus.SelectedValue = CStr(Enums.SystemModuleStatus.Pending)
    '    End If
    'End Sub
End Class