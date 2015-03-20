Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Data.OleDb
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.IO
Imports System.Net
Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffRmb
Imports StaffBroker
Imports DotNetNuke.Services.FileSystem
Namespace DotNetNuke.Modules.StaffRmbMod
    Partial Class ViewStaffRmb
        Inherits Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable

#Region "Properties"
        Dim d As New StaffRmbDataContext
        Dim ds As New StaffBrokerDataContext
        Dim theControl As Object
        Dim objEventLog As New DotNetNuke.Services.Log.EventLog.EventLogController
        'Dim SpouseList As IQueryable(Of StaffBroker.User)  '= AgapeStaffFunctions.SpouseIsLeader()
        Dim VAT3ist As String() = {"111X", "112X", "113", "116", "514X"}

#End Region

#Region "Page Events"

        'Private Sub AddClientActionOld(ByVal Title As String, ByVal theScript As String, ByRef root As DotNetNuke.Entities.Modules.Actions.ModuleAction)
        '    Dim jsAction As New DotNetNuke.Entities.Modules.Actions.ModuleAction(ModuleContext.GetNextActionID)
        '    With jsAction
        '        .Title = Title
        '        .CommandName = DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent
        '        .ClientScript = theScript
        '        .Secure = Security.SecurityAccessLevel.Edit
        '    End With
        '    root.Actions.Add(jsAction)
        'End Sub
        Protected Sub Page_Load1(sender As Object, e As System.EventArgs) Handles Me.Load
            hfPortalId.Value = PortalId
            lblMovedMenu.Visible = IsEditable


            For i As Integer = 2 To hfRows.Value
                Dim insert As New TableRow()
                Dim insertDesc As New TableCell()
                Dim insertAmt As New TableCell()
                Dim tbDesc As New TextBox()
                ' tbDesc.ID = "tbDesc" & i
                tbDesc.Width = Unit.Percentage(100)
                tbDesc.CssClass = "Description"
                Dim tbAmt As New TextBox()
                tbAmt.Width = Unit.Pixel(100)
                '  tbAmt.ID = "tbAmt" & i
                tbAmt.CssClass = "Amount"
                tbAmt.Attributes.Add("onblur", "calculateTotal();")
                insertDesc.Controls.Add(tbDesc)
                insertAmt.Controls.Add(tbAmt)

                insert.Cells.Add(insertDesc)
                insert.Cells.Add(insertAmt)
                tblSplit.Rows.Add(insert)
            Next
        End Sub



        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Init

            'Dim addTitle = New DotNetNuke.Entities.Modules.Actions.ModuleAction(GetNextActionID, "AgapeConnect", "AgapeConnect", "", "", "", "", True, SecurityAccessLevel.Edit, True, False)

            'MyBase.Actions.Insert(0, addTitle)


            'addTitle.Actions.Add(GetNextActionID, "Settings", "RmbSettings", "", "action_settings.gif", EditUrl("RmbSettings"), False, SecurityAccessLevel.Edit, True, False)

            'AddClientActionOld("Download Batched Transactions", "showDownload()", addTitle)
            'AddClientActionOld("Suggested Payments", "showSuggestedPayments()", addTitle)

            'For Each a As DotNetNuke.Entities.Modules.Actions.ModuleAction In addTitle.Actions
            '    If a.Title = "Download Batched Transactions" Or a.Title = "Suggested Payments" Then
            '        a.Icon = "FileManager/Icons/xls.gif"
            '    End If
            'Next
           
            If Not Page.IsPostBack And Request.QueryString("RmbNo") <> "" Then
                hfRmbNo.Value = CInt(Request.QueryString("RmbNo"))
            End If

            lblError.Visible = False
            If Not String.IsNullOrEmpty(Settings("NoReceipt")) Then
                hfNoReceiptLimit.Value = Settings("NoReceipt")
            End If

            'DownloadPeriodReport(7, 2013)


            If Not Page.IsPostBack Then
                If Settings("isLoaded") = "" Then
                    LoadDefaultSettings()
                End If
                Try
                    If Not ddlBankAccount.Items.FindByValue(Settings("BankAccount")) Is Nothing Then


                        ddlBankAccount.SelectedValue = CStr(Settings("BankAccount"))
                    End If
                Catch ex As Exception

                End Try
                If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then
                    tbAccountCode.Visible = True
                    tbCostCenter.Visible = True
                    ddlAccountCode.Visible = False
                    ddlCostcenter.Visible = False

                End If
                If StaffBrokerFunctions.GetSetting("ZA-Mode", PortalId) = "True" Then
                    cbExpenses.Enabled = False
                    cbExpenses.Checked = True
                    cbSalaries.Checked = True
                    cbSalaries.Enabled = False

                End If
                ddlDownloadExpenseYEar.Items.Clear()
                ddlDownloadExpenseYEar.Items.Add(Today.Year - 1)
                ddlDownloadExpenseYEar.Items.Add(Today.Year)
                ddlDownloadExpenseYEar.Items.Add(Today.Year + 1)
                ddlDownloadExpenseYEar.SelectedValue = (Today.Year)

                ddlDownloadExpensePeriod.Items.Clear()
                For i As Integer = 1 To 12
                    ddlDownloadExpensePeriod.Items.Add(New ListItem(MonthName(i), i))
                Next
                ddlDownloadExpensePeriod.SelectedValue = Today.AddMonths(-1).Month

                
                hfAccountingCurrency.Value = StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId)
                If hfAccountingCurrency.Value = "" Then
                    hfAccountingCurrency.Value = "USD"
                    StaffBrokerFunctions.SetSetting("AccountingCurrency", "USD", PortalId)
                End If

                Dim staff = StaffBrokerFunctions.GetStaffMember(UserId)
                Dim PayOnly As Boolean = False
                Dim PAC = StaffBrokerFunctions.GetStaffProfileProperty(staff.StaffId, "PersonalAccountCode")
                Dim CC = ""
                If staff Is Nothing Then
                    'Cannot Use
                    lblError.Text = "Access Denied. You have not been setup as a staff member on this website. Please ask your accounts team or administrator to setup your staff profile."

                    lblError.Visible = True
                    pnlEverything.Visible = False
                    Return
                ElseIf staff.CostCenter = Nothing And PAC = "" Then

                    'cannot use
                    lblError.Text = "Access Denied. Your account has not been setup with a valid Responsibility Center. Please ask your accounts team or administrator to setup your staff profile."
                    lblError.Visible = True
                    pnlEverything.Visible = False
                    Return


                Else
                    CC = staff.CostCenter
                    PayOnly = StaffBrokerFunctions.GetStaffProfileProperty(staff.StaffId, "PayOnly")
                    'PAC = StaffBrokerFunctions.GetStaffProfileProperty(staff.StaffId, "PersonalAccountCode")
                    If CC = "" And PAC = "" Then
                        'cannot use
                        lblError.Text = "Access Denied. Your account has not been setup with a valid Responsibility Center. Please ask your accounts team or administrator to setup your staff profile."
                        lblError.Visible = True
                        pnlEverything.Visible = False
                        Return
                    End If


                End If



                Dim q = From c In ds.AP_StaffBroker_Staffs Where (c.UserId1 = UserId Or c.UserId2 = UserId) And (Not PayOnly) And c.CostCenter.Trim().Length > 0 And c.PortalId = PortalId Select DisplayName = (c.DisplayName & "(" & c.CostCenter & ")"), c.CostCenter, ViewOrder = 1
                q = q.Union(From c In ds.AP_StaffBroker_Departments Where c.CanRmb = True And c.CostCentre.Length > 0 And c.PortalId = PortalId Select DisplayName = (c.Name & "(" & c.CostCentre & ")"), CostCenter = c.CostCentre, ViewOrder = 2)

                ddlNewChargeTo.DataSource = From c In q Order By c.ViewOrder, c.DisplayName
                ddlNewChargeTo.DataTextField = "DisplayName"
                ddlNewChargeTo.DataValueField = "CostCenter"
                ddlNewChargeTo.DataBind()

                ddlChargeTo.DataSource = From c In q Order By c.ViewOrder, c.DisplayName
                ddlChargeTo.DataTextField = "DisplayName"
                ddlChargeTo.DataValueField = "CostCenter"
                ddlChargeTo.DataBind()


                GridView1.Columns(0).HeaderText = Translate("TransDate")
                GridView1.Columns(1).HeaderText = Translate("LineType")
                GridView1.Columns(2).HeaderText = Translate("Comment")
                GridView1.Columns(3).HeaderText = Translate("Amount")
                GridView1.Columns(4).HeaderText = Translate("ReceiptNo")

               

                Dim acc As Boolean = IsAccounts()
                ' btnDownloadBatch.Visible = acc
                btnAdvDownload.Visible = acc
                ' btnShowSuggestedPayments.Visible = acc
                ddlCostcenter.Enabled = acc
                ddlAccountCode.Enabled = acc
                tbCostCenter.Enabled = acc
                tbAccountCode.Enabled = acc
                pnlAccountsOptions.Visible = acc
                pnlVAT.Visible = Settings("VatAttrib")

                lblHighlight.Visible = acc
                If acc Then
                    Dim errors = From c In d.AP_Staff_Rmbs Where c.PortalId = PortalId And c.Error = True And (c.Status = RmbStatus.PendingDownload Or c.Status = RmbStatus.DownloadFailed Or c.Status = RmbStatus.Approved)

                    If errors.Count > 0 Then
                        Dim s As String = ""
                        For Each rmb In errors
                            s &= "<a href='" & NavigateURL() & "?RmbNo=" & rmb.RMBNo & "'>R" & rmb.RID & "</a>, "
                        Next
                        lblErrors.Text = Translate("Errors") & Left(s, s.Length - 2)
                        lblErrors.Visible = True
                    Else

                        lblErrors.Visible = False
                    End If
                End If



                lblDefatulSettings.Visible = (Settings("isLoaded") <> "Yes")

                hfPortalId.Value = PortalId

                pnlMain.Visible = False
                pnlSplash.Visible = True

                '  btnSettings.Visible = IsEditable





                If hfRmbNo.Value <> "" Then
                    If CInt(hfRmbNo.Value) < 0 Then
                        LoadAdv(-hfRmbNo.Value)
                    Else
                        LoadRmb(hfRmbNo.Value)
                    End If
                Else
                    ltSplash.Text = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("RmbSplash", PortalId))
                End If



                'Dim lineTypes = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId Order By c.LocalName Select c.AP_Staff_RmbLineType.LineTypeId, c.LocalName
                'ddlLineTypes.DataSource = lineTypes
                'ddlLineTypes.DataBind()

                ResetMenu()







            End If

        End Sub
        Protected Sub UpdatePanel2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdatePanel2.Load




            Try


                Dim lt = From c In d.AP_Staff_RmbLineTypes Where c.LineTypeId = ddlLineTypes.SelectedValue

                If lt.Count > 0 Then



                    phLineDetail.Controls.Clear()
                    theControl = LoadControl(lt.First.ControlPath)
                    theControl.ID = "theControl"
                    phLineDetail.Controls.Add(theControl)



                End If


            Catch ex As Exception

            End Try




        End Sub
