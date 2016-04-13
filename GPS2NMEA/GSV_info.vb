'$GPGSV,3,1,11,18,88,353,,10,51,324,,12,44,100,,25,41,152,*71
'$GPGSV,3,2,11,24,39,035,,21,29,213,,14,28,301,,20,25,140,*77
'$GPGSV,3,3,11,15,25,075,,32,24,302,,31,12,231,*4F

'$GLGSV,2,1,6,79,83,229,,68,59,056,,78,48,025,,69,34,339,*58
'$GLGSV,2,2,6,80,23,210,,67,22,116,*5E

Public Class GSV_info
    Public TotalNumberOfMessage As Byte
    Public OrderNumber As Byte
    Public GP_TotalNumberInView As Byte
    Public GL_TotalNumberInView As Byte

    Public GP_Satelliate_info() As GSV_Satellite_info
    Public GL_Satelliate_info() As GSV_Satellite_info
    Public GN_Satelliate_info() As GSV_Satellite_info

    Private Sub EmptyInit()
        TotalNumberOfMessage = 0
        OrderNumber = 0
        GP_TotalNumberInView = 0
        GL_TotalNumberInView = 0
        ReDim GP_Satelliate_info(GP_TotalNumberInView)
        ReDim GL_Satelliate_info(GL_TotalNumberInView)
        GP_Satelliate_info(0) = New GSV_Satellite_info()
        GL_Satelliate_info(0) = New GSV_Satellite_info()
    End Sub

    Public Sub New(ByVal GSVSENTENSE As String)
        SetSubPage(GSVSENTENSE)
    End Sub

    Public Function SetSubPage(ByVal GSVSUBSENTENSE As String) As Boolean
        Dim tempArray() As String = Split(GSVSUBSENTENSE, ",")
        Dim OrderNumber = tempArray(2)
        Select Case tempArray(0)
            Case "$GPGSV"
                GP_TotalNumberInView = tempArray(3)
                GP_SatInfoCheck(GP_TotalNumberInView)
                Dim sateliate_number_base As Byte = (OrderNumber - 1) * 4
                Dim sateliate_number_max As Byte = sateliate_number_base + 3
                If sateliate_number_max >= GP_TotalNumberInView Then
                    sateliate_number_max = GP_TotalNumberInView - 1
                End If
                Dim base As Byte = 4
                For x = 0 To (sateliate_number_max Mod 4)
                    Byte.TryParse(tempArray((base * x) + 4), GP_Satelliate_info(sateliate_number_base + x).PRN_num)
                    Integer.TryParse(tempArray((base * x) + 5), GP_Satelliate_info(sateliate_number_base + x).elevation)
                    Integer.TryParse(tempArray((base * x) + 6), GP_Satelliate_info(sateliate_number_base + x).azimuth)
                    Double.TryParse(tempArray((base * x) + 7), GP_Satelliate_info(sateliate_number_base + x).SNR_in_dB)
                Next
                Return True
            Case "$GLGSV"
                GL_TotalNumberInView = tempArray(3)
                GL_SatInfoCheck(GL_TotalNumberInView)
                Dim sateliate_number_base As Byte = (OrderNumber - 1) * 4
                Dim sateliate_number_max As Byte = sateliate_number_base + 3
                If sateliate_number_max >= GL_TotalNumberInView Then
                    sateliate_number_max = GL_TotalNumberInView - 1
                End If
                Dim base As Byte = 4
                For x = 0 To (sateliate_number_max Mod 4)
                    Byte.TryParse(tempArray((base * x) + 4), GL_Satelliate_info(sateliate_number_base + x).PRN_num)
                    Integer.TryParse(tempArray((base * x) + 5), GL_Satelliate_info(sateliate_number_base + x).elevation)
                    Integer.TryParse(tempArray((base * x) + 6), GL_Satelliate_info(sateliate_number_base + x).azimuth)
                    Double.TryParse(tempArray((base * x) + 7), GL_Satelliate_info(sateliate_number_base + x).SNR_in_dB)
                Next
                Return True
            Case "$GNGSV"

        End Select
        Return False
    End Function

    Private Sub GP_SatInfoCheck(ByVal totalinview As Integer)
        If (GP_Satelliate_info Is Nothing) Then
            ReDim GP_Satelliate_info(totalinview)
            For x = 0 To (totalinview - 1)
                GP_Satelliate_info(x) = New GSV_Satellite_info()
            Next
        End If
    End Sub

    Private Sub GL_SatInfoCheck(ByVal totalinview As Integer)
        If (GL_Satelliate_info Is Nothing) Then
            ReDim GL_Satelliate_info(totalinview)
            For x = 0 To (totalinview - 1)
                GL_Satelliate_info(x) = New GSV_Satellite_info()
            Next
        End If
    End Sub

    Public Function getTotalSateInViewNumber() As Byte
        Return GL_TotalNumberInView + GP_TotalNumberInView
    End Function

    Public Function getMaxSNR() As String
        Dim MaxSnr = 0
        For x = 0 To (GP_TotalNumberInView - 1)
            If GP_Satelliate_info(x).SNR_in_dB > MaxSnr Then
                MaxSnr = GP_Satelliate_info(x).SNR_in_dB
            End If
        Next
        For x = 0 To (GL_TotalNumberInView - 1)
            If GL_Satelliate_info(x).SNR_in_dB > MaxSnr Then
                MaxSnr = GL_Satelliate_info(x).SNR_in_dB
            End If
        Next
        Return MaxSnr.ToString()
    End Function

    Public Function getMinSNR() As String
        Dim MinSnr = 40
        For x = 0 To (GP_TotalNumberInView - 1)
            If GP_Satelliate_info(x).SNR_in_dB < MinSnr Then
                MinSnr = GP_Satelliate_info(x).SNR_in_dB
            End If
        Next
        For x = 0 To (GL_TotalNumberInView - 1)
            If GL_Satelliate_info(x).SNR_in_dB < MinSnr Then
                MinSnr = GL_Satelliate_info(x).SNR_in_dB
            End If
        Next
        Return MinSnr.ToString()
    End Function

End Class
