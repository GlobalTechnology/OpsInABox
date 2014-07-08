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

Imports GMA
Namespace DotNetNuke.Modules.GMA
    Partial Class GMADirectory
        Inherits Entities.Modules.PortalModuleBase
        Public isAdmin As Boolean = False
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            isAdmin = UserInfo.IsSuperUser Or UserInfo.Roles.Contains("Administrators")


        End Sub

      
        Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
            Dim gmaLocation As Uri


            Try
                gmaLocation = New Uri(tbURL.Text, UriKind.Absolute)

            Catch ex As Exception
                lblValidation.Text = "Invalid URL! Don't forget into start will http:// or https://"
                Return
            End Try


            Dim d As New gmaDataContext()
            Dim q = From c In d.gma_Servers Where c.displayName.ToLower() = tbDisplayName.Text.ToLower() Or gmaLocation.AbsoluteUri.ToLower = c.serviceURL.ToLower
            If q.Count > 0 Then
                lblValidation.Text = "This server already exists in our directory. It can only be changed by an administrator (Please contact support@agapeconnect.me)."
                Return
            End If
            If gma_global_directory.AddGMAService(tbDisplayName.Text, gmaLocation, UserId) Then
                lblValidation.Text = "The GMA Server has been successfully added"
                tbURL.Text = ""
                tbDisplayName.Text = ""
                GridView1.DataBind()
            Else
                lblValidation.Text = "There was a problem adding your GMA server. Please check the URL and try again. If the problem persists, please contact support@agapeconnect.me. "
            End If




        End Sub
       

    End Class
End Namespace
