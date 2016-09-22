Imports Microsoft.VisualBasic
Imports System.ServiceModel.Syndication
Imports System.Xml
Imports System.Net
Imports DotNetNuke
Imports DotNetNuke.Services.FileSystem
Imports Stories

Namespace Stories
    Class StoryController
        Implements Entities.Modules.ISearchable

        Public Function GetSearchItems(ModInfo As DotNetNuke.Entities.Modules.ModuleInfo) As DotNetNuke.Services.Search.SearchItemInfoCollection Implements DotNetNuke.Entities.Modules.ISearchable.GetSearchItems
            Dim d As New StoriesDataContext
            Dim SearchItemCollection As New Services.Search.SearchItemInfoCollection

            Dim mc = New DotNetNuke.Entities.Modules.ModuleController

            Dim Stories = StoryFunctions.GetVisibleNonBlockedStories(ModInfo.TabModuleID, ModInfo.PortalID)

            For Each row In Stories

                ' Dim t = mc.GetTabModule(row.TabModuleId)
                'If (t.ModuleID = ModInfo.ModuleID) Then
                Dim Keywords = row.Keywords
                For Each tag In row.AP_Stories_Tag_Metas
                    Keywords &= "," & tag.AP_Stories_Tag.TagName & " " & tag.AP_Stories_Tag.Keywords

                Next

                Dim SearchText = (row.Headline & " " & Keywords & " " & row.StoryText & " " & row.Author)
                Dim summary As String = ""



                If String.IsNullOrEmpty(row.TextSample) Then
                    summary = Left(StoryFunctions.StripTags(row.StoryText), 500)
                Else
                    summary = Left(StoryFunctions.StripTags(row.TextSample), 500)
                End If
                If (summary.IndexOf(".") > 0) Then
                    summary = summary.Substring(0, summary.LastIndexOf(".") + 1)

                End If


                Dim SearchItem As Services.Search.SearchItemInfo
                SearchItem = New Services.Search.SearchItemInfo _
                 (row.Headline,
                 summary,
                row.UserId,
               row.StoryDate, ModInfo.ModuleID,
                 "S" & row.StoryId,
              SearchText, Guid:="StoryId=" & row.StoryId, Image:=row.PhotoId, TabID:=row.TabId)
                SearchItemCollection.Add(SearchItem)
                ' End If
            Next



            Return SearchItemCollection
        End Function

    End Class

End Namespace

Public Class StoryModuleType
    Public Const Rotator As Integer = 1
    Public Const List As Integer = 2
    Public Const TagList As Integer = 3
    Public Shared Function Name(ByVal value As Integer) As String
        Select Case value
            Case 1 : Return "Rotator"
            Case 2 : Return "List"
            Case 3 : Return "TagList"
            Case Else : Return "Unknown"

        End Select
    End Function

End Class

'StoryFunctions constants
Public Module StoryFunctionsProperties
    Public StoriesModulePath As String = "/DesktopModules/AgapeConnect/Stories"

    Public Const SOCIAL_MEDIA_ARTICLE As String = "article"
    Public Const TAGS_IN_URL As String = "?tags="
    Public Const TAGS_KEYWORD As String = "tags"
    Public Const SUPERPOWERS_TEMPLATE_KEY As String = "[SUPERPOWERS]"
    Public Const FIRST_CONTROL As String = "FIRST"

    Public imageExtensions() As String = {"jpg", "jpeg", "gif", "png", "bmp"}
    Public noImage As String = "/images/no-content.png?"
    Public socialMediaProperties() As String = {"og:image", "og:title", "og:url", "og:description", "og:site_name", "fb:app_id", "og:type"}

    Public Const LatitudeKey As String = "Latitude"
    Public Const LongitudeKey As String = "Longitude"

End Module

Public Module StoryFunctionsConstants

    ' Module control keys used in DNN extension definition
    Public Const StorySettingsControlKey As String = "StorySettings"
    Public Const StoryMixerControlKey As String = "Mixer"
    Public Const UnpublishedControlKey As String = "Unpublished"
    Public Const TagSettingsControlKey As String = "TagSettings"
    Public Const NewStoryControlKey As String = "AddEditStory"
End Module

'ViewStory constants
Public Module ViewStoryConstants

    Public Const STORYID As String = "StoryID"
    Public Const BOOSTED As String = "Boosted"
    Public Const BLOCKED As String = "Blocked"
    Public Const TEMPLATE_SETTING As String = "template"
    Public Const TEMPLATE_DEFAULT As String = "StoryView"
    Public Const ORIGINAL_TABID As String = "origTabId"
    Public Const ORIGINAL_MODULEID As String = "origModId"
    Public Const FRENCH_EVENT As String = "Evénement"
    Public Const FRENCH_ARTICLE As String = "Article"
    Public Const NUM_OF_RELATED_STORIES As Integer = 5
    Public Const NUM_OF_RELATED_ARTICLES As Integer = 3
    Public Const NUM_OF_RELATED_AGENDA As Integer = 3
    Public Const HTML_META_PROPERTY As String = "property"

    Public eventIcon As String = StoryFunctionsProperties.StoriesModulePath & "/images/eventIcon.png"
    Public articleIcon As String = StoryFunctionsProperties.StoriesModulePath & "/images/articleIcon.png"
    Public calendarIcon As String = StoryFunctionsProperties.StoriesModulePath & "/images/cal.png"

End Module

Public Class StoryFunctions

