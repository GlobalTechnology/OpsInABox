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
            If Not Page.IsPostBack Then
                hfTabModuleID.Value = TabModuleId
                BuildUnpublishedList()
            End If


        End Sub

        Protected Sub BuildUnpublishedList()
            gvPublish.DataSource = StoryFunctions.GetUnpublishedStories(TabModuleId)
            gvPublish.DataBind()
        End Sub

        Protected Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub gvPublish_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPublish.RowCommand
            If e.CommandName = "Publish" Then
                Dim commandArgs() As String = e.CommandArgument.ToString.Split(",")

                If StoryFunctions.PublishStory(CInt(commandArgs(0))) Then
                    gvPublish.DataBind()
                    PublishValidator.Visible = False
                Else
                    PublishValidator.Text = LocalizeString("NoPhoto") & commandArgs(1)
                    PublishValidator.Visible = True
                End If
            End If
        End Sub
    End Class
End Namespace