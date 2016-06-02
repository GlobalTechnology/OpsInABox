Imports DotNetNuke.Services.FileSystem
Imports Telerik.Web.UI
Imports Documents
Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage

Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class AddEditDocument
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

        'SearchWords retrieved in request
        Protected ReadOnly Property SearchWords() As String
            Get
                If String.IsNullOrEmpty(Request.QueryString(DocumentsControllerConstants.SearchWordsParamKey)) Then
                    Return ""
                Else
                    Return HttpUtility.UrlDecode(Request.QueryString(DocumentsControllerConstants.SearchWordsParamKey))
                End If
            End Get
        End Property

#End Region 'Page properties

#Region "Page events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then

                Dim editDoc As AP_Documents_Doc 'Resource to be edited if edit mode

                ' Get DocId from request if provided (to be done first as needed afterwards)
                If Not String.IsNullOrEmpty(Request.QueryString(DocumentsControllerConstants.DocIdParamKey)) Then
                    DocId = CInt(HttpUtility.UrlDecode(Request.QueryString(DocumentsControllerConstants.DocIdParamKey)))

                    'Retrieve existing values for resource to be edited
                    editDoc = DocumentsController.GetDocument(DocId)
                End If

                ' Set page title for add mode or edit mode
                If IsEditMode Then
                    CType(Page, DotNetNuke.Framework.CDefault).Title = LocalizeString("TitleEditResource")
                Else
                    CType(Page, DotNetNuke.Framework.CDefault).Title = LocalizeString("TitleAddResource")
                End If

                ' Load list of portal pages
                BuildDdlPages()

                ' Init resource types

                'If edit mode and resource is already a file, add option to keep existing file.
                If IsEditMode AndAlso editDoc.LinkType = DocumentConstants.LinkTypeFile Then
                    rbLinkType.Items.Add(New ListItem(LocalizeString("rbKeepResource"), DocumentConstants.LinkTypeKeepFileForUpdate))
                End If
                rbLinkType.Items.Add(New ListItem(LocalizeString("rbUpload"), DocumentConstants.LinkTypeFile))
                rbLinkType.Items.Add(New ListItem(LocalizeString("rbGoogleDoc"), DocumentConstants.LinkTypeGoogleDoc))
                rbLinkType.Items.Add(New ListItem(LocalizeString("rbUrl"), DocumentConstants.LinkTypeUrl))
                rbLinkType.Items.Add(New ListItem(LocalizeString("rbPage"), DocumentConstants.LinkTypePage))
                rbLinkType.Items.Add(New ListItem(LocalizeString("rbYouTube"), DocumentConstants.LinkTypeYouTube))
                rbLinkType.SelectedValue = DocumentConstants.LinkTypeFile

                If IsEditMode Then
                    ' Fill in existing values for edited resource
                    tbName.Text = editDoc.DisplayName
                    tbDescription.Text = editDoc.Description

                    'Select the right Option panel to display and set the corresponding value to edit

                    If editDoc.LinkType = DocumentConstants.LinkTypeFile Then 'If type file, select option to keep existing file.
                        rbLinkType.SelectedValue = DocumentConstants.LinkTypeKeepFileForUpdate
                    Else 'select option according to link type
                        rbLinkType.SelectedValue = editDoc.LinkType
                    End If

                    If editDoc.LinkType = DocumentConstants.LinkTypeGoogleDoc Then 'Radio button selected was Google Doc
                        tbGoogle.Text = editDoc.LinkValue
                    ElseIf editDoc.LinkType = DocumentConstants.LinkTypeUrl Then 'Radio button selected was external URL
                        tbURL.Text = editDoc.LinkValue
                    ElseIf editDoc.LinkType = DocumentConstants.LinkTypePage Then 'Radio button selected was internal page
                        ddlPages.SelectedValue = editDoc.LinkValue
                    ElseIf editDoc.LinkType = DocumentConstants.LinkTypeYouTube Then 'Radio button selected was youtube
                        tbYouTube.Text = editDoc.LinkValue
                    End If

                End If

            End If

        End Sub

        Protected Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click

            'TODO: Add client and server validations
            If Page.IsValid Then
                If IsEditMode Then
                    Try
                        UpdateResource()
                        GotoDocuments()
                    Catch ex As InvalidFileExtensionException
                        'Log error
                        AgapeLogger.Error(UserId, ex.ToString)

                        'Display error message
                        AddModuleMessage(Me, LocalizeString("UPDATE_FILE_INVALID_EXTENSION_ERROR_MSG").Replace("[extension]", System.IO.Path.GetExtension(FileUpload1.FileName)), ModuleMessageType.RedError)
                    End Try
                Else 'Add mode
                    Try
                        AddResource()
                        Response.Redirect(NavigateURL()) 'Close modal popup and refresh Resource list

                    Catch ex As InvalidFileExtensionException
                        'Log error
                        AgapeLogger.Error(UserId, ex.ToString)

                        'Display error message
                        AddModuleMessage(Me, LocalizeString("ADD_FILE_INVALID_EXTENSION_ERROR_MSG").Replace("[extension]", System.IO.Path.GetExtension(FileUpload1.FileName)), ModuleMessageType.RedError)
                    End Try
                End If
            End If
        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
            GotoDocuments()
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
                DocumentsController.InsertResourceWithFile(tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeFile, "", "False", TabModuleId, tbDescription.Text, DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_") & FileUpload1.FileName, FileUpload1.FileContent) 'need to add permissions eventually
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeGoogleDoc Then 'Radio button selected was Google Doc
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeGoogleDoc, tbGoogle.Text, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeUrl Then 'Radio button selected was external URL
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeUrl, tbURL.Text, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypePage Then 'Radio button selected was internal page
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypePage, ddlPages.SelectedValue, "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeYouTube Then 'Radio button selected was youtube
                DocumentsController.InsertResource(DocumentConstants.FileIdForLinks, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeYouTube, tbYouTube.Text, "False", TabModuleId, tbDescription.Text)
            End If
        End Sub

        Protected Sub UpdateResource()
            If rbLinkType.SelectedValue = DocumentConstants.LinkTypeKeepFileForUpdate Then 'Radio button selected was "Keep existing file"
                ' Update all values but the FileId
                DocumentsController.UpdateResource(DocId, DocumentsController.GetDocument(DocId).FileId, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeFile, "", "False", TabModuleId, tbDescription.Text)
            ElseIf rbLinkType.SelectedValue = DocumentConstants.LinkTypeFile Then 'Radio button selected was upload
                DocumentsController.UpdateResourceWithFile(DocId, tbName.Text, UserInfo.DisplayName, DocumentConstants.LinkTypeFile, "", "False", TabModuleId, tbDescription.Text, DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_") & FileUpload1.FileName, FileUpload1.FileContent)
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

#Region "Validation"

        Protected Sub ValidateUpload(sender As Object, e As ServerValidateEventArgs)
            If rbLinkType.SelectedValue = DocumentConstants.LinkTypeFile Then
                If FileUpload1.HasFile Then
                    e.IsValid = True
                Else
                    e.IsValid = False
                End If
            Else
                e.IsValid = True
            End If
        End Sub

        Protected Sub ValidateGoogle(sender As Object, e As ServerValidateEventArgs)
            If rbLinkType.SelectedValue = DocumentConstants.LinkTypeGoogleDoc Then
                Dim pattern As String
                pattern = "http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?"
                If Regex.IsMatch(tbGoogle.Text, pattern) Then
                    e.IsValid = True
                Else
                    e.IsValid = False
                End If
            Else
                e.IsValid = True
            End If
        End Sub

        Protected Sub ValidateUrl(sender As Object, e As ServerValidateEventArgs)
            If rbLinkType.SelectedValue = DocumentConstants.LinkTypeUrl Then
                Dim pattern As String
                pattern = "http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?"
                If Regex.IsMatch(tbURL.Text, pattern) Then
                    e.IsValid = True
                Else
                    e.IsValid = False
                End If
            Else
                e.IsValid = True
            End If
        End Sub

        Protected Sub ValidateYouTube(sender As Object, e As ServerValidateEventArgs)
            If rbLinkType.SelectedValue = DocumentConstants.LinkTypeYouTube Then
                If tbYouTube.Text = "" Then
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = True
            End If
        End Sub

#End Region 'Validation

#Region "Helper Functions"
        Protected Sub GotoDocuments()
            If Not String.IsNullOrEmpty(SearchWords) Then
                ' Get search words from request if provided
                Response.Redirect(NavigateURL("", "", DocumentsControllerConstants.SearchWordsParamKey, SearchWords))
            Else
                ' Close modal popup and refresh Resource list
                Response.Redirect(NavigateURL())
            End If
        End Sub
#End Region 'Helper Functions

    End Class
End Namespace
