<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSaver
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSaver))
        Me.TmrUpdateScreen = New System.Windows.Forms.Timer(Me.components)
        Me.RightClickContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RightClickContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'TmrUpdateScreen
        '
        Me.TmrUpdateScreen.Enabled = True
        Me.TmrUpdateScreen.Interval = 500
        '
        'RightClickContextMenu
        '
        Me.RightClickContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfigToolStripMenuItem})
        Me.RightClickContextMenu.Name = "ContextMenu"
        Me.RightClickContextMenu.Size = New System.Drawing.Size(153, 48)
        '
        'ConfigToolStripMenuItem
        '
        Me.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem"
        Me.ConfigToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ConfigToolStripMenuItem.Text = "Config"
        '
        'FrmSaver
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(782, 397)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmSaver"
        Me.Text = "Railtime"
        Me.RightClickContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TmrUpdateScreen As System.Windows.Forms.Timer
    Friend WithEvents RightClickContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ConfigToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
