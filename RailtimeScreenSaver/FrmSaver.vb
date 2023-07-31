Imports System.Net
Imports System.IO
Imports System.Xml

Public Class FrmSaver

    Private nMouseMoves As Integer

    Protected m_DisplayNr As Integer
    Public IsClosing As Boolean

    Public Sub New(ByVal DisplayNr As Integer)
        MyBase.New()
        InitializeComponent()
        m_DisplayNr = DisplayNr
    End Sub

    Private Sub FrmSaver_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        IsClosing = True
        ExitScreenSaver(True)
    End Sub

    Private Sub FrmSaver_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub FrmSaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        ExitScreenSaver()
    End Sub

    Private Sub FrmSaver_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        ExitScreenSaver()
    End Sub

    Private Sub FrmSaver_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick
        ExitScreenSaver()
    End Sub

    Private Sub FrmSaver_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If RunningMode = ScreenSaverRunningModes.ScreenSaverEnabled Then
            ExitScreenSaver()
        ElseIf RunningMode = ScreenSaverRunningModes.NormalApplication OrElse RunningMode = ScreenSaverRunningModes.Configuration Then
            If e.Button = Windows.Forms.MouseButtons.Right Then
                RightClickContextMenu.Show(Me, e.Location)
            End If
        End If
    End Sub

    Private Sub FrmSaver_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        Static nTimeDelay As Integer
        nMouseMoves = nMouseMoves + 1

        'There will probably be one or two MouseMove events at
        'startup that must be ignored.
        'Change the value for more or less mouse sensitivity.

        If nMouseMoves = 50 Then
            ExitScreenSaver()
        End If

        'MouseMove events are cumulative, so over time there
        'might be mouse creep.  Reset the counter if more
        'than 10 seconds have elapsed since mouse movement
        'began.
        If nTimeDelay = 0 Then
            nTimeDelay = DateAndTime.Timer
        ElseIf DateAndTime.Timer - nTimeDelay > 1 Then
            nTimeDelay = 0
            nMouseMoves = 0
        End If
    End Sub

    Private Sub FrmSaver_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        ExitScreenSaver()
    End Sub

    Private Sub FrmSaver_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        'Try
        Template.Render(m_DisplayNr, e.Graphics, Me.ClientSize(), TrainDataGrabber)
        ' Catch Ex As Exception
        ' Debug.Print(Ex.ToString())
        '     Console.WriteLine(Ex.ToString())
        ' End Try
    End Sub

    Private Sub FrmSaver_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Invalidate()
    End Sub

    Private Sub TmrUpdateScreen_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TmrUpdateScreen.Tick
        Invalidate()
    End Sub

    Private Sub ConfigToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigToolStripMenuItem.Click
        FrmOptions.Show()
    End Sub
End Class
