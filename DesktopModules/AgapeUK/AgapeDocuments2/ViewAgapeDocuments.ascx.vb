Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports DotNetNuke
Imports DotNetNuke.Security
Imports UK.AgapeDocuments

Namespace DotNetNuke.Modules.AgapeDocuments
    Partial Class ViewAgapeDocuments
        Inherits Entities.Modules.PortalModuleBase
        Dim d As New AgapeDocumentsDataContext
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

            'Dim q = From c In d.Agape_Main_AgapeDocuments Where c.PortalId = PortalId And c.ModuleId = ModuleId Order By c.SortOrder

            'DataList1.DataSource = q
            'DataList1.RepeatColumns = Settings("Columns")

            'DataList1.DataBind()

            If Not Page.IsPostBack() Then


                Dim r = From c In d.Agape_Main_AgapeDocuments Where c.PortalId = PortalId And c.ModuleId = ModuleId Order By c.SortOrder Group By c.Subtitle Into Group
                TreeView1.Nodes.Clear()
                For Each row In r
                    Dim insertNode = New TreeNode("<span class=""tbLinkText"" style=""color:" & GetForeColor() & """>" & row.Subtitle & "</span>")

                    TreeView1.Nodes.Add(insertNode)
                    For Each doc In row.Group
                        insertNode.ChildNodes.Add(New TreeNode("<span class=""ToolboxLink"" style=""color:" & GetForeColor() & """>" & doc.DocTitle & "</span>", doc.AgapeDocumentId, "", GetDocument(doc.FileId, doc.URL, doc.LinkType), "_blank"))
                    Next

                Next


                TreeView1.ExpandImageUrl = "Arrow" & GetBulletName() & "R.gif"
                TreeView1.CollapseImageUrl = "Arrow" & GetBulletName() & "D.gif"


                AddButton.Visible = IsEditable
                EditBtn.Visible = IsEditable
            End If

        End Sub
        Public Function GetDocument(ByVal FileId As Integer, ByVal URL As String, ByVal LinkType As String) As String
            If LinkType = "F" Then
                Dim ImageFileId As Integer = Integer.Parse(FileId)
                Dim objFileController As New DotNetNuke.Services.FileSystem.FileController
                Dim objImageInfo As DotNetNuke.Services.FileSystem.FileInfo = objFileController.GetFileById(ImageFileId, PortalId)
                Return PortalSettings.HomeDirectory & objImageInfo.Folder & objImageInfo.FileName
            Else
                Return URL
            End If




        End Function



        Public Function GetIcon(ByVal FileId As Integer, ByVal URL As String, ByVal LinkType As String) As String
            If LinkType = "F" Then
                Dim ImageFileId As Integer = Integer.Parse(FileId)
                Dim objFileController As New DotNetNuke.Services.FileSystem.FileController
                Dim objImageInfo As DotNetNuke.Services.FileSystem.FileInfo = objFileController.GetFileById(ImageFileId, PortalId)
                Select Case objImageInfo.Extension
                    Case "gif"
                        Return "~/images/FileIcons/GIF.png"
                    Case "bmp"
                        Return "~/images/FileIcons/BMP.png"
                    Case "doc"
                        Return "~/images/FileIcons/DOC.png"
                    Case "jpg"
                        Return "~/images/FileIcons/JPG.png"
                    Case "mov"
                        Return "~/images/FileIcons/MOV.png"
                    Case "mp3"
                        Return "~/images/FileIcons/MP3.png"
                    Case "mp4"
                        Return "~/images/FileIcons/MP4.png"
                    Case "mpg"
                        Return "~/images/FileIcons/MPG.png"
                    Case "png"
                        Return "~/images/FileIcons/PNG.png"
                    Case "psd"
                        Return "~/images/FileIcons/PSD.png"
                    Case "tiff"
                        Return "~/images/FileIcons/TIFF.png"
                    Case "txt"
                        Return "~/images/FileIcons/TXT.png"
                    Case "wav"
                        Return "~/images/FileIcons/WAV.png"
                    Case "zip"
                        Return "~/images/FileIcons/ZIP.png"


                    Case Else
                        Return "~/images/FileIcons/Blank.png"

                End Select


            Else
                Return "~/images/FileIcons/Blank.png"
            End If
        End Function

        Protected Sub AddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddButton.Click
            Response.Redirect(EditUrl("Add"))

        End Sub

        Protected Sub EditBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditBtn.Click
            Response.Redirect(EditUrl("Layout"))
        End Sub

        Public Function GetForeColor() As String
            Dim ParentSkin = DotNetNuke.UI.Skins.Skin.GetParentSkin(Me).ToString

            If ParentSkin.IndexOf("win") > 0 Then
                Return "#28686E"
            ElseIf ParentSkin.IndexOf("build") > 0 Then
                Return "#86bb41"
            ElseIf ParentSkin.IndexOf("send") > 0 Then
                Return "#8c3b3b"
            ElseIf ParentSkin.IndexOf("maps") > 0 Then
                Return "#876c49"
            ElseIf ParentSkin.IndexOf("coaching") > 0 Then
                Return "#f1a519"
            ElseIf ParentSkin.IndexOf("everyone") > 0 Then
                Return "#1f594f"
            End If

             
            Return "#28686E"


        End Function
        Public Function GetBulletName() As String
            Dim ParentSkin = DotNetNuke.UI.Skins.Skin.GetParentSkin(Me).ToString

            If ParentSkin.IndexOf("win") > 0 Then
                Return "Win"
            ElseIf ParentSkin.IndexOf("build") > 0 Then
                Return "Build"
            ElseIf ParentSkin.IndexOf("send") > 0 Then
                Return "Send"
            ElseIf ParentSkin.IndexOf("maps") > 0 Then
                Return "Maps"
            ElseIf ParentSkin.IndexOf("coaching") > 0 Then
                Return "Coaching"
            ElseIf ParentSkin.IndexOf("everyone") > 0 Then
                Return "Everyone"
            End If


            Return "Win"


        End Function

        Protected Sub TreeView1_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView1.SelectedNodeChanged
            If TreeView1.SelectedNode.Depth = 0 Then
                TreeView1.SelectedNode.ToggleExpandState()
            End If
        End Sub
    End Class
End Namespace
