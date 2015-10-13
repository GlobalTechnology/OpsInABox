Imports DotNetNuke
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Math
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Collections.Specialized
Imports System.Xml.Linq
Imports System.Linq

Namespace DotNetNuke.Modules.Portals



    Partial Class Overview
        Inherits Entities.Modules.PortalModuleBase




        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            Dim pc As New DotNetNuke.Entities.Portals.PortalController
            If rblSort.SelectedValue = "PortalId" Then
                gvPortals.DataSource = From c As PortalInfo In pc.GetPortals() Order By c.PortalID


            ElseIf rblSort.SelectedValue = "PortalName" Then
                gvPortals.DataSource = From c As PortalInfo In pc.GetPortals() Order By c.PortalName

            End If
            gvPortals.DataBind()

        End Sub
        Public Function GetAliases(ByVal thePortalId As Integer) As String
            Dim PS As New PortalSettings(thePortalId)
            Return PS.DefaultPortalAlias
        End Function
        Public Function IsUsingRmb(ByVal thePortalId As Integer) As Boolean
            Dim d As New StaffRmb.StaffRmbDataContext
            Dim isRmb As Boolean = (From c In d.AP_Staff_Rmbs Where c.PortalId = thePortalId And c.Status = StaffRmb.RmbStatus.Processed).Count > 2

            Dim isAdv As Boolean = (From c In d.AP_Staff_AdvanceRequests Where c.PortalId = thePortalId And c.RequestStatus = StaffRmb.RmbStatus.Processed).Count > 2
            Return isRmb Or isAdv

        End Function
        Public Function RmbsEver(ByVal thePortalId As Integer) As Integer
            Dim d As New StaffRmb.StaffRmbDataContext
            Dim rmbs = (From c In d.AP_Staff_Rmbs Where c.PortalId = thePortalId And c.Status = StaffRmb.RmbStatus.Processed).Count

            Dim adv = (From c In d.AP_Staff_AdvanceRequests Where c.PortalId = thePortalId And c.RequestStatus = StaffRmb.RmbStatus.Processed).Count

            Return rmbs + adv

        End Function

        Public Function RmbsMonth(ByVal thePortalId As Integer) As Integer
            Dim d As New StaffRmb.StaffRmbDataContext
            Dim rmbs = (From c In d.AP_Staff_Rmbs Where c.PortalId = thePortalId And c.Status = StaffRmb.RmbStatus.Processed And c.ProcDate > Today.AddDays(-40)).Count

            Dim adv = (From c In d.AP_Staff_AdvanceRequests Where c.PortalId = thePortalId And c.RequestStatus = StaffRmb.RmbStatus.Processed And c.ProcessedDate > Today.AddDays(-40)).Count

            Return rmbs + adv

        End Function
        Public Function RmbTrans(ByVal thePortalId As Integer) As Integer
            Dim d As New StaffRmb.StaffRmbDataContext
            Dim rmbs = (From c In d.AP_Staff_RmbLines Where c.AP_Staff_Rmb.PortalId = thePortalId And c.AP_Staff_Rmb.Status = StaffRmb.RmbStatus.Processed).Count

            Dim adv = (From c In d.AP_Staff_AdvanceRequests Where c.PortalId = thePortalId And c.RequestStatus = StaffRmb.RmbStatus.Processed).Count

            Return rmbs + adv

        End Function
        Public Function GRLink(ByVal thePortalId As Integer) As Boolean
            Return StaffBrokerFunctions.GetSetting("gr_api_key", thePortalId) <> ""
          

        End Function
        Public Function LastProcDate(ByVal thePortalId As Integer) As String
            Dim d As New StaffRmb.StaffRmbDataContext
            Dim lastRmb = (From c In d.AP_Staff_Rmbs Where c.PortalId = thePortalId And c.Status = StaffRmb.RmbStatus.Processed Select c.ProcDate Order By ProcDate Descending).FirstOrDefault
            '  Dim lastRmb As Date = Nothing

            '  If procRmb.Count > 0 Then
            ' lastRmb = procRmb.First
            ' End If

            Dim lastAdv = (From c In d.AP_Staff_AdvanceRequests Where c.PortalId = thePortalId And c.RequestStatus = StaffRmb.RmbStatus.Processed Select c.ProcessedDate Order By ProcessedDate Descending).FirstOrDefault
            ' Dim lastAdv As Date = Nothing

            '  If procAdv.Count > 0 Then
            ' ' lastAdv = procRmb.First
            ' End If

            If lastAdv Is Nothing And lastRmb Is Nothing Then
                Return ""
            End If
            If lastRmb Is Nothing Then
                Return lastAdv
            End If
            If lastAdv Is Nothing Then
                Return lastRmb
            End If

            Return IIf(lastRmb > lastAdv, lastRmb, lastAdv)



        End Function
    End Class
End Namespace
