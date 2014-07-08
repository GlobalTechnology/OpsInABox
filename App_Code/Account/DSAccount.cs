using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinistryViewDS;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Modules;
using System.IO;
using System.Xml;

/// <summary>
/// Summary description for Account
/// </summary>
public class DSAccount
{
    #region Constructors

    public DSAccount(string countryUrl, string ssoGuid, string accountCode, DateTime dateFrom, DateTime dateTo, string staffProfileCode)
    {
        _countryUrl = countryUrl;
        _ssoGuid = ssoGuid;

        if (dateFrom == null)
        { _dateFrom = FirstDayOfMonthFromDateTime(DateTime.Today); }
        else
        { _dateFrom = dateFrom; }
        
        if (dateTo == null)
        { _dateTo = LastDayOfMonthFromDateTime(DateTime.Today); }
        else
        { _dateTo = dateTo; }

        if (accountCode == null)
        { _accountCode = ""; }
        else
        { _accountCode = accountCode; }

        if (staffProfileCode == null)
        { _profileCode = getProfile(accountCode); } 
        else 
        { _profileCode = staffProfileCode; }
       
        RefreshData();
    }
    #endregion

    #region Non-Static Properties



    private DateTime? _dateFrom;
    public DateTime? DateFrom
    {
        get { return _dateFrom; }
        set
        {
            _dateFrom = value;
        }
    }
    private DateTime? _dateTo;
    public DateTime? DateTo
    {
        get { return _dateTo; }
        set
        {
            _dateTo = value;
        }
    }

    private string _profileCode;
    public string ProfileCode
    {
        get { return _profileCode; }
        set 
        { 
            _profileCode = value;
        }
    }

    private string _accountCode;
    public string AccountCode
    {
        get { return _accountCode; }
        set
        {
            _profileCode = value;
            _profileCode = getProfile(_accountCode);
        }
    }

    private FinancialAccount[] _financialAccount;
    public FinancialAccount[] FinancialAccount
    {
        get { return _financialAccount; }
        set { _financialAccount = value; }
    }

    private IQueryable<FinancialTransaction> _transactions;

    public IQueryable<FinancialTransaction> Transactions
    {
        get 
        {
            if(_transactions==null) getTransactions();
            return _transactions; 
        }
    }

    private string _countryUrl;

    public string CountryUrl
    {
        get { return _countryUrl; }
        set
        {
            _countryUrl = value;
        }
    }
    #endregion

    #region Static Properties
    private string _pgtId { get; set; }
    private string _ssoGuid { get; set; }
    private int _portalId { get; set; }
    private int _userId { get; set; }
    private MinistryViewDSServices _tnt = new MinistryViewDSServices();
    private myAccounts _myAccount;

    public myAccounts MyAccounts
    {
        get { return _myAccount; }
        set { getSummary(); }
    }
    #endregion

    #region Basic Methods
    private DateTime FirstDayOfMonthFromDateTime(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
    }
    


    private string getProfile(string AccountCode)
    {
        return _myAccount
            .Countries.Where<Country>(c => c.URL == _countryUrl).First<Country>()
            .Profiles.Where(cp => AccountCode == cp.Accounts
                .Where<FinancialAccounts>(ac => ac.AccountID == AccountCode).First().AccountID.ToString())
                .First().ProfileCode.ToString();
    }

    public double BalanceForAccount(string AccountCode)
    {
        return _myAccount.Countries.Where<Country>(c => c.URL == _countryUrl).First<Country>()
            .Profiles.Where<CountryProfile>(p => p.ProfileCode == _profileCode).First()
            .Accounts.Where<FinancialAccounts>(a => a.AccountID == AccountCode)
            .Sum<FinancialAccounts>(accts => accts.Balance);
    }


    public double BalanceForProfile(string ProfileCode)
    {
        return _myAccount.Countries.Where<Country>(c => c.URL == _countryUrl).First<Country>()
            .Profiles.Where<CountryProfile>(p => p.ProfileCode == _profileCode).First()
            .Accounts.Sum<FinancialAccounts>(accts => accts.Balance);
    }

    


    public void RefreshData()
    {
        getSummary();
    }
    #endregion

    #region Web Service Methods
    private void getSummary()
    {
        if ((_ssoGuid == null)||(_pgtId == null))
        { 
            setProps(); 
        }

        if (string.IsNullOrEmpty(_pgtId))
        { 
            setPgtId(); 
        }

        try
        {
            _myAccount = _tnt.GetSummary(_pgtId, _ssoGuid);
            if (_myAccount == null)
            {

            }
        }
        catch (Exception)
        {

            _myAccount = null;
        }
    }

