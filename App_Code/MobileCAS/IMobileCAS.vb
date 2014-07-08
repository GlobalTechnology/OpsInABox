Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports System.ServiceModel.Activation






' NOTE: You can use the "Rename" command on the context menu to change the interface name "IMobileCAS" in both code and config file together.
<ServiceContract(Namespace:="https://agapeconnect.me/MobileCAS/", Name:="MobileCAS.svc")>
Public Interface IMobileCAS

    <OperationContract()>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, UriTemplate:="AuthenticateWithSpecifiedCAS?username={username}&password={password}&targetService={targetService}&casServer={casServer}")>
    Function AuthenticateWithSpecifiedCAS(ByVal username As String, ByVal password As String, ByVal targetService As String, ByVal casServer As String) As CASforMobile.KeyUserDetails

    <OperationContract()>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, UriTemplate:="AuthenticateWithTheKey?username={username}&password={password}&targetService={targetService}")>
    Function AuthenticateWithTheKey(ByVal username As String, ByVal password As String, ByVal targetService As String) As CASforMobile.KeyUserDetails


    <OperationContract()>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, UriTemplate:="GetAccountBalance?CountryURL={CountryURL}&GUID={GUID}&PGTIOU={PGTIOU}&Account={Account}")>
    Function GetAccountBalance(ByVal CountryURL As String, ByVal GUID As String, ByVal PGTIOU As String, ByVal Account As String) As Double

    <OperationContract()>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, UriTemplate:="ConvertCurrency?FromCur={FromCur}&ToCur={ToCur}")>
    Function ConvertCurrency(ByVal FromCur As String, ByVal ToCur As String) As Double

End Interface
