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
    Partial Class ViewPDF
        Inherits Entities.Modules.PortalModuleBase
        Dim d As New AgapeDocumentsDataContext
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request.QueryString("DocId") <> "" Then
                Dim q = From c In d.Agape_Main_AgapeDocuments Where c.AgapeDocumentId = Request.QueryString("DocId")
                If q.Count > 0 Then
                    'pdfFrame.Attributes("src") = "http://docs.google.com/viewer?url=http://www.agape.org.uk/Portals/0/AccommodationHoniley2010.pdf&embedded=true"
                    ' pdfFrame.Attributes("src") = "http://docs.google.com/viewer?url=" & Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & GetDocument(q.First.FileId, q.First.URL, q.First.LinkType) & "&embedded=true"
                    ShowPdf1.FilePath = GetDocument(q.First.FileId, q.First.URL, q.First.LinkType).Replace(" ", "%20")
                    'pdfFrame.Attributes("src") = Resources.GlobalFunctions.GetDownloadURL("FileID=" & q.First.FileId).Replace(" ", "%20")
                End If
            End If
            If UserId < 0 Then
                pnlAddComment.Visible = False
                pnlNotLoggedIn.Visible = True
            Else
                pnlAddComment.Visible = True
                pnlNotLoggedIn.Visible = False
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
        

        Protected Sub AddCommentButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AddCommentButton.Click
            Dim insert As New Agape_Main_AgapeDocuments_Comment
            insert.AgapeDocumentId = Request.QueryString("DocId")
            insert.Comment = CommentText.Text
            insert.UserId = UserId
            insert.CreatedDate = Now
            d.Agape_Main_AgapeDocuments_Comments.InsertOnSubmit(insert)
            d.SubmitChanges()
            DataList1.DataBind()

        End Sub

        Protected Sub DataList1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles DataList1.ItemCommand
            If e.CommandName = "DeleteComment" Then
                Dim q = From c In d.Agape_Main_AgapeDocuments_Comments Where c.CommentId = CInt(e.CommandArgument)
                d.Agape_Main_AgapeDocuments_Comments.DeleteAllOnSubmit(q)
                d.SubmitChanges()
                DataList1.DataBind()
            End If
        End Sub

        Public Function CanDelete(ByVal UID As Integer) As String
            If IsEditable Or UID = UserId Then
                Return "True"
            Else
                Return "False"

            End If
        End Function



    End Class
End Namespace
