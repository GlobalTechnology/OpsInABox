Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker


Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class EditStaffProfile
        Inherits Entities.Modules.PortalModuleBase

        Dim staff As New StaffBroker.AP_StaffBroker_Staff
        Dim User1 As UserInfo
        Dim User2 As UserInfo

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            If Not UserInfo.IsInRole("Staff") Then


                pnlStaffProfile.Visible = False
                If Request.QueryString("UserId") Then
                    Response.Redirect(NavigateURL() & "?ctl=Profile&UserId=" & UserId)
                End If
            End If
        End Sub


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load



            staff = StaffBrokerFunctions.GetStaffMember(UserId)
            If Not staff Is Nothing Then
                User1 = UserController.GetUserById(PortalId, staff.UserId1)
                If staff.UserId2 > 0 Then
                    User2 = UserController.GetUserById(PortalId, staff.UserId2)
                End If
                hfUserId1.Value = staff.UserId1
                hfUserId2.Value = staff.UserId2
                hfPortalId.Value = PortalId

                If hfUserId2.Value <= 0 Then
                    tbEmail2.Visible = False
                End If
                lblWarning.Text = Translate("lblWarning")
                lblPhoto.Text = Translate("lblPhoto")
                lblPhoto.HelpText = Translate("lblPhotoHelpText")
                lblProfile.Text = Translate("lblProfile")
                lblEmployment.Text = Translate("lblEmployment")
                lblLeadership.Text = Translate("lblLeadership")
                lblFinance.Text = Translate("lblFinance")
                lblEmail.Text = Translate("lblEmail")
                lblEmail.HelpText = Translate("lblEmailHelpText")
                lblResponsibility.Text = Translate("lblResponsibility")
                lblResponsibility.HelpText = Translate("lblResponsibilityHelpText")
                lblReport.Text = Translate("lblReport")
                lblReport.HelpText = Translate("lblReportHelpText")
                lblLeading.Text = Translate("lblLeading")
                lblLeading.HelpText = Translate("lblLeadingHelpText")
                lblManager.Text = Translate("lblManager")
                lblManager.HelpText = Translate("lblManagerHelpText")
                lblCustomize.Text = Translate("lblCustomize")
                lblGivePage.Text = Translate("lblGivePage")
                lblGiveInstructions.Text = Translate("lblGiveInstructions")
                lblJointPhoto.Text = Translate("lblJointPhoto")
                btnUpdate.Text = Translate("btnUpdate")
                btnSettings.Text = Translate("btnSettings")
                btnProfile.Text = Translate("btnProfile")
            End If


            If Not Page.IsPostBack Then
                If Not staff Is Nothing Then



                    LoadStaff()
                    If Request.QueryString("tab") <> "" Then
                        theHiddenTabIndex.Value = Request.QueryString("tab")
                    End If

                End If
            End If
        End Sub


        Protected Sub LoadStaff()
            lblName1.Text = staff.User.FirstName
            lbl2Name1.Text = staff.User.FirstName
            tbEmail1.Text = staff.User.Email
            tbGivingText.Text = StaffBrokerFunctions.GetStaffProfileProperty(staff.StaffId, "GivingText")

            If User1.Profile.GetPropertyValue("Photo") <> "" Then
                profileImage1.FileId = User1.Profile.GetPropertyValue("Photo")
            End If
            If StaffBrokerFunctions.GetStaffProfileProperty(staff.StaffId, "JointPhoto") <> "" Then
                JointPhoto.FileId = StaffBrokerFunctions.GetStaffProfileProperty(staff.StaffId, "JointPhoto")

            End If

            tbCostCenter.Text = staff.CostCenter
            Dim ProfProps As New ArrayList()

            If Not String.IsNullOrEmpty(CStr(Settings("ProfProps"))) Then
                Dim pp = CStr(Settings("ProfProps")).Split(";")

                For Each row In pp
                    Dim thisProp = DotNetNuke.Entities.Profile.ProfileController.GetPropertyDefinition(row.TrimStart(";"), PortalId)
                    ProfProps.Add(thisProp)

                Next
            End If


            dlProfileProps.DataSource = ProfProps
            dlProfileProps.DataBind()

            Leaders1.UID = staff.UserId1
            Leaders1.DataBind()
            Leaders2.UID = staff.UserId2
            Leaders2.Visible = staff.UserId2 > 0
            Leaders2.DataBind()
            Plebs1.UID = staff.UserId1
            Plebs1.DataBind()
            Plebs2.UID = staff.UserId2
            Plebs2.Visible = staff.UserId2 > 0
            Plebs2.DataBind()
            Depts1.UID = staff.UserId1
            Depts1.DataBind()
            Depts2.UID = staff.UserId2
            Depts2.Visible = staff.UserId2 > 0
            Depts2.DataBind()
            If staff.UserId2 > 0 Then

                lblName2.Text = staff.User2.FirstName
                lbl2Name2.Text = staff.User2.FirstName
                tbEmail2.Text = staff.User2.Email
                If User2.Profile.GetPropertyValue("Photo") <> "" Then
                    profileImage2.FileId = User2.Profile.GetPropertyValue("Photo")
                End If
            Else
                lblName2.Visible = False
                profileImage2.Visible = False
            End If
            'profileImage1.LazyLoad(False)
            'profileImage2.LazyLoad(False)


        End Sub

        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
        End Function

        Public Function GetTextFromNullable(ByVal TextIn As String, ByVal AlternateText As String) As String
            If String.IsNullOrEmpty(TextIn) Then
                Return AlternateText
            Else
                Return TextIn
            End If
        End Function

        Public Function AmIVisible(ByVal StaffPropertyDefinitionId As Integer) As Boolean

            If CStr(Settings("StaffProps")) <> "" Then
                Dim sp = From c In CStr(Settings("StaffProps")).Split(";")
                Return (From c In sp Where c.TrimStart(";") = StaffPropertyDefinitionId).Count > 0
            End If
            Return False
        End Function
        Public Function AmIVisibleGiving(ByVal StaffPropertyDefinitionId As Integer) As Boolean

            If CStr(Settings("GivingProps")) <> "" Then
                Dim gp = From c In CStr(Settings("GivingProps")).Split(";")
                Return (From c In gp Where c.TrimStart(";") = StaffPropertyDefinitionId).Count > 0
            End If
            Return False
        End Function

        Public Function GetProfileValue(ByVal PropertyName As String, ByVal Spouse As Boolean, Optional ByVal Type As Integer = -1) As String
            If User1 Is Nothing Then
                Return ""
            End If
            If Spouse And User2 Is Nothing Then
                Return ""
            End If
            Dim ukCulture As CultureInfo = New CultureInfo("en-GB")
            If Not Spouse Then
                Dim val = User1.Profile.GetPropertyValue(PropertyName)
                If Type = 359 And val <> "" Then
                    Try

                    
                    Dim value As DateTime = DateTime.Parse(val, ukCulture.DateTimeFormat)

                        Return value.ToShortDateString()
                    Catch ex As Exception
                        Return ""
                    End Try
                Else
                    Return val
                End If
            Else
                Dim val = User2.Profile.GetPropertyValue(PropertyName)
                If Type = 359 And val <> "" Then
                    Dim value As DateTime = DateTime.Parse(val, ukCulture.DateTimeFormat)

                    Return value.ToShortDateString()
                Else
                    Return val
                End If
            End If
            Return ""
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
        Public Function GetDateFormat() As String
            Dim sdp As String = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower
            If sdp.IndexOf("d") < sdp.IndexOf("m") Then
                Return "dd/mm/yy"
            Else
                Return "mm/dd/yy"
            End If
        End Function

        Public Function GetStaffProfileValue(ByVal SP As System.Data.Linq.EntitySet(Of AP_StaffBroker_StaffProfile)) As String
            Dim q = From c In SP Where c.StaffId = staff.StaffId
            If q.Count > 0 Then
                Return q.First.PropertyValue
            Else
                Return ""
            End If
        End Function

        Public Function GetStaffProfileValueChecked(ByVal SP As System.Data.Linq.EntitySet(Of AP_StaffBroker_StaffProfile)) As String

            Try
                Dim q = From c In SP Where c.StaffId = staff.StaffId
                If q.Count > 0 Then
                    Return CBool(q.First.PropertyValue)
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function