#End Region
#Region "Loading Functions"
        Public Sub ResetMenu()


            Try



                Dim MenuSize = Settings("MenuSize")
                Dim isAcc = IsAccounts()
                Dim Spouse = From c In ds.AP_StaffBroker_Staffs Where c.UserId1 = UserId Or c.UserId2 = UserId Select c.UserId1, c.UserId2
                If Spouse.Count = 0 Then
                    Return
                End If
                Dim SpouseId = -1
                If Spouse.First.UserId1 = UserId Then
                    SpouseId = Spouse.First.UserId2
                Else
                    SpouseId = Spouse.First.UserId1
                End If


                Dim SpouseList = StaffBrokerFunctions.GetTeam(SpouseId)



                'Select Reimbursements that you submitted that are awaiting approval
                Dim allStaff As IQueryable(Of StaffBroker.User)
                Dim Team = StaffBrokerFunctions.GetTeam(UserId)
                Dim CostCentres = StaffBrokerFunctions.GetDepartments(UserId)
                If isAcc Then
                    AgapeLogger.WriteEventLog(UserId, "loading menu")
                    pnlSubmittedView.Visible = False
                    pnlApprovedAcc.Visible = True
                    pnlApprovedView.Visible = False

                    allStaff = StaffBrokerFunctions.GetStaff()
                    Dim AllStaffNode As New TreeNode("All Staff")
                    AllStaffNode.SelectAction = TreeNodeSelectAction.Expand
                    AllStaffNode.Expanded = False
                    Dim AllStaffNode2 As New TreeNode("All Staff")
                    AllStaffNode2.SelectAction = TreeNodeSelectAction.Expand
                    AllStaffNode2.Expanded = False
                    For Each person In allStaff
                        '  AgapeLogger.WriteEventLog(UserId, person.DisplayName)
                        '  AgapeLogger.WriteEventLog(UserId, "loading Submitted")
                        Dim Submitted = (From c In d.AP_Staff_Rmbs Where c.Status = RmbStatus.Submitted And c.PortalId = PortalId And (c.UserId = person.UserID) Order By c.RID Descending Select c.RMBNo, c.RmbDate, c.UserRef, c.RID, c.UserId).Take(MenuSize)
                        Dim node As New TreeNode(person.DisplayName)
                        node.Expanded = False
                        node.SelectAction = TreeNodeSelectAction.Expand
                        For Each row In Submitted
                            Dim node2 As New TreeNode()
                            node2.Text = GetRmbTitleTeamShort(row.RID, row.RmbDate)
                            node2.NavigateUrl = NavigateURL() & "?RmbNo=" & row.RMBNo

                            node.ChildNodes.Add(node2)

                            If IsSelected(row.RMBNo) Then
                                node.Expanded = True
                                AllStaffNode.Expanded = True
                            End If
                        Next
                        '     AgapeLogger.WriteEventLog(UserId, "loading Submitted Adv")
                        Dim SubmittedAdv = (From c In d.AP_Staff_AdvanceRequests Where c.RequestStatus = RmbStatus.Submitted And c.PortalId = PortalId And c.UserId = person.UserID Order By c.LocalAdvanceId Descending Select c.AdvanceId, c.RequestDate, c.LocalAdvanceId).Take(MenuSize)
                        For Each row In SubmittedAdv
                            Dim node2 As New TreeNode()
                            node2.Text = GetAdvTitleTeamShort(row.LocalAdvanceId, row.RequestDate)
                            node2.NavigateUrl = NavigateURL() & "?RmbNo=" & -row.AdvanceId
                            node.ChildNodes.Add(node2)
                            If IsSelected(-row.AdvanceId) Then
                                node.Expanded = True
                                AllStaffNode.Expanded = True
                            End If
                        Next
                        '     AgapeLogger.WriteEventLog(UserId, "loading Processed")
                        Dim nodeB As New TreeNode(person.DisplayName)
                        nodeB.Expanded = False
                        nodeB.SelectAction = TreeNodeSelectAction.Expand
                        Dim Processed = (From c In d.AP_Staff_Rmbs Where c.Status = RmbStatus.Processed And c.PortalId = PortalId And (c.UserId = person.UserID) Order By c.RID Descending Select c.RMBNo, c.RmbDate, c.UserRef, c.RID, c.UserId).Take(MenuSize)
                        For Each row In Processed
                            Dim node2 As New TreeNode()
                            node2.Text = GetRmbTitleTeamShort(row.RID, row.RmbDate)
                            node2.NavigateUrl = NavigateURL() & "?RmbNo=" & row.RMBNo

                            nodeB.ChildNodes.Add(node2)
                            If IsSelected(row.RMBNo) Then
                                nodeB.Expanded = True
                                AllStaffNode2.Expanded = True
                            End If
                        Next
                        '  AgapeLogger.WriteEventLog(UserId, "loading processed Adv")
                        Dim ProcessedAdv = (From c In d.AP_Staff_AdvanceRequests Where c.RequestStatus = RmbStatus.Processed And c.PortalId = PortalId And c.UserId = person.UserID Order By c.LocalAdvanceId Descending Select c.AdvanceId, c.RequestDate, c.LocalAdvanceId).Take(MenuSize)
                        For Each row In ProcessedAdv
                            Dim node2 As New TreeNode()
                            node2.Text = GetAdvTitleTeamShort(row.LocalAdvanceId, row.RequestDate)
                            node2.NavigateUrl = NavigateURL() & "?RmbNo=" & -row.AdvanceId
                            nodeB.ChildNodes.Add(node2)
                            If IsSelected(-row.AdvanceId) Then
                                nodeB.Expanded = True
                                AllStaffNode2.Expanded = True
                            End If
                        Next


                        AllStaffNode2.ChildNodes.Add(nodeB)
                        AllStaffNode.ChildNodes.Add(node)
                    Next
                    tvProcessed.Nodes.Clear()
                    tvAllSubmitted.Nodes.Clear()
                    tvAllSubmitted.Nodes.Add(AllStaffNode)
                    tvProcessed.Nodes.Add(AllStaffNode2)

                    AgapeLogger.WriteEventLog(UserId, "loading approved")
                    Dim AllApproved = (From c In d.AP_Staff_Rmbs
                                       Where (c.Status = RmbStatus.Approved Or c.Status >= RmbStatus.PendingDownload) And c.PortalId = PortalId Order By c.RID Descending
               Select c.RMBNo, c.RmbDate, c.UserRef, c.RID, c.UserId, c.Status, Receipts = ((c.AP_Staff_RmbLines.Where(Function(x) x.Receipt And (x.ReceiptImageId Is Nothing))).Count > 0)).Take(MenuSize)

                    Dim AllApprovedAdv = (From c In d.AP_Staff_AdvanceRequests Where (c.RequestStatus = RmbStatus.Approved Or c.RequestStatus >= RmbStatus.PendingDownload) And c.PortalId = PortalId Order By c.LocalAdvanceId Descending).Take(MenuSize)

                    Dim rec = From c In AllApproved Where c.Status = RmbStatus.Approved And c.Receipts


                    Dim nonRec = From c In AllApproved Where c.Status = RmbStatus.Approved And Not c.Receipts



                    Dim nonRecAdv = From c In AllApprovedAdv Where c.RequestStatus = RmbStatus.Approved



                    Dim PendingDownload = From c In AllApproved Where c.Status >= RmbStatus.PendingDownload

                    Dim PendingDownloadAdv = From c In AllApprovedAdv Where c.RequestStatus >= RmbStatus.PendingDownload

                    dlReceipts.DataSource = rec
                    dlReceipts.DataBind()
                    dlNoReceipts.DataSource = nonRec
                    dlNoReceipts.DataBind()
                    dlAdvNoReceipts.DataSource = nonRecAdv
                    dlAdvNoReceipts.DataBind()
                    dlAdvNoReceipts.AlternatingItemStyle.CssClass = IIf(dlNoReceipts.Items.Count Mod 2 = 1, "dnnGridItem", "dnnGridAltItem")
                    dlAdvNoReceipts.ItemStyle.CssClass = IIf(dlNoReceipts.Items.Count Mod 2 = 1, "dnnGridAltItem", "dnnGridItem")

                    '  AgapeLogger.WriteEventLog(UserId, "loading pending download")

                    If PendingDownload.Count + PendingDownloadAdv.Count = 0 Then
                        pnlPendingDownload.Visible = False
                    Else
                        pnlPendingDownload.Visible = True
                        dlPendingDownload.DataSource = PendingDownload
                        dlPendingDownload.DataBind()
                        dlAdvPendingDownload.DataSource = PendingDownloadAdv
                        dlAdvPendingDownload.DataBind()
                        dlAdvPendingDownload.AlternatingItemStyle.CssClass = IIf(dlPendingDownload.Items.Count Mod 2 = 1, "dnnGridItem", "dnnGridAltItem")
                        dlAdvPendingDownload.ItemStyle.CssClass = IIf(dlPendingDownload.Items.Count Mod 2 = 1, "dnnGridAltItem", "dnnGridItem")

                    End If
                    If AllApproved.Count + AllApprovedAdv.Count > 0 Then
                        lblToProcess.Text = "(" & AllApproved.Count + AllApprovedAdv.Count & ")"
                        pnlToProcess.CssClass = "ui-state-highlight ui-corner-all"
                    Else
                        lblToProcess.Text = ""
                        pnlToProcess.CssClass = ""
                    End If



                Else
                    'Select Reimbursements that you haven't submitted yet
                    Dim MoreInfo = From c In d.AP_Staff_Rmbs Where c.MoreInfoRequested = True And c.Status <> RmbStatus.Processed And c.Status <> RmbStatus.Processed And c.PortalId = PortalId And c.UserId = UserId Select c.UserRef, c.RID, c.RMBNo

                    For Each row In MoreInfo
                        Dim hyp As New HyperLink()
                        hyp.CssClass = "ui-state-highlight ui-corner-all AgapeWarning"
                        hyp.Font.Size = FontUnit.Small
                        hyp.Font.Bold = True

                        hyp.Text = Translate("MoreInfo").Replace("[RMBNO]", row.RID).Replace("[USERREF]", row.UserRef)
                        hyp.NavigateUrl = NavigateURL() & "?RmbNo=" & row.RMBNo


                        PlaceHolder1.Controls.Add(hyp)
                    Next


                    Dim Pending = (From c In d.AP_Staff_Rmbs Where c.Status = RmbStatus.Draft And c.PortalId = PortalId And (c.UserId = UserId) Order By c.RID Descending Select c.RMBNo, c.RmbDate, c.UserRef, c.RID, c.UserId).Take(MenuSize)
                    dlPending.DataSource = Pending
                    dlPending.DataBind()


                    pnlSubmittedView.Visible = True
                    pnlApprovedAcc.Visible = False
                    pnlApprovedView.Visible = True
                    Dim Submitted = (From c In d.AP_Staff_Rmbs Where c.Status = RmbStatus.Submitted And c.PortalId = PortalId And (c.UserId = UserId) Order By c.RID Descending Select c.RMBNo, c.RmbDate, c.UserRef, c.RID, c.UserId).Take(MenuSize)
                    dlSubmitted.DataSource = Submitted
                    dlSubmitted.DataBind()

                    Dim AdvSubmitted = (From c In d.AP_Staff_AdvanceRequests Where c.RequestStatus = RmbStatus.Submitted And c.PortalId = PortalId And c.UserId = UserId Order By c.LocalAdvanceId Descending Select c.AdvanceId, c.RequestDate, c.LocalAdvanceId, UserId).Take(MenuSize)
                    dlAdvSubmitted.DataSource = AdvSubmitted
                    dlAdvSubmitted.DataBind()

                    dlAdvSubmitted.AlternatingItemStyle.CssClass = IIf(dlSubmitted.Items.Count Mod 2 = 1, "dnnGridItem", "dnnGridAltItem")
                    dlAdvSubmitted.ItemStyle.CssClass = IIf(dlSubmitted.Items.Count Mod 2 = 1, "dnnGridAltItem", "dnnGridItem")


                    'Select Reimbursements that you have to approve

                    'Create a list to add things into
                    Dim list As New ArrayList
                    Dim Advlist As New ArrayList
                    Dim LargeTransaction As Integer = Settings("TeamLeaderLimit")
                    'Fill the variables: get your team, your cost centres and create a blank 'to approve' list

                    ' Dim ToApprove = From c In d.AP_Staff_Rmbs Where 1 = 0 Select c.RMBNo, c.RmbDate, c.UserRef, c.UserId, c.RID

                    'Add in your team's reimbursements that are awaiting approval
                    For Each row In From c In Team Where c.UserID <> UserId

                        Dim tAdv = (From c In d.AP_Staff_AdvanceRequests Where c.UserId = row.UserID And c.RequestStatus = RmbStatus.Submitted And (c.RequestAmount < LargeTransaction) And c.PortalId = PortalId
                                   Select c.AdvanceId, c.RequestDate, c.LocalAdvanceId, c.UserId)
                        For Each row2 In tAdv
                            Advlist.Add(row2)
                        Next




                    Next






                    Dim Over1000 = From c In d.AP_Staff_RmbLines Where c.GrossAmount > LargeTransaction And c.AP_Staff_Rmb.PortalId = PortalId Select c.AP_Staff_Rmb Distinct

                    Dim Over1000Adv = From c In d.AP_Staff_AdvanceRequests Where c.RequestAmount > LargeTransaction And c.PortalId = PortalId

                    If UserId = Settings("AuthUser") Then


                        'Adv over Limit
                        Dim tAdv = (From c In Over1000Adv Where c.UserId <> UserId And c.RequestStatus = RmbStatus.Submitted
                                  Select c.AdvanceId, c.RequestDate, c.LocalAdvanceId, c.UserId)
                        For Each row2 In tAdv
                            Advlist.Add(row2)
                        Next

                        'Spouse is team leader
                        'If SpouseList.Count > 0 Then
                        '    For Each row In SpouseList
                        '        Dim SpouseRmbs = From c In d.AP_Staff_Rmbs Where c.UserId = row.UserID And c.Status = RmbStatus.Submitted And c.UserId <> UserId And c.PortalId = PortalId Select c.RMBNo, c.RmbDate, c.UserRef, c.UserId
                        '        If SpouseRmbs.Count > 0 Then
                        '            For Each rmb In SpouseRmbs
                        '                list.Add(rmb)
                        '            Next
                        '        End If
                        '    Next
                        'End If

                    End If

                    'Get AuthUser's Departmental Reimbursements for Auth Auth User
                    If UserId = Settings("AuthAuthUser") Then
                        Dim AuthUser = CInt(Settings("AuthUser"))


                        'Adv over Limit
                        Dim tAdv = (From c In Over1000Adv Where c.UserId = AuthUser And c.RequestStatus = RmbStatus.Submitted
                                   Select c.AdvanceId, c.RequestDate, c.LocalAdvanceId, c.UserId)
                        For Each row2 In tAdv
                            Advlist.Add(row2)
                        Next


                    End If


                    'btnToApprove.Visible = (list.Count > 0)

                    'Add the full list of reimbursements needing to be approved by the user to the data list



                    Dim submittedRmbs = From c In d.AP_Staff_Rmbs Where c.PortalId = PortalId And c.Status = RmbStatus.Submitted

                    Dim theAuthUser = UserController.GetUserById(PortalId, CInt(Settings("AuthUser")))
                    Dim TheAuthAuthUser = UserController.GetUserById(PortalId, CInt(Settings("AuthAuthUser")))

                    For Each row In submittedRmbs
                        Dim access = StaffRmbFunctions.getApprovers(row, theAuthUser, TheAuthAuthUser)
                        If access.UserIds.Contains(UserInfo) Then
                            list.Add(row)
                        End If
                    Next


                    dlToApprove.DataSource = (From c In list Order By c.RMBNo Descending)
                    dlToApprove.DataBind()
                    dlAdvToApprove.DataSource = From c In Advlist Order By c.LocalAdvanceId Descending
                    dlAdvToApprove.DataBind()
                    dlAdvToApprove.AlternatingItemStyle.CssClass = IIf(dlToApprove.Items.Count Mod 2 = 1, "dnnGridItem", "dnnGridAltItem")
                    dlAdvToApprove.ItemStyle.CssClass = IIf(dlToApprove.Items.Count Mod 2 = 1, "dnnGridAltItem", "dnnGridItem")

                    If list.Count + Advlist.Count + Submitted.Count + AdvSubmitted.Count > 0 Then
                        lblSubmittedCount.Text = "(" & list.Count + Advlist.Count + Submitted.Count + AdvSubmitted.Count & ")"
                        pnlSubmitted.CssClass = "ui-state-highlight ui-corner-all"
                    Else
                        lblSubmittedCount.Text = ""
                        pnlSubmitted.CssClass = ""
                    End If


                    'Create a list of all the cancelled reimbursements made by the user
                    Dim Cancelled = (From c In d.AP_Staff_Rmbs Where c.Status = RmbStatus.Cancelled And c.PortalId = PortalId And (c.UserId = UserId) Order By c.RID Descending Select c.RMBNo, c.RmbDate, c.UserRef, c.RID, c.UserId).Take(MenuSize)
                    dlCancelled.DataSource = Cancelled
                    dlCancelled.DataBind()

                    'Create a list of all of the approved (but un-processed) reimbursements made  by the user

                    Dim Approved = (From c In d.AP_Staff_Rmbs Where (c.Status = RmbStatus.Approved Or c.Status = RmbStatus.PendingDownload Or c.Status = RmbStatus.DownloadFailed) And c.PortalId = PortalId And (c.UserId = UserId) Order By c.RID Descending Select c.RMBNo, c.RmbDate, c.UserRef, c.RID, c.UserId).Take(MenuSize)
                    dlApproved.DataSource = Approved
                    dlApproved.DataBind()

                    Dim AdvApproved = (From c In d.AP_Staff_AdvanceRequests Where c.PortalId = PortalId And (c.RequestStatus = RmbStatus.Approved Or c.RequestStatus = RmbStatus.PendingDownload Or c.RequestStatus = RmbStatus.DownloadFailed) And c.UserId = UserId Order By c.LocalAdvanceId Descending Select c.AdvanceId, c.RequestDate, c.LocalAdvanceId, c.UserId).Take(MenuSize)

                    dlAdvApproved.DataSource = AdvApproved
                    dlAdvApproved.DataBind()
                    dlAdvApproved.AlternatingItemStyle.CssClass = IIf(dlApproved.Items.Count Mod 2 = 1, "dnnGridItem", "dnnGridAltItem")
                    dlAdvApproved.ItemStyle.CssClass = IIf(dlApproved.Items.Count Mod 2 = 1, "dnnGridAltItem", "dnnGridItem")






                    Dim myteamIds = From c In Team Select c.UserID

                    Dim myteamApproved = From c In d.AP_Staff_Rmbs Where myteamIds.Contains(c.UserId) And (c.Status = RmbStatus.Approved Or c.Status = RmbStatus.PendingDownload Or c.Status = RmbStatus.DownloadFailed) And c.PortalId = PortalId And c.UserId <> UserId Select c.RMBNo, c.RmbDate, c.UserRef, c.UserId, c.RID

                    dlTeamApproved.DataSource = myteamApproved
                    dlTeamApproved.DataBind()

                    Dim myteamAdvApproved = From c In d.AP_Staff_AdvanceRequests Where myteamIds.Contains(c.UserId) And (c.RequestStatus = RmbStatus.Approved Or c.RequestStatus = RmbStatus.PendingDownload Or c.RequestStatus = RmbStatus.DownloadFailed) And c.PortalId = PortalId And c.UserId <> UserId Select c.AdvanceId, c.RequestDate, c.UserId, c.LocalAdvanceId
                    dlAdvTeamApproved.DataSource = myteamAdvApproved
                    dlAdvTeamApproved.DataBind()
                    dlAdvTeamApproved.AlternatingItemStyle.CssClass = IIf(dlTeamApproved.Items.Count Mod 2 = 1, "dnnGridItem", "dnnGridAltItem")
                    dlAdvTeamApproved.ItemStyle.CssClass = IIf(dlTeamApproved.Items.Count Mod 2 = 1, "dnnGridAltItem", "dnnGridItem")

                    tvProcessed.Nodes.Clear()
                    'Create a list of all of the processed reimbursements made by the user
                    Dim Complete = (From c In d.AP_Staff_Rmbs Where c.Status = RmbStatus.Processed And c.PortalId = PortalId And (c.UserId = UserId) Order By c.RID Descending Select c.RMBNo, c.RmbDate, c.UserRef, c.RID, c.UserId).Take(MenuSize)
                    Dim CompleteAdv = (From c In d.AP_Staff_AdvanceRequests Where c.PortalId = PortalId And c.RequestStatus = RmbStatus.Processed And c.UserId = UserId Order By c.LocalAdvanceId Descending Select c.AdvanceId, c.RequestDate, c.LocalAdvanceId).Take(MenuSize)
                    Dim YouNode As New TreeNode("You")
                    YouNode.SelectAction = TreeNodeSelectAction.Expand
                    For Each row In Complete
                        Dim node2 As New TreeNode()
                        node2.Text = GetRmbTitleTeamShort(row.RID, row.RmbDate)
                        node2.NavigateUrl = NavigateURL() & "?RmbNo=" & row.RMBNo
                        YouNode.ChildNodes.Add(node2)
                    Next
                    For Each row In CompleteAdv
                        Dim node2 As New TreeNode()
                        node2.Text = GetAdvTitleTeamShort(row.LocalAdvanceId, row.RequestDate)
                        node2.NavigateUrl = NavigateURL() & "?RmbNo=" & -row.AdvanceId
                        YouNode.ChildNodes.Add(node2)
                    Next

                    tvProcessed.Nodes.Add(YouNode)


                    'add the approvedTeam tree
                    If Team.Count > 0 Then
                        Dim TeamNode As New TreeNode("Team")
                        TeamNode.SelectAction = TreeNodeSelectAction.Expand
                        TeamNode.Expanded = False
                        For Each row In Team
                            Dim node As New TreeNode(row.DisplayName)
                            node.Expanded = False
                            node.SelectAction = TreeNodeSelectAction.Expand
                            Dim teamApproved = From c In d.AP_Staff_Rmbs
                                Where c.UserId = row.UserID And c.Status = RmbStatus.Processed And c.PortalId = PortalId And c.Department = False Select c.RMBNo, c.RmbDate, c.UserRef, c.UserId, c.RID

                            Dim teamApprovedAdv = From c In d.AP_Staff_AdvanceRequests Where c.RequestStatus = RmbStatus.Processed And c.UserId = row.UserID And c.PortalId = PortalId Select c.AdvanceId, c.RequestDate, c.UserId, c.LocalAdvanceId

                            For Each row2 In teamApproved
                                Dim node2 As New TreeNode()
                                node2.Text = GetRmbTitleTeamShort(row2.RID, row2.RmbDate)
                                node2.NavigateUrl = NavigateURL() & "?RmbNo=" & row2.RMBNo

                                node.ChildNodes.Add(node2)
                                If IsSelected(row2.RMBNo) Then
                                    node.Expanded = True
                                    TeamNode.Expanded = True
                                End If
                            Next
                            For Each row2 In teamApprovedAdv
                                Dim node2 As New TreeNode()
                                node2.Text = GetAdvTitleTeamShort(row2.LocalAdvanceId, row2.RequestDate)
                                node2.NavigateUrl = NavigateURL() & "?RmbNo=" & -row2.AdvanceId
                                node.ChildNodes.Add(node2)
                                If IsSelected(-row2.AdvanceId) Then
                                    node.Expanded = True
                                    TeamNode.Expanded = True
                                End If
                            Next


                            TeamNode.ChildNodes.Add(node)
                        Next
                        tvProcessed.Nodes.Add(TeamNode)
                    End If

                    'add the approved Departements 
                    If CostCentres.Count > 0 Then
                        Dim DeptNode As New TreeNode("Departments")
                        DeptNode.SelectAction = TreeNodeSelectAction.Expand
                        DeptNode.Expanded = False
                        For Each row In CostCentres
                            Dim node As New TreeNode(row.Name)
                            node.Expanded = False
                            node.SelectAction = TreeNodeSelectAction.Expand
                            'Dim tCC = From c In d.AP_Staff_Rmbs Where StaffBrokerFunctions.IsDept(PortalId, c.CostCenter) And c.CostCenter = row.CostCentre And c.PortalId = PortalId And c.Status = RmbStatus.Processed And c.UserId <> UserId Select c.RMBNo, c.RmbDate, c.UserRef, c.UserId
                            Dim tCC = From c In d.AP_Staff_Rmbs
                                 Where c.CostCenter = row.CostCentre And c.PortalId = PortalId And c.Department And c.Status = RmbStatus.Processed And c.UserId <> UserId Select c.RMBNo, c.RmbDate, c.UserRef, c.UserId, c.RID

                            For Each row2 In tCC
                                Dim node2 As New TreeNode()
                                node2.Text = GetRmbTitleTeamShort(row2.RID, row2.RmbDate)
                                node2.NavigateUrl = NavigateURL() & "?RmbNo=" & row2.RMBNo
                                node.ChildNodes.Add(node2)
                                If IsSelected(row2.RMBNo) Then
                                    node.Expanded = True
                                    DeptNode.Expanded = True
                                End If
                            Next
                            DeptNode.ChildNodes.Add(node)
                        Next
                        tvProcessed.Nodes.Add(DeptNode)
                    End If
                End If

            Catch ex As Exception
                lblError.Text = "Error loading Menu: " & ex.Message
                lblError.Visible = True
            End Try




        End Sub

        Public Sub LoadAdv(ByVal AdvanceId As Integer)
            Try


                pnlMain.Visible = False
                pnlMainAdvance.Visible = True
                pnlSplash.Visible = False
                hfRmbNo.Value = -AdvanceId

                Dim q = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = AdvanceId And c.PortalId = PortalId

                If q.Count > 0 Then
                    Dim advRel As Integer = StaffRmbFunctions.AuthenticateAdv(UserId, q.First.AdvanceId, PortalId)
                    If advRel = RmbAccess.Denied And Not IsAccounts() And Not (UserId = Settings("AuthUser") Or UserId = Settings("AuthAuthUser")) Then
                        pnlMain.Visible = False
                        'Need an access denied warning
                        pnlSplash.Visible = True
                        Return
                    End If


                    lblAdvanceId.Text = ZeroFill(q.First.LocalAdvanceId, 5)
                    imgAdvAvatar.ImageUrl = GetProfileImage(q.First.UserId)
                    lblAdvStatus.Text = Translate(RmbStatus.StatusName(q.First.RequestStatus))
                    Dim StaffMember = UserController.GetUserById(PortalId, q.First.UserId)

                    btnAdvPrint.NavigateUrl = "/DesktopModules/AgapeConnect/StaffRmb/AdvPrintout.aspx?AdvNo=" & AdvanceId & "&UID=" & q.First.UserId

                    lblAdvCur.Text = "" 'StaffBrokerFunctions.GetSetting("Currency", PortalId)
                    Dim ac = StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId)
                    hfAccountingCurrency.Value = ac
                    hfOrigCurrency.Value = ac
                    hfOrigCurrencyValue.Value = q.First.RequestAmount
                    If Not String.IsNullOrEmpty(q.First.OrigCurrency) Then
                        If q.First.OrigCurrency.ToUpper <> ac.ToUpper Then
                            lblAdvCur.Text = q.First.OrigCurrencyAmount.Value.ToString("0.00") & " " & q.First.OrigCurrency.ToUpper

                            hfOrigCurrency.Value = q.First.OrigCurrency
                            hfOrigCurrencyValue.Value = q.First.OrigCurrencyAmount.Value.ToString("0.00")


                            Dim jscript As String = ""

                            hfOrigCurrencyValue.Value = q.First.OrigCurrencyAmount
                            jscript &= " $('.currency').attr('value'," & q.First.OrigCurrencyAmount & ");"

                            hfOrigCurrency.Value = q.First.OrigCurrency

                            jscript &= " $('.ddlCur').val('" & q.First.OrigCurrency & "'); checkCur();"

                            hfExchangeRate.Value = q.First.RequestAmount / q.First.OrigCurrencyAmount
                            Dim t As Type = AdvAmount.GetType()
                            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                            sb.Append("<script language='javascript'>")
                            sb.Append(jscript)
                            sb.Append("</script>")
                            ScriptManager.RegisterStartupScript(AdvAmount, t, "loadEditAdvCur", sb.ToString, False)
                        End If
                    End If


                    AdvAmount.Text = q.First.RequestAmount.Value.ToString("0.00")
                    AdvReason.Text = q.First.RequestText
                    AdvDate.Text = Translate("AdvDate").Replace("[DATE]", q.First.RequestDate.Value.ToShortDateString)
                    If q.First.ApprovedDate Is Nothing Then
                        Dim AuthUser = UserController.GetUserById(PortalId, Settings("AuthUser"))
                        Dim AuthAuthUser = UserController.GetUserById(PortalId, Settings("AuthAuthUser"))
                        Dim approvers = StaffRmbFunctions.getAdvApprovers(q.First, CDbl(Settings("TeamLeaderLimit")), AuthUser, AuthAuthUser)
                        Dim appList = ""
                        For Each row In approvers.UserIds
                            If Not row Is Nothing Then
                                appList &= row.DisplayName & ", "
                            End If

                        Next
                        lblApprovedBy.Text = Left(appList, appList.Length - 2)
                        AdvDate.Text &= "<br />" & Translate("AdvNotApproved").Replace("[STAFFNAMES]", Left(appList, appList.Length - 2))
                    Else
                        Dim Approver = UserController.GetUserById(PortalId, q.First.ApproverId).DisplayName
                        AdvDate.Text &= "<br />" & Translate("AdvApproved").Replace("[DATE]", q.First.ApprovedDate.Value.ToShortDateString).Replace("[STAFFNAME]", Approver)
                    End If
                    If Not q.First.ProcessedDate Is Nothing Then
                        AdvDate.Text &= "<br />" & Translate("AdvProcessed").Replace("[DATE]", q.First.ProcessedDate.Value.ToShortDateString)
                    End If

                    Dim theStaff = StaffBrokerFunctions.GetStaffMember(q.First.UserId)


                    AccBal.Text = "Unknown"
                    AdvBal.Text = "Unknown"
                    Dim AdvPay = From c In ds.AP_Staff_SuggestedPayments Where c.PortalId = PortalId And c.CostCenter.StartsWith(theStaff.CostCenter)

                    If AdvPay.Count > 0 Then
                        If Not AdvPay.First.AdvanceBalance Is Nothing Then
                            AdvBal.Text = StaffBrokerFunctions.GetFormattedCurrency(PortalId, AdvPay.First.AdvanceBalance.Value.ToString("0.00"))
                        End If
                        If Not AdvPay.First.AccountBalance Is Nothing Then
                            AccBal.Text = StaffBrokerFunctions.GetFormattedCurrency(PortalId, AdvPay.First.AccountBalance.Value.ToString("0.00"))
                        End If
                    End If




                    '   AccBal.Text = StaffBrokerFunctions.GetSetting("Currency", PortalId) & "3000"
                    '   AdvBal.Text = StaffBrokerFunctions.GetSetting("Currency", PortalId) & "150"

                    Select Case q.First.RequestStatus

                        Case RmbStatus.Submitted
                            btnAdvApprove.Visible = (q.First.UserId <> UserId)
                            btnAdvReject.Visible = (q.First.UserId <> UserId)
                            btnAdvSave.Visible = (q.First.UserId = UserId)
                            btnAdvCancel.Visible = (q.First.UserId = UserId)
                            btnAdvProcess.Visible = False
                            btnAdvUnProcess.Visible = False
                            If q.First.UserId = UserId Then
                                lblAdv1.Visible = False
                            Else
                                lblAdv1.Visible = True
                                lblAdv1.Text = Translate("AdvIntro").Replace("[STAFFNAME]", StaffMember.DisplayName)

                            End If

                        Case RmbStatus.Approved
                            btnAdvApprove.Visible = False
                            btnAdvCancel.Visible = False
                            btnAdvUnProcess.Visible = False
                            If IsAccounts() Then
                                btnAdvReject.Visible = True
                                btnAdvSave.Visible = True
                                btnAdvProcess.Visible = True

                            Else
                                btnAdvSave.Visible = False
                                btnAdvReject.Visible = False
                                btnAdvProcess.Visible = False

                            End If

                        Case RmbStatus.PendingDownload, RmbStatus.DownloadFailed
                            btnAdvApprove.Visible = False
                            btnAdvCancel.Visible = False
                            btnAdvProcess.Visible = False
                            btnAdvReject.Visible = False
                            btnAdvSave.Visible = False
                            If IsAccounts() Then

                                btnAdvUnProcess.Visible = True
                            Else

                                btnAdvUnProcess.Visible = False
                            End If

                        Case RmbStatus.Processed
                            btnAdvApprove.Visible = False
                            btnAdvCancel.Visible = False
                            btnAdvProcess.Visible = False
                            btnAdvReject.Visible = False
                            btnAdvSave.Visible = False
                            If IsAccounts() Then

                                btnAdvUnProcess.Visible = True
                            Else

                                btnAdvUnProcess.Visible = False
                            End If

                        Case RmbStatus.Cancelled
                            btnAdvApprove.Visible = False
                            btnAdvReject.Visible = False
                            btnAdvSave.Visible = False
                            btnAdvCancel.Visible = False
                            btnAdvProcess.Visible = False
                            btnAdvUnProcess.Visible = False
                            btnAdvApprove.Visible = False
                            btnAdvCancel.Visible = False
                            btnAdvProcess.Visible = False
                            btnAdvReject.Visible = False
                            btnAdvSave.Visible = False


                        Case Else
                            btnAdvApprove.Visible = False
                            btnAdvReject.Visible = False
                            btnAdvSave.Visible = False
                            btnAdvCancel.Visible = False
                            btnAdvProcess.Visible = False
                            btnAdvUnProcess.Visible = False
                    End Select
                    lblAdvDownloadError.Visible = False
                    If IsAccounts() Then
                        pnlAdvPeriodYear.Visible = True
                        SetYear(ddlAdvYear, q.First.Year)

                        If q.First.Error And Not String.IsNullOrEmpty(q.First.ErrorMessage) Then
                            lblAdvDownloadError.Text = q.First.ErrorMessage
                            lblAdvDownloadError.Visible = True
                        ElseIf Not String.IsNullOrEmpty(q.First.ErrorMessage) Then
                            AdvDate.Text &= "<br /> " & q.First.ErrorMessage
                        End If


                        If q.First.Period Is Nothing Then
                            ddlAdvPeriod.SelectedIndex = 0
                        Else
                            ddlAdvPeriod.SelectedValue = q.First.Period
                        End If
                    Else
                        pnlAdvPeriodYear.Visible = False
                    End If

                    AdvAmount.Enabled = btnAdvSave.Visible
                End If
            Catch ex As Exception
                lblError.Text = "Error loading Advance: " & ex.Message
                lblError.Visible = True
            End Try
        End Sub


        Public Sub SetYear(ByRef ddlYearIn As DropDownList, ByVal selectedYear As Integer?)
            ddlYearIn.Items.Clear()
            ddlYearIn.Items.Add(New ListItem("Default", ""))
            ddlYearIn.Items.Add(Year(Today) - 1)
            ddlYearIn.Items.Add(Year(Today))
            ddlYearIn.Items.Add(Year(Today) + 1)
            If selectedYear Is Nothing Then
                ddlYearIn.SelectedIndex = 0
            Else
                If selectedYear < Year(Today) - 1 Then
                    ddlYearIn.Items.Insert(0, selectedYear)
                ElseIf selectedYear > Year(Today) + 1 Then
                    ddlYearIn.Items.Add(selectedYear)
                End If
                ddlYearIn.SelectedValue = selectedYear
            End If
        End Sub

        Public Sub LoadRmb(ByVal RmbNo As Integer)
            Try


                pnlError.Visible = False
                btnSubmit.Enabled = True
                btnProcess.Enabled = True
                btnApprove.Enabled = True
                pnlMain.Visible = True
                pnlMainAdvance.Visible = False
                hfRmbNo.Value = RmbNo
                Dim q = From c In d.AP_Staff_Rmbs Where c.RMBNo = RmbNo
                If q.Count > 0 Then



                    'Dim lineTypes = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId Order By c.LocalName Select c.AP_Staff_RmbLineType.LineTypeId, c.LocalName, c.PCode, c.DCode

                    'If q.First.Department Then
                    '    lineTypes = lineTypes.Where(Function(x) Not String.IsNullOrEmpty(x.DCode))

                    'Else
                    '    lineTypes = lineTypes.Where(Function(x) Not String.IsNullOrEmpty(x.DCode))
                    'End If
                    'ddlLineTypes.DataSource = lineTypes
                    'ddlLineTypes.DataBind()



                    Dim RmbRel As Integer = StaffRmbFunctions.Authenticate(UserId, RmbNo, PortalId)

                    If RmbRel = RmbAccess.Denied And Not IsAccounts() And Not (UserId = Settings("AuthUser") Or UserId = Settings("AuthAuthUser")) Then
                        pnlMain.Visible = False
                        'Need an access denied warning
                        pnlSplash.Visible = True
                        Return
                    End If
                    pnlMain.Visible = True
                    pnlSplash.Visible = False



                    btnPrint.OnClientClick = "window.open('/DesktopModules/AgapeConnect/StaffRmb/RmbPrintout.aspx?RmbNo=" & RmbNo & "&UID=" & q.First.UserId & "', '_blank'); "
                    btnSubmit.OnClientClick = "window.open('/DesktopModules/AgapeConnect/StaffRmb/RmbPrintout.aspx?RmbNo=" & RmbNo & "&UID=" & q.First.UserId & "&mode=1', '_blank'); "


                    lblRmbNo.Text = ZeroFill(q.First.RID, 5)
                    imgAvatar.ImageUrl = GetProfileImage(q.First.UserId)

                    If Not q.First.UserRef Is Nothing Then
                        tbYouRef.Text = q.First.UserRef
                    Else
                        tbYouRef.Text = ""

                    End If
                    If Not q.First.RmbDate Is Nothing Then
                        lblSubmittedDate.Text = q.First.RmbDate.Value.ToShortDateString
                    Else
                        lblSubmittedDate.Text = ""
                    End If

                    If q.First.ApprUserId Is Nothing Then
                        Dim AuthUser = UserController.GetUserById(PortalId, Settings("AuthUser"))
                        Dim AuthAuthUser = UserController.GetUserById(PortalId, Settings("AuthAuthUser"))
                        Dim approvers = StaffRmbFunctions.getApprovers(q.First, AuthUser, AuthAuthUser)
                        Dim appList = ""
                        For Each row In approvers.UserIds
                            If Not row Is Nothing Then
                                appList &= row.DisplayName & ", "
                            End If

                        Next
                        If (appList.Length > 2) Then
                            lblApprovedBy.Text = Left(appList, appList.Length - 2)
                        Else
                            lblApprovedBy.Text = ""
                        End If


                        ttlWaitingApp.Visible = True
                        ttlApprovedBy.Visible = False
                        lblApprovedDate.Text = ""
                    Else
                        ttlWaitingApp.Visible = False
                        ttlApprovedBy.Visible = True
                        Dim approver = UserController.GetUserById(PortalId, q.First.ApprUserId)
                        lblApprovedBy.Text = approver.DisplayName
                        lblApprovedDate.Text = q.First.ApprDate.Value.ToShortDateString

                    End If

                    If q.First.ProcUserId Is Nothing Then
                        lblProcessedBy.Text = ""
                    Else
                        lblProcessedBy.Text = UserController.GetUserById(PortalId, q.First.ProcUserId).DisplayName
                    End If
                    If q.First.ProcDate Is Nothing Then
                        lblProcessedDate.Text = ""
                    Else
                        lblProcessedDate.Text = q.First.ProcDate.Value.ToShortDateString

                    End If

                    If Not q.First.ApprDate Is Nothing Then
                        lblApprovedDate.Text = q.First.ApprDate.Value.ToShortDateString
                    Else
                        lblApprovedDate.Text = ""
                    End If


                    ttlYourComments.Visible = (q.First.UserId = UserId)
                    ttlUserComments.Visible = (q.First.UserId <> UserId)

                    If q.First.MoreInfoRequested Is Nothing Then
                        cbMoreInfo.Checked = False
                    Else
                        cbMoreInfo.Checked = q.First.MoreInfoRequested
                    End If



                    lblStatus.Text = Translate(RmbStatus.StatusName(q.First.Status))
                    Dim findcc = ddlChargeTo.Items.FindByValue(q.First.CostCenter)
                    If findcc Is Nothing Then
                        ddlChargeTo.Items.Add(New ListItem(q.First.CostCenter, q.First.CostCenter))
                        ddlCostcenter.Items.Add(New ListItem(q.First.CostCenter, q.First.CostCenter))
                    End If
                    ddlChargeTo.SelectedValue = q.First.CostCenter






                    tbComments.Text = q.First.UserComment
                    lbComments.Text = q.First.UserComment



                    If Not q.First.ApprComment Is Nothing Then
                        lblApprComments.Text = q.First.ApprComment
                        tbApprComments.Text = q.First.ApprComment
                    Else
                        lblApprComments.Text = ""
                        tbApprComments.Text = ""
                    End If
                    If Not q.First.AcctComment Is Nothing Then

                        lblAccComments.Text = q.First.AcctComment
                        tbAccComments.Text = q.First.AcctComment

                    Else
                        lblAccComments.Text = ""
                        tbAccComments.Text = ""
                    End If
                    Dim theUser = UserController.GetUserById(PortalId, q.First.UserId)
                    lblSubBy.Text = theUser.DisplayName
                    staffInitials.Value = theUser.FirstName.Substring(0, 1) & theUser.LastName.Substring(0, 1)


                    lblWrongType.Visible = False





                    ' btnAddLine.Visible = Not q.First.Locked
                    ' tbComments.Enabled = Not q.First.Locked
                    ' tbYouRef.Enabled = Not q.First.Locked

                    'Try


                    Dim theStaff = StaffBrokerFunctions.GetStaffMember(q.First.UserId)
                    'Dim AdvanceCode As String = ""
                    'Dim accountCode As String = ""

                    'If Not String.IsNullOrEmpty(theStaff.CostCenter) Then


                    '    AdvanceCode = theStaff.CostCenter.Trim() & "-" & StaffBrokerFunctions.GetSetting("AdvanceSuffix", PortalId).Trim()


                    '    accountCode = q.First.CostCenter.Trim
                    'Else

                    'End If



                    Dim PACMode = (String.IsNullOrEmpty(theStaff.CostCenter) And StaffBrokerFunctions.GetStaffProfileProperty(theStaff.StaffId, "PersonalAccountCode") <> "")


                    pnlAdvance.Visible = (q.First.AP_Staff_RmbLines.Count > 0) And Not PACMode




                    'Dim JsonMessage As String = "GetAccountBalance('" & GetJsonAccountString(accountCode) & "'); "
                    'JsonMessage &= "GetAdvanceBalance('" & GetJsonAccountString(AdvanceCode) & "'); "
                    'SendMessage("", JsonMessage, True)


                    lblAccountBalance.Text = "Unknown"
                    lblAdvanceBalance.Text = "Unknown"
                    hfAccountBalance.Value = 0.0
                    Dim AdvPay = From c In ds.AP_Staff_SuggestedPayments Where c.PortalId = PortalId And c.CostCenter.StartsWith(theStaff.CostCenter)

                    If AdvPay.Count > 0 Then
                        If Not AdvPay.First.AdvanceBalance Is Nothing Then
                            lblAdvanceBalance.Text = StaffBrokerFunctions.GetFormattedCurrency(PortalId, AdvPay.First.AdvanceBalance.Value.ToString("0.00"))
                        End If

                    End If

                    Dim AccPay = From c In ds.AP_Staff_SuggestedPayments Where c.PortalId = PortalId And c.CostCenter.StartsWith(q.First.CostCenter)

                    If AccPay.Count > 0 Then
                        If Not AccPay.First.AccountBalance Is Nothing Then
                            lblAccountBalance.Text = StaffBrokerFunctions.GetFormattedCurrency(PortalId, AccPay.First.AccountBalance.Value.ToString("0.00"))
                            hfAccountBalance.Value = AccPay.First.AccountBalance.Value
                        End If

                    End If



                    GridView1.DataSource = q.First.AP_Staff_RmbLines
                    GridView1.DataBind()
                    
                    pnlTaxable.Visible = (From c In q.First.AP_Staff_RmbLines Where c.Taxable = True).Count > 0

                    'StaffBrokerFunctions.GetSetting("DataserverURL", PortalId)
                    '   Dim CountryURL = "https://tntdataserver.eu/dataserver/devtest/dataquery/dataqueryservice.asmx"

                    '  AccountCode


                    ' Dim dsa As New DSAccount(CountryURL, UserInfo.Profile.GetPropertyValue("ssoGUID"), "012226", New Date(2012, 1, 1), Today, "")


                    '  lblAccountBalance.Text = dsa.BalanceForAccount("012226")

                    'lblAccountBalance.Text = DSAccount.getAccountBalance(UserInfo.Profile.GetPropertyValue("ssoGUID"), UserInfo.Profile.GetPropertyValue("GCXPGTIOU"), CountryURL, "012226")











                    'lblLoanBalance.Text = dsa.Transactions.Count
                    'Dim dsa As New DSAccount(CountryURL, UserInfo.Profile.GetPropertyValue("ssoGUID"))
                    '  Dim dsa As New DSAccount(CountryURL, UserInfo.Profile.GetPropertyValue("ssoGUID"))
                    'Dim Dsa As New DSAccount("https://tntdataserver.eu/dataserver/devtest/dataquery/DataQueryService.asmx", UserInfo.Profile.GetPropertyValue("ssoGUID"), AccountCode)
                    ' lblAccBal.Text = dsa.BalanceForAccount(AccountCode)




                    ' lblLoanBalance.Text = Dsa.Account.Countries.Where(Function(p) p.Name = "devtest").First.Profiles.First.Accounts.Where(Function(p) p.AccountID = AccountCode).First.Balance

                    '  Dsa.AccountBalanceForAccount(StaffBrokerFunctions.GetStaffMember(q.First.UserId).CostCenter.Trim() & "-" & StaffBrokerFunctions.GetSetting("AdvanceSuffix", PortalId).Trim(), ).ToString("0.00")

                    ' Catch ex As Exception
                    'lblLoanBalance.Text = ex.Message
                    'End Try


                    ' lblLoanBalance.Text = StaffBrokerFunctions.GetStaffMember(q.First.UserId).CostCenter.Trim() & "-" & StaffBrokerFunctions.GetSetting("AdvanceSuffix", PortalId).Trim()
                    ' lblLoanBalance.Visible = True

                    If q.First.AdvanceRequest = Nothing Then
                        tbAdvanceAmount.Text = ""
                    Else
                        tbAdvanceAmount.Text = q.First.AdvanceRequest.ToString("0.00", New CultureInfo("en-US").NumberFormat)
                    End If



                    Select Case q.First.Status
                        Case RmbStatus.Draft, RmbStatus.MoreInfo
                            ddlChargeTo.Enabled = True
                            btnSubmit.Visible = True
                            btnSave.Visible = True
                            btnSaveAdv.Visible = True
                            btnCancel.Visible = True
                            btnPrint.Visible = True
                            btnApprove.Visible = False
                            addLinebtn2.Visible = True
                            tbYouRef.Enabled = True
                            tbAdvanceAmount.Enabled = True
                        Case RmbStatus.Submitted
                            ddlChargeTo.Enabled = False
                            btnSubmit.Visible = False
                            btnSave.Visible = True
                            btnSaveAdv.Visible = True
                            btnCancel.Visible = True
                            btnPrint.Visible = True
                            btnApprove.Visible = (q.First.UserId <> UserId)
                            addLinebtn2.Visible = True
                            tbYouRef.Enabled = False
                            tbAdvanceAmount.Enabled = True
                        Case RmbStatus.Approved
                            ddlChargeTo.Enabled = False
                            btnSubmit.Visible = False
                            btnSave.Visible = True
                            btnSaveAdv.Visible = True
                            btnCancel.Visible = True
                            btnPrint.Visible = True

                            btnApprove.Visible = False
                            addLinebtn2.Visible = False
                            tbYouRef.Enabled = False
                            tbAdvanceAmount.Enabled = True
                        Case RmbStatus.PendingDownload, RmbStatus.DownloadFailed
                            ddlChargeTo.Enabled = False
                            btnSubmit.Visible = False
                            btnSave.Visible = False
                            btnSaveAdv.Visible = False
                            btnCancel.Visible = False
                            btnPrint.Visible = True
                            btnApprove.Visible = False
                            addLinebtn2.Visible = False
                            tbYouRef.Enabled = False
                            tbAdvanceAmount.Enabled = False
                            cbMoreInfo.Visible = False
                        Case RmbStatus.Processed
                            ddlChargeTo.Enabled = False
                            btnSubmit.Visible = False
                            btnSave.Visible = False
                            btnSaveAdv.Visible = False
                            btnCancel.Visible = False
                            btnPrint.Visible = True
                            btnApprove.Visible = False
                            addLinebtn2.Visible = False
                            tbYouRef.Enabled = False
                            tbAdvanceAmount.Enabled = False
                            cbMoreInfo.Visible = False
                        Case RmbStatus.Cancelled
                            ddlChargeTo.Enabled = True
                            btnSubmit.Visible = True
                            btnSave.Visible = True
                            btnSaveAdv.Visible = True
                            btnCancel.Visible = True
                            btnPrint.Visible = True
                            btnApprove.Visible = False
                            addLinebtn2.Visible = True
                            tbYouRef.Enabled = True
                            tbAdvanceAmount.Enabled = True
                        Case Else
                            ddlChargeTo.Enabled = False
                            btnSubmit.Visible = False
                            btnSave.Visible = False
                            btnSaveAdv.Visible = False
                            btnCancel.Visible = False
                            btnPrint.Visible = False
                            btnApprove.Visible = False
                            ddlChargeTo.Enabled = False
                            addLinebtn2.Visible = False
                            tbYouRef.Enabled = False
                            tbAdvanceAmount.Enabled = False
                    End Select



                    If IsAccounts() And (q.First.UserId = UserId Or q.First.Status = RmbStatus.Approved Or q.First.Status = RmbStatus.Processed Or q.First.Status >= RmbStatus.PendingDownload) Then

                        btnProcess.Visible = (q.First.Status <> RmbStatus.Processed And q.First.Status <> RmbStatus.PendingDownload And q.First.Status <> RmbStatus.DownloadFailed)
                        If q.First.UserId = UserId And q.First.Status <> RmbStatus.Approved Then
                            btnProcess.Visible = False
                        End If

                        btnUnProcess.Visible = (q.First.Status = RmbStatus.Processed Or q.First.Status = RmbStatus.PendingDownload Or q.First.Status = RmbStatus.DownloadFailed)

                        addLinebtn2.Visible = (q.First.Status <> RmbStatus.Processed And q.First.Status <> RmbStatus.PendingDownload And q.First.Status <> RmbStatus.DownloadFailed)
                        btnSubmit.Visible = False
                        btnApprove.Visible = False
                        btnDownload.Visible = True
                        pnlPeriodYear.Visible = True
                        SetYear(ddlYear, q.First.Year)



                        If q.First.Period Is Nothing Then
                            ddlPeriod.SelectedIndex = 0
                            'ddlPeriod.SelectedValue = Month(Now)
                        Else
                            ddlPeriod.SelectedValue = q.First.Period
                        End If

                        cbMoreInfo.Enabled = True



                    Else
                        btnDownload.Visible = False
                        cbMoreInfo.Enabled = False

                        btnProcess.Visible = False
                        btnUnProcess.Visible = False
                        pnlPeriodYear.Visible = False
                    End If


                    Select Case RmbRel
                        Case RmbAccess.Owner, RmbAccess.Spouse
                            tbComments.Visible = True
                            tbComments.Enabled = True
                            lbComments.Visible = False
                            tbApprComments.Visible = False
                            lblApprComments.Visible = True
                        Case RmbAccess.Approver, RmbAccess.Leader
                            tbComments.Visible = False
                            lbComments.Visible = True
                            tbApprComments.Visible = True
                            tbApprComments.Enabled = True
                            lblApprComments.Visible = False


                    End Select






                    If IsAccounts() Then
                        tbAccComments.Visible = True
                        tbAccComments.Enabled = True
                        lblAccComments.Visible = False
                        cbMoreInfo.Visible = q.First.Status = RmbStatus.Approved

                        If q.First.Error Then
                            pnlError.Visible = True
                            lblErrorMessage.Text = q.First.ErrorMessage & "<br /><i>" & Translate("ErrorHelp") & "</i>"

                        End If
                        If q.First.Status < RmbStatus.Processed Then
                            ddlChargeTo.Enabled = True
                        End If

                    Else
                        tbAccComments.Visible = False
                        lblAccComments.Visible = True
                        cbMoreInfo.Visible = False
                    End If
                    If q.First.Status >= RmbStatus.Processed Then
                        tbComments.Enabled = False
                        tbApprComments.Enabled = False
                        tbAccComments.Enabled = False
                    End If
                    'pnlMain.Visible = True
                    'pnlSplash.Visible = False  
                Else
                    pnlMain.Visible = False
                    pnlSplash.Visible = True
                End If


            Catch ex As Exception
                lblError.Text = "Error loading Rmb: " & ex.Message
                StaffBrokerFunctions.EventLog("Error loading Rmb", ex.ToString, UserId)

                lblError.Visible = True
            End Try

        End Sub
