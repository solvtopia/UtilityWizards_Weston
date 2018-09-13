﻿Imports System.Xml
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
                Else
                    CType(Me.FindInControl("pnlStep" & x), Panel).Visible = True

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
    Private CurrentModule As SystemModule

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Session("ModuleWizardQuestions") = Nothing

            Me.WizardStep = 1
            Me.LoadLists()
            Me.txtName.Focus()

            If Me.EditId > 0 Then
                Me.CurrentModule = New SystemModule(Me.EditId)
                Me.txtName.Text = Me.CurrentModule.Name
                Me.txtDescription.Text = Me.CurrentModule.Description
                If Me.CurrentModule.SupervisorID > 0 Then
                    Me.ddlSupervisor.SelectedValue = Me.CurrentModule.SupervisorID.ToString
                Else
                    Dim fldr As New SystemModule(Me.CurrentModule.FolderID)
                    If fldr.SupervisorID > 0 Then
                        Me.ddlSupervisor.SelectedValue = fldr.SupervisorID.ToString
                    End If
                End If
                Me.ddlIcon.SelectedValue = Me.CurrentModule.Icon
                Me.imgIcon.ImageUrl = Me.CurrentModule.Icon

                Me.ModuleView1.ModuleQuestions = LoadModuleQuestions(Me.EditId)

                Me.ModuleView1.TopLeftTitle = Me.CurrentModule.TopLeftTitle
                Me.ModuleView1.TopMiddleTitle = Me.CurrentModule.TopMiddleTitle
                Me.ModuleView1.TopRightTitle = Me.CurrentModule.TopRightTitle
                Me.ModuleView1.FullPageTitle = Me.CurrentModule.FullPageTitle
                Me.ModuleView1.BottomLeftTitle = Me.CurrentModule.BottomLeftTitle
                Me.ModuleView1.BottomMiddleTitle = Me.CurrentModule.BottomMiddleTitle
                Me.ModuleView1.BottomRightTitle = Me.CurrentModule.BottomRightTitle
            Else
                Me.CurrentModule = New SystemModule()
            End If
        End If

        Me.ModuleView1.CurrentModule = Me.CurrentModule

        If Me.Type = Enums.SystemModuleType.Module Then
            Me.Master.TitleText = "Module Wizard"
        ElseIf Me.Type = Enums.SystemModuleType.Folder Then
            Me.Master.TitleText = "Folder Wizard"
        End If
        Me.Master.InfoBoxTitleText = "Field Editor"

        Me.ModuleView1.BuildQuestionList()

        Me.pnlModuleStep1Questions.Visible = (Me.Type = Enums.SystemModuleType.Module)

        Me.imgIcon.ImageUrl = Me.ddlIcon.SelectedItem.Value

        Me.ShowOptions()
    End Sub

    Private Sub ShowOptions()
        ' appearance
        Me.pnlTextBoxAppearanceOptions.Visible = (Me.ddlQuestionType.SelectedValue = CStr(Enums.SystemQuestionType.TextBox) _
            Or Me.ddlQuestionType.SelectedValue = CStr(Enums.SystemQuestionType.NumericTextBox) _
            Or Me.ddlQuestionType.SelectedValue = CStr(Enums.SystemQuestionType.CurrencyTextBox))
        Me.pnlPlainTextBoxAppearanceOptions.Visible = (Me.ddlQuestionType.SelectedValue = CStr(Enums.SystemQuestionType.TextBox))
        Me.pnlMemoTextBoxAppearanceOptions.Visible = (Me.ddlQuestionType.SelectedValue = CStr(Enums.SystemQuestionType.MemoField))
        Me.pnlNumericTextBoxAppearanceOptions.Visible = (Me.ddlQuestionType.SelectedValue = CStr(Enums.SystemQuestionType.NumericTextBox))
        Me.pnlDropDownAppearanceOptions.Visible = (Me.ddlQuestionType.SelectedValue = CStr(Enums.SystemQuestionType.DropDownList))

        ' data
        Me.pnlMasterFeedField.Visible = (Me.ddlBindingType.SelectedValue = CStr(Enums.SystemQuestionBindingType.MasterFeed))
        Me.pnlFormulaField.Visible = (Me.ddlBindingType.SelectedValue = CStr(Enums.SystemQuestionBindingType.Formula))
        Me.pnlDropDownDataOptions.Visible = (Me.ddlQuestionType.SelectedValue = CStr(Enums.SystemQuestionType.DropDownList))
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

            Me.ddlMasterFeedField.Items.Clear()
            Me.ddlMasterFeedField.Items.Add(New ListItem(""))
            cmd = New SqlClient.SqlCommand("select COLUMN_NAME from information_schema.columns where table_name like '_MasterFeed' ORDER BY COLUMN_NAME", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            rs = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlMasterFeedField.Items.Add(New ListItem(rs("COLUMN_NAME").ToString))
            Loop
            rs.Close()
            cmd.Cancel()

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
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            If Me.WizardStep < Me.wizardMaxSteps Then
                pnlQuestionEditor.Visible = False
                Me.WizardStep += 1
                Select Case Me.WizardStep
                    Case 2
                        Me.txtQuestion.Focus()
                End Select
            Else
                If Me.EditId > 0 Then Me.CurrentModule = New SystemModule(Me.EditId) Else Me.CurrentModule = New SystemModule()

                ' save the module
                Me.CurrentModule.ID = Me.EditId
                Me.CurrentModule.ClientID = App.CurrentClient.ID
                Me.CurrentModule.Name = Me.txtName.Text
                Me.CurrentModule.Description = Me.txtDescription.Text
                Me.CurrentModule.Icon = Me.ddlIcon.SelectedValue
                Me.CurrentModule.Type = Me.Type
                Me.CurrentModule.FolderID = 0
                Me.CurrentModule.SupervisorID = Me.ddlSupervisor.SelectedValue.ToInteger
                Me.CurrentModule.TopLeftTitle = Me.ModuleView1.TopLeftTitle
                Me.CurrentModule.TopMiddleTitle = Me.ModuleView1.TopMiddleTitle
                Me.CurrentModule.TopRightTitle = Me.ModuleView1.TopRightTitle
                Me.CurrentModule.FullPageTitle = Me.ModuleView1.FullPageTitle
                Me.CurrentModule.BottomLeftTitle = Me.ModuleView1.BottomLeftTitle
                Me.CurrentModule.BottomMiddleTitle = Me.ModuleView1.BottomMiddleTitle
                Me.CurrentModule.BottomRightTitle = Me.ModuleView1.BottomRightTitle
                Me.CurrentModule = Me.CurrentModule.Save

                ' save the questions
                Dim cmd As New SqlClient.SqlCommand("DELETE FROM [Questions] WHERE [xModuleID] = " & Me.CurrentModule.ID, cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                cmd.ExecuteNonQuery()

                For Each q As SystemQuestion In Me.ModuleView1.ModuleQuestions
                    q.ModuleID = Me.CurrentModule.ID
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
                Else CommonCore.Shared.Common.LogHistory(Me.txtName.Text & " Tab Updated", App.CurrentUser.ID)
                End If

                Response.Redirect("~/Default.aspx", False)
                Exit Sub
            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("~/account/ModuleManager.aspx?fid=" & Me.FolderId, False)
        Exit Sub
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            Dim btn As RadButton = CType(sender, RadButton)
            Dim q As New SystemQuestion

            ' definition
            q.ID = 0
            q.ModuleID = 0
            q.Question = Me.txtQuestion.Text
            q.Type = CType(Me.ddlQuestionType.SelectedValue, Enums.SystemQuestionType)

            ' appearance
            q.Visible = Me.chkDisplay.Checked
            q.Location = CType(Me.ddlDisplay.SelectedValue, Enums.SystemQuestionLocation)
            q.Sort = Me.txtSort.Text.ToInteger
            ' text boxes
            If q.Type = Enums.SystemQuestionType.TextBox Then
                q.TextBoxSize = CType(Me.ddlTextBoxSize.SelectedValue, Enums.SystemQuestionTextBoxSize)
                q.DisplayAsDate = Me.chkDisplayAsDate.Checked
            End If
            ' drop-downs
            If q.Type = Enums.SystemQuestionType.DropDownList Then
                q.DropDownSize = CType(Me.ddlDropDownSize.SelectedValue, Enums.SystemQuestionDropDownSize)
            End If
            ' memos
            If q.Type = Enums.SystemQuestionType.MemoField Then
                q.Rows = Me.txtMemoRows.Text.ToInteger
            End If
            ' numeric text boxes
            If q.Type = Enums.SystemQuestionType.NumericTextBox Then
                q.DecimalDigits = Me.txtNumbersAfterComma.Text.ToInteger
                q.ThousandsSeparator = Me.chkThousandsSeparator.Checked
            End If

            ' data
            q.BindingType = CType(Me.ddlBindingType.SelectedValue, Enums.SystemQuestionBindingType)
            If q.BindingType = Enums.SystemQuestionBindingType.MasterFeed Then
                q.MasterFeedField = Me.ddlMasterFeedField.SelectedValue
            ElseIf q.BindingType = Enums.SystemQuestionBindingType.Formula Then
                q.Rule = Me.txtFormula.Text
            End If

            If q.Type = Enums.SystemQuestionType.DropDownList Then
                For Each itm As ListItem In Me.lstValues.Items
                    If itm.Text.Trim <> "" Then q.DropDownValues.Add(itm.Text.Trim)
                Next
            End If

            ' miscellaneous
            q.Required = Me.chkRequired.Checked
            q.ReportField = Me.chkReporting.Checked
            q.ExportField = Me.chkExport.Checked

            If Not Me.btnDelete.Visible Then
                Me.ModuleView1.ModuleQuestions.Add(q)
            Else
                Me.ModuleView1.ModuleQuestions.Item(btn.CommandArgument.ToInteger) = q
            End If

            Me.ModuleView1.Refresh()

            pnlQuestionEditor.Visible = False
            Me.ModuleView1.Refresh()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        End Try
    End Sub

    Protected Sub btnAddValue_Click(sender As Object, e As EventArgs) Handles btnAddValue.Click
        If Me.txtDDLText.Text.Trim <> "" Then
            If Me.txtDDLValue.Text.Trim = "" Then Me.txtDDLValue.Text = Me.txtDDLText.Text.Trim
            Me.lstValues.Items.Add(New ListItem(Me.txtDDLText.Text.Trim & "=" & Me.txtDDLValue.Text.Trim))
            Me.txtDDLText.Text = ""
            Me.txtDDLValue.Text = ""
            Me.txtDDLText.Focus()
        End If
    End Sub

    Protected Sub btnRemoveValue_Click(sender As Object, e As EventArgs) Handles btnRemoveValue.Click
        Me.lstValues.Items.RemoveAt(Me.lstValues.SelectedIndex)
    End Sub

    Protected Sub ddlIcon_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlIcon.SelectedIndexChanged
        Me.imgIcon.ImageUrl = e.Value
    End Sub

    Private Sub btnAddNewQuestion_Click(sender As Object, e As EventArgs) Handles btnAddNewQuestion.Click
        Me.btnDelete.Visible = False

        ' set the wizard step to 2 so we see the editor
        Me.WizardStep = 2

        Me.FillEditor(New SystemQuestion)

        Me.txtQuestion.Focus()
        pnlQuestionEditor.Visible = True
    End Sub

    Private Sub btnCancelEditor_Click(sender As Object, e As EventArgs) Handles btnCancelEditor.Click
        pnlQuestionEditor.Visible = False
    End Sub

    Private Sub FillEditor(ByVal q As SystemQuestion)
        ' definition
        Me.txtQuestion.Text = q.Question
        Me.ddlQuestionType.SelectedValue = CStr(q.Type)

        ' appearance
        Me.chkDisplay.Checked = q.Visible
        Me.ddlDisplay.SelectedValue = CStr(q.Location)
        Me.txtSort.Text = q.Sort.ToString
        ' text boxes
        Me.ddlTextBoxSize.SelectedValue = CStr(q.TextBoxSize)
        Me.chkDisplayAsDate.Checked = q.DisplayAsDate
        ' drop-downs
        Me.ddlDropDownSize.SelectedValue = CStr(q.DropDownSize)
        ' memos
        Me.txtMemoRows.Text = q.Rows.ToString
        ' numeric text boxes
        Me.txtNumbersAfterComma.Text = q.DecimalDigits.ToString
        Me.chkThousandsSeparator.Checked = q.ThousandsSeparator

        ' data
        Me.ddlBindingType.SelectedValue = CStr(q.BindingType)
        Me.ddlMasterFeedField.SelectedValue = q.MasterFeedField
        Me.txtFormula.Text = q.Rule
        Me.lstValues.Items.Clear()

        ' miscellaneous
        Me.chkRequired.Checked = q.Required
        Me.chkReporting.Checked = q.ReportField
        Me.chkExport.Checked = q.ExportField

        For Each itm As RadPanelItem In Me.pbProperties.GetAllItems()
            itm.Expanded = True
        Next
        Me.ShowOptions()
    End Sub

    Protected Sub ModuleView_EditQuestion(ByVal q As SystemQuestion, ByVal qid As Integer) Handles ModuleView1.EditQuestion
        Me.btnDelete.Visible = True
        Me.btnAdd.CommandArgument = qid.ToString
        Me.btnDelete.CommandArgument = qid.ToString

        Me.FillEditor(q)

        Me.txtQuestion.Focus()
        pnlQuestionEditor.Visible = True
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim btn As RadButton = CType(sender, RadButton)
        Me.ModuleView1.ModuleQuestions.RemoveAt(btn.CommandArgument.ToInteger)
        Me.ModuleView1.Refresh()
    End Sub
End Class