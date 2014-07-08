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
Imports DotNetNuke.Services.Authentication

Imports StaffBroker



Namespace DotNetNuke.Modules.StaffAdmin
    Partial Class ViewStaffAdmin
        Inherits Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable

        Dim d As New StaffBrokerDataContext
        Dim isMarried As Boolean

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            '    jQuery.RequestDnnPluginsRegistration()

            'Dim addTitle = MyBase.Actions.Add(GetNextActionID, "Staff Broker", "StaffBroker", "", "", "", "", True, SecurityAccessLevel.Edit, True, False)
            'addTitle.Actions.Add(GetNextActionID, "Add Staff", "AddStaff", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="AddStaff"), ModuleContext.EditUrl(controlKey:="AddStaff").Substring(11), True, SecurityAccessLevel.Edit, True, False)
            'addTitle.Actions.Add(GetNextActionID, "Staff Types", "StaffTypes", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="StaffTypes"), ModuleContext.EditUrl(controlKey:="StaffTypes").Substring(11), True, SecurityAccessLevel.Edit, True, False)
            'addTitle.Actions.Add(GetNextActionID, "Staff Profiles", "StaffProfileFields", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="StaffProfileFields"), ModuleContext.EditUrl(controlKey:="StaffProfileFields").Substring(11), True, SecurityAccessLevel.Edit, True, False)
            'addTitle.Actions.Add(GetNextActionID, "Departments", "Departments", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="Departments"), ModuleContext.EditUrl(controlKey:="Departments").Substring(11), True, SecurityAccessLevel.Edit, True, False)
            'addTitle.Actions.Add(GetNextActionID, "Templates", "Templates", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="Templates"), ModuleContext.EditUrl(controlKey:="Templates").Substring(11), True, SecurityAccessLevel.Edit, True, True)


            AddStaff1.PortalId = PortalId
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            'Label5.Text = CDate(UserController.GetUserById(PortalId, 7).Profile.GetPropertyValue("Birthday")).ToString("dd/MM/yyyy")
            ' tbTest.Text = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
            'tbTest.Text = Left(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, 8).ToLower
            ' Dim auth As New DatatSync





            Dim t As Type = Me.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")
            sb.Append("ddlUpdate();")
            'sb.Append("setUpMydTabs();")

            sb.Append("</script>")
            ScriptManager.RegisterStartupScript(Page, t, "ddlUpdate", sb.ToString, False)
            If Not Page.IsPostBack Then
                hfPortalId.Value = PortalId

                'HyperLink1.NavigateUrl = ModuleContext.EditUrl(controlKey:="AddStaff") & " ;"
                'HyperLink1.Attributes.Add("onclick", HyperLink1.NavigateUrl)
                Dim TabId = TabController.GetTabByTabPath(PortalId, "//Admin//UserAccounts", CultureInfo.CurrentCulture.Name)
                Dim modcont As New Entities.Modules.ModuleController

                Dim m As String = modcont.GetTabModules(TabId).First.Value.ModuleID


                hlStaffProfileProps.NavigateUrl = EditUrl(ControlKey:="StaffProfileFields") & " ;"

                hlStaffProfileProps.Attributes.Add("onclick", hlStaffProfileProps.NavigateUrl)

                '  Label5.Text = m & " : " & hlEditProfile.NavigateUrl


                Dim StaffList = (From c In d.AP_StaffBroker_Staffs Where c.Active And c.PortalId = PortalId Order By c.User.LastName Select DisplayName = c.User.LastName & ", " & c.User.FirstName, c.StaffId, c.UserId2, c.User2)
                Dim Staff As New Dictionary(Of String, Integer)
                For Each row In StaffList

                    Try


                        If row.UserId2 > 0 Then
                            If Not (Staff.ContainsKey(row.DisplayName & " & " & row.User2.FirstName)) Then
                                Staff.Add(row.DisplayName & " & " & row.User2.FirstName, row.StaffId)
                            End If

                        Else
                            If Not (Staff.ContainsKey(row.DisplayName)) Then
                                Staff.Add(row.DisplayName, row.StaffId)
                            End If


                        End If
                    Catch ex As Exception
                        AgapeLogger.WriteEventLog(UserId, "Staff Admin-> Error adding " & row.DisplayName & " to Staff List" & ex.ToString)
                    End Try
                Next



                ddlStaff.DataSource = Staff
                ddlStaff.DataTextField = "Key"
                ddlStaff.DataValueField = "Value"
                ddlStaff.DataBind()
                If Not Session("LastStaffId") Is Nothing Then
                    Try
                        ddlStaff.SelectedValue = Session("LastStaffId")
                    Catch ex As Exception

                    End Try
                End If

            End If



        End Sub
        Public Function GetBirthday(ByVal UID As String) As String
            Try
                Dim ukCulture As CultureInfo = New CultureInfo("en-GB")
                Dim value As DateTime = DateTime.Parse(UserController.GetUserById(PortalId, UID).Profile.GetPropertyValue("Birthday"), ukCulture.DateTimeFormat)

                Return value.ToShortDateString()


            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function GetLocalStaffProfileName(ByVal StaffProfileName As String) As String
            Dim s = Localization.GetString("ProfileProperties_" & StaffProfileName & ".Text", "/DesktopModules/Admin/Security/App_LocalResources/Profile.ascx.resx", System.Threading.Thread.CurrentThread.CurrentCulture.Name)
            If String.IsNullOrEmpty(s) Then
                Return StaffProfileName
            Else
                Return s
            End If
        End Function
        Public Function GetLocalStaffProfileHelp(ByVal StaffProfileName As String) As String
            Dim s = Localization.GetString("ProfileProperties_" & StaffProfileName & ".Help", "/DesktopModules/Admin/Security/App_LocalResources/Profile.ascx.resx", System.Threading.Thread.CurrentThread.CurrentCulture.Name)
            If String.IsNullOrEmpty(s) Then
                Return StaffProfileName
            Else
                Return s
            End If
        End Function
        Public Function GetPayroll(ByVal Category As String) As IEnumerable(Of DotNetNuke.Entities.Profile.ProfilePropertyDefinition)
            ' Dim theStaff = StaffBrokerFunctions.GetStaffbyStaffId(CInt(ddlStaff.SelectedValue))
            Dim pd = DotNetNuke.Entities.Profile.ProfileController.GetPropertyDefinitionsByCategory(PortalId, Category)
            Dim ProfProps As New ArrayList(pd)
            Dim q = (From c As DotNetNuke.Entities.Profile.ProfilePropertyDefinition In ProfProps Where c.Deleted = False)
            Return q

        End Function
        Public Function GetProfileValue(ByVal PropertyName As String, ByVal Spouse As Boolean, Optional ByVal Type As Integer = -1) As String
            Dim theStaff = StaffBrokerFunctions.GetStaffbyStaffId(CInt(ddlStaff.SelectedValue))
            Dim User1 = UserController.GetUserById(PortalId, theStaff.UserId1)
            Dim User2 = UserController.GetUserById(PortalId, theStaff.UserId2)
            If User1 Is Nothing Then
                Return ""
            End If
            If Spouse And User2 Is Nothing Then
                Return ""
            End If
            Dim ukCulture As CultureInfo = New CultureInfo("en-GB")
            If Not Spouse Then
                Dim val = User1.Profile.GetPropertyValue(PropertyName)
                Return val

            Else
                Dim val = User2.Profile.GetPropertyValue(PropertyName)

                Return val

            End If
            Return ""
        End Function
        Public Function GetPhoto(ByVal UID As Integer) As String
            Try
                Dim FileID = UserController.GetUserById(PortalId, UID).Profile.GetPropertyValue("Photo")
                Dim _theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileID)
                Return DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(_theFile)

            Catch ex As Exception
                Return "/images/no_avatar.gif"
            End Try
        End Function

        Public Function ConvertMaritalStatus(ByVal input As Integer) As Integer
            If input > 0 Then
                Return 0
            Else
                Return input
            End If
        End Function




        Public Function getEditProfileUrl(ByVal UID As Integer) As String
            Dim TabId = TabController.GetTabByTabPath(PortalId, "//Admin//UserAccounts", CultureInfo.CurrentCulture.Name)
            Dim modcont As New Entities.Modules.ModuleController

            Dim m As String = modcont.GetTabModules(TabId).First.Value.ModuleID
            Dim rtn = EditUrl(TabController.GetTabByTabPath(PortalId, "//Admin//UserAccounts", CultureInfo.CurrentCulture.Name), "Edit", False, "UserId", UID.ToString, "mid", m)
            ' rtn = rtn.Replace(",true,", ",false,")

            Return rtn
        End Function


        Protected Sub ddlStaff_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStaff.SelectedIndexChanged
            Session("LastStaffId") = ddlStaff.SelectedValue

        End Sub


        Public Function GetProfileValue(ByVal SP As System.Data.Linq.EntitySet(Of AP_StaffBroker_StaffProfile)) As String
            Dim q = From c In SP Where c.StaffId = CInt(ddlStaff.SelectedValue)
            If q.Count > 0 Then
                Return q.First.PropertyValue
            Else
                Return ""
            End If
        End Function

        Public Function GetProfileValueChecked(ByVal SP As System.Data.Linq.EntitySet(Of AP_StaffBroker_StaffProfile)) As String

            Try


                Dim q = From c In SP Where c.StaffId = CInt(ddlStaff.SelectedValue)
                If q.Count > 0 Then
                    Return CBool(q.First.PropertyValue)
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetClientIdForm(ByVal name As String) As String
            If FormView1.DataItemCount = 0 Then
                Return ""
            Else
                Return FormView1.FindControl(name).ClientID

            End If
        End Function
        Public Function GetClientId(ByVal name As String) As String
            Dim ctrl = FindControl(name)

            If ctrl Is Nothing Then
                Return ""
            Else
                Return ctrl.ClientID

            End If
        End Function
        Public Function GetDateFormat() As String
            Dim sdp As String = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower
            If sdp.IndexOf("d") < sdp.IndexOf("m") Then
                Return "dd/mm/yy"
            Else
                Return "mm/dd/yy"
            End If
        End Function

        Public Function GetAddStaffClientId(ByVal id As String) As String
            Return AddStaff1.GetClientId(id)
        End Function

        Protected Sub FormView1_ItemCommand(sender As Object, e As System.Web.UI.WebControls.FormViewCommandEventArgs) Handles FormView1.ItemCommand
            If e.CommandName = "Impersonate" Then
                Try
                    Dim objUserInfo = UserController.GetUserById(PortalId, CInt(e.CommandArgument))


                    UserController.UserLogin(PortalId, objUserInfo, PortalSettings.PortalName, AuthenticationLoginBase.GetIPAddress(), True)
                    Dim cookie = New HttpCookie("portalroles")
                    cookie.Expires = Now.AddYears(-2)
                    Response.Cookies.Add(cookie)
                    Response.Redirect(NavigateURL(PortalSettings.HomeTabId))
                Catch ex As Exception
                    lblWarning.Text = ex.Message
                    lblWarning.Visible = True
                End Try
            End If
        End Sub

        Protected Sub FormView1_ItemUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewUpdateEventArgs) Handles FormView1.ItemUpdating
            Try


                Dim q = From c In d.AP_StaffBroker_Staffs Where c.StaffId = CInt(e.Keys("StaffId"))
                If q.Count > 0 Then
                    Dim CC As String = ""
                    If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then
                        CC = CType(FormView1.FindControl("tbCostCentreCode"), TextBox).Text
                        If q.First.CostCenter <> CC Then
                            q.First.CostCenter = CC
                            d.SubmitChanges()
                        End If
                    Else

                        CC = CType(FormView1.FindControl("DropDownList1"), DropDownList).SelectedValue
                        If q.First.CostCenter <> CC Then
                            q.First.CostCenter = CC
                            d.SubmitChanges()
                            StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", PortalId)
                            Dim x As New tntWebUsers()
                            x.RefreshWebUsers({q.First.UserId1})
                            x.RefreshWebUsers(StaffBrokerFunctions.GetLeaders(q.First.UserId1, True).ToArray)

                            If q.First.UserId2 > 0 Then
                                x.RefreshWebUsers({q.First.UserId2})
                                x.RefreshWebUsers(StaffBrokerFunctions.GetLeaders(q.First.UserId2, True).ToArray)
                            End If


                        End If
                    End If





                    ' Dim User1 = UserController.GetUserById(PortalId, q.First.UserId1)
                    Dim dob1 As String = CType(FormView1.FindControl("tbUserDate"), TextBox).Text
                    If dob1 <> "" Then
                        Try
                            Dim B1 As Date = DateTime.Parse(dob1, CultureInfo.CurrentCulture)
                            StaffBrokerFunctions.SetUserProfileProperty(PortalId, q.First.UserId1, "Birthday", B1.ToString("dd/MM/yyyy"), 359)

                        Catch ex As Exception
                            CType(FormView1.FindControl("tbUserDate"), TextBox).Text = ""
                        End Try

                    End If




                    Dim PAC As String = CType(FormView1.FindControl("ddlPAC"), DropDownList).SelectedValue

                    StaffBrokerFunctions.AddProfileValue(PortalId, q.First.StaffId, "PersonalAccountCode", PAC)


                    If PAC <> e.OldValues("PAC") Then
                        StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", PortalId)
                    End If

                    Dim PayOnly As Boolean = CType(FormView1.FindControl("cbPayOnly"), CheckBox).Checked

                    StaffBrokerFunctions.AddProfileValue(PortalId, q.First.StaffId, "PayOnly", PayOnly)

                    ' User1.Profile.SetProfileProperty("Birthday", B1.ToString("dd/MM/yyyy"))
                    Dim MaritalStatus = CType(FormView1.FindControl("ddlMaritalStatus"), DropDownList).SelectedValue


                    If MaritalStatus = -1 Then 'notstaff


                        q.First.UserId2 = MaritalStatus
                        Dim dob2 As String = CType(FormView1.FindControl("tbSpouseDateNotStaff"), TextBox).Text
                        If dob2 <> "" Then
                            Try
                                Dim B2 As Date = DateTime.Parse(dob2, CultureInfo.CurrentCulture)
                                StaffBrokerFunctions.AddProfileValue(PortalId, q.First.StaffId, "SpouseDOB", B2.ToString("dd/MM/yyyy"))

                            Catch ex As Exception
                                CType(FormView1.FindControl("tbSpouseDateNotStaff"), TextBox).Text = ""
                            End Try

                        End If

                        StaffBrokerFunctions.AddProfileValue(PortalId, q.First.StaffId, "SpouseName", CType(FormView1.FindControl("tbName2"), TextBox).Text)

                    ElseIf MaritalStatus = -2 Then 'single
                        If q.First.UserId2 <> -2 Then
                            q.First.DisplayName = q.First.User.LastName & ", " & q.First.User.FirstName
                        End If

                        q.First.UserId2 = MaritalStatus


                    Else


                        Try
                            Dim B2 As Date = DateTime.Parse(CType(FormView1.FindControl("tbSpouseDateStaff"), TextBox).Text, CultureInfo.CurrentCulture)
                            StaffBrokerFunctions.SetUserProfileProperty(PortalId, q.First.UserId2, "Birthday", B2.ToString, 359)

                        Catch ex As Exception
                            CType(FormView1.FindControl("tbSpouseDateStaff"), TextBox).Text = ""
                        End Try

                        'Dim User2 = UserController.GetUserById(PortalId, q.First.UserId2)

                        'User2.Profile.SetProfileProperty("Birthday", B2.ToString("dd/MM/yyyy"))
                        'UserController.UpdateUser(PortalId, User2)
                    End If
                    'UserController.UpdateUser(PortalId, User1)
                    'StaffBrokerFunctions.AddProfileValue(PortalId, q.First.UserId1, "Birthday", )



                    'Now we need to update the Staff Details

                    'Dim SPP = From c In d.AP_StaffBroker_StaffPropertyDefinitions Where c.Display = True And c.PortalId = PortalId
                    'For Each row In SPP
                    '    StaffBrokerFunctions.AddProfileValue(PortalId, q.First.StaffId, row.PropertyName, )
                    'Next
                    Dim SPP = CType(FormView1.FindControl("DataList1"), DataList).Items

                    For Each row As DataListItem In SPP
                        Dim Type = CInt(CType(row.FindControl("hfPropType"), HiddenField).Value)
                        Select Case Type
                            Case 0
                                StaffBrokerFunctions.AddProfileValue(PortalId, q.First.StaffId, CType(row.FindControl("hfPropName"), HiddenField).Value, CType(row.FindControl("tbPropValue"), TextBox).Text)
                            Case 1
                                StaffBrokerFunctions.AddProfileValue(PortalId, q.First.StaffId, CType(row.FindControl("hfPropName"), HiddenField).Value, CType(row.FindControl("tbPropValueNumber"), TextBox).Text)
                            Case 2
                                StaffBrokerFunctions.AddProfileValue(PortalId, q.First.StaffId, CType(row.FindControl("hfPropName"), HiddenField).Value, CType(row.FindControl("cbProbValue"), CheckBox).Checked)

                        End Select


                    Next






                    Dim User1 = UserController.GetUserById(PortalId, q.First.UserId1)
                    If Not User1 Is Nothing Then
                        User1.Email = CType(FormView1.FindControl("tbEmail"), TextBox).Text
                        UserController.UpdateUser(PortalId, User1)
                    End If
                    Dim User2 As UserInfo
                    If q.First.UserId2 > 0 Then
                        User2 = UserController.GetUserById(PortalId, q.First.UserId2)
                        If Not User2 Is Nothing Then
                            User2.Email = CType(FormView1.FindControl("tbEmailSpouse"), TextBox).Text
                            UserController.UpdateUser(PortalId, User2)
                        End If
                    End If


                    Dim Payroll = CType(FormView1.FindControl("dlPayroll"), DataList).Items

                    For Each row As DataListItem In Payroll
                        Dim PropName As String = CType(row.FindControl("hfPropName"), HiddenField).Value
                        Dim PropType As Integer = CType(row.FindControl("hfPropType"), HiddenField).Value

                        User1.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue1"), TextBox).Text)
                        If Not User2 Is Nothing Then
                            User2.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue2"), TextBox).Text)
                        End If
                    Next

                    Dim PayrollDeductions = CType(FormView1.FindControl("dlPayrollDeductions"), DataList).Items

                    For Each row As DataListItem In PayrollDeductions
                        Dim PropName As String = CType(row.FindControl("hfPropName"), HiddenField).Value
                        Dim PropType As Integer = CType(row.FindControl("hfPropType"), HiddenField).Value

                        User1.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue1"), TextBox).Text)
                        If Not User2 Is Nothing Then
                            User2.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue2"), TextBox).Text)
                        End If
                    Next

                    Dim PayrollEarnings = CType(FormView1.FindControl("dlPayrollEarnings"), DataList).Items

                    For Each row As DataListItem In PayrollEarnings
                        Dim PropName As String = CType(row.FindControl("hfPropName"), HiddenField).Value
                        Dim PropType As Integer = CType(row.FindControl("hfPropType"), HiddenField).Value

                        User1.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue1"), TextBox).Text)
                        If Not User2 Is Nothing Then
                            User2.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue2"), TextBox).Text)
                        End If
                    Next

                    UserController.UpdateUser(PortalId, User1)
                    If Not User2 Is Nothing Then
                        UserController.UpdateUser(PortalId, User2)
                    End If


                    'Dim cc = CType(FormView1.FindControl("DropDownList1"), DropDownList).SelectedValue
                    'q.First.CostCenter = cc


                    d.SubmitChanges()

                End If
            Catch ex As Exception
                lblWarning.Text = ex.Message
                lblWarning.Visible = True

            End Try
        End Sub
        Protected Sub btnRepRel_Click(sender As Object, e As System.EventArgs) Handles btnRepRel.Click
            Response.Redirect(EditUrl("StaffReportingRelationships"))
        End Sub

        Public Function IsMarriedStaff() As Boolean
            If isMarried = Nothing Then
                isMarried = StaffBrokerFunctions.GetStaffbyStaffId(CInt(ddlStaff.SelectedValue)).UserId2 > 0
            End If

            Return isMarried
        End Function


        Protected Sub btnAddSpouse_Click(sender As Object, e As System.EventArgs) Handles btnAddSpouse.Click

            Dim staff = From c In d.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.StaffId = CInt(ddlStaff.SelectedValue)
            Dim rc As New Security.Roles.RoleController()
            If staff.First.UserId2 > 0 Then
                rc.DeleteUserRole(PortalId, staff.First.UserId2, rc.GetRoleByName(PortalId, "Staff").RoleID)
            End If
            Dim spouse As UserInfo = UserController.GetUserByName(PortalId, tbGCXUserName.Text & PortalId)

            If spouse Is Nothing Then
                spouse = StaffBrokerFunctions.CreateUser(PortalId, tbGCXUserName.Text, tbFirstName.Text, staff.First.User.LastName)
            End If

            rc.AddUserRole(PortalId, spouse.UserID, rc.GetRoleByName(PortalId, "Staff").RoleID, DateTime.MaxValue)

            staff.First.UserId2 = spouse.UserID
            staff.First.DisplayName = staff.First.User.LastName & ", " & staff.First.User.FirstName & " & " & spouse.FirstName
            d.SubmitChanges()
            Dim StaffList = (From c In d.AP_StaffBroker_Staffs Where c.Active And c.PortalId = PortalId Order By c.User.LastName Select DisplayName = c.User.LastName & ", " & c.User.FirstName, c.StaffId, c.UserId2, c.User2)
            Dim Staffs As New Dictionary(Of String, Integer)
            For Each row In StaffList
                If row.UserId2 > 0 Then
                    Staffs.Add(row.DisplayName & " & " & row.User2.FirstName, row.StaffId)
                Else
                    Staffs.Add(row.DisplayName, row.StaffId)

                End If

            Next



            ddlStaff.DataSource = Staffs
            ddlStaff.DataTextField = "Key"
            ddlStaff.DataValueField = "Value"
            ddlStaff.DataBind()
            If Not Session("LastStaffId") Is Nothing Then
                Try
                    ddlStaff.SelectedValue = Session("LastStaffId")
                Catch ex As Exception

                End Try
            End If
            FormView1.DataBind()
            Dim x As New tntWebUsers()
            x.CheckOrCreateWebUser(spouse.UserID)
            x.CheckOrCreateTNTProfile(spouse.UserID, "", "Personal Accounts")
            If Not String.IsNullOrEmpty(staff.First.CostCenter) Then
                x.CheckOrCreateTNTAccount(spouse.UserID, "", staff.First.CostCenter)
            End If



        End Sub

        Protected Sub btnTnT_Click(sender As Object, e As System.EventArgs) Handles btnTnT.Click
            Dim x As New tntWebUsers()
            x.RefreshWebUsers()
        End Sub

        Protected Sub btnBulkAdd_Click(sender As Object, e As EventArgs) Handles btnBulkAdd.Click
            Response.Redirect(EditUrl("BulkUpload"))
        End Sub

        Protected Sub btnChangeUsername_Click(sender As Object, e As EventArgs) Handles btnChangeUsername.Click
            Response.Redirect(EditUrl("ChangeUsername"))
        End Sub





        Protected Sub btnRCReport_Click(sender As Object, e As EventArgs) Handles btnRCReport.Click
            Dim csvOut As String = "R/C Code, RC Name, Type, Staff/Dept, Leader" & vbNewLine
            Try
                d = New StaffBrokerDataContext


                Dim ccs = From c In d.AP_StaffBroker_CostCenters Where c.PortalId = PortalId

                Dim Staff = From c In d.AP_StaffBroker_Staffs Where c.PortalId = PortalId And c.Active = True

                For Each row In ccs
                    Dim Depts = From c In d.AP_StaffBroker_Departments Where row.CostCentreCode = c.CostCentre

                    Dim s = From c In Staff Where c.CostCenter = row.CostCentreCode

                    For Each dep In Depts
                        If Not dep.CostCentreManager Is Nothing Then


                            Dim Man = UserController.GetUserById(PortalId, dep.CostCentreManager)
                            If Not Man Is Nothing Then
                                csvOut &= row.CostCentreCode & ",""" & row.CostCentreName & """, Manager of ,""" & dep.Name & ""","
                                csvOut &= Man.DisplayName & vbNewLine
                            End If
                        End If
                        If Not dep.CostCentreDelegate Is Nothing Then


                            Dim del = UserController.GetUserById(PortalId, dep.CostCentreDelegate)
                            If Not del Is Nothing Then
                                csvOut &= row.CostCentreCode & ",""" & row.CostCentreName & """, Manager of ,""" & dep.Name & ""","
                                csvOut &= """" & del.DisplayName & """" & vbNewLine
                            End If
                        End If
                    Next


                    For Each sm In s
                        Dim u1 = UserController.GetUserById(PortalId, sm.UserId1)

                        Dim u1Name = "Error - unknown"
                        If Not u1 Is Nothing Then
                            u1Name = u1.DisplayName
                        Else

                            StaffBrokerFunctions.EventLog("Error Exporting RC Report", "user does not exists for " & row.CostCentreCode & " : " & sm.UserId1, UserId)

                        End If
                        Dim leaders1 = StaffBrokerFunctions.GetLeadersDetailed(sm.UserId1, PortalId)

                        For Each l In leaders1
                            csvOut &= row.CostCentreCode & ",""" & row.CostCentreName & """,""" & IIf(l.isDelegate, "Delegate ", "") & "Leader of"",""" & u1Name & """,""" & l.DisplayName & """" & vbNewLine

                        Next



                        If sm.UserId2 > 0 Then
                            Dim u2 = UserController.GetUserById(PortalId, sm.UserId2)
                            Dim leaders2 = StaffBrokerFunctions.GetLeadersDetailed(sm.UserId2, PortalId)

                            For Each l In leaders1
                                csvOut &= row.CostCentreCode & ",""" & row.CostCentreName & """,""" & IIf(l.isDelegate, "Delegate ", "") & "Leader of"",""" & u1Name & """,""" & l.DisplayName & """" & vbNewLine

                            Next
                        End If


                    Next


                Next


                Dim attachment As String = "attachment; filename=RCReport.csv"

                HttpContext.Current.Response.Clear()

                HttpContext.Current.Response.ClearContent()
                HttpContext.Current.Response.AppendHeader("content-disposition", attachment)
                HttpContext.Current.Response.ContentType = "text/csv"
                HttpContext.Current.Response.AddHeader("Pragma", "public")
                HttpContext.Current.Response.Write(csvOut)

            Catch ex As Exception
                StaffBrokerFunctions.EventLog("RCReport error", ex.ToString, UserId)

            End Try

            HttpContext.Current.Response.End()

        End Sub

        


#Region "Optional Interfaces"
        Private Sub AddClientAction(ByVal Title As String, ByVal theScript As String, ByRef root As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection)
            Dim jsAction As New DotNetNuke.Entities.Modules.Actions.ModuleAction(ModuleContext.GetNextActionID)
            With jsAction
                .Title = Title
                ' .CommandName = DotNetNuke.Entities.Modules.Actions.ModuleActionType.
                .Url = "javascript: " & theScript & ";"
                .ClientScript = theScript
                .Secure = Security.SecurityAccessLevel.Edit
                .UseActionEvent = False
            End With
            root.Add(jsAction)
        End Sub
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get

                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection

                
                Dim addTitle = MyBase.Actions.Add(GetNextActionID, "Staff Broker", "StaffBroker", "", "", "", "", True, SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, "Add Staff", "AddStaff", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="AddStaff"), ModuleContext.EditUrl(controlKey:="AddStaff").Substring(11), True, SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, "Staff Types", "StaffTypes", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="StaffTypes"), ModuleContext.EditUrl(controlKey:="StaffTypes").Substring(11), True, SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, "Staff Profiles", "StaffProfileFields", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="StaffProfileFields"), ModuleContext.EditUrl(controlKey:="StaffProfileFields").Substring(11), True, SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, "Departments", "Departments", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="Departments"), ModuleContext.EditUrl(controlKey:="Departments").Substring(11), True, SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, "Templates", "Templates", "", "action_settings.gif", ModuleContext.EditUrl(controlKey:="Templates"), ModuleContext.EditUrl(controlKey:="Templates").Substring(11), True, SecurityAccessLevel.Edit, True, True)



                Return Actions
            End Get
        End Property

#End Region
    End Class


End Namespace
