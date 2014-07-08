Imports Microsoft.VisualBasic
Imports DotNetNuke
Imports DotNetNuke.Services.FileSystem

Namespace Documents
    Public Class DocumentsController
        Implements Entities.Modules.ISearchable

        Public Function CheckPermissions() As Boolean  ' want a different type: Read Write None
            Return True
        End Function


        Public Function GetSearchItems(ModInfo As DotNetNuke.Entities.Modules.ModuleInfo) As DotNetNuke.Services.Search.SearchItemInfoCollection Implements DotNetNuke.Entities.Modules.ISearchable.GetSearchItems

            Dim d As New DocumentsDataContext

            Dim SearchItemCollection As New Services.Search.SearchItemInfoCollection
            Dim Folders = From c In d.AP_Documents_Folders Where c.PortalId = ModInfo.PortalID

            For Each row In Folders


                Dim SearchItem As Services.Search.SearchItemInfo
                SearchItem = New Services.Search.SearchItemInfo _
                (row.Name, _
                row.Description, _
                1, _
               Today, ModInfo.ModuleID, _
                "F" & row.FolderId, _
             row.Name & " " & row.Description, Guid:="FolderId=" & row.FolderId)
                SearchItemCollection.Add(SearchItem)
            Next
            Dim Docs = From c In d.AP_Documents_Docs Where c.AP_Documents_Folder.PortalId = ModInfo.PortalID

            For Each row In Docs
                Dim tags As String = ""
                For Each tag In row.AP_Documents_TagMetas
                    tags &= tag.AP_Documents_Tag.TagName & " "
                Next

                Dim SearchText = (row.DisplayName & " " & row.LinkType & " " & row.Keywords & " " & tags & " " & row.Description).Replace(".", " ").Replace(";", " ").Replace("-", " ").Replace(":", " ")


                Dim SearchItem As Services.Search.SearchItemInfo
                SearchItem = New Services.Search.SearchItemInfo _
                (row.DisplayName, _
                row.Description, _
                1, _
               Today, ModInfo.ModuleID, _
                "D" & row.DocId, _
              SearchText, Guid:="DocId=" & row.DocId)
                SearchItemCollection.Add(SearchItem)
            Next



            Return SearchItemCollection

        End Function
        Public Function GetIcon(ByVal FileId As Integer?, ByVal Folderid As Integer) As String
            If FileId Is Nothing Then
                Return "images/folder.png"
            Else
                Return GetFileIcon(FileId, 4)
            End If

        End Function

        Protected Function GetFileIcon(ByVal FileId As Integer?, ByVal LinkType As Integer, Optional IconId As Integer? = -1) As String


            If FileId Is Nothing Then
                Return "images/folder.png"
            End If
            Dim Path As String = "images/"
            Dim theFile = FileManager.Instance.GetFile(FileId)
            If Not IconId Is Nothing And IconId > 0 Then
                Return FileManager.Instance.GetUrl(FileManager.Instance.GetFile(IconId))
            End If

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


    End Class
End Namespace
