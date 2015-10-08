Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq

Imports Documents



Namespace DotNetNuke.Modules.Documents

    Partial Class DocumentSettings
        Inherits Entities.Modules.ModuleSettingsBase

        Dim d As New DocumentsDataContext

#Region "Base Method Implementations"

        'Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        '    jQuery.RegisterJQuery(Page)
        'End Sub


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                hfPortalId.Value = PortalId

                Dim IsPageRefresh = CancelUnexpectedRePost()

                lbTags.ClearSelection()
                If (IsPageRefresh = False) Then

                    'Build list of paths to choose from to select the root folder where the acDocuments folder will start.
                    Dim folders = (From c In d.AP_Documents_Folders Where c.PortalId = PortalId Order By c.FolderId Descending)
                    Dim pathName As String = ""

                    For Each folder In folders
                        pathName = GetPathName(folder, pathName)
                        ddlRoot.Items.Add(New ListItem(pathName, folder.FolderId))
                        pathName = ""
                    Next

                    'ddlRoot.Items.Add(New ListItem("Search Results...", -3))

                    'Build list of tags
                    Dim tags = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName, c.TagId

                    lbTags.DataSource = tags
                    lbTags.DataTextField = "TagName"
                    lbTags.DataValueField = "TagId"
                    lbTags.DataBind()

                    'If CType(TabModuleSettings("RootFolder"), String) <> "" Then
                    '        If ddlRoot.Items.FindByValue(CType(TabModuleSettings("RootFolder"), Integer)) Is Nothing Then
                    '            ddlRoot.SelectedIndex = 0
                    '        Else
                    '            ddlRoot.SelectedValue = CType(TabModuleSettings("RootFolder"), Integer)
                    '        End If
                    '        If CType(TabModuleSettings("RootFolder"), String) = "-3" Then
                    '            If CType(TabModuleSettings("SearchType"), String) <> "" Then
                    '                ddlSearchType.SelectedValue = CType(TabModuleSettings("SearchType"), String)
                    '            End If
                    '            If CType(TabModuleSettings("SearchValue"), String) <> "" Then
                    '                tbSearchValue.Text = CType(TabModuleSettings("SearchValue"), String)
                    '            End If

                    '        End If
                    '    End If

                    '    If CType(TabModuleSettings("DisplayStyle"), String) <> "" Then
                    'Select Case CType(TabModuleSettings("DisplayStyle"), String)
                    '    Case "BasicSearch"
                    '        rblStyle.SelectedValue = "BasicSearch"
                    '        ' cbShowTree.Checked = False
                    '    Case "Icons"
                    '        rblStyle.SelectedValue = "Icons"
                    '        '  cbShowTree.Checked = False
                    '    Case "Table"
                    '        rblStyle.SelectedValue = "Table"
                    '        '  tbWidth.Text = CType(TabModuleSettings("ColumnWidth"), String)
                    '    Case "Tree"
                    '        rblStyle.SelectedValue = "Tree"
                    '        ' ddlTreeStyle.SelectedValue = "Tree"
                    '        'Case "GTree"
                    '        '    rblStyle.SelectedValue = "GTree"
                    '        '    ' ddlTreeStyle.SelectedValue = "GTree"
                    '        '    If CType(TabModuleSettings("GTreeColor"), String) <> "" Then
                    '        '        'ddlColors.SelectedValue = CType(TabModuleSettings("GTreeColor"), String)
                    '        '    End If
                    '    Case "BasicSearch"
                    '        rblStyle.SelectedValue = "BasicSearch"

                    '    Case Else
                    'End Select


                    '    End If



                End If                       'Page.IsPostBack
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
#End Region

        Protected Function CancelUnexpectedRePost() As Boolean
            Dim r = False

            If Not IsPostBack Then
                ViewState("ViewStateId") = System.Guid.NewGuid().ToString()
                Session("SessionId") = ViewState("ViewStateId").ToString()
            Else
                If ViewState("ViewStateId").ToString() <> Session("SessionId").ToString() Then
                    r = True
                End If
                Session("SessionId") = System.Guid.NewGuid().ToString()
                ViewState("ViewStateId") = Session("SessionId").ToString()
            End If

            Return r
        End Function
        Protected Function GetPathName(ByVal Folder As AP_Documents_Folder, ByRef pathName As String) As String

            pathName = Folder.Name & "/" & pathName

            If Folder.ParentFolder > 0 Then
                Dim parent = From c In d.AP_Documents_Folders Where c.FolderId = Folder.ParentFolder And c.PortalId = PortalId
                If parent.Count > 0 Then
                    GetPathName(parent.First, pathName)
                End If
            End If
            Return pathName
        End Function

        Protected Sub rbStyle_SelectedIndexChanged(sender As Object, e As EventArgs)

            updatePanelStyle.ContentTemplateContainer.Controls.Clear()

            Select Case rblStyle.SelectedValue
                Case "Icons"
                    'show the tree checkbox option
                    Dim showTree As New CheckBox()
                    showTree.ID = "cbShowTree"
                    showTree.Text = LocalizeString("TreeOption")
                    updatePanelStyle.ContentTemplateContainer.Controls.Add(showTree)

                Case "Table"
                    'show column width option
                    Dim lblColumnWidth As New Label()
                    lblColumnWidth.ID = "lblColumnWidth"
                    lblColumnWidth.Text = LocalizeString("ColumnWidthLabel")
                    updatePanelStyle.ContentTemplateContainer.Controls.Add(lblColumnWidth)

                    Dim showColumnWidth As New TextBox()
                    showColumnWidth.ID = "tbColumnWidth"
                    updatePanelStyle.ContentTemplateContainer.Controls.Add(showColumnWidth)

                    Dim intOnly As New RegularExpressionValidator()
                    intOnly.ID = "intOnly"
                    intOnly.ControlToValidate = "tbColumnWidth"
                    intOnly.ValidationExpression = "\d*"
                    intOnly.ErrorMessage = LocalizeString("InvalidInteger")
                    intOnly.Text = LocalizeString("InvalidInteger")
                    intOnly.Display = ValidatorDisplay.Dynamic
                    updatePanelStyle.ContentTemplateContainer.Controls.Add(New LiteralControl("<div class=""MandatoryFieldErrorMsg"">"))
                    updatePanelStyle.ContentTemplateContainer.Controls.Add(intOnly)
                    updatePanelStyle.ContentTemplateContainer.Controls.Add(New LiteralControl("</div>"))

                Case "Tree"
                    'show colors option
                    Dim lblColors As New Label()
                    lblColors.ID = "lblColors"
                    lblColors.Text = LocalizeString("TextColor")
                    updatePanelStyle.ContentTemplateContainer.Controls.Add(lblColors)

                    Dim ddlColors As New DropDownList()
                    ddlColors.ID = "ddlColors"

                    Dim itemOlive As New ListItem()
                    itemOlive.Text = LocalizeString("ColorOlive")
                    itemOlive.Value = "#86bb41"
                    ddlColors.Items.Add(itemOlive)

                    Dim itemRed As New ListItem()
                    itemRed.Text = LocalizeString("ColorRed")
                    itemRed.Value = "#8c3b3b"
                    ddlColors.Items.Add(itemRed)

                    Dim itemTurquoise As New ListItem()
                    itemTurquoise.Text = LocalizeString("ColorTurquoise")
                    itemTurquoise.Value = "#28686E"
                    ddlColors.Items.Add(itemTurquoise)

                    updatePanelStyle.ContentTemplateContainer.Controls.Add(ddlColors)

            End Select
        End Sub

        Protected Sub btnAddTag_Click(sender As Object, e As System.EventArgs) Handles btnAddTag.Click
            If (lbTags.Items.FindByText(tbAddTag.Text) Is Nothing And tbAddTag.Text.Length <> 0) Then
                Dim insertTag As New AP_Documents_Tag
                insertTag.PortalId = 0
                insertTag.TagName = tbAddTag.Text

                d.AP_Documents_Tags.InsertOnSubmit(insertTag)
                d.SubmitChanges()

                lbTags.DataSource = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName, c.TagId
                lbTags.DataBind()
                tbAddTag.Text = ""
                ' lbTags.ClearSelection()
            End If
        End Sub

        Protected Sub btnRemoveTag_Click(sender As Object, e As System.EventArgs) Handles btnRemoveTag.Click
            If lbTags.SelectedIndex > 0 Then

                Dim removeTag = From c In d.AP_Documents_Tags Where c.TagId = lbTags.SelectedValue

                d.AP_Documents_Tags.DeleteAllOnSubmit(removeTag)
                d.SubmitChanges()
                lbTags.DataSource = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName, c.TagId
                lbTags.DataBind()
                tbRemoveTag.Text = ""
                lbTags.ClearSelection()
            End If
        End Sub

        'Protected Sub lbTags_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        '    'lbTags.SelectedItem(DataValueField)
        '    Dim selectedTag = From c In d.AP_Documents_Tags Where c.PortalId = PortalId And c.TagId = lbTags.SelectedValue Select TagName = c.TagName
        '    ' tbRemoveTag.Text = lbTags.DataTextField()

        '    tbRemoveTag.Text = selectedTag.ToString

        '    'tags.ToString()
        '    'lblTags.Tag()
        '    ' tbRemoveTag.Text = tags.ToString()


        'End Sub



            'Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
            '    Dim objModules As New Entities.Modules.ModuleController

            '    objModules.UpdateTabModuleSetting(TabModuleId, "RootFolder", ddlRoot.SelectedValue)

            '    '    If ddlRoot.SelectedValue = -3 Then
            '    '        objModules.UpdateTabModuleSetting(TabModuleId, "SearchType", ddlSearchType.SelectedValue)
            '    '        objModules.UpdateTabModuleSetting(TabModuleId, "SearchValue", tbSearchValue.Text)
            '    '    End If

            '    '    Dim displaystyle = ddlStyle.SelectedValue
            '    '    If displaystyle = "Explorer" And cbShowTree.Checked = False Then
            '    '        displaystyle = "ExplorerNoTree"
            '    '    ElseIf displaystyle = "GTree" Then
            '    '        displaystyle = ddlTreeStyle.SelectedValue
            '    '        If displaystyle = "GTree" Then
            '    '            objModules.UpdateTabModuleSetting(TabModuleId, "GTreeColor", ddlColors.SelectedValue)
            '    '        End If

            '    '    ElseIf displaystyle = "Table" Then
            '    '        objModules.UpdateTabModuleSetting(TabModuleId, "ColumnWidth", tbWidth.Text)
            '    '    End If

            '    '    objModules.UpdateTabModuleSetting(TabModuleId, "DisplayStyle", displaystyle)


            '    '    ' refresh cache
            '    '    SynchronizeModule()
            '    '    Response.Redirect(NavigateURL())
            'End Sub


            'Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            '    Response.Redirect(NavigateURL())
            'End Sub



    End Class

End Namespace

