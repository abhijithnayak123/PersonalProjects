using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{
    public partial class DesktopWSImpl : ICustomerService
    {
		public Customer Lookup(long customerSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
			return DesktopEngine.Lookup(customerSessionId, alloyId, mgiContext);
        }

        public string Save(long agentSessionId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.Save(agentSessionId, prospect, mgiContext);
        }

        public void Save(long agentSessionId, long alloyId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            DesktopEngine.Save(agentSessionId, alloyId, prospect, mgiContext);
        }

		//public bool Activate(string sessionId, long alloyId, Dictionary<string, string> context)
        //{
		//    return DesktopEngine.Activate(sessionId, AlloyID, mgiContext);
        //}

        public List<CustomerSearchResult> SearchCustomers(long agentSessionId, CustomerSearchCriteria searchCriteria, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.SearchCustomers(agentSessionId, searchCriteria, mgiContext);
        }

        public CustomerSession InitiateCustomerSession(long agentSessionId, CustomerAuthentication authentication, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.InitiateCustomerSession(agentSessionId, authentication, mgiContext);
        }

        public Prospect GetProspect(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.GetProspect(agentSessionId, alloyId, mgiContext);
        }

        public void UpdateCustomer(long agentSessionId, long alloyId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            DesktopEngine.UpdateCustomer(agentSessionId, alloyId, prospect, mgiContext);
        }

        public string RecordIdentificationConfirmation(long customerSessionId, string agentId, bool IdentificationStatus, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.RecordIdentificationConfirmation(customerSessionId, agentId, IdentificationStatus, mgiContext);
        }

		public bool CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			return DesktopEngine.CanChangeProfileStatus(agentSessionId, userId, profileStatus, mgiContext);
		}

        public Customer Get(long agentSessionId, string Phone, string PIN, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.Get(agentSessionId, Phone, PIN, mgiContext);
        }

        public bool ValidateSSN(long agentSessionId, string SSN, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.ValidateSSN(agentSessionId, SSN, mgiContext);
        }

        public long GetAnonymousUserPAN(long agentSessionId, long channelPartnerId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.GetAnonymousUserPAN(agentSessionId, channelPartnerId, mgiContext);
        }

        /// <summary>
        ///  Customers Serach by Parameters
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="customerLookUpCriteria"></param>
        /// <returns></returns>
        public List<Prospect> CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.CustomerLookUp(agentSessionId, customerLookUpCriteria, mgiContext);
        }

        public void ValidateCustomerStatus(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            DesktopEngine.ValidateCustomerStatus(agentSessionId, alloyId, mgiContext);

        }

        public void NexxoActivate(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            DesktopEngine.NexxoActivate(agentSessionId, alloyId, mgiContext);
        }

        public void ClientActivate(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            DesktopEngine.ClientActivate(agentSessionId, alloyId, mgiContext);
        }

        public void UpdateCustomerToClient(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            DesktopEngine.UpdateCustomerToClient(agentSessionId, alloyId, mgiContext);
        }

        public void CustomerSyncInFromClient(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            DesktopEngine.CustomerSyncInFromClient(agentSessionId, alloyId, mgiContext);
        }


        public bool ValidateCustomer(long agentSessionId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            return DesktopEngine.ValidateCustomer(agentSessionId, prospect, mgiContext);
        }
    }
}

