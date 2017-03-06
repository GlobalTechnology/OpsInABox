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

        Public PauseTime As Integer = 3000 ' milliseconds
        Public divWidth As Integer = 150
        Public divHeight As Integer = 150
        Public manualAdvance As String = "false"
        Public linkImage As String = TagSettingsConstants.LinkImage.None
        Public openStyle As String = TagSettingsConstants.OpenStyle.NewPage

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

        Public Sub Initialize(ByVal Stories As List(Of AP_Stories_Module_Channel_Cache), settings As Hashtable)

            If (Not String.IsNullOrEmpty(settings("ManualAdvance"))) Then
                manualAdvance = settings("ManualAdvance").ToLower
            End If

            If (Not String.IsNullOrEmpty(settings("Speed"))) Then
                PauseTime = CInt(settings("Speed")) * 1000   ' milliseconds
            End If

            Dim photoWidth As Integer = 150
            If Not String.IsNullOrEmpty(settings("PhotoWidth")) Then
                photoWidth = settings("PhotoWidth")
            End If

            Dim photoAspect As Double = 1.0
            If Not String.IsNullOrEmpty(settings("PhotoWidth")) Then
                photoAspect = Double.Parse(CStr(settings("Aspect")), New CultureInfo(""))
            End If

            Dim photoHeight As Integer = CDbl(photoWidth) / photoAspect

            divWidth = photoWidth
            divHeight = photoHeight
            hfChannelId.Value = Stories.First.ChannelId

            Dim playButtonDimension = 100
            Dim playButtonTop = (divHeight / 2) - (playButtonDimension / 2)
            Dim playButtonLeft = (divWidth / 2) - (playButtonDimension / 2)

            Dim sliderData As New DataTable()
            sliderData.Columns.Add("sliderLink")
            sliderData.Columns.Add("sliderImage")
            sliderData.Columns.Add("sliderImageStyle")
            sliderData.Columns.Add("sliderImageAltText")
            sliderData.Columns.Add("sliderImageTitle")
            sliderData.Columns.Add("sliderLinkImageCSS")


            For Each row In Stories
                Try
                    Dim dataRow As DataRow = sliderData.NewRow()

                    'setup for the link
                    Dim target = "_blank"
                    If row.Link.Contains(PortalSettings.DefaultPortalAlias) Then
                        target = "_self"
                    End If

                    dataRow("sliderLink") = "javascript: registerClick(" & row.CacheId & "); window.open('" & row.Link & "', '" & target & "');"

                    'setup for the image
                    Dim sliderImage As New System.Web.UI.WebControls.Image
                    sliderImage.ImageUrl = row.ImageId
                    sliderImage.AlternateText = row.Headline
                    sliderImage.Attributes("title") = "<h1 class='slider-image-text'>" & HttpUtility.HtmlEncode(row.Headline) & "</h1>"

                    If photoAspect < (CDbl(row.ImageWidth) / CDbl(row.ImageHeight)) Then
                        sliderImage.Width = divWidth
                        sliderImage.Height = CInt((CDbl(divWidth) * row.ImageHeight) / row.ImageWidth)
                    Else
                        sliderImage.Width = CInt((CDbl(divHeight) * row.ImageWidth) / row.ImageHeight)
                        sliderImage.Height = divHeight
                    End If
                    sliderImage.Style.Add("height", sliderImage.Height.ToString)
                    sliderImage.Style.Add("width", sliderImage.Width.ToString)

                    dataRow("sliderImage") = sliderImage.ImageUrl
                    dataRow("sliderImageAltText") = sliderImage.AlternateText
                    dataRow("sliderImageTitle") = sliderImage.Attributes("title")
                    dataRow("sliderImageStyle") = sliderImage.Style

                    'check for personalized link image and open style
                    Dim viewStyles As Dictionary(Of String, String) = StoryFunctions.GetStoryPersonalisation(row.GUID, TabModuleId)
                    Dim cssHyperlink As String = ""
                    AgapeLogger.Info(UserId, viewStyles.TryGetValue("linkImage", TagSettingsConstants.LinkImage.PlayButton.ToString))
                    If viewStyles.TryGetValue("linkImage", TagSettingsConstants.LinkImage.PlayButton.ToString) Then
                        cssHyperlink = "nivo-imageLink playbutton"
                    Else
                        cssHyperlink = "nivo-imageLink"
                    End If
                    dataRow("sliderLinkImageCSS") = cssHyperlink

                    sliderData.Rows.Add(dataRow)

                Catch ex As Exception
                End Try

            Next
            SliderImageList.DataSource = sliderData
            SliderImageList.DataBind()

        End Sub

    End Class
End Namespace
