// MS libraries
using System;
using System.Collections.Generic;

// 3rd party libraries
using System.Linq;
using AutoMapper;
using MGI.Biz.Customer.Data;
using Spring.Transaction.Interceptor;

using MGI.Channel.Consumer.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using CustomerSessionDTO = MGI.Channel.Shared.Server.Data.CustomerSession;
using FundsAccountDTO = MGI.Channel.Shared.Server.Data.FundsProcessorAccount;

// biz customer 
using bizCustomerService = MGI.Biz.Customer.Contract.ICustomerService;
using bizProspectService = MGI.Biz.Partner.Contract.ICustomerProspectService;
using bizProspect = MGI.Biz.Partner.Data.Prospect;
using bizPartnerSessionContext = MGI.Biz.Partner.Data.SessionContext;
using bizCustomerSessionContext = MGI.Biz.Customer.Data.SessionContext;
using bizIdentification = MGI.Biz.Customer.Data.Identification;
using bizCustomer = MGI.Biz.Customer.Data.Customer;
using bizCustomerProfile = MGI.Biz.Customer.Data.CustomerProfile;
using bizEmployment = MGI.Biz.Customer.Data.EmploymentDetails;
using bizCustomerSession = MGI.Biz.Customer.Data.CustomerSession;

//Biz Compliance
using MGI.Biz.Compliance.Contract;
using Customer = MGI.Channel.Shared.Server.Data.Customer;
using Identification = MGI.Channel.Shared.Server.Data.Identification;
using sharedSessionContext = MGI.Channel.Shared.Server.Data.SessionContext;


namespace MGI.Channel.Consumer.Server.Impl
{
    /// <summary>
    /// This Class Implements ICustomerService 
    /// </summary>
    public partial class ConsumerEngine : ICustomerService
    {
        #region Injected Services
        public bizCustomerService BizCustomerService { get; set; }
		public NLoggerCommon NLogger = new NLoggerCommon();

        #endregion

        #region Customer Data Mapper
        //Icustomer related mapper
        internal static void CustomerConverter()
        {
            Mapper.CreateMap<bizCustomerProfile, Address>();
            Mapper.CreateMap<bizCustomerProfile, PersonalInformation>()
                .ForMember(d => d.FName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.MName, o => o.MapFrom(s => s.MiddleName))
                .ForMember(d => d.LName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.LName2, o => o.MapFrom(s => s.LastName2))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth == null ? DateTime.MinValue : Convert.ToDateTime(s.DateOfBirth)));
            Mapper.CreateMap<bizCustomerProfile, Preferences>();
            Mapper.CreateMap<bizCustomerProfile, Fund>();
            Mapper.CreateMap<bizEmployment, Employment>();
            Mapper.CreateMap<bizIdentification, Identification>()
                .ForMember(d => d.IssueDate, o => o.MapFrom(s => s.IssueDate == null ? DateTime.MinValue : Convert.ToDateTime(s.IssueDate)));

