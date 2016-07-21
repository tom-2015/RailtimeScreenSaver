Imports System.Xml

Public Class Settings

    Public Language As String
    Public Station As String

    Dim XmlDoc As New XmlDocument()

    Public Sub New()

    End Sub

    Public Sub Load(ByVal ConfigFile As String)
        If System.IO.File.Exists(ConfigFile) Then
            Try
                XmlDoc.Load(ConfigFile)
            Catch ex As Exception
                Debug.Print("Load settings error " & ex.Message)
            End Try
        End If
        If XmlDoc.SelectSingleNode("/configuration") Is Nothing Then
            XmlDoc.AppendChild(XmlDoc.CreateElement("configuration"))
        End If
    End Sub

    Public Sub Save(ByVal ConfigFile As String)
        ' Dim XmlDoc As New XmlDocument()
        'If System.IO.File.Exists(ConfigFile) Then Kill(ConfigFile)
        '(XmlDoc.AppendChild(XmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes"))
        XmlDoc.Save(ConfigFile)
    End Sub

    Public Function GetSetting(ByVal Section As String, ByVal Setting As String, ByVal DefaultValue As String) As String
        Dim Node As XmlNode = XmlDoc.DocumentElement.SelectSingleNode("/configuration/" & Section & "/" & Setting)
        If Node IsNot Nothing Then
            Return Node.InnerText
        End If
        Return DefaultValue
    End Function

    Public Sub SaveSetting(ByVal Section As String, ByVal SettingName As String, ByVal SettingValue As String)
        Dim Root As XmlElement = XmlDoc.SelectSingleNode("/configuration")
        If Root Is Nothing Then
            Root = XmlDoc.AppendChild(XmlDoc.CreateElement("configuration"))
        End If
        Dim SectionElement As XmlElement = XmlDoc.SelectSingleNode("/configuration/" & Section)
        If SectionElement Is Nothing Then
            SectionElement = Root.AppendChild(XmlDoc.CreateElement(Section))
        End If
        Dim SettingElement As XmlElement = SectionElement.SelectSingleNode("./" & SettingName)
        If SettingElement Is Nothing Then
            SettingElement = SectionElement.AppendChild(XmlDoc.CreateElement(SettingName))
        End If
        SettingElement.InnerText = SettingValue
    End Sub



End Class