#Region "Tags"
    Public Shared Function StripTags(ByVal HTML As String) As String
        ' Removes tags from passed HTML

        Dim pattern As String = "<(.|\n)*?>"
        Dim pattern2 As String = "\[.*?]]\]"
        Dim pattern3 As String = "\[.*?]\]"
        Dim pattern4 As String = "<script[\d\D]*?>[\d\D]*?</script>"
        Dim pattern5 As String = "<style[\d\D]*?>[\d\D]*?</style>"
        Dim s = HTML
        s = Regex.Replace(s, pattern4, String.Empty)
        s = Regex.Replace(s, pattern5, String.Empty)
        s = Regex.Replace(s, pattern, String.Empty)

        Return Regex.Replace(Regex.Replace(s, pattern2, String.Empty), pattern3, String.Empty).Trim()
    End Function

    Public Shared Function GetTags(ByVal TabModuleId As Integer) As IQueryable(Of AP_Stories_Tag)
        Dim d As New StoriesDataContext
        Return From c In d.AP_Stories_Tags Where c.StoryModuleId = GetStoryModule(TabModuleId).StoryModuleId Order By c.TagName
    End Function

    ' Get list of tags in module linked to at least one story
    Public Shared Function GetTagsOfStories(ByRef StoriesCache As List(Of AP_Stories_Module_Channel_Cache)) As IQueryable(Of AP_Stories_Tag)
        Dim d As New StoriesDataContext

        Dim StoriesTagMetaIds = From sc In StoriesCache
                                Join stmi In d.AP_Stories_Tag_Metas On sc.GUID Equals stmi.StoryId
                                Select stmi.TagId

        Return From st In d.AP_Stories_Tags
               Where StoriesTagMetaIds.Contains(st.StoryTagId)
               Select st Order By st.TagName

    End Function

    Public Shared Function GetTagsOfStory(ByRef Story As List(Of AP_Story)) As IQueryable(Of AP_Stories_Tag)
        Dim d As New StoriesDataContext

        Dim StoriesTagMetaIds = From s In Story
                                Join stmi In d.AP_Stories_Tag_Metas On s.StoryId Equals stmi.StoryId
                                Select stmi.TagId

        Return From st In d.AP_Stories_Tags
               Where StoriesTagMetaIds.Contains(st.StoryTagId)
               Select st Order By st.TagName

    End Function

    Public Shared Function GetTag(ByVal tagId As Integer, ByVal TabModuleId As Integer) As AP_Stories_Tag
        Dim d As New StoriesDataContext
        Return (From c In d.AP_Stories_Tags Where c.StoryTagId = tagId).First
    End Function

    Public Shared Function GetTagByName(ByVal name As String, ByVal TabModuleId As Integer) As AP_Stories_Tag
        Dim d As New StoriesDataContext
        Dim tag As New AP_Stories_Tag
        Dim tags = (From c In GetTags(TabModuleId) Where c.TagName = name)
        If tags.Count > 0 Then
            tag = tags.First
        End If
        Return tag
    End Function

    Public Shared Function IsTagValid(ByVal name As String, ByVal TabModuleId As Integer) As Boolean
        Dim d As New StoriesDataContext
        Return (From c In GetTags(TabModuleId) Where c.TagName = name).Count > 0
    End Function

    Public Shared Function ValidTagList(ByVal tagsString As String, ByVal TabModuleId As Integer) As List(Of String)
        Dim d As New StoriesDataContext
        Dim validTags As New List(Of String)

        If Not String.IsNullOrEmpty(tagsString) Then
            Dim tagList As New List(Of String)(tagsString.Split(","))

            For Each tag In tagList
                If (IsTagValid(tag, TabModuleId)) Then
                    validTags.Add(tag)
                End If
            Next
        End If

        Return validTags
    End Function

    Public Shared Sub SetTag(ByVal name As String, ByVal TabModuleId As Integer)
        Dim d As New StoriesDataContext
        Dim insert As New AP_Stories_Tag

        insert.TagName = name
        insert.Master = False
        insert.Keywords = ""
        insert.StoryModuleId = GetStoryModule(TabModuleId).StoryModuleId
        d.AP_Stories_Tags.InsertOnSubmit(insert)
        d.SubmitChanges()
    End Sub

    Public Shared Sub DeleteTag(tagId As Integer, ByVal TabModuleId As Integer)
        Dim d As New StoriesDataContext
        Dim tagToDelete = (From c In d.AP_Stories_Tags Where c.StoryModuleId = GetStoryModule(TabModuleId).StoryModuleId And c.StoryTagId = CInt(tagId)).First
        If (tagToDelete IsNot Nothing) Then
            Dim metaTagsToDelete = From c In d.AP_Stories_Tag_Metas Where c.TagId = CInt(tagId)
            d.AP_Stories_Tag_Metas.DeleteAllOnSubmit(metaTagsToDelete)
            d.AP_Stories_Tags.DeleteOnSubmit(tagToDelete)
            d.SubmitChanges()
        End If
    End Sub

    Public Shared Sub UpdateTag(ByVal name As String, ByVal keywords As String, ByVal master As String, ByVal tagId As Integer, ByVal TabModuleId As Integer)
        Dim d As New StoriesDataContext
        Dim tagToUpdate As AP_Stories_Tag = (From c In d.AP_Stories_Tags Where c.StoryModuleId = GetStoryModule(TabModuleId).StoryModuleId And c.StoryTagId = tagId).First

        tagToUpdate.TagName = name
        tagToUpdate.Keywords = keywords
        tagToUpdate.Master = master
        d.SubmitChanges()
    End Sub

    Public Shared Sub SetTagPhotoId(ByVal imageId As Integer, ByVal tagId As Integer)
        Dim d As New StoriesDataContext
        Dim tag As AP_Stories_Tag = (From c In d.AP_Stories_Tags Where c.StoryTagId = tagId).First

        tag.PhotoId = imageId
        d.SubmitChanges()
    End Sub

    Public Shared Function FormatTagsSelected(ByVal tagsQueryString As String) As String
        If String.IsNullOrEmpty(tagsQueryString) Then
            Return ""
        Else
            Return (": " & String.Join(", ", tagsQueryString.Split(",")))
        End If
    End Function

