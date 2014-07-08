Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ServiceModel.Web

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Description:="This webservice is designed to be used by Mobile Apps, to greatly simplify the process of authenticating via CAS (TheKey/Relay).", Name:="casAuth", Namespace:="https://agapeconnect.me/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class CASforMobile
    Inherits System.Web.Services.WebService

    Structure KeyUserDetails
        Dim LoginSuccess As Boolean
        Dim GUID As String
        Dim ProxyTicket As String
        Dim ServiceTicket As String
        Dim UserName As String
    End Structure



    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function
    '  <System.Web.Script.Services.ScriptMethod(ResponseFormat:= Script.Services.ResponseFormat.Json) ;

    <WebMethod(Description:="This WebMethod will authenticate a user with The Key. By supplying the users Username and Password for the Key, you will receive a response indicating if the login was successfull and the user's GUID (unique identifier for the KEY). If your app uses another webService, you will also want to supply its address (under TargetService). You will then receive back a Proxy Ticket, which you can pass to your webservice allowing your webservice to independently verify the users identity with TheKey.")> _
    <WebGet(RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="key/json/username/{username}/password/{password}/targetService/{targetService}")> _
    Public Function AuthenticateWithTheKey(ByVal username As String, ByVal password As String, ByVal targetService As String) As KeyUserDetails
        Dim rtn As New KeyUserDetails()
        rtn.LoginSuccess = False

        Dim objKey As New KeyUser.KeyAuthentication(UserName, password, targetService)
        If Not (String.IsNullOrEmpty(objKey.KeyGuid)) Then
            rtn.LoginSuccess = True
            rtn.GUID = objKey.KeyGuid
            rtn.ProxyTicket = objKey.ProxyTicket
            rtn.ServiceTicket = objKey.ServiceTicket
        End If

        Return rtn
    End Function

    <WebMethod(Description:="This WebMethod is indentical to AuthenticateWithTheKey, except that it allows you to authenticate against a specified CAS Server (eg. https://thekey.me/cas/). ")> _
     <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function Authenticate(ByVal UserName As String, ByVal Password As String, ByVal TargetService As String, ByVal Service As String) As KeyUserDetails
        Dim rtn As New KeyUserDetails()
        rtn.LoginSuccess = False

        Dim objKey As New KeyUser.KeyAuthentication(UserName, Password, TargetService, Service)
        If Not (String.IsNullOrEmpty(objKey.KeyGuid)) Then
            rtn.LoginSuccess = True
            rtn.GUID = objKey.KeyGuid
            rtn.ProxyTicket = objKey.ProxyTicket
            rtn.ServiceTicket = objKey.ServiceTicket
        End If

        Return rtn
    End Function

End Class