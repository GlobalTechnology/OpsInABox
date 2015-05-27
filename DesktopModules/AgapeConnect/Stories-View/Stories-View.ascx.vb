'===============================================================
'Stories-View.ascx.vb
'---------------------------------------------------------------
'Purpose :  Builds the fields that are used in the Story 
'           templates for showing the related/associated Stories.
'===============================================================
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

Namespace DotNetNuke.Modules.FullStory


    Partial Class acViewFullStory




        Inherits Entities.Modules.PortalModuleBase


        ''SET THESE TO CONTAIN Old TabIds (key) and New Tab Ids (Value)... likewise for modules
        Private tabTranslation As New Dictionary(Of String, String) From {{"190", "2341"}, {"55", "2335"}, {"200", "2351"}}
        Private modTranslation As New Dictionary(Of String, String) From {{"419", "5548"}, {"434", "5524"}, {"507", "5571"}}



        Public IsBoosted As Boolean = False
        Public IsBlocked As Boolean = False
        Public location As String = ""
        Public zoomLevel As Integer = 4
        Public eventIcon As String = "/DesktopModules/AgapeConnect/Stories/images/eventIcon.png"
        Public articleIcon As String = "/DesktopModules/AgapeConnect/Stories/images/articleIcon.png"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If String.IsNullOrEmpty(Request.QueryString("StoryId")) Then
                PagePanel.Visible = False
                NotFoundLabel.Visible = True
                NotFoundLabel.Text = ""
                Return
            End If

            Dim d As New StoriesDataContext
            Dim r = (From c In d.AP_Stories Where c.StoryId = Request.QueryString("StoryID")).First
            Dim thecache = From c In d.AP_Stories_Module_Channel_Caches Where c.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = r.TabModuleId And c.Link.EndsWith("StoryId=" & r.StoryId)


            If Not String.IsNullOrEmpty(Request.Form("Boosted")) Then

                If thecache.Count > 0 Then
                    If CBool(Request.Form("Blocked")) And Not thecache.First.Block Then

                        StoryFunctions.BlockStoryAccrossSite(thecache.First.Link)

                        'thecache.First.Block = CBool(Request.Form("Blocked"))
                    ElseIf (Not CBool(Request.Form("Blocked"))) And thecache.First.Block Then
                        StoryFunctions.UnBlockStoryAccrossSite(thecache.First.Link)

                    End If
                    Dim changed As Boolean = False
                    If (Not CBool(Request.Form("Blocked"))) And CBool(Request.Form("Boosted")) Then
                        changed = True
                        If Not thecache.First.BoostDate Is Nothing Then
                            thecache.First.BoostDate = Today.AddDays(StoryFunctions.GetBoostDuration(PortalId))

                        Else
                            thecache.First.BoostDate = Today.AddDays(StoryFunctions.GetBoostDuration(PortalId))

                        End If
                    ElseIf (Not CBool(Request.Form("Blocked"))) And (Not CBool(Request.Form("Boosted"))) Then
                        thecache.First.BoostDate = Nothing
                        changed = True
                    End If
                    If changed Then
                        d.SubmitChanges()
                        Dim theMod = StoryFunctions.GetStoryModule(TabModuleId)
                        StoryFunctions.RefreshFeed(r.TabModuleId, thecache.First.ChannelId, True)

                        StoryFunctions.PrecalAllCaches(r.TabModuleId)

                    End If



                End If
                Return
            End If



            If String.IsNullOrEmpty(Request.QueryString("StoryID")) Then
                PagePanel.Visible = False
                NotFoundLabel.Visible = True

            Else

                '!!!!!!!btnEdit.Visible = IsEditable
                '!!!!!!!btnNew.Visible = IsEditable

                If Me.UserInfo.IsSuperUser And IsEditable() Then
                    'SuperPowers.Visible = True
                End If


                PagePanel.Visible = True
                NotFoundLabel.Visible = False
                StoryIdHF.Value = Request.QueryString("StoryId")

                ' Get template name from Story module advanced settings - "StoryView" is default template
                Dim AdvSettings As Dictionary(Of String, String) = StoryFunctions.GetAdvancedSettings(r.TabModuleId)
                Dim TemplateName As String = AdvSettings.GetValue("template", "StoryView")

                Dim sv As String = StaffBrokerFunctions.GetTemplate(TemplateName, PortalId)

                ReplaceField(sv, "[HEADLINE]", r.Headline)
                Dim tp As DotNetNuke.Framework.CDefault = CType(Me.Page, DotNetNuke.Framework.CDefault)
                tp.Title = r.Headline & " - " & PortalSettings.PortalName
                location = r.Latitude.Value.ToString(New CultureInfo("")) & ", " & r.Longitude.Value.ToString(New CultureInfo(""))

                ReplaceField(sv, "[STORYTEXT]", r.StoryText)
                ReplaceField(sv, "[MAP]", " <div id=""map_canvas""></div>")
                Dim thePhoto = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(r.PhotoId)

                Dim URL = "http://" & PortalSettings.PortalAlias.HTTPAlias & DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(thePhoto)
                ReplaceField(sv, "[IMAGEURL]", URL)




                Dim meta As New HtmlMeta
                meta.Attributes.Add("property", "og:image")
                meta.Content = URL
                Page.Header.Controls.AddAt(0, meta)
                Dim meta2 As New HtmlMeta
                meta2.Attributes.Add("property", "og:title")
                meta2.Content = r.Headline
                Page.Header.Controls.AddAt(0, meta2)

                Dim permalink = NavigateURL(TabId, "", "StoryId=" & Request.QueryString("StoryId"), "origTabId=" & Request.QueryString("origTabId"), "origModId=" & Request.QueryString("origModId"))
                ReplaceField(sv, "[DATAHREF]", permalink)
                Dim meta3 As New HtmlMeta
                meta3.Attributes.Add("property", "og:url")

                meta3.Content = permalink
                Page.Header.Controls.AddAt(0, meta3)

                Dim meta4 As New HtmlMeta
                meta4.Attributes.Add("property", "og:description")
                meta4.Content = r.TextSample.Replace("<br />", " / ")
                Page.Header.Controls.AddAt(0, meta4)

                Dim meta5 As New HtmlMeta
                meta5.Attributes.Add("property", "og:site_name")
                meta5.Content = PortalSettings.PortalName
                Page.Header.Controls.AddAt(0, meta5)

                Dim Fid = StaffBrokerFunctions.GetSetting("FacebookId", PortalId)
                If Not String.IsNullOrEmpty(Fid) Then
                    Dim meta6 As New HtmlMeta
                    meta6.Attributes.Add("property", "fb:app_id")
                    meta6.Content = Fid
                    Page.Header.Controls.AddAt(0, meta6)
                End If

                Dim meta7 As New HtmlMeta
                meta7.Attributes.Add("property", "og:type")
                meta7.Content = "article"
                Page.Header.Controls.AddAt(0, meta7)


                ReplaceField(sv, "[FACEBOOKID]", Fid)



                ReplaceField(sv, "[AUTHOR]", r.Author)
                ReplaceField(sv, "[DATE]", r.StoryDate.ToString("d MMMM yyyy"))

                If (Not r.UpdatedDate Is Nothing) Then
                    If (DateDiff(DateInterval.Day, r.StoryDate, r.UpdatedDate.Value) > 14) Then
                        'Only show updated date if the update was more than two weeks after the creation date
                        ReplaceField(sv, "[UPDATEDDATE]", "(updated " & r.UpdatedDate.Value.ToString("d MMM yyyy") & ")")
                    End If

                End If
                ReplaceField(sv, "[UPDATEDDATE]", "")
                ReplaceField(sv, "[RSSURL]", "/DesktopModules/AgapeConnect/Stories/Feed.aspx?channel=" & TabModuleId)
                ReplaceField(sv, "[SAMPLE]", r.TextSample)
                ReplaceField(sv, "[SUBTITLE]", r.Subtitle)
                ReplaceField(sv, "[FIELD1]", r.Field1)
                ReplaceField(sv, "[FIELD2]", r.Field2)

                Dim isEventTag As Boolean = (From c In r.AP_Stories_Tag_Metas Where c.AP_Stories_Tag.TagName = "Evénement").Count > 0

                ReplaceField(sv, "[TYPEICON]", IIf(isEventTag, eventIcon, articleIcon))
                ReplaceField(sv, "[TYPENAME]", IIf(isEventTag, "Evénement", "Article"))

                If (isEventTag) Then
                    'The story is an event
                    ReplaceField(sv, "[AGENDA]", GetEventAgenda(r.StoryId, r.TabModuleId))
                    zoomLevel = 15
                Else
                    'The story is an article
                    'the call GetArticleAgenda passes the list of tag ids of the current story as parameter
                    ReplaceField(sv, "[AGENDA]", GetArticleAgenda(r.AP_Stories_Tag_Metas.Select(Function(c) c.AP_Stories_Tag.StoryTagId).ToList, r.TabModuleId))
                End If




                If r.Field3.Contains("#selAuth#") Then
                    Dim authID = r.Field3.Substring(9)
                    Dim uc As New UserController()
                    Dim auth = uc.GetUser(Me.PortalId, authID)
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
                    ReplaceField(sv, "[AUTHPHOTO]", thisPhoto)
                    ReplaceField(sv, "[AUTHBIO]", thisBio)
                Else
                    ReplaceField(sv, "[FIELD3]", r.Field3)
                End If



                PhotoIdHF.Value = r.PhotoId



                'Generate Related Stories Sections
                ReplaceField(sv, "[RELATEDSTORIES]", GetRelatedStories(r.AP_Stories_Tag_Metas.Select(Function(c) c.AP_Stories_Tag.StoryTagId).ToList, r.Author))


                ReplaceField(sv, "[RELATEDARTICLES]", GetRelatedArticles(r.AP_Stories_Tag_Metas.Select(Function(c) c.AP_Stories_Tag.StoryTagId).ToList, r.TabModuleId))



                If Not r.TranslationGroup Is Nothing Then

                    'TranslationGroupHF.Value = r.TranslationGroup
                    SuperPowers.TranslationGroupId = r.TranslationGroup

                    Dim Translist = From c In d.AP_Stories Where c.TranslationGroup = r.TranslationGroup And c.PortalID = r.PortalID And c.StoryId <> r.StoryId Select c.Language, c.StoryId

                    If Translist.Count > 0 Then
                        Dim Flags As String = "<div style=""width: 100%;""><i>This story is also available in:</i> <div style=""margin: 4px 0 12px 0;"">"


                        For Each row In Translist
                            Dim Lang = GetLanguageName(row.Language)
                            Flags &= "<a href=""" & NavigateURL() & "?StoryId=" & row.StoryId & "&origModId=" & GetModId(Request.QueryString("origModId")) & "&origTabId=" & GetTabId(Request.QueryString("origTabId")) & """ target=""_self""><span title=""" & Lang & """><img  src=""" & GetFlag(row.Language) & """ alt=""" & Lang & """  /></span></a>"

                        Next

                        Flags &= "</div> </div>"


                        ReplaceField(sv, "[LANGUAGES]", Flags)
                    End If


                End If
                ReplaceField(sv, "[LANGUAGES]", "")
                If (sv.IndexOf("[SUPERPOWERS]") < 0) Then
                    ltStory1.Text = sv
                    ltStory2.Text = ""
                Else
                    ltStory1.Text = sv.Substring(0, sv.IndexOf("[SUPERPOWERS]"))

                    ltStory2.Text = sv.Substring(sv.IndexOf("[SUPERPOWERS]") + 13)
                End If




                If IsEditable Then
                    SuperPowers.Visible = True
                    If thecache.Count > 0 Then

                        SuperPowers.CacheId = thecache.First.CacheId


                    End If
                    SuperPowers.SuperEditor = UserInfo.IsSuperUser
                    SuperPowers.EditUrl = NavigateURL(CInt(GetTabId(Request.QueryString("origTabId"))), "AddEditStory", {"mid", GetModId(Request.QueryString("origModId"))})
                    SuperPowers.PortalId = PortalId
                    SuperPowers.SetControls()

                End If


                'Get Current Channel 



                'If thecache.Count > 0 Then
                '    If thecache.First.Block Then
                '        lblPowerStatus.Text = "This story has been blocked, and won't appear in the channel feed."
                '        IsBlocked = True
                '    ElseIf Not thecache.First.BoostDate Is Nothing Then
                '        If thecache.First.BoostDate >= Today Then
                '            IsBoosted = True
                '            lblPowerStatus.Text = "Boosted until " & thecache.First.BoostDate.Value.ToString("dd MMM yyyy")

                '        End If
                '    End If
                'End If



            End If





        End Sub
        Private Function GetTabId(ByVal OrigTabId As String) As String
            If modTranslation.ContainsKey(OrigTabId) Then
                Return modTranslation(OrigTabId)
            Else
                Return OrigTabId
            End If
        End Function
        Private Function GetModId(ByVal OrigModId As String) As String
            If modTranslation.ContainsKey(OrigModId) Then
                Return modTranslation(OrigModId)
            Else
                Return OrigModId
            End If
        End Function
        Private Sub ReplaceField(ByRef sv As String, ByVal fieldName As String, ByVal fieldValue As String)
            If Not String.IsNullOrEmpty(fieldValue) Then
                sv = sv.Replace(fieldName, fieldValue)
            Else
                sv = sv.Replace(fieldName, "")
            End If

        End Sub

        'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        '    Response.Redirect(EditUrl("AddEditStory") & "?StoryID=" & Request.QueryString("StoryID"))

        'End Sub


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



        Protected Function GetRelatedStories(ByVal Tags As List(Of Integer), ByVal Author As String) As String
            Dim d As New StoriesDataContext
            Dim rtn As String = ""


            'Dim q = From c In d.AP_Stories Where c.PortalID = PortalId And c.IsVisible And (c.Author.ToLower = Author.ToLower Or c.AP_Stories_Tag_Metas.Where(Function(x) Tags.Contains(x.TagId)).Count > 0) And Not c.StoryId = Request.QueryString("StoryId")

            Dim q = From c In d.AP_Stories Where c.PortalID = PortalId And c.IsVisible And (c.AP_Stories_Tag_Metas.Where(Function(x) Tags.Contains(x.TagId)).Count > 0) And Not c.StoryId = Request.QueryString("StoryId")


            If q.Count > 0 Then


                rtn &= "<h3>Related News Items:</h3><ul class=""nav nav-tabs nav-stacked"">"
                For Each row In q.Take(5).OrderByDescending(Function(c) c.StoryDate)

                    rtn &= "<li><a href=""" & NavigateURL() & "?StoryId=" & row.StoryId & "&origModId=" & GetModId(Request.QueryString("origModId")) & "&origTabId=" & GetTabId(Request.QueryString("origTabId")) & """>" & row.Headline & "</a></li>"

                Next


                rtn &= "</ul>"


            End If

            Return rtn
        End Function

        'The story is an event or an article. This finds the related articles (with same TabModuleID to exclude stories from other installed Stories module instances).
        Protected Function GetRelatedArticles(ByVal Tags As List(Of Integer), ByVal TabModuleID As Integer) As String
            Dim d As New StoriesDataContext
            Dim rtn As String = ""


            Dim q = From c In d.AP_Stories Where c.PortalID = PortalId And c.TabModuleId = TabModuleID And c.IsVisible And c.AP_Stories_Tag_Metas.Where(Function(x) x.AP_Stories_Tag.TagName = "Evénement").Count = 0 And (c.AP_Stories_Tag_Metas.Where(Function(x) Tags.Contains(x.TagId)).Count > 0) And Not c.StoryId = Request.QueryString("StoryId") Order By c.StoryDate Descending Distinct


            If q.Count > 0 Then


                rtn &= "<h2 class=""agendaTitle"">A lire aussi</h2>"
                For Each row In q.Take(3).OrderByDescending(Function(c) c.StoryDate)
                    rtn &= "<div class='eventDiv'><a href=""" & NavigateURL() & "?StoryId=" & row.StoryId & "&origModId=" & GetModId(Request.QueryString("origModId")) & "&origTabId=" & GetTabId(Request.QueryString("origTabId")) & """>"
                    rtn &= "<table><tr><td style='vertical-align: top;'>"
                    rtn &= "<img src='/DesktopModules/AgapeConnect/Stories/images/articleIcon.png' style='width:30px;' /></td><td style='padding-left: 12px;'>"
                    rtn &= "<h4 class='eventTitle'>" & row.Headline & "</h4>"

                    rtn &= "<h6  class='eventSample'>" & row.StoryDate.ToString("dd MMMM yyyy", New CultureInfo("fr-fr")) & "</h6></td></tr></table></a></div>"

                Next


                rtn &= "</ul>"


            End If

            Return rtn
        End Function

        'The story is an event. This finds the first 3 upcoming events (with same TabModuleID to exclude stories from other installed Stories module instances).
        Protected Function GetEventAgenda(ByVal StoryId As String, ByVal TabModuleID As Integer) As String
            Dim d As New StoriesDataContext
            Dim rtn As String = ""


            'gets all stories with date of today or future and tagged Evénement. 
            Dim q = From c In d.AP_Stories Where c.PortalID = PortalId And c.TabModuleId = TabModuleID And c.IsVisible And (c.AP_Stories_Tag_Metas.Where(Function(x) x.AP_Stories_Tag.TagName = "Evénement").Count > 0) And c.StoryDate >= Today And Not c.StoryId = StoryId Order By c.StoryDate Ascending

            'prints results under Agenda heading on left hand side of each story
            If q.Count > 0 Then


                rtn &= "<h2 class=""agendaTitle"">Agenda</h2>"
                For Each row In q.Take(3)

                    rtn &= "<div class='eventDiv'><a href=""" & NavigateURL() & "?StoryId=" & row.StoryId & "&origModId=" & GetModId(Request.QueryString("origModId")) & "&origTabId=" & GetTabId(Request.QueryString("origTabId")) & """>"
                    rtn &= "<table><tr><td style='vertical-align: top;'><div class='eventDay' >" & row.StoryDate.Day & "</div>"
                    rtn &= "<div class='eventMonth'>" & row.StoryDate.ToString("MMM", New CultureInfo("fr-fr")) & "</div>"
                    rtn &= "<img src='/DesktopModules/AgapeConnect/Stories/images/cal.png' style='width:32px;' /></td><td style='padding-left: 12px;'>"
                    rtn &= "<h4 class='eventTitle'>" & row.Headline & "</h4>"

                    rtn &= "<h6  class='eventSample'>" & row.TextSample & "</h6></td></tr></table></a></div>"

                Next
            End If

            Return rtn
        End Function

        'The story is an article. This finds the first 3 upcoming events with the same tag (with same TabModuleID to exclude stories from other installed Stories module instances).
        Protected Function GetArticleAgenda(ByVal Tags As List(Of Integer), ByVal TabModuleID As Integer) As String
            Dim d As New StoriesDataContext
            Dim rtn As String = ""

            'gets all stories tagged Evénement and having the same tag as current and with date of today or future
            'Dim q = From c In d.AP_Stories Where c.PortalID = PortalId And c.TabModuleId = TabModuleID And c.IsVisible And c.AP_Stories_Tag_Metas.Where(Function(x) x.AP_Stories_Tag.TagName = "Evénement").Count > 0 And (c.AP_Stories_Tag_Metas.Where(Function(x) Tags.Contains(x.TagId)).Count > 0) And c.StoryDate >= Today And Not c.StoryId = Request.QueryString("StoryId") Order By c.StoryDate Ascending
            Dim q = From story In d.AP_Stories Join channel In d.AP_Stories_Module_Channel_Caches On channel.GUID Equals story.StoryId Where story.PortalID = PortalId And story.TabModuleId = TabModuleID And story.IsVisible And story.AP_Stories_Tag_Metas.Where(Function(x) x.AP_Stories_Tag.TagName = "Evénement").Count > 0 And (story.AP_Stories_Tag_Metas.Where(Function(x) Tags.Contains(x.TagId)).Count > 0) And story.StoryDate >= Today And Not story.StoryId = Request.QueryString("StoryId") And Not channel.Block = True And channel.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = story.TabModuleId Order By story.StoryDate Ascending

            If q.Count > 0 Then

                rtn &= "<h2 class=""agendaTitle"">Agenda</h2>"
                For Each row In q.Take(3).OrderBy(Function(c) c.story.StoryDate)

                    rtn &= "<div class='eventDiv'><a href=""" & NavigateURL() & "?StoryId=" & row.story.StoryId & "&origModId=" & GetModId(Request.QueryString("origModId")) & "&origTabId=" & GetTabId(Request.QueryString("origTabId")) & """>"
                    rtn &= "<table><tr><td style='vertical-align: top;'><div class='eventDay' >" & row.story.StoryDate.Day & "</div>"
                    rtn &= "<div class='eventMonth'>" & row.story.StoryDate.ToString("MMM", New CultureInfo("fr-fr")) & "</div>"
                    rtn &= "<img src='/DesktopModules/AgapeConnect/Stories/images/cal.png' style='width:32px;' /></td><td style='padding-left: 12px;'>"
                    rtn &= "<h4 class='eventTitle'>" & row.story.Headline & "</h4>"

                    rtn &= "<h6  class='eventSample'>" & row.story.TextSample & "</h6></td></tr></table></a></div>"

                Next
            End If

            Return rtn
        End Function

    End Class
End Namespace