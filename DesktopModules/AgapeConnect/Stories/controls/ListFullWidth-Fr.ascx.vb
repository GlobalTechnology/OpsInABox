Imports Stories

Namespace DotNetNuke.Modules.AgapeConnect.Stories
    Partial Class ListFullWidth_Fr
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

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Add tag list to page title (to be displayed on the browser tag and on the page blue rectangle)
            TabController.CurrentPage.TabName = TabController.CurrentPage.TabName + " " +
                StoryFunctions.FormatTagsSelected(Request.QueryString(TAGS_KEYWORD))
        End Sub

        Public Sub Initialize(ByVal stories As List(Of AP_Stories_Module_Channel_Cache), settings As Hashtable)

            Dim skip As Integer = 0
            Dim pg As Integer = 0
            If Not String.IsNullOrEmpty(Request.QueryString("p")) Then
                pg = Request.QueryString("p")
                skip = pg * CInt(settings(ControlerConstants.NUMSTORIES))
            End If

            Dim listData As DataTable = StoryFunctions.GetListData(stories.Skip(skip).Take(CInt(settings(ControlerConstants.NUMSTORIES))),
                                                                   PortalSettings.DefaultPortalAlias)

            dlStories.DataSource = listData
            dlStories.DataBind()

            'Pagination handling
            If stories.Count > CInt(settings(ControlerConstants.NUMSTORIES)) Then
                btnPrev.Visible = True
                btnNext.Visible = True
                Dim urlStub = NavigateURL()

                'Construct the URLs for btnPrev and btnNext
                If (Request.QueryString(TAGS_KEYWORD) <> "") Then
                    urlStub &= TAGS_IN_URL & Request.QueryString(TAGS_KEYWORD).ToString & "&p="
                Else
                    urlStub &= "?p="
                End If

                btnPrev.NavigateUrl = urlStub & (pg - 1)
                btnNext.NavigateUrl = urlStub & (pg + 1)

                'determine state of btnPrev
                If (pg = 0) Then
                    btnPrev.Style.Add("opacity", "0.5")
                    btnPrev.Enabled = False
                Else
                    btnPrev.Enabled = True
                End If

                'determine state of btnNext
                If (Math.Floor((stories.Count - 1) / CInt(settings(ControlerConstants.NUMSTORIES))) > pg) Then
                    btnNext.Enabled = True
                Else
                    btnNext.Style.Add("opacity", "0.5")
                    btnNext.Enabled = False
                End If
            End If 'End Pagination handling

        End Sub
    End Class
End Namespace
