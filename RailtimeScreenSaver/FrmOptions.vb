Imports System.IO

Public Class FrmOptions

    Private Sub FrmOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TemplateProperties.SelectedObject = Template
        DataGrabberProperties.SelectedObject = TrainDataGrabber
        If TypeOf Template Is SNCBClassicTemplate Then
            CmbTemplate.SelectedIndex = 0
        ElseIf TypeOf Template Is SNCBTemplate Then
            CmbTemplate.SelectedIndex = 1
        End If
        CmbDataSource.SelectedIndex = 0
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
End Class