#End Region 'Tags

#Region "Channels"

    Public Shared Function getChannelTitle(ByVal TabModuleId As Integer) As String
        Dim d As New Stories.StoriesDataContext
        Try
            Return (From moduleChannel In d.AP_Stories_Module_Channels
                    Join storiesModule In d.AP_Stories_Modules On moduleChannel.StoryModuleId Equals storiesModule.StoryModuleId
                    Where storiesModule.TabModuleId = TabModuleId
                    Select moduleChannel.ChannelTitle).First
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Shared Function AddLocalChannel(ByVal tabModuleId As Integer, ByVal PortalAlias As String, ByVal Name As String, ByVal Longitude As Double, ByVal Latitude As Double, ByVal logo As String, ByVal autoDetectLanguage As Boolean) As Integer
        Dim theModule = GetStoryModule(tabModuleId)

        Dim insert As New Stories.AP_Stories_Module_Channel
        insert.StoryModuleId = theModule.StoryModuleId
        insert.Weight = 1.0
        insert.Type = 2
        insert.URL = "https://" & PortalAlias & StoryFunctionsProperties.StoriesModulePath & "/Feed.aspx?channel=" & tabModuleId

        Name = Name

        insert.ChannelTitle = Name
        insert.Language = CultureInfo.CurrentCulture.Name
        insert.Latitude = Latitude
        insert.Longitude = Longitude
        insert.AutoDetectLanguage = autoDetectLanguage

        insert.ImageId = logo

        Dim d2 As New Stories.StoriesDataContext
        d2.AP_Stories_Module_Channels.InsertOnSubmit(insert)
        d2.SubmitChanges()
        RefreshFeed(tabModuleId, insert.ChannelId, False)

        Return insert.ChannelId
    End Function

    Public Shared Sub RefreshLocalChannel(ByVal tabModuleId As Integer)
        Dim d As New Stories.StoriesDataContext
        Dim Channels = From c In d.AP_Stories_Module_Channels Where c.URL.Contains("channel=" & tabModuleId)
        For Each row In Channels
            RefreshFeed(row.AP_Stories_Module.TabModuleId, row.ChannelId, False)
        Next



    End Sub

#End Region 'Channels

#Region "Publishing"
    'Determines if a story is publishable, if true it is published
    Public Shared Function PublishStory(ByVal StoryId As Integer) As Boolean
        Dim d As New Stories.StoriesDataContext
        Dim theStory = From c In d.AP_Stories Where c.StoryId = StoryId
        Dim r = False

        If theStory.Count > 0 Then
            'check if a photo has been uploaded for the story
            If (theStory.First.PhotoId > 0) Then
                r = True
                theStory.First.IsVisible = True
                d.SubmitChanges()

                'Refresh all stories that are listening to the current channel
                StoryFunctions.RefreshAfterStoryPublished(theStory.First.TabModuleId)
            End If
        End If
        Return r
    End Function

    Public Shared Sub RefreshAfterStoryPublished(ByVal TabModuleId As Integer)
        Dim d As New Stories.StoriesDataContext

        Dim Channels = From c In d.AP_Stories_Module_Channels Where c.URL.EndsWith("channel=" & TabModuleId)

        For Each channel In Channels
            RefreshFeed(channel.AP_Stories_Module.TabModuleId, channel.ChannelId, False)
            PrecalAllCaches(channel.AP_Stories_Module.TabModuleId)
        Next
        StoryFunctions.RefreshLocalChannel(TabModuleId)
    End Sub

    Public Shared Function GetUnpublishedStories(ByVal tabModuleId As Integer) As IQueryable(Of AP_Story)
        Dim d As New Stories.StoriesDataContext

        Return From c In d.AP_Stories
               Where c.TabModuleId = tabModuleId And c.IsVisible = False
               Order By c.StoryDate Descending
    End Function

#End Region 'Publishing

