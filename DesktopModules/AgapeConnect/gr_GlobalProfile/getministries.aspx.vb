Imports GR_NET
Partial Class DesktopModules_AgapeConnect_gr_GlobalProfile_getministries
    Inherits System.Web.UI.Page

    Public gr_server As String = "http://192.168.2.244:3000/"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim PS = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
        Dim ui = CType(HttpContext.Current.Items("UserInfo"), UserInfo)
        If Not String.IsNullOrEmpty(ui.Username) Then

            Response.Write("<option value=''></option>")

            If Request.QueryString("scope") <> "" Then
                Dim gr = New GR(StaffBrokerFunctions.GetSetting("gr_api_key", PS.PortalId), gr_server)
                Dim m = gr.GetEntities("ministry", "&created_by=all&filters[ministry_scope]=" & Request.QueryString("scope"), Nothing, 1000, 0)


                For Each row In m
                    Response.Write("<option value='" & row.ID & "'>" & row.GetPropertyValue("name.value") & "</option>")

                Next

            End If

        End If

        Response.End()
    End Sub
End Class
