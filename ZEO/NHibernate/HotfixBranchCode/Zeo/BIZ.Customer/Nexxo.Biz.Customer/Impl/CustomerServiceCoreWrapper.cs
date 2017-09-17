using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Biz.Customer.Contract;
using BizCustomerService = MGI.Biz.Customer.Contract.ICustomerService;
using MGI.Biz.Customer.Data;
using CustomerSearchCriteriaDTO = MGI.Biz.Customer.Data.CustomerSearchCriteria;
using CustomerSessionDTO = MGI.Biz.Customer.Data.CustomerSession;
using CustomerDTO = MGI.Biz.Customer.Data.Customer;
using CustomerProfileDTO = MGI.Biz.Customer.Data.CustomerProfile;

using MGI.Core.CXE.Data;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;

using MGI.Core.CXE.Contract;
using ICXECustomerService = MGI.Core.CXE.Contract.ICustomerService;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using CXECustomerSearchCriteria = MGI.Core.CXE.Data.CustomerSearchCriteria;
using CXEAccount = MGI.Core.CXE.Data.Account;

using MGI.Core.Partner.Contract;
using IPtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using MGI.Core.Partner.Data;
using PtnrAccount = MGI.Core.Partner.Data.Account;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using PtnrCustomerSession = MGI.Core.Partner.Data.CustomerSession;

using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.Data;

using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;

using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.Data;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using MGI.Biz.Events.Contract;
using CxnCustomerData = MGI.Cxn.Customer.Data;

