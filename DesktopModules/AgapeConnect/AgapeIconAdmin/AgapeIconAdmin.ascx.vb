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
Imports DotNetNuke.Services.FileSystem


Namespace DotNetNuke.Modules.aAgapeIconAdmin
    Partial Class myAgapeIconAdmin
        Inherits Entities.Modules.PortalModuleBase
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            jQuery.RequestDnnPluginsRegistration()
        End Sub


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

            PortalIdHF.Value = PortalId

            If Not Page.IsPostBack Then

                Dim d As New AgapeIconAdmin.AgapeIconsDataContext
                Dim q = From c In d.Agape_Skin_IconSettings Where c.PortalId = PortalId
                If q.Count > 0 Then
                    HeightTB.Text = q.First.IconHeight
                    tbPadding.Text = q.First.Padding
                    cbTitle.Checked = q.First.ShowTitles

                Else
                    HeightTB.Text = 110
                    tbPadding.Text = 5
                    cbTitle.Checked = True
                End If



            End If

        End Sub


        Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
            If e.CommandName = "EInsert" Then
                Dim d As New AgapeIconAdmin.AgapeIconsDataContext
                Dim insert As New AgapeIconAdmin.Agape_Skin_AgapeIcon
                Dim url As DotNetNuke.UI.UserControls.UrlControl
                url = GridView1.Controls(0).Controls(0).FindControl("EIconChooser")
                insert.IconFile = CInt(url.Url.Substring(7))
                url = GridView1.Controls(0).Controls(0).FindControl("EHoverIconChooser")
                insert.HovrIconFile = CInt(url.Url.Substring(7))

                url = GridView1.Controls(0).Controls(0).FindControl("EUrlChooser")

                insert.Title = CType(GridView1.Controls(0).Controls(0).FindControl("tbTitleEInsert"), TextBox).Text

                insert.LinkType = url.UrlType
                insert.LinkLoc = url.Url
                insert.PortalId = PortalId
                insert.ViewOrder = 0


                d.Agape_Skin_AgapeIcons.InsertOnSubmit(insert)

                d.SubmitChanges()
                GridView1.DataBind()
            ElseIf e.CommandName = "Insert" Then
                Dim d As New AgapeIconAdmin.AgapeIconsDataContext
                Dim MaxViewOrder As Integer = (From c In d.Agape_Skin_AgapeIcons Where c.PortalId = PortalId Select c.ViewOrder).Max



                Dim insert As New AgapeIconAdmin.Agape_Skin_AgapeIcon
                Dim url As DotNetNuke.UI.UserControls.UrlControl
                url = GridView1.FooterRow.Controls(0).FindControl("IIconChooser")
                insert.IconFile = CInt(url.Url.Substring(7))
                url = GridView1.FooterRow.Controls(0).FindControl("IHoverIconChooser")
                insert.HovrIconFile = CInt(url.Url.Substring(7))
                url = GridView1.FooterRow.Controls(0).FindControl("IUrlChooser")
                insert.LinkType = url.UrlType
                insert.LinkLoc = url.Url
                insert.Title = CType(GridView1.FooterRow.Controls(0).FindControl("tbTitleInsert"), TextBox).Text

                insert.PortalId = PortalId
                insert.ViewOrder = MaxViewOrder + 1

                d.Agape_Skin_AgapeIcons.InsertOnSubmit(insert)

                d.SubmitChanges()
                GridView1.DataBind()
            ElseIf e.CommandName = "Promote" Then
                Dim d As New AgapeIconAdmin.AgapeIconsDataContext
                Dim thisDoc = (From c In d.Agape_Skin_AgapeIcons Where c.AgapeIconid = CInt(e.CommandArgument)).First

                If thisDoc.ViewOrder <> 0 Then
                    thisDoc.ViewOrder = thisDoc.ViewOrder - 1
                    Dim nextDoc = (From c In d.Agape_Skin_AgapeIcons Where c.PortalId = PortalId And c.ViewOrder = thisDoc.ViewOrder).First
                    nextDoc.ViewOrder = nextDoc.ViewOrder + 1

                End If

                d.SubmitChanges()
                GridView1.DataBind()


            ElseIf e.CommandName = "Demote" Then
                Dim d As New AgapeIconAdmin.AgapeIconsDataContext
                Dim thisDoc = (From c In d.Agape_Skin_AgapeIcons Where c.AgapeIconid = CInt(e.CommandArgument)).First


                thisDoc.ViewOrder = thisDoc.ViewOrder + 1
                Dim nextDoc = (From c In d.Agape_Skin_AgapeIcons Where c.PortalId = PortalId And c.ViewOrder = thisDoc.ViewOrder)

                If nextDoc.Count > 0 Then
                    nextDoc.First.ViewOrder = nextDoc.First.ViewOrder - 1
                    d.SubmitChanges()
                    GridView1.DataBind()

                End If
            End If
        End Sub

        Public Function GetUrl(ByVal FileType As String, ByVal Url As String) As String
            If FileType = "T" Then
                Return NavigateURL(CInt(Url))
            Else
                Return Url
            End If
        End Function

        Public Function GetUrlFromFileId(ByVal FileId As Integer) As String
            Dim ImageFileId As Integer = Integer.Parse(FileId)
            Dim objFileController As New DotNetNuke.Services.FileSystem.FileController
            Dim objImageInfo As DotNetNuke.Services.FileSystem.FileInfo = objFileController.GetFileById(ImageFileId, PortalId)
            Return PortalSettings.HomeDirectory & objImageInfo.Folder & objImageInfo.FileName

        End Function
        Public Function WordWrap(ByVal input As String, ByVal length As Integer) As String
            Dim cursor As Integer = length
            While cursor < input.Length
                input = input.Insert(cursor, "<br/>")
                cursor += length + 5
            End While
            Return input

        End Function

        Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
            Dim d As New AgapeIconAdmin.AgapeIconsDataContext
            Dim q = From c In d.Agape_Skin_AgapeIcons Where c.PortalId = PortalId Order By c.ViewOrder

            Dim i As Integer = 0
            For Each row In q

                row.ViewOrder = i
                i += 1
            Next
            d.SubmitChanges()
        End Sub

        Protected Sub UpdateBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateBtn.Click
            Dim d As New AgapeIconAdmin.AgapeIconsDataContext
            Dim q = From c In d.Agape_Skin_IconSettings Where c.PortalId = PortalId
            If q.Count > 0 Then
                q.First.IconHeight = CInt(HeightTB.Text)
                q.First.Padding = CInt(tbPadding.Text)
                q.First.ShowTitles = cbTitle.Checked
            Else
                Dim insert As New AgapeIconAdmin.Agape_Skin_IconSetting
                insert.PortalId = PortalId
                insert.IconHeight = CInt(HeightTB.Text)
                insert.Padding = CInt(tbPadding.Text)
                insert.ShowTitles = cbTitle.Checked
                d.Agape_Skin_IconSettings.InsertOnSubmit(insert)
            End If
            d.SubmitChanges()
        End Sub


        Protected Sub btnQuickAdd_Click(sender As Object, e As EventArgs) Handles btnQuickAdd.Click


            Dim d As New AgapeIconAdmin.AgapeIconsDataContext
            Dim newViewOrder = (From c In d.Agape_Skin_AgapeIcons Where c.PortalId = PortalId).Count

            Dim folder As IFolderInfo

            If Not FolderManager.Instance.FolderExists(PortalId, "acIcons") Then
                folder = FolderManager.Instance.AddFolder(PortalId, "acIcons")
            Else
                folder = FolderManager.Instance.GetFolder(PortalId, "acIcons")
            End If

            Dim insert As New AgapeIconAdmin.Agape_Skin_AgapeIcon
            Dim image1 As String
            Dim image2 As String


            Dim mc As New DotNetNuke.Entities.Modules.ModuleController
            Dim x = mc.GetModuleByDefinition(PortalId, ddlInsertType.SelectedValue)
            If Not x Is Nothing Then ' The Module exists
                insert.LinkLoc = x.TabID
            End If
            Select Case ddlInsertType.SelectedValue
                Case "acStaffRmb"
                    image1 = "Rmb.png"
                    image2 = "Rmb-2.png"

                Case "acAccounts"
                    image1 = "ViewAccount.png"
                    image2 = "ViewAccount2.png"

                Case "acMpdCalc"
                    image1 = "Budgets-light.png"
                    image2 = "Budgets.png"
            End Select

            'Dim File1 = New System.IO.FileInfo(Server.MapPath("/DesktopModules/AgapeConnect/AgapeIconAdmin/images/" & image1))
            'Dim File2 = New System.IO.FileInfo(Server.MapPath("/DesktopModules/AgapeConnect/AgapeIconAdmin/images/" & image2))
            Dim theFile1 As FileInfo
            Dim theFile2 As FileInfo
            Using FileStream As New System.IO.FileStream(Server.MapPath("/DesktopModules/AgapeConnect/AgapeIconAdmin/images/" & image1), System.IO.FileMode.Open)
                theFile1 = FileManager.Instance.AddFile(folder, image1, FileStream, False, False, "image/png")
            End Using
            Using FileStream As New System.IO.FileStream(Server.MapPath("/DesktopModules/AgapeConnect/AgapeIconAdmin/images/" & image2), System.IO.FileMode.Open)
                theFile2 = FileManager.Instance.AddFile(folder, image2, FileStream, False, False, "image/png")
            End Using

          
            insert.LinkType = "T"
            insert.Title = ddlInsertType.SelectedItem.Text
            insert.ViewOrder = newViewOrder

            insert.PortalId = PortalId
            If Not theFile1 Is Nothing And Not theFile2 Is Nothing And Not insert.LinkLoc Is Nothing Then
                insert.IconFile = theFile1.FileId
                insert.HovrIconFile = theFile2.FileId

                d.Agape_Skin_AgapeIcons.InsertOnSubmit(insert)
                d.SubmitChanges()

                GridView1.DataBind()
            End If

            

        End Sub
    End Class

End Namespace
