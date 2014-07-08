using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StaffRmb
/// </summary>
/// 
namespace StaffRmb
{
    public static class RmbReceiptMode
    {
        public const int Disabled = 0;
        public const int NoVAT = 1;
        public const int VAT = 2;
        static public string ModeName(int ModeNo)
        {
            switch (ModeNo)
            {
                case 0: return "Disabled";
                case 1: return "NoVAT";
                case 2: return "VAT";
                default: return "Unknown";
            }

        }
    }
    public static class RmbIncomeType
    {
        public const int Donation = 0;
        public const int Conference = 1;
        public const int Other = 2;
        static public string IncomeTypeName(int IncomeTypeNo)
        {
            switch (IncomeTypeNo)
            {
                case 0: return "Donation";
                case 1: return "Conference";
                case 2: return "Other";
                default: return "Unknown";
            }

        }
    }
    public static class RmbStatus
    {
        public const int Draft = 0;
        public const int Submitted = 1;
        public const int Approved = 2;
        public const int Processed = 3;
        public const int Cancelled = 4;
        public const int MoreInfo = 5;
        public const int PendingDownload = 10;
        public const int DownloadFailed = 20;
        static public string StatusName(int StatusNo)
        {
            switch (StatusNo)
            {
                case 0: return "Draft";
                case 1: return "Submitted";
                case 2: return "Approved";
                case 3: return "Processed";
                case 4: return "Cancelled";
                case 5: return "MoreInfo";
                case 10: return "PendingDownload";
                case 20: return "DownloadFailed";
                

                default: return "Unknown";
            }

        }
    }
    public static class RmbAccess
    {
        public const int Denied = 0;
        public const int Owner = 1;
        public const int Spouse = 2;
        public const int Approver = 3;
        public const int Leader = 4;
        public const int Accounts = 5;


        static public string StatusName(int StatusNo)
        {
            switch (StatusNo)
            {
                case 0: return "Denied";
                case 1: return "Owner";
                case 2: return "Spouse";
                case 3: return "Approver";
                case 4: return "Leader";
                case 5: return "Accounts";

                default: return "Denied";
            }

        }

    }
    public class StaffRmbFunctions
    {
        public struct Approvers
        {
            public List<DotNetNuke.Entities.Users.UserInfo> UserIds;
            public Boolean CCMSpecial, SpouseSpecial, AmountSpecial, isDept;
            public string Name;
        }
        public StaffRmbFunctions()
        {
        }



