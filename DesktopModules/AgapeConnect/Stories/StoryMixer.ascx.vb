Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq
Imports System.ServiceModel.Syndication
Imports System.Xml
Imports System.Net
Imports Stories
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Entities.Modules


Namespace DotNetNuke.Modules.Stories

    Partial Class StoryMixer
        Inherits Entities.Modules.ModuleSettingsBase

        Dim d As New StoriesDataContext
#Region "Base Method Implementations"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If (Page.IsPostBack = False) Then
                    LoadMixer()
                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Protected Sub LoadMixer()
            ddlLanguages.DataSource = From c In CultureInfo.GetCultures(CultureTypes.AllCultures) Order By c.EnglishName Select Name = c.Name.ToLower, EnglishName = c.EnglishName
            ddlLanguages.DataValueField = "Name"
            ddlLanguages.DataTextField = "EnglishName"
            ddlLanguages.DataBind()
            Dim theModule = StoryFunctions.GetStoryModule(TabModuleId)
            Dim objModules As New Entities.Modules.ModuleController
            Dim newSettings As Boolean = False

            If theModule.AP_Stories_Module_Channels.Where(Function(x) x.Type = 2).Count = 0 Then
                'add a local channel!
                Dim RssName As String = ""
                If CType(TabModuleSettings("RssName"), String) <> "" Then
                    RssName = TabModuleSettings("RssName")

                Else
                    RssName = ModuleConfiguration.ParentTab.TabName
                    objModules.UpdateTabModuleSetting(TabModuleId, "RssName", RssName)
                    newSettings = True
                End If
                Dim l = Location.GetLocation(Request.ServerVariables("remote_addr"))
                Dim logoFile = StoryFunctions.SetLogo("https://" & PortalSettings.PortalAlias.HTTPAlias & PortalSettings.HomeDirectory & PortalSettings.LogoFile, PortalId)

                Dim imageId = "https://" & PortalAlias.HTTPAlias & FileManager.Instance.GetUrl(FileManager.Instance.GetFile(logoFile))

                StoryFunctions.AddLocalChannel(TabModuleId, PortalAlias.HTTPAlias, RssName, l.longitude, l.latitude, imageId)

                theModule = StoryFunctions.GetStoryModule(TabModuleId)

            End If

            hfStoryModuleId.Value = theModule.StoryModuleId

            '  Dim channels = From c In d.AP_Stories_Module_Channels Where c.AP_Stories_Module.TabModuleId = TabModuleId

            Dim volumes = ""
            For Each row In theModule.AP_Stories_Module_Channels
                volumes &= row.ChannelId & "=" & (row.Weight * 30) & ";"
            Next

            hfLoadVolumes.Value = volumes

            If CType(TabModuleSettings("NumberOfStories"), String) <> "" Then
                hfNumberOfStories.Value = CType(TabModuleSettings("NumberOfStories"), Integer)
            Else
                hfNumberOfStories.Value = 20
                objModules.UpdateTabModuleSetting(TabModuleId, "NumberOfStories", 20)
                newSettings = True
            End If
            If CType(TabModuleSettings("WeightPopular"), String) <> "" Then
                hfPopular.Value = Double.Parse(TabModuleSettings("WeightPopular"), New CultureInfo("")) * 100
            Else
                hfPopular.Value = 75
                objModules.UpdateTabModuleSetting(TabModuleId, "WeightPopular", "0.75")
                newSettings = True
            End If

            If CType(TabModuleSettings("WeightRegional"), String) <> "" Then
                hfRegional.Value = Double.Parse(TabModuleSettings("WeightRegional"), New CultureInfo("")) * 100
            Else
                hfRegional.Value = 75
                objModules.UpdateTabModuleSetting(TabModuleId, "WeightRegional", "0.75")
                newSettings = True
            End If
            If CType(TabModuleSettings("WeightRecent"), String) <> "" Then
                hfRecent.Value = Double.Parse(TabModuleSettings("WeightRecent"), New CultureInfo("")) * 100
            Else
                hfRecent.Value = 75
                objModules.UpdateTabModuleSetting(TabModuleId, "WeightRecent", "0.75")
                newSettings = True
            End If
            dlChannelMixer.DataSource = theModule.AP_Stories_Module_Channels.OrderByDescending(Function(x) x.Type = 2).ThenBy(Function(x) x.ChannelId)
            dlChannelMixer.DataBind()

            PreviewResults()

            If newSettings Then
                'SynchronizeModule()
                ModuleController.SynchronizeModule(ModuleId)
            End If
        End Sub