            Mapper.CreateMap<bizCustomer, Customer>()
                .AfterMap((s, d) =>
                {
                    if (s.Profile != null)
                    {
                        d.CIN = s.Profile.CIN;
                        d.Email = s.Profile.Email;
                        d.SSN = s.Profile.SSN;
                        d.MailingAddressDifferent = s.Profile.MailingAddressDifferent;
                        d.PersonalInformation = Mapper.Map<PersonalInformation>(s.Profile);
                        d.Address = Mapper.Map<Address>(s.Profile);
                        d.Preferences = Mapper.Map<Preferences>(s.Profile);
                        d.Fund = Mapper.Map<Fund>(s.Profile);
                        d.Employment = Mapper.Map<Employment>(s.Employment);
						d.ID = Mapper.Map<Identification>(s.ID);
                        d.MailingAddress = new Address()
                        {
                            Address1 = s.Profile.MailingAddress1,
                            Address2 = s.Profile.MailingAddress2,
                            City = s.Profile.MailingCity,
                            PostalCode = s.Profile.MailingZipCode,
                            State = s.Profile.MailingState
                        };
                        d.Phone1 = new Phone()
                        {
                            Number = s.Profile.Phone1,
                            Provider = s.Profile.Phone1Provider,
                            Type = s.Profile.Phone1Type
                        };
                        d.Phone2 = new Phone()
                        {
                            Number = s.Profile.Phone2,
                            Provider = s.Profile.Phone2Provider,
                            Type = s.Profile.Phone2Type
                        };
                    }

                });
        }
        #endregion

        #region Consumer ICustomerService Impl

        [Transaction()]
		public long CreateProspect(long agentSessionId, Prospect prospect, MGIContext mgiContext)
        {
			return SharedEngine.CreateProspect(agentSessionId, prospect, GetSharedSessionContext(mgiContext), mgiContext);
        }

        [Transaction()]
		public void SaveProspect(long agentSessionId, long alloyId, Prospect prospect, MGIContext mgiContext)
        {
			SharedEngine.SaveProspect(agentSessionId, alloyId, prospect, GetSharedSessionContext(mgiContext), mgiContext);
        }

        [Transaction()]
		public void NexxoActivate(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
			SharedEngine.NexxoActivate(agentSessionId, alloyId, GetSharedSessionContext(mgiContext), mgiContext);
        }

        [Transaction(Spring.Transaction.TransactionPropagation.NotSupported)]
		public void UpdateCustomer(long agentSessionId, long alloyId, Prospect prospect, MGIContext mgiContext)
        {
			SharedEngine.UpdateCustomer(agentSessionId, alloyId, prospect, mgiContext);
        }

        [Transaction()]
		public CustomerSessionDTO InitiateCustomerSession(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
			CustomerSessionDTO sessionDTO = SharedEngine.InitiateCustomerSession(agentSessionId, alloyId, mgiContext);
			/*
			bizCustomerSession customerSession = BizCustomerService.InitiateCustomerSession(string.Empty, alloyId, context);

			CustomerSessionDTO sessionDTO = new CustomerSessionDTO();
			sessionDTO.CustomerSessionId = customerSession.CustomerSessionId;
			sessionDTO.Customer = Mapper.Map<bizCustomer, Customer>(customerSession.Customer);
			if (customerSession.Customer.ID != null)
				sessionDTO.Customer.ID = Mapper.Map<bizIdentification, Identification>(customerSession.Customer.ID);
			if (customerSession.Customer.Employment != null)
			{
				sessionDTO.Customer.Employment.Occupation = customerSession.Customer.Employment.Occupation;
				sessionDTO.Customer.Employment.Employer = customerSession.Cusmer.Employment.Employer;
				sessionDTO.Customer.Employment.EmployerPhone = customerSession.Customer.Employment.EmployerPhone;
			}
			//TODO : Once Fundengin is created following comment has to be replaced
			//FundsAccountDTO fundAccount =  LookupForPAN(Convert.ToInt64(customerSession.CustomerSessionId));
			//if (fundAccount != null)
			//{
			//    sessionDTO.Customer.Fund.CardNumber = fundAccount.CardNumber;
			//    sessionDTO.Customer.Fund.AccountNumber = fundAccount.AccountNumber;
			//    sessionDTO.Customer.Fund.CardBalance = fundAccount.CardBalance;
			//    sessionDTO.Customer.Fund.IsGPRCard = true;
			//}
			//TODO : once Billpay engin is emplemented following commented line has to be replaced
			sessionDTO.Customer.IsWUGoldCard = false;// GetWUCardAccount(long.Parse(customerSession.CustomerSessionId));
			sessionDTO.CardPresent = customerSession.CardPresent;
			sessionDTO.TimezoneID = customerSession.TimezoneID;
			*/
			return sessionDTO;
        }

        [Transaction(Spring.Transaction.TransactionPropagation.NotSupported)]
		public void ClientActivate(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            return;
        }

        [Transaction(Spring.Transaction.TransactionPropagation.NotSupported)]
		public void UpdateCustomerToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
        {
            throw new System.NotImplementedException();
        }
        
        [Transaction(ReadOnly = true)]
		public Prospect CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext mgiContext)
        {
            //TODO Customer pull from Bazil has to be implemented here
            Prospect prospectDto = new Prospect()
            {
                SSN = "457555462",
                FName = "NEXXOMVA",
                MName = "TEST",
                LName = "CARDCUSTOMER",
                LName2 = "",
                Address1 = "111 Anza Blvd",
                Address2 = "",
                City = "Burlingame",
                State = "CA",
                PostalCode = "94010",
                MailingAddressDifferent = false,
                MailingAddress1 = "111 Anza Blvd",
                MailingAddress2 = "",
                MailingCity = "Burlingame",
                MailingState = "CA",
                MailingZipCode = "94010",
                PIN = "6565",
                Phone1 = "6503894140",
                Phone1Type = "Home",
                MoMaName = "MOTHER",
                DateOfBirth = new DateTime(1950, 10, 10),
                //ChannelPartnerId = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),//Synovus
                //ChannelPartnerId = Guid.Parse("6D7E785F-7BDD-42C8-BC49-44536A1885FC"),//TCF
                ChannelPartnerId = Guid.Parse("10F2865B-DBC5-4A0B-983C-62E0A0574354"),//MGI
                ID = new Identification()
                {
                    CountryOfBirth = "US",
                    GovernmentId = "D8574963",
                    IDType = "DRIVER'S LICENSE",
                    Country = "UNITED STATES",
                    State = "CALIFORNIA",
                    ExpirationDate = new DateTime(2020, 10, 10),
                    IssueDate = new DateTime(1990, 10, 10),
                },
                Gender = "MALE",
				ProfileStatus = ProfileStatus.Active,
                Employer = "Nexxo Finatial Corp",
                Occupation = "Engineer",
                EmployerPhone = "",
                BankId = null,
                BranchId = null,
                CardNumber = null,
                DoNotCall = false,
                Email = "mvatest@moneygram.com",
                TextMsgOptIn = false,
                WUGoldCardNumber = null,
                WUSMSNotification = false,
                ReferralCode = string.Empty
            };

            return prospectDto;
        }

        [Transaction(ReadOnly = true)]
		public Customer GetCustomerByCardNumber(long agentSessionId, string CardNumber, MGIContext mgiContext)
        {
            //TODO Get customer by Card number need to be implemented
            try
            {
				Customer customer = SharedEngine.GetCustomerByCardNumber(agentSessionId, CardNumber, mgiContext);
                return customer;
            }
			catch (Exception ex)
			{
				NLogger.Error(string.Format("Error in get get customer by card number : {0} \n Stack Trace: {1}" , ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available"));
				try
				{
					return Mapper.Map<bizCustomer, Customer>(BizCustomerService.Get(agentSessionId, CardNumber, "6565", mgiContext));
				}
				catch (Exception exc)
				{
					NLogger.Error(string.Format("Error in customer data mapping: {0} \n Stack Trace: {1}",exc.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? exc.StackTrace : "No stack trace available"));
				}
			}

            return null;
        }

        [Transaction(ReadOnly = true)]
		public bool IsValidSSN(long customerSessionId, string last4DigitsOfSSN, MGIContext mgiContext)
        {
			return BizCustomerService.IsValidSSN(customerSessionId, last4DigitsOfSSN, mgiContext);
        }
        #endregion

    }
}

