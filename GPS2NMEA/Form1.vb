Imports System.IO
Imports Microsoft.VisualBasic.PowerPacks
Imports System.Drawing.Drawing2D

Public Module GlobalVariables
    Public Structure single_completed_nmea_info
        Public GSA As GSA_Info
        Public GSV As GSV_info
        Public GGA As GGA_info
        Public RMC As RMC_info
        Public Accuracy_MTK As Double
    End Structure

    Public tempDIR As String = Directory.GetCurrentDirectory() + "\GPSTemp"
    Public extracted_nmea_data As String = tempDIR + "\nmea_out.txt"
    Public extracted_mtk_data As String = tempDIR + "\mtk_out.txt"

    Public fileReader As System.IO.StreamReader
    Public stringReader As String

    Public greenBrush As New Drawing.SolidBrush(Color.Green)
    Public yellowBrush As New Drawing.SolidBrush(Color.Yellow)
    Public blueBrush As New Drawing.SolidBrush(Color.Blue)
    Public redBrush As New Drawing.SolidBrush(Color.Red)
    Public blackBrush As New Drawing.SolidBrush(Color.Black)

    Public whitePen As New Pen(Color.FromArgb(255, 200, 200, 200), 2)
    Public blackPen As New Pen(Color.FromArgb(255, 0, 0, 0), 1)
    Public redPen As New Drawing.Pen(Color.FromArgb(255, 255, 0, 0), 1)
    Public FixedModePen As New Pen(Color.FromArgb(255, 255, 255, 0), 3)

    Public fontObj As Font = New System.Drawing.Font("Calibri", 10, FontStyle.Regular)
    Public InfofontObj As Font = New System.Drawing.Font("Calibri", 10, FontStyle.Bold)

    Public TTD_parsed_nmea_array(total_nmea_cnt) As single_completed_nmea_info
    Public total_nmea_cnt As Integer = 0
    Public current_nmea_cnt As Integer = 0
    Public mMaxInViewSatNumber As Integer = 0

End Module

