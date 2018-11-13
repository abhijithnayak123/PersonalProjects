using System;
using System.Diagnostics;
using System.Collections.Generic;

using ICustomerService = MGI.Biz.Customer.Contract.ICustomerService;
using MGI.Biz.Customer.Data;
using CustomerSessionDTO = MGI.Biz.Customer.Data.CustomerSession;
using CustomerDTO = MGI.Biz.Customer.Data.Customer;
using CustomerProfileDTO = MGI.Biz.Customer.Data.CustomerProfile;
using CustomerSearchCriteriaDTO = MGI.Biz.Customer.Data.CustomerSearchCriteria;

using MGI.Core.CXE.Data;
using MGI.Common.Util;
using CXECustomer = MGI.Core.CXE.Data.Customer;

using Spring.Transaction.Interceptor;

namespace MGI.Biz.Customer.Impl
{
	public class CustomerServiceImpl : ICustomerService
	{
		private ICustomerService _innerService;
		public ICustomerService InnerService { set { _innerService = value; } }
        public NLoggerCommon NLogger { get; set; }

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void Register(long agentSessionId, SessionContext sessionContext, long alloyId, MGIContext mgiContext)
		{
            NLogger.Info("Register!");
            _innerService.Register(agentSessionId, sessionContext, alloyId, mgiContext);
		}

        public List<CustomerSearchResult> Search(long agentSessionId, CustomerSearchCriteriaDTO criteria, MGIContext mgiContext)
		{
            return _innerService.Search(agentSessionId, criteria, mgiContext);
		}

        public CustomerSessionDTO InitiateCustomerSession(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
            return _innerService.InitiateCustomerSession(agentSessionId, alloyId, mgiContext);
		}

		public bool CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGIContext mgiContext)
		{
            return _innerService.CanChangeProfileStatus(agentSessionId, userId, profileStatus, mgiContext);
		}

		//private void UpdateProspect( ProspectDTO prospectDTO, CXEProspect prospect )
		//{
		//    _customerServiceManager.UpdateProspectProfile( prospectDTO, prospect );

		//    IdType idType = _customerServiceManager.GetIdType( prospectDTO.ID );

		//    _customerServiceManager.UpdateIdentification( prospectDTO.ID, idType, prospect );

		//    _customerServiceManager.UpdateEmploymentDetails( prospectDTO, prospect );
		//}
        
        public CustomerDTO GetCustomer(long customerSessionId, long alloyId, MGIContext mgiContext)
		{
            return _innerService.GetCustomer(customerSessionId, alloyId, mgiContext);
		}

		[Transaction( Spring.Transaction.TransactionPropagation.RequiresNew )]
        public void SaveCustomer(long agentSessionId, long alloyId, CustomerDTO customer, MGIContext mgiContext)
		{
            _innerService.SaveCustomer(agentSessionId, alloyId, customer, mgiContext);
		}

        public CustomerDTO Get(long agentSessionId, string Phone, string PIN, MGIContext mgiContext)
        {
            return _innerService.Get(agentSessionId, Phone, PIN, mgiContext);
        }

        public CustomerDTO GetCustomerForCardNumber(long agentSessionId, string CardNumber, MGIContext mgiContext)
        {
            return _innerService.GetCustomerForCardNumber(agentSessionId, CardNumber, mgiContext);
        }

        public bool ValidateSSN(long agentSessionId, string SSN, MGIContext mgiContext)
        {
            return _innerService.ValidateSSN(agentSessionId, SSN, mgiContext);
        }



        public long GetAnonymousUserPAN(long agentSessionId, long channelPartnerId, string firstName, string lastName, MGIContext mgiContext)
        {
            return _innerService.GetAnonymousUserPAN(agentSessionId, channelPartnerId, firstName, lastName, mgiContext);
        }

        public List<CustomerDTO> CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext mgiContext)
        {
            return _innerService.CustomerLookUp(agentSessionId, customerLookUpCriteria, mgiContext);
        }

        public void ValidateCustomerStatus(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            _innerService.ValidateCustomerStatus(agentSessionId, alloyId, mgiContext);
        }

        public void RegisterToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            _innerService.RegisterToClient(agentSessionId, alloyId, mgiContext);
        }

        public void UpdateCustomerToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            _innerService.UpdateCustomerToClient(agentSessionId, alloyId, mgiContext);
        }

        public void CustomerSyncInFromClient(long agentSessionId, long cxeCustomerId, MGIContext mgiContext)
        {
            _innerService.CustomerSyncInFromClient(agentSessionId, cxeCustomerId, mgiContext);
        }


        public ProfileStatus GetClientProfileStatus(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            return _innerService.GetClientProfileStatus(agentSessionId, alloyId, mgiContext);
        }


        public bool IsValidSSN(long customerSessionId, string last4DigitsOfSSN, MGIContext mgiContext)
        {
            return _innerService.IsValidSSN(customerSessionId, last4DigitsOfSSN, mgiContext);
        }


        public bool ValidateCustomer(long agentSessionId, Biz.Customer.Data.Customer customer, MGIContext mgiContext)
        {
            return _innerService.ValidateCustomer(agentSessionId, customer,  mgiContext);
        }
    }
}
