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
            End If
        End Sub

        Protected Sub LoadFolder()
            Dim FolderId As Integer = DocumentsController.GetRootFolderId(Settings)
            Dim Items As New ArrayList
            Dim rc As New DotNetNuke.Security.Roles.RoleController
            'Dim UserRoles = rc.GetUserRoles(PortalId, UserId)
            Dim Docs = DocumentsController.GetDocuments(Settings)
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

        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
        End Function

        Public Function GetFileDate(ByVal FileId As Integer) As String
            Return DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileId).LastModificationTime.ToString("dd MMM yyyy")

        End Function

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Translate("DocumentsSettings"), "DocumentSettings", "", "action_settings.gif", EditUrl("DocumentSettings"), False, SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property
#End Region
    End Class
End Namespace
