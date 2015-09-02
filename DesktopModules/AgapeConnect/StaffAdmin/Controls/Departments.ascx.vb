Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker

Partial Class DesktopModules_AgapePortal_StaffBroker_Depts
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


    Dim d As New StaffBrokerDataContext

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim FileName As String = System.IO.Path.GetFileNameWithoutExtension(Me.AppRelativeVirtualPath)
        If Not (Me.ID Is Nothing) Then
            'this will fix it when its placed as a ChildUserControl 
            Me.LocalResourceFile = Me.LocalResourceFile.Replace(Me.ID, FileName)
        Else
            ' this will fix it when its dynamically loaded using LoadControl method 
            Me.LocalResourceFile = Me.LocalResourceFile & FileName & ".ascx.resx"
            Dim Locale = System.Threading.Thread.CurrentThread.CurrentCulture.Name
            Dim AppLocRes As New System.IO.DirectoryInfo(Me.LocalResourceFile.Replace(FileName & ".ascx.resx", ""))
            If Locale = PortalSettings.CultureCode Then
                'look for portal varient
                If AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                    Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                End If
            Else

                If AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".Portal-" & PortalId & ".resx").Count > 0 Then
                    'lookFor a CulturePortalVarient
                    Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".Portal-" & PortalId & ".resx")
                ElseIf AppLocRes.GetFiles(FileName & ".ascx." & Locale & ".resx").Count > 0 Then
                    'look for a CultureVarient
                    Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", Locale & ".resx")
                ElseIf AppLocRes.GetFiles(FileName & ".ascx.Portal-" & PortalId & ".resx").Count > 0 Then
                    'lookFor a PortalVarient
                    Me.LocalResourceFile = Me.LocalResourceFile.Replace("resx", "Portal-" & PortalId & ".resx")
                End If
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Not Page.IsPostBack Then
            hfUserId.Value = _UID
        End If

        ddlDelegate.Items.FindByValue(0).Text = LocalizeString("liNotSet")

    End Sub


    Public Function getDepartments(ByVal UserId As Integer) As IQueryable(Of AP_StaffBroker_Department)
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim depts = From c In d.AP_StaffBroker_Departments Where c.PortalId = PS.PortalId And (c.CostCentreManager = UserId Or c.CostCentreDelegate = UserId)


        Return depts


    End Function
    
   
    Protected Sub DataList2_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles DataList2.ItemCommand
        If e.CommandName = "GotoDept" Then
            '  Dim DModId = Entities.Modules.DesktopModuleController.GetDesktopModuleByFriendlyName("acDepartemnts")
            Dim mc As New DotNetNuke.Entities.Modules.ModuleController
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim x = mc.GetModuleByDefinition(PS.PortalId, "acDepartments")
            Response.Redirect(NavigateURL(x.TabID) & "?DeptId=" & e.CommandArgument)
        ElseIf e.CommandName = "RemoveDelegate" Then
            Dim q = From c In d.AP_StaffBroker_Departments Where c.CostCenterId = CInt(e.CommandArgument)
            If q.Count > 0 Then
                q.First.CostCentreDelegate = Nothing
            End If
            d.SubmitChanges()
            DataList2.DataBind()
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", PS.PortalId)

        End If
    End Sub

    Public Function isDelegated(ByVal DeptId As Integer) As Boolean
        Dim q = From c In d.AP_StaffBroker_Departments Where c.CostCenterId = DeptId And Not c.CostCentreDelegate Is Nothing
        Return q.Count > 0
    End Function

    Public Function GetDelegateName(ByVal DeptId As Integer) As String
        Dim q = From c In d.AP_StaffBroker_Departments Where c.CostCenterId = DeptId And Not c.CostCentreDelegate Is Nothing
        If q.Count > 0 Then
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim DelUser = UserController.GetUserById(PS.PortalId, q.First.CostCentreDelegate)

            Return DelUser.FirstName & " " & DelUser.LastName

        End If
        Return ""
    End Function
    Public Function GetLeaderName(ByVal DeptId As Integer) As String
        Dim q = From c In d.AP_StaffBroker_Departments Where c.CostCenterId = DeptId And Not c.CostCentreDelegate Is Nothing
        If q.Count > 0 Then
            Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim ManUser = UserController.GetUserById(PS.PortalId, q.First.CostCentreManager)

            Return ManUser.FirstName & " " & ManUser.LastName

        End If
        Return ""
    End Function
    Protected Sub btnDelegate_Click(sender As Object, e As System.EventArgs) Handles btnDelegate.Click
        Dim q = From c In d.AP_StaffBroker_Departments Where (CheckBox1.Checked Or c.CostCenterId = CInt(hfDeptId.Value)) And c.CostCentreManager = CInt(hfUserId.Value)
        For Each row In q
            If row.CostCentreDelegate Is Nothing Then
                If ddlDelegate.SelectedIndex = 0 Then
                    row.CostCentreDelegate = Nothing
                Else
                    row.CostCentreDelegate = ddlDelegate.SelectedValue
                End If
            End If
        Next
        d.SubmitChanges()
        DataList2.DataBind()
        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        StaffBrokerFunctions.SetSetting("tntFlag", "Dirty", PS.PortalId)

        Dim x As New tntWebUsers()
        x.RefreshWebUsers({CInt(ddlDelegate.SelectedValue)})
    End Sub
End Class
