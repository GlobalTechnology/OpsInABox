Imports DotNetNuke.Services.FileSystem

Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class AddDocument

        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim pages = TabController.GetPortalTabs(PortalId, TabId, False, False)
                ddlPages.DataSource = pages.OrderBy(Function(x) x.TabName)
                ddlPages.DataTextField = "TabName"
                ddlPages.DataValueField = "TabId"
                ddlPages.DataBind()
                rbLinkType.Items.Add(New ListItem("Upload a new file", DocumentConstants.LinkTypeFile))
                rbLinkType.Items.Add(New ListItem("Google Doc", DocumentConstants.LinkTypeGoogleDoc))
                rbLinkType.Items.Add(New ListItem("External URL", DocumentConstants.LinkTypeUrl))
                rbLinkType.Items.Add(New ListItem("A Page on this site", DocumentConstants.LinkTypePage))
                rbLinkType.Items.Add(New ListItem("YouTube Video", DocumentConstants.LinkTypeYouTube))
            End If
            If Request.QueryString("edit") <> "" Then
                CType(Page, DotNetNuke.Framework.CDefault).Title = "Edit Resource"
                Dim editDoc = DocumentsController.GetDocument(Request.QueryString("edit"))
                tbName.Text = editDoc.DisplayName
                tbDescription.Text = editDoc.Description
                rbLinkType.SelectedValue = editDoc.LinkType
            Else
                CType(Page, DotNetNuke.Framework.CDefault).Title = "Add Resource"
            End If
        End Sub

        Protected Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
            If rbLinkType.SelectedValue = 0 Then 'Radio button selected was upload
                Try
                    'TODO: Externalize in DocumentsController
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
                        DocumentsController.InsertResource(theFile.FileId, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeFile, "", "False", TabModuleId, tbDescription.Text) 'need to add permissions eventually
                    End If
                Catch ex As Exception
                End Try
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeGoogleDoc Then 'Radio button selected was Google Doc
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeGoogleDoc, tbGoogle.Text, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeUrl Then 'Radio button selected was external URL
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeUrl, tbURL.Text, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypePage Then 'Radio button selected was internal page
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypePage, ddlPages.SelectedValue, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeYouTube Then 'Radio button selected was youtube
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeYouTube, tbYouTube.Text, "False", TabModuleId, tbDescription.Text)
            End If
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
            Response.Redirect(NavigateURL())
        End Sub



    End Class
End Namespace
