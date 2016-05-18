Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq
Imports System.ServiceModel.Syndication
Imports System.Xml
Imports System.Net
Imports Stories

Imports DotNetNuke.Services.FileSystem


Namespace DotNetNuke.Modules.Stories

    Partial Class TagSettings
        Inherits Entities.Modules.ModuleSettingsBase

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then
                BuildTagList()
            End If
        End Sub

#Region "Helper Functions"

        Protected Sub BuildTagList()
            gvTags.Columns(0).HeaderText = Translate("Image")
            gvTags.Columns(1).HeaderText = Translate("TagName")
            gvTags.Columns(2).HeaderText = Translate("Keywords")
            gvTags.Columns(3).HeaderText = Translate("Master")
            gvTags.DataSource = StoryFunctions.GetTags(TabModuleId)
            gvTags.DataBind()
        End Sub

        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
        End Function

#End Region 'Helper Functions

#Region "Page Events"

        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub btnAddTag_Click(sender As Object, e As System.EventArgs) Handles btnAddTag.Click
            StoryFunctions.SetTag(tbAddTag.Text, TabModuleId)
            BuildTagList()
            tbAddTag.Text = ""
            tbAddTag.Focus()
        End Sub

        Protected Sub gvTags_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvTags.RowDeleting
            StoryFunctions.DeleteTag(e.Keys(0), TabModuleId)
            BuildTagList()
        End Sub

        Protected Sub gvTags_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvTags.RowEditing
            'save the row that is being edited
            gvTags.EditIndex = e.NewEditIndex
            BuildTagList()
        End Sub

        Protected Sub gvTags_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvTags.RowCancelingEdit
            'Reset the edit index
            gvTags.EditIndex = -1
            BuildTagList()
        End Sub

        Protected Sub gvTags_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvTags.RowUpdating
            Dim tagIdToUpdate = gvTags.DataKeys(gvTags.EditIndex).Value
            Dim name As String = ""
            Dim keywords As String = ""

            If (e.NewValues.Values(0) IsNot Nothing) Then
                name = e.NewValues(0).ToString
            End If

            If (e.NewValues.Values(1) IsNot Nothing) Then
                keywords = e.NewValues(1).ToString
            End If

            Dim master As Boolean = e.NewValues(2)

            StoryFunctions.UpdateTag(name, keywords, master, tagIdToUpdate, TabModuleId)

            'Reset the edit index
            gvTags.EditIndex = -1
            BuildTagList()
        End Sub

        Protected Sub gvTags_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvTags.RowCreated

            ' bind only rows that contain data (not header or footer rows...)
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim tagPhotoId As Nullable(Of Integer) = StoryFunctions.GetTag(gvTags.DataKeys(e.Row.RowIndex).Value, TabModuleId).PhotoId

                If (e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Alternate) Then
                    'get thumbnail of image
                    Dim thumbnail As WebControls.Image = CType(e.Row.FindControl("TagThumbnail"), WebControls.Image)
                    thumbnail.ImageUrl = StoryFunctions.GetPhotoURL(tagPhotoId)

                ElseIf ((e.Row.RowState And DataControlRowState.Edit) > 0) Then
                    'get reference to the image
                    Dim image As DesktopModules_AgapePortal_StaffBroker_acImage =
                        DirectCast(e.Row.FindControl("ImagePicker"), DesktopModules_AgapePortal_StaffBroker_acImage)

                    'add event handler for updated event raised in acImage
                    AddHandler image.UpdatedWithImage, AddressOf ImagePicker_ImageUpdated

                    'get image if one has already been uploaded 
                    If (tagPhotoId IsNot Nothing) Then
                        image.FileId = tagPhotoId
                    End If

                End If
            End If
        End Sub

        Protected Sub ImagePicker_ImageUpdated(image As DesktopModules_AgapePortal_StaffBroker_acImage)
            If image.CheckAspect() Then
                StoryFunctions.SetTagPhotoId(image.FileId, gvTags.DataKeys(gvTags.EditIndex).Value)
            End If
        End Sub

#End Region 'Page Events

    End Class

End Namespace

