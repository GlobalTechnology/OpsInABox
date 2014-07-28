Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports DotNetNuke
Imports DotNetNuke.Security
Imports MPD
Imports GR_NET
Imports StaffBrokerFunctions
Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class GlobalStatusDashboard
        Inherits Entities.Modules.PortalModuleBase
        Dim d As New MPDDataContext
        Const greenLight = "/DesktopModules/AgapeConnect/GlobalStatusDashboard/green_light.png"
        Const yellowLight = "/DesktopModules/AgapeConnect/GlobalStatusDashboard/yellow_light.png"
        Const redLight = "/DesktopModules/AgapeConnect/GlobalStatusDashboard/red_light.png"
        Const greyLight = "/DesktopModules/AgapeConnect/GlobalStatusDashboard/grey_light.png"

        Const globalops = ""

        Dim membership_sites As String() = {"4d45a710-f634-11e3-8b5a-12768b82bfd5", "aed6627c-10e6-11e4-83ed-12543788cf06"}
        Dim countries As IQueryable(Of ministry_system)
        Public json_area As String = ""
        Protected Function CheckPermissions() As Boolean

            Dim rc As New DotNetNuke.Security.Roles.RoleController()
            Dim theRole = rc.GetRoleByName(PortalId, "Global Leaders")

            Dim gr As New GR(GetSetting("gr_root_key", PortalId), "https://api.global-registry.org", False)
          
            Dim tmp = gr.GetEntities("person", "&filters[authentication][key_guid]=" & UserInfo.Profile.GetPropertyValue("ssoGUID") & "&filters[owned_by]=all")
            AgapeLogger.WriteEventLog(UserId, "guid:" & UserInfo.Profile.GetPropertyValue("ssoGUID"))
            If tmp.Count > 0 Then
                AgapeLogger.WriteEventLog(UserId, tmp.First.ToJson)
                If (tmp.First.collections.ContainsKey("gcx_site:relationship")) Then


                    If tmp.First.collections("gcx_site:relationship").Exists(Function(c) membership_sites.Contains(c.GetPropertyValue("gcx_site"))) Then

                        rc.AddUserRole(PortalId, UserId, theRole.RoleID, DateTime.MaxValue)
                        StaffBrokerFunctions.SetUserProfileProperty(PortalId, UserId, "gr_person_id", tmp.First.ID)
                        UserController.UpdateUser(PortalId, UserInfo)
                        rc.UpdateRole(theRole)
                        Return True
                    Else
                        Return False
                    End If
                    Return False
                End If
            End If
            Return False
        End Function

        'Protected Sub BulkAddPermission()
        '    Dim gr As New GR("6d50408d0d92ff8090c0a0a71f8edf7b5069858b04ba57d5d643121d851e", "https://api.global-registry.org")
        '    Dim tmp = gr.GetEntities("person", "&entity_type=person&filters[owned_by]=all&filters[gcx_site:relationship][name]=Global Team", 0, 500, 0)
        '    For Each row In tmp
        '        Dim rc As New DotNetNuke.Security.Roles.RoleController()

        '        StaffBrokerFunctions.CreateUser(PortalId, row.GetPropertyValue("email_address"))

        '        rc.AddUserRole(PortalId, UserId, theRole.RoleID, DateTime.MaxValue)
        '        StaffBrokerFunctions.SetUserProfileProperty(PortalId, UserId, "gr_person_id", tmp.First.ID)
        '        UserController.UpdateUser(PortalId, UserInfo)
        '        rc.UpdateRole(theRole)
        '    Next


        'End Sub
        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            'Dim rc As New DotNetNuke.Security.Roles.RoleController()

            If Not UserInfo.Roles.Contains("Global Leaders") Then
                If Not CheckPermissions() Then
                    pnlMain.Visible = False
                    lblDeny.Visible = True
                    Return
                End If


            End If


            countries = d.ministry_systems.Where(Function(c) c.is_active)



            Dim areas = From c In countries Select c.area_code, c.area_name Distinct






            rpAreas.DataSource = areas.OrderBy(Function(c) c.area_name)
            rpAreas.DataBind()

            

        End Sub
        Public Function getCountriesForArea(ByVal area_code As String, ByVal area_name As String) As Object
            'If area_code = "AAOP" Or area_code = "PACT" Then
            '    Return Nothing
            'End If
            Dim ac = (From c In countries Where c.area_code = area_code Select c.fcx, c.stage, c.is_active, c.min_name, c.min_logo, min_scope = c.ministry_scope.Replace(" ", "-"), donationImg = getDonationImage(c.last_dataserver_donation), gmaImage = getGmaImage(c.gma_status), finRepImage = getFinRepIMage(c.last_fin_rep), donationMessage = GetDonationMessage(c.last_dataserver_donation), finRepMessage = GetFinRepMessage(c.last_fin_rep), gmaMessage = getGmaMessage(c.gma_status) Order By min_name).ToList
            Dim don_perc = 100 * ac.Where(Function(c) (c.stage > 0 Or c.min_scope = "Area") And c.donationImg.Contains("green")).Count / ac.Where(Function(c) (c.stage > 0 Or c.min_scope = "Area")).Count
            Dim gma_perc = 100 * ac.Where(Function(c) (c.stage > 0) And c.gmaImage.Contains("green")).Count / ac.Where(Function(c) (c.stage > 0)).Count
            Dim fin_perc = 100 * ac.Where(Function(c) (c.stage > 0 Or c.min_scope = "Area") And c.finRepImage.Contains("green")).Count / ac.Where(Function(c) (c.stage > 0 Or c.min_scope = "Area")).Count

            json_area &= "['" & area_name & "', " & don_perc.ToString("0.0") & ", " & gma_perc.ToString("0.0") & ", " & fin_perc.ToString("0.0") & ", '" & area_code & "'],"
            Return ac
        End Function

        Private Function GetDonationMessage(ByVal dt As Date?) As String
            If dt Is Nothing Then
                Return "Not Implemented - No donations have been received by tntDataserver for this country."

            End If
            Dim diff_day = DateDiff(DateInterval.Day, CDate(dt), Today)
            If diff_day <= 1 Then
                Return "last donation was " & diff_day & " day ago"
            ElseIf diff_day < 60 Then
                Return "last donation was " & diff_day & " days ago"

            End If
            Dim diff_month = DateDiff(DateInterval.Month, CDate(dt), Today)

            Return "last donation was " & diff_month & " months ago"
        End Function
        Private Function GetFinRepMessage(ByVal dt As Date?) As String
            If dt Is Nothing Then
                Return "No finanical reports have been received from this country"

            End If
            Dim nowPeriod = New Date(Today.Year, Today.Month, 1)
            Dim diff = DateDiff(DateInterval.Month, CDate(dt), nowPeriod)
            Return dt.ToString & ":" & diff
            If diff <= 1 Then
                Return "last report was " & diff & " month ago"
            Else
                Return "last report was " & diff & " months ago"

            End If
            
        End Function
        Private Function getGmaMessage(ByVal status As Integer?) As String
            If status Is Nothing Then
                Return "Not Implemented"
            End If
            Select Case status
                Case 0 : Return "Not Implemented"
                Case 1 : Return "Implemented - but not reporting"
                Case 2 : Return "Reporting"
                Case Else
                    Return "Not Implemented"

            End Select
        End Function

        Private Function getDonationImage(ByVal dt As Date?) As String
            If dt Is Nothing Then
                Return greyLight
            End If
            Dim diff = DateDiff(DateInterval.Day, CDate(dt), Today)
            If diff < 7 Then
                Return greenLight
            ElseIf diff < 21 Then
                Return yellowLight
            Else
                Return redLight
            End If
        End Function
        Private Function getGmaImage(ByVal status As Integer?) As String
            If status Is Nothing Then
                Return greyLight
            End If
            Select Case status
                Case 0 : Return greyLight
                Case 1 : Return redLight
                Case 2 : Return greenLight
                Case Else
                    Return greyLight

            End Select
        End Function

        Private Function getFinRepIMage(ByVal dt As Date?) As String
            If dt Is Nothing Then
                Return greyLight
            End If
            Dim nowPeriod = New Date(Today.Year, Today.Month, 1)
            Dim diff = DateDiff(DateInterval.Month, CDate(dt), nowPeriod)
            If diff <= 2 Then
                Return greenLight
            ElseIf diff <= 4 Then
                Return yellowLight
            Else
                Return redLight
            End If
        End Function
    End Class
End Namespace
