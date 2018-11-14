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
            try
            {
                NLogger.Info("Register!");
                _innerService.Register(agentSessionId, sessionContext, alloyId, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_REGISTRATION_FAILED, ex);
            }
		}

        public List<CustomerSearchResult> Search(long agentSessionId, CustomerSearchCriteriaDTO criteria, MGIContext mgiContext)
		{
            try
            {
                return _innerService.Search(agentSessionId, criteria, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_SEARCH_FAILED, ex);
            }
		}

        public CustomerSessionDTO InitiateCustomerSession(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
            try
            {
                return _innerService.InitiateCustomerSession(agentSessionId, alloyId, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_INITIATION_FAILED, ex);
            }
		}

		public bool CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGIContext mgiContext)
		{
            try
            {
                return _innerService.CanChangeProfileStatus(agentSessionId, userId, profileStatus, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.PROFILE_STATUS_FETCH_FAILED, ex);
            }
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
            try
            {
                return _innerService.GetCustomer(customerSessionId, alloyId, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_FETCH_FAILED, ex);
            }
		}

		[Transaction( Spring.Transaction.TransactionPropagation.RequiresNew )]
        public void SaveCustomer(long agentSessionId, long alloyId, CustomerDTO customer, MGIContext mgiContext)
		{
            try
            {
                _innerService.SaveCustomer(agentSessionId, alloyId, customer, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_SAVE_FAILED, ex);
            }
		}

        public CustomerDTO Get(long agentSessionId, string Phone, string PIN, MGIContext mgiContext)
        {
            try
            {
                return _innerService.Get(agentSessionId, Phone, PIN, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_SEARCH_FAILED, ex);
            }
        }

        public CustomerDTO GetCustomerForCardNumber(long agentSessionId, string CardNumber, MGIContext mgiContext)
        {
            try
            {
                return _innerService.GetCustomerForCardNumber(agentSessionId, CardNumber, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_FETCH_FAILED, ex);
            }
        }

        public bool ValidateSSN(long agentSessionId, string SSN, MGIContext mgiContext)
        {
            try
            {
                bool isValidSSN = _innerService.ValidateSSN(agentSessionId, SSN, mgiContext);

                return isValidSSN;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.SSN_VALIDATION_FAILED, ex);
            }
        }



        public long GetAnonymousUserPAN(long agentSessionId, long channelPartnerId, string firstName, string lastName, MGIContext mgiContext)
        {
           try
           {
               return _innerService.GetAnonymousUserPAN(agentSessionId, channelPartnerId, firstName, lastName, mgiContext);
           }
           catch (Exception ex)
           {
               if (ExceptionHelper.IsExceptionHandled(ex)) throw;
               throw new BizCustomerException(BizCustomerException.CUSTOMER_FETCH_FAILED, ex);
           }
        }

        public List<CustomerDTO> CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext mgiContext)
        {
            try
            {
                return _innerService.CustomerLookUp(agentSessionId, customerLookUpCriteria, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_FETCH_FAILED, ex);
            }
        }

        public void ValidateCustomerStatus(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {
                _innerService.ValidateCustomerStatus(agentSessionId, alloyId, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_VALIDATION_FAILED, ex);
            }
        }

        public void RegisterToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {
                _innerService.RegisterToClient(agentSessionId, alloyId, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_REGISTRATION_FAILED, ex);
            }
        }

        public void UpdateCustomerToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
           try
           {
               _innerService.UpdateCustomerToClient(agentSessionId, alloyId, mgiContext);
           }
           catch(Exception ex)
           {
               if (ExceptionHelper.IsExceptionHandled(ex)) throw;
               throw new BizCustomerException(BizCustomerException.CUSTOMER_UPDATE_FAILED, ex);
           }
        }

        public void CustomerSyncInFromClient(long agentSessionId, long cxeCustomerId, MGIContext mgiContext)
        {
            try
            {
                _innerService.CustomerSyncInFromClient(agentSessionId, cxeCustomerId, mgiContext);
            }
            catch(Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_SYNC_IN_FAILED, ex);
            }
        }


        public ProfileStatus GetClientProfileStatus(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {
                return _innerService.GetClientProfileStatus(agentSessionId, alloyId, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.PROFILE_STATUS_FETCH_FAILED, ex);
            }
        }


        public bool IsValidSSN(long customerSessionId, string last4DigitsOfSSN, MGIContext mgiContext)
        {
            try
            {
                return _innerService.IsValidSSN(customerSessionId, last4DigitsOfSSN, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.SSN_VALIDATION_FAILED, ex);
            }
        }


        public bool ValidateCustomer(long agentSessionId, Biz.Customer.Data.Customer customer, MGIContext mgiContext)
        {
            try
            {
                return _innerService.ValidateCustomer(agentSessionId, customer, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_VALIDATION_FAILED, ex);
            }
        }
    }
}
