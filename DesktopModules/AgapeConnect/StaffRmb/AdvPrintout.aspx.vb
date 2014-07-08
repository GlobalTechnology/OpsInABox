Imports System.Linq
Imports StaffRmb
Partial Class DesktopModules_StaffRmb_RmbPrintout
    Inherits System.Web.UI.Page

    ' Private et As String = "<table width=""100%"">[RMBHEADER1] [RMBLINES1] <tr> <td colspan=""4""> </td> </tr> <tr> <td colspan=""6"" class=""Agape_SubTitle"">[RCPTINSTRUCTIONS]</td> </tr> [RMBLINES2] <tr> <td colspan=""4"" class=""AgapeH5"" align=""right""><strong>" & Translate("Total") & "</strong></td> <td><strong>[RMBTOTAL]</strong></td> <td></td> <td></td> </tr></table> "
    Private LocalResourceFile As String

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)




        Dim FileName As String = "RmbPrintout"

        'System.IO.Path.GetFileNameWithoutExtension(Me.AppRelativeVirtualPath)

        ' this will fix it when its dynamically loaded using LoadControl method 
        'Me.LocalResourceFile = Me.LocalResourceFile & FileName & ".ascx.resx"
        LocalResourceFile = "/DesktopModules/AgapeConnect/StaffRmb/App_LocalResources/RmbPrintout.ascx.resx"



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
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim Cur As String = StaffBrokerFunctions.GetSetting("Currency", PS.PortalId)

        Dim d As New StaffRmbDataContext
        Dim dt As New StaffBroker.TemplatesDataContext
        Dim q = From c In d.AP_Staff_AdvanceRequests Where c.AdvanceId = Request.QueryString("AdvNo") And c.UserId = Request.QueryString("UID")

        If q.Count > 0 Then

            Dim User = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo




            If User.UserID > 0 Then
                Dim mc As New DotNetNuke.Entities.Modules.ModuleController
                lblAccessDenied.Text = Translate("lblAccessDenied")
                Dim x = mc.GetModuleByDefinition(PS.PortalId, "acStaffRmb")
                Dim RmbSettings = x.TabModuleSettings

                Dim AdvRel = StaffRmbFunctions.AuthenticateAdv(User.UserID, q.First.AdvanceId, PS.PortalId)
                If AdvRel = RmbAccess.Denied And Not User.IsInRole("Administrators") And Not (User.UserID = RmbSettings("AuthUser") Or User.UserID = RmbSettings("AuthAuthUser")) Then

                    Dim isAccounts = False
                    For Each role In CStr(RmbSettings("AccountsRoles")).Split(";")
                        If (User.Roles().Contains(role)) Then
                            isAccounts = True
                        End If
                    Next
                    If Not isAccounts Then
                        pnlAccessDenied.Visible = True
                        btnLogin.Visible = False
                        Return
                    End If
                End If

            Else
                pnlAccessDenied.Visible = True
                btnLogin.Visible = True
                lblAccessDenied.Text = Translate("lblNotLoggedIn")

                Return
            End If


            'Dim printout = From c In dt.AP_StaffBroker_Templates Where c.TemplateName = "RmbPrintOut" And c.PortalId = PS.PortalId Select c.TemplateHTML

            'If (Request.QueryString("mode") = "test") Then
            'printout = StaffBrokerFunctions.GetTemplate("RmbPrintOut", PS.PortalId)

            'End If

            'Dim output As String = ""
            'If printout.Count > 0 Then
            'output = Server.HtmlDecode(printout.First)
            'End If
            ' Dim output As String = System.IO.File.ReadAllText(Server.MapPath("RmbPrintOut.htm"))
            Dim output = StaffBrokerFunctions.GetTemplate("AdvPrintout", PS.PortalId)




            output = output.Replace("[ADVNO]", q.First.LocalAdvanceId)
            If Not q.First.RequestDate Is Nothing Then
                output = output.Replace("[SUBMITTEDDATE]", q.First.RequestDate.Value.ToString("dd/MM/yyyy"))
            Else
                If Request.QueryString("mode") = 1 Then
                    output = output.Replace("[SUBMITTEDDATE]", Today.ToString("dd/MM/yyyy"))
                Else
                    output = output.Replace("[SUBMITTEDDATE]", "")
                End If
            End If
            output = output.Replace("[SUBMITTEDBY]", UserController.GetUserById(q.First.PortalId, q.First.UserId).DisplayName)


            If Not Request.QueryString("Period") Is Nothing And Not Request.QueryString("Year") Is Nothing Then
                output = output.Replace("[POSTED]", "<span class=""Agape_Body_Text"">" & Translate("YearPosted") & Request.QueryString("Year") & ", " & Translate("PeriodPosted") & Request.QueryString("Period") & "</span><br/>")
            Else
                output = output.Replace("[POSTED]", "")
            End If

            If Not q.First.Period Is Nothing And Not q.First.Year Is Nothing Then
                output = output.Replace("[POSTED]", "<span class=""Agape_Body_Text"">" & Translate("YearPosted") & q.First.Year & ", " & Translate("PeriodPosted") & q.First.Period & "</span><br/>")
            End If

            Dim staffMember = StaffBrokerFunctions.GetStaffMember(q.First.UserId)





            output = output.Replace("[CHARGETO]", staffMember.DisplayName & "(" & staffMember.CostCenter & ")")






            output = output.Replace("[REASON]", q.First.RequestText)

            Dim curString = StaffBrokerFunctions.GetFormattedCurrency(PS.PortalId, q.First.RequestAmount.Value.ToString("0.00"))
            If Not String.IsNullOrEmpty(q.First.OrigCurrencyAmount) Then
                If q.First.OrigCurrency <> StaffBrokerFunctions.GetSetting("AccountingCurrency", PS.PortalId) Then
                    curString &= " (" & q.First.OrigCurrency & q.First.OrigCurrencyAmount.Value.ToString("0.00").Replace(".00", "") & ")"
                End If
            End If
           
            output = output.Replace("[AMOUNT]", curString)



            If Not q.First.ApprovedDate Is Nothing Then
                output = output.Replace("[APPROVEDON]", q.First.ApprovedDate.Value.ToString("dd/MM/yyyy"))
            Else
                output = output.Replace("[APPROVEDON]", "")
            End If
            If Not q.First.ProcessedDate Is Nothing Then
                output = output.Replace("[PROCESSEDON]", q.First.ProcessedDate.Value.ToString("dd/MM/yyyy"))
            Else
                output = output.Replace("[PROCESSEDON]", "")
            End If
            output = output.Replace("[PAIDON]", "")
            output = output.Replace("[PAIDBY]", "")
            If Not q.First.ApproverId Is Nothing Then
                output = output.Replace("[APPROVEDBY]", UserController.GetUserById(PS.PortalId, q.First.ApproverId).DisplayName)
            Else
                output = output.Replace("[APPROVEDBY]", "")
            End If



            output = output.Replace("[PROCESSEDBY]", "")




            PlaceHolder1.Controls.Add(New LiteralControl(output))

        End If
    End Sub

    Public Function Translate(ByVal ResourceString As String) As String
        Dim rtn As String
        Try
            rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)

        Catch ex As Exception
            rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/StaffRmb/App_LocalResources/RmbPrintout.ascx.resx")

        End Try

        Return rtn

    End Function



    Private Function GetCostCentreName(ByVal CostCentre As String, ByVal UserId As Integer, ByVal PortalId As Integer) As String

        Dim sm = StaffBrokerFunctions.GetStaffMember(UserId)
        'Dim d As New StaffBroker
        If (sm.CostCenter = CostCentre) Then
            Return sm.DisplayName & "(" & CostCentre & ")"
        End If
        Dim d As New StaffBroker.StaffBrokerDataContext
        Dim dept = From c In d.AP_StaffBroker_Departments Where c.CostCentre = CostCentre

        If dept.Count > 0 Then
            Return dept.First.Name & "(" & CostCentre & ")"
        Else
            Return CostCentre
        End If




    End Function
    Public Function GetLocalTypeName(ByVal LineTypeId As Integer, ByVal PortalId As Integer) As String
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

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Response.Redirect(NavigateURL(PS.LoginTabId) & "?returnurl=" & Server.UrlEncode(Request.Url.ToString))


    End Sub
End Class
