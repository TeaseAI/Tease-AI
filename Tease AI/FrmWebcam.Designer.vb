<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmWebcam
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
		Me.PictureBox1 = New System.Windows.Forms.PictureBox()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.videoDevices = New System.Windows.Forms.ComboBox()
		Me.Button1 = New System.Windows.Forms.Button()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		Me.SuspendLayout()
		'
		'PictureBox1
		'
		Me.PictureBox1.BackColor = System.Drawing.Color.Black
		Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PictureBox1.Location = New System.Drawing.Point(3, 16)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(649, 388)
		Me.PictureBox1.TabIndex = 1
		Me.PictureBox1.TabStop = False
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.PictureBox1)
		Me.GroupBox1.Location = New System.Drawing.Point(12, 39)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(655, 407)
		Me.GroupBox1.TabIndex = 2
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Preview"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(12, 15)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(74, 13)
		Me.Label1.TabIndex = 3
		Me.Label1.Text = "Video Device:"
		'
		'videoDevices
		'
		Me.videoDevices.DisplayMember = "Name"
		Me.videoDevices.FormattingEnabled = True
		Me.videoDevices.Location = New System.Drawing.Point(93, 12)
		Me.videoDevices.Name = "videoDevices"
		Me.videoDevices.Size = New System.Drawing.Size(493, 21)
		Me.videoDevices.TabIndex = 4
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(592, 12)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(75, 23)
		Me.Button1.TabIndex = 5
		Me.Button1.Text = "Update"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'FormMarkusWebcam
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(679, 458)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.videoDevices)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.GroupBox1)
		Me.Name = "FormMarkusWebcam"
		Me.Text = "Tease AI Camera View"
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents PictureBox1 As PictureBox
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents Label1 As Label
	Friend WithEvents videoDevices As ComboBox
	Friend WithEvents Button1 As Button
End Class
