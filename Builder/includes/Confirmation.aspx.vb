Imports UtilityWizards.CommonCore.Shared.Common

Public Class Confirmation
    Inherits builderPage

#Region "Properties"

    Private ReadOnly Property Buttons As Enums.InformationPopupButtons
        Get
            Return CType(Request.QueryString("b"), Enums.InformationPopupButtons)
        End Get
    End Property
    Private ReadOnly Property Type As Enums.InformationPopupType
        Get
            Return CType(Request.QueryString("t"), Enums.InformationPopupType)
        End Get
    End Property
    Private ReadOnly Property Icon As Enums.InformationPopupIcon
        Get
            Return CType(Request.QueryString("i"), Enums.InformationPopupIcon)
        End Get
    End Property
    Private ReadOnly Property Message As String
        Get
            Return Server.UrlDecode(Request.QueryString("m"))
        End Get
    End Property
    Private ReadOnly Property EditId As Integer
        Get
            Return Request.QueryString("id").ToInteger
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadLists()

            Select Case Me.Buttons
                Case Enums.InformationPopupButtons.OkCancel
                    Me.btnYes.Text = "Ok"
                    Me.btnNo.Text = "Cancel"
                Case Enums.InformationPopupButtons.YesNo
                    Me.btnYes.Text = "Yes"
                    Me.btnNo.Text = "No"
                Case Enums.InformationPopupButtons.OkOnly
                    Me.btnYes.Text = "Ok"
                    Me.btnNo.Visible = False
            End Select

            Me.pnlChooseFolder.Visible = False
            If Me.Message <> "" Then
                Select Case Me.Type
                    Case Enums.InformationPopupType.ErrorMessage
                        Me.litMessage.Text = "If you can see this, we already know about the issue and are working on it!<br/><br/>" & Me.Message
                    Case Enums.InformationPopupType.InformationOnly
                        Me.litMessage.Text = Me.Message
                End Select
            Else
                Select Case Me.Type
                    Case Enums.InformationPopupType.DeleteReport
                        Me.litMessage.Text = "Are you sure you want to delete this Report?<br/><br/>This action can only be undone by Solvtopia Administrators."
                    Case Enums.InformationPopupType.DeleteFolder
                        Me.litMessage.Text = "Are you sure you want to delete this Folder?<br/><br/>All Modules in this folder will be moved to the dashboard."
                    Case Enums.InformationPopupType.DeleteModule
                        Me.litMessage.Text = "Are you sure you want to delete this Module?<br/><br/>This action can only be undone by Solvtopia Administrators."
                    Case Enums.InformationPopupType.DeleteUser
                        Me.litMessage.Text = "Are you sure you want to delete this User?<br/><br/>This action can only be undone by Solvtopia Administrators."
                    Case Enums.InformationPopupType.MoveModule
                        Me.litMessage.Text = "Choose a folder from the list below or place this module on the dashboard.<br/><br/>"
                        Me.pnlChooseFolder.Visible = True
                End Select
            End If

            Select Case Me.Icon
                Case Enums.InformationPopupIcon.Error
                    Me.imgIcon.ImageUrl = "~/images/icon_error.png"
                    Me.lblTitle.Text = "Oh no!! Something went wrong."
                Case Enums.InformationPopupIcon.Information
                    Me.imgIcon.ImageUrl = "~/images/icon_information.png"
                    If Me.Type = Enums.InformationPopupType.MoveModule Then
                        Me.lblTitle.Text = "Selection Required!"
                    Else Me.lblTitle.Text = "News Flash!"
                    End If
                Case Enums.InformationPopupIcon.Question
                    Me.imgIcon.ImageUrl = "~/images/icon_question.png"
                    Me.lblTitle.Text = "Hold on a second!"
            End Select
        End If
    End Sub

    Private Sub LoadLists()
        Me.ddlFolder.Items.Clear()
        Me.ddlFolder.Items.Add(New Telerik.Web.UI.RadComboBoxItem("Show on Dashboard", "0"))
        For Each m As SystemModule In App.CurrentClient.Modules
            If m.Type = Enums.SystemModuleType.Folder Then
                Me.ddlFolder.Items.Add(New Telerik.Web.UI.RadComboBoxItem(m.Name, m.ID.ToString))
            End If
        Next
    End Sub

    Protected Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            If Me.Type = Enums.InformationPopupType.DeleteFolder Then
                Dim m As New SystemModule(Me.EditId)
                m.Delete()
                CommonCore.Shared.Common.LogHistory(m.Name & " Folder Deleted", App.CurrentUser.ID)

                App.ActiveFolderID = 0

            ElseIf Me.Type = Enums.InformationPopupType.DeleteModule Then
                Dim m As New SystemModule(Me.EditId)
                m.Delete()
                CommonCore.Shared.Common.LogHistory(m.Name & " Module Deleted", App.CurrentUser.ID)

            ElseIf Me.Type = Enums.InformationPopupType.DeleteUser Then
                Dim usr As New SystemUser(Me.EditId)
                usr.Delete()
                CommonCore.Shared.Common.LogHistory(usr.Name & " User Deleted", App.CurrentUser.ID)

            ElseIf Me.Type = Enums.InformationPopupType.DeleteReport Then
                Dim rpt As New SystemReport(Me.EditId)
                rpt.Delete()
                CommonCore.Shared.Common.LogHistory(rpt.Name & " Report Deleted", App.CurrentUser.ID)

            ElseIf Me.Type = Enums.InformationPopupType.MoveModule Then
                Dim m As New SystemModule(Me.EditId)
                m.Move(Me.ddlFolder.SelectedValue.ToInteger)
                CommonCore.Shared.Common.LogHistory(m.Name & " Module Moved", App.CurrentUser.ID)

            End If

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(App.CurrentUser.ID, App.CurrentClient.ID, Enums.ProjectName.Builder))
        Finally
            cn.Close()
        End Try

        Response.Redirect("~/Default.aspx", False)
        Exit Sub
    End Sub

    Private Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        Response.Redirect("~/Default.aspx", False)
        Exit Sub
    End Sub
End Class