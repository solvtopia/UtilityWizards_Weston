<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class fMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.pbrProgress = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.tmrTimer = New System.Windows.Forms.Timer(Me.components)
        Me.lblStatus2 = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.rbAppend = New System.Windows.Forms.RadioButton()
        Me.rbTruncate = New System.Windows.Forms.RadioButton()
        Me.chkUseSandboxDb = New System.Windows.Forms.CheckBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.btnRenameProcessed = New System.Windows.Forms.Button()
        Me.btnPresignedUrls = New System.Windows.Forms.Button()
        Me.txtExtras = New System.Windows.Forms.TextBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnProcess
        '
        Me.btnProcess.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnProcess.Location = New System.Drawing.Point(639, 12)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(100, 23)
        Me.btnProcess.TabIndex = 0
        Me.btnProcess.Text = "Process Files"
        Me.btnProcess.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(12, 17)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(39, 13)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Text = "Label1"
        '
        'pbrProgress
        '
        Me.pbrProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbrProgress.Location = New System.Drawing.Point(12, 66)
        Me.pbrProgress.Name = "pbrProgress"
        Me.pbrProgress.Size = New System.Drawing.Size(607, 23)
        Me.pbrProgress.TabIndex = 2
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(12, 96)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(39, 13)
        Me.lblProgress.TabIndex = 3
        Me.lblProgress.Text = "Label1"
        '
        'tmrTimer
        '
        Me.tmrTimer.Enabled = True
        Me.tmrTimer.Interval = 60000
        '
        'lblStatus2
        '
        Me.lblStatus2.AutoSize = True
        Me.lblStatus2.Location = New System.Drawing.Point(12, 39)
        Me.lblStatus2.Name = "lblStatus2"
        Me.lblStatus2.Size = New System.Drawing.Size(39, 13)
        Me.lblStatus2.TabIndex = 8
        Me.lblStatus2.Text = "Label1"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 123)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(727, 361)
        Me.TabControl1.TabIndex = 10
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.txtLog)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(719, 335)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Log"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(6, 6)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(707, 323)
        Me.txtLog.TabIndex = 11
        Me.txtLog.WordWrap = False
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.rbAppend)
        Me.TabPage2.Controls.Add(Me.rbTruncate)
        Me.TabPage2.Controls.Add(Me.chkUseSandboxDb)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(719, 335)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Settings"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'rbAppend
        '
        Me.rbAppend.AutoSize = True
        Me.rbAppend.Checked = True
        Me.rbAppend.Location = New System.Drawing.Point(177, 30)
        Me.rbAppend.Name = "rbAppend"
        Me.rbAppend.Size = New System.Drawing.Size(105, 17)
        Me.rbAppend.TabIndex = 10
        Me.rbAppend.TabStop = True
        Me.rbAppend.Text = "Append Records"
        Me.rbAppend.UseVisualStyleBackColor = True
        '
        'rbTruncate
        '
        Me.rbTruncate.AutoSize = True
        Me.rbTruncate.Location = New System.Drawing.Point(6, 30)
        Me.rbTruncate.Name = "rbTruncate"
        Me.rbTruncate.Size = New System.Drawing.Size(165, 17)
        Me.rbTruncate.TabIndex = 9
        Me.rbTruncate.Text = "Truncate Tables before Insert"
        Me.rbTruncate.UseVisualStyleBackColor = True
        '
        'chkUseSandboxDb
        '
        Me.chkUseSandboxDb.AutoSize = True
        Me.chkUseSandboxDb.Checked = True
        Me.chkUseSandboxDb.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUseSandboxDb.Location = New System.Drawing.Point(6, 6)
        Me.chkUseSandboxDb.Name = "chkUseSandboxDb"
        Me.chkUseSandboxDb.Size = New System.Drawing.Size(130, 17)
        Me.chkUseSandboxDb.TabIndex = 8
        Me.chkUseSandboxDb.Text = "Update Sandbox Only"
        Me.chkUseSandboxDb.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.txtExtras)
        Me.TabPage3.Controls.Add(Me.btnPresignedUrls)
        Me.TabPage3.Controls.Add(Me.btnRenameProcessed)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(719, 335)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Extras"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'btnRenameProcessed
        '
        Me.btnRenameProcessed.Location = New System.Drawing.Point(3, 19)
        Me.btnRenameProcessed.Name = "btnRenameProcessed"
        Me.btnRenameProcessed.Size = New System.Drawing.Size(161, 23)
        Me.btnRenameProcessed.TabIndex = 0
        Me.btnRenameProcessed.Text = "Rename Processed to Txt"
        Me.btnRenameProcessed.UseVisualStyleBackColor = True
        '
        'btnPresignedUrls
        '
        Me.btnPresignedUrls.Location = New System.Drawing.Point(170, 19)
        Me.btnPresignedUrls.Name = "btnPresignedUrls"
        Me.btnPresignedUrls.Size = New System.Drawing.Size(150, 23)
        Me.btnPresignedUrls.TabIndex = 1
        Me.btnPresignedUrls.Text = "Generate Presigned Urls"
        Me.btnPresignedUrls.UseVisualStyleBackColor = True
        '
        'txtExtras
        '
        Me.txtExtras.Location = New System.Drawing.Point(3, 48)
        Me.txtExtras.Multiline = True
        Me.txtExtras.Name = "txtExtras"
        Me.txtExtras.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtExtras.Size = New System.Drawing.Size(713, 284)
        Me.txtExtras.TabIndex = 2
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(751, 496)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.lblStatus2)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.pbrProgress)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnProcess)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "fMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Utility Wizards - File Processor"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnProcess As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents pbrProgress As ProgressBar
    Friend WithEvents lblProgress As Label
    Friend WithEvents tmrTimer As Timer
    Friend WithEvents lblStatus2 As Label
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents txtLog As TextBox
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents rbAppend As RadioButton
    Friend WithEvents rbTruncate As RadioButton
    Friend WithEvents chkUseSandboxDb As CheckBox
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents btnRenameProcessed As Button
    Friend WithEvents btnPresignedUrls As Button
    Friend WithEvents txtExtras As TextBox
End Class
