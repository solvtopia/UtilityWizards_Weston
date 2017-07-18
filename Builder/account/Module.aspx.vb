Imports System.Xml
Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Xml
Imports Telerik.Web.UI

Public Class _Module
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
    Private ReadOnly Property LocationNum As String
        Get
            Return Request.QueryString("locationnum")
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LoadQuestions()

        If Not IsPostBack Then
            Me.lblHeader.Text = GetFolderName(App.ActiveFolderID) & " > " & App.ActiveModule.Name & " Module (Work Order #" & Me.RecordId & ")"

            Me.LoadLists()

            If Me.RecordId > 0 Then
                Me.LoadData()
            Else
                Me.txtUserEmail.Text = App.CurrentUser.Email
                Me.txtAcctNumber.Text = Me.CustAcctNum
                Me.txtLocationNum.Text = Me.LocationNum
            End If

            Me.LoadCustomerInfo()

            Me.SetupForm()
        End If
    End Sub

    Private Sub SetupForm()
        Me.ddlSupervisor.Enabled = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.User Or
                                    App.CurrentUser.IsAdminUser Or
                                    App.CurrentUser.IsSysAdmin)
        Me.ddlTechnician.Enabled = (App.CurrentUser.Permissions = Enums.SystemUserPermissions.Supervisor Or
                                    App.CurrentUser.IsAdminUser Or
                                    App.CurrentUser.IsSysAdmin)

        Me.txtTechComments.Enabled = True
    End Sub

    Private Sub LoadLists()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Me.ddlSupervisor.Items.Clear()
            Me.ddlSupervisor.Items.Add(New RadComboBoxItem("Unassigned", "0"))
            Dim sql As String = "SELECT [ID], [xName] FROM [vwUserInfo] WHERE [xClientID] = " & App.CurrentClient.ID & " AND ([xPermissions] IN ('" & Enums.SystemUserPermissions.Supervisor.ToString & "', '" & Enums.SystemUserPermissions.Administrator.ToString & "', '" & Enums.SystemUserPermissions.SystemAdministrator.ToString & "')) ORDER BY [xName]"
            Dim cmd As New SqlClient.SqlCommand(sql, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlSupervisor.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            rs.Close()
            cmd.Cancel()
            If App.ActiveModule.SupervisorID > 0 Then
                Me.ddlSupervisor.SelectedValue = App.ActiveModule.SupervisorID.ToString
            Else
                Dim fldr As New SystemModule(App.ActiveFolderID)
                If fldr.SupervisorID > 0 Then
                    Me.ddlSupervisor.SelectedValue = fldr.SupervisorID.ToString
                End If
            End If

            Me.LoadTechnicians()

            Me.ddlStatus.Items.Clear()
            Dim enumValues As Array = System.[Enum].GetValues(GetType(Enums.SystemModuleStatus))
            For Each resource As Enums.SystemModuleStatus In enumValues
                Me.ddlStatus.Items.Add(New RadComboBoxItem(resource.ToString, CStr(resource)))
            Next

            Me.ddlPriority.Items.Clear()
            enumValues = System.[Enum].GetValues(GetType(Enums.SystemModulePriority))
            For Each resource As Enums.SystemModulePriority In enumValues
                Me.ddlPriority.Items.Add(New RadComboBoxItem(resource.ToString, CStr(resource)))
            Next

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadTechnicians()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim sql As String = ""
            Me.ddlTechnician.Items.Clear()
            Me.ddlTechnician.Items.Add(New RadComboBoxItem("Unassigned", "0"))
            sql = "SELECT [ID], [xName] FROM [vwUserInfo] WHERE [xClientID] = " & App.CurrentClient.ID & " AND [xPermissions] = '" & Enums.SystemUserPermissions.Technician.ToString & "' AND [xSupervisorID] = " & Me.ddlSupervisor.SelectedValue & " ORDER BY [xName]"
            Dim cmd As New SqlClient.SqlCommand(sql, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlTechnician.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            rs.Close()
            cmd.Cancel()
            Me.ddlTechnician.SelectedIndex = 0

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadCustomerInfo()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim sql As String = "SELECT [FullName], [ServiceAddress], [City], [State], [ZipCode] FROM [vwCustomerSearch] WHERE [Account] LIKE '" & Me.txtAcctNumber.Text & "';"
            If Me.CustAcctNum = "" Then
                sql = "SELECT [FullName], [ServiceAddress], [City], [State], [ZipCode] FROM [vwCustomerSearch] WHERE [xLocationID] LIKE '" & Me.LocationNum & "';"
            End If
            Dim cmd As New SqlClient.SqlCommand(sql, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.txtCustomerName.Text = rs("FullName").ToString
                Me.txtCustomerServiceAddress.Text = rs("ServiceAddress").ToString & ", " & rs("City").ToString

                Dim latLon As List(Of String) = AddressToLatLon(rs("ServiceAddress").ToString & " " & rs("City").ToString & " " & rs("State").ToString & " " & rs("ZipCode").ToString.Substring(0, 5))
                Me.txtCustomerServiceAddressLat.Text = latLon(0)
                Me.txtCustomerServiceAddressLon.Text = latLon(1)

                Dim ds As New DataSet("ServiceLocation")
                Dim dt As New DataTable("ServiceLocationTable")
                dt.Columns.Add("Shape", Type.[GetType]("System.String"))
                dt.Columns.Add("Country", Type.[GetType]("System.String"))
                dt.Columns.Add("City", Type.[GetType]("System.String"))
                dt.Columns.Add("Address", Type.[GetType]("System.String"))
                dt.Columns.Add("Latitude", Type.[GetType]("System.Decimal"))
                dt.Columns.Add("Longitude", Type.[GetType]("System.Decimal"))
                dt.Rows.Add("PinTarget", "", rs("FullName").ToString, rs("ServiceAddress").ToString & "<br />" & rs("City").ToString & " " & rs("ZipCode").ToString, latLon(0).ToDecimal, latLon(1).ToDecimal)
                ds.Tables.Add(dt)

                Me.RadMap1.CenterSettings.Latitude = latLon(0).ToDecimal
                RadMap1.CenterSettings.Longitude = latLon(1).ToDecimal

                RadMap1.DataSource = ds
                RadMap1.DataBind()
            End If
            rs.Close()
            cmd.Cancel()

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadQuestions()
        If App.RootFolderQuestions.Count > 0 Then
            Dim folderName As String = GetFolderName(App.ActiveModule.FolderID)
            Me.LoadQuestions(App.RootFolderQuestions, tblFolderQuestions, folderName.Replace(" ", "_"))
        Else
            Me.tblFolderQuestions.Visible = False
        End If
        Me.LoadQuestions(App.ActiveModuleQuestions, tblModuleQuestions, "")
    End Sub
    Private Sub LoadQuestions(ByVal qList As List(Of SystemQuestion), ByVal tbl As Table, ByVal xmlPath As String)
        tbl.Rows.Clear()

        If qList.Count = 0 Then
            Dim tr As New TableRow
            Dim tc1 As New TableCell
            tc1.Text = "There are no questions To show In this view."
            tc1.VerticalAlign = VerticalAlign.Top
            tr.Cells.Add(tc1)
            tbl.Rows.Add(tr)
        Else
            For Each q In qList
                Dim tr1 As New TableRow
                Dim tc1 As New TableCell
                tc1.Text = q.Question.XmlDecode
                tc1.VerticalAlign = VerticalAlign.Top
                tr1.Cells.Add(tc1)
                tbl.Rows.Add(tr1)

                Dim tr2 As New TableRow
                Dim tc2 As New TableCell
                Select Case q.Type
                    Case Enums.SystemQuestionType.CheckBox
                        Dim chk As New Controls.CheckBoxes.CheckBox
                        chk.ID = "chk_" & q.ID
                        chk.Required = q.Required
                        chk.XmlPath = xmlPath
                        chk.DataFieldName = q.DataFieldName
                        chk.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler chk.CheckedChanged, AddressOf CheckBox_CheckedChanged
                        tc2.Controls.Add(chk)

                    Case Enums.SystemQuestionType.DropDownList
                        Dim ddl As New Controls.DropDownLists.DropDownList
                        ddl.ID = "ddl_" & q.ID
                        ddl.Width = New Unit(100, UnitType.Percentage)
                        For Each itm As String In q.Values
                            ddl.Items.Add(New RadComboBoxItem(itm, itm))
                        Next
                        ddl.Required = q.Required
                        ddl.XmlPath = xmlPath
                        ddl.DataFieldName = q.DataFieldName
                        ddl.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler ddl.SelectedIndexChanged, AddressOf DropDownList_SelectedIndexChanged
                        tc2.Controls.Add(ddl)

                    Case Enums.SystemQuestionType.MemoField
                        Dim txt As New Controls.TextBoxes.TextBox
                        txt.ID = "txt_" & q.ID
                        txt.Width = New Unit(100, UnitType.Percentage)
                        txt.Rows = 3
                        txt.TextMode = InputMode.MultiLine
                        txt.Required = q.Required
                        txt.XmlPath = xmlPath
                        txt.DataFieldName = q.DataFieldName
                        txt.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler txt.TextChanged, AddressOf TextBox_TextChanged
                        tc2.Controls.Add(txt)

                    Case Enums.SystemQuestionType.TextBox
                        Dim txt As New Controls.TextBoxes.TextBox
                        txt.ID = "txt_" & q.ID
                        txt.Width = New Unit(100, UnitType.Percentage)
                        txt.MaxLength = 255
                        txt.Required = q.Required
                        txt.XmlPath = xmlPath
                        txt.DataFieldName = q.DataFieldName
                        txt.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler txt.TextChanged, AddressOf TextBox_TextChanged
                        tc2.Controls.Add(txt)

                    Case Enums.SystemQuestionType.NumericTextBox
                        Dim txt As New Controls.TextBoxes.NumericTextBox
                        txt.ID = "txt_" & q.ID
                        txt.Width = New Unit(100, UnitType.Percentage)
                        txt.Required = q.Required
                        txt.XmlPath = xmlPath
                        txt.DataFieldName = q.DataFieldName
                        txt.NumberFormat.DecimalDigits = 0
                        txt.AutoPostBack = (q.Rule <> "")
                        If q.Rule <> "" Then AddHandler txt.TextChanged, AddressOf TextBox_TextChanged
                        tc2.Controls.Add(txt)
                End Select
                tc2.VerticalAlign = VerticalAlign.Top
                tr2.Cells.Add(tc2)

                tbl.Rows.Add(tr2)
            Next
        End If

        If Me.OnMobile Then
            Me.SetSkin(tbl, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Mobile"))
        Else Me.SetSkin(tbl, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Desktop"))
        End If
    End Sub

    Private Sub LoadData()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("Select [ID], [xmlData], [xUserEmail] FROM [vwModuleData] WHERE [ID] = " & Me.RecordId, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.FromXml(rs("xmlData").ToString)
            End If
            rs.Close()
            cmd.Cancel()

            If Me.txtLocationNum.Text = "" Then Me.txtLocationNum.Text = Me.LocationNum

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            cn.Close()
        End Try
    End Sub

    Private Function SaveChanges() As Boolean
        Dim retVal As Boolean = True

        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
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

                    If xD.Item("Data").Item("Status") IsNot Nothing Then statusUpdated = (xD.Item("Data").Item("Status").InnerText.ToInteger <> Me.ddlStatus.SelectedValue.ToInteger)
                    If xD.Item("Data").Item("Technician") IsNot Nothing Then technicianUpdated = (xD.Item("Data").Item("Technician").InnerText.ToInteger <> Me.ddlTechnician.SelectedValue.ToInteger)
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
                Dim xLocDoc As New System.Xml.XmlDocument
                cmd = New SqlClient.SqlCommand("SELECT [xmlData] FROM [Locations] WHERE [xLocationID] Like '" & xDoc.GetNodeValue("LocationNum") & "';", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    xLocDoc.LoadXml(rs("xmlData").ToString)
                End If
                rs.Close()
                cmd.Cancel()

                If xLocDoc.Item("Data").Item("Serial01") Is Nothing Then
                    xLocDoc.Item("Data").AppendChild(xLocDoc.NewElement("Serial01", trashCanSerial1.XmlEncode))
                Else xLocDoc.Item("Data").Item("Serial01").InnerText = trashCanSerial1.XmlEncode
                End If

                If xLocDoc.Item("Data").Item("Serial02") Is Nothing Then
                    xLocDoc.Item("Data").AppendChild(xLocDoc.NewElement("Serial02", trashCanSerial2.XmlEncode))
                Else xLocDoc.Item("Data").Item("Serial02").InnerText = trashCanSerial2.XmlEncode
                End If

                If xLocDoc.Item("Data").Item("Serial03") Is Nothing Then
                    xLocDoc.Item("Data").AppendChild(xLocDoc.NewElement("Serial03", trashCanSerial3.XmlEncode))
                Else xLocDoc.Item("Data").Item("Serial03").InnerText = trashCanSerial3.XmlEncode
                End If

                cmd = New SqlClient.SqlCommand("UPDATE [Locations] SET [xmlData] = @xmlData WHERE [xLocationID] LIKE '" & xDoc.GetNodeValue("LocationNum") & "';", cn)
                cmd.Parameters.AddWithValue("@xmlData", xLocDoc.ToXmlString)
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

                Common.LogHistory("New Work Order #" & Me.RecordId & " Created")

                Dim assigned As String = "with no technician "
                ' text the technician only if it has been assigned
                If technicianUpdated And Me.ddlTechnician.SelectedValue.ToInteger > 0 Then
                    Dim txt As String = App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been created and assigned to you as the technician."
                    CommonCore.Messaging.SendTwilioNotification(New SystemUser(Me.ddlTechnician.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)
                    assigned = ""
                End If

                ' text the supervisor that a work order has been created
                If CType(Me.ddlPriority.SelectedValue, Global.UtilityWizards.CommonCore.Enums.SystemModulePriority) = Enums.SystemModulePriority.Normal Then
                    Dim txt As String = App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been created " & assigned & "and assigned to you as the supervisor."
                    CommonCore.Messaging.SendTwilioNotification(New SystemUser(Me.ddlSupervisor.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)
                ElseIf CType(Me.ddlPriority.SelectedValue, Global.UtilityWizards.CommonCore.Enums.SystemModulePriority) = Enums.SystemModulePriority.Emergency Then
                    Dim txt As String = assigned & "EMERGENCY " & App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been created " & assigned & "and assigned to you as the supervisor."
                    CommonCore.Messaging.SendTwilioNotification(New SystemUser(Me.ddlSupervisor.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)
                End If

            Else
                Common.LogHistory("Work Order #" & Me.RecordId & " Updated")

                ' text the supervisor if the status has been changed
                If statusUpdated Then
                    Dim txt As String = "The status for " & App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been updated to " & Me.ddlStatus.SelectedItem.Text & "."
                    CommonCore.Messaging.SendTwilioNotification(New SystemUser(Me.ddlSupervisor.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)
                End If

                ' text the technician if it has been assigned
                If technicianUpdated And Me.ddlTechnician.SelectedValue.ToInteger > 0 Then
                    Dim txt As String = App.ActiveModule.Name & " Work Order #" & Me.RecordId & " has been assigned to you."
                    CommonCore.Messaging.SendTwilioNotification(New SystemUser(Me.ddlTechnician.SelectedValue.ToInteger), Enums.NotificationType.Custom, txt, Me.RecordId)
                End If
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
            ex.WriteToErrorLog()
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

    Private Sub lnkNew_Click(sender As Object, e As EventArgs) Handles lnkNew.Click
        Response.Redirect("~/account/ModuleLandingPage.aspx?t=" & CStr(Enums.TransactionType.[New]) & "&modid=" & Me.ModId, False)
    End Sub

    Private Sub lnkCopyModule_Click(sender As Object, e As EventArgs) Handles lnkCopyModule.Click

    End Sub

    Private Sub lnkDeleteModule_Click(sender As Object, e As EventArgs) Handles lnkDeleteModule.Click
        ShowInformationPopup(Enums.InformationPopupType.DeleteModule, Enums.InformationPopupButtons.YesNo, Me.ModId)
    End Sub

    Private Sub lnkEditModule_Click(sender As Object, e As EventArgs) Handles lnkEditModule.Click
        Response.Redirect("~/account/ModuleWizard.aspx?id=" & Me.ModId & "&fid=" & App.ActiveFolderID & "&t=" & CStr(Enums.SystemModuleType.Module), False)
    End Sub

    Private Sub lnkMoveModule_Click(sender As Object, e As EventArgs) Handles lnkMoveModule.Click
        ShowInformationPopup(Enums.InformationPopupType.MoveModule, Enums.InformationPopupButtons.OkCancel, Me.ModId)
    End Sub

    Private Sub lnkPrint_Click(sender As Object, e As EventArgs) Handles lnkPrint.Click
        Response.Redirect("~/account/Print.aspx?modid=" & Me.ModId & "&id=", False)
    End Sub

    Private Sub lnkReset_Click(sender As Object, e As EventArgs) Handles lnkReset.Click
        Me.ClearForm()
    End Sub

    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click
        If Me.SaveChanges Then
            Me.MsgBox("Your changes have been saved.")
        Else Me.MsgBox("We were unable to save your changes.")
        End If
    End Sub

    Private Sub lnkSaveHome_Click(sender As Object, e As EventArgs) Handles lnkSaveHome.Click
        If Me.SaveChanges Then
            Response.Redirect("~/Default.aspx", False)
            Exit Sub
        Else Me.MsgBox("We were unable to save your changes.")
        End If
    End Sub

    Private Sub lnkSearch_Click(sender As Object, e As EventArgs) Handles lnkSearch.Click
        Response.Redirect("~/account/ModuleLandingPage.aspx?t=" & CStr(Enums.TransactionType.Existing) & "&modid=" & Me.ModId, False)
    End Sub

    Private Sub ddlSupervisor_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlSupervisor.SelectedIndexChanged
        Me.LoadTechnicians()
    End Sub

    Private Sub ddlPriority_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlPriority.SelectedIndexChanged
        If Me.ddlPriority.SelectedValue = CStr(Enums.SystemModulePriority.Emergency) Or Me.ddlPriority.SelectedValue = CStr(Enums.SystemModulePriority.High) Then
            Me.ddlStatus.SelectedValue = CStr(Enums.SystemModuleStatus.Pending)
        End If
    End Sub
End Class