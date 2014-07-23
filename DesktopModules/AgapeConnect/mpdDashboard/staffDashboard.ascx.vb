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
    Partial Class staffDashboard
        Inherits Entities.Modules.PortalModuleBase

        Public jsonPI As String = ""
        Public jsonLi As String = ""

        Dim d As New MPDDataContext
        Dim Pid As Integer = -1
        Dim ds As New StaffBroker.StaffBrokerDataContext
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load



            Dim mpdu = (From c In d.Ap_mpd_Users Where c.AP_mpd_UserId = Request.QueryString("mpd_user_id")).First
            lblStaffName.Text = mpdu.Name
            Dim incomeData = (From c In mpdu.AP_mpd_UserAccountInfos Where c.period >= Today.AddMonths(-13).ToString("yyyy-MM")).ToList


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
           
          




        End Sub


    End Class
End Namespace
