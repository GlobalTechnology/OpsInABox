Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker
Imports StaffBrokerFunctions
Imports Stories
Imports DotNetNuke.Services.FileSystem
Namespace DotNetNuke.Modules.AgapeConnect.Stories
    Partial Class ContentRotator
        Inherits Entities.Modules.PortalModuleBase

        Dim d As New StoriesDataContext

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            Dim addTitle = MyBase.Actions.Add(GetNextActionID, "AgapeConnect", "AgapeConnect", "", "", "", "", True, SecurityAccessLevel.Edit, True, False)
            addTitle.Actions.Add(GetNextActionID, "Story Settings", "StorySettings", "", "action_settings.gif", EditUrl("StorySettings"), False, SecurityAccessLevel.Edit, True, False)
            addTitle.Actions.Add(GetNextActionID, "New Story", "NewStory", "", "add.gif", EditUrl("AddEditStory"), False, SecurityAccessLevel.Edit, True, False)

        End Sub


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request.QueryString("StoryId") <> "" Then
                Response.Redirect(EditUrl("ViewStory") & "?StoryId=" & Request.QueryString("StoryId"))
            End If
            If Not Page.IsPostBack Then


                Dim q = From c In d.AP_Stories Where c.PortalID = PortalId And c.IsVisible = True
                Dim out As String = ""
                If Settings("NumberOfStories") <> "" Then
                    q = q.Take(CInt(Settings("NumberOfStories")))
                End If
                For Each row In q
                    Try
                        Dim PhotoURL = FileManager.Instance.GetUrl(FileManager.Instance.GetFile(row.PhotoId))
                        out &= "<a href=""" & EditUrl("ViewStory") & "?StoryId=" & row.StoryId & """>"
                        out &= "<img src=""" & PhotoURL & """ data-thumb=""" & PhotoURL & """ alt=""" & EditUrl("ViewStory") & "?StoryId=" & row.StoryId & """  title=""" & row.Headline & """ /></a>"

                    Catch ex As Exception

                    End Try
                  
                Next


                ltStories.Text = out
            End If
        End Sub



        
    End Class
End Namespace