#Region "Events"
       
        Protected Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
            Try
                staff = StaffBrokerFunctions.GetStaffMember(UserId)
                If Not staff Is Nothing Then
                    User1 = UserController.GetUserById(PortalId, staff.UserId1)
                    If staff.UserId2 > 0 Then
                        User2 = UserController.GetUserById(PortalId, staff.UserId2)
                    End If

                    StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, "GivingText", tbGivingText.Text)
                    If profileImage1.FileId > 0 Then
                        If profileImage1.CheckAspect() Then
                            User1.Profile.SetProfileProperty("Photo", profileImage1.FileId)
                        Else
                            'Flag Validation - Aspect not Set
                            lblTest.Text = "* Error - Incorect Photo aspect. Please crop your photo, to the correct size - by dragging a rectangle on the image"
                            lblTest.Visible = True
                            Return
                        End If
                    End If
                    If JointPhoto.FileId > 0 Then
                        If JointPhoto.CheckAspect() Then
                            StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, "JointPhoto", JointPhoto.FileId)

                        Else
                            'Flag Validation - Aspect not Set
                            lblTest.Text = "* Error - Incorect Photo aspect. Please crop your photo, to the correct size - by dragging a rectangle on the image"
                            lblTest.Visible = True
                            Return
                        End If
                    End If

                    For Each row As DataListItem In dlProfileProps.Items
                        Dim PropName As String = CType(row.FindControl("hfPropName"), HiddenField).Value
                        Dim PropType As Integer = CType(row.FindControl("hfPropType"), HiddenField).Value
                        If PropType = 359 Then
                            Dim theText = CType(row.FindControl("tbDateValue1"), TextBox).Text
                            If theText <> "" Then
                                Dim B1 As Date = DateTime.Parse(CType(row.FindControl("tbDateValue1"), TextBox).Text, CultureInfo.CurrentCulture)
                                User1.Profile.SetProfileProperty(PropName, B1.ToString("dd/MM/yyyy"))
                            Else
                                User1.Profile.SetProfileProperty(PropName, theText)
                            End If


                        Else
                            User1.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue1"), TextBox).Text)
                        End If
                        If staff.UserId2 > 0 Then
                            If PropType = 359 Then
                                Dim theText2 = CType(row.FindControl("tbDateValue2"), TextBox).Text
                                If theText2 <> "" Then
                                    Dim B2 As Date = DateTime.Parse(theText2, CultureInfo.CurrentCulture)
                                    User2.Profile.SetProfileProperty(PropName, B2.ToString("dd/MM/yyyy"))
                                Else
                                    User2.Profile.SetProfileProperty(PropName, theText2)
                                End If


                            Else
                                User2.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue2"), TextBox).Text)
                            End If
                        End If
                    Next

                    For Each row As DataListItem In dlGivingQuestions.Items
                        If row.Visible Then
                            Dim Type = CInt(CType(row.FindControl("hfPropType"), HiddenField).Value)
                            Select Case Type
                                Case 0
                                    StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, CType(row.FindControl("hfPropName"), HiddenField).Value, CType(row.FindControl("tbPropValue"), TextBox).Text)
                                Case 1
                                    StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, CType(row.FindControl("hfPropName"), HiddenField).Value, CType(row.FindControl("tbPropValueNumber"), TextBox).Text)
                                Case 2
                                    StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, CType(row.FindControl("hfPropName"), HiddenField).Value, CType(row.FindControl("cbProbValue"), CheckBox).Checked)

                            End Select



                        End If
                    Next




                    User1.Email = tbEmail1.Text

                    UserController.UpdateUser(PortalId, User1)
                    If staff.UserId2 > 0 Then

                        If profileImage1.FileId > 0 Then
                            If profileImage2.CheckAspect() Then
                                User2.Profile.SetProfileProperty("Photo", profileImage2.FileId)
                            Else
                                'Flag Validation - Aspect not Set
                                lblTest.Text = "* Error - Incorect Photo aspect. Please crop your photo, to the correct size - by dragging a rectangle on the image"
                                lblTest.Visible = True
                                Return
                            End If
                            User2.Email = tbEmail2.Text
                            UserController.UpdateUser(PortalId, User2)
                        End If
                    End If
                    LoadStaff()
                    dlStaffProfile.DataBind()
                    dlGivingQuestions.DataBind()

                End If
            Catch ex As Exception
                lblTest.Text = ex.Message
                lblTest.Visible = True
            End Try
        End Sub
