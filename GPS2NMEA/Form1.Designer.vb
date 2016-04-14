<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main_Form
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請不要使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main_Form))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Status_Page = New System.Windows.Forms.TabPage()
        Me.SNRBARPictureBox = New System.Windows.Forms.PictureBox()
        Me.SatViewPictureBox = New System.Windows.Forms.PictureBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MTK_Info_CheckBox = New System.Windows.Forms.CheckBox()
        Me.Total_data_number = New System.Windows.Forms.Label()
        Me.HScrollBar1 = New System.Windows.Forms.HScrollBar()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.UTC = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.UTC_data = New System.Windows.Forms.Label()
        Me.Latitude_data = New System.Windows.Forms.Label()
        Me.Longitude_data = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Fixed_T = New System.Windows.Forms.Label()
        Me.MTK_CLK_TYPE = New System.Windows.Forms.Label()
        Me.CLK_TYPE = New System.Windows.Forms.Label()
        Me.StatusControl1 = New System.Windows.Forms.TabControl()
        Me.Others = New System.Windows.Forms.TabPage()
        Me.MaxMinSNR_PB = New System.Windows.Forms.PictureBox()
        Me.SNRCheckBox = New System.Windows.Forms.CheckBox()
        Me.InfoPictureBox = New System.Windows.Forms.PictureBox()
        Me.V_Value = New System.Windows.Forms.Label()
        Me.H_Value = New System.Windows.Forms.Label()
        Me.P_Value = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.VDOPCB = New System.Windows.Forms.CheckBox()
        Me.HDOPCB = New System.Windows.Forms.CheckBox()
        Me.PDOPCB = New System.Windows.Forms.CheckBox()
        Me.SatelliteNum = New System.Windows.Forms.TabPage()
        Me.Satellite_PB = New System.Windows.Forms.PictureBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.current_data_index = New System.Windows.Forms.Label()
        Me.MTKTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.AccuracyLabel = New System.Windows.Forms.Label()
        Me.ACLabel = New System.Windows.Forms.Label()
        Me.AGPSLabel = New System.Windows.Forms.Label()
        Me.Alma_Label = New System.Windows.Forms.Label()
        Me.AGPS_TYPE = New System.Windows.Forms.Label()
        Me.MenuStrip1.SuspendLayout()
        Me.Status_Page.SuspendLayout()
        CType(Me.SNRBARPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SatViewPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.StatusControl1.SuspendLayout()
        Me.Others.SuspendLayout()
        CType(Me.MaxMinSNR_PB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InfoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SatelliteNum.SuspendLayout()
        CType(Me.Satellite_PB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MTKTableLayoutPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1110, 24)
        Me.MenuStrip1.TabIndex = 6
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'Status_Page
        '
        Me.Status_Page.BackColor = System.Drawing.Color.LightGray
        Me.Status_Page.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Status_Page.Controls.Add(Me.SNRBARPictureBox)
        Me.Status_Page.Controls.Add(Me.SatViewPictureBox)
        Me.Status_Page.Location = New System.Drawing.Point(4, 22)
        Me.Status_Page.Name = "Status_Page"
        Me.Status_Page.Padding = New System.Windows.Forms.Padding(3)
        Me.Status_Page.Size = New System.Drawing.Size(1090, 524)
        Me.Status_Page.TabIndex = 1
        Me.Status_Page.Text = "Status"
        '
        'SNRBARPictureBox
        '
        Me.SNRBARPictureBox.Location = New System.Drawing.Point(2, 23)
        Me.SNRBARPictureBox.Name = "SNRBARPictureBox"
        Me.SNRBARPictureBox.Size = New System.Drawing.Size(615, 495)
        Me.SNRBARPictureBox.TabIndex = 9
        Me.SNRBARPictureBox.TabStop = False
        '
        'SatViewPictureBox
        '
        Me.SatViewPictureBox.Location = New System.Drawing.Point(619, 23)
        Me.SatViewPictureBox.Name = "SatViewPictureBox"
        Me.SatViewPictureBox.Size = New System.Drawing.Size(468, 495)
        Me.SatViewPictureBox.TabIndex = 8
        Me.SatViewPictureBox.TabStop = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(559, 34)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(424, 39)
        Me.ProgressBar1.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(14, 101)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 15)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Log Path"
        '
        'MTK_Info_CheckBox
        '
        Me.MTK_Info_CheckBox.AutoSize = True
        Me.MTK_Info_CheckBox.Location = New System.Drawing.Point(483, 33)
        Me.MTK_Info_CheckBox.Name = "MTK_Info_CheckBox"
        Me.MTK_Info_CheckBox.Size = New System.Drawing.Size(72, 16)
        Me.MTK_Info_CheckBox.TabIndex = 5
        Me.MTK_Info_CheckBox.Text = "MTK Info"
        Me.MTK_Info_CheckBox.UseVisualStyleBackColor = True
        '
        'Total_data_number
        '
        Me.Total_data_number.AutoSize = True
        Me.Total_data_number.Location = New System.Drawing.Point(1078, 671)
        Me.Total_data_number.Name = "Total_data_number"
        Me.Total_data_number.Size = New System.Drawing.Size(11, 12)
        Me.Total_data_number.TabIndex = 4
        Me.Total_data_number.Text = "1"
        '
        'HScrollBar1
        '
        Me.HScrollBar1.LargeChange = 1
        Me.HScrollBar1.Location = New System.Drawing.Point(0, 683)
        Me.HScrollBar1.Maximum = 0
        Me.HScrollBar1.Name = "HScrollBar1"
        Me.HScrollBar1.Size = New System.Drawing.Size(1110, 27)
        Me.HScrollBar1.TabIndex = 1
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.BackColor = System.Drawing.Color.LightGray
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.06383!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.93617!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 74.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.UTC, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.UTC_data, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Latitude_data, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Longitude_data, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Fixed_T, 3, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 33)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.7037!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.2963!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(428, 64)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'UTC
        '
        Me.UTC.AutoSize = True
        Me.UTC.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UTC.Location = New System.Drawing.Point(3, 0)
        Me.UTC.Name = "UTC"
        Me.UTC.Size = New System.Drawing.Size(34, 15)
        Me.UTC.TabIndex = 0
        Me.UTC.Text = "Time"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 15)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Latitude"
        '
        'UTC_data
        '
        Me.UTC_data.AutoSize = True
        Me.UTC_data.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UTC_data.Location = New System.Drawing.Point(76, 0)
        Me.UTC_data.Name = "UTC_data"
        Me.UTC_data.Size = New System.Drawing.Size(0, 15)
        Me.UTC_data.TabIndex = 3
        '
        'Latitude_data
        '
        Me.Latitude_data.AutoSize = True
        Me.Latitude_data.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Latitude_data.Location = New System.Drawing.Point(76, 23)
        Me.Latitude_data.Name = "Latitude_data"
        Me.Latitude_data.Size = New System.Drawing.Size(0, 15)
        Me.Latitude_data.TabIndex = 4
        '
        'Longitude_data
        '
        Me.Longitude_data.AutoSize = True
        Me.Longitude_data.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Longitude_data.Location = New System.Drawing.Point(76, 42)
        Me.Longitude_data.Name = "Longitude_data"
        Me.Longitude_data.Size = New System.Drawing.Size(0, 15)
        Me.Longitude_data.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(3, 42)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 15)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Longitude"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(238, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 15)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Fixed Type"
        '
        'Fixed_T
        '
        Me.Fixed_T.AutoSize = True
        Me.Fixed_T.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Fixed_T.Location = New System.Drawing.Point(312, 0)
        Me.Fixed_T.Name = "Fixed_T"
        Me.Fixed_T.Size = New System.Drawing.Size(0, 15)
        Me.Fixed_T.TabIndex = 10
        '
        'MTK_CLK_TYPE
        '
        Me.MTK_CLK_TYPE.AutoSize = True
        Me.MTK_CLK_TYPE.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MTK_CLK_TYPE.Location = New System.Drawing.Point(3, 18)
        Me.MTK_CLK_TYPE.Name = "MTK_CLK_TYPE"
        Me.MTK_CLK_TYPE.Size = New System.Drawing.Size(64, 15)
        Me.MTK_CLK_TYPE.TabIndex = 11
        Me.MTK_CLK_TYPE.Text = "Clock Type"
        '
        'CLK_TYPE
        '
        Me.CLK_TYPE.AutoSize = True
        Me.CLK_TYPE.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CLK_TYPE.Location = New System.Drawing.Point(92, 18)
        Me.CLK_TYPE.Name = "CLK_TYPE"
        Me.CLK_TYPE.Size = New System.Drawing.Size(0, 15)
        Me.CLK_TYPE.TabIndex = 12
        '
        'StatusControl1
        '
        Me.StatusControl1.Controls.Add(Me.Status_Page)
        Me.StatusControl1.Controls.Add(Me.Others)
        Me.StatusControl1.Controls.Add(Me.SatelliteNum)
        Me.StatusControl1.Location = New System.Drawing.Point(12, 118)
        Me.StatusControl1.Name = "StatusControl1"
        Me.StatusControl1.SelectedIndex = 0
        Me.StatusControl1.Size = New System.Drawing.Size(1098, 550)
        Me.StatusControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.StatusControl1.TabIndex = 5
        '
        'Others
        '
        Me.Others.BackColor = System.Drawing.Color.LightGray
        Me.Others.Controls.Add(Me.MaxMinSNR_PB)
        Me.Others.Controls.Add(Me.SNRCheckBox)
        Me.Others.Controls.Add(Me.InfoPictureBox)
        Me.Others.Controls.Add(Me.V_Value)
        Me.Others.Controls.Add(Me.H_Value)
        Me.Others.Controls.Add(Me.P_Value)
        Me.Others.Controls.Add(Me.Label7)
        Me.Others.Controls.Add(Me.VDOPCB)
        Me.Others.Controls.Add(Me.HDOPCB)
        Me.Others.Controls.Add(Me.PDOPCB)
        Me.Others.ForeColor = System.Drawing.Color.Green
        Me.Others.Location = New System.Drawing.Point(4, 22)
        Me.Others.Name = "Others"
        Me.Others.Size = New System.Drawing.Size(1090, 524)
        Me.Others.TabIndex = 2
        Me.Others.Text = "Others"
        '
        'MaxMinSNR_PB
        '
        Me.MaxMinSNR_PB.Location = New System.Drawing.Point(3, 49)
        Me.MaxMinSNR_PB.Name = "MaxMinSNR_PB"
        Me.MaxMinSNR_PB.Size = New System.Drawing.Size(1087, 472)
        Me.MaxMinSNR_PB.TabIndex = 12
        Me.MaxMinSNR_PB.TabStop = False
        '
        'SNRCheckBox
        '
        Me.SNRCheckBox.AutoSize = True
        Me.SNRCheckBox.BackColor = System.Drawing.Color.Transparent
        Me.SNRCheckBox.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SNRCheckBox.ForeColor = System.Drawing.Color.Black
        Me.SNRCheckBox.Location = New System.Drawing.Point(487, 13)
        Me.SNRCheckBox.Name = "SNRCheckBox"
        Me.SNRCheckBox.Size = New System.Drawing.Size(72, 19)
        Me.SNRCheckBox.TabIndex = 11
        Me.SNRCheckBox.Text = "SNR Info"
        Me.SNRCheckBox.UseVisualStyleBackColor = False
        '
        'InfoPictureBox
        '
        Me.InfoPictureBox.Location = New System.Drawing.Point(563, 8)
        Me.InfoPictureBox.Name = "InfoPictureBox"
        Me.InfoPictureBox.Size = New System.Drawing.Size(170, 33)
        Me.InfoPictureBox.TabIndex = 9
        Me.InfoPictureBox.TabStop = False
        '
        'V_Value
        '
        Me.V_Value.AutoSize = True
        Me.V_Value.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.V_Value.ForeColor = System.Drawing.Color.Black
        Me.V_Value.Location = New System.Drawing.Point(452, 14)
        Me.V_Value.Name = "V_Value"
        Me.V_Value.Size = New System.Drawing.Size(0, 15)
        Me.V_Value.TabIndex = 10
        '
        'H_Value
        '
        Me.H_Value.AutoSize = True
        Me.H_Value.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.H_Value.ForeColor = System.Drawing.Color.Blue
        Me.H_Value.Location = New System.Drawing.Point(307, 14)
        Me.H_Value.Name = "H_Value"
        Me.H_Value.Size = New System.Drawing.Size(0, 15)
        Me.H_Value.TabIndex = 9
        '
        'P_Value
        '
        Me.P_Value.AutoSize = True
        Me.P_Value.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.P_Value.ForeColor = System.Drawing.Color.Red
        Me.P_Value.Location = New System.Drawing.Point(141, 14)
        Me.P_Value.Name = "P_Value"
        Me.P_Value.Size = New System.Drawing.Size(0, 15)
        Me.P_Value.TabIndex = 8
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(2, 28)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 15)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Fixed Type"
        '
        'VDOPCB
        '
        Me.VDOPCB.AutoSize = True
        Me.VDOPCB.BackColor = System.Drawing.Color.Transparent
        Me.VDOPCB.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VDOPCB.ForeColor = System.Drawing.Color.Black
        Me.VDOPCB.Location = New System.Drawing.Point(382, 13)
        Me.VDOPCB.Name = "VDOPCB"
        Me.VDOPCB.Size = New System.Drawing.Size(58, 19)
        Me.VDOPCB.TabIndex = 3
        Me.VDOPCB.Text = "VDOP"
        Me.VDOPCB.UseVisualStyleBackColor = False
        '
        'HDOPCB
        '
        Me.HDOPCB.AutoSize = True
        Me.HDOPCB.BackColor = System.Drawing.Color.Transparent
        Me.HDOPCB.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HDOPCB.ForeColor = System.Drawing.Color.Blue
        Me.HDOPCB.Location = New System.Drawing.Point(243, 13)
        Me.HDOPCB.Name = "HDOPCB"
        Me.HDOPCB.Size = New System.Drawing.Size(58, 19)
        Me.HDOPCB.TabIndex = 2
        Me.HDOPCB.Text = "HDOP"
        Me.HDOPCB.UseVisualStyleBackColor = False
        '
        'PDOPCB
        '
        Me.PDOPCB.AutoSize = True
        Me.PDOPCB.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PDOPCB.ForeColor = System.Drawing.Color.Red
        Me.PDOPCB.Location = New System.Drawing.Point(78, 13)
        Me.PDOPCB.Name = "PDOPCB"
        Me.PDOPCB.Size = New System.Drawing.Size(57, 19)
        Me.PDOPCB.TabIndex = 1
        Me.PDOPCB.Text = "PDOP"
        Me.PDOPCB.UseVisualStyleBackColor = True
        '
        'SatelliteNum
        '
        Me.SatelliteNum.BackColor = System.Drawing.Color.LightGray
        Me.SatelliteNum.Controls.Add(Me.Satellite_PB)
        Me.SatelliteNum.Controls.Add(Me.Label9)
        Me.SatelliteNum.Controls.Add(Me.Label8)
        Me.SatelliteNum.Location = New System.Drawing.Point(4, 22)
        Me.SatelliteNum.Name = "SatelliteNum"
        Me.SatelliteNum.Size = New System.Drawing.Size(1090, 524)
        Me.SatelliteNum.TabIndex = 3
        Me.SatelliteNum.Text = "Satellite"
        '
        'Satellite_PB
        '
        Me.Satellite_PB.Location = New System.Drawing.Point(3, 43)
        Me.Satellite_PB.Name = "Satellite_PB"
        Me.Satellite_PB.Size = New System.Drawing.Size(1079, 475)
        Me.Satellite_PB.TabIndex = 9
        Me.Satellite_PB.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(5, 25)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 15)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Total Sat# used"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(4, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(103, 15)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Total Sat# in View"
        '
        'current_data_index
        '
        Me.current_data_index.AutoSize = True
        Me.current_data_index.Location = New System.Drawing.Point(31, 670)
        Me.current_data_index.Name = "current_data_index"
        Me.current_data_index.Size = New System.Drawing.Size(11, 12)
        Me.current_data_index.TabIndex = 8
        Me.current_data_index.Text = "0"
        '
        'MTKTableLayoutPanel
        '
        Me.MTKTableLayoutPanel.ColumnCount = 2
        Me.MTKTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.93023!))
        Me.MTKTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.06977!))
        Me.MTKTableLayoutPanel.Controls.Add(Me.Label4, 0, 3)
        Me.MTKTableLayoutPanel.Controls.Add(Me.AccuracyLabel, 0, 0)
        Me.MTKTableLayoutPanel.Controls.Add(Me.ACLabel, 1, 0)
        Me.MTKTableLayoutPanel.Controls.Add(Me.MTK_CLK_TYPE, 0, 1)
        Me.MTKTableLayoutPanel.Controls.Add(Me.CLK_TYPE, 1, 1)
        Me.MTKTableLayoutPanel.Controls.Add(Me.AGPSLabel, 0, 2)
        Me.MTKTableLayoutPanel.Controls.Add(Me.Alma_Label, 1, 3)
        Me.MTKTableLayoutPanel.Controls.Add(Me.AGPS_TYPE, 1, 2)
        Me.MTKTableLayoutPanel.Location = New System.Drawing.Point(559, 37)
        Me.MTKTableLayoutPanel.Name = "MTKTableLayoutPanel"
        Me.MTKTableLayoutPanel.RowCount = 4
        Me.MTKTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.94118!))
        Me.MTKTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.05882!))
        Me.MTKTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14.0!))
        Me.MTKTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15.0!))
        Me.MTKTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.MTKTableLayoutPanel.Size = New System.Drawing.Size(430, 64)
        Me.MTKTableLayoutPanel.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 48)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 15)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Almanac data"
        '
        'AccuracyLabel
        '
        Me.AccuracyLabel.AutoSize = True
        Me.AccuracyLabel.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AccuracyLabel.Location = New System.Drawing.Point(3, 0)
        Me.AccuracyLabel.Name = "AccuracyLabel"
        Me.AccuracyLabel.Size = New System.Drawing.Size(54, 15)
        Me.AccuracyLabel.TabIndex = 12
        Me.AccuracyLabel.Text = "Accuracy"
        '
        'ACLabel
        '
        Me.ACLabel.AutoSize = True
        Me.ACLabel.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ACLabel.Location = New System.Drawing.Point(92, 0)
        Me.ACLabel.Name = "ACLabel"
        Me.ACLabel.Size = New System.Drawing.Size(0, 15)
        Me.ACLabel.TabIndex = 13
        '
        'AGPSLabel
        '
        Me.AGPSLabel.AutoSize = True
        Me.AGPSLabel.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AGPSLabel.Location = New System.Drawing.Point(3, 34)
        Me.AGPSLabel.Name = "AGPSLabel"
        Me.AGPSLabel.Size = New System.Drawing.Size(71, 14)
        Me.AGPSLabel.TabIndex = 14
        Me.AGPSLabel.Text = "AGPS Mode"
        '
        'Alma_Label
        '
        Me.Alma_Label.AutoSize = True
        Me.Alma_Label.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Alma_Label.Location = New System.Drawing.Point(92, 48)
        Me.Alma_Label.Name = "Alma_Label"
        Me.Alma_Label.Size = New System.Drawing.Size(0, 15)
        Me.Alma_Label.TabIndex = 16
        '
        'AGPS_TYPE
        '
        Me.AGPS_TYPE.AutoSize = True
        Me.AGPS_TYPE.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AGPS_TYPE.Location = New System.Drawing.Point(92, 34)
        Me.AGPS_TYPE.Name = "AGPS_TYPE"
        Me.AGPS_TYPE.Size = New System.Drawing.Size(0, 14)
        Me.AGPS_TYPE.TabIndex = 17
        '
        'Main_Form
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1110, 717)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.MTKTableLayoutPanel)
        Me.Controls.Add(Me.current_data_index)
        Me.Controls.Add(Me.Total_data_number)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.HScrollBar1)
        Me.Controls.Add(Me.MTK_Info_CheckBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.StatusControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "Main_Form"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "CEI GPS Parser"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Status_Page.ResumeLayout(False)
        CType(Me.SNRBARPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SatViewPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.StatusControl1.ResumeLayout(False)
        Me.Others.ResumeLayout(False)
        Me.Others.PerformLayout()
        CType(Me.MaxMinSNR_PB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InfoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SatelliteNum.ResumeLayout(False)
        Me.SatelliteNum.PerformLayout()
        CType(Me.Satellite_PB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MTKTableLayoutPanel.ResumeLayout(False)
        Me.MTKTableLayoutPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Status_Page As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents MTK_Info_CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Total_data_number As System.Windows.Forms.Label
    Friend WithEvents HScrollBar1 As System.Windows.Forms.HScrollBar
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents UTC As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents UTC_data As System.Windows.Forms.Label
    Friend WithEvents Latitude_data As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Fixed_T As System.Windows.Forms.Label
    Friend WithEvents MTK_CLK_TYPE As System.Windows.Forms.Label
    Friend WithEvents Longitude_data As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents StatusControl1 As System.Windows.Forms.TabControl
    Friend WithEvents CLK_TYPE As System.Windows.Forms.Label
    Friend WithEvents Others As System.Windows.Forms.TabPage
    Friend WithEvents VDOPCB As System.Windows.Forms.CheckBox
    Friend WithEvents HDOPCB As System.Windows.Forms.CheckBox
    Friend WithEvents PDOPCB As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents P_Value As System.Windows.Forms.Label
    Friend WithEvents H_Value As System.Windows.Forms.Label
    Friend WithEvents V_Value As System.Windows.Forms.Label
    Friend WithEvents current_data_index As System.Windows.Forms.Label
    Friend WithEvents SatelliteNum As System.Windows.Forms.TabPage
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents SNRCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents InfoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents SatViewPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents SNRBARPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents MaxMinSNR_PB As System.Windows.Forms.PictureBox
    Friend WithEvents Satellite_PB As System.Windows.Forms.PictureBox
    Friend WithEvents MTKTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents AccuracyLabel As System.Windows.Forms.Label
    Friend WithEvents ACLabel As System.Windows.Forms.Label
    Friend WithEvents AGPSLabel As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Alma_Label As System.Windows.Forms.Label
    Friend WithEvents AGPS_TYPE As System.Windows.Forms.Label

End Class