#Region "Story"

    Public Shared Function GetStory(ByVal storyID As Integer) As AP_Story
        Dim d As New StoriesDataContext
        Return (From c In d.AP_Stories Where c.StoryId = storyID).First
    End Function

    Public Shared Function GetStoryInCache(ByVal storyID As Integer, ByVal tabModuleID As Integer) As IQueryable(Of AP_Stories_Module_Channel_Cache)
        Dim d As New StoriesDataContext
        Return From c In d.AP_Stories_Module_Channel_Caches
               Where c.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = tabModuleID _
                                And c.GUID = storyID
    End Function

    'Returns only visible and non-blocked stories 
    Public Shared Function GetVisibleNonBlockedStories(ByVal TabModuleID As Integer, ByVal portalID As Integer) As IQueryable(Of AP_Story)
        Dim d As New StoriesDataContext

        Dim stories As IQueryable(Of AP_Story) = From story In d.AP_Stories
                                                 Join channelCache In d.AP_Stories_Module_Channel_Caches On channelCache.GUID Equals story.StoryId
                                                 Where story.PortalID = portalID _
                                                     And story.TabModuleId = TabModuleID _
                                                     And story.IsVisible = True _
                                                     And channelCache.Block = False
                                                 Select story
        Return stories
    End Function

    Public Shared Function IsStoryType(ByVal story As AP_Story, ByVal type As String) As Boolean
        Dim d As New StoriesDataContext
        Return (From c In story.AP_Stories_Tag_Metas Where c.AP_Stories_Tag.TagName = type).Count > 0
    End Function

    'Input Parameters
    '   StoryId : an event
    '   Quantity : the number of storys that will be returned 
    'Returns
    '   The events the most in the future 
    '   with same TabModuleID (in same channel)
    '   if they are not blocked in the cache table
    '   with date of today or future
    Public Shared Function GetRelatedEventsForEvents(ByVal StoryId As Integer, ByVal TabModuleID As Integer,
                                            ByVal portalID As Integer, ByVal quantity As Integer) As IQueryable(Of AP_Story)
        Dim d As New StoriesDataContext

        Dim related As IQueryable(Of AP_Story) = From story In d.AP_Stories
                                                 Join channel In d.AP_Stories_Module_Channel_Caches On channel.GUID Equals story.StoryId
                                                 Where story.PortalID = portalID _
                                                     And story.TabModuleId = TabModuleID _
                                                     And story.IsVisible _
                                                     And (story.AP_Stories_Tag_Metas.Where(Function(x) x.AP_Stories_Tag.TagName = FRENCH_EVENT).Count > 0) _
                                                     And story.StoryDate >= Today _
                                                     And Not story.StoryId = StoryId _
                                                     And Not channel.Block = True _
                                                     And channel.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = story.TabModuleId
                                                 Select story
                                                 Order By story.StoryDate Ascending
        Return related.Take(quantity)
    End Function

    'Input Parameters
    '   StoryId : an article
    '   Quantity : the number of storys that will be returned 
    'Returns
    '   The events the most in the future 
    '   with same TabModuleID (in same channel)
    '   with the same tag
    '   if they are not blocked in the cache table
    '   with date of today or future
    Public Shared Function GetRelatedEventsForArticles(ByVal storyId As Integer, ByVal tabModuleID As Integer,
                                            ByVal portalID As Integer, ByVal quantity As Integer) As IQueryable(Of AP_Story)
        Dim d As New StoriesDataContext
        Dim storyList As New List(Of AP_Story)()
        storyList.Add(GetStory(storyId))

        Dim tagIdList As List(Of Integer) = (From c In GetTagsOfStory(storyList) Select c.StoryTagId).ToList
        Dim related As IQueryable(Of AP_Story) = From story In d.AP_Stories
                                                 Join channel In d.AP_Stories_Module_Channel_Caches On channel.GUID Equals story.StoryId
                                                 Where story.PortalID = portalID _
                                                     And story.TabModuleId = tabModuleID _
                                                     And story.IsVisible _
                                                     And story.AP_Stories_Tag_Metas.Where(Function(x) x.AP_Stories_Tag.TagName = FRENCH_EVENT).Count > 0 _
                                                     And (story.AP_Stories_Tag_Metas.Where(Function(x) tagIdList.Contains(x.TagId)).Count > 0) _
                                                     And story.StoryDate >= Today _
                                                     And Not story.StoryId = storyId _
                                                     And Not channel.Block = True _
                                                     And channel.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = story.TabModuleId
                                                 Select story
                                                 Order By story.StoryDate Ascending
        Return related.Take(quantity)
    End Function

    Public Shared Function GetRelatedStories(ByVal storyId As Integer, ByVal tabModuleID As Integer,
                                          ByVal portalID As Integer, ByVal quantity As Integer) As IQueryable(Of AP_Story)
        Dim d As New StoriesDataContext
        Dim storyList As New List(Of AP_Story)()
        storyList.Add(GetStory(storyId))

        Dim tagIdList As List(Of Integer) = (From c In GetTagsOfStory(storyList) Select c.StoryTagId).ToList

        Dim related As IQueryable(Of AP_Story) = From story In d.AP_Stories
                                                 Where story.PortalID = portalID _
                                                     And story.IsVisible _
                                                     And (story.AP_Stories_Tag_Metas.Where(Function(x) tagIdList.Contains(x.TagId)).Count > 0) _
                                                     And Not story.StoryId = storyId
                                                 Select story
        Return related.Take(quantity)
    End Function

    'Input Parameters
    '   StoryId : an event or an article
    '   Quantity : the number of storys that will be returned 
    'Returns
    '   related articles 
    '   the most recent
    '   with same TabModuleID (in same channel)
    '   with the same tag
    Public Shared Function GetRelatedArticles(ByVal storyId As Integer, ByVal tabModuleID As Integer,
                                          ByVal portalID As Integer, ByVal quantity As Integer) As IQueryable(Of AP_Story)
        Dim d As New StoriesDataContext
        Dim storyList As New List(Of AP_Story)()
        storyList.Add(GetStory(storyId))

        Dim tagIdList As List(Of Integer) = (From c In GetTagsOfStory(storyList) Select c.StoryTagId).ToList

        Dim related As IQueryable(Of AP_Story) = From story In d.AP_Stories
                                                 Join channel In d.AP_Stories_Module_Channel_Caches On channel.GUID Equals story.StoryId
                                                 Where story.PortalID = portalID _
                                                     And story.TabModuleId = tabModuleID _
                                                     And story.IsVisible _
                                                     And story.AP_Stories_Tag_Metas.Where(Function(x) x.AP_Stories_Tag.TagName = FRENCH_EVENT).Count = 0 _
                                                     And (story.AP_Stories_Tag_Metas.Where(Function(x) tagIdList.Contains(x.TagId)).Count > 0) _
                                                     And Not story.StoryId = storyId _
                                                     And Not channel.Block = True _
                                                     And channel.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = story.TabModuleId
                                                 Select story
                                                 Order By story.StoryDate Descending
        Return related.Take(quantity)
    End Function