#End Region

#Region "Functions"

        
        Public Function GetSelected(ByVal LeaderId As Integer, ByVal ThisUserId As Integer) As Integer
            Dim d As New StaffBrokerDataContext
            Dim q = From c In d.AP_StaffBroker_LeaderMetas Where c.LeaderId = LeaderId And c.UserId = ThisUserId Select c.DelegateId
            If q.Count > 0 Then
                If q.First > 0 Then
                    Return q.First
                Else
                    Return -100
                End If
            Else
                Return -100
            End If
        End Function
        Public Function GetCCs(ByVal ManagerId As Integer) As IQueryable(Of StaffBroker.AP_StaffBroker_Department)
            Dim d As New StaffBrokerDataContext
            Dim q = From c In d.AP_StaffBroker_Departments Where c.CostCentreManager = ManagerId And c.PortalId = PortalId Select c

            Return From c In q Order By c.Name


        End Function
        Public Function GetSelectedCC(ByVal ManagerId As Integer, ByVal CCId As Integer) As Integer
            Dim d As New StaffBrokerDataContext
            Dim q = From c In d.AP_StaffBroker_Departments Where c.CostCentreManager = ManagerId And c.CostCenterId = CCId And c.PortalId = PortalId Select c.CostCentreDelegate
            If q.Count > 0 Then
                If q.First > 0 Then
                    Return q.First
                Else
                    Return -100
                End If
            Else
                Return -100
            End If
        End Function

     


