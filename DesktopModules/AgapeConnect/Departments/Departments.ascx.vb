Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq

Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker
Imports System.IO
Imports System.Data.OleDb



Namespace DotNetNuke.Modules.StaffAdmin
    Partial Class Departments
        Inherits Entities.Modules.PortalModuleBase


        ' Private newDeptsId As Integer = Nothing
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not (Page.IsPostBack) Then
                hfPortalId.Value = PortalId
                If Request.QueryString("DeptId") <> "" Then
                    lbDepartments.SelectedValue = CInt(Request.QueryString("DeptId"))
                End If



            End If
        End Sub

        Protected Sub FormView1_DataBound(sender As Object, e As System.EventArgs) Handles FormView1.DataBound
           
        End Sub

        Protected Sub FormView1_ItemInserted(sender As Object, e As System.Web.UI.WebControls.FormViewInsertedEventArgs) Handles FormView1.ItemInserted
            lbDepartments.DataBind()
            StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", PortalId)
        End Sub

        Protected Sub FormView1_ItemInserting(sender As Object, e As System.Web.UI.WebControls.FormViewInsertEventArgs) Handles FormView1.ItemInserting

            Dim acx = CType(FormView1.FindControl("acImage1"), DesktopModules_AgapePortal_StaffBroker_acImage)
            Dim lblError = CType(FormView1.FindControl("lblError"), Label)
            e.Values("Spare1") = CType(FormView1.FindControl("CanRecieveNonDonaitonIncome"), CheckBox).Checked.ToString


            If acx.FileId <> 0 Then


                If acx.CheckAspect() = False Then

                    lblError.Visible = True
                    e.Cancel = True

                    Return
                Else
                    lblError.Visible = False
                End If
            Else
                e.Values("PhotoId") = Nothing
            End If

            If e.Values("CostCentreDelegate") = "" Then
                e.Values("CostCentreDelegate") = Nothing
            End If
            If e.Values("CostCentreManager") = "" Then
                e.Values("CostCentreManager") = Nothing
            End If
            e.Values("PortalId") = PortalId

        End Sub




        Protected Sub FormView1_ModeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FormView1.ModeChanged
            If FormView1.CurrentMode = FormViewMode.Insert Then
                lbDepartments.ClearSelection()
                'lbDepartments.SelectedIndex = -1
            End If
        End Sub

        Protected Sub lbDepartments_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDepartments.SelectedIndexChanged
            FormView1.ChangeMode(FormViewMode.Edit)
        End Sub

        Public Function getStaffAdd(ByVal CurrentUserId As Integer) As List(Of StaffBroker.User)
            Dim staff = StaffBrokerFunctions.GetStaff().ToList
            If (From c In staff Where c.UserID = CurrentUserId).Count = 0 Then
                Dim insert As New StaffBroker.User
                insert.UserID = -3
                insert.DisplayName = "Not Found!"
                staff.Add(insert)

            End If
            Return staff
        End Function

        Protected Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click
            FormView1.ChangeMode(FormViewMode.Insert)

        End Sub

        Protected Sub FormView1_ItemUpdating(sender As Object, e As System.Web.UI.WebControls.FormViewUpdateEventArgs) Handles FormView1.ItemUpdating
            Dim acx = CType(FormView1.FindControl("acImage1"), DesktopModules_AgapePortal_StaffBroker_acImage)
            Dim lblError = CType(FormView1.FindControl("lblError"), Label)
            If acx.FileId <> 0 Then


                If acx.CheckAspect() = False Then

                    lblError.Visible = True
                    e.Cancel = True

                    Return
                Else
                    lblError.Visible = False
                End If
            Else
                e.NewValues("PhotoId") = Nothing
            End If
            e.NewValues("Spare1") = CType(FormView1.FindControl("CanRecieveNonDonaitonIncome"), CheckBox).Checked.ToString

            'If e.NewValues("CostCentre") <> e.OldValues("CostCentre") Or _
            '   e.NewValues("CostCentreManager") <> e.OldValues("CostCentreManager") Or _
            '   e.NewValues("CostCentreDelegate") <> e.OldValues("CostCentreDelegate") Then
            '    StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", PortalId)
            'End If
            StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", PortalId)
            If e.NewValues("CostCentreDelegate") = "" Then
                e.NewValues("CostCentreDelegate") = Nothing
            End If
            If e.NewValues("CostCentreManager") = "" Then
                e.NewValues("CostCentreManager") = Nothing
            End If
            Dim d As New StaffBrokerDataContext
            Dim q = From c In d.AP_StaffBroker_Departments Where c.CostCenterId = lbDepartments.SelectedValue
            If q.Count > 0 Then
                If StaffBrokerFunctions.GetSetting("NonDynamics", PortalId) = "True" Then

                    q.First.CostCentre = CType(FormView1.FindControl("tbCostCentreCode"), TextBox).Text
                    d.SubmitChanges()

                Else




                    q.First.CostCentre = CType(FormView1.FindControl("ddlCostCentre"), DropDownList).SelectedValue

                    d.SubmitChanges()

                End If
            End If
            Try
                Dim x As New tntWebUsers()
                If Not String.IsNullOrEmpty(e.NewValues("CostCentreManager")) Then
                    x.RefreshWebUsers({CInt(e.NewValues("CostCentreManager"))})
                End If
                If Not String.IsNullOrEmpty(e.NewValues("CostCentreDelegate")) Then
                    x.RefreshWebUsers({CInt(e.NewValues("CostCentreDelegate"))})
                End If
                If Not String.IsNullOrEmpty(e.OldValues("CostCentreManager")) Then
                    x.RefreshWebUsers({CInt(e.OldValues("CostCentreManager"))})
                End If
                If Not String.IsNullOrEmpty(e.OldValues("CostCentreDelegate")) Then
                    x.RefreshWebUsers({CInt(e.OldValues("CostCentreDelegate"))})
                End If

            Catch ex As Exception

            End Try
        End Sub





        Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
            'Dim aci = CType(FormView1..FindControl("acImage1"), DesktopModules_AgapePortal_StaffBroker_acImage)
            'aci.LazyLoad(False)
        End Sub

        Protected Sub LoadImage(sender As Object, e As System.EventArgs)
            Dim x = CType(sender, DesktopModules_AgapePortal_StaffBroker_acImage)
            If x.Aspect = "" Then
                x.Aspect = "1"
                x.Width = "200"

                x.LazyLoad(False, True)

            End If

        End Sub

        
        Protected Sub btnBulkUpload_Click(sender As Object, e As EventArgs) Handles btnBulkUpload.Click
            Response.Redirect(EditUrl("BulkUpload"))
        End Sub
    End Class
End Namespace
