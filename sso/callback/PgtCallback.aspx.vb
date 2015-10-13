Imports GCX

Partial Class sso_PgtCallback
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.QueryString("pgtId") <> "" Then
            Dim d As New GCXDataContext

            Dim insert As New Agape_GCX_Proxy
            insert.PGTID = Request.QueryString("pgtId")
            insert.PGTIOU = Request.QueryString("pgtIou")
            insert.Created = Now
            d.Agape_GCX_Proxies.InsertOnSubmit(insert)

            d.SubmitChanges()
            Dim old = From c In d.Agape_GCX_Proxies Where c.Created < Now.AddHours(-6)

            d.Agape_GCX_Proxies.DeleteAllOnSubmit(old)
            d.SubmitChanges()
        Else
            Dim d As New GCXDataContext
            Dim old = From c In d.Agape_GCX_Proxies Where c.Created < Now.AddHours(-6)

            d.Agape_GCX_Proxies.DeleteAllOnSubmit(old)
            d.SubmitChanges()
            'DEBUG ONLY
            '    Dim d As New GCXDataContext
            '    Dim insert As New Agape_GCX_Proxy
            '    insert.PGTID = Today.Date.ToShortDateString & "Error" & Request.QueryString.Count
            '    insert.Created = Now
            '    insert.PGTIOU = Request.ServerVariables("remote_addr")
            '    d.Agape_GCX_Proxies.InsertOnSubmit(insert)
            '    d.SubmitChanges()
        End If

        Response.Write(Response.StatusCode)
    End Sub
End Class
