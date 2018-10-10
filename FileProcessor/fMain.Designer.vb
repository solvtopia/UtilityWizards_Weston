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
        Me.chkUseSandboxDb = New System.Windows.Forms.CheckBox()
        Me.rbTruncate = New System.Windows.Forms.RadioButton()
        Me.rbAppend = New System.Windows.Forms.RadioButton()
        Me.tmrTimer = New System.Windows.Forms.Timer(Me.components)
        Me.lblStatus2 = New System.Windows.Forms.Label()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnProcess
        '
        Me.btnProcess.Location = New System.Drawing.Point(545, 12)
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
        Me.pbrProgress.Location = New System.Drawing.Point(12, 66)
        Me.pbrProgress.Name = "pbrProgress"
        Me.pbrProgress.Size = New System.Drawing.Size(513, 23)
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
        'chkUseSandboxDb
        '
        Me.chkUseSandboxDb.AutoSize = True
        Me.chkUseSandboxDb.Checked = True
        Me.chkUseSandboxDb.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUseSandboxDb.Location = New System.Drawing.Point(12, 122)
        Me.chkUseSandboxDb.Name = "chkUseSandboxDb"
        Me.chkUseSandboxDb.Size = New System.Drawing.Size(90, 17)
        Me.chkUseSandboxDb.TabIndex = 5
        Me.chkUseSandboxDb.Text = "Use Sandbox"
        Me.chkUseSandboxDb.UseVisualStyleBackColor = True
        '
        'rbTruncate
        '
        Me.rbTruncate.AutoSize = True
        Me.rbTruncate.Location = New System.Drawing.Point(12, 146)
        Me.rbTruncate.Name = "rbTruncate"
        Me.rbTruncate.Size = New System.Drawing.Size(165, 17)
        Me.rbTruncate.TabIndex = 6
        Me.rbTruncate.Text = "Truncate Tables before Insert"
        Me.rbTruncate.UseVisualStyleBackColor = True
        '
        'rbAppend
        '
        Me.rbAppend.AutoSize = True
        Me.rbAppend.Checked = True
        Me.rbAppend.Location = New System.Drawing.Point(183, 146)
        Me.rbAppend.Name = "rbAppend"
        Me.rbAppend.Size = New System.Drawing.Size(105, 17)
        Me.rbAppend.TabIndex = 7
        Me.rbAppend.TabStop = True
        Me.rbAppend.Text = "Append Records"
        Me.rbAppend.UseVisualStyleBackColor = True
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
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 169)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(633, 323)
        Me.txtLog.TabIndex = 9
        Me.txtLog.WordWrap = False
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(657, 504)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.lblStatus2)
        Me.Controls.Add(Me.rbAppend)
        Me.Controls.Add(Me.rbTruncate)
        Me.Controls.Add(Me.chkUseSandboxDb)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.pbrProgress)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnProcess)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "fMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Utility Wizards - File Processor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnProcess As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents pbrProgress As ProgressBar
    Friend WithEvents lblProgress As Label
    Friend WithEvents chkUseSandboxDb As CheckBox
    Friend WithEvents rbTruncate As RadioButton
    Friend WithEvents rbAppend As RadioButton
    Friend WithEvents tmrTimer As Timer
    Friend WithEvents lblStatus2 As Label
    Friend WithEvents txtLog As TextBox
End Class
