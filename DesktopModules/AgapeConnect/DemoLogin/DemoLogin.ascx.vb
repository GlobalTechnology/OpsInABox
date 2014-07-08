Imports System.IO
Imports System.Xml
Imports System.Net
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Services.Authentication
Imports System.Linq

'Imports Resources


Namespace DotNetNuke.Modules.AgapePortal
    Partial Class DemoLogin
        Inherits Entities.Modules.PortalModuleBase
        Dim CASHOST As String = "https://thekey.me/cas/"
        Dim Service As String = ""


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Select Case UserInfo.Username
                Case "staff.member@agapeconnect.me" & PortalId
                    lblUserName.Text = UserInfo.DisplayName
                    lblTask.Text = "submit"
                    pnlDemo.Visible = True
                    pnlNotKnown.Visible = False
                Case "team.leader@agapeconnect.me" & PortalId
                    lblUserName.Text = UserInfo.DisplayName
                    lblTask.Text = "approve"
                    pnlDemo.Visible = True
                    pnlNotKnown.Visible = False
                Case "accounts.team@agapeconnect.me" & PortalId
                    lblUserName.Text = UserInfo.DisplayName
                    lblTask.Text = "process"
                    pnlDemo.Visible = True
                    pnlNotKnown.Visible = False
                Case Else
                    pnlDemo.Visible = False
                    pnlNotKnown.Visible = True

            End Select
        End Sub

        'Public Sub StaffLogin()

        'End Sub




        'Private Sub LoginUser(ByVal objUserInfo As UserInfo, ByVal PS As PortalSettings, ByVal returnURL As String)
        '    If returnURL Is Nothing Then
        '        FormsAuthentication.RedirectFromLoginPage(objUserInfo.Username, False)
        '    Else

        '        UserController.UserLogin(PortalSettings.PortalId, objUserInfo, PortalSettings.PortalName, AuthenticationLoginBase.GetIPAddress(), False)


        '        Response.Redirect(Server.HtmlDecode(returnURL))

        '    End If

        'End Sub



        'Private Function GetUIDFromGUID(ByVal ssoGUID As String) As Integer


        '    Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        '    Dim totalRecords As New Integer
        '    Dim q = UserController.GetUsersByProfileProperty(PS.PortalId, "ssoGUID", ssoGUID, 1, 1000, totalRecords)

        '    If q.Count > 0 Then
        '        Return CType(q(0), UserInfo).UserID
        '    Else
        '        Return -1
        '    End If

        'End Function


        'Private Sub SetProfileProperty(ByVal PropertyName As String, ByRef theUser As UserInfo, ByVal Value As String)
        '    Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)

        '    If DotNetNuke.Entities.Profile.ProfileController.GetPropertyDefinitionByName(PS.PortalId, PropertyName) Is Nothing Then
        '        Dim insert As New DotNetNuke.Entities.Profile.ProfilePropertyDefinition()

        '        insert.ModuleDefId = -1
        '        insert.Deleted = False
        '        insert.DataType = 349
        '        insert.DefaultValue = ""
        '        insert.PropertyCategory = "Authentication"
        '        insert.PropertyName = PropertyName
        '        insert.Length = 50
        '        insert.Required = False
        '        insert.ViewOrder = 100
        '        insert.PortalId = PS.PortalId
        '        insert.DefaultVisibility = 0


        '        DotNetNuke.Entities.Profile.ProfileController.AddPropertyDefinition(insert)

        '        theUser.Profile.ProfileProperties.Add(insert)

        '    End If


        '    ' theUser.Profile.InitialiseProfile(PS.PortalId)



        '    theUser.Profile.SetProfileProperty(PropertyName, Value)


        '    '  UserController.UpdateUser(PS.PortalId, theUser)
        'End Sub

        Protected Sub btnStaff_Click(sender As Object, e As System.EventArgs) Handles btnStaff.Click
            Dim tuPass = StaffBrokerFunctions.GetSetting("TestUserPassword", 0)


            Dim objKey As New KeyUser.KeyAuthentication("staff.member@agapeconnect.me", tuPass)
            Dim objUserInfo = UserController.GetUserByName(PortalId, "staff.member@agapeconnect.me" & PortalId)
            If Not objKey.KeyGuid Is Nothing Then
                If objKey.KeyGuid <> "" Then
                    StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "ssoGUID", objKey.KeyGuid)
                End If
            End If
            If Not objKey.ProxyGrantingTicketIOU Is Nothing Then
                If objKey.ProxyGrantingTicketIOU <> "" Then
                    StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "GCXPGTIOU", objKey.ProxyGrantingTicketIOU)
                End If
            End If


            Session("pgtId") = Nothing

            UserController.UserLogin(PortalId, objUserInfo, PortalSettings.PortalName, AuthenticationLoginBase.GetIPAddress(), True)
            Dim cookie = New HttpCookie("portalroles")
            cookie.Expires = Now.AddYears(-2)
            Response.Cookies.Add(cookie)


            Response.Redirect(NavigateURL)
            'FormsAuthentication.RedirectFromLoginPage("staff.member@agapeconnect.me0", False)
        End Sub



        Protected Sub btnLeader_Click(sender As Object, e As System.EventArgs) Handles btnLeader.Click
            Dim tuPass = StaffBrokerFunctions.GetSetting("TestUserPassword", 0)
            Dim objKey As New KeyUser.KeyAuthentication("team.leader@agapeconnect.me", tuPass)
            Dim objUserInfo = UserController.GetUserByName(PortalId, "team.leader@agapeconnect.me" & PortalId)
            If Not objKey.KeyGuid Is Nothing Then
                If objKey.KeyGuid <> "" Then
                    StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "ssoGUID", objKey.KeyGuid)
                End If
            End If
            If Not objKey.ProxyGrantingTicketIOU Is Nothing Then
                If objKey.ProxyGrantingTicketIOU <> "" Then
                    StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "GCXPGTIOU", objKey.ProxyGrantingTicketIOU)
                End If
            End If
            Session("pgtId") = Nothing
            UserController.UserLogin(PortalId, objUserInfo, PortalSettings.PortalName, AuthenticationLoginBase.GetIPAddress(), True)
            Dim cookie = New HttpCookie("portalroles")
            cookie.Expires = Now.AddYears(-2)
            Response.Cookies.Add(cookie)


            Response.Redirect(NavigateURL)
        End Sub

        Protected Sub btnAccounts_Click(sender As Object, e As System.EventArgs) Handles btnAccounts.Click
            Dim tuPass = StaffBrokerFunctions.GetSetting("TestUserPassword", 0)
            Dim objKey As New KeyUser.KeyAuthentication("accounts.team@agapeconnect.me", tuPass)
            Dim objUserInfo = UserController.GetUserByName(PortalId, "accounts.team@agapeconnect.me" & PortalId)
            If Not objKey.KeyGuid Is Nothing Then
                If objKey.KeyGuid <> "" Then
                    StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "ssoGUID", objKey.KeyGuid)
                End If
            End If
            If Not objKey.ProxyGrantingTicketIOU Is Nothing Then
                If objKey.ProxyGrantingTicketIOU <> "" Then
                    StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "GCXPGTIOU", objKey.ProxyGrantingTicketIOU)
                End If
            End If
            Session("pgtId") = Nothing
            UserController.UserLogin(PortalId, objUserInfo, PortalSettings.PortalName, AuthenticationLoginBase.GetIPAddress(), True)
            Dim cookie = New HttpCookie("portalroles")
            cookie.Expires = Now.AddYears(-2)
            Response.Cookies.Add(cookie)


            Response.Redirect(NavigateURL)

        End Sub
    End Class
End Namespace
