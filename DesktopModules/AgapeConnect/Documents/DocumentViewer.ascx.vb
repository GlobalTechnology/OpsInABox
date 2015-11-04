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
Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class DocumentViewer
        Inherits Entities.Modules.PortalModuleBase
        Dim d As New DocumentsDataContext

        ' Check if the current user is authorized to view the document
        Public Function isAuthorized() As Boolean
            Dim rc As New DotNetNuke.Security.Roles.RoleController
            Dim UserRoles = rc.GetUserRoles(PortalId, UserId)
            Return UserInfo.IsSuperUser Or UserInfo.IsInRole("Administrators")
        End Function

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim theDoc = From c In d.AP_Documents_Docs Where c.DocId = Request.QueryString("docId")

            'Check permission
            If Not isAuthorized() Then
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

            Dim Googletypes As String() = {"pdf", "doc", "docx", "ppt", "tiff"}

            Dim theFileURL = Request.Url.Scheme & "://" & Request.Url.Authority & Request.ApplicationPath & FileManager.Instance.GetUrl(theFile)

            lblFileUrl.Text = theFileURL
            lblFileName.Text = theDoc.First.DisplayName & "<span style=""font-size: large; font-style: italic;"">" & theVersionNumber & "</span>"
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

    End Class

End Namespace
