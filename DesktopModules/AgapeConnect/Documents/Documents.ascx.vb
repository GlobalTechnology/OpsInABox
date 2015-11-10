Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Framework.JavaScriptLibraries
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class Documents
        Inherits Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            ' Register DNN Jquery plugins
            ClientAPI.RegisterClientReference(Me.Page, ClientAPI.ClientNamespaceReferences.dnn)
            JavaScript.RequestRegistration(CommonJs.DnnPlugins)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                LoadFolder()
            End If
        End Sub

        Protected Sub LoadFolder()
            Dim FolderId As Integer = DocumentsController.GetModuleFolderId(TabModuleId)
            Dim Items As New ArrayList
            Dim rc As New DotNetNuke.Security.Roles.RoleController
            'TODO: Modify GetDocuments in Controller to add a boolean parameter 'includeTrashed' and delete loop below
            Dim Docs = DocumentsController.GetDocuments(TabModuleId)
            For Each document In Docs
                If document.Trashed = False Then
                    Items.Add(document)
                End If
            Next
            dlFolderView.DataSource = Items
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
                    'TODO: This should be displayed by the DocumentViewer in an iframe
                    Return theDoc.LinkValue
                Case DocumentConstants.LinkTypeYouTube 'The document is a YouTube video
                    'TODO: This should be displayed by the DocumentViewer in an iframe
                    Return "https://www.youtube.com/watch?v=" & theDoc.LinkValue
                    'Dim rtn = EditUrl("DocumentViewer")
                    'If rtn.Contains("?") Then
                    '    rtn &= "&DocId=" & DocId
                    'Else
                    '    rtn &= "?DocId=" & DocId
                    'End If
                    'Return rtn
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
                    'TODO: This should be displayed by the DocumentViewer in an iframe
                    Return _BLANK
                Case DocumentConstants.LinkTypeYouTube 'The document is a YouTube video
                    'TODO: This should be displayed by the DocumentViewer in an iframe
                    Return _BLANK
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

            btneditdoc.NavigateUrl = "javascript:editButtonClick('" & hyperlink1.ClientID & "')"
            docbuttons.Visible = IsEditable
        End Sub

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, LocalizeString("DocumentSettingsAction"), "DocumentSettings", "", "action_settings.gif", EditUrl("DocumentSettings"), False, SecurityAccessLevel.Admin, True, False)
                Actions.Add(GetNextActionID, LocalizeString("AddDocumentAction"), "AddDocument", "", "action_settings.gif", EditUrl("AddDocument"), False, SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property
#End Region
    End Class
End Namespace
