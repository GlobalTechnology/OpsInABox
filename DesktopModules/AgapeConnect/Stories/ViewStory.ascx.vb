'===============================================================
'ViewStory.ascx.vb
'---------------------------------------------------------------
'Purpose :  Builds the fields that are used in the Story 
'           templates for showing the related/associated Stories.
'===============================================================
Imports System.IO
Imports Stories

Namespace DotNetNuke.Modules.FullStory


    Partial Class ViewFullStory
        Inherits Entities.Modules.PortalModuleBase
        ' Implements Entities.Modules.IActionable

#Region "Constants"

        'Translation for France migration to OIB (Old TabIds -key- > New Tab Ids -Value-, likewise for modules)
        Private tabTranslation As New Dictionary(Of String, String) From {{"190", "2341"}, {"55", "2335"}, {"200", "2351"}}
        Private modTranslation As New Dictionary(Of String, String) From {{"419", "5548"}, {"434", "5524"}, {"507", "5571"}}

        Public location As String = ""
        Public zoomLevel As Integer = 4

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim storyIdString As String = Request.QueryString(ViewStoryConstants.STORYID)

            Dim story As AP_Story = StoryFunctions.GetStory(storyIdString)

            'Check if story isn't found or it isn't published 
            If (story.IsVisible Or IsEditable Or UserInfo.IsInRole("Administrators")) Then

                Dim thecache As AP_Stories_Module_Channel_Cache = StoryFunctions.GetStoryInCache(story.StoryId, story.TabModuleId)

                If (Not String.IsNullOrEmpty(thecache.Headline)) Then

                    'Boost/Block section
                    Dim RequestBoosted As String = Request.Form(BOOSTED)
                    If Not String.IsNullOrEmpty(RequestBoosted) Then
                        Dim requestIsBoosted As Boolean = CBool(RequestBoosted)
                        Dim requestIsBlocked As Boolean = CBool(Request.Form(BLOCKED))


                        If requestIsBlocked And Not thecache.Block Then

                            StoryFunctions.BlockStoryAccrossSite(thecache.Link)

                        ElseIf (Not requestIsBlocked) And thecache.Block Then
                            StoryFunctions.UnBlockStoryAccrossSite(thecache.Link)
                        End If

                            Dim changed As Boolean = False
                            Dim changedDate As New System.Nullable(Of Date)
                            If (Not requestIsBlocked) And requestIsBoosted Then
                                changed = True
                            If Not thecache.BoostDate Is Nothing Then
                                changedDate = Today.AddDays(StoryFunctions.GetBoostDuration(PortalId))
                            Else
                                changedDate = Today.AddDays(StoryFunctions.GetBoostDuration(PortalId))
                                End If

                            ElseIf (Not requestIsBlocked) And (Not requestIsBoosted) Then
                                changedDate = Nothing
                                changed = True
                            End If
                            If changed Then
                            StoryFunctions.SetBoostDate(thecache.GUID, changedDate, story.TabModuleId)
                            Dim theMod = StoryFunctions.GetStoryModule(TabModuleId)
                            StoryFunctions.RefreshFeed(story.TabModuleId, thecache.ChannelId, True)
                            StoryFunctions.PrecalAllCaches(story.TabModuleId)
                            End If

                        End If
                    'End Boost/Block section

                    Dim template As String = TemplateActions(story)

                    'Setup display of SuperPowers and template together
                    Dim superpowersIndex As Integer = template.IndexOf(SUPERPOWERS_TEMPLATE_KEY)
                    If (superpowersIndex < 0) Then
                        ltStory1.Text = template
                        ltStory2.Text = ""
                    Else
                        ltStory1.Text = template.Substring(0, superpowersIndex)
                        ltStory2.Text = template.Substring(superpowersIndex + SUPERPOWERS_TEMPLATE_KEY.Length)
                    End If

                    'If in edit mode
                    If IsEditable Then
                        SuperPowers.Visible = True
                        SuperPowers.CacheId = thecache.CacheId
                        SuperPowers.SuperEditor = UserInfo.IsSuperUser
                        SuperPowers.EditUrl = NavigateURL(CInt(GetTabId(Request.QueryString("origTabId"))), "AddEditStory", {"mid", GetModId(Request.QueryString("origModId"))})
                        SuperPowers.PortalId = PortalId
                        SuperPowers.SetControls()
                    End If
                Else
                    Response.Redirect(NavigateURL(PortalSettings.Current.ErrorPage404))
                End If
            Else
                    Response.Redirect(NavigateURL(PortalSettings.Current.ErrorPage404))
            End If
        End Sub

        Public Function GetLanguageName(ByVal language As String) As String

            Dim thename = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(Function(x) x.Name.ToLower = language.ToLower).Select(Function(x) x.EnglishName & " / " & x.NativeName)
            If thename.Count > 0 Then
                Return thename.First()
            Else
                Return ""
            End If
        End Function

        Public Function GetFlag(ByVal language As String) As String
            If String.IsNullOrEmpty(language) Then
                Return ""
            End If
            If language = "en" Then
                language = "en-GB"

            ElseIf language.Length = 2 Then
                language = language.ToLower & "-" & language.ToUpper

            End If


            Dim flagDir = New DirectoryInfo(Server.MapPath("/images/Flags/"))
            If Not flagDir Is Nothing Then

                Dim flags = flagDir.GetFiles().Where(Function(x) x.Name.ToLower.Contains(language.ToLower))

                If flags.Count = 0 Then
                    Return ""  ' couldn't find flag
                Else
                    Return "/images/Flags/" & flags.First.Name

                End If
            Else
                Return ""
            End If

        End Function

