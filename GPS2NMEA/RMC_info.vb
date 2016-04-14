'$GNRMC,235943.000,V,2504.3218,N,12134.4874,E,0.000,0.00,050180,,,N*5F
Public Class RMC_info
    Private UMC_Time As String
    Private Status As String
    Private Latitude As Double
    Private N_S As String
    Private Longitude As Double
    Private E_W As String
    Private SpeedOverGround As Double
    Private Course As Double
    Private Date_ddmmyy As String
    Private Magnetic As Double


    Private Sub EmptyInit()
        UMC_Time = "000000.00"
        Status = "V"
        Latitude = 0.0
        N_S = "unknown"
        Longitude = 0.0
        E_W = "unknown"
        SpeedOverGround = -1
        Course = 0.0
        Date_ddmmyy = "unknown"
        Magnetic = 0.0
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
            Double.TryParse(tempArray(3), Latitude)
            N_S = tempArray(4)
            Double.TryParse(tempArray(5), Longitude)
            E_W = tempArray(6)
            Double.TryParse(tempArray(7), SpeedOverGround)
            Double.TryParse(tempArray(8), Course)
            Date_ddmmyy = tempArray(9)
            Double.TryParse(tempArray(10), Magnetic)
        Else
            EmptyInit()
        End If
    End Sub

    Public Function getDateMMDDYY() As String
        Return Mid(Date_ddmmyy, 3, 2) + "_" + Mid(Date_ddmmyy, 1, 2) + "/20" + Mid(Date_ddmmyy, 5, 2)
    End Function
End Class
