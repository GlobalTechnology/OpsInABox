﻿Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Security
Imports StaffBroker
Imports StaffBrokerFunctions
Imports Stories
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Entities.Modules
Imports System.Reflection

Namespace DotNetNuke.Modules.AgapeConnect.Stories
    Partial Class ViewStories
        Inherits Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable

#Region "Class variables"

        Dim d As New StoriesDataContext
        Dim theControl As Object

#End Region 'Class variables

#Region "Base Method Implementations"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' Redirect to view requested story
            Dim RequestStoryURL As String = "?StoryId=" & Request.QueryString("StoryId") & "&origTabId=" & TabId & "&origModId=" & ModuleId

            'REDIRECTION
            'Verify that storyid is part of tabmoduleid (in case there are multiple story modules on one page).
            If Request.QueryString("StoryId") <> "" And
                    StoryFunctions.GetStory(Request.QueryString("StoryId")).TabModuleId = TabModuleId Then
                If String.IsNullOrEmpty(Settings("ViewTab")) Or Settings("ViewTab") = 0 Then
                    Response.Redirect(EditUrl("ViewStory") & RequestStoryURL)
                Else
                    Response.Redirect(NavigateURL(CInt(Settings("ViewTab"))) & RequestStoryURL)
                End If
            End If

            'Register a click for this story
            If Not String.IsNullOrEmpty(Request.Form("StoryLink")) Then
                StoryFunctions.UpdateClicks(Request.Form("StoryLink"))
            Else

                If Not Page.IsPostBack Then

                    Dim tagsQueryString As String = Request.QueryString(TAGS_KEYWORD)
                    Dim validTagQueryList As List(Of String) = StoryFunctions.ValidTagList(tagsQueryString, TabModuleId)
                    Dim cleanTagsQueryString As String = String.Join(",", validTagQueryList)

                    'If tags query string isn't empty and it is different from the clean query string
                    'make query string editable, remove tags query string and add clean tags query string.
                    If ((Not String.IsNullOrEmpty(tagsQueryString)) And (Not String.Equals(cleanTagsQueryString, tagsQueryString))) Then

                        Dim isreadonly As PropertyInfo = GetType(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance Or BindingFlags.NonPublic)

                        ' make collection editable
                        isreadonly.SetValue(Me.Request.QueryString, False, Nothing)

                        ' remove
                        Me.Request.QueryString.Remove(TAGS_KEYWORD)

                        ' add
                        Me.Request.QueryString.Add(TAGS_KEYWORD, cleanTagsQueryString)

                        ' make collection read only again
                        isreadonly.SetValue(Me.Request.QueryString, True, Nothing)
                    End If

                    'NO tag is chosen or NO valid tag is in query string
                    'Will show a list of stories or a list of tags
                    If (validTagQueryList.Count < 1) Then

                        'A tag list is viewable
                        If Not String.IsNullOrEmpty(Settings("TagListControlId")) Then
                            Dim control = StoryFunctions.GetStoryControlLocation(Settings("TagListControlId"))
                            LoadStoryControl(control.Location, validTagQueryList, True)

                        Else  'A tag list is not viewable

                            'Load default Display Type (first row in database) if none defined
                            If String.IsNullOrEmpty(Settings("StoryControlId")) Then
                                Dim control = StoryFunctions.GetStoryControlLocation(StoryFunctionsProperties.FIRST_CONTROL)
                                LoadStoryControl(control.Location, validTagQueryList, control.Type = 2)

                            Else  'Load Display Type defined in module settings
                                Dim control = StoryFunctions.GetStoryControlLocation(Settings("StoryControlId"))
                                LoadStoryControl(control.Location, validTagQueryList, control.Type = 2)
                            End If

                        End If

                    Else ' Show list of stories that correspond to the valid tag(s)

                        'Load default Display Type (first row in database) if none defined
                        If String.IsNullOrEmpty(Settings("StoryControlId")) Then
                            Dim control = StoryFunctions.GetStoryControlLocation(StoryFunctionsProperties.FIRST_CONTROL)
                            LoadStoryControl(control.Location, validTagQueryList, control.Type = 2)

                        Else  'Load Display Type defined in module settings
                            Dim control = StoryFunctions.GetStoryControlLocation(Settings("StoryControlId"))
                            LoadStoryControl(control.Location, validTagQueryList, control.Type = 2)

                            'Html Meta tags for social media for tags
                            Dim tag = StoryFunctions.GetTagByName(validTagQueryList.First, TabModuleId)

                            StoryFunctions.SetSocialMediaMetaTags(StoryFunctions.GetPhotoURL(tag.PhotoId),
                                                                       TabController.CurrentPage.TabName & " " & StoryFunctions.FormatTagsSelected(tagsQueryString),
                                                                       TabController.CurrentPage.FullUrl & TAGS_IN_URL & tagsQueryString,
                                                                       TabController.CurrentPage.Description,
                                                                       PortalSettings.PortalName,
                                                                       StaffBrokerFunctions.GetSetting("FacebookId", PortalSettings.PortalId),
                                                                       StoryFunctionsProperties.SOCIAL_MEDIA_ARTICLE,
                                                                       Page.Header.Controls)
                        End If
                    End If
                End If 'Page.IsPostBack
            End If 'Register a click for this story
        End Sub

#End Region 'Base Method Implementations

