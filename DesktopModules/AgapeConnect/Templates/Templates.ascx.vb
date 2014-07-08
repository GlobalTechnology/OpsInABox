Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker



Namespace DotNetNuke.Modules.StaffAdmin
    Partial Class TemplateManager
        Inherits Entities.Modules.PortalModuleBase
        'Dim insertedTemplateId As Integer = -1


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'If Not Page.IsPostBack Then
            hfPortalID.Value = PortalId
            'End If
            If DropDownList1.SelectedIndex > 0 Then
                litPreview.Text = StaffBrokerFunctions.GetTemplate(DropDownList1.SelectedItem.Text, PortalId)

            End If
            


        End Sub

        Protected Sub FormView1_ItemCreated(sender As Object, e As System.EventArgs) Handles FormView1.ItemCreated
            If FormView1.CurrentMode = FormViewMode.Insert Then


                Dim defaultText = System.IO.File.ReadAllText(Server.MapPath("/DesktopModules/AgapeConnect/Templates/emailTemplate.htm"))
                CType(FormView1.Row.FindControl("tbInsertTemplate"), DotNetNuke.UI.UserControls.TextEditor).Text = defaultText



            End If
        End Sub


        Protected Sub FormView1_ItemInserting(sender As Object, e As System.Web.UI.WebControls.FormViewInsertEventArgs) Handles FormView1.ItemInserting
            e.Values("PortalId") = PortalId

            ' insertedTemplateId=e.Values.
        End Sub

        Protected Sub FormView1_ItemInserted(sender As Object, e As System.Web.UI.WebControls.FormViewInsertedEventArgs) Handles FormView1.ItemInserted
            ' DropDownList1.DataBind()
            'DropDownList1.SelectedValue = e.Values("TemplateId")
        End Sub

        Protected Sub dsTheTemplate_Inserted(sender As Object, e As System.Web.UI.WebControls.LinqDataSourceStatusEventArgs) Handles dsTheTemplate.Inserted
            DropDownList1.DataBind()
            DropDownList1.SelectedValue = CType(e.Result, StaffBroker.AP_StaffBroker_Template).TemplateId
        End Sub

        Protected Sub FormView1_ItemDeleted(sender As Object, e As System.Web.UI.WebControls.FormViewDeletedEventArgs) Handles FormView1.ItemDeleted
            DropDownList1.DataBind()
        End Sub
    End Class
End Namespace
