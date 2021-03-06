﻿Imports System
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
    Partial Class staffDashboard
        Inherits Entities.Modules.PortalModuleBase

        Public jsonPI As String = ""
        Public jsonLi As String = ""

        Dim d As New MPDDataContext
        Dim Pid As Integer = -1
        Dim ds As New StaffBroker.StaffBrokerDataContext
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim ssoGuid = UserInfo.Profile.GetPropertyValue("ssoGUID")
           
            'ssoGuid = "126AA989-238B-2EBD-5BE2-187DA7EDE3B7"  'Beni
            ' ssoGuid = "109764C9-CD24-CF94-839C-65F41C9C2E5C"  ' Eric
            'ssoGuid = "C17C80EC-D8C5-484C-0A43-4CE345ADFADF"  ' Tomas
            'ssoGuid = "1FF92F95-DD56-AFCA-9489-FC6E8F253237" ' Goce
            ' ssoGuid = "3925839A-F828-4087-8223-816DE32A7BAF" 'Chontelle

            mpdDashboardMenu.CountryURL = EditUrl("countryDashboard")
            mpdDashboardMenu.StaffUrl = EditUrl("staffDashboard")

            mpdDashboardMenu.ssoGuid = ssoGuid
            Dim mpdu = (From c In d.Ap_mpd_Users Where c.AP_mpd_UserId = Request.QueryString("mpd_user_id")).First


            If String.IsNullOrEmpty(ssoGuid) Or (d.AP_mpd_AreaAdmins.Where(Function(c) c.area = mpdu.AP_mpd_Country.Area).Count = 0 And mpdu.Key_GUID <> ssoGuid And (mpdu.AP_mpd_Country.AP_MPD_CountryAdmins.Where(Function(c) c.sso_guid = ssoGuid).Count = 0) And d.AP_mpd_AreaAdmins.Where(Function(c) c.area = "GLBL" And c.sso_guid = ssoGuid).Count = 0) Then
                'TODO display error message
                pnlError.Visible = True
                pnlMain.Visible = False
                Return
            End If



            mpdDashboardMenu.Title = mpdu.Name

            If Not String.IsNullOrEmpty(Request.QueryString("country")) Then
                hlBack.Text = "&lt;&lt;Back to " & mpdu.AP_mpd_Country.name
                hlBack.NavigateUrl = EditUrl("countryDashboard") & "?country=" & Request.QueryString("country")
            Else
                hlBack.Visible = False
                mpdDashboardMenu.Mode = DesktopModules_AgapeConnect_mpdCalc_controls_mpdAdmin.mpdMenuMode.Staff
            End If

            Dim incomeData = (From c In mpdu.AP_mpd_UserAccountInfos Where c.period >= Today.AddMonths(-14).ToString("yyyy-MM") And c.period <= Today.AddMonths(-1).ToString("yyyy-MM")).ToList


            jsonLi = ""
            For Each row In incomeData
                Dim start = New Date(CInt(Left(row.period, 4)), CInt(Right(row.period, 2)), 1)
                '  Dim bud = mpdFunctions.getBudgetForStaffPeriod(row.staffId, row.period)


                jsonLi &= "['" & start.ToString("MMM yy") & "', " & row.balance.ToString("0.00") & ", " & (row.income + row.foreignIncome + row.compensation).ToString("0.00") & ", " & row.toRaiseBudget.ToString("0.00") & ", " & (-row.expense).ToString("0.00") & ", " & (row.expBudget).ToString("0.00") & "],"
            Next

            

            jsonPI = "['Local Income', " & mpdu.LocalIncome12.ToString("0.00") & "],['Foreign Income', " & mpdu.ForeignIncome12.ToString("0.00") & "], ['Subsidy', " & mpdu.SubsidyIncome12.ToString("0.00") & "]"



           
            If mpdu.AvgExpenseBudget12 = 0 Then
                lblYear.Text = mpdu.EstSupLevel12.ToString("0.0%") & "*"
                lblQuarter.Text = mpdu.EstSupLevel3.ToString("0.0%") & "*"
                lblMonth.Text = mpdu.EstSupLevel1.ToString("0.0%") & "*"
                lblEstimates.Visible = True
            Else
                lblYear.Text = mpdu.AvgSupLevel12.ToString("0.0%")
                lblQuarter.Text = mpdu.AvgSupLevel3.ToString("0.0%")
                lblMonth.Text = mpdu.AvgSupLevel1.ToString("0.0%")
            End If
            Dim period = Today.AddMonths(-1).ToString("yyyy-MM")
            Dim curBal = incomeData.Where(Function(c) c.period = period).Select(Function(c) c.balance).FirstOrDefault()
            If curBal < 0 Then
                'negative account balance
                imgMpdHealth.ImageURL = "/DesktopModules/AgapeConnect/GlobalStatusDashboard/red_light.png"
                lblMpdHealth.Text = "Account balance is currently negative"
            Else
                If mpdu.AvgIncome12 < -mpdu.AvgExpenses Then
                    'negative tragectory
                    Dim death_date = -CInt((curBal + mpdu.AvgExpenses) / (mpdu.AvgIncome12 + mpdu.AvgExpenses))
                    If death_date < 1 Then
                        imgMpdHealth.ImageURL = "/DesktopModules/AgapeConnect/GlobalStatusDashboard/red_light.png"
                        lblMpdHealth.Text = "This account does not have enough balance to cover 1 months expenses"
                    ElseIf death_date <= 6 Then
                        imgMpdHealth.ImageURL = "/DesktopModules/AgapeConnect/GlobalStatusDashboard/red_light.png"
                        lblMpdHealth.Text = "Our analysis shows that this account will have insufficient funds within " & death_date & " months."
                    ElseIf death_date <= 12 Then
                        imgMpdHealth.ImageURL = "/DesktopModules/AgapeConnect/GlobalStatusDashboard/yellow_light.png"
                        lblMpdHealth.Text = "Our analysis shows that this account will have insufficient funds within " & death_date & " months."
                    End If
                Else
                    imgMpdHealth.ImageURL = "/DesktopModules/AgapeConnect/GlobalStatusDashboard/green_light.png"
                    lblMpdHealth.Text = "This staff account appears healthy. "
                End If
            End If

           








        End Sub


    End Class
End Namespace
