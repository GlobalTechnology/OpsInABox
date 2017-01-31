Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

'Imports DotNetNuke
'Imports DotNetNuke.Security
'Imports StaffBroker
Imports StaffBrokerFunctions
Imports Stories
'Imports DotNetNuke.Services.FileSystem
Namespace DotNetNuke.Modules.AgapeConnect.Stories
    Partial Class Rotator_Fr
        Inherits Entities.Modules.PortalModuleBase
        'Adding Stories Translation
        Dim d As New StoriesDataContext
        Public PauseTime As Integer = 3000 ' milliseconds
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

        Public Sub Initialize(ByVal Stories As List(Of AP_Stories_Module_Channel_Cache), settings As Hashtable)

            hfChannelId.Value = Stories.First.ChannelId
            Dim out As String = ""

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

            Dim AspectMode As Integer = 1
            If Not String.IsNullOrEmpty(settings("AspectMode")) Then
                AspectMode = settings("AspectMode")
            End If
            If AspectMode > 2 Then
                AspectMode = 1  ' Ascpect Modes 3 and 4 are not valid with this rotator. It requires a fixed size.
            End If
            divWidth = photoWidth
            divHeight = photoHeight

            For Each row In Stories

                Try
                    If True Then '(CultureInfo.CurrentCulture.Name.ToLower.Contains(row.Langauge.ToLower) Or row.Langauge.ToLower.Contains(CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower)) Then

                        Dim target = "_blank"
                        If row.Link.Contains(PortalSettings.DefaultPortalAlias) Then
                            target = "_self"
                        End If
                        Dim href = "javascript: registerClick(" & row.CacheId & "); window.open('" & row.Link & "', '" & target & "');"

                        out &= "<a href=""" & href & """> "


                        Dim title As String = "<h1 style='line-height: 42px;'><span style=' opacity: 1.0 !important;'>" & HttpUtility.HtmlEncode(row.Headline) & "</span></h1>"

                        Select Case AspectMode
                            Case 0

                                If photoAspect < (CDbl(row.ImageWidth) / CDbl(row.ImageHeight)) Then
                                    out &= "<img src=""" & row.ImageId & """ style=""width: " & divWidth & "px; height: " & CInt((CDbl(divWidth) * row.ImageHeight) / row.ImageWidth) & "px;"" data-thumb=""" & row.ImageId & """ alt=""" & href & """  data-title=""" & title & """ />"
                                    'out &= "<img src=""" & row.ImageId & """ style=""width: " & divWidth & "px; data-thumb=""" & row.ImageId & """ alt=""""  title="""" />"

                                Else
                                    out &= "<img src=""" & row.ImageId & """ style=""width: " & CInt((CDbl(divHeight) * row.ImageWidth) / row.ImageHeight) & "px; height: " & divHeight & "px;"" data-thumb=""" & row.ImageId & """ alt=""" & href & """  data-title=""" & title & """ />"
                                    ' out &= "<img src=""" & row.ImageId & """ style="" height: " & divHeight & "px;"" data-thumb=""" & row.ImageId & """ alt=""""  title="""" />"

                                End If
                            Case 1

                                If photoAspect < (CDbl(row.ImageWidth) / CDbl(row.ImageHeight)) Then
                                    out &= "<img src=""" & row.ImageId & """ style=""width: " & divWidth & "px; height: " & CInt((CDbl(divWidth) * row.ImageHeight) / row.ImageWidth) & "px;"" data-thumb=""" & row.ImageId & """ alt=""" & href & """  data-title=""" & title & """ />"
                                    'out &= "<img src=""" & row.ImageId & """ style=""width: " & divWidth & "px; data-thumb=""" & row.ImageId & """ alt=""""  title="""" />"

                                Else
                                    out &= "<img src=""" & row.ImageId & """ style=""width: " & CInt((CDbl(divHeight) * row.ImageWidth) / row.ImageHeight) & "px; height: " & divHeight & "px;"" data-thumb=""" & row.ImageId & """ alt=""" & href & """  data-title=""" & title & """ />"
                                    ' out &= "<img src=""" & row.ImageId & """ style="" height: " & divHeight & "px;"" data-thumb=""" & row.ImageId & """ alt=""""  title="""" />"

                                End If

                            Case 2

                            Case 3

                            Case 4

                            Case Else

                        End Select
                        '        out &= "<img src=""" & row.ImageId & """ style=""width: " & photoWidth & "px; height: " & CInt((photoWidth * row.ImageHeight) / row.ImageWidth) & "px;"" data-thumb=""" & row.ImageId & """ alt=""" & row.Headline & """  title=""" & row.Headline & """ />"





                        out &= "</a>"
                    End If
                Catch ex As Exception

                End Try

            Next

            ltStories.Text = out
        End Sub






    End Class
End Namespace
