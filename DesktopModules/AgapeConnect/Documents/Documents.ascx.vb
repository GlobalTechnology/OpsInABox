Imports Documents ' TODO after moving GetFileUrl delete this
Imports DotNetNuke.Services.FileSystem

Namespace DotNetNuke.Modules.AgapeConnect.Documents
    Partial Class Documents
        Inherits Entities.Modules.PortalModuleBase
        'Dim d As New DocumentsDataContext()
        'Public templateMode As String = "Icons"


        'Dim rc As New DotNetNuke.Security.Roles.RoleController
        'Dim UserRoles As ArrayList
        'Dim doc As Object
        'Dim GTreeColor As String
        'Dim TreeStyles() As String = {"Explorer", "GTree", "Tree"}

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        End Sub





        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


            If Not IsPostBack Then
                LoadFolder()
            End If



        End Sub

        Protected Sub LoadFolder()
            Dim FolderId As Integer = DocumentsController.GetRootFolderId(Settings)

            Dim Items As New ArrayList
            Dim rc As New DotNetNuke.Security.Roles.RoleController
            Dim UserRoles = rc.GetUserRoles(PortalId, UserId)

            Dim Docs = DocumentsController.GetDocuments(Settings)

            For Each document In Docs
                If document.Trashed = False Then
                    Items.Add(document)
                End If

            Next
            dlFolderView.DataSource = Items
            dlFolderView.DataBind()

        End Sub



        Public Function GetIcon(ByVal FileId As Integer?, ByVal Folderid As Integer) As String

            Return GetFileIcon(FileId, 4)

        End Function

        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
        End Function


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
            Dim d As New DocumentsDataContext()

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



        'Protected Sub btnSettings_Click(sender As Object, e As System.EventArgs) Handles btnSettings.Click
        '    Response.Redirect(EditUrl("DocumentSettings"))

        'End Sub

    End Class

End Namespace
