Option Explicit On

Module Preview

    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    Private Const WS_CHILD As Integer = &H40000000
    Private Const GWL_STYLE As Integer = (-16)
    Private Const GWL_HWNDPARENT As Integer = (-8)
    Private Const HWND_TOP As Integer = 0&
    Private Const SWP_NOZORDER As Integer = &H4
    Private Const SWP_NOACTIVATE As Integer = &H10
    Private Const SWP_SHOWWINDOW As Integer = &H40

    Private Declare Function GetClientRect Lib "user32" (ByVal Hwnd As Integer, ByVal lpRect As RECT) As Integer
    Private Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (ByVal Hwnd As Integer, ByVal nIndex As Integer) As Integer
    Private Declare Sub SetWindowPos Lib "user32" (ByVal Hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer)
    Private Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal Hwnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    Private Declare Function SetParent Lib "user32" (ByVal hWndChild As Integer, ByVal hWndNewParent As Integer) As Integer


    Public Sub DoPreviewMode(ByVal dispHWND As Integer, ByVal PreviewFormHwnd As Integer)
        Dim lngStyle As Long, DispRec As RECT

        GetClientRect(dispHWND, DispRec)
        lngStyle = GetWindowLong(PreviewFormHwnd, GWL_STYLE)
        lngStyle = lngStyle Or WS_CHILD ' Append "WS_CHILD"
        SetWindowLong(PreviewFormHwnd, GWL_STYLE, lngStyle)

        SetParent(PreviewFormHwnd, dispHWND)

        SetWindowLong(PreviewFormHwnd, GWL_HWNDPARENT, dispHWND)

        SetWindowPos(PreviewFormHwnd, HWND_TOP, 0&, 0&, DispRec.Right, DispRec.Bottom, SWP_NOZORDER Or SWP_NOACTIVATE Or SWP_SHOWWINDOW)

    End Sub
End Module
