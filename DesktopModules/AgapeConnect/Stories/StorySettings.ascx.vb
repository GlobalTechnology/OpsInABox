Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq
Imports System.ServiceModel.Syndication
Imports System.Xml
Imports System.Net
Imports Stories

Imports DotNetNuke.Services.FileSystem


Namespace DotNetNuke.Modules.Stories

    Partial Class StorySettings
        Inherits Entities.Modules.ModuleSettingsBase

        Dim d As New StoriesDataContext
#Region "Base Method Implementations"


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

            If (Not UserInfo.IsInRole("Administrators")) Then
                Response.Redirect(NavigateURL(PortalSettings.Current.ErrorPage404))
            End If

            If Not Page.IsPostBack Then

                    ddlDisplayTypes.DataSource = (From c In d.AP_Stories_Controls Where c.Type <> StoryModuleType.TagList Select c.Name, Value = c.Type & ":" & c.StoryControlId)

                    ddlTagsDisplayTypes.DataSource = (From c In d.AP_Stories_Controls Where c.Type = StoryModuleType.TagList Select c.Name, Value = c.Type & ":" & c.StoryControlId)

                    Dim mc As New DotNetNuke.Entities.Modules.ModuleController

                    Dim dtp = DotNetNuke.Entities.Modules.DesktopModuleController.GetDesktopModuleByFriendlyName("ac_ViewStory")
                    Dim allTabs As New ArrayList()
                    If (Not dtp Is Nothing) Then


                        Dim tabs = (New TabController).GetTabsByPortal(PortalId)
                        For Each row In tabs

                            Dim mods = mc.GetTabModules(row.Value.TabID)
                            For Each row2 In mods
                                If row2.Value.DesktopModuleID = dtp.DesktopModuleID Then
                                    allTabs.Add(row2.Value)
                                End If
                            Next
                        Next
                    End If
                    ddlTabs.DataSource = (From c As DotNetNuke.Entities.Modules.ModuleInfo In allTabs Select c.ParentTab.TabName, c.ParentTab.TabID)
                    ddlTabs.DataTextField = "TabName"
                    ddlTabs.DataValueField = "TabId"
                    ddlTabs.DataBind()


                    ddlDisplayTypes.DataTextField = "Name"
                    ddlDisplayTypes.DataValueField = "Value"
                    ddlDisplayTypes.DataBind()

                    ddlTagsDisplayTypes.DataTextField = "Name"
                    ddlTagsDisplayTypes.DataValueField = "Value"
                    ddlTagsDisplayTypes.DataBind()


                    Dim newSettings As Boolean = False
                    Dim objModules As New Entities.Modules.ModuleController
                    Dim theModule = StoryFunctions.GetStoryModule(TabModuleId)
                    Dim geoLocation As String = StoryFunctions.GetDefaultLatLong(TabModuleId)

                    If theModule.AP_Stories_Module_Channels.Where(Function(x) x.Type = 2).Count = 0 Then
                        'add a local channel!
                        Dim RssName As String = ""
                        If CType(TabModuleSettings("WeightPopular"), String) <> "" Then
                            RssName = TabModuleSettings("RssName")

                        Else
                            RssName = ModuleConfiguration.ParentTab.TabName
                            objModules.UpdateTabModuleSetting(TabModuleId, "RssName", RssName)
                            newSettings = True
                        End If

                        Dim logoFile = StoryFunctions.SetLogo("https://" & PortalSettings.PortalAlias.HTTPAlias & PortalSettings.HomeDirectory & PortalSettings.LogoFile, PortalId)


                        Dim imageId = "https://" & PortalAlias.HTTPAlias & FileManager.Instance.GetUrl(FileManager.Instance.GetFile(logoFile))
                        Dim autoDetectLanguage As Boolean = False

                        StoryFunctions.AddLocalChannel(TabModuleId, PortalAlias.HTTPAlias, RssName, geoLocation.Split(",")(1), geoLocation.Split(",")(0), imageId, autoDetectLanguage)

                        theModule = StoryFunctions.GetStoryModule(TabModuleId)

                    End If




                    If CType(TabModuleSettings("StoryControlId"), String) <> "" Then
                        If d.AP_Stories_Controls.Where(Function(x) x.StoryControlId = CInt(TabModuleSettings("StoryControlId"))).Count > 0 Then
                            For Each row As ListItem In ddlDisplayTypes.Items
                                If row.Value.EndsWith(":" & TabModuleSettings("StoryControlId")) Then
                                    ddlDisplayTypes.SelectedValue = row.Value
                                    Exit For
                                End If
                            Next
                        End If
                    End If

                    If CType(TabModuleSettings("TagListControlId"), String) <> "" Then
                        If d.AP_Stories_Controls.Where(Function(x) x.StoryControlId = CInt(TabModuleSettings("TagListControlId"))).Count > 0 Then
                            For Each row As ListItem In ddlTagsDisplayTypes.Items
                                If row.Value.EndsWith(":" & TabModuleSettings("TagListControlId")) Then
                                    ddlTagsDisplayTypes.SelectedValue = row.Value
                                    Exit For
                                End If
                            Next
                        End If
                    End If

                    If CType(TabModuleSettings("AspectMode"), String) <> "" Then
                        ddlAspectMode.SelectedValue = CInt(TabModuleSettings("AspectMode"))


                    End If

                    If CType(TabModuleSettings("ViewTab"), String) <> "" Then
                        ddlTabs.SelectedValue = CInt(TabModuleSettings("ViewTab"))


                    End If

                    'Init Story Photo Aspect
                    Dim storyPhotoAspect = "1.3"
                    If CType(TabModuleSettings("Aspect"), String) <> "" Then
                        storyPhotoAspect = TabModuleSettings("Aspect")
                    Else
                        objModules.UpdateTabModuleSetting(TabModuleId, "Aspect", storyPhotoAspect)
                        newSettings = True
                    End If
                    lblStoryPhotoAspect.Text = Double.Parse(storyPhotoAspect, New CultureInfo("")).ToString(New CultureInfo(""))
                    resizableStoryPhotoAspect.Height = Unit.Pixel(80)
                    resizableStoryPhotoAspect.Width = Unit.Pixel(Double.Parse(storyPhotoAspect, New CultureInfo("")) * 80)
                    hfStoryPhotoAspect.Value = lblStoryPhotoAspect.Text

                    'Init Tag Photo Aspect
                    Dim tagPhotoAspect = "1.3"
                    If CType(TabModuleSettings("TagAspect"), String) <> "" Then
                        tagPhotoAspect = TabModuleSettings("TagAspect")
                    Else
                        objModules.UpdateTabModuleSetting(TabModuleId, "TagAspect", tagPhotoAspect)
                        newSettings = True
                    End If
                    lblTagPhotoAspect.Text = Double.Parse(tagPhotoAspect, New CultureInfo("")).ToString(New CultureInfo(""))
                    resizableTagPhotoAspect.Height = Unit.Pixel(80)
                    resizableTagPhotoAspect.Width = Unit.Pixel(Double.Parse(tagPhotoAspect, New CultureInfo("")) * 80)
                    hfTagPhotoAspect.Value = lblTagPhotoAspect.Text

                    If CType(TabModuleSettings("Speed"), String) <> "" Then

                        hfSpeed.Value = TabModuleSettings("Speed")
                        lblSpeed.Text = TabModuleSettings("Speed")
                    End If

                    If CType(TabModuleSettings("PhotoWidth"), String) <> "" Then
                        tbPhotoSize.Text = TabModuleSettings("PhotoWidth")
                    Else
                        tbPhotoSize.Text = 150
                    End If

                    cblShow.ClearSelection()



                    If CType(TabModuleSettings("AdvancedSettings"), String) <> "" Then
                        tbAdvanceSettings.Text = TabModuleSettings("AdvancedSettings")

                    End If


                    If CType(TabModuleSettings("ShowFields"), String) <> "" Then
                        Dim s = CStr(TabModuleSettings("ShowFields")).Split(",")
                        For Each row As ListItem In cblShow.Items
                            If s.Contains(row.Text) Then
                                row.Selected = True
                            End If
                        Next

                    End If
                    Dim mode As String = StaffBrokerFunctions.GetSetting("StoryPublishMode", PortalId)
                    If Not String.IsNullOrEmpty(mode) Then
                        ddlMode.SelectedValue = mode
                    End If
                    Dim bl As String = StaffBrokerFunctions.GetSetting("StoryBoostLength", PortalId)
                    If Not String.IsNullOrEmpty(bl) Then
                        tbBoostLength.Text = bl
                    End If

                    If newSettings Then
                        DotNetNuke.Entities.Modules.ModuleController.SynchronizeModule(ModuleId)
                    End If
                End If 'Not Page.IsPostBack

        End Sub
