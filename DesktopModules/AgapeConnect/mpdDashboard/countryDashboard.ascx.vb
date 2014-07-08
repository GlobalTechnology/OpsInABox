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
Imports StaffBrokerFunctions

Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class countryDashboard
        Inherits Entities.Modules.PortalModuleBase

        Public jsonPI = ""
        Public jsonLi = ""

        Dim d As New MPDDataContext
        Dim Pid As Integer = -1
        Dim ds As New StaffBroker.StaffBrokerDataContext
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


            Dim thisCountry = From c In d.AP_mpd_Countries Where c.isoCode = Request.QueryString("country")

            If thisCountry.Count > 0 Then

                lblCountryTitle.Text = thisCountry.First.name
                jsonPI = "['>100% Raised', " & thisCountry.First.FullCount & "],"
                jsonPI &= "['80-100% Raised', " & thisCountry.First.HighCount & "],"
                jsonPI &= "['50-80% Raised', " & thisCountry.First.LowCount & "],"
                jsonPI &= "['<50% Raised', " & thisCountry.First.VeryLowCount & "],"
                jsonPI &= "['No Budget', " & thisCountry.First.NoBudgetCount & "]"
                jsonLi = ""

                lblAvgSupport.Text = 0

                lblAvgSupport.Text = thisCountry.First.AvgSupport12.Value.ToString("0.0%")
                lblBdgVsAct.Text = thisCountry.First.BudgetAccuracy.Value.ToString("0.0%")
                lblBdgVsActLabel.Text = IIf(thisCountry.First.BudgetAccuracy > 1.0, "(budgets under-estimated expenses)", "(budgets over-estimated expenses)")




               

                rpLessThan50.DataSource = thisCountry.First.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 < 0.5)
                rpLessThan50.DataBind()

                rpLow.DataSource = thisCountry.First.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 0.5 And c.AvgSupLevel12 < 0.8)
                rpLow.DataBind()

                'rpMedium.DataSource = g3
                'rpMedium.DataBind()

                rpHigh.DataSource = thisCountry.First.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 0.8 And c.AvgSupLevel12 < 1.0)
                rpHigh.DataBind()

                rpFull.DataSource = thisCountry.First.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 1.0)
                rpFull.DataBind()

                rpNone.DataSource = thisCountry.First.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 = 0)
                rpNone.DataBind()


                Dim lastPeriod As String = d.AP_mpd_UserAccountInfos.Where(Function(c) c.mpdCountryId = thisCountry.First.mpdCountryId And c.income > 0).Max(Function(c) c.period)
                If String.IsNullOrEmpty(lastPeriod) Then
                    lastPeriod = Today.ToString("yyyyMM")
                End If
                Dim LastPeriodDate = New Date(CInt(Left(lastPeriod, 4)), CInt(Right(lastPeriod, 2)), 1)

                Dim firstPeriod As String = LastPeriodDate.AddMonths(-12).ToString("yyyyMM")
                Dim quateerPeriod As String = LastPeriodDate.AddMonths(-3).ToString("yyyyMM")
                Dim monthPeriod As String = LastPeriodDate.AddMonths(-1).ToString("yyyyMM")

                ' Label3.Text = lastPeriod



                Dim incomeData = (From c In d.AP_mpd_UserAccountInfos Where c.mpdCountryId = thisCountry.First.mpdCountryId And c.period >= firstPeriod And c.period <= lastPeriod _
                    Select Period = c.period, staffId = c.staffId, income = c.income, expense = c.expense, mpd_budget = c.toRaiseBudget).ToList


            

                For i As Integer = -12 To 0

                    Dim count As Integer = i
                    Dim AvgSupport = (From c In incomeData Where c.Period = LastPeriodDate.AddMonths(count).ToString("yyyyMM") And c.mpd_budget > 0 Select c.income / c.mpd_budget)
                    Dim ag = 0.0
                    Dim full = 0.0
                    If AvgSupport.Count > 0 Then
                        ag = AvgSupport.Average()
                        full = AvgSupport.Where(Function(c) c > 0.9).Count / thisCountry.First.Ap_mpd_Users.Count
                    End If




                    jsonLi &= "['" & LastPeriodDate.AddMonths(count).ToString("MMM yy") & "', " & ag.ToString("0.00") & ", " & full.ToString("0.00") & " ], "



                Next



            End If

        End Sub
        Public Function GetStaffName(ByVal StaffId As Integer) As String
            Dim thisStaff = From c In ds.AP_StaffBroker_Staffs Where c.PortalId = Pid And c.StaffId = StaffId

            If thisStaff.Count > 0 Then
                Return thisStaff.First.DisplayName
            Else
                Return ""
            End If

        End Function

    End Class
End Namespace
