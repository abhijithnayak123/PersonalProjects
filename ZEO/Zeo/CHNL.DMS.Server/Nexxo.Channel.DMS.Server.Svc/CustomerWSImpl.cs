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
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
    public partial class DesktopWSImpl : ICustomerService
    {
        public Response Lookup(long customerSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.Lookup(customerSessionId, alloyId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response Save(long agentSessionId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.Save(agentSessionId, prospect, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response Save(long agentSessionId, long alloyId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.Save(agentSessionId, alloyId, prospect, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

		//public bool Activate(string sessionId, long alloyId, Dictionary<string, string> context)
        //{
		//    return DesktopEngine.Activate(sessionId, AlloyID, mgiContext);
        //}

        public Response SearchCustomers(long agentSessionId, CustomerSearchCriteria searchCriteria, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
               response = DesktopEngine.SearchCustomers(agentSessionId, searchCriteria, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response InitiateCustomerSession(long agentSessionId, CustomerAuthentication authentication, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.InitiateCustomerSession(agentSessionId, authentication, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response GetProspect(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetProspect(agentSessionId, alloyId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response UpdateCustomer(long agentSessionId, long alloyId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.UpdateCustomer(agentSessionId, alloyId, prospect, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response RecordIdentificationConfirmation(long customerSessionId, string agentId, bool IdentificationStatus, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.RecordIdentificationConfirmation(customerSessionId, agentId, IdentificationStatus, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

		public Response CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.CanChangeProfileStatus(agentSessionId, userId, profileStatus, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
		}

        public Response Get(long agentSessionId, string Phone, string PIN, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.Get(agentSessionId, Phone, PIN, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
        }

        public Response ValidateSSN(long agentSessionId, string SSN, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.ValidateSSN(agentSessionId, SSN, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response GetAnonymousUserPAN(long agentSessionId, long channelPartnerId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetAnonymousUserPAN(agentSessionId, channelPartnerId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        /// <summary>
        ///  Customers Serach by Parameters
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="customerLookUpCriteria"></param>
        /// <returns></returns>
        public Response CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.CustomerLookUp(agentSessionId, customerLookUpCriteria, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response ValidateCustomerStatus(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.ValidateCustomerStatus(agentSessionId, alloyId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response NexxoActivate(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.NexxoActivate(agentSessionId, alloyId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response ClientActivate(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.ClientActivate(agentSessionId, alloyId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response UpdateCustomerToClient(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.UpdateCustomerToClient(agentSessionId, alloyId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }

        public Response CustomerSyncInFromClient(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.CustomerSyncInFromClient(agentSessionId, alloyId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }


        public Response ValidateCustomer(long agentSessionId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Response response = new Response();
            try
            {
                response = DesktopEngine.ValidateCustomer(agentSessionId, prospect, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
        }
    }
}