#End Region

#Region "Button Events"
        Protected Sub btnAddLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLine.Click


            Dim ucType As Type = theControl.GetType()

            If btnAddLine.CommandName = "Save" Then
                Dim theUserId = (From c In d.AP_Staff_Rmbs Where c.RMBNo = hfRmbNo.Value Select c.UserId).First
                If ucType.GetMethod("ValidateForm").Invoke(theControl, New Object() {theUserId}) = True Then

                    Dim q = From c In d.AP_Staff_RmbLines Where c.RmbNo = hfRmbNo.Value And c.Receipt Select c.ReceiptNo

                    'Dim AccType = Right(ddlChargeTo.SelectedValue, 1)


                    Dim insert As New AP_Staff_RmbLine
                    insert.Comment = CStr(ucType.GetProperty("Comment").GetValue(theControl, Nothing))

                    insert.GrossAmount = CDbl(ucType.GetProperty("Amount").GetValue(theControl, Nothing))

                    'Look for currency conversion

                    If hfCurOpen.Value = "false" Or String.IsNullOrEmpty(hfOrigCurrency.Value) Or hfOrigCurrency.Value = StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId) Then
                        insert.OrigCurrency = StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId)
                        insert.OrigCurrencyAmount = insert.GrossAmount
                    Else
                        insert.OrigCurrency = hfOrigCurrency.Value
                        insert.OrigCurrencyAmount = hfOrigCurrencyValue.Value
                    End If
                    Dim LineTypeName = d.AP_Staff_RmbLineTypes.Where(Function(c) c.LineTypeId = CInt(ddlLineTypes.SelectedValue)).First.TypeName.ToString()


                    insert.ShortComment = GetLineComment(insert.Comment, insert.OrigCurrency, insert.OrigCurrencyAmount, tbShortComment.Text, False, Nothing, IIf(LineTypeName = "Mileage", CStr(ucType.GetProperty("Spare2").GetValue(theControl, Nothing)), ""))


                    If insert.GrossAmount >= Settings("TeamLeaderLimit") Then
                        insert.LargeTransaction = True
                    Else
                        insert.LargeTransaction = False
                    End If
                    insert.LineType = CInt(ddlLineTypes.SelectedValue)
                    insert.TransDate = CDate(ucType.GetProperty("theDate").GetValue(theControl, Nothing))

                    Dim age = DateDiff(DateInterval.Month, insert.TransDate, Today)
                    If ddlOverideTax.SelectedIndex > 0 Then
                        insert.Taxable = (ddlOverideTax.SelectedValue = 1)
                        If (age > Settings("Expire")) Then
                            insert.OutOfDate = True
                            insert.ForceTaxOrig = True
                        Else
                            insert.OutOfDate = False
                            insert.ForceTaxOrig = CBool(ucType.GetProperty("Taxable").GetValue(theControl, Nothing))
                        End If
                    Else
                        insert.ForceTaxOrig = Nothing
                        Dim theCC = From c In d.AP_StaffBroker_CostCenters Where c.CostCentreCode = SelectedCC And c.PortalId = PortalId And c.Type = CostCentreType.Department

                        If age > Settings("Expire") Then
                            Dim msg As String = ""
                            If theCC.Count > 0 Then

                                Dim staffMember = StaffBrokerFunctions.GetStaffMember(theUserId)

                                If Not String.IsNullOrEmpty(staffMember.CostCenter) Then
                                    insert.CostCenter = (staffMember.CostCenter)
                                    msg = Translate("ExpireDept").Replace("[EXPIRE]", Settings("Expire"))
                                Else
                                    msg = Translate("ExpireImpossible").Replace("[EXPIRE]", Settings("Expire"))
                                    SendMessage(msg)
                                    Return
                                End If



                            Else
                                msg = Translate("Expire").Replace("[EXPIRE]", Settings("Expire"))
                            End If





                            insert.OutOfDate = True
                            insert.Taxable = True



                            Dim t1 As Type = Me.GetType()
                            Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()
                            sb1.Append("<script language='javascript'>")
                            sb1.Append("alert(""" & msg & """);")
                            sb1.Append("</script>")
                            ScriptManager.RegisterClientScriptBlock(Page, t1, "popup", sb1.ToString, False)
                        Else
                            insert.OutOfDate = False
                            If theCC.Count > 0 Then
                                insert.Taxable = False
                            Else
                                insert.Taxable = CBool(ucType.GetProperty("Taxable").GetValue(theControl, Nothing))
                            End If


                        End If
                    End If

                    insert.VATReceipt = CBool(ucType.GetProperty("VAT").GetValue(theControl, Nothing))

                    Try
                        If cbRecoverVat.Checked And CDbl(tbVatRate.Text) > 0 Then
                            insert.VATRate = CDbl(tbVatRate.Text)
                        Else
                            insert.VATRate = Nothing
                        End If
                    Catch ex As Exception
                        insert.VATRate = Nothing
                    End Try


                    'If VAT3ist.Contains(ddlChargeTo.SelectedValue) And insert.VATReceipt Then

                    '    insert.VATCode = "3"
                    '    insert.VATRate = 20

                    '    insert.VATAmount = insert.GrossAmount / (1 + (100 / insert.VATRate))
                    'Else
                    '    insert.VATCode = 8
                    '    insert.VATAmount = 0.0
                    '    insert.VATRate = 0.0
                    'End If

                    insert.Receipt = CBool(ucType.GetProperty("Receipt").GetValue(theControl, Nothing))


                    'If insert.AP_Staff_RmbLineType.TypeName <> "Mileage" And insert.LineType <> 14 And insert.LineType <> 7 And insert.Receipt = False And (insert.GrossAmount > Settings("NoReceipt")) Then
                    '    ucType.GetProperty("ErrorText").SetValue(theControl, "*For transactions over " & Settings("NoReceipt") & ", a receipt must be supplied.", Nothing)
                    '    Return
                    'End If
                    insert.RmbNo = hfRmbNo.Value


                    Dim theFile As IFileInfo
                    Dim ElectronicReceipt As Boolean = False
                    Try
                        If (CInt(ucType.GetProperty("ReceiptType").GetValue(theControl, Nothing) = 2)) Then

                            ElectronicReceipt = True


                            Dim theFolder As IFolderInfo = FolderManager.Instance.GetFolder(PortalId, "_RmbReceipts/" & theUserId)
                            theFile = FileManager.Instance.GetFile(theFolder, "R" & hfRmbNo.Value & "LNew.jpg")


                            If Not theFile Is Nothing Then
                                'FileManager.Instance.RenameFile(theFile, "R" & hfRmbNo.Value & "L" & line.First.RmbLineNo & ".jpg")

                                insert.ReceiptImageId = theFile.FileId
                            Else
                                theFile = FileManager.Instance.GetFile(theFolder, "R" & hfRmbNo.Value & "LNew.pdf")
                                If Not theFile Is Nothing Then
                                    insert.ReceiptImageId = theFile.FileId
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        StaffBrokerFunctions.EventLog("Rmb" & hfRmbNo.Value, "Failed to Add Electronic Receipt: " & ex.ToString, UserId)

                    End Try

                    insert.Spare1 = CStr(ucType.GetProperty("Spare1").GetValue(theControl, Nothing))
                    insert.Spare2 = CStr(ucType.GetProperty("Spare2").GetValue(theControl, Nothing))
                    insert.Spare3 = CStr(ucType.GetProperty("Spare3").GetValue(theControl, Nothing))
                    insert.Spare4 = CStr(ucType.GetProperty("Spare4").GetValue(theControl, Nothing))
                    insert.Spare5 = CStr(ucType.GetProperty("Spare5").GetValue(theControl, Nothing))
                    insert.Split = False

                    'If insert.LineType = 16 Then
                    '    If insert.Spare1 = True Then
                    '        'Staff Meeting that is split
                    '        insert.Split = True
                    '    End If

                    'End If



                    ' insert.AnalysisCode = GetAnalysisCode(insert.LineType)

                    'insert.AccountCode = GetAccountCode(insert.LineType, insert.AP_Staff_Rmb.CostCenter, insert.AP_Staff_Rmb.UserId)
                    'insert.CostCenter = insert.AP_Staff_Rmb.CostCenter

                    If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then
                        insert.AccountCode = tbAccountCode.Text
                        insert.CostCenter = tbCostCenter.Text
                    Else
                        insert.AccountCode = ddlAccountCode.SelectedValue
                        insert.CostCenter = ddlCostcenter.SelectedValue
                    End If
                    If LineTypeName.Contains("Non-Donation") Then
                        insert.CostCenter = insert.Spare2
                    End If

                    If insert.Receipt Then
                        If q.Count = 0 Then
                            insert.ReceiptNo = 1
                        ElseIf q.Max Is Nothing Then
                            insert.ReceiptNo = 1
                        Else
                            insert.ReceiptNo = q.Max + 1
                        End If

                    End If



                    d.AP_Staff_RmbLines.InsertOnSubmit(insert)

                    If btnApprove.Visible Then
                        'If this the Aprrover makes a change, the staff member must be notified upon submit
                        Dim theRmb = (From c In d.AP_Staff_Rmbs Where c.RMBNo = CInt(hfRmbNo.Value)).First
                        theRmb.Changed = True
                    End If
                    d.SubmitChanges()


                    If ElectronicReceipt And Not theFile Is Nothing Then
                        FileManager.Instance.RenameFile(theFile, "R" & hfRmbNo.Value & "L" & insert.RmbLineNo & "." & theFile.Extension)


                    End If

                    LoadRmb(hfRmbNo.Value)
                    Dim t As Type = Me.GetType()
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script language='javascript'>")
                    sb.Append("closePopup();")
                    sb.Append("</script>")
                    ScriptManager.RegisterClientScriptBlock(Page, t, "", sb.ToString, False)

                End If
            ElseIf btnAddLine.CommandName = "Edit" Then
                If ucType.GetMethod("ValidateForm").Invoke(theControl, New Object() {UserId}) = True Then

                    Dim line = From c In d.AP_Staff_RmbLines Where c.RmbLineNo = CInt(btnAddLine.CommandArgument)



                    If line.Count > 0 Then

                        Dim LineTypeName = d.AP_Staff_RmbLineTypes.Where(Function(c) c.LineTypeId = CInt(ddlLineTypes.SelectedValue)).First.TypeName.ToString()









                        Dim GrossAmount = CDbl(ucType.GetProperty("Amount").GetValue(theControl, Nothing))

                        If String.IsNullOrEmpty(hfOrigCurrency.Value) Then
                            line.First.OrigCurrency = StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId)
                            line.First.OrigCurrencyAmount = line.First.GrossAmount
                        Else
                            line.First.OrigCurrency = hfOrigCurrency.Value
                            line.First.OrigCurrencyAmount = hfOrigCurrencyValue.Value
                        End If

                        Dim comment As String = CStr(ucType.GetProperty("Comment").GetValue(theControl, Nothing))
                        line.First.Comment = comment
                        Dim sc = tbShortComment.Text
                        If (sc <> line.First.ShortComment) Then
                            'the short comment was manully changed, so this should take precidence over anything else.
                            line.First.ShortComment = GetLineComment(comment, line.First.OrigCurrency, line.First.OrigCurrencyAmount, tbShortComment.Text, False, Nothing, IIf(LineTypeName = "Mileage", CStr(ucType.GetProperty("Spare2").GetValue(theControl, Nothing)), ""))
                        Else
                            line.First.ShortComment = GetLineComment(comment, line.First.OrigCurrency, line.First.OrigCurrencyAmount, "", False, Nothing, IIf(LineTypeName = "Mileage", CStr(ucType.GetProperty("Spare2").GetValue(theControl, Nothing)), ""))
                        End If
                        'If line.First.ShortComment <> comment Then
                        '    line.First.Comment = comment
                        '    If line.First.ShortComment = tbShortComment.Text Then
                        '        line.First.ShortComment = GetLineComment(comment, line.First.OrigCurrency, line.First.OrigCurrencyAmount, "", False, Nothing, IIf(LineTypeName = "Mileage", CStr(ucType.GetProperty("Spare2").GetValue(theControl, Nothing)), ""))
                        '    Else
                        '        line.First.ShortComment = GetLineComment(comment, line.First.OrigCurrency, line.First.OrigCurrencyAmount, tbShortComment.Text, False, Nothing, IIf(LineTypeName = "Mileage", CStr(ucType.GetProperty("Spare2").GetValue(theControl, Nothing)), ""))

                        '    End If


                        'Else
                        '    line.First.ShortComment = GetLineComment(comment, line.First.OrigCurrency, line.First.OrigCurrencyAmount, tbShortComment.Text, False, Nothing, IIf(LineTypeName = "Mileage", CStr(ucType.GetProperty("Spare2").GetValue(theControl, Nothing)), ""))

                        'End If



                        line.First.GrossAmount = GrossAmount

                        If line.First.GrossAmount >= Settings("TeamLeaderLimit") Then
                            line.First.LargeTransaction = True
                        Else
                            line.First.LargeTransaction = False
                        End If


                        'look for electronic receipt


                        Try
                            If (CInt(ucType.GetProperty("ReceiptType").GetValue(theControl, Nothing) = 2)) Then

                                Dim theFolder As IFolderInfo = FolderManager.Instance.GetFolder(PortalId, "_RmbReceipts/" & line.First.AP_Staff_Rmb.UserId)
                                Dim theFile = FileManager.Instance.GetFile(theFolder, "R" & line.First.RmbNo & "L" & line.First.RmbLineNo & ".jpg")
                                If Not theFile Is Nothing Then
                                    line.First.ReceiptImageId = theFile.FileId
                                Else
                                    theFile = FileManager.Instance.GetFile(theFolder, "R" & line.First.RmbNo & "L" & line.First.RmbLineNo & ".pdf")
                                    If Not theFile Is Nothing Then
                                        line.First.ReceiptImageId = theFile.FileId
                                    End If
                                End If
                            End If
                        Catch ex As Exception

                        End Try



                        If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then
                            line.First.AccountCode = tbAccountCode.Text
                            line.First.CostCenter = tbCostCenter.Text
                        Else
                            line.First.AccountCode = ddlAccountCode.SelectedValue
                            line.First.CostCenter = ddlCostcenter.SelectedValue
                        End If

                        If LineTypeName.Contains("Non-Donation") Then
                            line.First.CostCenter = line.First.Spare2
                        End If

                        line.First.LineType = CInt(ddlLineTypes.SelectedValue)
                        line.First.TransDate = CDate(ucType.GetProperty("theDate").GetValue(theControl, Nothing))
                        Dim age = DateDiff(DateInterval.Month, line.First.TransDate, Today)
                        Dim theCC = From c In d.AP_StaffBroker_CostCenters Where c.CostCentreCode = SelectedCC And c.PortalId = PortalId And c.Type = CostCentreType.Department
                        If ddlOverideTax.SelectedIndex > 0 And Not (theCC.Count > 0 And ddlOverideTax.SelectedValue = 1) Then
                            line.First.Taxable = (ddlOverideTax.SelectedValue = 1)
                            If (age > Settings("Expire")) Then
                                line.First.OutOfDate = True
                                line.First.ForceTaxOrig = True
                            Else
                                line.First.OutOfDate = False
                                line.First.ForceTaxOrig = CBool(ucType.GetProperty("Taxable").GetValue(theControl, Nothing))
                            End If

                        Else
                            line.First.ForceTaxOrig = Nothing
                            If age > Settings("Expire") Then
                                Dim msg As String = ""
                                If theCC.Count > 0 Then

                                    Dim staffMember = StaffBrokerFunctions.GetStaffMember(line.First.AP_Staff_Rmb.UserId)

                                    If Not String.IsNullOrEmpty(staffMember.CostCenter) Then
                                        line.First.CostCenter = (staffMember.CostCenter)
                                        msg = Translate("ExpireDept").Replace("[EXPIRE]", Settings("Expire"))
                                    Else
                                        msg = Translate("ExpireImpossible").Replace("[EXPIRE]", Settings("Expire"))
                                        SendMessage(msg)
                                        Return
                                    End If



                                Else
                                    msg = Translate("Expire").Replace("[EXPIRE]", Settings("Expire"))
                                End If

                                line.First.OutOfDate = True
                                line.First.Taxable = True

                                Dim t1 As Type = Me.GetType()
                                Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()
                                sb1.Append("<script language='javascript'>")
                                sb1.Append("alert(""" & msg & """);")
                                sb1.Append("</script>")
                                ScriptManager.RegisterClientScriptBlock(Page, t1, "popup", sb1.ToString, False)
                            Else
                                line.First.OutOfDate = False

                                If theCC.Count > 0 Then
                                    line.First.Taxable = False
                                Else
                                    line.First.Taxable = CBool(ucType.GetProperty("Taxable").GetValue(theControl, Nothing))
                                End If



                            End If
                        End If

                        line.First.VATReceipt = CBool(ucType.GetProperty("VAT").GetValue(theControl, Nothing))
                        'Dim AccType = Right(ddlChargeTo.SelectedValue, 1)





                        line.First.Receipt = CBool(ucType.GetProperty("Receipt").GetValue(theControl, Nothing))
                        Try
                            If cbRecoverVat.Checked And CDbl(tbVatRate.Text) > 0 Then
                                line.First.VATRate = CDbl(tbVatRate.Text)
                            Else
                                line.First.VATRate = Nothing
                            End If
                        Catch ex As Exception
                            line.First.VATRate = Nothing
                        End Try

                        'If line.First.LineType <> 9 And line.First.LineType <> 14 And line.First.LineType <> 7 And line.First.Receipt = False And (line.First.GrossAmount > Settings("NoReceipt")) Then
                        '    ucType.GetProperty("ErrorText").SetValue(theControl, "*For transactions over " & Settings("NoReceipt") & ", a receipt must be supplied.", Nothing)
                        '    Return
                        'End If

                        line.First.Spare1 = CStr(ucType.GetProperty("Spare1").GetValue(theControl, Nothing))
                        line.First.Spare2 = CStr(ucType.GetProperty("Spare2").GetValue(theControl, Nothing))
                        line.First.Spare3 = CStr(ucType.GetProperty("Spare3").GetValue(theControl, Nothing))
                        line.First.Spare4 = CStr(ucType.GetProperty("Spare4").GetValue(theControl, Nothing))
                        line.First.Spare5 = CStr(ucType.GetProperty("Spare5").GetValue(theControl, Nothing))

                        'line.First.Split = False
                        'If line.First.LineType = 16 Then
                        '    If line.First.Spare1 = True Then
                        '        'Staff Meeting that is split
                        '        line.First.Split = True
                        '    End If
                        'End If

                        ' line.First.AnalysisCode = GetAnalysisCode(line.First.LineType)

                        If line.First.Receipt Then
                            If line.First.ReceiptNo Is Nothing Then
                                Dim q = From c In d.AP_Staff_RmbLines Where c.RmbNo = hfRmbNo.Value And c.Receipt Select c.ReceiptNo
                                If q.Max Is Nothing Then
                                    line.First.ReceiptNo = 1
                                Else
                                    line.First.ReceiptNo = q.Max + 1
                                End If


                            End If
                        Else
                            line.First.ReceiptNo = Nothing
                        End If


                        If btnApprove.Visible Then
                            'If this the Aprrover makes a change, the staff member must be notified upon submit
                            Dim theRmb = (From c In d.AP_Staff_Rmbs Where c.RMBNo = CInt(hfRmbNo.Value)).First
                            theRmb.Changed = True
                        End If

                        d.SubmitChanges()
                        'If ddlLineTypes.SelectedItem.Text = "Mileage" Then
                        '    'Mileage
                        '    ucType.GetMethod("AddStaff").Invoke(theControl, New Object() {line.First.RmbLineNo})
                        '    If line.First.Spare3 <> Settings("Motorcycle") And line.First.Spare3 <> Settings("Bicycle") Then

                        '        GetMilesForYear(line.First.RmbLineNo, line.First.AP_Staff_Rmb.UserId)

                        '    End If

                        'End If


                        btnAddLine.CommandName = "Save"

                        LoadRmb(hfRmbNo.Value)

                    End If
                    Dim t As Type = Me.GetType()
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script language='javascript'>")
                    sb.Append("closePopup();")
                    sb.Append("</script>")
                    ScriptManager.RegisterClientScriptBlock(Page, t, "", sb.ToString, False)

                End If
            End If


        End Sub
        Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
            Dim insert As New AP_Staff_Rmb
            If tbNewYourRef.Text = "" Then
                insert.UserRef = Translate("Expenses")
            Else

                insert.UserRef = tbNewYourRef.Text
            End If
            insert.RID = StaffRmbFunctions.GetNewRID(PortalId)
            insert.CostCenter = ddlNewChargeTo.SelectedValue
            insert.UserComment = tbNewComments.Text
            insert.UserId = UserId
            ' insert.PersonalCC = ddlNewChargeTo.Items(0).Value
            insert.AdvanceRequest = 0.0

            insert.PortalId = PortalId

            insert.Status = RmbStatus.Draft

            insert.Locked = False

            Dim CC = From c In ds.AP_StaffBroker_Staffs Where (c.UserId1 = UserId Or c.UserId2 = UserId) Select c.CostCenter
            Dim isDept As Boolean = True

            If CC.Count > 0 Then
                insert.SupplierCode = "R" & CC.First
                If ddlNewChargeTo.SelectedValue = CC.First Then
                    isDept = False
                End If

                'Else
                '    Dim PCC = From c In ds.AP_StaffBroker_Staffs Where (c.UserId1 = UserId Or c.UserId2 = UserId) And c.PersonalCostCentre <> "" Select c.PersonalCostCentre
                '    If PCC.Count > 0 Then
                '        insert.SupplierCode = "P-" & PCC.First & "0"
                '    Else
                '        insert.SupplierCode = ""
                '    End If
            End If

            insert.Department = isDept

            btnApprove.Visible = False
            btnSubmit.Visible = True


            d.AP_Staff_Rmbs.InsertOnSubmit(insert)
            d.SubmitChanges()

            LoadRmb(insert.RMBNo)
            ResetMenu()

            Dim t As Type = ddlNewChargeTo.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")
            sb.Append("closePopup2();")
            sb.Append("</script>")
            ScriptManager.RegisterClientScriptBlock(ddlNewChargeTo, t, "", sb.ToString, False)

        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click



            Dim rmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = hfRmbNo.Value
            If rmb.Count > 0 Then



                Dim NewStatus As Integer = RmbStatus.Submitted
                Dim message As String = Translate("Printout")

                If rmb.First.Status = RmbStatus.MoreInfo Then
                    NewStatus = RmbStatus.Approved
                    message = Translate("MoreInfo")
                    rmb.First.Locked = True
                Else
                    rmb.First.Locked = False
                End If


                rmb.First.Status = NewStatus

                rmb.First.RmbDate = Now
                rmb.First.Period = Nothing
                rmb.First.Year = Nothing

                rmb.First.SpareField1 = System.Guid.NewGuid().ToString




                'If NewStatus = 2 Then
                '    rmb.First.Locked = True
                'End If
                d.SubmitChanges()
                'dlPending.DataBind()
                'dlSubmitted.DataBind()

                ResetMenu()




                LoadRmb(hfRmbNo.Value)



                btnSubmit.Visible = False


                ' blockedLink.NavigateUrl = "http://www.agape.org.uk/DesktopModules/StaffRmb/RmbPrintout.aspx?RmbNo=" & hfRmbNo.Value & "&UID=" & rmb.First.UserId




                'Send Email to Staff Member
                If NewStatus = 1 Then
                    SendStaffMail(rmb.First)
                End If


                Log(rmb.First.RMBNo, "SUBMITTED")


                'Load the printout in a new window
                Dim t As Type = Me.GetType()
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb.Append("<script language='javascript'>")
                sb.Append("alert(""" & message & """);")
                sb.Append("</script>")
                ScriptManager.RegisterStartupScript(Page, t, "popup", sb.ToString, False)







            End If
        End Sub
        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

            Dim rmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = hfRmbNo.Value
            If rmb.Count > 0 Then
                rmb.First.Status = RmbStatus.Cancelled
                d.SubmitChanges()


                If rmb.First.UserId = UserId Then
                    LoadRmb(hfRmbNo.Value)
                    ResetMenu()

                    Dim t As Type = btnCancel.GetType()
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script language='javascript'>")
                    sb.Append("selectIndex(4);")

                    sb.Append("</script>")
                    ScriptManager.RegisterStartupScript(btnCancel, t, "select4", sb.ToString, False)
                    btnCancel.Visible = False
                    Log(rmb.First.RMBNo, "CANCELLED by owner")
                Else
                    'Send an email to the end user
                    Dim Message = ""
                    Dim dr As New TemplatesDataContext
                    '  Dim ConfTemplate = From c In dr.AP_StaffBroker_Templates Where c.TemplateName = "RmbCancelled" And c.PortalId = PortalId Select c.TemplateHTML

                    Message = StaffBrokerFunctions.GetTemplate("RmbCancelled", PortalId)



                    '  If ConfTemplate.Count > 0 Then
                    'Message = Server.HtmlDecode(ConfTemplate.First)
                    ' End If

                    Dim StaffMbr = UserController.GetUserById(PortalId, rmb.First.UserId)


                    Message = Message.Replace("[STAFFNAME]", StaffMbr.FirstName)
                    Message = Message.Replace("[APPRNAME]", UserInfo.FirstName & " " & UserInfo.LastName)
                    Message = Message.Replace("[APPRFIRSTNAME]", UserInfo.FirstName)

                    Dim comments As String = ""
                    If tbApprComments.Text.Trim().Length > 0 Then
                        comments = Translate("CommentLeft").Replace("[FIRSTNAME]", UserInfo.FirstName).Replace("[COMMENT]", tbApprComments.Text)

                    End If

                    Message = Message.Replace("[COMMENTS]", comments)


                    'DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", theUser.Email, "donotreply@agape.org.uk", "Rmb#: " & hfRmbNo.Value & "-" & rmb.First.UserRef & " has been cancelled", Message, "", "HTML", "", "", "", "")
                    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", StaffMbr.Email, "", Translate("EmailCancelledSubject").Replace("[RMBNO]", rmb.First.RID).Replace("[USERREF]", rmb.First.UserRef), Message, "", "HTML", "", "", "", "")

                    'ltSplash.Text = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("RmbSplash", PortalId))

                    pnlMain.Visible = False
                    pnlSplash.Visible = True

                    Log(rmb.First.RMBNo, "CANCELLED")
                    ResetMenu()

                    Dim t As Type = btnCancel.GetType()
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script language='javascript'>")
                    sb.Append("selectIndex(0);")
                    sb.Append("</script>")
                    ScriptManager.RegisterStartupScript(btnCancel, t, "select0", sb.ToString, False)

                End If

                If btnApprove.Visible = True Then
                    btnApprove.Visible = False
                End If



            End If


        End Sub

        Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
            Dim rmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = hfRmbNo.Value
            If rmb.Count > 0 Then

                rmb.First.Status = RmbStatus.Approved
                rmb.First.Locked = True
                rmb.First.ApprDate = Now
                rmb.First.ApprUserId = UserId
                rmb.First.Period = Nothing
                rmb.First.Year = Nothing

                'SEND EMAIL TO OTHER APPROVERS
                Dim Auth = UserController.GetUserById(PortalId, Settings("AuthUser"))
                Dim AuthAuth = UserController.GetUserById(PortalId, Settings("AuthAuthUser"))
                Dim myApprovers = StaffRmbFunctions.getApprovers(rmb.First, Auth, AuthAuth)
                Dim SpouseId As Integer = StaffBrokerFunctions.GetSpouseId(rmb.First.UserId)

                Dim ObjAppr As UserInfo = UserController.GetUserById(PortalId, Me.UserId)
                Dim theUser As UserInfo = UserController.GetUserById(PortalId, rmb.First.UserId)
                Dim ApprMessage = ""
                '   Dim dr As New TemplatesDataContext
                '  Dim ConfTemplate = From c In dr.AP_StaffBroker_Templates Where c.TemplateName = "RmbApprovedEmail-ApproversVersion" And c.PortalId = PortalId Select c.TemplateHTML

                '  If ConfTemplate.Count > 0 Then
                'ApprMessage = Server.HtmlDecode(ConfTemplate.First)
                ' End If
                ApprMessage = StaffBrokerFunctions.GetTemplate("RmbApprovedEmail-ApproversVersion", PortalId)

                ApprMessage = ApprMessage.Replace("[APPRNAME]", ObjAppr.DisplayName).Replace("[RMBNO]", rmb.First.RMBNo).Replace("[STAFFNAME]", theUser.DisplayName)


                For Each row In (From c In myApprovers.UserIds Where c.UserID <> rmb.First.UserId And c.UserID <> SpouseId)
                    ApprMessage = ApprMessage.Replace("[THISAPPRNAME]", row.DisplayName)
                    'DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", row.Email, "donotreply@agape.org.uk", "Rmb#:" & hfRmbNo.Value & " has been approved by " & ObjAppr.DisplayName, ApprMessage, "", "HTML", "", "", "", "")
                    Try

                    
                    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", row.Email, "", Translate("EmailApprovedSubjectA").Replace("[RMBNO]", rmb.First.RID).Replace("[APPROVER]", ObjAppr.DisplayName), ApprMessage, "", "HTML", "", "", "", "")
                    Catch ex As Exception
                        Log(rmb.First.RMBNo, "error sending approved email to approver")
                    End Try
                Next





                'SEND APRROVE EMAIL

                Dim Emessage = ""

                ' Dim ApprovedTemp = From c In dr.AP_StaffBroker_Templates Where c.TemplateName = "RmbApprovedEmail" And PortalId = c.PortalId Select c.TemplateHTML
                Emessage = StaffBrokerFunctions.GetTemplate("RmbApprovedEmail", PortalId)
                'If ApprovedTemp.Count > 0 Then
                'Emessage = Server.HtmlDecode(ApprovedTemp.First)
                ' End If
                Emessage = Emessage.Replace("[STAFFNAME]", theUser.DisplayName).Replace("[RMBNO]", rmb.First.RID).Replace("[USERREF]", IIf(rmb.First.UserRef <> "", rmb.First.UserRef, "None"))
                Emessage = Emessage.Replace("[APPROVER]", ObjAppr.DisplayName)
                If rmb.First.Changed = True Then
                    Emessage = Emessage.Replace("[CHANGES]", ". " & Translate("EmailApproverChanged"))
                    rmb.First.Changed = False
                Else
                    Emessage = Emessage.Replace("[CHANGES]", "")
                End If
                d.SubmitChanges()
                Dim toEmail = theUser.Email
                If Settings("SendAppEmail") = "True" Then 'copy accounts if SendAppEmail is enabled
                    toEmail &= ";" & Settings("AccountsEmail")

                End If
                Try
                    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", toEmail, "", Translate("EmailApprovedSubjectP").Replace("[RMBNO]", rmb.First.RID).Replace("[USERREF]", rmb.First.UserRef), Emessage, "", "HTML", "", "", "", "")

                Catch ex As Exception
                    Log(rmb.First.RMBNo, "error sending approved email to staff member")
                End Try
             





                btnApprove.Visible = False


                ResetMenu()

                Log(rmb.First.RMBNo, "Approved")
                Dim message As String = Translate("RmbApproved").Replace("[RMBNO]", rmb.First.RID)
                Dim t As Type = btnApprove.GetType()
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb.Append("<script language='javascript'>")

                sb.Append("$('#ApprovedPane').addClass(""in"");")
                sb.Append("alert(""" & message & """);")
                sb.Append("</script>")
                ScriptManager.RegisterStartupScript(btnApprove, t, "select2", sb.ToString, False)
                btnApprove.Visible = False
                pnlMain.Visible = False
                pnlSplash.Visible = True



            End If
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveAdv.Click
            Dim RmbNo = CInt(hfRmbNo.Value)

            Dim rmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = RmbNo And c.PortalId = PortalId
            lblAdvError.Text = ""
            If rmb.Count > 0 Then
                rmb.First.UserComment = tbComments.Text
                rmb.First.UserRef = tbYouRef.Text
                rmb.First.ApprComment = tbApprComments.Text
                rmb.First.MoreInfoRequested = cbMoreInfo.Checked
                'If ddlPeriod.SelectedIndex > 0 Then
                '    rmb.First.Period = ddlPeriod.SelectedValue
                'End If
                'If ddlYear.SelectedIndex > 0 Then
                '    rmb.First.Year = ddlYear.SelectedValue
                'End If

                For Each row In (From c In rmb.First.AP_Staff_RmbLines Where c.CostCenter = rmb.First.CostCenter)
                    row.CostCenter = ddlChargeTo.SelectedValue
                Next
                rmb.First.CostCenter = ddlChargeTo.SelectedValue
                If tbAdvanceAmount.Text = "" Then
                    tbAdvanceAmount.Text = 0
                End If

                Try
                    rmb.First.AdvanceRequest = Double.Parse(tbAdvanceAmount.Text, New CultureInfo("en-US"))
                    If rmb.First.AdvanceRequest > rmb.First.AP_Staff_RmbLines.Sum(Function(x) x.GrossAmount) Then
                        rmb.First.AdvanceRequest = rmb.First.AP_Staff_RmbLines.Sum(Function(x) x.GrossAmount)
                        tbAdvanceAmount.Text = rmb.First.AdvanceRequest.ToString("0.00")
                    End If
                Catch
                    lblAdvError.Text = Translate("AdvanceError")
                    Return
                End Try

                d.SubmitChanges()
            End If
            LoadRmb(hfRmbNo.Value)

        End Sub



        Protected Sub addLinebtn2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles addLinebtn2.Click

            'ddlLineTypes_SelectedIndexChanged(Me, Nothing)


            If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then
                tbCostCenter.Text = ddlChargeTo.SelectedValue
            Else
                ddlCostcenter.SelectedValue = ddlChargeTo.SelectedValue
            End If


            ddlLineTypes.Items.Clear()
            Dim lineTypes = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId Order By c.AP_Staff_RmbLineType.SpareField2, c.LocalName Select c.AP_Staff_RmbLineType.LineTypeId, c.LocalName, c.PCode, c.DCode

            If StaffBrokerFunctions.IsDept(PortalId, ddlChargeTo.SelectedValue) Then
                lineTypes = lineTypes.Where(Function(x) x.DCode <> "")

            Else
                lineTypes = lineTypes.Where(Function(x) x.PCode <> "")
            End If
            ddlLineTypes.DataSource = lineTypes
            ddlLineTypes.DataBind()

            pnlElecReceipts.Attributes("style") = "display: none;"
            ifReceipt.Attributes("src") = "https://" & PortalSettings.DefaultPortalAlias & "/DesktopModules/AgapeConnect/StaffRmb/ReceiptEditor.aspx?RmbNo=" & hfRmbNo.Value & "&RmbLine=New"

            ResetNewExpensePopup(True)
            cbRecoverVat.Checked = False
            tbVatRate.Text = ""
            tbShortComment.Text = ""

            'PopupTitle.Text = "Add New Reimbursement Expense"
            btnAddLine.CommandName = "Save"

            hfOrigCurrency.Value = ""
            hfOrigCurrencyValue.Value = ""


           
            Dim jscript As String = ""
            jscript &= " $('#" & hfOrigCurrency.ClientID & "').attr('value', '');"
            jscript &= " $('#" & hfOrigCurrencyValue.ClientID & "').attr('value', '');"

            Dim t As Type = addLinebtn2.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")

            sb.Append(jscript & "showPopup();")
            sb.Append("</script>")
            ScriptManager.RegisterStartupScript(addLinebtn2, t, "popupAdd", sb.ToString, False)



        End Sub
        'Protected Sub btnSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSettings.Click
        '    Response.Redirect(EditUrl("RmbSettings"))
        'End Sub
        Protected Sub btnSplitAdd_Click(sender As Object, e As System.EventArgs) Handles btnSplitAdd.Click
            hfRows.Value += 1
            'tblSplit.Rows.Clear()

            'For i As Integer = 2 To hfRows.Value
            Dim insert As New TableRow()
            Dim insertDesc As New TableCell()
            Dim insertAmt As New TableCell()
            Dim tbDesc As New TextBox()
            ' tbDesc.ID = "tbDesc" & hfRows.Value
            tbDesc.Width = Unit.Percentage(100)
            tbDesc.CssClass = "Description"
            tbDesc.Text = lblOriginalDesc.Text
            Dim tbAmt As New TextBox()
            tbAmt.Width = Unit.Pixel(100)
            ' tbAmt.ID = "tbAmt" & hfRows.Value
            tbAmt.CssClass = "Amount"
            tbAmt.Attributes.Add("onblur", "calculateTotal();")
            insertDesc.Controls.Add(tbDesc)
            insertAmt.Controls.Add(tbAmt)

            insert.Cells.Add(insertDesc)
            insert.Cells.Add(insertAmt)
            tblSplit.Rows.Add(insert)
            '  Next
        End Sub

        Protected Sub btnOK_Click(sender As Object, e As System.EventArgs) Handles btnOK.Click
            Dim theLine = From c In d.AP_Staff_RmbLines Where c.RmbLineNo = CInt(hfSplitLineId.Value)
            If theLine.Count > 0 Then
                For Each row As TableRow In tblSplit.Rows
                    Dim RowAmount = CType(row.Cells(1).Controls(0), TextBox).Text
                    Dim RowDesc = CType(row.Cells(0).Controls(0), TextBox).Text
                    If RowAmount = "" Or RowDesc = "" Then
                        lblSplitError.Visible = True
                        Return
                    ElseIf CDbl(RowAmount) = 0 Then
                        lblSplitError.Visible = True
                        Return
                    End If
                    Dim insert As New AP_Staff_RmbLine()
                    insert.AnalysisCode = theLine.First.AnalysisCode
                    insert.Comment = RowDesc
                    insert.GrossAmount = CDbl(RowAmount)
                    insert.LargeTransaction = RowAmount > CDbl(Settings("TeamLeaderLimit"))
                    insert.LineType = theLine.First.LineType
                    insert.Mileage = theLine.First.Mileage
                    insert.MileageRate = theLine.First.MileageRate
                    insert.OutOfDate = theLine.First.OutOfDate
                    insert.Receipt = theLine.First.Receipt
                    insert.ReceiptNo = theLine.First.ReceiptNo
                    insert.RmbNo = theLine.First.RmbNo
                    insert.Spare1 = theLine.First.Spare1
                    insert.Spare2 = theLine.First.Spare2
                    insert.Spare3 = theLine.First.Spare3
                    insert.Spare4 = theLine.First.Spare4
                    insert.Spare5 = theLine.First.Spare5
                    insert.OrigCurrency = StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId)

                    insert.OrigCurrencyAmount = CDbl(RowAmount)
                    insert.ShortComment = GetLineComment(RowDesc, StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId), RowAmount, "", False, Nothing, "")
                    insert.ReceiptMode = theLine.First.ReceiptMode
                    insert.ReceiptImageId = theLine.First.ReceiptImageId

                    insert.Split = True
                    insert.Taxable = theLine.First.Taxable
                    insert.TransDate = theLine.First.TransDate
                    insert.VATReceipt = theLine.First.VATReceipt
                    insert.CostCenter = theLine.First.CostCenter
                    insert.AccountCode = theLine.First.AccountCode
                    d.AP_Staff_RmbLines.InsertOnSubmit(insert)
                Next
            End If
            d.AP_Staff_RmbLines.DeleteAllOnSubmit(theLine)
            d.SubmitChanges()
            lblSplitError.Visible = False
            LoadRmb(hfRmbNo.Value)


            Dim t As Type = btnOK.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")
            sb.Append("closePopupSplit();")
            sb.Append("</script>")
            ScriptManager.RegisterStartupScript(btnOK, t, "clostSplit", sb.ToString, False)

        End Sub

        Protected Sub btnProcess_Click(sender As Object, e As System.EventArgs) Handles btnProcess.Click, btnAccountWarningYes.Click



            'Mark as Pending Download in next batch.
            Dim theRmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = CInt(hfRmbNo.Value)

            'Check Balance
            If CType(sender, Button).ID = "btnProcess" And Settings("WarnIfNegative") Then
                Dim pendingBalance = GetNumericRemainingBalance(2)

                Dim RmbBalance = theRmb.First.AP_Staff_RmbLines.Where(Function(x) x.CostCenter = x.AP_Staff_Rmb.CostCenter).Sum(Function(x) x.GrossAmount)
                If RmbBalance > pendingBalance Then
                    Dim message2 = Translate("NextBatch")
                    Dim t2 As Type = Me.GetType()
                    Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb2.Append("<script language='javascript'>")
                    sb2.Append("showAccountWarning();")
                    sb2.Append("</script>")
                    ScriptManager.RegisterStartupScript(Page, t2, "popupWarning", sb2.ToString, False)
                    Return
                End If
            End If

            theRmb.First.Status = RmbStatus.PendingDownload
            theRmb.First.ProcDate = Today
            theRmb.First.MoreInfoRequested = False
            theRmb.First.ProcUserId = UserId
            d.SubmitChanges()
            LoadRmb(hfRmbNo.Value)
            ResetMenu()
            Log(theRmb.First.RMBNo, "Processed - this reimbursement will be added to the next download batch")
            Dim message = Translate("NextBatch")
            Dim t As Type = Me.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")
            sb.Append("alert(""" & message & """);")
            sb.Append("</script>")
            ScriptManager.RegisterStartupScript(Page, t, "popup", sb.ToString, False)

        End Sub

        Protected Sub btnDownload_Click(sender As Object, e As System.EventArgs) Handles btnDownload.Click
            Dim RmbNo As Integer = CInt(hfRmbNo.Value)
            Dim thisRID = (From c In d.AP_Staff_Rmbs Where c.RMBNo = RmbNo And c.PortalId = PortalId Select c.RID).First

            Dim export As String = "Account,Subaccount,Ref,Date," & GetOrderedString("Description", "Debit", "Credit", "Company", Nothing, True)


            export &= DownloadRmbSingle(CInt(hfRmbNo.Value))


            Dim attachment As String = "attachment; filename=Rmb" & ZeroFill(thisRID, 5) & ".csv"

            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.AddHeader("content-disposition", attachment)
            HttpContext.Current.Response.ContentType = "text/csv"
            HttpContext.Current.Response.AddHeader("Pragma", "public")
            HttpContext.Current.Response.Write(export)

            HttpContext.Current.Response.End()

        End Sub

        Protected Sub btnMarkProcessed_Click(sender As Object, e As System.EventArgs) Handles btnMarkProcessed.Click
            DownloadBatch(True)

            'Dim RmbList As List(Of Integer) = Session("RmbList")
            'If Not RmbList Is Nothing Then
            '    Dim q = From c In d.AP_Staff_Rmbs Where RmbList.Contains(c.RMBNo) And c.PortalId = PortalId

            '    For Each row In q
            '        row.Status = RmbStatus.Processed
            '        row.ProcDate = Now
            '        Log(row.RMBNo, "Marked as Processed - after a manual download")
            '    Next
            'End If
            'Dim AdvList As List(Of Integer) = Session("AdvList")
            'If Not AdvList Is Nothing Then

            '    Dim r = From c In d.AP_Staff_AdvanceRequests Where AdvList.Contains(c.AdvanceId) And c.PortalId = PortalId

            '    For Each row In r
            '        row.RequestStatus = RmbStatus.Processed
            '        row.ProcessedDate = Now
            '        Log(row.AdvanceId, "Advance Marked as Processed - after a manual download")
            '    Next

            'End If

            'd.SubmitChanges()




            'If hfRmbNo.Value <> "" Then
            '    If hfRmbNo.Value > 0 Then
            '        LoadRmb(CInt(hfRmbNo.Value))
            '    Else
            '        LoadAdv(-CInt(hfRmbNo.Value))
            '    End If


            'End If

            'ResetMenu()


            'Dim t As Type = Me.GetType()
            'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            'sb.Append("<script language='javascript'>")
            'sb.Append("closePopupDownload();")
            'sb.Append("</script>")
            'ScriptManager.RegisterClientScriptBlock(Page, t, "popupDownload", sb.ToString, False)
            'HttpContext.Current.Response.End()
        End Sub

        Protected Sub btnDontMarkProcessed_Click(sender As Object, e As System.EventArgs) Handles btnDontMarkProcessed.Click
            DownloadBatch()

            'Dim t As Type = Me.GetType()
            'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            'sb.Append("<script language='javascript'>")
            'sb.Append("closePopupDownload();")
            'sb.Append("</script>")
            'ScriptManager.RegisterClientScriptBlock(Page, t, "popupDownload", sb.ToString, False)
            'HttpContext.Current.Response.End()
        End Sub

        'Protected Sub btnDownloadBatch_Click(sender As Object, e As System.EventArgs) Handles btnDownloadBatch.Click
        '    DownloadBatch()

        'End Sub

        'Protected Sub btnAdvPrint_Click(sender As Object, e As System.EventArgs) Handles btnAdvPrint.Click


        '    Dim theAdv = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = -CInt(hfRmbNo.Value)


        '    Dim t As Type = btnAdvPrint.GetType()
        '    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        '    sb.Append("<script language='javascript'>")

        '    sb.Append("window.open('/DesktopModules/AgapeConnect/StaffRmb/AdvPrintout.aspx?AdvNo=" & -CInt(hfRmbNo.Value) & "&UID=" & theAdv.First.UserId & "', '_blank'); ")

        '    sb.Append("</script>")
        '    ScriptManager.RegisterStartupScript(btnAdvPrint, t, "AdvPrintOut", sb.ToString, False)



        'End Sub
        Protected Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click

            Dim theRmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = CInt(hfRmbNo.Value)


            Dim t As Type = btnPrint.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")

            sb.Append("window.open('/DesktopModules/AgapeConnect/StaffRmb/RmbPrintout.aspx?RmbNo=" & hfRmbNo.Value & "&UID=" & theRmb.First.UserId & "', '_blank'); ")

            sb.Append("</script>")
            ScriptManager.RegisterStartupScript(btnPrint, t, "printOut", sb.ToString, False)





        End Sub
        Protected Sub btnUnProcess_Click(sender As Object, e As System.EventArgs) Handles btnUnProcess.Click
            Dim theRmb = (From c In d.AP_Staff_Rmbs Where c.RMBNo = CInt(hfRmbNo.Value)).First
            If theRmb.Status = RmbStatus.Processed Then
                'If the reimbursement has already been downloaded, a warning should be displayed - but hte reimbursement can be simply unprocessed
                theRmb.Status = RmbStatus.Approved
                d.SubmitChanges()
                Log(theRmb.RMBNo, "UNPROCESSED, after it had been fully processed")
            Else
                'if it has not been downloaded, it will be downloaded very soon. We need to check if a download is already in progress.
                If StaffBrokerFunctions.GetSetting("Datapump", PortalId) = "locked" Then
                    'If a download is in progress, we need to display a "not at this time" message
                    Dim message = "This reimbursement cannot be unprocessed at this time, as it is currently being downloaded by the automatic datapump (transaction broker). You can try again in a few minutes, but be aware that it will already have been processed into your accounts program."
                    Dim t As Type = Me.GetType()
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script language='javascript'>")
                    sb.Append("alert(""" & message & """);")
                    sb.Append("</script>")
                    ScriptManager.RegisterStartupScript(Page, t, "popup", sb.ToString, False)
                    Log(theRmb.RMBNo, "Attempted unprocessed, but could not as it was in the process of being downloaded by the automatic transaction broker")
                    Return
                Else
                    'If not, we need to lock the download progress (to ensure that it is not downloaded whilsts we are playing with it
                    StaffBrokerFunctions.SetSetting("Datapump", "Locked", PortalId)
                    'Then we can unprocess it
                    theRmb.Status = RmbStatus.Approved
                    theRmb.Period = Nothing
                    theRmb.Year = Nothing
                    theRmb.ProcDate = Nothing

                    d.SubmitChanges()
                    'Then release the lock.
                    StaffBrokerFunctions.SetSetting("Datapump", "Unlocked", PortalId)
                    Log(theRmb.RMBNo, "UNPROCESSED - before it was downloaded")
                End If
            End If
            LoadRmb(CInt(hfRmbNo.Value))

            ResetMenu()






        End Sub
        Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
            If e.CommandName = "myDelete" Then
                ' d.AP_Staff_RmbLineAddStaffs.DeleteAllOnSubmit(From c In d.AP_Staff_RmbLineAddStaffs Where c.RmbLineId = CInt(e.CommandArgument))
                d.AP_Staff_RmbLines.DeleteAllOnSubmit(From c In d.AP_Staff_RmbLines Where c.RmbLineNo = CInt(e.CommandArgument))
                d.SubmitChanges()
                LoadRmb(hfRmbNo.Value)
            ElseIf e.CommandName = "myEdit" Then


                Dim theLine = From c In d.AP_Staff_RmbLines Where c.RmbLineNo = CInt(e.CommandArgument)
                If theLine.Count > 0 Then
                    'PopupTitle.Text = "Edit Reimbursement Transaction"
                    'Dim lt = From c In d.AP_Staff_RmbLineTypes Where c.LineTypeId = theLine.First.LineType
                    'If lt.Count > 0 Then
                    '    phLineDetail.Controls.Clear()
                    '    theControl = LoadControl(lt.First.ControlPath)
                    '    theControl.ID = "theControl"
                    '    phLineDetail.Controls.Add(theControl)
                    'End If
                    'theControl = Nothing


                    ddlLineTypes.Items.Clear()
                    Dim lineTypes = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId Order By c.LocalName Select c.AP_Staff_RmbLineType.LineTypeId, c.LocalName, c.PCode, c.DCode

                    If StaffBrokerFunctions.IsDept(PortalId, theLine.First.CostCenter) Then
                        lineTypes = lineTypes.Where(Function(x) x.DCode <> "")

                    Else
                        lineTypes = lineTypes.Where(Function(x) x.PCode <> "")
                    End If
                    ddlLineTypes.DataSource = lineTypes
                    ddlLineTypes.DataBind()
                    lblIncType.Visible = False
                    btnAddLine.Enabled = True

                    If lineTypes.Where(Function(x) x.LineTypeId = theLine.First.LineType).Count = 0 Then
                        ddlLineTypes.Items.Add(New ListItem(theLine.First.AP_Staff_RmbLineType.AP_StaffRmb_PortalLineTypes.Where(Function(x) x.PortalId = PortalId).First.LocalName, theLine.First.LineType))
                        '  ddlLineTypes.Items.Add(New ListItem(theLine.First.LineType,"Wrong type"))

                        'Wrong Type... needs changing!
                        lblIncType.Visible = True
                        btnAddLine.Enabled = False



                    End If




                    ddlLineTypes.SelectedValue = theLine.First.LineType
                    ddlLineTypes_SelectedIndexChanged(Me, Nothing)


                    Dim ucType As Type = theControl.GetType()
                    ucType.GetProperty("Comment").SetValue(theControl, theLine.First.Comment, Nothing)
                    ucType.GetProperty("Amount").SetValue(theControl, CDbl(theLine.First.GrossAmount), Nothing)
                    Dim jscript As String = ""
                    If (Not theLine.First.OrigCurrencyAmount Is Nothing) Then
                        hfOrigCurrencyValue.Value = theLine.First.OrigCurrencyAmount
                        jscript &= " $('#" & hfOrigCurrencyValue.ClientID & "').attr('value', '" & theLine.First.OrigCurrencyAmount & "');"
                        'jscript &= " $('.currency').attr('value'," & theLine.First.OrigCurrencyAmount & ");"
                        hfExchangeRate.Value = (theLine.First.GrossAmount / theLine.First.OrigCurrencyAmount).Value.ToString(New CultureInfo(""))
                    End If
                    If (Not String.IsNullOrEmpty(theLine.First.OrigCurrency)) Then
                        jscript &= " $('#" & hfOrigCurrency.ClientID & "').attr('value', '" & theLine.First.OrigCurrency & "');"
                        hfOrigCurrency.Value = theLine.First.OrigCurrency
                        'jscript &= " $('.ddlCur').val('" & theLine.First.OrigCurrency & "');"

                    End If

                    ucType.GetProperty("theDate").SetValue(theControl, theLine.First.TransDate, Nothing)
                    ucType.GetProperty("VAT").SetValue(theControl, theLine.First.VATReceipt, Nothing)
                    ucType.GetProperty("Receipt").SetValue(theControl, theLine.First.Receipt, Nothing)
                    ucType.GetProperty("Spare1").SetValue(theControl, theLine.First.Spare1, Nothing)
                    ucType.GetProperty("Spare2").SetValue(theControl, theLine.First.Spare2, Nothing)
                    ucType.GetProperty("Spare3").SetValue(theControl, theLine.First.Spare3, Nothing)
                    ucType.GetProperty("Spare4").SetValue(theControl, theLine.First.Spare4, Nothing)
                    ucType.GetProperty("Spare5").SetValue(theControl, theLine.First.Spare5, Nothing)

                    Dim receiptMode = 2
                    If theLine.First.VATReceipt Then
                        receiptMode = 0
                    ElseIf Not theLine.First.Receipt Then
                        receiptMode = -1
                    ElseIf theLine.First.ReceiptImageId Is Nothing Then
                        receiptMode = 1
                    ElseIf theLine.First.ReceiptImageId < 0 Then
                        receiptMode = 1

                    End If
                    Try


                        ucType.GetProperty("ReceiptType").SetValue(theControl, receiptMode, Nothing)
                    Catch ex As Exception

                    End Try

                    ucType.GetMethod("Initialize").Invoke(theControl, New Object() {Settings})
                    cbRecoverVat.Checked = False
                    If theLine.First.ForceTaxOrig Is Nothing Then
                        ddlOverideTax.SelectedIndex = 0
                    Else
                        ddlOverideTax.SelectedValue = IIf(theLine.First.Taxable, 1, 2)

                    End If
                    tbVatRate.Text = ""
                    If theLine.First.VATRate > 0 Then
                        If theLine.First.VATRate > 0 Then
                            cbRecoverVat.Checked = True
                            tbVatRate.Text = theLine.First.VATRate
                        End If
                    End If

                    tbShortComment.Text = GetLineComment(theLine.First.Comment, theLine.First.OrigCurrency, theLine.First.OrigCurrencyAmount, theLine.First.ShortComment, False, Nothing, IIf(theLine.First.AP_Staff_RmbLineType.TypeName = "Mileage", theLine.First.Spare2, ""))



                    'If ddlLineTypes.SelectedValue = 7 Then

                    '    ucType.GetMethod("LoadStaff").Invoke(theControl, New Object() {theLine.First.RmbLineNo, Settings, CanAddPass()})
                    'End If

                    btnAddLine.CommandName = "Edit"
                    btnAddLine.CommandArgument = CInt(e.CommandArgument)

                    If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then
                        tbCostCenter.Text = theLine.First.CostCenter
                        tbAccountCode.Text = theLine.First.AccountCode
                    Else
                        ddlCostcenter.SelectedValue = theLine.First.CostCenter
                        ddlAccountCode.SelectedValue = theLine.First.AccountCode
                    End If



                    ifReceipt.Attributes("src") = Request.Url.Scheme & "://" & Request.Url.Host & "/DesktopModules/AgapeConnect/StaffRmb/ReceiptEditor.aspx?RmbNo=" & theLine.First.RmbNo & "&RmbLine=" & theLine.First.RmbLineNo

                    If Not theLine.First.ReceiptImageId Is Nothing Then
                        pnlElecReceipts.Attributes("style") = ""
                    Else
                        pnlElecReceipts.Attributes("style") = "display: none;"
                    End If




                    Dim t As Type = GridView1.GetType()
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script language='javascript'>")
                    sb.Append(jscript & "showPopup();")
                    sb.Append("</script>")
                    ScriptManager.RegisterStartupScript(GridView1, t, "popupedit", sb.ToString, False)

                End If
            ElseIf e.CommandName = "mySplit" Then
                hfRows.Value = 1
                hfSplitLineId.Value = CInt(e.CommandArgument)
                lblSplitError.Visible = False
                Dim theLine = From c In d.AP_Staff_RmbLines Where c.RmbLineNo = CInt(e.CommandArgument)

                If theLine.Count > 0 Then
                    lblOriginalDesc.Text = theLine.First.Comment
                    lblOriginalAmt.Text = theLine.First.GrossAmount.ToString("0.00")
                    tbSplitDesc.Text = lblOriginalDesc.Text
                End If
                tbSplitAmt.Attributes.Add("onblur", "calculateTotal();")
                tbSplitAmt.Text = ""
                tbSplitDesc.Text = ""

                Dim t As Type = GridView1.GetType()
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb.Append("<script language='javascript'>")
                sb.Append("showPopupSplit();")
                sb.Append("</script>")
                ScriptManager.RegisterStartupScript(GridView1, t, "popupSplit", sb.ToString, False)

            ElseIf e.CommandName = "myDefer" Then
                'Try to find a deferred transactions pending reimbursement
                Dim theLine = From c In d.AP_Staff_RmbLines Where c.RmbLineNo = CInt(e.CommandArgument)
                If theLine.Count > 0 Then
                    theLine.First.Spare5 = theLine.First.RmbNo
                    Dim q = From c In d.AP_Staff_RmbLines Where c.Spare5 = theLine.First.RmbNo And c.AP_Staff_Rmb.Status = RmbStatus.Draft And c.AP_Staff_Rmb.UserId = theLine.First.AP_Staff_Rmb.UserId And c.AP_Staff_Rmb.PortalId = PortalId Select c.AP_Staff_Rmb
                    If q.Count = 0 Then

                        Dim insert As New AP_Staff_Rmb
                        insert.UserRef = "Deferred"
                        insert.AcctComment = "Contains transactions deferred from previous month"
                        insert.RID = StaffRmbFunctions.GetNewRID(PortalId)
                        insert.CostCenter = theLine.First.AP_Staff_Rmb.CostCenter

                        insert.UserComment = ""
                        insert.UserId = theLine.First.AP_Staff_Rmb.UserId
                        ' insert.PersonalCC = ddlNewChargeTo.Items(0).Value
                        insert.AdvanceRequest = 0.0

                        insert.PortalId = PortalId

                        insert.Status = RmbStatus.Draft

                        insert.Locked = False
                        insert.SupplierCode = theLine.First.AP_Staff_Rmb.SupplierCode

                        insert.Department = theLine.First.AP_Staff_Rmb.Department

                        d.AP_Staff_Rmbs.InsertOnSubmit(insert)
                        d.SubmitChanges()
                        theLine.First.AP_Staff_Rmb = insert


                    Else
                        theLine.First.AP_Staff_Rmb = q.First
                    End If


                    d.SubmitChanges()

                    LoadRmb(hfRmbNo.Value)
                    Dim theUser = UserController.GetUserById(PortalId, theLine.First.AP_Staff_Rmb.UserId)
                    Dim t As Type = GridView1.GetType()
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script language='javascript'>")
                    sb.Append("window.open('mailto:" & theUser.Email & "?subject=Reimbursment " & theLine.First.AP_Staff_Rmb.RID & ": Deferred Transactions');")
                    sb.Append("</script>")
                    ScriptManager.RegisterStartupScript(GridView1, t, "email", sb.ToString, False)

                End If
            End If

        End Sub
        Protected Sub dlTeamApproved_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlTeamApproved.ItemCommand, dlApproved.ItemCommand, dlCancelled.ItemCommand, dlToApprove.ItemCommand, dlSubmitted.ItemCommand, dlPending.ItemCommand, dlReceipts.ItemCommand, dlNoReceipts.ItemCommand, dlPendingDownload.ItemCommand, dlAdvSubmitted.ItemCommand, dlAdvToApprove.ItemCommand, dlAdvApproved.ItemCommand, dlAdvTeamApproved.ItemCommand, dlAdvNoReceipts.ItemCommand, dlAdvPendingDownload.ItemCommand

            If e.CommandName = "Goto" Then
                LoadRmb(e.CommandArgument)
                ResetMenu()
            ElseIf e.CommandName = "GotoAdvance" Then
                LoadAdv(e.CommandArgument)
                ResetMenu()
            End If
        End Sub



#End Region


        Public Property SelectedCC() As String
            Get
                Dim CC = ""
                If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then
                    CC = tbCostCenter.Text
                Else
                    CC = ddlCostcenter.SelectedValue
                End If
                Return CC
            End Get
            Set(ByVal value As String)

            End Set
        End Property


#Region "DropDownList Events"
        Protected Sub ddlLineTypes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLineTypes.SelectedIndexChanged
            If lblIncType.Visible And ddlLineTypes.SelectedIndex <> ddlLineTypes.Items.Count - 1 Then
                Dim oldValue = ddlLineTypes.SelectedValue
                ddlLineTypes.Items.Clear()
                Dim lineTypes = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId Order By c.LocalName Select c.AP_Staff_RmbLineType.LineTypeId, c.LocalName, c.PCode, c.DCode



                If StaffBrokerFunctions.IsDept(PortalId, SelectedCC) Then
                    lineTypes = lineTypes.Where(Function(x) x.DCode <> "")

                Else
                    lineTypes = lineTypes.Where(Function(x) x.PCode <> "")
                End If
                ddlLineTypes.DataSource = lineTypes
                ddlLineTypes.DataBind()
                lblIncType.Visible = False
                btnAddLine.Enabled = True


            End If

            ResetNewExpensePopup(False)


        End Sub
        Protected Sub ddlChargeTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChargeTo.SelectedIndexChanged
            'The User selected a new cost centre

            'Detect if Dept is now Personal or vica versa
            Dim Dept = StaffBrokerFunctions.IsDept(PortalId, ddlChargeTo.SelectedValue)
            Dim RmbNo = CInt(hfRmbNo.Value)
            Dim rmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = RmbNo And c.PortalId = PortalId

            If Dept <> StaffBrokerFunctions.IsDept(PortalId, rmb.First.CostCenter) Then
                'We now need to redetermine the AccountCodes

                For Each row In rmb.First.AP_Staff_RmbLines
                    If rmb.First.CostCenter = row.CostCenter Then
                        row.Department = Dept
                        row.AccountCode = GetAccountCode(row.LineType, ddlChargeTo.SelectedValue)
                        row.CostCenter = ddlChargeTo.SelectedValue
                    End If
                Next



            End If

            rmb.First.CostCenter = ddlChargeTo.SelectedValue
            rmb.First.Department = Dept
            d.SubmitChanges()


            btnSave_Click(Me, Nothing)
        End Sub

#End Region
#Region "Formatting and shortcuts"
        Public Function GetRmbTitle(ByVal UserRef As String, ByVal RID As Integer, ByVal RmbDate As Date) As String
            If String.IsNullOrEmpty(UserRef) Then
                UserRef = Translate("Expenses")
            End If

            Dim rtn = Left(UserRef, 20) & "<br />" & "<span style=""font-size: 6.5pt; color: #999999;"">#" & ZeroFill(RID.ToString, 5)
            If RmbDate > (New Date(2010, 1, 1)) Then
                rtn = rtn & ": " & RmbDate.ToShortDateString & "</span>"
            Else
                rtn = rtn & "</span>"
            End If
            Return rtn


        End Function
        Public Function GetLineComment(ByVal comment As String, ByVal Currency As String, ByVal CurrencyValue As Double, ByVal ShortComment As String, Optional ByVal includeInitials As Boolean = True, Optional ByVal explicitStaffInitals As String = Nothing, Optional ByVal Mileage As String = "") As String




            'Prefix initials  // suffix Currency   // Trim to 30 char

            Dim initials As String = ""
            If includeInitials Then
                If Not String.IsNullOrEmpty(explicitStaffInitals) Then
                    initials = UnidecodeSharpFork.Unidecoder.Unidecode(explicitStaffInitals & "-").Substring(0, 3)
                Else
                    initials = UnidecodeSharpFork.Unidecoder.Unidecode(staffInitials.Value & "-").Substring(0, 3)
                End If

            End If
            If Not String.IsNullOrEmpty(ShortComment) Then
                Return initials & UnidecodeSharpFork.Unidecoder.Unidecode(ShortComment)
            End If


            Dim CurString = ""
            If Mileage <> "" Then
                'this is a mileage expense item, so don't show currency - show milage instead.
                CurString = "-" & Mileage & Left(Settings("DistanceUnit").ToString(), 2)


            Else
                If Not String.IsNullOrEmpty(Currency) Then
                    If Currency <> StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId) Then
                        CurString = Currency & CurrencyValue.ToString("f2", New CultureInfo(""))
                        CurString = CurString.Replace(".00", "")

                    End If
                End If
            End If

            Dim c = UnidecodeSharpFork.Unidecoder.Unidecode(comment)
            Return initials & c.Substring(0, Math.Min(c.Length, 27 - CurString.Length)) & CurString

        End Function

        Private Function FormatNumber(ByVal num As Double) As String
            If (num >= 1000000) Then
                Return (num / 1000000).ToString("0.#", New CultureInfo("")) + "M"
            End If

            If (num >= 100000) Then
                Return (num / 1000).ToString("#,0", New CultureInfo("")) + "K"
            End If

            If (num >= 10000) Then
                Return (num / 1000D).ToString("0.#", New CultureInfo("")) + "K"
            End If

            Return num.ToString("#,0", New CultureInfo(""))


        End Function

        Public Function GetAdvTitle(ByVal LocalAdvanceId As Integer, ByVal RequestDate As Date) As String

            Dim rtn = "Advance:<br />" & "<span style=""font-size: 6.5pt; color: #999999;"">Adv#" & ZeroFill(LocalAdvanceId.ToString, 4)
            If RequestDate > (New Date(2010, 1, 1)) Then
                rtn = rtn & ": " & RequestDate.ToShortDateString & "</span>"
            Else
                rtn = rtn & "</span>"
            End If
            Return rtn

            ' Return Left("RMB#" & RmbNo & " " & UserController.GetUser(PortalId, UID, False).DisplayName, 24)
        End Function
        Public Function GetAdvTitleTeam(ByVal LocalAdvanceId As Integer, ByVal UID As Integer, ByVal RequestDate As Date) As String
            Dim Sm = UserController.GetUserById(PortalId, UID)

            Dim rtn = Left(Sm.FirstName & " " & Sm.LastName, 20) & "<br />" & "<span style=""font-size: 6.5pt; color: #999999;"">Adv#" & ZeroFill(LocalAdvanceId.ToString, 4)

            If RequestDate > (New Date(2010, 1, 1)) Then
                rtn = rtn & ": " & RequestDate.ToShortDateString & "</span>"
            Else
                rtn = rtn & "</span>"
            End If
            Return rtn

            ' Return Left("RMB#" & RmbNo & " " & UserController.GetUser(PortalId, UID, False).DisplayName, 24)
        End Function
        Public Function GetRmbTitleTeam(ByVal RID As Integer, ByVal UID As Integer, ByVal RmbDate As Date) As String
            Dim Sm = UserController.GetUserById(PortalId, UID)

            Dim rtn = Left(Sm.FirstName & " " & Sm.LastName, 20) & "<br />" & "<span style=""font-size: 6.5pt; color: #999999;"">#" & ZeroFill(RID.ToString, 5)
            If RmbDate > (New Date(2010, 1, 1)) Then
                rtn = rtn & ": " & RmbDate.ToShortDateString & "</span>"
            Else
                rtn = rtn & "</span>"
            End If
            Return rtn

            ' Return Left("RMB#" & RmbNo & " " & UserController.GetUser(PortalId, UID, False).DisplayName, 24)
        End Function
        Protected Function GetRmbTitleTeamShort(ByVal RID As Integer, ByVal RmbDate As Date) As String

            Dim rtn As String = "<span style=""font-size: 6.5pt; color: #999999;"">#" & ZeroFill(RID.ToString, 5)

            If RmbDate > (New Date(2010, 1, 1)) Then
                rtn = rtn & ": " & RmbDate.ToShortDateString & "</span>"
            Else
                rtn = rtn & "</span>"
            End If
            Return rtn

            ' Return Left("RMB#" & RmbNo & " " & UserController.GetUser(PortalId, UID, False).DisplayName, 24)
        End Function
        Protected Function GetAdvTitleTeamShort(ByVal LocalAdvanceId As Integer, ByVal RequestDate As Date) As String

            Dim rtn As String = "<span style=""font-size: 6.5pt; color: #999999;"">Adv#" & ZeroFill(LocalAdvanceId.ToString, 4)

            If RequestDate > (New Date(2010, 1, 1)) Then
                rtn = rtn & ": " & RequestDate.ToShortDateString & "</span>"
            Else
                rtn = rtn & "</span>"
            End If
            Return rtn

            ' Return Left("RMB#" & RmbNo & " " & UserController.GetUser(PortalId, UID, False).DisplayName, 24)
        End Function
        Public Function GetDateFormat() As String
            Dim sdp As String = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower
            If sdp.IndexOf("d") < sdp.IndexOf("m") Then
                Return "dd/mm/yy"
            Else
                Return "mm/dd/yy"
            End If
        End Function

#End Region

#Region "GetValues"
        Public Function getSelectedTab() As Integer
            

            If hfRmbNo.Value = "" Then
                Return 0
            End If

            Dim RmbNo As Integer = hfRmbNo.Value
            If RmbNo > 0 Then

                Dim rmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = RmbNo
                If rmb.Count > 0 Then
                    Select Case rmb.First.Status
                        Case RmbStatus.MoreInfo
                            Return 0
                        Case Is >= RmbStatus.PendingDownload
                            Return 2
                        Case Else
                            Return rmb.First.Status
                    End Select

                End If
            Else
                'Advance
                Dim adv = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = -RmbNo And c.PortalId = PortalId

                If adv.Count > 0 Then
                    Select Case adv.First.RequestStatus
                        Case RmbStatus.MoreInfo
                            Return 0
                        Case Is >= RmbStatus.PendingDownload
                            Return 2
                        Case Else
                            Return adv.First.RequestStatus
                    End Select
                End If

            End If
            Return 0
        End Function
        Protected Function IsAccounts() As Boolean
            If Not ModuleContext.IsEditable Then
                Return False
            End If
            Try


                For Each role In CStr(Settings("AccountsRoles")).Split(";")
                    If (UserInfo.Roles().Contains(role)) Then
                        Return True
                    End If
                Next

            Catch ex As Exception

            End Try
            Return False
        End Function
        Protected Function CanEdit(ByVal status As Integer) As Boolean
            Return status <> RmbStatus.Processed And status <> RmbStatus.PendingDownload And status <> RmbStatus.DownloadFailed And (status <> RmbStatus.Approved Or IsAccounts())


        End Function
        Protected Sub GetMilesForYear(ByVal RMBLineId As Integer, ByVal UID As Integer)
            Dim RMBLine = (From c In d.AP_Staff_RmbLines Where c.RmbLineNo = RMBLineId).First

            Dim TaxDate1 As New Date(2010, 4, 5)
            Dim TaxDate2 As New Date(2010, 4, 5)
            If RMBLine.TransDate.DayOfYear < TaxDate1.DayOfYear Then
                TaxDate1 = "05/04/" & (Year(RMBLine.TransDate) - 1)
                TaxDate2 = "05/04/" & (Year(RMBLine.TransDate))
            Else
                TaxDate1 = "05/04/" & (Year(RMBLine.TransDate))
                TaxDate2 = "05/04/" & (Year(RMBLine.TransDate) + 1)
            End If


            Dim q = (From c In d.AP_Staff_RmbLines Where c.LineType = 7 And c.AP_Staff_Rmb.Status <> RmbStatus.Cancelled And c.Spare3 <> CInt(Settings("Motorcycle")) And c.Spare3 <> CInt(Settings("Bicycle")) And c.AP_Staff_Rmb.UserId = UID And c.TransDate >= TaxDate1 And c.TransDate < TaxDate2 And c.Spare2 <> "" Select Miles = CInt(c.Spare2))

            Try

                If q.Sum > CInt(Settings("MThreshold")) Then

                    If q.Sum - CInt(Settings("MThreshold")) < CInt(RMBLine.Spare2) Then
                        'Need to split

                        Dim Diff As Integer = CInt(Settings("MThreshold")) + CInt(RMBLine.Spare2) - q.Sum


                        Dim insert As New AP_Staff_RmbLine
                        insert.AnalysisCode = RMBLine.AnalysisCode
                        insert.Comment = RMBLine.Comment & "(>" & CInt(Settings("MThreshold")) & " rate)"

                        insert.LineType = RMBLine.LineType
                        insert.Receipt = RMBLine.Receipt
                        insert.ReceiptNo = RMBLine.ReceiptNo
                        insert.RmbNo = RMBLine.RmbNo
                        insert.Spare1 = RMBLine.Spare1
                        insert.Spare2 = RMBLine.Spare2 - Diff
                        RMBLine.Spare2 = Diff
                        RMBLine.GrossAmount = RMBLine.Spare2 * ((CInt(RMBLine.Spare3) + (CInt(Settings("AddPass")) * RMBLine.Spare1)) / 100)

                        insert.Spare3 = RMBLine.Spare3 - (CInt(Settings("MRate1")) - CInt(Settings("MRate2")))
                        insert.GrossAmount = insert.Spare2 * ((CInt(insert.Spare3) + (CInt(Settings("AddPass")) * insert.Spare1)) / 100)
                        insert.Taxable = RMBLine.Taxable
                        insert.TransDate = RMBLine.TransDate
                        insert.VATReceipt = RMBLine.VATReceipt
                        'Dim AccType = Right(ddlChargeTo.SelectedValue, 1)

                        'If insert.VATReceipt Then
                        '    If AccType = "X" Then
                        '        If insert.TransDate >= "04/01/2011" Then
                        '            insert.VATCode = "3"
                        '            insert.VATRate = 20
                        '        Else
                        '            insert.VATCode = "3T"
                        '            insert.VATRate = 17.5
                        '        End If
                        '        insert.VATAmount = insert.GrossAmount / (1 + (100 / insert.VATRate))
                        '    Else
                        '        insert.VATCode = 8
                        '        insert.VATAmount = 0.0
                        '        insert.VATRate = 0.0
                        '    End If
                        'Else
                        '    If AccType = "X" Then
                        '        insert.VATCode = 6
                        '        insert.VATAmount = 0.0
                        '        insert.VATRate = 0.0
                        '    Else
                        '        insert.VATCode = 8
                        '        insert.VATAmount = 0.0
                        '        insert.VATRate = 0.0
                        '    End If
                        'End If


                        d.AP_Staff_RmbLines.InsertOnSubmit(insert)
                        d.SubmitChanges()

                        'Dim r = From c In d.AP_Staff_RmbLineAddStaffs Where c.RmbLineId = RMBLineId
                        'For Each row In r
                        '    Dim person = New AP_Staff_RmbLineAddStaff
                        '    person.UserId = row.UserId
                        '    person.Name = row.Name

                        '    person.RmbLineId = insert.RmbLineNo
                        '    d.AP_Staff_RmbLineAddStaffs.InsertOnSubmit(person)

                        'Next
                        'd.SubmitChanges()

                    Else
                        RMBLine.Spare3 -= (CInt(Settings("MRate1")) - CInt(Settings("MRate2")))
                        RMBLine.GrossAmount = CInt(RMBLine.Spare2) * ((CInt(RMBLine.Spare3) + (CInt(Settings("AddPass")) * RMBLine.Spare1)) / 100)
                        d.SubmitChanges()
                    End If





                End If



            Catch ex As Exception
                lblTest.Text = lblTest.Text & " " & ex.Message
            End Try


        End Sub
        Public Function GetTotal(ByVal theRmbNo As Integer) As Double

            Return (From c In d.AP_Staff_RmbLines Where c.RmbNo = hfRmbNo.Value Select c.GrossAmount).Sum


        End Function
        Public Function IsSelected(ByVal RmbNo As Integer) As Boolean
            If hfRmbNo.Value = "" Then
                Return False
            Else
                Return (CInt(hfRmbNo.Value) = RmbNo)
            End If
        End Function
        Public Function IsAdvSelected(ByVal AdvanceNo As Integer) As Boolean
            If hfRmbNo.Value = "" Then
                Return False
            Else
                Return (CInt(hfRmbNo.Value) = -AdvanceNo)
            End If
        End Function

        Public Function GetNumericRemainingBalance(ByVal mode As Integer) As Double
            Dim statusList = {RmbStatus.Approved, RmbStatus.PendingDownload, RmbStatus.DownloadFailed}
            If mode = 2 Then
                statusList = {RmbStatus.PendingDownload, RmbStatus.DownloadFailed}
            End If
            Dim AccountBalance As Double = 0
            If hfAccountBalance.Value <> "" Then
                AccountBalance = hfAccountBalance.Value
            End If

            Dim r = (From c In d.AP_Staff_Rmbs Where c.RMBNo = CInt(hfRmbNo.Value) And PortalId = PortalId).First

            Dim Advance As Double = 0
            If Not r.AdvanceRequest = Nothing Then
                Advance = r.AdvanceRequest
            End If

            Dim theStaff = StaffBrokerFunctions.GetStaffMember(r.UserId)
            Dim rTotal As Double = 0
            Dim rT = (From c In d.AP_Staff_RmbLines Where c.AP_Staff_Rmb.PortalId = PortalId And (c.AP_Staff_Rmb.UserId = theStaff.UserId1 Or c.AP_Staff_Rmb.UserId = theStaff.UserId2) And statusList.Contains(c.AP_Staff_Rmb.Status) Select c.GrossAmount)
            If rT.Count > 0 Then
                rTotal = rT.Sum()
            End If
            Dim a = (From c In d.AP_Staff_AdvanceRequests Where c.PortalId = PortalId And (c.UserId = theStaff.UserId1 Or c.UserId = theStaff.UserId2) And statusList.Contains(c.RequestStatus) Select c.RequestAmount)
            Dim aTotal As Double = 0
            If a.Count > 0 Then
                aTotal = a.Sum()
            End If

            Return AccountBalance + Advance - (rTotal + aTotal)

        End Function
        Public Function GetRemainingBalance() As String


            Return StaffBrokerFunctions.GetFormattedCurrency(PortalId, GetNumericRemainingBalance(1).ToString("0.00"))

        End Function
        Public Function IsWrongType(ByVal CostCenter As String, ByVal LineTypeId As Integer) As Boolean

            Dim isD = StaffBrokerFunctions.IsDept(PortalId, CostCenter)
            Dim rtn As Boolean
            If isD Then
                rtn = d.AP_StaffRmb_PortalLineTypes.Where(Function(x) x.PortalId = PortalId And x.LineTypeId = LineTypeId And x.DCode <> "").Count = 0
            Else
                rtn = d.AP_StaffRmb_PortalLineTypes.Where(Function(x) x.PortalId = PortalId And x.LineTypeId = LineTypeId And x.PCode <> "").Count = 0


            End If
            If rtn Then
                lblWrongType.Visible = True
                pnlError.Visible = True
                btnSubmit.Enabled = False
                btnProcess.Enabled = False
                btnApprove.Enabled = False
            End If
            Return rtn
        End Function

        Public Function GetLineTypeClass(ByVal CostCenter As String, ByVal LineTypeId As Integer) As String
            If IsWrongType(CostCenter, LineTypeId) Then
                Return "ui-state-error ui-corner-all"
            End If

            Dim q = From c In d.AP_Staff_RmbLineTypes Where c.LineTypeId = LineTypeId

            If q.Count > 0 Then
                If q.First.SpareField2 = "INCOME" Then
                    Return "ui-state-highlight ui-corner-all"
                End If

            End If
            Return ""
        End Function
        Public Function GetLineTypeMessage(ByVal CostCenter As String, ByVal LineTypeId As Integer) As String
            If IsWrongType(CostCenter, LineTypeId) Then
                Return Translate("lblWrongType")
            End If

            Dim q = From c In d.AP_Staff_RmbLineTypes Where c.LineTypeId = LineTypeId

            If q.Count > 0 Then
                If q.First.SpareField2 = "INCOME" Then
                    Return Translate("lblIncome")
                End If

            End If
            Return ""
        End Function

#End Region

#Region "Utilities"
        Protected Sub Log(ByVal RmbNo As Integer, ByVal Message As String)
            objEventLog.AddLog("Rmb" & RmbNo, Message, PortalSettings, UserId, Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
        End Sub
        Protected Function GetAccountCode(ByVal LineTypeId As Integer, ByVal CostCenter As String) As String

            Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.LineTypeId = LineTypeId And c.PortalId = PortalId


            If q.Count > 0 Then
                If (q.First.AP_Staff_RmbLineType.ControlPath.Contains("RmbDonation")) Then
                    Return Settings("HoldingAccount")
                End If

                If q.First.PCode.Length = 0 And q.First.DCode.Length > 0 Then
                    Return q.First.DCode
                ElseIf q.First.DCode.Length = 0 And q.First.PCode.Length > 0 Then
                    Return q.First.PCode
                End If

                If StaffBrokerFunctions.IsDept(PortalId, CostCenter) And q.First.DCode.Length > 0 Then
                    Return q.First.DCode
                Else
                    Return q.First.PCode
                End If
            End If
            Return ""

        End Function


        Public Sub LoadDefaultSettings()
            Dim tmc As New DotNetNuke.Entities.Modules.ModuleController
            If Settings("NoReceipt") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "NoReceipt", 5)
            End If
            If Settings("Expire") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "Expire", 3)
            End If
            If Settings("TeamLeaderLimit") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "TeamLeaderLimit", 10000)
            End If
            If Settings("MRate1") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "MRate1", 0.4)
            End If
            'If Settings("MRate2") = "" Then
            '    tmc.UpdateTabModuleSetting(TabModuleId, "MRate2", 25)
            'End If
            'If Settings("MThreshold") = "" Then
            '    tmc.UpdateTabModuleSetting(TabModuleId, "MThreshold", 100000)
            'End If
            'If Settings("AddPass") = "" Then
            '    tmc.UpdateTabModuleSetting(TabModuleId, "AddPass", 5)
            'End If
            If Settings("Motorcycle") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "Motorcycle", 0.25)
            End If
            If Settings("Bicycle") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "Bicycle", 0.05)
            End If
            If Settings("AccountsEmail") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "AccountsEmail", "accounts@your-domain.com")
            End If
            If Settings("SubBreakfast") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "SubBreakfast", 5)
            End If
            If Settings("SubLunch") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "SubLunch", 10)
            End If
            If Settings("SubDinner") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "SubDinner", 15)
            End If
            If Settings("SubDay") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "SubDay", 30)
            End If
            If Settings("EntBreakfast") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "EntBreakfast", 5)
            End If
            If Settings("EntLunch") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "EntLunch", 10)
            End If

            If Settings("EntDinner") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "EntDinner", 15)
            End If
            If Settings("EntOvernight") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "EntOvernight", 5)
            End If
            If Settings("EntDay") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "EntDay", 20)
            End If
            If Settings("MenuSize") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "MenuSize", 15)
            End If
            If Settings("VatAttrib") = "" Then
                tmc.UpdateTabModuleSetting(TabModuleId, "VatAttrib", False)
            End If
            tmc.UpdateTabModuleSetting(TabModuleId, "isLoaded", "No")

            SynchronizeModule()

        End Sub
        Protected Function ZeroFill(ByVal number As Integer, ByVal len As Integer) As String
            If number.ToString.Length > len Then
                Return Right(number.ToString, len)
            Else
                Dim Filler As String = ""
                For i As Integer = 1 To len - number.ToString.Length
                    Filler &= "0"
                Next
                Return Filler & number.ToString
            End If


        End Function
        Protected Sub ResetNewExpensePopup(ByVal blankValues As Boolean)
            Try


                Dim lt = From c In d.AP_Staff_RmbLineTypes Where c.LineTypeId = ddlLineTypes.SelectedValue
                If lt.Count > 0 Then
                    Dim Comment As String = ""
                    Dim Amount As Double = 0.0
                    Dim theDate As Date = Today
                    Dim VAT As Boolean = False
                    Dim Receipt As Boolean = True
                    Dim ReceiptType = 1
                    If Settings("ElectronicReceipts") = "True" Then
                        ReceiptType = 2
                    End If

                    If Not blankValues Then


                        Try

                            If Not (theControl Is Nothing) Then
                                Dim ucTypeOld As Type = theControl.GetType()
                                Comment = CStr(ucTypeOld.GetProperty("Comment").GetValue(theControl, Nothing))
                                theDate = CDate(ucTypeOld.GetProperty("theDate").GetValue(theControl, Nothing))
                                Amount = CDbl(ucTypeOld.GetProperty("Amount").GetValue(theControl, Nothing))
                                VAT = CStr(ucTypeOld.GetProperty("VAT").GetValue(theControl, Nothing))
                                Receipt = CStr(ucTypeOld.GetProperty("Receipt").GetValue(theControl, Nothing))



                            End If

                        Catch ex As Exception

                        End Try
                    End If
                    ' Save the standard values



                    phLineDetail.Controls.Clear()

                    ddlOverideTax.SelectedIndex = 0

                    theControl = LoadControl(lt.First.ControlPath)

                    theControl.ID = "theControl"
                    phLineDetail.Controls.Add(theControl)

                    Dim ucType As Type = theControl.GetType()

                    ucType.GetProperty("Comment").SetValue(theControl, Comment, Nothing)
                    ucType.GetProperty("Amount").SetValue(theControl, Amount, Nothing)
                    ucType.GetProperty("theDate").SetValue(theControl, theDate, Nothing)
                    ucType.GetProperty("VAT").SetValue(theControl, VAT, Nothing)
                    ucType.GetProperty("Receipt").SetValue(theControl, Receipt, Nothing)
                    Dim non_receipt As Integer() = {31, 39, 40}
                    If ReceiptType = 2 And Not non_receipt.Contains(lt.First.LineTypeId) Then
                        pnlElecReceipts.Attributes("style") = ""
                        ucType.GetProperty("ReceiptType").SetValue(theControl, ReceiptType, Nothing)
                    End If

                    ucType.GetProperty("Spare1").SetValue(theControl, "", Nothing)
                    ucType.GetProperty("Spare2").SetValue(theControl, "", Nothing)
                    ucType.GetProperty("Spare3").SetValue(theControl, "", Nothing)
                    ucType.GetProperty("Spare4").SetValue(theControl, "", Nothing)
                    ucType.GetProperty("Spare5").SetValue(theControl, "", Nothing)
                    ucType.GetMethod("Initialize").Invoke(theControl, New Object() {Settings})
                    'ifReceipt.Attributes("src") = Request.Url.Scheme & "://" & Request.Url.Host & "/DesktopModules/AgapeConnect/StaffRmb/ReceiptEditor.aspx?RmbNo=" & hfRmbNo.Value & "&RmbLine=New"
                   


                    'Dim rmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = CInt(hfRmbNo.Value)
                    'If rmb.Count > 0 Then
                    '    'Dim codes = From c In lt.First.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId
                    '    'If codes.Count > 0 Then
                    '    '    If codes.First.DCode.Length = 0 Or rmb.First.CostCenter = StaffBrokerFunctions.GetStaffMember(rmb.First.UserId).CostCenter Then
                    '    '        ddlAccountCode.SelectedValue = codes.First.PCode
                    '    '    Else
                    '    '        ddlAccountCode.SelectedValue = codes.First.DCode
                    '    '    End If
                    '    'End If

                    'End If
                    If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then

                        If (lt.First.ControlPath.Contains("RmbDonation")) Then
                            tbAccountCode.Text = Settings("HoldingAccount")
                            tbCostCenter.Text = Settings("ControlAccount")
                        Else
                            tbAccountCode.Text = GetAccountCode(lt.First.LineTypeId, tbCostCenter.Text)
                            tbCostCenter.Text = ddlChargeTo.SelectedValue
                        End If
                    Else

                        If (lt.First.ControlPath.Contains("RmbDonation")) Then
                            ddlAccountCode.SelectedValue = Settings("HoldingAccount")
                            ddlCostcenter.SelectedValue = Settings("ControlAccount")
                        Else
                            ddlAccountCode.SelectedValue = GetAccountCode(lt.First.LineTypeId, ddlCostcenter.SelectedValue)

                            ddlCostcenter.SelectedValue = ddlChargeTo.SelectedValue



                        End If

                    End If




                End If


            Catch ex As Exception
                StaffBrokerFunctions.EventLog("Error Resetting Expense Popup", ex.ToString, UserId)
            End Try
        End Sub
        Protected Sub SendStaffMail(ByVal theRmb As AP_Staff_Rmb)
            Dim SpouseSpecial As Boolean = False
            Dim AmountSpecial As Boolean = False
            Dim CCMSpecial As Boolean = False
            Dim AuthUserName As String = ""
            Dim AuthAuthUserName As String = ""

            Dim ThisObjUser As UserInfo
            ThisObjUser = UserController.GetUserById(PortalId, Settings("AuthUser"))
            AuthUserName = ThisObjUser.DisplayName
            ThisObjUser = UserController.GetUserById(PortalId, Settings("AuthAuthUser"))
            AuthAuthUserName = ThisObjUser.DisplayName
            Dim SpouseId As Integer = StaffBrokerFunctions.GetSpouseId(theRmb.UserId)

            'Generate the attachment
            'Dim input As WebRequest = WebRequest.Create(Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & "/DesktopModules/AgapeConnect/StaffRmb/RmbPrintout.aspx?RmbNo=" & theRmb.RMBNo & "&UID=" & theRmb.UserId)

            'Dim webResponse As Stream = input.GetResponse().GetResponseStream
            'Dim oReader As New StreamReader(webResponse, Encoding.ASCII)
            'Dim oWriter As New StreamWriter(Server.MapPath("/Portals/" & PortalId & "/RmbForm" & theRmb.RMBNo & ".htm"))

            'oWriter.Write(oReader.ReadToEnd)
            'oWriter.Close()
            'oReader.Close()
            'webResponse.Close()

            Dim Auth = UserController.GetUserById(PortalId, Settings("AuthUser"))
            Dim AuthAuth = UserController.GetUserById(PortalId, Settings("AuthAuthUser"))
            Dim myApprovers = StaffRmbFunctions.getApprovers(theRmb, Auth, AuthAuth)
            ' Dim message As String = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("RmbConfirmation", PortalId))
            Dim message As String = StaffBrokerFunctions.GetTemplate("RmbConfirmation", PortalId)

            message = message.Replace("[STAFFNAME]", UserInfo.DisplayName).Replace("[RMBNO]", theRmb.RID).Replace("[USERREF]", IIf(theRmb.UserRef <> "", theRmb.UserRef, "None"))
            If myApprovers.SpouseSpecial And theRmb.UserId <> Settings("AuthUser") Then
                message = message.Replace("[EXTRA]", Translate("SpouseSpecial").Replace("[AUTHUSER]", AuthUserName))
            ElseIf myApprovers.AmountSpecial And theRmb.UserId <> Settings("AuthUser") Then
                message = message.Replace("[EXTRA]", Translate("AmountSpecial").Replace("[TEAMLEADERLIMIT]", StaffBrokerFunctions.GetSetting("Currency", PortalId) & Settings("TeamLeaderLimit")).Replace("[AUTHUSER]", AuthUserName) & " ")
            ElseIf CCMSpecial Then
            ElseIf theRmb.UserId = Settings("AuthUser") And (myApprovers.SpouseSpecial Or myApprovers.AmountSpecial) Then
                message = message.Replace("[EXTRA]", Translate("AuthSpecial").Replace("[AUTHAUTHUSER]", AuthAuthUserName))
            Else
                message = message.Replace("[EXTRA]", "")
            End If
            If (From c In theRmb.AP_Staff_RmbLines Where c.Receipt = True And (Not c.ReceiptImageId Is Nothing)).Count > 0 Then
                message = message.Replace("[STAFFACTION]", Translate("PostReceipts"))
            Else
                message = message.Replace("[STAFFACTION]", Translate("NoPostReceipts"))
            End If

            message = message.Replace("[PRINTOUT]", "<a href='" & Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & "DesktopModules/AgapeConnect/StaffRmb/RmbPrintout.aspx?RmbNo=" & theRmb.RMBNo & "&UID=" & theRmb.UserId & "' target-'_blank' style='width: 134px; display:block;)'><div style='text-align: center; width: 122px; margin: 10px;'><img src='" _
                & Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & "DesktopModules/AgapeConnect/StaffRmb/Images/PrintoutIcon.jpg' /><br />Printout</div></a><style> a div:hover{border: solid 1px blue;}</style>")


            Dim Approvers As String = ""
            ' Dim message2 As String = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("RmbApproverEmail", PortalId))
            Dim message2 As String = StaffBrokerFunctions.GetTemplate("RmbApproverEmail", PortalId)


            'Dim app As UserInfo


            ' If myApprovers.AmountSpecial Then
            'message2 = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("RmbLargeTransaction", PortalId))
            'End If


            If myApprovers.isDept Then
                'Department Reimbursement
                Dim toEmail As String = ""
                Dim toName As String = ""
                For Each row In myApprovers.UserIds
                    Approvers &= row.FirstName & " " & row.LastName & "<br/>"
                    toEmail &= row.Email & ";"
                    toName &= row.FirstName & ", "
                Next


                'Send Approvers Instructions Here
                message2 = message2.Replace("[STAFFNAME]", UserInfo.DisplayName).Replace("[RMBNO]", theRmb.RID).Replace("[USERREF]", IIf(theRmb.UserRef <> "", theRmb.UserRef, "None"))
                message2 = message2.Replace("[APPRNAME]", Left(toName, Math.Max(toName.Length - 2, 0)))
                message2 = message2.Replace("[TEAMLEADERLIMIT]", StaffBrokerFunctions.GetSetting("Currency", PortalId) & Settings("TeamLeaderLimit"))
                If theRmb.UserComment <> "" Then
                    message2 = message2.Replace("[COMMENTS]", Translate("EmailComments") & "<br />" & theRmb.UserComment)
                Else
                    message2 = message2.Replace("[COMMENTS]", "")
                End If
                message2 = message2.Replace("[LINEDETAIL]", GetDetailTable(theRmb))
                For Each row In myApprovers.UserIds
                    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", row.Email, "", Translate("SubmittedApprEmailSubject").Replace("[STAFFNAME]", UserInfo.FirstName & " " & UserInfo.LastName), message2.Replace("[BUTTONS]", GetApproveButton(theRmb.SpareField1, row.UserID)), "", "HTML", "", "", "", "")

                Next


                'DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", Left(toEmail, toEmail.Length - 1), "", Translate("SubmittedApprEmailSubject").Replace("[STAFFNAME]", UserInfo.FirstName & " " & UserInfo.LastName), message2, "", "HTML", "", "", "", "")
            Else
                'Personal Reimbursement




                Dim toEmail As String = ""
                Dim toName As String = ""
                For Each row In myApprovers.UserIds
                    Approvers = Approvers & row.FirstName & " " & row.LastName & "<br />"
                    'Send Approvers Instructions Here
                    toEmail = toEmail & row.Email & ";"
                    toName = toName & row.FirstName & ", "
                Next
                message2 = message2.Replace("[STAFFNAME]", UserInfo.DisplayName).Replace("[RMBNO]", theRmb.RMBNo).Replace("[USERREF]", IIf(theRmb.UserRef <> "", theRmb.UserRef, "None"))
                message2 = message2.Replace("[APPRNAME]", Left(toName, Math.Max(toName.Length - 2, 0)))
                message2 = message2.Replace("[TEAMLEADERLIMIT]", StaffBrokerFunctions.GetSetting("Currency", PortalId) & Settings("TeamLeaderLimit"))
                If theRmb.UserComment <> "" Then
                    message2 = message2.Replace("[COMMENTS]", Translate("EmailComments") & "<br />" & theRmb.UserComment)
                Else
                    message2 = message2.Replace("[COMMENTS]", "")
                End If
                message2 = message2.Replace("[LINEDETAIL", GetDetailTable(theRmb))

                For Each row In myApprovers.UserIds
                    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", row.Email, "", Translate("SubmittedApprEmailSubject").Replace("[STAFFNAME]", UserInfo.FirstName & " " & UserInfo.LastName), message2.Replace("[BUTTONS]", GetApproveButton(theRmb.SpareField1, row.UserID)), "", "HTML", "", "", "", "")

                Next

                'If toEmail.Length > 0 Then
                'DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", Left(toEmail, toEmail.Length - 1), "", Translate("SubmittedApprEmailSubject").Replace("[STAFFNAME]", UserInfo.FirstName & " " & UserInfo.LastName), message2, "", "HTML", "", "", "", "")

                'End If

            End If

            message = message.Replace("[APPROVERS]", Approvers)




            '  DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", UserInfo.Email, "donotreply@agape.org.uk", "Reimbursement #" & theRmb.RMBNo, message, Server.MapPath("/Portals/0/RmbForm" & theRmb.RMBNo & ".htm"), "HTML", "", "", "", "")
            DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", UserInfo.Email, "", Translate("EmailSubmittedSubject").Replace("[RMBNO]", theRmb.RID), message, "", "HTML", "", "", "", "")





        End Sub

        Private Function GetApproveButton(ByVal Code As String, ByVal apprId As String) As String
            Dim encrypt = Server.UrlEncode(AgapeEncryption.AgapeEncrypt.Encrypt(Code & ";" & apprId))

            Dim rtn = "<div style=""width: 100%; text-align: center; font-size: x-large;""><a href='" & Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & "DesktopModules/AgapeConnect/StaffRmb/RmbApprove.aspx?r=" & encrypt & "' target='_blank'>" & Translate("btnApprove") & "</a>"
            Dim returnURL = NavigateURL() & "?RmbNo=" & hfRmbNo.Value
           



            rtn &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='" & NavigateURL(PortalSettings.LoginTabId) & "?returnurl=" & Server.UrlEncode(returnURL) & "' target='_blank'>" & Translate("btnLogin") & "</a></div>"
            Return rtn
        End Function
        Private Function GetApproveButtonAdv(ByVal Code As String, ByVal apprId As String) As String
            Dim encrypt = Server.UrlEncode(AgapeEncryption.AgapeEncrypt.Encrypt(Code & ";" & apprId))

            Dim rtn = "<div style=""width: 100%; text-align: center; font-size: x-large;""><a href='" & Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & "DesktopModules/AgapeConnect/StaffRmb/AdvApprove.aspx?a=" & encrypt & "' target='_blank'>" & Translate("btnApprove") & "</a>"
            Dim returnURL = NavigateURL() & "?RmbNo=" & hfRmbNo.Value




            rtn &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='" & NavigateURL(PortalSettings.LoginTabId) & "?returnurl=" & Server.UrlEncode(returnURL) & "' target='_blank'>" & Translate("btnLogin") & "</a></div>"
            Return rtn
        End Function

        Private Function GetDetailTable(ByVal theRmb As AP_Staff_Rmb) As String

            Dim rtn As String = "<div style='float:right; font-style: italic; color: #AAC;'>" & ddlChargeTo.SelectedItem.Text & "</div><div style='clear: both;' />"
            rtn &= "<table style='width: 100%; margin: 10px; text-align: left;'>"
            rtn &= "<tr><th style='text-align: left;'>Date</th><th style='text-align: left;'>Description</th><th style='text-align: left;'>Amount</th></tr>"

            For Each row In theRmb.AP_Staff_RmbLines
                rtn &= "<tr><td>" & row.TransDate.ToString("MMM dd") & "</td><td>" & row.Comment & "</td><td>" & row.GrossAmount.ToString("0.00") & "</td></tr>"
            Next
            rtn &= "<tr><td colspan='2' style='text-align: right; font-weight: bold;'> Total:</td><td>" & theRmb.AP_Staff_RmbLines.Sum(Function(c) c.GrossAmount).ToString("0.00") & "</td></tr>"
            rtn &= "</table>"
            Return rtn
        End Function
        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)

        End Function


        Public Function GetLocalTypeName(ByVal LineTypeId As Integer) As String
            Dim d As New StaffRmbDataContext
            Dim q = From c In d.AP_StaffRmb_PortalLineTypes Where c.LineTypeId = LineTypeId And c.PortalId = PortalId Select c.LocalName

            If q.Count > 0 Then
                Return q.First
            Else
                Dim r = From c In d.AP_Staff_RmbLineTypes Where c.LineTypeId = LineTypeId Select c.TypeName
                If r.Count > 0 Then
                    Return r.First

                Else

                    Return "?"
                End If

            End If

        End Function
#End Region

#Region "Downloading"
        Protected Function DownloadRmbSingle(ByVal RmbNo As Integer) As String
            Dim rtn As String = ""
            Dim theRmb = From c In d.AP_Staff_RmbLines Where c.RmbNo = RmbNo

            If theRmb.Count > 0 Then
                Dim theUserId = (From c In d.AP_Staff_Rmbs Where c.RMBNo = RmbNo Select c.UserId).First
                Dim theUser = UserController.GetUserById(PortalId, theUserId)

                Dim Supplier As String = theRmb.First.AP_Staff_Rmb.SupplierCode

                Dim ref = "R" & ZeroFill(theRmb.First.AP_Staff_Rmb.RID, 5)
                Dim theDate As String = "=""" & Today.ToString("dd-MMM-yy", New CultureInfo("")) & """"

                For Each line In theRmb
                    theDate = "=""" & line.TransDate.ToString("dd-MMM-yy", New CultureInfo("")) & """"

                    If line.Taxable Then
                        rtn &= "=""" & Settings("TaxAccountsReceivable") & ""","
                    Else
                        If line.AccountCode Is Nothing Then

                            rtn &= "=""" & GetAccountCode(line.LineType, line.CostCenter) & ""","
                        Else
                            rtn &= "=""" & line.AccountCode & ""","
                        End If
                    End If

                    rtn &= "=""" & line.CostCenter & ""","
                    rtn &= ref & ","
                    rtn &= theDate & ","
                    Dim Debit As String = ""
                    Dim Credit As String = ""
                    If line.GrossAmount > 0 Then
                        Debit = line.GrossAmount.ToString("0.00", New CultureInfo(""))
                    Else
                        Credit = (-line.GrossAmount).ToString("0.00", New CultureInfo(""))
                    End If
                    Dim shortComment = GetLineComment(line.Comment, line.OrigCurrency, line.OrigCurrencyAmount, line.ShortComment, True, Left(theUser.FirstName, 1) & Left(theUser.LastName, 1), IIf(line.AP_Staff_RmbLineType.TypeName = "Mileage", line.Spare2, ""))
                    rtn &= GetOrderedString(shortComment,
                                         Debit, Credit, "", line, False, Supplier)






                    'If line.GrossAmount > 0 Then
                    '    rtn &= line.GrossAmount.ToString("0.00") & ",,"
                    'Else

                    '    rtn &= "," & -line.GrossAmount.ToString("0.00") & ","
                    'End If

                    'rtn &= "=""" & Left(theUser.FirstName, 1) & Left(theUser.LastName, 1) & "-" & line.Comment & """" & vbNewLine

                Next


                ' IF we opt to go like UK... take out the "IF Line.Taxable..." at the beginning of the above loop
                ' Then add two more transactions. One to back out on 7012 (we will need to get this from a setting)
                ' Then back in on Tax Accounts Payable

                theDate = "=""" & Today.ToString("dd-MMM-yy", New CultureInfo("")) & """"
                Dim theStaff = StaffBrokerFunctions.GetStaffMember(theUserId)
                Dim PACMode = (
                    theStaff.CostCenter = "" And StaffBrokerFunctions.GetStaffProfileProperty(theStaff.StaffId, "PersonalAccountCode") <> "")

                If theRmb.Count > 0 Then




                    Dim RmbTotal As Double = (From c In theRmb Select c.GrossAmount).Sum


                    If PACMode Then
                        If RmbTotal <> 0 Then

                            rtn &= "=""" & StaffBrokerFunctions.GetStaffProfileProperty(theStaff.StaffId, "PersonalAccountCode") & ""","
                            rtn &= "=""" & Settings("ControlAccount") & """" & ","
                            rtn &= ref & ","
                            rtn &= theDate & ","

                            Dim Debit As String = ""
                            Dim Credit As String = ""



                            If RmbTotal > 0 Then
                                Credit = RmbTotal.ToString("0.00", New CultureInfo(""))

                                'rtn &= "," & RmbTotal.ToString("0.00") & ","
                            Else
                                'rtn &= -RmbTotal.ToString("0.00") & ",,"
                                Debit = (-RmbTotal).ToString("0.00", New CultureInfo(""))
                            End If

                            'rtn &= Left(theUser.FirstName, 1) & Left(theUser.LastName, 1) & "-Payment for " & ref & vbNewLine
                            rtn &= GetOrderedString(Left(theUser.FirstName, 1) & Left(theUser.LastName, 1) & "-Payment for " & ref,
                                                    Debit, Credit, "", Nothing, False, Supplier)


                        End If

                    Else
                        Dim rmbAdvance As Double = 0.0
                        ' Dim rmbAdvanceBalance As Double = 99999.99

                        If theRmb.Count > 0 Then

                            Dim Adv As Double = theRmb.First.AP_Staff_Rmb.AdvanceRequest
                            If Not Adv = Nothing Then
                                rmbAdvance = Math.Min(RmbTotal, Adv)


                            End If


                        End If



                        RmbTotal -= rmbAdvance





                        If RmbTotal <> 0 Then



                            rtn &= "=""" & Settings("AccountsPayable") & ""","
                            rtn &= "=""" & theStaff.CostCenter & """" & ","
                            rtn &= ref & ","
                            rtn &= theDate & ","

                            Dim Debit As String = ""
                            Dim Credit As String = ""



                            If RmbTotal > 0 Then
                                Credit = RmbTotal.ToString("0.00", New CultureInfo(""))

                                'rtn &= "," & RmbTotal.ToString("0.00") & ","
                            Else
                                'rtn &= -RmbTotal.ToString("0.00") & ",,"
                                Debit = (-RmbTotal).ToString("0.00", New CultureInfo(""))
                            End If

                            'rtn &= Left(theUser.FirstName, 1) & Left(theUser.LastName, 1) & "-Payment for " & ref & vbNewLine
                            rtn &= GetOrderedString(Left(theUser.FirstName, 1) & Left(theUser.LastName, 1) & "-Payment for " & ref,
                                                    Debit, Credit, "", Nothing, False, Supplier)


                        End If
                        If rmbAdvance <> 0 Then

                            rtn &= "=""" & Settings("AccountsReceivable") & ""","

                            rtn &= "=""" & theStaff.CostCenter & """" & ","

                            rtn &= ref & ","
                            rtn &= theDate & ","

                            Dim Debit As String = ""
                            Dim Credit As String = ""

                            If rmbAdvance > 0 Then
                                Credit = rmbAdvance.ToString("0.00", New CultureInfo(""))
                                'rtn &= "," & rmbAdvance.ToString("0.00") & ","
                            Else
                                Debit = (-rmbAdvance).ToString("0.00", New CultureInfo(""))
                                ' rtn &= -rmbAdvance.ToString("0.00") & ",,"
                            End If

                            '  rtn &= Left(theUser.FirstName, 1) & Left(theUser.LastName, 1) & "-Pay off advance with " & ref & vbNewLine
                            rtn &= GetOrderedString(Left(theUser.FirstName, 1) & Left(theUser.LastName, 1) & "-Pay off advance with " & ref,
                                                    Debit, Credit, "", Nothing, False, Supplier)

                        End If



                    End If
                End If
            End If

            Return rtn
        End Function

        Protected Function DownloadAdvSingle(ByVal AdvanceNo As Integer) As String
            Dim rtn As String = ""
            Dim theAdv = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = AdvanceNo And c.PortalId = PortalId
            If theAdv.Count > 0 Then
                Dim ac = StaffBrokerFunctions.GetSetting("AccountingCurrency", PortalId)
                Dim theUser = UserController.GetUserById(PortalId, theAdv.First.UserId)
                Dim StaffMember = StaffBrokerFunctions.GetStaffMember(theAdv.First.UserId)

                'First Debit 12xx
                Dim ref = "A" & ZeroFill(theAdv.First.LocalAdvanceId, 5)
                'Dim theDate As String = "=""" & Today.ToString("dd-MMM-yy") & """"
                Dim theDate As String = "=""" & theAdv.First.RequestDate.Value.ToString("dd-MMM-yy", New CultureInfo("")) & """"
                rtn &= "=""" & Settings("AccountsReceivable") & ""","
                rtn &= "=""" & StaffMember.CostCenter & ""","
                rtn &= ref & ","
                rtn &= theDate & ","

                Dim Debit As String = ""
                Dim Credit As String = ""
                If theAdv.First.RequestAmount > 0 Then
                    Debit = (theAdv.First.RequestAmount.Value.ToString("0.00", New CultureInfo("")))
                Else
                    Credit = (-theAdv.First.RequestAmount.Value).ToString("0.00", New CultureInfo(""))
                End If
                Dim curSuffix = ""
                If Not String.IsNullOrEmpty(theAdv.First.OrigCurrency) Then

                    If theAdv.First.OrigCurrency <> ac Then
                        curSuffix = "-" & theAdv.First.OrigCurrency & theAdv.First.OrigCurrencyAmount.Value.ToString("0.00").Replace(".00", "")

                    End If
                End If
                rtn &= GetOrderedString(Left(theUser.FirstName, 1) & Left(theUser.LastName, 1) & "-Adv#" & theAdv.First.LocalAdvanceId & curSuffix,
                                        Debit, Credit)

                'Now Credit 23xx
                Dim PAC = StaffBrokerFunctions.GetStaffProfileProperty(StaffMember.StaffId, "PersonalAccountCode")
                Dim PACMode = (
                     StaffMember.CostCenter = "" And PAC <> "")

                If PACMode Then

                    rtn &= "=""" & PAC & ""","
                    rtn &= "=""" & Settings("ControlAccount") & ""","
                Else
                    rtn &= "=""" & Settings("AccountsPayable") & ""","
                    rtn &= "=""" & StaffMember.CostCenter & ""","
                End If

                rtn &= ref & ","
                rtn &= theDate & ","

                Debit = ""
                Credit = ""
                If theAdv.First.RequestAmount > 0 Then
                    Credit = (theAdv.First.RequestAmount.Value.ToString("0.00", New CultureInfo("")))
                Else
                    Debit = (-theAdv.First.RequestAmount.Value).ToString("0.00", New CultureInfo(""))
                End If





                rtn &= GetOrderedString(Left(theUser.FirstName, 1) & Left(theUser.LastName, 1) & "-Adv#" & theAdv.First.LocalAdvanceId & curSuffix,
                                        Debit, Credit)

            End If
            Return rtn
        End Function
        Protected Function GetOrderedString(ByVal Desc As String, ByVal Debit As String, ByVal Credit As String, Optional Company As String = "", Optional line As AP_Staff_RmbLine = Nothing, Optional title As Boolean = False, Optional Supplier As String = "") As String
            Dim format As String = "DDC"
            If CStr(Settings("DownloadFormat")) <> "" Then
                format = CStr(Settings("DownloadFormat"))
            End If


            Dim CompanyName = CStr(StaffBrokerFunctions.GetSetting("CompanyName", PortalId))
            If Company <> "" Then
                CompanyName = Company
            End If

            Select Case format
                Case "DCD"
                    Return "=""" & Debit & """,=""" & Credit & """,=""" & Desc & """" & vbNewLine
                Case "CDCD"
                    Return "=""" & CompanyName & """,=""" & Debit & """,=""" & Credit & """,=""" & Desc & """" & vbNewLine
                Case "CDDC"
                    Return "=""" & CompanyName & """,=""" & Desc & """,=""" & Debit & """,=""" & Credit & """" & vbNewLine
                Case "DDC"
                    Return "=""" & Desc & """,=""" & Debit & """,=""" & Credit & """" & vbNewLine
                Case Else 'Including GEN
                    Dim VAT As String = "N"
                    Dim Cur As String = ""
                    Dim CurAmt As String = ""
                    Dim TRXDate As String = ""
                    Dim ApprDate As String = ""
                    Dim FullComment As String = ""
                    Dim ProcDate As String = ""
                    Dim DownloadDate As String = ""
                    If title Then
                        Supplier = "Supplier"
                        VAT = "VAT"
                        Cur = "Original Currency"
                        CurAmt = "Original Currency Amount"
                        TRXDate = "Transaction Date"
                        ApprDate = "Approval Date"
                        ProcDate = "Processed Date"
                        DownloadDate = "Download Date"
                        FullComment = "Orignal Description (Long)"
                    ElseIf Not line Is Nothing Then
                        ' Supplier = line.AP_Staff_Rmb.SupplierCode
                        VAT = IIf(line.VATReceipt, "Y", "N")
                        Cur = line.OrigCurrency
                        CurAmt = line.OrigCurrencyAmount
                        TRXDate = line.TransDate.ToString("yyyy-MM-dd")
                        If Not line.AP_Staff_Rmb.ApprDate Is Nothing Then
                            ApprDate = line.AP_Staff_Rmb.ApprDate.Value.ToString("yyyy-MM-dd")
                        End If
                       

                        DownloadDate = Today.ToString("yyyy-MM-dd")
                        FullComment = line.Comment
                    End If

                    Return "=""" & Desc & """,=""" & Debit & """,=""" & Credit & """,=""" & Supplier & """,=""" & VAT & """,=""" & Cur & """,=""" & CurAmt & """,=""" & TRXDate & """,=""" & ApprDate & """,=""" & DownloadDate & """,=""" & FullComment & """" & vbNewLine
            End Select



        End Function

        Protected Sub DownloadPeriodReport(ByVal Period As Integer, ByVal Year As Integer)
            Dim q = From c In d.AP_Staff_Rmbs Where c.PortalId = PortalId And c.Period = Period And c.Year = Year And c.Status = RmbStatus.Processed
            Dim csvOut As String = "RmbNo, Primary RC, Type, Submitted by, Submitted on,Approved by, Approved on, Processed By, Processed on, Amount" & vbNewLine

            For Each row In q
                Dim SubmitterName = ""
                Dim ApproverName = ""
                Dim ProcessorName = ""

                Dim submitter = UserController.GetUserById(PortalId, row.UserId)
                If Not submitter Is Nothing Then
                    SubmitterName = submitter.DisplayName
                End If
                Dim approver = UserController.GetUserById(PortalId, row.ApprUserId)
                If Not approver Is Nothing Then
                    ApproverName = approver.DisplayName
                End If
                Dim Processor = UserController.GetUserById(PortalId, row.ProcUserId)
                If Not Processor Is Nothing Then
                    ProcessorName = Processor.DisplayName
                End If



                csvOut &= row.RID & ","
                csvOut &= """" & row.CostCenter & ""","
                csvOut &= """" & IIf(row.Department, "D", "P") & ""","
                csvOut &= """" & SubmitterName & ""","
                csvOut &= row.RmbDate.Value.ToString("yyyy-MM-dd", New CultureInfo("")) & ","
                csvOut &= """" & ApproverName & ""","
                csvOut &= row.ApprDate.Value.ToString("yyyy-MM-dd", New CultureInfo("")) & ","
                csvOut &= """" & ProcessorName & ""","
                csvOut &= row.ProcDate.Value.ToString("yyyy-MM-dd", New CultureInfo("")) & ","
                csvOut &= row.AP_Staff_RmbLines.Sum(Function(c) c.GrossAmount).ToString(New CultureInfo("")) & vbNewLine


            Next

            csvOut &= vbNewLine
            csvOut &= "AdvNo, Staff RC, Type, Submitted by, Submitted on,Approved by, Approved on, Processed By, Processed on, Amount" & vbNewLine
            Dim r = From c In d.AP_Staff_AdvanceRequests Where c.PortalId = PortalId And c.Period = Period And c.Year = Year And c.RequestStatus = RmbStatus.Processed

            For Each row In r
                Dim SubmitterName = ""
                Dim ApproverName = ""
                Dim ProcessorName = ""

                Dim submitter = UserController.GetUserById(PortalId, row.UserId)
                If Not submitter Is Nothing Then
                    SubmitterName = submitter.DisplayName
                End If
                Dim approver = UserController.GetUserById(PortalId, row.ApproverId)
                If Not approver Is Nothing Then
                    ApproverName = approver.DisplayName
                End If


                Dim RC = StaffBrokerFunctions.GetStaffMember(row.UserId).CostCenter


                csvOut &= row.LocalAdvanceId & ","
                csvOut &= """" & RC & ""","
                csvOut &= """Advance"","
                csvOut &= """" & SubmitterName & ""","
                csvOut &= row.RequestDate.Value.ToString("yyyy-MM-dd", New CultureInfo("")) & ","
                csvOut &= """" & ApproverName & ""","
                csvOut &= row.ApprovedDate.Value.ToString("yyyy-MM-dd", New CultureInfo("")) & ","
                csvOut &= """" & ProcessorName & ""","
                csvOut &= row.ProcessedDate.Value.ToString("yyyy-MM-dd", New CultureInfo("")) & ","
                csvOut &= row.RequestAmount.Value.ToString(New CultureInfo("")) & vbNewLine


            Next

            Dim t As Type = Me.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")
            sb.Append("closePopupDownloadExpense();")
            sb.Append("</script>")
            ScriptManager.RegisterStartupScript(Page, t, "popupDownloadExpense", sb.ToString, False)



            Dim attachment As String = "attachment; filename=Expense Report - " & Year & " Period " & Period & ".csv"




            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.AddHeader("content-disposition", attachment)
            HttpContext.Current.Response.ContentType = "text/csv"
            HttpContext.Current.Response.AddHeader("Pragma", "public")
            HttpContext.Current.Response.Write(csvOut)
            HttpContext.Current.Response.End()


        End Sub


        Protected Sub DownloadBatch(Optional ByVal MarkAsProcessed As Boolean = False)
            Dim downloadStatuses() As Integer = {RmbStatus.PendingDownload, RmbStatus.DownloadFailed}
            Log(0, "Downloading Batched Transactions")

            Dim pendDownload = From c In d.AP_Staff_Rmbs Where downloadStatuses.Contains(c.Status) And c.PortalId = PortalId

            Dim export As String = "Account,Subaccount,Ref,Date," & GetOrderedString("Description", "Debit", "Credit", "Company", Nothing, True)
            Dim RmbList As New List(Of Integer)
            For Each rmb In pendDownload
                Log(rmb.RMBNo, "Downloading Rmb")
                export &= DownloadRmbSingle(rmb.RMBNo)

                RmbList.Add(rmb.RMBNo)

            Next
            Log(0, "Downloaded Rmbs " & pendDownload.Count & " : " & export)
            Dim pendDownloadAdv = From c In d.AP_Staff_AdvanceRequests Where downloadStatuses.Contains(c.RequestStatus) And c.PortalId = PortalId

            Dim AdvList As New List(Of Integer)
            For Each adv In pendDownloadAdv
                Log(adv.AdvanceId, "Downloading Advance")
                export &= DownloadAdvSingle(adv.AdvanceId)

                AdvList.Add(adv.AdvanceId)

            Next
            Log(0, "Downloaded Advs " & pendDownloadAdv.Count & " : " & export)


            If (MarkAsProcessed) Then

                If Not RmbList Is Nothing Then
                    Dim q = From c In d.AP_Staff_Rmbs Where RmbList.Contains(c.RMBNo) And c.PortalId = PortalId

                    For Each row In q
                        row.Status = RmbStatus.Processed
                        row.ProcDate = Now
                        Log(row.RMBNo, "Marked as Processed - after a manual download")
                    Next
                End If

                If Not AdvList Is Nothing Then

                    Dim r = From c In d.AP_Staff_AdvanceRequests Where AdvList.Contains(c.AdvanceId) And c.PortalId = PortalId

                    For Each row In r
                        row.RequestStatus = RmbStatus.Processed
                        row.ProcessedDate = Now
                        Log(row.AdvanceId, "Advance Marked as Processed - after a manual download")
                    Next

                End If

                d.SubmitChanges()




                If hfRmbNo.Value <> "" Then
                    If hfRmbNo.Value > 0 Then
                        LoadRmb(CInt(hfRmbNo.Value))
                    Else
                        LoadAdv(-CInt(hfRmbNo.Value))
                    End If


                End If

                ResetMenu()



            End If


            Dim t As Type = Me.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")
            sb.Append("closePopupDownload();")
            sb.Append("</script>")
            ScriptManager.RegisterClientScriptBlock(Page, t, "popupDownload", sb.ToString, False)


            Session("RmbList") = RmbList
            Session("AdvList") = AdvList
            Dim attachment As String = "attachment; filename=RmbDownload " & Today.ToString("yy-MM-dd") & ".csv"




            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.AddHeader("content-disposition", attachment)
            HttpContext.Current.Response.ContentType = "text/csv"
            HttpContext.Current.Response.AddHeader("Pragma", "public")
            HttpContext.Current.Response.Write(export)
            HttpContext.Current.Response.End()

        End Sub




#End Region



#Region "UKOnly"
        'Protected Function GetAnalysisCode(ByVal LineType As Integer, Optional ByVal CCin As String = "-1") As String
        '    Dim Acc = From c In d.AP_StaffRmb_PortalLineTypes Where c.PortalId = PortalId And c.LineTypeId = LineType Select c.DCode, c.PCode
        '    Dim CC = (From c In d.AP_Staff_Rmbs Where c.RMBNo = hfRmbNo.Value Select c.CostCenter)

        '    If CCin = "-1" Then
        '        If CC.Count > 0 And Acc.Count > 0 Then
        '            If Right(CC.First, 1) = "X" Then
        '                Return "D-" & Left(CC.First, 3) & "X-" & Acc.First.DCode
        '            Else
        '                Return "P-" & Left(CC.First, 3) & "0-" & Acc.First.PCode
        '            End If

        '        Else
        '            Return ""
        '        End If
        '    Else
        '        If Acc.Count > 0 Then
        '            If Right(CCin, 1) = "X" Then
        '                Return "D-" & Left(CCin, 3) & "X-" & Acc.First.DCode
        '            Else
        '                Return "P-" & Left(CCin, 3) & "0-" & Acc.First.PCode
        '            End If

        '        Else
        '            Return ""
        '        End If
        '    End If



        ' End Function

#End Region


        Protected Sub btnAdvanceRequest_Click(sender As Object, e As System.EventArgs) Handles btnAdvanceRequest.Click
            Dim insert As New AP_Staff_AdvanceRequest
            insert.Error = False
            insert.ErrorMessage = ""
            insert.LocalAdvanceId = StaffRmbFunctions.GetNewAdvId(PortalId)
            insert.RequestAmount = StaffAdvanceRmb1.Amount
            insert.RequestText = StaffAdvanceRmb1.ReqMessage
            insert.UserId = UserId
            insert.RequestDate = Today
            insert.PortalId = PortalId
            insert.RequestStatus = RmbStatus.Submitted
            insert.OrigCurrency = hfOrigCurrency.Value
            insert.OrigCurrencyAmount = Double.Parse(hfOrigCurrencyValue.Value, New CultureInfo(""))
            insert.SpareField1 = System.Guid.NewGuid().ToString
            d.AP_Staff_AdvanceRequests.InsertOnSubmit(insert)
            d.SubmitChanges()

            'Send Confirmation
            Dim Auth = UserController.GetUserById(PortalId, Settings("AuthUser"))
            Dim AuthAuth = UserController.GetUserById(PortalId, Settings("AuthAuthUser"))

            Dim myApprovers = StaffRmbFunctions.getAdvApprovers(insert, Settings("TeamLeaderLimit"), Auth, AuthAuth)
            'Dim ConfMessage As String = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("AdvConfirmation", PortalId))
            Dim ConfMessage As String = StaffBrokerFunctions.GetTemplate("AdvConfirmation", PortalId)
            ConfMessage = ConfMessage.Replace("[STAFFNAME]", UserInfo.DisplayName).Replace("[ADVNO]", insert.LocalAdvanceId)
            If myApprovers.SpouseSpecial And insert.UserId <> Auth.UserID Then
                ConfMessage = ConfMessage.Replace("[EXTRA]", Translate("AdvSpouseSpecial").Replace("[AUTHUSER]", Auth.DisplayName))
            ElseIf myApprovers.AmountSpecial And insert.UserId <> Auth.UserID Then
                ConfMessage = ConfMessage.Replace("[EXTRA]", Translate("AdvAmountSpecial").Replace("[TEAMLEADERLIMIT]", StaffBrokerFunctions.GetSetting("Currency", PortalId) & Settings("TeamLeaderLimit")).Replace("[AUTHUSER]", Auth.DisplayName) & " ")

            ElseIf insert.UserId = Auth.UserID And (myApprovers.SpouseSpecial Or myApprovers.AmountSpecial) Then
                ConfMessage = ConfMessage.Replace("[EXTRA]", Translate("AdvAuthSpecial").Replace("[AUTHAUTHUSER]", AuthAuth.DisplayName))
            Else
                ConfMessage = ConfMessage.Replace("[EXTRA]", "")
            End If

            Dim Approvers As String = ""
            Dim message2 As String = StaffBrokerFunctions.GetTemplate("AdvApproverEmail", PortalId)

            If myApprovers.AmountSpecial Then
                'message2 = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("AdvLargeTransaction", PortalId))
                message2 = StaffBrokerFunctions.GetTemplate("AdvLargeTransaction", PortalId)
            End If
            Dim toEmail As String = ""
            Dim toName As String = ""
            For Each row In myApprovers.UserIds
                Approvers = Approvers & row.FirstName & " " & row.LastName & "<br />"
                'Send Approvers Instructions Here
                toEmail = toEmail & row.Email & ";"
                toName = toName & row.FirstName & ", "
            Next
            message2 = message2.Replace("[STAFFNAME]", UserInfo.DisplayName).Replace("[ADVNO]", insert.AdvanceId)
            message2 = message2.Replace("[AMOUNT]", StaffBrokerFunctions.GetSetting("Currency", PortalId) & insert.RequestAmount)
            message2 = message2.Replace("[APPRNAME]", Left(toName, Math.Max(toName.Length - 2, 0)))
            message2 = message2.Replace("[TEAMLEADERLIMIT]", StaffBrokerFunctions.GetSetting("Currency", PortalId) & Settings("TeamLeaderLimit"))

            message2 = message2.Replace("[COMMENTS]", insert.RequestText)

            For Each row In myApprovers.UserIds
                DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", row.Email, "", Translate("AdvSubmittedApprEmailSubject").Replace("[STAFFNAME]", UserInfo.FirstName & " " & UserInfo.LastName), message2.Replace("[BUTTONS]", GetApproveButtonAdv(insert.SpareField1, row.UserID)), "", "HTML", "", "", "", "")

            Next

            'If toEmail.Length > 0 Then
            '    ' DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", Left(toEmail, toEmail.Length - 1), "donotreply@agape.org.uk", "Reimbursement for " & UserInfo.FirstName & " " & UserInfo.LastName, message2, "", "HTML", "", "", "", "")
            '    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", Left(toEmail, toEmail.Length - 1), "", Translate("AdvSubmittedApprEmailSubject").Replace("[STAFFNAME]", UserInfo.FirstName & " " & UserInfo.LastName), message2, "", "HTML", "", "", "", "")

            'End If



            ConfMessage = ConfMessage.Replace("[APPROVERS]", Approvers)
            '  DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", UserInfo.Email, "donotreply@agape.org.uk", "Reimbursement #" & theRmb.RMBNo, message, Server.MapPath("/Portals/0/RmbForm" & theRmb.RMBNo & ".htm"), "HTML", "", "", "", "")

           
           
           

            DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", UserInfo.Email, "", Translate("AdvEmailSubmittedSubject").Replace("[ADVNO]", insert.LocalAdvanceId), ConfMessage, "", "HTML", "", "", "", "")


            

            'Need to load the Advance!
            ResetMenu()

        End Sub

        Public Function GetJsonAccountString(ByVal Account As String) As String
            Dim rtn As String = "/MobileCAS/MobileCAS.svc/GetAccountBalance?CountryURL="
            rtn &= StaffBrokerFunctions.GetSetting("DataserverURL", PortalId)
            rtn &= "&GUID=" & UserInfo.Profile.GetPropertyValue("ssoGUID")
            rtn &= "&PGTIOU=" & UserInfo.Profile.GetPropertyValue("GCXPGTIOU") & "&Account=" & Account
            Return rtn

        End Function

        Protected Sub btnAdvApprove_Click(sender As Object, e As System.EventArgs) Handles btnAdvApprove.Click
            btnAdvSave_Click(Me, Nothing)
            Dim q = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = -CInt(hfRmbNo.Value) And c.PortalId = PortalId
            If q.Count > 0 Then
                If q.First.RequestStatus = RmbStatus.Submitted Then
                    q.First.RequestStatus = RmbStatus.Approved
                    q.First.ApproverId = UserId
                    q.First.ApprovedDate = Today
                    q.First.Period = Nothing
                    q.First.Year = Nothing

                    d.SubmitChanges()


                    Dim Auth = UserController.GetUserById(PortalId, Settings("AuthUser"))
                    Dim AuthAuth = UserController.GetUserById(PortalId, Settings("AuthAuthUser"))
                    Dim myApprovers = StaffRmbFunctions.getAdvApprovers(q.First, Settings("TeamLeaderLimit"), Auth, AuthAuth)
                    Dim SpouseId As Integer = StaffBrokerFunctions.GetSpouseId(q.First.UserId)

                    Dim ObjAppr As UserInfo = UserController.GetUserById(PortalId, Me.UserId)
                    Dim theUser As UserInfo = UserController.GetUserById(PortalId, q.First.UserId)

                    Dim dr As New TemplatesDataContext
                    'Dim ApprMessage As String = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("AdvApprovedEmail-ApproversVersion", PortalId))
                    Dim ApprMessage As String = StaffBrokerFunctions.GetTemplate("AdvApprovedEmail-ApproversVersion", PortalId)

                    ApprMessage = ApprMessage.Replace("[APPRNAME]", ObjAppr.DisplayName).Replace("[ADVNO]", q.First.LocalAdvanceId).Replace("[STAFFNAME]", theUser.DisplayName)


                    For Each row In (From c In myApprovers.UserIds Where c.UserID <> q.First.UserId And c.UserID <> SpouseId)
                        ApprMessage = ApprMessage.Replace("[THISAPPRNAME]", row.DisplayName)
                        'DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", row.Email, "donotreply@agape.org.uk", "Rmb#:" & hfRmbNo.Value & " has been approved by " & ObjAppr.DisplayName, ApprMessage, "", "HTML", "", "", "", "")
                        DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", row.Email, "", Translate("AdvEmailApprovedSubject").Replace("[ADVNO]", q.First.LocalAdvanceId).Replace("[APPROVER]", ObjAppr.DisplayName), ApprMessage, "", "HTML", "", "", "", "")

                    Next

                    'SEND APRROVE EMAIL


                    ' Dim Emessage As String = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("AdvApprovedEmail", PortalId))
                    Dim Emessage As String = StaffBrokerFunctions.GetTemplate("AdvApprovedEmail", PortalId)

                    Emessage = Emessage.Replace("[STAFFNAME]", theUser.DisplayName).Replace("[ADVNO]", q.First.LocalAdvanceId)
                    Emessage = Emessage.Replace("[APPROVER]", ObjAppr.DisplayName)

                    d.SubmitChanges()
                    Dim toEmail = theUser.Email
                    If Settings("SendAppEmail") = "True" Then 'copy accounts if SendAppEmail is enabled
                        toEmail &= ";" & Settings("AccountsEmail")

                    End If
                    ' DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", theUser.Email, "donotreply@agape.org.uk", "Rmb#: " & hfRmbNo.Value & "-" & rmb.First.UserRef & " has been approved", Emessage, "", "HTML", "", "", "", "")
                    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", toEmail, "", Translate("AdvEmailApprovedSubject").Replace("[ADVNO]", q.First.LocalAdvanceId).Replace("[APPROVER]", ObjAppr.DisplayName), Emessage, "", "HTML", "", "", "", "")










                    LoadAdv(-hfRmbNo.Value)
                    ResetMenu()

                    Log(q.First.AdvanceId, "Advance Approved")

                    SendMessage(Translate("AdvanceApproved").Replace("[ADVANCEID]", q.First.LocalAdvanceId), "selectIndex(2);")



                End If
            End If
        End Sub

        Protected Sub btnAdvReject_Click(sender As Object, e As System.EventArgs) Handles btnAdvReject.Click, btnAdvCancel.Click
            Dim q = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = -CInt(hfRmbNo.Value) And c.PortalId = PortalId

            If q.Count > 0 Then
                Dim LockedList() = {RmbStatus.PendingDownload, RmbStatus.DownloadFailed, RmbStatus.Processed}
                If LockedList.Contains(q.First.RequestStatus) Then
                    SendMessage(Translate("AdvLocked") & "<br />")
                Else
                    q.First.RequestStatus = RmbStatus.Cancelled
                    Log(q.First.AdvanceId, "Advance Cancelled")
                    d.SubmitChanges()

                    Dim Message As String = StaffBrokerFunctions.GetTemplate("AdvCancelled", PortalId)

                    ' Dim dr As New TemplatesDataContext
                    '  Dim Message As String = Server.HtmlDecode(StaffBrokerFunctions.GetTemplate("AdvCancelled", PortalId))

                    Dim StaffMbr = UserController.GetUserById(PortalId, q.First.UserId)


                    Message = Message.Replace("[STAFFNAME]", StaffMbr.FirstName)
                    Message = Message.Replace("[APPRNAME]", UserInfo.FirstName & " " & UserInfo.LastName)
                    Message = Message.Replace("[APPRFIRSTNAME]", UserInfo.FirstName)




                    ' DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", theUser.Email, "donotreply@agape.org.uk", "Rmb#: " & hfRmbNo.Value & "-" & rmb.First.UserRef & " has been cancelled", Message, "", "HTML", "", "", "", "")
                    DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", StaffMbr.Email, "", Translate("AdvEmailCancelledSubject").Replace("[ADVNO]", q.First.LocalAdvanceId), Message, "", "HTML", "", "", "", "")



                    ResetMenu()
                    LoadAdv(-hfRmbNo.Value)
                End If
            End If

        End Sub



        Protected Sub btnAdvSave_Click(sender As Object, e As System.EventArgs) Handles btnAdvSave.Click
            Dim q = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = -CInt(hfRmbNo.Value) And c.PortalId = PortalId
            If q.Count > 0 Then
                Dim LockedList() = {RmbStatus.PendingDownload, RmbStatus.DownloadFailed, RmbStatus.Processed}
                If LockedList.Contains(q.First.RequestStatus) Then
                    lblAdvErr.Text = "* " & Translate("AdvLocked") & "<br />"
                Else
                    lblAdvErr.Text = ""
                    q.First.RequestAmount = CDbl(AdvAmount.Text)
                    q.First.OrigCurrencyAmount = CDbl(hfOrigCurrencyValue.Value)
                    q.First.OrigCurrency = hfOrigCurrency.Value


                    'If IsAccounts() Then
                    '    If ddlAdvPeriod.SelectedIndex > 0 Then
                    '        q.First.Period = ddlAdvPeriod.SelectedValue
                    '    End If
                    '    If ddlAdvYear.SelectedIndex > 0 Then
                    '        q.First.Year = ddlAdvYear.SelectedValue
                    '    End If
                    'End If

                    d.SubmitChanges()
                End If
            End If
            ResetMenu()
            LoadAdv(-hfRmbNo.Value)
        End Sub


        Protected Sub SendMessage(ByVal Message As String, Optional ByVal AppendLines As String = "", Optional ByVal Startup As Boolean = True)

            Dim t As Type = Me.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")
            If Message <> "" Then
                sb.Append("alert(""" & Message & """);")
            End If
            If AppendLines <> "" Then
                sb.Append(AppendLines)
            End If
            sb.Append("</script>")
            If Startup Then
                ScriptManager.RegisterStartupScript(Page, t, "popup", sb.ToString, False)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, t, "popup", sb.ToString, False)
            End If

        End Sub

        Protected Sub btnAdvProcess_Click(sender As Object, e As System.EventArgs) Handles btnAdvProcess.Click
            btnAdvSave_Click(Me, Nothing)
            Dim q = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = -CInt(hfRmbNo.Value) And c.PortalId = PortalId
            If q.Count > 0 Then

                q.First.RequestStatus = RmbStatus.PendingDownload
                q.First.ProcessedDate = Today
                d.SubmitChanges()

                ResetMenu()
                LoadAdv(-hfRmbNo.Value)
                Log(q.First.AdvanceId, "Advance Processed - this advance will be added to the next download batch")
                SendMessage(Translate("NextBatchAdvance"))

            End If
        End Sub

        Protected Sub btnAdvUnProcess_Click(sender As Object, e As System.EventArgs) Handles btnAdvUnProcess.Click
            Dim q = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = -CInt(hfRmbNo.Value) And c.PortalId = PortalId
            If q.Count > 0 Then
                q.First.RequestStatus = RmbStatus.Approved
                q.First.Period = Nothing
                q.First.Year = Nothing
                q.First.ProcessedDate = Nothing

                d.SubmitChanges()

                ResetMenu()
                LoadAdv(-hfRmbNo.Value)

                Log(q.First.AdvanceId, "Advance UnProcessed")

                SendMessage("", "selectIndex(2);")

            End If
        End Sub


        Protected Sub btnAdvDownload_Click(sender As Object, e As System.EventArgs) Handles btnAdvDownload.Click
            btnAdvSave_Click(Me, Nothing)
            Dim AdvanceNo As Integer = -CInt(hfRmbNo.Value)
            Dim LocalAdvId = (From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = AdvanceNo And c.PortalId = PortalId Select c.LocalAdvanceId).First
            Dim export As String = "Account,Subaccount,Ref,Date,Debit,Credit,Description" & vbNewLine
            export &= DownloadAdvSingle(AdvanceNo)


            Dim attachment As String = "attachment; filename=Adv" & ZeroFill(LocalAdvId, 5) & ".csv"

            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.AddHeader("content-disposition", attachment)
            HttpContext.Current.Response.ContentType = "text/csv"
            HttpContext.Current.Response.AddHeader("Pragma", "public")
            HttpContext.Current.Response.Write(export)
            HttpContext.Current.Response.End()
        End Sub


        Public Function GetProfileImage(ByVal UserId As Integer) As String
            Dim FileId = UserController.GetUserById(PortalId, UserId).Profile.GetPropertyValue("Photo")
            If FileId Is Nothing Or FileId = "" Then
                Return "/images/no_avatar.gif"
            Else
                Try


                    Dim theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileId)
                    Return DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(theFile)
                Catch ex As Exception
                    Return "/images/no_avatar.gif"
                End Try
            End If

        End Function

        Protected Sub cbMoreInfo_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbMoreInfo.CheckedChanged
            btnSave_Click(Me, Nothing)
            If cbMoreInfo.Checked Then


                Dim theRmb = From c In d.AP_Staff_Rmbs Where c.RMBNo = CInt(hfRmbNo.Value) And c.PortalId = PortalId Select c.UserId, c.RID

                If theRmb.Count > 0 Then
                    Dim theUser = UserController.GetUserById(PortalId, theRmb.First.UserId)

                    SendMessage(Translate("MoreInfoMsg"), "window.open('mailto:" & theUser.Email & "?subject=Reimbursment " & theRmb.First.RID & ": More info requested');")
                End If

            End If

        End Sub

        Public Function GetLocalStaffProfileName(ByVal StaffProfileName As String) As String
            Dim s = Localization.GetString("ProfileProperties_" & StaffProfileName & ".Text", "/DesktopModules/Admin/Security/App_LocalResources/Profile.ascx.resx", System.Threading.Thread.CurrentThread.CurrentCulture.Name)
            If String.IsNullOrEmpty(s) Then
                Return StaffProfileName
            Else
                Return s
            End If
        End Function

        Private Sub GetRSADownload(ByRef myCommand As OleDbCommand)

            'Dim sql2 = "Update [Deductions$A2:J2000] Set F1='', F2='', F3='', F4='', F5='', F6='', F7='', F8='', F9='' ;" ', F10='', F11='', F12='', F13='', F14='', F15='' ;" ',F16='', F17='', F18='', F19='', F20='', F21='', F22='', F23='', F24='', F25='', F26='' ;"
            'myCommand.CommandText = sql2
            'myCommand.ExecuteNonQuery()

            'sql2 = "Update [Earnings$A4:H2000] Set F1='', F2='', F3='', F4='', F5='', F6='', F7='';"
            'myCommand.CommandText = sql2
            'myCommand.ExecuteNonQuery()

            'Maybe this would be better to do my working out the current period
            Log(0, "Step A")
            Dim currentRmbs = From c In d.AP_Staff_RmbLines Where c.AP_Staff_Rmb.PortalId = PortalId And c.AP_Staff_Rmb.Status = RmbStatus.Processed 'And c.AP_Staff_Rmb.ProcDate > Today.AddDays(-15) And c.Department = False

            Dim Deductions = DotNetNuke.Entities.Profile.ProfileController.GetPropertyDefinitionsByCategory(PortalId, "Payroll-Deductions")
            Dim Earnings = DotNetNuke.Entities.Profile.ProfileController.GetPropertyDefinitionsByCategory(PortalId, "Payroll-Earnings")


            Dim sql = "Update [Deductions$A2:Z2] Set F1='', F2='' "
            Dim j As Integer = 3
            For Each item As DotNetNuke.Entities.Profile.ProfilePropertyDefinition In Deductions
                sql &= ",F" & j & "='" & GetLocalStaffProfileName(item.PropertyName) & "' "
                j += 1
            Next

            myCommand.CommandText = sql
            myCommand.ExecuteNonQuery()

            Dim StaffTypes = {"National Staff", "National Staff, Overseas", "Centrally Funded"}
            Dim allStaff = StaffBrokerFunctions.GetStaff(-1)

            Log(0, "Step B")
            '.OrderBy(Function(x) x.LastName).ThenBy(Function(x) x.AP_StaffBroker_Staffs.StaffId)
            Dim i As Integer = 3
            For Each row In allStaff
                'Load Values
                Dim theUser = UserController.GetUserById(PortalId, row.UserID)
                Dim theStaff = StaffBrokerFunctions.GetStaffMember(theUser.UserID)
                If Not (theStaff Is Nothing Or theUser Is Nothing) Then


                    '  Dim CurrentPeriod = StaffBrokerFunctions.GetSetting("CurrentFiscalPeriod", PortalId)

                    Dim EmpCode As String = theUser.Profile.GetPropertyValue("EmployeeCode")
                    If EmpCode Is Nothing Or EmpCode = "0" Then
                        EmpCode = ""
                    End If


                    Dim CostCenter = theStaff.CostCenter

                    Dim salary As Double = 0
                    For Each item As DotNetNuke.Entities.Profile.ProfilePropertyDefinition In Earnings
                        If item.Deleted = False Then
                            salary += theUser.Profile.GetPropertyValue(item.PropertyName)
                        End If

                    Next

                    'Dim VehicleInsurance As Double = theUser.Profile.GetPropertyValue("VehicleInsurance")
                    'Dim RetirementPolicies As Double = theUser.Profile.GetPropertyValue("RetirementPolicies")
                    'Dim DependantParent As Double = theUser.Profile.GetPropertyValue("DependantParent")
                    'Dim HousingAllowance As Double = theUser.Profile.GetPropertyValue("HousingAllowance")

                    Dim NormalSalary As Double = theUser.Profile.GetPropertyValue("NormalSalary")

                    salary += NormalSalary





                    Dim AccountBalance As Double = 0
                    Dim AdvanceBalance As Double = 0
                    Try


                        Dim sugPay = From c In ds.AP_Staff_SuggestedPayments Where c.PortalId = PortalId And c.CostCenter = CostCenter


                        If sugPay.Count > 0 Then
                            If Not sugPay.First.AccountBalance Is Nothing Then
                                AccountBalance = sugPay.First.AccountBalance
                            End If

                            If Not sugPay.First.AdvanceBalance Is Nothing Then
                                AdvanceBalance = sugPay.First.AdvanceBalance
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                    'lookup Expenses for current period, user
                    Dim myRmbs = From c In currentRmbs Where c.AP_Staff_Rmb.UserId = row.UserID

                    Dim Travel As Double = 0
                    Dim AllowancesNontax As Double = 0
                    Dim AllowancesTax As Double = 0
                    '######################## NEED TO SET THE TRAVEL TYPE ########################
                    Dim TravelExpenseTypes = {58}
                    '######################## NEED TO SET THE TRAVEL TYPE ########################
                    Try
                        Travel = myRmbs.Where(Function(x) TravelExpenseTypes.Contains(x.LineType)).Sum(Function(y) CType(y.GrossAmount, Decimal?)) ' This needs better definition

                    Catch ex As Exception

                    End Try


                    Try
                        AllowancesNontax = myRmbs.Where(Function(x) x.Taxable = False And Not TravelExpenseTypes.Contains(x.LineType)).Sum(Function(y) CType(y.GrossAmount, Decimal?))

                    Catch ex As Exception

                    End Try
                    Try
                        AllowancesTax = myRmbs.Where(Function(x) x.Taxable = True And Not TravelExpenseTypes.Contains(x.LineType)).Sum(Function(y) CType(y.GrossAmount, Decimal?))

                    Catch ex As Exception

                    End Try

                    'Check Balances
                    If AccountBalance - (salary + Travel + AllowancesNontax + AllowancesTax) < 0 Then
                        'We need to reduce Salary
                        salary = Math.Max(AccountBalance - (Travel + AllowancesNontax + AllowancesTax), 0)
                    End If


                    'Dim SanLamGroupLife As Double = theUser.Profile.GetPropertyValue("SanlamGroupLife")
                    'Dim LibertyLifeRAF As Double = theUser.Profile.GetPropertyValue("LibertyLifeRAF")
                    'Dim OldMutualRAF As Double = theUser.Profile.GetPropertyValue("OldMutualRAF")
                    'Dim SalaryAdvance As Double = theUser.Profile.GetPropertyValue("SalaryAdvance")
                    'Dim SavingsScheme As Double = theUser.Profile.GetPropertyValue("SavingsScheme")



                    'Complete the Deductions columns
                    sql = "Update [Deductions$A" & i & ":Z" & i & "] Set F1=@EmpCode, F2='CCCSA-FIELD STAFF'"
                    myCommand.Parameters.AddWithValue("@EmpCode", EmpCode.Trim(" "))
                    j = 3
                    For Each item As DotNetNuke.Entities.Profile.ProfilePropertyDefinition In Deductions
                        If item.Deleted = False Then
                            Dim Value = theUser.Profile.GetPropertyValue(item.PropertyName)
                            If Value <> 0 Then
                                sql &= ",F" & j & "=" & Value

                            End If
                            j += 1
                        End If
                    Next


                    sql &= ",F" & j & "=" & 999
                    j += 1

                    sql &= ",F" & j & "=" & 1

                    sql &= " ;"

                    myCommand.CommandText = sql


                    myCommand.ExecuteNonQuery()
                    myCommand.Parameters.Clear()

                    'Complete the Earnings Columns
                    sql = "Update [Earnings$A" & i & ":H" & i & "] Set F1=@EmpCode, F2='CCCSA-FIELD STAFF'"
                    myCommand.Parameters.AddWithValue("@EmpCode", EmpCode.Trim(" "))
                    If salary <> 0 Then
                        sql &= ",F3=@Salary"
                        myCommand.Parameters.AddWithValue("@Salary", salary)
                    End If
                    If Travel <> 0 Then
                        sql &= ",F4=@Travel"
                        myCommand.Parameters.AddWithValue("@Travel", Travel)
                    End If
                    If AllowancesTax <> 0 Then
                        sql &= ",F5=@AllowancesTax"
                        myCommand.Parameters.AddWithValue("@AllowancesTax", AllowancesTax)
                    End If
                    If AllowancesNontax <> 0 Then
                        sql &= ",F6=@AllowancesNonTax"
                        myCommand.Parameters.AddWithValue("@AllowancesNonTax", AllowancesNontax)
                    End If
                    If CostCenter <> "" Then
                        sql &= ",F7=@RC"
                        myCommand.Parameters.AddWithValue("@RC", CostCenter.Trim(" "))
                    End If
                    If CostCenter <> "" Then
                        sql &= ",F8=@AccountBalance"
                        myCommand.Parameters.AddWithValue("@AccountBalance", AccountBalance)
                    End If
                    sql &= " ;"

                    myCommand.CommandText = sql








                    myCommand.ExecuteNonQuery()
                    myCommand.Parameters.Clear()




                    i += 1
                End If
            Next







        End Sub

        Protected Sub btnSuggestedPayments_Click(sender As Object, e As System.EventArgs) Handles btnSuggestedPayments.Click


            Dim filename As String = "SuggestedPayments.xls"
            If StaffBrokerFunctions.GetSetting("NetSalaries", PortalId) = "True" Then
                filename = "SuggestedPayments-NETSalary.xls"
            End If
            If StaffBrokerFunctions.GetSetting("ZA-Mode", PortalId) = "True" Then
                filename = "SuggestedPayments-ZA.xls"
                'filename = filename
            End If

            File.Copy(Server.MapPath("/DesktopModules/AgapeConnect/StaffRmb/" & filename), PortalSettings.HomeDirectoryMapPath & filename, True)



            Dim connStr As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & PortalSettings.HomeDirectoryMapPath & filename & "';Extended Properties='Excel 8.0;HDR=NO'"
            Dim MyConnection As OleDbConnection
            MyConnection = New OleDbConnection(connStr)

            MyConnection.Open()

            'Dim sql = ""
            Dim MyCommand As New OleDbCommand()
            MyCommand.Connection = MyConnection

            'Clear the form
            Try


                Dim sql2 = "Update [Suggested Payments$A4:J1000] Set F1='', F2='', F3='', F4='', F5='',F6='', F7='', F8='', F10='' ;"
                'MyCommand.CommandText = sql2
                'MyCommand.ExecuteNonQuery()




                Dim q = From c In ds.AP_Staff_SuggestedPayments Where c.PortalId = PortalId

                Dim i As Integer = 4


                For Each row In q
                    If d.AP_StaffBroker_CostCenters.Where(Function(x) x.PortalId = PortalId And x.CostCentreCode.Trim() = row.CostCenter.Trim() And x.Type = 1).Count > 0 Then


                        Dim salary1 = ""
                        Dim salary2 = ""
                        Dim expense = ""
                        Dim taxexpenses = ""
                        If cbSalaries.Checked And Not StaffBrokerFunctions.GetSetting("ZA-Mode", PortalId) = "True" Then
                            Dim staffMember = From c In ds.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.CostCenter = row.CostCenter.TrimEnd(" ")
                            If staffMember.Count > 0 Then
                                Dim s1 = From c In staffMember.First.AP_StaffBroker_StaffProfiles Where c.AP_StaffBroker_StaffPropertyDefinition.FixedFieldName = "Salary1" Select c.PropertyValue
                                If s1.Count > 0 Then
                                    salary1 = s1.First
                                End If
                                If staffMember.First.UserId2 > 0 Then
                                    Dim s2 = From c In staffMember.First.AP_StaffBroker_StaffProfiles Where c.AP_StaffBroker_StaffPropertyDefinition.FixedFieldName = "Salary2" Select c.PropertyValue
                                    If s2.Count > 0 Then
                                        salary2 = s2.First
                                    End If
                                End If


                            End If
                        End If

                        If cbExpenses.Checked Then

                            expense = row.ExpPayable
                            taxexpenses = row.ExpTaxable


                        End If

                        If Not (salary1 = "" And salary2 = "" And (expense = "" Or expense = "0") And (taxexpenses = "" Or taxexpenses = "0")) Then


                            Dim Name As String = (From c In d.AP_StaffBroker_CostCenters Where c.PortalId = PortalId And c.CostCentreCode = row.CostCenter.TrimEnd(" ")).First.CostCentreName
                            'Dim test = From c In d.AP_StaffBroker_CostCenters





                            Dim sql = "Update [Suggested Payments$A" & i & ":F" & i & "] Set F1=@RC , F2=@Name"
                            MyCommand.Parameters.AddWithValue("@RC", row.CostCenter.TrimEnd(" "))
                            MyCommand.Parameters.AddWithValue("@Name", Name)

                            If expense <> "" Then
                                sql &= ",F3=@Expense"
                                MyCommand.Parameters.AddWithValue("@Expense", expense)
                            End If
                            If taxexpenses <> "" Then
                                sql &= ",F4=@TaxExpense"
                                MyCommand.Parameters.AddWithValue("@TaxExpense", taxexpenses)
                            End If
                            If salary1 <> "" Then
                                sql &= ",F5=@Salary1"
                                MyCommand.Parameters.AddWithValue("@Salary1", salary1)
                            End If
                            If salary2 <> "" Then
                                sql &= ",F6=@Salary2"
                                MyCommand.Parameters.AddWithValue("@Salary2", salary2)
                            End If
                            sql &= " ;"

                            MyCommand.CommandText = sql






                            MyCommand.ExecuteNonQuery()
                            MyCommand.Parameters.Clear()
                            i += 1
                        End If
                    End If
                Next

                If cbSalaries.Checked Then
                    'Now insert the Salaries for staff who don't have a suggested payments entry
                    Dim otherStaff = From c In ds.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.Active = True

                    For Each member In otherStaff
                        If Not String.IsNullOrEmpty(member.CostCenter) Then
                            Dim search = From c In ds.AP_Staff_SuggestedPayments Where c.PortalId = PortalId And c.CostCenter.StartsWith(member.CostCenter)
                            If search.Count = 0 Then
                                'need to insert this one!
                                Dim salary1 = ""
                                Dim salary2 = ""
                                Dim s1 = From c In member.AP_StaffBroker_StaffProfiles Where c.AP_StaffBroker_StaffPropertyDefinition.FixedFieldName = "Salary1" Select c.PropertyValue
                                If s1.Count > 0 Then
                                    salary1 = s1.First
                                End If
                                If member.UserId2 > 0 Then
                                    Dim s2 = From c In member.AP_StaffBroker_StaffProfiles Where c.AP_StaffBroker_StaffPropertyDefinition.FixedFieldName = "Salary2" Select c.PropertyValue
                                    If s2.Count > 0 Then
                                        salary2 = s2.First
                                    End If
                                End If
                                If Not (salary1 = "" And salary2 = "") Then


                                    Dim Name As String = (From c In d.AP_StaffBroker_CostCenters Where c.PortalId = PortalId And c.CostCentreCode = member.CostCenter).First.CostCentreName

                                    Dim sql = "Update [Suggested Payments$A" & i & ":F" & i & "] Set F1=@RC, F2=@Name"
                                    MyCommand.Parameters.AddWithValue("@RC", member.CostCenter.TrimEnd(" "))
                                    MyCommand.Parameters.AddWithValue("@Name", Name)
                                    If salary1 <> "" Then
                                        sql &= ",F5=@Salary1"
                                        MyCommand.Parameters.AddWithValue("@Salary1", salary1)
                                    End If
                                    If salary2 <> "" Then
                                        sql &= ",F6=@Salary2"
                                        MyCommand.Parameters.AddWithValue("@Salary2", salary2)
                                    End If
                                    sql &= " ;"

                                    MyCommand.CommandText = sql





                                    MyCommand.ExecuteNonQuery()
                                    MyCommand.Parameters.Clear()
                                    i += 1
                                End If

                            End If

                        End If
                    Next


                End If


                'Now refresh the settings
                Dim period = StaffBrokerFunctions.GetSetting("CurrentFiscalPeriod", PortalId).Insert(4, "-")
                sql2 = "Update [Suggested Payments$P2:P2] Set F1='" & period & "' ;"
                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()

                sql2 = "Update [Suggested Payments$P3:P3] Set F1='" & Settings("ControlAccount") & "' ;"
                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()

                sql2 = "Update [Suggested Payments$P4:P4] Set F1='" & Settings("AccountsPayable") & "' ;"
                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()

                sql2 = "Update [Suggested Payments$P5:P5] Set F1='" & Settings("PayrollPayable") & "' ;"
                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()

                sql2 = "Update [Suggested Payments$P6:P6] Set F1='" & Settings("TaxAccountsReceivable") & "' ;"
                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()

                sql2 = "Update [Suggested Payments$P7:P7] Set F1='" & Settings("SalaryAccount") & "' ;"
                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()

                sql2 = "Update [Suggested Payments$P8:P8] Set F1='" & ddlBankAccount.SelectedValue & "' ;"
                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()

                sql2 = "Update [Suggested Payments$P9:P9] Set F1='" & StaffBrokerFunctions.GetSetting("CompanyName", PortalId) & "' ;"
                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()

                Dim DownloadFormat As Integer = 1
                Select Case Settings("DownloadFormat")
                    Case "DDC"
                        DownloadFormat = 1
                    Case "DCD"
                        DownloadFormat = 2
                    Case "CDDC"
                        DownloadFormat = 3
                    Case "CDCD"
                        DownloadFormat = 4

                End Select


                sql2 = "Update [Suggested Payments$P10:P10] Set F1=" & DownloadFormat & " ;"
                MyCommand.CommandText = sql2
                MyCommand.ExecuteNonQuery()


                If StaffBrokerFunctions.GetSetting("ZA-Mode", PortalId) = "True" Then
                    GetRSADownload(MyCommand)
                End If




                MyConnection.Close()
                Dim attachment As String = "attachment; filename=SuggestedPayments " & period & ".xls"

                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.ClearHeaders()
                HttpContext.Current.Response.ClearContent()
                HttpContext.Current.Response.AddHeader("content-disposition", attachment)
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
                HttpContext.Current.Response.AddHeader("Pragma", "public")
                HttpContext.Current.Response.WriteFile(PortalSettings.HomeDirectoryMapPath & filename)
                HttpContext.Current.Response.End()


            Catch ex As Exception
                lblError.Text = ex.Message
                lblError.Visible = True
                MyConnection.Close()
                ' File.Delete(PortalSettings.HomeDirectoryMapPath & filename)
            Finally


            End Try




        End Sub
#Region "Optional Interfaces"
        Private Sub AddClientAction(ByVal Title As String, ByVal theScript As String, ByRef root As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection, Optional ByVal Id As String = "")
            Dim jsAction As New DotNetNuke.Entities.Modules.Actions.ModuleAction(ModuleContext.GetNextActionID)
            With jsAction
                .Title = Title
                ' .CommandName = DotNetNuke.Entities.Modules.Actions.ModuleActionType.
                .Url = "javascript: " & theScript & ";"

                .ClientScript = theScript
                .CommandName = DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent

                .Secure = Security.SecurityAccessLevel.Edit
                .UseActionEvent = False
            End With
            root.Add(jsAction)
        End Sub
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get

                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection

                Actions.Add(GetNextActionID, "Expense Settings", "RmbSettings", "", "action_settings.gif", EditUrl("RmbSettings"), False, SecurityAccessLevel.Edit, True, False)

                AddClientAction("Download Batched Transactions", "showDownload()", Actions, "lnkBatched")
                AddClientAction("Suggested Payments", "showSuggestedPayments()", Actions, "lnkSuggested")
                AddClientAction("Download Period Expense Report", "showDownloadExpense()", Actions, "linkPeriod")
                For Each a As DotNetNuke.Entities.Modules.Actions.ModuleAction In Actions
                    If a.Title = "Download Batched Transactions" Or a.Title = "Suggested Payments" Or a.Title = "Download Period Expense Report" Then
                        a.Icon = "FileManager/Icons/xls.gif"


                    End If


                Next
                Return Actions
            End Get
        End Property

#End Region

        Protected Sub btnDownloadExpenseOK_Click(sender As Object, e As EventArgs) Handles btnDownloadExpenseOK.Click
            DownloadPeriodReport(ddlDownloadExpensePeriod.SelectedValue, ddlDownloadExpenseYEar.SelectedValue)

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
            If Not GridView1.HeaderRow Is Nothing Then
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
                GridView1.FooterRow.TableSection = TableRowSection.TableFooter
            End If
           

        End Sub
    End Class
End Namespace
