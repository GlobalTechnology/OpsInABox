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

    Partial Class AddEditStory
        Inherits Entities.Modules.PortalModuleBase

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim d As New StoriesDataContext
            Dim q = From chan In d.AP_Stories_Module_Channels Join smod In d.AP_Stories_Modules On chan.StoryModuleId Equals smod.StoryModuleId Where smod.TabModuleId = TabModuleId And chan.Type = 2 Select chan.ChannelTitle
            lblChannel.Text = q.First

            If Not Page.IsPostBack Then
                If Settings("Aspect") <> "" Then
                    acImage1.Aspect = Double.Parse(Settings("Aspect"), New CultureInfo(""))
                End If
                lblSample.Style.Add("Display", "none")

                Dim mc As New DotNetNuke.Entities.Modules.ModuleController

                Dim dtp = DotNetNuke.Entities.Modules.DesktopModuleController.GetDesktopModuleByFriendlyName(Me.ModuleConfiguration.DesktopModule.FriendlyName).DesktopModuleID
                'Dim allTabs = mc.GetModulesByDefinition(PortalId, Me.ModuleConfiguration.DesktopModule.FriendlyName)
                'Dim allTabs = mc.GetAllTabsModulesByModuleID(ModuleId)
                Dim allTabs As New ArrayList()

                Dim tabs = (New TabController).GetTabsByPortal(PortalId)
                For Each row In tabs

                    Dim mods = mc.GetTabModules(row.Value.TabID)
                    For Each row2 In mods
                        If row2.Value.DesktopModuleID = dtp Then
                            allTabs.Add(row2.Value)
                        End If
                    Next
                Next

                    StaffBrokerFunctions.EventLog("TabCount", allTabs.Count, UserId)

                    ddlLanguage.DataSource = From c In CultureInfo.GetCultures(CultureTypes.SpecificCultures) Order By c.EnglishName Select Name = c.Name.ToLower, EnglishName = c.EnglishName
                    ddlLanguage.DataValueField = "Name"
                    ddlLanguage.DataTextField = "EnglishName"
                    ddlLanguage.DataBind()



                Dim tags = From c In d.AP_Stories_Tags Where c.PortalId = PortalId

                cblTags.DataSource = d.AP_Stories_Tags.Where(Function(c) c.PortalId = PortalId)
                cblTags.DataTextField = "TagName"
                cblTags.DataValueField = "StoryTagId"
                cblTags.DataBind()

                

                If Me.UserInfo.IsSuperUser And IsEditable() Then
                        'SuperPowers.Visible = True
                    End If
                    PagePanel.Visible = True
                    NotFoundLabel.Visible = False

                    Dim authorTitle = StaffBrokerFunctions.GetSetting("Authors", Me.PortalId)
                    If authorTitle <> "" Then
                        Dim rc As New DotNetNuke.Security.Roles.RoleController()
                        Dim theseAuthors = rc.GetUsersByRoleName(Me.PortalId, authorTitle)
                        ddlAuthor.DataTextField = "DisplayName"
                        ddlAuthor.DataValueField = "UserID"
                        ddlAuthor.DataSource = theseAuthors
                        ddlAuthor.DataBind()
                        ddlAuthor.Visible = True
                        Author.Visible = False
                        tbField3.Visible = False
                        lblField3.Visible = False
                    Else
                        ddlAuthor.Visible = False
                        Author.Visible = True
                        tbField3.Visible = True
                        lblField3.Visible = True
                    End If

                    If Request.QueryString("StoryID") <> "" Then

                        StoryIdHF.Value = Request.QueryString("StoryId")

                        Dim r = (From c In d.AP_Stories Where c.StoryId = Request.QueryString("StoryID")).First



                    Headline.Text = r.Headline
                    ' imgbtnPrint.OnClientClick = "window.open('/DesktopModules/FullStory/PrintStory.aspx?StoryId=" & Request.QueryString("StoryId") & "', '_blank'); "
                    'If Me.UserInfo.IsSuperUser Then
                    '    If Not Page.IsPostBack Then
                    '        Dim BoostDate As String
                    '        If r.EditorBoost <= Date.Now Then
                    '            BoostLabel.Text = "Not currently boosted."
                    '        Else
                    '            BoostDate = r.EditorBoost.Value.ToShortDateString()
                    '            BoostLabel.Text = "Boosted until " & BoostDate
                    '        End If
                    '        Editable.Checked = r.Editable
                    '    End If
                    'End If

                    StoryText.Text = r.StoryText

                    tbLocation.Text = r.Latitude.Value.ToString(New CultureInfo("")) & ", " & r.Longitude.Value.ToString(New CultureInfo(""))

                    StoryDate.Text = r.StoryDate.ToShortDateString

                    tbKeywords.Text = r.Keywords
                    For Each row As ListItem In cblTags.Items

                        row.Selected = r.AP_Stories_Tag_Metas.Where(Function(c) c.AP_Stories_Tag.StoryTagId = CInt(row.Value)).Count > 0

                    Next

                    If (Not String.IsNullOrEmpty(r.Field1)) Then
                        tbField1.Text = r.Field1
                    End If
                    If (Not String.IsNullOrEmpty(r.Field2)) Then
                        tbField2.Text = r.Field2
                    End If
                    If Not (authorTitle <> "") Then
                        Author.Text = r.Author
                        If (Not String.IsNullOrEmpty(r.Field3)) Then
                            tbField3.Text = r.Field3
                        End If
                    End If
                    If (Not String.IsNullOrEmpty(r.TextSample)) Then
                        tbSample.Text = r.TextSample
                    End If
                    If (Not String.IsNullOrEmpty(r.Subtitle)) Then
                        Subtitle.Text = r.Subtitle
                    End If

                    ' Dim thePhoto = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(r.PhotoId)
                    '  StoryImage.ImageUrl = DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(thePhoto)
                    acImage1.FileId = r.PhotoId
                    PhotoIdHF.Value = r.PhotoId
                    If (From c As ListItem In ddlLanguage.Items Where c.Value = r.Language).Count > 0 Then
                        ddlLanguage.SelectedValue = r.Language
                    Else
                        ddlLanguage.SelectedValue = CultureInfo.CurrentCulture.Name.ToLower
                    End If
                    pnlLanguages.Visible = False
                    If Not (r.TranslationGroup Is Nothing) Then

                        Dim Translist = From c In d.AP_Stories Where c.TranslationGroup = r.TranslationGroup And c.PortalID = r.PortalID And c.StoryId <> r.StoryId Select c.Language, c.StoryId

                        If Translist.Count > 0 Then
                            pnlLanguages.Visible = True
                            dlLanuages.DataSource = Translist
                            dlLanuages.DataBind()

                        End If

                    End If

                Else

                    If String.IsNullOrEmpty(Session("Long")) Or String.IsNullOrEmpty(Session("Lat")) Then
                        Dim ls As New LookupService(Server.MapPath("~/App_Data/GeoLiteCity.dat"), LookupService.GEOIP_STANDARD)
                        ' Dim l As Location = ls.getRegion(Request.ServerVariables("remote_addr"))

                        Dim l As Location = ls.getLocation("80.193.180.102")   '(Solihill)
                        Session("Long") = l.longitude
                        Session("Lat") = l.latitude
                    End If
                    If Request.QueryString("tg") <> "" Then
                        pnlLanguages.Visible = False

                        Dim Translist = From c In d.AP_Stories Where c.TranslationGroup = CInt(Request.QueryString("tg")) And c.PortalID = PortalId Select c.Language, c.StoryId

                        If Translist.Count > 0 Then
                            pnlLanguages.Visible = True
                            dlLanuages.DataSource = Translist
                            dlLanuages.DataBind()

                        End If

                    End If

                    Dim lg As Double = Session("Long")
                    Dim lt As Double = Session("Lat")
                    tbLocation.Text = lt.ToString(New CultureInfo("")) & ", " & lg.ToString(New CultureInfo(""))

                    StoryDate.Text = Today.ToString("dd MMM yyyy")
                    StoryText.Text = "Enter your news here..."

                    Author.Text = UserInfo.DisplayName

                    'Headline.Text = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
                    ddlLanguage.SelectedValue = CultureInfo.CurrentCulture.Name.ToLower

                End If

            End If

        End Sub

        Public Function GetLanguageName(ByVal language As String) As String

            Dim thename = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(Function(x) x.Name.ToLower = language.ToLower).Select(Function(x) x.DisplayName)
            If thename.Count > 0 Then
                Return thename.First()
            Else
                Return ""
            End If
        End Function
        Public Function GetDateFormat() As String
            Dim sdp As String = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower
            If sdp.IndexOf("d") < sdp.IndexOf("m") Then
                Return "dd/mm/yy"
            Else
                Return "mm/dd/yy"
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

        Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
            Dim d As New StoriesDataContext

            Dim authorTitle = StaffBrokerFunctions.GetSetting("Authors", Me.PortalId)

            Dim q = From c In d.AP_Stories Where c.StoryId = Request.QueryString("StoryId")
            Dim sd As Date = DateTime.Parse(StoryDate.Text, CultureInfo.CurrentCulture)
            If q.Count > 0 Then
                q.First.TabModuleId = CInt(TabModuleId)
                'TODO if selected is different than original tabmoduleid, then we need to update the cache as well...

                q.First.StoryText = StoryText.Text
                q.First.Headline = Headline.Text
                q.First.StoryDate = sd
                If authorTitle <> "" Then
                    q.First.Author = ddlAuthor.SelectedItem.Text
                    q.First.Field3 = "#selAuth#" & ddlAuthor.SelectedItem.Value
                Else
                    q.First.Author = Author.Text
                    q.First.Field3 = tbField3.Text
                End If

                q.First.Subtitle = Subtitle.Text
                q.First.Field1 = tbField1.Text
                q.First.Field2 = tbField2.Text
                q.First.UpdatedDate = Today
                q.First.Keywords = tbKeywords.Text
                For Each row As ListItem In cblTags.Items
                    Dim cur = q.First.AP_Stories_Tag_Metas.Where(Function(c) CInt(c.TagId = row.Value))
                    If row.Selected Then

                        If cur.Count = 0 Then
                            Dim tm As New AP_Stories_Tag_Meta()
                            tm.StoryId = q.First.StoryId
                            tm.TagId = CInt(row.Value)
                            d.AP_Stories_Tag_Metas.InsertOnSubmit(tm)

                        End If
                    Else
                        If (cur.Count > 0) Then
                            d.AP_Stories_Tag_Metas.DeleteAllOnSubmit(cur)

                        End If
                    End If

                Next

                d.SubmitChanges()
                If cbAutoGenerate.Checked Then

                    q.First.TextSample = Left(StoryFunctions.StripTags(StoryText.Text), 200)

                Else
                    q.First.TextSample = tbSample.Text
                End If

                Try
                    Dim geoLoc = tbLocation.Text.Split(",")
                    If geoLoc.Count = 2 Then

                        q.First.Latitude = Double.Parse(geoLoc(0).Replace(" ", ""), New CultureInfo(""))
                        q.First.Longitude = Double.Parse(geoLoc(1).Replace(" ", ""), New CultureInfo(""))

                    End If
                Catch ex As Exception

                End Try

                If acImage1.CheckAspect() Then

                    q.First.PhotoId = acImage1.FileId
                    d.SubmitChanges()
                Else
                    Return
                End If

                StoryFunctions.RefreshLocalChannel(CInt(TabModuleId))

                ' Response.Redirect(EditUrl("ViewStory") & "?StoryId=" & Request.QueryString("StoryId"))
                If CType(Settings("ViewTab"), String) <> "" Then
                    If Settings("ViewTab") <> TabId Then
                        Response.Redirect(NavigateURL(CInt(Settings("ViewTab"))) & "?StoryId=" & Request.QueryString("StoryId") & "&origTabId=" & TabId & "&origModId=" & ModuleId)
                    Else
                        Response.Redirect(EditUrl("ViewStory") & "?StoryId=" & Request.QueryString("StoryId"))
                    End If
                Else
                    Response.Redirect(EditUrl("ViewStory") & "?StoryId=" & Request.QueryString("StoryId"))
                End If

            Else
                Dim insert As New AP_Story
                insert.Headline = Headline.Text
                If authorTitle <> "" Then
                    insert.Author = ddlAuthor.SelectedItem.Text
                    insert.Field3 = "#selAuth#" & ddlAuthor.SelectedItem.Value
                Else
                    insert.Author = Author.Text
                    insert.Field3 = tbField3.Text
                End If

                insert.Subtitle = Subtitle.Text
                insert.Field1 = tbField1.Text
                insert.Field2 = tbField2.Text

                If cbAutoGenerate.Checked Then

                    insert.TextSample = Left(StoryFunctions.StripTags(StoryText.Text), 200)

                Else
                    insert.TextSample = tbSample.Text
                End If

                If acImage1.CheckAspect() Then
                    insert.PhotoId = acImage1.FileId
                Else
                    Return
                End If

                insert.StoryDate = sd
                insert.StoryText = StoryText.Text
                insert.PortalID = PortalId
                insert.Sent = False
                insert.RegionId = 0
                insert.Editable = True
                insert.EditorBoost = Today
                insert.IsVisible = True
                insert.UserId = UserId
                insert.TabId = TabId
                insert.Language = ddlLanguage.SelectedValue
                insert.TabModuleId = CInt(TabModuleId)

                Dim mode As String = StaffBrokerFunctions.GetSetting("StoryPublishMode", PortalId)
                If Not String.IsNullOrEmpty(mode) Then
                    If mode = "Staged" Then
                        insert.IsVisible = False
                    End If
                End If

                insert.Keywords = tbKeywords.Text
                If Request.QueryString("tg") <> "" Then
                    insert.TranslationGroup = Request.QueryString("tg")
                End If

                Try
                    Dim geoLoc = tbLocation.Text.Split(",")
                    If geoLoc.Count = 2 Then

                        insert.Latitude = Double.Parse(geoLoc(0).Replace(" ", ""), New CultureInfo(""))
                        insert.Longitude = Double.Parse(geoLoc(1).Replace(" ", ""), New CultureInfo(""))

                    End If
                Catch ex As Exception

                End Try
                d.AP_Stories.InsertOnSubmit(insert)
                d.SubmitChanges()

                For Each row As ListItem In cblTags.Items
                    If row.Selected Then
                        Dim tm As New AP_Stories_Tag_Meta()
                        tm.StoryId = insert.StoryId
                        tm.TagId = CInt(row.Value)
                        d.AP_Stories_Tag_Metas.InsertOnSubmit(tm)

                    End If
                Next
                d.SubmitChanges()
                Dim feeds = From c In d.AP_Stories_Module_Channels Where c.URL.EndsWith("?channel=" & insert.TabModuleId)

                For Each row In feeds
                    StoryFunctions.RefreshFeed(row.AP_Stories_Module.TabModuleId, row.ChannelId)
                    StoryFunctions.PrecalAllCaches(row.AP_Stories_Module.TabModuleId)
                Next

                StoryFunctions.RefreshLocalChannel(CInt(TabModuleId))
                'Response.Redirect(EditUrl("ViewStory") & "?StoryId=" & insert.StoryId)

                If CType(Settings("ViewTab"), String) <> "" Then
                    If Settings("ViewTab") <> TabId Then
                        Response.Redirect(NavigateURL(CInt(Settings("ViewTab"))) & "?StoryId=" & insert.StoryId & "&origTabId=" & TabId & "&origModId=" & ModuleId)
                    Else
                        Response.Redirect(EditUrl("ViewStory") & "?StoryId=" & insert.StoryId)
                    End If
                Else
                    Response.Redirect(EditUrl("ViewStory") & "?StoryId=" & insert.StoryId)
                End If
            End If

        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
           
            If Request.QueryString("StoryId") = "" Then
                Response.Redirect(NavigateURL())
            Else
                If CType(Settings("ViewTab"), String) <> "" Then
                    If Settings("ViewTab") <> TabId Then
                        Response.Redirect(NavigateURL(CInt(Settings("ViewTab"))) & "?StoryId=" & Request.QueryString("StoryId") & "&origTabId=" & TabId & "&origModId=" & ModuleId)
                    Else
                        Response.Redirect(EditUrl("ViewStory") & "?StoryId=" & Request.QueryString("StoryId"))
                    End If
                Else
                    Response.Redirect(EditUrl("ViewStory") & "?StoryId=" & Request.QueryString("StoryId"))
                End If

            End If
        End Sub

        Protected Sub acImage1_Updated() Handles acImage1.Updated
            Dim d As New StoriesDataContext
            Dim q = From c In d.AP_Stories Where c.StoryId = Request.QueryString("StoryId")

            If q.Count > 0 Then

                If acImage1.CheckAspect() Then
                    PhotoIdHF.Value = acImage1.FileId
                    q.First.PhotoId = acImage1.FileId
                    d.SubmitChanges()
                    'If Settings("Aspect") <> "" Then
                    '    acImage1.Aspect = Settings("Aspect")
                    'End If
                    ' acImage1.LazyLoad()
                End If

            End If

        End Sub

        Protected Function Translate(ByVal input As String, inputLanguage As String) As String
            Dim HTML As String

            Dim lTo = Left(ddlLanguage.SelectedValue, 2)
            Dim URL As String = "https://www.googleapis.com/language/translate/v2?key=AIzaSyBCSoev7-yyoFLIBOcsnbJqcNifaLwOnPc&source=" & inputLanguage & "&target=" & lTo & "&q=" & Server.UrlEncode(Server.HtmlEncode(input))
            Dim Request As HttpWebRequest
            Dim Response As HttpWebResponse
            Dim Reader As StreamReader
            Try
                Request = HttpWebRequest.Create(URL)
                Response = Request.GetResponse

                Reader = New StreamReader(Response.GetResponseStream())

                HTML = Reader.ReadToEnd
                'StoryText.Text = HTML
                Dim translatedText = HTML.Substring(HTML.IndexOf("translatedText") + 18)

                translatedText = translatedText.Substring(0, translatedText.IndexOf(""""))

                Return Server.HtmlDecode(Server.UrlDecode(translatedText))
            Catch ex As Exception

            End Try

        End Function

        Protected Sub dlLanuages_ItemCommand(source As Object, e As DataListCommandEventArgs) Handles dlLanuages.ItemCommand
            If e.CommandName = "Translate" Then
                Dim d As New StoriesDataContext
                Dim fromStory = From c In d.AP_Stories Where c.StoryId = CInt(e.CommandArgument)

                If fromStory.Count > 0 Then
                    StoryText.Text = Translate(fromStory.First.StoryText, Left(fromStory.First.Language, 2))

                    Headline.Text = Translate(fromStory.First.Headline, Left(fromStory.First.Language, 2))

                    Subtitle.Text = Translate(fromStory.First.Subtitle, Left(fromStory.First.Language, 2))

                    tbSample.Text = Translate(fromStory.First.TextSample, Left(fromStory.First.Language, 2))

                End If
            End If
        End Sub
    End Class
End Namespace