Partial Class Portals__default_Skins_AgapeFR_controls_Header

    Inherits System.Web.UI.UserControl
    Protected Overrides Sub OnLoad(ByVal e As EventArgs)

        Dim userInfo = UserController.GetCurrentUserInfo()
        If userInfo.UserID > 0 Then 'User is connected
            userMenu.Attributes.Add("class", "parent")
            userIcon.Attributes.Add("class", "usericon")
            userIconLink.Attributes.Add("href", "https://dons.agapefrance.org/compte/")
            userIconLink.Attributes.Add("title", Translate("Profile"))
            lblConnectText.Text=UserController.Instance.GetCurrentUserInfo().DisplayName
            lblAccountText.Text=Translate("myAccount")
            lblmyDonPage.Text=Translate("myDonPage")
            If (userInfo.IsInRole("StaffDons")) Then
                lnkEditDonPage.visible=True
            Else
                lnkEditDonPage.visible=False
            End If
        Else 'User is not connected
            userIcon.Attributes.Add("class", "usericon login")
            Dim connectionLink as String=""
            If Request.QueryString("StoryID") <> "" Then
                connectionLink=HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Authority & "/caslogin?returnurl=" & TabController.CurrentPage.FullUrl.ToString & "?StoryId=" & Request.QueryString("StoryID")
            Else
                connectionLink=HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Authority & "/caslogin?returnurl=" & TabController.CurrentPage.FullUrl.ToString
            End If
            userIconLink.Attributes.Add("title", Translate("Connect"))
            userIconLink.Attributes.Add("href", connectionLink)
            lblConnectText.Text=Translate("Login")
            userConnectLink.Attributes.Add("href",connectionLink)
        End If
    End Sub


    Protected Function Translate(ResourceKey As String) As String
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim strFile As String = System.IO.Path.GetFileName(Server.MapPath(PS.ActiveTab.SkinSrc))
        strFile = PS.ActiveTab.SkinPath + Localization.LocalResourceDirectory + "/" + strFile
        Return Localization.GetString(ResourceKey, strFile)
    End Function
End Class
