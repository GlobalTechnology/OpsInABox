Imports DotNetNuke
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Math
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Collections.Specialized
Imports System.Linq
Imports Stories

Namespace DotNetNuke.Modules.Stories


    Partial Class Unpublished
        Inherits Entities.Modules.PortalModuleBase

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            hfPortalId.Value = PortalId




        End Sub


        Protected Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
            If e.CommandName = "Publish" Then
                StoryFunctions.PublishStory(CInt(e.CommandArgument))
                GridView1.DataBind()
            End If
        End Sub
    End Class
End Namespace