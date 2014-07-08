Imports DotNetNuke
Imports DotNetNuke.Security
Imports StaffBroker

Partial Class DesktopModules_AgapePortal_StaffBroker_StaffChildren
    Inherits System.Web.UI.UserControl

   
    Private _StaffId As Integer
    Public Property StaffId() As Integer
        Get
            Return _StaffId
        End Get
        Set(ByVal value As Integer)
            _StaffId = value

            hfChildStaffId.Value = _StaffId
        End Set
    End Property
 

    Dim d As New StaffBrokerDataContext

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        'jQuery.RequestDnnPluginsRegistration()
    End Sub

    
   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        'GridView1.DataSource = (From c In d.AP_StaffBroker_Childrens Where c.StaffId = _StaffId)
        'GridView1.DataBind()
        If Not Page.IsPostBack Then
            hfChildStaffId.Value = _StaffId
        End If

        'GridView1.DataBind()
    End Sub


    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "myInsert" Then
            Dim d As New StaffBrokerDataContext
            Dim insert = New AP_StaffBroker_Children

            insert.FirstName = CType(GridView1.FooterRow.Controls(0).FindControl("tbFirstName"), TextBox).Text
            insert.Birthday = CType(GridView1.FooterRow.Controls(0).FindControl("ftDOB"), TextBox).Text
            insert.StaffId = CInt(hfChildStaffId.value)

            d.AP_StaffBroker_Childrens.InsertOnSubmit(insert)
            d.SubmitChanges()
            GridView1.DataBind()
        ElseIf e.CommandName = "myInsertE" Then
            Dim d As New StaffBrokerDataContext
            Dim insert = New AP_StaffBroker_Children

            insert.FirstName = CType(GridView1.Controls(0).Controls(1).FindControl("tbFirstNameE"), TextBox).Text
            insert.Birthday = CType(GridView1.Controls(0).Controls(1).FindControl("tbDOBE"), TextBox).Text
            insert.StaffId = CInt(hfChildStaffId.value)

            d.AP_StaffBroker_Childrens.InsertOnSubmit(insert)
            d.SubmitChanges()
            GridView1.DataBind()
        End If
    End Sub

   
    Public Function GetDateFormat() As String
        Dim sdp As String = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower
        If sdp.IndexOf("d") < sdp.IndexOf("m") Then
            Return "dd/mm/yy"
        Else
            Return "mm/dd/yy"
        End If
    End Function
End Class
