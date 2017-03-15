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

            hfManualAdvance.Value = rotatorSettings.Item(RotatorConstants.MANUALADVANCE)
            hfPauseTime.Value = rotatorSettings.Item(RotatorConstants.SPEED)
            hfDivWidth.Value = rotatorSettings.Item(RotatorConstants.PHOTOWIDTH)
            hfDivHeight.Value = rotatorSettings.Item(RotatorConstants.PHOTOHEIGHT)
            hfChannelId.Value = rotatorSettings.Item(RotatorConstants.CHANNELID)

            Dim sliderData As DataTable = StoryFunctions.GetRotatorSlides(stories, rotatorSettings,
                                                                          PortalSettings.DefaultPortalAlias,
                                                                          TabModuleId)

            'customize title for this rotator
            For Each row As DataRow In sliderData.Rows
                row.Item(RotatorConstants.SLIDEIMAGETITLE) = "<h1 class='slider-image-text'>" & row.Item(RotatorConstants.SLIDEIMAGETITLE) & "</h1>"
            Next

            SliderImageList.DataSource = sliderData
            SliderImageList.DataBind()

        End Sub

    End Class
End Namespace
