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


            FixSortOrder()
            Dim q = From c In d.Agape_Main_AgapeDocuments Where c.PortalId = PortalId And c.ModuleId = ModuleId Order By c.SortOrder

            DataList1.DataSource = q
            Dim Columns As Integer = 3
            If Settings("Columns") <> "" Then
                Columns = Settings("Columns")
            End If
            Dim ModWidth As Integer = 1000
            If Settings("ModWidth") <> "" Then
                ModWidth = Settings("ModWidth")
            End If

            
            DataList1.RepeatColumns = Columns
            DataList1.ItemStyle.Width = CInt(ModWidth / Columns)

            

            DataList1.DataBind()



            AddButton.Visible = IsEditable
            EditBtn.Visible = IsEditable

            'Response.Write(ModuleId)


        End Sub


        Public Sub FixSortOrder()
            Dim d As New AgapeDocumentsDataContext
            Dim q = From c In d.Agape_Main_AgapeDocuments Where c.ModuleId = ModuleId And c.PortalId = PortalId Order By c.SortOrder

            Dim i As Integer = 0
            For Each row In q

                row.SortOrder = i
                i = i + 1
            Next
            d.SubmitChanges()

        End Sub

        Public Function IsNewWindow(ByVal FileId As Integer, ByVal LinkType As String) As String
            If LinkType = "F" Then
                Dim ImageFileId As Integer = Integer.Parse(FileId)
                Dim objFileController As New DotNetNuke.Services.FileSystem.FileController
                Dim objImageInfo As DotNetNuke.Services.FileSystem.FileInfo = objFileController.GetFileById(ImageFileId, PortalId)
                If objImageInfo.Extension = "pdf" Or objImageInfo.Extension = "ppt" Then
                    Return "_self"
                End If


            End If
            Return "_blank"
        End Function

        Public Function GetDocument(ByVal DocId As Integer, ByVal FileId As Integer, ByVal URL As String, ByVal LinkType As String) As String
            If LinkType = "F" Then
                Dim ImageFileId As Integer = Integer.Parse(FileId)
                Dim objFileController As New DotNetNuke.Services.FileSystem.FileController
                Dim objImageInfo As DotNetNuke.Services.FileSystem.FileInfo = objFileController.GetFileById(ImageFileId, PortalId)
                If objImageInfo.Extension = "pdf" Or objImageInfo.Extension = "ppt" Then
                    Return EditUrl("PDF") & "?DocId=" & DocId
                End If

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
                        Return "images/GIF.png"
                    Case "bmp"
                        Return "images/BMP.png"
                    Case "doc"
                        Return "images/DOC.png"
                    Case "jpg"
                        Return "images/JPG.png"
                    Case "mov"
                        Return "images/MOV.png"
                    Case "mp3"
                        Return "images/MP3.png"
                    Case "mp4"
                        Return "images/MP4.png"
                    Case "mpg"
                        Return "images/MPG.png"
                    Case "pdf"
                        Return "images/PDF.png"

                    Case "png"
                        Return "images/PNG.png"
                    Case "psd"
                        Return "images/PSD.png"
                    Case "tiff"
                        Return "images/TIFF.png"
                    Case "txt"
                        Return "images/TXT.png"
                    Case "wav"
                        Return "images/WAV.png"
                    Case "zip"
                        Return "images/ZIP.png"


                    Case Else
                        Return "images/Blank.png"

                End Select


            Else
                Return "images/Blank.png"
            End If
        End Function

        Protected Sub AddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddButton.Click
            Response.Redirect(EditUrl("Add"))

        End Sub

        Protected Sub EditBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditBtn.Click
            Response.Redirect(EditUrl("Layout"))
        End Sub
    End Class
End Namespace
