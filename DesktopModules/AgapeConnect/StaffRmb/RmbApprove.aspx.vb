Imports System.Linq
Imports StaffRmb

Partial Class DesktopModules_StaffRmb_RmbApprove
    Inherits System.Web.UI.Page

    
    Private LocalResourceFile As String

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)




        Dim FileName As String = "RmbApprove"

        'System.IO.Path.GetFileNameWithoutExtension(Me.AppRelativeVirtualPath)

        ' this will fix it when its dynamically loaded using LoadControl method 
        'Me.LocalResourceFile = Me.LocalResourceFile & FileName & ".ascx.resx"
        LocalResourceFile = "/DesktopModules/AgapeConnect/StaffRmb/App_LocalResources/RmbApprove.ascx.resx"



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
                rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/StaffRmb/App_LocalResources/RmbApprove.ascx.resx")
            End If
        Catch ex As Exception

            rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/StaffRmb/App_LocalResources/RmbApprove.ascx.resx")

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
            Dim d As New StaffRmbDataContext
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            If Not String.IsNullOrEmpty(Request.QueryString("r")) Then

                Dim decrypt = AgapeEncryption.AgapeEncrypt.Decrypt(HttpContext.Current.Server.UrlDecode(Request.QueryString("r")).Replace(" ", "+")).Split(";")

                Dim userId As Integer = decrypt(1)
                hfUserId.Value = userId
                Dim q = From c In d.AP_Staff_Rmbs Where c.SpareField1 = decrypt(0) And c.PortalId = PS.PortalId And Not c.SpareField1 = Nothing
                If q.Count > 0 Then
                    hfRmbNo.Value = q.First.RMBNo

                    Dim mc As New DotNetNuke.Entities.Modules.ModuleController
                    Dim x = mc.GetModuleByDefinition(PS.PortalId, "acStaffRmb")
                    Dim RmbSettings = x.TabModuleSettings

                    Dim theAuthUser = UserController.GetUserById(PS.PortalId, CInt(RmbSettings("AuthUser")))
                    Dim TheAuthAuthUser = UserController.GetUserById(PS.PortalId, CInt(RmbSettings("AuthAuthUser")))
                    Dim User = UserController.GetUserById(PS.PortalId, userId)
                    lblTitle.Text = "Reimbursement " & q.First.RID
                    lblSubTitle.Text = User.DisplayName
                    If q.First.Department Then
                        Dim dept = From c In d.AP_StaffBroker_Departments Where c.CostCentre = q.First.CostCenter

                        If dept.Count > 0 Then
                            lblSubTitle.Text &= " - " & dept.First.Name & "(" & q.First.CostCenter & ")"
                        Else
                            lblSubTitle.Text &= " - " & q.First.CostCenter
                        End If


                    End If
                    Dim approvers = StaffRmbFunctions.getApprovers(q.First, theAuthUser, TheAuthAuthUser)
                    If approvers.UserIds.Exists(Function(c) c.UserID = userId) Then
                        If q.First.Status = RmbStatus.Cancelled Or q.First.Status = RmbStatus.Draft Then
                            lblApprove.Text = Translate("AlreadyCancelled")
                            btnUndo.Visible = False

                        ElseIf q.First.Status >= RmbStatus.Approved Then
                            Dim Approver = UserController.GetUserById(PS.PortalId, q.First.ApprUserId)
                            lblApprove.Text = Translate("AlreadyApproved").Replace("[APPROVER]", Approver.DisplayName)

                            btnUndo.Visible = False
                            btnUndo.Visible = q.First.Status = RmbStatus.Approved
                        Else

                            Dim submitter = UserController.GetUserById(PS.PortalId, q.First.UserId)

                            'This user can approve
                            q.First.Status = RmbStatus.Approved
                            q.First.ApprUserId = userId
                            q.First.ApprDate = Now
                            q.First.Locked = True

                            d.SubmitChanges()


                            lblApprove.Text = Translate("Approved").Replace("[RMBNO]", q.First.RID).Replace("[SUBMITTER]", submitter.DisplayName)

                            btnUndo.Visible = True

                            StaffBrokerFunctions.EventLog("Rmb Approved", "Rmb " & hfRmbNo.Value & " was approved", hfUserId.Value)

                            SendApproveEmails(q.First, approvers, User, submitter)
                            q.First.Changed = False
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


    Private Sub SendApproveEmails(ByVal Rmb As AP_Staff_Rmb, ByVal myApprovers As StaffRmbFunctions.Approvers, ByVal approver As UserInfo, ByVal Submitter As UserInfo)
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        
        'SEND EMAIL TO OTHER APPROVERS
       
       
        Dim ApprMessage = ""
        '   Dim dr As New TemplatesDataContext
        '  Dim ConfTemplate = From c In dr.AP_StaffBroker_Templates Where c.TemplateName = "RmbApprovedEmail-ApproversVersion" And c.PortalId = PortalId Select c.TemplateHTML

        '  If ConfTemplate.Count > 0 Then
        'ApprMessage = Server.HtmlDecode(ConfTemplate.First)
        ' End If
        ApprMessage = StaffBrokerFunctions.GetTemplate("RmbApprovedEmail-ApproversVersion", PS.PortalId)

        ApprMessage = ApprMessage.Replace("[APPRNAME]", approver.DisplayName).Replace("[RMBNO]", Rmb.RMBNo).Replace("[STAFFNAME]", Submitter.DisplayName)


        For Each row In (From c In myApprovers.UserIds Where c.UserID <> Rmb.UserId)
            ApprMessage = ApprMessage.Replace("[THISAPPRNAME]", row.DisplayName)
            'DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", row.Email, "donotreply@agape.org.uk", "Rmb#:" & hfRmbNo.Value & " has been approved by " & ObjAppr.DisplayName, ApprMessage, "", "HTML", "", "", "", "")
            DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", row.Email, "", TranslateRmb("EmailApprovedSubjectA").Replace("[RMBNO]", Rmb.RID).Replace("[APPROVER]", approver.DisplayName), ApprMessage, "", "HTML", "", "", "", "")

        Next





        'SEND APRROVE EMAIL

        Dim Emessage = ""

        ' Dim ApprovedTemp = From c In dr.AP_StaffBroker_Templates Where c.TemplateName = "RmbApprovedEmail" And PortalId = c.PortalId Select c.TemplateHTML
        Emessage = StaffBrokerFunctions.GetTemplate("RmbApprovedEmail", PS.PortalId)
        'If ApprovedTemp.Count > 0 Then
        'Emessage = Server.HtmlDecode(ApprovedTemp.First)
        ' End If
        Emessage = Emessage.Replace("[STAFFNAME]", Submitter.DisplayName).Replace("[RMBNO]", Rmb.RID).Replace("[USERREF]", IIf(Rmb.UserRef <> "", Rmb.UserRef, "None"))
        Emessage = Emessage.Replace("[APPROVER]", approver.DisplayName)
        If Rmb.Changed = True Then
            Emessage = Emessage.Replace("[CHANGES]", ". " & TranslateRmb("EmailApproverChanged"))
            ' Rmb.Changed = False
        Else
            Emessage = Emessage.Replace("[CHANGES]", "")
        End If
        ' d.SubmitChanges()

        ' DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agape.org.uk", theUser.Email, "donotreply@agape.org.uk", "Rmb#: " & hfRmbNo.Value & "-" & rmb.UserRef & " has been approved", Emessage, "", "HTML", "", "", "", "")
        DotNetNuke.Services.Mail.Mail.SendMail("donotreply@agapeconnect.me", Submitter.Email, "", TranslateRmb("EmailApprovedSubjectP").Replace("[RMBNO]", Rmb.RID).Replace("[USERREF]", Rmb.UserRef), Emessage, "", "HTML", "", "", "", "")

       
       



    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim mc As New DotNetNuke.Entities.Modules.ModuleController
        Dim x = mc.GetModuleByDefinition(PS.PortalId, "acStaffRmb")

        Dim returnURL = NavigateURL(x.TabID)

        If Not String.IsNullOrEmpty(hfRmbNo.Value) Then
            returnURL &= "?RmbNo=" & hfRmbNo.Value

        End If

        Response.Redirect(NavigateURL(PS.LoginTabId) & "?returnurl=" & Server.UrlEncode(returnURL))


    End Sub

    Protected Sub btnUndo_Click(sender As Object, e As EventArgs) Handles btnUndo.Click
        'reset's the current reimbrusement to submitted (provided it is in the approved state
        If Not String.IsNullOrEmpty(hfRmbNo.Value) Then
            Dim d As New StaffRmbDataContext
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim q = From c In d.AP_Staff_Rmbs Where c.RMBNo = hfRmbNo.Value And c.PortalId = PS.PortalId

            If q.Count > 0 Then
                If q.First.Status <> RmbStatus.Approved Then
                    lblApprove.Text = "Error: Approval and only be undone when the reimbursement is in the 'approved' state. This reimbursement is currently '" & RmbStatus.StatusName(q.First.Status) & "'"
                Else
                    q.First.Status = RmbStatus.Submitted
                    q.First.Locked = False
                    d.SubmitChanges()
                    lblApprove.Text = Translate("Undone").Replace("[RMBNO]", q.First.RID)
                    btnUndo.Visible = False
                    btnApprove.Visible = True

                    StaffBrokerFunctions.EventLog("Rmb Approval Undone", "Rmb " & hfRmbNo.Value & " was reverted from approved to submitted", hfUserId.Value)
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
