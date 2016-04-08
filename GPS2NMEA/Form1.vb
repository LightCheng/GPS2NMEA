Imports System.IO
Imports Microsoft.VisualBasic.PowerPacks
Imports System.Drawing.Drawing2D

Public Module GlobalVariables
    Public tempDIR As String = Directory.GetCurrentDirectory() + "\GPSTemp"
    Public extracted_nmea_data As String = tempDIR + "\nmea_out.txt"
    Public extracted_mtk_data As String = tempDIR + "\mtk_out.txt"

    Public totaltick As Integer = 0
    Public fileReader As System.IO.StreamReader
    Public stringReader As String
    Public mTotalSatellites As Integer = 0

    Public greenBrush As New Drawing.SolidBrush(Color.Green)
    Public yellowBrush As New Drawing.SolidBrush(Color.Yellow)
    Public blueBrush As New Drawing.SolidBrush(Color.Blue)
    Public redBrush As New Drawing.SolidBrush(Color.Red)
    Public blackBrush As New Drawing.SolidBrush(Color.Black)
    Public fontObj As Font = New System.Drawing.Font("Calibri", 10, FontStyle.Regular)
    Public InfofontObj As Font = New System.Drawing.Font("Calibri", 10, FontStyle.Bold)

    Public Structure Satellite_info
        Public Prn As Integer 'Satellite PRN ("pseudo-random noise" sequences) number (GPGSV)
        Public Elevation As Integer 'Elevation, degrees , 0 ~ 90 (GPGSV)
        Public Azimuth As Integer 'Azimuth, degrees , 0 ~ 360 (GPGSV)
        Public Snr As Integer 'higher is better.(GPGSV)
        Public Valid As Boolean 'check the satellite valid or not.
        Public UsedInFix As Boolean 'If be used to compute the most recent fix (GPGSA)
    End Structure

    Public Structure DOP_info
        Public PDOP As Double 'dilution of precision (GPGSA)
        Public HDOP As Double 'Horizontal dilution of precision (GPGSA)
        Public VDOP As Double 'Vertical dilution of precision (GPGSA)
    End Structure

    Public Structure GGA
        Public strUtcTime As String
        Public varLatitude As Object
        Public strNSIndicator As String
        Public varLongitude As Object
        Public strEWIndicator As String
        Public strPositionFix As String
        Public strSatsUsed As String
        Public strIIDOP As String
        Public strAltitude As String
        Public strAltUnits As String
        Public strGeoid As String
        Public strSepUnits As String
        Public strDgpsAge As String
        Public strDgpsid As String
        Public strchecksum As String
        Public strTerminator As String
    End Structure

    Public Structure VTG
        Public strCourse1 As String
        Public strReference1 As String
        Public strCourse2 As String
        Public strReference2 As String
        Public strSpeed1 As String
        Public strSpeed2 As String
        Public strSpeedUnit1 As String
        Public strSpeedUnit2 As String
    End Structure

    'GPGSA include GPS system only
    'GSA : Satellite status
    Public Structure GSA
        Public strMode As String
        Public strFixType As String
        Public mSA() As Integer '1 ~ 12 Satalites from GPS system
        Public strPDOP As String
        Public strHDOP As String
        Public strVDOP As String
        Public mSANum As Integer
    End Structure

    'GNGSA include GPS & GLNASS systems
    Public Structure GSA_GN
        Public strMode As String
        Public strFixType As String
        Public mSA(,) As Integer 'GPS system + 1 ~ 12 Satalites from GLONASS (Prn from 65 ~ 96).1st row for GPS , 2nd row for GLONASS
        Public strPDOP As String
        Public strHDOP As String
        Public strVDOP As String
        Public mSANum As Integer
    End Structure

    'GSV info from GPS
    Public Structure GP_GSV
        Public StrTotalNoMessages As String
        Public StrSeq As String
        Public StrSatsinview As String
        Public mSV() As Satellite_info 'Detailed Satellite data
        Public mMaxSNR As Integer
        Public mMinSNR As Integer
    End Structure

    'GSV info from GLONASS
    Public Structure GL_GSV
        Public StrTotalNoMessages As String
        Public StrSeq As String
        Public StrSatsinview As String
        Public mSV() As Satellite_info 'Detailed Satellite data
        Public mMaxSNR As Integer
        Public mMinSNR As Integer
    End Structure

    'GSV info from BD system , 
    'PhaseII : Regional service for Asia-Pacific area by 2012 , Global service by 2020
    Public Structure BD_GSV
        Public StrTotalNoMessages As String
        Public StrSeq As String
        Public StrSatsinview As String
        Public mSV() As Satellite_info 'Detailed Satellite data
        Public mMaxSNR As Integer
        Public mMinSNR As Integer
    End Structure

    Public Structure RMC
        Public strUtcTime As String
        Public strFixStatus As String
        Public varLatitude As String
        Public strNSIndicator As String
        Public varLongitude As String
        Public strEWIndicator As String
        Public StrSpeedoverground As String
        Public StrCourse As String
        Public StrDate_of_fix As String
        Public StrMagnetic As String
    End Structure

    Public Structure all_GSV_info
        Public mBDGSV As BD_GSV
        Public mGLGSV As GL_GSV
        Public mGPGSV As GP_GSV
    End Structure

    Public Structure all_nmea_info
        Public mGGA As GGA
        Public mRMC As RMC
        Public mGSA As GSA
        Public mAllGSV As all_GSV_info
        Public mAccuracy As Double
        Public mEPH As String
        Public mGSA_GN As GSA_GN
    End Structure

    Public MTKGPSArray(,) As String = {
            {"PMTK010", "Check if GPS works"},
            {"PMTK101", "GPS Hot start, Use all available data in the NV Store."},
            {"PMTK102", "GPS Warm start , Don't use Ephemeris at re-start"},
            {"PMTK103", "GPS Cold start , Don't use Time, Position, Almanacs and Ephemeris data at re-start."},
            {"PMTK104", "GPS Full Cold start , It’s essentially a Cold Restart, but additionally clear system/user configurations at re-start. That is, reset the receiver to the factory status."},
            {"PMTK106", "AGPS re-start"},
            {"PMTK710", "Get Ephemeris assistant info"},
            {"PMTK712", "Get Time assistant info"},
            {"PMTK713", "Lack of 位置輔助資訊"},
            {"PMTK730", "是否觸發AGPS"},
            {"PMTKEPH", "目前有效的衛星星曆,若衛星代號是負數 , 表示資料是從 EPO or BEE (hotstill) 取得"},
            {"", ""}}

    'MTK_GPS_AGPS_DT_ACK_T rAck;              // PMTK001
    'MTK_GPS_AGPS_CMD_MODE_T rAgpsMode;       // PMTK290
    'MTK_GPS_AGPS_DT_REQ_ASSIST_T rReqAssist; // PMTK730
    'MTK_GPS_AGPS_DT_LOC_EST_T rLoc;          // PMTK731
    'MTK_GPS_AGPS_DT_GPS_MEAS_T rPRM;         // PMTK732
    'MTK_GPS_AGPS_DT_LOC_ERR_T rLocErr;       // PMTK733
    'MTK_GPS_AGPS_DT_FTIME_T rFTime;          // PMTK734
    'MTK_GPS_AGPS_DT_FTIME_ERR_T rFTimeErr;   // PMTK735
    'MTK_GPS_AGPS_DT_LOC_EXTRA_T rLocExtra;     // PMTK742/743/744
    'MTK_AGNSS_DT_REQ_ASSIST_T  rGnssReqAssist; // PMTK760
    'MTK_AGNSS_DT_LOC_EST_T     rGnssLoc;       // PMTK761
    'MTK_AGNSS_DT_MEAS_T        rGnssPRM;       // PMTK763
    'MTK_AGNSS_DT_CAPBILITY_T   rGnssCap;       // PMTK764

    Public all_nmea_sentence_info(256) As all_nmea_info
    Public total_nmea_cnt As Integer = 0
    Public current_nmea_cnt As Integer = 0

    ' Public view_start_x As Double = (TabPage2.Width * 0.7) / 2 + 150
    '////////////////////////////////////////////////////////////////////////
    Public whitePen As New Pen(Color.FromArgb(255, 200, 200, 200), 1)
    Public blackPen As New Pen(Color.FromArgb(255, 0, 0, 0), 1)
    Public redPen As New Drawing.Pen(Color.FromArgb(255, 255, 0, 0), 1)
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
        'Me.DoubleBuffered = True
        KeyPreview = True
        blackPen.Alignment = PenAlignment.Inset
        redPen.Alignment = PenAlignment.Inset
        whitePen.DashStyle = DashStyle.DashDot
    End Sub

    Private Function DisplayOpenFileDialog() As Boolean
        Dim openFile As New System.Windows.Forms.OpenFileDialog()
        openFile.DefaultExt = "*.*"
        openFile.Filter = "all files (*.*)|*.*"
        openFile.ShowDialog()
        If openFile.FileNames.Length > 0 Then
            Dim filename As String
            For Each filename In openFile.FileNames
                ' Insert code here to process the files.
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
            InitUI()
        End If
        InitProcess()
    End Sub

    Private Function InitUI() As Boolean
        If StatusControl1.TabPages.Contains(SatelliteNum) = False Then
            StatusControl1.TabPages.Insert(1, SatelliteNum)
        End If
        If StatusControl1.TabPages.Contains(Others) = False Then
            StatusControl1.TabPages.Insert(2, Others)
        End If
        ProgressBar1.Show()
        StatusControl1.SelectTab(0)
        current_data_index.Text = 1
        mMaxInViewSatNumber = 0
        Return True
    End Function

    Private Function InitProcess() As Boolean
        Dim lineCount = File.ReadAllLines(Label1.Text).Length
        ProgressBar1.Value = 0
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = lineCount
        ProgressBar1.Value = 0

        total_nmea_cnt = 0
        AGPS_TYPE.Text = ""

        extraNmeaAndMTKfromLog(Label1.Text)

        'Dim TotalLines As Integer = File.ReadAllLines(extracted_nmea_data).Length
        ReDim all_nmea_sentence_info(total_nmea_cnt)
        Total_data_number.Text = total_nmea_cnt
        fileReader = My.Computer.FileSystem.OpenTextFileReader(extracted_nmea_data)
        Dim GP_index As Integer = 0

        Do While fileReader.Peek() > -1
            stringReader = fileReader.ReadLine()
            If stringReader.Contains("$GPGGA") Or stringReader.Contains("$GNGGA") Then
                GP_index += 1
            End If
            If stringReader.Contains("$GPGSV") Then
                Dim tempArray() As String = Split(stringReader, ",")
                'tempArray(3) : total sat# in view.
                If tempArray(2) = 1 Then 'redefine SV array only need at 1st page
                    ReDim all_nmea_sentence_info(GP_index).mAllGSV.mGPGSV.mSV(tempArray(3))
                End If
            End If
            If stringReader.Contains("$GLGSV") Then
                Dim tempArray() As String = Split(stringReader, ",")
                'tempArray(3) : total sat# in view.
                If tempArray(2) = 1 Then 'redefine SV array only need at 1st page
                    ReDim all_nmea_sentence_info(GP_index).mAllGSV.mGLGSV.mSV(tempArray(3))
                End If
            End If
            ParseSentence(stringReader, all_nmea_sentence_info(GP_index))
        Loop
        fileReader.Close()
        ProgressBar1.Hide()
        current_nmea_cnt = 0
        If total_nmea_cnt = 0 And GP_index = 0 Then 'no valid info , the file may be an invalid file
            HScrollBar1.Maximum = 0
            HScrollBar1.Minimum = 0
            HScrollBar1.Value = 0
            Dim dr As DialogResult = MessageBox.Show("Invalid file , please check again.", "Caption", MessageBoxButtons.OK)
            Status_Page.Refresh()
            Return False
        End If
        mTotalSatellites = CInt(all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.StrSatsinview) + CInt(all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGLGSV.StrSatsinview) + CInt(all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mBDGSV.StrSatsinview)
        'Init scrollbar range.
        HScrollBar1.Maximum = GP_index - 1
        HScrollBar1.Minimum = 0
        HScrollBar1.Value = 0

        UpdateBasicInfoDashboard(1)
        SNRBARPictureBox.Refresh()
        SatViewPictureBox.Refresh()
        Return False
    End Function

    Private Function extraNmeaAndMTKfromLog(ByVal filename As String) As Boolean
        If Directory.Exists(tempDIR) = False Then
            Directory.CreateDirectory(tempDIR)
        End If
        fileReader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Dim nmea_writer = My.Computer.FileSystem.OpenTextFileWriter(extracted_nmea_data, False)
        Dim mtk_writer = My.Computer.FileSystem.OpenTextFileWriter(extracted_mtk_data, False)

        Do While fileReader.Peek() > -1
            stringReader = fileReader.ReadLine()
            Dim TestPos As Integer = InStr(stringReader, "$G")
            If TestPos > 1 Then  'For some log not start by "$G" , such as MTK main log.
                Dim tempString As String = stringReader.Substring(TestPos - 1)
                stringReader = tempString
            End If

            If IsNmeaNeededData(stringReader) = True Then
                nmea_writer.WriteLine(stringReader)
                If stringReader.Contains("GGA") Then
                    total_nmea_cnt += 1
                End If
            End If

            'Add GPGGA as time token for MTK info
            If IsMtkNeededData(stringReader) = True Then
                mtk_writer.WriteLine(stringReader)
                'Check PMTK013 for GPS clock source info , 254:co-clock , 255:TCXO
                'with co-clock , C0 : (+-5) , C1 : (-0.1 ~ -0.35) , if c1 or c2 = 0 means no calibration
                If stringReader.Contains("ClkType") Then
                    Dim i As Integer = stringReader.IndexOf("ClkType")
                    If Mid(stringReader, i + 9, 3) = "254" Then
                        CLK_TYPE.Text = "co-Clock"
                    ElseIf Mid(stringReader, i + 9, 3) = "255" Then
                        CLK_TYPE.Text = "TCXO"
                    Else
                        CLK_TYPE.Text = ""
                    End If
                End If
            End If

            If stringReader.Contains("wkssi") Then
                AGPS_TYPE.Text = AGPS_TYPE.Text + "[AGPS]"
            ElseIf stringReader.Contains("wkbee") Then
                AGPS_TYPE.Text = AGPS_TYPE.Text + "[Hotstill]"
            ElseIf stringReader.Contains("wk,epo") Then
                AGPS_TYPE.Text = AGPS_TYPE.Text + "[EPO]"
            End If
            ProgressBar1.Value += 1
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
        Return False
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
        '{"$PMTKEPH", "MTK only , 判斷是即時接收解算下来的Almanac、或是透過EPP or HotStill"}}
        Dim size As Integer = NeededArray.Length
        For x = 0 To ((size / 2) - 1)
            Dim tString As String = NeededArray(x, 0)
            If sentence.Contains(NeededArray(x, 0)) = True Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function IsMtkNeededData(ByVal sentence As String) As Boolean
        Dim NeededArray(,) As String = {
            {"PMTK", "Global Positioning System receiver"},
            {"$GPGGA", "Galileo Positioning System"},
            {"$GNGGA", "GLONASS, according to IEIC 61162-1"},
            {"ClkType", "Mixed GPS and GLONASS data, according to IEIC 61162-1"},
            {"$PMTKEPH", "MTK only , 判斷是即時接收解算下来的Almanac、或是透過EPP or HotStill"}}

        Dim size As Integer = NeededArray.Length
        For x = 0 To ((size / 2) - 1)
            Dim tString As String = NeededArray(x, 0)
            If sentence.Contains(NeededArray(x, 0)) = True Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function UpdateBasicInfoDashboard(ByVal currentValue As Integer) As Boolean
        UTC_data.Text = Mid(all_nmea_sentence_info(currentValue).mGGA.strUtcTime, 1, 2) + ":" + Mid(all_nmea_sentence_info(currentValue).mGGA.strUtcTime, 3, 2) + ":" + Mid(all_nmea_sentence_info(currentValue).mGGA.strUtcTime, 5, 2) 'all_nmea_sentence_info(currentValue).mGGA.strUtcTime
        Latitude_data.Text = all_nmea_sentence_info(currentValue).mGGA.varLatitude + " " + all_nmea_sentence_info(currentValue).mGGA.strNSIndicator
        Longitude_data.Text = all_nmea_sentence_info(currentValue).mGGA.varLongitude + " " + all_nmea_sentence_info(currentValue).mGGA.strEWIndicator

        Select Case all_nmea_sentence_info(currentValue).mGSA.strFixType
            Case 1
                Fixed_T.Text = "No Fix"
            Case 2
                Fixed_T.Text = "2D Fix"
            Case 3
                Fixed_T.Text = "3D Fix"
        End Select

        ACLabel.Text = all_nmea_sentence_info(currentValue).mAccuracy.ToString + " (m)"

        If IsNothing(all_nmea_sentence_info(currentValue).mEPH) <> True Then
            Dim TestArray() As String = Split(all_nmea_sentence_info(currentValue).mEPH, ",")
            Alma_Label.Text = "(" + TestArray(0) + ") : " + all_nmea_sentence_info(currentValue).mEPH.Substring(all_nmea_sentence_info(currentValue).mEPH.IndexOf(",") + 1)
        End If
        Return True
    End Function

    'Process scrollbar event and update "Status_Page"
    Private Sub HScrollBar1_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar1.Scroll
        If e.NewValue < 0 Then
            HScrollBar1.Value = 0
        End If
        current_data_index.Text = (HScrollBar1.Value + 1).ToString
        Dim currentValue = HScrollBar1.Value + 1
        Dim canvas As New ShapeContainer
        Dim theLine As New LineShape
        Dim temp As String = HScrollBar1.ToString()
        current_nmea_cnt = HScrollBar1.Value

        'By the new index , update UTC/Lat/Longitude data from array "all_nmea_sentence_info"
        UpdateBasicInfoDashboard(currentValue)
        mTotalSatellites = CInt(all_nmea_sentence_info(currentValue).mAllGSV.mGPGSV.StrSatsinview) + CInt(all_nmea_sentence_info(currentValue).mAllGSV.mGLGSV.StrSatsinview) + CInt(all_nmea_sentence_info(currentValue).mAllGSV.mBDGSV.StrSatsinview)
        If PDOPCB.Checked() = True Then
            P_Value.Text = all_nmea_sentence_info(current_nmea_cnt + 1).mGSA.strPDOP
        End If
        If HDOPCB.Checked() = True Then
            H_Value.Text = all_nmea_sentence_info(current_nmea_cnt + 1).mGSA.strHDOP
        End If
        If VDOPCB.Checked() = True Then
            V_Value.Text = all_nmea_sentence_info(current_nmea_cnt + 1).mGSA.strVDOP
        End If

        If SNRCheckBox.Checked() = True Then
            Dim info As String = "Max : " + all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.mMaxSNR.ToString + ";" + "Min : " + all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.mMinSNR.ToString
            Dim g As Graphics = InfoPictureBox.CreateGraphics()
            g.Clear(Color.LightGray)
            g.DrawString(info, InfofontObj, Brushes.Black, 2, 5)
        End If

        Select Case all_nmea_sentence_info(current_nmea_cnt + 1).mGSA.strFixType
            Case 1
                Fixed_T.Text = "No Fix"
            Case 2
                Fixed_T.Text = "2D Fix"
            Case 3
                Fixed_T.Text = "3D Fix"
        End Select

        ACLabel.Text = all_nmea_sentence_info(current_nmea_cnt + 1).mAccuracy.ToString + " (m)"

        If IsNothing(all_nmea_sentence_info(current_nmea_cnt + 1).mEPH) <> True Then
            Dim TestArray() As String = Split(all_nmea_sentence_info(current_nmea_cnt + 1).mEPH, ",")
            Alma_Label.Text = "(" + TestArray(0) + ") : " + all_nmea_sentence_info(current_nmea_cnt + 1).mEPH.Substring(all_nmea_sentence_info(current_nmea_cnt + 1).mEPH.IndexOf(",") + 1)
        End If

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

        If total_nmea_cnt = 0 Then
            Return
        End If

        For x = 1 To CInt(all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.StrSatsinview)
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
                ElseIf mSNR > 0 Then
                    e.Graphics.FillEllipse(yellowBrush, CInt(px), CInt(py), mSatallite_size, mSatallite_size)
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

        For x = 1 To CInt(all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGLGSV.StrSatsinview)
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
                ElseIf mSNR > 0 Then
                    e.Graphics.FillRectangle(yellowBrush, CInt(px), CInt(py), mSatallite_size, mSatallite_size)
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

    End Sub

    Private Sub SNRBARPictureBox_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles SNRBARPictureBox.Paint
        Dim devide As Integer = 15
        If mTotalSatellites > 32 Then
            devide = mTotalSatellites
        ElseIf mTotalSatellites > 30 Then
            devide = 32
        ElseIf mTotalSatellites > 25 Then
            devide = 30
        ElseIf mTotalSatellites > 20 Then
            devide = 25
        ElseIf mTotalSatellites > 15 Then
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

        If total_nmea_cnt = 0 Then
            Return
        End If

        Dim strSateInView As String = "Satellite in View : " + mTotalSatellites.ToString
        e.Graphics.DrawString(strSateInView, fontObj, Brushes.Black, CInt(margin), SNRBARPictureBox.Height / 5)
        strSateInView = "Satellite Used : " + (all_nmea_sentence_info(current_nmea_cnt).mGSA.mSANum + all_nmea_sentence_info(current_nmea_cnt).mGSA_GN.mSANum).ToString
        e.Graphics.DrawString(strSateInView, fontObj, Brushes.Black, CInt(margin), SNRBARPictureBox.Height / 4)

        For x = 1 To CInt(all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.StrSatsinview)
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

        For x = 1 To CInt(all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGLGSV.StrSatsinview)
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
            If (all_nmea_sentence_info(x + 1).mAllGSV.mGPGSV.StrSatsinview) > 0 Then
                Dim sat_num_height As Integer = CInt(Satellite_PB.Height - (temp_h * all_nmea_sentence_info(x + 1).mAllGSV.mGPGSV.StrSatsinview) + Y_offset) - 3
                e.Graphics.FillEllipse(Brushes.Red, CInt(start_offset_x + (x * interval_width)) + 2, sat_num_height - 1, 2, 3)

                If (all_nmea_sentence_info(x + 1).mGSA.mSANum) > 0 Then
                    sat_num_height = CInt(Satellite_PB.Height - (temp_h * all_nmea_sentence_info(x + 1).mGSA.mSANum) + Y_offset) - 3
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
            Select Case all_nmea_sentence_info(x).mGSA.strFixType
                Case 1
                    'e.Graphics.FillEllipse(greenBrush, CInt(start_offset_x + (x * interval_width)), CInt(Label_NoFix.Location.Y + 2), 3, 5)
                Case 2
                    e.Graphics.DrawRectangle(Pens.Green, CInt(start_offset_x + (x * interval_width)), CInt(MaxMinSNR_PB.Height / 10) + 80, 1, 1)
                Case 3
                    e.Graphics.DrawRectangle(Pens.Green, CInt(start_offset_x + (x * interval_width)), CInt(MaxMinSNR_PB.Height / 10) + 30, 1, 1)
            End Select

            If SNRCheckBox.Checked() = True Then
                Dim mMaxSNR As Integer = all_nmea_sentence_info(x + 1).mAllGSV.mGPGSV.mMaxSNR
                Dim mMinSNR As Integer = all_nmea_sentence_info(x + 1).mAllGSV.mGPGSV.mMinSNR
                Dim offset_y As Integer = MaxMinSNR_PB.Height - (mMaxSNR * mOffset)
                e.Graphics.DrawRectangle(Pens.Blue, CInt(start_offset_x + (x * interval_width)), offset_y, 1, 1)

                offset_y = MaxMinSNR_PB.Height - (mMinSNR * mOffset)
                e.Graphics.DrawRectangle(Pens.Red, CInt(start_offset_x + (x * interval_width)), offset_y, 1, 1)
                ' End of drawing Max / Min SNR chart
            End If
            Dim temp As Integer = MaxMinSNR_PB.Height / 12

            If PDOPCB.Checked() = True And all_nmea_sentence_info(x + 1).mGSA.strPDOP <> "99.99" Then
                Dim dop_start_offset_y As Integer = MaxMinSNR_PB.Height - all_nmea_sentence_info(x + 1).mGSA.strPDOP * temp
                e.Graphics.FillEllipse(redBrush, CInt(start_offset_x + (x * interval_width)), dop_start_offset_y, 2, 3)
            End If

            If HDOPCB.Checked() = True And all_nmea_sentence_info(x + 1).mGSA.strHDOP <> "99.99" Then
                Dim dop_start_offset_y As Integer = MaxMinSNR_PB.Height - all_nmea_sentence_info(x + 1).mGSA.strHDOP * temp
                e.Graphics.FillEllipse(blueBrush, CInt(start_offset_x + (x * interval_width)), dop_start_offset_y, 2, 3)
            End If

            If VDOPCB.Checked() = True And all_nmea_sentence_info(x + 1).mGSA.strVDOP <> "99.99" Then
                Dim dop_start_offset_y As Integer = MaxMinSNR_PB.Height - all_nmea_sentence_info(x + 1).mGSA.strVDOP * temp
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
            P_Value.Text = all_nmea_sentence_info(current_nmea_cnt + 1).mGSA.strPDOP
        Else
            P_Value.Hide()
        End If
        Others.Refresh()
    End Sub

    Private Sub HDOPCB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HDOPCB.CheckedChanged
        If HDOPCB.Checked() = True Then
            H_Value.Text = all_nmea_sentence_info(current_nmea_cnt + 1).mGSA.strHDOP
        Else
            H_Value.Hide()
        End If
        Others.Refresh()
    End Sub

    Private Sub VDOPCB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VDOPCB.CheckedChanged
        If VDOPCB.Checked() = True Then
            V_Value.Text = all_nmea_sentence_info(current_nmea_cnt + 1).mGSA.strVDOP
        Else
            V_Value.Hide()
        End If
        Others.Refresh()
    End Sub

    Private Sub SNRCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SNRCheckBox.CheckedChanged
        Dim g As Graphics = InfoPictureBox.CreateGraphics()
        'DOPPage.Refresh()
        If SNRCheckBox.Checked() = True Then
            Dim info As String = "Max : " + all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.mMaxSNR.ToString + ";" + "Min : " + all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.mMinSNR.ToString
            g.Clear(Color.LightGray)
            g.DrawString(info, InfofontObj, Brushes.Black, 2, 5)
        Else
            g.Clear(Color.LightGray)
        End If
        MaxMinSNR_PB.Refresh()
    End Sub

    'Parse nmea sentense .
    Sub ParseSentence(ByVal InBuff As String, ByRef mCurrent_nmea_info As all_nmea_info)
        Dim aryData(25)
        Dim x, z, st As Integer
        Dim strSentence As String
        Dim intComma As Integer = 0

        '** Find the start of the sentence...locate $... st stores start position.
        For x = 1 To Len(InBuff)
            If Mid(InBuff, x, 1) = "$" Then
                st = x
                Exit For
            End If
        Next

        strSentence = InBuff
        '** How many fields are there in the sentence...Count the comma's
        For x = 1 To Len(strSentence)
            If Mid(strSentence, x, 1) = "," Then
                intComma = intComma + 1
            End If
        Next

        '** Fill an array with the field data..
        z = 8
        For x = 1 To intComma
            Do While Mid(strSentence, z, 1) <> "," And Mid(strSentence, z, 1) <> "*" And z <= strSentence.Length
                aryData(x) = aryData(x) & Mid(strSentence, z, 1)
                z = z + 1
            Loop
            z = z + 1
        Next
        Select Case Microsoft.VisualBasic.Left(strSentence, 6)
            Case "$PMTKE" 'MTK only, PMTK EPH , 判斷是即時接收解算下来的Almanac、或是透過EPP or HotStill。
                'mEPH
                Dim j As Integer = strSentence.IndexOf(",")
                mCurrent_nmea_info.mEPH = strSentence.Substring(j + 1)
            Case "$GPACC"  'MTK only ,Accuracy value.
            Case "$GNACC"
                Dim TestArray() As String = Split(strSentence, ",")
                If TestArray.Length > 0 Then
                    Dim TestArray2() As String = Split(TestArray(1), "*")
                    mCurrent_nmea_info.mAccuracy = TestArray2(0)
                End If
            Case "$GPGGA"
            Case "$GNGGA"
                mCurrent_nmea_info.mGGA.strUtcTime = aryData(1)
                mCurrent_nmea_info.mGGA.varLatitude = aryData(2)
                mCurrent_nmea_info.mGGA.strNSIndicator = aryData(3)
                mCurrent_nmea_info.mGGA.varLongitude = aryData(4)
                mCurrent_nmea_info.mGGA.strEWIndicator = aryData(5)
                mCurrent_nmea_info.mGGA.strPositionFix = aryData(6)
                mCurrent_nmea_info.mGGA.strSatsUsed = aryData(7)
                mCurrent_nmea_info.mGGA.strIIDOP = aryData(8)
                mCurrent_nmea_info.mGGA.strAltitude = aryData(9)
                mCurrent_nmea_info.mGGA.strAltUnits = aryData(10)
                mCurrent_nmea_info.mGGA.strGeoid = aryData(11)
                mCurrent_nmea_info.mGGA.strSepUnits = aryData(12)
                mCurrent_nmea_info.mGGA.strDgpsAge = aryData(13)
                mCurrent_nmea_info.mGGA.strDgpsid = aryData(14)
            Case "$GPRMC"
                mCurrent_nmea_info.mRMC.strUtcTime = aryData(1)
                mCurrent_nmea_info.mRMC.strFixStatus = aryData(2)
                mCurrent_nmea_info.mRMC.varLatitude = aryData(3)
                mCurrent_nmea_info.mRMC.strNSIndicator = aryData(4)
                mCurrent_nmea_info.mRMC.varLongitude = aryData(5)
                mCurrent_nmea_info.mRMC.strEWIndicator = aryData(6)
                mCurrent_nmea_info.mRMC.StrSpeedoverground = aryData(7)
                mCurrent_nmea_info.mRMC.StrCourse = aryData(8)
            Case "$GNGNS"  'GN : GNSS position fix from more than one constellation (eg. GPS + GLONASS)

            Case "$GNGSA" 'GPS + GLONASS , Glonass satellites are identified with IDs from 65 to 96
                mCurrent_nmea_info.mGSA_GN.strMode = aryData(1)
                mCurrent_nmea_info.mGSA_GN.strFixType = aryData(2)
                ReDim mCurrent_nmea_info.mGSA_GN.mSA(2, 12)
                mCurrent_nmea_info.mGSA_GN.mSANum = 0
                For x = 0 To 11
                    If CInt(aryData(3 + x)) <> 0 And CInt(aryData(3 + x)) >= 65 Then
                        mCurrent_nmea_info.mGSA_GN.mSA(1, x) = aryData(3 + x)
                        If IsNothing(mCurrent_nmea_info.mGSA_GN.mSA(1, x)) <> True And mCurrent_nmea_info.mGSA_GN.mSA(1, x) > 0 Then
                            mCurrent_nmea_info.mGSA_GN.mSANum += 1
                        End If
                    Else
                        mCurrent_nmea_info.mGSA_GN.mSA(0, x) = aryData(3 + x)
                        If IsNothing(mCurrent_nmea_info.mGSA_GN.mSA(0, x)) <> True And mCurrent_nmea_info.mGSA_GN.mSA(0, x) > 0 Then
                            mCurrent_nmea_info.mGSA_GN.mSANum += 1
                        End If
                    End If
                Next
                mCurrent_nmea_info.mGSA_GN.strPDOP = aryData(15)
                mCurrent_nmea_info.mGSA_GN.strHDOP = aryData(16)
                mCurrent_nmea_info.mGSA_GN.strVDOP = aryData(17)

            Case "$GPGSA"
                mCurrent_nmea_info.mGSA.strMode = aryData(1)
                mCurrent_nmea_info.mGSA.strFixType = aryData(2)
                ReDim mCurrent_nmea_info.mGSA.mSA(12)
                mCurrent_nmea_info.mGSA.mSANum = 0
                For x = 0 To 11
                    mCurrent_nmea_info.mGSA.mSA(x) = aryData(3 + x)
                    If IsNothing(mCurrent_nmea_info.mGSA.mSA(x)) <> True And mCurrent_nmea_info.mGSA.mSA(x) > 0 Then
                        mCurrent_nmea_info.mGSA.mSANum += 1
                    End If
                Next
                mCurrent_nmea_info.mGSA.strPDOP = aryData(15)
                mCurrent_nmea_info.mGSA.strHDOP = aryData(16)
                mCurrent_nmea_info.mGSA.strVDOP = aryData(17)

            Case "$GLGSV"  'GLONASS constellation GSV
                mCurrent_nmea_info.mAllGSV.mGLGSV.StrTotalNoMessages = aryData(1)
                mCurrent_nmea_info.mAllGSV.mGLGSV.StrSeq = aryData(2)
                mCurrent_nmea_info.mAllGSV.mGLGSV.StrSatsinview = aryData(3)

                'GetUpperBound
                Dim bound As Integer = mCurrent_nmea_info.mAllGSV.mGLGSV.mSV.GetUpperBound(0)
                Dim max As Integer = 0
                Dim min As Integer = 0

                For y = 0 To 3
                    Dim index As Integer = ((mCurrent_nmea_info.mAllGSV.mGLGSV.StrSeq - 1) * 4) + y
                    If index < bound Then
                        mCurrent_nmea_info.mAllGSV.mGLGSV.mSV(index).Prn = aryData(4 + y * 4)
                        mCurrent_nmea_info.mAllGSV.mGLGSV.mSV(index).Elevation = aryData(5 + y * 4)
                        mCurrent_nmea_info.mAllGSV.mGLGSV.mSV(index).Azimuth = aryData(6 + y * 4)
                        mCurrent_nmea_info.mAllGSV.mGLGSV.mSV(index).Snr = aryData(7 + y * 4)
                    End If

                    If IsNothing(mCurrent_nmea_info.mGSA_GN.mSA) <> True Then
                        If index = (bound - 1) Then 'sync if the sat be used from GSA.
                            For x = 0 To (bound - 1)
                                For z = 0 To 11
                                    If mCurrent_nmea_info.mGSA_GN.mSA(1, z) = mCurrent_nmea_info.mAllGSV.mGLGSV.mSV(x).Prn Then
                                        mCurrent_nmea_info.mAllGSV.mGLGSV.mSV(x).UsedInFix = True
                                        '                    If mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).Snr > max Then
                                        '                        max = mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).Snr
                                        '                    End If
                                        '                    If mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).Snr <= min Or min = 0 Then
                                        '                        min = mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).Snr
                                        '                    End If
                                    End If
                                Next
                            Next
                        End If
                    End If
                Next

            Case "$GPGSV"
                mCurrent_nmea_info.mAllGSV.mGPGSV.StrTotalNoMessages = aryData(1)
                mCurrent_nmea_info.mAllGSV.mGPGSV.StrSeq = aryData(2)
                mCurrent_nmea_info.mAllGSV.mGPGSV.StrSatsinview = aryData(3)

                If mMaxInViewSatNumber < mCurrent_nmea_info.mAllGSV.mGPGSV.StrSatsinview Then
                    mMaxInViewSatNumber = mCurrent_nmea_info.mAllGSV.mGPGSV.StrSatsinview
                End If
                'GetUpperBound
                Dim bound As Integer = mCurrent_nmea_info.mAllGSV.mGPGSV.mSV.GetUpperBound(0)
                '
                Dim max As Integer = 0
                Dim min As Integer = 0

                For y = 0 To 3
                    Dim index As Integer = ((mCurrent_nmea_info.mAllGSV.mGPGSV.StrSeq - 1) * 4) + y
                    If index < bound Then
                        mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(index).Prn = aryData(4 + y * 4)
                        mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(index).Elevation = aryData(5 + y * 4)
                        mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(index).Azimuth = aryData(6 + y * 4)
                        mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(index).Snr = aryData(7 + y * 4)
                    End If

                    If IsNothing(mCurrent_nmea_info.mGSA.mSA) <> True Then
                        If index = (bound - 1) Then 'sync if the sat be used from GSA.
                            For x = 0 To (bound - 1)
                                For z = 0 To 11
                                    If mCurrent_nmea_info.mGSA.mSA(z) = mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).Prn Then
                                        mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).UsedInFix = True
                                        If mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).Snr > max Then
                                            max = mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).Snr
                                        End If
                                        If mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).Snr <= min Or min = 0 Then
                                            min = mCurrent_nmea_info.mAllGSV.mGPGSV.mSV(x).Snr
                                        End If
                                    End If
                                Next
                            Next
                        End If
                    End If
                Next
                mCurrent_nmea_info.mAllGSV.mGPGSV.mMaxSNR = max
                mCurrent_nmea_info.mAllGSV.mGPGSV.mMinSNR = min
        End Select
    End Sub

    'By index to get Satallite SNR value , if it be used , its ID , Azimuth and elevation
    Private Function GetSNRByIndex(ByVal index As Integer, ByRef beUsed As Boolean, ByRef mSatID As Integer, ByRef Azimuth As String, ByRef mStrElevation As String, ByVal mIndex As Integer) As Integer
        'mIndex : 0:GPS , 1:GLONASS , 2:BD
        Select Case mIndex
            Case 0 'GPS
                mSatID = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.mSV(index - 1).Prn
                Azimuth = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.mSV(index - 1).Azimuth
                mStrElevation = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.mSV(index - 1).Elevation

                beUsed = False
                beUsed = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.mSV(index - 1).UsedInFix
                Return all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGPGSV.mSV(index - 1).Snr
            Case 1 'GLONASS
                mSatID = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGLGSV.mSV(index - 1).Prn
                Azimuth = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGLGSV.mSV(index - 1).Azimuth
                mStrElevation = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGLGSV.mSV(index - 1).Elevation

                beUsed = False
                beUsed = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGLGSV.mSV(index - 1).UsedInFix
                Return all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mGLGSV.mSV(index - 1).Snr
            Case 2 'BD
                mSatID = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mBDGSV.mSV(index - 1).Prn
                Azimuth = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mBDGSV.mSV(index - 1).Azimuth
                mStrElevation = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mBDGSV.mSV(index - 1).Elevation

                beUsed = False
                beUsed = all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mBDGSV.mSV(index - 1).UsedInFix
                Return all_nmea_sentence_info(current_nmea_cnt + 1).mAllGSV.mBDGSV.mSV(index - 1).Snr
        End Select

        Return -1
    End Function

    Private Sub Main_Form_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        Dim max As Integer = 0
        ' when the file is dropped on the form, we get the filename from it.
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        Label1.Text = files(0)
        InitUI()
        InitProcess()
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
