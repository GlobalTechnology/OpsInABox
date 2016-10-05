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
            If (Not IsEditable And Not UserInfo.IsInRole("Administrators")) Then
                Response.Redirect(NavigateURL(PortalSettings.Current.ErrorPage404))
            End If
            If Not Page.IsPostBack Then
                BuildUnpublishedList()
            End If
        End Sub

#Region "HelperFunctions"

        Protected Sub BuildUnpublishedList()
            gvPublish.Columns(0).HeaderText = Translate("StoryDate")
            gvPublish.Columns(1).HeaderText = Translate("Headline")
            gvPublish.Columns(2).HeaderText = Translate("Author")
            gvPublish.DataSource = StoryFunctions.GetUnpublishedStories(TabModuleId)
            gvPublish.DataBind()
        End Sub

        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
        End Function
#End Region 'HelperFunctions

#Region "PageEvents"
        Protected Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub gvPublish_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPublish.RowCommand
            If e.CommandName = "Publish" Then
                Dim commandArgs() As String = e.CommandArgument.ToString.Split(",")

                If StoryFunctions.PublishStory(CInt(commandArgs(0))) Then
                    BuildUnpublishedList()
                    PublishValidator.Visible = False
                Else
                    PublishValidator.Text = LocalizeString("NoPhoto") & commandArgs(1)
                    PublishValidator.Visible = True
                End If
            End If
        End Sub
#End Region 'PageEvents

    End Class
End Namespace