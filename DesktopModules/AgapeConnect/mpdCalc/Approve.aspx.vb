Imports System.Linq
Imports StaffRmb

Partial Class DesktopModules_StaffRmb_AdvApprove
    Inherits System.Web.UI.Page


    Private LocalResourceFile As String

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)




        Dim FileName As String = "Approve"

        'System.IO.Path.GetFileNameWithoutExtension(Me.AppRelativeVirtualPath)

        ' this will fix it when its dynamically loaded using LoadControl method 
        'Me.LocalResourceFile = Me.LocalResourceFile & FileName & ".ascx.resx"
        LocalResourceFile = "/DesktopModules/AgapeConnect/mpdCalc/App_LocalResources/Approve.ascx.resx"



        Dim Locale = PS.CultureCode

        Dim AppLocRes As New System.IO.DirectoryInfo(Server.MapPath(LocalResourceFile.Replace(FileName & ".ascx.resx", "")))
        If Locale = PS.CultureCode Then
            'look for portal varient
            If AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PS.PortalId & ".resx").Count > 0 Then
                LocalResourceFile = LocalResourceFile.Replace("resx", "Portal-" & PS.PortalId & ".resx")
            End If
        Else

            If AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".Portal-" & PS.PortalId & ".resx").Count > 0 Then
                'lookFor a CulturePortalVarient
                LocalResourceFile = LocalResourceFile.Replace("resx", Locale & ".Portal-" & PS.PortalId & ".resx")
            ElseIf AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".resx").Count > 0 Then
                'look for a CultureVarient
                LocalResourceFile = LocalResourceFile.Replace("resx", Locale & ".resx")
            ElseIf AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PS.PortalId & ".resx").Count > 0 Then
                'lookFor a PortalVarient
                LocalResourceFile = LocalResourceFile.Replace("resx", "Portal-" & PS.PortalId & ".resx")
            End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnApprove.Text = Translate("btnApprove")
        btnUndo.Text = Translate("btnUndo")
        btnLogin.Text = Translate("btnLogin")

        If Not Page.IsPostBack Then

            Approve()

        End If
    End Sub

    Public Function Translate(ByVal ResourceString As String) As String
        Dim rtn As String
        Try
            rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
            If String.IsNullOrEmpty(rtn) Then
                rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/mpdCalc/App_LocalResources/Approve.ascx.resx")
            End If
        Catch ex As Exception

            rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/mpdCalc/App_LocalResources/Approve.ascx.resx")

        End Try

        Return rtn

    End Function
    Public Function TranslateRmb(ByVal ResourceString As String) As String
        Dim rtn As String

        rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/StaffRmb/App_LocalResources/StaffRmb.ascx.resx")


        Return rtn

    End Function

    Private Sub Approve()
        Try
            btnApprove.Visible = False
            Dim d As New MPD.MPDDataContext
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            If Not String.IsNullOrEmpty(Request.QueryString("m")) Then

                Dim decrypt = AgapeEncryption.AgapeEncrypt.Decrypt(HttpContext.Current.Server.UrlDecode(Request.QueryString("m")).Replace(" ", "+")).Split(";")

                Dim userId As Integer = decrypt(1)
                hfUserId.Value = userId
                Dim q = From c In d.AP_mpdCalc_StaffBudgets Where c.ApproveCode = decrypt(0) And c.AP_mpdCalc_Definition.PortalId = PS.PortalId And Not (c.ApproveCode = Nothing Or c.ApproveCode = "")
                If q.Count > 0 Then
                    hfSBNo.Value = q.First.StaffBudgetId

                    ' Dim mc As New DotNetNuke.Entities.Modules.ModuleController
                    ' Dim x = mc.GetModuleByDefinition(PS.PortalId, "acStaffRmb")
                    ' Dim RmbSettings = x.TabModuleSettings

                    'Dim theAuthUser = UserController.GetUserById(PS.PortalId, CInt(RmbSettings("AuthUser")))
                    ' Dim TheAuthAuthUser = UserController.GetUserById(PS.PortalId, CInt(RmbSettings("AuthAuthUser")))
                    Dim User = UserController.GetUserById(PS.PortalId, userId)
                    lblTitle.Text = Translate("MPDBudget")
                    lblSubTitle.Text = User.DisplayName
                    Dim Staff = StaffBrokerFunctions.GetStaffbyStaffId(q.First.StaffId)

                    Dim leaders = StaffBrokerFunctions.GetLeaders(Staff.UserId1)
                    If Staff.UserId2 > 0 Then
                        leaders.AddRange(StaffBrokerFunctions.GetLeaders(Staff.UserId2))
                    End If

                    If leaders.Count = 0 Then
                        leaders.Add(42)
                    End If

                    ' Dim approvers = StaffRmbFunctions.getAdvApprovers(q.First, RmbSettings("LargeTransaction"), theAuthUser, TheAuthAuthUser)

                    If leaders.Contains(userId) Then

                        If q.First.Status = RmbStatus.Cancelled Or q.First.Status = RmbStatus.Draft Then
                            lblApprove.Text = Translate("AlreadyCancelled")
                            btnUndo.Visible = False

                        ElseIf q.First.Status >= RmbStatus.Approved Then
                            Dim Approver = UserController.GetUserById(PS.PortalId, q.First.ApprovedBy)
                            lblApprove.Text = Translate("AlreadyApproved").Replace("[APPROVER]", Approver.DisplayName)

                            btnUndo.Visible = False
                            btnUndo.Visible = q.First.Status = RmbStatus.Approved
                        Else

                            'Dim submitter = UserController.GetUserById(PS.PortalId, Staff.)

                            'This user can approve
                            q.First.Status = RmbStatus.Approved
                            q.First.ApprovedBy = userId
                            q.First.ApprovedOn = Now

                            d.SubmitChanges()


                            lblApprove.Text = Translate("Approved").Replace("[SUBMITTER]", Staff.DisplayName)

                            btnUndo.Visible = True

                            StaffBrokerFunctions.EventLog("Adv Approved", "StaffBudget " & hfSBNo.Value & " was approved", hfUserId.Value)

                            SendApproveEmails(q.First, leaders, User, Staff)

                            d.SubmitChanges()
                        End If


                    End If
                Else
                    lblApprove.Text = Translate("NotFound")
                    btnUndo.Visible = False


                End If

            End If

            '  lblApprove.Text = HttpContext.Current.Server.UrlEncode(AgapeEncryption.AgapeEncrypt.Encrypt("TEST;41"))

            pnlApprove.Visible = True
        Catch ex As Exception
            lblApprove.Text = ex.ToString
            pnlApprove.Visible = True
        End Try
    End Sub


    Private Sub SendApproveEmails(ByVal bud As MPD.AP_mpdCalc_StaffBudget, ByVal myApprovers As List(Of Integer), ByVal approver As UserInfo, ByVal Submitter As StaffBroker.AP_StaffBroker_Staff)
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)



        Dim Emessage As String = StaffBrokerFunctions.GetTemplate("mpdApproved", PS.PortalId)

        Emessage = Emessage.Replace("[STAFFNAME]", Submitter.DisplayName)
        Emessage = Emessage.Replace("[APPROVER]", approver.DisplayName)
        Dim dt As New Date(Left(bud.BudgetPeriodStart, 4), Right(bud.BudgetPeriodStart, 2), 1)
        Emessage = Emessage.Replace("[STARTPERIOD]", dt.ToString("MMMM yyyy"))
        Dim toEmail = Submitter.User.Email

        If Submitter.UserId2 > 0 Then
            toEmail &= "; " & Submitter.User2.Email
        End If
        ' DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", theUser.Email, "donotreply@agape.org.uk", "Rmb#: " & hfRmbNo.Value & "-" & rmb.First.UserRef & " has been approved", Emessage, "", "HTML", "", "", "", "")
        DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", toEmail, "", "Budget Approved", Emessage, "", "HTML", "", "", "", "")





    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim mc As New DotNetNuke.Entities.Modules.ModuleController
        Dim x = mc.GetModuleByDefinition(PS.PortalId, "ac_mpdCalc")

        Dim returnURL = NavigateURL(x.TabID)

        If Not String.IsNullOrEmpty(hfSBNo.Value) Then
            returnURL &= "?sb=" & hfSBNo.Value

        End If

        Response.Redirect(NavigateURL(PS.LoginTabId) & "?returnurl=" & Server.UrlEncode(returnURL))


    End Sub

    Protected Sub btnUndo_Click(sender As Object, e As EventArgs) Handles btnUndo.Click
        'reset's the current reimbrusement to submitted (provided it is in the approved state
        If Not String.IsNullOrEmpty(hfSBNo.Value) Then
            Dim d As New MPD.MPDDataContext
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim q = From c In d.AP_mpdCalc_StaffBudgets Where c.StaffBudgetId = hfSBNo.Value And c.AP_mpdCalc_Definition.PortalId = PS.PortalId

            If q.Count > 0 Then
                If q.First.Status <> RmbStatus.Approved Then
                    lblApprove.Text = "Error: Approval and only be undone when the advance is in the 'approved' state. This budget is currently '" & RmbStatus.StatusName(q.First.Status) & "'"
                Else
                    q.First.Status = RmbStatus.Submitted

                    d.SubmitChanges()
                    lblApprove.Text = Translate("Undone")
                    btnUndo.Visible = False
                    btnApprove.Visible = True

                    StaffBrokerFunctions.EventLog("Adv Approval Undone", "Adv " & hfSBNo.Value & " was reverted from approved to submitted", hfUserId.Value)
                End If

            Else
                lblApprove.Text = Translate("NotFound")
            End If

        End If
    End Sub

    Protected Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        Approve()
    End Sub
End Class
