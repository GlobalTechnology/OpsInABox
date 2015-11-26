Imports Documents
Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports System.IO
Imports DotNetNuke.Services.FileSystem


Namespace DotNetNuke.Modules.AgapeConnect.Documents

    Partial Class DownloadDocument
        Inherits Entities.Modules.PortalModuleBase

#Region "Page properties"

        'DocId retrieved in request
        Protected ReadOnly Property DocId() As Nullable(Of Integer)
            Get
                If String.IsNullOrEmpty(Request.QueryString(DocumentsControllerConstants.DocIdParamKey)) Then
                    Return Nothing
                Else
                    Return CInt(HttpUtility.UrlDecode(Request.QueryString(DocumentsControllerConstants.DocIdParamKey)))
                End If
            End Get
        End Property

        'YouTube ID to be used in YouTube video URL
        Private _YouTubeID As String = ""
        Protected Property YouTubeID() As String
            Get
                Return _YouTubeID
            End Get
            Set(ByVal value As String)
                _YouTubeID = value
            End Set
        End Property

#End Region 'Page properties

#Region "Page events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' Translate error messages
            Dim GENERIC_ERROR_MSG As String = LocalizeString("GENERIC_ERROR_MSG")
            Dim NOT_AUTHORIZED_ERROR_MSG As String = LocalizeString("NOT_AUTHORIZED_ERROR_MSG")

            ' Display error message if no DocId was provided
            If Not DocId.HasValue Then

                'Display error message
                AddModuleMessage(Me, GENERIC_ERROR_MSG, ModuleMessageType.RedError)

                'Log error
                AgapeLogger.Warn(UserId, GENERIC_ERROR_MSG + " - No DocId in request")

                Return

            End If

            'Retrieve info for the doc to view
            Try
                Dim theDoc = DocumentsController.GetDocument(DocId)

                ' Check if user has permissions to view the ressource
                If Not DocumentsController.IsAuthorized(UserId, DocId) Then

                    'Display error message
                    AddModuleMessage(Me, NOT_AUTHORIZED_ERROR_MSG, ModuleMessageType.RedError)

                    'Log error
                    AgapeLogger.Warn(UserId, NOT_AUTHORIZED_ERROR_MSG + " - User not authorized for DocId '" + DocId + "'")

                    Return

                End If

                ' Only files are handled in this viewer => Display error message otherwise
                If Not theDoc.LinkType = DocumentConstants.LinkTypeFile Then

                    'Display error message
                    AddModuleMessage(Me, GENERIC_ERROR_MSG, ModuleMessageType.RedError)

                    'Log error
                    AgapeLogger.Warn(UserId, GENERIC_ERROR_MSG + " - Unhandled LinkType '" + theDoc.LinkType + "'")

                    Return

                End If

                'Get file info
                Dim theFile = FileManager.Instance.GetFile(theDoc.FileId)
                'TODO: Check that file exists
                DownloadFile(theFile.PhysicalPath, theFile.FileName, theFile.ContentType)

            Catch ex As Exception 'Unexisting DocId throws an exception when calling GetDocument

                'Display error message
                AddModuleMessage(Me, GENERIC_ERROR_MSG, ModuleMessageType.RedError)

                'Log error
                AgapeLogger.Warn(UserId, GENERIC_ERROR_MSG + " - Exception: " + ex.Message + " - " + ex.StackTrace)

                Return

            End Try

        End Sub

#End Region 'Page events


        Protected Sub DownloadFile(ByVal strPath As String, ByVal strFileName As String, ByVal strContentType As String)

            If (File.Exists(strPath + strFileName)) Then

                Response.ContentType = strContentType
                Response.AddHeader("content-disposition", "attachment; filename=" + strFileName)
                Dim sourceFile As FileStream = New FileStream(strPath + strFileName, FileMode.Open)
                Dim fileSize As Long
                fileSize = sourceFile.Length
                Dim getContent(CInt(fileSize)) As Byte
                sourceFile.Read(getContent, 0, CInt(sourceFile.Length))
                sourceFile.Close()

                Response.BinaryWrite(getContent)

            Else
                'TODO: display and log error
            End If
        End Sub

    End Class

End Namespace
