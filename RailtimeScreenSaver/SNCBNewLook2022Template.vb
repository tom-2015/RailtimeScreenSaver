Imports System.Drawing.Drawing2D

Public Class SNCBNewLook2022Template
    Inherits ScreenSaverTemplate

    Protected m_Font As Font
    Protected m_UseColors As Boolean
    Protected m_ColorNormal As Color
    Protected m_ColorEurostar As Color
    Protected m_ColorThalys As Color
    Protected m_ColorICE As Color
    Protected m_ColorINT As Color
    Protected m_ColorICT As Color
    Protected m_ColorTGV As Color
    Protected m_ColorBus As Color
    Protected m_ColorTram As Color
    Protected m_ColorMetro As Color
    'Protected m_ColorRow1 As Color
    'Protected m_ColorRow2 As Color
    'Protected m_HeaderColor As Color
    Protected m_HeaderTextColor As Color
    Protected m_HeaderColor1 As Color
    Protected m_HeaderColor2 As Color
    Protected m_BackColor1 As Color
    Protected m_BackColor2 As Color
    Protected m_TextColor As Color
    Protected m_FontName As String
    Protected m_PrintTrainNumber As Boolean
    Protected m_LineColor1 As Color
    Protected m_LineColor2 As Color
    Protected m_DelayArrowColor As Color
    Protected m_DelayFontColor As Color

    'Protected m_TrainFontColor As Color
    'Protected m_FontSize As Single


    Public Sub New(ByVal Settings As Settings)
        MyBase.New(Settings)
        m_Font = SystemFonts.DefaultFont

        m_UseColors = Boolean.Parse(Settings.GetSetting("template_sncb_2022", "use_color", "false"))
        m_ColorNormal = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_default", Color.FromArgb(185, 160, 51).ToArgb()))
        m_ColorBus = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_bus", Color.Yellow.ToArgb()))
        m_ColorEurostar = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_eurostar", Color.Green.ToArgb()))
        m_ColorICE = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_ice", Color.White.ToArgb()))
        m_ColorICT = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_ict", Color.DarkGreen.ToArgb()))
        m_ColorINT = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_int", Color.FromArgb(&HFF, &H8F, &H0, &HFF).ToArgb()))
        m_ColorMetro = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_metro", Color.Yellow.ToArgb()))
        m_ColorTGV = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_tgv", Color.Yellow.ToArgb()))
        m_ColorThalys = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_thalys", Color.Red.ToArgb()))
        m_ColorTram = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "color_tram", Color.Yellow.ToArgb()))
        m_HeaderTextColor = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "header_text_color", Color.White.ToArgb()))
        m_HeaderColor1 = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "header_color1", Color.FromArgb(255, 0, 37, 145).ToArgb()))
        m_HeaderColor2 = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "header_color2", Color.FromArgb(255, 0, 74, 157).ToArgb()))
        m_BackColor1 = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "back_color1", Color.FromArgb(255, 0, 10, 20).ToArgb()))
        m_BackColor2 = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "back_color2", Color.FromArgb(255, 0, 25, 53).ToArgb()))
        m_TextColor = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "text_color", Color.White.ToArgb()))
        m_PrintTrainNumber = Settings.GetSetting("template_sncb_2022", "print_tn", False)
        'm_TrainFontColor = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "train_font_color", Color.White.ToArgb()))
        m_FontName = Settings.GetSetting("template_sncb_2022", "text_font", "Arial Rounded MT Bold")
        'm_FontSize = Settings.GetSetting("template_sncb_2022", "text_font_size", 10)
        m_LineColor1 = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "line_color1", Color.FromArgb(255, 200, 200, 200).ToArgb()))
        m_LineColor2 = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "line_color2", Color.FromArgb(255, 230, 230, 230).ToArgb()))
        m_DelayArrowColor = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "delay_arrow_color", Color.Red.ToArgb()))
        m_DelayFontColor = Color.FromArgb(Settings.GetSetting("template_sncb_2022", "delay_font_color", Color.White.ToArgb()))

        Try
            m_Font = New Font(m_FontName, 10) 'Arial Unicode MS ??
        Catch ex As Exception

        End Try

    End Sub

    Public Overrides Function Render(ByVal DisplayNr As Integer, ByVal Graphics As System.Drawing.Graphics, ByVal WindowSize As SizeF, ByVal TrainData As TrainDataGrabberAPI) As Boolean
        Const HeaderSize As Single = 1 / 10
        Dim HeaderHeight As Single = WindowSize.Height * HeaderSize

        TrainData.LockTrainData() 'prevent update of train data while we are painting 
        Graphics.Clear(Color.Black)

        Dim HeaderbackgroundBrush As New LinearGradientBrush(New PointF(0, 0), New PointF(WindowSize.Width / 2, HeaderHeight), m_HeaderColor1, m_HeaderColor2)
        Dim BackgroundBrush As New LinearGradientBrush(New PointF(WindowSize.Width / 2, 0), New PointF(WindowSize.Width, HeaderHeight), m_HeaderColor2, m_HeaderColor1)

        Graphics.FillRectangle(HeaderbackgroundBrush, New RectangleF(0, 0, WindowSize.Width / 2, HeaderHeight))

        Graphics.FillRectangle(BackgroundBrush, New RectangleF(WindowSize.Width / 2, 0, WindowSize.Width / 2, HeaderHeight))

        Dim CenterFormat As New StringFormat()
        CenterFormat.Alignment = StringAlignment.Center

        Dim TimeCenterX As Single = WindowSize.Width * 0.055263
        Dim TimeWidth As Single = WindowSize.Width * 0.086842
        Dim DelayX As Single = TimeCenterX + TimeWidth * 1.2 / 2 ' WindowSize.Width * 0.094736
        'Dim DelayWidth As Single = WindowSize.Width * 0.139266
        'Dim DestinationX As Single = WindowSize.Width * 0.252631
        Dim TrainTypeRightX As Single = WindowSize.Width * 0.930263
        Dim TrainTypeWidth As Single = WindowSize.Width * 0.0748502
        Dim TrackCenterX As Single = WindowSize.Width * 0.963157
        Dim TrackWidth As Single = WindowSize.Width * 0.0473684
        Dim CornerRadius As Single = WindowSize.Width * 0.004166 'for rounding corners
        Dim SeperaterLineHeight As Single = WindowSize.Height * 0.003 'height of the line between trains

        Dim StationSize As New SizeF(WindowSize.Width, WindowSize.Height * HeaderSize)
        Dim StationText As String = TrainData.StatioName

        If StationText IsNot Nothing Then
            Dim HeaderBrush As New SolidBrush(m_HeaderTextColor)
            Dim HeaderFont As Font = PrintCenteredAutoFontSize(StationText, New RectangleF(0, 0, StationSize.Width, StationSize.Height), Graphics, m_Font, HeaderBrush)

            Dim ClockSize As SizeF = Graphics.MeasureString(TrainData.Time(), HeaderFont)
            Dim ClockLocation As PointF = New PointF(TimeCenterX - ClockSize.Width / 2 + WindowSize.Width * 0.008, 0)

            Graphics.DrawString(TrainData.Time(), HeaderFont, HeaderBrush, ClockLocation)

            If (Now.Second Mod 2) = 0 AndAlso TrainData.Time().Length > 2 Then

                Dim sFormat As New StringFormat()
                sFormat.SetMeasurableCharacterRanges({New CharacterRange(2, 1)})

                Dim Chars() As Region = Graphics.MeasureCharacterRanges(TrainData.Time(), HeaderFont, New RectangleF(ClockLocation, ClockSize), sFormat)

                'fill the : sign in the clock with the original background
                Graphics.FillRectangle(HeaderbackgroundBrush, Chars(0).GetBounds(Graphics))

            End If

        End If


            Dim gPath As New GraphicsPath()
        Dim r As Double = Math.Sqrt((WindowSize.Height - HeaderHeight) ^ 2 + (WindowSize.Width / 2) ^ 2)
        gPath.AddEllipse(New Rectangle((WindowSize.Width / 2) - r, HeaderHeight - r, r * 2, r * 2))

        Dim PathGradient As New PathGradientBrush(gPath)

        'Dim m_BackColor1 As Color = Color.FromArgb(255, 0, 10, 20)
        'Dim m_BackColor2 As Color = Color.FromArgb(255, 0, 25, 53)
        PathGradient.CenterColor = m_BackColor2
        PathGradient.SurroundColors = {m_BackColor1}

        Graphics.FillRectangle(PathGradient, New Rectangle(0, HeaderHeight, WindowSize.Width, WindowSize.Height - HeaderHeight))

        'print trains
        Dim TrainsToDisplay As Integer = Math.Min(TrainData.MaxTrains, TrainData.Trains.Count)

        Dim TrainLineHeight As Single = (WindowSize.Height - HeaderHeight) / IIf(TrainsToDisplay = 0, 1, TrainsToDisplay) - SeperaterLineHeight
        Dim DelayRectHeight As Single = TrainLineHeight * 0.8
        Dim TrainFont As Font = New Font(m_Font.FontFamily, FindFontSizeHeight(Graphics, "012345679AZERTYUIOPQSDFGHJKLMWXCVBNazertyuiopqsdfghjklmwxcvbsn',?./:;:ùµµ^$g[]}", TrainLineHeight, m_Font)) 'find the best height that fit's all characters
        Dim DelayFont As Font = New Font(m_Font.FontFamily, TrainFont.Size * 0.8)

        Dim TrainFontSize As SizeF = Graphics.MeasureString("01679AZERTYIOPQSDCVBNtyuiopqsdfghjklsn',?./:;µµ^$g[}", TrainFont)

        Dim DelayArrowBrush As New SolidBrush(m_DelayArrowColor)
        Dim DelayFontBrush As New SolidBrush(m_DelayFontColor)
        Dim DelayMaxTextSize As SizeF = Graphics.MeasureString("+999 > 00:00", DelayFont)
        Dim DelayWidth As Single = DelayMaxTextSize.Width
        Dim DelayRectSize As SizeF = New SizeF(DelayWidth, DelayRectHeight)
        Dim DelaySize As SizeF = Graphics.MeasureString("+999", DelayFont) 'only size of max delay in min.
        Dim GTSize As SizeF = Graphics.MeasureString(">", DelayFont) 'size of the arrow
        Dim DestinationX As Single = DelayX + DelayWidth * 1.2

        Dim TextBrush As New SolidBrush(m_TextColor)
        Dim AltTextBrush As New SolidBrush(m_BackColor1)

        Dim AirPlaneSize As Integer = Graphics.MeasureString("XX", TrainFont).Width * 4 / 5
        Dim AirplaneIcon As Bitmap = My.Resources.airplane
        Dim AirplanePallete As Imaging.ColorPalette = AirplaneIcon.Palette
        Dim RenderAirPlane As Boolean = (Now.Second Mod 2) = 0
        If RenderAirPlane Then
            AirplanePallete.Entries(0) = TextBrush.Color
            AirplanePallete.Entries(1) = Color.FromArgb(0, 0, 0, 0)
            AirplaneIcon.Palette = AirplanePallete
        End If

        Dim TrainNumberRightX As Single = TrainTypeRightX - TrainTypeWidth - AirPlaneSize - WindowSize.Width * 0.05
        Dim Trainnumberwidth As Single = Graphics.MeasureString("0", TrainFont).Width * 6

        Dim RightToLeftLayoutFormat As New StringFormat()
        RightToLeftLayoutFormat.Alignment = StringAlignment.Far

        For i As Integer = 0 To TrainsToDisplay - 1
            Dim TrainLineY As Single = (TrainLineHeight + SeperaterLineHeight) * i + HeaderHeight
            If i < TrainData.Trains.Count Then

                With TrainData.Trains(i)

                    Dim TimeRect As New RectangleF(TimeCenterX - TimeWidth / 2, TrainLineY, TimeWidth, TrainLineHeight)

                    PrintCentered(.Time, TimeRect, Graphics, TrainFont, TextBrush)

                    Graphics.DrawString(.Destination, TrainFont, New SolidBrush(GetTextColor(TrainData.Trains(i))), New PointF(DestinationX, TrainLineY))

                    Dim TrackRect As New RectangleF(TrackCenterX - TrackWidth / 2, TrainLineY + TrainLineHeight * 0.1, TrackWidth, DelayRectHeight)

                    If TrainData.Trains(i).TrackChanged Then
                        Graphics.FillPath(TextBrush, New RoundedRectangle(TrackRect, CornerRadius).GetGraphicsPath)
                        PrintCentered(.Track, TrackRect, Graphics, TrainFont, AltTextBrush)
                    Else
                        PrintCentered(.Track, TrackRect, Graphics, TrainFont, TextBrush)
                    End If

                    Dim TrainTypeRect As New RectangleF(TrainTypeRightX - TrainTypeWidth, TrainLineY, TrainTypeWidth, TrainLineHeight)

                    Graphics.DrawString(.Type, TrainFont, TextBrush, TrainTypeRect, RightToLeftLayoutFormat)

                    If .IsAirportTrain AndAlso RenderAirPlane Then
                        Dim TrainTypeSize As SizeF = Graphics.MeasureString(.Type, TrainFont)
                        Graphics.DrawImage(AirplaneIcon, New RectangleF(TrainTypeRightX - TrainTypeSize.Width - AirPlaneSize, TrainTypeRect.Y, AirPlaneSize, AirPlaneSize))
                    End If

                    If m_PrintTrainNumber Then
                        Graphics.DrawString(.TrainNumber, TrainFont, TextBrush, New RectangleF(TrainNumberRightX, TrainTypeRect.Y, Trainnumberwidth, TrainTypeRect.Height), RightToLeftLayoutFormat)
                    End If

                    If .Delay > 0 OrElse .Info <> "" Then
                        'Dim DelayRectSize As SizeF = New SizeF(DelayWidth, DelayRectHeight)
                        'Dim DelaySize As SizeF = Graphics.MeasureString("+999", DelayFont)
                        ' Dim GTSize As SizeF = Graphics.MeasureString(">", DelayFont)

                        Dim DelayContainer As Drawing2D.GraphicsContainer = Graphics.BeginContainer()
                        Graphics.TranslateTransform(DelayX, TrainLineY + TrainLineHeight * 0.1)

                        Dim DelayRect As New RectangleF(0, 0, DelayRectSize.Width, DelayRectSize.Height)

                        'draw delay
                        If .Delay > 0 Then

                            'draw white rectangle
                            Graphics.FillPath(TextBrush, New RoundedRectangle(DelayRect, CornerRadius).GetGraphicsPath)

                            'draw red rectangle
                            Dim DelayPath As New Drawing2D.GraphicsPath
                            DelayPath.AddArc(DelayRect.X, DelayRect.Y, CornerRadius, CornerRadius, 180, 90)
                            DelayPath.AddLine(New PointF(DelaySize.Width, 0), New PointF(DelaySize.Width + GTSize.Width / 2, DelayRectSize.Height / 2))
                            DelayPath.AddLine(New PointF(DelaySize.Width + GTSize.Width / 2, DelayRectSize.Height / 2), New PointF(DelaySize.Width, DelayRectSize.Height))
                            DelayPath.AddArc(DelayRect.X, DelayRect.Bottom - CornerRadius, CornerRadius, CornerRadius, 90, 90)
                            DelayPath.CloseFigure()

                            Graphics.FillPath(DelayArrowBrush, DelayPath)

                            'draw delay
                            Graphics.DrawString("+" & .Delay, DelayFont, DelayFontBrush, DelayRect.X, 0)

                            Dim NewTime As String = SecToTime(TimeToSec(.Time) + .Delay * 60)

                            Graphics.DrawString(NewTime, DelayFont, AltTextBrush, New RectangleF(DelayRect.X + DelaySize.Width + GTSize.Width / 2, DelayRect.Y, DelayWidth - (DelaySize.Width + GTSize.Width / 2), DelayRectSize.Height))

                        ElseIf .Info <> "" Then

                            'draw red rectangle
                            Dim rounded As New RoundedRectangle(DelayRect, CornerRadius)

                            Graphics.FillPath(New SolidBrush(Color.Red), rounded.GetGraphicsPath)

                            Dim Format As New StringFormat()
                            Format.Alignment = StringAlignment.Center
                            Format.LineAlignment = StringAlignment.Center

                            Graphics.DrawString(.Info, DelayFont, New SolidBrush(Color.White), DelayRect, Format)
                        End If

                        Graphics.EndContainer(DelayContainer)
                    End If

                    Dim LineGradient As New PathGradientBrush(gPath)

                    LineGradient.CenterColor = m_LineColor1 ' Color.FromArgb(255, 15, 42, 64)
                    LineGradient.SurroundColors = {m_LineColor2} 'Color.FromArgb(255, 10, 25, 39)

                    Graphics.DrawLine(New Pen(LineGradient, SeperaterLineHeight), New PointF(TimeRect.Left, TrainLineY + TrainLineHeight + SeperaterLineHeight / 2), New PointF(TrackRect.Right, TrainLineY + TrainLineHeight + SeperaterLineHeight / 2))

                End With
            End If
        Next

        TrainData.UnlockTrainData()
        Return True
    End Function

    Private Function GetTextColor(ByVal TrainData As TrainData) As Color
        If m_UseColors Then
            Select Case UCase$(TrainData.Type)
                Case "EUR", "EST"
                    Return m_ColorEurostar
                Case "THA"
                    Return m_ColorThalys
                Case "ICE"
                    Return m_ColorICE
                Case "INT"
                    Return m_ColorINT
                Case "ICT", "EXT"
                    Return m_ColorICT
                Case "TGV"
                    Return m_ColorTGV
                Case "BUS"
                    Return m_ColorBus
                Case "TRA"
                    Return m_ColorTram
                Case "MET"
                    Return m_ColorMetro
                Case Else
                    Return m_ColorNormal
            End Select
        End If
        Return m_ColorNormal
    End Function

    Public Overrides Sub SaveSettings(ByVal Settings As Settings)
        Settings.SaveSetting("template_sncb_2022", "use_color", m_UseColors)
        Settings.SaveSetting("template_sncb_2022", "color_default", m_ColorNormal.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "color_bus", m_ColorBus.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "color_eurostar", m_ColorEurostar.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "color_ice", m_ColorICE.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "color_int", m_ColorINT.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "color_metro", m_ColorMetro.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "color_tgv", m_ColorTGV.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "color_thalys", m_ColorThalys.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "color_tram", m_ColorTram.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "header_text_color", m_HeaderTextColor.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "header_color1", m_HeaderColor1.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "header_color2", m_HeaderColor2.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "back_color1", m_BackColor1.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "back_color2", m_BackColor2.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "text_color", m_TextColor.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "text_font", m_FontName)
        Settings.SaveSetting("template_sncb_2022", "print_tn", m_PrintTrainNumber)
        Settings.SaveSetting("template_sncb_2022", "line_color1", m_LineColor1.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "line_color2", m_LineColor2.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "delay_arrow_color", m_DelayArrowColor.ToArgb())
        Settings.SaveSetting("template_sncb_2022", "delay_font_color", m_DelayFontColor.ToArgb())

        'Settings.SaveSetting("template_sncb_2022", "text_font_size", m_FontSize)

    End Sub

    ''' <summary>
    ''' Gets / sets if we should use different colors for special trains like Thalys
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enable / disable use of different colors for special trains like Thalys, Eurostar,...")>
    Public Property UseColors() As Boolean
        Get
            Return m_UseColors
        End Get
        Set(ByVal value As Boolean)
            m_UseColors = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets the default text color fpr trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Default text color for trains.")>
    Public Property ColorDefault() As Color
        Get
            Return m_ColorNormal
        End Get
        Set(ByVal value As Color)
            m_ColorNormal = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / Sets the default text color
    ''' </summary>
    ''' <returns></returns>
    <System.ComponentModel.Description("Changes the default text color.")>
    Public Property TextColor() As Color
        Get
            Return m_TextColor
        End Get
        Set(ByVal value As Color)
            m_TextColor = value
        End Set
    End Property

    ''' <summary>
    ''' Color for buses
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the color to use for a bus.")>
    Public Property ColorBus() As Color
        Get
            Return m_ColorBus
        End Get
        Set(ByVal value As Color)
            m_ColorBus = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / Sets the color for Eurostar trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for Eurostar trains.")>
    Public Property ColorEurostar() As Color
        Get
            Return m_ColorEurostar
        End Get
        Set(ByVal value As Color)
            m_ColorEurostar = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / Sets the color for ICE trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for ICE trains.")>
    Public Property ColorICE() As Color
        Get
            Return m_ColorICE
        End Get
        Set(ByVal value As Color)
            m_ColorICE = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / Sets color for extra and ICT trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for ICT / EXT trains.")>
    Public Property ColorICT() As Color
        Get
            Return m_ColorICT
        End Get
        Set(ByVal value As Color)
            m_ColorICT = value
        End Set
    End Property

    ''' <summary>
    ''' Color for international trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for international trains.")>
    Public Property ColorINT() As Color
        Get
            Return m_ColorINT
        End Get
        Set(ByVal value As Color)
            m_ColorINT = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets the color for metro trains
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for metro.")>
    Public Property ColorMetro() As Color
        Get
            Return m_ColorMetro
        End Get
        Set(ByVal value As Color)
            m_ColorMetro = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets the color for TGV
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for TGV trains.")>
    Public Property ColorTGV() As Color
        Get
            Return m_ColorTGV
        End Get
        Set(ByVal value As Color)
            m_ColorTGV = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / Sets the color for thalys
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for Thalys trains.")>
    Public Property ColorThalys() As Color
        Get
            Return m_ColorThalys
        End Get
        Set(ByVal value As Color)
            m_ColorThalys = value
        End Set
    End Property

    ''' <summary>
    ''' Color for trams
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for trams.")>
    Public Property ColorTram() As Color
        Get
            Return m_ColorTram
        End Get
        Set(ByVal value As Color)
            m_ColorTram = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets header background color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Header background color 1 (gradient).")>
    Public Property HeaderColor1() As Color
        Get
            Return m_HeaderColor1
        End Get
        Set(ByVal value As Color)
            m_HeaderColor1 = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets header background color
    ''' </summary>
    ''' <returns></returns>
    <System.ComponentModel.Description("Header background color 2 (gradient).")>
    Public Property HeaderColor2() As Color
        Get
            Return m_HeaderColor2
        End Get
        Set(ByVal value As Color)
            m_HeaderColor2 = value
        End Set
    End Property

    ''' <summary>
    ''' Header text color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Header text color.")>
    Public Property HeaderTextColor() As Color
        Get
            Return m_HeaderTextColor
        End Get
        Set(ByVal value As Color)
            m_HeaderTextColor = value
        End Set
    End Property

    ''' <summary>
    ''' Line gradient color 1
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Line gradient color 1.")>
    Public Property LineColor1() As Color
        Get
            Return m_LineColor1
        End Get
        Set(ByVal value As Color)
            m_LineColor1 = value
        End Set
    End Property

    ''' <summary>
    ''' Line gradient color 2
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Line gradient color 2.")>
    Public Property LineColor2() As Color
        Get
            Return m_LineColor2
        End Get
        Set(ByVal value As Color)
            m_LineColor2 = value
        End Set
    End Property

    ''' <summary>
    ''' Color of the red delay arrow.
    ''' </summary>
    ''' <returns></returns>
    <System.ComponentModel.Description("Color of the red delay arrow.")>
    Public Property DelayArrowColor() As Color
        Get
            Return m_DelayArrowColor
        End Get
        Set(ByVal value As Color)
            m_DelayArrowColor = value
        End Set
    End Property

    ''' <summary>
    ''' Font color inside the red delay arrow.
    ''' </summary>
    ''' <returns></returns>
    <System.ComponentModel.Description("Font color inside the red delay arrow.")>
    Public Property DelayFontColor() As Color
        Get
            Return m_DelayFontColor
        End Get
        Set(ByVal value As Color)
            m_DelayFontColor = value
        End Set
    End Property


    ''' <summary>
    ''' Gets / sets header background color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Background color 1 (gradient).")>
    Public Property BackgroundColor1() As Color
        Get
            Return m_BackColor1
        End Get
        Set(ByVal value As Color)
            m_BackColor1 = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets header background color
    ''' </summary>
    ''' <returns></returns>
    <System.ComponentModel.Description("Background color 2 (gradient).")>
    Public Property BackgroundColor2() As Color
        Get
            Return m_BackColor2
        End Get
        Set(ByVal value As Color)
            m_BackColor2 = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / Sets the font name
    ''' </summary>
    ''' <returns></returns>
    <System.ComponentModel.Description("Font name.")>
    Public Property FontName() As String
        Get
            Return m_FontName
        End Get
        Set(ByVal value As String)
            m_FontName = value
        End Set
    End Property

    ''' <summary>
    ''' Returns if the train number must be printed
    ''' </summary>
    ''' <returns></returns>
    <System.ComponentModel.Description("Print the train number.")>
    Public Property PrintTrainNr() As Boolean
        Get
            Return m_PrintTrainNumber
        End Get
        Set(ByVal value As Boolean)
            m_PrintTrainNumber = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / Sets the font name
    ''' </summary>
    ''' <returns></returns>
    '<System.ComponentModel.Description("Font name.")>
    'Public Property FontName() As String
    '    Get
    '        Return m_FontName
    '    End Get
    '    Set(ByVal value As String)
    '        m_FontName = value
    '    End Set
    'End Property

    ''' <summary>
    ''' Gets / Sets the font size
    ''' </summary>
    ''' <returns></returns>
    '<System.ComponentModel.Description("Font size.")>
    'Public Property FontSize() As Single
    '    Get
    '        Return m_FontSize
    '    End Get
    '    Set(ByVal value As Single)
    '        m_FontSize = value
    '    End Set
    'End Property

End Class
