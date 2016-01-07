Imports System.Linq
Imports Stories
Imports System.ServiceModel.Syndication
Imports DotNetNuke.Services.FileSystem

Partial Class DesktopModules_AgapeConnect_Stories_RSS
    Inherits System.Web.UI.Page

    Const maxItemsInfeed = 50

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim d As New StoriesDataContext
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim mc As New DotNetNuke.Entities.Modules.ModuleController

        Dim x = mc.GetModuleByDefinition(PS.PortalId, "acRotator")
        Dim Stories As IQueryable(Of Stories.AP_Story)

        If Not String.IsNullOrEmpty(Request.QueryString("channel")) Then
            Stories = From c In d.AP_Stories Where c.PortalID = PS.PortalId And c.IsVisible = True And c.TabModuleId = CInt(Request.QueryString("channel"))
                      Order By c.StoryDate Descending

        ElseIf Not String.IsNullOrEmpty(Request.QueryString("name")) Then
            Stories = From c In d.AP_Stories Where c.PortalID = PS.PortalId And c.IsVisible = True And c.TabModuleId = StoryFunctions.GetTabModuleId(Request.QueryString("name"))
                      Order By c.StoryDate Descending
        Else
            Stories = From c In d.AP_Stories Where c.PortalID = PS.PortalId And c.IsVisible = True
                      Order By c.StoryDate Descending
        End If



        Response.ContentType = "application/rss+xml"
        Dim myFeed As New SyndicationFeed()
        Dim channeltitle = ""
        If Stories.Count > 0 Then
            channeltitle = StoryFunctions.getChannelTitle(Stories.First.TabModuleId)
        End If

        myFeed.Title = TextSyndicationContent.CreatePlaintextContent(channeltitle)
        myFeed.Description = TextSyndicationContent.CreatePlaintextContent(PS.PortalName & "/" & channeltitle)
        myFeed.Links.Add(SyndicationLink.CreateAlternateLink(New Uri(NavigateURL().Replace("http://", "https://"))))
        myFeed.Links.Add(SyndicationLink.CreateSelfLink(New Uri(NavigateURL(x.TabID).Replace("http://", "https://"))))
        myFeed.Copyright = SyndicationContent.CreatePlaintextContent("Copyright " & PS.PortalName)
        myFeed.Language = PS.DefaultLanguage
        Dim myList As New List(Of SyndicationItem)

        For Each row In Stories.Take(maxItemsInfeed)
            Dim insert As New SyndicationItem

            insert.Title = TextSyndicationContent.CreatePlaintextContent(row.Headline)

            insert.Links.Add(New SyndicationLink(New Uri(NavigateURL(CInt(row.TabId)).Replace("en-us/", "").Replace("http://", "https://") & "?StoryId=" & row.StoryId)))
            Dim summary As String = ""
            If String.IsNullOrEmpty(row.TextSample) Then
                summary = Left(StoryFunctions.StripTags(row.StoryText), 500)
            Else
                summary = Left(StoryFunctions.StripTags(row.TextSample), 500)
            End If
            If (summary.IndexOf(".") > 0) Then
                summary = summary.Substring(0, summary.LastIndexOf(".") + 1)

            End If

            insert.Summary = TextSyndicationContent.CreatePlaintextContent(summary)

            insert.PublishDate = row.StoryDate
            Dim author As New SyndicationPerson
            author.Name = row.Author
            author.Email = ""
            insert.Authors.Add(author)
            myList.Add(insert)
            Dim thePhoto = FileManager.Instance.GetFile(row.PhotoId)
            Dim media As XNamespace = XNamespace.Get("http://www.w3.org/2003/01/media/wgs84_pos#")


            If Not thePhoto Is Nothing Then

                Dim photourl = FileManager.Instance.GetUrl(thePhoto)
                If photourl.StartsWith("/") Then
                    photourl = "https://" & PortalSettings.Current.PortalAlias.HTTPAlias & photourl
                ElseIf Not photourl.StartsWith("http") Then
                    photourl = "https://" & photourl
                End If

                insert.ElementExtensions.Add(New XElement(media + "thumbnail", New XAttribute(XNamespace.Xmlns + "media", "http://www.w3.org/2003/01/media/wgs84_pos#"), New XAttribute("url", photourl), New XAttribute("width", thePhoto.Width), New XAttribute("height", thePhoto.Height)))


                'insert.ElementExtensions.Add(New XElement("enclosure",
                '    New XElement("Key", New XAttribute("type", "image/jpeg")),
                '     New XElement("Value", New XAttribute("url", photourl))).CreateReader())
            End If
            Dim geo As XNamespace = XNamespace.Get("http://www.w3.org/2003/01/geo/wgs84_pos#")


            If Not row.Latitude Is Nothing Then
                insert.ElementExtensions.Add(New XElement(geo + "lat", New XAttribute(XNamespace.Xmlns + "geo", "http://www.w3.org/2003/01/geo/wgs84_pos#"), row.Latitude))
            End If
            If Not row.Latitude Is Nothing Then
                insert.ElementExtensions.Add(New XElement(geo + "long", New XAttribute(XNamespace.Xmlns + "geo", "http://www.w3.org/2003/01/geo/wgs84_pos#"), row.Longitude))
            End If


            Dim lang As XNamespace = XNamespace.Get("http://www.w3.org/2003/01/lang/wgs84_pos#")


            If Not row.Language Is Nothing Then
                insert.ElementExtensions.Add(New XElement(lang + "language", New XAttribute(XNamespace.Xmlns + "lang", "http://www.w3.org/2003/01/lang/wgs84_pos#"), row.Language))
            End If
            If Not row.TranslationGroup Is Nothing Then
                insert.ElementExtensions.Add(New XElement(lang + "translationGroup", New XAttribute(XNamespace.Xmlns + "lang", "http://www.w3.org/2003/01/lang/wgs84_pos#"), row.TranslationGroup))
            End If

            insert.Id = row.StoryId

        Next

        myFeed.Items = myList
        myFeed.ImageUrl = New Uri("https://" & PS.PortalAlias.HTTPAlias & PS.HomeDirectory & PS.LogoFile)
        Dim feedWriter = System.Xml.XmlWriter.Create(Response.OutputStream)

        Dim rssFormatter As New System.ServiceModel.Syndication.Rss20FeedFormatter(myFeed)

        rssFormatter.WriteTo(feedWriter)
        feedWriter.Close()

    End Sub
End Class
