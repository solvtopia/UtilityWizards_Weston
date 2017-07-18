Imports System.Xml
Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Xml
Imports Telerik.Web.UI

Public Class AdminUsers
    Inherits builderPage

#Region "Properties"

    Public Property EditId As Integer
        Get
            If Not IsNumeric(Me.lblEditID.Text) Then Me.lblEditID.Text = "0"
            Return Me.lblEditID.Text.ToInteger
        End Get
        Set(value As Integer)
            Me.lblEditID.Text = value.ToString
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LoadUsers()

        If Not IsPostBack Then
            Me.EditId = 0
        End If
    End Sub

    Private Sub LoadUsers()
        Dim cn As New SqlClient.SqlConnection(ConnectionString)

        Try
            Dim lst As New List(Of SystemUser)

            Dim sWhere As String = ""
            If App.CurrentUser.Permissions <> Enums.SystemUserPermissions.Solvtopia Then
                sWhere = " AND [xClientID] = " & App.CurrentClient.ID
            End If

            Dim sql As String = "SELECT [ID] FROM [Users] WHERE [xActive] = 1" & sWhere & " ORDER BY [xPermissions], [xName]"
            Dim cmd As New SqlClient.SqlCommand(sql, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            Do While rs.Read
                lst.Add(New SystemUser(rs("ID").ToString.ToInteger))
            Loop
            rs.Close()
            cmd.Cancel()

            Me.tblAdmins.Rows.Clear()
            Me.tblUsers.Rows.Clear()
            Me.tblTechnicians.Rows.Clear()
            Me.tblSupervisors.Rows.Clear()
            Me.tblSolvtopia.Rows.Clear()

            Dim trAdmins As New TableRow
            Dim trUsers As New TableRow
            Dim trTechnicians As New TableRow
            Dim trSupervisors As New TableRow
            Dim trSolvtopia As New TableRow

            Dim iconSize As Integer = 80
            If App.CurrentClient.IconSize = Enums.IconSize.Large Then
                iconSize = 128
            End If

            For Each u As SystemUser In lst
                'If Me.EditId = 0 Then Me.EditId = u.ID

                Dim tc As New TableCell
                tc.Attributes.Add("style", "display: inline-block; padding: 10px;")
                tc.VerticalAlign = VerticalAlign.Top
                tc.HorizontalAlign = HorizontalAlign.Center

                Dim ibtn As New Telerik.Web.UI.RadButton
                ibtn.ID = "ibtnUser_" & u.ID
                ibtn.Image.ImageUrl = u.ImageUrl
                ibtn.Image.IsBackgroundImage = True
                ibtn.Height = New Unit(iconSize, UnitType.Pixel)
                ibtn.Width = New Unit(iconSize, UnitType.Pixel)
                AddHandler ibtn.Click, AddressOf ibtnUser_Click

                Dim lit As New Literal
                lit.ID = "litUser_" & u.ID
                lit.Text = "<br/>"

                Dim lit2 As New Literal
                lit2.ID = "litUser2_" & u.ID
                lit2.Text = "<br/>"

                Dim lbl As New Label
                lbl.ID = "lblUser_" & u.ID
                lbl.Text = u.Name

                tc.Controls.Add(lit)
                tc.Controls.Add(ibtn)
                tc.Controls.Add(lit2)
                tc.Controls.Add(lbl)

                Select Case u.Permissions
                    Case Enums.SystemUserPermissions.Administrator, Enums.SystemUserPermissions.SystemAdministrator
                        trAdmins.Cells.Add(tc)
                    Case Enums.SystemUserPermissions.User
                        trUsers.Cells.Add(tc)
                    Case Enums.SystemUserPermissions.Technician
                        trTechnicians.Cells.Add(tc)
                    Case Enums.SystemUserPermissions.Supervisor
                        trSupervisors.Cells.Add(tc)
                    Case Enums.SystemUserPermissions.Solvtopia
                        trSolvtopia.Cells.Add(tc)
                End Select
            Next

            Me.tblAdmins.Rows.Add(trAdmins)
            Me.tblUsers.Rows.Add(trUsers)
            Me.tblTechnicians.Rows.Add(trTechnicians)
            Me.tblSupervisors.Rows.Add(trSupervisors)
            Me.tblSolvtopia.Rows.Add(trSolvtopia)

        Catch ex As Exception
            ex.WriteToErrorLog
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub ibtnUser_Click(sender As Object, e As EventArgs)
        Dim ibtn As Telerik.Web.UI.RadButton = CType(sender, Telerik.Web.UI.RadButton)
        Dim usrId As Integer = ibtn.ID.Split("_"c)(1).ToInteger

        Response.Redirect("~/admin/UserEditor.aspx?t=" & CStr(Enums.TransactionType.Existing) & "&id=" & usrId, False)
    End Sub

    'Protected Sub rbUser_CheckedChanged(sender As Object, e As EventArgs)
    '    Dim rb As RadioButton = CType(sender, RadioButton)
    '    Me.EditId = rb.ID.Split("_"c)(1).ToString.ToInteger
    '    Me.SetupToolbar()
    'End Sub

    Private Sub ClearForm()

    End Sub

    Private Sub lnkDeleteUser_Click(sender As Object, e As EventArgs) Handles lnkDeleteUser.Click
        ShowInformationPopup(Enums.InformationPopupType.DeleteUser, Enums.InformationPopupButtons.YesNo, Me.EditId)
    End Sub

    Private Sub lnkClearUser_Click(sender As Object, e As EventArgs) Handles lnkClearUser.Click
        Me.ClearForm()
    End Sub

    Private Sub lnkNewUser_Click(sender As Object, e As EventArgs) Handles lnkNewUser.Click
        Response.Redirect("~/admin/UserEditor.aspx?t=" & CStr(Enums.TransactionType.[New]) & "&id=0", False)
    End Sub

End Class