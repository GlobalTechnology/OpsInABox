Imports Stories
Imports System.Linq
Partial Class DesktopModules_SuperPowers
    Inherits Entities.Modules.PortalModuleBase
    Private _cacheId As Integer

    <Bindable(True, BindingDirection.TwoWay)> <Browsable(True)> <Category("Common")> <Description("The DotNetNuke FileID")> <DefaultValue(0)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
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

    Public Sub SetControls(ByVal story As AP_Story)

        If story.IsVisible Then

            Dim theCache = StoryFunctions.GetCacheByStoryId(story.StoryId, story.TabModuleId)

            If (Not String.IsNullOrEmpty(theCache.Headline)) Then
                If theCache.Block Then
                    lblPowerStatus.Text = Translate("Blocked")
                    IsBlocked = True
                ElseIf Not theCache.BoostDate Is Nothing Then
                    If theCache.BoostDate >= Today Then
                        IsBoosted = True
                        lblPowerStatus.Text = Translate("BoostDate") & theCache.BoostDate.Value.ToString("dd MMM yyyy")

                    End If
                End If
            End If
            pnlBoostBlock.Visible = True
            btnPublish.Visible = False
            btnUnPublish.Visible = True
        Else
            lblPowerStatus.Text = Translate("NotPublished")
            pnlBoostBlock.Visible = False
            btnPublish.Visible = True
            btnUnPublish.Visible = False
        End If

    End Sub

    Protected Sub Edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Response.Redirect(_editUrl & "?StoryID=" & Request.QueryString("StoryID"))
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect(_editUrl)
    End Sub

    Protected Sub btnPublish_Click(sender As Object, e As EventArgs) Handles btnPublish.Click
        If StoryFunctions.PublishStory(Request.QueryString("StoryId")) Then
            lblPowerStatus.Visible = False

        Else
            lblPowerStatus.Text = LocalizeString("NoPhoto")
            lblPowerStatus.Visible = True
        End If
    End Sub

    Protected Sub btnUnpublish_Click(sender As Object, e As EventArgs) Handles btnUnpublish.Click
        AgapeLogger.Warn(UserId, "inside unpublish")
    End Sub

#Region "HelperFunctions"
    Public Function Translate(ByVal ResourceString As String) As String

        Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
    End Function
#End Region 'HelperFunctions
End Class
