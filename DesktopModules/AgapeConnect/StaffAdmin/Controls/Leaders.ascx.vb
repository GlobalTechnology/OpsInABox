Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker

Partial Class DesktopModules_AgapePortal_StaffBroker_Leaders
    'Inherits System.Web.UI.UserControl
    Inherits Entities.Modules.PortalModuleBase

    Private _UID As Integer
    Public Property UID() As Integer
        Get
            Return _UID
        End Get
        Set(ByVal value As Integer)
            _UID = value

            hfUserId.Value = _UID
        End Set
    End Property
    Private _isReadOnly As Boolean = False
    Public Property isReadOnly() As Boolean
        Get
            Return _isReadOnly
        End Get
        Set(ByVal value As Boolean)
            _isReadOnly = value

        End Set
    End Property

    Dim d As New StaffBrokerDataContext

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Not Page.IsPostBack Then
            hfUserId.Value = _UID

        End If

    End Sub



    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim q = From c In d.AP_StaffBroker_LeaderMetas Where c.UserId = CInt(hfUserId.Value) And c.LeaderId = ddladd.SelectedValue
        If q.Count = 0 Then
            Dim insert As New StaffBroker.AP_StaffBroker_LeaderMeta
            insert.UserId = hfUserId.Value
            insert.LeaderId = ddladd.SelectedValue
            d.AP_StaffBroker_LeaderMetas.InsertOnSubmit(insert)
            d.SubmitChanges()

            DataList2.DataBind()
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", PS.PortalId)

        End If
    End Sub

    Protected Sub DataList2_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles DataList2.ItemCommand
        If e.CommandName = "DeleteLeader" Then
            Dim q = From c In d.AP_StaffBroker_LeaderMetas Where c.UserId = CInt(hfUserId.Value) And c.LeaderId = CInt(e.CommandArgument)

            d.AP_StaffBroker_LeaderMetas.DeleteAllOnSubmit(q)
            d.SubmitChanges()
            DataList2.DataBind()
            If DataList2.Items.Count = 0 Then
                lblEmpty.Visible = True
            End If

        ElseIf e.CommandName = "DeleteDelegate" Then
            Dim q = From c In d.AP_StaffBroker_LeaderMetas Where c.UserId = CInt(hfUserId.Value) And c.LeaderId = CInt(e.CommandArgument)

            If q.Count > 0 Then
                q.First.DelegateId = Nothing
                d.SubmitChanges()
                DataList2.DataBind()
            End If
        ElseIf e.CommandName = "AddDelegate" Then

            Dim q = From c In d.AP_StaffBroker_LeaderMetas Where c.UserId = CInt(hfUserId.Value) And c.LeaderId = CInt(e.CommandArgument)
            Dim ddl As DropDownList = e.Item.FindControl("ddlDelegate")

            If q.Count > 0 And ddl.SelectedValue > 0 Then
                q.First.DelegateId = ddl.SelectedValue
                d.SubmitChanges()
                DataList2.DataBind()
            End If
        End If
        Try
            Dim x As New tntWebUsers()
            x.RefreshWebUsers({CInt(e.CommandArgument)})
        Catch ex As Exception

        End Try
        
    End Sub


    Protected Sub DataList2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataList2.ItemDataBound
        lblEmpty.Visible = False
    End Sub
End Class