#Region "Templating functions"

        Protected Function TemplateActions(ByRef story As AP_Story) As String

            'Get template name from Story module advanced settings - "StoryView" is default template
            Dim template As String = StoryFunctions.GetTemplate(story.TabModuleId, TEMPLATE_SETTING, TEMPLATE_DEFAULT, PortalId)

            'Set template
            If StoryFunctions.IsStoryType(story, FRENCH_EVENT) Then
                Dim relatedAgenda As IQueryable(Of AP_Story) =
                        StoryFunctions.GetRelatedEventsForEvents(story.StoryId, story.TabModuleId, PortalId, NUM_OF_RELATED_AGENDA)
                SetTemplateFields(template, story, eventIcon, FRENCH_EVENT, FormatingRelatedAgenda(relatedAgenda))
                zoomLevel = 15
            Else
                Dim relatedAgenda As IQueryable(Of AP_Story) =
                        StoryFunctions.GetRelatedEventsForArticles(story.StoryId, story.TabModuleId, PortalId, NUM_OF_RELATED_AGENDA)
                SetTemplateFields(template, story, articleIcon, FRENCH_ARTICLE, FormatingRelatedAgenda(relatedAgenda))
            End If

            Return template
        End Function

        Protected Sub SetTemplateFields(ByRef template As String, ByVal story As AP_Story, ByRef icon As String,
                                        ByRef typeName As String, ByRef relatedAgendaStories As String)

            CType(Me.Page, DotNetNuke.Framework.CDefault).Title = story.Headline & " - " & PortalSettings.PortalName

            Dim URL = StoryFunctions.GetPhotoURL(story.PhotoId)
            Dim Fid = StaffBrokerFunctions.GetSetting("FacebookId", PortalSettings.PortalId)
            Dim permalink = NavigateURL(TabId, "", GetStoryURLParams(story.StoryId, Request.QueryString(ORIGINAL_MODULEID), Request.QueryString(ORIGINAL_TABID)))

            ReplaceField(template, "[HEADLINE]", story.Headline)
            ReplaceField(template, "[STORYTEXT]", story.StoryText)
            ReplaceField(template, "[MAP]", " <div id=""map_canvas""></div>")
            ReplaceField(template, "[IMAGEURL]", URL)
            ReplaceField(template, "[FACEBOOKID]", Fid)
            ReplaceField(template, "[AUTHOR]", story.Author)
            ReplaceField(template, "[DATE]", story.StoryDate.ToString("d MMMM yyyy"))
            ReplaceField(template, "[UPDATEDDATE]", "")
            ReplaceField(template, "[RSSURL]", StoryFunctionsProperties.StoriesModulePath & "/Feed.aspx?channel=" & story.TabModuleId)
            ReplaceField(template, "[SAMPLE]", story.TextSample)
            ReplaceField(template, "[SUBTITLE]", story.Subtitle)
            ReplaceField(template, "[FIELD1]", story.Field1)
            ReplaceField(template, "[FIELD2]", story.Field2)
            ReplaceField(template, "[TYPEICON]", icon)
            ReplaceField(template, "[TYPENAME]", typeName)
            ReplaceField(template, "[AGENDA]", relatedAgendaStories)
            ReplaceField(template, "[DATAHREF]", permalink)
            ReplaceField(template, "[LANGUAGES]", "")

            If ((story.Latitude IsNot Nothing) And (story.Longitude IsNot Nothing)) Then
                location = story.Latitude.Value.ToString(New CultureInfo("")) & ", " & story.Longitude.Value.ToString(New CultureInfo(""))
            End If

            'Only show updated date if the update was more than two weeks after the creation date
            If (Not IsNothing(story.UpdatedDate)) _
                AndAlso DateDiff(DateInterval.Day, story.StoryDate, story.UpdatedDate.Value) > 14 Then
                ReplaceField(template, "[UPDATEDDATE]", "(updated " & story.UpdatedDate.Value.ToString("d MMM yyyy") & ")")
            End If

            'ReplaceField logic for Field3
            If story.Field3.Contains("#selAuth#") Then
                Dim authID = story.Field3.Substring(9)
                Dim uc As New UserController()
                Dim auth = uc.GetUser(PortalSettings.PortalId, authID)
                Dim thisPhoto As String = ""
                Dim thisBio As String = ""
                Try
                    Dim FileID = auth.Profile.GetPropertyValue("Photo")
                    Dim _theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileID)
                    thisPhoto = DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(_theFile)
                    thisBio = auth.Profile.GetPropertyValue("Biography")
                Catch ex As Exception
                    thisPhoto = "/images/no_avatar.gif"
                    thisBio = "No Bio information"
                End Try
                ReplaceField(template, "[AUTHPHOTO]", thisPhoto)
                ReplaceField(template, "[AUTHBIO]", thisBio)
            Else
                ReplaceField(template, "[FIELD3]", story.Field3)
            End If

            'If Not story.TranslationGroup Is Nothing Then
            '    SuperPowers.TranslationGroupId = story.TranslationGroup
            '    Dim Translist = From c In d.AP_Stories Where c.TranslationGroup = r.TranslationGroup And c.PortalID = r.PortalID And c.StoryId <> r.StoryId Select c.Language, c.StoryId

            '    If Translist.Count > 0 Then
            '        Dim Flags As String = "<div style=""width: 100%;""><i>This story is also available in:</i> <div style=""margin: 4px 0 12px 0;"">"

            '        For Each row In Translist
            '            Dim Lang = GetLanguageName(row.Language)
            '            Flags &= "<a href=""" & NavigateURL() & "?StoryId=" & row.StoryId & "&origModId=" & GetModId(Request.QueryString("origModId")) & "&origTabId=" & GetTabId(Request.QueryString("origTabId")) & """ target=""_self""><span title=""" & Lang & """><img  src=""" & GetFlag(row.Language) & """ alt=""" & Lang & """  /></span></a>"
            '        Next

            '        Flags &= "</div> </div>"
            '        ReplaceField(template, "[LANGUAGES]", Flags)
            '    End If
            'End If

            ' Generate related Stories Sections
            Dim relatedStories As IQueryable(Of AP_Story) = StoryFunctions.GetRelatedStories(story.StoryId, story.TabModuleId,
                                                                                             PortalId, NUM_OF_RELATED_STORIES)
            ReplaceField(template, "[RELATEDSTORIES]", FormatingRelatedStories(relatedStories))

            Dim relatedArticles As IQueryable(Of AP_Story) = StoryFunctions.GetRelatedArticles(story.StoryId, story.TabModuleId,
                                                                                               PortalId, NUM_OF_RELATED_ARTICLES)
            ReplaceField(template, "[RELATEDARTICLES]", FormatingRelatedArticles(relatedArticles))

            ' HTML Meta tags for social media 
            StoryFunctions.SetSocialMediaMetaTags(URL, story.Headline, permalink,
                                                  story.TextSample.Replace("<br />", " / "),
                                                  PortalSettings.PortalName, Fid,
                                                  StoryFunctionsProperties.SOCIAL_MEDIA_ARTICLE,
                                                  Page.Header.Controls)
        End Sub

        Private Sub ReplaceField(ByRef template As String, ByVal fieldName As String, ByVal fieldValue As String)
            If Not String.IsNullOrEmpty(fieldValue) Then
                template = template.Replace(fieldName, fieldValue)
            Else
                template = template.Replace(fieldName, "")
            End If
        End Sub

        'Prints results under Calendar heading on left hand side of each story
        Protected Function FormatingRelatedAgenda(ByRef relatedStories As IQueryable(Of AP_Story)) As String
            Dim returnString As String = ""

            If relatedStories.Count > 0 Then
                returnString &= "<h2 class=""agendaTitle"">" & LocalizeString("Agenda") & "</h2>"
                For Each relatedStory In relatedStories
                    returnString &= "<div class='eventDiv'><a href=""" & NavigateURL() & "?"
                    returnString &= GetStoryURLParams(relatedStory.StoryId, Request.QueryString(ORIGINAL_MODULEID), Request.QueryString(ORIGINAL_TABID)) & """>"
                    returnString &= "<table><tr><td style='vertical-align: top;'><div class='eventDay' >" & relatedStory.StoryDate.Day & "</div>"
                    returnString &= "<div class='eventMonth'>" & relatedStory.StoryDate.ToString("MMM", New CultureInfo("fr-fr")) & "</div>"
                    returnString &= "<img src='" & ViewStoryConstants.calendarIcon & "' style='width:32px;' /></td><td style='padding-left: 12px;'>"
                    returnString &= "<h4 class='eventTitle'>" & relatedStory.Headline & "</h4>"

                    returnString &= "<h6  class='eventSample'>" & relatedStory.TextSample & "</h6></td></tr></table></a></div>"
                    relatedStory.StoryDate.ToString()
                Next
            End If


            Return returnString
        End Function

        'Prints results under Article heading on left hand side of each story
        Protected Function FormatingRelatedArticles(ByRef relatedStories As IQueryable(Of AP_Story)) As String
            Dim returnString As String = ""

            If relatedStories.Count > 0 Then
                returnString &= "<h2 class=""agendaTitle"">" & LocalizeString(StoryFunctions.getChannelTitle(relatedStories.First.TabModuleId)) & "</h2>"

                For Each relatedStory In relatedStories
                    returnString &= "<div class='eventDiv'><a href=""" & NavigateURL() & "?"
                    returnString &= GetStoryURLParams(relatedStory.StoryId, Request.QueryString(ORIGINAL_MODULEID), Request.QueryString(ORIGINAL_TABID)) & """>"
                    returnString &= "<table><tr><td style='vertical-align: top;'>"
                    returnString &= "<img src='" & ViewStoryConstants.articleIcon & "' /></td><td style='padding-left: 12px;'>"
                    returnString &= "<h4 class='eventTitle'>" & relatedStory.Headline & "</h4>"
                    returnString &= "<h6  class='eventSample'>" & relatedStory.StoryDate.ToString("dd MMMM yyyy", New CultureInfo("fr-fr")) & "</h6></td></tr></table></a></div>"
                Next
                returnString &= "</ul>"
            End If
            Return returnString
        End Function

        Protected Function FormatingRelatedStories(ByRef relatedStories As IQueryable(Of AP_Story)) As String
            Dim returnString As String = ""

            If relatedStories.Count > 0 Then
                returnString &= "<h3>" & LocalizeString("RelatedNews") & "</h3><ul class=""nav nav-tabs nav-stacked"">"
                For Each relatedStory In relatedStories
                    returnString &= "<li><a href=""" & NavigateURL() & "?"
                    returnString &= GetStoryURLParams(relatedStory.StoryId, Request.QueryString(ORIGINAL_MODULEID), Request.QueryString(ORIGINAL_TABID)) & """>"
                    returnString &= relatedStory.Headline & "</a></li>"
                Next
                returnString &= "</ul>"
            End If
            Return returnString
        End Function

#End Region 'Templating functions

#Region "Helper functions for translation after France's migration to OIB"

        Private Function GetStoryURLParams(ByVal storyId As Integer, ByVal origModString As String, ByVal origTabString As String) As String
            Return ViewStoryConstants.STORYID & "=" & storyId & "&" &
                ViewStoryConstants.ORIGINAL_MODULEID & "=" & GetModId(origModString) & "&" &
                ViewStoryConstants.ORIGINAL_TABID & "=" & GetTabId(origTabString)
        End Function

        Private Function GetTabId(ByVal OrigTabId As String) As String
            If String.IsNullOrEmpty(OrigTabId) Then
                Return OrigTabId
            ElseIf tabTranslation.ContainsKey(OrigTabId) Then
                Return tabTranslation(OrigTabId)
            Else
                Return OrigTabId
            End If
        End Function

        Private Function GetModId(ByVal OrigModId As String) As String
            If String.IsNullOrEmpty(OrigModId) Then
                Return OrigModId
            ElseIf modTranslation.ContainsKey(OrigModId) Then
                Return modTranslation(OrigModId)
            Else
                Return OrigModId
            End If
        End Function

#End Region 'Helper functions for translation after France's migration to OIB

        '#Region "Optional Interfaces"

        '        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
        '            Get

        '                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection

        '                Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.NewStoryControlKey),
        '                            StoryFunctionsConstants.NewStoryControlKey, "", "eip_edit.png",
        '                            NavigateURL(CInt(GetTabId(Request.QueryString("origTabId"))), "AddEditStory", {"mid", GetModId(Request.QueryString("origModId"))}), False, SecurityAccessLevel.Edit, True, False)
        '                Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.UnpublishedControlKey),
        '                            StoryFunctionsConstants.UnpublishedControlKey, "", "icon_lists_16px.gif",
        '                            EditUrl(StoryFunctionsConstants.UnpublishedControlKey), False, SecurityAccessLevel.Edit, True, False)
        '                Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.TagSettingsControlKey),
        '                            StoryFunctionsConstants.TagSettingsControlKey, "", "action_settings.gif",
        '                            EditUrl(StoryFunctionsConstants.TagSettingsControlKey), False, SecurityAccessLevel.Edit, True, False)

        '                If UserInfo.IsInRole("Administrators") Or UserInfo.IsSuperUser Then
        '                    Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.StorySettingsControlKey),
        '                                StoryFunctionsConstants.StorySettingsControlKey, "", "action_settings.gif",
        '                                EditUrl(StoryFunctionsConstants.StorySettingsControlKey), False, SecurityAccessLevel.Edit, True, False)
        '                End If

        '                Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.NewStoryControlKey),
        '                            StoryFunctionsConstants.NewStoryControlKey, "", "add.gif",
        '                            EditUrl(StoryFunctionsConstants.NewStoryControlKey), False, SecurityAccessLevel.Edit, True, False)

        '                Return Actions
        '            End Get
        '        End Property

        '#End Region

    End Class
End Namespace