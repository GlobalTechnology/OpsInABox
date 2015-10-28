Imports DotNetNuke.Services.FileSystem

Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class AddDocument

        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        End Sub

        Protected Sub btnUpoadFiles_Click(sender As Object, e As System.EventArgs) Handles btnUpoadFiles.Click
            Try
                Dim folder As IFolderInfo
                If Not FolderManager.Instance.FolderExists(PortalId, "acDocuments") Then
                    folder = FolderManager.Instance.AddFolder(PortalId, "acDocuments")
                Else
                    folder = FolderManager.Instance.GetFolder(PortalId, "acDocuments")
                End If
                ' Get the HttpFileCollection
                Dim hfc As HttpFileCollection = Request.Files
                For i As Integer = 0 To hfc.Count - 1
                    Dim hpf As HttpPostedFile = hfc(i)
                    If hpf.ContentLength > 0 Then
                        ' hpf.SaveAs(Server.MapPath("MyFiles") & "\" & System.IO.Path.GetFileName(hpf.FileName))
                        'Need to add the files to the dnn file system
                        Dim theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.AddFile(folder, hpf.FileName, hpf.InputStream)
                        DocumentsController.InsertDocument(theFile.FileId, theFile.FileName, UserInfo.DisplayName, Settings) 'need to add permissions eventually
                    End If
                Next i
            Catch ex As Exception
            End Try
            Response.Redirect(NavigateURL())
        End Sub

    End Class
End Namespace
