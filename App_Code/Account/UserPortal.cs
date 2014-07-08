using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
//using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Account;

namespace AgapeConnect
{
    public class DSUserPortalView
    {
        public List<DataserverPortal> GetPortals(Guid _ssocode)
        {
            DataserverPortalUsersEntities dspuEntities = new DataserverPortalUsersEntities();

            List<DataserverPortal> portallist = new List<DataserverPortal>();

            IQueryable<Account.DataserverPortalUser> plist = from pl in dspuEntities.DataserverPortalUsers where pl.SsoCode == _ssocode select pl;

            foreach (Account.DataserverPortalUser p in plist)
            {
                portallist.Add(new DataserverPortal { InstanceUri = p.PortalUri, InstanceName = p.PortalName});
            }

            return portallist;
        }
    }
}