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


            If Request.QueryString("StaffId") <> "" Then
                Response.Redirect(EditUrl("staffDashboard") & "?staffId=" & Request.QueryString("StaffId"))
            End If

            Dim d As New MPDDataContext
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

            For Each thisCountry In d.AP_mpd_Countries
                Dim totalCount = thisCountry.VeryLowCount + thisCountry.LowCount + thisCountry.HighCount + thisCountry.FullCount + thisCountry.NoBudgetCount

                jsonMap &= "['" & thisCountry.isoCode & "', " & (thisCountry.EstAvgSupport12.Value * 100).ToString("0.0") & "],"

                Dim withBud = 1.0 - (thisCountry.NoBudgetCount / totalCount)


                'dt.Rows.Add(thisCountry.name, thisCountry.AvgSupport12.Value.ToString("0.0%"), thisCountry.AvgSupport3.Value.ToString("0.0%"), thisCountry.AvgSupport1.Value.ToString("0.0%"), withBud.ToString("0.0%"), thisCountry.BudgetAccuracy.Value.ToString("0.0%"), _
                '           (thisCountry.VeryLowCount / totalCount).ToString("0%"), (thisCountry.LowCount / totalCount).ToString("0%"), (thisCountry.HighCount / totalCount).ToString("0%"), (thisCountry.FullCount / totalCount).ToString("0%"), thisCountry.BudgetAccuracy.Value.ToString("0%"), thisCountry.SplitLocal.Value.ToString("0%"), thisCountry.isoCode)
                dt.Rows.Add(thisCountry.name, thisCountry.EstAvgSupport12.Value.ToString("0.0%"), thisCountry.EstAvgSupport3.Value.ToString("0.0%"), thisCountry.EstAvgSupport1.Value.ToString("0.0%"), withBud.ToString("0.0%"), thisCountry.BudgetAccuracy.Value.ToString("0.0%"), _
                        (thisCountry.EstVeryLowCount / totalCount).ToString("0%"), (thisCountry.EstLowCount / totalCount).ToString("0%"), (thisCountry.EstHighCount / totalCount).ToString("0%"), (thisCountry.EstFullCount / totalCount).ToString("0%"), thisCountry.BudgetAccuracy.Value.ToString("0%"), thisCountry.SplitLocal.Value.ToString("0%"), thisCountry.isoCode)


            Next

            rpCountriesSummaryData.DataSource = dt
            rpCountriesSummaryData.DataBind()

        End Sub


    End Class
End Namespace
