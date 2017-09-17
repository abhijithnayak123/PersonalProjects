using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

#region Zeo References
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Customer.Contract;
using TCF.Zeo.Common.DataProtection.Impl;
using TCF.Zeo.Processor;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Cxn.Fund.Contract;
using TCF.Zeo.Cxn.Fund.Data;
using COREDATA = TCF.Zeo.Core.Data;
using CoreContract = TCF.Zeo.Core.Contract;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.DataProtection.Contract;
using CXNFundsException = TCF.Zeo.Cxn.Fund.Data.Exceptions;
using CXNDATA = TCF.Zeo.Cxn.Customer.Data;
#endregion

#region External References
using AutoMapper;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using TCF.Zeo.Core.Impl;
using TCF.Zeo.Biz.Customer.Impl.TCF;
using TCF.Zeo.Cxn.Customer.Contract;
using System.Data;
using System.IO;
#endregion

namespace TCF.Zeo.Biz.Customer.Impl
{
    public class CustomerService : ICustomerService
    {
        IMapper mapper;
        ProcessorRouter processorRouter;
        CoreContract.ICustomerService corecustomerService;
        CoreContract.IAgentService coreagentService;

        public CustomerService()
        {
            #region Mapping
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerProfile, COREDATA.CustomerProfile>();

                cfg.CreateMap<Phone, COREDATA.Phone>();
                cfg.CreateMap<COREDATA.Phone, Phone>();
                cfg.CreateMap<Address, COREDATA.Address>();
                cfg.CreateMap<COREDATA.Address, Address>();
                cfg.CreateMap<CustomerSearchCriteria, COREDATA.CustomerSearchCriteria>();
                cfg.CreateMap<COREDATA.CustomerProfile, CustomerProfile>();
                cfg.CreateMap<COREDATA.CustomerSession, CustomerSession>()
                     .ForMember(x => x.CustomerSessionId, opt => opt.MapFrom(y => y.Id));

            });

