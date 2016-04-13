﻿'$GPGSA,A,3,10,25,32,24,31,20,12,,,,,,1.37,0.99,0.95*09
'$GLGSA,A,3,,,,,,,,,,,,,1.37,0.99,0.95*17
Public Class GSA_Info
    Public Selection_Mode As String
    Public FixMode As Byte '1 : No fixed , 2 : 2D fixed , 3 : 3D Fixed.
    Public SA_ID(,) As Byte 'GP , GL ; 1 ~ 12 Satalites ID which be used for fixing position.
    Public PDOP As Decimal = 0D
    Public HDOP As Decimal = 0D
    Public VDOP As Decimal = 0D

    Public Sub setDOP(ByVal PD As Decimal, ByVal HD As Decimal, ByVal VD As Decimal)
        PDOP = PD
        HDOP = HD
        VDOP = VD
    End Sub

    Private Sub EmptyInit()
        Selection_Mode = "unknown"
        FixMode = 1
        PDOP = 0
        HDOP = 0
        VDOP = 0.0
        SA_ID = New Byte(2, 12) {}
    End Sub

    Public Sub New()
        EmptyInit()
    End Sub

    Public Sub New(ByVal GSASENTENSE As String)
        If GSASENTENSE.Contains("$G") And GSASENTENSE.Contains("GSA") Then
            Dim tempArray() As String = Split(GSASENTENSE, ",")
            Selection_Mode = tempArray(1)
            FixMode = tempArray(2)
            SA_ID = New Byte(2, 12) {}
            For x = 0 To (11)
                If GSASENTENSE.Contains("$GP") Then
                    Byte.TryParse(tempArray(3 + x), SA_ID(0, x))
                ElseIf GSASENTENSE.Contains("$GL") Then
                    Byte.TryParse(tempArray(3 + x), SA_ID(1, x))
                End If
            Next
            PDOP = tempArray(15)
            HDOP = tempArray(16)
            VDOP = tempArray(17)
        Else
            EmptyInit()
        End If
    End Sub

    Public Sub SetGLSentense(ByVal GSASENTENSE As String)
        If GSASENTENSE.Contains("$GL") And GSASENTENSE.Contains("GSA") Then
            Dim tempArray() As String = Split(GSASENTENSE, ",")
            'Selection_Mode = tempArray(1)
            'FixMode = tempArray(2)
            'SA_ID = New Byte(2, 12) {}
            For x = 0 To (11)
                If GSASENTENSE.Contains("$GP") Then
                    Byte.TryParse(tempArray(3 + x), SA_ID(0, x))
                ElseIf GSASENTENSE.Contains("$GL") Then
                    Byte.TryParse(tempArray(3 + x), SA_ID(1, x))
                End If
            Next
        End If
    End Sub
End Class