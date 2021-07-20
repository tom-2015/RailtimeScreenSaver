Imports System.IO
Imports System.Reflection

Public Class FrmOptions

    Private Sub FrmOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TemplateProperties.SelectedObject = Template
        DataGrabberProperties.SelectedObject = TrainDataGrabber
        If TypeOf Template Is SNCBClassicTemplate Then
            CmbTemplate.SelectedIndex = 0
        ElseIf TypeOf Template Is SNCBTemplate Then
            CmbTemplate.SelectedIndex = 1
        End If
        Dim Grabber As String = Main.Settings.GetSetting("general", "data_grabber", "m.nmbs.be")
        For i As Integer = 0 To CmbDataSource.Items.Count - 1
            If CmbDataSource.Items(i).ToString() = Grabber Then CmbDataSource.SelectedIndex = i
        Next
        LblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version().ToString()

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.



    End Sub

    Private Sub CmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdCancel.Click
        Me.Close()
    End Sub

    Private Sub CmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSave.Click
        Template.SaveSettings(Main.Settings)
        TrainDataGrabber.SaveSettings(Main.Settings)
        Main.Settings.SaveSetting("general", "template", CmbTemplate.Text)
        Main.Settings.SaveSetting("general", "data_grabber", CmbDataSource.Text)
        Main.Settings.Save(SettingsFile)
        Me.Close()
    End Sub

    Private Sub CmbDataSource_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbDataSource.SelectedIndexChanged
        'add code to change data grabber
    End Sub

    Private Sub CmbTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbTemplate.SelectedIndexChanged
        Select Case CmbTemplate.Text
            Case "SNCB Classic"
                If Not TypeOf Template Is SNCBClassicTemplate Then
                    Template.SaveSettings(Main.Settings)
                    Template = New SNCBClassicTemplate(Main.Settings)
                End If
            Case "SNCB"
                If Not TypeOf Template Is SNCBTemplate Then
                    Template.SaveSettings(Main.Settings)
                    Template = New SNCBTemplate(Main.Settings)
                End If
        End Select
        TemplateProperties.SelectedObject = Template
    End Sub

    Private Sub DataGrabberProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGrabberProperties.Click

    End Sub

    Private Sub DataGrabberProperties_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles DataGrabberProperties.PropertyValueChanged
        TrainDataGrabber.RefreshTrainData(Now())
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Try
            Dim sInfo As New ProcessStartInfo("https://github.com/tom-2015/RailtimeScreenSaver")
            Process.Start(sInfo)
        Catch ex As Exception
            MsgBox("Oops error " & ex.Message())
        End Try
    End Sub
End Class