#End Region 'Story

#Region "Story Controls"

    Public Shared Function GetStoryControlLocation(ByVal storyControlId As String) As AP_Stories_Control
        Dim d As New StoriesDataContext
        Dim control As New AP_Stories_Control

        If String.Equals(StoryFunctionsProperties.FIRST_CONTROL, storyControlId) Then
            control = d.AP_Stories_Controls.First
        Else

            Dim controls = From c In d.AP_Stories_Controls Where c.StoryControlId = CInt(storyControlId)
            If controls.Count > 0 Then
                control = controls.First
            End If

        End If

        Return control
    End Function

#End Region

#Region "Templates"

    Public Shared Function GetTemplate(ByVal TabModuleID As Integer, ByVal templateSetting As String,
                                ByVal templateDefault As String, ByVal portalID As Integer) As String

        Dim AdvSettings As Dictionary(Of String, String) = StoryFunctions.GetAdvancedSettings(TabModuleID)
        Dim TemplateName As String = AdvSettings.GetValue(templateSetting, templateDefault)
        Return StaffBrokerFunctions.GetTemplate(TemplateName, portalID)
    End Function

    Public Shared Sub SetSocialMediaMetaTags(ByVal image As String, ByVal title As String,
                                             ByVal url As String, ByVal description As String,
                                             ByVal siteName As String, ByVal appId As String,
                                             ByVal type As String, ByRef controls As ControlCollection)

        Dim dictionary As New Dictionary(Of String, String)
        dictionary.Add(StoryFunctionsProperties.socialMediaProperties.ElementAt(0), image)
        dictionary.Add(StoryFunctionsProperties.socialMediaProperties.ElementAt(1), title)
        dictionary.Add(StoryFunctionsProperties.socialMediaProperties.ElementAt(2), url)
        dictionary.Add(StoryFunctionsProperties.socialMediaProperties.ElementAt(3), description)
        dictionary.Add(StoryFunctionsProperties.socialMediaProperties.ElementAt(4), siteName)
        dictionary.Add(StoryFunctionsProperties.socialMediaProperties.ElementAt(5), appId)
        dictionary.Add(StoryFunctionsProperties.socialMediaProperties.ElementAt(6), type)

        For Each pair As KeyValuePair(Of String, String) In dictionary
            If Not String.IsNullOrEmpty(pair.Value) Then
                Dim meta As New HtmlMeta
                meta.Attributes.Add(HTML_META_PROPERTY, pair.Key)
                meta.Content = pair.Value
                controls.AddAt(0, meta)
            End If
        Next
    End Sub

#End Region 'Templates

#Region "Boost/Block"

    Public Shared Sub BlockStoryAccrossSite(ByVal StoryURL As String)
        Dim d As New Stories.StoriesDataContext

        Dim theCacheItems = From c In d.AP_Stories_Module_Channel_Caches Where c.Link = StoryURL


        For Each row In theCacheItems
            row.Block = True
            row.BoostDate = Nothing


        Next
        d.SubmitChanges()
    End Sub

    Public Shared Sub UnBlockStoryAccrossSite(ByVal StoryURL As String)
        Dim d As New Stories.StoriesDataContext

        Dim theCacheItems = From c In d.AP_Stories_Module_Channel_Caches Where c.Link = StoryURL


        For Each row In theCacheItems
            row.Block = False

        Next
        d.SubmitChanges()
    End Sub

    Public Shared Function GetBoostDuration(ByVal PortalId As Integer) As Integer
        Dim bl As String = StaffBrokerFunctions.GetSetting("StoryBoostLength", PortalId)
        If Not String.IsNullOrEmpty(bl) Then
            Return CInt(bl)
        Else
            Return 30
        End If
    End Function

    Public Shared Function GetBoost(ByVal boostDate As Date?) As Double
        If boostDate Is Nothing Then
            Return 0
        Else
            Return IIf(boostDate >= Today, 3, 0)

        End If
    End Function

    Public Shared Sub SetBoostDate(ByVal GUID As Integer, ByVal boostDate As Nullable(Of Date), ByVal tabModuleID As Integer)
        Dim d As New StoriesDataContext
        Dim storyInCache As AP_Stories_Module_Channel_Cache =
            (From c In d.AP_Stories_Module_Channel_Caches
             Where c.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = tabModuleID _
                 And c.GUID = GUID).First

        storyInCache.BoostDate = boostDate
        d.SubmitChanges()
    End Sub

#End Region 'Boost/Block

