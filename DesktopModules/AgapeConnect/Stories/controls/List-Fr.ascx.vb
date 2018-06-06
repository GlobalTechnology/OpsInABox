Imports Stories
Namespace DotNetNuke.Modules.AgapeConnect.Stories
    Partial Class List_Fr
        Inherits Entities.Modules.PortalModuleBase
        'Adding Stories Translation
        Dim d As New StoriesDataContext

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
                    urlStub &= "?tags=" & Request.QueryString("tags") & "&p="
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
        End Sub

        Public Function GetTypeText(ByVal StoryId As String) As String
            Dim d As New StoriesDataContext
            Dim q = From c In d.AP_Stories Where c.StoryId.ToString = StoryId And c.AP_Stories_Tag_Metas.Where(Function(x) x.AP_Stories_Tag.TagName = "Evénement").Count > 0

            Return IIf(q.Count > 0, "Evénement", "Article")

        End Function

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