#End Region 'Base Method Implementations

#Region "Page Events"

        Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
            'Save Module Settings
            Dim objModules As New Entities.Modules.ModuleController

            Dim s As String = ddlDisplayTypes.SelectedValue
            If Not String.IsNullOrEmpty(s) Then
                Dim StoryControlId As Integer = s.Substring(s.IndexOf(":") + 1)
                objModules.UpdateTabModuleSetting(TabModuleId, "StoryControlId", StoryControlId)
            End If

            Dim s2 As String = ddlTagsDisplayTypes.SelectedValue
            If Not String.IsNullOrEmpty(s2) Then
                Dim TagListControlId As Integer = s2.Substring(s2.IndexOf(":") + 1)
                objModules.UpdateTabModuleSetting(TabModuleId, "TagListControlId", TagListControlId)
            Else
                objModules.UpdateTabModuleSetting(TabModuleId, "TagListControlId", "")
            End If

            'Speed
            objModules.UpdateTabModuleSetting(TabModuleId, "Speed", CInt(hfSpeed.Value))

            'PhotoWidth
            objModules.UpdateTabModuleSetting(TabModuleId, "PhotoWidth", CInt(tbPhotoSize.Text))

            'AspectMode
            objModules.UpdateTabModuleSetting(TabModuleId, "AspectMode", ddlAspectMode.SelectedValue)

            'View in Tab
            objModules.UpdateTabModuleSetting(TabModuleId, "ViewTab", ddlTabs.SelectedValue)


            'Story Photo Aspect
            objModules.UpdateTabModuleSetting(TabModuleId, "Aspect", Double.Parse(hfStoryPhotoAspect.Value, New CultureInfo("")).ToString(New CultureInfo("")))

            'Tag Photo Aspect
            objModules.UpdateTabModuleSetting(TabModuleId, "TagAspect", Double.Parse(hfTagPhotoAspect.Value, New CultureInfo("")).ToString(New CultureInfo("")))

            '
            objModules.UpdateTabModuleSetting(TabModuleId, "AdvancedSettings", tbAdvanceSettings.Text)


            'ShowFields
            Dim Fields As String = ""

            For Each row As ListItem In cblShow.Items
                If row.Selected Then
                    Fields &= row.Text & ","

                End If
            Next

            objModules.UpdateTabModuleSetting(TabModuleId, "ShowFields", Fields.Trim(","))

            StaffBrokerFunctions.SetSetting("StoryPublishMode", ddlMode.SelectedValue, PortalId)
            StaffBrokerFunctions.SetSetting("StoryBoostLength", CInt(tbBoostLength.Text), PortalId)

            SynchronizeModule()
            Response.Redirect(NavigateURL())

        End Sub

        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())

        End Sub

#End Region 'Page Events

    End Class

End Namespace

