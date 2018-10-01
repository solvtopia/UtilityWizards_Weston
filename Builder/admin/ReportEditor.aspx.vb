Imports System.Xml
Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Shared.Common
Imports UtilityWizards.CommonCore.Shared.Xml
Imports Telerik.Web.UI

Public Class ReportEditor
    Inherits builderPage

#Region "Properties"

    Dim maxSteps As Integer = 6
    Dim wizardMaxSteps As Integer = 4
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
    Private ReadOnly Property EditId As Integer
        Get
            Return Request.QueryString("id").ToInteger
        End Get
    End Property
    Private ReadOnly Property TransactionType As Enums.TransactionType
        Get
            Return CType(Request.QueryString("t").ToInteger, Enums.TransactionType)
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.WizardStep = 1

            Me.LoadLists()

            If Me.EditId > 0 Then
                Me.LoadData()
            End If
        End If

        Me.RadReportGrid.DataBind()
    End Sub

    Private Sub LoadData()
        Dim rpt As New SystemReport(Me.EditId, App.UseSandboxDb)
        Dim m As New SystemModule(rpt.ModuleId, App.UseSandboxDb)

        Me.txtName.Text = rpt.Name
        Me.txtDescription.Text = rpt.Description
        Me.ddlFolder.SelectedValue = m.FolderID.ToString
        Me.LoadModules()
        Me.ddlModule.SelectedValue = rpt.ModuleId.ToString
        Me.LoadQuestions()

        Me.lstValues.Items.Clear()
        For Each s As String In rpt.Fields
            Me.lstValues.Items.Add(New ListItem(s.Split("|"c)(0), s.Split("|"c)(1)))
        Next
    End Sub

    Private Sub LoadLists()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Me.ddlFolder.Items.Clear()
            Me.ddlFolder.Items.Add(New RadComboBoxItem("Dashboard", "0"))

            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Modules] WHERE [xType] = '" & Enums.SystemModuleType.Folder.ToString & "' AND [xClientID] = " & App.CurrentClient.ID & " AND [Active] = 1", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlFolder.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()
            Me.ddlFolder.SelectedIndex = 0
            Me.LoadModules()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadModules()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Me.ddlModule.Items.Clear()
            Dim cmd As New SqlClient.SqlCommand("SELECT [ID], [xName] FROM [Modules] WHERE [xType] = '" & Enums.SystemModuleType.Module.ToString & "' AND [xFolderID] = " & Me.ddlFolder.SelectedValue & " AND [xClientID] = " & App.CurrentClient.ID & " AND [Active] = 1", cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                Me.ddlModule.Items.Add(New RadComboBoxItem(rs("xName").ToString, rs("ID").ToString))
            Loop
            cmd.Cancel()
            rs.Close()
            Me.ddlModule.SelectedIndex = 0
            Me.LoadQuestions()

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub LoadGeneralQuestions()
        ' general questions added to all modules
        For x As Integer = 0 To CommonQuestions.DataFieldName.Count - 1
            Dim itemValue As String = ""
            If Me.ddlFolder.SelectedValue.ToInteger = 0 Then
                itemValue = CommonQuestions.DataFieldName(x)
            Else itemValue = Me.ddlFolder.SelectedItem.Text & "~" & CommonQuestions.DataFieldName(x)
            End If
            Me.ddlQuestion.Items.Add(New RadComboBoxItem(CommonQuestions.Question(x), CommonQuestions.DataFieldName(x)))
        Next
    End Sub

    Private Sub LoadQuestions()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Me.ddlQuestion.Items.Clear()

            Me.LoadGeneralQuestions()

            Dim lst As List(Of SystemQuestion) = LoadModuleQuestions(Me.ddlModule.SelectedValue.ToInteger)
            For Each q As SystemQuestion In lst
                If q.ReportField Then
                    Dim itemValue As String = ""
                    If Me.ddlFolder.SelectedValue.ToInteger = 0 Then
                        itemValue = q.DataFieldName
                    Else itemValue = Me.ddlFolder.SelectedItem.Text & "~" & q.DataFieldName
                    End If
                    Me.ddlQuestion.Items.Add(New RadComboBoxItem(q.Question, itemValue))
                End If
            Next
            Me.ddlQuestion.SelectedIndex = 0

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

    Private Function LoadSearchFields() As String
        Dim retVal As String = ""

        For Each itm As ListItem In Me.lstValues.Items
            If retVal = "" Then
                retVal = "ISNULL(xmlData.value('(/Data/" & itm.Value.Replace("~", "/") & "/text())[1]', 'varchar(255)'), '') AS [" & itm.Text & "]"
            Else retVal &= ", ISNULL(xmlData.value('(/Data/" & itm.Value.Replace("~", "/") & "/text())[1]', 'varchar(255)'), '') AS [" & itm.Text & "]"
            End If
        Next

        Return retVal
    End Function

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.WizardStep -= 1
    End Sub

    Private Sub btnAddQuestion_Click(sender As Object, e As EventArgs) Handles btnAddQuestion.Click
        Me.lstValues.Items.Add(New ListItem(Me.ddlQuestion.SelectedItem.Text, Me.ddlQuestion.SelectedValue))
        Me.ddlQuestion.Focus()
    End Sub

    Private Sub btnRemoveQuestion_Click(sender As Object, e As EventArgs) Handles btnRemoveQuestion.Click
        Me.lstValues.Items.RemoveAt(Me.lstValues.SelectedIndex)
    End Sub

    Protected Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If Me.WizardStep < Me.wizardMaxSteps Then
            Me.WizardStep += 1

            Select Case Me.WizardStep
                Case 3
                    Me.lblFields.Text = Me.LoadSearchFields
                    Me.RadReportGrid.DataBind()

            End Select
        Else
            Dim rpt As New SystemReport(App.UseSandboxDb)
            rpt.ID = Me.EditId
            rpt.Name = Me.txtName.Text
            rpt.Description = Me.txtDescription.Text
            rpt.Active = True
            rpt.ClientID = App.CurrentClient.ID
            For Each itm As ListItem In Me.lstValues.Items
                rpt.Fields.Add(itm.Text & "|" & itm.Value)
            Next
            rpt.ModuleId = Me.ddlModule.SelectedValue.ToInteger

            rpt.Save()

            LogHistory(Me.txtName.Text & " Report Updated", App.CurrentUser.ID, App.UseSandboxDb)

            Response.Redirect("~/admin/Reports.aspx", False)
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("~/admin/Reports.aspx", False)
        Exit Sub
    End Sub

    Protected Sub ddlFolder_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlFolder.SelectedIndexChanged
        Me.LoadModules()
    End Sub

    Protected Sub ddlModule_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlModule.SelectedIndexChanged
        Me.LoadQuestions()
    End Sub

    Private Sub lnkDeleteReport_Click(sender As Object, e As EventArgs) Handles lnkDeleteReport.Click
        ShowInformationPopup(Enums.InformationPopupType.DeleteReport, Enums.InformationPopupButtons.YesNo, Me.EditId)
    End Sub

    Private Sub lnkPrintReport_Click(sender As Object, e As EventArgs) Handles lnkPrintReport.Click
        Response.Redirect("~/account/ReportPreview.aspx?id=" & Me.EditId, False)
    End Sub
End Class