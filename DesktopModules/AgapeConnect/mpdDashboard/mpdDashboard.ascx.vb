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

Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class mpdDashboard
        Inherits Entities.Modules.PortalModuleBase


        Public jsonMap As String = ""


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then



                Dim ssoGuid = UserInfo.Profile.GetPropertyValue("ssoGUID")


                Dim d As New MPDDataContext
                'ssoGuid = "126AA989-238B-2EBD-5BE2-187DA7EDE3B7"  'Beni
                ' ssoGuid = "109764C9-CD24-CF94-839C-65F41C9C2E5C"  ' Eric
                'ssoGuid = "C17C80EC-D8C5-484C-0A43-4CE345ADFADF"  ' Tomas
                'ssoGuid = "1FF92F95-DD56-AFCA-9489-FC6E8F253237" ' Goce
                'ssoGuid = "1FF92F95-DD56-AFCA-9489-FC6E8F253238" ' Unknown
                'ssoGuid = "3925839A-F828-4087-8223-816DE32A7BAF" 'Chontelle    
         

                If String.IsNullOrEmpty(ssoGuid) Then
                    'TODO display error message
                    pnlError.Visible = True
                    pnlMain.Visible = False
                    Return
                End If
                mpdDashboardMenu.CountryURL = EditUrl("countryDashboard")
                mpdDashboardMenu.StaffUrl = EditUrl("staffDashboard")

                mpdDashboardMenu.ssoGuid = ssoGuid
              

                'look for area admin status
                Dim areas_admins = (From c In d.AP_mpd_AreaAdmins Where c.sso_guid = ssoGuid Select c.area).ToArray
                Dim countries = (From c In d.AP_mpd_Countries Where areas_admins.Contains(c.Area) Or areas_admins.Contains("GLBL") Or c.AP_MPD_CountryAdmins.Where(Function(b) b.sso_guid = ssoGuid).Count > 0)

                'If areas_admins.Contains("GLBL") Then
                '    'global instance
                '    countries = d.AP_mpd_Countries.ToList

                'Else



                '    countries = From c In d.AP_MPD_CountryAdmins Where c.sso_guid = ssoGuid Select c.AP_mpd_Country Distinct



                'End If

                If countries.Count = 1 Then
                    Response.Redirect(EditUrl("countryDashboard") & "?country=" & countries.First.isoCode)
                ElseIf countries.Count = 0 Then
                    Dim users = From c In d.Ap_mpd_Users Where c.Key_GUID = ssoGuid Order By c.ForeignIncome12 + c.LocalIncome12 + c.SubsidyIncome12 Descending

                    If users.Count > 0 Then
                        'need to find the right country!
                        'lets take the one with the largest income (including foreign)
                        Response.Redirect(EditUrl("staffDashboard") & "?mpd_user_id=" & users.First.AP_mpd_UserId & "&country=" & users.First.AP_mpd_Country.isoCode)
                    Else
                        lblError.Text = "We could not find any users or countries with MPD data that you have permission to view."
                        pnlError.Visible = True
                        pnlMain.Visible = False
                        Return
                    End If
                End If


                If Request.QueryString("StaffId") <> "" Then
                    Response.Redirect(EditUrl("staffDashboard") & "?staffId=" & Request.QueryString("StaffId"))
                End If


                jsonMap = ""
                Dim tableData As New ArrayList

                Dim dt As New DataTable()
                dt.Columns.Add("Name")
                dt.Columns.Add("Year")
                dt.Columns.Add("Quarter")
                dt.Columns.Add("Month")
                dt.Columns.Add("Budget")
                dt.Columns.Add("Accuracy")
                dt.Columns.Add("less50")
                dt.Columns.Add("from50to80")
                dt.Columns.Add("from80to100")
                dt.Columns.Add("more100")
                dt.Columns.Add("BudgetSpent")
                dt.Columns.Add("Local")

                dt.Columns.Add("ISO")

                For Each thisCountry In countries.OrderBy(Function(c) c.name)
                    Try


                        Dim totalCount = thisCountry.VeryLowCount + thisCountry.LowCount + thisCountry.HighCount + thisCountry.FullCount + thisCountry.NoBudgetCount

                        jsonMap &= "['" & thisCountry.isoCode & "', " & (thisCountry.EstAvgSupport12.Value * 100).ToString("0") & "],"

                        Dim withBud = 1.0 - (thisCountry.NoBudgetCount / totalCount)


                        'dt.Rows.Add(thisCountry.name, thisCountry.AvgSupport12.Value.ToString("0.0%"), thisCountry.AvgSupport3.Value.ToString("0.0%"), thisCountry.AvgSupport1.Value.ToString("0.0%"), withBud.ToString("0.0%"), thisCountry.BudgetAccuracy.Value.ToString("0.0%"), _
                        '           (thisCountry.VeryLowCount / totalCount).ToString("0%"), (thisCountry.LowCount / totalCount).ToString("0%"), (thisCountry.HighCount / totalCount).ToString("0%"), (thisCountry.FullCount / totalCount).ToString("0%"), thisCountry.BudgetAccuracy.Value.ToString("0%"), thisCountry.SplitLocal.Value.ToString("0%"), thisCountry.isoCode)
                        dt.Rows.Add(thisCountry.name, thisCountry.EstAvgSupport12.Value.ToString("0%"), thisCountry.EstAvgSupport3.Value.ToString("0%"), thisCountry.EstAvgSupport1.Value.ToString("0%"), withBud.ToString("0%"), thisCountry.BudgetAccuracy.Value.ToString("0%"), _
                                (thisCountry.EstVeryLowCount / totalCount).ToString("0%"), (thisCountry.EstLowCount / totalCount).ToString("0%"), (thisCountry.EstHighCount / totalCount).ToString("0%"), (thisCountry.EstFullCount / totalCount).ToString("0%"), thisCountry.BudgetAccuracy.Value.ToString("0%"), thisCountry.SplitLocal.Value.ToString("0%"), thisCountry.isoCode)
                    Catch ex As Exception

                    End Try

                Next

                rpCountriesSummaryData.DataSource = dt
                rpCountriesSummaryData.DataBind()
            End If
        End Sub


        
    End Class
End Namespace
