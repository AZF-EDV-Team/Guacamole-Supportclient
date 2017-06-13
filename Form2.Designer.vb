<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtbIP = New System.Windows.Forms.TextBox()
        Me.txtbUser = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtbDBName = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtbPass = New System.Windows.Forms.MaskedTextBox()
        Me.lblBGW = New System.Windows.Forms.Label()
        Me.timLbl = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IP:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "User:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 88)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Passwort:"
        '
        'txtbIP
        '
        Me.txtbIP.Location = New System.Drawing.Point(74, 33)
        Me.txtbIP.Name = "txtbIP"
        Me.txtbIP.Size = New System.Drawing.Size(198, 20)
        Me.txtbIP.TabIndex = 1
        '
        'txtbUser
        '
        Me.txtbUser.Location = New System.Drawing.Point(74, 59)
        Me.txtbUser.Name = "txtbUser"
        Me.txtbUser.Size = New System.Drawing.Size(198, 20)
        Me.txtbUser.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(63, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Datenbank:"
        '
        'txtbDBName
        '
        Me.txtbDBName.Location = New System.Drawing.Point(74, 112)
        Me.txtbDBName.Name = "txtbDBName"
        Me.txtbDBName.Size = New System.Drawing.Size(198, 20)
        Me.txtbDBName.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 115)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "DB Name:"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(12, 138)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(121, 23)
        Me.btnSave.TabIndex = 5
        Me.btnSave.Text = "Speichern"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(156, 139)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(116, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Abbrechen"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtbPass
        '
        Me.txtbPass.Location = New System.Drawing.Point(74, 85)
        Me.txtbPass.Name = "txtbPass"
        Me.txtbPass.Size = New System.Drawing.Size(198, 20)
        Me.txtbPass.TabIndex = 3
        Me.txtbPass.UseSystemPasswordChar = True
        '
        'lblBGW
        '
        Me.lblBGW.AutoSize = True
        Me.lblBGW.Location = New System.Drawing.Point(13, 168)
        Me.lblBGW.Name = "lblBGW"
        Me.lblBGW.Size = New System.Drawing.Size(0, 13)
        Me.lblBGW.TabIndex = 9
        '
        'timLbl
        '
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 189)
        Me.Controls.Add(Me.lblBGW)
        Me.Controls.Add(Me.txtbPass)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtbDBName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtbUser)
        Me.Controls.Add(Me.txtbIP)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(300, 227)
        Me.MinimumSize = New System.Drawing.Size(300, 227)
        Me.Name = "Form2"
        Me.Text = "Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtbIP As System.Windows.Forms.TextBox
    Friend WithEvents txtbUser As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtbDBName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtbPass As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lblBGW As System.Windows.Forms.Label
    Friend WithEvents timLbl As System.Windows.Forms.Timer
End Class