#Region "Latitude/Longitude"

    Public Shared Function GetDefaultLatLong(ByVal TabModuleId As Integer) As String
        Dim d As New StoriesDataContext
        Dim location As String = ""
        Dim localChannel = (From c In d.AP_Stories_Module_Channels
                            Where c.Type = 2 _
                            And c.AP_Stories_Module.TabModuleId = TabModuleId)

        If localChannel.Count > 0 AndAlso (localChannel.First.Latitude IsNot Nothing And localChannel.First.Longitude IsNot Nothing) Then
            location = CDbl(localChannel.First.Latitude).ToString(New CultureInfo("")) & ", " & CDbl(localChannel.First.Longitude).ToString(New CultureInfo(""))
        Else
            location = CDbl(0.000000).ToString(New CultureInfo("")) & ", " & CDbl(0.000000).ToString(New CultureInfo(""))
        End If
        Return location

    End Function

    Public Shared Sub SetStoryLatLong(ByRef location As String, ByRef story As AP_Story, ByVal TabModuleId As Integer)
        Dim geoLoc = location.Split(",")
        If geoLoc.Count = 2 Then
            story.Latitude = Double.Parse(geoLoc(0).Replace(" ", ""), New CultureInfo(""))
            story.Longitude = Double.Parse(geoLoc(1).Replace(" ", ""), New CultureInfo(""))
        Else 'the contents of the map textbox is empty
            Dim geoLocation As String = StoryFunctions.GetDefaultLatLong(TabModuleId)
            story.Latitude = Double.Parse(geoLocation.Split(",")(0).Replace(" ", ""), New CultureInfo(""))
            story.Longitude = Double.Parse(geoLocation.Split(",")(1).Replace(" ", ""), New CultureInfo(""))
        End If
    End Sub

