using System;
using System.Collections.Generic;
using System.Linq;
using StaffBroker;
using DotNetNuke;
using System.Net;
using System.Text.RegularExpressions;
using DotNetNuke.Services.FileSystem;

public static class MenuLinkType
{
    public const int URL = 0;
    public const int Page = 1;
    public const int Document = 2;
    public const int Folder = 3;
    public const int Resource = 4;
    public const int Conference = 5;
    public const int Story = 6;
    public const int Blank = 20;


    static public string TypeName(int TypeNo)
    {
        switch (TypeNo)
        {
            case 0: return "URL";
            case 1: return "Page";
            case 2: return "Document";
            case 3: return "Folder";
            case 4: return "Resource";
            case 5: return "Coference";
            case 6: return "Story";
            case 20: return "Blank";
            default: return "URL";
        }

    }
}
public static class CostCentreType
{
    public const int Department = 0;
    public const int Staff = 1;
    public const int Other = 2;
    static public string TypeName(int TypeNo)
    {
        switch (TypeNo)
        {
            case 0: return "Department";
            case 1: return "Staff";
            case 2: return "Other";
            default: return "Other";
        }

    }
}
public static class AccountType
{
    public const int Normal = 0;
    public const int AccountsReceivable = 1;
    public const int AccountsPayable = 2;
    public const int Income = 3;
    public const int Exspnse = 4;
    public const int Other = 5;
    static public string TypeName(int TypeNo)
    {
        switch (TypeNo)
        {
            case 0: return "Not Used";
            case 1: return "AccountsReceivable";
            case 2: return "AccountsPayable";
            case 3: return "Income";
            case 4: return "Expense";
            case 5: return "Other";
            default: return "Normal";
        }

    }
}
/// <summary>
/// Summary description for StaffBrokerFunctions
/// </summary>
/// 

public class StaffBrokerFunctions
{
    public struct LeaderRelationship
    {

        public int UserId { get; set; }

        public string UserName { get; set; }
        public int LeaderId { get; set; }
        public string LeaderName { get; set; }
        public int DelegateId { get; set; }
        public string Delegatename { get; set; }
    }
    public struct LeaderInfo
    {

        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public Boolean isDelegate;
        public Boolean hasDelegated;
    }
    public StaffBrokerFunctions()
    {
    }


    static public void EventLog(string title, string message, int userid)
    {

        DotNetNuke.Entities.Portals.PortalSettings PS = (DotNetNuke.Entities.Portals.PortalSettings)System.Web.HttpContext.Current.Items["PortalSettings"];
        DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();

        objEventLog.AddLog(title, message, PS, userid, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);


    }


    #region Department functions

    static public Boolean IsDept(int PortalId, string costCenter)
    {
        DotNetNuke.Entities.Portals.PortalSettings PS = (DotNetNuke.Entities.Portals.PortalSettings)System.Web.HttpContext.Current.Items["PortalSettings"];
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        if (GetSetting("NonDynamics", PS.PortalId) == "True")
        {
            return (from c in d.AP_StaffBroker_Departments where c.CostCentre.ToLower() == costCenter.ToLower() && c.PortalId == PS.PortalId select c.CostCenterId).Count() > 0;

        }
        else
        {



            var cc = from c in d.AP_StaffBroker_CostCenters where c.CostCentreCode == costCenter select c.Type;
            if (cc.Count() > 0)
                return cc.First() == CostCentreType.Department;

            else return false;
        }
    }

    static public String GetDeptGiveToURL(int PortalId, int costCenter)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var cc = from c in d.AP_StaffBroker_Departments where c.CostCenterId == costCenter && c.PortalId == PortalId select c.GivingShortcut;
        if (cc.Count() > 0)
            return cc.First();

