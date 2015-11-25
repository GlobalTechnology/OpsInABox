Imports Documents
Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage


Namespace DotNetNuke.Modules.AgapeConnect.Documents

    Partial Class DocumentViewer
        Inherits Entities.Modules.PortalModuleBase

#Region "Page properties"

        'DocId retrieved in request
        Protected ReadOnly Property DocId() As Nullable(Of Integer)
            Get
                If String.IsNullOrEmpty(Request.QueryString(DocumentsControllerConstants.DocIdParamKey)) Then
                    Return Nothing
                Else
                    Return CInt(HttpUtility.UrlDecode(Request.QueryString(DocumentsControllerConstants.DocIdParamKey)))
                End If
            End Get
        End Property

        'YouTube ID to be used in YouTube video URL
        Private _YouTubeID As String = ""
        Protected Property YouTubeID() As String
            Get
                Return _YouTubeID
            End Get
            Set(ByVal value As String)
                _YouTubeID = value
            End Set
        End Property

#End Region 'Page properties

#Region "Page events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' Translate error messages
            Dim GENERIC_ERROR_MSG As String = LocalizeString("GENERIC_ERROR_MSG")
            Dim NOT_AUTHORIZED_ERROR_MSG As String = LocalizeString("NOT_AUTHORIZED_ERROR_MSG")

            ' Display error message if no DocId was provided
            If Not DocId.HasValue Then

                'Display error message
                AddModuleMessage(Me, GENERIC_ERROR_MSG, ModuleMessageType.RedError)

                'Log error
                AgapeLogger.Warn(UserId, GENERIC_ERROR_MSG + " - No DocId in request")

                Return

            End If

            'Retrieve info for the doc to view
            Try
                Dim theDoc = DocumentsController.GetDocument(DocId)

                ' Check if user has permissions to view the ressource
                If Not DocumentsController.IsAuthorized(UserId, DocId) Then

                    'Display error message
                    AddModuleMessage(Me, NOT_AUTHORIZED_ERROR_MSG, ModuleMessageType.RedError)

                    'Log error
                    AgapeLogger.Warn(UserId, NOT_AUTHORIZED_ERROR_MSG + " - User not authorized for DocId '" + DocId + "'")

                    Return

                End If

                ' Only YouTube videos are handled in this viewer => Display error message otherwise
                If Not theDoc.LinkType = DocumentConstants.LinkTypeYouTube Then

                    'Display error message
                    AddModuleMessage(Me, GENERIC_ERROR_MSG, ModuleMessageType.RedError)

                    'Log error
                    AgapeLogger.Warn(UserId, GENERIC_ERROR_MSG + " - Unhandled LinkType '" + theDoc.LinkType + "'")

                    Return

                End If

                'Set page title with ressource name
                CType(Page, DotNetNuke.Framework.CDefault).Title = theDoc.DisplayName

                'Set YouTubeID property to be used to generate YouTube video URL
                YouTubeID = theDoc.LinkValue

                ' Show the video panel
                pnlVideo.Visible = True

            Catch ex As Exception 'Unexisting DocId throws an exception when calling GetDocument

                'Display error message
                AddModuleMessage(Me, GENERIC_ERROR_MSG, ModuleMessageType.RedError)

                'Log error
                AgapeLogger.Warn(UserId, GENERIC_ERROR_MSG + " - Exception: " + ex.Message + " - " + ex.StackTrace)

                Return

            End Try

        End Sub

#End Region 'Page events

    End Class

End Namespace
