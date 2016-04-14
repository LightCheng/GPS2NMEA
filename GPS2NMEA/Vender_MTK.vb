Public Class Vender_MTK
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
End Class
