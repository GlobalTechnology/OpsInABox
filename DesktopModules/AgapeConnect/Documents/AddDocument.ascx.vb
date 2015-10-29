Imports DotNetNuke.Services.FileSystem

Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class AddDocument

        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        End Sub

        Protected Sub btnUploadFiles_Click(sender As Object, e As System.EventArgs) Handles btnUploadFiles.Click
            Try
                Dim folder As IFolderInfo
                If Not FolderManager.Instance.FolderExists(PortalId, "acDocuments") Then
                    folder = FolderManager.Instance.AddFolder(PortalId, "acDocuments")
                Else
                    folder = FolderManager.Instance.GetFolder(PortalId, "acDocuments")
                End If

                If (FileUpload1.HasFile) Then
                    'Need to add the files to the dnn file system
                    Dim theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.AddFile(folder, FileUpload1.FileName, FileUpload1.FileContent)
                    'Now instert the document into the database
                    DocumentsController.InsertDocument(theFile.FileId, tbName.Text, UserInfo.DisplayName, Settings, tbDescription.Text) 'need to add permissions eventually
                End If
            Catch ex As Exception
            End Try
            Response.Redirect(NavigateURL())
        End Sub

    End Class
End Namespace
