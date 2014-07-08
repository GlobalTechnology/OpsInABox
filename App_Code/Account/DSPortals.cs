using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using AgapeConnect;

/// <summary>
/// Summary description for DSPortals
/// </summary>
[WebService(Namespace = "http://agapeconnect.me/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class DSPortals : System.Web.Services.WebService {
    public DSPortals () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    
    [WebMethod]
    //public List<string> GetPortalsForUserJson(Guid _ssoguid)
    public List<DataserverPortal> GetPortalsForUserJson(string ssoguid)
    {
        return GetPortals(Guid.Parse(ssoguid));
    }

    public List<DataserverPortal> GetPortals(Guid _ssoguid)
    {
        DSUserPortalView userportal = new DSUserPortalView();
        List<DataserverPortal> portallist = new List<DataserverPortal>();

        portallist = userportal.GetPortals(_ssoguid);

        return portallist;
    }
}