        else return "";
    }

    static public string GetDeptPhoto(int deptID)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var cc = from c in d.AP_StaffBroker_Departments where c.CostCenterId == deptID select c.PhotoId;
        String fileId = cc.First().ToString();

        if (fileId == null || fileId.Equals(""))
        {
            return "/images/no_avatar.gif";
        }
        else
        {
            DotNetNuke.Services.FileSystem.IFileInfo theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(fileId));
            return DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(theFile);
        }
    }

    static public AP_StaffBroker_Department GetDeptByGivingShortcut(String GivingShortcut)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var depts = from c in d.AP_StaffBroker_Departments where c.GivingShortcut == GivingShortcut select c;
        if (depts.Count() > 0)
        {
            return depts.First();
        }
        else
        {
            return null;
        }
    }

    #endregion





    public static decimal CurrencyConvert(decimal amount, string fromCurrency, string toCurrency)
    {
        string mode = "Yahoo";
        WebClient web = new WebClient();

        if (mode == "Google")
        {

            // string url = string.Format("http://www.google.com/ig/calculator?hl=en&q={2}{0}%3D%3F{1}", fromCurrency.ToUpper(), toCurrency.ToUpper(), amount);

            //string response = web.DownloadString(url);  


            //                decimal rate = System.Convert.ToDecimal(response);

            return 1;
        }
        else if (mode == "Yahoo")
        {
            string url = string.Format("http://download.finance.yahoo.com/d/quotes.csv?s={0}{1}=X&f=l1", fromCurrency.ToUpper(), toCurrency.ToUpper(), amount);

            string response = web.DownloadString(url);
            decimal rate = System.Convert.ToDecimal(response);

            return rate * amount;

        }
        return 1;
    }

    static public Boolean VerifyCostCenter(int PortalId, string costCenter)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var cc = from c in d.AP_StaffBroker_CostCenters where c.CostCentreCode == costCenter select c;
        return cc.Count() > 0;
    }

    static public Boolean VerifyAccountCode(int PortalId, string AccountCode)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var acc = from c in d.AP_StaffBroker_AccountCodes where c.AccountCode == AccountCode select c;
        return acc.Count() > 0;
    }

    static public AP_StaffBroker_Staff CreateStaffMember(int PortalId, DotNetNuke.Entities.Users.UserInfo User1in, DotNetNuke.Entities.Users.UserInfo User2in, short staffTypeIn)
    {
        //Create Married Staff


        DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
        if (rc.GetRoleByName(PortalId, "Staff") == null)
        {
            DotNetNuke.Security.Roles.RoleInfo insert = new DotNetNuke.Security.Roles.RoleInfo();
            insert.Description = "Staff Members";
            insert.RoleName = "Staff";
            insert.AutoAssignment = false;
            insert.IsPublic = false;
            insert.RoleGroupID = -1;
            insert.PortalID = PortalId;
            rc.AddRole(insert);
        }

        rc.AddUserRole(PortalId, User1in.UserID, rc.GetRoleByName(PortalId, "Staff").RoleID, DateTime.MaxValue);
        rc.AddUserRole(PortalId, User2in.UserID, rc.GetRoleByName(PortalId, "Staff").RoleID, DateTime.MaxValue);



        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var searchStaff = from c in d.AP_StaffBroker_Staffs where c.Active && (c.UserId1 == User1in.UserID || c.UserId2 == User1in.UserID || c.UserId1 == User2in.UserID || c.UserId2 == User2in.UserID) select c;
        if (searchStaff.Count() > 0)
            return searchStaff.First();




        AP_StaffBroker_Staff rtn = new AP_StaffBroker_Staff();
        rtn.UserId1 = User1in.UserID;
        rtn.UserId2 = User2in.UserID;
        rtn.PortalId = PortalId;
        rtn.Active = true;
        rtn.DisplayName = User1in.FirstName + " & " + User2in.FirstName + " " + User1in.LastName;

        rtn.StaffTypeId = staffTypeIn;
        rtn.CostCenter = "";

        d.AP_StaffBroker_Staffs.InsertOnSubmit(rtn);
        d.SubmitChanges();




        return rtn;




    }

    static public void ChangeUsername(string OldUsername, string NewUsername)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();

        d.Agape_Main_AlterUserName(OldUsername, NewUsername);

    }

    static public String ValidateAccountCode(string AccountCode, int PortalId)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        return (from c in d.AP_StaffBroker_AccountCodes where c.AccountCode == AccountCode && c.PortalId == PortalId select c).Count() > 0 ? AccountCode : "";

    }
    static public String ValidateCostCenter(string CostCenter, int PortalId)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        return (from c in d.AP_StaffBroker_CostCenters where c.CostCentreCode == CostCenter && c.PortalId == PortalId select c).Count() > 0 ? CostCenter : "";



    }
    static public AP_StaffBroker_Department CreateDept(string Name, string RC, int manager, int? del)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();

        DotNetNuke.Entities.Portals.PortalSettings PS = (DotNetNuke.Entities.Portals.PortalSettings)System.Web.HttpContext.Current.Items["PortalSettings"];
        if (del == 0) del = null;
        var q = from c in d.AP_StaffBroker_Departments where c.CostCentre == RC && c.PortalId == PS.PortalId select c;
        if (q.Count() > 0)
        {
            q.First().Name = Name;
            q.First().CostCentreManager = manager;
            q.First().CostCentreDelegate = del;
            d.SubmitChanges();
            return q.First();

        }
        else
        {
            AP_StaffBroker_Department insert = new AP_StaffBroker_Department();
            insert.PortalId = PS.PortalId;
            insert.Name = Name;
            insert.CostCentre = RC;
            insert.CostCentreManager = manager;
            insert.CanRmb = true;
            insert.CanGiveTo = false;

            insert.CanCharge = false;
            insert.IsProject = false;
            insert.Spare1 = "False";
            insert.GivingText = "";
            insert.GivingShortcut = "";
            insert.PayType = "";


            insert.CostCentreDelegate = del;

            d.AP_StaffBroker_Departments.InsertOnSubmit(insert);
            d.SubmitChanges();
            return insert;
        }

    }

    static public AP_StaffBroker_Staff CreateStaffMember(int PortalId, DotNetNuke.Entities.Users.UserInfo User1in, short staffTypeIn = 1)
    {

        DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
        if (rc.GetRoleByName(PortalId, "Staff") == null)
        {
            DotNetNuke.Security.Roles.RoleInfo insert = new DotNetNuke.Security.Roles.RoleInfo();
            insert.Description = "Staff Members";
            insert.RoleName = "Staff";
            insert.AutoAssignment = false;
            insert.IsPublic = false;
            insert.RoleGroupID = -1;
            insert.PortalID = PortalId;
            rc.AddRole(insert);
        }

        rc.AddUserRole(PortalId, User1in.UserID, rc.GetRoleByName(PortalId, "Staff").RoleID, DateTime.MaxValue);


        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var searchStaff = from c in d.AP_StaffBroker_Staffs where c.Active && (c.UserId1 == User1in.UserID || c.UserId2 == User1in.UserID) select c;
        if (searchStaff.Count() > 0)
            return searchStaff.First();
        //Create Single Staff
        AP_StaffBroker_Staff rtn = new AP_StaffBroker_Staff();
        rtn.UserId1 = User1in.UserID;
        rtn.UserId2 = -2;

        rtn.DisplayName = User1in.FirstName + " " + User1in.LastName;

        rtn.StaffTypeId = staffTypeIn;
        rtn.CostCenter = "";
        rtn.PortalId = PortalId;
        rtn.Active = true;
        d.AP_StaffBroker_Staffs.InsertOnSubmit(rtn);
        d.SubmitChanges();


        return rtn;
    }
    static public AP_StaffBroker_Staff CreateStaffMember(int PortalId, DotNetNuke.Entities.Users.UserInfo User1in, string SpouseName, DateTime SpouseDOB, short staffTypeIn = 1)
    {
        DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
        if (rc.GetRoleByName(PortalId, "Staff") == null)
        {
            DotNetNuke.Security.Roles.RoleInfo insert = new DotNetNuke.Security.Roles.RoleInfo();
            insert.Description = "Staff Members";
            insert.RoleName = "Staff";
            insert.AutoAssignment = false;
            insert.IsPublic = false;
            insert.RoleGroupID = -1;
            insert.PortalID = PortalId;
            rc.AddRole(insert);
        }

        rc.AddUserRole(PortalId, User1in.UserID, rc.GetRoleByName(PortalId, "Staff").RoleID, DateTime.MaxValue);



        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var searchStaff = from c in d.AP_StaffBroker_Staffs where c.Active && (c.UserId1 == User1in.UserID || c.UserId2 == User1in.UserID) select c;
        if (searchStaff.Count() > 0)
            return searchStaff.First();
        //Create Married to Non-Staff
        AP_StaffBroker_Staff rtn = new AP_StaffBroker_Staff();
        rtn.UserId1 = User1in.UserID;
        rtn.UserId2 = -1;
        rtn.DisplayName = User1in.FirstName + " " + User1in.LastName;

        rtn.StaffTypeId = staffTypeIn;
        rtn.CostCenter = "";
        rtn.PortalId = PortalId;
        rtn.Active = true;
        d.AP_StaffBroker_Staffs.InsertOnSubmit(rtn);
        d.SubmitChanges();
        //Now add Spouse data
        AddProfileValue(PortalId, rtn.StaffId, "SpouseDOB", SpouseDOB.ToShortDateString());
        AddProfileValue(PortalId, rtn.StaffId, "SpouseName", SpouseName);



        return rtn;
    }

    static public void AddProfileValue(int PortalId, int staffId, string propertyName, string propertyValue, int Type = 0)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        AP_StaffBroker_StaffProfile insert = new AP_StaffBroker_StaffProfile();
        var definition = (from c in d.AP_StaffBroker_StaffPropertyDefinitions where c.PropertyName == propertyName && c.PortalId == PortalId select c.StaffPropertyDefinitionId);
        if (definition.Count() == 0)
        {
            AP_StaffBroker_StaffPropertyDefinition newDef = new AP_StaffBroker_StaffPropertyDefinition();
            newDef.PortalId = PortalId;
            newDef.PropertyName = propertyName;
            newDef.Display = false;
            newDef.ViewOrder = (short?)((from c in d.AP_StaffBroker_StaffPropertyDefinitions where c.PortalId == PortalId select c.ViewOrder).Max() + 1);
            newDef.Type = Convert.ToByte(Type);


            newDef.PropertyHelp = "";
            d.AP_StaffBroker_StaffPropertyDefinitions.InsertOnSubmit(newDef);
            d.SubmitChanges();
            insert.StaffPropertyDefinitionId = newDef.StaffPropertyDefinitionId;
        }
        else
            insert.StaffPropertyDefinitionId = definition.First();

        insert.StaffId = staffId;
        insert.PropertyValue = propertyValue;
        var existing = from c in d.AP_StaffBroker_StaffProfiles where c.StaffId == insert.StaffId && c.StaffPropertyDefinitionId == insert.StaffPropertyDefinitionId select c;

        if (existing.Count() > 0)
            existing.First().PropertyValue = propertyValue;
        else
            d.AP_StaffBroker_StaffProfiles.InsertOnSubmit(insert);

        d.SubmitChanges();

    }

    static public string GetStaffProfileProperty(int staffId, string propertyName)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var existing = from c in d.AP_StaffBroker_StaffProfiles where c.StaffId == staffId && c.AP_StaffBroker_StaffPropertyDefinition.PropertyName == propertyName select c.PropertyValue;

        if (existing.Count() == 0)
            return "";
        else
            return existing.First();

    }

    static public string GetStaffProfileProperty(ref AP_StaffBroker_Staff Staffin, string propertyName)
    {
        var existing = from c in Staffin.AP_StaffBroker_StaffProfiles where c.AP_StaffBroker_StaffPropertyDefinition.PropertyName == propertyName select c.PropertyValue;
        if (existing.Count() == 0)
            return "";
        else
            return existing.First();

    }

    static public string GetStaffJointPhoto(int staffID)
    {
        String fileId = GetStaffProfileProperty(staffID, "JointPhoto");

        if (fileId == null || fileId.Equals("") || (GetStaffProfileProperty(staffID, "UnNamedStaff") == "True"))
        {
            return "/images/no_avatar.gif";
        }
        else
        {
            DotNetNuke.Services.FileSystem.IFileInfo theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(fileId));
            return DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(theFile);
        }
    }

    static public AP_StaffBroker_Staff GetStaffbyStaffId(int StaffId)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var q = from c in d.AP_StaffBroker_Staffs where c.StaffId == StaffId select c;
        if (q.Count() > 0)
            return q.First();
        else
            return null;

    }



    static public AP_StaffBroker_Staff GetStaffMember(int UserId)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var q = from c in d.AP_StaffBroker_Staffs where c.UserId1 == UserId || c.UserId2 == UserId select c;

        if (q.Count() > 0)
            return q.First();
        else
            return null;



    }

    static public DotNetNuke.Entities.Users.UserInfo CreateUser(int PortalId, String GCXUsername, String FirstName, String LastName)
    {
        DotNetNuke.Security.Membership.UserCreateStatus objUserCreateStatus;
        DotNetNuke.Entities.Users.UserInfo objUserInfo = new DotNetNuke.Entities.Users.UserInfo();


        objUserInfo.FirstName = FirstName;
        objUserInfo.LastName = LastName;
        objUserInfo.DisplayName = FirstName + " " + LastName;
        objUserInfo.Username = GCXUsername + PortalId.ToString();
        objUserInfo.PortalID = PortalId;
        objUserInfo.Membership.Password = DotNetNuke.Entities.Users.UserController.GeneratePassword(8);
        objUserInfo.Email = GCXUsername;
        objUserCreateStatus = DotNetNuke.Entities.Users.UserController.CreateUser(ref objUserInfo);

        if (objUserCreateStatus == DotNetNuke.Security.Membership.UserCreateStatus.Success || objUserCreateStatus == DotNetNuke.Security.Membership.UserCreateStatus.UsernameAlreadyExists || objUserCreateStatus == DotNetNuke.Security.Membership.UserCreateStatus.UserAlreadyRegistered)
            return objUserInfo;
        else
            return null;

    }
    static public IQueryable<StaffBroker.User> GetStaff(params int[] ExcludeIDs)
    {
        DotNetNuke.Entities.Portals.PortalSettings PS = (DotNetNuke.Entities.Portals.PortalSettings)System.Web.HttpContext.Current.Items["PortalSettings"];

        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var q = from c in d.Users where c.AP_StaffBroker_Staffs.Active && c.AP_StaffBroker_Staffs.PortalId == PS.PortalId && !(ExcludeIDs.Contains(c.UserID)) select c;
        q = q.Union(from c in d.Users join b in d.AP_StaffBroker_Staffs on c.UserID equals b.UserId2 where b.Active && b.PortalId == PS.PortalId && !(ExcludeIDs.Contains(c.UserID)) select c);
        return q.OrderBy(c => c.LastName).ThenBy(c => c.FirstName);
    }
    static public IQueryable<StaffBroker.User> GetStaffExcl(int PortalId = 0, params string[] ExcludeTypes)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var q = from c in d.Users where c.AP_StaffBroker_Staffs.Active && c.AP_StaffBroker_Staffs.PortalId == PortalId && !(ExcludeTypes.Contains(c.AP_StaffBroker_Staffs.AP_StaffBroker_StaffType.Name)) select c;
        q = q.Union(from c in d.Users join b in d.AP_StaffBroker_Staffs on c.UserID equals b.UserId2 where b.Active && b.PortalId == PortalId && !(ExcludeTypes.Contains(b.AP_StaffBroker_StaffType.Name)) select c);
        return q.OrderBy(c => c.LastName).ThenBy(c => c.FirstName);
    }
    static public IQueryable<StaffBroker.User> GetStaffIncl(int PortalId = 0, params string[] IncludeTypes)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var q = from c in d.Users where c.AP_StaffBroker_Staffs.Active && c.AP_StaffBroker_Staffs.PortalId == PortalId && (IncludeTypes.Contains(c.AP_StaffBroker_Staffs.AP_StaffBroker_StaffType.Name)) select c;
        q = q.Union(from c in d.Users join b in d.AP_StaffBroker_Staffs on c.UserID equals b.UserId2 where b.Active && b.PortalId == PortalId && (IncludeTypes.Contains(b.AP_StaffBroker_StaffType.Name)) select c);
        return q.OrderBy(c => c.LastName).ThenBy(c => c.FirstName);
    }

    static public List<LeaderRelationship> GetReportsTo(int UserId)
    {

        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var s = from c in d.AP_StaffBroker_LeaderMetas where c.UserId == UserId select c;
        List<LeaderRelationship> rtn = new List<LeaderRelationship>();
        foreach (StaffBroker.AP_StaffBroker_LeaderMeta row in s)
        {
            LeaderRelationship x = new LeaderRelationship();
            x.UserId = row.UserId;
            x.UserName = row.User.DisplayName;
            x.LeaderId = row.LeaderId;
            try
            {
                x.LeaderName = row.Leaders.DisplayName;
            }
            catch (Exception)
            {
                x.LeaderName = "Unknown";

            }

            x.DelegateId = -1;
            x.Delegatename = "";
            if (row.DelegateId != null)
            {
                x.DelegateId = (int)row.DelegateId;
                x.Delegatename = row.Delegate.DisplayName;
            }

            rtn.Add(x);
        }

        return rtn;

    }

    static public List<User> GetTeam(int LeaderId)
    {

        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var s = from c in d.AP_StaffBroker_LeaderMetas where c.LeaderId == LeaderId || c.DelegateId == LeaderId select c.User;


        return s.ToList();

    }

    static public List<LeaderInfo> GetLeadersDetailed(int UserId, int PortalId)
    {

        StaffBrokerDataContext d = new StaffBrokerDataContext();



        var s = from c in d.AP_StaffBroker_LeaderMetas where c.UserId == UserId select new { UserId = c.LeaderId, hasDelegated = c.DelegateId != null, isDelegate = false };

        s = s.Union(from c in d.AP_StaffBroker_LeaderMetas where (c.UserId == UserId) && !(c.DelegateId == null) select new { UserId = (int)c.DelegateId, hasDelegated = false, isDelegate = true });

        List<LeaderInfo> rtn = new List<LeaderInfo>();

        foreach (var row in s)
        {
            LeaderInfo insert = new LeaderInfo();
            insert.UserId = row.UserId;
            insert.DisplayName = DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, row.UserId).DisplayName;
            insert.isDelegate = row.isDelegate;
            insert.hasDelegated = row.hasDelegated;
            rtn.Add(insert);
        }
        return rtn;

    }

    static public List<int> GetLeaders(int UserId, Boolean includeDelegates = true)
    {

        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var s = from c in d.AP_StaffBroker_LeaderMetas where c.UserId == UserId select c.LeaderId;
        if (includeDelegates)
            s = s.Union(from c in d.AP_StaffBroker_LeaderMetas where (c.UserId == UserId) && !(c.DelegateId == null) select (int)c.DelegateId);

        return s.ToList();

    }

    static public List<AP_StaffBroker_Department> GetDepartments(int UserId)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();

        List<AP_StaffBroker_Department> q;
        q = (from c in d.AP_StaffBroker_Departments where c.CostCentreManager == UserId || c.CostCentreDelegate == UserId select c).ToList();

        return q;
    }

    static public string GetFormattedCurrency(int PortalId, string Value)
    {
        string cur = GetSetting("Currency", PortalId);
        if (GetSetting("CurrencyLocation", PortalId) == "Right")
        {
            return Value + " " + cur;
        }
        else
        {

            return cur + Value;
        }



    }
    static public int GetSpouseId(int UserId)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();
        var q = from c in d.AP_StaffBroker_Staffs where c.UserId1 == UserId select c.UserId2;
        if (q.Count() > 0)
            return (int)(q.First());
        else
        {
            var r = from c in d.AP_StaffBroker_Staffs where c.UserId2 == UserId select c.UserId1;
            if (r.Count() > 0)
                return r.First();
            else
                return -1;

        }

    }


    static public string GetTemplate(string templateName, int PortalId)
    {
        TemplatesDataContext d = new TemplatesDataContext();
        var q = from c in d.AP_StaffBroker_Templates where c.PortalId == PortalId && c.TemplateName == templateName select c.TemplateHTML;
        if (q.Count() == 0)
        {

            var def = from c in d.AP_StaffBroker_Templates where c.PortalId == null && c.TemplateName == templateName select c;

            if (def.Count() > 0)
            {
                AP_StaffBroker_Template insert = new AP_StaffBroker_Template();
                insert.PortalId = PortalId;
                insert.TemplateDescription = def.First().TemplateDescription;
                insert.TemplateHTML = def.First().TemplateHTML;
                insert.TemplateName = def.First().TemplateName;
                d.AP_StaffBroker_Templates.InsertOnSubmit(insert);
                d.SubmitChanges();

                q = from c in d.AP_StaffBroker_Templates where c.PortalId == PortalId && c.TemplateName == templateName select c.TemplateHTML;

            }
        }

        if (q.Count() > 0)
        {
            var PS = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings();
            string Ministry = PS.PortalName;

            string root = DotNetNuke.Common.Globals.NavigateURL(PS.HomeTabId);
            root = root.Substring(0, root.IndexOf("/"));
            string logo = "";
            if (PS.LogoFile.Contains("http"))
                logo = PS.LogoFile;
            else
                logo = "http://" + PS.PortalAlias.HTTPAlias + PS.HomeDirectory + PS.LogoFile;
            string loginURL = DotNetNuke.Common.Globals.NavigateURL(PS.LoginTabId);



            return q.First().Replace("[MINISTRY]", Ministry).Replace("[LOGOURL]", logo).Replace("[LOGINURL]", loginURL);
        }


        else
            return "";

    }



    static public void SetSetting(string SettingName, string value, int portalId)
    {
        StaffBrokerDataContext d = new StaffBrokerDataContext();

        var q = from c in d.AP_StaffBroker_Settings where c.SettingName == SettingName && c.PortalId == portalId select c;

        if (q.Count() == 0)
        {
            AP_StaffBroker_Setting insert = new AP_StaffBroker_Setting();
            insert.SettingName = SettingName;
            insert.SettingValue = value;
            insert.PortalId = portalId;
            d.AP_StaffBroker_Settings.InsertOnSubmit(insert);

        }
        else
        {
            q.First().SettingValue = value;
        }

        d.SubmitChanges();

    }

    static public string GetSetting(string SettingName, int portalId)
    {
        try
        {
            StaffBrokerDataContext d = new StaffBrokerDataContext();

            var q = from c in d.AP_StaffBroker_Settings where c.SettingName == SettingName && c.PortalId == portalId select c;

            if (q.Count() == 0)
                return "";
            else if (q.First().SettingValue == null)
                return "";
            else
                return q.First().SettingValue;
        }
        catch (Exception)
        {

            return "";
        }


    }


    static public void SetUserProfileProperty(int PortalId, int UserId, String PropertyName, String PropertyValue, int DataType = 349)
    {
        var pd = DotNetNuke.Entities.Profile.ProfileController.GetPropertyDefinitionByName(PortalId, PropertyName);
        if (pd == null)
        {
            DotNetNuke.Entities.Profile.ProfilePropertyDefinition insert = new DotNetNuke.Entities.Profile.ProfilePropertyDefinition(PortalId);
            insert.DefaultValue = "";
            insert.Deleted = false;
            insert.DataType = DataType;
            insert.PropertyCategory = "Other";
            insert.PropertyName = PropertyName;
            insert.Length = 50;
            insert.Required = false;
            insert.ViewOrder = 200;
            insert.Visible = true;

            DotNetNuke.Entities.Profile.ProfileController.AddPropertyDefinition(insert);
        }

        DotNetNuke.Entities.Users.UserInfo theUser = DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, UserId);
        theUser.Profile.SetProfileProperty(PropertyName, PropertyValue);
        DotNetNuke.Entities.Users.UserController.UpdateUser(PortalId, theUser);
    }

    static public string CreateUniqueFileName(IFolderInfo theFolder, string ext)
    {
        string allChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

        string uniqueCode = "";
        System.Text.StringBuilder str = new System.Text.StringBuilder();
        int xx;
        Random r = new Random();
        while (uniqueCode == "" || FileManager.Instance.FileExists(theFolder, uniqueCode == "" ? "X" : uniqueCode))
        {
            for (Byte i = 1; i <= 10; i++)
            {
                xx = r.Next(0, allChars.Length);
                str.Append(allChars.Trim()[xx]);

            }
            uniqueCode = str.ToString() + "." + ext;

        }
        return uniqueCode;

    }
    static public string GetStaffJointPhotoByFileId(int staffID, int fileId)
    {
        if (GetStaffProfileProperty(staffID, "UnNamedStaff") != "True")
        {
            // fileId may be 0 if no photo defined, then theFile would be null
            DotNetNuke.Services.FileSystem.IFileInfo theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(fileId);
            if (theFile != null)
                return DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(theFile);
        }
        return "/images/no_avatar.gif";
    }
    static public string GetDeptPhotoByFileId(int fileId)
    {
        // fileId may be 0 if no photo defined, then theFile would be null
        DotNetNuke.Services.FileSystem.IFileInfo theFile = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile((int)fileId);
        if (theFile != null)
            return DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(theFile);
        return "/images/no_avatar.gif";
    }

}