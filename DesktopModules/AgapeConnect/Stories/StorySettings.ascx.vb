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
            hfPortalId.Value = PortalId
            If Not Page.IsPostBack Then

               
                ddlDisplayTypes.DataSource = (From c In d.AP_Stories_Controls Select c.Name, Value = c.Type & ":" & c.StoryControlId)
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


                Dim newSettings As Boolean = False
                Dim objModules As New Entities.Modules.ModuleController
                Dim theModule = StoryFunctions.GetStoryModule(TabModuleId)
                Dim l = Location.GetLocation(Request.ServerVariables("remote_addr"))

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

                    StoryFunctions.AddLocalChannel(TabModuleId, PortalAlias.HTTPAlias, RssName, l.longitude, l.latitude, imageId)

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

                If CType(TabModuleSettings("AspectMode"), String) <> "" Then
                    ddlAspectMode.SelectedValue = CInt(TabModuleSettings("AspectMode"))


                End If

                If CType(TabModuleSettings("ViewTab"), String) <> "" Then
                    ddlTabs.SelectedValue = CInt(TabModuleSettings("ViewTab"))


                End If


                If CType(TabModuleSettings("Aspect"), String) <> "" Then
                    lblAspect.Text = Double.Parse(TabModuleSettings("Aspect"), New CultureInfo("")).ToString(New CultureInfo(""))
                    resizable.Height = Unit.Pixel(80)
                    resizable.Width = Unit.Pixel(Double.Parse(TabModuleSettings("Aspect"), New CultureInfo("")) * 80)
                    hfAspect.Value = lblAspect.Text
                Else
                    hfAspect.Value = 1.3

                    objModules.UpdateTabModuleSetting(TabModuleId, "Aspect", "1.3")
                    newSettings = True


                End If

                lblRssPrefix.Text = Request.Url.Authority & Request.ApplicationPath & "DesktopModules/AgapeConnect/Stories/Feed.aspx?name="

                If CType(TabModuleSettings("RssName"), String) = "" Then
                    objModules.UpdateTabModuleSetting(TabModuleId, "RssName", TabController.CurrentPage.TabName)
                    tbRssName.Text = TabController.CurrentPage.TabName

                    newSettings = True
                Else
                    tbRssName.Text = CType(TabModuleSettings("RssName"), String)

                End If


                If CType(TabModuleSettings("Speed"), String) <> "" Then

                    hfSpeed.Value = TabModuleSettings("Speed")
                    lblSpeed.Text = TabModuleSettings("Speed")
                End If

                If CType(TabModuleSettings("Latitude"), String) <> "" And CType(TabModuleSettings("Longitude"), String) <> "" Then
                    tbLocation.Text = CDbl(TabModuleSettings("Latitude")).ToString(New CultureInfo("")) & ", " & Double.Parse(TabModuleSettings("Longitude"), New CultureInfo("")).ToString(New CultureInfo(""))

                Else
                    tbLocation.Text = l.latitude.ToString(New CultureInfo("")) & ", " & l.longitude.ToString(New CultureInfo(""))
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
                    SynchronizeModule()
                End If
            End If



        End Sub

       

#End Region
       

        Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
            'Save Module Settings
            Dim objModules As New Entities.Modules.ModuleController

            'Location
            Dim geoLoc = tbLocation.Text.Split(",")
            If geoLoc.Count <> 2 Then
                lblFeedError.Text = "Invalid location. Please click search, to convert into latitude/longitude<br />"
                Return
            Else
                Try
                    objModules.UpdateTabModuleSetting(TabModuleId, "Latitude", Double.Parse(geoLoc(0).Replace(" ", ""), New CultureInfo("")))
                    objModules.UpdateTabModuleSetting(TabModuleId, "Longitude", Double.Parse(geoLoc(1).Replace(" ", ""), New CultureInfo("")))

                Catch ex As Exception
                    lblFeedError.Text = "Invalid location. Please click search, to convert into latitude/longitude<br />"
                    Return
                End Try
            End If
            
            Dim s As String = ddlDisplayTypes.SelectedValue

            If Not String.IsNullOrEmpty(s) Then
                Dim StoryControlId As Integer = s.Substring(s.IndexOf(":") + 1)
                objModules.UpdateTabModuleSetting(TabModuleId, "StoryControlId", StoryControlId)

            End If
           
            'Speed
            objModules.UpdateTabModuleSetting(TabModuleId, "Speed", CInt(hfSpeed.Value))

            'RssName
            objModules.UpdateTabModuleSetting(TabModuleId, "RssName", tbRssName.Text)

            'PhotoWidth
            objModules.UpdateTabModuleSetting(TabModuleId, "PhotoWidth", CInt(tbPhotoSize.Text))

            'AspectMode
            objModules.UpdateTabModuleSetting(TabModuleId, "AspectMode", ddlAspectMode.SelectedValue)

            'View in Tab
            objModules.UpdateTabModuleSetting(TabModuleId, "ViewTab", ddlTabs.SelectedValue)


            'Aspect
            objModules.UpdateTabModuleSetting(TabModuleId, "Aspect", Double.Parse(hfAspect.Value, New CultureInfo("")).ToString(New CultureInfo("")))

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


            '====== Need to update some of the channel table properties (for this module)
            
            Dim LocalChannel = From c In d.AP_Stories_Module_Channels Where c.Type = 2 And c.AP_Stories_Module.TabModuleId = TabModuleId

            If localChannel.Count > 0 Then
                localChannel.First.Latitude = Double.Parse(geoLoc(0).Replace(" ", ""), New CultureInfo(""))
                localChannel.First.Longitude = Double.Parse(geoLoc(1).Replace(" ", ""), New CultureInfo(""))
                localChannel.First.ChannelTitle = tbRssName.Text
                d.SubmitChanges()
            End If

           
           


           
            SynchronizeModule()
            Response.Redirect(NavigateURL())

        End Sub

        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())

        End Sub

      

        Protected Sub btnAddTag_Click(sender As Object, e As System.EventArgs) Handles btnAddTag.Click
            Dim insert As New AP_Stories_Tag
            insert.PortalId = PortalId
            insert.TagName = tbAddTag.Text
            insert.Master = False
            insert.Keywords = ""
            insert.StoryModuleId = StoryFunctions.GetStoryModule(TabModuleId).StoryModuleId 'TODO: A tester
            d.AP_Stories_Tags.InsertOnSubmit(insert)
            d.SubmitChanges()

            GridView1.DataBind()

        End Sub


        Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting
            Dim q = From c In d.AP_Stories_Tag_Metas Where c.TagId = CInt(e.Keys(0))

            d.AP_Stories_Tag_Metas.DeleteAllOnSubmit(q)
            d.SubmitChanges()

        End Sub
    End Class

End Namespace

