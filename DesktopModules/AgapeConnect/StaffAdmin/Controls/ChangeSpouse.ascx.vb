Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker



Partial Class DesktopModules_AgapeConnect_StaffAdmin_Controls_ChangeSpouse
    'Inherits Entities.Modules.PortalModuleBase
    Inherits System.Web.UI.UserControl
    Private _PortalId As Integer
    Private _StaffId As Integer
    Public Property PortalId() As Integer
        Get
            Return _PortalId
        End Get
        Set(ByVal value As Integer)
            _PortalId = value

            hfPortalId.Value = _PortalId
        End Set
    End Property
    Public Property StaffId() As Integer
        Get
            Return _StaffId
        End Get
        Set(ByVal value As Integer)
            _StaffId = value

            hfOrigStaffId.Value = _StaffId
        End Set
    End Property
    Protected Sub UpdatePanel1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdatePanel1.Load
        lblNewError.Visible = False
        lblNewError.Text = ""
        Dim t1 As Type = btnNewSpouse.GetType()
        Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb1.Append("<script language='javascript'>")
        sb1.Append("OnStaffNot();")
        sb1.Append("</script>")
        ScriptManager.RegisterStartupScript(btnNewSpouse, t1, "startUp2", sb1.ToString, False)
    End Sub
    Public Function GetSpouseClientId(ByVal Id As String) As String
        Return FindControl(Id).ClientID
    End Function
    Protected Sub Page_Render(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        hfOrigStaffId.Value = _StaffId
        If Not Page.IsPostBack Then

            hfPortalId.Value = _PortalId
        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            
            Dim d As New StaffBrokerDataContext
            Dim UnMarriedStaffList = (From c In d.AP_StaffBroker_Staffs Where c.Active And c.PortalId = _PortalId And c.UserId2 = -2 And c.AP_StaffBroker_StaffType.Name <> "Office" Order By c.User.LastName Select DisplayName = c.User.LastName & ", " & c.User.FirstName, c.StaffId, c.UserId2, c.User2)
            Dim UnMarriedStaff As New Dictionary(Of String, Integer)
            For Each row In UnMarriedStaffList
                If row.UserId2 = -2 Then
                    UnMarriedStaff.Add(row.DisplayName, row.StaffId)
                End If
            Next
            ddlAllStaff.DataSource = UnMarriedStaff
            ddlAllStaff.DataTextField = "Key"
            ddlAllStaff.DataValueField = "Value"
            ddlAllStaff.DataBind()
        End If
    End Sub
    Protected Sub btnNewSpouse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSpouse.Click
        Dim valStatus As Boolean = True
        lblNewError.Text = ""
        Dim user As New UserInfo
        Dim d As New StaffBrokerDataContext
        Dim q = From c In d.AP_StaffBroker_Staffs Where c.StaffId = hfOrigStaffId.Value
        If q.Count > 0 Then
            If Not q.First.UserId2 = -2 Then
                lblNewError.Text = "You are trying to change the spouse of someone who has already got a spouse on the system."
                valStatus = False
            End If
        Else
            lblNewError.Text = "There was a problem finding the staff id of the person you are trying to add a spouse to."
            valStatus = False
        End If
        If ddlCurrent.SelectedValue = 0 Then
            user = UserController.GetUserByName(_PortalId, tbNewSpouseGCX.Text & _PortalId)
            If tbNewSpouseGCX.Text = "" Or tbNewSpouseGCX.Text = "GCX Username" Then
                valStatus = False
                lblNewError.Text = "Please enter the new staff member's GCX username."
            ElseIf tbNewFirstName.Text = "" Or tbNewFirstName.Text = "First name" Then
                valStatus = False
                lblNewError.Text = "Please enter the new staff member's first name."
            ElseIf tbNewLastName.Text = "" Or tbNewLastName.Text = "Last name" Then
                valStatus = False
                lblNewError.Text = "Please enter the new staff member's last name."
            End If
            If Not user Is Nothing Then
                valStatus = False
                lblNewError.Text = "This person already exists on our system."
            End If
        Else
            If ddlAllStaff.SelectedValue = q.First.StaffId Then
                lblNewError.Text = "Don't be stupid."
                valStatus = False
            End If
        End If
        If Not valStatus Then
            lblNewError.Visible = True
            Return
        End If

        Dim sbf As New StaffBrokerFunctions
        'Dim user As UserInfo = UserController.GetUserByName(_PortalId, tbNewSpouseGCX.Text & _PortalId)
        Try
        If ddlCurrent.SelectedValue = 0 Then

            'Create user, add them as staff and add to staff profile
            If user Is Nothing Then
                If q.Count > 0 Then
                    If q.First.UserId2 = -2 Then
                        user = StaffBrokerFunctions.CreateUser(_PortalId, tbNewSpouseGCX.Text, tbNewFirstName.Text, tbNewLastName.Text)
                        Dim rc As DotNetNuke.Security.Roles.RoleController = New DotNetNuke.Security.Roles.RoleController()
                        If rc.GetRoleByName(PortalId, "Staff") Is Nothing Then
                            Dim insert As DotNetNuke.Security.Roles.RoleInfo = New DotNetNuke.Security.Roles.RoleInfo()
                            insert.Description = "Staff Members"
                            insert.RoleName = "Staff"
                            insert.AutoAssignment = False
                            insert.IsPublic = False
                            insert.RoleGroupID = -1
                            rc.AddRole(insert)
                        End If
                        rc.AddUserRole(PortalId, user.UserID, rc.GetRoleByName(PortalId, "Staff").RoleID, DateTime.Now)
                            sbf.AddProfileValue(PortalId, q.First.StaffId, "Hours2", 40)
                            sbf.AddProfileValue(PortalId, q.First.StaffId, "Single", "False")
                            sbf.AddProfileValue(PortalId, q.First.StaffId, "PrimeUserId", q.First.UserId1)
                            sbf.AddProfileValue(PortalId, q.First.StaffId, "PrimeGetsEmails", "True")
                            sbf.AddProfileValue(PortalId, q.First.StaffId, "MaritalStatus", "Married")
                            q.First.UserId2 = user.UserID
                            d.SubmitChanges()
                    End If
                End If
            End If
            ElseIf ddlCurrent.SelectedValue = 1 Then
                'Add user to staff profile, move theif profile properties and disable their old staff profile
                If q.Count > 0 Then
                    If q.First.UserId2 = -2 Then
                        Dim r = From c In d.AP_StaffBroker_Staffs Where c.StaffId = ddlAllStaff.SelectedValue
                        If r.Count > 0 Then
                            If r.First.UserId1 > 0 And r.First.UserId2 = -2 Then
                                Dim s = From c In d.AP_StaffBroker_StaffProfiles Where c.StaffId = r.First.StaffId
                                If s.Count > 0 Then
                                    'Dim Hours = sbf.GetStaffProfileProperty(r.First.StaffId, "Hours1")
                                    Dim Hours = From c In s Where c.AP_StaffBroker_StaffPropertyDefinition.PropertyName = "Hours1" Select c.PropertyValue
                                    Dim Job = From c In s Where c.AP_StaffBroker_StaffPropertyDefinition.PropertyName = "JobTitle1" Select c.PropertyValue
                                    Dim nonCTC = From c In s Where c.AP_StaffBroker_StaffPropertyDefinition.PropertyName = "NonCTC" Select c.PropertyValue
                                    If Hours.Count > 0 Then
                                        sbf.AddProfileValue(PortalId, q.First.StaffId, "Hours2", Hours.First)
                                    End If
                                    If Job.Count > 0 Then
                                        sbf.AddProfileValue(PortalId, q.First.StaffId, "JobTitle2", Job.First)
                                    End If
                                    If nonCTC.Count > 0 Then
                                        Dim t = From c In d.AP_StaffBroker_StaffProfiles Where c.StaffId = q.First.StaffId
                                        Dim NewNon As Integer = 0
                                        Dim non = From c In t Where c.AP_StaffBroker_StaffPropertyDefinition.PropertyName = "NonCTC" Select c.PropertyValue
                                        If non.Count > 0 Then
                                            NewNon = CInt(non.First) + CInt(nonCTC.First)
                                        Else
                                            NewNon = CInt(nonCTC.First)
                                        End If
                                        sbf.AddProfileValue(PortalId, q.First.StaffId, "NonCTC", NewNon)
                                    End If
                                    sbf.AddProfileValue(PortalId, q.First.StaffId, "Single", "False")
                                    sbf.AddProfileValue(PortalId, q.First.StaffId, "PrimeUserId", q.First.UserId1)
                                    sbf.AddProfileValue(PortalId, q.First.StaffId, "PrimeGetsEmails", "True")
                                    sbf.AddProfileValue(PortalId, q.First.StaffId, "MaritalStatus", "Married")
                                End If
                                Dim u = From c In d.AP_StaffBroker_Childrens Where c.StaffId = r.First.StaffId
                                For Each child In u
                                    child.StaffId = q.First.StaffId
                                Next
                                r.First.Active = False
                                q.First.UserId2 = r.First.UserId1
                                d.SubmitChanges()
                            End If
                        End If

                    End If
                End If
        End If
        Catch ex As Exception
            lblNewError.Text = ex.Message
            lblNewError.Visible = True
            Return
        End Try
        Response.Redirect(NavigateURL())
    End Sub
End Class
