'$GPGSV,3,1,11, [18,88,353,],[10,51,324,],[12,44,100,],[25,41,152,]*71

Public Class GSV_Satellite_info
    Public PRN_num As Byte
    Public elevation As Integer
    Public azimuth As Integer
    Public SNR_in_dB As Double
    Public Sub New()
        PRN_num = 0
        elevation = 0
        azimuth = 0
        SNR_in_dB = 0
    End Sub

    Public Sub New(ByVal prn As Byte, ByVal ele As Integer, ByVal azi As Integer, ByVal snr As Double)
        PRN_num = prn
        elevation = ele
        azimuth = azi
        SNR_in_dB = snr
    End Sub
End Class
