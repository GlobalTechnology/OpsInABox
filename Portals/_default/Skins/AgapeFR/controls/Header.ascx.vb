Partial Class Portals__default_Skins_AgapeFR_controls_Header

    Inherits System.Web.UI.UserControl
    Protected Overrides Sub OnLoad(ByVal e As EventArgs)


        Dim myUrl = HttpContext.Current.Request.Url
        AgapeLogger.Error(1, UserController.Instance.GetCurrentUserInfo().UserID)

        If UserController.Instance.GetCurrentUserInfo().UserID > 0 Then
            userConnected.Visible = True
            userIcon.Attributes.Add("class", "usericon")
            userIconLink.Attributes.Add("href", "https://cruwp-stage.agapefrance.fr/dons/compte/")
            userIconLink.Attributes.Add("title", Translate("Profile"))
        Else
            userConnected.Visible = False
            userIcon.Attributes.Add("class", "usericon login")
            userIconLink.Attributes.Add("href", HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Authority & "/caslogin?service=" & TabController.CurrentPage.FullUrl.ToString)
            ' userIconLink.Attributes.Add("href", "https://thekey.me/cas/login?service=" & TabController.CurrentPage.FullUrl.ToString)
            userIconLink.Attributes.Add("title", Translate("Connect"))

        End If

    End Sub


    Protected Function Translate(ResourceKey As String) As String
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim strFile As String = System.IO.Path.GetFileName(Server.MapPath(PS.ActiveTab.SkinSrc))
        strFile = PS.ActiveTab.SkinPath + Localization.LocalResourceDirectory + "/" + strFile
        Return Localization.GetString(ResourceKey, strFile)
    End Function
End Class
