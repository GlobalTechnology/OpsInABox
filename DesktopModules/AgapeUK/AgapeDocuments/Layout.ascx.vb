Imports DotNetNuke
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Math
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Collections.Specialized

Imports System.Linq
Imports UK.AgapeDocuments




Namespace DotNetNuke.Modules.AgapeDocuments

    Partial Class Layout
        Inherits Entities.Modules.ModuleSettingsBase
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
            ModuleIdHF.Value = ModuleId
            PortalIdHF.Value = PortalId
            If (Page.IsPostBack = False) Then
                If CType(Settings("Columns"), String) <> "" Then
                    Columns.SelectedValue = CType(Settings("Columns"), Integer)
                Else
                    Columns.SelectedValue = 3
                End If

                If CType(Settings("ModWidth"), String) <> "" Then
                    WidthTB.Text = CType(Settings("ModWidth"), Integer)
                Else
                    WidthTB.Text = "1000"
                End If
            End If

        End Sub



        Protected Sub Return_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReturnBtn.Click
            Dim objModules As New Entities.Modules.ModuleController




            objModules.UpdateTabModuleSetting(TabModuleId, "Columns", CInt(Columns.SelectedValue))
            objModules.UpdateTabModuleSetting(TabModuleId, "ModWidth", CInt(WidthTB.Text))

            ' refresh cache
            SynchronizeModule()


            Response.Redirect(NavigateURL)
        End Sub

        Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
            Dim d As New AgapeDocumentsDataContext


            If e.CommandName = "Promote" Then
                
                Dim thisDoc = (From c In d.Agape_Main_AgapeDocuments Where c.AgapeDocumentId = CInt(e.CommandArgument)).First

                If thisDoc.SortOrder <> 0 Then
                    thisDoc.SortOrder = thisDoc.SortOrder - 1
                    Dim nextDoc = (From c In d.Agape_Main_AgapeDocuments Where c.ModuleId = ModuleId And c.PortalId = PortalId And c.SortOrder = thisDoc.SortOrder).First
                    nextDoc.SortOrder = nextDoc.SortOrder + 1

                End If

                d.SubmitChanges()
                GridView1.DataBind()


            ElseIf e.CommandName = "Demote" Then
                
                Dim thisDoc = (From c In d.Agape_Main_AgapeDocuments Where c.AgapeDocumentId = CInt(e.CommandArgument)).First


                thisDoc.SortOrder = thisDoc.SortOrder + 1
                Dim nextDoc = (From c In d.Agape_Main_AgapeDocuments Where c.ModuleId = ModuleId And c.PortalId = PortalId And c.SortOrder = thisDoc.SortOrder)

                If nextDoc.count > 0 Then
                    nextDoc.First.SortOrder = nextDoc.First.SortOrder - 1
                    d.SubmitChanges()
                    GridView1.DataBind()

                End If





               

            End If
        End Sub

        Protected Sub CancelBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelBtn.Click
            Response.Redirect(NavigateURL)
        End Sub

        Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
            Dim d As New AgapeDocumentsDataContext
            Dim q = From c In d.Agape_Main_AgapeDocuments Where c.ModuleId = ModuleId And c.PortalId = PortalId Order By c.SortOrder

            Dim i As Integer = 0
            For Each row In q

                row.SortOrder = i
                i = i + 1
            Next
            d.SubmitChanges()



        End Sub

    End Class
End Namespace
