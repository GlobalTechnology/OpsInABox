Imports System.IO
Imports System.Xml
Imports System.Net
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Services.Authentication
Imports System.Linq
Imports Documents
Imports DotNetNuke.Services.FileSystem
'Imports Resources


Namespace DotNetNuke.Modules.AgapeConnect
    Partial Class Documents
        Inherits Entities.Modules.PortalModuleBase
        Dim d As New DocumentsDataContext()
        Public templateMode As String = "Icons"
        Dim FolderId As Integer = -1
        Dim rc As New DotNetNuke.Security.Roles.RoleController
        Dim UserRoles As ArrayList
        Dim doc As Object
        Dim GTreeColor As String
        Dim TreeStyles() As String = {"Explorer", "GTree", "Tree"}
        Dim DocumentsModuleRoot As String = "acDocuments"


        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

            GTreeColor = Settings("GTreeColor")
            graphicalTreeView.CollapseImageUrl = GetGTreeImageURL() & "D.gif"
            graphicalTreeView.ExpandImageUrl = GetGTreeImageURL() & "R.gif"

        End Sub

        Protected Function GetGTreeImageURL() As String
            Select Case GTreeColor
                Case "#28686E" : Return "images/ArrowWin"
                Case "#86bb41" : Return "images/ArrowBuild"
                Case "#8c3b3b" : Return "images/ArrowSend"
                Case "#876c49" : Return "images/ArrowMaps"
                Case "#f1a519" : Return "images/ArrowCoaching"
                Case "#1f594f" : Return "images/ArrowEveryone"

            End Select
            Return "images/ArrowWin"
        End Function

        Protected Function GetCurrentRootDirectory() As String
            Dim parentFolder As Integer = -1
            Dim rootFolderId As Integer

            If Not String.IsNullOrEmpty(Settings("RootFolder")) Then
                rootFolderId = Settings("RootFolder")
            Else

                If Not Page.IsPostBack Then

                    Dim rootNode = From c In d.AP_Documents_Folders Where c.PortalId = PortalId And c.ParentFolder = parentFolder

                    'No rootNode found
                    If rootNode.Count = 0 Then

                        'Add the rootNode acDocuments in the database
                        Dim insert As New AP_Documents_Folder
                        insert.CustomIcon = Nothing
                        insert.ParentFolder = -1
                        insert.Name = DocumentsModuleRoot
                        insert.Description = DocumentsModuleRoot & " root directory"
                        insert.Permission = Settings("DefaultPermissions")
                        insert.PortalId = PortalId
                        d.AP_Documents_Folders.InsertOnSubmit(insert)
                        d.SubmitChanges()
                        DotNetNuke.Entities.Modules.ModuleController.SynchronizeModule(ModuleId)

                        'Get the new rootNode that has been inserted in the database
                        rootNode = From c In d.AP_Documents_Folders Where c.PortalId = PortalId And c.ParentFolder = parentFolder
                    End If

                    LoadTree(rootNode.First)

                End If          'Page.IsPostBack
            End If
            Return rootFolderId
        End Function
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Request.QueryString("search") <> "" Then
                Dim SearchURL = EditUrl("searchDocs") & "?search=" & Request.QueryString("search")
                If Request.QueryString("mode") <> "" Then
                    SearchURL &= "&" & Request.QueryString("mode")
                End If
                'Response.Redirect(SearchURL)

            End If

            If String.IsNullOrEmpty(Settings("DefaultPermissions")) Then
                Dim rc As New DotNetNuke.Security.Roles.RoleController()
                Dim RoleId = rc.GetRoleByName(PortalId, "Administrators").RoleID
                Settings("DefaultPermissions") = RoleId & ":-1;" & RoleId

            End If

            editbuttons.Visible = IsEditable

            FolderId = GetCurrentRootDirectory()

            If Request.QueryString("DocId") <> "" Then
                'Open the Document!
                Dim theDoc = From c In d.AP_Documents_Docs Where c.DocId = Request.QueryString("DocId") And c.AP_Documents_Folder.PortalId = PortalId
                Response.Redirect(EditUrl("DocumentViewer") & "?DocId=" & theDoc.First.DocId)
            Else
                'load the folder
                If hfMoveId.Value <> "" Then
                    Dim moveFolder = From c In d.AP_Documents_Folders Where c.FolderId = CInt(hfMoveId.Value)

                    moveFolder.First.ParentFolder = CInt(hfMoveToId.Value)
                    d.SubmitChanges()
                    hfMoveId.Value = ""
                    hfMoveToId.Value = ""
                    hfFileMoveId.Value = ""
                    FolderId = tvFolders.SelectedValue.TrimStart("F")
                    Dim rootNode = From c In d.AP_Documents_Folders Where c.PortalId = PortalId And c.FolderId = FolderId

                    LoadTree(rootNode.First)

                ElseIf hfFileMoveId.Value <> "" Then
                    Dim moveDoc = From c In d.AP_Documents_Docs Where c.DocId = CInt(hfFileMoveId.Value)

                    moveDoc.First.FolderId = CInt(hfMoveToId.Value)
                    d.SubmitChanges()
                    hfMoveId.Value = ""
                    hfMoveToId.Value = ""
                    hfFileMoveId.Value = ""
                    FolderId = tvFolders.SelectedValue.TrimStart("F")
                    Dim rootNode = From c In d.AP_Documents_Folders Where c.PortalId = PortalId And c.FolderId = FolderId

                    LoadTree(rootNode.First)

                End If

                If Request.QueryString("FolderId") <> "" Then
                    FolderId = Request.QueryString("FolderId")

                End If

            End If

            If Not Page.IsPostBack Then
                Dim folder = FolderManager.Instance.GetFolder(PortalId, "acDocuments")
                If Not folder Is Nothing Then
                    ddlFiles.DataSource = FolderManager.Instance.GetFiles(folder)
                    ddlFiles.DataTextField = "FileName"
                    ddlFiles.DataValueField = "FileId"
                    ddlFiles.DataBind()
                End If
                Dim pages = TabController.GetPortalTabs(PortalId, TabId, False, False)
                ddlPages.DataSource = pages.OrderBy(Function(x) x.TabName)
                ddlPages.DataTextField = "TabName"
                ddlPages.DataValueField = "TabId"
                ddlPages.DataBind()

            End If

            tbNewLinkAuthor.Text = UserInfo.DisplayName
            LoadFolder(FolderId)

        End Sub

        Public Sub LoadTree(ByVal StartNode As AP_Documents_Folder)
            tvFolders.Nodes.Clear()
            UserRoles = rc.GetUserRoles(PortalId, UserId)

            Dim rootNode As New TreeNode()

            AddSubFolders(StartNode.ParentFolder, rootNode)

            If Settings("DisplayStyle") = "GTree" Then


                graphicalTreeView.Nodes.Clear()
                ' rootNode.ChildNodes.RemoveAt(0)
                Dim RootNodes(rootNode.ChildNodes(0).ChildNodes.Count() - 1) As TreeNode

                rootNode.ChildNodes(0).ChildNodes.CopyTo(RootNodes, 0)

                For Each node As TreeNode In RootNodes
                    graphicalTreeView.Nodes.Add(node)
                Next

                'For i As Integer = 0 To 12
                '    graphicalTreeView.Nodes.Add(rootNode.ChildNodes(0).ChildNodes(i))
                'Next
                '  graphicalTreeView.Nodes.Add(rootNode.ChildNodes(0))

                'graphicalTreeView.Nodes.RemoveAt(0)
                graphicalTreeView.Visible = True
                tvFolders.Visible = False
            Else

                tvFolders.Nodes.Add(rootNode.ChildNodes(0))
            End If
        End Sub

        Public Function GetFilePermission(ByVal Permissions As String) As String
            If UserInfo.IsSuperUser Or UserInfo.IsInRole("Administrators") Then
                Return "Edit"
            ElseIf Not Permissions Is Nothing Then
                Dim ReadPermissions = Permissions.Replace(";", ":").Split(":")
                Dim EditPermissions = Permissions.Split(";")(1).Trim(";").Split(":")
                If UserRoles Is Nothing Then

                    Return IIf(Permissions.Contains("-1"), "Read", "None")


                End If

                For Each row In EditPermissions
                    If (From c As DotNetNuke.Security.Roles.RoleInfo In UserRoles Where CStr(c.RoleID) = row.Trim(":")).Count > 0 Then
                        Return "Edit"

                    End If
                Next


                If Permissions.Contains("-1") Then
                    Return "Read"
                Else

                    For Each row In ReadPermissions
                        If (From c As DotNetNuke.Security.Roles.RoleInfo In UserRoles Where CStr(c.RoleID) = row.Trim(":")).Count > 0 Then

                            Return "Read"
                            Exit For
                        End If
                    Next
                End If


            End If
            Return "None"
        End Function

        Protected Sub LoadFolder(ByVal FolderId As Integer)
            If FolderId = -1 Then
                FolderId = (From c In d.AP_Documents_Folders Where c.PortalId = PortalId And c.ParentFolder = -1).First.FolderId
            End If

            Dim Items As New ArrayList
            Dim rc As New DotNetNuke.Security.Roles.RoleController
            Dim UserRoles = rc.GetUserRoles(PortalId, UserId)
            Dim Folders As IQueryable(Of AP_Documents_Folder)
            Dim Docs As IQueryable(Of AP_Documents_Doc)
            Dim search = Server.HtmlDecode(Request.QueryString("search"))
            If search = "" Then
                Folders = From c In d.AP_Documents_Folders Where c.ParentFolder = FolderId
                Docs = From c In d.AP_Documents_Docs Where c.FolderId = FolderId
                'lblDisplayingSearch.Visible = False
                'lblDisplaying.Visible = True
                

            ElseIf Request.QueryString("mode") = "tags" Then
                Folders = From c In d.AP_Documents_Folders Where 1 = 0
                Docs = From c In d.AP_Documents_Docs Where c.AP_Documents_Folder.PortalId = PortalId And (c.AP_Documents_TagMetas.Where(Function(x) x.AP_Documents_Tag.TagName = search).Count > 0)
                'lblDisplayingSearch.Visible = True
                'lblDisplaying.Visible = False
                'lblFolder.Text = " Tag<: " & search

            ElseIf Request.QueryString("mode") = "keywords" Then

                Folders = From c In d.AP_Documents_Folders Where 1 = 0
                Docs = From c In d.AP_Documents_Docs Where c.AP_Documents_Folder.PortalId = PortalId And c.Keywords.Contains("search")
                'lblDisplayingSearch.Visible = True
                'lblDisplaying.Visible = False
                'lblFolder.Text = " Keyword(s): " & search
            Else


                Folders = From c In d.AP_Documents_Folders Where c.PortalId = PortalId And (c.Name.Contains(search) Or c.Description.Contains(search))

                Docs = From c In d.AP_Documents_Docs Where c.AP_Documents_Folder.PortalId = PortalId And (c.DisplayName.Contains(search) Or c.Description.Contains(search) Or c.Keywords.Contains("search") Or (c.AP_Documents_TagMetas.Where(Function(x) x.AP_Documents_Tag.TagName.Contains(search)).Count > 0))
                'lblDisplayingSearch.Visible = True
                'lblDisplaying.Visible = False
                'lblFolder.Text = ": " & search


            End If
           


            For Each f In Folders
                Dim perm = GetFilePermission(f.Permission)
                If perm = "Read" Or perm = "Edit" Then
                    Dim insert As New AP_Documents_Doc
                    insert.DisplayName = f.Name
                    insert.Description = f.Description
                    insert.FileId = Nothing
                    'insert.CustomIcon = 
                    If f.CustomIcon Is Nothing Then
                        insert.CustomIcon = -1
                    Else
                        insert.CustomIcon = f.CustomIcon
                    End If

                    insert.FolderId = f.FolderId
                    Items.Add(insert)
                End If


            Next

            For Each docu In Docs
                Dim perm = ""
                If docu.LinkType < 4 Then
                    perm = "Link"
                Else
                    perm = GetFilePermission(docu.Permissions)
                End If

                If perm = "Edit" Or perm = "Read" Or perm = "Link" Then
                    'doc.Permissions = perm

                    Items.Add(docu)
                End If
            Next

            dlFolderView.DataSource = Items
            dlFolderView.DataBind()

            Dim thisNode = From c As TreeNode In tvFolders.Nodes Where c.Value.TrimStart("F").TrimStart("D") = FolderId

            If thisNode.Count > 0 Then
                thisNode.First.Selected = True
            End If
            If FolderId > 0 Then
                Dim thisPerm = GetFilePermission((From c In d.AP_Documents_Folders Where c.FolderId = FolderId Select c.Permission).First)
                hlUpload.Visible = thisPerm = "Edit"
                hlNewLink.Visible = thisPerm = "Edit"
                'hlFolderButton.Visible = thisPerm = "Edit"
            End If
        End Sub

        Protected Sub AddSubFolders(ByVal ParentId As Integer, ByRef ParentNode As TreeNode)
            Dim folders = From c In d.AP_Documents_Folders Where c.PortalId = PortalId And c.ParentFolder = ParentId Order By c.Name

            For Each row In folders

                Dim FolderURL = NavigateURL() & "?FolderId=" & row.FolderId
                Dim fperm = GetFilePermission(row.Permission)
                Dim NewNode As TreeNode
                Dim ShowFolderImage As Boolean = Settings("DisplayStyle") <> "GTree"
                If fperm = "Edit" Then
                    NewNode = New TreeNode("<a class=""aFolder"" " & IIf(ShowFolderImage, "", "style=""font-size: x-Large; font-weight: bold; color: " & GTreeColor & ";""") & " href=""" & FolderURL & """>" & row.Name & "</a>", "F" & row.FolderId, IIf(ShowFolderImage, "WebResource.axd?d=fYSQ59BWBhpvZOEdflo5zZ9R3lwpigzAHgbcJsuH3fOV3-S9GsDhkmxHmGGB-2hzMOZlCq4vjWa7tH8fDgcuLbK9U4Cn3Z5uwEy9xDdDGtUVNig70&t=634661034603432464", ""), FolderURL, "_self")

                ElseIf fperm = "Read" Then
                    NewNode = New TreeNode("<a class=""aFolderRead""  " & IIf(ShowFolderImage, "", "style=""font-size: x-Large; font-weight: bold; color: " & GTreeColor & ";""") & " href=""" & FolderURL & """>" & row.Name & "</a>", "F" & row.FolderId, IIf(ShowFolderImage, "WebResource.axd?d=fYSQ59BWBhpvZOEdflo5zZ9R3lwpigzAHgbcJsuH3fOV3-S9GsDhkmxHmGGB-2hzMOZlCq4vjWa7tH8fDgcuLbK9U4Cn3Z5uwEy9xDdDGtUVNig70&t=634661034603432464", ""), FolderURL, "_self")
                End If




                If fperm = "Edit" Or fperm = "Read" Then
                    If (FolderId = row.FolderId) Then
                        NewNode.Selected = True



                        Dim breadcrumb As String = NewNode.Text
                        Dim ancestors As TreeNode = ParentNode
                        While Not ancestors Is Nothing
                            ancestors.Expand()
                            breadcrumb = ancestors.Text & "\" & breadcrumb
                            ancestors = ancestors.Parent

                        End While
                        'lblFolder.Text = breadcrumb
                    End If



                    ParentNode.ChildNodes.Add(NewNode)



                    AddSubFolders(row.FolderId, NewNode)
                    For Each doc In row.AP_Documents_Docs.OrderBy(Function(x) x.DisplayName)

                        Select Case doc.LinkType
                            Case 0
                                Dim NewDocNode As New TreeNode("<a class=""aLink"" " & IIf(ShowFolderImage, "", "style=""font-size: Medium; color: " & GTreeColor & ";""") & " href=""" & doc.LinkValue & """ >" & doc.DisplayName & "</a>", "D" & doc.DocId, IIf(ShowFolderImage, "/DesktopModules/AgapeConnect/Documents/images/URL.png", ""), doc.LinkValue, "_blank")
                                NewNode.ChildNodes.Add(NewDocNode)

                            Case 1
                                'you tube!
                                Dim NewDocNode As New TreeNode("<a class=""aLink"" " & IIf(ShowFolderImage, "", "style=""font-size: Medium; color: " & GTreeColor & ";""") & " href=""" & doc.LinkValue & """ >" & doc.DisplayName & "</a>", "D" & doc.DocId, IIf(ShowFolderImage, "/DesktopModules/AgapeConnect/Documents/images/YouTube.png", ""), doc.LinkValue, "_self")
                                NewNode.ChildNodes.Add(NewDocNode)
                            Case 2
                                Dim NewDocNode As New TreeNode("<a class=""aLink"" " & IIf(ShowFolderImage, "", "style=""font-size: Medium; color: " & GTreeColor & ";""") & " href=""" & doc.LinkValue & """ >" & doc.DisplayName & "</a>", "D" & doc.DocId, IIf(ShowFolderImage, "/DesktopModules/AgapeConnect/Documents/images/GoogleDoc.png", ""), doc.LinkValue, "_blank")
                                NewNode.ChildNodes.Add(NewDocNode)
                            Case 3
                                'Page on this site
                                Dim NewDocNode As New TreeNode("<a class=""aLink"" " & IIf(ShowFolderImage, "", "style=""font-size: Medium; color: " & GTreeColor & ";""") & " href=""" & doc.LinkValue & """ >" & doc.DisplayName & "</a>", "D" & doc.DocId, IIf(ShowFolderImage, "/DesktopModules/AgapeConnect/Documents/images/URL.png", ""), doc.LinkValue, "_blank")
                                NewNode.ChildNodes.Add(NewDocNode)
                            Case 4
                                Dim perm = GetFilePermission(doc.Permissions)
                                Dim FileURL As String = GetFileUrl(doc.DocId, doc.FileId)

                                If perm = "Edit" Then
                                    Dim NewDocNode As New TreeNode("<a class=""aFile"" " & IIf(ShowFolderImage, "", "style=""font-size: Medium; color: " & GTreeColor & ";""") & " href=""" & FileURL & """ >" & doc.DisplayName & "</a>", "D" & doc.DocId, IIf(ShowFolderImage, GetFileIcon(doc.FileId, doc.LinkType, doc.CustomIcon), ""), FileURL, "_self")
                                    NewNode.ChildNodes.Add(NewDocNode)
                                ElseIf perm = "Read" Then
                                    Dim NewDocNode As New TreeNode("<a  class=""aFileRead"" " & IIf(ShowFolderImage, "", "style=""font-size: Medium; color: " & GTreeColor & ";""") & " href=""" & FileURL & """ >" & doc.DisplayName & "</a>", "D" & doc.DocId, IIf(ShowFolderImage, GetFileIcon(doc.FileId, doc.LinkType, doc.CustomIcon), ""), FileURL, "_self")
                                    NewNode.ChildNodes.Add(NewDocNode)
                                End If
                        End Select



                    Next
                End If

            Next
        End Sub

        Public Function GetIcon(ByVal FileId As Integer?, ByVal Folderid As Integer) As String
            If FileId Is Nothing Then
                Return "images/folder.png"
            Else
                Return GetFileIcon(FileId, 4)
            End If

        End Function

        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
        End Function

        Protected Sub btnAddFolder_Click(sender As Object, e As System.EventArgs) Handles btnAddFolder.Click
            Dim insert As New AP_Documents_Folder
            insert.ParentFolder = tvFolders.SelectedValue.TrimStart("F")
            insert.Name = tbNewFolderName.Text
            insert.Description = tbNewFolderDescription.Text
            insert.PortalId = PortalId
            insert.Permission = (From c In d.AP_Documents_Folders Where c.FolderId = insert.ParentFolder Select c.Permission).First

            d.AP_Documents_Folders.InsertOnSubmit(insert)
            d.SubmitChanges()
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

                        Dim insert As New AP_Documents_Doc
                        insert.FolderId = tvFolders.SelectedValue.TrimStart("F")
                        insert.FileId = theFile.FileId
                        insert.DisplayName = theFile.FileName
                        insert.Author = UserInfo.DisplayName
                        insert.VersionNumber = "1.0"
                        insert.CustomIcon = -1
                        insert.LinkType = "4"  ' a File
                        insert.Permissions = (From c In d.AP_Documents_Folders Where c.FolderId = insert.FolderId Select c.Permission).First
                        d.AP_Documents_Docs.InsertOnSubmit(insert)

                    End If
                Next i

                d.SubmitChanges()

                LoadFolder(tvFolders.SelectedValue.TrimStart("F"))
            Catch ex As Exception

            End Try
        End Sub

        Protected Sub upEditFolder_Load(sender As Object, e As System.EventArgs) Handles upEditFolder.PreRender

            Dim FolderId As Integer = 0
            If hfEditFolderId.Value <> "" Then
                FolderId = CInt(hfEditFolderId.Value)
            End If
            If FolderId = -1 Then
                'Root Folder - Cant Edit!!!
                tbEditFolderName.Text = "root"
                tbEditFolderDescription.Text = "The root folder cannot be edited."
                pnlEditFolder.Enabled = False

            Else
                Dim folder = From c In d.AP_Documents_Folders Where c.PortalId = PortalId And c.FolderId = FolderId

                If folder.Count > 0 Then
                    tbEditFolderName.Text = folder.First.Name
                    tbEditFolderDescription.Text = folder.First.Description
                    pnlEditFolder.Enabled = True
                    btnEditFolder.Enabled = True

                   


                    Dim ReadRoleIds As New ArrayList
                    Dim EditRoleIds As New ArrayList
                    If Not folder.First.Permission Is Nothing Then
                        Dim Permissions = folder.First.Permission.Split(";")

                        If Permissions.Count >= 2 Then
                            For Each row In Permissions(0).Split(":")
                                ReadRoleIds.Add(row.Trim(":"))
                            Next
                            For Each row In Permissions(1).Split(":")
                                EditRoleIds.Add(row.Trim(":"))
                            Next
                        End If

                    End If

                    Dim rc As New DotNetNuke.Security.Roles.RoleController

                    Dim roles = rc.GetPortalRoles(PortalId)
                    Dim PermissionGrid As New ArrayList
                    For Each role As Security.Roles.RoleInfo In roles
                        If role.RoleName <> "Administrators" Then
                            Dim insert As New RolePermission(role.RoleID, role.RoleName, ReadRoleIds.Contains(CStr(role.RoleID)), EditRoleIds.Contains(CStr(role.RoleID)))
                            PermissionGrid.Add(insert)
                        End If

                    Next
                    Dim unauth As New RolePermission(-1, "Unauthenticated Users", ReadRoleIds.Contains("-1"), False)

                    PermissionGrid.Add(unauth)
                    gvFolderPermissions.DataSource = (From c As RolePermission In PermissionGrid)
                    gvFolderPermissions.DataBind()


                    'Load Icons
                    Dim iconFolder As IFolderInfo
                    If Not FolderManager.Instance.FolderExists(PortalId, "acDocuments/icons") Then
                        iconFolder = FolderManager.Instance.AddFolder(PortalId, "acDocuments/icons")
                    Else
                        iconFolder = FolderManager.Instance.GetFolder(PortalId, "acDocuments/icons")
                    End If


                    Dim Files = FolderManager.Instance.GetFiles(iconFolder)

                    Dim CustomIcon As Integer = -1
                    If Not folder.First.CustomIcon Is Nothing Then
                        CustomIcon = folder.First.CustomIcon
                    End If
                    ddlIconsF.DataSource = From c In Files Order By c.FileName Select c.FileName, c.FileId, Path = FileManager.Instance.GetUrl(c), Selected = (c.FileId = CustomIcon)

                    ddlIconsF.DataBind()

                    hfSelectedIcon.Value = CustomIcon



                End If


            End If

        End Sub

        Protected Sub upEditFile_Load(sender As Object, e As System.EventArgs) Handles upEditFile.PreRender

            Dim DocId As Integer = 0
            If hfEditFileId.Value <> "" Then
                DocId = CInt(hfEditFileId.Value)
            End If

            Dim doc = From c In d.AP_Documents_Docs Where c.DocId = DocId

            If doc.Count > 0 Then
                tbEditFileName.Text = doc.First.DisplayName
                tbEditFileDescription.Text = doc.First.Description
                tbEditFileAuthor.Text = doc.First.Author
                tbVersion.Text = doc.First.VersionNumber
                tbKeywords.Text = doc.First.Keywords
                cbTrashed.Checked = doc.First.Trashed


                Dim ReadRoleIds As New ArrayList
                Dim EditRoleIds As New ArrayList
                If Not doc.First.Permissions Is Nothing Then
                    Dim Permissions = doc.First.Permissions.Split(";")

                    If Permissions.Count >= 2 Then
                        For Each row In Permissions(0).Split(":")
                            ReadRoleIds.Add(row.Trim(":"))
                        Next
                        For Each row In Permissions(1).Split(":")
                            EditRoleIds.Add(row.Trim(":"))
                        Next
                    End If

                End If


                Dim rc As New DotNetNuke.Security.Roles.RoleController

                Dim roles = rc.GetPortalRoles(PortalId)
                Dim PermissionGrid As New ArrayList
                For Each role As Security.Roles.RoleInfo In roles
                    If role.RoleName <> "Administrators" Then
                        Dim insert As New RolePermission(role.RoleID, role.RoleName, ReadRoleIds.Contains(CStr(role.RoleID)), EditRoleIds.Contains(CStr(role.RoleID)))
                        PermissionGrid.Add(insert)
                    End If

                Next
                Dim unauth As New RolePermission(-1, "Unauthenticated Users", ReadRoleIds.Contains("-1"), False)

                PermissionGrid.Add(unauth)
                gvPermissions.DataSource = (From c As RolePermission In PermissionGrid)
                gvPermissions.DataBind()



                'Load Versions

                If doc.First.AP_Documents_Versions Is Nothing Then

                    'Add the Current Doc as a Version now
                    AddVersion(doc.First.FileId, doc.First.DocId)

                    hfVersionDocId.Value = doc.First.DocId


                Else
                    hfVersionDocId.Value = doc.First.AP_Documents_Versions.DocId

                End If

                Dim Versions = From c In d.AP_Documents_Versions Where c.DocId = hfVersionDocId.Value Order By c.VersionId Descending
                gvFileVersions.DataSource = Versions
                gvFileVersions.DataBind()



                'Load Icons
                Dim folder As IFolderInfo
                If Not FolderManager.Instance.FolderExists(PortalId, "acDocuments/icons") Then
                    folder = FolderManager.Instance.AddFolder(PortalId, "acDocuments/icons")
                Else
                    folder = FolderManager.Instance.GetFolder(PortalId, "acDocuments/icons")
                End If


                Dim Files = FolderManager.Instance.GetFiles(folder)

                Dim CustomIcon As Integer = -1
                If Not doc.First.CustomIcon Is Nothing Then
                    CustomIcon = doc.First.CustomIcon
                End If
                ddlIcons.DataSource = From c In Files Order By c.FileName Select c.FileName, c.FileId, Path = FileManager.Instance.GetUrl(c), Selected = (c.FileId = CustomIcon)
                
                ddlIcons.DataBind()
            

                hfSelectedIcon.Value = CustomIcon

                Dim tags = From c In d.AP_Documents_Tags Where c.PortalId = PortalId Select c.TagName, c.TagId, Checked = c.AP_Documents_TagMetas.Where(Function(x) x.DocId = doc.First.DocId).Count > 0

                cbTags.DataSource = tags
                cbTags.DataTextField = "TagName"
                cbTags.DataValueField = "Checked"
                cbTags.DataBind()

                For Each row As ListItem In cbTags.Items
                    row.Selected = row.Value
                Next

            End If

        End Sub

        Protected Sub btnEditFile_Click(sender As Object, e As System.EventArgs) Handles btnEditFile.Click
            'Save File (Edit)
            Dim theDoc = From c In d.AP_Documents_Docs Where c.DocId = CInt(hfEditFileId.Value)

            If theDoc.Count > 0 Then
                theDoc.First.DisplayName = tbEditFileName.Text
                theDoc.First.Description = tbEditFileDescription.Text
                theDoc.First.Author = tbEditFileAuthor.Text
                theDoc.First.Keywords = tbKeywords.Text
                theDoc.First.VersionNumber = tbVersion.Text
                theDoc.First.Trashed = cbTrashed.Checked
                If hfSelectedIcon.Value = "" Then
                    theDoc.First.CustomIcon = Nothing
                Else
                    theDoc.First.CustomIcon = CInt(hfSelectedIcon.Value)
                End If
                d.AP_Documents_TagMetas.DeleteAllOnSubmit(From c In d.AP_Documents_TagMetas Where c.DocId = theDoc.First.DocId)
                d.SubmitChanges()
                For Each row As ListItem In cbTags.Items
                    If row.Selected Then
                        Dim theTag = From c In d.AP_Documents_Tags Where c.TagName = row.Text Select c.TagId
                        If theTag.Count > 0 Then
                            Dim insert As New AP_Documents_TagMeta
                            insert.TagId = theTag.First
                            insert.DocId = theDoc.First.DocId
                            d.AP_Documents_TagMetas.InsertOnSubmit(insert)

                        End If

                    End If
                Next
                d.SubmitChanges()
                Dim strRead As String = ""
                For Each row As GridViewRow In gvPermissions.Rows

                    If CType(row.Cells(1).FindControl("cbRead"), CheckBox).Checked Then
                        strRead &= CStr(CType(row.Cells(1).FindControl("hfRoleId"), HiddenField).Value) & ":"
                    End If
                Next
                Dim strEdit As String = ""
                For Each row As GridViewRow In gvPermissions.Rows
                    If CType(row.Cells(2).FindControl("cbEdit"), CheckBox).Checked Then
                        strEdit &= CStr(CType(row.Cells(1).FindControl("hfRoleId"), HiddenField).Value) & ":"
                    End If
                Next
                theDoc.First.Permissions = strRead.TrimEnd(":") & ";" & strEdit.TrimEnd(":")
                d.SubmitChanges()
                LoadFolder(tvFolders.SelectedValue.TrimStart("F"))



            End If


        End Sub

        Protected Sub AddVersion(ByVal FileId As Integer, ByVal DocId As Integer)
            'Check if the doc's file has anoter VersionDocid
            Dim doc = From c In d.AP_Documents_Docs Where c.DocId = DocId
            Dim VersionNumber As String = "1.0"
            If doc.Count > 0 Then
                Dim version = From c In d.AP_Documents_Versions Where c.FileId = doc.First.FileId
                If version.Count > 0 Then
                    DocId = version.First.DocId



                End If
                Dim versions = From c In d.AP_Documents_Versions Where c.DocId = DocId Order By CDbl(c.VersionNumber) Descending


                If version.Count > 0 Then
                    VersionNumber = version.First.VersionNumber.Substring(0, version.First.VersionNumber.LastIndexOf(".") + 1) _
& CInt(version.First.VersionNumber.Substring(version.First.VersionNumber.LastIndexOf(".") + 1)) + 1
                End If

                If (From c In versions Where c.FileId = FileId).Count > 0 Then
                    'alreadyexists!
                    Return
                End If

                Dim insert As New AP_Documents_Version

                insert.DocId = DocId
                insert.FileId = FileId
                insert.VersionNumber = VersionNumber

                d.AP_Documents_Versions.InsertOnSubmit(insert)
                doc.First.FileId = FileId
                doc.First.VersionNumber = VersionNumber
                d.SubmitChanges()
            End If
        End Sub

        Protected Sub btnEditFolder_Click(sender As Object, e As System.EventArgs) Handles btnEditFolder.Click
            'Save Folder (Edit)
            Dim theFolder = From c In d.AP_Documents_Folders Where c.FolderId = CInt(hfEditFolderId.Value) And c.PortalId = PortalId

            If theFolder.Count > 0 Then
                theFolder.First.Name = tbEditFolderName.Text
                theFolder.First.Description = tbEditFolderDescription.Text
                If hfSelectedIcon.Value = "" Then
                    theFolder.First.CustomIcon = Nothing
                Else
                    theFolder.First.CustomIcon = CInt(hfSelectedIcon.Value)
                End If

                Dim strRead As String = ""
                For Each row As GridViewRow In gvFolderPermissions.Rows

                    Dim test = row.Cells(1).FindControl("cbRead")
                    If CType(row.Cells(1).FindControl("cbRead"), CheckBox).Checked Then
                        strRead &= CStr(CType(row.Cells(1).FindControl("hfRoleId"), HiddenField).Value) & ":"
                    End If
                Next
                Dim strEdit As String = ""
                For Each row As GridViewRow In gvFolderPermissions.Rows
                    If CType(row.Cells(2).FindControl("cbEdit"), CheckBox).Checked Then
                        strEdit &= CStr(CType(row.Cells(1).FindControl("hfRoleId"), HiddenField).Value) & ":"
                    End If
                Next
                theFolder.First.Permission = strRead.TrimEnd(":") & ";" & strEdit.TrimEnd(":")
                d.SubmitChanges()
                LoadFolder(tvFolders.SelectedValue.TrimStart("F"))
            End If


        End Sub

        Protected Sub btnSaveVersion_Click(sender As Object, e As System.EventArgs) Handles btnSaveNewVersion.Click
            Dim folder As IFolderInfo
            If Not FolderManager.Instance.FolderExists(PortalId, "acDocuments") Then
                folder = FolderManager.Instance.AddFolder(PortalId, "acDocuments")
            Else
                folder = FolderManager.Instance.GetFolder(PortalId, "acDocuments")
            End If
            'Dim newFile = FileManager.Instance.AddFile(folder,   , fuNewVersion.FileContent)
            ' Dim hfc As HttpFileCollection = Request.Files
            Dim i As Integer = 0
            Dim fileName = fuNewVersion.FileName
            While FileManager.Instance.FileExists(folder, fileName)
                fileName = fuNewVersion.FileName.Insert(fuNewVersion.FileName.LastIndexOf("."), "_" & i)
                i += 1
            End While
            Dim newFile = FileManager.Instance.AddFile(folder, fileName, fuNewVersion.FileContent)
            AddVersion(newFile.FileId, hfVersionDocId.Value)
            upEditFile_Load(Me, Nothing)
            Dim t1 As Type = Me.GetType()
            Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb1.Append("<script language='javascript'>")
            sb1.Append(" $(document).ready(function () { showEditFile(); }); ")
            sb1.Append("</script>")
            ScriptManager.RegisterStartupScript(Page, t1, "popup", sb1.ToString, False)
        End Sub

        Protected Function GetFileIcon(ByVal FileId As Integer?, ByVal LinkType As Integer, Optional IconId As Integer? = -1) As String

            If Not IconId Is Nothing And IconId > 0 Then
                Return FileManager.Instance.GetUrl(FileManager.Instance.GetFile(IconId))
            End If
            If FileId Is Nothing Then
                Return "images/folder.png"
            End If

          
            Dim Path As String = "images/"
            Dim theFile = FileManager.Instance.GetFile(FileId)
           

            If FileId = -2 Then
                Select Case LinkType
                    Case 0 : Return Path & "URL.png"
                    Case 1 : Return Path & "YouTube.png"
                    Case 2 : Return Path & "GoogleDoc.png"
                    Case 3 : Return Path & "Url.png"

                End Select
            End If

            If Not theFile Is Nothing Then



                Select Case theFile.Extension
                    Case "gif"
                        Return Path & "GIF.png"
                    Case "bmp"
                        Return Path & "BMP.png"
                    Case "doc"
                        Return Path & "DOC.png"
                    Case "jpg"
                        Return Path & "JPG.png"
                    Case "mov"
                        Return Path & "MOV.png"
                    Case "mp3"
                        Return Path & "MP3.png"
                    Case "mp4"
                        Return Path & "MP4.png"
                    Case "mpg"
                        Return Path & "MPG.png"
                    Case "pdf"
                        Return Path & "PDF.png"

                    Case "png"
                        Return Path & "PNG.png"
                    Case "psd"
                        Return Path & "PSD.png"
                    Case "tiff"
                        Return Path & "TIFF.png"
                    Case "txt"
                        Return Path & "TXT.png"
                    Case "wav"
                        Return Path & "WAV.png"
                    Case "zip"
                        Return Path & "ZIP.png"


                    Case Else
                        Return Path & "Blank.png"

                End Select

            End If
            Return "images/Blank.png"


        End Function

        Public Function GetFileUrl(ByVal DocId As Integer, ByVal FileId As Integer) As String
            If FileId = -2 Then
                Dim theDoc = From c In d.AP_Documents_Docs Where c.DocId = DocId

                Select Case theDoc.First.LinkType
                    Case 0, 2
                        Return theDoc.First.LinkValue
                    Case 1
                        Return "http://www.youtube.com"
                    Case 3
                        Return NavigateURL(CInt(theDoc.First.LinkValue))
                End Select
            End If

            Dim theFile = FileManager.Instance.GetFile(FileId)
            If Not theFile Is Nothing Then
                Dim rtn = EditUrl("DocumentViewer") ' FileManager.Instance.GetUrl(theFile)

                If rtn.Contains("?") Then
                    rtn &= "&DocId=" & DocId
                Else
                    rtn &= "?DocId=" & DocId
                End If
                Return rtn
            Else
                Return EditUrl("DocumentViewer") & "?DocId=" & DocId
            End If
        End Function

        Public Function GetFileDate(ByVal FileId As Integer) As String
            Return DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileId).LastModificationTime.ToString("dd MMM yyyy")

        End Function

        Protected Sub btnNewLink_Click(sender As Object, e As System.EventArgs) Handles btnNewLink.Click
            Dim insert As New AP_Documents_Doc
            insert.Author = tbNewLinkAuthor.Text
            insert.Description = tbNewLinkDescripiton.Text
            insert.DisplayName = tbNewLinkName.Text
            insert.LinkType = rbLinkType.SelectedValue
            insert.FileId = -2
            insert.FolderId = CInt(tvFolders.SelectedValue.TrimStart("F"))
            insert.Permissions = (From c In d.AP_Documents_Folders Where c.FolderId = insert.FolderId Select c.Permission).First
            Select Case insert.LinkType
                Case 0
                    insert.LinkValue = IIf(tbURL.Text.StartsWith("http"), tbURL.Text, "http://" & tbURL.Text)
                    'To Do: link icon
                Case 1
                    If tbURL.Text.Contains("=") Then
                        insert.LinkValue = tbURL.Text.Substring(tbURL.Text.LastIndexOf("=") + 1)
                    ElseIf tbURL.Text.Contains("/") Then
                        'Restful
                        insert.LinkValue = tbURL.Text.Substring(tbURL.Text.LastIndexOf("/") + 1)
                    Else
                        'video code
                        insert.LinkValue = tbURL.Text
                    End If
                    'To Do: YouTube icon
                Case 2
                    insert.LinkValue = IIf(tbURL.Text.StartsWith("http"), tbURL.Text, "http://" & tbURL.Text)
                    'To Do: Google icon
                Case 3
                    insert.LinkValue = ddlPages.SelectedValue
                Case 4
                    insert.FileId = ddlFiles.SelectedValue

            End Select
            d.AP_Documents_Docs.InsertOnSubmit(insert)
            d.SubmitChanges()
            LoadFolder(tvFolders.SelectedValue.TrimStart("F"))
        End Sub

        Protected Sub btnNewIcon_Click(sender As Object, e As System.EventArgs) Handles btnNewIcon.Click
            Dim folder As IFolderInfo

            If Not FolderManager.Instance.FolderExists(PortalId, "acDocuments/icons") Then
                folder = FolderManager.Instance.AddFolder(PortalId, "acDocuments/icons")
            Else
                folder = FolderManager.Instance.GetFolder(PortalId, "acDocuments/icons")
            End If
            'Dim newFile = FileManager.Instance.AddFile(folder,   , fuNewVersion.FileContent)
            ' Dim hfc As HttpFileCollection = Request.Files


            Dim i As Integer = 0
            Dim ext As String() = {".jpg", "jpeg", ".gif", ".png"}
            Dim fileName = fuNewIcon.FileName
            If Not ext.Contains(Right(fileName, 4)) Then
                Dim t2 As Type = Me.GetType()
                Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb2.Append("<script language='javascript'>")

                sb2.Append(" $(document).ready(function () { showEditFile(); }); ")
                sb2.Append("alert('Invalid image file. Icon files must be of type .png, .gif or .jpg');")
                sb2.Append("</script>")
                ScriptManager.RegisterStartupScript(Page, t2, "popup2", sb2.ToString, False)
                Return
            End If
            While FileManager.Instance.FileExists(folder, fileName)
                fileName = fuNewIcon.FileName.Insert(fuNewIcon.FileName.LastIndexOf("."), "_" & i)

                i += 1
            End While

            Dim newFile = FileManager.Instance.AddFile(folder, fileName, fuNewIcon.FileContent)

            If hfNewIconMode.Value = "Folder" Then

                Dim thisFolder = From c In d.AP_Documents_Folders Where c.FolderId = CInt(hfEditFolderId.Value)

                thisFolder.First.CustomIcon = newFile.FileId
                upEditFolder_Load(Me, Nothing)
                Dim t1 As Type = Me.GetType()
                Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb1.Append("<script language='javascript'>")
                sb1.Append(" $(document).ready(function () { showEditFolder(); }); ")
                sb1.Append("</script>")
                ScriptManager.RegisterStartupScript(Page, t1, "popup", sb1.ToString, False)
            Else
                Dim thisDoc = From c In d.AP_Documents_Docs Where c.DocId = CInt(hfEditFileId.Value)

                thisDoc.First.CustomIcon = newFile.FileId
                d.SubmitChanges()

                upEditFile_Load(Me, Nothing)
                LoadFolder(thisDoc.First.FolderId)

                Dim t1 As Type = Me.GetType()
                Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb1.Append("<script language='javascript'>")
                sb1.Append(" $(document).ready(function () { showEditFile(); }); ")
                sb1.Append("</script>")
                ScriptManager.RegisterStartupScript(Page, t1, "popup", sb1.ToString, False)
            End If

        End Sub

        Protected Sub btnSettings_Click(sender As Object, e As System.EventArgs) Handles btnSettings.Click
            Response.Redirect(EditUrl("DocumentSettings"))

        End Sub

        Protected Sub dlFolderView_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles dlFolderView.ItemDataBound
            Dim btneditdoc As HyperLink = CType(e.Item.FindControl("btnEditDoc"), HyperLink)
            'Dim btndeletedoc As HyperLink = CType(e.Item.FindControl("btnDeleteDoc"), HyperLink)
            Dim hyperlink1 As HyperLink = CType(e.Item.FindControl("HyperLink1"), HyperLink)
            btneditdoc.NavigateUrl = "javascript:editButtonClick(" & hyperlink1.ClientID & ")"
            'btndeletedoc.NavigateUrl = "javascript:deleteButtonClick(" & hyperlink1.ClientID & ")"
            btneditdoc.Visible = IsEditable
            'btndeletedoc.Visible = IsEditable
        End Sub

    End Class

    Public Class RolePermission
        Private _roleName As String
        Public Property RoleName() As String
            Get
                Return _roleName
            End Get
            Set(ByVal value As String)
                _roleName = value
            End Set
        End Property

        Private _edit As Boolean
        Public Property Edit() As Boolean
            Get
                Return _edit
            End Get
            Set(ByVal value As Boolean)
                _edit = value
            End Set
        End Property
        Private _read As Boolean
        Public Property Read() As Boolean
            Get
                Return _read
            End Get
            Set(ByVal value As Boolean)
                _read = value
            End Set
        End Property

        Private _roleId As Integer
        Public Property RoleId() As Integer
            Get
                Return _roleId
            End Get
            Set(ByVal value As Integer)
                _roleId = value
            End Set
        End Property

        Public Sub New(ByVal rId As Integer, ByVal name As String, ByVal canRead As Boolean, ByVal canEdit As Boolean)
            RoleName = name
            Read = canRead
            Edit = canEdit
            RoleId = rId
        End Sub
    End Class

End Namespace