Public Class Main_Form
    Private Sub Main_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        StatusControl1.TabPages.Remove(Others)
        StatusControl1.TabPages.Remove(SatelliteNum)
        MTK_Info_CheckBox.Enabled = False
        MTKTableLayoutPanel.Hide()
        ProgressBar1.Hide()
        Label1.Text = "Open log file from Menu -> File -> Open" 'Directory.GetCurrentDirectory()
        KeyPreview = True
        blackPen.Alignment = PenAlignment.Inset
        redPen.Alignment = PenAlignment.Inset
        'whitePen.DashStyle = DashStyle.DashDot
    End Sub

    Private Function DisplayOpenFileDialog() As Boolean
        Dim openFile As New System.Windows.Forms.OpenFileDialog()
        openFile.DefaultExt = "*.*"
        openFile.Filter = "all files (*.*)|*.*"
        openFile.ShowDialog()
        If openFile.FileNames.Length > 0 Then
            Dim filename As String
            For Each filename In openFile.FileNames
                Label1.Text = filename
            Next
            Return True
        Else
            Return False
        End If
    End Function

    'From "menu/open" , launch a dialog to select log file
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        If DisplayOpenFileDialog() <> True Then
            'User didn't select any file , do nothing.
            Return
        Else
            InitUI(InitProcess())
        End If
    End Sub

    Private Function InitUI(ByVal state As Boolean) As Boolean
        If state = True Then
            If StatusControl1.TabPages.Contains(SatelliteNum) = False Then
                StatusControl1.TabPages.Insert(1, SatelliteNum)
            End If
            If StatusControl1.TabPages.Contains(Others) = False Then
                StatusControl1.TabPages.Insert(2, Others)
            End If
            ProgressBar1.Show()
        Else
            StatusControl1.TabPages.Remove(Others)
            StatusControl1.TabPages.Remove(SatelliteNum)
            SatViewPictureBox.Refresh()
            SNRBARPictureBox.Refresh()
            HScrollBar1.Maximum = 0
            HScrollBar1.Minimum = 0
            HScrollBar1.Value = 0
            Status_Page.Refresh()
        End If
        StatusControl1.SelectTab(0)
        mMaxInViewSatNumber = 0
        Return True
    End Function

    Private Function InitProcess() As Boolean
        total_nmea_cnt = 0
        total_nmea_cnt = extraNmeaAndMTKfromLog(Label1.Text)

        ProgressBar1.Value = 0
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = total_nmea_cnt
        ProgressBar1.Value = 0
        AGPS_TYPE.Text = ""
        MTK_Info_CheckBox.Enabled = False

        If total_nmea_cnt = 0 Then
            Return False
        End If
        ReDim TTD_parsed_nmea_array(total_nmea_cnt)
        initTTD_parsed_nmea_array(extracted_nmea_data)
        Total_data_number.Text = total_nmea_cnt
        ProgressBar1.Hide()
        current_nmea_cnt = 0
        If TTD_parsed_nmea_array.Length = 0 Then 'no valid info , the file may be an invalid file
            HScrollBar1.Maximum = 0
            HScrollBar1.Minimum = 0
            HScrollBar1.Value = 0
            Dim dr As DialogResult = MessageBox.Show("Invalid file , please check again.", "Caption", MessageBoxButtons.OK)
            Status_Page.Refresh()
            Return False
        End If

        'Init scrollbar range.
        HScrollBar1.Maximum = TTD_parsed_nmea_array.Length - 1
        HScrollBar1.Minimum = 0
        HScrollBar1.Value = 0

        UpdateBasicInfoDashboard(0)
        SNRBARPictureBox.Refresh()
        SatViewPictureBox.Refresh()
        Return True
    End Function

    Public Function initTTD_parsed_nmea_array(ByVal filename As String) As Boolean
        fileReader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Dim currentDataIndex As Integer = 0
        Do While fileReader.Peek() > -1
            stringReader = fileReader.ReadLine()
            Dim tempArray() As String = Split(stringReader, ",")
            Select Case tempArray(0)
                Case "$GPGSV", "$GLGSV", "$GNGSV"
                    Dim pageOrderNumber = tempArray(2)
                    If IsNothing(TTD_parsed_nmea_array(currentDataIndex).GSV) = True Then
                        TTD_parsed_nmea_array(currentDataIndex).GSV = New GSV_info(stringReader)
                    Else
                        TTD_parsed_nmea_array(currentDataIndex).GSV.SetSubPage(stringReader)
                    End If
                Case "$GPGGA", "$GLGGA", "$GNGGA"
                    TTD_parsed_nmea_array(currentDataIndex).GGA = New GGA_info(stringReader)
                Case "$GPGSA", "$GLGSA", "$GBGSA"
                    If IsNothing(TTD_parsed_nmea_array(currentDataIndex).GSA) = True Then
                        TTD_parsed_nmea_array(currentDataIndex).GSA = New GSA_Info(stringReader)
                    Else
                        TTD_parsed_nmea_array(currentDataIndex).GSA.SetGXSentense(stringReader)
                    End If
                Case "$GPRMC", "$GLRMC", "$GNRMC"
                    TTD_parsed_nmea_array(currentDataIndex).RMC = New RMC_info(stringReader)
                    mMaxInViewSatNumber = Math.Max(mMaxInViewSatNumber, TTD_parsed_nmea_array(currentDataIndex).GSV.getTotalSateInViewNumber())
                    currentDataIndex += 1
                Case "$GPACCURACY", "$GNACCURACY"
                    If currentDataIndex = 0 Then
                        TTD_parsed_nmea_array(currentDataIndex).Accuracy_MTK = tempArray(1)
                    Else
                        TTD_parsed_nmea_array(currentDataIndex - 1).Accuracy_MTK = tempArray(1)
                    End If
            End Select
        Loop
        fileReader.Close()
        Return False
    End Function


    Private Function extraNmeaAndMTKfromLog(ByVal filename As String) As Integer
        If Directory.Exists(tempDIR) = False Then
            Directory.CreateDirectory(tempDIR)
        End If
        fileReader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Dim nmea_writer = My.Computer.FileSystem.OpenTextFileWriter(extracted_nmea_data, False)
        Dim mtk_writer = My.Computer.FileSystem.OpenTextFileWriter(extracted_mtk_data, False)
        Dim validDataCount As Integer = 0
        Do While fileReader.Peek() >= 0
            stringReader = fileReader.ReadLine()
            Dim TestPos As Integer = InStr(stringReader, "$G")
            If TestPos > 1 Then  'For some log not start by "$G" , such as MTK main log.
                Dim tempString As String = stringReader.Substring(TestPos - 1)
                stringReader = tempString
            End If

            If IsNmeaNeededData(stringReader) = True Then
                Dim Pos As Integer = InStr(stringReader, "*")
                If Pos > 0 Then
                    nmea_writer.WriteLine(Strings.Left(stringReader, (Pos - 1)))
                Else
                    nmea_writer.WriteLine(stringReader)
                End If
                If stringReader.Contains("RMC") Then
                    validDataCount += 1
                End If
            End If

            'Add RMC as time token for MTK info
            If IsMtkNeededData(stringReader) = True Then
                mtk_writer.WriteLine(stringReader)
                'Check PMTK013/ClkType for GPS clock source info , 254:co-clock , 255:TCXO
                If stringReader.Contains("ClkType") Then
                    Dim i As Integer = stringReader.IndexOf("ClkType")
                    If Mid(stringReader, i + 9, 3) = "254" Then
                        CLK_TYPE.Text = "co-Clock"
                    ElseIf Mid(stringReader, i + 9, 3) = "255" Then
                        CLK_TYPE.Text = "TCXO"
                    Else
                        CLK_TYPE.Text = ""
                    End If
                ElseIf stringReader.Contains("$PMTKEPH") Then
                    Dim tempArray() As String = Split(stringReader, ",")
                End If
            End If

            If stringReader.Contains("wkssi") Then
                AGPS_TYPE.Text = AGPS_TYPE.Text + "[AGPS]"
            ElseIf stringReader.Contains("wkbee") Then
                AGPS_TYPE.Text = AGPS_TYPE.Text + "[Hotstill]"
            ElseIf stringReader.Contains("wk,epo") Then
                AGPS_TYPE.Text = AGPS_TYPE.Text + "[EPO]"
            End If
        Loop

        If CLK_TYPE.Text.Length = 0 Then
            MTK_Info_CheckBox.Enabled = False
        Else
            MTK_Info_CheckBox.Enabled = True
            If MTK_Info_CheckBox.Checked = False Then
                MTKTableLayoutPanel.Hide()
            End If
        End If

        nmea_writer.Close()
        mtk_writer.Close()
        fileReader.Close()
        Return validDataCount
    End Function

    Private Function IsNmeaNeededData(ByVal sentence As String) As Boolean
        Dim NeededArray(,) As String = {
            {"$GP", "Global Positioning System receiver"},
            {"$GA", "Galileo Positioning System"},
            {"$GL", "GLONASS, according to IEIC 61162-1"},
            {"$GN", "Mixed GPS and GLONASS data, according to IEIC 61162-1"},
            {"$GB", "BeiDou (China)"},
            {"$BD", "BeiDou (China)"},
            {"$QZ", "QZSS regional GPS augmentation system (Japan)"}}
        Dim size As Integer = NeededArray.Length
        For x = 0 To ((size / 2) - 1)
            If sentence.Contains(NeededArray(x, 0)) = True Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function IsMtkNeededData(ByVal sentence As String) As Boolean
        Dim NeededArray(,) As String = {
            {"PMTK", "MTK GPS info"},
            {"RMC", ""},
            {"ClkType", "254:co-clock , 255:TCXO"},
            {"$PMTKEPH", "MTK only , 判斷是即時接收解算下来的Almanac、或是透過EPP or HotStill"}}
        Dim size As Integer = NeededArray.Length
        For x = 0 To ((size / 2) - 1)
            If sentence.Contains(NeededArray(x, 0)) = True Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function UpdateBasicInfoDashboard(ByVal currentValue As Integer) As Boolean

        UTC_data.Text = TTD_parsed_nmea_array(currentValue).GGA.getUtcTime() + " On " + TTD_parsed_nmea_array(currentValue).RMC.getDateMMDDYY()

        Latitude_data.Text = TTD_parsed_nmea_array(currentValue).GGA.getLatitude_NS()
        Longitude_data.Text = TTD_parsed_nmea_array(currentValue).GGA.getLongitide_EW()

        Select Case TTD_parsed_nmea_array(currentValue).GSA.FixMode
            Case 1
                Fixed_T.Text = "No Fix"
            Case 2
                Fixed_T.Text = "2D Fix"
            Case 3
                Fixed_T.Text = "3D Fix"
        End Select

        ACLabel.Text = TTD_parsed_nmea_array(currentValue).Accuracy_MTK.ToString + " (m)"
        Return True
    End Function

    'Process scrollbar event and update "Status_Page"
    Private Sub HScrollBar1_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar1.Scroll
        If total_nmea_cnt = 0 Then
            Return
        End If
        If e.NewValue <= 0 Then
            HScrollBar1.Value = 0
        End If
        Dim canvas As New ShapeContainer
        Dim theLine As New LineShape
        Dim temp As String = HScrollBar1.ToString()
        current_nmea_cnt = HScrollBar1.Value
        Total_data_number.Text = HScrollBar1.Value
        'By the new index , update UTC/Lat/Longitude data from array "parsed_nmea_array"
        UpdateBasicInfoDashboard(HScrollBar1.Value)

        If PDOPCB.Checked() = True Then
            P_Value.Text = TTD_parsed_nmea_array(HScrollBar1.Value).GSA.PDOP
        End If
        If HDOPCB.Checked() = True Then
            H_Value.Text = TTD_parsed_nmea_array(HScrollBar1.Value).GSA.HDOP
        End If
        If VDOPCB.Checked() = True Then
            V_Value.Text = TTD_parsed_nmea_array(HScrollBar1.Value).GSA.VDOP
        End If

        If SNRCheckBox.Checked() = True Then
            Dim info As String = "Max : " + TTD_parsed_nmea_array(HScrollBar1.Value).GSV.getMaxSNR() + ";" + "Min : " + TTD_parsed_nmea_array(HScrollBar1.Value).GSV.getMinSNR()
            Dim g As Graphics = InfoPictureBox.CreateGraphics()
            g.Clear(Color.LightGray)
            g.DrawString(info, InfofontObj, Brushes.Black, 2, 5)
        End If

        Select Case TTD_parsed_nmea_array(HScrollBar1.Value).GSA.FixMode
            Case 1
                Fixed_T.Text = "No Fix"
            Case 2
                Fixed_T.Text = "2D Fix"
            Case 3
                Fixed_T.Text = "3D Fix"
        End Select

        ACLabel.Text = TTD_parsed_nmea_array(current_nmea_cnt).Accuracy_MTK.ToString() + " (m)"

        If StatusControl1.SelectedTab.Name = Status_Page.Name Then
            SatViewPictureBox.Refresh()
            SNRBARPictureBox.Refresh()
        ElseIf StatusControl1.SelectedTab.Name = Others.Name Then
            MaxMinSNR_PB.Refresh()
        ElseIf StatusControl1.SelectedTab.Name = SatelliteNum.Name Then
            Satellite_PB.Refresh()
        End If
    End Sub

    Private Sub SatViewPictureBox_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles SatViewPictureBox.Paint
        Dim Display_width = SatViewPictureBox.Width
        Dim Display_height = SatViewPictureBox.Height

        'Draw triangle for BD ?
        'Dim poly(3) As PointF ', NumCoords As Long, hBrush As Long, hRgn As Long
        'poly(1).X = 0
        'poly(1).Y = 0
        'poly(2).X = 0
        'poly(2).Y = SatViewPictureBox.Height / 5
        'poly(3).X = SatViewPictureBox.Width / 5
        'poly(3).Y = SatViewPictureBox.Height / 5
        'e.Graphics.DrawPolygon(Pens.AliceBlue, poly)

        Dim centerX As Double = Math.Floor(Display_width / 2)
        Dim centerY As Double = Math.Floor(Display_height / 2)

        Dim radius As Integer = 0
        Dim DRAW_MARGIN As Integer = 100

        If centerX > centerY Then
            radius = (Display_height) - DRAW_MARGIN
        Else
            radius = (Display_width) - DRAW_MARGIN
        End If

        Dim offset_X As Double = 0.25 * radius
        Dim offset_Y As Double = 0.5 * radius
        Dim view_start_x As Integer = DRAW_MARGIN / 2
        Dim view_start_y As Integer = DRAW_MARGIN / 2
        'Start to draw the background of 衛星分佈圖
        e.Graphics.FillEllipse(blueBrush, view_start_x, view_start_y, radius, radius)
        e.Graphics.DrawEllipse(whitePen, view_start_x, view_start_y, radius, radius)
        e.Graphics.DrawEllipse(whitePen, CInt(view_start_x + (0.25 * radius / 2)), CInt((view_start_y + (0.25 * radius / 2))), CInt(radius * 0.75), CInt(radius * 0.75))
        e.Graphics.DrawEllipse(whitePen, CInt(view_start_x + (0.25 * radius)), CInt(view_start_y + (0.25 * radius)), CInt(radius * 0.5), CInt(radius * 0.5))
        e.Graphics.DrawEllipse(whitePen, CInt(view_start_x + (0.75 * radius / 2)), CInt(view_start_y + (0.75 * radius / 2)), CInt(radius * 0.25), CInt(radius * 0.25))

        e.Graphics.DrawLine(whitePen, CInt(view_start_x), CInt(view_start_y + (radius / 2)), CInt(view_start_x + (0.75 * radius / 2)), CInt(view_start_y + (radius / 2)))
        e.Graphics.DrawLine(whitePen, CInt(view_start_x + (0.75 * radius / 2) + (radius * 0.25)), CInt(view_start_y + (radius / 2)), CInt(view_start_x + radius), CInt(view_start_y + (radius / 2)))

        e.Graphics.DrawLine(whitePen, CInt(view_start_x + (radius / 2)), view_start_y, CInt(view_start_x + (radius / 2)), CInt(view_start_y + (0.75 * radius / 2)))
        e.Graphics.DrawLine(whitePen, CInt(view_start_x + (radius / 2)), CInt(view_start_y + (0.75 * radius / 2 + (radius * 0.25))), CInt(view_start_x + (radius / 2)), CInt(view_start_y + radius))
        'Draw background of 衛星分佈圖 done.

        Dim RIGHT_ANGLE As Double = 90.0
        Dim STRAIGHT_ANGLE_D As Double = 180.0

        Dim view_scale As Double = radius / RIGHT_ANGLE
        Dim mSNR As Integer = 0
        Dim mbeUsed As Boolean = False
        Dim mSataID As Integer = 0
        Dim mStrAzimuth As String = ""
        Dim mStrElevation As String = ""

        Dim mSatallite_size As Integer = radius / 16
        Dim mSatallite_size_factor As Double = 0.48
        Dim mfont_offset_x As Integer = 1
        Dim mfont_offset_y As Integer = 2
        Dim Satfontsize As Integer = CInt(mSatallite_size / 2)
        Dim SatfontObj As Font = New System.Drawing.Font("Times", Satfontsize, FontStyle.Regular)
        e.Graphics.DrawString("W[270]", SatfontObj, Brushes.Black, CInt(view_start_x) - (Satfontsize * 4 + 3), CInt(view_start_y + (radius / 2)) - 8)
        e.Graphics.DrawString("E[90]", SatfontObj, Brushes.Black, CInt(view_start_x + radius) + 3, CInt(view_start_y + (radius / 2)) - 8)
        e.Graphics.DrawString("N[0]", SatfontObj, Brushes.Black, CInt(view_start_x + (radius / 2)) - 8, view_start_y - 18)
        e.Graphics.DrawString("S[180]", SatfontObj, Brushes.Black, CInt(view_start_x + (radius / 2)) - 15, CInt(view_start_y + radius) + 2)

        If total_nmea_cnt <= 0 Then
            Return
        End If

        If TTD_parsed_nmea_array(current_nmea_cnt).GSV.getTotalSateInViewNumber() <= 0 Then
            Return
        End If

        If TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGPSateInViewNumber() > 0 Then
            For x = 0 To (TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGPSateInViewNumber() - 1)
                mSNR = GetSNRByIndex(x, mbeUsed, mSataID, mStrAzimuth, mStrElevation, 0)
                If IsNothing(mStrAzimuth) <> True And CInt(mStrAzimuth) > 0 Then
                    Dim theta As Double = -(mStrAzimuth - RIGHT_ANGLE) 'theta : 0 ~ 359
                    Dim rad As Double = theta * Math.PI / STRAIGHT_ANGLE_D ' rad : 0 ~ 90
                    Dim a As Double = (RIGHT_ANGLE - mStrElevation) * view_scale * mSatallite_size_factor

                    Dim t_centerX As Integer = centerX
                    Dim t_centerY As Integer = centerY

                    Dim px As Double = Math.Round(t_centerX + Math.Cos(rad) * a) - 18
                    Dim t As Double = -Math.Sin(rad) * a
                    Dim py As Double = Math.Round(t_centerY + t) - 18

                    If mbeUsed = True Then
                        e.Graphics.FillEllipse(greenBrush, CInt(px), CInt(py), mSatallite_size, mSatallite_size)
                        e.Graphics.DrawEllipse(FixedModePen, CInt(px), CInt(py), mSatallite_size + 2, mSatallite_size + 2)
                    ElseIf mSNR > 0 Then
                        e.Graphics.FillEllipse(greenBrush, CInt(px), CInt(py), mSatallite_size, mSatallite_size)
                    Else
                        e.Graphics.FillEllipse(redBrush, CInt(px), CInt(py), mSatallite_size, mSatallite_size)
                    End If

                    If mSataID.ToString().Length > 2 Then
                        e.Graphics.DrawString(mSataID, SatfontObj, Brushes.Black, CInt(px), CInt(py + mfont_offset_y))
                    Else
                        e.Graphics.DrawString(mSataID, SatfontObj, Brushes.Black, CInt(px + mfont_offset_x), CInt(py + mfont_offset_y))
                    End If
                End If
            Next
        End If

        If TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGLSateInViewNumber() > 0 Then
            For x = 0 To (TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGLSateInViewNumber() - 1)
                mSNR = GetSNRByIndex(x, mbeUsed, mSataID, mStrAzimuth, mStrElevation, 1)
                If IsNothing(mStrAzimuth) <> True And CInt(mStrAzimuth) > 0 Then
                    Dim theta As Double = -(mStrAzimuth - RIGHT_ANGLE) 'theta : 0 ~ 359
                    Dim rad As Double = theta * Math.PI / STRAIGHT_ANGLE_D ' rad : 0 ~ 90
                    Dim a As Double = (RIGHT_ANGLE - mStrElevation) * view_scale * mSatallite_size_factor

                    Dim t_centerX As Integer = centerX
                    Dim t_centerY As Integer = centerY

                    Dim px As Double = Math.Round(t_centerX + Math.Cos(rad) * a) - 18
                    Dim t As Double = -Math.Sin(rad) * a
                    Dim py As Double = Math.Round(t_centerY + t) - 18

                    If mbeUsed = True Then
                        e.Graphics.FillRectangle(greenBrush, CInt(px), CInt(py), mSatallite_size, mSatallite_size)
                        e.Graphics.DrawRectangle(FixedModePen, CInt(px - 1), CInt(py - 1), mSatallite_size + 2, mSatallite_size + 2)
                    ElseIf mSNR > 0 Then
                        e.Graphics.FillRectangle(greenBrush, CInt(px), CInt(py), mSatallite_size, mSatallite_size)
                    Else
                        e.Graphics.FillRectangle(redBrush, CInt(px), CInt(py), mSatallite_size, mSatallite_size)
                    End If

                    If mSataID.ToString().Length > 2 Then
                        e.Graphics.DrawString(mSataID, SatfontObj, Brushes.Black, CInt(px), CInt(py + mfont_offset_y))
                    Else
                        e.Graphics.DrawString(mSataID, SatfontObj, Brushes.Black, CInt(px + mfont_offset_x), CInt(py + mfont_offset_y))
                    End If

                End If
            Next
        End If

    End Sub

    Private Sub SNRBARPictureBox_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles SNRBARPictureBox.Paint
        Dim devide As Integer = 15
        Dim TotalSateNumber As Byte = 0
        If current_nmea_cnt > 0 Then
            TotalSateNumber = TTD_parsed_nmea_array(current_nmea_cnt).GSV.getTotalSateInViewNumber()
        End If
        If TotalSateNumber > 32 Then
            devide = TotalSateNumber
        ElseIf TotalSateNumber > 30 Then
            devide = 32
        ElseIf TotalSateNumber > 25 Then
            devide = 30
        ElseIf TotalSateNumber > 20 Then
            devide = 25
        ElseIf TotalSateNumber > 15 Then
            devide = 20
        End If

        Dim Display_Width As Integer = SNRBARPictureBox.Width - 40

        Dim slotWidth As Double = Math.Floor(Display_Width / devide)
        Dim barWidth As Double = slotWidth / 100.0F * 75.0F
        Dim fill As Double = slotWidth - barWidth
        Dim margin As Double = (Display_Width - slotWidth * devide)
        Dim rowHeight As Double = Math.Floor(SNRBARPictureBox.Height / 4.0F)
        Dim maxHeight As Double = rowHeight * 4
        Dim scale As Double = maxHeight / 100.0F
        Dim baseline As Double = SNRBARPictureBox.Height - (rowHeight / 2)

        Dim UsedbarBrush As New SolidBrush(Color.LightGreen)
        Dim unUsedbarBrush As New SolidBrush(Color.LightYellow)
        Dim drawn As Integer = 0

        e.Graphics.DrawLine(Pens.Black, CInt(margin), 0, CInt(margin), CInt(baseline))
        e.Graphics.DrawLine(Pens.Black, CInt(margin), CInt(baseline), SNRBARPictureBox.Width - CInt(margin), CInt(baseline))

        e.Graphics.DrawLine(Pens.Red, CInt(margin), CInt(baseline - (10 * scale)), SNRBARPictureBox.Width - CInt(margin), CInt(baseline - (10 * scale)))
        e.Graphics.DrawString("10dB", fontObj, Brushes.Black, SNRBARPictureBox.Width - CInt(margin) - 30, CInt(baseline - (10 * scale) - 12))
        e.Graphics.DrawLine(Pens.Yellow, CInt(margin), CInt(baseline - (20 * scale)), SNRBARPictureBox.Width - CInt(margin), CInt(baseline - (20 * scale)))
        e.Graphics.DrawString("20dB", fontObj, Brushes.Black, SNRBARPictureBox.Width - CInt(margin) - 30, CInt(baseline - (20 * scale) - 12))
        e.Graphics.DrawLine(Pens.Blue, CInt(margin), CInt(baseline - (30 * scale)), SNRBARPictureBox.Width - CInt(margin), CInt(baseline - (30 * scale)))
        e.Graphics.DrawString("30dB", fontObj, Brushes.Black, SNRBARPictureBox.Width - CInt(margin) - 30, CInt(baseline - (30 * scale) - 12))
        e.Graphics.DrawLine(Pens.Green, CInt(margin), CInt(baseline - (40 * scale)), SNRBARPictureBox.Width - CInt(margin), CInt(baseline - (40 * scale)))
        e.Graphics.DrawString("40dB", fontObj, Brushes.Black, SNRBARPictureBox.Width - CInt(margin) - 30, CInt(baseline - (40 * scale) - 12))

        If total_nmea_cnt <= 0 Then
            Return
        End If

        Dim strSateInView As String = "Satellite in View : " + TTD_parsed_nmea_array(current_nmea_cnt).GSV.getTotalSateInViewNumber().ToString
        e.Graphics.DrawString(strSateInView, fontObj, Brushes.Black, CInt(margin), SNRBARPictureBox.Height / 5)
        strSateInView = "Satellite Used : " + TTD_parsed_nmea_array(current_nmea_cnt).GSA.getUsedforFixNumber().ToString()
        e.Graphics.DrawString(strSateInView, fontObj, Brushes.Black, CInt(margin), SNRBARPictureBox.Height / 4)

        If TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGPSateInViewNumber() > 0 Then
            For x = 0 To (TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGPSateInViewNumber() - 1)
                Dim mSNR As Integer = 0
                Dim mbeUsed As Boolean = False
                Dim mSataID As Integer = 0
                Dim mStrAzimuth As String = ""
                Dim mStrElevation As String = ""
                mSNR = GetSNRByIndex(x, mbeUsed, mSataID, mStrAzimuth, mStrElevation, 0)
                If IsNothing(mStrAzimuth) <> True And CInt(mStrAzimuth) > 0 Then
                    'Signal strength bar
                    Dim left As Double = margin + (drawn * slotWidth) + fill
                    Dim top As Double = 0
                    Dim right As Double = left + barWidth
                    Dim center As Double = left + barWidth / 2
                    Dim tleft As Integer = left

                    top = baseline - (mSNR * scale)
                    If mbeUsed = True Then 'used for fixed , green
                        e.Graphics.FillRectangle(UsedbarBrush, CInt(left), CInt(top), CInt(right) - CInt(left), CInt(baseline - top))
                    ElseIf CInt(mStrAzimuth) > 0 And CInt(mStrElevation) > 0 Then 'SNR + valid Azimuth & Elevation but used for fixed , yellow
                        e.Graphics.FillRectangle(unUsedbarBrush, CInt(left), CInt(top), CInt(right) - CInt(left), CInt(baseline - top))
                    End If
                    e.Graphics.DrawRectangle(blackPen, CInt(left), CInt(top), CInt(right) - CInt(left), CInt(baseline - top))
                    e.Graphics.DrawString(mSataID, fontObj, Brushes.Black, left + barWidth / 8, baseline + 5)
                    If x = 1 Then
                        e.Graphics.DrawString("GPS", fontObj, Brushes.Black, left + barWidth / 8, baseline + 25)
                    End If
                    e.Graphics.DrawString(mSNR, fontObj, Brushes.Black, left + barWidth / 8, top - 15)
                    drawn += 1
                End If
            Next
        End If

        If TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGPSateInViewNumber() > 0 Then
            For x = 0 To (TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGLSateInViewNumber() - 1)
                'Signal strength bar
                Dim left As Double = margin + (drawn * slotWidth) + fill
                Dim top As Double = 0
                Dim right As Double = left + barWidth
                Dim center As Double = left + barWidth / 2
                Dim tleft As Integer = left

                Dim mSNR As Integer = 0
                Dim mbeUsed As Boolean = False
                Dim mSataID As Integer = 0
                Dim mStrAzimuth As String = ""
                Dim mStrElevation As String = ""

                mSNR = GetSNRByIndex(x, mbeUsed, mSataID, mStrAzimuth, mStrElevation, 1)
                If IsNothing(mStrAzimuth) <> True And CInt(mStrAzimuth) > 0 Then
                    top = baseline - (mSNR * scale)
                    If mbeUsed = True Then 'used for fixed , green
                        e.Graphics.FillRectangle(UsedbarBrush, CInt(left), CInt(top), CInt(right) - CInt(left), CInt(baseline - top))
                    ElseIf CInt(mStrAzimuth) > 0 And CInt(mStrElevation) > 0 Then 'SNR + valid Azimuth & Elevation but used for fixed , yellow
                        e.Graphics.FillRectangle(unUsedbarBrush, CInt(left), CInt(top), CInt(right) - CInt(left), CInt(baseline - top))
                    End If
                    e.Graphics.DrawRectangle(blackPen, CInt(left), CInt(top), CInt(right) - CInt(left), CInt(baseline - top))

                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, CInt(left + barWidth / 8), CInt(baseline + 5), 20, 20)
                    e.Graphics.DrawString(mSataID, fontObj, Brushes.Black, left + barWidth / 8, baseline + 5)
                    If x = 1 Then
                        e.Graphics.DrawString("GLONASS", fontObj, Brushes.Black, left + barWidth / 8, baseline + 25)
                    End If
                    e.Graphics.DrawString(mSNR, fontObj, Brushes.Black, left + barWidth / 8, top - 15)
                    drawn += 1
                End If
            Next
        End If
    End Sub

    Private Sub Satellite_PB_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Satellite_PB.Paint
        Dim h_margin As Integer = 12
        If mMaxInViewSatNumber > 12 Then
            h_margin = mMaxInViewSatNumber
        End If

        Dim interval_width As Double = Satellite_PB.Width / total_nmea_cnt
        Dim start_offset_x As Integer = 20

        Dim temp_h As Integer = Satellite_PB.Height / h_margin
        Dim Y_offset = 0

        e.Graphics.DrawLine(redPen, CInt(current_nmea_cnt * interval_width) + start_offset_x, 1, CInt(current_nmea_cnt * interval_width) + start_offset_x, Satellite_PB.Height)

        e.Graphics.DrawLine(Pens.Black, start_offset_x, 0, start_offset_x, Satellite_PB.Height)
        e.Graphics.DrawLine(Pens.Black, start_offset_x, Satellite_PB.Height - 1, Satellite_PB.Width, Satellite_PB.Height - 1)
        For x = 1 To h_margin
            Dim point1 As New Point(start_offset_x, CInt(Satellite_PB.Height - (temp_h * x) + Y_offset) - 3)
            Dim point2 As New Point(Satellite_PB.Width - start_offset_x, CInt(Satellite_PB.Height - (temp_h * x) + Y_offset) - 3)

            e.Graphics.DrawString(x, fontObj, Brushes.Black, 0, CInt(Satellite_PB.Height - (temp_h * x) + Y_offset - 10))
            e.Graphics.DrawLine(Pens.WhiteSmoke, point1, point2)
        Next

        For x = 0 To (total_nmea_cnt - 1)
            If (TTD_parsed_nmea_array(x).GSV.getTotalSateInViewNumber()) > 0 Then
                Dim sat_num_height As Integer = CInt(Satellite_PB.Height - (temp_h * TTD_parsed_nmea_array(x).GSV.getTotalSateInViewNumber()) + Y_offset) - 3
                e.Graphics.FillEllipse(Brushes.Red, CInt(start_offset_x + (x * interval_width)) + 2, sat_num_height - 1, 2, 3)

                If (TTD_parsed_nmea_array(x).GSA.getUsedforFixNumber()) > 0 Then
                    sat_num_height = CInt(Satellite_PB.Height - (temp_h * TTD_parsed_nmea_array(x).GSA.getUsedforFixNumber()) + Y_offset) - 3
                    e.Graphics.FillEllipse(Brushes.Blue, CInt(start_offset_x + (x * interval_width)) + 2, sat_num_height - 1, 2, 3)
                End If
            End If
        Next
    End Sub

    Private Sub MaxMinSNR_PB_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MaxMinSNR_PB.Paint
        If total_nmea_cnt < 1 Then
            Return
        End If
        Dim point1 As Point
        Dim point2 As Point
        Dim start_offset_x As Integer = 0
        Dim interval_width As Double = MaxMinSNR_PB.Width / total_nmea_cnt

        e.Graphics.DrawLine(redPen, CInt(current_nmea_cnt * interval_width), CInt(MaxMinSNR_PB.Height / 10), CInt(current_nmea_cnt * interval_width), MaxMinSNR_PB.Height)
        e.Graphics.DrawLine(Pens.Black, 0, 0, 0, MaxMinSNR_PB.Height)
        e.Graphics.DrawLine(Pens.Black, 0, MaxMinSNR_PB.Height - 1, MaxMinSNR_PB.Width, MaxMinSNR_PB.Height - 1)
        Dim FixedfontObj As Font = New System.Drawing.Font("Calibri", 10, FontStyle.Regular)

        e.Graphics.DrawString("2D", FixedfontObj, Brushes.Black, start_offset_x, CInt(MaxMinSNR_PB.Height / 10) + 60)
        e.Graphics.DrawString("3D", FixedfontObj, Brushes.Black, start_offset_x, CInt(MaxMinSNR_PB.Height / 10) + 10)
        point1 = New Point(start_offset_x, CInt(MaxMinSNR_PB.Height / 10) + 80)
        point2 = New Point(start_offset_x + (MaxMinSNR_PB.Width), CInt(MaxMinSNR_PB.Height / 10) + 80)
        e.Graphics.DrawLine(Pens.WhiteSmoke, point1, point2)

        point1 = New Point(start_offset_x, CInt(MaxMinSNR_PB.Height / 10) + 30)
        point2 = New Point(start_offset_x + (MaxMinSNR_PB.Width), CInt(MaxMinSNR_PB.Height / 10) + 30)
        e.Graphics.DrawLine(Pens.WhiteSmoke, point1, point2)
        Dim mOffset As Integer = (MaxMinSNR_PB.Height / 200) 'half part with 50 dB
        If SNRCheckBox.Checked() = True Then
            point1 = New Point(start_offset_x, MaxMinSNR_PB.Height - (40 * mOffset))
            point2 = New Point(start_offset_x + (MaxMinSNR_PB.Width), MaxMinSNR_PB.Height - (40 * mOffset))
            e.Graphics.DrawLine(Pens.WhiteSmoke, point1, point2)
            e.Graphics.DrawString("40dB", FixedfontObj, Brushes.Black, start_offset_x, MaxMinSNR_PB.Height - (40 * mOffset) - 20)

            point1 = New Point(start_offset_x, MaxMinSNR_PB.Height - (20 * mOffset))
            point2 = New Point(start_offset_x + (MaxMinSNR_PB.Width), MaxMinSNR_PB.Height - (20 * mOffset))
            e.Graphics.DrawLine(Pens.Yellow, point1, point2)
            e.Graphics.DrawString("20dB", FixedfontObj, Brushes.Black, start_offset_x, MaxMinSNR_PB.Height - (20 * mOffset) - 20)
        End If
        For x = 0 To (total_nmea_cnt - 1)
            Select Case TTD_parsed_nmea_array(x).GSA.FixMode
                Case 1
                    'e.Graphics.FillEllipse(greenBrush, CInt(start_offset_x + (x * interval_width)), CInt(Label_NoFix.Location.Y + 2), 3, 5)
                Case 2
                    e.Graphics.DrawRectangle(Pens.Green, CInt(start_offset_x + (x * interval_width)), CInt(MaxMinSNR_PB.Height / 10) + 80, 1, 1)
                Case 3
                    e.Graphics.DrawRectangle(Pens.Green, CInt(start_offset_x + (x * interval_width)), CInt(MaxMinSNR_PB.Height / 10) + 30, 1, 1)
            End Select

            If SNRCheckBox.Checked() = True Then
                Dim mMaxSNR As Integer = TTD_parsed_nmea_array(x).GSV.getMaxSNR()
                Dim mMinSNR As Integer = TTD_parsed_nmea_array(x).GSV.getMinSNR()
                Dim offset_y As Integer = MaxMinSNR_PB.Height - (mMaxSNR * mOffset)
                e.Graphics.DrawRectangle(Pens.Blue, CInt(start_offset_x + (x * interval_width)), offset_y, 1, 1)

                offset_y = MaxMinSNR_PB.Height - (mMinSNR * mOffset)
                e.Graphics.DrawRectangle(Pens.Red, CInt(start_offset_x + (x * interval_width)), offset_y, 1, 1)
                ' End of drawing Max / Min SNR chart
            End If
            Dim temp As Integer = MaxMinSNR_PB.Height / 12

            If PDOPCB.Checked() = True And TTD_parsed_nmea_array(x).GSA.PDOP <> "99.99" Then
                Dim dop_start_offset_y As Integer = MaxMinSNR_PB.Height - TTD_parsed_nmea_array(x).GSA.PDOP * temp
                e.Graphics.FillEllipse(redBrush, CInt(start_offset_x + (x * interval_width)), dop_start_offset_y, 2, 3)
            End If

            If HDOPCB.Checked() = True And TTD_parsed_nmea_array(x).GSA.HDOP <> "99.99" Then
                Dim dop_start_offset_y As Integer = MaxMinSNR_PB.Height - TTD_parsed_nmea_array(x).GSA.HDOP * temp
                e.Graphics.FillEllipse(blueBrush, CInt(start_offset_x + (x * interval_width)), dop_start_offset_y, 2, 3)
            End If

            If VDOPCB.Checked() = True And TTD_parsed_nmea_array(x).GSA.VDOP <> "99.99" Then
                Dim dop_start_offset_y As Integer = MaxMinSNR_PB.Height - TTD_parsed_nmea_array(x).GSA.VDOP * temp
                e.Graphics.FillEllipse(blackBrush, CInt(start_offset_x + (x * interval_width)), dop_start_offset_y, 2, 3)
            End If
        Next
    End Sub

    Private Sub MTK_Info_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MTK_Info_CheckBox.CheckedChanged
        If MTK_Info_CheckBox.Checked = True Then
            MTKTableLayoutPanel.Show()
        Else
            MTKTableLayoutPanel.Hide()
        End If
    End Sub


    Private Sub PDOPCB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PDOPCB.CheckedChanged
        If PDOPCB.Checked() = True Then
            P_Value.Text = TTD_parsed_nmea_array(current_nmea_cnt).GSA.PDOP
        Else
            P_Value.Hide()
        End If
        Others.Refresh()
    End Sub

    Private Sub HDOPCB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HDOPCB.CheckedChanged
        If HDOPCB.Checked() = True Then
            H_Value.Text = TTD_parsed_nmea_array(current_nmea_cnt).GSA.HDOP
        Else
            H_Value.Hide()
        End If
        Others.Refresh()
    End Sub

    Private Sub VDOPCB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VDOPCB.CheckedChanged
        If VDOPCB.Checked() = True Then
            V_Value.Text = TTD_parsed_nmea_array(current_nmea_cnt).GSA.VDOP
        Else
            V_Value.Hide()
        End If
        Others.Refresh()
    End Sub

    Private Sub SNRCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SNRCheckBox.CheckedChanged
        Dim g As Graphics = InfoPictureBox.CreateGraphics()
        'DOPPage.Refresh()
        If SNRCheckBox.Checked() = True Then
            Dim info As String = "Max : " + TTD_parsed_nmea_array(current_nmea_cnt).GSV.getMaxSNR() + ";" + "Min : " + TTD_parsed_nmea_array(current_nmea_cnt).GSV.getMinSNR()
            g.Clear(Color.LightGray)
            g.DrawString(info, InfofontObj, Brushes.Black, 2, 5)
        Else
            g.Clear(Color.LightGray)
        End If
        MaxMinSNR_PB.Refresh()
    End Sub

    'By index to get Satallite SNR value , if it be used , its ID , Azimuth and elevation
    Private Function GetSNRByIndex(ByVal index As Integer, ByRef beUsed As Boolean, ByRef mSatID As Integer, ByRef Azimuth As String, ByRef mStrElevation As String, ByVal mIndex As Integer) As Integer
        'mIndex : 0:GPS , 1:GLONASS , 2:BD
        Select Case mIndex
            Case 0 'GPS
                If TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGPSateInViewNumber > 0 And IsNothing(TTD_parsed_nmea_array(current_nmea_cnt).GSV.GP_Satelliate_info(index)) <> True Then
                    mSatID = TTD_parsed_nmea_array(current_nmea_cnt).GSV.getPRN(0, index)
                    Azimuth = TTD_parsed_nmea_array(current_nmea_cnt).GSV.GP_Satelliate_info(index).azimuth
                    mStrElevation = TTD_parsed_nmea_array(current_nmea_cnt).GSV.GP_Satelliate_info(index).elevation
                    beUsed = False
                    beUsed = TTD_parsed_nmea_array(current_nmea_cnt).GSA.beUsedforFix(mSatID)
                    Return TTD_parsed_nmea_array(current_nmea_cnt).GSV.GP_Satelliate_info(index).SNR_in_dB
                Else
                    mSatID = 0
                    Azimuth = 0
                    mStrElevation = 0
                    beUsed = False
                    Return 0
                End If
            Case 1 'GLONASS
                If TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGLSateInViewNumber > 0 And IsNothing(TTD_parsed_nmea_array(current_nmea_cnt).GSV.GL_Satelliate_info(index)) <> True Then
                    mSatID = TTD_parsed_nmea_array(current_nmea_cnt).GSV.getPRN(1, index)
                    Azimuth = TTD_parsed_nmea_array(current_nmea_cnt).GSV.GL_Satelliate_info(index).azimuth
                    mStrElevation = TTD_parsed_nmea_array(current_nmea_cnt).GSV.GL_Satelliate_info(index).elevation
                    beUsed = False
                    beUsed = TTD_parsed_nmea_array(current_nmea_cnt).GSA.beUsedforFix(mSatID)
                    Return TTD_parsed_nmea_array(current_nmea_cnt).GSV.GL_Satelliate_info(index).SNR_in_dB
                Else
                    mSatID = 0
                    Azimuth = 0
                    mStrElevation = 0
                    beUsed = False
                    Return 0
                End If
            Case 2 'BD
                If TTD_parsed_nmea_array(current_nmea_cnt).GSV.getGBSateInViewNumber > 0 And IsNothing(TTD_parsed_nmea_array(current_nmea_cnt).GSV.GB_Satelliate_info(index)) <> True Then
                    mSatID = TTD_parsed_nmea_array(current_nmea_cnt).GSV.getPRN(1, index)
                    Azimuth = TTD_parsed_nmea_array(current_nmea_cnt).GSV.GB_Satelliate_info(index).azimuth
                    mStrElevation = TTD_parsed_nmea_array(current_nmea_cnt).GSV.GB_Satelliate_info(index).elevation
                    beUsed = False
                    beUsed = TTD_parsed_nmea_array(current_nmea_cnt).GSA.beUsedforFix(mSatID)
                    Return TTD_parsed_nmea_array(current_nmea_cnt).GSV.GB_Satelliate_info(index).SNR_in_dB
                Else
                    mSatID = 0
                    Azimuth = 0
                    mStrElevation = 0
                    beUsed = False
                    Return 0
                End If
        End Select

        Return -1
    End Function

    Private Sub Main_Form_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        Dim max As Integer = 0
        ' when the file is dropped on the form, we get the filename from it.
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        Label1.Text = files(0)
        InitUI(InitProcess())
    End Sub

    Private Sub Main_Form_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter
        Dim max As Integer = 0
        If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub Main_Form_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Left Then

        ElseIf e.KeyCode = Keys.Right Then

        End If
    End Sub

    Private Sub Main_Form_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If My.Computer.FileSystem.DirectoryExists("C:\Documents and Settings\All Users\Documents\GPSTemp") Then
            My.Computer.FileSystem.DeleteDirectory("C:\Documents and Settings\All Users\Documents\GPSTemp", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
    End Sub

End Class
