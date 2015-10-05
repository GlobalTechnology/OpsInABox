
Partial Class sso_GetLogo
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim logoFile = DotNetNuke.Common.GetPortalSettings.HomeDirectory & DotNetNuke.Common.GetPortalSettings.LogoFile
        'Response.Write(DotNetNuke.Services.FileSystem.FileManager.Instance.GetFileContent(DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(pi.LogoFile)))
        Dim original As System.Drawing.Image = Bitmap.FromFile(Server.MapPath(logoFile))

        Dim aspect = original.Width / original.Height

        original = original.GetThumbnailImage(300, 300 / aspect, Nothing, Nothing)
        original.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)

    End Sub
End Class
