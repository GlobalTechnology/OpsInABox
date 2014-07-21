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
    Partial Class mpdMenu
        Inherits Entities.Modules.PortalModuleBase

       
        Dim myBudgets As IQueryable(Of AP_mpdCalc_StaffBudget)

        Public mpdDefId As Integer
        Public StaffId As Integer
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not String.IsNullOrEmpty(Request.QueryString("sb")) Then
                Response.Redirect(EditUrl("mpdCalc") & "?sb=" & Request.QueryString("sb"))
            End If

            'Verify that there is a tabmoduleid - otherwise create it from the null portalid template
            Dim d As New MPDDataContext

            'Dim q = From c In d.AP_mpdCalc_Definitions Where c.PortalId = PortalId

            'If q.Count = 0 Then
            '    mpdFunctions.CreateNewDef(PortalId, TabModuleId)
            'End If

            LoadMenu()
            lblEditMode.Visible = IsEditMode()
            pnlToProcess.Visible = IsEditable()
            mpdAdminPanel.Visible = IsEditable()
            pnlAdminHelp.Visible = (UserInfo.IsInRole("Administrators") Or UserInfo.IsInRole("Accounts Team"))
            lblViewMode.Visible = Not IsEditMode()
        End Sub

        'Private Sub CreateNewBudgetDefinition()
        '    Dim d As New MPDDataContext
        '    Dim q = From c In d.AP_mpdCalc_Definitions Where c.PortalId = -1

        '    If q.Count > 0 Then
        '        Dim insertDef As New AP_mpdCalc_Definition
        '        insertDef.ActiveFromYear = q.First.ActiveFromYear
        '        insertDef.AssessmentRate = q.First.AssessmentRate
        '        insertDef.Complience = q.First.Complience
        '        insertDef.ShowComplience = q.First.ShowComplience
        '        insertDef.PortalId = PortalId
        '        insertDef.TabModuleId = TabModuleId
        '        d.AP_mpdCalc_Definitions.InsertOnSubmit(insertDef)
        '        d.SubmitChanges()
        '        For Each s In q.First.AP_mpdCalc_Sections
        '            Dim insertSec As New AP_mpdCalc_Section
        '            insertSec.Name = s.Name
        '            insertSec.Number = s.Number
        '            insertSec.TotalMode = s.TotalMode
        '            insertSec.mpdDefId = insertDef.mpdDefId
        '            d.AP_mpdCalc_Sections.InsertOnSubmit(insertSec)
        '            d.SubmitChanges()
        '            For Each row In s.AP_mpdCalc_Questions
        '                Dim insertq As New AP_mpdCalc_Question
        '                insertq.AccountCode = row.AccountCode
        '                insertq.Fixed = row.Fixed
        '                insertq.Formula = row.Formula
        '                insertq.Help = row.Help
        '                insertq.Max = row.Max
        '                insertq.Min = row.Min
        '                insertq.Name = row.Name
        '                insertq.QuestionNumber = row.QuestionNumber
        '                insertq.Rate1 = row.Rate1
        '                insertq.Rate2 = row.Rate2
        '                insertq.Rate3 = row.Rate3
        '                insertq.Rate4 = row.Rate4
        '                insertq.SectionId = insertSec.SectionId
        '                insertq.TaxSystem = row.TaxSystem
        '                insertq.Threshold1 = row.Threshold1
        '                insertq.Threshold2 = row.Threshold2
        '                insertq.Threshold3 = row.Threshold3
        '                insertq.Type = row.Type
        '                d.AP_mpdCalc_Questions.InsertOnSubmit(insertq)
        '            Next
        '            d.SubmitChanges()
        '        Next
        '    End If
        'End Sub

        Public Sub LoadMenu()
            Dim d As New MPDDataContext
  

            If Not Page.IsPostBack Then

          
                Dim mpdDefs = (From c In d.AP_mpdCalc_Definitions Where c.PortalId = PortalId And c.TabModuleId = TabModuleId)
                Dim mpdDef As AP_mpdCalc_Definition
                If mpdDefs.Count > 0 Then
                    mpdDef = mpdDefs.First
                Else
                    mpdDef = mpdFunctions.CreateNewDef(PortalId, TabModuleId)
                End If

                mpdDefId = mpdDef.mpdDefId
                mpdAdminPanel.MpdDefId = mpdDefId
                Dim Staff = StaffBrokerFunctions.GetStaffMember(UserId)


                StaffId = Staff.StaffId




                If IsEditMode() Then
                    Dim includeTypes = mpdDef.StaffTypes.Split(";")
                    ' Label5.Text = mpdDef.StaffTypes

                    Dim ds As New StaffBroker.StaffBrokerDataContext
                   
                    Dim allStaff = From c In ds.AP_StaffBroker_Staffs Where c.PortalId = PortalId And includeTypes.Contains(c.StaffTypeId) Order By c.DisplayName Select c.DisplayName, c.StaffId

                    rpTeam.DataSource = allStaff
                    rpTeam.DataBind()
                    rpMenuDetail.DataSource = allStaff
                    rpMenuDetail.DataBind()



                    Dim team = StaffBrokerFunctions.GetStaffIncl

                    Dim toProcess = From c In d.AP_mpdCalc_StaffBudgets Where c.AP_mpdCalc_Definition.PortalId = PortalId And c.Status = StaffRmb.RmbStatus.Approved Select c.StaffBudgetId, DisplayName = getDisplayName(c.StaffId, c.BudgetPeriodStart)
                    rpToProcess.DataSource = toProcess
                    rpToProcess.DataBind()
                Else
                    Dim team = StaffBrokerFunctions.GetTeam(UserId).Select(Function(c) c.AP_StaffBroker_Staffs.StaffId)


                    Dim myTeam = From c In StaffBrokerFunctions.GetTeam(UserId) Select c.AP_StaffBroker_Staffs.DisplayName, c.AP_StaffBroker_Staffs.StaffId
                    rpTeam.DataSource = myTeam
                    rpTeam.DataBind()
                    rpMenuDetail.DataSource = myTeam
                    rpMenuDetail.DataBind()

                    myMenuDetail.PortalId = PortalId
                    myMenuDetail.EditURL = EditUrl("mpdCalc")
                    myMenuDetail.DisplayName = Staff.DisplayName
                    myMenuDetail.StaffId = Staff.StaffId
                    myMenuDetail.ShowCreate = True
                    myMenuDetail.MpdDefId = mpdDefId

                    '   pnlNoBudget.Visible = Not d.AP_mpdCalc_StaffBudgets.Any(Function(c) c.StaffId = StaffId And c.DefinitionId = mpdDefId)

                End If









            End If



        End Sub

        Public Function getDisplayName(ByVal StaffId As Integer, ByVal PeriodStart As String) As String
            Dim dt As New Date(Left(PeriodStart, 4), Right(PeriodStart, 2), 1)

            Return StaffBrokerFunctions.GetStaffbyStaffId(StaffId).DisplayName & " (" & dt.ToString("MMMM yyyy") & ")"
        End Function
    End Class
End Namespace