#End Region 'Latitude/Longitude

    Public Shared Function GetPhotoURL(ByVal imageId As Nullable(Of Integer)) As String

        Dim imageUrl As String
        If (imageId IsNot Nothing And imageId > 0) Then
            Dim imageFile = FileManager.Instance.GetFile(imageId)
            If (imageFile IsNot Nothing) And (StoryFunctionsProperties.imageExtensions.Contains(imageFile.Extension.ToLower)) Then
                imageUrl = FileManager.Instance.GetUrl(imageFile)
            Else
                imageUrl = StoryFunctionsProperties.noImage
            End If
        Else
            imageUrl = StoryFunctionsProperties.noImage
        End If

        Return imageUrl
    End Function

    Public Shared Function GetAdvancedSettings(ByVal TabModuleId As Integer) As Dictionary(Of String, String)

        Dim resultDictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)

        Dim mc As New DotNetNuke.Entities.Modules.ModuleController
        Dim tm = mc.GetTabModule(TabModuleId)
        Dim AdvancedSettingsStr As String = tm.TabModuleSettings("AdvancedSettings")
        If AdvancedSettingsStr <> "" Then

            Dim pairSeparator = ","
            Dim keyValueSeparator = ":"
            Dim quote = "'"
            Dim pairs As String() = AdvancedSettingsStr.Split(pairSeparator)
            For Each pair As String In pairs

                Dim nameValue As String() = pair.Split(keyValueSeparator)
                If nameValue.Length = 2 Then
                    resultDictionary.Add(nameValue(0).Trim(), nameValue(1).Trim().Trim(quote))
                End If
            Next pair

        End If

        Return resultDictionary

    End Function

    Private Shared Sub set_if(ByRef setting As Object, ByVal value As Object)
        If value Is Nothing Then
            Return
        Else
            setting = value

        End If
    End Sub

    Public Shared Function GetStoryModule(ByVal TabModuleId As Integer) As AP_Stories_Module
        Dim d As New StoriesDataContext

        If d.AP_Stories_Modules.Where(Function(x) x.TabModuleId = TabModuleId).Count = 0 Then
            Dim insert As New Stories.AP_Stories_Module
            insert.TabModuleId = TabModuleId
            insert.FilterType = 0
            insert.MaxDisplayItems = 15

            d.AP_Stories_Modules.InsertOnSubmit(insert)
            d.SubmitChanges()
        End If

        Return (From c In d.AP_Stories_Modules Where c.TabModuleId = TabModuleId).First
    End Function

    Public Shared Function GetTabModuleId(ByVal ChannelTitle As String) As Integer
        Dim d As New Stories.StoriesDataContext
        Try
            Dim smid = (From c In d.AP_Stories_Module_Channels Where c.ChannelTitle = ChannelTitle).First.StoryModuleId
            Return (From c In d.AP_Stories_Modules Where c.StoryModuleId = smid).First.TabModuleId

        Catch
            Return -1
        End Try
    End Function

    Public Shared Sub RefreshFeed(ByVal tabModuleId As Integer, ByVal ChannelId As Integer, Optional ByVal ClearCache As Boolean = False)

        'StaffBrokerFunctions.EventLog("Refreshing Channel: " & ChannelId, "", 1)

        Dim d As New Stories.StoriesDataContext

        If d.AP_Stories_Modules.Where(Function(x) x.TabModuleId = tabModuleId).Count = 0 Then
            Dim insert As New Stories.AP_Stories_Module
            insert.TabModuleId = tabModuleId
            insert.FilterType = 0
            insert.MaxDisplayItems = 15
            d.AP_Stories_Modules.InsertOnSubmit(insert)
            d.SubmitChanges()
        End If

        Dim theModule = (From c In d.AP_Stories_Modules Where c.TabModuleId = tabModuleId).First

        'Dim reader = XmlReader.Create("http://rss.cnn.com/rss/edition.rss")
        ' Dim reader = XmlReader.Create("http://feeds.bbci.co.uk/news/rss.xml")
        ' Dim reader = XmlReader.Create("http://www.agapeeurope.com/?feed=rss2")

        Try





            'Refresh the feed


            If ClearCache Then
                ' d.AP_Stories_Module_Channel_Caches.DeleteAllOnSubmit(theModule.AP_Stories_Module_Channels.Where(Function(x) x.ChannelId = ChannelId).First.AP_Stories_Module_Channel_Caches.Where(Function(x) x.Block <> True And (x.BoostDate Is Nothing Or x.BoostDate < Today)))
                'd.SubmitChanges()
            End If

            Dim theChannel = (From c In theModule.AP_Stories_Module_Channels Where c.ChannelId = ChannelId).First
            Dim reader = XmlReader.Create(theChannel.URL)
            Dim feed = SyndicationFeed.Load(reader)
            'If Not feed.BaseUri Is Nothing Then
            '    set_if(theChannel.URL, feed.BaseUri.AbsoluteUri)
            'End If
            'If Not feed.Title Is Nothing Then
            '    set_if(theChannel.ChannelTitle, Left(feed.Title.Text, 154))
            'End If

            'set_if(theChannel.Language, feed.Language)



            For Each row In feed.Items
                Try

                    Dim existingStory = From c In theChannel.AP_Stories_Module_Channel_Caches Where c.Link = row.Links.First.Uri.AbsoluteUri
                    If existingStory.Count = 0 Then
                        Dim insert As New Stories.AP_Stories_Module_Channel_Cache
                        If Not row.Title Is Nothing Then
                            insert.Headline = Left(row.Title.Text, 154)
                        End If
                        If Not row.Summary Is Nothing Then
                            insert.Description = Left(row.Summary.Text, 500)
                        ElseIf Not row.Content Is Nothing Then

                            insert.Description = Left(CType(row.Content, TextSyndicationContent).Text, 500)
                        End If
                        insert.ChannelId = theChannel.ChannelId

                        insert.Link = row.Links.First.Uri.AbsoluteUri
                        insert.Block = False
                        insert.Precal = 0

                        'Story Location
                        If row.ElementExtensions.Where(Function(x) x.OuterName = "lat").Count > 0 And row.ElementExtensions.Where(Function(x) x.OuterName = "long").Count > 0 Then
                            insert.Latitude = Double.Parse(row.ElementExtensions.Where(Function(x) x.OuterName = "lat").First.GetObject(Of XElement).Value, New CultureInfo(""))
                            insert.Longitude = Double.Parse(row.ElementExtensions.Where(Function(x) x.OuterName = "long").First.GetObject(Of XElement).Value, New CultureInfo(""))
                        Else
                            insert.Latitude = theChannel.Latitude
                            insert.Longitude = theChannel.Longitude
                        End If
                        Try


                            If row.ElementExtensions.Where(Function(x) x.OuterName = "translationGroup").Count > 0 Then
                                insert.TranslationGroup = CInt(row.ElementExtensions.Where(Function(x) x.OuterName = "translationGroup").First.GetObject(Of XElement).Value)
                            End If




                            If row.ElementExtensions.Where(Function(x) x.OuterName = "language").Count > 0 Then
                                insert.Langauge = row.ElementExtensions.Where(Function(x) x.OuterName = "language").First.GetObject(Of XElement).Value

                            ElseIf theChannel.AutoDetectLanguage Then

                                Dim req = "https://www.googleapis.com/language/translate/v2/detect?key=AIzaSyBCSoev7-yyoFLIBOcsnbJqcNifaLwOnPc&q=" & HttpUtility.UrlEncode(Left(insert.Description, 80))

                                Dim web As New WebClient()

                                Dim response = web.DownloadString(req)
                                If response.IndexOf("""language"": """) > 0 Then
                                    Dim lang = response.Substring(response.IndexOf("""language"": """) + 13)
                                    If lang.IndexOf(""",") > 0 Then
                                        lang = lang.Substring(0, lang.IndexOf(""","))
                                        If lang.Length >= 2 Then
                                            insert.Langauge = Left(lang, 8)
                                        End If
                                    End If
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                        If insert.Langauge Is Nothing Then
                            insert.Langauge = theChannel.Language
                        End If
                        ' insert.TranslationGroup = row.Id



                        If Not row.Id Is Nothing Then
                            insert.GUID = Left(row.Id, 154)
                        End If


                        If row.PublishDate = Nothing Then
                            insert.StoryDate = Today
                        Else
                            insert.StoryDate = row.PublishDate.DateTime
                        End If

                        insert.Clicks = 1
                        SetImage(insert, row, theChannel.ImageId)

                        d.AP_Stories_Module_Channel_Caches.InsertOnSubmit(insert)
                    Else
                        If Not row.Title Is Nothing Then
                            existingStory.First.Headline = Left(row.Title.Text, 154)
                        End If
                        If Not row.Summary Is Nothing Then
                            existingStory.First.Description = Left(row.Summary.Text, 500)
                        ElseIf Not row.Content Is Nothing Then
                            existingStory.First.Description = Left(CType(row.Content, TextSyndicationContent).Text, 500)
                        End If


                        set_if(existingStory.First.StoryDate, row.PublishDate.DateTime)

                        SetImage(existingStory.First, row, theChannel.ImageId)

                        Try


                            If row.ElementExtensions.Where(Function(x) x.OuterName = "translationGroup").Count > 0 Then
                                existingStory.First.TranslationGroup = CInt(row.ElementExtensions.Where(Function(x) x.OuterName = "translationGroup").First.GetObject(Of XElement).Value)
                            End If

                        Catch ex As Exception

                        End Try


                    End If
                    d.SubmitChanges()
                Catch ex As Exception
                    'If a story wont load, just skip to the nect one..
                    Dim s = ex.ToString

                    StaffBrokerFunctions.EventLog("AddStoryToCache Failed", s, 1)
                End Try

            Next




        Catch ex As Exception
            StaffBrokerFunctions.EventLog("Refresh Cache Failed", ex.ToString(), 1)
        End Try



    End Sub

    Public Shared Sub PrecalAllCaches(ByVal TabModuleId As Integer)
        Dim mc As New DotNetNuke.Entities.Modules.ModuleController

        Dim tm = mc.GetTabModule(TabModuleId)
        Dim recentWeight As Double = 0

        If (tm.TabModuleSettings("WeightRecent") <> "") Then
            recentWeight = Double.Parse(tm.TabModuleSettings("WeightRecent"), New CultureInfo(""))
        End If
        'StaffBrokerFunctions.EventLog("RecencyWeight", recentWeight, 1)
        Dim d As New Stories.StoriesDataContext
        Dim allStories = From c In d.AP_Stories_Module_Channel_Caches Where c.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = TabModuleId
        For Each row In allStories
            row.Precal = Math.Sqrt(row.AP_Stories_Module_Channel.Weight) * ((GetRecencyScore(row.StoryDate) * recentWeight) + GetBoost(row.BoostDate) + 0.5) / 1.5
        Next

        d.SubmitChanges()
    End Sub

    Public Shared Function GetRecencyScore(ByVal theDate As Date?) As Double
        If theDate Is Nothing Then
            Return 0.3
        End If
        Dim age = DateDiff(DateInterval.Day, CDate(theDate), Today)
        Dim initialBoost = 1
        If age <= 7 Then
            'give an extra boost for the first week
            initialBoost = 2
        End If
        Return Math.Max(0, (1 - (CDbl(age) / 365.0)) * initialBoost)




    End Function

    Public Shared Function SetLogo(ByVal ChannelImage As String, ByVal Portalid As Integer) As Integer
        Try


            Dim req = WebRequest.Create(ChannelImage)
            Dim response = req.GetResponse
            Dim imageStream = response.GetResponseStream
            Dim theFolder As IFolderInfo
            If FolderManager.Instance.FolderExists(Portalid, "_imageCropper") Then
                theFolder = FolderManager.Instance.GetFolder(Portalid, "_imageCropper")
            Else
                theFolder = FolderManager.Instance.AddFolder(Portalid, "_imageCropper")
            End If
            Dim FileName = ChannelImage.Substring(ChannelImage.LastIndexOf("/"))

            Dim theFile = FileManager.Instance.AddFile(theFolder, StaffBrokerFunctions.CreateUniqueFileName(theFolder, ChannelImage.Substring(ChannelImage.LastIndexOf(".") + 1)), imageStream)

            Return theFile.FileId



        Catch ex As Exception
            Return -1
        End Try
    End Function

    Private Shared Sub SetImage(ByRef theField As Stories.AP_Stories_Module_Channel_Cache, ByVal theRow As SyndicationItem, ByVal ChannelImage As String)
        If theRow.ElementExtensions.Where(Function(x) x.OuterName = "thumbnail").Count > 0 Then
            'First look for a thumbnail in the rss feed element
            theField.ImageId = theRow.ElementExtensions.Where(Function(x) x.OuterName = "thumbnail").First.GetObject(Of XElement).Attribute("url").Value
            theField.ImageWidth = theRow.ElementExtensions.Where(Function(x) x.OuterName = "thumbnail").First.GetObject(Of XElement).Attribute("width").Value
            theField.ImageHeight = theRow.ElementExtensions.Where(Function(x) x.OuterName = "thumbnail").First.GetObject(Of XElement).Attribute("height").Value
        Else
            'Look for an OgImage
            'Try
            '    'Look For an og:Image
            '    Dim reqPage = HttpWebRequest.Create(theField.Link)
            '    reqPage.Method = WebRequestMethods.Http.Head
            '    reqPage.Timeout = 5000
            '    Dim responsePage = reqPage.GetResponse()
            '    theField.ImageId = responsePage.Headers("og:image")
            'Catch ex As Exception
            'End Try

        End If

        If String.IsNullOrEmpty(theField.ImageId) Then
            theField.ImageId = ChannelImage

        ElseIf (Not (theField.ImageWidth Is Nothing Or theField.ImageHeight Is Nothing)) And theField.ImageWidth < 50 Or theField.ImageId.Length > 250 Then
            theField.ImageId = ChannelImage
        End If
        If theField.ImageWidth Is Nothing Or theField.ImageHeight Is Nothing Then
            'lookupURL and get ImageSize
            Dim req = WebRequest.Create(theField.ImageId)
            Dim response = req.GetResponse
            Dim imageStream = response.GetResponseStream
            Dim theImage = System.Drawing.Image.FromStream(imageStream)
            imageStream.Close()
            theField.ImageWidth = theImage.Width
            theField.ImageHeight = theImage.Height

        End If

    End Sub

    Public Shared Function distance(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double) As Double

        Dim theta As Double = lon1 - lon2
        Dim dist As Double = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta))

        dist = Math.Acos(dist)
        dist = rad2deg(dist)
        dist = dist * 1.1515 * 60
        Return dist
        dist = Math.Max(200, dist) ' if distance /200 limit to 200

        '(should be a value between 0 and 2 ish)
        'Now convert to a weight 
        Dim w As Double = 1.0 - dist / 200
        Return (1.0 + w) / 2.0



    End Function

    Private Shared Function deg2rad(ByVal deg As Double) As Double
        Return (deg * Math.PI / 180.0)
    End Function

    Private Shared Function rad2deg(ByVal rad As Double) As Double
        Return rad / Math.PI * 180.0
    End Function

End Class

