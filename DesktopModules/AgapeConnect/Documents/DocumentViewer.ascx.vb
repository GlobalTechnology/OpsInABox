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
Imports Documents
Imports DotNetNuke.Services.FileSystem
Namespace DotNetNuke.Modules.Documents
    Partial Class DocumentViewer
        Inherits Entities.Modules.PortalModuleBase
        Dim d As New DocumentsDataContext


        Public Function GetFilePermission(ByVal Permissions As String) As String
            Dim UserRoles As ArrayList
            Dim rc As New DotNetNuke.Security.Roles.RoleController
            UserRoles = rc.GetUserRoles(PortalId, UserId)
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

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim theDoc = From c In d.AP_Documents_Docs Where c.DocId = Request.QueryString("docId")

            'validatePermisisons
            Dim rights = GetFilePermission(theDoc.First.Permissions)
            If rights = "None" Then
                lblError.Text = "Error: you do not have permissions to view this document"
                lblError.Visible = True
                theMainPanel.Visible = False
                Return
            Else
                lblError.Text = ""
                lblError.Visible = False
                theMainPanel.Visible = True
            End If




            Dim theFile = FileManager.Instance.GetFile(theDoc.First.FileId)
            Dim theVersionNumber = IIf(theDoc.First.VersionNumber = "1.0", "", "(" & theDoc.First.VersionNumber & ")")

            If Request.QueryString("Version") <> "" Then

                Dim theVersion = (From c In d.AP_Documents_Versions Where c.DocId = theDoc.First.DocId And c.FileId = CInt(Request.QueryString("Version"))).First
                theFile = FileManager.Instance.GetFile(theVersion.FileId)
                theVersionNumber = "(" & theVersion.VersionNumber & ")"
            End If
            hfFileId.Value = theFile.FileId

            Dim Googletypes As String() = {"pdf", "doc", "docx", "ppt", "tiff"}

            Dim theFileURL = Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & FileManager.Instance.GetUrl(theFile)

            lblFileUrl.Text = theFileURL
            lblFileName.Text = theDoc.First.DisplayName & "<span style=""font-size: large; font-style: italic;"">" & theVersionNumber & "</span>"
            btnDownload.Text = "Download (" & FormatFileSize(theFile.Size) & ")"
            Dim Versions = From c In d.AP_Documents_Versions Where c.DocId = theDoc.First.DocId Order By c.VersionId Descending
            gvFileVersions.DataSource = Versions 'theDoc.First.AP_Documents_Versions
            gvFileVersions.DataBind()
            If theFile.Width > 0 Then
                Viewer.Attributes("Width") = theFile.Width
            End If
            If theFile.Height > 0 Then
                Viewer.Attributes("Height") = theFile.Height
            End If


            If Googletypes.Contains(theFile.Extension) Then
                'Viewer.Attributes("src") = "http://docs.google.com/viewer?url=" & Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & theFileURL & "&embedded=true"
                ' theFileURL = "http://www.agape.org.uk/Portals/0/Staff/LD/Personal%20Development%20Plan%20(07%20version).doc"

                Viewer.Attributes("src") = "http://docs.google.com/viewer?url=" & theFileURL & "&embedded=true"

            Else


                Viewer.Attributes("src") = theFileURL

            End If



            LoadComments(theDoc.First.DocId)
        End Sub
        Public Shared Function FormatFileSize(ByVal FileSizeBytes As Long) As String
            Dim sizeTypes() As String = {"b", "Kb", "Mb", "Gb"}
            Dim Len As Decimal = FileSizeBytes
            Dim sizeType As Integer = 0
            Do While Len > 1024
                Len = Decimal.Round(Len / 1024, 2)
                sizeType += 1
                If sizeType >= sizeTypes.Length - 1 Then Exit Do
            Loop

            Dim Resp As String = Len.ToString & " " & sizeTypes(sizeType)
            Return Resp
        End Function

        Protected Sub LoadComments(ByVal DocId As Integer)
            phComments.Controls.Clear()

            Dim comments = From c In d.AP_Documents_Comments Where c.DocId = DocId

            For Each row In comments
                If UserId = row.UserId Then

                    Dim bubble As String = "<div id=C" & row.CommentId & " class=""myBubble"">" & row.Comment & "</div><div class=""subMyBubble"">" & "You " & row.CreatedDate.Value.ToLongTimeString & "</div>"
                    phComments.Controls.Add(New LiteralControl(bubble))

                Else
                    Dim thePerson = UserController.GetUserById(PortalId, row.UserId)
                    Dim bubble As String = "<div id=C" & row.CommentId & " class=""bubble"">" & row.Comment & "</div><div class=""subBubble"">" & thePerson.DisplayName & " " & row.CreatedDate.Value.ToLongTimeString & "</div>"
                    phComments.Controls.Add(New LiteralControl(bubble))

                End If
               



            Next
        End Sub





        Protected Sub btnDownload_Click(sender As Object, e As System.EventArgs) Handles btnDownload.Click
            Dim theFile = FileManager.Instance.GetFile(CInt(hfFileId.Value))
            Response.Clear()

            Response.AddHeader("content-disposition", "attachment; filename=" + theFile.FileName)
            Response.WriteFile(theFile.PhysicalPath)
            Response.ContentType = ""

            Response.End()

        End Sub

        Protected Sub btnAddMessage_Click(sender As Object, e As System.EventArgs) Handles btnAddMessage.Click
            If tbMessage.Text <> "" And tbMessage.Text <> "Enter comment here..." Then


                Dim insert As New AP_Documents_Comment
                insert.UserId = UserId
                insert.DocId = CInt(Request.QueryString("DocId"))
                insert.CreatedDate = Now
                insert.Comment = tbMessage.Text
                d.AP_Documents_Comments.InsertOnSubmit(insert)
                d.SubmitChanges()
                LoadComments(insert.DocId)
                tbMessage.Text = ""
            End If
        End Sub

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


        Protected Sub commentUpdatePanel_Load(sender As Object, e As System.EventArgs) Handles commentUpdatePanel.Load

            If hfCommentId.Value <> "" Then
                Dim cid As Integer = CInt(hfCommentId.Value.TrimStart("C"))
                Dim theComment = From c In d.AP_Documents_Comments Where c.CommentId = cid And c.DocId = CInt(Request.QueryString("DocId"))

                d.AP_Documents_Comments.DeleteAllOnSubmit(theComment)
                d.SubmitChanges()
                LoadComments(Request.QueryString("DocId"))
            End If

            hfCommentId.Value = ""
        End Sub
    End Class


End Namespace