#End Region
        Private Sub PreviewResults()
            Dim l As Location = Location.GetLocation(Request.ServerVariables("remote_addr"))

            Dim lg = l.longitude
            Dim lt = l.latitude

            Dim localFactor As Double = 1
            Dim deg2Rad As Double = Math.PI / CDbl(180.0)
            Dim G As Double = hfRegional.Value / 100
            Dim P As Double = hfPopular.Value / 100
            Dim N As Integer = hfNumberOfStories.Value

            Dim culture = CultureInfo.CurrentCulture.Name.ToLower
            Dim r = From c In d.AP_Stories_Module_Channel_Caches Select c, c.Clicks, Age = DateDiff(DateInterval.Day, c.StoryDate.Value, Today),
                  Distance = (0.0 + (1 * (CDbl(1.0) - CDbl(CDbl(Math.Min(CDbl(200), ((Math.Acos(CDbl(Math.Sin(CDbl(deg2Rad) * CDbl(lt))) * CDbl(Math.Sin(deg2Rad * CDbl(c.Latitude))) + CDbl(Math.Cos(CDbl(deg2Rad) * CDbl(lt))) * CDbl(Math.Cos(CDbl(deg2Rad) * CDbl(c.Latitude))) * CDbl(Math.Cos(CDbl(deg2Rad) * (CDbl(lg) - CDbl(c.Longitude)))))) / CDbl(Math.PI) * CDbl(180.0)) * CDbl(1.1515) * CDbl(60.0))) / CDbl(200.0)))) / CDbl(2.0)),
                  ViewOrder = CDbl(c.Precal) * (CDbl(1.0 + (Math.Log(c.Clicks) * P / 200))) * (1.0 + (G * (CDbl(1.0) - CDbl(CDbl(Math.Min(CDbl(200), ((Math.Acos(CDbl(Math.Sin(CDbl(deg2Rad) * CDbl(lt))) * CDbl(Math.Sin(deg2Rad * CDbl(c.Latitude))) + CDbl(Math.Cos(CDbl(deg2Rad) * CDbl(lt))) * CDbl(Math.Cos(CDbl(deg2Rad) * CDbl(c.Latitude))) * CDbl(Math.Cos(CDbl(deg2Rad) * (CDbl(lg) - CDbl(c.Longitude)))))) / CDbl(Math.PI) * CDbl(180.0)) * CDbl(1.1515) * CDbl(60.0))) / CDbl(200.0)))) / CDbl(2.0))

            If (Request.QueryString("tags") <> "") Then

                Dim tagList = Request.QueryString("tags").Split(",")

                r = From c In r Join b In d.AP_Stories On CInt(c.c.GUID) Equals b.StoryId Where b.AP_Stories_Tag_Metas.Where(Function(x) tagList.Contains(x.AP_Stories_Tag.TagName)).Count > 0 Select c

            End If

            r = r.Where(Function(c) CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower = c.c.Langauge.Substring(0, 2) And c.c.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = TabModuleId And Not c.c.Block) _
                            .OrderByDescending(Function(c) c.ViewOrder) _
                        .Take(N)
            Dim storyList = From c In r Select c.c.Headline, c.Clicks, c.Distance, c.Age, c.c.Precal, Score = c.ViewOrder

            gvPreview.DataSource = storyList
            gvPreview.DataBind()
        End Sub

        Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
            Dim objModules As New Entities.Modules.ModuleController

            ' objModules.UpdateTabModuleSetting(TabModuleId, "Aspect", Double.Parse(hfAspect.Value, New CultureInfo("")).ToString(New CultureInfo("")))

            objModules.UpdateTabModuleSetting(TabModuleId, "NumberOfStories", CInt(hfNumberOfStories.Value))
            objModules.UpdateTabModuleSetting(TabModuleId, "WeightPopular", CDbl(IIf(hfPopular.Value = "", 0, hfPopular.Value / 100)).ToString(New CultureInfo("")))
            objModules.UpdateTabModuleSetting(TabModuleId, "WeightRegional", CDbl(IIf(hfRegional.Value = "", 0, hfRegional.Value / 100)).ToString(New CultureInfo("")))
            objModules.UpdateTabModuleSetting(TabModuleId, "WeightRecent", CDbl(IIf(hfRecent.Value = "", 0, hfRecent.Value / 100)).ToString(New CultureInfo("")))

            'Save each channel weight:
            Dim x = hfVolumes.Value.Split(";")
            Dim Volumes As New Dictionary(Of Integer, Integer)
            For Each row In x
                If row.Contains("=") Then
                    Dim theSplit = row.Split("=")

                    Volumes.Add(theSplit(0), theSplit(1))
                End If
            Next

            Dim channels = From c In d.AP_Stories_Module_Channels Where c.StoryModuleId = CInt(hfStoryModuleId.Value)

            For Each row In channels
                row.Weight = CDbl(Volumes(row.ChannelId)) / 30
            Next

            Dim boosts = hfBoosts.Value.Split(";")
            For Each boost In boosts
                If boost <> "" Then
                    Dim theCache = (From c In d.AP_Stories_Module_Channel_Caches Where c.CacheId = CInt(boost))
                    If theCache.Count > 0 Then
                        theCache.First.Block = False
                        If theCache.First.BoostDate Is Nothing Then
                            theCache.First.BoostDate = Today.AddDays(StoryFunctions.GetBoostDuration(PortalId))
                        ElseIf theCache.First.BoostDate < Today Then
                            theCache.First.BoostDate = Today.AddDays(StoryFunctions.GetBoostDuration(PortalId))
                        End If
                    End If
                End If
            Next
            Dim blocks = hfBlocks.Value.Split(";")
            For Each block In blocks
                If block <> "" Then
                    Dim theCache = (From c In d.AP_Stories_Module_Channel_Caches Where c.CacheId = CInt(block))
                    If theCache.Count > 0 Then

                        StoryFunctions.BlockStoryAccrossSite(theCache.First.Link)

                    End If
                End If
            Next

            Dim PrevioslyBoosted = From c In d.AP_Stories_Module_Channel_Caches Where c.AP_Stories_Module_Channel.StoryModuleId = CInt(hfStoryModuleId.Value) And (Not c.BoostDate Is Nothing) And (Not boosts.Contains(c.CacheId))

            For Each row In PrevioslyBoosted
                row.BoostDate = Nothing
            Next
            Dim PrevioslyBlocked = From c In d.AP_Stories_Module_Channel_Caches Where c.AP_Stories_Module_Channel.StoryModuleId = CInt(hfStoryModuleId.Value) And (c.Block = True) And (Not blocks.Contains(c.CacheId))

            For Each row In PrevioslyBlocked
                StoryFunctions.UnBlockStoryAccrossSite(row.Link)
            Next

            d.SubmitChanges()

            ' refresh cache
            'SynchronizeModule()
            ModuleController.SynchronizeModule(ModuleId)

            StoryFunctions.PrecalAllCaches(TabModuleId)
            LoadMixer()
        End Sub

        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub btnCache_Click(sender As Object, e As System.EventArgs) Handles btnCache.Click
            ' StoryFunctions.RefreshFeed(1)
            SaveBtn_Click(Me, Nothing)
            Dim theMod = StoryFunctions.GetStoryModule(TabModuleId)
            For Each channel In theMod.AP_Stories_Module_Channels
                StoryFunctions.RefreshFeed(TabModuleId, channel.ChannelId, True)
            Next
            SaveBtn_Click(Me, Nothing)
        End Sub

        Protected Sub lbVerifyURL_Click(sender As Object, e As System.EventArgs) Handles lbVerifyURL.Click
            Try
                Dim q = From c In d.AP_Stories_Module_Channels Where c.URL = tbRssFeed.Text And c.StoryModuleId = CInt(hfStoryModuleId.Value)
                If q.Count > 0 And btnEditChannel.Visible = False Then
                    lblFeedError.Text = "Error: This feed already exists. Please select a new feed."
                    Return
                End If

                Dim reader = XmlReader.Create(tbRssFeed.Text)
                Dim feed = SyndicationFeed.Load(reader)

                If Not feed.BaseUri Is Nothing Then
                    tbRssFeed.Text = feed.BaseUri.AbsoluteUri
                End If

                tbTitle.Text = feed.Title.Text

                If Not String.IsNullOrEmpty(feed.Language) Then
                    Dim search = From c In CultureInfo.GetCultures(CultureTypes.AllCultures) Where c.Name.ToLower = feed.Language.ToLower
                    If search.Count = 0 Then
                        search = From c In CultureInfo.GetCultures(CultureTypes.AllCultures) Where c.TwoLetterISOLanguageName.ToLower = feed.Language.ToLower
                    End If

                    If search.Count <> 0 Then
                        ddlLanguages.SelectedValue = search.First.Name.ToLower
                    Else
                        ddlLanguages.Items.Add(New ListItem(feed.Language, feed.Language))

                    End If
                Else
                    ddlLanguages.SelectedValue = "en"
                End If
                cbAutoDetectLanguage.Checked = False
                Try
                    Dim simpleURL = tbRssFeed.Text.Replace("http://", "").Replace("https://", "")
                    If simpleURL.IndexOf("/") > 0 Then
                        simpleURL = simpleURL.Substring(0, simpleURL.IndexOf("/"))
                    End If

                    Dim ls As New LookupService(Server.MapPath("~/App_Data/GeoLiteCity.dat"), LookupService.GEOIP_STANDARD)
                    Dim ip = System.Net.Dns.GetHostAddresses(simpleURL)
                    Dim l As Location = ls.getLocation(ip.First)
                    tbLocation.Text = l.latitude.ToString(New CultureInfo("")) & ", " & l.longitude.ToString(New CultureInfo(""))
                Catch ex As Exception

                End Try
                If CType(TabModuleSettings("Aspect"), String) <> "" Then
                    icImage.Aspect = CDbl(TabModuleSettings("Aspect")).ToString(New CultureInfo(""))

                Else
                    icImage.Aspect = "1.0"
                End If
                'icImage.Aspect = lblAspect.Text
                If Not feed.ImageUrl Is Nothing Then
                    icImage.FileId = StoryFunctions.SetLogo(feed.ImageUrl.AbsoluteUri, PortalId)

                Else
                    Dim logo = FileManager.Instance.GetFile(PortalId, PortalSettings.LogoFile)
                    icImage.FileId = StoryFunctions.SetLogo("https://" & PortalSettings.DefaultPortalAlias & FileManager.Instance.GetUrl(FileManager.Instance.GetFile(logo.FileId)), PortalId)

                End If
                'icImage.Aspect = lblAspect.Text
                icImage.LazyLoad(True)
                'pnlloaded.Visible = True
                btnAddChannel.Enabled = True

                lblFeedError.Text = ""
                tbRssFeed.Enabled = True

            Catch ex As Exception
                lblFeedError.Text = "Error: There was a problem loading your feed"

            End Try
        End Sub

        'Private Function SetImage(ByVal ChannelImage As String) As Integer
        '    Try

        '        Dim req = WebRequest.Create(ChannelImage)
        '        Dim response = req.GetResponse
        '        Dim imageStream = response.GetResponseStream
        '        Dim theFolder As IFolderInfo
        '        If FolderManager.Instance.FolderExists(PortalId, "_imageCropper") Then
        '            theFolder = FolderManager.Instance.GetFolder(PortalId, "_imageCropper")
        '        Else
        '            theFolder = FolderManager.Instance.AddFolder(PortalId, "_imageCropper")
        '        End If
        '        Dim FileName = ChannelImage.Substring(ChannelImage.LastIndexOf("/"))

        '        Dim theFile = FileManager.Instance.AddFile(theFolder, StaffBrokerFunctions.CreateUniqueFileName(theFolder, ChannelImage.Substring(ChannelImage.LastIndexOf(".") + 1)), imageStream)

        '        Return theFile.FileId

        '    Catch ex As Exception
        '        Return -1
        '    End Try
        'End Function

        Protected Sub btnAddChannel_Click(sender As Object, e As System.EventArgs) Handles btnAddChannel.Click
            'Validations
            lblFeedError.Text = ""
            If tbRssFeed.Text = "" Then
                lblFeedError.Text = "No feed found!<br />"
            End If

            Dim check = From c In d.AP_Stories_Module_Channels Where c.StoryModuleId = CInt(hfStoryModuleId.Value) And c.URL = tbRssFeed.Text
            If check.Count > 0 Then
                lblFeedError.Text = "This feed is already loaded.<br />"
            End If

            Dim insert As New AP_Stories_Module_Channel
            insert.StoryModuleId = CInt(hfStoryModuleId.Value)
            insert.Weight = 0.8
            insert.Type = 0
            insert.URL = tbRssFeed.Text

            If tbTitle.Text = "" Then
                lblFeedError.Text = "Please enter a title for this feed.<br />"
            End If
            insert.ChannelTitle = tbTitle.Text
            insert.Language = ddlLanguages.SelectedValue
            insert.AutoDetectLanguage = cbAutoDetectLanguage.Checked
            Dim geoLoc = tbLocation.Text.Split(",")
            If geoLoc.Count <> 2 Then
                lblFeedError.Text = "Invalid location. Please click search, to convert into latitude/longitude<br />"
            Else
                Try
                    insert.Latitude = Double.Parse(geoLoc(0).Replace(" ", ""), New CultureInfo(""))
                    insert.Longitude = Double.Parse(geoLoc(1).Replace(" ", ""), New CultureInfo(""))
                Catch ex As Exception
                    lblFeedError.Text = "Invalid location. Please click search, to convert into latitude/longitude<br />"
                End Try
            End If

            insert.ImageId = "https://" & PortalSettings.PortalAlias.HTTPAlias & FileManager.Instance.GetUrl(FileManager.Instance.GetFile(icImage.FileId))

            If lblFeedError.Text <> "" Then
                Dim t As Type = icImage.GetType()
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb.Append("<script language='javascript'>")
                sb.Append("$(document).ready(function() { $(""#AddChannel"").dialog(""open"");});")
                sb.Append("</script>")
                ScriptManager.RegisterStartupScript(icImage, t, "thePopup", sb.ToString, False)
                Return
            End If

            d.AP_Stories_Module_Channels.InsertOnSubmit(insert)
            d.SubmitChanges()

            StoryFunctions.RefreshFeed(TabModuleId, insert.ChannelId, False)
            Dim d2 As New StoriesDataContext
            Dim channels = From c In d2.AP_Stories_Module_Channels Where c.StoryModuleId = CInt(hfStoryModuleId.Value)

            dlChannelMixer.DataSource = channels.OrderByDescending(Function(x) x.Type = 2).ThenBy(Function(x) x.ChannelId)
            dlChannelMixer.DataBind()

            tbRssFeed.Text = ""
            tbRssFeed.Enabled = True
            tbLocation.Text = ""
            icImage.FileId = 0
            tbTitle.Text = ""
            'pnlloaded.Visible = False
            btnAddChannel.Enabled = False

            Dim volumes = ""
            For Each row In channels
                volumes &= row.ChannelId & "=" & (row.Weight * 30).ToString(New CultureInfo("")) & ";"
            Next

            hfLoadVolumes.Value = volumes

            'Dim t2 As Type = Page.GetType()
            'Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
            'sb2.Append("<script language='javascript'>")
            'sb2.Append("")
            'sb2.Append("</script>")
            'ScriptManager.RegisterStartupScript(Page, t2, "theDials", sb2.ToString, False)

        End Sub

        Protected Sub dlChannelMixer_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlChannelMixer.ItemCommand
            If e.CommandName = "DeleteChannel" Then

                Dim q = From c In d.AP_Stories_Module_Channels Where c.ChannelId = CInt(e.CommandArgument)

                d.AP_Stories_Module_Channels.DeleteAllOnSubmit(q)
                Dim r = From c In d.AP_Stories_Module_Channel_Caches Where c.ChannelId = CInt(e.CommandArgument)

                d.AP_Stories_Module_Channel_Caches.DeleteAllOnSubmit(r)

                d.SubmitChanges()
                Dim channels = From c In d.AP_Stories_Module_Channels Where c.StoryModuleId = CInt(hfStoryModuleId.Value)

                dlChannelMixer.DataSource = channels.OrderByDescending(Function(x) x.Type = 2).ThenBy(Function(x) x.ChannelId)
                dlChannelMixer.DataBind()
            ElseIf e.CommandName = "EditChannel" Then
                Dim q = From c In d.AP_Stories_Module_Channels Where c.ChannelId = CInt(e.CommandArgument)
                If q.Count > 0 Then
                    tbRssFeed.Text = q.First.URL
                    tbTitle.Text = q.First.ChannelTitle
                    tbLocation.Text = q.First.Latitude.Value.ToString(New CultureInfo("")) & ", " & q.First.Longitude.Value.ToString(New CultureInfo(""))
                    ddlLanguages.SelectedValue = q.First.Language.ToLower
                    cbAutoDetectLanguage.Checked = q.First.AutoDetectLanguage
                    Try
                        icImage.FileId = FileManager.Instance.GetFile(PortalId, "_imageCropper/" & q.First.ImageId.Substring(q.First.ImageId.LastIndexOf("/") + 1)).FileId
                        If CType(TabModuleSettings("Aspect"), String) <> "" Then
                            icImage.Aspect = CDbl(TabModuleSettings("Aspect")).ToString(New CultureInfo(""))

                        Else
                            icImage.Aspect = "1.0"
                        End If
                        icImage.LazyLoad(True)
                    Catch ex As Exception

                    End Try

                    'pnlloaded.Visible = True
                    btnAddChannel.Enabled = True

                    lblFeedError.Text = ""
                    tbRssFeed.Enabled = True

                    btnAddChannel.Visible = False
                    btnEditChannel.Visible = True
                    tbRssFeed.Enabled = False

                    'Need to re popup the form.

                    Dim t As Type = Page.GetType()
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script language='javascript'>")
                    sb.Append("$(document).ready(function() { showPopup();});")
                    sb.Append("</script>")
                    ScriptManager.RegisterStartupScript(Page, t, "thePopup2", sb.ToString, False)

                End If

            End If
        End Sub

        Public Function IsBoosted(ByVal CacheId As String, ByVal Boosted As Date?) As String
            If Boosted Is Nothing Then
                Return "/DesktopModules/AgapeConnect/Stories/images/thumb_up_off.png"
            ElseIf (Boosted >= Today) Then
                If Not hfBoosts.Value.Contains(";" & CacheId & ";") Then
                    hfBoosts.Value &= CacheId & ";"
                End If

                Return "/DesktopModules/AgapeConnect/Stories/images/thumb_up.png"

            Else
                Return "/DesktopModules/AgapeConnect/Stories/images/thumb_up_off.png"
            End If

        End Function

        Public Function IsBlocked(ByVal CacheId As String, ByVal Blocked As Boolean?) As String
            If Blocked Is Nothing Then
                Return "/DesktopModules/AgapeConnect/Stories/images/thumb_down_off.png"
            ElseIf Blocked Then
                If Not hfBlocks.Value.Contains(";" & CacheId & ";") Then
                    hfBlocks.Value &= CacheId & ";"
                End If
                Return "/DesktopModules/AgapeConnect/Stories/images/thumb_down.png"
            Else
                Return "/DesktopModules/AgapeConnect/Stories/images/thumb_down_off.png"
            End If

        End Function

        Public Function GetCache(ByVal Caches As System.Data.Linq.EntitySet(Of AP_Stories_Module_Channel_Cache)) As IQueryable(Of AP_Stories_Module_Channel_Cache)

            Dim q = From c In Caches.AsQueryable Order By (CDbl(c.Precal) * (1.0 + (CDbl(c.Clicks) * CDbl(CDbl(hfPopular.Value) / 100)))) Descending
            Return q.AsQueryable ' limit to 30 stories.
        End Function

        Protected Sub btnAddCancel_Click(sender As Object, e As System.EventArgs) Handles btnAddCancel.Click
            tbRssFeed.Enabled = True
            tbRssFeed.Text = ""
            tbLocation.Text = ""
            icImage.FileId = 0
            tbTitle.Text = ""
            'pnlloaded.Visible = False
            btnAddChannel.Enabled = False
            btnAddChannel.Visible = True
            btnEditChannel.Visible = False
        End Sub

        Protected Sub icImage_Uploaded() Handles icImage.Uploaded
            'Need to re popup the form.

            If CType(TabModuleSettings("Aspect"), String) <> "" Then
                icImage.Aspect = TabModuleSettings("Aspect")

            Else
                icImage.Aspect = "1.0"
            End If
            icImage.LazyLoad(True)
            Dim t As Type = icImage.GetType()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script language='javascript'>")
            sb.Append("$(document).ready(function() { $(""#AddChannel"").dialog(""open"");});")
            sb.Append("</script>")
            ScriptManager.RegisterStartupScript(icImage, t, "thePopup", sb.ToString, False)

        End Sub

        Protected Sub btnEditChannel_Click(sender As Object, e As System.EventArgs) Handles btnEditChannel.Click
            'Validations
            lblFeedError.Text = ""
            If tbRssFeed.Text = "" Then
                lblFeedError.Text = "No feed found!<br />"
            End If
            Dim theChannel = From c In d.AP_Stories_Module_Channels Where c.StoryModuleId = CInt(hfStoryModuleId.Value) And c.URL = tbRssFeed.Text
            If theChannel.Count > 0 Then

                If tbTitle.Text = "" Then
                    lblFeedError.Text = "Please enter a title for this feed.<br />"
                End If
                theChannel.First.ChannelTitle = tbTitle.Text
                theChannel.First.Language = ddlLanguages.SelectedValue
                theChannel.First.AutoDetectLanguage = cbAutoDetectLanguage.Checked
                Dim geoLoc = tbLocation.Text.Split(",")
                If geoLoc.Count <> 2 Then
                    lblFeedError.Text = "Invalid location. Please click search, to convert into latitude/longitude<br />"
                Else

                    Try
                        theChannel.First.Latitude = Double.Parse(geoLoc(0).Replace(" ", ""), New CultureInfo(""))
                        theChannel.First.Longitude = Double.Parse(geoLoc(1).Replace(" ", ""), New CultureInfo(""))
                    Catch ex As Exception
                        lblFeedError.Text = "Invalid location. Please click search, to convert into latitude/longitude<br />"
                    End Try
                End If

                theChannel.First.ImageId = "https://" & PortalSettings.PortalAlias.HTTPAlias & FileManager.Instance.GetUrl(FileManager.Instance.GetFile(icImage.FileId))

                If lblFeedError.Text <> "" Then
                    Dim t As Type = icImage.GetType()
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("<script language='javascript'>")
                    sb.Append("$(document).ready(function() { $(""#AddChannel"").dialog(""open"");});")
                    sb.Append("</script>")
                    ScriptManager.RegisterStartupScript(icImage, t, "thePopup", sb.ToString, False)
                    Return
                End If

                d.SubmitChanges()

                StoryFunctions.RefreshFeed(TabModuleId, theChannel.First.ChannelId, False)
                Dim d2 As New StoriesDataContext
                Dim channels = From c In d2.AP_Stories_Module_Channels Where c.StoryModuleId = CInt(hfStoryModuleId.Value)

                dlChannelMixer.DataSource = channels.OrderByDescending(Function(c) c.Type = 2).ThenBy(Function(c) c.ChannelId)
                dlChannelMixer.DataBind()

            End If

            tbRssFeed.Text = ""
            tbRssFeed.Enabled = True
            tbLocation.Text = ""
            icImage.FileId = 0
            tbTitle.Text = ""
            'pnlloaded.Visible = False
            btnAddChannel.Enabled = False
            btnAddChannel.Visible = True
            btnEditChannel.Visible = False
        End Sub
    End Class

End Namespace