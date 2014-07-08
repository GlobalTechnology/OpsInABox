using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

// NOTE: You can use the "Rename" command on the context menu to change the interface name "IDSPortalUsers" in both code and config file together.
[ServiceContract()]
public interface IDSPortalUsers
{
    [OperationContract]
    [WebGet(ResponseFormat = WebMessageFormat.Xml,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "dsportals/xml/ssoguid={ssoguid}")]
    List<DataserverPortal> GetPortalsForUserXML(Guid _ssoguid);

    [OperationContract]
    [WebGet(ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "dsportals/json/ssoguid={ssoguid}")]
    List<DataserverPortal> GetPortalsForUserJson(Guid _ssoguid);

    List<DataserverPortal> GetPortals(Guid _ssoguid);
}