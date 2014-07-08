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
Imports System.ServiceModel.Syndication

Namespace DotNetNuke.Modules.AgapeConnect.Stories


    Partial Class RSS
        Inherits Entities.Modules.PortalModuleBase

        Const maxItemsInfeed = 15
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim d As New StoriesDataContext
            Dim Stories = From c In d.AP_Stories Where c.PortalID = PortalId And c.TabId = TabId And c.IsVisible = True
                          Order By c.StoryDate Descending

            Response.ContentType = "application/rss+xml"
            Dim myFeed As New SyndicationFeed()
            myFeed.Title = TextSyndicationContent.CreatePlaintextContent(TabController.CurrentPage.TabName)
            myFeed.Description = TextSyndicationContent.CreatePlaintextContent("Latest news from " & PortalSettings.PortalName & "/" & TabController.CurrentPage.TabName)
            myFeed.Links.Add(SyndicationLink.CreateAlternateLink(New Uri(NavigateURL())))
            myFeed.Links.Add(SyndicationLink.CreateSelfLink(New Uri(EditUrl("RSS"))))
            myFeed.Copyright = SyndicationContent.CreatePlaintextContent("Copyright " & PortalSettings.PortalName)
            myFeed.Language = PortalSettings.DefaultLanguage
            Dim myList As New List(Of SyndicationItem)

            For Each row In Stories.Take(maxItemsInfeed)
                Dim insert As New SyndicationItem
                insert.Title = TextSyndicationContent.CreatePlaintextContent(row.Headline)
                insert.Links.Add(New SyndicationLink(New Uri(EditUrl("ViewStory") & "?StoryId" & row.StoryId)))
                insert.Summary = TextSyndicationContent.CreatePlaintextContent(Left(StoryFunctions.StripTags(row.StoryText), 500))
                insert.PublishDate = row.StoryDate
                Dim author As New SyndicationPerson
                author.Name = row.Author
                author.Email = ""
                insert.Authors.Add(author)
                myList.Add(insert)





            Next

            myFeed.Items = myList
            Dim feedWriter = System.Xml.XmlWriter.Create(Response.OutputStream)

            Dim rssFormatter As New Rss20FeedFormatter(myFeed)

         
            rssFormatter.WriteTo(feedWriter)
            feedWriter.Close()








        End Sub

       
    End Class
End Namespace