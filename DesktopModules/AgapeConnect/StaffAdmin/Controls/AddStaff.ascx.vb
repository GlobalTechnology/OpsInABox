Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker



Partial Class DesktopModules_AgapePortal_StaffBroker_AddStaff
    'Inherits Entities.Modules.PortalModuleBase
    Inherits System.Web.UI.UserControl
    Private _PortalId As Integer
    Public Property PortalId() As Integer
        Get
            Return _PortalId
        End Get
        Set(ByVal value As Integer)
            _PortalId = value

            hfPortalId.Value = _PortalId
        End Set
    End Property
   
    Public Function GetClientId(ByVal Id As String) As String
        Return FindControl(Id).ClientID
    End Function


    Protected Sub UpdatePanel1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdatePanel1.Load
        Dim t1 As Type = Button1.GetType()
        Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb1.Append("<script language='javascript'>")
        sb1.Append("MaritalChange();Create1();Create2();")
        sb1.Append("</script>")
        ScriptManager.RegisterStartupScript(Button1, t1, "startUp", sb1.ToString, False)

    End Sub
    Public Function GetDateFormat() As String
        Dim sdp As String = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower
        If sdp.IndexOf("d") < sdp.IndexOf("m") Then
            Return "dd/mm/yy"
        Else
            Return "mm/dd/yy"
        End If
    End Function
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim valStatus As Boolean = True
        lblError.Text = ""
        If tbGCXUserName.Text = "" Then
            lblError.Text &= "Please enter the User's GCX Email Address"
        End If
        If cbCreate1.Checked Then
            If tbFirstName1.Text = "" Or tbLastName1.Text = "" Or tbFirstName1.Text = "First name" Or tbLastName1.Text = "Last name" Then
                lblError.Text &= "Please enter a name for User"
            End If
        End If
        If ddlMaritalStatus.SelectedValue = 0 And tbSpouseGCX.Text = "" Then
            lblError.Text &= "Please enter the Spouse's GCX Email Address"
        End If
        If cbCreate2.Checked And ddlMaritalStatus.SelectedValue = 0 Then
            If tbFirstName2.Text = "" Or tbLastName2.Text = "" Or tbFirstName2.Text = "First name" Or tbLastName2.Text = "Last name" Then
                lblError.Text &= "Please enter a name for Spouse"
            End If
        End If

        If ddlMaritalStatus.SelectedValue = -1 And tbSpouseName.Text = "" Then
            lblError.Text &= "Please enter a name for Spouse"
        End If

        If lblError.Text <> "" Then
            Return
        End If


        Dim user As UserInfo = UserController.GetUserByName(_PortalId, tbGCXUserName.Text & _PortalId)
        Dim spouse As UserInfo = UserController.GetUserByName(_PortalId, tbSpouseGCX.Text & _PortalId)
       
        If user Is Nothing And cbCreate1.Checked = False Then
            lblError.Text = "User <i>" & tbGCXUserName.Text & "</i> does not exist. Check the 'Create?' box to automatically register this account on this site.<br />"
            valStatus = False

        End If
        If ddlMaritalStatus.SelectedValue > 0 Then

            If spouse Is Nothing And cbCreate2.Checked = False Then
                lblError.Text &= "User <i>" & tbSpouseGCX.Text & "</i> does not exist. Check the 'Create?' box to automatically register this account on this site.<br />"
                valStatus = False

            End If
        End If
        If valStatus = False Then
            Return
        End If
        If user Is Nothing And cbCreate1.Checked Then
            user = StaffBrokerFunctions.CreateUser(_PortalId, tbGCXUserName.Text, tbFirstName1.Text, tbLastName1.Text)
        End If
        If spouse Is Nothing And cbCreate2.Checked Then
            spouse = StaffBrokerFunctions.CreateUser(_PortalId, tbSpouseGCX.Text, tbFirstName2.Text, tbLastName2.Text)
        End If
        Dim stff As New AP_StaffBroker_Staff
        If ddlMaritalStatus.SelectedValue = 0 Then
            stff = StaffBrokerFunctions.CreateStaffMember(_PortalId, user, User2in:=spouse, staffTypeIn:=ddlStaffType.SelectedValue)
            StaffBrokerFunctions.AddProfileValue(_PortalId, stff.StaffId, "Single", "False")
        ElseIf ddlMaritalStatus.SelectedValue = -1 Then
            stff = StaffBrokerFunctions.CreateStaffMember(_PortalId, user, tbSpouseName.Text, dtSpouseDOB.Text, staffTypeIn:=ddlStaffType.SelectedValue)
            StaffBrokerFunctions.AddProfileValue(_PortalId, stff.StaffId, "Single", "True")
        Else
            stff = StaffBrokerFunctions.CreateStaffMember(_PortalId, user, staffTypeIn:=ddlStaffType.SelectedValue)
            StaffBrokerFunctions.AddProfileValue(_PortalId, stff.StaffId, "Single", "True")
        End If

        StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", _PortalId)

        Response.Redirect(NavigateURL())
    End Sub


   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            hfPortalId.Value = _PortalId
        End If
    End Sub
End Class
