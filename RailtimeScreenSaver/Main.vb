Imports System.IO
Imports System.Threading

Module Main

    Public Enum ScreenSaverRunningModes
        NormalApplication
        ScreenSaverEnabled
        Configuration
        Preview
    End Enum

    Public Settings As Settings
    Public ExitApplication As Boolean
    Public RefreshThread As Thread
    Public TrainDataGrabber As TrainDataGrabberAPI
    Public Template As ScreenSaverTemplate
    Public ScreenSaverForms As List(Of FrmSaver)
    Public RunningMode As ScreenSaverRunningModes
    Public SettingsFile As String

    Private Const SPI_SCREENSAVERRUNNING As Integer = &H61

    Private Declare Function SystemParametersInfo Lib "user32" Alias "SystemParametersInfoA" (ByVal uAction As Integer, ByVal uParam As Integer, ByVal lpvParam As Integer, ByVal fuWinIni As Integer) As Integer

    Public Sub Main()
        Dim CmdLine As String = LCase(Command)
        Dim StartupType As String = LCase(Left(CmdLine, 2))

        SettingsFile = Path.GetDirectoryName(Application.ExecutablePath) & Path.DirectorySeparatorChar & "railtime_settings.xml"
        Settings = New Settings()
        Settings.Load(SettingsFile)

        Select Case LCase(Settings.GetSetting("general", "data_grabber", "m.nmbs.be"))
            Case "m.nmbs.be"
                TrainDataGrabber = New SNCB(Settings)
            Case Else
                TrainDataGrabber = New SNCB(Settings)
        End Select

        Select Case LCase(Settings.GetSetting("general", "template", "sncb"))
            Case "sncb"
                Template = New SNCBTemplate(Settings)
            Case "sncb classic"
                Template = New SNCBClassicTemplate(Settings)
            Case Else
                Template = New SNCBClassicTemplate(Settings)
        End Select

        ScreenSaverForms = New List(Of FrmSaver)

        Dim Saver As New FrmSaver(0)

        ScreenSaverForms.Add(Saver) 'for /p (preview) only load 1 screen saver form!

        Select Case StartupType
            Case "/c"
                RunningMode = ScreenSaverRunningModes.Configuration
                FrmOptions.ShowDialog()
                Return
            Case "/t" 'test mode, show options and screen saver window
                RunningMode = ScreenSaverRunningModes.Configuration
                FrmOptions.Show()
            Case "/s"
                Cursor.Hide()
                SetScreenSaverRunning(True)
                RunningMode = ScreenSaverRunningModes.ScreenSaverEnabled
                For Each ScreenSaverForm As FrmSaver In ScreenSaverForms
                    ScreenSaverForm.FormBorderStyle = FormBorderStyle.None
                    ScreenSaverForm.TopMost = True
                    ScreenSaverForm.WindowState = FormWindowState.Maximized
                Next
            Case "/p"
                ScreenSaverForms(0).ShowInTaskbar = False
                RunningMode = ScreenSaverRunningModes.Configuration
                Dim Hwnd As Integer = Val(Trim(Mid(Command, 3)))
                If Hwnd > 0 Then
                    If IsWindows() Then
                        ScreenSaverForms(0).FormBorderStyle = FormBorderStyle.None 'important do this before DoPreviewMode or the program will not detect the close window event from the preview dialog and continue running when other is selected
                        DoPreviewMode(Hwnd, ScreenSaverForms(0).Handle)
                        ScreenSaverForms(0).WindowState = FormWindowState.Maximized
                    End If
                End If
            Case "/w"
                TrainDataGrabber.Station = Trim(Mid(Command, 3))
        End Select

        RefreshThread = New Thread(AddressOf UpdateTrainDataThreadSub)
        RefreshThread.Start()

        For Each ScreenSaverForm As FrmSaver In ScreenSaverForms
            ScreenSaverForm.Show()
        Next

        Do Until ExitApplication
            Application.DoEvents()
            Thread.Sleep(10)
        Loop

        Select Case RunningMode
            Case ScreenSaverRunningModes.ScreenSaverEnabled
                SetScreenSaverRunning(False)
                Cursor.Show()
        End Select

    End Sub

    ''' <summary>
    ''' Exits the application (when key is pressed, mouse move,...), if force is true it will always exit
    ''' if Force is false it will only exit if running as screen saver
    ''' </summary>
    ''' <param name="Force"></param>
    ''' <remarks></remarks>
    Public Sub ExitScreenSaver(Optional ByVal Force As Boolean = False)
        If Not ExitApplication Then
            If Force OrElse RunningMode = ScreenSaverRunningModes.ScreenSaverEnabled Then
                ExitApplication = True
                For Each SaverForm As FrmSaver In ScreenSaverForms
                    If Not SaverForm.IsClosing Then
                        SaverForm.Close()
                    End If
                Next
                RefreshThread.Abort()
            End If
        End If
    End Sub

    Public Sub UpdateTrainDataThreadSub()
        Try
            Dim Timeout As Integer = 0
            Do Until ExitApplication
                Try
                    If Timeout <= 0 Then
                        TrainDataGrabber.RefreshTrainData(Now())
                        For Each SaverForm As FrmSaver In ScreenSaverForms
                            SaverForm.Invalidate()
                        Next
                        Timeout = TrainDataGrabber.RefreshDelay
                    End If
                    Timeout = Timeout - 1
                    Thread.Sleep(1000)
                Catch Ex As Exception
                    If TypeOf Ex Is ThreadAbortException Then Throw Ex
                    Console.WriteLine("Error update train data " & Ex.ToString())
                    Debug.Print("Error update train data " & Ex.ToString())
                End Try
            Loop
        Catch tabe As ThreadAbortException
            Exit Sub
        End Try
    End Sub

    Public Function IsWindows() As Boolean
        Select Case Environment.OSVersion.Platform
            Case PlatformID.Win32NT, PlatformID.Win32S, PlatformID.Win32Windows, PlatformID.WinCE
                Return True
        End Select
        Return False
    End Function

    Public Sub SetScreenSaverRunning(ByVal Value As Boolean)
        If IsWindows() Then
            If Value Then
                SystemParametersInfo(SPI_SCREENSAVERRUNNING, 1&, 0&, 0&)
            Else
                SystemParametersInfo(SPI_SCREENSAVERRUNNING, 0&, 0&, 0&)
            End If
        End If
    End Sub

End Module
