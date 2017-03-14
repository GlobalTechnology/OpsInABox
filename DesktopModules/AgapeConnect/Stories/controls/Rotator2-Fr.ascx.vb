Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports StaffBrokerFunctions
Imports Stories

Namespace DotNetNuke.Modules.AgapeConnect.Stories
    Partial Class Rotator_Fr
        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            'Allowing dynamically loaded controls to be translated using the DNN translation system is complex...
            'However this code does the trick. Just copy this Sub (Page_Init) ,as is, to make it work
            'An App_LocalResources Folder must be located in the same location as this file - and Contain your resx files (using the usual dnn resx file naming convention)


            Dim FileName As String = System.IO.Path.GetFileNameWithoutExtension(Me.AppRelativeVirtualPath)
            If Not (Me.ID Is Nothing) Then
                'this will fix it when its placed as a ChildUserControl 
                Me.LocalResourceFile = Me.LocalResourceFile.Replace(Me.ID, FileName)
            Else
                ' this will fix it when its dynamically loaded using LoadControl method 
                Me.LocalResourceFile = Me.LocalResourceFile & FileName & ".ascx.resx"
                Dim Locale = System.Threading.Thread.CurrentThread.CurrentCulture.Name
                Dim AppLocRes As New System.IO.DirectoryInfo(Me.LocalResourceFile.Replace(FileName & ".ascx.resx", ""))
                If Locale = PortalSettings.CultureCode Then
                    'look for portal varient
                    If AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                    End If
                Else

                    If AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".Portal-" & PortalId & ".resx").Count > 0 Then
                        'lookFor a CulturePortalVarient
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".Portal-" & PortalId & ".resx")
                    ElseIf AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".resx").Count > 0 Then
                        'look for a CultureVarient
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".resx")
                    ElseIf AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                        'lookFor a PortalVarient
                        Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                    End If
                End If
            End If
        End Sub

        Public Sub Initialize(ByVal stories As List(Of AP_Stories_Module_Channel_Cache), settings As Hashtable)

            Dim rotatorSettings As Hashtable = StoryFunctions.GetRotatorSettings(stories.First.ChannelId, settings)

            hfManualAdvance.Value = rotatorSettings.Item("ManualAdvance")
            hfPauseTime.Value = rotatorSettings.Item("Speed")
            hfDivWidth.Value = rotatorSettings.Item("PhotoWidth")
            hfDivHeight.Value = rotatorSettings.Item("PhotoHeight")
            hfChannelId.Value = rotatorSettings.Item("ChannelId")

            Dim sliderData As New DataTable()
            sliderData.Columns.Add("sliderLink")
            sliderData.Columns.Add("sliderImage")
            sliderData.Columns.Add("sliderImageStyle")
            sliderData.Columns.Add("sliderImageAltText")
            sliderData.Columns.Add("sliderImageTitle")
            sliderData.Columns.Add("sliderLinkImageCSS")

            For Each row In stories
                Try
                    Dim dataRow As DataRow = sliderData.NewRow()

                    'setup for the link

                    Dim viewStyles As Dictionary(Of String, String) = StoryFunctions.GetTagPersonalisation(row.GUID, TabModuleId)
                    Dim cssHyperlink As String = ""
                    Dim clickAction As String = ""

                    Dim target = "_blank"
                    If row.Link.Contains(PortalSettings.DefaultPortalAlias) Then
                        target = "_self"
                    End If

                    'check for personalized link image
                    If (viewStyles.Item(TagSettingsConstants.LINKIMAGESTRING).Equals(TagSettingsConstants.LinkImage.PlayButton.ToString)) Then
                        cssHyperlink = "nivo-imageLink " & TagSettingsConstants.LinkImage.PlayButton.ToString
                    Else
                        cssHyperlink = "nivo-imageLink"
                    End If
                    dataRow("sliderLinkImageCSS") = cssHyperlink

                    'check for personalized opening style
                    If (viewStyles.Item(TagSettingsConstants.OPENSTYLESTRING).Equals(TagSettingsConstants.OpenStyle.Popup.ToString)) Then
                        clickAction = "onclick=alert('Here is the pop-up')"
                    Else
                        clickAction = "window.open('" & row.Link & "', '" & target & "');"
                    End If

                    dataRow("sliderLink") = "javascript: registerClick(" & row.CacheId & "); " & clickAction

                    'setup for the image
                    Dim sliderImage As New System.Web.UI.WebControls.Image
                    sliderImage.ImageUrl = row.ImageId
                    sliderImage.AlternateText = row.Headline
                    sliderImage.Attributes("title") = "<h1 class='slider-image-text'>" & HttpUtility.HtmlEncode(row.Headline) & "</h1>"

                    If CInt(settings("Aspect")) < (CDbl(row.ImageWidth) / CDbl(row.ImageHeight)) Then
                        sliderImage.Width = hfDivWidth.Value
                        sliderImage.Height = CInt((CDbl(hfDivWidth.Value) * row.ImageHeight) / row.ImageWidth)
                    Else
                        sliderImage.Width = CInt((CDbl(hfDivHeight.Value) * row.ImageWidth) / row.ImageHeight)
                        sliderImage.Height = hfDivHeight.Value
                    End If
                    sliderImage.Style.Add("height", sliderImage.Height.ToString)
                    sliderImage.Style.Add("width", sliderImage.Width.ToString)

                    dataRow("sliderImage") = sliderImage.ImageUrl
                    dataRow("sliderImageAltText") = sliderImage.AlternateText
                    dataRow("sliderImageTitle") = sliderImage.Attributes("title")
                    dataRow("sliderImageStyle") = sliderImage.Style

                    sliderData.Rows.Add(dataRow)
                Catch ex As Exception
                End Try

            Next
            SliderImageList.DataSource = sliderData
            SliderImageList.DataBind()

        End Sub

    End Class
End Namespace
