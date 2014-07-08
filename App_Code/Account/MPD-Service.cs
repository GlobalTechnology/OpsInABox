using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
/// <summary>
/// Summary description for MPD_Service
/// </summary>
public static  class MPD_Service
{
    //static string serviceURL = "https://staffweb.cru.org:443/ss/servlet/TntMPDServlet/";


	public struct Donation
    {
        public int account{get;set;}
        public int PeopleId{ get; set; }
        public string DonorName { get; set; }
        public DateTime DonationDate { get; set; }
        public string DonationId { get; set; }
        public string Payment_Method { get; set; }
        public double Amount { get; set; }
        
        public string FiscalPeriod { get; set; }
        public string MonthName { get; set; }
       
    }

    public static double? GetAccountBalance(string Username, string Password, string serviceURL, string Action)
    {
        try
        {




            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceURL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            StringBuilder data = new StringBuilder();
            data.Append("Username=" + HttpUtility.UrlEncode(Username));
            data.Append("&Password=" + HttpUtility.UrlEncode(Password));
            data.Append("&Action=" + HttpUtility.UrlEncode(Action));
            data.Append("&TextError=y");
            request.ContentLength = data.Length;

            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(data);
            requestWriter.Close();
            WebResponse webResponse = request.GetResponse();
            if (!webResponse.ContentType.Contains("csv")) return null;
            Stream webStream = webResponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(webStream);
            string response = responseReader.ReadToEnd();
            responseReader.Close();
            CSVHelper csv = new CSVHelper(response, ",");
            bool first = true;
            double rtn = 0.0;
            if (csv.Count <= 1) return null;
            foreach (string[] line in csv)
            {
                if (first) first = false;
                else
                {
                    rtn += double.Parse(line[1]);
                }
            }
            return rtn;

        }
        catch (Exception)
        {

            return null;
        }
    } 

    public static List<Donation> getDonations(string Username, string Password, string serviceURL, string Action, DateTime DateFrom, DateTime DateTo )
    {

        List<Donation> donations = new List<Donation>();
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceURL);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        StringBuilder data = new StringBuilder();
        data.Append("Username=" + HttpUtility.UrlEncode(Username));
        data.Append("&Password=" + HttpUtility.UrlEncode(Password));
        data.Append("&Action=" + HttpUtility.UrlEncode(Action));
        data.Append("&DateFrom=" + HttpUtility.UrlEncode(DateFrom.ToString("M/d/yyyy")));
        data.Append("&DateTo=" + HttpUtility.UrlEncode(DateTo.ToString("M/d/yyyy")));
        data.Append("&Order=Date");
        request.ContentLength = data.Length;
        
        StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
        requestWriter.Write(data);
        requestWriter.Close();

        try
        {
            WebResponse webResponse = request.GetResponse();
            if (!webResponse.ContentType.Contains("csv"))
            {
                Donation rtn = new Donation();
                rtn.DonorName = "Authentication/Connection Error. <a href=\"javascript: $('#divAddCountry').dialog('open');\">Click here</a> to reauthenticate.";

                rtn.PeopleId = -1;
                StaffBrokerFunctions.EventLog("MPD-ServiceError downloading Transaction for " + Username, rtn.DonorName, 1);
                donations.Add(rtn);
                return donations;
            }

            Stream webStream = webResponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(webStream);
            string response = responseReader.ReadToEnd();

            


            //Console.Out.WriteLine(response);
            responseReader.Close();

            

            
            CSVHelper csv = new CSVHelper(response,",");
            bool first = true;
            foreach(string[] line in csv)
            {
                
                if (first) first = false;
                else
                {
                    try
                    {

                   
                        Donation don = new Donation();
                        don.account = int.Parse(line[0]);
                        don.PeopleId = int.Parse(line[1]);
                        don.DonorName = line[2].Replace("\"", "");
                        IFormatProvider culture = new System.Globalization.CultureInfo("en-US");

                        don.DonationDate = DateTime.Parse(line[3], culture);
                        don.DonationId = line[4];
                        don.Payment_Method = line[6];
                        try
                        {
                            don.Amount = double.Parse(line[10], new System.Globalization.CultureInfo(""));
                  
                        }
                        catch (Exception)
                        {

                            don.Amount = double.Parse(line[9], new System.Globalization.CultureInfo(""));
                  
                        }

                    
                   
                        don.FiscalPeriod = don.DonationDate.ToString("yyyyMM");
                        don.MonthName = don.DonationDate.ToString("MMM yy");
                        donations.Add(don);
                    }
                    catch (Exception e)
                    {
                        Donation rtn = new Donation();
                        StaffBrokerFunctions.EventLog("MPD-Service Error downloading a donation for " + Username, e.Message, 1);

                        rtn.DonorName = "There was an error downloading one or more of your donations: this list may not be complete.";
                        rtn.PeopleId = -3;
                        donations.Add(rtn);
                    }
                }
            }
                    




            return donations;
           
        }
        catch (Exception e)
        {
            
                Donation rtn = new Donation();
                StaffBrokerFunctions.EventLog("MPD-Service Error Downloading Donations for " + Username, e.Message, 1);

                rtn.DonorName = "There was an error downloading your transations from the remote server.";
                rtn.PeopleId = -2;
                donations.Add(rtn);
                return donations;
            
        }

    }

   
}

public class CSVHelper : List<string[]>
{
    protected string csv = string.Empty;
    protected string separator = ",";

    public CSVHelper(string csv, string separator = "\",\"")
    {
        this.csv = csv;
        this.separator = separator;
        var objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
        var PS = (DotNetNuke.Entities.Portals.PortalSettings)HttpContext.Current.Items["PortalSettings"];



        foreach (string line in Regex.Split(csv, System.Environment.NewLine).ToList().Where(s => !string.IsNullOrEmpty(s)))
        {
            

            string[] values = SplitCSV(line).ToArray();
            //string  str = "";
            for (int i = 0; i < values.Length; i++)
            {
                //Trim values
                values[i] = values[i].Trim(',').Trim('\"');
               // str += "Value[" + i + "]: " + values[i];
            }
           // objEventLog.AddLog("Account",values[9], PS, 0, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);


            this.Add(values);
        }
    }
    public static IEnumerable<string> SplitCSV(string csvString)
    {
        var sb = new StringBuilder();
        bool quoted = false;

        foreach (char c in csvString)
        {
            if (quoted)
            {
                if (c == '"')
                    quoted = false;
                else
                    sb.Append(c);
            }
            else
            {
                if (c == '"')
                {
                    quoted = true;
                }
                else if (c == ',')
                {
                    yield return sb.ToString();
                    sb.Length = 0;
                }
                else
                {
                    sb.Append(c);
                }
            }
        }

        if (quoted)
            throw new ArgumentException("csvString", "Unterminated quotation mark.");

        yield return sb.ToString();
    }
}