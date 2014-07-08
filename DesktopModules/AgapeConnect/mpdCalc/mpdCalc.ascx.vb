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
    Partial Class mpdCalc
        Inherits Entities.Modules.PortalModuleBase

        Private _age1 As Integer = 0
        Public Property Age1() As Integer
            Get
                Return _age1
            End Get
            Set(ByVal value As Integer)
                _age1 = value
            End Set
        End Property

        Private _age2 As Integer = 0
        Public Property Age2() As Integer
            Get
                Return _age2
            End Get
            Set(ByVal value As Integer)
                _age2 = value
            End Set
        End Property

        Private _isCouple As Boolean = False
        Public Property IsCouple() As Boolean
            Get
                Return _isCouple
            End Get
            Set(ByVal value As Boolean)
                _isCouple = value
            End Set
        End Property

        Private _staffType As String = ""
        Public Property StaffType() As String
            Get
                Return _staffType
            End Get
            Set(ByVal value As String)
                _staffType = value
            End Set
        End Property


        Private StaffBudId As Integer = -1
        Public LastSection As Integer = 0
        Public DefaultAccount As String = ""
        Public TagReplacementScript As String = ""
  

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            If (Not String.IsNullOrEmpty(Request.QueryString("sb"))) Then
                StaffBudId = Request.QueryString("sb")
            End If

            If Not Page.IsPostBack Then

                Dim ds As New StaffBroker.StaffBrokerDataContext


                pnlInsert.Visible = IsEditMode()


              
                Dim d As New MPDDataContext()
                Dim theForm = From c In d.AP_mpdCalc_Definitions Where c.TabModuleId = TabModuleId And c.PortalId = PortalId
                Dim Staff = StaffBrokerFunctions.GetStaffMember(UserId)
                Dim thisForm As AP_mpdCalc_Definition
                If theForm.Count > 0 Then
                    thisForm = theForm.First
                Else
                    thisForm = mpdFunctions.CreateNewDef(PortalId, TabModuleId)
                End If

                mpdAdminPanel.mpdDefId = thisForm.mpdDefId
                mpdAdminPanel.Visible = IsEditMode()
                



                DefaultAccount = thisForm.DefaultAccount

                Dim bud = From c In theForm.First.AP_mpdCalc_StaffBudgets Where c.StaffBudgetId = StaffBudId
                If bud.Count > 0 Then
                    'If Not bud.First.CurrentSupportLevel Is Nothing Then
                    '    itemCurrent.Monthly = bud.First.CurrentSupportLevel.Value.ToString("F0", New CultureInfo("en-US"))
                    'End If

                    Dim FirstBudgetMonth As Integer? = bud.First.AP_mpdCalc_Definition.FirstBudgetPeriod
                    If FirstBudgetMonth Is Nothing Then
                        FirstBudgetMonth = 7

                    End If
                    Dim fpStartDate As Date
                    If FirstBudgetMonth <= Today.Month Then
                        fpStartDate = New Date(Today.Year, FirstBudgetMonth, 1)
                    Else
                        fpStartDate = New Date(Today.Year - 1, FirstBudgetMonth, 1)
                    End If

                    For i As Integer = -1 To 1
                        ddlStartPeriod.Items.Add(New ListItem(fpStartDate.AddYears(i).ToString("MMM yyyy"), fpStartDate.AddYears(i).ToString("yyyyMM")))
                    Next
                    ddlStartPeriod.Items.Clear()
                    ddlStartPeriod.Items.Add(New ListItem("Last Year: " & fpStartDate.AddYears(-1).ToString("MMMM yyyy"), fpStartDate.AddYears(-1).ToString("yyyyMM")))
                    ddlStartPeriod.Items.Add(New ListItem("This Year: " & fpStartDate.AddYears(0).ToString("MMMM yyyy"), fpStartDate.AddYears(0).ToString("yyyyMM")))
                    ddlStartPeriod.Items.Add(New ListItem("Next Year: " & fpStartDate.AddYears(1).ToString("MMMM yyyy"), fpStartDate.AddYears(1).ToString("yyyyMM")))

                    ddlStartPeriod.Items.Add(New ListItem("Custom (Please specify):", ""))

                    ddlStartPeriod.SelectedIndex = 1


                    ddlYear.Items.Clear()
                    ddlYear.Items.Add(New ListItem(fpStartDate.AddYears(-1).ToString("yyyy"), fpStartDate.AddYears(-1).ToString("yyyy")))
                    ddlYear.Items.Add(New ListItem(fpStartDate.AddYears(0).ToString("yyyy"), fpStartDate.AddYears(0).ToString("yyyy")))
                    ddlYear.Items.Add(New ListItem(fpStartDate.AddYears(1).ToString("yyyy"), fpStartDate.AddYears(1).ToString("yyyy")))




                    Select Case bud.First.Status
                        Case StaffRmb.RmbStatus.Draft
                            btnSubmit.Visible = True
                            btnCancel.Visible = False
                        Case StaffRmb.RmbStatus.Submitted
                            btnApprove.Visible = Staff.StaffId <> bud.First.StaffId
                        Case StaffRmb.RmbStatus.Approved
                            btnProcess.Visible = IsEditMode()
                        Case StaffRmb.RmbStatus.Processed
                            btnCancel.Visible = False
                        Case StaffRmb.RmbStatus.Cancelled
                            btnSubmit.Visible = True
                            btnCancel.Visible = False
                    End Select


                    lblStatus.Text = StaffRmb.RmbStatus.StatusName(bud.First.Status)
                    Dim dt = New Date(CInt(Left(bud.First.BudgetPeriodStart, 4)), CInt(Right(bud.First.BudgetPeriodStart, 2)), 1)

                    If ddlStartPeriod.Items.FindByValue(bud.First.BudgetPeriodStart) Is Nothing Then
                        ddlStartPeriod.SelectedValue = ""
                        ddlPeriod.SelectedValue = dt.Month

                        ddlYear.SelectedValue = dt.Year
                        customDate.Attributes.CssStyle.Remove("display")
                    Else
                        ddlPeriod.SelectedValue = bud.First.BudgetPeriodStart

                    End If

                    If bud.First.Status <> StaffRmb.RmbStatus.Draft And bud.First.Status <> StaffRmb.RmbStatus.Cancelled Then
                        cbCompliance.Enabled = False
                        cbCompliance.Checked = True
                        btnSubmit.Enabled = True
                    End If
                    Staff = StaffBrokerFunctions.GetStaffbyStaffId(bud.First.StaffId)

                End If
                'Dim d As New StaffBroker.StaffBrokerDataContext
                Dim staffDefs = From c In ds.AP_StaffBroker_StaffPropertyDefinitions Where c.PortalId = PortalId


                For Each row In staffDefs

                    Dim value = StaffBrokerFunctions.GetStaffProfileProperty(Staff.StaffId, row.PropertyName)
                    If row.Type = 2 Then 'Boolean
                        value = value.ToLower
                        If value = "" Then
                            value = "false"
                        End If
                    ElseIf row.Type = 0 Then 'string
                        value = "'" & value & "'"
                    ElseIf row.Type = 1 And String.IsNullOrEmpty(value) Then
                        value = 0

                    End If
                    TagReplacementScript &= "f = f.replace(/{" & row.PropertyName & "}/g, " & value & ");" & vbNewLine
                Next


                hfTagReplacementScript.Value = TagReplacementScript


                lblStaffName.Text = Translate("BudgetFor").Replace("[NAME]", "<b>" & Staff.DisplayName & "<b>")
                IsCouple = Staff.UserId2 > 0
                StaffType = Staff.AP_StaffBroker_StaffType.Name
                itemCurrent.Monthly = mpdFunctions.getAverageMonthlyIncomeOver12Periods(Staff.StaffId)

                If (thisForm.AP_mpdCalc_Sections.Count > 0) Then
                    LastSection = thisForm.AP_mpdCalc_Sections.Max(Function(c) c.Number)
                End If
                rpSections.DataSource = thisForm.AP_mpdCalc_Sections.OrderBy(Function(c) c.Number)
                rpSections.DataBind()





                ddlInsertOrder.DataSource = (From c In thisForm.AP_mpdCalc_Sections Select c.Number + 1)
                ddlInsertOrder.DataBind()


                hfAssessment.Value = thisForm.Assessment
                hfCompentation.Value = thisForm.Compensation


                If theForm.First.ShowComplience Then
                    cbCompliance.Text = thisForm.Complience

                End If
                cbCompliance.Visible = thisForm.ShowComplience
                Try
                    Dim user1 = UserController.GetUserById(PortalId, Staff.UserId1)
                    Age1 = DateDiff(DateInterval.Year, Date.Parse(User1.Profile.GetPropertyValue("Birthday"), New CultureInfo("en-gb")), Today)
                Catch ex As Exception
                    Age1 = 0
                End Try
                If Staff.UserId2 > 0 Then
                    Try
                        Dim user1 = UserController.GetUserById(PortalId, Staff.UserId2)
                        Age2 = DateDiff(DateInterval.Year, Date.Parse(User1.Profile.GetPropertyValue("Birthday"), New CultureInfo("en-gb")), Today)
                    Catch ex As Exception
                        Age2 = 0
                    End Try
                Else
                    Age2 = 0
                End If
                





            Else
                TagReplacementScript = hfTagReplacementScript.Value


            End If




        End Sub
        Public Function Translate(ByVal ResourceString As String) As String
            Dim rtn As String
            Try
                rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
                If String.IsNullOrEmpty(rtn) Then
                    rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/mpdCalc/App_LocalResources/mpdCalc.ascx.resx")
                End If
            Catch ex As Exception

                rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/mpdCalc/App_LocalResources/mpdCalc.ascx.resx")

            End Try

            Return rtn

        End Function

        Private Sub set_if(ByRef prop As Object, ByVal value As Object)
            If Not value Is Nothing Then
                prop = value
            End If
        End Sub


        Public Function GetAnswer(ByVal QuestionId As Integer) As String
            If (StaffBudId > 0) Then
                Dim d As New MPDDataContext

                Dim q = From c In d.AP_mpdCalc_Answers Where c.QuestionId = QuestionId And c.StaffBudgetId = StaffBudId

                If q.Count > 0 Then
                    Return q.First.Value.Value.ToString("n2", New CultureInfo("en-US"))
                End If
            End If
            Return ""
        End Function
        Public Function GetName(ByVal QuestionId As Integer, ByVal DefName As String) As String
            If Not String.IsNullOrEmpty(DefName) Then
                Return DefName
            End If
            If (StaffBudId > 0) Then
                Dim d As New MPDDataContext

                Dim q = From c In d.AP_mpdCalc_Answers Where c.QuestionId = QuestionId And c.StaffBudgetId = StaffBudId

                If q.Count > 0 Then
                    Return "!!" & q.First.Name
                End If
            End If
            Return ""
        End Function

        Private Sub SaveBudget(Optional ByVal ToStatus As Integer = -1)
            Dim d As New MPD.MPDDataContext

            Dim def = From c In d.AP_mpdCalc_Definitions Where c.TabModuleId = TabModuleId And c.PortalId = PortalId
            If def.Count > 0 Then

                Dim bud = From c In d.AP_mpdCalc_StaffBudgets Where c.StaffBudgetId = CInt(Request.QueryString("sb")) ' And c.BudgetYearStart = Today.Year
                Dim budId As Integer = -1
                If bud.Count > 0 Then

                    bud.First.CurrentSupportLevel = itemCurrent.Monthly
                    bud.First.TotalBudget = hfMpdGoal.Value
                    bud.First.Compensation = hfCompensationValue.Value
                    bud.First.ToRaise = bud.First.TotalBudget - bud.First.Compensation




                    bud.First.BudgetPeriodStart = IIf(ddlStartPeriod.SelectedValue = "", New Date(ddlYear.SelectedValue, ddlPeriod.SelectedValue, 1).ToString("yyyyMM"), ddlStartPeriod.SelectedValue)
                    bud.First.BudgetYearStart = Left(bud.First.BudgetPeriodStart, 4)






                    If ToStatus >= 0 Then
                        bud.First.Status = ToStatus
                        Select Case (ToStatus)
                            Case StaffRmb.RmbStatus.Submitted
                                bud.First.SubmittedOn = Now
                                bud.First.ApproveCode = Guid.NewGuid().ToString
                            Case StaffRmb.RmbStatus.Approved
                                bud.First.ApprovedOn = Now
                                bud.First.ApprovedBy = UserId

                            Case StaffRmb.RmbStatus.Processed
                                bud.First.ProcessedOn = Now
                                bud.First.ProcessedBy = UserId
                        End Select



                    End If
                    budId = bud.First.StaffBudgetId

                End If

                d.AP_mpdCalc_Answers.DeleteAllOnSubmit(d.AP_mpdCalc_Answers.Where(Function(c) c.StaffBudgetId = budId))

                Dim budgetvalues As New Dictionary(Of String, Double)

                For Each s In rpSections.Items
                    Dim rp As Repeater = s.FindControl("rpItems")
                    For Each q In rp.Items
                        Dim m As mpdItem = q.FindControl("theMpdItem")

                        Dim insert As New MPD.AP_mpdCalc_Answer
                        insert.QuestionId = m.QuestionId
                        If m.Monthly = 0 Then

                            insert.Value = m.Yearly / 12
                        Else
                            insert.Value = m.Monthly
                        End If


                        insert.Name = m.ItemName
                        insert.Tax = m.Tax
                        insert.StaffBudgetId = budId

                        d.AP_mpdCalc_Answers.InsertOnSubmit(insert)

                        AddToBudget(budgetvalues, m.AccountCode, insert.Value + insert.Tax)

                    Next
                Next
                If ToStatus = StaffRmb.RmbStatus.Processed And Not StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then


                    Dim FirstFiscalMonth = StaffBrokerFunctions.GetSetting("FirstFiscalMonth", PortalId)
                    If String.IsNullOrEmpty(FirstFiscalMonth) Then
                        FirstFiscalMonth = 7
                    End If
                    Dim rc = StaffBrokerFunctions.GetStaffbyStaffId(bud.First.StaffId).CostCenter
                    Dim fy = getFiscalYear(bud.First.BudgetPeriodStart, FirstFiscalMonth)

                    Dim fp = GetFiscalPeriod(bud.First.BudgetPeriodStart, FirstFiscalMonth)

                    For Each row In budgetvalues


                        AddBudgetFromPeriod(StaffBrokerFunctions.GetStaffbyStaffId(bud.First.StaffId).CostCenter, row.Key, fy, row.Value, fp, 12)
                        If fp <> 1 Then
                            AddBudgetFromPeriod(StaffBrokerFunctions.GetStaffbyStaffId(bud.First.StaffId).CostCenter, row.Key, fy + 1, row.Value, 1, fp)
                        End If





                    Next
                End If
                d.SubmitChanges()
                Dim Staff = StaffBrokerFunctions.GetStaffbyStaffId(bud.First.StaffId)
                If ToStatus = StaffRmb.RmbStatus.Submitted Then
                    'Send Email to Approver

                    'Dim Auth = UserController.GetUserById(PortalId, Settings("AuthUser"))
                    'Dim AuthAuth = UserController.GetUserById(PortalId, Settings("AuthAuthUser"))


                    Dim leaders = StaffBrokerFunctions.GetLeaders(Staff.UserId1)
                    If Staff.UserId2 > 0 Then
                        leaders.AddRange(StaffBrokerFunctions.GetLeaders(Staff.UserId2))
                    End If

                    If leaders.Count = 0 Then
                        leaders.Add(42)
                    End If
                    Dim Message As String = StaffBrokerFunctions.GetTemplate("mpdSubmitted", PortalId)
                    Message = Message.Replace("[STAFFNAME]", Staff.DisplayName).Replace("[BUDGETDETAIL]", GetDetailTable(bud.First))




                    For Each leader In leaders
                        Dim l = UserController.GetUserById(PortalId, leader)

                        DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", l.Email, "", "MPD Budget for " & Staff.DisplayName, Message.Replace("[BUTTONS]", GetApproveButtons(bud.First.ApproveCode, leader)).Replace("[APPROVER]", l.FirstName), "", "HTML", "", "", "", "")

                    Next
                    'Dim ConfMessage As String = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("AdvConfirmation", PortalId))



                ElseIf ToStatus = StaffRmb.RmbStatus.Approved Then
                    'Send Email to Staff Member
                    Dim Emessage As String = StaffBrokerFunctions.GetTemplate("mpdApproved", PortalId)

                    Emessage = Emessage.Replace("[STAFFNAME]", Staff.DisplayName)
                    Emessage = Emessage.Replace("[APPROVER]", UserInfo.DisplayName)
                    Dim dt As New Date(Left(bud.First.BudgetPeriodStart, 4), Right(bud.First.BudgetPeriodStart, 2), 1)
                    Emessage = Emessage.Replace("[STARTPERIOD]", dt.ToString("MMMM yyyy"))
                    Dim toEmail = Staff.User.Email

                    If Staff.UserId2 > 0 Then
                        toEmail &= "; " & Staff.User2.Email
                    End If
                    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", toEmail, "", "Budget Approved", Emessage, "", "HTML", "", "", "", "")






                End If

                Response.Redirect(Request.Url.ToString)

            End If


        End Sub
        Private Function GetApproveButtons(ByVal Code As String, ByVal apprId As String) As String
            Dim encrypt = Server.UrlEncode(AgapeEncryption.AgapeEncrypt.Encrypt(Code & ";" & apprId))

            Dim rtn = "<div style=""width: 100%; text-align: center; font-size: x-large;""><a href='" & Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & "DesktopModules/AgapeConnect/mpdCalc/Approve.aspx?m=" & encrypt & "' target='_blank'>" & "Approve" & "</a>"
            Dim returnURL = NavigateURL() & "?sb=" & Request.QueryString("sb")




            rtn &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='" & NavigateURL(PortalSettings.LoginTabId) & "?returnurl=" & Server.UrlEncode(returnURL) & "' target='_blank'>" & "Login" & "</a></div>"
            Return rtn
        End Function

        Private Function GetDetailTable(ByVal theBud As AP_mpdCalc_StaffBudget) As String
            Dim dt As New Date(Left(theBud.BudgetPeriodStart, 4), Right(theBud.BudgetPeriodStart, 2), 1)



            Dim rtn As String = "<div style='float:left; font-style: italic; color: #AAC;'>" & dt.ToString("MMM yyyy") & "</div><div style='clear: both;' />"
            rtn &= "<table style='width: 100%; margin: 10px; text-align: left;'>"
            rtn &= "<tr><th style='text-align: left;'>Name</th><th style='text-align: left;'>Monthly</th><th style='text-align: left;'>Yearly</th></tr>"

            For Each row In theBud.AP_mpdCalc_Answers.Where(Function(c) c.Value <> 0).OrderBy(Function(c) c.AP_mpdCalc_Question.AP_mpdCalc_Section.Number).ThenBy(Function(c) c.AP_mpdCalc_Question.QuestionNumber)
                rtn &= "<tr><td>" & row.Name & "</td><td>" & row.Value.ToString() & "</td><td>" & (row.Value * 12).ToString() & "</td></tr>"
            Next

            rtn &= "<tr><td  style='text-align: right; font-weight: bold;'> Total Budget:</td><td>" & theBud.TotalBudget.Value.ToString("f0") & "</td><td></td></tr>"
            If theBud.ToRaise <> theBud.TotalBudget.Value Then
                rtn &= "<tr><td  style='text-align: right;'> I am responsible to Raise:</td><td>" & theBud.ToRaise.ToString("f0") & "</td><td></td></tr>"

            End If

            rtn &= "</table>"
            Return rtn

        End Function

        Private Sub AddBudgetFromPeriod(ByVal RC As String, ByVal Account As String, ByVal FiscalYear As String, ByVal Value As Double, ByVal FromPeriod As Integer, ByVal ToPeriod As Integer)
            Dim b As New Budget.BudgetDataContext
            Dim q = From c In b.AP_Budget_Summaries Where c.Portalid = PortalId And c.RC = RC And c.Account = Account And c.FiscalYear = FiscalYear

            If q.Count > 0 Then

                q.First.Changed = True
                q.First.Error = False
                q.First.ErrorMessage = "From MPD Calculator"
                q.First.LastUpdated = Now

                q.First.P1 = CDbl(IIf(FromPeriod <= 1 And ToPeriod >= 1, Value, 0))
                q.First.P2 = CDbl(IIf(FromPeriod <= 2 And ToPeriod >= 2, Value, 0))
                q.First.P3 = CDbl(IIf(FromPeriod <= 3 And ToPeriod >= 3, Value, 0))
                q.First.P4 = CDbl(IIf(FromPeriod <= 4 And ToPeriod >= 4, Value, 0))
                q.First.P5 = CDbl(IIf(FromPeriod <= 5 And ToPeriod >= 5, Value, 0))
                q.First.P6 = CDbl(IIf(FromPeriod <= 6 And ToPeriod >= 6, Value, 0))
                q.First.P7 = CDbl(IIf(FromPeriod <= 7 And ToPeriod >= 7, Value, 0))
                q.First.P8 = CDbl(IIf(FromPeriod <= 8 And ToPeriod >= 8, Value, 0))
                q.First.P9 = CDbl(IIf(FromPeriod <= 9 And ToPeriod >= 9, Value, 0))
                q.First.P10 = CDbl(IIf(FromPeriod <= 10 And ToPeriod >= 10, Value, 0))
                q.First.P11 = CDbl(IIf(FromPeriod <= 11 And ToPeriod >= 11, Value, 0))
                q.First.P12 = CDbl(IIf(FromPeriod <= 12 And ToPeriod >= 12, Value, 0))


            ElseIf Value <> 0 Then
                Dim insert As New Budget.AP_Budget_Summary
                insert.Portalid = PortalId
                insert.Account = Account
                insert.Changed = True
                insert.Error = False
                insert.ErrorMessage = "From MPD Calculator"
                insert.LastUpdated = Now
                insert.RC = RC
                insert.FiscalYear = FiscalYear

                insert.P1 = CDbl(IIf(FromPeriod <= 1 And ToPeriod >= 1, Value, 0.0))
                insert.P2 = CDbl(IIf(FromPeriod <= 2 And ToPeriod >= 2, Value, 0))
                insert.P3 = CDbl(IIf(FromPeriod <= 3 And ToPeriod >= 3, Value, 0))
                insert.P4 = CDbl(IIf(FromPeriod <= 4 And ToPeriod >= 4, Value, 0))
                insert.P5 = CDbl(IIf(FromPeriod <= 5 And ToPeriod >= 5, Value, 0))
                insert.P6 = CDbl(IIf(FromPeriod <= 6 And ToPeriod >= 6, Value, 0))
                insert.P7 = CDbl(IIf(FromPeriod <= 7 And ToPeriod >= 7, Value, 0))
                insert.P8 = CDbl(IIf(FromPeriod <= 8 And ToPeriod >= 8, Value, 0))
                insert.P9 = CDbl(IIf(FromPeriod <= 9 And ToPeriod >= 9, Value, 0))
                insert.P10 = CDbl(IIf(FromPeriod <= 10 And ToPeriod >= 10, Value, 0))
                insert.P11 = CDbl(IIf(FromPeriod <= 11 And ToPeriod >= 11, Value, 0))
                insert.P12 = CDbl(IIf(FromPeriod <= 12 And ToPeriod >= 12, Value, 0))

                b.AP_Budget_Summaries.InsertOnSubmit(insert)

            End If
            b.SubmitChanges()
        End Sub


        Private Function getFiscalYear(ByVal CalendarString As String, ByVal FirstFiscalMonth As String) As Integer
            If CInt(Right(CalendarString, 2)) < FirstFiscalMonth Then
                Return CInt(Left(CalendarString, 4)) - 1
            Else
                Return Left(CalendarString, 4)
            End If
        End Function
        Private Function GetFiscalPeriod(ByVal CalendarString As String, ByVal FirstFiscalMonth As String) As Integer

            Dim tmp = CInt(Right(CalendarString, 2)) - ((FirstFiscalMonth) - 1)
            If tmp < 1 Then
                Return tmp + 12
            Else
                Return tmp
            End If

        End Function


        Private Sub AddToBudget(ByRef budgetValues As Dictionary(Of String, Double), ByVal AccountCode As String, ByVal GrossAmount As Double)
            If budgetValues.ContainsKey(AccountCode) Then
                budgetValues(AccountCode) += GrossAmount
            Else
                budgetValues.Add(AccountCode, GrossAmount)
            End If
        End Sub


        Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
            SaveBudget(StaffRmb.RmbStatus.Submitted)

        End Sub
        Public Function GetMaxQuestionNumber(ByVal questions As System.Data.Linq.EntitySet(Of MPD.AP_mpdCalc_Question)) As Integer
            If questions.Count = 0 Then
                Return 1
            Else
                Return questions.Max(Function(c) c.QuestionNumber) + 1
            End If
        End Function



        Protected Sub btnInsertSection_Click(sender As Object, e As EventArgs) Handles btnInsertSection.Click
            Dim d As New MPD.MPDDataContext

            Dim def = From c In d.AP_mpdCalc_Definitions Where c.TabModuleId = TabModuleId And c.PortalId = PortalId
            If def.Count > 0 Then
                Dim i As Integer = 1
                For Each row In def.First.AP_mpdCalc_Sections.OrderBy(Function(c) c.Number)
                    If ddlInsertOrder.SelectedValue = i Then
                        'insert

                        i += 1
                    End If
                    row.Number = i
                    i += 1
                Next
                Dim insert As New MPD.AP_mpdCalc_Section
                insert.mpdDefId = def.First.mpdDefId
                insert.Name = tbInsertSectionName.Text
                insert.TotalMode = "monthly"
                insert.Number = ddlInsertOrder.SelectedValue
                d.AP_mpdCalc_Sections.InsertOnSubmit(insert)
                d.SubmitChanges()
                ReloadPage()
            End If

        End Sub
        Private Sub ReloadPage()
            Response.Redirect(EditUrl("mpdCalc") & "?sb=" & StaffBudId)
        End Sub

        Protected Sub rpSections_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpSections.ItemCommand
            If e.CommandName = "EditSectionTitle" Then

                Dim d As New MPD.MPDDataContext
                Dim q = From c In d.AP_mpdCalc_Sections Where c.SectionId = CInt(e.CommandArgument) And c.AP_mpdCalc_Definition.PortalId = PortalId

                If q.Count > 0 Then
                    Dim Title As TextBox = rpSections.Items(e.Item.ItemIndex).FindControl("tbSectionName")
                    q.First.Name = Title.Text
                    d.SubmitChanges()
                    ReloadPage()
                End If
            ElseIf e.CommandName = "UP" Then
                Dim d As New MPD.MPDDataContext

                Dim q = From c In d.AP_mpdCalc_Sections Where c.SectionId = CInt(e.CommandArgument) And c.AP_mpdCalc_Definition.PortalId = PortalId


                If q.Count > 0 Then
                    Dim i As Integer = 1
                    Dim NewViewOrder = Math.Max(q.First.Number - 1, 1)


                    For Each row In q.First.AP_mpdCalc_Definition.AP_mpdCalc_Sections.Where(Function(c) c.SectionId <> q.First.SectionId).OrderBy(Function(c) c.Number)
                        If NewViewOrder = i Then
                            'skip if current index

                            i += 1
                        End If
                        row.Number = i
                        i += 1
                    Next
                    q.First.Number = NewViewOrder

                    d.SubmitChanges()
                    ReloadPage()
                End If

            ElseIf e.CommandName = "DOWN" Then
                Dim d As New MPD.MPDDataContext

                Dim q = From c In d.AP_mpdCalc_Sections Where c.SectionId = CInt(e.CommandArgument) And c.AP_mpdCalc_Definition.PortalId = PortalId


                If q.Count > 0 Then
                    Dim i As Integer = 1
                    Dim NewViewOrder = Math.Min(q.First.Number + 1, q.First.AP_mpdCalc_Definition.AP_mpdCalc_Sections.Max(Function(c) c.Number))


                    For Each row In q.First.AP_mpdCalc_Definition.AP_mpdCalc_Sections.Where(Function(c) c.SectionId <> q.First.SectionId).OrderBy(Function(c) c.Number)
                        If NewViewOrder = i Then
                            'skip if current index

                            i += 1
                        End If
                        row.Number = i
                        i += 1
                    Next

                    q.First.Number = NewViewOrder


                    d.SubmitChanges()
                    ReloadPage()



                End If
            ElseIf e.CommandName = "DeleteSection" Then
                Dim d As New MPD.MPDDataContext

                Dim q = From c In d.AP_mpdCalc_Sections Where c.SectionId = CInt(e.CommandArgument) And c.AP_mpdCalc_Definition.PortalId = PortalId


                Dim defid = q.First.mpdDefId

                d.AP_mpdCalc_Sections.DeleteAllOnSubmit(q)
                d.SubmitChanges()

                Dim i As Integer = 1


                For Each row In d.AP_mpdCalc_Sections.Where(Function(c) c.mpdDefId = defid).OrderBy(Function(c) c.Number)

                    row.Number = i
                    i += 1
                Next



                d.SubmitChanges()

                ReloadPage()
            End If
        End Sub

        Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            SaveBudget()
        End Sub

        Protected Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
            SaveBudget(StaffRmb.RmbStatus.Approved)
        End Sub

        Protected Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
            SaveBudget(StaffRmb.RmbStatus.Processed)


        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            SaveBudget(StaffRmb.RmbStatus.Cancelled)
        End Sub

        Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
            Response.Redirect(NavigateURL())
        End Sub

       

    
    End Class
End Namespace
