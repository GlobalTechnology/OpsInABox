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

        Dim d As New MPDDataContext()

        Dim Pid As Integer = -1
        Dim ds As New StaffBroker.StaffBrokerDataContext
        Public UsingEstimates As Boolean = False
        Public team_mode As Boolean = False
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim ssoGuid = UserInfo.Profile.GetPropertyValue("ssoGUID")

                ' ssoGuid = "126AA989-238B-2EBD-5BE2-187DA7EDE3B7"  'Beni
                ' ssoGuid = "109764C9-CD24-CF94-839C-65F41C9C2E5C"  ' Eric
                'ssoGuid = "C17C80EC-D8C5-484C-0A43-4CE345ADFADF"  ' Tomas
                'ssoGuid = "1FF92F95-DD56-AFCA-9489-FC6E8F253237" ' Goce
                'ssoGuid = "3925839A-F828-4087-8223-816DE32A7BAF" 'Chontelle
                mpdDashboardMenu.CountryURL = EditUrl("countryDashboard")
                mpdDashboardMenu.StaffUrl = EditUrl("staffDashboard")

                mpdDashboardMenu.ssoGuid = ssoGuid

                If Request.QueryString("country") = "team" Then
                    'team mode
                    team_mode = True
                    mpd_summary.Visible = False
                    ShowTeam(ssoGuid, False, True)
                Else
                    Dim thisCountry = (From c In d.AP_mpd_Countries Where c.isoCode = Request.QueryString("country")).FirstOrDefault
                    If String.IsNullOrEmpty(ssoGuid) Or (d.AP_mpd_AreaAdmins.Where(Function(c) c.area = thisCountry.Area And c.sso_guid = ssoGuid).Count = 0 And (thisCountry.AP_MPD_CountryAdmins.Where(Function(c) c.sso_guid = ssoGuid).Count = 0)) Then
                        'TODO display error message
                        pnlError.Visible = True
                        pnlMain.Visible = False
                        Return
                    End If





                    If Request.QueryString("StaffId") <> "" Then
                        Response.Redirect(EditUrl("staffDashboard") & "?staffId=" & Request.QueryString("StaffId"))
                    End If








                    If Not thisCountry Is Nothing Then
                        ShowReport(thisCountry, thisCountry.VeryLowCount + thisCountry.LowCount + thisCountry.HighCount + thisCountry.FullCount = 0)
                    End If


                End If


           

            End If


        End Sub

        Public Sub ShowTeam(ByVal ssoGuid As String, ByVal UseEstimates As Boolean, ByVal first_time As Boolean)

            ' Dim thisCountry = From c In d.AP_mpd_Countries Where c.isoCode = Request.QueryString("country")
            lblEstimatedBudgets.Visible = UseEstimates
            btnShowEstimatedBudgets.Text = IIf(UseEstimates, "Don't use estimated budgets", "Use estimated budgets")

            mpdDashboardMenu.Title = "Team Report for " & UserInfo.DisplayName
            mpdDashboardMenu.Mode = DesktopModules_AgapeConnect_mpdCalc_controls_mpdAdmin.mpdMenuMode.Team
            Dim team_ids = From c In d.ap_mpd_user_reportings Where c.leader_sso_guid = ssoGuid Select c.mpd_user_id

            Dim team = From c In d.Ap_mpd_Users Where team_ids.Contains(c.gr_person_id)

            If first_time And team.Where(Function(c) c.AvgExpenseBudget12 <> 0).Count = 0 Then
                UseEstimates = True

            End If
            UsingEstimates = UseEstimates

            jsonPI = "['>100% Raised', " & IIf(UseEstimates, team.Where(Function(c) c.EstSupLevel12 >= 1.0).Count, team.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 1.0).Count) & "],"
            jsonPI &= "['80-100% Raised', " & IIf(UseEstimates, team.Where(Function(c) c.EstSupLevel12 >= 0.8 And c.EstSupLevel12 < 1.0).Count, team.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 0.8 And c.AvgSupLevel12 < 1.0).Count) & "],"
            jsonPI &= "['50-80% Raised', " & IIf(UseEstimates, team.Where(Function(c) c.EstSupLevel12 >= 0.5 And c.EstSupLevel12 < 0.8).Count, team.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 0.6 And c.AvgSupLevel12 < 0.8).Count) & "],"
            jsonPI &= "['<50% Raised', " & IIf(UseEstimates, team.Where(Function(c) c.EstSupLevel12 < 0.5).Count, team.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 < 0.5).Count) & "],"
            jsonPI &= "['No Budget', " & IIf(UseEstimates, 0, team.Where(Function(c) c.AvgExpenseBudget12 = 0).Count) & "]"
            jsonLi = ""
            If UseEstimates Then
                rpLessThan50.DataSource = team.Where(Function(c) c.EstSupLevel12 < 0.5).OrderBy(Function(c) c.Name)
                rpLow.DataSource = team.Where(Function(c) c.EstSupLevel12 >= 0.5 And c.EstSupLevel12 < 0.8).OrderBy(Function(c) c.Name)
                rpHigh.DataSource = team.Where(Function(c) c.EstSupLevel12 >= 0.8 And c.EstSupLevel12 < 1.0).OrderBy(Function(c) c.Name)
                rpFull.DataSource = team.Where(Function(c) c.EstSupLevel12 >= 1.0).OrderBy(Function(c) c.Name)


                rpNone.DataSource = team.Where(Function(c) False)
            Else
                rpLessThan50.DataSource = team.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 < 0.5).OrderBy(Function(c) c.Name) '.Select(Function(c) New With {.Name = c.Name, .StaffId = c.StaffId, .SupLev = c.AvgSupLevel12})
                rpLow.DataSource = team.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 0.5 And c.AvgSupLevel12 < 0.8).OrderBy(Function(c) c.Name)  '.Select(Function(c) New With {.Name = c.Name, .StaffId = c.StaffId, .SupLev = c.AvgSupLevel12})
                rpHigh.DataSource = team.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 0.8 And c.AvgSupLevel12 < 1.0).OrderBy(Function(c) c.Name)  '.Select(Function(c) New With {.Name = c.Name, .StaffId = c.StaffId, .SupLev = c.AvgSupLevel12})
                rpFull.DataSource = team.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 1.0).OrderBy(Function(c) c.Name)  '.Select(Function(c) New With {.Name = c.Name, .StaffId = c.StaffId, .SupLev = c.AvgSupLevel12})

                rpNone.DataSource = team.Where(Function(c) c.AvgExpenseBudget12 = 0).OrderBy(Function(c) c.Name)
            End If

            rpNone.DataBind()

            rpLessThan50.DataBind()

            rpLow.DataBind()

            rpHigh.DataBind()
            rpFull.DataBind()



        End Sub

        Public Sub ShowReport(ByVal thisCountry As MPD.AP_mpd_Country, ByVal UseEstimates As Boolean)


            UsingEstimates = UseEstimates
            ' Dim thisCountry = From c In d.AP_mpd_Countries Where c.isoCode = Request.QueryString("country")
            lblEstimatedBudgets.Visible = UseEstimates
            btnShowEstimatedBudgets.Text = IIf(UseEstimates, "Don't use estimated budgets", "Use estimated budgets")



            mpdDashboardMenu.Title = thisCountry.name
            jsonPI = "['>100% Raised', " & IIf(UseEstimates, thisCountry.EstFullCount, thisCountry.FullCount) & "],"
            jsonPI &= "['80-100% Raised', " & IIf(UseEstimates, thisCountry.EstHighCount, thisCountry.HighCount) & "],"
            jsonPI &= "['50-80% Raised', " & IIf(UseEstimates, thisCountry.EstLowCount, thisCountry.LowCount) & "],"
            jsonPI &= "['<50% Raised', " & IIf(UseEstimates, thisCountry.EstVeryLowCount, thisCountry.VeryLowCount) & "],"
            jsonPI &= "['No Budget', " & IIf(UseEstimates, 0, thisCountry.NoBudgetCount) & "]"
            jsonLi = ""

            lblAvgSupport.Text = 0

            lblAvgSupport.Text = CDbl(IIf(UseEstimates, thisCountry.EstAvgSupport12.Value, thisCountry.AvgSupport12.Value)).ToString("0.0%")
            lblBdgVsAct.Text = thisCountry.BudgetAccuracy.Value.ToString("0.0%")
            lblBdgVsActLabel.Text = IIf(thisCountry.BudgetAccuracy > 1.0, "(budgets under-estimated expenses)", "(budgets over-estimated expenses)")


           

            If UseEstimates Then
                rpLessThan50.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) c.EstSupLevel12 < 0.5).OrderBy(Function(c) c.Name)
                rpLow.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) c.EstSupLevel12 >= 0.5 And c.EstSupLevel12 < 0.8).OrderBy(Function(c) c.Name)
                rpHigh.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) c.EstSupLevel12 >= 0.8 And c.EstSupLevel12 < 1.0).OrderBy(Function(c) c.Name)
                rpFull.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) c.EstSupLevel12 >= 1.0).OrderBy(Function(c) c.Name)


                rpNone.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) False)
            Else
                rpLessThan50.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 < 0.5).OrderBy(Function(c) c.Name) '.Select(Function(c) New With {.Name = c.Name, .StaffId = c.StaffId, .SupLev = c.AvgSupLevel12})
                rpLow.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 0.5 And c.AvgSupLevel12 < 0.8).OrderBy(Function(c) c.Name)  '.Select(Function(c) New With {.Name = c.Name, .StaffId = c.StaffId, .SupLev = c.AvgSupLevel12})
                rpHigh.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 0.8 And c.AvgSupLevel12 < 1.0).OrderBy(Function(c) c.Name)  '.Select(Function(c) New With {.Name = c.Name, .StaffId = c.StaffId, .SupLev = c.AvgSupLevel12})
                rpFull.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 <> 0 And c.AvgSupLevel12 >= 1.0).OrderBy(Function(c) c.Name)  '.Select(Function(c) New With {.Name = c.Name, .StaffId = c.StaffId, .SupLev = c.AvgSupLevel12})

                rpNone.DataSource = thisCountry.Ap_mpd_Users.Where(Function(c) c.AvgExpenseBudget12 = 0).OrderBy(Function(c) c.Name)
            End If


            rpNone.DataBind()

            rpLessThan50.DataBind()

            rpLow.DataBind()

            rpHigh.DataBind()
            rpFull.DataBind()



            Dim lastPeriod As String = d.AP_mpd_UserAccountInfos.Where(Function(c) c.mpdCountryId = thisCountry.mpdCountryId And c.income > 0).Max(Function(c) c.period)
            If String.IsNullOrEmpty(lastPeriod) Then
                lastPeriod = Today.ToString("yyyyMM")
            End If
            Dim LastPeriodDate = New Date(CInt(Left(lastPeriod, 4)), CInt(Right(lastPeriod, 2)), 1)

            Dim firstPeriod As String = LastPeriodDate.AddMonths(-12).ToString("yyyy-MM")
            Dim quateerPeriod As String = LastPeriodDate.AddMonths(-3).ToString("yyyy-MM")
            Dim monthPeriod As String = LastPeriodDate.AddMonths(-1).ToString("yyyy-MM")

            ' Label3.Text = lastPeriod

            If UseEstimates Then
                Dim incomeData = (From c In d.AP_mpd_UserAccountInfos Where c.mpdCountryId = thisCountry.mpdCountryId And c.period >= firstPeriod And c.period <= lastPeriod _
                     Select Period = c.period, staffId = c.staffId, income = c.income + c.foreignIncome + c.compensation, expense = -c.expense, mpd_budget = -c.expense * 1.1).ToList

                For i As Integer = -12 To 0

                    Dim count As Integer = i
                    Dim AvgSupport = (From c In incomeData Where c.Period = LastPeriodDate.AddMonths(count).ToString("yyyy-MM") And c.mpd_budget > 0 Select c.income / c.mpd_budget)


                    Dim ag = 0.0
                    Dim full = 0.0
                    If AvgSupport.Count > 0 Then
                        ag = AvgSupport.Average()
                        full = AvgSupport.Where(Function(c) c > 0.9).Count / thisCountry.Ap_mpd_Users.Count
                    End If

                    jsonLi &= "['" & LastPeriodDate.AddMonths(count).ToString("MMM yy") & "', " & ag.ToString("0.00") & ", " & full.ToString("0.00") & " ], "

                Next


            Else

                Dim incomeData = (From c In d.AP_mpd_UserAccountInfos Where c.mpdCountryId = thisCountry.mpdCountryId And c.period >= firstPeriod And c.period <= lastPeriod _
                      Select Period = c.period, staffId = c.staffId, income = c.income + c.foreignIncome + c.compensation, expense = -c.expense, mpd_budget = c.toRaiseBudget).ToList
                For i As Integer = -12 To 0

                    Dim count As Integer = i
                    Dim AvgSupport = (From c In incomeData Where c.Period = LastPeriodDate.AddMonths(count).ToString("yyyy-MM") And c.mpd_budget > 0 Select c.income / c.mpd_budget)


                    Dim ag = 0.0
                    Dim full = 0.0
                    If AvgSupport.Count > 0 Then
                        ag = AvgSupport.Average()
                        full = AvgSupport.Where(Function(c) c > 0.9).Count / thisCountry.Ap_mpd_Users.Count
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

        Protected Sub btnShowEstimatedBudgets_Click(sender As Object, e As EventArgs) Handles btnShowEstimatedBudgets.Click
            If Request.QueryString("country") = "team" Then

                Dim ssoGuid = UserInfo.Profile.GetPropertyValue("ssoGUID")
                ShowTeam(ssoGuid, Not btnShowEstimatedBudgets.Text.Contains("Don't"), False)
            Else
                Dim thisCountry = (From c In d.AP_mpd_Countries Where c.isoCode = Request.QueryString("country")).FirstOrDefault
                If Not thisCountry Is Nothing Then
                    ShowReport(thisCountry, Not btnShowEstimatedBudgets.Text.Contains("Don't"))

                End If
            End If
            
        End Sub
    End Class
End Namespace
