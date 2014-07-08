Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports System.ServiceModel.Activation
Imports System.Net
Imports System.Linq


' NOTE: You can use the "Rename" command on the context menu to change the class name "gma_global_directory" in code, svc and config file together.
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class gma_global_directory
    Implements Igma_global_directory
    Function GetAllGmaServers(ByVal authKey As String) As List(Of GMA.gma_Server) Implements Igma_global_directory.GetAllGmaServers
        Dim d As New GMA.gmaDataContext

        Dim password = AgapeEncryption.AgapeEncrypt.Encrypt(authKey)
        'StaffBrokerFunctions.SetSetting("gma_global_directory_authkey", password, 0)

        If password = StaffBrokerFunctions.GetSetting("gma_global_directory_authkey", 0) Then
            Return d.gma_Servers.ToList
        Else
            Return Nothing
        End If







    End Function

   
    Public Shared Function AddGMAService(ByVal displayName As String, ByVal URL As Uri, ByVal Userid As Integer) As Boolean
        Try


            Dim d As New GMA.gmaDataContext
            Dim insert As New GMA.gma_Server
            insert.displayName = displayName
            insert.rootUrl = URL.AbsoluteUri
            insert.addedByUser = Userid
            Dim service = GetTargetService(insert.rootUrl)
            If Not String.IsNullOrEmpty(service) Then
                insert.serviceURL = service

                d.gma_Servers.InsertOnSubmit(insert)
                d.SubmitChanges()

                Return True
            End If
            StaffBrokerFunctions.EventLog("Failed to get CASService for Server: " & displayName & " at: " & URL.AbsoluteUri, "", 1)
            Return False

        Catch ex As Exception
            StaffBrokerFunctions.EventLog("Failed to add GMA Server: " & displayName & " at: " & URL.AbsoluteUri, ex.ToString(), 1)
            Return False
        End Try

    End Function
    Private Shared Function GetTargetService(ByVal _endPoint As String) As String
        Try


            Dim method = "?q=gmaservices"

            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(_endPoint & method), HttpWebRequest)

            request.AllowAutoRedirect = False

            Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            If response.StatusCode = HttpStatusCode.Redirect Then
                Dim redr = response.Headers("Location")
                Return redr.Substring(redr.IndexOf("service=") + 8)


            End If
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class
