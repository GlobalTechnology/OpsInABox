Imports System.IO
Imports System.Xml
Imports System.Net
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Services.Authentication
Imports System.Linq
Imports GCX
'Imports Resources
Imports StaffBroker


Namespace DotNetNuke.Modules.AgapePortal
    Partial Class CasAuth
        Inherits Entities.Modules.PortalModuleBase
        Private Const Cashost As String = "https://thekey.me/cas/"
        Dim _service As String = ""


        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            'Look for the "ticket=" after the "?" in the URL

            If Request.QueryString("mode") = "host" Then
                ' Service = TabController.CurrentPage.FullUrl
                ' Response.Write(Service)
                Return
            End If
            If Request.QueryString("pgtId") <> "" Then
                Dim d As New GCXDataContext

                Dim insert As New Agape_GCX_Proxy
                insert.PGTID = Request.QueryString("pgtId")
                insert.PGTIOU = Request.QueryString("pgtIou")
                insert.Created = Now
                d.Agape_GCX_Proxies.InsertOnSubmit(insert)

                d.SubmitChanges()
                Dim old = From c In d.Agape_GCX_Proxies Where c.Created < Now.AddHours(-6)

                d.Agape_GCX_Proxies.DeleteAllOnSubmit(old)
                d.SubmitChanges()
                Return
            End If


            Dim tkt As String = Request.QueryString("ticket")



            'Service = TabController.CurrentPage.FullUrl
            _service = NavigateURL(PortalSettings.LoginTabId)
            ' Dim template = Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & "sso/template.css"

            ' First time through there is no ticket=, so redirect to CAS login
            If (tkt Is Nothing) Then
                Dim returnUrl As String = Request.QueryString("returnurl")
                If returnUrl Is Nothing Or returnUrl = "" Then

                    Session("returnurl") = Nothing
                    '  ElseIf Request.RawUrl.Contains(Server.HtmlDecode(returnUrl)) Then

                    'Session("returnurl") = Nothing

                Else
                    Session("returnurl") = returnUrl
                End If

                If PortalSettings.DefaultPortalSkin.ToLower.Contains("agape") Then


                    If Request.QueryString("renew") = "true" Or PortalId = 5 Then

                        Dim template = "http://" & Request.Url.Authority & Request.ApplicationPath & "sso/template-agapebluev4-no-FB.css"
                        'Service &= "&renew=true&template=" & template
                        _service &= "&template=" & template

                    Else
                        Dim template = "http://" & Request.Url.Authority & Request.ApplicationPath & "sso/template-agapebluev4.css"
                        _service &= "&template=" & template
                    End If
                Else
                    ' Dim template = "http://" & Request.Url.Authority & Request.ApplicationPath & "sso/template-agapebluev3.css"
                    'Service &= "&template=" & template
                End If


                ' Response.Redirect("https://thekey.me/cas/login?service=" & Service & "&template=https://www.agape.org.uk/sso/template2.css")

                Response.Redirect("https://thekey.me/cas/login?service=" & _service)


            Else
                StaffLogin()
            End If




            '   Response.Redirect("https://signin.mygcx.org/cas/login?service=" & Request.Url.GetLeftPart(UriPartial.Path) & "&template=https://www.agape.org.uk/sso/template2.css")

        End Sub

        Private Sub StaffLogin()
            Dim returnUrl As String = CType(Session("returnurl"), String)
            Dim ps As PortalSettings = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim tkt As String = Request.QueryString("ticket")
            ' Dim service As String = Request.Url.GetLeftPart(UriPartial.Path)
            ' service = "https://www.agape.org.uk"
            '  CASHOST = "https://173.45.237.49/cas"

            ' Second time (back from CAS) there is a ticket= to validate
            Dim validateurl As String = Cashost + "proxyValidate?" & "ticket=" & tkt & "&" & "service=" & _service & "&pgtUrl=https://agapeconnect.me/CasLogin.aspx"
            'Dim validateurl As String = Cashost + "proxyValidate?" & "ticket=" & tkt & "&" & "service=" & _service & "&pgtUrl=https://myagape.org.uk/PgtCallback.aspx"

            Dim reader1 As StreamReader = New StreamReader(New WebClient().OpenRead(validateurl))
            Dim doc As New XmlDocument()
            doc.Load(reader1)
            Dim namespaceMgr As New XmlNamespaceManager(doc.NameTable)
            namespaceMgr.AddNamespace("cas", "http://www.yale.edu/tp/cas")
            'Check for success
            Dim serviceResponse As XmlNode = doc.SelectSingleNode("/cas:serviceResponse/cas:authenticationFailure", namespaceMgr)
            If Not serviceResponse Is Nothing Then
                Response.Write("Error: " & serviceResponse.InnerText)
                Return
            End If

            Dim successNode As XmlNode = doc.SelectSingleNode("/cas:serviceResponse/cas:authenticationSuccess", namespaceMgr)

            If Not successNode Is Nothing Then 'User Is authenticated
                Dim netid As String = String.Empty

                Dim firstName As String = String.Empty
                Dim lastName As String = String.Empty
                Dim ssoGuid As String = String.Empty
                Dim pgtiou As String = String.Empty

                If Not successNode.SelectSingleNode("./cas:user", namespaceMgr) Is Nothing Then
                    netid = successNode.SelectSingleNode("./cas:user", namespaceMgr).InnerText
                End If
                If Not successNode.SelectSingleNode("./cas:attributes/firstName", namespaceMgr) Is Nothing Then
                    firstName = successNode.SelectSingleNode("./cas:attributes/firstName", namespaceMgr).InnerText
                End If
                If Not successNode.SelectSingleNode("./cas:attributes/lastName", namespaceMgr) Is Nothing Then
                    lastName = successNode.SelectSingleNode("./cas:attributes/lastName", namespaceMgr).InnerText
                End If
                If Not successNode.SelectSingleNode("./cas:attributes/ssoGuid", namespaceMgr) Is Nothing Then
                    ssoGuid = successNode.SelectSingleNode("./cas:attributes/ssoGuid", namespaceMgr).InnerText
                End If
                If Not successNode.SelectSingleNode("./cas:proxyGrantingTicket", namespaceMgr) Is Nothing Then

                    pgtiou = successNode.SelectSingleNode("./cas:proxyGrantingTicket", namespaceMgr).InnerText

                End If



                ' If there was a problem, leave the message on the screen. Otherwise, return to original page.
                If (netid = String.Empty) Then

                    Response.Write("There was an error during login.")
                Else
                    'If netid = "jon@vellacott.co.uk" Then

                    '    Response.Write(Server.HtmlEncode(doc.OuterXml))

                    '    Response.Write("PGTIOU" & Session("PGTIOU"))
                    '    Return
                    'End If
                    Dim email = netid


                    netid = netid & ps.PortalId



                    'For the public portal, we need to check if they are already registered.

                    Dim objUserCreateStatus As UserCreateStatus

                    Dim objUserInfo As UserInfo
                    'See if user exists in DNN Portal user DB
                    objUserInfo = UserController.GetUserByName(CType(HttpContext.Current.Items("PortalSettings"), PortalSettings).PortalId, netid)

                    'user doesn't exist - try to create on the fly
                    If (objUserInfo Is Nothing) Then
                        'User doesn't exists. Lets try looking up the GUID, to see if they have just changed their login email.
                        If ssoGuid <> String.Empty Then
                            Dim uid As Integer = GetUIDFromGUID(ssoGuid, netid)
                            If uid > 0 Then
                                objUserInfo = UserController.GetUserById(ps.PortalId, uid)
                                If (Not objUserInfo Is Nothing) Then

                                    LoginUser(objUserInfo, returnUrl)
                                    'FormsAuthentication.RedirectFromLoginPage(netid, False) 'set netid in ASP.NET blocks

                                End If
                            End If

                        End If

                        'Create the New User
                        objUserInfo = New UserInfo()


                        objUserInfo.FirstName = firstName
                        objUserInfo.LastName = lastName
                        objUserInfo.DisplayName = firstName & " " & lastName
                        objUserInfo.Username = netid
                        objUserInfo.PortalID = ps.PortalId
                        objUserInfo.Membership.Password = UserController.GeneratePassword(8)
                        objUserInfo.Email = email
                        objUserCreateStatus = UserController.CreateUser(objUserInfo)
                        If objUserCreateStatus <> UserCreateStatus.Success Then
                            Response.Write("Error creating Agape Account:- " & objUserCreateStatus.ToString)

                        Else
                            If ssoGuid <> String.Empty Then
                                StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "ssoGUID", ssoGuid)
                            End If
                            If pgtiou <> String.Empty Then
                                StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "GCXPGTIOU", pgtiou)
                            End If

                            LoginUser(objUserInfo, returnUrl)
                            'FormsAuthentication.RedirectFromLoginPage(netid, False) 'set netid in ASP.NET blocks

                        End If


                    Else

                        If ssoGuid <> String.Empty Then
                            StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "ssoGUID", ssoGuid)
                        End If
                        If pgtiou <> String.Empty Then
                            StaffBrokerFunctions.SetUserProfileProperty(PortalId, objUserInfo.UserID, "GCXPGTIOU", pgtiou)
                        End If


                        LoginUser(objUserInfo, returnUrl)
                        'FormsAuthentication.RedirectFromLoginPage(netid, False) 'set netid in ASP.NET blocks


                    End If
                End If
            End If
        End Sub

        Private Sub LoginUser(ByVal objUserInfo As UserInfo, ByVal returnUrl As String)




            If returnURL Is Nothing Then
                FormsAuthentication.RedirectFromLoginPage(objUserInfo.Username, False)
            Else
                UserController.UserLogin(PortalSettings.PortalId, objUserInfo, PortalSettings.PortalName, AuthenticationLoginBase.GetIPAddress(), False)


                Response.Redirect(Server.HtmlDecode(returnURL))

            End If

        End Sub

        'Private Function GetStaffProfileProperty(ByVal PropertyName As String, ByVal UserId As Integer) As String



        '    Dim answer = From c In d.UserProfiles Where c.UserID = UserId And c.ProfilePropertyDefinition.PropertyName = PropertyName And c.ProfilePropertyDefinition.PortalID = 0 Select c.PropertyValue
        '    If answer.Count > 0 Then
        '        Return answer.First
        '    Else
        '        Return String.Empty
        '    End If

        'End Function

        Private Function GetUidFromGuid(ByVal ssoGuid As String, ByVal newUsername As String) As Integer


            Dim ps = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            ' Dim q = UserController.GetUsersByProfileProperty(PS.PortalId, "ssoGUID", ssoGUID, -1, 0, 0)
            Dim d As New StaffBrokerDataContext
            Dim q = From c In d.UserProfiles Where c.ProfilePropertyDefinition.PropertyName = "ssoGUID" And c.PropertyValue = ssoGuid And c.ProfilePropertyDefinition.PortalID = ps.PortalId
            If q.Count > 0 Then
                StaffBrokerFunctions.ChangeUsername(q.First.User.Username, newUsername)
                Return q.First.UserID
            Else
                Return -1
            End If

        End Function
    End Class
End Namespace
