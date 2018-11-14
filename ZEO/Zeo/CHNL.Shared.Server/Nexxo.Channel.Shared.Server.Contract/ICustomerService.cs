using System;
using System.Collections.Generic;
using System.ServiceModel;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Contract
{
    public interface ICustomerService
    {
        long CreateProspect(long agentSessionId, Prospect prospect, SessionContext contextDTO, MGIContext mgiContext);

		void SaveProspect(long agentSessionId, long alloyId, Prospect prospect, SessionContext contextDTO, MGIContext mgiContext);

		void NexxoActivate(long agentSessionId, long alloyId, SessionContext contextDTO, MGIContext mgiContext);

		void ClientActivate(long agentSessionId, long alloyId, MGIContext mgiContext);

        void UpdateCustomer(long agentSessionId, long alloyId, Prospect prospect, MGIContext mgiContext);

		void UpdateCustomerToClient(long agentSessionId, long alloyId, MGIContext mgiContext);

		void CustomerSyncInFromClient(long agentSessionId, long alloyId, MGIContext mgiContext);

        CustomerSession InitiateCustomerSession(long agentSessionId, long alloyId, MGIContext context);

        Customer GetCustomerByCardNumber(long agentSessionId, string CardNumber, MGIContext context);
    }
}

