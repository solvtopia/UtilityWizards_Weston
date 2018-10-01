<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.pbrProgress = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.chkCustomers = New System.Windows.Forms.CheckBox()
        Me.chkLocations = New System.Windows.Forms.CheckBox()
        Me.rbAllDupllicates = New System.Windows.Forms.RadioButton()
        Me.rbSelectedDuplicates = New System.Windows.Forms.RadioButton()
        Me.tmrTimer = New System.Windows.Forms.Timer(Me.components)
        Me.chkUseSandboxDb = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'btnProcess
        '
        Me.btnProcess.Location = New System.Drawing.Point(484, 12)
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
        Me.pbrProgress.Location = New System.Drawing.Point(13, 46)
        Me.pbrProgress.Name = "pbrProgress"
        Me.pbrProgress.Size = New System.Drawing.Size(449, 23)
        Me.pbrProgress.TabIndex = 2
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(13, 76)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(39, 13)
        Me.lblProgress.TabIndex = 3
        Me.lblProgress.Text = "Label1"
        '
        'chkCustomers
        '
        Me.chkCustomers.AutoSize = True
        Me.chkCustomers.Checked = True
        Me.chkCustomers.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCustomers.Location = New System.Drawing.Point(12, 138)
        Me.chkCustomers.Name = "chkCustomers"
        Me.chkCustomers.Size = New System.Drawing.Size(105, 17)
        Me.chkCustomers.TabIndex = 4
        Me.chkCustomers.Text = "Customers Table"
        Me.chkCustomers.UseVisualStyleBackColor = True
        '
        'chkLocations
        '
        Me.chkLocations.AutoSize = True
        Me.chkLocations.Checked = True
        Me.chkLocations.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLocations.Location = New System.Drawing.Point(12, 162)
        Me.chkLocations.Name = "chkLocations"
        Me.chkLocations.Size = New System.Drawing.Size(102, 17)
        Me.chkLocations.TabIndex = 5
        Me.chkLocations.Text = "Locations Table"
        Me.chkLocations.UseVisualStyleBackColor = True
        '
        'rbAllDupllicates
        '
        Me.rbAllDupllicates.AutoSize = True
        Me.rbAllDupllicates.Checked = True
        Me.rbAllDupllicates.Location = New System.Drawing.Point(12, 186)
        Me.rbAllDupllicates.Name = "rbAllDupllicates"
        Me.rbAllDupllicates.Size = New System.Drawing.Size(133, 17)
        Me.rbAllDupllicates.TabIndex = 6
        Me.rbAllDupllicates.TabStop = True
        Me.rbAllDupllicates.Text = "Combine All Duplicates"
        Me.rbAllDupllicates.UseVisualStyleBackColor = True
        '
        'rbSelectedDuplicates
        '
        Me.rbSelectedDuplicates.AutoSize = True
        Me.rbSelectedDuplicates.Location = New System.Drawing.Point(12, 210)
        Me.rbSelectedDuplicates.Name = "rbSelectedDuplicates"
        Me.rbSelectedDuplicates.Size = New System.Drawing.Size(188, 17)
        Me.rbSelectedDuplicates.TabIndex = 7
        Me.rbSelectedDuplicates.Text = "Combine Selected Duplicates Only"
        Me.rbSelectedDuplicates.UseVisualStyleBackColor = True
        '
        'tmrTimer
        '
        Me.tmrTimer.Enabled = True
        Me.tmrTimer.Interval = 60000
        '
        'chkUseSandboxDb
        '
        Me.chkUseSandboxDb.AutoSize = True
        Me.chkUseSandboxDb.Location = New System.Drawing.Point(484, 52)
        Me.chkUseSandboxDb.Name = "chkUseSandboxDb"
        Me.chkUseSandboxDb.Size = New System.Drawing.Size(90, 17)
        Me.chkUseSandboxDb.TabIndex = 8
        Me.chkUseSandboxDb.Text = "Use Sandbox"
        Me.chkUseSandboxDb.UseVisualStyleBackColor = True
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(596, 255)
        Me.Controls.Add(Me.chkUseSandboxDb)
        Me.Controls.Add(Me.rbSelectedDuplicates)
        Me.Controls.Add(Me.rbAllDupllicates)
        Me.Controls.Add(Me.chkLocations)
        Me.Controls.Add(Me.chkCustomers)
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
    Friend WithEvents chkCustomers As CheckBox
    Friend WithEvents chkLocations As CheckBox
    Friend WithEvents rbAllDupllicates As RadioButton
    Friend WithEvents rbSelectedDuplicates As RadioButton
    Friend WithEvents tmrTimer As Timer
    Friend WithEvents chkUseSandboxDb As CheckBox
End Class
