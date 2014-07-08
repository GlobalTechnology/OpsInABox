Imports System.IO
Partial Class DesktopModules_AgapeConnect_gr_mapping_gr_sub
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.Clear()

        Try

       
        If Request.HttpMethod = "POST" Then
            Dim json = New StreamReader(Request.InputStream).ReadToEnd
            Dim jss = New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim resp = jss.Deserialize(Of Dictionary(Of String, Object))(json)
            If resp("action") = "confirm" Then
                'GET 
                Dim web As New System.Net.WebClient()

                    AgapeLogger.WriteEventLog(0, "GR sub confirm received:" & resp("confirmation_url"))
                Dim q = web.DownloadString(resp("confirmation_url"))
                Response.Write(q)




                Response.End()


            End If

        End If
        Catch ex As Exception

            AgapeLogger.WriteEventLog(0, "Error on subscription callback: " & ex.ToString)
            Response.End()
        End Try


    End Sub
End Class
