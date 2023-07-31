<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmOptions))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.CmbDataSource = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGrabberProperties = New System.Windows.Forms.PropertyGrid()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TemplateProperties = New System.Windows.Forms.PropertyGrid()
        Me.CmbTemplate = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CmdCancel = New System.Windows.Forms.Button()
        Me.CmdSave = New System.Windows.Forms.Button()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.LblVersion = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(337, 364)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.CmbDataSource)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.DataGrabberProperties)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(329, 338)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Source"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'CmbDataSource
        '
        Me.CmbDataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbDataSource.FormattingEnabled = True
        Me.CmbDataSource.Items.AddRange(New Object() {"m.nmbs.be", "api.irail.be"})
        Me.CmbDataSource.Location = New System.Drawing.Point(88, 9)
        Me.CmbDataSource.Name = "CmbDataSource"
        Me.CmbDataSource.Size = New System.Drawing.Size(228, 21)
        Me.CmbDataSource.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Get data from:"
        '
        'DataGrabberProperties
        '
        Me.DataGrabberProperties.Location = New System.Drawing.Point(6, 43)
        Me.DataGrabberProperties.Name = "DataGrabberProperties"
        Me.DataGrabberProperties.Size = New System.Drawing.Size(317, 285)
        Me.DataGrabberProperties.TabIndex = 1
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TemplateProperties)
        Me.TabPage2.Controls.Add(Me.CmbTemplate)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(329, 338)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Template"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TemplateProperties
        '
        Me.TemplateProperties.Location = New System.Drawing.Point(0, 39)
        Me.TemplateProperties.Name = "TemplateProperties"
        Me.TemplateProperties.Size = New System.Drawing.Size(326, 296)
        Me.TemplateProperties.TabIndex = 2
        '
        'CmbTemplate
        '
        Me.CmbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbTemplate.FormattingEnabled = True
        Me.CmbTemplate.Items.AddRange(New Object() {"SNCB Classic", "SNCB", "SNCB new look 2022"})
        Me.CmbTemplate.Location = New System.Drawing.Point(66, 10)
        Me.CmbTemplate.Name = "CmbTemplate"
        Me.CmbTemplate.Size = New System.Drawing.Size(252, 21)
        Me.CmbTemplate.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Template:"
        '
        'CmdCancel
        '
        Me.CmdCancel.Location = New System.Drawing.Point(150, 375)
        Me.CmdCancel.Name = "CmdCancel"
        Me.CmdCancel.Size = New System.Drawing.Size(86, 31)
        Me.CmdCancel.TabIndex = 6
        Me.CmdCancel.Text = "Cancel"
        Me.CmdCancel.UseVisualStyleBackColor = True
        '
        'CmdSave
        '
        Me.CmdSave.Location = New System.Drawing.Point(242, 375)
        Me.CmdSave.Name = "CmdSave"
        Me.CmdSave.Size = New System.Drawing.Size(86, 31)
        Me.CmdSave.TabIndex = 5
        Me.CmdSave.Text = "Save"
        Me.CmdSave.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(12, 375)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(46, 13)
        Me.LinkLabel1.TabIndex = 7
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Website"
        '
        'LblVersion
        '
        Me.LblVersion.AutoSize = True
        Me.LblVersion.Location = New System.Drawing.Point(12, 393)
        Me.LblVersion.Name = "LblVersion"
        Me.LblVersion.Size = New System.Drawing.Size(22, 13)
        Me.LblVersion.TabIndex = 8
        Me.LblVersion.Text = "1.2"
        '
        'FrmOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(337, 418)
        Me.Controls.Add(Me.LblVersion)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.CmdCancel)
        Me.Controls.Add(Me.CmdSave)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmOptions"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Screen saver options"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGrabberProperties As System.Windows.Forms.PropertyGrid
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents CmbDataSource As System.Windows.Forms.ComboBox
    Friend WithEvents CmbTemplate As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TemplateProperties As System.Windows.Forms.PropertyGrid
    Friend WithEvents CmdCancel As System.Windows.Forms.Button
    Friend WithEvents CmdSave As System.Windows.Forms.Button
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents LblVersion As System.Windows.Forms.Label
End Class
