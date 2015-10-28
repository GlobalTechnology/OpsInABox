Imports Microsoft.VisualBasic
'Imports DotNetNuke
Imports DotNetNuke.Services.FileSystem
Imports Documents


#Region "Modules defining constant values"

' List of view styles
Public Module StyleType
    Public Const View1 As String = "List"
End Module

' Folder constants
Public Module FolderConstants
    Public Const NoParentFolderId As Integer = -1
    Public Const RootFolderSettingKey As String = "RootFolder" 'cooresponds to a folder id in the settings table
    Public Const DocumentsModuleRootFolderName As String = "acDocuments"

End Module

#End Region ' Modules defining constant values

Public Class DocumentsController

#Region "????"

    Public Shared Function GetRootFolderId(ByRef moduleSettings As System.Collections.Hashtable) As Integer
        Dim d As New DocumentsDataContext()
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim rootFolderId As Integer = moduleSettings(RootFolderSettingKey)

        If String.IsNullOrEmpty(rootFolderId) Then

            Dim rootNode = From c In d.AP_Documents_Folders Where c.PortalId = PS.PortalId And c.ParentFolder = FolderConstants.NoParentFolderId
            'No rootNode found
            If rootNode.Count = 0 Then

                'TODO : regarde si acDocuments existe

                'Add the rootNode acDocuments in the database
                Dim insert As New AP_Documents_Folder
                insert.CustomIcon = Nothing
                insert.ParentFolder = NoParentFolderId
                insert.Name = DocumentsModuleRootFolderName
                insert.Permission = moduleSettings("DefaultPermissions")
                insert.PortalId = PS.PortalId
                d.AP_Documents_Folders.InsertOnSubmit(insert)
                d.SubmitChanges()

                rootFolderId = insert.FolderId
            End If

        End If
        Return rootFolderId
    End Function

    Public Shared Function GetDocuments(ByRef moduleSettings As System.Collections.Hashtable) As IQueryable(Of AP_Documents_Doc)
        Dim d As New DocumentsDataContext()
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim docs As IQueryable(Of AP_Documents_Doc) = From c In d.AP_Documents_Docs Where c.AP_Documents_Folder.PortalId = PS.PortalId

        ' TODO filter by rootfolder
        Return docs

    End Function

    Public Shared Function GetDocument(ByVal DocId As Integer) As AP_Documents_Doc
        Dim d As New DocumentsDataContext()
        Dim theDoc = From c In d.AP_Documents_Docs Where c.DocId = DocId
        Return theDoc.First
    End Function

    Public Shared Function GetFileIcon(ByVal FileId As Integer?, ByVal LinkType As Integer, Optional IconId As Integer? = -1) As String
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
            Select Case theFile.Extension.ToLower
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

#End Region

#Region "Add/Edit"

    'Public Shared Sub InsertDocument(ByVal FileId As Integer, FileName As String, Author As String, Permissions As String)
    Public Shared Sub InsertDocument(ByVal FileId As Integer, FileName As String, Author As String, ByRef moduleSettings As System.Collections.Hashtable)
        Dim d As New DocumentsDataContext()
        Dim insert As New AP_Documents_Doc
        insert.FolderId = GetRootFolderId(moduleSettings)
        insert.FileId = FileId
        insert.DisplayName = FileName
        insert.Author = Author
        insert.VersionNumber = "1.0"
        insert.CustomIcon = -1
        insert.LinkType = "4"  ' a File
        'insert.Permissions = Permissions  Todo: determine permissions implementation
        d.AP_Documents_Docs.InsertOnSubmit(insert)
        d.SubmitChanges()
    End Sub

#End Region


End Class

' Implements Entities.Modules.ISearchable

'Public Function CheckPermissions() As Boolean  ' want a different type: Read Write None
'    Return True
'End Function


'Public Function GetSearchItems(ModInfo As DotNetNuke.Entities.Modules.ModuleInfo) As DotNetNuke.Services.Search.SearchItemInfoCollection Implements DotNetNuke.Entities.Modules.ISearchable.GetSearchItems

'    Dim d As New DocumentsDataContext

'    Dim SearchItemCollection As New Services.Search.SearchItemInfoCollection
'    Dim Folders = From c In d.AP_Documents_Folders Where c.PortalId = ModInfo.PortalID

'    For Each row In Folders


'        Dim SearchItem As Services.Search.SearchItemInfo
'        SearchItem = New Services.Search.SearchItemInfo _
'        (row.Name, _
'        row.Description, _
'        1, _
'       Today, ModInfo.ModuleID, _
'        "F" & row.FolderId, _
'     row.Name & " " & row.Description, Guid:="FolderId=" & row.FolderId)
'        SearchItemCollection.Add(SearchItem)
'    Next
'    Dim Docs = From c In d.AP_Documents_Docs Where c.AP_Documents_Folder.PortalId = ModInfo.PortalID

'    For Each row In Docs
'        Dim tags As String = ""
'        For Each tag In row.AP_Documents_TagMetas
'            tags &= tag.AP_Documents_Tag.TagName & " "
'        Next

'        Dim SearchText = (row.DisplayName & " " & row.LinkType & " " & row.Keywords & " " & tags & " " & row.Description).Replace(".", " ").Replace(";", " ").Replace("-", " ").Replace(":", " ")


'        Dim SearchItem As Services.Search.SearchItemInfo
'        SearchItem = New Services.Search.SearchItemInfo _
'        (row.DisplayName, _
'        row.Description, _
'        1, _
'       Today, ModInfo.ModuleID, _
'        "D" & row.DocId, _
'      SearchText, Guid:="DocId=" & row.DocId)
'        SearchItemCollection.Add(SearchItem)
'    Next



'    Return SearchItemCollection

'End Function
