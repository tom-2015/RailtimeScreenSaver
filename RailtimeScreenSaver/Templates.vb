

Public Class SNCBTemplate
    Inherits ScreenSaverTemplate

    Protected m_Font As Font
    Protected m_Color1 As Color
    Protected m_Color2 As Color
    Protected m_ForeColor As Color
    Protected m_BackColor As Color
    Protected m_DelayBackColor As Color
    Protected m_Columns As Integer

    Protected m_UseColors As Boolean
    Protected m_ColorEurostar As Color
    Protected m_ColorThalys As Color
    Protected m_ColorICE As Color
    Protected m_ColorINT As Color
    Protected m_ColorICT As Color
    Protected m_ColorTGV As Color
    Protected m_ColorBus As Color
    Protected m_ColorTram As Color
    Protected m_ColorMetro As Color

    Public Sub New(ByVal Settings As Settings)
        MyBase.New(Settings)
        m_Font = New Font("Arial", 10)
        m_Color1 = Color.FromArgb(Settings.GetSetting("template_sncb", "color1", Color.FromArgb(255, 63, 168, 249).ToArgb()))
        m_Color2 = Color.FromArgb(Settings.GetSetting("template_sncb", "color2", Color.FromArgb(255, 30, 124, 186).ToArgb()))
        m_ForeColor = Color.FromArgb(Settings.GetSetting("template_sncb", "forecolor", Color.White.ToArgb()))
        m_BackColor = Color.FromArgb(Settings.GetSetting("template_sncb", "backcolor", Color.Black.ToArgb()))
        m_DelayBackColor = Color.FromArgb(Settings.GetSetting("template_sncb", "delay_backcolor", Color.OrangeRed.ToArgb()))
        m_UseColors = Boolean.Parse(Settings.GetSetting("template_sncb", "use_color", "true"))
        m_ColorBus = Color.FromArgb(Settings.GetSetting("template_sncb", "color_bus", Color.Yellow.ToArgb()))
        m_ColorEurostar = Color.FromArgb(Settings.GetSetting("template_sncb", "color_eurostar", Color.Green.ToArgb()))
        m_ColorICE = Color.FromArgb(Settings.GetSetting("template_sncb", "color_ice", Color.Orange.ToArgb()))
        m_ColorICT = Color.FromArgb(Settings.GetSetting("template_sncb", "color_ict", Color.DarkGreen.ToArgb()))
        m_ColorINT = Color.FromArgb(Settings.GetSetting("template_sncb_", "color_int", Color.FromArgb(&HFF, &H8F, &H0, &HFF).ToArgb()))
        m_ColorMetro = Color.FromArgb(Settings.GetSetting("template_sncb", "color_metro", Color.Yellow.ToArgb()))
        m_ColorTGV = Color.FromArgb(Settings.GetSetting("template_sncb", "color_tgv", Color.Yellow.ToArgb()))
        m_ColorThalys = Color.FromArgb(Settings.GetSetting("template_sncb", "color_thalys", Color.Red.ToArgb()))
        m_ColorTram = Color.FromArgb(Settings.GetSetting("template_sncb", "color_tram", Color.Yellow.ToArgb()))

        m_Columns = Settings.GetSetting("template_sncb", "columns", 2)
    End Sub

    ''' <summary>
    ''' Color for train row
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for even train rows.")> _
    Public Property RowColor1() As Color
        Get
            Return m_Color1
        End Get
        Set(ByVal value As Color)
            m_Color1 = value
        End Set
    End Property

    ''' <summary>
    ''' Alternative color for train row
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Color for odd train rows.")> _
    Public Property RowColor2() As Color
        Get
            Return m_Color2
        End Get
        Set(ByVal value As Color)
            m_Color2 = value
        End Set
    End Property

    ''' <summary>
    ''' Text color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text color.")> _
    Public Property ForeColor() As Color
        Get
            Return m_ForeColor
        End Get
        Set(ByVal value As Color)
            m_ForeColor = value
        End Set
    End Property

    ''' <summary>
    ''' Background color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Background color.")> _
    Public Property BackColor() As Color
        Get
            Return m_BackColor
        End Get
        Set(ByVal value As Color)
            m_BackColor = value
        End Set
    End Property

    ''' <summary>
    ''' Background color for delays / problems
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Background color for delays and problems.")>
    Public Property DelayBackColor() As Color
        Get
            Return m_DelayBackColor
        End Get
        Set(ByVal value As Color)
            m_DelayBackColor = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / Sets number of columns to display on 1 screen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets number of columns to display on 1 screen.")>
    Public Property Columns() As Integer
        Get
            Return m_Columns
        End Get
        Set(ByVal value As Integer)
            m_Columns = value
            If m_Columns <= 0 Then m_Columns = 1
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets if we should use different colors for special trains like Thalys
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enable / disable use of different colors for special trains or busses/trams like Thalys, Eurostar,...")>
    Public Property UseColors() As Boolean
        Get
            Return m_UseColors
        End Get
        Set(ByVal value As Boolean)
            m_UseColors = value
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



    Protected Function GetTrainRectColor(ByVal Data As TrainData, ByVal Index As Integer) As Color
        Dim DefaultColor As Color = IIf(Index Mod 2, m_Color1, m_Color2)
        If m_UseColors Then
            Select Case UCase$(Data.Type)
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
                    Return DefaultColor
            End Select
        End If
        Return DefaultColor
    End Function

    Protected Function GetTrainTextBrush(ByVal BackColor As Color, ByVal NormalBrush As Brush, ByVal DarkBrush As Brush) As Brush
        If m_UseColors Then
            Dim Luminance As Single = (0.299 * BackColor.R + 0.587 * BackColor.G + 0.114 * BackColor.B) / 255

            If Luminance > 0.7 Then
                Return DarkBrush
            Else
                Return NormalBrush
            End If
        Else
            Return NormalBrush
        End If

    End Function

    ''' <summary>
    ''' Renders the template on a graphics object
    ''' </summary>
    ''' <param name="DisplayNr"></param>
    ''' <param name="Graphics"></param>
    ''' <param name="WindowSize"></param>
    ''' <param name="TrainData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Render(ByVal DisplayNr As Integer, ByVal Graphics As System.Drawing.Graphics, ByVal WindowSize As SizeF, ByVal TrainData As TrainDataGrabberAPI) As Boolean
        Dim nColumns As Integer = Math.Max(m_Columns, 2)
        Dim ColumnWidth As Single = WindowSize.Width / nColumns
        Dim TrainsPerColumn As Integer = (TrainData.MaxTrains / nColumns)
        Dim RowHeight As Single = WindowSize.Height / TrainsPerColumn
        Dim BorderWidth As Single = RowHeight * 0.05
        Dim m_ForeColorBrush As New SolidBrush(m_ForeColor)
        Dim m_DarkForeColorBrush As New SolidBrush(Color.Black)

        TrainData.LockTrainData()
        Graphics.Clear(m_BackColor)

        Dim Tijd As String = TrainData.Time()

        If (Now.Second Mod 2) = 0 Then Tijd = Replace(Tijd, ":", " ")

        Dim TimeSize As New SizeF(ColumnWidth / 3, RowHeight * 0.6)
        Dim TimeFont As Font = FindFontSize(Graphics, Tijd, TimeSize, m_Font)

        Graphics.DrawString(Tijd, TimeFont, m_ForeColorBrush, BorderWidth, BorderWidth)

        Dim StationText As String = TrainData.DepartureArrivalText & " " & TrainData.StatioName
        Dim StationTextSize As SizeF = Graphics.MeasureString(StationText, TimeFont)

        Graphics.DrawString(StationText, TimeFont, m_ForeColorBrush, ColumnWidth - BorderWidth - StationTextSize.Width, RowHeight - StationTextSize.Height - BorderWidth * 2)

        Dim TrainLineHeight As Single = RowHeight / 2
        Dim TrainTextLineHeight As Single = TrainLineHeight - 2 * BorderWidth '(RowHeight - 4 * BorderWidth) / 2
        Dim TrainFont As Font = New Font(m_Font.FontFamily, FindFontSizeHeight(Graphics, "012345679AZERTYUIOPQSDFGHJKLMWXCVBNazertyuiopqsdfghjklmwxcvbsn',?./:;:ùµµ^$g[]}", TrainTextLineHeight, m_Font)) 'find the best height that fit's all characters
        Dim TrainTimeSize As SizeF = Graphics.MeasureString("00:00 ", TrainFont)
        Dim TrainIndex As Integer = 0 'index of next train info to print

        For j = 0 To nColumns - 1
            Dim Container As Drawing2D.GraphicsContainer = Graphics.BeginContainer()
            Graphics.TranslateTransform(ColumnWidth * j, 0)
            Dim TrainLineCount As Integer = TrainsPerColumn * 2 'by default this is the ammount of lines that we have
            Dim TrainLineIndex As Integer = 0 'this is the next line we can print on
            If j = 0 Then
                TrainLineIndex = 2 'first item is departure text / station info, if column 
            End If

            For i As Integer = 0 To TrainsPerColumn - 1 'trainspercolumn is the max number of trains that we can print in a column
                If TrainIndex < TrainData.Trains.Count Then
                    Dim Train As TrainData = TrainData.Trains(TrainIndex)
                    If TrainLineIndex + TrainLinesNeeded(Train) <= TrainLineCount Then 'check if there is enough space left
                        Dim TrainContainer As Drawing2D.GraphicsContainer = Graphics.BeginContainer()

                        'Graphics.TranslateTransform(0, TrainLineIndex * (TrainLineHeight + 2 * BorderWidth) + 2 * BorderWidth)
                        Graphics.TranslateTransform(0, TrainLineIndex * TrainLineHeight)

                        'draw background
                        Dim TrainRect As RectangleF = New RectangleF(BorderWidth, BorderWidth, ColumnWidth - 2 * BorderWidth, RowHeight - 2 * BorderWidth)
                        Dim TrainRectColor As Color = GetTrainRectColor(Train, TrainIndex)
                        Dim TrainTextBrush As Brush = GetTrainTextBrush(TrainRectColor, m_ForeColorBrush, m_DarkForeColorBrush)
                        Graphics.FillRectangle(New SolidBrush(TrainRectColor), TrainRect)

                        'draw destination & time
                        Graphics.DrawString(Train.Time, TrainFont, TrainTextBrush, BorderWidth, BorderWidth)
                        Graphics.DrawString(Train.Destination, TrainFont, TrainTextBrush, BorderWidth, TrainTextLineHeight + BorderWidth * 2)

                        'draw track info
                        Dim TrackSize As SizeF = Graphics.MeasureString("999", TrainFont)
                        Dim TrackRect As RectangleF = New RectangleF(TrainRect.Width - BorderWidth - TrackSize.Width, TrainRect.Y + BorderWidth, TrackSize.Width, TrainTextLineHeight)
                        Dim TrackColor As Color = m_ForeColor
                        Dim TrackText As String = Train.Track
                        If Train.TrackChanged Then
                            Graphics.FillRectangle(TrainTextBrush, TrackRect)
                            TrackColor = TrainRectColor
                        End If
                        If TrackText = "" AndAlso Train.Delay = 0 AndAlso Train.Info <> "" Then 'cancelled trains
                            TrackText = "--"
                        End If
                        PrintCentered(TrackText, TrackRect, Graphics, TrainFont, New SolidBrush(TrackColor))

                        Dim TrainType As String = UCase$(Train.Type)
                        'draw train type
                        If Train.IsSBahnTrain Then
                            Dim STrainType As String = Mid(Train.Type, 2)
                            Dim STrainTypeFont As New Font(TrainFont.FontFamily, TrainFont.Size * 3 / 4)
                            Dim TrainTypeSize As SizeF = Graphics.MeasureString("S", TrainFont)
                            Dim STrainTypeSize As SizeF = Graphics.MeasureString("0", STrainTypeFont) ' STrainType, STrainTypeFont)
                            Dim STrainTypeCricle As New RectangleF(TrackRect.X - BorderWidth - STrainTypeSize.Width * 5 / 4, TrackRect.Y + TrainTypeSize.Height * 1 / 4, STrainTypeSize.Width * 2 * 4 / 5, STrainTypeSize.Width * 2 * 4 / 5)
                            Graphics.DrawString("S", TrainFont, TrainTextBrush, TrackRect.X - BorderWidth - TrainTypeSize.Width - STrainTypeSize.Width, TrackRect.Y)
                            'Graphics.DrawString("S", TrainFont, m_ForeColorBrush, TrackRect.X - BorderWidth - TrainTypeSize.Width - STrainTypeCricle.Width, TrackRect.Y)

                            Graphics.FillEllipse(TrainTextBrush, STrainTypeCricle)
                            PrintCentered(STrainType, STrainTypeCricle, Graphics, STrainTypeFont, New SolidBrush(TrainRectColor))
                        ElseIf Train.IsAirportTrain AndAlso (Now.Second Mod 2) = 0 Then
                            Dim TrainTypeSize As SizeF = Graphics.MeasureString("XX", TrainFont)
                            Dim AirPlaneSize As Integer = TrainTypeSize.Width * 4 / 5
                            Dim AirplaneIcon As Bitmap = My.Resources.airplane
                            Dim Pallete As System.Drawing.Imaging.ColorPalette = AirplaneIcon.Palette
                            Pallete.Entries(0) = m_ForeColor
                            Pallete.Entries(1) = Color.FromArgb(0, TrainRectColor.R, TrainRectColor.G, TrainRectColor.B)
                            AirplaneIcon.Palette = Pallete
                            Graphics.DrawImage(AirplaneIcon, New RectangleF(TrackRect.X - BorderWidth - AirPlaneSize, TrackRect.Y, AirPlaneSize, AirPlaneSize))
                        ElseIf TrainType = "TGV" OrElse TrainType = "THA" OrElse TrainType = "ICE" OrElse TrainType = "TGV" OrElse TrainType = "EUR" Then
                            If (Now.Second Mod 2) = 0 OrElse Train.TrainNumber = "" Then
                                'Dim LogoSize As SizeF = Graphics.MeasureString("Ag", TrainFont)
                                ' LogoSize.Height * 4 / 5
                                Dim Logo As Bitmap = My.Resources.ResourceManager.GetObject(TrainType)
                                Dim LogoWidth As Double = TrackRect.Height / Logo.Height * Logo.Width * 7 / 8
                                Dim Pallete As System.Drawing.Imaging.ColorPalette = Logo.Palette

                                Pallete.Entries(0) = CType(TrainTextBrush, SolidBrush).Color
                                Pallete.Entries(1) = Color.FromArgb(0, TrainRectColor.R, TrainRectColor.G, TrainRectColor.B)
                                Logo.Palette = Pallete
                                Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
                                Graphics.DrawImage(Logo, New RectangleF(TrackRect.X - BorderWidth - LogoWidth, TrackRect.Y, LogoWidth, TrackRect.Height))
                                Graphics.InterpolationMode = Drawing2D.InterpolationMode.Default
                                Graphics.DrawImage(Logo, New RectangleF(TrackRect.X - BorderWidth - LogoWidth, TrackRect.Y, LogoWidth, TrackRect.Height))
                            Else
                                Dim TrainTypeSize As SizeF = Graphics.MeasureString(Train.TrainNumber, TrainFont)
                                Graphics.DrawString(Train.TrainNumber, TrainFont, TrainTextBrush, TrackRect.X - BorderWidth - TrainTypeSize.Width, TrackRect.Y)
                            End If
                        Else
                            Dim TrainTypeSize As SizeF = Graphics.MeasureString(Train.Type, TrainFont)
                            Graphics.DrawString(Train.Type, TrainFont, TrainTextBrush, TrackRect.X - BorderWidth - TrainTypeSize.Width, TrackRect.Y)
                        End If

                        'draw delay
                        If Train.Delay > 0 Then
                            Dim DelayRectSize As SizeF = Graphics.MeasureString("+999 > 00:00", TrainFont)
                            Dim DelaySize As SizeF = Graphics.MeasureString("+999 ", TrainFont)
                            Dim GTSize As SizeF = Graphics.MeasureString(">", TrainFont)

                            Dim DelayContainer As Drawing2D.GraphicsContainer = Graphics.BeginContainer()
                            Graphics.TranslateTransform(TrainTimeSize.Width + 2 * BorderWidth, BorderWidth)

                            'draw white rectangle
                            Dim DelayRect As New RectangleF(0, 0, DelayRectSize.Width, TrainTextLineHeight)
                            Graphics.FillRectangle(TrainTextBrush, DelayRect)

                            'draw red rectangle
                            Dim DelayPath As New Drawing2D.GraphicsPath
                            DelayPath.AddLine(New PointF(0, 0), New PointF(DelaySize.Width, 0))
                            DelayPath.AddLine(New PointF(DelaySize.Width, 0), New PointF(DelaySize.Width + GTSize.Width, TrainTextLineHeight / 2))
                            DelayPath.AddLine(New PointF(DelaySize.Width + GTSize.Width, TrainTextLineHeight / 2), New PointF(DelaySize.Width, TrainTextLineHeight))
                            DelayPath.AddLine(New PointF(DelaySize.Width, TrainTextLineHeight), New PointF(0, TrainTextLineHeight))
                            DelayPath.CloseFigure()

                            Graphics.FillPath(New SolidBrush(m_DelayBackColor), DelayPath)

                            'draw delay
                            Graphics.DrawString("+" & Train.Delay & "'", TrainFont, TrainTextBrush, DelayRect.X, 0)

                            Dim NewTime As String = SecToTime(TimeToSec(Train.Time) + Train.Delay * 60)

                            Graphics.DrawString(NewTime, TrainFont, New SolidBrush(TrainRectColor), DelayRect.X + DelaySize.Width + GTSize.Width, DelayRect.Y)

                            Graphics.EndContainer(DelayContainer)
                        ElseIf Train.Info <> "" Then
                            Graphics.FillRectangle(New SolidBrush(m_DelayBackColor), New RectangleF(BorderWidth, BorderWidth * 3 + TrainTextLineHeight * 2, ColumnWidth - 2 * BorderWidth, TrainTextLineHeight + 2 * BorderWidth))
                            Graphics.DrawString(Train.Info, TrainFont, TrainTextBrush, BorderWidth, TrainTextLineHeight * 2 + BorderWidth * 3)

                            TrainLineIndex += 1
                        End If

                        TrainIndex = TrainIndex + 1
                        TrainLineIndex += 2
                        Graphics.EndContainer(TrainContainer)
                    End If
                End If
            Next
            Graphics.EndContainer(Container)
        Next

        TrainData.UnlockTrainData()
        Return True

    End Function

    ''' <summary>
    ''' Returns number of lines we need to print information of traindata
    ''' </summary>
    ''' <param name="TrainData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TrainLinesNeeded(ByVal TrainData As TrainData) As Integer
        If TrainData.Delay = 0 AndAlso TrainData.Info <> "" Then
            Return 3
        End If
        Return 2
    End Function

    Public Overrides Sub SaveSettings(ByVal Settings As Settings)
        Settings.SaveSetting("template_sncb", "use_color", m_UseColors)
        Settings.SaveSetting("template_sncb", "color1", m_Color1.ToArgb())
        Settings.SaveSetting("template_sncb", "color2", m_Color2.ToArgb())
        Settings.SaveSetting("template_sncb", "forecolor", m_ForeColor.ToArgb())
        Settings.SaveSetting("template_sncb", "backcolor", m_BackColor.ToArgb())
        Settings.SaveSetting("template_sncb", "delay_backcolor", m_DelayBackColor.ToArgb())
        Settings.SaveSetting("template_sncb", "columns", m_Columns)

        Settings.SaveSetting("template_sncb", "color_bus", m_ColorBus.ToArgb())
        Settings.SaveSetting("template_sncb", "color_eurostar", m_ColorEurostar.ToArgb())
        Settings.SaveSetting("template_sncb", "color_ice", m_ColorICE.ToArgb())
        Settings.SaveSetting("template_sncb", "color_int", m_ColorINT.ToArgb())
        Settings.SaveSetting("template_sncb", "color_metro", m_ColorMetro.ToArgb())
        Settings.SaveSetting("template_sncb", "color_tgv", m_ColorTGV.ToArgb())
        Settings.SaveSetting("template_sncb", "color_thalys", m_ColorThalys.ToArgb())
        Settings.SaveSetting("template_sncb", "color_tram", m_ColorTram.ToArgb())
    End Sub




End Class

Public Class SNCBClassicTemplate
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
    Protected m_ColorRow1 As Color
    Protected m_ColorRow2 As Color
    Protected m_HeaderColor As Color
    Protected m_HeaderTextColor As Color

    Public Sub New(ByVal Settings As Settings)
        MyBase.New(Settings)
        m_Font = New Font("Arial", 10)
        m_UseColors = Boolean.Parse(Settings.GetSetting("template_sncb_classic", "use_color", "true"))
        m_ColorNormal = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_default", Color.Yellow.ToArgb()))
        m_ColorBus = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_bus", Color.Yellow.ToArgb()))
        m_ColorEurostar = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_eurostar", Color.Green.ToArgb()))
        m_ColorICE = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_ice", Color.White.ToArgb()))
        m_ColorICT = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_ict", Color.DarkGreen.ToArgb()))
        m_ColorINT = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_int", Color.FromArgb(&HFF, &H8F, &H0, &HFF).ToArgb()))
        m_ColorMetro = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_metro", Color.Yellow.ToArgb()))
        m_ColorTGV = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_tgv", Color.Yellow.ToArgb()))
        m_ColorThalys = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_thalys", Color.Red.ToArgb()))
        m_ColorTram = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_tram", Color.Yellow.ToArgb()))
        m_ColorRow1 = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_row_1", Color.Black.ToArgb()))
        m_ColorRow2 = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "color_row_2", Color.FromArgb(255, &H33, &H33, &H33).ToArgb()))
        m_HeaderColor = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "header_color", Color.FromArgb(255, 0, 0, &HAA).ToArgb()))
        m_HeaderTextColor = Color.FromArgb(Settings.GetSetting("template_sncb_classic", "header_text_color", Color.White.ToArgb()))
    End Sub

    Public Overrides Function Render(ByVal DisplayNr As Integer, ByVal Graphics As System.Drawing.Graphics, ByVal WindowSize As SizeF, ByVal TrainData As TrainDataGrabberAPI) As Boolean
        Dim i As Integer
        Dim Tijd As String
        Const HeaderSize As Single = 1 / 10

        TrainData.LockTrainData() 'prevent update of train data while we are painting 
        Graphics.Clear(Color.Black)

        Graphics.FillRectangle(New SolidBrush(m_HeaderColor), New RectangleF(0, 0, WindowSize.Width, WindowSize.Height * HeaderSize))
        'print time
        Tijd = TrainData.Time() 'Format(Now(), "hh:mm")

        Dim TimeSize As New SizeF(WindowSize.Width * 0.05, WindowSize.Height / 3)
        Dim TimeFont As Font = FindFontSize(Graphics, Tijd, TimeSize, m_Font)
        Dim TimeRect As RectangleF = New RectangleF(New PointF(2, 2), Graphics.MeasureString(Tijd, TimeFont))

        Graphics.DrawString(Tijd, TimeFont, New SolidBrush(m_HeaderTextColor), 4, 4)
        Graphics.DrawRectangle(New Pen(m_HeaderTextColor), TimeRect.X, TimeRect.Y, TimeRect.Width, TimeRect.Height)

        'print station name
        Dim StationSize As New SizeF(WindowSize.Width, WindowSize.Height * HeaderSize)
        Dim StationText As String = TrainData.DepartureArrivalText & " " & TrainData.StatioName
        'Dim StationFont As Font = FindFontSize(Graphics, StationText, StationSize, m_Font)

        'PrintCentered(StationText, New RectangleF(0, 0, StationSize.Width, StationSize.Height), Graphics, StationFont, New SolidBrush(Color.White))

        PrintCenteredAutoFontSize(StationText, New RectangleF(0, 0, StationSize.Width, StationSize.Height), Graphics, m_Font, New SolidBrush(m_HeaderTextColor))


        Dim Y As Long, TrainHeight As Integer, TrainsToDisplay As Integer = Math.Min(TrainData.MaxTrains, TrainData.Trains.Count)
        Y = WindowSize.Height * HeaderSize
        TrainHeight = (WindowSize.Height - Y) / TrainData.MaxTrains()

        Dim TrainFont As Font = New Font(m_Font.FontFamily, FindFontSizeHeight(Graphics, "012345679AZERTYUIOPQSDFGHJKLMWXCVBNazertyuiopqsdfghjklmwxcvbsn',?./:;:ùµµ^$g[]}", TrainHeight, m_Font)) 'find the best height that fit's all characters

        For i = 0 To TrainsToDisplay - 1
            If Y + TrainHeight > WindowSize.Height Then Exit For

            Dim RowRect As New RectangleF(0, Y, WindowSize.Width, TrainHeight)
            Dim RowColor As Color = IIf((i Mod 2) = 0, m_ColorRow1, m_ColorRow2)
            Dim TrainBrush As Brush = New SolidBrush(GetTextColor(TrainData.Trains(i)))

            Graphics.FillRectangle(New SolidBrush(RowColor), RowRect)

            'print time
            PrintCentered(TrainData.Trains(i).Time, New RectangleF(0, Y, WindowSize.Width * 0.1, TrainHeight), Graphics, TrainFont, TrainBrush)
            Graphics.DrawString(TrainData.Trains(i).Destination, TrainFont, TrainBrush, WindowSize.Width * 0.1 + 1, Y) ', ScreenSize.Width * 0.7, TrainHeight), Graphics, TrainFont, TrainBrush)
            PrintCentered(TrainData.Trains(i).Type & " " & TrainData.Trains(i).TrainNumber, New RectangleF(WindowSize.Width * 0.7, Y, WindowSize.Width * 0.15, TrainHeight), Graphics, TrainFont, TrainBrush)
            PrintCentered(TrainData.Trains(i).Track, New RectangleF(WindowSize.Width * 0.85, Y, WindowSize.Width * 0.05, TrainHeight), Graphics, TrainFont, TrainBrush)
            If TrainData.Trains(i).Info <> "" Then
                PrintCentered(Replace(TrainData.Trains(i).Info, ":", "H"), New RectangleF(WindowSize.Width * 0.9, Y, WindowSize.Width * 0.1, TrainHeight), Graphics, TrainFont, New SolidBrush(Color.Red))
            End If

            Y = Y + TrainHeight
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
        Settings.SaveSetting("template_sncb_classic", "use_color", m_UseColors)
        Settings.SaveSetting("template_sncb_classic", "color_default", m_ColorNormal.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_bus", m_ColorBus.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_eurostar", m_ColorEurostar.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_ice", m_ColorICE.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_int", m_ColorINT.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_metro", m_ColorMetro.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_tgv", m_ColorTGV.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_thalys", m_ColorThalys.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_tram", m_ColorTram.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_row_1", m_ColorRow1.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "color_row_2", m_ColorRow2.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "header_color", m_HeaderColor.ToArgb())
        Settings.SaveSetting("template_sncb_classic", "header_text_color", m_HeaderTextColor.ToArgb())

    End Sub

    ''' <summary>
    ''' Gets / sets if we should use different colors for special trains like Thalys
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enable / disable use of different colors for special trains like Thalys, Eurostar,...")> _
    Public Property UseColors() As Boolean
        Get
            Return m_UseColors
        End Get
        Set(ByVal value As Boolean)
            m_UseColors = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets the default text color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Default text color.")> _
    Public Property ColorDefault() As Color
        Get
            Return m_ColorNormal
        End Get
        Set(ByVal value As Color)
            m_ColorNormal = value
        End Set
    End Property

    ''' <summary>
    ''' Color for buses
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the color to use for a bus.")> _
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
    <System.ComponentModel.Description("Color for Eurostar trains.")> _
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
    <System.ComponentModel.Description("Color for ICE trains.")> _
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
    <System.ComponentModel.Description("Color for ICT / EXT trains.")> _
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
    <System.ComponentModel.Description("Color for international trains.")> _
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
    <System.ComponentModel.Description("Color for metro.")> _
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
    <System.ComponentModel.Description("Color for TGV trains.")> _
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
    <System.ComponentModel.Description("Color for Thalys trains.")> _
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
    <System.ComponentModel.Description("Color for trams.")> _
    Public Property ColorTram() As Color
        Get
            Return m_ColorTram
        End Get
        Set(ByVal value As Color)
            m_ColorTram = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets the color for even rows
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Background color for even rows.")> _
    Public Property ColorRow1() As Color
        Get
            Return m_ColorRow1
        End Get
        Set(ByVal value As Color)
            m_ColorRow1 = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets the color for odd rows
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Background color for odd rows.")> _
    Public Property ColorRow2() As Color
        Get
            Return m_ColorRow2
        End Get
        Set(ByVal value As Color)
            m_ColorRow2 = value
        End Set
    End Property

    ''' <summary>
    ''' Gets / sets header background color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Header background color.")> _
    Public Property HeaderColor() As Color
        Get
            Return m_HeaderColor
        End Get
        Set(ByVal value As Color)
            m_HeaderColor = value
        End Set
    End Property

    ''' <summary>
    ''' Header text color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Header text color.")> _
    Public Property HeaderTextColor() As Color
        Get
            Return m_HeaderTextColor
        End Get
        Set(ByVal value As Color)
            m_HeaderTextColor = value
        End Set
    End Property

End Class

Public MustInherit Class ScreenSaverTemplate

    Public Sub New(ByVal Settings As Settings)

    End Sub

    ''' <summary>
    ''' Renders train data on a graphics object
    ''' </summary>
    ''' <param name="DisplayNr">display number in case of multiple displays active</param>
    ''' <param name="Graphics"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function Render(ByVal DisplayNr As Integer, ByVal Graphics As Graphics, ByVal WindowSize As SizeF, ByVal TrainData As TrainDataGrabberAPI) As Boolean

    Public MustOverride Sub SaveSettings(ByVal Settings As Settings)



End Class
