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



            Dim Stories = From c In d.AP_Stories Where c.PortalID = ModInfo.PortalID And c.TabId = ModInfo.TabID And c.IsVisible = True



            'From c In d.AP_Stories_Module_Channel_Caches Where c.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = ModInfo.TabModuleID()

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
    Public imageExtensions() As String = {"jpg", "jpeg", "gif", "png", "bmp"}
    Public noImage As String = "/images/no-content.png?"
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
    Public Shared Function GetTagsWithStories(ByVal TabModuleId As Integer) As IQueryable(Of AP_Stories_Tag)
        Dim d As New StoriesDataContext

        Dim tags = From cache In d.AP_Stories_Module_Channel_Caches _
                       Join st In d.AP_Stories On CInt(cache.GUID) Equals st.StoryId _
                       Join meta In d.AP_Stories_Tag_Metas On meta.StoryId Equals st.StoryId _
                       Join tag In d.AP_Stories_Tags On meta.StoryTagMetaId Equals tag.StoryTagId _
                       Where tag.StoryModuleId = StoryFunctions.GetStoryModule(TabModuleId).StoryModuleId _
                       Select tag Distinct Order By tag.TagName

        Return tags
    End Function

    Public Shared Function GetTag(ByVal tagId As Integer, ByVal TabModuleId As Integer) As AP_Stories_Tag
        Dim d As New StoriesDataContext
        Return (From c In d.AP_Stories_Tags Where c.StoryTagId = tagId).First
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
        d.AP_Stories_Tags.DeleteOnSubmit(tagToDelete)
        d.SubmitChanges()
    End Sub

    Public Shared Sub DeleteMetaTags(tagId As Integer, ByVal TabModuleId As Integer)
        Dim d As New StoriesDataContext
        Dim tag = (From c In d.AP_Stories_Tags Where c.StoryModuleId = GetStoryModule(TabModuleId).StoryModuleId And c.StoryTagId = CInt(tagId)).First
        If (tag IsNot Nothing) Then
            Dim metaTagsToDelete = From c In d.AP_Stories_Tag_Metas Where c.TagId = CInt(tagId)
            d.AP_Stories_Tag_Metas.DeleteAllOnSubmit(metaTagsToDelete)
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

#End Region 'Tags

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

    Public Shared Function getChannelTitle(ByVal TabModuleId As Integer) As String
        Dim d As New Stories.StoriesDataContext
        Try
            Dim smid = (From c In d.AP_Stories_Modules Where c.TabModuleId = TabModuleId).First.StoryModuleId

            Return (From c In d.AP_Stories_Module_Channels Where c.StoryModuleId = smid).First.ChannelTitle

        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Shared Function AddLocalChannel(ByVal tabModuleId As Integer, ByVal PortalAlias As String, ByVal Name As String, ByVal Longitude As Double, ByVal Latitude As Double, ByVal logo As String) As Integer
        Dim theModule = GetStoryModule(tabModuleId)

        Dim insert As New Stories.AP_Stories_Module_Channel
        insert.StoryModuleId = theModule.StoryModuleId
        insert.Weight = 1.0
        insert.Type = 2
        insert.URL = "https://" & PortalAlias & "/DesktopModules/AgapeConnect/Stories/Feed.aspx?channel=" & tabModuleId

        Name = Name

        insert.ChannelTitle = Name
        insert.Language = CultureInfo.CurrentCulture.Name
        insert.Latitude = Latitude
        insert.Longitude = Longitude

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

    Public Shared Sub PublishStory(ByVal StoryId As Integer)
        Dim d As New Stories.StoriesDataContext

        Dim theStory = From c In d.AP_Stories Where c.StoryId = StoryId

        If theStory.Count > 0 Then
            theStory.First.IsVisible = True
            d.SubmitChanges()

            'Refresh all stories that are listening to the current channel
            StoryFunctions.RefreshEverythingListeningToFeedAtTab(theStory.First.TabModuleId)



            'theStory.First.TabModuleId
        End If
    End Sub

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

    Public Shared Sub RefreshEverythingListeningToFeedAtTab(ByVal TabModuleId As Integer)
        Dim d As New Stories.StoriesDataContext

        Dim Channels = From c In d.AP_Stories_Module_Channels Where c.URL.EndsWith("channel=" & TabModuleId)

        For Each channel In Channels
            RefreshFeed(channel.AP_Stories_Module.TabModuleId, channel.ChannelId, False)
            PrecalAllCaches(channel.AP_Stories_Module.TabModuleId)
        Next
        StoryFunctions.RefreshLocalChannel(TabModuleId)
    End Sub

    Public Shared Function GetBoost(ByVal boostDate As Date?) As Double
        If boostDate Is Nothing Then
            Return 0
        Else
            Return IIf(boostDate >= Today, 3, 0)

        End If
    End Function

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

