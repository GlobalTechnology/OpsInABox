Imports DotNetNuke.Services.FileSystem

Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class Documents
        Inherits Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable
        'Dim d As New DocumentsDataContext()
        'Public templateMode As String = "Icons"

        'Dim rc As New DotNetNuke.Security.Roles.RoleController
        'Dim UserRoles As ArrayList
        'Dim doc As Object
        'Dim GTreeColor As String
        'Dim TreeStyles() As String = {"Explorer", "GTree", "Tree"}

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                LoadFolder()
        Protected Sub LoadFolder()
            Dim FolderId As Integer = DocumentsController.GetRootFolderId(Settings)
            Dim Items As New ArrayList
            Dim rc As New DotNetNuke.Security.Roles.RoleController
            'Dim UserRoles = rc.GetUserRoles(PortalId, UserId)
            Dim Docs = DocumentsController.GetDocuments(Settings)
            For Each document In Docs
                If document.Trashed = False Then
                    Items.Add(document)
            Dim Docs = DocumentsController.GetDocuments(Settings)
            For Each document In Docs
                If document.Trashed = False Then
                    Items.Add(document)
            Next
            dlFolderView.DataSource = Items
            dlFolderView.DataBind()
        Public Function GetIcon(ByVal FileId As Integer?, ByVal Folderid As Integer) As String
            Return DocumentsController.GetFileIcon(FileId, 4)
        End Function

        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
        End Function

        Public Function GetFileDate(ByVal FileId As Integer) As String
            Return DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileId).LastModificationTime.ToString("dd MMM yyyy")

        End Function
            If FileId = -2 Then 'the file is a link
                Dim theDoc = DocumentsController.GetDocument(DocId)
                Select Case theDoc.LinkType
            If FileId = -2 Then 'the file is a link
                Dim theDoc = DocumentsController.GetDocument(DocId)
                Select Case theDoc.LinkType
                        Return theDoc.LinkValue
                        Return "https://www.youtube.com"
                        Return NavigateURL(CInt(theDoc.LinkValue))
                Dim rtn = EditUrl("DocumentViewer")
            Dim btndeletedoc As HyperLink = CType(e.Item.FindControl("btnDeleteDoc"), HyperLink)
            Dim docbuttons As HtmlGenericControl = CType(e.Item.FindControl("docButtons"), HtmlGenericControl)
            btndeletedoc.NavigateUrl = "javascript:deleteButtonClick(" & hyperlink1.ClientID & ")"
            docbuttons.Visible = IsEditable
#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Translate("DocumentsSettings"), "DocumentSettings", "", "action_settings.gif", EditUrl("DocumentSettings"), False, SecurityAccessLevel.Admin, True, False)
                Actions.Add(GetNextActionID, Translate("AddDocuemnt"), "AddDocument", "", "action_settings.gif", EditUrl("AddDocument"), False, SecurityAccessLevel.Edit, True, False)
                Return Actions
#End Region
                    rtn &= "?DocId=" & DocId
                End If
                Return rtn
            Else
                Return EditUrl("DocumentViewer") & "?DocId=" & DocId
            End If
        End Function

        Protected Sub dlFolderView_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles dlFolderView.ItemDataBound
            Dim btneditdoc As HyperLink = CType(e.Item.FindControl("btnEditDoc"), HyperLink)
            Dim btndeletedoc As HyperLink = CType(e.Item.FindControl("btnDeleteDoc"), HyperLink)
            Dim hyperlink1 As HyperLink = CType(e.Item.FindControl("HyperLink1"), HyperLink)
            Dim docbuttons As HtmlGenericControl = CType(e.Item.FindControl("docButtons"), HtmlGenericControl)
            btneditdoc.NavigateUrl = "javascript:editButtonClick(" & hyperlink1.ClientID & ")"
            btndeletedoc.NavigateUrl = "javascript:deleteButtonClick(" & hyperlink1.ClientID & ")"
            docbuttons.Visible = IsEditable
        End Sub

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Translate("DocumentsSettings"), "DocumentSettings", "", "action_settings.gif", EditUrl("DocumentSettings"), False, SecurityAccessLevel.Admin, True, False)
                Actions.Add(GetNextActionID, Translate("AddDocuemnt"), "AddDocument", "", "action_settings.gif", EditUrl("AddDocument"), False, SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property
#End Region
    End Class
End Namespace
