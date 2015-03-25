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
    Partial Class List_Fr
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
                btnPrev.Visible = True
                btnNext.Visible = True



                Dim urlStub = NavigateURL()
                If String.IsNullOrEmpty(Request.QueryString("tags")) Then
                    urlStub &= "?p="
                Else
                    urlStub &= "?tabs=" & Request.QueryString("tags") & "&p="
                End If

                btnPrev.Enabled = Not (pg = 0)

                If (pg = 0) Then
                    btnPrev.Style.Add("opacity", "0.5")
                End If

                btnPrev.NavigateUrl = urlStub & (pg - 1)
                btnNext.NavigateUrl = urlStub & (pg + 1)
                btnNext.Enabled = (Math.Floor((Stories.Count - 1) / CInt(settings("NumberOfStories"))) > pg)
                If Not btnNext.Enabled Then
                    btnNext.Style.Add("opacity", "0.5")
                End If
            End If






            Dim d As New StoriesDataContext
            Dim tags = From c In d.AP_Stories_Tags Where c.PortalId = PortalId And c.Master

            dlFilter.DataSource = tags
            dlFilter.DataBind()

        End Sub

    End Class
End Namespace
