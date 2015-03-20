Imports System.Linq
Imports StaffRmb

Partial Class DesktopModules_StaffRmb_RmbPrintout
    Inherits System.Web.UI.Page

    Private et As String = "<table width=""100%"">[RMBHEADER1] [RMBLINES1] <tr> <td colspan=""4""> </td> </tr> <tr> <td colspan=""6"" class=""Agape_SubTitle"">[RCPTINSTRUCTIONS]</td> </tr> [RMBLINES2]" _
                           & "<tr> <td colspan=""4"" class=""AgapeH5"" align=""right""><strong>" & Translate("Total") & "</strong></td> <td><strong>[RMBTOTAL]</strong></td> <td></td> <td></td> </tr>" _
                           & "[LESSADVANCE]" _
                           & "</table> "
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

        Try



            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim Cur As String = StaffBrokerFunctions.GetSetting("Currency", PS.PortalId)

            Dim d As New StaffRmbDataContext
            Dim dt As New StaffBroker.TemplatesDataContext
            Dim q = From c In d.AP_Staff_Rmbs Where c.RMBNo = Request.QueryString("RmbNo") And c.UserId = Request.QueryString("UID")
            If q.Count > 0 Then

                Dim User = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo




                If User.UserID > 0 Then

                    Dim mc As New DotNetNuke.Entities.Modules.ModuleController
                    lblAccessDenied.Text = Translate("lblAccessDenied")
                    Dim x = mc.GetModuleByDefinition(PS.PortalId, "acStaffRmb")
                    Dim RmbSettings = x.TabModuleSettings

                    Dim RmbRel = StaffRmbFunctions.Authenticate(User.UserID, q.First.RMBNo, PS.PortalId)
                    If RmbRel = RmbAccess.Denied And Not User.IsInRole("Administrators") And Not (User.UserID = RmbSettings("AuthUser") Or User.UserID = RmbSettings("AuthAuthUser")) Then

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
                Dim output = StaffBrokerFunctions.GetTemplate("RmbPrintOut", PS.PortalId)
            
                Dim RmbNoText As String = q.First.RID
                If Request.QueryString("Year") <> "" And Request.QueryString("Period") <> "" Then
                    RmbNoText &= "<br /><span style=""font-size: 12pt;"">(" & Translate("Period") & Request.QueryString("Period") & ", " & Request.QueryString("Year").ToString & ")</span>"

                End If

                output = output.Replace("[RMBNO]", RmbNoText)
                If Not q.First.RmbDate Is Nothing Then
                    output = output.Replace("[SUBMITTEDDATE]", q.First.RmbDate.Value.ToString("dd/MM/yyyy"))
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

                If Not q.First.UserRef Is Nothing Then
                    output = output.Replace("[YOURREF]", q.First.UserRef)
                End If
                


                output = output.Replace("[CHARGETO]", GetCostCentreName(q.First.CostCenter, q.First.UserId, q.First.PortalId))

                If (q.First.AdvanceRequest > 0) And q.First.AP_Staff_RmbLines.Count > 0 Then
                    Dim total = q.First.AP_Staff_RmbLines.Sum(Function(c) c.GrossAmount)
                    Dim lessAdv = "<tr> <td colspan=""4"" class=""AgapeH5"" align=""right"">" & Translate("ClearAdvance") & "</td> <td>" & StaffBrokerFunctions.GetFormattedCurrency(PS.PortalId, (-q.First.AdvanceRequest).ToString("0.00")) & "</td> <td></td> <td></td> </tr>"

                    lessAdv &= "<tr> <td colspan=""4"" class=""AgapeH5"" align=""right""><strong>" & Translate("AmountPayable") & "</strong></td> <td><strong>" & StaffBrokerFunctions.GetFormattedCurrency(PS.PortalId, (total - q.First.AdvanceRequest).ToString("0.00")) & " </strong></td> <td></td> <td></td> </tr>"
                    et = et.Replace("[LESSADVANCE]", lessAdv)

                    If q.First.AdvanceRequest = q.First.AP_Staff_RmbLines.Sum(Function(c) c.GrossAmount) Then
                        et &= "<p>" & Translate("ClearAdvAll") & "</p>"

                    Else

                        et &= "<p>" & Translate("ClearAdvPartial").Replace("[CLEARADV]", StaffBrokerFunctions.GetFormattedCurrency(PS.PortalId, q.First.AdvanceRequest.ToString("0.00"))).Replace("[PAYABLE]", StaffBrokerFunctions.GetFormattedCurrency(PS.PortalId, (total - q.First.AdvanceRequest).ToString("0.00"))) & "</p>"

                    End If
                Else

                End If
                output = output.Replace("[EXPENSESTABLE]", et)


                If q.First.AP_Staff_RmbLines.Count > 0 Then
                    output = output.Replace("[RMBTOTAL]", Cur & (From c In q.First.AP_Staff_RmbLines Select c.GrossAmount).Sum().ToString("0.00"))
                Else
                    output = output.Replace("[RMBTOTAL]", Cur & "0.00")
                End If

                Dim lines As String = ""

                Dim theLines = From c In q.First.AP_Staff_RmbLines Where c.Receipt = False Or Not (c.ReceiptImageId Is Nothing)
                If theLines.Count > 0 Then
                    output = output.Replace("[RMBHEADER1]", "<tr class=""Agape_Red_H5""><td>" & Translate("Date") & "</td><td>" & Translate("Type") & "</td><td>" & Translate("Description") & "</td><td>" & Translate("Taxed") & "</td><td>" & Translate("Amount") & "</td><td></td><td></td><td>" & Translate("ReceiptNo") & "</td></tr>")

                    For Each row In theLines
                        lines = lines & "<tr><td>" & row.TransDate.ToString("dd/MM/yyyy") & "</td>"

                        lines = lines & "<td><span style=""color: #AAA;"">" & row.AccountCode & "-</span>" & GetLocalTypeName(row.LineType, PS.PortalId) & "</td>"
                        lines = lines & "<td>" & row.Comment

                        If row.AP_Staff_RmbLineType.TypeName = "Mileage" Then
                            If row.Spare1 > 0 Then


                                'lines += "<br/ ><span class=""Agape_SubTitle"">Passengers: "
                                'For Each person In row.Agape_Staff_RmbLine.AddStaffs
                                '    lines += person.Name & " + "
                                'Next
                                'lines = Left(lines, lines.Length - 3)
                                'lines += "</span>"
                            End If
                        End If

                        lines = lines & "</td>"
                        lines = lines & "<td>" & "</td>" ' IIf(row.Taxable, "Yes", "No") & "</td>"

                        Dim amount = Cur & row.GrossAmount.ToString("0.00")
                        Dim ac = StaffBrokerFunctions.GetSetting("AccountingCurrency", PS.PortalId)
                        If Not String.IsNullOrEmpty(row.OrigCurrency) Then
                            If row.OrigCurrency.ToUpper <> ac.ToUpper Then
                                amount &= "<span style=""font-size: x-small; font-style: italic; color: #AAA;"">  (" & row.OrigCurrencyAmount.Value.ToString("0.00") & row.OrigCurrency.ToUpper & ")</span>"
                            End If
                        End If

                        lines = lines & "<td>" & amount & "</td>"
                        lines = lines & "<td>" & "</td>"   ' row.VATCode & "</td>"
                        lines = lines & "<td></td><td>"
                        If Not (row.ReceiptImageId Is Nothing) Then
                            lines = lines & row.ReceiptNo
                        End If
                        lines = lines & "</td></tr>"

                    Next
                Else
                    output = output.Replace("[RMBHEADER1]", "")

                End If



                output = output.Replace("[RMBLINES1]", lines)

                lines = ""


                theLines = From c In q.First.AP_Staff_RmbLines Where c.Receipt = True And c.ReceiptImageId Is Nothing Order By c.ReceiptNo
                If theLines.Count > 0 Then
                    For Each row In theLines

                        lines = lines & "<tr><td>" & row.TransDate.ToShortDateString & "</td>"
                        lines = lines & "<td><span style=""color: #AAA;"">" & row.AccountCode & "-</span>" & GetLocalTypeName(row.LineType, PS.PortalId) & "</td>"
                        lines = lines & "<td>" & row.Comment
                        If row.AP_Staff_RmbLineType.TypeName = "Mileage" Then
                            If row.Spare1 > 0 Then


                                'lines += "<br/ ><span class=""Agape_SubTitle"">Passengers: "
                                'For Each person In row.Agape_Staff_RmbLineAddStaffs
                                '    lines += person.Name & " + "
                                'Next
                                'lines = Left(lines, lines.Length - 3)
                                'lines += "</span>"
                            End If
                        End If

                        lines = lines & "</td>"
                        lines = lines & "<td>" & "</td>" ' IIf(row.Taxable, "Yes", "No") & "</td>"

                        Dim amount = Cur & row.GrossAmount.ToString("0.00")
                        Dim ac = StaffBrokerFunctions.GetSetting("AccountingCurrency", PS.PortalId)
                        If Not String.IsNullOrEmpty(row.OrigCurrency) Then
                            If row.OrigCurrency.ToUpper <> ac.ToUpper Then
                                amount &= "<span style=""font-size: x-small; font-style: italic; color: #AAA;"">  (" & row.OrigCurrencyAmount.Value.ToString("0.00") & row.OrigCurrency.ToUpper & ")</span>"
                            End If
                        End If

                        lines = lines & "<td>" & amount & "</td>"
                        lines = lines & "<td>" & "</td>" ' IIf(row.VATReceipt, "Yes", "No") & "</td>"
                        lines = lines & "<td>" & "</td>" ' row.VATCode & "</td>"
                        lines = lines & "<td>" & row.ReceiptNo & "</td></tr>"
                    Next
                    Dim newHeaders As String = " <tr class=""Agape_Red_H5""><td>" & Translate("Date") & "</td><td>" & Translate("Type") & "</td><td>" & Translate("Description") & "</td><td>" & Translate("Taxed") & "</td><td>" & Translate("Amount") & "</td><td></td><td></td><td>" & Translate("ReceiptNo") & "</td> </tr>"


                    'output = output.Replace("[RCPTINSTRUCTIONS]", "The following expenses require a receipt. Please attach the receipts to this page (use extra pages if necessary) and number as listed below. Post this form directly to the National Office.")
                    output = output.Replace("[RCPTINSTRUCTIONS]", Translate("needReceipts"))


                    output = output.Replace("[RMBLINES2]", newHeaders & lines)
                Else
                    '   output = output.Replace("[RCPTINSTRUCTIONS]", "This reimbursement requires no receipts and you do not need to send any paperwork to Agap&eacute;. This page is for your records only.")
                    output = output.Replace("[RCPTINSTRUCTIONS]", Translate("noReceipts"))

                    output = output.Replace("[RMBLINES2]", lines)
                End If

                theLines = From c In q.First.AP_Staff_RmbLines Where c.Receipt = True And Not c.ReceiptImageId Is Nothing Order By c.ReceiptNo
                Dim ER As String = ""
                For Each row In theLines


                    Dim theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(row.ReceiptImageId)
                    If Not theFile Is Nothing Then


                        ER &= "<div style='align: center; float: left; margin: 5px; ' >"
                        If theFile.Extension.ToLower = "pdf" Then

                            ER &= "<iframe style='width: 747px; height: 1000px;' src='" & DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(theFile) & "' ></iframe>"
                        Else
                            ER &= "<img src='" & DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(theFile) & "'/>"

                        End If
                        ER &= "<div style='font-style: italic; color: #AAA; font-size: small; width: 100%; text-align: center;'>" & Translate("ReceiptNo") & ": " & row.ReceiptNo
                        Dim amount = row.GrossAmount.ToString("0.00")
                        Dim cr = Cur
                        If Not row.OrigCurrency Is Nothing And Not row.OrigCurrencyAmount Is Nothing Then
                            amount = row.OrigCurrencyAmount.Value.ToString("0.00")
                            cr = row.OrigCurrency
                        End If
                        ER &= "&nbsp;&nbsp;" & cr & amount
                        ER &= "&nbsp;&nbsp;<a href='" & DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(theFile) & "' target='_blank'>(Click here to open in new tab/window)</a> "
                        ER &= "</div>"
                        ER &= " </div><div style='clear: both;' />"
                    Else
                        ER &= "<div class='alert alert-error'> Error: Electronic Receipt missing for - " & row.ShortComment & "</div>"

                    End If

                Next
                output = output.Replace("[ELECTRONIC_RECEIPTS]", ER)

                If Not q.First.ApprDate Is Nothing Then
                    output = output.Replace("[APPROVEDON]", q.First.ApprDate.Value.ToString("dd/MM/yyyy"))
                Else
                    output = output.Replace("[APPROVEDON]", "")
                End If
                If Not q.First.ProcDate Is Nothing Then
                    output = output.Replace("[PROCESSEDON]", q.First.ProcDate.Value.ToString("dd/MM/yyyy"))
                Else
                    output = output.Replace("[PROCESSEDON]", "")
                End If
                output = output.Replace("[PAIDON]", "")
                output = output.Replace("[PAIDBY]", "")
                If Not q.First.ApprUserId Is Nothing Then
                    output = output.Replace("[APPROVEDBY]", UserController.GetUserById(PS.PortalId, q.First.ApprUserId).DisplayName)
                Else
                    output = output.Replace("[APPROVEDBY]", "")
                End If


                If Not q.First.ProcUserId Is Nothing Then
                    output = output.Replace("[PROCESSEDBY]", UserController.GetUserById(PS.PortalId, q.First.ProcUserId).DisplayName)
                Else
                    output = output.Replace("[PROCESSEDBY]", "")
                End If



                PlaceHolder1.Controls.Add(New LiteralControl(output))

            End If

        Catch ex As Exception
            pnlAccessDenied.Visible = True
            btnLogin.Visible = False
            lblAccessDenied.Text = "Error displaying prinout. The following information will allow the technical team to debug the issue you are experiencing: <br />" & ex.ToString
        End Try
    End Sub

    Public Function Translate(ByVal ResourceString As String) As String
        Dim rtn As String
        Try
            rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
            If String.IsNullOrEmpty(rtn) Then
                rtn = DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", "/DesktopModules/AgapeConnect/StaffRmb/App_LocalResources/RmbPrintout.ascx.resx")
            End If
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
        Dim dept = From c In d.AP_StaffBroker_Departments Where c.CostCentre = CostCentre And c.PortalId = PortalId

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
