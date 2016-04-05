﻿Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports StaffBrokerFunctions
Imports Stories

Namespace DotNetNuke.Modules.AgapeConnect.Stories

    Partial Class TagList_Fr
        Inherits Entities.Modules.PortalModuleBase

        Dim d As New StoriesDataContext

        Public divWidth As Integer = 150
        Public divHeight As Integer = 150

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

        Public Sub Initialize(ByVal TagsCache As List(Of AP_Stories_Tag), settings As Hashtable)

            Dim out As String = ""

            Dim photoWidth As Integer = 150
            If Not String.IsNullOrEmpty(settings("PhotoWidth")) Then
                photoWidth = settings("PhotoWidth")
            End If

            Dim photoAspect As Double = 1.0
            If Not String.IsNullOrEmpty(settings("PhotoWidth")) Then
                photoAspect = Double.Parse(CStr(settings("Aspect")), New CultureInfo(""))
            End If

            Dim photoHeight As Integer = CDbl(photoWidth) / photoAspect

            Dim AspectMode As Integer = 1
            If Not String.IsNullOrEmpty(settings("AspectMode")) Then
                AspectMode = settings("AspectMode")
            End If
            divWidth = photoWidth
            divHeight = photoHeight

            Try
                dlTags.DataSource = TagsCache
                dlTags.DataBind()
            Catch e As Exception
                AgapeLogger.Error(UserId, e.Message & e.StackTrace)
            End Try

        End Sub

        'Return image URL corresponding to Tag Photo ID
        Public Function GetImageURL(ByVal photoID As Integer) As String
            Return StoryFunctions.GetPhotoURL(photoID)
        End Function

    End Class
End Namespace
