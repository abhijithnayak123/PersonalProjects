// MS libraries
using System;
using System.Collections.Generic;
using Spring.Transaction.Interceptor;
using IMVACustomerService = MGI.Channel.MVA.Server.Contract.ICustomerService;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.MVA.Server.Impl
{
    /// <summary>
    /// This Class Implements ICustomerService 
    /// </summary>
    public partial class MVAEngine : IMVACustomerService
    {

        #region MVA ICustomerService Impl

        public CustomerSession InitiateCustomerSession(string cardNumber, string channelPartnerName)
        {
            CustomerSession customerSession = new CustomerSession();
            bool IsNewCustomer = false;
			long AlloyID = 0L;
            long agentSessionId = 0L;
            MGI.Common.Util.MGIContext context = Self.GetPartnerContext(channelPartnerName);

            Customer customerDto = Get(cardNumber, context);

            if (customerDto == null)
            {
				AlloyID = CreateCustomer(cardNumber, context);
                IsNewCustomer = true;
            }
            else
				AlloyID = customerDto.CIN;

			customerSession = ConsumerEngine.InitiateCustomerSession(agentSessionId ,AlloyID, context);

            if (customerSession != null)
                customerSession.isNewCustomer = IsNewCustomer;

            return customerSession;
        }

        public bool IsValidSSN(string customerSessionId, string last4DigitsOfSSN)
        {
			MGI.Common.Util.MGIContext context = new MGI.Common.Util.MGIContext();
            return ConsumerEngine.IsValidSSN(long.Parse(customerSessionId), last4DigitsOfSSN, context);
        }

        #endregion

        #region Private Methods

        private Customer Get(string cardNumber, MGI.Common.Util.MGIContext context)
        {
            long agentSessionId =0L;
            return ConsumerEngine.GetCustomerByCardNumber(agentSessionId, cardNumber, context);
        }

		private long CreateCustomer(string cardNumber, MGI.Common.Util.MGIContext mgiContext)
        {
            long agentSessionId = 0L;
            Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>() { { "cardNumber", cardNumber } };

			Prospect prospectDto = ConsumerEngine.CustomerLookUp(agentSessionId, customerLookUpCriteria, mgiContext);

			long alloyId = ConsumerEngine.CreateProspect(agentSessionId, prospectDto, mgiContext);

			ConsumerEngine.SaveProspect(agentSessionId, alloyId, prospectDto, mgiContext);

			ConsumerEngine.NexxoActivate(agentSessionId, alloyId, mgiContext);

			ConsumerEngine.ClientActivate(agentSessionId, alloyId, mgiContext);

			return alloyId;
        }

        #endregion

    }
}

