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
    Partial Class List1
        Inherits Entities.Modules.PortalModuleBase
        'Adding Stories Translation
        Dim d As New StoriesDataContext

        Public divWidth As Integer = 150
        Public divHeight As Integer = 150
        Public selectedTags As String() = {""}

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
            If Not String.IsNullOrEmpty(Request.QueryString("tags")) Then
                hfSelectedTags.Value = Request.QueryString("tags")
            Else
                hfSelectedTags.Value = ""
            End If
        End Sub

        Public Sub Initialize(ByVal Stories As List(Of AP_Stories_Module_Channel_Cache), settings As Hashtable)

            'Dim d As New StoriesDataContext



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
            If AspectMode > 2 Then
                AspectMode = 1  ' Ascpect Modes 3 and 4 are not valid with this rotator. It requires a fixed size.
            End If
            divWidth = photoWidth
            divHeight = photoHeight

            Dim Skip As Integer = 0
            Dim pg As Integer = 0
            If Not String.IsNullOrEmpty(Request.QueryString("p")) Then
                pg = Request.QueryString("p")
                Skip = pg * CInt(settings("NumberOfStories"))
            End If


            dlStories.DataSource = Stories.Skip(Skip).Take(CInt(settings("NumberOfStories")))
            dlStories.DataBind()

            If Stories.Count > CInt(settings("NumberOfStories")) Then
                Dim urlStub = NavigateURL()
                If String.IsNullOrEmpty(Request.QueryString("tags")) Then
                    urlStub &= "?p="
                Else
                    urlStub &= "?tabs=" & Request.QueryString("tags") & "&p="
                End If


                Dim p As String = "<div class=""pagination pagination-centered""><ul>"
                If pg = 0 Then
                    p &= "<li class=""disabled""><a>Prev</a></li>"
                Else
                    p &= "<li><a href='" & urlStub & (pg - 1) & "'>Prev</a></li>"
                End If

                For i As Integer = 0 To CInt(Stories.Count / CInt(settings("NumberOfStories"))) - 1
                    If i = pg Then
                        p &= "<li class=""active""><a>" & (i + 1) & "</a></li>"
                    Else
                        p &= "<li><a href='" & urlStub & (i) & "'>" & (i + 1) & "</a></li>"
                    End If


                Next


                If pg = CInt(Stories.Count / CInt(settings("NumberOfStories"))) - 1 Then
                    p &= "<li class=""disabled""><a>Next</a></li>"
                Else
                    p &= "<li><a href='" & urlStub & (pg + 1) & "'>Next</a></li>"
                End If

                p &= "</ul></div>"
                ltPagination.Text = p
            End If





            ' PortalId is no longer in table AP_Stories_Tags

            'Dim d As New StoriesDataContext
            'Dim tags = From c In d.AP_Stories_Tags Where c.PortalId = PortalId And c.Master

            'dlFilter.DataSource = tags
            'dlFilter.DataBind()

        End Sub

        Public Function GetStoryDateString(ByVal StoryDate As Date, ByVal GUID As String, ByVal Link As String) As String
            Return StoryDate.ToString("dd MMM yyyy")
            Dim url = New Uri(Link)
            If NavigateURL.Contains(url.Authority) And Not String.IsNullOrEmpty(GUID) Then  ' local channel
                Dim d As New StoriesDataContext

                Dim q = From c In d.AP_Stories Where c.StoryId = CInt(GUID)

                If q.Count > 0 Then
                    If Not q.First.UpdatedDate Is Nothing Then
                        Return StoryDate.ToString("dd MMM yyyy") & " (updated " & q.First.UpdatedDate.Value.ToString("dd MMM yyyy") & ")"
                    End If

                End If

            End If

            Return StoryDate.ToString("dd MMM yyyy")



        End Function




    End Class
End Namespace