            mapper = config.CreateMapper();
            #endregion
        }


        #region Public Methods

        /// <summary>
        /// This method is used to search the customers at the provider level.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            List<CustomerProfile> customers = new List<CustomerProfile>();

            try
            {
                ICustomerRepository customerService = GetBizCustomerService(criteria.ChannelPartnerName);

                customers = customerService.SearchCoreCustomers(criteria, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_FETCH_FAILED, ex);
            }

            return customers;
        }

        /// <summary>
        /// This method is used to search the customer from the card number.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCardCustomer(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            List<CustomerProfile> customers = new List<CustomerProfile>();

            try
            {
                string decryptedCardNumber = criteria.CardNumber;

                string encryptedCardNo = EncryptCardNumber(criteria.CardNumber);

                ICustomerRepository customerService = GetBizCustomerService(context.ChannelPartnerName);

                criteria.ChannelPartnerId = context.ChannelPartnerId;

                criteria.CardNumber = encryptedCardNo;

                customers = customerService.SearchCardCustomer(criteria, context);

                if (customers == null || customers.Count <= 0)
                {
                    IFundProcessor fundProcessor = _GetProcessor(context.ChannelPartnerName);

                    int providerId = (int)(_GetFundProvider(context.ChannelPartnerName));

                    CardAccount cardAccount = new CardAccount()
                    {
                        CardNumber = decryptedCardNumber
                    };
                    bool isNewCard = true;

                    CustomerCard card = fundProcessor.AssociateCard(cardAccount, null, context, isNewCard);

                    if (card == null || (card.CustomerId == 0 && card.CustomerId != -1))
                        throw new CXNFundsException.FundException(CXNFundsException.FundException.CARD_MAPPING_ERROR);

                    criteria.CardNumber = card.CardNumber;
                    customers = customerService.SearchCardCustomer(criteria, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_FETCH_FAILED, ex);
            }

            return customers;
        }

        /// <summary>
        /// This method is used to validate the customer SSN against the exsisting customer SSN.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool ValidateSSN(commonData.ZeoContext context)
        {
            bool isValid = false;
            try
            {
                using (corecustomerService = new ZeoCoreImpl())
                {
                    isValid = corecustomerService.ValidateSSN(context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.SSN_VALIDATION_FAILED, ex);
            }

            return isValid;
        }

        /// <summary>
        /// This method is used to create a new customer in Alloy.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public long InsertCustomer(CustomerProfile customer, commonData.ZeoContext context)
        {
            try
            {
                COREDATA.CustomerProfile profile = mapper.Map<COREDATA.CustomerProfile>(customer);
                profile.DTServerCreate = DateTime.Now;
                profile.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);
                profile.IDCode = Helper.GetIDCode(profile.SSN);

                using (corecustomerService = new ZeoCoreImpl())
                {
                    return corecustomerService.InsertCustomer(profile, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_SAVE_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used to update the existing customer in Alloy.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool UpdateCustomer(CustomerProfile customer, commonData.ZeoContext context)
        {
            try
            {
                COREDATA.CustomerProfile profile = mapper.Map<COREDATA.CustomerProfile>(customer);
                profile.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                profile.DTServerLastModified = DateTime.Now;

                using (corecustomerService = new ZeoCoreImpl())
                {
                    return corecustomerService.UpdateCustomer(profile, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_UPDATE_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used to get customer by customer Id.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerProfile GetCustomer(commonData.ZeoContext context)
        {
            COREDATA.CustomerProfile profile = new COREDATA.CustomerProfile();
            CustomerProfile customerProfile = new CustomerProfile();

            try
            {
                using (corecustomerService = new ZeoCoreImpl())
                {
                    profile = corecustomerService.GetCustomer(context);
                }
                customerProfile = mapper.Map<COREDATA.CustomerProfile, CustomerProfile>(profile);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_FETCH_FAILED, ex);
            }
            return customerProfile;
        }

        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public long RegisterToClient(commonData.ZeoContext context)
        {
            try
            {
                using (var scope = TransactionHandler.CreateTransactionScope())
                {
                    ICustomerRepository customerService = GetBizCustomerService(context.ChannelPartnerName);
                    long accountId = customerService.RegisterToClient(context);
                    scope.Complete();
                    return accountId;
                }
            }
            catch (Exception ex)
            {
                UpdateErrorMessage(ex, context);

                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_REGISTRATION_FAILED, ex);
            }
        }

        /// <summary>
        /// This method will update the customer data to client.
        /// </summary>
        /// <param name="context"></param>
		public void UpdateCustomerToClient(commonData.ZeoContext context)
        {
            try
            {
                using (var scope = TransactionHandler.CreateTransactionScope())
                {
                    ICustomerRepository customerService = GetBizCustomerService(context.ChannelPartnerName);
                    customerService.UpdateCustomerToClient(context);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                UpdateErrorMessage(ex, context);
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_UPDATE_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used to sync in the customer details with TCF.
        /// </summary>
		/// <param name="context"></param>
        public void CustomerSyncInFromClient(commonData.ZeoContext context)
        {
            try
            {
                ICustomerRepository customerService = GetBizCustomerService(context.ChannelPartnerName);
                customerService.CustomerSyncInFromClient(context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_SYNC_IN_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used to create a customer session.
        /// </summary>
        /// <param name="alloyId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerSession InitiateCustomerSession(CardSearchType cardSearchType, commonData.ZeoContext context)
        {
            COREDATA.CustomerSession corecustomerSession = new COREDATA.CustomerSession();

            try
            {
                bool cardPresent = false;
                if (cardSearchType == CardSearchType.None)
                {
                    cardSearchType = CardSearchType.Other;
                }

                corecustomerSession = new COREDATA.CustomerSession()
                {
                    CustomerId = context.CustomerId,
                    CardPresent = cardPresent,
                    TimezoneID = context.TimeZone,
                    CardSearchType = cardSearchType,
                    DTServerCreate = DateTime.Now,
                    DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone)
                };

                using (corecustomerService = new ZeoCoreImpl())
                {
                    corecustomerSession = corecustomerService.CreateCustomerSession(corecustomerSession, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_INITIATION_FAILED, ex);
            }

            return mapper.Map<COREDATA.CustomerSession, CustomerSession>(corecustomerSession);
        }

        /// <summary>
        /// This method is used to validate the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool ValidateCustomer(CustomerProfile customer, commonData.ZeoContext context)
        {
            try
            {
                ICustomerRepository customerService = GetBizCustomerService(customer.ChannelPartnerName);
                return customerService.ValidateCustomer(customer, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_VALIDATION_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used to whether the agent has a permission to close the customer.
        /// </summary>
        /// <param name="profileStatus"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool CanChangeProfileStatus(string profileStatus, commonData.ZeoContext context)
        {
            try
            {
                bool hasPermission = false;
                COREDATA.UserDetails userDetails = new COREDATA.UserDetails();

                using (coreagentService = new ZeoCoreImpl())
                {
                    userDetails = coreagentService.GetAgentDetails(context.AgentSessionId, context);
                }

                switch (userDetails.UserRoleId)
                {
                    case (int)UserRoles.SystemAdmin:
                    case (int)UserRoles.Manager:
                    case (int)UserRoles.ComplianceManager:
                        {
                            if ((ProfileStatus.Active.ToString() == profileStatus) || (ProfileStatus.Inactive.ToString() == profileStatus)
                                || (ProfileStatus.Closed.ToString() == profileStatus))
                            {
                                hasPermission = true;
                            }
                            break;
                        }
                    case (int)UserRoles.Teller:
                        {
                            if ((ProfileStatus.Active.ToString() == profileStatus) || (ProfileStatus.Inactive.ToString() == profileStatus))
                            {
                                hasPermission = true;
                            }
                            break;
                        }
                }
                return hasPermission;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.PROFILE_STATUS_FETCH_FAILED, ex);
            }
        }

        public void UpdateCustomerRegistrationStatus(ProfileStatus status, string clientId, string errorReason,
            bool isRCIFSuccess, commonData.ZeoContext context)
        {
            try
            {
                using (corecustomerService = new ZeoCoreImpl())
                {
                    corecustomerService.UpdateCustomerRegistrationStatus(status, clientId, errorReason, isRCIFSuccess, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.UPDATE_ERRORREASON_FAILED, ex);
            }
        }

        /// <summary>
        /// Used to search the customers either from the card number or the customer details
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCustomers(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            List<CustomerProfile> customers = null;

            try
            {
                ICustomerRepository customerService = GetBizCustomerService(context.ChannelPartnerName);

                customers = customerService.SearchCustomers(criteria, context);
            }

            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;

                throw new CustomerException(CustomerException.CUSTOMER_FETCH_FAILED, ex);
            }

            return customers;
        }

        #endregion

        #region Private methods
        private ICustomerRepository GetBizCustomerService(string channelpartnerName)
        {
            switch (channelpartnerName.ToLower())
            {
                //case "synovus":
                //case "carver":
                case "tcf":
                    return new TCFCustomer();
                default:
                    break;
            }
            return null;
        }

        private string EncryptCardNumber(string cardNumber)
        {
            IDataProtectionService dataProtectionService = null;

            string type = (ConfigurationManager.AppSettings["DataProtectionService"]);

            if (type.ToLower() == "simulator")
            {
                dataProtectionService = new DataProtectionSimulator();
            }
            else
            {
                dataProtectionService = new DataProtectionService();
            }

            return dataProtectionService.Encrypt(cardNumber, 0);
        }

        private IFundProcessor _GetProcessor(string channelPartner)
        {
            ProcessorRouter processorRouter = new ProcessorRouter();
            // get the fund processor for the channel partner.
            return (IFundProcessor)processorRouter.GetFundProvider(channelPartner);
        }

        private ProviderId _GetFundProvider(string channelPartner)
        {
            ProcessorRouter processorRouter = new ProcessorRouter();
            // get the fund provider for the channel partner.
            return processorRouter.GetFundProviders(channelPartner);
        }

        private void UpdateErrorMessage(Exception ex, commonData.ZeoContext context)
        {
            commonData.ProviderException providerException = ex as commonData.ProviderException;
            if (providerException != null)
            {
                string errorMessage = string.Format("{0}.{1}.{2}|{3}", providerException.ProductCode, providerException.ProviderCode, providerException.ProviderErrorCode, ex.Message);
                UpdateCustomerRegistrationStatus(ProfileStatus.Inactive, string.Empty, errorMessage, false, context);
            }
        }

        #endregion
    }
}
