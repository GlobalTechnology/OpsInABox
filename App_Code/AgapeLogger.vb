Imports Microsoft.VisualBasic
Imports DotNetNuke.Instrumentation

Public Class AgapeLogger

    Private Shared ReadOnly _logger As ILog = LoggerSource.Instance.GetLogger(GetType(AgapeLogger))

    Public Shared Sub [Error](ByVal UserID As Integer, ByVal Msg As String)

        ' Write to Log4net appenders
        _logger.Error(Msg)

        ' Write to DNN EventLog
        WriteEventLog(UserID, Msg)

    End Sub

    Public Shared Sub Warn(ByVal UserID As Integer, ByVal Msg As String)

        ' Write to Log4net appenders
        _logger.Warn(Msg)

        ' Write to DNN EventLog
        WriteEventLog(UserID, Msg)

    End Sub

    Public Shared Sub Info(ByVal UserID As Integer, ByVal Msg As String)

        ' Write to Log4net appenders
        If _logger.IsInfoEnabled Then
            _logger.Info(Msg)
        End If

        ' Write to DNN EventLog
        WriteEventLog(UserID, Msg)

    End Sub

    Public Shared Sub Debug(ByVal UserID As Integer, ByVal Msg As String)

        ' Write to Log4net appenders
        If _logger.IsDebugEnabled Then
            _logger.Debug(Msg)
        End If

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