        static public Approvers getApprovers(AP_Staff_Rmb rmb, DotNetNuke.Entities.Users.UserInfo authUser, DotNetNuke.Entities.Users.UserInfo authAuthUser)
        {
            StaffBroker.StaffBrokerDataContext dStaff = new StaffBroker.StaffBrokerDataContext();
            Approvers rtn = new Approvers();

            var st = StaffBrokerFunctions.GetStaffMember(rmb.UserId);
            rtn.Name = st.DisplayName;
            int SpouseId = StaffBrokerFunctions.GetSpouseId(rmb.UserId);
            rtn.AmountSpecial = (from c in rmb.AP_Staff_RmbLines where c.LargeTransaction == true select c).Count() > 0;
            rtn.isDept = (rmb.CostCenter != st.CostCenter);
            rtn.SpouseSpecial = false;
            rtn.UserIds = new List<DotNetNuke.Entities.Users.UserInfo>();
            if (rtn.isDept)
            {
                var cc = from c in dStaff.AP_StaffBroker_Departments where (c.CostCentre == rmb.CostCenter) && c.PortalId == rmb.PortalId select c;
                rtn.CCMSpecial = (from c in cc
                                  where ((c.CostCentreManager == null && c.CostCentreDelegate == null) == false) &&
                                      (
                                      ((c.CostCentreManager != rmb.UserId) && (c.CostCentreManager != SpouseId)) ||
                                      ((c.CostCentreDelegate != rmb.UserId) && (c.CostCentreDelegate != SpouseId))

                                      )
                                  select c.CostCenterId).Count() == 0;
                
                if(rtn.CCMSpecial && !(cc.First().CostCentreManager==null && cc.First().CostCentreDelegate==null))
                {
                    //Try to add the CCM's own Team Leader instead of auth User
                    var app2 = StaffBrokerFunctions.GetLeaders(rmb.UserId, true);
                    foreach (int i in (from c in app2 where c != rmb.UserId && c != SpouseId select c))
                        rtn.UserIds.Add(DotNetNuke.Entities.Users.UserController.GetUserById(rmb.PortalId, i));

                }
                if ((rtn.CCMSpecial && rtn.UserIds.Count()==0 )|| rtn.AmountSpecial || rtn.SpouseSpecial)
                {
                    rtn.UserIds.Clear();
                    rtn.UserIds.Add(authUser.UserID == rmb.UserId ? authAuthUser : authUser);

                    if (cc.First().CostCentreManager == rtn.UserIds.First().UserID || cc.First().CostCentreDelegate == rtn.UserIds.First().UserID)
                    {
                        rtn.AmountSpecial = false;
                        rtn.CCMSpecial = false;
                    }

                   
                }
                else
                {

                    if (cc.First().CostCentreManager != rmb.UserId && cc.First().CostCentreManager != SpouseId && cc.First().CostCentreManager != null)
                        rtn.UserIds.Add(DotNetNuke.Entities.Users.UserController.GetUserById(rmb.PortalId, (int)cc.First().CostCentreManager));
                    if (cc.First().CostCentreDelegate != rmb.UserId && cc.First().CostCentreDelegate != SpouseId && cc.First().CostCentreDelegate != null)
                        rtn.UserIds.Add(DotNetNuke.Entities.Users.UserController.GetUserById(rmb.PortalId, (int)cc.First().CostCentreDelegate));

                }
                if (cc.Count() > 0)
                    rtn.Name = cc.First().Name;

            }
            else
            {
                rtn.CCMSpecial = false;
                var app2 = StaffBrokerFunctions.GetLeaders(rmb.UserId, true);
                rtn.SpouseSpecial = (app2.Count() == 1 && ((app2.First() == SpouseId) || (app2.First() == rmb.UserId)));
                if (rtn.AmountSpecial || rtn.SpouseSpecial || app2.Count() == 0)
                {
                    rtn.UserIds.Add(authUser.UserID == rmb.UserId ? authAuthUser : authUser);
                   
                    if (app2.Contains(rtn.UserIds.First().UserID))
                    {
                        rtn.AmountSpecial = false;
                    }
                }
                else
                {
                    foreach (int i in (from c in app2 where c != rmb.UserId && c != SpouseId select c))
                        rtn.UserIds.Add(DotNetNuke.Entities.Users.UserController.GetUserById(rmb.PortalId, i));
                }

            }

            if(rtn.UserIds.Count()==0)
                rtn.UserIds.Add(authUser.UserID == rmb.UserId ? authAuthUser : authUser);



            return rtn;
        }

        static public Approvers getAdvApprovers(AP_Staff_AdvanceRequest  adv, double LargeTransaction, DotNetNuke.Entities.Users.UserInfo authUser, DotNetNuke.Entities.Users.UserInfo authAuthUser)
        {
            StaffBroker.StaffBrokerDataContext dStaff = new StaffBroker.StaffBrokerDataContext();
            Approvers rtn = new Approvers();

            var st = StaffBrokerFunctions.GetStaffMember((int)adv.UserId );
            rtn.Name = st.DisplayName;
            int SpouseId = StaffBrokerFunctions.GetSpouseId((int)adv.UserId);
            rtn.AmountSpecial = ((double)adv.RequestAmount)>LargeTransaction ;
           
            rtn.SpouseSpecial = false;
            rtn.UserIds = new List<DotNetNuke.Entities.Users.UserInfo>();
            
            var app2 = StaffBrokerFunctions.GetLeaders((int)adv.UserId, true);
            rtn.SpouseSpecial = (app2.Count() == 1 && ((app2.First() == SpouseId) || (app2.First() == (int)adv.UserId)));
            if (rtn.AmountSpecial || rtn.SpouseSpecial || app2.Count() == 0)
            {
                rtn.UserIds.Add(authUser.UserID == adv.UserId ? authAuthUser : authUser);

                if (app2.Contains((authUser.UserID == adv.UserId ? (authAuthUser.UserID) : authUser.UserID)))
                {
                    rtn.AmountSpecial = false;
                }
            }
            else
            {
                foreach (int i in (from c in app2 where c != adv.UserId && c != SpouseId select c))
                    rtn.UserIds.Add(DotNetNuke.Entities.Users.UserController.GetUserById(adv.PortalId, i));
            }

         

               

            return rtn;
        }


