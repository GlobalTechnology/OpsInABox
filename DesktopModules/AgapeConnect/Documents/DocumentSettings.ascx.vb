Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq

Imports Documents



Namespace DotNetNuke.Modules.Documents

    Partial Class DocumentSettings
        Inherits Entities.Modules.ModuleSettingsBase

        Dim d As New DocumentsDataContext

#Region "Base Method Implementations"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                hfPortalId.Value = PortalId
                AgapeLogger.Info(UserId, "TOP OF PAGELOAD " & rblStyle.SelectedValue)
               
                If (Page.IsPostBack = False) Then
                    Session("Check_Page_Refresh") = DateTime.Now.ToString()
                  
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

                    If CType(TabModuleSettings("RootFolder"), String) <> "" Then

                        If ddlRoot.Items.FindByValue(CType(TabModuleSettings("RootFolder"), Integer)) Is Nothing Then
                            ddlRoot.SelectedIndex = 0
                        Else
                            ddlRoot.SelectedValue = CType(TabModuleSettings("RootFolder"), Integer)
                        End If

                        'If CType(TabModuleSettings("RootFolder"), String) = "-3" Then
                        '    If CType(TabModuleSettings("SearchType"), String) <> "" Then
                        '        ddlSearchType.SelectedValue = CType(TabModuleSettings("SearchType"), String)
                        '    End If
                        '    If CType(TabModuleSettings("SearchValue"), String) <> "" Then
                        '        tbSearchValue.Text = CType(TabModuleSettings("SearchValue"), String)
                        '    End If

                        'End If

                        'Load page with values saved previously
                        If CType(TabModuleSettings("DisplayStyle"), String) <> "" Then

                            Select Case CType(TabModuleSettings("DisplayStyle"), String)
                                Case "BasicSearch"
                                    rblStyle.SelectedValue = "BasicSearch"
                                    SetupBasicSearch()

                                Case "IconsNoTree"
                                    rblStyle.SelectedValue = "Icons"
                                    SetupIcons()
                                    cbshowtree.Checked = False

                                Case "Icons"
                                    rblStyle.SelectedValue = "Icons"
                                    SetupIcons()
                                    cbshowtree.Checked = True

                                Case "Table"
                                    rblStyle.SelectedValue = "Table"
                                    SetupTable()
                                    'tbcolumnwidth.Text = CType(TabModuleSettings("ColumnWidth"), String)

                                Case "Tree"
                                    rblStyle.SelectedValue = "Tree"
                                    SetupTree()
                                    'If CType(TabModuleSettings("TreeColor"), String) <> "" Then
                                    '    ddlColors.SelectedValue = CType(TabModuleSettings("TreeColor"), String)
                                    'End If
                            End Select

                        End If
                    End If
                End If                       'Page.IsPostBack

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Overrides Sub onPreRender(ByVal e As System.EventArgs)
            ViewState("Check_Page_Refresh") = Session("Check_Page_Refresh")
        End Sub

