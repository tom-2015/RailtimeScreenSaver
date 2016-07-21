Imports System.Threading
Imports System.ComponentModel

Public Class TrainData

    <Flags()> _
    Public Enum TrainTypeFlags As Long
        TrainTypeNormal = 0
        TrainTypeSBahn = 1
        TrainTypeAirport = 2
    End Enum

    Protected m_Time As String
    Protected m_TrainNumber As String
    Protected m_Destination As String
    Protected m_Type As String
    Protected m_Delay As Integer
    Protected m_Track As String
    Protected m_Info As String
    Protected m_TrackChanged As Boolean
    Protected m_Flags As TrainTypeFlags


    Public Sub New()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Time"></param>
    ''' <param name="TrainNumber"></param>
    ''' <param name="Destination"></param>
    ''' <param name="Type"></param>
    ''' <param name="Delay"></param>
    ''' <param name="Track"></param>
    ''' <param name="Info">info, delay as text or cancelled,...</param>
    ''' <param name="TrackChanged"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Time As String, ByVal TrainNumber As String, ByVal Destination As String, ByVal Type As String, ByVal Delay As Integer, ByVal Track As String, ByVal Info As String, ByVal TrackChanged As Boolean, ByVal Flags As TrainTypeFlags)
        m_Time = Time
        m_TrainNumber = TrainNumber
        m_Destination = Destination
        m_Type = Type
        m_Delay = Delay
        m_Track = Track
        m_Info = Info
        m_TrackChanged = TrackChanged
        m_Flags = Flags
    End Sub


    Public Property Time() As String
        Get
            Return m_Time
        End Get
        Set(ByVal value As String)
            m_Time = value
        End Set
    End Property

    Public Property TrainNumber() As String
        Get
            Return m_TrainNumber
        End Get
        Set(ByVal value As String)
            m_TrainNumber = value
        End Set
    End Property

    Public Property Destination() As String
        Get
            Return m_Destination
        End Get
        Set(ByVal value As String)
            m_Destination = value
        End Set
    End Property

    Public Property Type() As String
        Get
            Return m_Type
        End Get
        Set(ByVal value As String)
            m_Type = value
        End Set
    End Property

    Public Property Track() As String
        Get
            Return m_Track
        End Get
        Set(ByVal value As String)
            m_Track = value
        End Set
    End Property

    Public Property Info() As String
        Get
            Return m_Info
        End Get
        Set(ByVal value As String)
            m_Info = value
        End Set
    End Property

    Public Property TrackChanged() As String
        Get
            Return m_TrackChanged
        End Get
        Set(ByVal value As String)
            m_TrackChanged = value
        End Set
    End Property

    Public Property Delay() As Integer
        Get
            Return m_Delay
        End Get
        Set(ByVal value As Integer)
            m_Delay = value
        End Set
    End Property

    Public Property Flags() As TrainTypeFlags
        Get
            Return m_Flags
        End Get
        Set(ByVal value As TrainTypeFlags)
            m_Flags = value
        End Set
    End Property

    Public ReadOnly Property IsSBahnTrain() As Boolean
        Get
            Return (m_Flags And TrainTypeFlags.TrainTypeSBahn) <> 0
        End Get
    End Property

    Public ReadOnly Property IsAirportTrain() As Boolean
        Get
            Return (m_Flags And TrainTypeFlags.TrainTypeAirport) <> 0
        End Get
    End Property

End Class

Public MustInherit Class TrainDataGrabberAPI

    Public Enum DepArrTypes
        Departure
        Arrival
    End Enum

    Protected m_Trains As New List(Of TrainData)
    Protected m_Settings As Settings
    Protected m_StationName As String 'station name to display on screen
    Protected m_Station As String 'station name to get data from
    Protected m_DepArr As DepArrTypes
    Protected m_MaxTrains As Integer
    Protected m_TrainDataMutex As Mutex
    Protected m_RefreshDelay As Integer

    Public Sub New(ByVal Settings As Settings)
        m_Settings = Settings
        m_DepArr = Settings.GetSetting("general", "dep_arr", DepArrTypes.Departure)
        m_MaxTrains = Settings.GetSetting("general", "max_trains", 25)
        m_RefreshDelay = Settings.GetSetting("general", "refresh_delay", 60)
        m_Station = Settings.GetSetting("general", "station", "Brussels-Midi")
        m_TrainDataMutex = New Mutex()
    End Sub

    ''' <summary>
    ''' Locks access to the train data properties
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LockTrainData()
        m_TrainDataMutex.WaitOne()
    End Sub

    ''' <summary>
    ''' Release access for other threads to train data
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UnlockTrainData()
        m_TrainDataMutex.ReleaseMutex()
    End Sub

    ''' <summary>
    ''' Refresh the train list at time
    ''' </summary>
    ''' <param name="Time"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function RefreshTrainData(ByVal Time As DateTime) As Boolean

    ''' <summary>
    ''' Returns time string for the selected station
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Browsable(False)> _
    Public MustOverride ReadOnly Property Time() As String

    ''' <summary>
    ''' Saves the settings
    ''' </summary>
    ''' <param name="Settings"></param>
    ''' <remarks></remarks>
    Public Overridable Sub SaveSettings(ByVal Settings As Settings)
        Settings.SaveSetting("general", "dep_arr", m_DepArr)
        Settings.SaveSetting("general", "max_trains", m_MaxTrains)
        Settings.SaveSetting("general", "refresh_delay", m_RefreshDelay)
        Settings.SaveSetting("general", "station", m_Station)
    End Sub


    ''' <summary>
    ''' Returns the station name for selected language
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Browsable(False)> _
    Public Overridable ReadOnly Property StatioName() As String
        Get
            Return m_StationName
        End Get
    End Property

    ''' <summary>
    ''' Returns if we display departure or arrival times
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Display")> _
    <System.ComponentModel.Description("Display departing or arriving trains.")> _
    Public Overridable Property DepartureArrival() As DepArrTypes
        Get
            Return m_DepArr
        End Get
        Set(ByVal value As DepArrTypes)
            m_DepArr = value
        End Set
    End Property

    ''' <summary>
    ''' Returns text for departure/arrival (language dependend)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Browsable(False)> _
    Public MustOverride ReadOnly Property DepartureArrivalText() As String

    ''' <summary>
    ''' Returns all trains in the selected station
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Browsable(False)> _
    Public Overridable ReadOnly Property Trains() As List(Of TrainData)
        Get
            Return m_Trains
        End Get
    End Property


    ''' <summary>
    ''' Returns max nr of trains to display
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Max. trains")> _
    <System.ComponentModel.Description("Maximum trains displayed on the screen.")> _
    Public Overridable Property MaxTrains() As Integer
        Get
            Return m_MaxTrains
        End Get
        Set(ByVal value As Integer)
            m_MaxTrains = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / Sets the refresh delay in seconds
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Refresh Delay")> _
    <System.ComponentModel.Description("Number of seconds before refreshing the train data.")> _
    Public Overridable Property RefreshDelay() As Integer
        Get
            Return m_RefreshDelay
        End Get
        Set(ByVal value As Integer)
            m_RefreshDelay = value
            If m_RefreshDelay = 0 Then m_RefreshDelay = 1
        End Set
    End Property

    ''' <summary>
    ''' Returns the station
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.DisplayName("Station")> _
    <System.ComponentModel.Description("The station name you want to display trains.")> _
    Public Overridable Property Station() As String
        Get
            Return m_Station
        End Get
        Set(ByVal value As String)
            m_Station = value
        End Set
    End Property

End Class
