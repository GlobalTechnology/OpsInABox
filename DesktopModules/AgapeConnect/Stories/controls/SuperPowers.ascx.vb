Imports Stories
Imports System.Linq
Partial Class DesktopModules_SuperPowers
    Inherits System.Web.UI.UserControl
    Private _cacheId As Integer

    <Bindable(True, BindingDirection.TwoWay)> <Browsable(True)> <Category("Common")> <Description("The DotNetNuke FileID")> <DefaultValue(0)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)> _
    Public Property CacheId() As Integer
        Get
            Return _cacheId
        End Get
        Set(ByVal value As Integer)
            _cacheId = value
           
        End Set
    End Property

    Private _superEditor As Boolean
    Public Property SuperEditor() As Boolean
        Get
            Return _superEditor
        End Get
        Set(ByVal value As Boolean)
            _superEditor = value
        End Set
    End Property

    Private _isBlocked As Boolean
    Public Property IsBlocked() As Boolean
        Get
            Return _isBlocked
        End Get
        Set(ByVal value As Boolean)
            _isBlocked = value
        End Set
    End Property

    Private _isBoosted As Boolean
    Public Property IsBoosted() As Boolean
        Get
            Return _isBoosted
        End Get
        Set(ByVal value As Boolean)
            _isBoosted = value
        End Set
    End Property

    Private _editUrl As String
    Public Property EditUrl() As String
        Get
            Return _editUrl
        End Get
        Set(ByVal value As String)
            _editUrl = value
        End Set
    End Property

    Private _translationGroupId As String = ""
    Public Property TranslationGroupId() As String
        Get
            Return _translationGroupId
        End Get
        Set(ByVal value As String)
            _translationGroupId = value
        End Set
    End Property

    Private _portalId As Integer
    Public Property PortalId() As Integer
        Get
            Return _portalId
        End Get
        Set(ByVal value As Integer)
            _portalId = value
        End Set
    End Property

    Private _tabModuleId As Integer
    Public Property TabModuleId() As Integer
        Get
            Return _tabModuleId
        End Get
        Set(ByVal value As Integer)
            _tabModuleId = value
        End Set
    End Property
    Public Sub SetControls()
        Dim d As New StoriesDataContext
     
        Dim theStory = From c In d.AP_Stories Where c.StoryId = CInt(Request.QueryString("StoryId"))

        If theStory.Count > 0 Then
            If theStory.First.IsVisible Then


                Dim theCache = From c In d.AP_Stories_Module_Channel_Caches Where c.CacheId = _cacheId

                If theCache.Count > 0 Then
                    If theCache.First.Block Then
                        lblPowerStatus.Text = "This story has been blocked, and won't appear in the channel feed."
                        IsBlocked = True
                    ElseIf Not theCache.First.BoostDate Is Nothing Then
                        If theCache.First.BoostDate >= Today Then
                            IsBoosted = True
                            lblPowerStatus.Text = "Boosted until " & theCache.First.BoostDate.Value.ToString("dd MMM yyyy")

                        End If
                    End If
                End If
                pnlBoostBlock.Visible = True
                pnlPublish.Visible = False

            Else
                lblPowerStatus.Text = "This story not yet been published, and won't appear in any channel feeds."
                pnlBoostBlock.Visible = False
                pnlPublish.Visible = True
            End If

        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Response.Redirect(_editUrl & "?StoryID=" & Request.QueryString("StoryID"))

    End Sub

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect(_editUrl)
    End Sub

    Protected Sub btnTranslate_Click(sender As Object, e As EventArgs) Handles btnTranslate.Click
        'Dim tg As Integer
        If String.IsNullOrEmpty(_translationGroupId) Then
            'generate new Translation group
            Dim d As New StoriesDataContext
            Dim maxTransGroupId = d.AP_Stories.Where(Function(x) x.PortalID = PortalId And Not x.TranslationGroup Is Nothing)
            If maxTransGroupId.Count = 0 Then
                _translationGroupId = 1
            Else
                _translationGroupId = maxTransGroupId.Max(Function(x) x.TranslationGroup)
            End If
            'Need to apply  the translation Group to the Current Story

            Dim theStory = From c In d.AP_Stories Where c.StoryId = CInt(Request.QueryString("StoryId"))

            If theStory.Count > 0 Then
                theStory.First.TranslationGroup = _translationGroupId
                d.SubmitChanges()

            End If



        Else
            _translationGroupId = CInt(TranslationGroupId)
        End If



        Response.Redirect(_editUrl & "?tg=" & _translationGroupId)
    End Sub

    Protected Sub btnPublish_Click(sender As Object, e As EventArgs) Handles btnPublish.Click
        StoryFunctions.PublishStory(Request.QueryString("StoryId"))
        SetControls()
       
    End Sub
End Class