        static public int GetNewRID(int PortalId)
        {
            string NextRID = StaffBrokerFunctions.GetSetting("NextRID", PortalId);
            if (NextRID == "")
            {
                StaffRmbDataContext d = new StaffRmbDataContext();
                var MaxRID = (from c in d.AP_Staff_Rmbs where c.PortalId == PortalId select c.RID);
                if (MaxRID.Count() == 0)
                {
                    StaffBrokerFunctions.SetSetting("NextRID", "2", PortalId);
                    return 1;

                }
                else
                {
                    StaffBrokerFunctions.SetSetting("NextRID", (MaxRID.Max() + 1).ToString(), PortalId);
                    return MaxRID.First();
                }
            }
            else
            {

                StaffBrokerFunctions.SetSetting("NextRID",  (Convert.ToInt32(NextRID) + 1).ToString(), PortalId);
                return Convert.ToInt32( NextRID);
            }        
        }

        static public int GetNewAdvId(int PortalId)
        {
            string NextAdvID = StaffBrokerFunctions.GetSetting("NextAdvID", PortalId);
            if (NextAdvID == "")
            {
                StaffRmbDataContext d = new StaffRmbDataContext();
                var MaxAdvID = (from c in d.AP_Staff_AdvanceRequests  where c.PortalId == PortalId select c.LocalAdvanceId);
                if (MaxAdvID.Count() == 0)
                {
                    StaffBrokerFunctions.SetSetting( "NextAdvID",  "2", PortalId);
                    return 1;

                }
                else
                {
                    StaffBrokerFunctions.SetSetting("NextAdvID", (MaxAdvID.Max() + 1).ToString(), PortalId);
                    return (int) MaxAdvID.First();
                }
            }
            else
            {

                StaffBrokerFunctions.SetSetting("NextAdvID", (Convert.ToInt32(NextAdvID) + 1).ToString(), PortalId);
                return Convert.ToInt32(NextAdvID);
            }
        }
        static public int Authenticate(int UserId, int RmbNo, int PortalId )
        {
            StaffRmbDataContext d = new StaffRmbDataContext();
            var rmb = from c in d.AP_Staff_Rmbs where c.RMBNo == RmbNo  && c.PortalId == PortalId  select c;

            if (rmb.Count() > 0)
            {
                if (rmb.First().UserId == UserId)
                    return RmbAccess.Owner;
                else if (rmb.First().ApprUserId == UserId)
                    return RmbAccess.Approver;
                else
                {
                    var spouseId = StaffBrokerFunctions.GetSpouseId(UserId);
                    if (rmb.First().UserId == spouseId)
                        return RmbAccess.Spouse;
                    else
                    {
                        var team = from c in StaffBrokerFunctions.GetTeam(UserId) select c.UserID;
                        string pcc = StaffBrokerFunctions.GetStaffMember(UserId).CostCenter;
                        if (rmb.First().CostCenter == pcc)
                        {
                            if (team.Contains(rmb.First().UserId))
                                return RmbAccess.Approver;

                        }
                        else
                        {
                            var depts = from c in StaffBrokerFunctions.GetDepartments(UserId) select c.CostCentre;

                            if (depts.Contains(rmb.First().CostCenter))
                                return RmbAccess.Approver;
                            else if (team.Contains(rmb.First().UserId))
                                return RmbAccess.Leader;
                        }





                    }

                }

            }
            return RmbAccess.Denied;
        }


        static public int AuthenticateAdv(int UserId, int AdvanceId, int PortalId)
        {
            StaffRmbDataContext d = new StaffRmbDataContext();
            var adv = from c in d.AP_Staff_AdvanceRequests where c.AdvanceId == AdvanceId && c.PortalId == PortalId  select c;

            if (adv.Count() > 0)
            {
                if (adv.First().UserId == UserId)
                    return RmbAccess.Owner;
                else if (adv.First().ApproverId  == UserId)
                    return RmbAccess.Approver;
                else
                {
                    var spouseId = StaffBrokerFunctions.GetSpouseId(UserId);
                    if (adv.First().UserId == spouseId)
                        return RmbAccess.Spouse;
                    else
                    {
                        var team = from c in StaffBrokerFunctions.GetTeam(UserId) select c.UserID;
                        
                            if (team.Contains((int)(adv.First().UserId)) )
                                return RmbAccess.Approver;

                    }

                }

            }
            return RmbAccess.Denied;
        }

    }
}