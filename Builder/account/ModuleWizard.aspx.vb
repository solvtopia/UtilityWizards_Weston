Imports System.Xml
Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Shared.Common
Imports Telerik.Web.UI

Public Class ModuleWizard
    Inherits builderPage

#Region "Properties"

    Dim maxSteps As Integer = 6
    Dim wizardMaxSteps As Integer = 3
    Private ReadOnly Property EditId As Integer
        Get
            Return Request.QueryString("id").ToInteger
        End Get
    End Property
    Private ReadOnly Property FolderId As Integer
        Get
            Return Request.QueryString("fid").ToInteger
        End Get
    End Property
    Private ReadOnly Property Type As Enums.SystemModuleType
        Get
            Return CType(Request.QueryString("t"), Enums.SystemModuleType)
        End Get
    End Property
    Private Property WizardStep As Integer
        Get
            Dim retVal As Integer = 0

            For x As Integer = 1 To maxSteps
                If CType(Me.FindInControl("pnlStep" & x), Panel).Visible Then
                    retVal = x
                    Exit For
                End If
            Next

            Return retVal
        End Get
        Set(value As Integer)
            For x As Integer = 1 To maxSteps
                If x <> value Then
                    CType(Me.FindInControl("pnlStep" & x), Panel).Visible = False
                    CType(Me.FindInControl("pnlStepInfo" & x), Panel).Visible = False
                Else
                    CType(Me.FindInControl("pnlStep" & x), Panel).Visible = True
                    CType(Me.FindInControl("pnlStepInfo" & x), Panel).Visible = True

                    If value < wizardMaxSteps Then
                        Me.btnNext.Text = "Next"
                        Me.btnBack.Visible = (value > 1)
                    Else
                        Me.btnNext.Visible = True
                        Me.btnNext.Text = "Finish"
                    End If
                End If
            Next
        End Set
    End Property
    Private Property ModuleQuestions As List(Of SystemQuestion)
        Get
            Dim retVal As New List(Of SystemQuestion)

            If Session("ModuleWizardQuestions") Is Nothing Then Session("ModuleWizardQuestions") = New List(Of SystemQuestion)

            retVal = CType(Session("ModuleWizardQuestions"), List(Of SystemQuestion))

            Return retVal
        End Get
        Set(value As List(Of SystemQuestion))
            Session("ModuleWizardQuestions") = value
        End Set
    End Property
    Private currentModule As SystemModule

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("ModuleWizardQuestions") = Nothing

            Me.WizardStep = 1
            Me.LoadLists()
            Me.txtName.Focus()

            If Me.EditId > 0 Then
                Me.currentModule = New SystemModule(Me.EditId)
                Me.txtName.Text = Me.currentModule.Name
                Me.txtDescription.Text = Me.currentModule.Description
                If Me.currentModule.SupervisorID > 0 Then
                    Me.ddlSupervisor.SelectedValue = Me.currentModule.SupervisorID.ToString
                Else
                    Dim fldr As New SystemModule(Me.currentModule.FolderID)
                    If fldr.SupervisorID > 0 Then
                        Me.ddlSupervisor.SelectedValue = fldr.SupervisorID.ToString
                    End If
                End If
                Me.ddlImportTable.SelectedValue = Me.currentModule.FolderID.ToString
                Me.ddlIcon.SelectedValue = Me.currentModule.Icon
                Me.imgIcon.ImageUrl = Me.currentModule.Icon

                Me.ModuleQuestions = LoadModuleQuestions(Me.EditId)
            Else
                Me.currentModule = New SystemModule()
                Me.ddlImportTable.SelectedValue = Me.FolderId.ToString
            End If
        End If

        Me.ddlImportTable.Enabled = (Me.chkImportModule.Checked)

        Me.lblModuleName.Text = Me.txtName.Text
        Me.lblModuleName1.Text = Me.lblModuleName.Text & If(Me.chkImportModule.Checked, " Import", "")

        If Me.Type = Enums.SystemModuleType.Module Then
            Me.Master.TitleText = "Module Wizard"
        ElseIf Me.Type = Enums.SystemModuleType.Folder Then
            Me.Master.TitleText = "Folder Wizard"
        End If

        Me.BuildQuestionList()

        Me.pnlModuleStep1Questions.Visible = (Me.Type = Enums.SystemModuleType.Module)
        Me.pnlModuleStepInfo1.Visible = (Me.Type = Enums.SystemModuleType.Module)
        Me.pnlModuleStepInfo2.Visible = (Me.Type = Enums.SystemModuleType.Module)
        Me.pnlFolderStepInfo1.Visible = (Me.Type = Enums.SystemModuleType.Folder)
        Me.pnlFolderStepInfo2.Visible = (Me.Type = Enums.SystemModuleType.Folder)

        Me.imgIcon.ImageUrl = Me.ddlIcon.SelectedItem.Value

        Me.ShowOptions()
    End Sub

    Private Sub ShowOptions()
        Me.pnlDropDownValues.Visible = (Me.ddlQuestionType.SelectedValue = "1")
    End Sub

    Private Sub LoadLists()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim iconFolder As String = "modules"
            If App.CurrentClient.IconSize = Enums.IconSize.Large Then iconFolder = "modules_large"

            Me.ddlSupervisor.Items.Clear()
            Me.ddlSupervisor.Items.Add(New RadComboBoxItem("Unassigned", "0"))
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [vwUserInfo] WHERE [xClientID] = " & App.CurrentClient.ID & " AND [xPermissions] = '" & Enums.SystemUserPermissions.Supervisor.ToString & "' ORDER BY [xName]", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlSupervisor.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            rs.Close()
            cmd.Cancel()
            Me.ddlSupervisor.SelectedIndex = 0

            Me.ddlQuestion.Items.Clear()
            cmd = New SqlClient.SqlCommand("SELECT DISTINCT [xQuestion] FROM [Questions] WHERE [Active] = 1 ORDER BY [xQuestion];", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlQuestion.Items.Add(New RadComboBoxItem(rs("xQuestion").ToString))
            Loop
            rs.Close()
            cmd.Cancel()

            Me.ddlImportTable.Items.Clear()
            cmd = New SqlClient.SqlCommand("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME LIKE '_import_%' ORDER BY TABLE_NAME;", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlImportTable.Items.Add(New RadComboBoxItem(rs("TABLE_NAME").ToString.Replace("_import_", ""), rs("TABLE_NAME").ToString))
            Loop
            Me.ddlImportTable.SelectedIndex = 0

            Me.ddlIcon.Items.Clear()
            Dim files As List(Of String) = My.Computer.FileSystem.GetFiles(Server.MapPath("~/images/gallery/" & iconFolder & "/")).ToList
            For Each s As String In files
                Dim f As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(s)

                If f.Name.ToLower <> "up_folder.png" And f.Name.ToLower <> "folder.png" Then
                    Dim itm As New Telerik.Web.UI.RadComboBoxItem
                    itm.Text = f.Name
                    itm.Value = "~/images/gallery/" & iconFolder & "/" & f.Name
                    'itm.ImageUrl = itm.Value

                    Me.ddlIcon.Items.Add(itm)
                End If
            Next
            Me.ddlIcon.SelectedIndex = 0

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.WizardStep -= 1
    End Sub

    Protected Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If Me.WizardStep < Me.wizardMaxSteps Then
            If Me.chkImportModule.Checked = False Then
                Me.WizardStep += 1
                Select Case Me.WizardStep
                    Case 2 : Me.ddlQuestion.Focus()
                End Select
            Else
                ' import tables setup their questions from the table structure so cannot be edited
                ' so jump straight to the last step
                Me.WizardStep = Me.wizardMaxSteps
            End If
        Else
            Dim cn As New SqlClient.SqlConnection(ConnectionString)

            Try
                If Me.EditId > 0 Then Me.currentModule = New SystemModule(Me.EditId) Else Me.currentModule = New SystemModule()

                ' save the module
                Me.currentModule.ID = Me.EditId
                Me.currentModule.ClientID = App.CurrentClient.ID
                Me.currentModule.Name = Me.txtName.Text
                Me.currentModule.Description = Me.txtDescription.Text
                Me.currentModule.Icon = Me.ddlIcon.SelectedValue
                Me.currentModule.Type = Me.Type
                Me.currentModule.FolderID = 0
                Me.currentModule.ImportModule = Me.chkImportModule.Checked
                Me.currentModule.ImportTable = Me.ddlImportTable.SelectedValue
                Me.currentModule.SupervisorID = Me.ddlSupervisor.SelectedValue.ToInteger
                Me.currentModule = Me.currentModule.Save

                ' save the questions
                Dim cmd As New SqlClient.SqlCommand("DELETE FROM [Questions] WHERE [xModuleID] = " & Me.currentModule.ID, cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()

                If Me.chkImportModule.Checked Then
                    ' import module questions are generated from the fields in the table
                    Me.ModuleQuestions = New List(Of SystemQuestion)

                    cmd = New SqlClient.SqlCommand("select column_name from information_schema.columns where table_name like '" & Me.ddlImportTable.SelectedValue & "'", cn)
                    If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                    Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                    Do While rs.Read
                        Dim q As New SystemQuestion

                        q.ID = 0
                        q.ModuleID = 0
                        q.Question = rs("column_name").ToString.Replace("_", " ")
                        If q.Question.ToLower.Contains("desc") Or q.Question.ToLower.Contains("comment") Then
                            q.Type = Enums.SystemQuestionType.MemoField
                        Else q.Type = Enums.SystemQuestionType.TextBox
                        End If
                        q.Required = False
                        q.SearchField = False
                        q.ReportField = True
                        q.ExportField = True
                        q.MobileField = True

                        Me.ModuleQuestions.Add(q)
                    Loop
                End If

                For Each q As SystemQuestion In Me.ModuleQuestions
                    q.ModuleID = Me.currentModule.ID
                    q.ID = 0
                    q = q.Save()
                Next

                ' update the module id's
                cmd = New SqlClient.SqlCommand("EXEC [procRefreshModuleIDs];", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()

                ' update the question id's
                cmd = New SqlClient.SqlCommand("EXEC [procRefreshQuestionIDs];", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()

                If Me.Type = Enums.SystemModuleType.Folder Then
                    CommonCore.Shared.Common.LogHistory(Me.txtName.Text & " Folder Updated", App.CurrentUser.ID)
                Else CommonCore.Shared.Common.LogHistory(Me.txtName.Text & " Module Updated", App.CurrentUser.ID)
                End If

            Catch ex As Exception
                ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
            Finally
                cn.Close()
            End Try

            Response.Redirect("~/Default.aspx", False)
            Exit Sub
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("~/Default.aspx", False)
        Exit Sub
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            Dim q As New SystemQuestion

            q.ID = 0
            q.ModuleID = 0
            q.Question = Me.ddlQuestion.Text
            q.Type = CType(Me.ddlQuestionType.SelectedValue, Enums.SystemQuestionType)
            q.Required = Me.chkRequired.Checked
            q.SearchField = Me.chkSearch.Checked
            q.ReportField = Me.chkReporting.Checked
            q.ExportField = Me.chkExport.Checked
            q.MobileField = Me.chkMobile.Checked
            If q.Type = Enums.SystemQuestionType.DropDownList Then
                For Each itm As ListItem In Me.lstValues.Items
                    If itm.Text.Trim <> "" Then q.Values.Add(itm.Text.Trim)
                Next
            End If

            Me.ModuleQuestions.Add(q)
            Me.BuildQuestionList()

            Me.ddlQuestion.Text = ""
            Me.ddlQuestionType.SelectedIndex = -1
            Me.chkRequired.Checked = False
            Me.chkSearch.Checked = False
            Me.chkReporting.Checked = False
            Me.chkExport.Checked = False
            Me.chkMobile.Checked = False
            Me.lstValues.Items.Clear()
            Me.ddlQuestion.Focus()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        End Try
    End Sub

    Private Sub BuildQuestionList()
        Try
            Me.tblQuestions.Rows.Clear()

            Dim x As Integer = 0
            For Each q As SystemQuestion In Me.ModuleQuestions
                Dim tr1 As New TableRow
                Dim tc1 As New TableCell
                tc1.Width = New Unit(20, UnitType.Pixel)
                If q.Type = Enums.SystemQuestionType.MemoField Then tc1.VerticalAlign = VerticalAlign.Top
                Dim ibtnDelete As New Telerik.Web.UI.RadButton
                ibtnDelete.ID = "ibtnDelete_" & x
                ibtnDelete.Icon.PrimaryIconUrl = "~/images/toolbar/icon_delete.png"
                ibtnDelete.Width = New Unit(16, UnitType.Pixel)
                AddHandler ibtnDelete.Click, AddressOf ibtnDelete_Click
                tc1.Controls.Add(ibtnDelete)
                tr1.Cells.Add(tc1)

                Dim tc2 As New TableCell
                tc2.Width = New Unit(20, UnitType.Pixel)
                If q.Type = Enums.SystemQuestionType.MemoField Then tc2.VerticalAlign = VerticalAlign.Top
                Dim ibtnEdit As New Telerik.Web.UI.RadButton
                ibtnEdit.ID = "ibtnEdit_" & x
                ibtnEdit.Icon.PrimaryIconUrl = "~/images/toolbar/icon_edit.png"
                ibtnEdit.Width = New Unit(16, UnitType.Pixel)
                AddHandler ibtnEdit.Click, AddressOf ibtnEdit_Click
                tc2.Controls.Add(ibtnEdit)
                tr1.Cells.Add(tc2)

                Dim tc3 As New TableCell
                tc3.Text = q.Question
                If q.Type = Enums.SystemQuestionType.MemoField Then tc3.VerticalAlign = VerticalAlign.Top
                tr1.Cells.Add(tc3)

                'Dim tr2 As New TableRow
                Dim tc4 As New TableCell
                tc4.ColumnSpan = 3
                Select Case q.Type
                    Case Enums.SystemQuestionType.CheckBox
                        Dim chk As New Controls.CheckBoxes.CheckBox
                        chk.ID = "chk_" & x
                        tc4.Controls.Add(chk)

                    Case Enums.SystemQuestionType.DropDownList
                        Dim ddl As New Controls.DropDownLists.DropDownList
                        ddl.ID = "ddl_" & x
                        ddl.Width = New Unit(200, UnitType.Pixel)
                        For Each itm As String In q.Values
                            ddl.Items.Add(New RadComboBoxItem(itm, itm))
                        Next
                        tc4.Controls.Add(ddl)

                    Case Enums.SystemQuestionType.MemoField
                        Dim txt As New Controls.TextBoxes.TextBox
                        txt.ID = "txt_" & x
                        txt.Width = New Unit(250, UnitType.Pixel)
                        txt.Rows = 3
                        txt.TextMode = InputMode.MultiLine
                        tc4.Controls.Add(txt)

                    Case Enums.SystemQuestionType.TextBox
                        Dim txt As New Controls.TextBoxes.TextBox
                        txt.ID = "txt_" & x
                        txt.Width = New Unit(250, UnitType.Pixel)
                        txt.MaxLength = 255
                        tc4.Controls.Add(txt)

                    Case Enums.SystemQuestionType.NumericTextBox
                        Dim txt As New Controls.TextBoxes.NumericTextBox
                        txt.ID = "txt_" & x
                        txt.Width = New Unit(100, UnitType.Pixel)
                        txt.NumberFormat.DecimalDigits = 0
                        tc4.Controls.Add(txt)
                End Select
                tc4.VerticalAlign = VerticalAlign.Top
                tr1.Cells.Add(tc4)

                'Dim tr3 As New TableRow
                Dim tc5 As New TableCell
                tc5.Width = New Unit(20, UnitType.Pixel)
                Dim imgRequired As New Image
                imgRequired.ID = "imgRequired" & x
                imgRequired.ImageUrl = "~/images/toolbar/icon_error.png"
                imgRequired.ToolTip = "Required Field"
                If q.Required Then tc5.Controls.Add(imgRequired)
                tc5.VerticalAlign = VerticalAlign.Top
                tr1.Cells.Add(tc5)

                Dim tc6 As New TableCell
                tc6.Width = New Unit(20, UnitType.Pixel)
                Dim imgSearch As New Image
                imgSearch.ID = "imgSearch" & x
                imgSearch.ImageUrl = "~/images/toolbar/icon_search.png"
                imgSearch.ToolTip = "Include on Search Grid"
                If q.SearchField Then tc6.Controls.Add(imgSearch)
                tc6.VerticalAlign = VerticalAlign.Top
                tr1.Cells.Add(tc6)

                Dim tc7 As New TableCell
                tc7.Width = New Unit(20, UnitType.Pixel)
                Dim imgReporting As New Image
                imgReporting.ID = "imgReporting" & x
                imgReporting.ImageUrl = "~/images/toolbar/icon_report.png"
                imgReporting.ToolTip = "Allow Reporting"
                If q.ReportField Then tc7.Controls.Add(imgReporting)
                tc7.VerticalAlign = VerticalAlign.Top
                tr1.Cells.Add(tc7)

                Dim tc8 As New TableCell
                tc8.Width = New Unit(20, UnitType.Pixel)
                Dim imgExport As New Image
                imgExport.ID = "imgExport" & x
                imgExport.ImageUrl = "~/images/toolbar/icon_saveweb.png"
                imgExport.ToolTip = "Can be Exported"
                If q.ExportField Then tc8.Controls.Add(imgExport)
                tc8.VerticalAlign = VerticalAlign.Top
                tr1.Cells.Add(tc8)

                Me.tblQuestions.Rows.Add(tr1)
                'Me.tblQuestions.Rows.Add(tr2)
                'Me.tblQuestions.Rows.Add(tr3)

                x += 1
            Next

            If Me.OnMobile Then
                Me.SetSkin(Me.tblQuestions, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Mobile"))
            Else Me.SetSkin(Me.tblQuestions, System.Configuration.ConfigurationManager.AppSettings("Telerik_Skin_Desktop"))
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        End Try
    End Sub

    Protected Sub ibtnEdit_Click(sender As Object, e As EventArgs)
    End Sub

    Protected Sub ibtnDelete_Click(sender As Object, e As EventArgs)
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim ibtn As Telerik.Web.UI.RadButton = CType(sender, Telerik.Web.UI.RadButton)
            Dim qId As Integer = ibtn.ID.Split("_"c)(1).ToInteger

            Me.ModuleQuestions.RemoveAt(qId)
            Me.BuildQuestionList()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub btnAddValue_Click(sender As Object, e As EventArgs) Handles btnAddValue.Click
        Me.lstValues.Items.Add(New ListItem(Me.txtDDLValue.Text.Trim))
        Me.txtDDLValue.Text = ""
        Me.txtDDLValue.Focus()
    End Sub

    Protected Sub btnRemoveValue_Click(sender As Object, e As EventArgs) Handles btnRemoveValue.Click
        Me.lstValues.Items.RemoveAt(Me.lstValues.SelectedIndex)
    End Sub

    Protected Sub ddlIcon_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlIcon.SelectedIndexChanged
        Me.imgIcon.ImageUrl = e.Value
    End Sub
End Class