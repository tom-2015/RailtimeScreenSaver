Module Helpers

    ''' <summary>
    ''' Removes all new line characters and does trim
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveNewLines(ByVal Text As String) As String
        Return Trim(Replace(Replace(Replace(Text, vbCrLf, ""), vbLf, ""), vbCr, ""))
    End Function

    Public Function BinVal(ByVal Number As Long) As String
        Dim i As Long
        Dim Res As String = ""

        For i = 0 To 15
            Res = IIf(Number And (2 ^ i), "1", "0") & Res
        Next

        BinVal = Res
    End Function

    ''' <summary>
    ''' Capitalizes a string, turns brussel-midi into Brussel-Midi
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Capitalize(ByVal Text As String) As String
        Dim i As Long
        Dim Chr As String
        Dim PChr As String = ""

        For i = 1 To Len(Text)
            Chr = Mid$(Text, i, 1)
            If Chr >= "A" And Chr <= "Z" Then
                If PChr <> "" And PChr <> " " And PChr <> "-" Then
                    Mid$(Text, i, 1) = LCase$(Chr)
                End If
            Else
                If PChr = "" Or PChr = " " Or PChr = "-" Then
                    Mid$(Text, i, 1) = UCase$(Chr)
                End If
            End If
            PChr = Chr
        Next
        Capitalize = Text
    End Function

    ''' <summary>
    ''' Finds the font that fits text in room
    ''' </summary>
    ''' <param name="g"></param>
    ''' <param name="Text"></param>
    ''' <param name="Room"></param>
    ''' <param name="PreferedFont"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindFontSize(ByVal g As System.Drawing.Graphics, ByVal Text As String, ByVal Room As SizeF, ByVal PreferedFont As Font) As Font
        Dim RealSize As SizeF = g.MeasureString(Text, PreferedFont)
        Dim HeightScaleRatio As Single = Room.Height / RealSize.Height
        Dim WidthScaleRatio As Single = Room.Width / RealSize.Width
        Dim ScaleRatio As Single = IIf((HeightScaleRatio < WidthScaleRatio), HeightScaleRatio, WidthScaleRatio)
        Dim ScaleFontSize As Single = PreferedFont.Size * ScaleRatio
        Return New Font(PreferedFont.FontFamily, ScaleFontSize)
    End Function

    ''' <summary>
    ''' Returns max font height for text
    ''' </summary>
    ''' <param name="g"></param>
    ''' <param name="Text"></param>
    ''' <param name="Height"></param>
    ''' <param name="PreferedFont"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindFontSizeHeight(ByVal g As System.Drawing.Graphics, ByVal Text As String, ByVal Height As Integer, ByVal PreferedFont As Font) As Single
        Dim RealSize As SizeF = g.MeasureString(Text, PreferedFont)
        Return PreferedFont.Size * Height / RealSize.Height
    End Function

    ''' <summary>
    ''' Prints text centered
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <param name="Rect"></param>
    ''' <param name="g"></param>
    ''' <param name="Font"></param>
    ''' <param name="Brush"></param>
    ''' <remarks></remarks>
    Public Sub PrintCentered(ByVal Text As String, ByVal Rect As RectangleF, ByVal g As Graphics, ByVal Font As Font, ByVal Brush As Brush)
        Dim StringSize As SizeF = g.MeasureString(Text, Font)
        g.DrawString(Text, Font, Brush, Rect.X + (Rect.Width - StringSize.Width) / 2, Rect.Y + (Rect.Height - StringSize.Height) / 2)
    End Sub

    ''' <summary>
    ''' Prints text centered automatically adjust the font size to max height
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <param name="Rect"></param>
    ''' <param name="g"></param>
    ''' <param name="Font"></param>
    ''' <param name="Brush"></param>
    ''' <returns>The font used</returns>
    ''' <remarks></remarks>
    Public Function PrintCenteredAutoFontSize(ByVal Text As String, ByVal Rect As RectangleF, ByVal g As Graphics, ByVal Font As Font, ByVal Brush As Brush) As Font
        Dim FontToUse As Font = FindFontSize(g, Text, Rect.Size, Font)
        PrintCentered(Text, Rect, g, FontToUse, Brush)
        Return FontToUse
    End Function

    Public Function TimeToSec(ByVal Time As String) As Integer
        Dim Pos As Integer = InStr(Time, ":")
        If Pos > 0 Then
            Return CInt(Mid(Time, 1, Pos - 1)) * 3600 + CInt(Mid(Time, Pos + 1)) * 60
        End If
    End Function

    Public Function SecToTime(ByVal Seconds As Integer) As String
        Dim h As Integer = (Seconds \ 3600) Mod 24
        Dim m As Integer = (Seconds \ 60) Mod 60
        Return IIf(h < 10, "0", "") & h & ":" & IIf(m < 10, "0", "") & m
    End Function

End Module