#Region "Helper Functions"

        Private Sub LoadStoryControl(ByVal URL As String, ByVal validTagQueryList As List(Of String), Optional IsList As Boolean = False)

            Dim l As Location = Location.GetLocation(Request.ServerVariables("remote_addr"))
            Dim lg = l.longitude
            Dim lt = l.latitude

            'Set 10 as default number of stories displayed
            If Settings("NumberOfStories") = "" Then
                Dim objModules As New ModuleController
                objModules.UpdateTabModuleSetting(TabModuleId, "NumberOfStories", 10)
                ModuleController.SynchronizeModule(ModuleId)
            End If

            Dim localFactor As Double = 1

            Dim deg2Rad As Double = Math.PI / CDbl(180.0)
            Dim G As Double = 0.8
            Dim P As Double = 0.8
            Dim N As Integer = Settings("NumberOfStories")
            If IsList Then
                N = 500
            End If

            If Settings("WeightRegional") <> "" Then
                G = Double.Parse(Settings("WeightRegional"), New CultureInfo(""))

            End If
            If Settings("WeightPopular") <> "" Then
                P = Double.Parse(Settings("WeightPopular"), New CultureInfo(""))
            End If

            Dim culture = CultureInfo.CurrentCulture.Name.ToLower

            Dim sortedChannelCache = From channelCache In d.AP_Stories_Module_Channel_Caches Select channelCache, ViewOrder = CDbl(channelCache.Precal) * (CDbl(1.0 + (Math.Log(channelCache.Clicks) * P / 200))) * (1.0 + (G * (CDbl(1.0) - CDbl(CDbl(Math.Min(CDbl(200), ((Math.Acos(CDbl(Math.Sin(CDbl(deg2Rad) * CDbl(lt))) * CDbl(Math.Sin(deg2Rad * CDbl(channelCache.Latitude))) + CDbl(Math.Cos(CDbl(deg2Rad) * CDbl(lt))) * CDbl(Math.Cos(CDbl(deg2Rad) * CDbl(channelCache.Latitude))) * CDbl(Math.Cos(CDbl(deg2Rad) * (CDbl(lg) - CDbl(channelCache.Longitude)))))) / CDbl(Math.PI) * CDbl(180.0)) * CDbl(1.1515) * CDbl(60.0))) / CDbl(200.0)))) / CDbl(2.0))

            If (validTagQueryList.Count > 0) Then

                sortedChannelCache = From c In sortedChannelCache
                                     Join stories In d.AP_Stories On CInt(c.channelCache.GUID) Equals stories.StoryId
                                     Where stories.AP_Stories_Tag_Metas.Where(Function(x) validTagQueryList.Contains(x.AP_Stories_Tag.TagName)).Count > 0
                                     Select c

            End If

            sortedChannelCache = sortedChannelCache.Where(Function(c) c.channelCache.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = TabModuleId And Not _
                                                              c.channelCache.Block) _
                                                              .OrderByDescending(Function(c) c.ViewOrder) _
                                                              .Take(N)

            Dim storyList = sortedChannelCache.Select(Function(x) x.channelCache).ToList()

            phStoryControl.Controls.Clear()
            theControl = LoadControl(URL)

            theControl.ID = "theControl"
            phStoryControl.Controls.Add(theControl)
            Dim ucType As Type = theControl.GetType()

            ucType.GetMethod("Initialize").Invoke(theControl, New Object() {storyList, Settings})
            LoadVideoRepeater(storyList, Settings)
        End Sub

        Private Sub LoadVideoRepeater(ByVal stories As List(Of AP_Stories_Module_Channel_Cache), settings As Hashtable)
            Dim skip As Integer = 0
            Dim pg As Integer = 0
            If Not String.IsNullOrEmpty(Request.QueryString("p")) Then
                pg = Request.QueryString("p")
                skip = pg * CInt(settings(ControlerConstants.NUMSTORIES))
            End If

            Dim listData As DataTable = StoryFunctions.GetVideoList(stories.Skip(skip).Take(CInt(settings(ControlerConstants.NUMSTORIES))),
                                                                   PortalSettings.DefaultPortalAlias)

            SliderVideoPopup.DataSource = listData
            SliderVideoPopup.DataBind()
        End Sub



#End Region 'Helper Functions

#Region "Optional Interfaces"

        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get

                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection

                Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.StoryMixerControlKey),
                            StoryFunctionsConstants.StoryMixerControlKey, "", "icon_configuration_16px.png",
                            EditUrl(StoryFunctionsConstants.StoryMixerControlKey), False, SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.UnpublishedControlKey),
                            StoryFunctionsConstants.UnpublishedControlKey, "", "icon_lists_16px.gif",
                            EditUrl(StoryFunctionsConstants.UnpublishedControlKey), False, SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.TagSettingsControlKey),
                            StoryFunctionsConstants.TagSettingsControlKey, "", "action_settings.gif",
                            EditUrl(StoryFunctionsConstants.TagSettingsControlKey), False, SecurityAccessLevel.Edit, True, False)

                If UserInfo.IsInRole("Administrators") Or UserInfo.IsSuperUser Then
                    Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.StorySettingsControlKey),
                                StoryFunctionsConstants.StorySettingsControlKey, "", "action_settings.gif",
                                EditUrl(StoryFunctionsConstants.StorySettingsControlKey), False, SecurityAccessLevel.Edit, True, False)
                End If

                Actions.Add(GetNextActionID, LocalizeString(StoryFunctionsConstants.NewStoryControlKey),
                            StoryFunctionsConstants.NewStoryControlKey, "", "add.gif",
                            EditUrl(StoryFunctionsConstants.NewStoryControlKey), False, SecurityAccessLevel.Edit, True, False)

                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace
