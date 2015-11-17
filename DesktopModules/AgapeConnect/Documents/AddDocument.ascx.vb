Imports DotNetNuke.Services.FileSystem
Imports Telerik.Web.UI

Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class AddDocument
        Inherits Entities.Modules.PortalModuleBase

#Region "Page properties"

        'DocId retrieved in request if in edit mode (initialized in Page_Load)
        Protected Property DocId() As Integer
            Get
                Return ViewState("DocId") 'Value stored in Viewstate to be able to get it on button click events as well
            End Get

            Set(ByVal value As Integer)
                ViewState("DocId") = value
            End Set
        End Property
        Protected ReadOnly Property IsEditMode() As Boolean 'If no DocId then Add mode, otherwise Edit mode
            Get
                Return IIf(ViewState("DocId") Is Nothing, False, True)
            End Get
        End Property

#End Region 'Page properties

#Region "Page events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then

                ' Get DocId from request if provided
                If Not String.IsNullOrEmpty(Request.QueryString("edit")) Then
                    DocId = CInt(HttpUtility.UrlDecode(Request.QueryString("edit")))
                End If

                ' Load list of portal pages
                BuildDdlPages()

                ' Init resource types
                'TODO: Items to be translated
                'TODO: Si c'est déjà un fichier, ajouter l'option "Garder le fichier actuel"
                rbLinkType.Items.Add(New ListItem("Upload a new file", DocumentConstants.LinkTypeFile))
                rbLinkType.Items.Add(New ListItem("Google Doc", DocumentConstants.LinkTypeGoogleDoc))
                rbLinkType.Items.Add(New ListItem("External URL", DocumentConstants.LinkTypeUrl))
                rbLinkType.Items.Add(New ListItem("A Page on this site", DocumentConstants.LinkTypePage))
                rbLinkType.Items.Add(New ListItem("YouTube Video", DocumentConstants.LinkTypeYouTube))

                If IsEditMode Then
                    ' Set page title for edit mode
                    CType(Page, DotNetNuke.Framework.CDefault).Title = "Edit Resource" 'TODO: Title to be translated

                    ' Fill in existing values for edited resource
                    Dim editDoc = DocumentsController.GetDocument(DocId)
                    tbName.Text = editDoc.DisplayName
                    tbDescription.Text = editDoc.Description
                    rbLinkType.SelectedValue = editDoc.LinkType
                    'TODO: Select the right Option panel to display and set the corresponding value to edit
                Else
                    ' Set page title for add mode
                    CType(Page, DotNetNuke.Framework.CDefault).Title = "Add Resource" 'TODO: Title to be translated
                End If

            End If

        End Sub

        Protected Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
            If IsEditMode Then
                UpdateResource()
            Else 'Add mode
                AddResource()
            End If

            Response.Redirect(NavigateURL()) 'Close modal popup and refresh Resource list
        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
            Response.Redirect(NavigateURL()) 'Close modal popup and go back to Resource list
        End Sub

#End Region 'Page events

#Region "Object construction"

        ' Build the DropDownList list of portal pages
        Protected Sub BuildDdlPages()

            'Code inpired from https://radeditor.codeplex.com/SourceControl/latest#DNN_6/Admin/RadEditorProvider/Components/PageDropDownList.vb

            Dim userInfo As DotNetNuke.Entities.Users.UserInfo = UserController.Instance.GetCurrentUserInfo()
            If (Not IsNothing(userInfo) AndAlso userInfo.UserID <> Null.NullInteger) Then
                'check view permissions - Yes?
                Dim _PortalSettings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings()
                Dim pageCulture As String = _PortalSettings.ActiveTab.CultureCode
                If String.IsNullOrEmpty(pageCulture) Then
                    pageCulture = PortalController.GetActivePortalLanguage(_PortalSettings.PortalId)
                End If

                Dim tabs As List(Of Entities.Tabs.TabInfo) = TabController.GetTabsBySortOrder(_PortalSettings.PortalId, pageCulture, True)
                Dim sortedTabList = TabController.GetPortalTabs(tabs, Null.NullInteger, False, Null.NullString, True, False, True, True, True)

                For Each _tab In sortedTabList
                    Dim tabItem As New RadComboBoxItem(_tab.IndentedTabName, _tab.TabID)
                    tabItem.Enabled = Not _tab.DisableLink

                    ddlPages.Items.Add(tabItem)
                Next
            End If

        End Sub

#End Region 'Object construction

#Region "Add/update resource"

        Protected Sub AddResource()
            If rbLinkType.SelectedValue = DocumentConstants.LinkTypeFile Then 'Radio button selected was upload
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
                        DocumentsController.InsertResource(theFile.FileId, tbName.Text, userInfo.DisplayName, DocumentConstants.LinkTypeFile, "", "False", TabModuleId, tbDescription.Text) 'need to add permissions eventually
                    End If
                Catch ex As Exception
                End Try
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeGoogleDoc Then 'Radio button selected was Google Doc
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, userInfo.DisplayName, DocumentConstants.LinkTypeGoogleDoc, tbGoogle.Text, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeUrl Then 'Radio button selected was external URL
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, userInfo.DisplayName, DocumentConstants.LinkTypeUrl, tbURL.Text, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypePage Then 'Radio button selected was internal page
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, userInfo.DisplayName, DocumentConstants.LinkTypePage, ddlPages.SelectedValue, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeYouTube Then 'Radio button selected was youtube
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, userInfo.DisplayName, DocumentConstants.LinkTypeYouTube, tbYouTube.Text, "False", TabModuleId, tbDescription.Text)
            End If
        End Sub

        Protected Sub UpdateResource()
            If rbLinkType.SelectedValue = DocumentConstants.LinkTypeFile Then 'Radio button selected was upload
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
                        'Now update the document into the database
                        DocumentsController.UpdateResource(DocId, theFile.FileId, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeFile, "", "False", TabModuleId, tbDescription.Text) 'need to add permissions eventually
                    End If
                Catch ex As Exception
                End Try
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeGoogleDoc Then 'Radio button selected was Google Doc
                DocumentsController.UpdateResource(DocId, DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeGoogleDoc, tbGoogle.Text, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeUrl Then 'Radio button selected was external URL
                DocumentsController.UpdateResource(DocId, DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeUrl, tbURL.Text, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypePage Then 'Radio button selected was internal page
                DocumentsController.UpdateResource(DocId, DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypePage, ddlPages.SelectedValue, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeYouTube Then 'Radio button selected was youtube
                DocumentsController.UpdateResource(DocId, DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeYouTube, tbYouTube.Text, "False", TabModuleId, tbDescription.Text)
            End If
        End Sub

#End Region 'Add/update resource

    End Class
End Namespace