    static private string GetProxyTicketFromCAS(string targetService, string _pgt)
    {
        string pt = string.Empty;
        string server = "https://thekey.me/cas/";

        string validateurl = server + "proxy?targetService=" + targetService + "&pgt=" + _pgt.Trim().ToString();


        System.IO.Stream s;

        try
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            s = wc.OpenRead(validateurl);
        }
        catch (Exception e)
        {
            // error
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


    


    static public double getAccountBalance(string GUID, string PGTIOU, string CountryURL, string AccountCode)
    {
        string PGTID = (string) HttpContext.Current.Session["pgtId"] ;
        if(PGTID==null) 
            
            PGTID = new theKeyProxyTicket.PGTCallBack().RetrievePGTCallback("CASAUTH", "thecatsaysmeow3", PGTIOU);
            
        HttpContext.Current.Session.Add("pgtId", PGTID) ;
        var dTnT =  new dynamicTnT.TntMPDDataServerWebService ();
        dTnT.Url = CountryURL + "dataquery/dataqueryservice.asmx";
        dTnT.Discover();
        string sessionId = (string)HttpContext.Current.Session["TnT-" +  dTnT.Url];
        if (string.IsNullOrEmpty(sessionId))
        {
                string service =  dTnT.GetServiceName();
                string pt = GetProxyTicketFromCAS(service, PGTID);
                string UserName;
                bool IsReg;
                sessionId = dTnT.Login(service, pt,  true, out UserName, out IsReg);
                HttpContext.Current.Session.Add("TnT-" +  dTnT.Url, sessionId);
        }
        var resp = dTnT.GetStaffProfiles(sessionId);
        foreach( var prof in resp)
        {
             dynamicTnT.FinancialAccount[] fa  ;
            bool isT;
          var desg=  dTnT.GetStaffProfileSummary(sessionId, prof.Code,out fa,out  isT );
            var mycodes= fa.Where(x => x.Code==AccountCode);
            if( mycodes.Count() >0)
                return (double) mycodes.First().EndingBalance ;



        }

        return 0;

       //     tntTransactions = dTnT.GetFinancialTransactions(sessionId, MyProfiles.SelectedValue, _startDate, _endDate, (MyAccounts.SelectedValue == "All Accounts" ? "" : MyAccounts.SelectedValue), false, out tntAccounts);
       //     StartingBalance.Text = tntAccounts.Sum(x => x.BeginningBalance).ToString("0");
            




       //mvds.Url = CountryURL + "dataquery/dataqueryservice.asmx";
       // mvds.Discover() ;
       // string service = mvds.GetServiceName(
       // var x = mvds.login(

       // myAccounts acs = new MinistryViewDSServices().GetSummary(PGTID, GUID);
       // var ctry = acs.Countries.Where<Country>(c => c.URL == CountryURL) ;

       // if(ctry.Count()>0)
       // {
       //     var pFiles = from c in ctry.First().Profiles where c.Accounts.Any(ac => ac.AccountID == AccountCode) select c ;
       //     if (pFiles.Count()>0)
       //     {
       //         var Acct = from c in pFiles.First().Accounts where c.AccountID==AccountCode select c.Balance ;
       //         if (Acct.Count()>0)
       //             return Acct.First();
       //     }

           
       // }
       // return 0.0;
    }


    public void getTransactions()
    {
        if (!(string.IsNullOrEmpty(_countryUrl) || string.IsNullOrEmpty(_ssoGuid)))
        {
            try
            {
               
                _transactions = _tnt.GetFinancialTransactions(_countryUrl,
                    _ssoGuid,  _profileCode,
                    (DateTime)_dateFrom, (DateTime)_dateTo, _accountCode,
                    false, ref _financialAccount).AsQueryable<FinancialTransaction>();
            }
            catch (Exception e)
            {
                _transactions = null;
            }
            
        }
        else { _transactions = null; }
    }

    private void setProps()
    {
        PortalSettings portalInfo = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

        _userId = portalInfo.UserId;
        _portalId = portalInfo.PortalId;
        _ssoGuid = UserController.GetUserById(_portalId, _userId).Profile.GetPropertyValue("ssoGUID");
    }

    private void setPgtId()
    {
        string pgtiou = UserController.GetUserById(_portalId, _userId).Profile.GetPropertyValue("GCXPGTIOU");

        if (!string.IsNullOrEmpty(pgtiou))
        {
            try
            {
                _pgtId = new theKeyProxyTicket.PGTCallBack().RetrievePGTCallback("CASAUTH", "thecatsaysmeow3", pgtiou);
                HttpContext.Current.Session.Add("pgtId",_pgtId);
            }
            catch
            {
                _pgtId = string.Empty;
            }

            //KeyUser.KeyUser kUser = new KeyUser.KeyUser(new Guid("de4f5f1a-faf8-db81-ad4b-9e4244aabad7"), new Guid("3181c762-195d-4229-817e-c8f665ff3aa4"));
            //_pgtId = kUser.ProxyTicket;  //= null;
        }
        else
        {
            _pgtId = string.Empty;
        }
    }
    #endregion
}