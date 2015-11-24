Imports DotNetNuke
Imports System.Web.UI
Imports Documents

Namespace DotNetNuke.Modules.AgapeConnect.Documents

    Partial Class DocumentSettings
        Inherits Entities.Modules.ModuleSettingsBase

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                CType(Page, DotNetNuke.Framework.CDefault).Title = "Configuration"

                If Not IsPostBack Then

                    'Translate the action buttons tooltips
                    btnEdit.ToolTip = LocalizeString("btnEdit")
                    btnDelete.ToolTip = LocalizeString("btnDelete")
                    btnAdd.ToolTip = LocalizeString("btnAdd")

                    BuildPathList()

                End If                       'IsPostBack

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Sub BuildPathList()
            Dim pathName As String = ""

            Dim folders As List(Of AP_Documents_Folder) = DocumentsController.GetFolders()

            'Name is replaced by path
            For Each folder In folders
                folder.Name = DocumentsController.GetFullPath(folder)
            Next

            ddlRoot.DataSource = folders.OrderBy(Function(x) x.Name).ToList()
            ddlRoot.DataTextField = "Name"
            ddlRoot.DataValueField = "FolderId"
            ddlRoot.DataBind()

            'Populate the dropdown list with correct path 
            ddlRoot.SelectedValue = DocumentsController.GetModuleFolderId(TabModuleId)

        End Sub

#Region "Events"

        Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
            upAdd.Visible = False
            If Page.IsValid Then
                upEdit.Visible = True
                tbEditSubFolder.Text = DocumentsController.GetFolder(ddlRoot.SelectedValue).Name
            End If
        End Sub

        Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            upEdit.Visible = False
            upAdd.Visible = False

            If Page.IsValid Then
                DocumentsController.DeleteFolder(ddlRoot.SelectedItem.Value)

                'Rebuild the list of paths after directory was deleted
                BuildPathList()
            End If

        End Sub

        Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
            upEdit.Visible = False
            upAdd.Visible = True
        End Sub

        Protected Sub btnEditSubFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditSubFolder.Click

            If Page.IsValid Then
                DocumentsController.UpdateFolder(tbEditSubFolder.Text, ddlRoot.SelectedValue)

                'Rebuild the list of paths after a new directory was added
                BuildPathList()
            End If
            tbEditSubFolder.Text = ""
            upEdit.Visible = False
        End Sub

        Protected Sub btnAddSubFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddSubFolder.Click

            If Page.IsValid Then
                DocumentsController.InsertFolder(tbAddSubFolder.Text, ddlRoot.SelectedValue)

                'Rebuild the list of paths after a new directory was added
                BuildPathList()
            End If
            tbAddSubFolder.Text = ""
            upAdd.Visible = False
        End Sub

        Protected Sub ddlRoot_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRoot.SelectedIndexChanged
            If (upEdit.Visible And DocumentsController.HasParentFolder(ddlRoot.SelectedItem.Value)) Then
                tbEditSubFolder.Text = DocumentsController.GetFolder(ddlRoot.SelectedValue).Name
            Else
                upEdit.Visible = False
            End If
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

            'update module folder in settings
            DocumentsController.SetModuleFolderId(TabModuleId, ddlRoot.SelectedValue)

            'go back to documents main window
            Response.Redirect(NavigateURL())

        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
            Response.Redirect(NavigateURL())
        End Sub

#End Region 'Events

#Region "Validators"

        Protected Sub IsFolderRenamable(sender As Object, e As ServerValidateEventArgs)
            If (DocumentsController.HasParentFolder(ddlRoot.SelectedItem.Value)) Then
                e.IsValid = True
            Else
                e.IsValid = False
            End If
        End Sub

        Protected Sub EditFolder(sender As Object, e As ServerValidateEventArgs)
            'in an Edit the parent folder is one step to the left of what is chosen in the dropdownlist
            Dim parentFolder As Integer = DocumentsController.GetFolder(ddlRoot.SelectedValue).ParentFolder
            e.IsValid = Not DocumentsController.IsFolder(tbEditSubFolder.Text, parentFolder)

        End Sub

        Protected Sub AddFolder(sender As Object, e As ServerValidateEventArgs)
            'IsValid should be true when folder does not exist so boolean is reversed here
            e.IsValid = Not DocumentsController.IsFolder(tbAddSubFolder.Text, ddlRoot.SelectedValue)

        End Sub

        Protected Sub IsFolderDeletable(sender As Object, e As ServerValidateEventArgs)

            If (DocumentsController.IsFolderEmpty(ddlRoot.SelectedItem.Value) And _
                DocumentsController.HasParentFolder(ddlRoot.SelectedItem.Value)) Then
                e.IsValid = True
            Else
                e.IsValid = False
            End If
        End Sub

#End Region 'Validators

    End Class

End Namespace

