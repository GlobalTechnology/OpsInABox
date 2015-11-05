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

                    'Build the list of paths
                    Dim pathName As String = ""

                    For Each folder In DocumentsController.GetFolders()
                        pathName = DocumentsController.GetPathName(folder, pathName)
                        ddlRoot.Items.Add(New ListItem(pathName, folder.FolderId))
                        pathName = ""
                    Next

                    'Populate the dropdown list with correct path 
                    ddlRoot.SelectedValue = DocumentsController.GetModuleFolderId(TabModuleId)

                End If                       'IsPostBack

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click

            'update module folder in settings
            DocumentsController.SetModuleFolderId(TabModuleId, ddlRoot.SelectedValue)

            'go back to documents main window
            Response.Redirect(NavigateURL())

        End Sub


        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub


    End Class

End Namespace

