Imports System.Net
Imports System.IO
Imports System.Xml

Public Class SNCB
    Inherits TrainDataGrabberAPI

    Protected m_Url As String
    Protected m_Stations As Dictionary(Of String, Long)
    Protected m_Language As String
    Protected m_sDepArr As String
    Protected m_Filter As Long
    Protected m_ShowSmallDelay As Boolean
    Protected m_DepatureText As String


    Public Const FILTER_INT As Long = 1024
    Public Const FILTER_IC As Long = 256
    Public Const FILTER_S As Long = 64
    Public Const FILTER_L As Long = 16
    Public Const FILTER_METRO As Long = 4
    Public Const FILTER_BUS As Long = 2
    Public Const FILTER_TRAM As Long = 1

    Public Const FILTER_UNKNOWN As Long = &HFFFF And Not (FILTER_TRAM Or FILTER_BUS Or FILTER_METRO Or FILTER_L Or FILTER_IC Or FILTER_INT Or FILTER_S)


    Public Sub New(ByVal Settings As Settings)
        MyBase.New(Settings)
        LoadStations()
        'm_Url = Settings.GetSetting("sncb", "url", "http://www.belgianrail.be/jpm/sncb-nmbs-routeplanner/stboard.exe/$langox?ld=std&input=$station_id&date=$date&time=$time&maxJourneys=$nr_lines&REQProduct_list=2:$filter&boardType=$type&start=Vertrek&getExternalStationBoard=1")
        m_Url = Settings.GetSetting("sncb", "url2", "http://www.belgianrail.be/jp/nmbs-realtime/stboard.exe/$langox?ld=std&input=$station_id&boardType=$type&time=$time&productsFilter=$filter&date=$date&maxJourneys=$nr_lines&start=yes&&getExternalStationBoard=1")
        m_Language = UCase(Settings.GetSetting("sncb", "lang", "NL"))
        m_Filter = Settings.GetSetting("sncb", "filter2", FILTER_L Or FILTER_IC Or FILTER_INT)
        m_ShowSmallDelay = Boolean.Parse(Settings.GetSetting("sncb", "show_small_delay", True))
    End Sub

    Public Overrides Sub SaveSettings(ByVal Settings As Settings)
        MyBase.SaveSettings(Settings)
        Settings.SaveSetting("sncb", "url2", m_Url)
        Settings.SaveSetting("sncb", "lang", m_Language)
        Settings.SaveSetting("sncb", "filter2", m_Filter)
        Settings.SaveSetting("sncb", "show_small_delay", m_ShowSmallDelay)
    End Sub

    ''' <summary>
    ''' Determines if delay smaller than 5 min is displayed on the screen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Show small delays")> _
    <System.ComponentModel.Description("Determines if delay smaller than 5 min is displayed on the screen.")> _
    Public Property ShowSmallDelay() As Boolean
        Get
            Return m_ShowSmallDelay
        End Get
        Set(ByVal value As Boolean)
            m_ShowSmallDelay = value
        End Set
    End Property

    ''' <summary>
    ''' Gets/sets the url to download data from
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Url to download data from. Variables: $station_id,$time,$date,$lang,$type,$station,$nr_lines,$filter.")> _
    Public Property Url() As String
        Get
            Return m_Url
        End Get
        Set(ByVal value As String)
            m_Url = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets the language to query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Language to use on the screen saver: NL,FR,DE,EN.")> _
    Public Property Language() As String
        Get
            Return m_Language
        End Get
        Set(ByVal value As String)
            m_Language = value
        End Set
    End Property

    ''' <summary>
    ''' Returns text departure or arrival
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Browsable(False)> _
    Public Overrides ReadOnly Property DepartureArrivalText() As String
        Get
            Return m_DepatureText
        End Get
    End Property

    ''' <summary>
    ''' Reloads the train data for a given time
    ''' </summary>
    ''' <param name="Time"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function RefreshTrainData(ByVal Time As DateTime) As Boolean
        Dim TheTime As String
        Dim TheDate As String
        Dim StationID As String
        Dim Lang As String
        Dim Request As HttpWebRequest
        Dim Url As String = m_Url
        Dim Trains As New List(Of TrainData)

        StationID = GetStationId(m_Station)

        If StationID <= 0 Then
            Debug.Print("Invalid station ID")
            StationID = 8814001 'brussel-midi
        End If

        TheTime = Format(Time, "HH:mm")
        TheDate = Format(Time, "dd/MM/yy")

        Select Case m_Language
            Case "NL"
                Lang = "n"
            Case "FR"
                Lang = "f"
            Case "DE"
                Lang = "d"
            Case "EN"
                Lang = "e"
            Case Else
                Debug.Print("Invalid language")
                Return False
        End Select

        Dim sDepArr As String = "dep"
        Select Case m_DepArr
            Case DepArrTypes.Departure
                sDepArr = "dep"
            Case DepArrTypes.Arrival
                sDepArr = "arr"
        End Select

        Url = Replace(Url, "$station_id", StationID, 1, -1, vbTextCompare)
        Url = Replace(Url, "$time", TheTime)  'Tijd, 1, -1, vbTextCompare)
        Url = Replace(Url, "$date", TheDate, 1, -1, vbTextCompare)
        Url = Replace(Url, "$lang", Lang, 1, -1, vbTextCompare)
        Url = Replace(Url, "$type", sDepArr, 1, -1, vbTextCompare)
        Url = Replace(Url, "$station", m_Station, 1, -1, vbTextCompare)
        Url = Replace(Url, "$nr_lines", m_MaxTrains, 1, -1, vbTextCompare)
        Url = Replace(Url, "$filter", Right(BinVal(m_Filter), 11), 1, -1, vbTextCompare)

        '                       01111110000
        '                       1|||||||||| -> internationale 1024
        '                        ?|||||||||    512
        '                         1|||||||| -> IC en P treinen 256
        '                          ?|||||||    128
        '                           ?|||||| -> S 64
        '                            ?|||||    32
        '                             1|||| -> L treinen 16
        '                              ?|||    8
        '                               1|| -> Metro 4
        '                                1| -> bus 2
        '                                 1 -> tram 1
        '
        '
        '


        Debug.Print(Url)
        Request = HttpWebRequest.Create(Url)

        Dim Response As HttpWebResponse = Request.GetResponse()
        If Response.StatusCode = HttpStatusCode.OK Then
            Dim ResponseStreamReader As StreamReader = New StreamReader(Response.GetResponseStream())
            Dim Xml As String = ResponseStreamReader.ReadToEnd()
            'Debug.Print(Xml)
            Xml = Replace(Xml, "&sqType", "&amp;sqType")
            Xml = Replace(Xml, "xmlns=""http://www.w3.org/1999/xhtml""", "")
            Dim ResponseXML As New XmlDocument

            ResponseXML.XmlResolver = Nothing
            ResponseXML.LoadXml(Xml)

            Dim TrainNodes As XmlNodeList = ResponseXML.SelectNodes("//p[@class=""journey""]")
            For Each trainnode As XmlNode In TrainNodes
                Trains.Add(ProcessTrainNode(trainnode))
            Next

            Dim TmpDepartureText As String = ""
            Dim StationNameNode As XmlNode = ResponseXML.SelectSingleNode("//p[@class=""qs""]/strong")
            If StationNameNode.NextSibling() IsNot Nothing Then
                Dim DepartureNode As XmlNode = StationNameNode.NextSibling().NextSibling()
                If DepartureNode IsNot Nothing Then
                    TmpDepartureText = Replace(Replace(Replace(DepartureNode.InnerText, vbCrLf, ""), vbLf, ""), vbCr, "")
                    Dim Pos As Integer = InStr(TmpDepartureText, " ")
                    If Pos > 0 Then
                        TmpDepartureText = Trim(Mid(TmpDepartureText, 1, Pos))
                    End If
                End If
            End If
            LockTrainData()
            m_Trains = Trains
            m_StationName = StationNameNode.InnerText
            m_DepatureText = TmpDepartureText
            UnlockTrainData()

        End If
    End Function

    ''' <summary>
    ''' Processes a train node in the returned xml train table
    ''' </summary>
    ''' <param name="Node"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function ProcessTrainNode(ByVal Node As XmlNode) As TrainData
        Dim TrainData As TrainData
        Dim TrainNumberAndType As String = Node.SelectSingleNode("./a/strong").InnerText
        Dim TrainDestination As String = Replace(Replace(Node.ChildNodes(1).InnerText, "<", ""), ">", "")
        Dim TrainTime As String = Node.SelectSingleNode("./strong").InnerText
        Dim TrainPlatform As String = ""
        Dim TrainDelay As String = ""
        Dim TrainPlatformChanged As Boolean = False
        Dim PlatFormChangeNode As XmlNode = Node.SelectSingleNode("./span[@class=""platformChange""]")
        Dim DelayNode As XmlNode = Node.SelectSingleNode("./span[@class=""delay""]")
        Dim TrainType As String = ""
        Dim TrainNumber As String = ""

        If DelayNode IsNot Nothing Then
            TrainDelay = DelayNode.InnerText

            If PlatFormChangeNode IsNot Nothing Then
                TrainPlatformChanged = True
                TrainPlatform = PlatFormChangeNode.InnerText
            Else
                If Node.ChildNodes.Count >= 7 Then
                    TrainPlatform = Node.ChildNodes(5).InnerText
                End If
            End If
        Else
            If PlatFormChangeNode IsNot Nothing Then
                TrainPlatformChanged = True
                TrainPlatform = PlatFormChangeNode.InnerText
            Else
                If Node.ChildNodes.Count >= 6 Then
                    TrainPlatform = Node.ChildNodes(4).InnerText
                End If
            End If
        End If

        TrainPlatform = Trim(TrainPlatform)
        TrainPlatform = Replace(TrainPlatform, "today", "")
        TrainPlatform = Replace(TrainPlatform, "aujourd'hui", "")
        TrainPlatform = Replace(TrainPlatform, "heute", "")
        Dim Pos As Integer = InStrRev(TrainPlatform, " ")
        If Pos > 0 Then
            TrainPlatform = Mid(TrainPlatform, Pos + 1)
        End If

        TrainNumberAndType = RemoveNewLines(TrainNumberAndType)
        TrainNumberAndType = Replace(TrainNumberAndType, "EXT", "EXT ")
        TrainNumberAndType = Replace(TrainNumberAndType, "TRN", "TRN ")
        Pos = InStr(TrainNumberAndType, " ")
        If Pos > 0 Then
            TrainType = Trim(Mid(TrainNumberAndType, 1, Pos - 1))
            TrainNumber = Trim(Mid(TrainNumberAndType, Pos + 1))
            If LCase(TrainType) = "mét" Then TrainType = "MET"
        End If

        TrainDestination = RemoveNewLines(TrainDestination)

        'remove operator
        TrainDestination = Trim(Replace(TrainDestination, "[De Lijn]", "", 1, -1, vbTextCompare))
        TrainDestination = Trim(Replace(TrainDestination, "[MIVB]", "", 1, -1, vbTextCompare))
        TrainDestination = Trim(Replace(TrainDestination, "[TEC]", "", 1, -1, vbTextCompare))

        'remove country
        TrainDestination = Trim(Replace(TrainDestination, "(gb)", "", 1, -1, vbTextCompare))
        TrainDestination = Trim(Replace(TrainDestination, "(f)", "", 1, -1, vbTextCompare))
        TrainDestination = Trim(Replace(TrainDestination, "(nl)", "", 1, -1, vbTextCompare))
        TrainDestination = Trim(Replace(TrainDestination, "(d)", "", 1, -1, vbTextCompare))
        TrainDestination = Trim(Replace(TrainDestination, "(l)", "", 1, -1, vbTextCompare))
        TrainDestination = Trim(Replace(TrainDestination, "(ch)", "", 1, -1, vbTextCompare))
        TrainDestination = Capitalize(TrainDestination)

        Dim iDelay As Integer
        If TrainDelay <> "" Then
            TrainDelay = Trim(Replace(TrainDelay, "+", ""))
            If IsNumeric(TrainDelay) Then 'check if is numeric
                iDelay = CInt(Val(TrainDelay))
                If iDelay >= 5 Or m_ShowSmallDelay Then
                    TrainDelay = "+" & Math.Round(iDelay / 60, 0) & ":"
                    If (iDelay Mod 60) < 10 Then TrainDelay = TrainDelay & "0"
                    TrainDelay = TrainDelay & (iDelay Mod 60)
                Else
                    TrainDelay = ""
                End If
            End If
        End If

        Dim TrainTypeFlags As TrainData.TrainTypeFlags = RailtimeScreenSaver.TrainData.TrainTypeFlags.TrainTypeNormal

        If TrainType Like "S#" OrElse TrainType Like "S##" Then
            TrainTypeFlags = TrainTypeFlags Or RailtimeScreenSaver.TrainData.TrainTypeFlags.TrainTypeSBahn
        Else
            If InStr(TrainDestination, "airport", CompareMethod.Text) > 0 OrElse _
               InStr(TrainDestination, "luchthaven", CompareMethod.Text) > 0 Then
                TrainTypeFlags = TrainTypeFlags Or RailtimeScreenSaver.TrainData.TrainTypeFlags.TrainTypeAirport
            End If
        End If

        TrainData = New TrainData(TrainTime, TrainNumber, TrainDestination, TrainType, iDelay, TrainPlatform, TrainDelay, TrainPlatformChanged, TrainTypeFlags)

        Return TrainData
    End Function

    Protected Sub AddStation(ByVal Id As Long, ByVal Name As String)
        If Not m_Stations.ContainsKey(Name) Then
            m_Stations.Add(Name, Id)
        End If
    End Sub

    ''' <summary>
    ''' Loads all station names and ID's
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub LoadStations()
        m_Stations = New Dictionary(Of String, Long)

        'thanks to irail team for this list
        'https://github.com/iRail/iRail/blob/master/stationlists/stations.sql
        'https://raw.githubusercontent.com/iRail/stations/master/stations.csv

        Dim StationsList As String = My.Resources.stations

        Dim Lines() As String = Split(StationsList, vbLf)

        For Each Line As String In Lines
            If Line <> "" Then
                Dim Values() As String = Split(Line, ",")
                If Mid(Values(0), 1, 7) = "http://" Then
                    Dim Pos As Integer = InStrRev(Values(0), "/")
                    Dim Id As Long = Val(Mid(Values(0), Pos + 1))
                    AddStation(Id, Replace(Values(1).ToUpper, "-", " "))
                    If Values(2) <> "" Then AddStation(Id, Replace(Values(2).ToUpper, "-", " "))
                    If Values(3) <> "" Then AddStation(Id, Replace(Values(3).ToUpper, "-", " "))
                    If Values(4) <> "" Then AddStation(Id, Replace(Values(4).ToUpper, "-", " "))
                    If Values(5) <> "" Then AddStation(Id, Replace(Values(5).ToUpper, "-", " "))
                End If
            End If
        Next



        'AddStation(8819406, "BRUSSELS AIRPORT")
        'AddStation(8819406, "BRUXELLES NATIONAL AEROPORT")
        'AddStation(8819406, "BRUSSEL NATIONAAL LUCHTHAVEN")
        'AddStation(8885522, "ANTOING")
        'AddStation(8884566, "JEMAPPES")
        'AddStation(8841459, "MOMALLE")
        'AddStation(8864451, "BARVAUX")
        'AddStation(8833274, "ZICHEM")
        'AddStation(8892106, "DEINZE")
        'AddStation(8814266, "WATERLOO")
        'AddStation(8871829, "HOURPES")
        'AddStation(8885753, "HERSEAUX")
        'AddStation(8814175, "DE HOEK")
        'AddStation(8863156, "LUSTIN")
        'AddStation(8884335, "QUIEVRAIN")
        'AddStation(8864931, "ASSESSE")
        'AddStation(8861432, "SAINT DENIS BOVESSE")
        'AddStation(8892908, "RONSE")
        'AddStation(8892908, "RENAIX")
        'AddStation(8891009, "BRUGES")
        'AddStation(8891009, "BRUGGE")
        'AddStation(8811635, "LIMAL")
        'AddStation(8844404, "SPA")
        'AddStation(8831781, "BOKRIJK")
        'AddStation(8831088, "SCHULEN")
        'AddStation(8863438, "CHATEAU DE SEILLES")
        'AddStation(8864824, "CHAPOIS")
        'AddStation(8811254, "KORTENBERG")
        'AddStation(8883220, "MARCHE LEZ ECAUSSINNES")
        'AddStation(8863404, "ANDENNE")
        'AddStation(8833050, "VERTRIJK")
        'AddStation(8882230, "MORLANWELZ")
        'AddStation(8881562, "GENLY")
        'AddStation(8811221, "ZAVENTEM")
        'AddStation(8895125, "AALST KERREBROEK")
        'AddStation(8896925, "INGELMUNSTER")
        'AddStation(8886561, "PAPEGEM")
        'AddStation(8886561, "PAPIGNIES")
        'AddStation(8863461, "MARCHE LES DAMES")
        'AddStation(8822772, "BORNEM")
        'AddStation(8894821, "ZWIJNDRECHT")
        'AddStation(8832565, "LOMMEL")
        'AddStation(8871175, "JAMIOULX")
        'AddStation(8822277, "HOFSTADE")
        'AddStation(8824158, "HOBOKEN POLDER")
        'AddStation(8843208, "FLEMALLE HAUTE")
        'AddStation(8842002, "ANGLEUR")
        'AddStation(8843349, "HAUTE FLONE")
        'AddStation(8871183, "BEIGNEE")
        'AddStation(8814423, "BEERSEL")
        'AddStation(8892205, "LICHTERVELDE")
        'AddStation(8872066, "CHARLEROI WEST")
        'AddStation(8872066, "CHARLEROI OUEST")
        'AddStation(8893062, "MOORTSELE")
        'AddStation(8895729, "IDEGEM")
        'AddStation(8814209, "NIJVEL")
        'AddStation(8814209, "NIVELLES")
        'AddStation(8895455, "BURST")
        'AddStation(8822228, "SINT KATELIJNE WAVER")
        'AddStation(8861531, "BLANMONT")
        'AddStation(8864832, "HAVERSIN")
        'AddStation(8814241, "LILLOIS")
        'AddStation(8862018, "STOCKEM")
        'AddStation(8813003, "BRUSSELS CENTRAL")
        'AddStation(8813003, "BRUXELLES CENTRAL")
        'AddStation(8813003, "BRUSSEL CENTRAAL")
        'AddStation(8892692, "SINT DENIJS BOEKEL")
        'AddStation(8843323, "AMPSIN")
        'AddStation(8811718, "BIERGES WALIBI")
        'AddStation(8873387, "HAM SUR HEURE")
        'AddStation(8884350, "THULIN")
        'AddStation(8895760, "NINOVE")
        'AddStation(8821725, "BOUWEL")
        'AddStation(8821824, "MELKOUWEN")
        'AddStation(8864956, "SART BERNARD")
        'AddStation(8811916, "BRUSSELS SCHUMAN")
        'AddStation(8811916, "BRUXELLES SCHUMAN")
        'AddStation(8811916, "BRUSSEL SCHUMAN")
        'AddStation(8886348, "LEUZE")
        'AddStation(8811437, "BOSVOORDE")
        'AddStation(8811437, "BOITSFORT")
        'AddStation(8891173, "ZEEBRUGGE STRAND")
        'AddStation(8811510, "TERHULPEN")
        'AddStation(8811510, "LA HULPE")
        'AddStation(8863115, "JAMBES EAST")
        'AddStation(8863115, "JAMBES EST")
        'AddStation(8863115, "JAMBES")
        'AddStation(8863115, "JAMBES OOST")
        'AddStation(8821121, "ANTWERP BERCHEM")
        'AddStation(8821121, "ANVERS BERCHEM")
        'AddStation(8821121, "ANTWERPEN BERCHEM")
        'AddStation(8864949, "COURRIERE")
        'AddStation(8814332, "LEMBEEK")
        'AddStation(8864923, "FLOREE")
        'AddStation(8813037, "BRUSSELS CHAPELLE")
        'AddStation(8813037, "BRUXELLES CHAPELLE")
        'AddStation(8813037, "BRUSSEL KAPELLEKERK")
        'AddStation(8893401, "DENDERMONDE")
        'AddStation(8893401, "TERMONDE")
        'AddStation(8811411, "ETTERBEEK")
        'AddStation(8893542, "SCHELLEBELLE")
        'AddStation(8884640, "HARCHIES")
        'AddStation(8811445, "GROENENDAAL")
        'AddStation(8896115, "HARELBEKE")
        'AddStation(8842846, "HAMOIR")
        'AddStation(8864352, "AYE")
        'AddStation(8886074, "CAMBRON CASTEAU")
        'AddStation(8843307, "HOEI")
        'AddStation(8843307, "HUY")
        'AddStation(8832375, "KIEWIT")
        'AddStation(8822525, "WESPELAAR TILDONK")
        'AddStation(8891132, "MARIA AALTER")
        'AddStation(8821238, "MORTSEL OUDE GOD")
        'AddStation(8896909, "IZEGEM")
        'AddStation(8864469, "BOMAL")
        'AddStation(8871308, "LUTTRE")
        'AddStation(8871811, "THUIN")
        'AddStation(8881190, "LENS")
        'AddStation(8841202, "ANS")
        'AddStation(8884319, "BOUSSU")
        'AddStation(8811106, "EVERE")
        'AddStation(8812260, "ESSENE LOMBEEK")
        'AddStation(8883808, "TUBEKE")
        'AddStation(8883808, "TUBIZE")
        'AddStation(8882206, "LA LOUVIERE SOUTH")
        'AddStation(8882206, "LA LOUVIERE SUD")
        'AddStation(8882206, "LA LOUVIERE ZUID")
        'AddStation(8895737, "ZANDBERGEN")
        'AddStation(8812062, "ZELLIK")
        'AddStation(8893260, "SLEIDINGE")
        'AddStation(8881000, "MONS")
        'AddStation(8881000, "BERGEN")
        'AddStation(8821600, "LIER")
        'AddStation(8821600, "LIERRE")
        'AddStation(8882339, "LEVAL")
        'AddStation(8821816, "BERLAAR")
        'AddStation(8831138, "BILZEN")
        'AddStation(8892601, "OUDENAARDE")
        'AddStation(8892601, "AUDENARDE")
        'AddStation(8841731, "GLONS")
        'AddStation(8841731, "GLAAIEN")
        'AddStation(8895869, "HAALTERT")
        'AddStation(8874054, "COUILLET")
        'AddStation(8893526, "SCHOONAARDE")
        'AddStation(8891165, "HANSBEKE")
        'AddStation(8861168, "HAM SUR SAMBRE")
        'AddStation(8821444, "KALMTHOUT")
        'AddStation(8892031, "DRONGEN")
        'AddStation(8892031, "TRONCHIENNES")
        'AddStation(8844008, "VERVIERS CENTRAL")
        'AddStation(8844008, "VERVIERS CENTRAAL")
        'AddStation(8884004, "SAINT GHISLAIN")
        'AddStation(8863842, "HOUYET")
        'AddStation(8861143, "FRANIERE")
        'AddStation(8895422, "HILLEGEM")
        'AddStation(8884715, "BLATON")
        'AddStation(8821717, "HERENTALS")
        'AddStation(8811155, "HAREN")
        'AddStation(8813045, "BRUSSELS CONGRES")
        'AddStation(8813045, "BRUXELLES CONGRES")
        'AddStation(8813045, "BRUSSEL CONGRES")
        'AddStation(8893518, "OUDEGEM")
        'AddStation(8844255, "FRAIPONT")
        'AddStation(8861523, "CHASTRE")
        'AddStation(8814431, "MOENSBERG")
        'AddStation(8891124, "BEERNEM")
        'AddStation(8884541, "QUAREGNON")
        'AddStation(8891116, "OOSTKAMP")
        'AddStation(8891314, "TORHOUT")
        'AddStation(8892650, "EKE NAZARETH")
        'AddStation(8874716, "AUVELAIS")
        'AddStation(8884632, "VILLE POMMEROEUL")
        'AddStation(8892403, "KORTEMARK")
        'AddStation(8863818, "ANSEREMME")
        'AddStation(8892452, "DIKSMUIDE")
        'AddStation(8892452, "DIXMUDE")
        'AddStation(8896396, "WERVIK")
        'AddStation(8821006, "ANTWERP CENTRAL")
        'AddStation(8821006, "ANVERS CENTRAL")
        'AddStation(8821006, "ANTWERPEN CENTRAAL")
        'AddStation(8896370, "WEVELGEM")
        'AddStation(8864337, "GRUPONT")
        'AddStation(8814001, "BRUSSELS SOUTH")
        'AddStation(8814001, "BRUXELLES MIDI")
        'AddStation(8814001, "BRUSSEL ZUID")
        'AddStation(8814001, "BRUSSELS MIDI")
        'AddStation(8821196, "ANTWERP SOUTH")
        'AddStation(8821196, "ANVERS SUD")
        'AddStation(8821196, "ANTWERPEN ZUID")
        'AddStation(8822715, "PUURS")
        'AddStation(8824224, "HEMIKSEM")
        'AddStation(8812021, "BOCKSTAEL")
        'AddStation(8872413, "FLEURUS")
        'AddStation(8895257, "BALEGEM VILLAGE")
        'AddStation(8895257, "BALEGEM DORP")
        'AddStation(8891645, "HEIST")
        'AddStation(8822533, "HAMBOS")
        'AddStation(8811767, "FLORIVAL")
        'AddStation(8841004, "LIEGE GUILLEMINS")
        'AddStation(8841004, "LUTTICH GUILLEMINS")
        'AddStation(8841004, "LUIK GUILLEMINS")
        'AddStation(8866407, "VIRTON")
        'AddStation(8893708, "EEKLO")
        'AddStation(8812146, "OPWIJK")
        'AddStation(8842853, "SY")
        'AddStation(8886546, "ACREN")
        'AddStation(8811742, "GASTUCHE")
        'AddStation(8864410, "MARCHE EN FAMENNE")
        'AddStation(8871837, "LANDELIES")
        'AddStation(8863834, "GENDRON CELLES")
        'AddStation(8811775, "PECROT")
        'AddStation(8833266, "TESTELT")
        'AddStation(8842838, "COMBLAIN LA TOUR")
        'AddStation(8831807, "SAINT TROND")
        'AddStation(8831807, "SINT TRUIDEN")
        'AddStation(8831310, "TONGEREN")
        'AddStation(8831310, "TONGRES")
        'AddStation(8841434, "BLERET")
        'AddStation(8891629, "LISSEWEGE")
        'AddStation(8893559, "WETTEREN")
        'AddStation(8843141, "PONT DE SERAING")
        'AddStation(8863362, "DAVE SAINT MARTIN")
        'AddStation(8861200, "GEMBLOUX")
        'AddStation(8865300, "BERTRIX")
        'AddStation(8814118, "VORST EAST")
        'AddStation(8814118, "FOREST EST")
        'AddStation(8814118, "VORST OOST")
        'AddStation(8883006, "BRAINE LE COMTE")
        'AddStation(8883006, "'S GRAVENBRAKEL")
        'AddStation(8831039, "ALKEN")
        'AddStation(8843158, "JEMEPPE SUR MEUSE")
        'AddStation(8821147, "MORTSEL")
        'AddStation(8861549, "MONT SAINT GUIBERT")
        'AddStation(8822269, "EPPEGEM")
        'AddStation(8822459, "HEVER")
        'AddStation(8812153, "HEIZIJDE")
        'AddStation(8814258, "EIGENBRAKEL")
        'AddStation(8814258, "BRAINE L'ALLEUD")
        'AddStation(8814126, "UCCLE STALLE")
        'AddStation(8814126, "UKKEL STALLE")
        'AddStation(8811544, "PROFONDSART")
        'AddStation(8841665, "MILMORT")
        'AddStation(8811734, "BASSE WAVRE")
        'AddStation(8861440, "RHISNES")
        'AddStation(8844321, "JUSLENVILLE")
        'AddStation(8841525, "LIEGE PALAIS")
        'AddStation(8841525, "LUTTICH PALAIS")
        'AddStation(8841525, "LUIK PALEIS")
        'AddStation(8821907, "TURNHOUT")
        'AddStation(8843430, "BAS OHA")
        'AddStation(8833308, "TIENEN")
        'AddStation(8833308, "TIRLEMONT")
        'AddStation(8895620, "GALMAARDEN")
        'AddStation(8895620, "GAMMERAGES")
        'AddStation(8893013, "MERELBEKE")
        'AddStation(8861333, "MAZY")
        'AddStation(8892627, "EINE")
        'AddStation(8896388, "BISSEGEM")
        'AddStation(8886587, "REBAIX")
        'AddStation(8844339, "THEUX")
        'AddStation(8871647, "ERQUELINNES VILLAGE")
        'AddStation(8871647, "ERQUELINNES DORP")
        'AddStation(8865649, "GEDINNE")
        'AddStation(8861416, "LONZEE")
        'AddStation(8871365, "PONT A CELLES")
        'AddStation(8894755, "MELSELE")
        'AddStation(8895232, "MUNKZWALM")
        'AddStation(8863560, "GODINNE")
        'AddStation(8811726, "WAVER")
        'AddStation(8811726, "WAVRE")
        'AddStation(8846201, "WEZET")
        'AddStation(8846201, "VISE")
        'AddStation(8873320, "BERZEE")
        'AddStation(8833233, "WEZEMAAL")
        'AddStation(8892643, "GAVERE ASPER")
        'AddStation(8893047, "GONTRODE")
        'AddStation(8872553, "TILLY")
        'AddStation(8892320, "KOKSIJDE")
        'AddStation(8895844, "IDDERGEM")
        'AddStation(8822111, "LONDERZEEL")
        'AddStation(8833670, "NEERWINDEN")
        'AddStation(8875002, "MARIEMBOURG")
        'AddStation(8882107, "LA LOUVIERE CENTER")
        'AddStation(8882107, "LA LOUVIERE CENTRE")
        'AddStation(8882107, "LA LOUVIERE CENTRUM")
        'AddStation(8874005, "CHATELET")
        'AddStation(8871688, "FONTAINE VALMONT")
        'AddStation(8864006, "JEMELLE")
        'AddStation(8833209, "AARSCHOT")
        'AddStation(8844057, "VERVIERS PALAIS")
        'AddStation(8844057, "VERVIERS PALEIS")
        'AddStation(8895471, "ERPE MERE")
        'AddStation(8871332, "OBAIX BUZET")
        'AddStation(8895240, "BALEGEM SOUTH")
        'AddStation(8895240, "BALEGEM SUD")
        'AddStation(8895240, "BALEGEM ZUID")
        'AddStation(8871514, "FORCHIES")
        'AddStation(8871373, "GOUY LEZ PIETON")
        'AddStation(8811304, "BRUSSELS LUXEMBOURG")
        'AddStation(8811304, "BRUXELLES LUXEMBOURG")
        'AddStation(8811304, "BRUSSEL LUXEMBURG")
        'AddStation(8894433, "BELSELE")
        'AddStation(8842689, "POULSEUR")
        'AddStation(8811262, "ERPS KWERPS")
        'AddStation(8895711, "SCHENDELBEKE")
        'AddStation(8832458, "OLEN")
        'AddStation(8822475, "BOORTMEERBEEK")
        'AddStation(8822210, "DUFFEL")
        'AddStation(8883311, "EDINGEN")
        'AddStation(8883311, "ENGHIEN")
        'AddStation(8814134, "UCCLE CALEVOET")
        'AddStation(8814134, "UKKEL KALEVOET")
        'AddStation(8821402, "ESSEN")
        'AddStation(8821519, "HEIDE")
        'AddStation(8843166, "FLEMALLE GRANDE")
        'AddStation(8811460, "HOEILAART")
        'AddStation(8845146, "VIELSALM")
        'AddStation(8866142, "HABAY")
        'AddStation(8811171, "MEISER")
        'AddStation(8895091, "EREMBODEGEM")
        'AddStation(8893120, "GHENT DAMPOORT")
        'AddStation(8893120, "GAND DAMPOORT")
        'AddStation(8893120, "GENT DAMPOORT")
        'AddStation(8884327, "HAININ")
        'AddStation(8885704, "MOUSCRON")
        'AddStation(8885704, "MOESKROEN")
        'AddStation(8865110, "BASTOGNE SOUTH")
        'AddStation(8865110, "BASTOGNE SUD")
        'AddStation(8865110, "BASTENAKEN ZUID")
        'AddStation(8896230, "VICHTE")
        'AddStation(8812237, "DILBEEK")
        'AddStation(8895851, "WELLE")
        'AddStation(8833001, "LEUVEN")
        'AddStation(8833001, "LOUVAIN")
        'AddStation(8821964, "TIELEN")
        'AddStation(8821832, "HEIST OP DEN BERG")
        'AddStation(8871217, "ROUX")
        'AddStation(8873122, "PHILIPPEVILLE")
        'AddStation(8881455, "THIEU")
        'AddStation(8814159, "HOLLEKEN")
        'AddStation(8822053, "KAPELLE OP DEN BOS")
        'AddStation(8885530, "MAUBRAY")
        'AddStation(8865128, "BASTOGNE NORTH")
        'AddStation(8865128, "BASTOGNE NORD")
        'AddStation(8865128, "BASTENAKEN NOORD")
        'AddStation(8811205, "DELTA")
        'AddStation(8896305, "MENEN")
        'AddStation(8896305, "MENIN")
        'AddStation(8824240, "NIEL")
        'AddStation(8812252, "TERNAT")
        'AddStation(8864311, "FORRIERES")
        'AddStation(8864816, "LEIGNON")
        'AddStation(8894714, "NIEUWKERKEN WAAS")
        'AddStation(8845229, "COO")
        'AddStation(8812005, "BRUSSELS NORTH")
        'AddStation(8812005, "BRUXELLES NORD")
        'AddStation(8812005, "BRUSSEL NOORD")
        'AddStation(8821436, "WILDERT")
        'AddStation(8893054, "LANDSKOUTER")
        'AddStation(8882701, "MANAGE")
        'AddStation(8892635, "ZINGEM")
        'AddStation(8861515, "ERNAGE")
        'AddStation(8814365, "RUISBROEK")
        'AddStation(8886009, "ATH")
        'AddStation(8886009, "AAT")
        'AddStation(8831112, "DIEPENBEEK")
        'AddStation(8832227, "BERINGEN")
        'AddStation(8832409, "MOL")
        'AddStation(8812120, "MERCHTEM")
        'AddStation(8872306, "LODELINSART")
        'AddStation(8821634, "BOECHOUT")
        'AddStation(8821535, "KAPELLEN")
        'AddStation(8886066, "BRUGELETTE")
        'AddStation(8842036, "CHENEE")
        'AddStation(8821154, "MORTSEL DEURNESTEENWEG")
        'AddStation(8886041, "MAFFLE")
        'AddStation(8882362, "BINCHE")
        'AddStation(8881166, "JURBISE")
        'AddStation(8881166, "JURBEKE")
        'AddStation(8845203, "TROIS PONTS")
        'AddStation(8832433, "GEEL")
        'AddStation(8896412, "COMINES")
        'AddStation(8896412, "KOMEN")
        'AddStation(8822343, "MALINES NEKKERSPOEL")
        'AddStation(8822343, "MECHELEN NEKKERSPOEL")
        'AddStation(8881315, "NIMY")
        'AddStation(8861127, "FLAWINNE")
        'AddStation(8814308, "HALLE")
        'AddStation(8814308, "HAL")
        'AddStation(8842705, "RIVAGE")
        'AddStation(8882248, "CARNIERES")
        'AddStation(8866845, "FLORENVILLE")
        'AddStation(8891702, "OSTEND")
        'AddStation(8891702, "OSTENDE")
        'AddStation(8891702, "OOSTENDE")
        'AddStation(8875127, "COUVIN")
        'AddStation(8832045, "BALEN")
        'AddStation(8811189, "VILVOORDE")
        'AddStation(8811189, "VILVORDE")
        'AddStation(8886058, "MEVERGNIES ATTRE")
        'AddStation(8895448, "TERHAGEN")
        'AddStation(8811536, "RIXENSART")
        'AddStation(8895208, "ZOTTEGEM")
        'AddStation(8831765, "GENK")
        'AddStation(8811247, "NOSSEGEM")
        'AddStation(8895802, "DENDERLEEUW")
        'AddStation(8844271, "TROOZ")
        'AddStation(8892338, "DE PANNE")
        'AddStation(8892338, "LA PANNE")
        'AddStation(8842754, "AYWAILLE")
        'AddStation(8861135, "FLOREFFE")
        'AddStation(8884889, "CALLENELLE")
        'AddStation(8865003, "LIBRAMONT")
        'AddStation(8873007, "WALCOURT")
        'AddStation(8814449, "SAINT JOB")
        'AddStation(8814449, "SINT JOB")
        'AddStation(8866258, "NEUFCHATEAU")
        'AddStation(8833175, "WIJGMAAL")
        'AddStation(8811528, "GENVAL")
        'AddStation(8891553, "ZEEBRUGGE DORP")
        'AddStation(8841400, "WAREMME")
        'AddStation(8841400, "BORGWORM")
        'AddStation(8861424, "BEUZET")
        'AddStation(8821667, "NIJLEN")
        'AddStation(8865540, "PALISEUL")
        'AddStation(8892734, "ANZEGEM")
        'AddStation(8863867, "BEAURAING")
        'AddStation(8833605, "LANDEN")
        'AddStation(8893534, "WICHELEN")
        'AddStation(8893815, "WAARSCHOOT")
        'AddStation(8821865, "BEGIJNENDIJK")
        'AddStation(8883022, "HENNUYERES")
        'AddStation(8873312, "PRY")
        'AddStation(8811429, "WATERMAAL")
        'AddStation(8811429, "WATERMAEL")
        'AddStation(8831401, "DIEST")
        'AddStation(8833258, "LANGDORP")
        'AddStation(8811759, "EERKEN")
        'AddStation(8811759, "ARCHENNES")
        'AddStation(8874567, "FARCIENNES")
        'AddStation(8893567, "KWATRECHT")
        'AddStation(8891264, "ZEDELGEM")
        'AddStation(8891652, "DUINBERGEN")
        'AddStation(8812211, "SINT AGATHA BERCHEM")
        'AddStation(8812211, "BERCHEM SAINTE AGATHE")
        'AddStation(8895612, "VIANE MOERBEKE")
        'AddStation(8895745, "APPELTERRE")
        'AddStation(8893179, "GENTBRUGGE")
        'AddStation(8822004, "MALINES")
        'AddStation(8822004, "MECHELEN")
        'AddStation(8814142, "LINKEBEEK")
        'AddStation(8885068, "FROYENNES")
        'AddStation(8895646, "HERNE")
        'AddStation(8895877, "EDE")
        'AddStation(8844347, "FRANCHIMONT")
        'AddStation(8844313, "PEPINSTER CITE")
        'AddStation(8811825, "COURT SAINT ETIENNE")
        'AddStation(8844503, "WELKENRAEDT")
        'AddStation(8821311, "KONTICH")
        'AddStation(8872009, "CHARLEROI SOUTH")
        'AddStation(8872009, "CHARLEROI SUD")
        'AddStation(8872009, "CHARLEROI ZUID")
        'AddStation(8895778, "OKEGEM")
        'AddStation(8843406, "STATTE")
        'AddStation(8892080, "DE PINTE")
        'AddStation(8822145, "BUGGENHOUT")
        'AddStation(8864501, "CINEY")
        'AddStation(8842663, "ESNEUX")
        'AddStation(8861317, "CHAPELLE DIEU")
        'AddStation(8891611, "ZWANKENDAMME")
        'AddStation(8833449, "EZEMAAL")
        'AddStation(8812161, "LEBBEKE")
        'AddStation(8844230, "NESSONVAUX")
        'AddStation(8832243, "HEUSDEN")
        'AddStation(8845005, "GOUVY")
        'AddStation(8812047, "JETTE")
        'AddStation(8861119, "RONET")
        'AddStation(8811148, "BUDA")
        'AddStation(8891405, "BLANKENBERGE")
        'AddStation(8895570, "LIERDE")
        'AddStation(8843133, "SCLESSIN")
        'AddStation(8843901, "BRESSOUX")
        'AddStation(8896008, "COURTRAI")
        'AddStation(8896008, "KORTRIJK")
        'AddStation(8832615, "NEERPELT")
        'AddStation(8883212, "ECAUSSINNES")
        'AddStation(8841608, "HERSTAL")
        'AddStation(8811007, "SCHAARBEEK")
        'AddStation(8811007, "SCHAERBEEK")
        'AddStation(8863545, "YVOIR")
        'AddStation(8894508, "SAINT NICOLAS")
        'AddStation(8894508, "SINT NIKLAAS")
        'AddStation(8895463, "BAMBRUGGE")
        'AddStation(8892304, "FURNES")
        'AddStation(8892304, "VEURNE")
        'AddStation(8885001, "TOURNAI")
        'AddStation(8885001, "DOORNIK")
        'AddStation(8841319, "BIERSET AWANS")
        'AddStation(8822426, "MUIZEN")
        'AddStation(8864345, "MARLOIE")
        'AddStation(8871225, "COURCELLES MOTTE")
        'AddStation(8881505, "QUEVY")
        'AddStation(8881158, "ERBISOEUL")
        'AddStation(8871852, "MARCHIENNE ZONE")
        'AddStation(8881430, "HAVRE")
        'AddStation(8841467, "FEXHE LE HAUT CLOCHER")
        'AddStation(8811270, "VELTEM")
        'AddStation(8842655, "HONY")
        'AddStation(8894672, "TEMSE")
        'AddStation(8894672, "TAMISE")
        'AddStation(8821659, "KESSEL")
        'AddStation(8833134, "OUD HEVERLEE")
        'AddStation(8873239, "YVES GOMEZEE")
        'AddStation(8843224, "LEMAN")
        'AddStation(8893443, "SINT GILLIS")
        'AddStation(8895489, "VIJFHUIZEN")
        'AddStation(8841673, "LIERS")
        'AddStation(8811213, "DIEGEM")
        'AddStation(8811130, "HAREN SOUTH")
        'AddStation(8811130, "HAREN SUD")
        'AddStation(8811130, "HAREN ZUID")
        'AddStation(8822251, "WEERDE")
        'AddStation(8821451, "KIJKUIT")
        'AddStation(8893583, "SERSKAMP")
        'AddStation(8872611, "FAUX")
        'AddStation(8892056, "LANDEGEM")
        'AddStation(8891660, "KNOKKE")
        'AddStation(8822160, "BAASRODE SOUTH")
        'AddStation(8822160, "BAASRODE SUD")
        'AddStation(8822160, "BAASRODE ZUID")
        'AddStation(8814373, "VORST SOUTH")
        'AddStation(8814373, "FOREST MIDI")
        'AddStation(8814373, "VORST ZUID")
        'AddStation(8866001, "ARLON")
        'AddStation(8866001, "AARLEN")
        'AddStation(8874724, "JEMEPPE SUR SAMBRE")
        'AddStation(8814357, "LOT")
        'AddStation(8892007, "GHENT SINT PIETERS")
        'AddStation(8892007, "GAND SAINT PIERRE")
        'AddStation(8892007, "GENT SINT PIETERS")
        'AddStation(8841558, "LIEGE JONFOSSE")
        'AddStation(8841558, "LUTTICH JONFOSSE")
        'AddStation(8841558, "LUIK JONFOSSE")
        'AddStation(8895836, "LIEDEKERKE")
        'AddStation(8883238, "FAMILLEUREUX")
        'AddStation(8814167, "RHODE SAINT GENESE")
        'AddStation(8814167, "SINT GENESIUS RODE")
        'AddStation(8894748, "BEVEREN")
        'AddStation(8892288, "AARSELE")
        'AddStation(8881463, "BRACQUEGNIES")
        'AddStation(8895752, "EICHEM")
        'AddStation(8833126, "HEVERLEE")
        'AddStation(8812112, "MOLLEM")
        'AddStation(8821337, "HOVE")
        'AddStation(8871605, "ERQUELINNES")
        'AddStation(8863446, "SCLAIGNEAUX")
        'AddStation(8886504, "LESSEN")
        'AddStation(8886504, "LESSINES")
        'AddStation(8812070, "ASSE")
        'AddStation(8895067, "LEDE")
        'AddStation(8893039, "MELLE")
        'AddStation(8814456, "BOONDAAL")
        'AddStation(8814415, "HUIZINGEN")
        'AddStation(8844628, "EUPEN")
        'AddStation(8884855, "PERUWELZ")
        'AddStation(8811817, "CEROUX MOUSTY")
        'AddStation(8865227, "POIX SAINT HUBERT")
        'AddStation(8894235, "ZELE")
        'AddStation(8821543, "SINT MARIABURG")
        'AddStation(8896735, "POPERINGE")
        'AddStation(8844545, "DOLHAIN GILEPPE")
        'AddStation(8821089, "ANTWERP NOORDERDOKKEN")
        'AddStation(8821089, "ANVERS NOORDERDOKKEN")
        'AddStation(8821089, "ANTWERPEN NOORDERDOKKEN")
        'AddStation(8864436, "MELREUX HOTTON")
        'AddStation(8872520, "LIGNY")
        'AddStation(8843331, "AMAY")
        'AddStation(8883121, "NEUFVILLES")
        'AddStation(8886553, "HOURAING")
        'AddStation(8821030, "ANWERP DAM")
        'AddStation(8821030, "ANVERS DAM")
        'AddStation(8821030, "ANTWERPEN DAM")
        'AddStation(8832003, "LEOPOLDSBURG")
        'AddStation(8832003, "BOURG LEOPOLD")
        'AddStation(8881406, "OBOURG")
        'AddStation(8811163, "BORDET")
        'AddStation(8822814, "BOOM")
        'AddStation(8871712, "LOBBES")
        'AddStation(8895505, "GERAARDSBERGEN")
        'AddStation(8895505, "GRAMMONT")
        'AddStation(8893070, "SCHELDEWINDEKE")
        'AddStation(8871100, "MARCHIENNE AU PONT")
        'AddStation(8812245, "SINT MARTENS BODEGEM")
        'AddStation(8822137, "MALDEREN")
        'AddStation(8863503, "DINANT")
        'AddStation(8896503, "IEPER")
        'AddStation(8896503, "YPRES")
        'AddStation(8864964, "NANINNE")
        'AddStation(8821071, "EKEREN")
        'AddStation(8833159, "SINT JORIS WEERT")
        'AddStation(8864915, "NATOYE")
        'AddStation(8865565, "CARLSBOURG")
        'AddStation(8874559, "LE CAMPINAIRE")
        'AddStation(8822608, "WILLEBROEK")
        'AddStation(8893211, "WONDELGEM")
        'AddStation(8812229, "GROOT BIJGAARDEN")
        'AddStation(8866175, "MARBEHAN")
        'AddStation(8866118, "VIVILLE")
        'AddStation(8814340, "BUIZINGEN")
        'AddStation(8871670, "LABUISSIERE")
        'AddStation(8842648, "MERY")
        'AddStation(8871662, "SOLRE SUR SAMBRE")
        'AddStation(8895430, "HERZELE")
        'AddStation(8811197, "MERODE")
        'AddStation(8881570, "FRAMERIES")
        'AddStation(8821022, "ANTWERP EAST")
        'AddStation(8821022, "ANVERS EST")
        'AddStation(8821022, "ANTWERPEN OOST")
        'AddStation(8873379, "COUR SUR HEURE")
        'AddStation(8881174, "MASNUY SAINT PIERRE")
        'AddStation(8842630, "TILFF")
        'AddStation(8881125, "GHLIN")
        'AddStation(8865615, "GRAIDE")
        'AddStation(8891157, "BELLEM")
        'AddStation(8822517, "HAACHT")
        'AddStation(8871415, "PIETON")
        'AddStation(8896149, "WAREGEM")
        'AddStation(8843240, "ENGIS")
        'AddStation(8896800, "ROULERS")
        'AddStation(8896800, "ROESELARE")
        'AddStation(8874583, "AISEAU")
        'AddStation(8894201, "LOKEREN")
        'AddStation(8883436, "SILLY")
        'AddStation(8883436, "OPZULLIK")
        'AddStation(8821857, "BOOISCHOT")
        'AddStation(8895638, "TOLLEMBEEK")
        'AddStation(8831005, "HASSELT")
        'AddStation(8863008, "NAMEN")
        'AddStation(8863008, "NAMUR")
        'AddStation(8872579, "VILLERS LA VILLE")
        'AddStation(8891140, "AALTER")
        'AddStation(8841442, "REMICOURT")
        'AddStation(8863453, "NAMECHE")
        'AddStation(8871381, "GODARVILLE")
        'AddStation(8811601, "OTTIGNIES")
        'AddStation(8883113, "ZINNIK")
        'AddStation(8883113, "SOIGNIES")
        'AddStation(8832573, "OVERPELT")
        'AddStation(8844206, "PEPINSTER")
        'AddStation(8811288, "HERENT")
        'AddStation(8895000, "AALST")
        'AddStation(8895000, "ALOST")
        'AddStation(8861150, "MOUSTIER")
        'AddStation(8874609, "TAMINES")
        'AddStation(8892254, "TIELT")

        'AddStation(8812013, "SIMONIS")
        'AddStation(8844644, "HERGENRATH")
        'AddStation(8866662, "MESSANCY")
        'AddStation(261547, "VOROUX GOREUX") 'TEC
        'AddStation(8821063, "ANTWERPEN LUCHTBAL")
        'AddStation(8814001, "BRUSSEL-WEST")

    End Sub

    ''' <summary>
    ''' Returns the ID of a station name
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function GetStationId(ByVal Name As String) As Integer
        Name = UCase(Replace(Name, "-", " "))

        If m_Stations.ContainsKey(Name) Then
            Return m_Stations(Name)
        End If
        Return -1
    End Function

    ''' <summary>
    ''' Returns time string for the selected station
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property Time() As String
        Get
            Return Format(Now(), "HH:mm")
        End Get
    End Property

    ''' <summary>
    ''' Gets / sets if we should display trams
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Show Tram")> _
    <System.ComponentModel.Description("Determines if trams are displayed.")> _
    Public Property ShowTram() As Boolean
        Get
            Return (m_Filter And FILTER_TRAM) <> 0
        End Get
        Set(ByVal value As Boolean)
            If value Then
                m_Filter = m_Filter Or FILTER_TRAM
            Else
                m_Filter = m_Filter And Not FILTER_TRAM
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets if we should display buses
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Show Buses")> _
    <System.ComponentModel.Description("Determines if buses are displayed.")> _
    Public Property Showbus() As Boolean
        Get
            Return (m_Filter And FILTER_BUS) <> 0
        End Get
        Set(ByVal value As Boolean)
            If value Then
                m_Filter = m_Filter Or FILTER_BUS
            Else
                m_Filter = m_Filter And Not FILTER_BUS
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets if we should display metros
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Show Metro")> _
    <System.ComponentModel.Description("Determines if metro trains are displayed.")> _
    Public Property ShowMetro() As Boolean
        Get
            Return (m_Filter And FILTER_METRO) <> 0
        End Get
        Set(ByVal value As Boolean)
            If value Then
                m_Filter = m_Filter Or FILTER_METRO
            Else
                m_Filter = m_Filter And Not FILTER_METRO
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets if we should display local trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Show L trains")>
    <System.ComponentModel.Description("Determines if local/slow trains are displayed.")>
    Public Property ShowLocalTrains() As Boolean
        Get
            Return (m_Filter And FILTER_L) <> 0
        End Get
        Set(ByVal value As Boolean)
            If value Then
                m_Filter = m_Filter Or FILTER_L
            Else
                m_Filter = m_Filter And Not FILTER_L
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets if we should display local trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Show S trains")>
    <System.ComponentModel.Description("Determines if S trains are displayed.")>
    Public Property ShowSTrains() As Boolean
        Get
            Return (m_Filter And FILTER_S) <> 0
        End Get
        Set(ByVal value As Boolean)
            If value Then
                m_Filter = m_Filter Or FILTER_S
            Else
                m_Filter = m_Filter And Not FILTER_S
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets if we should display intercity trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Show IC")> _
    <System.ComponentModel.Description("Determines if we should display intercity trains.")> _
    Public Property ShowICTrains() As Boolean
        Get
            Return (m_Filter And FILTER_IC) <> 0
        End Get
        Set(ByVal value As Boolean)
            If value Then
                m_Filter = m_Filter Or FILTER_IC
            Else
                m_Filter = m_Filter And Not FILTER_IC
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets if we should display international trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Show INT")> _
    <System.ComponentModel.Description("Determines if we should display international/high speed trains.")> _
    Public Property ShowINTTrains() As Boolean
        Get
            Return (m_Filter And FILTER_INT) <> 0
        End Get
        Set(ByVal value As Boolean)
            If value Then
                m_Filter = m_Filter Or FILTER_INT
            Else
                m_Filter = m_Filter And Not FILTER_INT
            End If
        End Set
    End Property

End Class
