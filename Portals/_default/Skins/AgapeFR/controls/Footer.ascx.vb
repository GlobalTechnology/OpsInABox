
Partial Class Portals__default_Skins_AgapeFR_controls_Footer
    Inherits System.Web.UI.UserControl
    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        fr_popup.Visible = True

    End Sub


    Protected Function Translate(ResourceKey As String) As String
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim strFile As String = System.IO.Path.GetFileName(Server.MapPath(PS.ActiveTab.SkinSrc))
        strFile = PS.ActiveTab.SkinPath + Localization.LocalResourceDirectory + "/" + strFile
        Return Localization.GetString(ResourceKey, strFile)
    End Function
End Class
