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
        Me.btnAWSUpload = New System.Windows.Forms.Button()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'btnAWSUpload
        '
        Me.btnAWSUpload.Location = New System.Drawing.Point(12, 12)
        Me.btnAWSUpload.Name = "btnAWSUpload"
        Me.btnAWSUpload.Size = New System.Drawing.Size(105, 23)
        Me.btnAWSUpload.TabIndex = 0
        Me.btnAWSUpload.Text = "Upload to AWS"
        Me.btnAWSUpload.UseVisualStyleBackColor = True
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.Multiselect = True
        Me.OpenFileDialog.Title = "Select a file to Upload"
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(306, 239)
        Me.Controls.Add(Me.btnAWSUpload)
        Me.Name = "fMain"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnAWSUpload As Button
    Friend WithEvents OpenFileDialog As OpenFileDialog
End Class
