'$GNRMC,235943.000,V,2504.3218,N,12134.4874,E,0.000,0.00,050180,,,N*5F
Public Class RMC_info
    Public UMC_Time As String
    Public Status As String
    Public Latitude As Double
    Public N_S As String
    Public Longitude As Double
    Public E_W As String
    Public SpeedOverGround As Double

    Private Sub EmptyInit()
        UMC_Time = "000000.00"
        Status = "V"
        Latitude = 0.0
        N_S = "unknown"
        Longitude = 0.0
        E_W = "unknown"
        SpeedOverGround = -1
    End Sub


    Public Sub New()
        EmptyInit()
    End Sub

    Public Sub New(ByVal umc As String, ByVal st As String)
        UMC_Time = umc
        Status = st
    End Sub

    Public Sub New(ByVal RMCSENTENSE As String)
        If RMCSENTENSE.Contains("$G") And RMCSENTENSE.Contains("RMC") Then
            Dim tempArray() As String = Split(RMCSENTENSE, ",")
            UMC_Time = tempArray(1)
            Status = tempArray(2)
            Latitude = tempArray(3)
            N_S = tempArray(4)
            Longitude = tempArray(5)
            E_W = tempArray(6)
            SpeedOverGround = tempArray(7)
        Else
            EmptyInit()
        End If
    End Sub
End Class
