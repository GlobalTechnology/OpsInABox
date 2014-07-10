Imports Microsoft.VisualBasic
Imports GR_NET

Public Class gr_sync
    Private _gr As GR
    Private _PortalId As Integer
    Public Sub New(ByVal PortalId As Integer)
        _PortalId = PortalId
        Dim gr As New GR(StaffBrokerFunctions.GetSetting("gr_api_key", PortalId), StaffBrokerFunctions.GetSetting("gr_api_url", PortalId), False)


    End Sub


    

    Public Sub UpdatePersonFromGR(ByVal gr_id As String)

    End Sub


End Class
