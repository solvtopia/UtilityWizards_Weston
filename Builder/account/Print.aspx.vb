Imports System.Xml
Imports UtilityWizards.CommonCore.Common
Imports UtilityWizards.CommonCore.Shared.Common
Imports UtilityWizards.CommonCore.Shared.Xml
Imports Telerik.Web.UI
Imports System.Configuration.ConfigurationManager

Public Class Print
    Inherits System.Web.UI.Page

#Region "Properties"

    Private ReadOnly Property ModId As Integer
        Get
            Return Request.QueryString("modid").ToInteger
        End Get
    End Property
    Private ReadOnly Property RecordId As Integer
        Get
            Return Request.QueryString("id").ToInteger
        End Get
    End Property
    Private ReadOnly Property Is811Module As Boolean
        Get
            Return Me.ActiveModule.Name.Contains("811")
        End Get
    End Property
    Private ReadOnly Property UserId As Integer
        Get
            Return Request.QueryString("usr").ToInteger
        End Get
    End Property
    Private ReadOnly Property ActiveUser As SystemUser
        Get
            Return New SystemUser(Me.UserId, App.UseSandboxDb)
        End Get
    End Property
    Private ReadOnly Property ActiveClient As SystemClient
        Get
            Return New SystemClient(Me.ActiveUser.ClientID, App.UseSandboxDb)
        End Get
    End Property
    Private ReadOnly Property ActiveModule As SystemModule
        Get
            Return New SystemModule(Me.ModId, App.UseSandboxDb)
        End Get
    End Property
    Private ReadOnly Property RootFolderQuestions() As List(Of SystemQuestion)
        Get
            Return LoadModuleQuestions(Me.ActiveModule.FolderID)
        End Get
    End Property
    Private ReadOnly Property ActiveModuleQuestions() As List(Of SystemQuestion)
        Get
            Return LoadModuleQuestions(Me.ActiveModule.ID)
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LoadQuestions()

        If Not IsPostBack Then
            Me.LoadData()

            'Me.LoadCustomerInfo()

            Me.SetupForm()
        End If
    End Sub

    Private Sub SetupForm()
        Me.txtTechComments.Enabled = True

        ' setup the form for the 811 locates
        If Me.Is811Module Then
            Me.RadMap1.Visible = False
            Me.txtPrintView.Visible = True
            Me.lblAcctNum.Visible = False
            Me.txtAcctNumber.Visible = False
            Me.lblLocationNum.Visible = False
            Me.txtLocationNum.Visible = False
            Me.tbl811SignOff.Visible = True
        Else
            Me.RadMap1.Visible = True
            Me.txtPrintView.Visible = False
            Me.lblAcctNum.Visible = True
            Me.txtAcctNumber.Visible = True
            Me.lblLocationNum.Visible = True
            Me.txtLocationNum.Visible = True
            Me.tbl811SignOff.Visible = False
        End If
    End Sub

    Private Sub LoadQuestions()
        If Me.RootFolderQuestions.Count > 0 Then
            Dim folderName As String = GetFolderName(Me.ActiveModule.FolderID)
            Me.LoadQuestions(Me.RootFolderQuestions, tblFolderQuestions, folderName.Replace(" ", "_"))
        Else
            Me.tblFolderQuestions.Visible = False
        End If
        Me.LoadQuestions(Me.ActiveModuleQuestions, tblModuleQuestions, "")

        ' build the list of names for the 811 tickets
        Me.tbl811SignOff.Rows.Clear()
        If Me.Is811Module Then
            Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

            Try
                Dim hr As New TableRow
                Dim htc As New TableCell
                htc.Text = "Ticket Sign-off"
                hr.Cells.Add(htc)
                Me.tbl811SignOff.Rows.Add(hr)

                Dim notifyIDs As New List(Of Integer)
                Dim cmd As New SqlClient.SqlCommand("SELECT [NotifyIDs] FROM [811Settings] WHERE [ClientID] = " & Me.ActiveClient.ID & ";", cn)
                If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
                Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
                If rs.Read Then
                    Dim tmp As List(Of String) = rs("NotifyIDs").ToString.Split("|"c).ToList
                    For Each s As String In tmp
                        If IsNumeric(s) Then notifyIDs.Add(s.ToInteger)
                    Next
                End If
                cmd.Cancel()
                rs.Close()

                For Each i As Integer In notifyIDs
                    Dim usr As New SystemUser(i, App.UseSandboxDb)
                    Dim tr1 As New TableRow
                    Dim tc1 As New TableCell
                    tc1.Text = usr.Name
                    tr1.Cells.Add(tc1)
                    Me.tbl811SignOff.Rows.Add(tr1)
                    Dim tr2 As New TableRow
                    Dim tc2 As New TableCell
                    Dim ddl As New Controls.Labels.DataLabel
                    ddl.ID = "ddl811SignOff_" & usr.ID
                    'ddl.Items.Add(New RadComboBoxItem("- select one -", "- select one -"))
                    'ddl.Items.Add(New RadComboBoxItem("Clear And No Conflict", "Clear And No Conflict"))
                    'ddl.Items.Add(New RadComboBoxItem("Marked", "Marked"))
                    'ddl.Width = New Unit(100, UnitType.Percentage)
                    ddl.XmlPath = ""
                    tc2.Controls.Add(ddl)
                    tr2.Cells.Add(tc2)
                    Me.tbl811SignOff.Rows.Add(tr2)
                Next

            Catch ex As Exception
                ex.WriteToErrorLog(New ErrorLogEntry(Me.ActiveUser.ID, Me.ActiveClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
            Finally
                cn.Close()
            End Try
        End If
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
                'Select Case q.Type
                '    Case Enums.SystemQuestionType.CheckBox
                Dim ctl As New Controls.Labels.DataLabel
                ctl.ID = "chk_" & q.ID
                'chk.Required = q.Required
                ctl.XmlPath = xmlPath
                ctl.DataFieldName = q.DataFieldName
                'chk.AutoPostBack = (q.Rule <> "")
                tc2.Controls.Add(ctl)

                '    Case Enums.SystemQuestionType.DropDownList
                '        Dim ddl As New Controls.DropDownLists.DropDownList
                '        ddl.ID = "ddl_" & q.ID
                '        ddl.Width = New Unit(100, UnitType.Percentage)
                '        For Each itm As String In q.Values
                '            ddl.Items.Add(New RadComboBoxItem(itm, itm))
                '        Next
                '        ddl.Required = q.Required
                '        ddl.XmlPath = xmlPath
                '        ddl.DataFieldName = q.DataFieldName
                '        ddl.AutoPostBack = (q.Rule <> "")
                '        tc2.Controls.Add(ddl)

                '    Case Enums.SystemQuestionType.MemoField
                '        Dim txt As New Controls.TextBoxes.TextBox
                '        txt.ID = "txt_" & q.ID
                '        txt.Width = New Unit(100, UnitType.Percentage)
                '        txt.Rows = 3
                '        txt.TextMode = InputMode.MultiLine
                '        txt.Required = q.Required
                '        txt.XmlPath = xmlPath
                '        txt.DataFieldName = q.DataFieldName
                '        txt.AutoPostBack = (q.Rule <> "")
                '        tc2.Controls.Add(txt)

                '    Case Enums.SystemQuestionType.TextBox
                '        Dim txt As New Controls.TextBoxes.TextBox
                '        txt.ID = "txt_" & q.ID
                '        txt.Width = New Unit(100, UnitType.Percentage)
                '        txt.MaxLength = 255
                '        txt.Required = q.Required
                '        txt.XmlPath = xmlPath
                '        txt.DataFieldName = q.DataFieldName
                '        txt.AutoPostBack = (q.Rule <> "")
                '        tc2.Controls.Add(txt)

                '    Case Enums.SystemQuestionType.NumericTextBox
                '        Dim txt As New Controls.TextBoxes.NumericTextBox
                '        txt.ID = "txt_" & q.ID
                '        txt.Width = New Unit(100, UnitType.Percentage)
                '        txt.Required = q.Required
                '        txt.XmlPath = xmlPath
                '        txt.DataFieldName = q.DataFieldName
                '        txt.NumberFormat.DecimalDigits = 0
                '        txt.AutoPostBack = (q.Rule <> "")
                '        tc2.Controls.Add(txt)
                'End Select
                tc2.VerticalAlign = VerticalAlign.Top
                tr2.Cells.Add(tc2)

                tbl.Rows.Add(tr2)
            Next
        End If
    End Sub

    Private Sub LoadData()
        Dim cn As New SqlClient.SqlConnection(Common.ConnectionString)

        Try
            Dim cmd As New SqlClient.SqlCommand("Select [ID], [xmlData], [xUserEmail] FROM [vwModuleData] WHERE [ID] = " & Me.RecordId, cn)
            If cmd.Connection.State = ConnectionState.Closed Then cmd.Connection.Open()
            Dim rs As SqlClient.SqlDataReader = cmd.ExecuteReader
            If rs.Read Then
                Me.FromXml(rs("xmlData").ToString)
            End If
            rs.Close()
            cmd.Cancel()

            'If Me.txtLocationNum.Text = "" Then Me.txtLocationNum.Text = Me.LocationNum

        Catch ex As Exception
            ex.WriteToErrorLog(New ErrorLogEntry(Me.ActiveUser.ID, Me.ActiveClient.ID, Enums.ProjectName.Builder, App.UseSandboxDb))
        Finally
            cn.Close()
        End Try
    End Sub

End Class