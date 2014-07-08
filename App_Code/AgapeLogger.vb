Imports Microsoft.VisualBasic
Imports DotNetNuke.Instrumentation

Public Class AgapeLogger

    Public Shared Sub [Error](ByVal UserID As Integer, ByVal Msg As String)

        ' Write to Log4net appenders
        DnnLog.Error(Msg)

        ' Write to DNN EventLog
        WriteEventLog(UserID, Msg)

    End Sub

    Public Shared Sub Warn(ByVal UserID As Integer, ByVal Msg As String)

        ' Write to Log4net appenders
        DnnLog.Warn(Msg)

        ' Write to DNN EventLog
        WriteEventLog(UserID, Msg)

    End Sub

    Public Shared Sub Info(ByVal UserID As Integer, ByVal Msg As String)

        ' Write to Log4net appenders
        DnnLog.Info(Msg)

        ' Write to DNN EventLog
        WriteEventLog(UserID, Msg)

    End Sub

    Public Shared Sub Debug(ByVal UserID As Integer, ByVal Msg As String)

        ' Write to Log4net appenders
        DnnLog.Debug(Msg)

        ' Write to DNN EventLog
        'WriteEventLog(UserID, Msg)

    End Sub

    ' Write to DNN EventLog
    Public Shared Sub WriteEventLog(ByVal UserID As Integer, ByVal Msg As String)
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim objEventLog As New DotNetNuke.Services.Log.EventLog.EventLogController
        objEventLog.AddLog("Message", Msg, PS, UserID, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
    End Sub

End Class

