Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Framework.JavaScriptLibraries
Imports DotNetNuke.UI.Utilities
Imports Documents

Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class Documents
        Inherits Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable

#Region "Constants"
        'Command names for actions event handlers
        Private Const DELETE_DOC_COMMAND_NAME As String = "DeleteDoc"

#End Region 'Constants

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            ' Register DNN Jquery plugins
            ClientAPI.RegisterClientReference(Me.Page, ClientAPI.ClientNamespaceReferences.dnn)
            JavaScript.RequestRegistration(CommonJs.DnnPlugins)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                LoadDocuments()
            End If
        End Sub

        Protected Sub LoadDocuments()
            Dim folderId As Integer = DocumentsController.GetModuleFolderId(TabModuleId)
            dlFolderView.DataSource = DocumentsController.GetDocuments(folderId, False, False) 'Get no trashed docs
            dlFolderView.DataBind()
        End Sub

        Public Function GetIcon(ByVal FileId As Integer?, ByVal Folderid As Integer) As String
            Return DocumentsController.GetFileIcon(FileId, 4)
        End Function

        Public Function GetDocUrl(ByVal DocId As Integer) As String
            Dim theDoc = DocumentsController.GetDocument(DocId)
            Select Case theDoc.LinkType
                Case DocumentConstants.LinkTypeUrl 'The document is an external URL
                    Return theDoc.LinkValue
                Case DocumentConstants.LinkTypeGoogleDoc 'The document is a Google Doc
                    Return theDoc.LinkValue
                Case DocumentConstants.LinkTypeYouTube 'The document is a YouTube video
                    'Displayed by the ViewDocument control in a modal popup
                    Return EditUrl("", "", DocumentsControllerConstants.ViewDocumentControlKey, DocumentsControllerConstants.DocIdParamKey, HttpUtility.UrlEncode(theDoc.DocId))
                Case DocumentConstants.LinkTypePage 'The document is a page on this site
                    Return NavigateURL(CInt(theDoc.LinkValue))
                Case DocumentConstants.LinkTypeFile 'The document is an uploaded file
                    Dim theFile = FileManager.Instance.GetFile(theDoc.FileId)
                    If Not theFile Is Nothing Then
                        Return FileManager.Instance.GetUrl(theFile)
                    End If
            End Select
            Return ""
        End Function

        Public Function GetDocTarget(ByVal DocId As Integer) As String
            Const _BLANK As String = "_blank"
            Const _SELF As String = "_self"

            Dim theDoc = DocumentsController.GetDocument(DocId)
            Select Case theDoc.LinkType
                Case DocumentConstants.LinkTypeUrl 'The document is an external URL
                    Return _BLANK
                Case DocumentConstants.LinkTypeGoogleDoc 'The document is a Google Doc
                    Return _BLANK
                Case DocumentConstants.LinkTypeYouTube 'The document is a YouTube video
                    'Displayed in a modal popup on the same page
                    Return _SELF
                Case DocumentConstants.LinkTypePage 'The document is a page on this site
                    Return _SELF
                Case DocumentConstants.LinkTypeFile 'The document is an uploaded file
                    Return _BLANK
            End Select
            Return _SELF
        End Function

        Protected Sub dlFolderView_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles dlFolderView.ItemDataBound
            'For each resource

            Dim btneditdoc As HyperLink = CType(e.Item.FindControl("btnEditDoc"), HyperLink)
            Dim btndeletedoc As LinkButton = CType(e.Item.FindControl("btnDeleteDoc"), LinkButton)
            Dim hyperlink1 As HyperLink = CType(e.Item.FindControl("HyperLink1"), HyperLink)
            Dim docbuttons As HtmlGenericControl = CType(e.Item.FindControl("docButtons"), HtmlGenericControl)

            'Translate the action buttons tooltips
            btneditdoc.ToolTip = LocalizeString("btnEditDoc")
            btndeletedoc.ToolTip = LocalizeString("btnDeleteDoc")

            'Configure "Delete resource" command
            btndeletedoc.CommandName = DELETE_DOC_COMMAND_NAME

            'Show edition buttons in Edit mode
            docbuttons.Visible = IsEditable
        End Sub

        Protected Sub dlFolderView_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles dlFolderView.ItemCommand
            'Handle "Delete resource" action
            If e.CommandName = DELETE_DOC_COMMAND_NAME Then
                DocumentsController.DeleteDocument(CType(e.CommandArgument, Integer)) 'e.CommandArgument contains the DocId
            End If

            'Reload the documents to update the list view
            LoadDocuments()
        End Sub

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, LocalizeString("DocumentSettingsAction"), DocumentsControllerConstants.DocumentSettingsControlKey, "", "action_settings.gif", EditUrl(DocumentsControllerConstants.DocumentSettingsControlKey), False, SecurityAccessLevel.Admin, True, False)
                Actions.Add(GetNextActionID, LocalizeString("AddDocumentAction"), DocumentsControllerConstants.AddEditDocumentControlKey, "", "action_settings.gif", EditUrl(DocumentsControllerConstants.AddEditDocumentControlKey), False, SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property
#End Region

    End Class
End Namespace
