Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker



Namespace DotNetNuke.Modules.StaffAdmin
    Partial Class stafftypes
        Inherits Entities.Modules.PortalModuleBase

        Public Function CanDelete(ByVal StaffTypeIdIn As Integer) As Boolean
            Dim d As New StaffBrokerDataContext
            Dim q = From c In d.AP_StaffBroker_StaffTypes Where c.StaffTypeId = StaffTypeIdIn Select c.AP_StaffBroker_Staffs.Count

            If q.Count = 1 Then
                If q.First = 0 Then
                    Return True
                End If
            End If

            Return False
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            hfPortalId.Value = PortalId
        End Sub

        Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
            If e.CommandName = "myinsert" Then
                Dim d As New StaffBrokerDataContext
                Dim insert = New AP_StaffBroker_StaffType()
                insert.Name = CType(GridView1.FooterRow.Controls(0).FindControl("NameI"), TextBox).Text
                insert.PortalId = PortalId
                d.AP_StaffBroker_StaffTypes.InsertOnSubmit(insert)
                d.SubmitChanges()
                GridView1.DataBind()
            ElseIf e.CommandName = "emptyinsert" Then
                Dim d As New StaffBrokerDataContext
                Dim insert = New AP_StaffBroker_StaffType()
                Dim tb As TextBox = GridView1.Controls(0).Controls(1).FindControl("NameE")
                insert.Name = tb.Text
                insert.PortalId = PortalId
                d.AP_StaffBroker_StaffTypes.InsertOnSubmit(insert)
                d.SubmitChanges()
                GridView1.DataBind()
            End If
        End Sub
    End Class
End Namespace