using AutoMapper;
using MGI.Cxn.Common.Processor.Contract;
using MGI.Cxn.Common.Processor.Util;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Biz.Customer.Impl
{
    public class CustomerServiceCoreWrapper : BizCustomerService
    {
        private INexxoDataStructuresService _ptnrIdTypeService;
        public INexxoDataStructuresService PTNRIdTypeService { set { _ptnrIdTypeService = value; } }

        private ICXECustomerService _cxeCustomerService;
        public ICXECustomerService CXECustomerService { set { _cxeCustomerService = value; } }

        private IAccountService _cxeAccountService;
        public IAccountService CXEAccountService { set { _cxeAccountService = value; } }

        private IPtnrCustomerService _ptnrCustomerService;
        public IPtnrCustomerService PartnerCustomerService { get { return _ptnrCustomerService; } set { _ptnrCustomerService = value; } }

        private IAgentSessionService _agtSessionService;
        public IAgentSessionService AgentSessionService { set { _agtSessionService = value; } }

        private ICustomerSessionService _custSessionService;
        public ICustomerSessionService CustomerSessionService { set { _custSessionService = value; } }

        private IChannelPartnerService _channelPartnerService;
        public IChannelPartnerService ChannelPartnerService { set { _channelPartnerService = value; } }

        private IChannelPartnerGroupService _ptnrGroupSvc;
        public IChannelPartnerGroupService ChannelPartnerGroupService { set { _ptnrGroupSvc = value; } }

        private ILimitService _limitService;
        public ILimitService LimitService { set { _limitService = value; } }

        private IManageLocations _locationSvc;
        public IManageLocations LocationSvc { set { _locationSvc = value; } }

        public IMessageCenter MessageCenterService { private get; set; }

        public INexxoBizEventPublisher EventPublisher { private get; set; }

        public IProcessorRouter ProcessorSvc { private get; set; }

        public IProcessorRouter CustomerProcessorSvc { private get; set; }

        public IManageUsers ManageUserSvc { private get; set; }

        public MGI.Common.Util.TLoggerCommon MongoDBLogger { private get; set; }

        public CustomerServiceCoreWrapper()
        {

            Mapper.CreateMap<CXECustomer, CustomerDTO>()
                .AfterMap((s, d) =>
                    {
                        d.Profile = new CustomerProfileDTO()
                        {
                            Address1 = s.Address1,
                            Address2 = s.Address2,
                            City = s.City,
                            DateOfBirth = s.DateOfBirth,
                            DoNotCall = s.DoNotCall,
                            Email = s.Email,
                            FirstName = s.FirstName,
                            Gender = s.Gender,
                            LastName = s.LastName,
                            LastName2 = s.LastName2,
                            MailingAddress1 = s.MailingAddress1,
                            MailingAddress2 = s.MailingAddress2,
                            MailingAddressDifferent = s.MailingAddressDifferent,
                            MailingCity = s.MailingCity,
                            MailingState = s.MailingState,
                            MailingZipCode = s.MailingZipCode,
                            MarketingSMSEnabled = s.MarketingSMSEnabled,
                            MiddleName = s.MiddleName,
                            MothersMaidenName = s.MothersMaidenName,
                            Phone1 = s.Phone1,
                            Phone1Provider = s.Phone1Provider,
                            Phone1Type = s.Phone1Type,
                            Phone2 = s.Phone2,
                            Phone2Provider = s.Phone2Provider,
                            Phone2Type = s.Phone2Type,
                            PIN = s.PIN,
                            SMSEnabled = s.SMSEnabled,
                            SSN = s.SSN,
                            State = s.State,
                            TaxpayerId = s.TaxpayerId,
                            ZipCode = s.ZipCode,
                            ReceiptLanguage = s.ReceiptLanguage,
                            ProfileStatus = s.ProfileStatus,
                            IDCode = s.IDCode
                        };
                    })
                .ForMember(c => c.ID, o => o.MapFrom(s => s.GovernmentId))
                .AfterMap((s, d) =>
                {
                    d.ID.CountryOfBirth = s.CountryOfBirth;
                })
                .ForMember(c => c.Employment, o => o.MapFrom(s => s.EmploymentDetails));

            Mapper.CreateMap<CustomerGovernmentId, Identification>()
                .ForMember(g => g.ExpirationDate, o => o.MapFrom(s => s.ExpirationDate))
                .ForMember(g => g.IssueDate, o => o.MapFrom(s => s.IssueDate))
                .ForMember(g => g.GovernmentId, o => o.MapFrom(s => s.Identification));

            Mapper.CreateMap<CustomerEmploymentDetails, EmploymentDetails>();
            Mapper.CreateMap<EmploymentDetails, CustomerEmploymentDetails>();

            Mapper.CreateMap<CustomerSearchCriteriaDTO, CXECustomerSearchCriteria>();

            Mapper.CreateMap<CXECustomer, CustomerSearchResult>()
                .ForMember(r => r.Address, o => o.MapFrom(s => s.Address1))
                .ForMember(r => r.CardNumber, o => o.Ignore())
                .ForMember(r => r.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth))
                .ForMember(r => r.FullName, o => o.MapFrom(s => string.Format("{0} {1}", s.FirstName, s.LastName)))
                .ForMember(r => r.GovernmentId, o => o.MapFrom(s => (s.GovernmentId != null ? s.GovernmentId.Identification : "")))
                .ForMember(r => r.MothersMaidenName, o => o.MapFrom(s => s.MothersMaidenName))
                .ForMember(r => r.AlloyID, o => o.MapFrom(s => s.Id))
                .ForMember(r => r.PhoneNumber, o => o.MapFrom(s => s.Phone1))
                .ForMember(r => r.SSN, o => o.MapFrom(s => s.SSN))
                .AfterMap((s, d) =>
                    {
                        if (s.Accounts.Where(a => a.Type == (int)AccountTypes.Funds).Count() > 0)
                        {
                            string channelPartner = _channelPartnerService.ChannelPartnerConfig(s.ChannelPartnerId).Name;
                            long cxeAccountID = s.Accounts.First(a => a.Type == (int)AccountTypes.Funds).Id;
                            int providerId = (int)Enum.Parse(typeof(ProviderIds), _GetFundProvider(channelPartner));
                            // AL-2059 Changes
                            // Get cxn account based on provider id, instead of assuming 1-1 mapping.
                            //long cxnAccountID = _ptnrCustomerService.Lookup(s.Id).FindAccountByCXEId(cxeAccountID).CXNId;
                            PtnrAccount ptnrAccount = _ptnrCustomerService.Lookup(s.Id).Accounts.FirstOrDefault(x => x.ProviderId == providerId);
                            if (ptnrAccount != null)
                            {
                                long cxnAccountID = ptnrAccount.CXNId;
                                CardAccount cardAccount = null;
                                if (cxnAccountID != 0)
                                {
                                    cardAccount = _GetProcessor(channelPartner).LookupCardAccount(cxnAccountID);
                                }
                                if (cardAccount != null && cardAccount.IsCardActive)
                                    d.CardNumber = cardAccount.CardNumber;
                            }

                        }
                        d.ProfileStatus = _ptnrCustomerService.LookupByCxeId(s.Id).CustomerProfileStatus.ToString();//"Active";
                    });


            Mapper.CreateMap<PTNRProspect, Identification>()
                .ForMember(x => x.GovernmentId, o => o.MapFrom(s => s.GovernmentId.Identification))
                .ForMember(x => x.IDType, o => o.MapFrom(s => s.GovernmentId.IdType.Name))
                .ForMember(x => x.IssueDate, o => o.MapFrom(s => s.GovernmentId.IssueDate))
                .ForMember(x => x.ExpirationDate, o => o.MapFrom(s => s.GovernmentId.ExpirationDate));

            Mapper.CreateMap<ProspectEmploymentDetails, CustomerEmploymentDetails>()
                .ForMember(x => x.DTServerCreate, opt => opt.MapFrom(r => DateTime.Now))
                .ForMember(x => x.DTServerLastModified, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());

            Mapper.CreateMap<ProspectGovernmentId, CustomerGovernmentId>()
                .ForMember(x => x.DTServerCreate, opt => opt.MapFrom(r => DateTime.Now))
                .ForMember(x => x.DTServerLastModified, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());

            Mapper.CreateMap<PTNRProspect, CXECustomer>()
                .ForMember(x => x.DTTerminalCreate, opt => opt.Ignore())
                .ForMember(x => x.DTTerminalLastModified, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.AlloyID))
                .ForMember(x => x.rowguid, opt => opt.MapFrom(c => c.id))
                .ForMember(x => x.ChannelPartnerId, opt => opt.Ignore());

            Mapper.CreateMap<CustomerProfileDTO, CXECustomer>();

            Mapper.CreateMap<CXECustomer, Biz.Customer.Data.CustomerProfile>()
                .ForMember(x => x.GovernmentIDType, opt => opt.MapFrom(y => _ptnrIdTypeService.Find(y.ChannelPartnerId, y.GovernmentId.IdTypeId).Name))
                .ForMember(x => x.GovernmentId, opt => opt.MapFrom(y => y.GovernmentId.Identification))
                .ForMember(x => x.IDExpirationDate, opt => opt.MapFrom(y => y.GovernmentId.ExpirationDate))
                .ForMember(x => x.IDIssuingCountry, opt => opt.MapFrom(y => _ptnrIdTypeService.Find(y.ChannelPartnerId, y.GovernmentId.IdTypeId).CountryId.Name))
                .ForMember(x => x.IDIssuingCountryId, opt => opt.MapFrom(y => _ptnrIdTypeService.Find(y.ChannelPartnerId, y.GovernmentId.IdTypeId).CountryId.Abbr2))
                .ForMember(x => x.IDIssuingState, opt => opt.MapFrom(y => _ptnrIdTypeService.Find(y.ChannelPartnerId, y.GovernmentId.IdTypeId).StateId.Name))
                .ForMember(x => x.IDIssuingStateAbbr, opt => opt.MapFrom(y => _ptnrIdTypeService.Find(y.ChannelPartnerId, y.GovernmentId.IdTypeId).StateId.Abbr))
                .ForMember(x => x.IDIssueDate, opt => opt.MapFrom(y => y.GovernmentId.IssueDate))
                .ForMember(x => x.Occupation, opt => opt.MapFrom(y => y.EmploymentDetails.Occupation))
                .ForMember(x => x.OccupationDescription, opt => opt.MapFrom(y => y.EmploymentDetails.OccupationDescription))
                .ForMember(x => x.Employer, opt => opt.MapFrom(y => y.EmploymentDetails.Employer))
                .ForMember(x => x.EmployerPhone, opt => opt.MapFrom(y => y.EmploymentDetails.EmployerPhone));

            Mapper.CreateMap<CXECustomer, CxnCustomer>();
            Mapper.CreateMap<CxnCustomer, CustomerDTO>();
            Mapper.CreateMap<PtnrCustomer, CustomerGroupSetting>()
                .ForMember(x => x.customer, opt => opt.MapFrom(y => y));
        }

        private IFundProcessor _GetProcessor(string channelPartner)
        {
            // get the fund processor for the channel partner.
            return (IFundProcessor)ProcessorSvc.GetProcessor(channelPartner);
        }

        private ICustomerRepository _GetCustomerProcessor(string channelPartner)
        {
            // get the Customer FIS Gateway for the channel partner.
            return (ICustomerRepository)CustomerProcessorSvc.GetProcessor(channelPartner);
        }

        public void Register(long agentSessionId, SessionContext sessionContext, long alloyId, MGIContext mgiContext)
        {
            PTNRProspect PTNRProspect = _ptnrCustomerService.LookupProspect(alloyId);

            CXECustomer customerProfile = new CXECustomer();
            ChannelPartner channelPartner = _channelPartnerService.ChannelPartnerConfig(PTNRProspect.ChannelPartnerId);
            Mapper.Map<PTNRProspect, CXECustomer>(PTNRProspect, customerProfile);

            //Added time stamp
            Location location = _locationSvc.GetAll().Find(x => x.rowguid == sessionContext.LocationId);

            if (sessionContext.AppName == "DMS-Server" && location == null)
                throw new BizCustomerException(BizCustomerException.LOCATION_NOT_SET);

            customerProfile.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(sessionContext.TimezoneId);
            customerProfile.DTServerCreate = DateTime.Now;

            customerProfile.ChannelPartnerId = channelPartner.Id;

            if (PTNRProspect.EmploymentDetails != null)
            {
                CustomerEmploymentDetails employmentDetails = new CustomerEmploymentDetails();
                Mapper.Map<ProspectEmploymentDetails, CustomerEmploymentDetails>(PTNRProspect.EmploymentDetails, employmentDetails);
                employmentDetails.Customer = customerProfile;
                employmentDetails.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(sessionContext.TimezoneId);
                customerProfile.EmploymentDetails = employmentDetails;
            }
            if (PTNRProspect.GovernmentId != null)
            {
                CustomerGovernmentId governmentId = new CustomerGovernmentId();
                Mapper.Map<ProspectGovernmentId, CustomerGovernmentId>(PTNRProspect.GovernmentId, governmentId);
                governmentId.Customer = customerProfile;
                governmentId.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(sessionContext.TimezoneId);
                customerProfile.GovernmentId = governmentId;
            }

            CXECustomer cxeCustomer = _cxeCustomerService.Register(customerProfile);
            CXEAccount cashAccount = _cxeAccountService.AddCustomerCashAccount(cxeCustomer);
            PtnrCustomer ptnrCustomer = _ptnrCustomerService.Create(
                new MGI.Core.Partner.Data.Customer
                {
                    Id = alloyId,
                    CXEId = alloyId,
                    IsPartnerAccountHolder = PTNRProspect.IsAccountHolder,
                    ReferralCode = PTNRProspect.ReferralCode,
                    ChannelPartnerId = PTNRProspect.ChannelPartnerId,
                    AgentSessionId = sessionContext.AgentSessionId,
                    CustomerProfileStatus = PTNRProspect.ProfileStatus,
                    DTServerCreate = DateTime.Now,
                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(sessionContext.TimezoneId)
                });

            ptnrCustomer.AddAccount((int)ProviderIds.Cash, cashAccount.Id, cashAccount.Id);
            List<CustomerGroupSetting> _CustomerGroupSetting = new List<CustomerGroupSetting>();
            List<ProspectGroupSetting> ProspectGroupSettings = PTNRProspect.Groups.ToList();
            foreach (var item in ProspectGroupSettings)
            {
                CustomerGroupSetting CustomerGroup = new CustomerGroupSetting();
                CustomerGroup.channelPartnerGroup = item.ChannelPartnerGroup;
                CustomerGroup.DTServerCreate = item.DTServerCreate;
                CustomerGroup.DTServerLastModified = item.DTServerLastModified;
                CustomerGroup.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(sessionContext.TimezoneId);
                CustomerGroup.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(sessionContext.TimezoneId);
                Mapper.Map<PtnrCustomer, CustomerGroupSetting>(ptnrCustomer, CustomerGroup);
                _CustomerGroupSetting.Add(CustomerGroup);
                _ptnrCustomerService.SaveGroupSetting(CustomerGroup);
            }
        }

        private void PublishEvent(string channelPartner, CustomerRegistrationEvent customerRegisterEvent)
        {
            EventPublisher.Publish(channelPartner, customerRegisterEvent);
        }


        private MGIContext PrepareCXNContext(Guid locationId, long cxeCustomerId, PtnrCustomer ptnrCustomer, MGIContext mgiContext)
        {
            List<Location> location = _locationSvc.GetAll();

            Location thisLocation = location.Where(s => s.rowguid == locationId).FirstOrDefault();

            mgiContext.CXECustomerId = cxeCustomerId;
            mgiContext.ProviderId = GetProviderId(thisLocation.ChannelPartnerId);

            if (!mgiContext.Context.ContainsKey("PTNRCustomer"))
            {
                mgiContext.Context.Add("PTNRCustomer", ptnrCustomer);
            }
            return mgiContext;
        }

        public List<CustomerSearchResult> Search(long agentSessionId, CustomerSearchCriteriaDTO criteria, MGIContext mgiContext)
        {
            try
            {
                CXECustomerSearchCriteria cxeCriteria = Mapper.Map<CustomerSearchCriteriaDTO, CXECustomerSearchCriteria>(criteria);
                if (criteria.DateOfBirth == DateTime.MinValue)
                    cxeCriteria.DateOfBirth = null;

                string channelPartnerName = mgiContext.ChannelPartnerName;
                long channelPartnerId = mgiContext.ChannelPartnerId;
                List<CXECustomer> customers = new List<CXECustomer>();
                //List<CXECustomer> channelPartnerCustomers = new List<CXECustomer>();
                //if cardnumber specified, look it up in cxn funds

                cxeCriteria.LastName = !string.IsNullOrWhiteSpace(cxeCriteria.LastName) ? cxeCriteria.LastName.Replace("''", "'") : null;

                if (!string.IsNullOrWhiteSpace(criteria.CardNumber))
                {
                    //Code has changed to consider ProviderId instead of HardCoded FirstViewID
                    IFundProcessor fundProcessor = _GetProcessor(channelPartnerName);
                    int providerId = (int)Enum.Parse(typeof(ProviderIds), _GetFundProvider(channelPartnerName));
                    long cxnAccountId = fundProcessor.GetPanForCardNumber(criteria.CardNumber.Trim(), mgiContext);
                    //AL-1955 : As Synovus, Handle VISA cards update for existing customers in Alloy
                    if (cxnAccountId == 0)
                    {
                        CardAccount cardAccount = new CardAccount()
                        {
                            CardNumber = criteria.CardNumber
                        };
                        bool isNewCard = true;
                        // isNewcard is used for enrolling new Visa DPS Card for exsisting Card Customer, if its not used then it will use associate card functionality
                        cxnAccountId = fundProcessor.AssociateCard(cardAccount, mgiContext, isNewCard);
                        if (cxnAccountId == 0 && cxnAccountId != -1)
                            throw new FundException(FundException.CARD_MAPPING_ERROR, "Please enter valid card number or, if correct, register customer and/or associate card");
                    }
                    long alloyId = _ptnrCustomerService.LookupByCXNAccountId(cxnAccountId, providerId).Id;
                    CXECustomer customer = _cxeCustomerService.Lookup(alloyId);
                    customers.Add(customer);
                }
                else
                    customers = _cxeCustomerService.Lookup(cxeCriteria);

                List<CXECustomer> channelPartnerCustomers = customers.Where(x => x.ChannelPartnerId == channelPartnerId).ToList();

                // mapping is setup with AfterMap to pull in Cardnumber, if available
                List<CustomerSearchResult> searchResults = Mapper.Map<List<CXECustomer>, List<CustomerSearchResult>>(channelPartnerCustomers);

                //Ordering is done after mapping, since Profile status is retrieved during mapping
                return searchResults.OrderBy(a => a.ProfileStatus).ThenBy(a => a.FullName).ToList();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Host API"))
                {
                    throw new Exception(ex.InnerException.Message, ex);
                }
                else
                    throw ex;
            }
        }

        public CustomerSessionDTO InitiateCustomerSession(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            #region AL-1071 Transactional Log class for MO flow
            string id = "AlloyId:" + Convert.ToString(alloyId);

            MongoDBLogger.Info<string>(0, id, "InitiateCustomerSession", AlloyLayerName.BIZ,
                ModuleName.Customer, "Begin InitiateCustomerSession - MGI.Biz.Customer.Impl.CustomerServiceCoreWrapper",
                mgiContext);
            #endregion


            //US1458	
            int cardPresentedType = 3;
            bool _CardPresent = false;
            if (mgiContext.CardPresentedType != 0)
            {
                cardPresentedType = mgiContext.CardPresentedType;
            }
            Customer.Data.CardSearchType cardSearchType = (Customer.Data.CardSearchType)cardPresentedType;

            if (agentSessionId == 0 && (mgiContext.ApplicationName == "DMS-Server"))
                throw new BizCustomerException(BizCustomerException.INVALID_SESSION_ID, string.Format("SessionId {0} could not be parsed.", agentSessionId));

            AgentSession agtSession = _agtSessionService.Lookup(agentSessionId);

            PtnrCustomer partnerCustomer = _ptnrCustomerService.Lookup(alloyId);

            //US1458 
            ChannelPartner channelPartner = _channelPartnerService.ChannelPartnerConfig(partnerCustomer.ChannelPartnerId);

            if ((channelPartner.CardPresenceVerificationConfig == 1) && cardSearchType == Customer.Data.CardSearchType.Swipe)
                _CardPresent = true;
            else if ((channelPartner.CardPresenceVerificationConfig == 2) && (cardSearchType == Customer.Data.CardSearchType.Enter || cardSearchType == Customer.Data.CardSearchType.Swipe))
                _CardPresent = true;

            PtnrCustomerSession partnerCustSession = _custSessionService.Create(agtSession, partnerCustomer, _CardPresent, mgiContext.TimeZone);

            if (agtSession != null)
            {
                //Unpark transactions
                UnparkTransactions(partnerCustSession);

                if (partnerCustSession.ActiveShoppingCart != null)
                    _GetMessageCenterForParkedTransactions(partnerCustSession);
            }
            CustomerSessionDTO customerSession = new CustomerSessionDTO();
            customerSession.CustomerSessionId = partnerCustSession.Id.ToString();
            customerSession.CardPresent = _CardPresent;
            customerSession.Customer = getBizCustomer(_cxeCustomerService.Lookup(alloyId), partnerCustomer);

            //_complianceSvc.VerifyCustomer(alloyId, channelPartner.ComplianceProgramName, new Dictionary<string, object>());

            #region AL-1071 Transactional Log class for MO flow
            MongoDBLogger.Info<PtnrCustomer>(0, partnerCustomer, "InitiateCustomerSession", AlloyLayerName.BIZ,
                ModuleName.Customer, "End InitiateCustomerSession - MGI.Biz.Customer.Impl.CustomerServiceCoreWrapper",
                mgiContext);
            #endregion

            return customerSession;
        }

        private void UnparkTransactions(PtnrCustomerSession partnerCustSession)
        {
            // Get permission required to perform un-parking of transaction from Permission Types Enum
            string permissionRequired = PermissionTypes.CanUnparkTransactions.ToString();
            int userId;
            // Convert agentId string to integer
            bool result = Int32.TryParse(partnerCustSession.AgentSession.Agent.Id.ToString(), out userId);

            if (result && partnerCustSession.Customer.CustomerProfileStatus != ProfileStatus.Closed)
            {
                // Check if the user has permission to do un-parking and then proceed
                if (ManageUserSvc.HasPermission(userId, permissionRequired))
                {
                    // US1488 Parking Transaction Changes
                    _custSessionService.GetParkingShoppingCart(partnerCustSession);
                }
            }
        }

        //A private method to Get Fund Provider based on ChannelPartner
        private string _GetFundProvider(string channelPartnerName)
        {
            // get the fund provider for the channel partner.
            return ProcessorSvc.GetProvider(channelPartnerName);
        }

        private void _GetMessageCenterForParkedTransactions(PtnrCustomerSession partnerCustSession)
        {
            foreach (var transaction in partnerCustSession.ActiveShoppingCart.ShoppingCartTransactions)
            {
                if (transaction.Transaction.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Pending)
                {
                    AgentMessage agentMessage = MessageCenterService.Lookup(transaction.Transaction);
                    if (!agentMessage.IsActive)
                    {
                        agentMessage.IsActive = true;
                        MessageCenterService.Update(agentMessage);
                    }
                }
            }
        }

        public CustomerDTO GetCustomer(long customerSessionId, long alloyId, MGIContext mgiContext)
        {
            return getBizCustomer(_cxeCustomerService.Lookup(alloyId), _ptnrCustomerService.Lookup(alloyId));
        }

        public void SaveCustomer(long agentSessionId, long alloyId, CustomerDTO customerDTO, MGIContext mgiContext)
        {
            CXECustomer cxeCustomer = new CXECustomer();

            //Changes for timestamp
            AgentSession agentsession = _agtSessionService.Lookup(agentSessionId);

            cxeCustomer = Mapper.Map<CustomerProfileDTO, CXECustomer>(customerDTO.Profile);
            cxeCustomer.Id = alloyId;
            if (customerDTO.Employment != null)
                cxeCustomer.EmploymentDetails = Mapper.Map<EmploymentDetails, CustomerEmploymentDetails>(customerDTO.Employment);

            if (customerDTO.ID != null)
            {
                NexxoIdType idType = new NexxoIdType();
                if (!string.IsNullOrEmpty(customerDTO.ID.Country) && !string.IsNullOrEmpty(customerDTO.ID.IDType))
                {
                    idType = _ptnrIdTypeService.Find(cxeCustomer.ChannelPartnerId, customerDTO.ID.IDType, customerDTO.ID.Country, customerDTO.ID.State);
                    if (idType == null)
                        throw new MGI.Biz.Customer.Contract.BizCustomerException(BizCustomerException.INVALID_CUSTOMER_DATA_ID_TYPE_NOT_FOUND, string.Format("Could not find Identification Type {0} {1}", customerDTO.ID.Country, customerDTO.ID.IDType));
                }
                DateTime? expirationDate;
                if (customerDTO.ID.ExpirationDate == DateTime.MinValue)
                    expirationDate = null;
                else
                    expirationDate = customerDTO.ID.ExpirationDate;

                DateTime? issueDate;
                if (customerDTO.ID.IssueDate == DateTime.MinValue)
                    issueDate = null;
                else
                    issueDate = customerDTO.ID.IssueDate;
                cxeCustomer.GovernmentId = new CustomerGovernmentId();
                cxeCustomer.GovernmentId.ExpirationDate = expirationDate;
                cxeCustomer.GovernmentId.Identification = customerDTO.ID.GovernmentId;
                cxeCustomer.GovernmentId.IssueDate = issueDate;
                if (idType != null)
                    cxeCustomer.GovernmentId.IdTypeId = idType.Id;

                cxeCustomer.CountryOfBirth = customerDTO.ID.CountryOfBirth;
            }
            cxeCustomer.IDCode = NexxoUtil.GetIDCode(customerDTO.Profile.SSN);
            cxeCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(agentsession.Terminal.Location.TimezoneID);
            cxeCustomer.DTServerLastModified = DateTime.Now;
            _cxeCustomerService.Save(cxeCustomer);
            //PtnrCustomer ptnrCustomer = _ptnrCustomerService.Create(alloyId, PTNRProspect.IsAccountHolder, PTNRProspect.ReferralCode, PTNRProspect.ChannelPartnerId, cxeCustomer.rowguid);
            PtnrCustomer ptnrCustomer = _ptnrCustomerService.LookupByCxeId(cxeCustomer.Id);
            ptnrCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(agentsession.Terminal.Location.TimezoneID);
            ptnrCustomer.AgentSessionId = agentsession.rowguid;
            ptnrCustomer.CustomerProfileStatus = cxeCustomer.ProfileStatus;
            ptnrCustomer.ReferralCode = customerDTO.Profile.ReferralCode;
            ptnrCustomer.IsPartnerAccountHolder = customerDTO.Profile.IsPartnerAccountHolder;
            _ptnrCustomerService.Update(ptnrCustomer);
            //PtnrCustomer partnerCustomer = _ptnrCustomerService.Lookup( alloyId );
            //ChannelPartner channelPartner = _channelPartnerService.ChannelPartnerConfig( partnerCustomer.ChannelPartnerId );

            PtnrCustomer partnerCustomer = _ptnrCustomerService.Lookup(alloyId);

            if (customerDTO.Groups.Count > 0)
            {
                List<ChannelPartnerGroup> cpGroups = _ptnrGroupSvc.GetAll(partnerCustomer.ChannelPartnerId);
                List<string> ptnrCustomerGroups = partnerCustomer.Groups.Select(g => g.channelPartnerGroup.Name).ToList();

                // get list of groups that need to be added and removed
                var toAdd = customerDTO.Groups.Except(ptnrCustomerGroups);
                var toRemove = ptnrCustomerGroups.Except(customerDTO.Groups);

                // remove any group settings for groups in "toRemove"
                partnerCustomer.Groups.ToList().RemoveAll(m => toRemove.Contains(m.channelPartnerGroup.Name));

                // remove any group settings for groups in "toRemove"
                List<CustomerGroupSetting> groupSettingsList = partnerCustomer.Groups.ToList();

                foreach (CustomerGroupSetting g in groupSettingsList)
                {
                    if (toRemove.Contains(g.channelPartnerGroup.Name))
                        partnerCustomer.Groups.Remove(g);
                }

                // add group settings for groups in "toAdd"
                toAdd.ToList().ForEach(m => partnerCustomer.AddtoGroup(cpGroups.Find(g => g.Name == m)));
                partnerCustomer.Groups.ToList().ForEach(x => x.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(agentsession.Terminal.Location.TimezoneID));
                partnerCustomer.Groups.ToList().ForEach(x => x.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(agentsession.Terminal.Location.TimezoneID));
            }
            else if (partnerCustomer.Groups.Count > 0)
            {
                // if there are existing group settings that need to be removed
                partnerCustomer.Groups.Clear();
            }

            //_complianceSvc.ClearCustomerUpdate( alloyId, channelPartner.ComplianceProgramName, new Dictionary<string, object>() );
        }

        public CustomerDTO Get(long agentSessionId, string Phone, string PIN, MGIContext mgiContext)
        {
            long cxeId = _cxeCustomerService.Get(Phone, PIN);
            return getBizCustomer(_cxeCustomerService.Lookup(cxeId), _ptnrCustomerService.Lookup(cxeId));
        }


        public CustomerDTO GetCustomerForCardNumber(long agentSessionId, string CardNumber, MGIContext mgiContext)
        {
            var channelPartnerName = mgiContext.ChannelPartnerName;

            IFundProcessor fundProcessor = _GetProcessor(channelPartnerName);
            //Changes to consider ProviderId for Lookup CXN Account instead of hardcoded providerId
            int providerId = (int)Enum.Parse(typeof(ProviderIds), _GetFundProvider(channelPartnerName));
            long cxnAccountId = fundProcessor.GetPanForCardNumber(CardNumber.Trim(), mgiContext);
            long cxeId = _ptnrCustomerService.LookupByCXNAccountId(cxnAccountId, providerId).Id;

            CardAccount cardAccount = fundProcessor.Lookup(cxnAccountId);

            CustomerDTO customerdto = getBizCustomer(_cxeCustomerService.Lookup(cxeId), _ptnrCustomerService.Lookup(cxeId));

            customerdto.Profile.CardNumber = CardNumber;
            customerdto.Profile.AccountNumber = cardAccount.AccountNumber;

            return customerdto;
        }

        public bool ValidateSSN(long agentSessionId, string SSN, MGIContext mgiContext)
        {
            CXECustomerSearchCriteria cxeCriteria = new CXECustomerSearchCriteria()
            {
                SSN = SSN
            };

            List<CXECustomer> customers = new List<CXECustomer>();

            customers = _cxeCustomerService.Lookup(cxeCriteria);

            if (mgiContext.AlloyId != 0)
            {
                customers.RemoveAll(x => x.Id == mgiContext.AlloyId);
            }
            long channelPartnerId = mgiContext.ChannelPartnerId;

            ChannelPartner channelPartner = _channelPartnerService.ChannelPartnerConfig(channelPartnerId);

            string masterSSN = channelPartner.ChannelPartnerConfig.MasterSSN;
            if (SSN != masterSSN)
            {
                //if (customers.Where(x => x.ChannelPartnerId == channelPartnerId).Count()>0)
                if (customers.Where(x => x.ChannelPartnerId == channelPartnerId && x.ProfileStatus != ProfileStatus.Closed).Count() > 0)//AL-232
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validates last 4 digits of customer's SSN
        /// </summary>
        /// <param name="customerSessionId">Customer Session Id</param>
        /// <param name="last4DigitsOfSSN">Last 4 digits of customer SSN</param>
        /// <returns></returns>
        public bool IsValidSSN(long customerSessionId, string last4DigitsOfSSN, MGIContext mgiContext)
        {
            PtnrCustomerSession customerSession = _custSessionService.Lookup(customerSessionId);

            CXECustomer customer = _cxeCustomerService.Lookup(customerSession.Customer.Id);

            if (string.IsNullOrEmpty(customer.SSN) || customer.SSN.Length < 4)
            {
                return false;
            }
            else
            {
                return customer.SSN.Substring(customer.SSN.Length - 4) == last4DigitsOfSSN ? true : false;
            }
        }


        /// <summary>
        /// Get the Customer by customerLookUpCriteria
        /// </summary>
        /// <param name="customerLookUpCriteria"></param>
        /// <param name="mgiContext"></param>
        /// <returns></returns>
        public List<CustomerDTO> CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext mgiContext)
        {
            List<CustomerDTO> customerdtolist;

            string channelPartnerName = mgiContext.ChannelPartnerName;

            customerdtolist = null;

            ICustomerRepository _clientCustomerService = _GetCustomerProcessor(channelPartnerName);
            if (_clientCustomerService != null)
            {
                customerdtolist = _clientCustomerService.FetchAll(agentSessionId, customerLookUpCriteria, mgiContext);
                MapStates(customerdtolist);

            }

            if (customerdtolist != null)
            {
                customerdtolist.Sort(delegate(CustomerDTO a, CustomerDTO b)
                {
                    if (a.Profile.FirstName == null && b.Profile.FirstName == null) return 0;
                    else if (a.Profile.FirstName == null) return -1;
                    else if (b.Profile.FirstName == null) return 1;
                    else return a.Profile.FirstName.CompareTo(b.Profile.FirstName);
                });
            }
            return customerdtolist;

        }

        /// <summary>
        /// Maping States to List of Customers
        /// </summary>
        /// <param name="customerdtolist"></param>
        private void MapStates(List<CustomerDTO> customerdtolist)
        {

            foreach (var customerdto in customerdtolist)
            {
                if (customerdto.ID != null && !string.IsNullOrWhiteSpace(customerdto.ID.Country) && !string.IsNullOrWhiteSpace(customerdto.ID.State))
                {
                    string state = _ptnrIdTypeService.GetIDState(customerdto.ID.Country, customerdto.ID.State);
                    if (!string.IsNullOrWhiteSpace(state))
                    {
                        customerdto.ID.State = state;
                    }
                }
            }
        }

        private void MapStates(CustomerDTO customerdto)
        {
            if (customerdto.ID != null && !string.IsNullOrWhiteSpace(customerdto.ID.Country) && !string.IsNullOrWhiteSpace(customerdto.ID.State))
            {
                string state = _ptnrIdTypeService.GetIDState(customerdto.ID.Country, customerdto.ID.State);
                if (!string.IsNullOrWhiteSpace(state))
                {
                    customerdto.ID.State = state;
                }
            }
        }

        private CustomerDTO getBizCustomer(CXECustomer cxeCustomer, PtnrCustomer partnerCustomer)
        {
            if (cxeCustomer == null || partnerCustomer == null)
                return null;
            CustomerDTO customerDTO = new CustomerDTO
            {
                Profile = Mapper.Map<Biz.Customer.Data.CustomerProfile>(cxeCustomer),
                Employment = cxeCustomer.EmploymentDetails != null ? Mapper.Map<EmploymentDetails>(cxeCustomer.EmploymentDetails) : null,
                ID = cxeCustomer.GovernmentId != null ? Mapper.Map<Identification>(cxeCustomer.GovernmentId) : null,
                Groups = partnerCustomer.Groups.Select(g => g.channelPartnerGroup.Name).ToList()
            };

            customerDTO.Profile.IsPartnerAccountHolder = partnerCustomer.IsPartnerAccountHolder;
            customerDTO.Profile.ReferralCode = partnerCustomer.ReferralCode;
            // 2080 Changes
            if (cxeCustomer.GovernmentId != null)
            {
                NexxoIdType idType = _ptnrIdTypeService.Find(cxeCustomer.ChannelPartnerId, cxeCustomer.GovernmentId.IdTypeId);
                if (idType != null)
                {
                    customerDTO.ID.Country = idType.CountryId.Name;
                    customerDTO.ID.IDType = idType.Name;
                    if (idType.StateId != null)
                        customerDTO.ID.State = idType.StateId.Name;
                }
            }
            customerDTO.Profile.CIN = cxeCustomer.Id;
            return customerDTO;
        }

        public long GetAnonymousUserPAN(long agentSessionId, long channelPartnerId, string firstName, string lastName, MGIContext mgiContext)
        {
            CXECustomer cxeCustomer = _cxeCustomerService.Lookup(channelPartnerId, firstName, lastName);
            if (cxeCustomer == null || cxeCustomer.Id < 1)
                throw new BizCustomerException(BizCustomerException.ANONYMOUS_CUSTOMER_NOT_EXISTS, "Anonymous Customer does not Exists");
            return cxeCustomer.Id;
        }

        /// <summary>
        /// AL-231 Checking whether user can able to change profile status
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="profileStatus"></param>
        /// <returns></returns>
        public bool CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGIContext mgiContext)
        {
            bool hasPermission = false;
            UserDetails userDetails = ManageUserSvc.GetUser(Convert.ToInt32(userId));
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

        public void ValidateCustomerStatus(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            CXECustomer cxeCustomer = _cxeCustomerService.Lookup(alloyId);

            _cxeCustomerService.ValidateStatus(alloyId);

            string channelPartnerName = mgiContext.ChannelPartnerName;
            long channelPartnerId = mgiContext.ChannelPartnerId;

            ICustomerRepository _clientCustomerService = _GetCustomerProcessor(channelPartnerName);
            if (_clientCustomerService != null)
            {
                PtnrCustomer ptnrCustomer = _ptnrCustomerService.Lookup(alloyId);
                long cxnAccountId = 0;
                int providerId = 0;

                providerId = GetProviderId(channelPartnerId);

                cxnAccountId = (ptnrCustomer.GetAccount(providerId) == null) ? 0 : ptnrCustomer.GetAccount(providerId).CXNId;

                //_clientCustomerService.GetClientProfileStatus(cxnAccountId, cxnContext);

                _clientCustomerService.ValidateCustomerStatus(agentSessionId, cxnAccountId, mgiContext);
            }
        }

        public void RegisterToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            long channelPartnerId = mgiContext.ChannelPartnerId;

            string channelPartnerName = mgiContext.ChannelPartnerName;

            CXECustomer cxeCustomer = _cxeCustomerService.Lookup(alloyId);

            PtnrCustomer ptnrCustomer = _ptnrCustomerService.Lookup(alloyId);

            mgiContext = PrepareCXNContext(mgiContext.LocationRowGuid, cxeCustomer.Id, ptnrCustomer, mgiContext);

            PublishEvent(channelPartnerName, new CustomerRegistrationEvent()
            {
                profile = Mapper.Map<CXECustomer, Biz.Customer.Data.CustomerProfile>(cxeCustomer),
                mgiContext = mgiContext
            });
        }

        public void UpdateCustomerToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            long channelPartnerId = mgiContext.ChannelPartnerId;

            string channelPartnerName = mgiContext.ChannelPartnerName;

            CXECustomer cxeCustomer = _cxeCustomerService.Lookup(alloyId);

            PtnrCustomer ptnrCustomer = _ptnrCustomerService.Lookup(alloyId);

            Biz.Customer.Data.CustomerProfile customerProfile = Mapper.Map<Biz.Customer.Data.CustomerProfile>(cxeCustomer);
            customerProfile.BankId = mgiContext.BankId;
            customerProfile.BranchId = Convert.ToString(mgiContext.BranchId);

            mgiContext = PrepareCXNContext(mgiContext.LocationRowGuid, cxeCustomer.Id, ptnrCustomer, mgiContext);
            PublishEvent(channelPartnerName, new CustomerEditEvent()
                                                 {
                                                     profile = customerProfile,
                                                     mgiContext = mgiContext
                                                 });

        }

        public void CustomerSyncInFromClient(long agentSessionId, long cxeCustomerId, MGIContext mgiContext)
        {
            string channelPartnerName = mgiContext.ChannelPartnerName;

            CXECustomer cxeCustomer = _cxeCustomerService.Lookup(cxeCustomerId);

            PtnrCustomer ptnrCustomer = _ptnrCustomerService.Lookup(cxeCustomerId);

            MGI.Core.Partner.Data.Account cxnCustomer = ptnrCustomer.Accounts.FirstOrDefault(c => c.CXEId == cxeCustomer.Id);

            if (cxnCustomer != null && ptnrCustomer.CustomerProfileStatus != ProfileStatus.Closed && cxeCustomer.ProfileStatus != ProfileStatus.Closed)
            {
                mgiContext = PrepareCXNContext(mgiContext.LocationRowGuid, cxeCustomer.Id, ptnrCustomer, mgiContext);
                PublishEvent(channelPartnerName, new CustomerSyncInEvent()
                {
                    mgiContext = mgiContext,
                    cxnCustomerId = cxnCustomer.CXNId
                });
            }
        }

        private void PublishEvent(string channelPartner, CustomerSyncInEvent customerSyncInEvent)
        {
            EventPublisher.Publish(channelPartner, customerSyncInEvent);
        }

        private void PublishEvent(string channelPartner, CustomerEditEvent customerEditEvent)
        {
            EventPublisher.Publish(channelPartner, customerEditEvent);
        }

        public ProfileStatus GetClientProfileStatus(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            long channelPartnerId = mgiContext.ChannelPartnerId;
            string channelPartnerName = mgiContext.ChannelPartnerName;

            ICustomerRepository _clientCustomerService = _GetCustomerProcessor(channelPartnerName);
            if (_clientCustomerService != null)
            {
                PtnrCustomer ptnrCustomer = _ptnrCustomerService.Lookup(alloyId);
                long cxnAccountId = 0;
                int providerId = 0;

                providerId = GetProviderId(channelPartnerId);

                cxnAccountId = (ptnrCustomer.GetAccount(providerId) == null)
                                   ? 0
                                   : ptnrCustomer.GetAccount(providerId).CXNId;

                return _clientCustomerService.GetClientProfileStatus(agentSessionId, cxnAccountId, mgiContext);
            }
            else
            {
                return ProfileStatus.Active;
            }
        }

        public bool ValidateCustomer(long agentSessionId, Biz.Customer.Data.Customer customer, MGIContext mgiContext)
        {
            string channelPartnerName = mgiContext.ChannelPartnerName;

            ICustomerRepository _clientCustomerService = _GetCustomerProcessor(channelPartnerName);
            if (_clientCustomerService != null)
            {
                return _clientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
            }
            else
            {
                return false;
            }
        }

        private int GetProviderId(long channelPartnerId)
        {
            int providerId = 0;

            //TODO: This logic to be replaced once Channel partner VS Provider mapping is implemented 
            switch (channelPartnerId)
            {
                case 33://Synovus
                    providerId = (int)ProviderIds.FIS;
                    break;
                case 28://Carver
                    providerId = (int)ProviderIds.CCISCustomer;
                    break;
                case 34://TCF
                    providerId = (int)ProviderIds.TCISCustomer;
                    break;
            }
            return providerId;
        }
    }
}
