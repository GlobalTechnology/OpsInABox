using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using System.Web;

namespace KeyUser
{
    public class KeyAuthentication
    {
        #region Constructor
        public KeyAuthentication(string username, string password, string targetService = "", string server = "https://thekey.me/cas/")
        {
            _username = username;
            _password = password;
            _keyGuid = string.Empty;
            _targetService = targetService;
            _server = server;
            GetProxyTicket();
        }
        #endregion //Constructor

        #region Properties
        private string _username;
        private string _password;
        private string _keyGuid;
        private string _tgt;
        private string _tgtLocation;
        private string _st;
        private string _pgtIou;
        private string _pgt;
        private string _pt;
        private string _targetService;
        private string _server;

        public string server
        {
            get { return _server; }
        }

        public string TargetService
        {
            get { return _targetService; }
        }
        public string ProxyTicket
        {
            get { return _pt; }
        }

        public string ProxyGrantingTicket
        {
            get { return _pgt; }
        }

        public string ProxyGrantingTicketIOU
        {
            get { return _pgtIou; }
        }

        public string TicketGrantingTicket
        {
            get { return _tgt; }
        }

        public string ServiceTicket
        {
            get { return _st; }
        }

        public string KeyGuid
        {
            get { return _keyGuid; }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                GetProxyTicket();
            }
        }

        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                GetProxyTicket();
            }
        }

        public List<Exception> _exceptions = new List<Exception>();
        #endregion //Properties

        #region Methods
        private void GetProxyTicket()
        {
            _tgt = _st = _pgtIou = _pgt = string.Empty;

            if (!(string.IsNullOrEmpty(_username) || (string.IsNullOrEmpty(_password))))
            {
               // string server = "https://thekey.me/cas/";
                string restServer = server + "v1/tickets";
               //string service = "http://localhost:13059/Default.aspx";
                string service = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);

                //Call theKey REST Api, passing username and password
                _tgt = GetTicketGrantingTicket(restServer, service);

                if (!string.IsNullOrEmpty(_tgt))
                {
                    
                    _st = GetServiceTicket(service);

                    if (!string.IsNullOrEmpty(_st))
                    {
                        _pgtIou = GetProxyTicketIou(_server, service);

                        if (!string.IsNullOrEmpty(_pgtIou))
                        {
                            _pgt = GetProxyGrantingTicket(_server, restServer, service);
                            if (!string.IsNullOrEmpty(_pgt) && _targetService!="")
                            {
                                _pt = GetProxyTicketFromCAS(_server, _targetService);
                            }
                        }
                    }
                }


            }
        }

        private string GetProxyTicketFromCAS(string server, string targetService)
   {
            string pt = string.Empty;

            string validateurl = server + "proxy?targetService=" + targetService + "&pgt=" + _pgt.Trim().ToString();
            

            Stream s;

            try
            {
                WebClient wc = new WebClient();
                s = wc.OpenRead(validateurl);
            }
            catch (Exception e)
            {
                //Log error
                _exceptions.Add(e);

                return pt;
            }

            StreamReader streamReader = new StreamReader(s);

            XmlDocument doc = new XmlDocument();
            doc.Load(streamReader);
            XmlNamespaceManager NamespaceMgr = new XmlNamespaceManager(doc.NameTable);
            NamespaceMgr.AddNamespace("cas", "http://www.yale.edu/tp/cas");
          
            XmlNode SuccessNode = doc.SelectSingleNode("/cas:serviceResponse/cas:proxySuccess", NamespaceMgr);
            
            if (!(SuccessNode == null))
            {
                XmlNode ProxyTicketNode = SuccessNode.SelectSingleNode("./cas:proxyTicket", NamespaceMgr);
                if(!(ProxyTicketNode == null))
                    return ProxyTicketNode.InnerText ;
            }

            return pt;
        }


        static public string GetProxyTicketFromCAS(string server, string targetService, string tgt)
        {
            string pt = string.Empty;

            string validateurl = server + "proxy?targetService=" + targetService + "&pgt=" + tgt.Trim().ToString();


            Stream s;

            try
            {
                WebClient wc = new WebClient();
                s = wc.OpenRead(validateurl);
            }
            catch (Exception e)
            {
                //Log error
               
                return pt;
            }

            StreamReader streamReader = new StreamReader(s);

            XmlDocument doc = new XmlDocument();
            doc.Load(streamReader);
            XmlNamespaceManager NamespaceMgr = new XmlNamespaceManager(doc.NameTable);
            NamespaceMgr.AddNamespace("cas", "http://www.yale.edu/tp/cas");

            XmlNode SuccessNode = doc.SelectSingleNode("/cas:serviceResponse/cas:proxySuccess", NamespaceMgr);

            if (!(SuccessNode == null))
            {
                XmlNode ProxyTicketNode = SuccessNode.SelectSingleNode("./cas:proxyTicket", NamespaceMgr);
                if (!(ProxyTicketNode == null))
                    return ProxyTicketNode.InnerText;
            }

            return pt;
        }



        static public string getProxyTicket(string server, string service, string username, string password)
        {
            string tgt = string.Empty;
            HtmlDocument responseDoc;

            string postData = "service=" + service + "&username=" + username + "&password=" + password;
            responseDoc = postToCAS(server + "v1/tickets/", postData);

            if (responseDoc != null)
            {
                tgt = parseTGT(responseDoc, server + "v1/tickets/");
            }
            string pt =  GetProxyTicketFromCAS(server, service, tgt);
            return pt;
        }

        private string GetTicketGrantingTicket(string server, string service)
        {
            string tgt = string.Empty;
            HtmlDocument responseDoc;

            string postData = "service=" + service + "&username=" + _username + "&password=" + _password;
            responseDoc = PostToCAS(server, postData);

            if (responseDoc != null)
            {
                tgt = ParseTGT(responseDoc, server);
            }

            return tgt;
        }

        private string GetServiceTicket(string service)
        {
            string postData = "service=" + service;
            HtmlDocument responseDoc;
            responseDoc = PostToCAS(_tgtLocation, postData);

            if (!string.IsNullOrEmpty(responseDoc.DocumentNode.InnerHtml))
            {
                return responseDoc.DocumentNode.InnerHtml;
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetProxyTicketIou(string server, string service)
        {
            string pgtIou = string.Empty;

            string validateurl = server + "proxyValidate?ticket=" + _st.Trim().ToString()
                + "&service=" + service
                + "&pgtUrl=https://agapeconnect.me/CasLogin.aspx";

            Stream s;

            try
            {
                WebClient wc = new WebClient();
                s = wc.OpenRead(validateurl);
            }
            catch (Exception e)
            {
                //Log error
                _exceptions.Add(e);

                return pgtIou;
            }

            StreamReader streamReader = new StreamReader(s);

            XmlDocument doc = new XmlDocument();
            doc.Load(streamReader);
            XmlNamespaceManager NamespaceMgr = new XmlNamespaceManager(doc.NameTable);
            NamespaceMgr.AddNamespace("cas", "http://www.yale.edu/tp/cas");
            //Check for success
            XmlNode ServiceResponse = doc.SelectSingleNode("/cas:serviceResponse/cas:authenticationFailure", NamespaceMgr);

            if (!(ServiceResponse == null))
            {
                return pgtIou;
            }

            XmlNode SuccessNode = doc.SelectSingleNode("/cas:serviceResponse/cas:authenticationSuccess", NamespaceMgr);

            if (!(SuccessNode == null))
            {
                if (!(SuccessNode.SelectSingleNode("./cas:attributes/ssoGuid", NamespaceMgr) == null))
                { _keyGuid = SuccessNode.SelectSingleNode("./cas:attributes/ssoGuid", NamespaceMgr).InnerText; }

                if (!(SuccessNode.SelectSingleNode("./cas:proxyGrantingTicket", NamespaceMgr) == null))
                { pgtIou = SuccessNode.SelectSingleNode("./cas:proxyGrantingTicket", NamespaceMgr).InnerText; }
                else { pgtIou = string.Empty; }
            }

            return pgtIou;
        }

        private string GetProxyGrantingTicket(string server, string restServer, string service)
        {
            string returnPgt;

            try
            {
                
                returnPgt = new theKeyProxyTicket.PGTCallBack().RetrievePGTCallback("CASAUTH", "thecatsaysmeow3", _pgtIou);
            }
            catch
            {
                returnPgt = string.Empty;
            }

            if (string.IsNullOrEmpty(returnPgt)) 
            { 
                returnPgt = string.Empty; 
            }
            return returnPgt;
        }

        private string ParseTGT(HtmlDocument responseDoc, string restServer)
        {
            string returnTgt = string.Empty;

            foreach (var item in responseDoc.DocumentNode.SelectSingleNode("/html/body/form").Attributes)
            {
                if (item.Name == "action")
                {
                    returnTgt = item.Value.Remove(0, (restServer.Length));
                }
            }

            return returnTgt;
        }
        static private string parseTGT(HtmlDocument responseDoc, string restServer)
        {
            string returnTgt = string.Empty;

            foreach (var item in responseDoc.DocumentNode.SelectSingleNode("/html/body/form").Attributes)
            {
                if (item.Name == "action")
                {
                    returnTgt = item.Value.Remove(0, (restServer.Length));
                }
            }

            return returnTgt;
        }

       static private HtmlDocument postToCAS(string restServer, string postData)
        {
            Stream dataStream;
            WebResponse response;
            HtmlDocument responseDoc;

            WebRequest request = WebRequest.Create(restServer);

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                response = request.GetResponse();
                dataStream = response.GetResponseStream();

                if (string.IsNullOrEmpty(restServer ))
                {
                    restServer = response.Headers.GetValues("Location").ToArray()[0];
                }

                responseDoc = new HtmlDocument();
                responseDoc.Load(dataStream);

                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                responseDoc = null;
                //_exceptions.Add(e);
            }

            return responseDoc;
        }


        private HtmlDocument PostToCAS(string restServer, string postData)
        {
            Stream dataStream;
            WebResponse response;
            HtmlDocument responseDoc;

            WebRequest request = WebRequest.Create(restServer);

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                response = request.GetResponse();
                dataStream = response.GetResponseStream();

                if (string.IsNullOrEmpty(_tgtLocation))
                {
                    _tgtLocation = response.Headers.GetValues("Location").ToArray()[0];
                }

                responseDoc = new HtmlDocument();
                responseDoc.Load(dataStream);

                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                responseDoc = null;
                _exceptions.Add(e);
            }

            return responseDoc;
        }


        #endregion //Methods
    }
}