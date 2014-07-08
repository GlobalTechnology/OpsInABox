// NOTE: You can use the "Rename" command on the context menu to change the class name "DSPortalUsers" in code, svc and config file together.
using System.Collections.Generic;
using AgapeConnect;
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

[ServiceContract]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class DSPortalUsers : IDSPortalUsers
{
    [OperationContract]
    [WebGet(ResponseFormat = WebMessageFormat.Xml,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "xml/ssoguid={ssoguid}")]
    public List<DataserverPortal> GetPortalsForUserXML(Guid _ssoguid)
    {
        return GetPortals(_ssoguid);
    }

    [OperationContract]
    [WebGet(ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "json/ssoguid={ssoguid}")]
    public List<DataserverPortal> GetPortalsForUserJson(Guid _ssoguid)
    {
        return GetPortals(_ssoguid);
    }

    public List<DataserverPortal> GetPortals(Guid _ssoguid)
    {
        DSUserPortalView userportal = new DSUserPortalView();
        List<DataserverPortal> portallist = new List<DataserverPortal>();

        portallist = userportal.GetPortals(_ssoguid);

        return portallist;
    }
}
