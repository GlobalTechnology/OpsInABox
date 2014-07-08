
Partial Class DesktopModules_AgapeConnect_GMA_Graphlet
    Inherits System.Web.UI.Page




    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim nodeId As Integer = Request.QueryString("nodeId")
        Dim measurementId As Integer = Request.QueryString("measurementId")
        Dim gmaURL = Server.UrlDecode(Request.QueryString("gmaURL"))

        Dim gmaServers As List(Of GmaServices.gmaServer) = Session("gmaServers")
        Dim gmaServer = (From c In gmaServers Where c.URL = gmaURL).First
        gmaServer.gma.GetReportGraph(nodeId, measurementId).Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)


    End Sub
End Class
