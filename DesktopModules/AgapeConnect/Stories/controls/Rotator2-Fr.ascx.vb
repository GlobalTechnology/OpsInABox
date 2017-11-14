Imports System
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

            'No stories in module
            If stories.Count < 1 Then
                lblNoStories.Visible = True
            End If

            Dim rotatorSettings As Hashtable = StoryFunctions.GetRotatorSettings(stories, settings)

            hfManualAdvance.Value = rotatorSettings.Item(ControlerConstants.MANUALADVANCE)
            hfPauseTime.Value = rotatorSettings.Item(ControlerConstants.SPEED)
            hfChannelId.Value = rotatorSettings.Item(ControlerConstants.CHANNELID)

            Dim sliderData As DataTable = StoryFunctions.GetRotatorSlides(stories, rotatorSettings,
                                                                          PortalSettings.DefaultPortalAlias)

            'customize title for this rotator
            For Each row As DataRow In sliderData.Rows
                row.Item(ControlerConstants.SLIDEIMAGETITLE) = row.Item(ControlerConstants.SLIDETEXTLINK) &
                    "<h1>" & row.Item(ControlerConstants.SLIDEIMAGETITLE) & "</h1> " &
                    "<h2>" & row.Item(ControlerConstants.SLIDEIMAGESUBTITLE) & "</h2> " &
                    "<p>" & row.Item(ControlerConstants.SLIDEIMAGEDESC) & "</p></a>" &
                     AddThisDiv(row)
            Next

            SliderImageList.DataSource = sliderData
            SliderImageList.DataBind()
        End Sub

#Region "Helper functions"

        Private Function AddThisDiv(ByRef row As DataRow) As String
            Return " <div class='addthis_toolbox addthis_default_style addthis_32x32_style;' " &
                    "addthis:url=""" & row.Item(ControlerConstants.SLIDERAWURL) & """ " &
                    "addthis:title=""" & row.Item(ControlerConstants.SLIDEIMAGETITLE) & """ " &
                    "addthis:description=""" & row.Item(ControlerConstants.SLIDEIMAGEDESC) & """ " &
                    "addthis:media=""" & row.Item(ControlerConstants.SLIDEIMAGE) & """ " & ">" &
                    " <a class='addthis_button_facebook'></a> " &
                    " <a class='addthis_button_twitter'></a> " &
                    " <a class='addthis_button_google_plusone_share'></a> " &
                    " <a class='addthis_button_email'></a> " &
                    " <a class='addthis_button_compact'></a></div>"
        End Function
#End Region
    End Class
End Namespace
