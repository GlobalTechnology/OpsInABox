Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq

Imports Documents



Namespace DotNetNuke.Modules.Documents

    Partial Class DocumentSettings
        Inherits Entities.Modules.ModuleSettingsBase
        Dim d As New DocumentsDataContext
        Protected Sub GetPathName(ByVal Folder As AP_Documents_Folder)
            pathName = Folder.Name & "/" & pathName
            If Folder.ParentFolder > 0 Then
                Dim parent = From c In d.AP_Documents_Folders Where c.FolderId = Folder.ParentFolder And c.PortalId = PortalId
                If parent.Count > 0 Then


                    GetPathName(parent.First)
                End If
            End If
        End Sub

        Dim pathName As String = ""
#Region "Base Method Implementations"

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            jQuery.RegisterJQuery(Page)
        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                hfPortalId.Value = PortalId
                If (Page.IsPostBack = False) Then



                    Dim folders = (From c In d.AP_Documents_Folders Where c.PortalId = PortalId Order By c.FolderId Descending)

                    For Each folder In folders
                        pathName = ""
                        GetPathName(folder)
                        ddlRoot.Items.Add(New ListItem(pathName, folder.FolderId))


                    Next
                    ddlRoot.Items.Add(New ListItem("Search Results...", -3))


                    Dim tags = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName.ToUpper, c.TagId
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
                        If CType(TabModuleSettings("RootFolder"), String) = "-3" Then
                            If CType(TabModuleSettings("SearchType"), String) <> "" Then
                                ddlSearchType.SelectedValue = CType(TabModuleSettings("SearchType"), String)
                            End If
                            If CType(TabModuleSettings("SearchValue"), String) <> "" Then
                                tbSearchValue.Text = CType(TabModuleSettings("SearchValue"), String)
                            End If

                        End If
                    End If
                    If CType(TabModuleSettings("DisplayStyle"), String) <> "" Then
                        Select Case CType(TabModuleSettings("DisplayStyle"), String)
                            Case "ExplorerNoTree"
                                ddlStyle.SelectedValue = "Explorer"
                                cbShowTree.Checked = False
                            Case "Explorer"
                                ddlStyle.SelectedValue = "Explorer"
                                cbShowTree.Checked = False
                            Case "Table"
                                ddlStyle.SelectedValue = "Table"
                                tbWidth.Text = CType(TabModuleSettings("ColumnWidth"), String)
                            Case "Tree"
                                ddlStyle.SelectedValue = "GTree"
                                ddlTreeStyle.SelectedValue = "Tree"
                            Case "GTree"
                                ddlStyle.SelectedValue = "GTree"
                                ddlTreeStyle.SelectedValue = "GTree"
                                If CType(TabModuleSettings("GTreeColor"), String) <> "" Then
                                    ddlColors.SelectedValue = CType(TabModuleSettings("GTreeColor"), String)
                                End If
                        End Select


                    End If



                End If



            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try



        End Sub


#End Region


        Protected Sub SaveBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
            Dim objModules As New Entities.Modules.ModuleController
          
            objModules.UpdateTabModuleSetting(TabModuleId, "RootFolder", ddlRoot.SelectedValue)

            If ddlRoot.SelectedValue = -3 Then
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchType", ddlSearchType.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchValue", tbSearchValue.Text)
            End If

            Dim displaystyle = ddlStyle.SelectedValue
            If displaystyle = "Explorer" And cbShowTree.Checked = False Then
                displaystyle = "ExplorerNoTree"
            ElseIf displaystyle = "GTree" Then
                displaystyle = ddlTreeStyle.SelectedValue
                If displaystyle = "GTree" Then
                    objModules.UpdateTabModuleSetting(TabModuleId, "GTreeColor", ddlColors.SelectedValue)
                End If

            ElseIf displaystyle = "Table" Then
                objModules.UpdateTabModuleSetting(TabModuleId, "ColumnWidth", tbWidth.Text)
            End If

            objModules.UpdateTabModuleSetting(TabModuleId, "DisplayStyle", displaystyle)

       
            ' refresh cache
            SynchronizeModule()
            Response.Redirect(NavigateURL())
        End Sub


        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub btnAddTag_Click(sender As Object, e As System.EventArgs) Handles btnAddTag.Click
            If lbTags.Items.FindByText(tbNewTag.Text.ToUpper) Is Nothing Then
                Dim insert As New AP_Documents_Tag
                insert.PortalId = 0
                insert.TagName = tbNewTag.Text.ToUpper

                d.AP_Documents_Tags.InsertOnSubmit(insert)
                d.SubmitChanges()

                lbTags.DataSource = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName.ToUpper, c.TagId
                lbTags.DataBind()
                tbNewTag.Text = ""
            End If




        End Sub

        Protected Sub btnRemove_Click(sender As Object, e As System.EventArgs) Handles btnRemove.Click
            If lbTags.SelectedIndex > 0 Then

                Dim theTag = From c In d.AP_Documents_Tags Where c.TagId = lbTags.SelectedValue

                d.AP_Documents_Tags.DeleteAllOnSubmit(theTag)
                d.SubmitChanges()
                lbTags.DataSource = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Order By c.TagName Select TagName = c.TagName.ToUpper, c.TagId
                lbTags.DataBind()
                tbNewTag.Text = ""
            End If
        End Sub
    End Class

End Namespace

