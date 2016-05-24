Imports System
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

            If Request.QueryString("StoryId") <> "" Then
                If String.IsNullOrEmpty(Settings("ViewTab")) Or Settings("ViewTab") = 0 Then
                    Response.Redirect(EditUrl("ViewStory") & RequestStoryURL)
                Else
                    Response.Redirect(NavigateURL(CInt(Settings("ViewTab"))) & RequestStoryURL)
                End If

            End If

            If Not String.IsNullOrEmpty(Request.Form("StoryLink")) Then
                'Register a click for this story
                Dim theCache = From c In d.AP_Stories_Module_Channel_Caches Where c.CacheId = CInt(Request.Form("StoryLink"))

                If theCache.Count > 0 Then
                    theCache.First.Clicks += 1
                End If
                d.SubmitChanges()
                Return
            End If

            If Not Page.IsPostBack Then

                If Not String.IsNullOrEmpty(Settings("TagListControlId")) And (Request.QueryString("tags") = "") Then ' Show tag list for current Story module

                    Dim dControl = From c In d.AP_Stories_Controls Where c.StoryControlId = CInt(Settings("TagListControlId"))

                    If dControl.Count > 0 Then

                        LoadStoryControl(dControl.First.Location, True)

                    End If

                Else ' Show list of stories

                    If String.IsNullOrEmpty(Settings("StoryControlId")) Then 'Load default Display Type if none defined in module settings
                        If d.AP_Stories_Controls.Count > 0 Then
                            LoadStoryControl(d.AP_Stories_Controls.First.Location)
                        End If
                    Else 'Load Display Type defined in module settings

                        Dim dControl = From c In d.AP_Stories_Controls Where c.StoryControlId = CInt(Settings("StoryControlId"))

                        If dControl.Count > 0 Then

                            LoadStoryControl(dControl.First.Location, dControl.First.Type = 2)

                        End If

                    End If

                End If

            End If

        End Sub

#End Region 'Base Method Implementations

#Region "Helper Functions"

        Private Sub LoadStoryControl(ByVal URL As String, Optional IsList As Boolean = False)

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

            Dim r = From c In d.AP_Stories_Module_Channel_Caches Select c, ViewOrder = CDbl(c.Precal) * (CDbl(1.0 + (Math.Log(c.Clicks) * P / 200))) * (1.0 + (G * (CDbl(1.0) - CDbl(CDbl(Math.Min(CDbl(200), ((Math.Acos(CDbl(Math.Sin(CDbl(deg2Rad) * CDbl(lt))) * CDbl(Math.Sin(deg2Rad * CDbl(c.Latitude))) + CDbl(Math.Cos(CDbl(deg2Rad) * CDbl(lt))) * CDbl(Math.Cos(CDbl(deg2Rad) * CDbl(c.Latitude))) * CDbl(Math.Cos(CDbl(deg2Rad) * (CDbl(lg) - CDbl(c.Longitude)))))) / CDbl(Math.PI) * CDbl(180.0)) * CDbl(1.1515) * CDbl(60.0))) / CDbl(200.0)))) / CDbl(2.0))

            If (Request.QueryString("tags") <> "") Then

                Dim tagList = Request.QueryString("tags").Split(",")

                r = From c In r Join b In d.AP_Stories On CInt(c.c.GUID) Equals b.StoryId Where b.AP_Stories_Tag_Metas.Where(Function(x) tagList.Contains(x.AP_Stories_Tag.TagName)).Count > 0 Select c

            End If

            r = r.Where(Function(c) CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower = c.c.Langauge.Substring(0, 2) And c.c.AP_Stories_Module_Channel.AP_Stories_Module.TabModuleId = TabModuleId And Not c.c.Block) _
                            .OrderByDescending(Function(c) c.ViewOrder) _
                        .Take(N)
            Dim storyList = r.Select(Function(x) x.c).ToList()

            phStoryControl.Controls.Clear()
            theControl = LoadControl(URL)

            theControl.ID = "theControl"
            phStoryControl.Controls.Add(theControl)
            Dim ucType As Type = theControl.GetType()

            ucType.GetMethod("Initialize").Invoke(theControl, New Object() {storyList, Settings})

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
