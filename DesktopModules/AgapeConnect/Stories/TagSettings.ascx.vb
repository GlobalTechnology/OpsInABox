Imports DotNetNuke
Imports System.Web.UI
Imports System.Linq
Imports System.ServiceModel.Syndication
Imports System.Xml
Imports System.Net
Imports Stories
Imports DotNetNuke.Framework.JavaScriptLibraries
Imports DotNetNuke.UI.Utilities

Imports DotNetNuke.Services.FileSystem


Namespace DotNetNuke.Modules.Stories

    Partial Class TagSettings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Constants"
        'Command names for actions event handlers
        Private Const DELETE_TAG_COMMAND_NAME As String = "DeleteTag"

        'Setting names
        Private Const TAG_ASPECT_SETTING As String = "TagAspect"

#End Region 'Constants

#Region "Page properties"

        'TagAspect retrieved in module settings. This is the width/length ratio for the tag photos.
        Protected ReadOnly Property TagAspect() As String
            Get
                Dim tagPhotoAspect = "1.3"
                If CType(TabModuleSettings(TAG_ASPECT_SETTING), String) <> "" Then
                    tagPhotoAspect = TabModuleSettings(TAG_ASPECT_SETTING)
                End If
                Return tagPhotoAspect
            End Get
        End Property

#End Region 'Page properties

#Region "Helper Functions"

        Protected Sub BuildTagList()
            gvTags.DataSource = StoryFunctions.GetTags(TabModuleId)
            gvTags.DataBind()
        End Sub

        Protected Sub TranslateGridViewWords()
            Dim cfedit As CommandField = DirectCast(gvTags.Columns(6), CommandField)
            cfedit.EditText = Translate("Edit")
            cfedit.CancelText = Translate("Cancel")
            cfedit.UpdateText = Translate("Update")
            gvTags.Columns(0).HeaderText = Translate("Image")
            gvTags.Columns(1).HeaderText = Translate("TagName")
            gvTags.Columns(2).HeaderText = Translate("Keywords")
            gvTags.Columns(3).HeaderText = Translate("Master")
            gvTags.Columns(4).HeaderText = Translate("LinkImage")
            gvTags.Columns(5).HeaderText = Translate("OpenStyle")
        End Sub

        Public Function Translate(ByVal ResourceString As String) As String
            Return DotNetNuke.Services.Localization.Localization.GetString(ResourceString & ".Text", LocalResourceFile)
        End Function

#End Region 'Helper Functions

#Region "Page Events"

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            ' Register DNN Jquery plugins
            ClientAPI.RegisterClientReference(Me.Page, ClientAPI.ClientNamespaceReferences.dnn)
            JavaScript.RequestRegistration(CommonJs.DnnPlugins)
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

            If (Not IsEditable And Not UserInfo.IsInRole("Administrators")) Then
                Response.Redirect(NavigateURL(PortalSettings.Current.ErrorPage404))
            End If

            If Not Page.IsPostBack Then
                TranslateGridViewWords()
                BuildTagList()
            End If
        End Sub

        Protected Sub CancelBtn_Click(sender As Object, e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Protected Sub btnAddTag_Click(sender As Object, e As System.EventArgs) Handles btnAddTag.Click
            StoryFunctions.SetTag(tbAddTag.Text, TabModuleId)
            BuildTagList()
            tbAddTag.Text = ""
            tbAddTag.Focus()
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

            Dim linkImage = TryCast(gvTags.Rows(e.RowIndex).FindControl("ddlLinkImage"), DropDownList).SelectedItem.Value
            Dim openStyle = TryCast(gvTags.Rows(e.RowIndex).FindControl("ddlOpenStyle"), DropDownList).SelectedItem.Value

            StoryFunctions.UpdateTag(name, keywords, master, linkImage, openStyle, tagIdToUpdate, TabModuleId)

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

                    'set image aspect
                    image.Aspect = TagAspect

                    'get image if one has already been uploaded 
                    If (tagPhotoId IsNot Nothing) Then
                        image.FileId = tagPhotoId
                    End If

                End If
            End If
        End Sub

        Protected Sub gvTags_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvTags.RowCommand
            If e.CommandName = DELETE_TAG_COMMAND_NAME Then
                StoryFunctions.DeleteTag(e.CommandArgument, TabModuleId)
                BuildTagList()
            End If
        End Sub

        Protected Sub ImagePicker_ImageUpdated(image As DesktopModules_AgapePortal_StaffBroker_acImage)
            If image.CheckAspect() Then
                StoryFunctions.SetTagPhotoId(image.FileId, gvTags.DataKeys(gvTags.EditIndex).Value)
            End If
        End Sub

        Protected Sub gvTags_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvTags.RowDataBound
            If (e.Row.RowType = DataControlRowType.DataRow And e.Row.RowState = DataControlRowState.Edit) Then
                'Populate ddlLinkImage in the Row
                Dim ddlLinkImage As DropDownList = CType(e.Row.FindControl("ddlLinkImage"), DropDownList)
                Dim linkImageItems As Array = System.Enum.GetNames(GetType(TagSettingsConstants.LinkImage))

                For Each linkImageItem In linkImageItems
                    Dim item As ListItem = New ListItem(Translate(linkImageItem), linkImageItem)
                    ddlLinkImage.Items.Add(item)
                Next
                ddlLinkImage.DataBind()

                'Populate ddlOpenStyle in the Row
                Dim ddlOpenStyle As DropDownList = CType(e.Row.FindControl("ddlOpenStyle"), DropDownList)
                Dim openStyleItems As Array = System.Enum.GetNames(GetType(TagSettingsConstants.OpenStyle))

                For Each openStyleItem In openStyleItems
                    Dim item As ListItem = New ListItem(Translate(openStyleItem), openStyleItem)
                    ddlOpenStyle.Items.Add(item)
                Next
                ddlOpenStyle.DataBind()

                'Use hidden label to remember the selected row in each ddl
                ddlLinkImage.Items.FindByValue(TryCast(e.Row.FindControl("lblLinkImage"), Label).Text).Selected = True
                ddlOpenStyle.Items.FindByValue(TryCast(e.Row.FindControl("lblOpenStyle"), Label).Text).Selected = True

            End If
        End Sub

#End Region 'Page Events

    End Class

End Namespace