#End Region


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

            Select Case rblStyle.SelectedValue

                Case "BasicSearch"
                    AgapeLogger.Info(UserId, rblStyle.SelectedValue)
                    SetupBasicSearch()

                Case "Icons"
                    AgapeLogger.Info(UserId, rblStyle.SelectedValue)
                    SetupIcons()

                Case "Table"
                    AgapeLogger.Info(UserId, rblStyle.SelectedValue)
                    SetupTable()

                Case "Tree"
                    AgapeLogger.Info(UserId, rblStyle.SelectedValue)
                    SetupTree()

            End Select
        End Sub

        Protected Sub btnAddTag_Click(sender As Object, e As System.EventArgs) Handles btnAddTag.Click

            If ViewState("Check_Page_Refresh").ToString() = Session("Check_Page_Refresh").ToString() Then

                If (lbTags.Items.FindByText(tbAddTag.Text) Is Nothing And tbAddTag.Text.Length <> 0) Then

                    Dim insertTag As New AP_Documents_Tag
                    insertTag.PortalId = PortalId
                    insertTag.TagName = tbAddTag.Text

                    d.AP_Documents_Tags.InsertOnSubmit(insertTag)
                    d.SubmitChanges()

                    lbTags.DataSource = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName, c.TagId
                    lbTags.DataBind()
                    tbAddTag.Text = ""

                    AgapeLogger.Info(UserId, "Tag sucessfully added: " & "  Tag: " & insertTag.TagName)
                    Session("Check_Page_Refresh") = DateTime.Now.ToString()
                End If

            Else
                lbTags.DataSource = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName, c.TagId
                lbTags.DataBind()
                tbAddTag.Text = ""
            End If

        End Sub

        Protected Sub btnRemoveTag_Click(sender As Object, e As System.EventArgs) Handles btnRemoveTag.Click

            If ViewState("Check_Page_Refresh").ToString() = Session("Check_Page_Refresh").ToString() Then

                If lbTags.SelectedIndex > -1 Then

                    Dim removeTag = From c In d.AP_Documents_Tags Where c.PortalId = PortalId And c.TagId = lbTags.SelectedValue
                    d.AP_Documents_Tags.DeleteAllOnSubmit(removeTag)
                    d.SubmitChanges()
                    AgapeLogger.Info(UserId, "Tag sucessfully removed: " & "  Tag: " & lbTags.SelectedValue)

                    lbTags.DataSource = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName, c.TagId
                    lbTags.DataBind()
                    lbTags.ClearSelection()
                    btnRemoveTag.Enabled = "false"

                    Session("Check_Page_Refresh") = DateTime.Now.ToString()
                End If

            Else
                lbTags.DataSource = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName, c.TagId
                lbTags.DataBind()
                lbTags.ClearSelection()
            End If

        End Sub

        Protected Sub lbTags_SelectedIndexChanged(sender As Object, e As System.EventArgs)

            If lbTags.SelectedIndex > -1 Then
                btnRemoveTag.Enabled = "true"
            Else
                btnRemoveTag.Enabled = "false"
            End If

        End Sub



        Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
            Dim objModules As New Entities.Modules.ModuleController

            objModules.UpdateTabModuleSetting(TabModuleId, "RootFolder", ddlRoot.SelectedValue)

            '    If ddlRoot.SelectedValue = -3 Then
            '        objModules.UpdateTabModuleSetting(TabModuleId, "SearchType", ddlSearchType.SelectedValue)
            '        objModules.UpdateTabModuleSetting(TabModuleId, "SearchValue", tbSearchValue.Text)
            '    End If

            Dim displaystyle = rblStyle.SelectedValue
            Select Case displaystyle
                Case "Icons"
                    If cbShowTree.Checked = False Then
                        displaystyle = "IconsNoTree"
                    End If

                Case "Table"
                    AgapeLogger.Info(UserId, "ABOVE intOnly")

                    'If Not intOnly.IsValid Then
                    '    AgapeLogger.Info(UserId, "In not intOnly")
                    '    tbcolumnwidth.Focus()
                    '    tbcolumnwidth.BackColor = Color.DarkRed

                    'End If
                    objModules.UpdateTabModuleSetting(TabModuleId, "ColumnWidth", tbcolumnwidth.Text)

                Case "Tree"
                    objModules.UpdateTabModuleSetting(TabModuleId, "TreeColor", ddlColors.SelectedValue)

            End Select

            objModules.UpdateTabModuleSetting(TabModuleId, "DisplayStyle", displaystyle)

            ' refresh cache
            DotNetNuke.Entities.Modules.ModuleController.SynchronizeModule(ModuleId)
            Response.Redirect(NavigateURL())

        End Sub


        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub SetupBasicSearch()
            AgapeLogger.Info(UserId, "In SetupBasicSearch()")
            cbshowtree.Visible = False

            lblColumnWidth.Visible = False
            tbcolumnwidth.Visible = False
            intOnly.Enabled = False

            lblColors.Visible = False
            ddlColors.Visible = False
        End Sub

        Protected Sub SetupIcons()
            AgapeLogger.Info(UserId, "In SetupIcons()")
            cbshowtree.Visible = True

            lblColumnWidth.Visible = False
            tbcolumnwidth.Visible = False
            intOnly.Enabled = False

            lblColors.Visible = False
            ddlColors.Visible = False
        End Sub

        Protected Sub SetupTable()
            AgapeLogger.Info(UserId, "In SetupTable()")
            cbshowtree.Visible = False

            lblColumnWidth.Visible = True
            tbcolumnwidth.Visible = True
            intOnly.Enabled = True

            lblColors.Visible = False
            ddlColors.Visible = False

            tbcolumnwidth.Text = CType(TabModuleSettings("ColumnWidth"), String)
        End Sub

        Protected Sub SetupTree()
            AgapeLogger.Info(UserId, "In SetupTree()")
            cbshowtree.Visible = False

            lblColumnWidth.Visible = False
            tbcolumnwidth.Visible = False
            intOnly.Enabled = False

            lblColors.Visible = True
            ddlColors.Visible = True

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

            If CType(TabModuleSettings("TreeColor"), String) <> "" Then
                ddlColors.SelectedValue = CType(TabModuleSettings("TreeColor"), String)
            End If

        End Sub

    End Class

End Namespace

