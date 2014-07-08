Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker



Namespace DotNetNuke.Modules.StaffAdmin
    Partial Class StaffProfileDefinition
        Inherits Entities.Modules.PortalModuleBase

       

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                hfPortalId.Value = PortalId
            End If

        End Sub

        Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
            If e.CommandName = "myinsert" Then
                Dim d As New StaffBrokerDataContext
                Dim insert = New AP_StaffBroker_StaffPropertyDefinition
                insert.PropertyName = CType(GridView1.FooterRow.Controls(0).FindControl("tbPropertyName"), TextBox).Text
                insert.PropertyHelp = CType(GridView1.FooterRow.Controls(0).FindControl("tbPropertyHelp"), TextBox).Text
                insert.Display = CType(GridView1.FooterRow.Controls(0).FindControl("cbDisplay"), CheckBox).Checked
                insert.vieworder = (From c In d.AP_StaffBroker_StaffPropertyDefinitions Where c.PortalId = PortalId Select c.ViewOrder).Max() + 1
                insert.Type = CType(GridView1.FooterRow.Controls(0).FindControl("ddlType"), DropDownList).SelectedValue
                insert.PortalId = PortalId
                d.AP_StaffBroker_StaffPropertyDefinitions.InsertOnSubmit(insert)
                d.SubmitChanges()
                GridView1.DataBind()
            ElseIf e.CommandName = "emptyinsert" Then
                Dim d As New StaffBrokerDataContext
                Dim insert = New AP_StaffBroker_StaffPropertyDefinition
                insert.PropertyName = CType(GridView1.Controls(0).Controls(1).FindControl("tbPropertyNameE"), TextBox).Text
                insert.PropertyHelp = CType(GridView1.Controls(0).Controls(1).FindControl("tbPropertyHelpE"), TextBox).Text
                insert.Display = CType(GridView1.Controls(0).Controls(1).FindControl("cbDisplayE"), TextBox).Text
                insert.ViewOrder = 0
                insert.Type = CType(GridView1.Controls(0).Controls(1).FindControl("ddlTypeE"), DropDownList).SelectedValue
                insert.PortalId = PortalId
                d.AP_StaffBroker_StaffPropertyDefinitions.InsertOnSubmit(insert)
                d.SubmitChanges()
                GridView1.DataBind()
            End If
        End Sub
    End Class
End Namespace
