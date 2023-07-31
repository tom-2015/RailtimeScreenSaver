Imports System.Drawing.Drawing2D

Public Class RoundedRectangle

    Protected m_gp As New GraphicsPath


    Public Sub New(ByVal Rectangle As RectangleF, ByVal RoundRadius As Single)

        m_gp.AddArc(Rectangle.X, Rectangle.Y, RoundRadius, RoundRadius, 180, 90)
        m_gp.AddArc(Rectangle.Right - RoundRadius, Rectangle.Y, RoundRadius, RoundRadius, 270, 90)
        m_gp.AddArc(Rectangle.Right - RoundRadius, Rectangle.Bottom - RoundRadius, RoundRadius, RoundRadius, 0, 90)
        m_gp.AddArc(Rectangle.X, Rectangle.Bottom - RoundRadius, RoundRadius, RoundRadius, 90, 90)
        m_gp.CloseFigure()

    End Sub

    Public ReadOnly Property GetGraphicsPath() As GraphicsPath
        Get
            Return m_gp
        End Get
    End Property


End Class
