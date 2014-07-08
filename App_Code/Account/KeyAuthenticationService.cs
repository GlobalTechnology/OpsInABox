using System.Data.Services;
using System.ServiceModel;
using System.ServiceModel.Web;
using KeyUser;
using System;
using System.ServiceModel.Activation;

namespace KeyUser
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class KeyAuthenticationService
    {
        public struct KeyLoginResponce
        {
            public bool LoginSuccess;
            public string GUID;
            public string ProxyTicket;
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "key/xml/username={username}&password={password}&targetService={targetService}")]
        public KeyLoginResponce AuthenticateKeyXML(string username, string password, string targetService)
        {
            return LoginUser(username, password, targetService);

        }
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "key/json/username={username}&password={password}&targetService={targetService}")]
        public KeyLoginResponce AuthenticateKeyJSON(string username, string password, string targetService)
        {



            return LoginUser(username, password, targetService);
        }


        private KeyLoginResponce LoginUser(string username, string password, string targetService)
        {
            if (targetService == null) targetService = "";
            KeyLoginResponce rtn = new KeyLoginResponce();
            rtn.LoginSuccess = false;
            KeyAuthentication objKey = new KeyAuthentication(username, password, targetService);
            if (!String.IsNullOrEmpty(objKey.KeyGuid))
            {
                rtn.LoginSuccess = true;
                rtn.GUID = objKey.KeyGuid;
                rtn.ProxyTicket = objKey.ProxyTicket;
            }
            return rtn;
        }

        //[OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Xml,
        //    BodyStyle = WebMessageBodyStyle.Wrapped,
        //    UriTemplate = "key/xml/username={username}&password={password}&device={device}")]
        //public string AuthenticateKeyXML(string username, string password, string device)
        //{
        //    return HelloWorld(username, password, device).KeyGuid.ToString();
        //}

        //[OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Wrapped,
        //    UriTemplate = "key/json/username={username}&password={password}&device={device}")]
        //public KeyUser AuthenticateKeyJSON(string username, string password, string device)
        //{
        //    return HelloWorld(username, password, device);
        //}

        //[OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Xml,
        //    BodyStyle = WebMessageBodyStyle.Wrapped,
        //    UriTemplate = "mobile/xml/ssoGuid={ssoGuid}&mobilePasscode={mobilePasscode}")]
        //public KeyUser AuthenticateMobileXML(string ssoGuid, string mobilePasscode)
        //{
        //    return HelloWorld(ssoGuid, mobilePasscode);
        //}

        //[OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Wrapped,
        //    UriTemplate = "mobile/json/ssoGuid={ssoGuid}&mobilePasscode={mobilePasscode}")]
        //public KeyUser AuthenticateMobileJSON(string ssoGuid, string mobilePasscode)
        //{
        //    return HelloWorld(ssoGuid, mobilePasscode);
        //}

        private static KeyUser HelloWorld(string username, string password, string device)
        {
            KeyUser kUser = new KeyUser(username, password, device);

            return kUser;
        }

        private static KeyUser HelloWorld(string ssoGuid, string mobilePasscode)
        {
            Guid sg, mp;
            sg = new Guid(ssoGuid);
            mp = new Guid(mobilePasscode);

            KeyUser kUser = new KeyUser(sg, mp);

            return kUser;
        }
    }
}