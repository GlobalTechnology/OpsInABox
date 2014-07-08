Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports System.ServiceModel.Activation


' NOTE: You can use the "Rename" command on the context menu to change the interface name "Igma_global_directory" in both code and config file together.

<ServiceContract(Namespace:="https://agapeconnect.me/GMA/", Name:="gma_global_directory.svc")>
Public Interface Igma_global_directory

    <OperationContract()>
        <WebGet(ResponseFormat:=WebMessageFormat.Json, UriTemplate:="GetAllGmaServers?authKey={authKey}")>
    Function GetAllGmaServers(ByVal authKey As String) As List(Of GMA.gma_Server)

    'Sub DoWork()

End Interface
