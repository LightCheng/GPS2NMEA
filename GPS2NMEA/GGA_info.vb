'$GNGGA,010352.000,2504.3720,N,12134.5197,E,1,8,0.95,37.4,M,15.2,M,,*7F
Public Class GGA_info
    Private UMC_Time As String
    Private Latitude As Double
    Private N_S As String
    Private Longitude As Double
    Private E_W As String
    Private Quality_Indicator As Byte
    Private Satellite_In_View As Byte
    Private Horzaontal_Dilution As Double
    Private Antenna_Altitude_above_mean_sea_level As Double 'No use at 1st stage
    Private Geoidal_separation As Double 'No use at 1st stage

    Private Sub EmptyInit()
        UMC_Time = "000000.00"
        Latitude = 0.0
        N_S = "unknown"
        Longitude = 0.0
        E_W = "unknown"
        Quality_Indicator = 0
        Satellite_In_View = 0
        Horzaontal_Dilution = 0.0
        Antenna_Altitude_above_mean_sea_level = 0.0
        Geoidal_separation = 0.0
    End Sub

    Public Sub New()
        EmptyInit()
    End Sub

    Public Sub New(ByVal GGASENTENSE As String)
        If GGASENTENSE.Contains("$G") And GGASENTENSE.Contains("GGA") Then
            Dim tempArray() As String = Split(GGASENTENSE, ",")
            UMC_Time = tempArray(1)
            Double.TryParse(tempArray(2), Latitude)
            N_S = tempArray(3)
            Double.TryParse(tempArray(4), Longitude)
            E_W = tempArray(5)
            Byte.TryParse(tempArray(6), Quality_Indicator)
            Byte.TryParse(tempArray(7), Satellite_In_View)
            Double.TryParse(tempArray(8), Horzaontal_Dilution)
            Double.TryParse(tempArray(9), Antenna_Altitude_above_mean_sea_level)
            Double.TryParse(tempArray(11), Geoidal_separation)
        Else
            EmptyInit()
        End If
    End Sub

    Public Function getUtcTime() As String
        Return Mid(UMC_Time, 1, 2) + ":" + Mid(UMC_Time, 3, 2) + ":" + Mid(UMC_Time, 5, 2)
    End Function

    Public Function getLatitude_NS() As String
        Return Latitude.ToString() + " " + N_S
    End Function

    Public Function getLongitide_EW() As String
        Return Longitude.ToString() + " " + E_W
    End Function

End Class
