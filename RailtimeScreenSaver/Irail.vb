Imports System.IO
Imports System.Net
Imports System.Xml

Public Class Irail
    Inherits TrainDataGrabberAPI

    Protected m_Url As String
    Protected m_DepatureText As String
    Protected m_Language As String
    Protected m_ProxyServer As String
    Protected m_ProxyUser As String
    Protected m_ProxyPass As String

    ' Protected m_sDepArr As String


    Public Sub New(ByVal Settings As Settings)
        MyBase.New(Settings)
        m_Url = Settings.GetSetting("irail", "url", "http://api.irail.be/liveboard/?station={$station}&date={$date}&time={$time}&arrdep={$arr_dep}&lang={$lang}&format=xml&alerts=false")
        m_Language = UCase(Settings.GetSetting("irail", "lang", "NL"))
        m_ProxyServer = Settings.GetSetting("irail", "proxy", "")
        m_ProxyPass = Settings.GetSetting("irail", "proxy_pass", "")
        m_ProxyUser = Settings.GetSetting("irail", "proxy_user", "")
        ' m_sDepArr = Settings.GetSetting("irail", "arr_dep", "departure")
    End Sub

    Public Overrides Sub SaveSettings(ByVal Settings As Settings)
        MyBase.SaveSettings(Settings)
        Settings.SaveSetting("irail", "url", m_Url)
        Settings.SaveSetting("irail", "lang", m_Language)
        Settings.SaveSetting("irail", "proxy", m_ProxyServer)
        Settings.SaveSetting("irail", "proxy_pass", m_ProxyPass)
        Settings.SaveSetting("irail", "porxy_user", m_ProxyUser)
    End Sub

    Public Overrides ReadOnly Property Time As String
        Get
            Return Format(Now(), "HH:mm")
        End Get
    End Property

    Public Overrides ReadOnly Property DepartureArrivalText As String
        Get
            Return m_DepatureText
        End Get
    End Property

    Public Overrides Function RefreshTrainData(Time As Date) As Boolean
        Dim TheTime As String
        Dim TheDate As String
        Dim Url As String = m_Url
        Dim Lang As String = "nl"

        TheTime = Format(Time, "HHmm")
        TheDate = Format(Time, "ddMMyy")

        Select Case m_Language
            Case "NL"
                Lang = "nl"
            Case "FR"
                Lang = "fr"
            Case "DE"
                Lang = "de"
            Case "EN"
                Lang = "en"
            Case Else
                Debug.Print("Invalid language")
                Return False
        End Select

        Dim sDepArr As String = "depature"
        Select Case m_DepArr
            Case DepArrTypes.Departure
                sDepArr = "departure"
            Case DepArrTypes.Arrival
                sDepArr = "arrival"
        End Select

        Url = Replace(Url, "{$time}", TheTime)  'Tijd, 1, -1, vbTextCompare)
        Url = Replace(Url, "{$date}", TheDate, 1, -1, vbTextCompare)
        Url = Replace(Url, "{$lang}", Lang, 1, -1, vbTextCompare)
        Url = Replace(Url, "{$station}", m_Station, 1, -1, vbTextCompare)
        Url = Replace(Url, "{$arr_dep}", sDepArr, 1, -1, vbTextCompare)

        Dim wp As WebProxy
        Dim wc As New WebClient()

        If m_ProxyServer <> "" Then
            wp = New WebProxy(m_ProxyServer)

            If m_ProxyUser <> "" Then
                wp.Credentials = New NetworkCredential(m_ProxyUser, m_ProxyPass)
                wc.Proxy = wp
            End If
        End If


        Dim ms As New MemoryStream(wc.DownloadData(Url))

        Dim Doc As New XmlDocument
        Try
            Doc.Load(ms)
        Catch ex As Exception
            Debug.Print("Error loading xml: " & ex.Message & ", ")
        End Try

        Dim StationNode As XmlNode = Doc.SelectSingleNode("/liveboard/station")
        If StationNode IsNot Nothing Then m_StationName = StationNode.InnerText

        Dim TrainNodes As XmlNodeList
        If m_DepArr = DepArrTypes.Arrival Then
            TrainNodes = Doc.SelectNodes("/liveboard/arrivals/arrival")
        Else
            TrainNodes = Doc.SelectNodes("/liveboard/departures/departure")
        End If

        LockTrainData()
        m_Trains.Clear()

        For Each TrainNode As XmlNode In TrainNodes
            m_Trains.Add(ProcessTrainNode(TrainNode))
        Next
        UnlockTrainData()

    End Function

    Private Function ProcessTrainNode(ByVal Node As XmlNode) As TrainData
        Dim Time As String = Node.SelectSingleNode("time").Attributes.GetNamedItem("formatted").Value

        Dim Destination As String = Node.SelectSingleNode("station").InnerText
        Dim Vehicle As XmlNode = Node.SelectSingleNode("vehicle")

        Dim Type As String = Vehicle.Attributes.GetNamedItem("type").Value
        Dim Number As String = Vehicle.Attributes.GetNamedItem("number").Value

        Dim Delay As Integer = Node.Attributes.GetNamedItem("delay").InnerText
        Dim TrackNode As XmlNode = Node.SelectSingleNode("platform")
        Dim Track As String = TrackNode.InnerText
        Dim TrackChanged As Boolean = TrackNode.Attributes.GetNamedItem("normal").InnerText <> "1"

        Time = Time.Substring(Time.IndexOf("T") + 1, 5)

        Return New TrainData(Time, Number, Destination, Type, 0, Track, "", TrackChanged, TrainData.TrainTypeFlags.TrainTypeNormal)


    End Function


End Class