#End Region

        Protected Sub btnSettings_Click(sender As Object, e As System.EventArgs) Handles btnSettings.Click
            Response.Redirect(EditUrl("ProfileSettings"))

        End Sub

        Protected Sub profileImage1_Updated() Handles profileImage1.Updated
            If profileImage1.FileId > 0 Then
                If profileImage1.CheckAspect() Then
                    User1.Profile.SetProfileProperty("Photo", profileImage1.FileId)
                Else
                    'Flag Validation - Aspect not Set
                    lblTest.Text = "* Error - Incorect Photo aspect. Please crop your photo, to the correct size - by dragging a rectangle on the image"
                    lblTest.Visible = True
                    Return
                End If
            End If
        End Sub

        Protected Sub profileImage2_Updated() Handles profileImage2.Updated
            If profileImage2.FileId > 0 Then
                If profileImage2.CheckAspect() Then
                    User2.Profile.SetProfileProperty("Photo", profileImage2.FileId)
                Else
                    'Flag Validation - Aspect not Set
                    lblTest.Text = "* Error - Incorect Photo aspect. Please crop your photo, to the correct size - by dragging a rectangle on the image"
                    lblTest.Visible = True
                    Return
                End If
            End If
        End Sub

        Protected Sub profileImageJoint_Updated() Handles JointPhoto.Updated
            If JointPhoto.FileId > 0 Then

                If JointPhoto.CheckAspect() Then
                    staff = StaffBrokerFunctions.GetStaffMember(UserId)
                    StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, "JointPhoto", JointPhoto.FileId)


                Else
                    'Flag Validation - Aspect not Set
                    lblTest.Text = "* Error - Incorect Photo aspect. Please crop your photo, to the correct size - by dragging a rectangle on the image"
                    lblTest.Visible = True
                    Return
                End If
            End If
        End Sub


        Protected Sub btnProfile_Click(sender As Object, e As System.EventArgs) Handles btnProfile.Click
            Try
                staff = StaffBrokerFunctions.GetStaffMember(UserId)
                If Not staff Is Nothing Then
                    User1 = UserController.GetUserById(PortalId, staff.UserId1)
                    If staff.UserId2 > 0 Then
                        User2 = UserController.GetUserById(PortalId, staff.UserId2)
                    End If

                    StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, "GivingText", tbGivingText.Text)

                    If profileImage1.FileId > 0 Then
                        If profileImage1.CheckAspect() Then
                            User1.Profile.SetProfileProperty("Photo", profileImage1.FileId)
                        Else
                            'Flag Validation - Aspect not Set
                            lblTest.Text = "* Error - Incorect Photo aspect. Please crop your photo, to the correct size - by dragging a rectangle on the image"
                            lblTest.Visible = True
                            Return
                        End If
                    End If
                    If JointPhoto.FileId > 0 Then
                        If JointPhoto.CheckAspect() Then
                            StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, "JointPhoto", JointPhoto.FileId)

                        Else
                            'Flag Validation - Aspect not Set
                            lblTest.Text = "* Error - Incorect Photo aspect. Please crop your photo, to the correct size - by dragging a rectangle on the image"
                            lblTest.Visible = True
                            Return
                        End If
                    End If


                    For Each row As DataListItem In dlProfileProps.Items
                        Dim PropName As String = CType(row.FindControl("hfPropName"), HiddenField).Value
                        Dim PropType As Integer = CType(row.FindControl("hfPropType"), HiddenField).Value
                        If PropType = 359 Then
                            Dim theText = CType(row.FindControl("tbDateValue1"), TextBox).Text
                            If theText <> "" Then
                                Dim B1 As Date = DateTime.Parse(CType(row.FindControl("tbDateValue1"), TextBox).Text, CultureInfo.CurrentCulture)
                                User1.Profile.SetProfileProperty(PropName, B1.ToString("dd/MM/yyyy"))
                            Else
                                User1.Profile.SetProfileProperty(PropName, theText)
                            End If


                        Else
                            User1.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue1"), TextBox).Text)
                        End If
                        If staff.UserId2 > 0 Then
                            If PropType = 359 Then
                                Dim theText2 = CType(row.FindControl("tbDateValue2"), TextBox).Text
                                If theText2 <> "" Then
                                    Dim B2 As Date = DateTime.Parse(theText2, CultureInfo.CurrentCulture)
                                    User2.Profile.SetProfileProperty(PropName, B2.ToString("dd/MM/yyyy"))
                                Else
                                    User2.Profile.SetProfileProperty(PropName, theText2)
                                End If


                            Else
                                User2.Profile.SetProfileProperty(PropName, CType(row.FindControl("tbPropValue2"), TextBox).Text)
                            End If
                        End If
                    Next

                    For Each row As DataListItem In dlGivingQuestions.Items

                        If CType(row.FindControl("tblStaffProfile"), System.Web.UI.HtmlControls.HtmlTable).Visible Then
                            Dim Type = CInt(CType(row.FindControl("hfPropType"), HiddenField).Value)
                            Select Case Type
                                Case 0
                                    StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, CType(row.FindControl("hfPropName"), HiddenField).Value, CType(row.FindControl("tbPropValueNumber"), TextBox).Text)
                                Case 1
                                    StaffBrokerFunctions.AddProfileValue(PortalId, staff.StaffId, CType(row.FindControl("hfPropName"), HiddenField).Value, CType(row.FindControl("cbProbValue"), CheckBox).Checked)

                            End Select



                        End If
                    Next




                    User1.Email = tbEmail1.Text

                    UserController.UpdateUser(PortalId, User1)
                    If staff.UserId2 > 0 Then

                        If profileImage1.FileId > 0 Then
                            If profileImage2.CheckAspect() Then
                                User2.Profile.SetProfileProperty("Photo", profileImage2.FileId)
                            Else
                                'Flag Validation - Aspect not Set
                                lblTest.Text = "* Error - Incorect Photo aspect. Please crop your photo, to the correct size - by dragging a rectangle on the image"
                                lblTest.Visible = True
                                Return
                            End If
                            User2.Email = tbEmail2.Text
                            UserController.UpdateUser(PortalId, User2)
                        End If
                    End If
                    LoadStaff()
                    dlStaffProfile.DataBind()
                    dlGivingQuestions.DataBind()

                End If
            Catch ex As Exception
                lblTest.Text = ex.Message
                lblTest.Visible = True
            End Try
            Dim shortcut = StaffBrokerFunctions.GetStaffProfileProperty(staff.StaffId, "GivingShortcut")
            Dim mc As New DotNetNuke.Entities.Modules.ModuleController
            Dim x = mc.GetModuleByDefinition(PortalId, "frPresentationPage")
            If Not x Is Nothing Then
                If Not x.TabID = Nothing Then
                    Response.Redirect(NavigateURL(x.TabID) & "?giveto=" & shortcut)
                End If
            End If
        End Sub


    End Class
End Namespace
