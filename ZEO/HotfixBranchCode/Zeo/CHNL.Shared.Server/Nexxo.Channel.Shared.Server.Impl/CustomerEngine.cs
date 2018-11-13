using System;
using System.Collections.Generic;

using AutoMapper;
using MGI.Channel.Shared.Server.Data;


// biz customer 
using Spring.Transaction.Interceptor;
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
using Customer = MGI.Channel.Shared.Server.Data.Customer;
using Identification = MGI.Channel.Shared.Server.Data.Identification;
using SessionContextDTO = MGI.Channel.Shared.Server.Data.SessionContext;
using CustomerSessionDTO = MGI.Channel.Shared.Server.Data.CustomerSession;
using FundsAccountDTO = MGI.Channel.Shared.Server.Data.FundsProcessorAccount;
using MGI.Biz.Compliance.Contract;
using MGI.Channel.Shared.Server.Contract;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;
namespace MGI.Channel.Shared.Server.Impl
{
	/// <summary>
	/// This Class Implements ICustomerService 
	/// </summary>
	public partial class SharedEngine : ICustomerService
	{
		#region Injected Services
		public bizProspectService BizProspectService { get; set; }
		public bizCustomerService BizCustomerService { get; set; }
		public ILimitService LimitService { get; set; }
		#endregion

		#region Customer Data Mapper
		//Icustomer related mapper
		internal static void CustomerConverter()
		{
			Mapper.CreateMap<Prospect, bizProspect>();
			Mapper.CreateMap<Identification, MGI.Biz.Partner.Data.Identification>();
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
						d.PersonalInformation = Mapper.Map<PersonalInformation>(s.Profile); d.Address = new Address()
	{
		Address1 = s.Profile.Address1,
		Address2 = s.Profile.Address2,
		City = s.Profile.City,
		PostalCode = s.Profile.ZipCode,
		State = s.Profile.State
	};
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

			Mapper.CreateMap<bizProspect, Prospect>()
			.AfterMap((s, d) =>
			{
				if (s.ID != null)
				{
					d.ID = new Identification()
					{
						Country = s.ID.Country,
						ExpirationDate = s.ID.ExpirationDate,
						GovernmentId = s.ID.GovernmentId,
						IDType = s.ID.IDType,
						IssueDate = s.ID.IssueDate ?? DateTime.MinValue,
						State = s.ID.State
					};
				}
			})
			
			 .AfterMap((s, d) =>
			 {
				 if (d.ID != null)
					 d.ID.CountryOfBirth = s.ID.CountryOfBirth;
			 })
		   .ForMember(x => x.ID, o => o.Ignore());

			Mapper.CreateMap<Prospect, bizCustomer>()
				.AfterMap((s, d) =>
				{
					d.Profile = new bizCustomerProfile()
					{
						Address1 = s.Address1,
						Address2 = s.Address2,
						City = s.City,
						DateOfBirth = s.DateOfBirth,
						DoNotCall = s.DoNotCall,
						Email = s.Email,
						FirstName = s.FName,
						Gender = s.Gender,
						IsPartnerAccountHolder = s.IsAccountHolder,
						LastName = s.LName,
						LastName2 = s.LName2,
						MailingAddress1 = s.MailingAddress1,
						MailingAddress2 = s.MailingAddress2,
						MailingAddressDifferent = s.MailingAddressDifferent,
						MailingCity = s.MailingCity,
						MailingState = s.MailingState,
						MailingZipCode = s.MailingZipCode,
						MiddleName = s.MName,
						MothersMaidenName = s.MoMaName,
						Phone1 = s.Phone1,
						Phone1Provider = s.Phone1Provider,
						Phone1Type = s.Phone1Type,
						Phone2 = s.Phone2,
						Phone2Provider = s.Phone2Provider,
						Phone2Type = s.Phone2Type,
						PIN = s.PIN,
						ReferralCode = s.ReferralCode,
						SMSEnabled = s.TextMsgOptIn,
						SSN = s.SSN,
						State = s.State,
						ZipCode = s.PostalCode,
						ReceiptLanguage = s.ReceiptLanguage,
						ProfileStatus = s.ProfileStatus,
						PartnerAccountNumber = s.PartnerAccountNumber,
						RelationshipAccountNumber = s.RelationshipAccountNumber,
						BankId = s.BankId,
						BranchId = s.BranchId,
						ClientID = s.ClientID,
						LegalCode = s.LegalCode,
						PrimaryCountryCitizenship = s.PrimaryCountryCitizenShip,
						SecondaryCountryCitizenship = s.SecondaryCountryCitizenShip,
						Notes = s.Notes
					};
				})
				.AfterMap((s, d) =>
				{
					d.Employment = new bizEmployment()
					{
						Employer = s.Employer,
						EmployerPhone = s.EmployerPhone,
						Occupation = s.Occupation,
						OccupationDescription = s.OccupationDescription
					};
				})
				.AfterMap((s, d) =>
				{
					if (s.ID != null)
					{
						d.ID = new bizIdentification()
						{
							Country = s.ID.Country,
							ExpirationDate = s.ID.ExpirationDate,
							GovernmentId = s.ID.GovernmentId,
							IDType = s.ID.IDType,
							IssueDate = s.ID.IssueDate,
							State = s.ID.State,
							CountryOfBirth = s.ID.CountryOfBirth
						};
					}
				});

			Mapper.CreateMap<SessionContextDTO, bizPartnerSessionContext>();
			Mapper.CreateMap<SessionContextDTO, bizCustomerSessionContext>();
		}
		#endregion

		#region Consumer ICustomerService Impl

		public long CreateProspect(long agentSessionId, Prospect prospect, SessionContextDTO contextDTO, MGIContext context)
		{
			bizProspect bizProspectDto = Mapper.Map<Prospect, bizProspect>(prospect);

			bizPartnerSessionContext sessionContext = Mapper.Map<SessionContextDTO, bizPartnerSessionContext>(contextDTO);

			return Convert.ToInt64(BizProspectService.SaveProspect(agentSessionId, sessionContext, bizProspectDto, context));
		}

		public void SaveProspect(long agentSessionId, long alloyId, Prospect prospect, SessionContextDTO contextDTO, MGIContext context)
		{
			bizProspect bizProspectDto = Mapper.Map<Prospect, bizProspect>(prospect);

			bizPartnerSessionContext sessionContext = Mapper.Map<SessionContextDTO, bizPartnerSessionContext>(contextDTO);

			BizProspectService.SaveProspect(agentSessionId, sessionContext, alloyId, bizProspectDto, context);
		}

		public void NexxoActivate(long agentSessionId, long alloyId, SessionContextDTO contextDTO, MGIContext context)
		{
			bizCustomerSessionContext sessionContext = Mapper.Map<SessionContextDTO, bizCustomerSessionContext>(contextDTO);
			BizCustomerService.Register(agentSessionId, sessionContext, alloyId, context);
		}

		public void ClientActivate(long agentSessionId, long alloyId, MGIContext context)
		{
			BizCustomerService.RegisterToClient(agentSessionId, alloyId, context);
			//ComplianceService.ClearNewCustomer(alloyId, context);    
		}

		public void UpdateCustomer(long agentSessionId, long alloyId, Prospect prospect, MGIContext context)
		{
			bizCustomer customer = Mapper.Map<Prospect, bizCustomer>(prospect);
			customer.Profile.ChannelPartnerId = Convert.ToInt32(ChannelPartnerService.ChannelPartnerConfig(prospect.ChannelPartnerId, context).Id);
			BizCustomerService.SaveCustomer(agentSessionId, alloyId, customer, context);
			//ComplianceService.ClearCustomerUpdate(alloyId, new Dictionary<string, string>());
		}

		public void UpdateCustomerToClient(long agentSessionId, long alloyId, MGIContext context)
		{
			BizCustomerService.UpdateCustomerToClient(agentSessionId, alloyId, context);
			//ComplianceService.ClearNewCustomer(alloyId, context);
		}

		public void CustomerSyncInFromClient(long agentSessionId, long alloyId, MGIContext context)
		{
			BizCustomerService.CustomerSyncInFromClient(agentSessionId, alloyId, context);
			//ComplianceService.ClearCustomerUpdate(alloyId, new Dictionary<string, string>());
		}


		public CustomerSessionDTO InitiateCustomerSession(long agentSessionId, long alloyId, MGIContext context)
		{
			#region AL-1071 Transactional Log class for MO flow
			string id = "Alloy Id:" + Convert.ToString(alloyId);

			MongoDBLogger.Info<string>(0, id, "InitiateCustomerSession", AlloyLayerName.SERVICE,
				ModuleName.Customer, "Begin InitiateCustomerSession - MGI.Channel.Shared.Server.Impl.CustomerEngine",
				context);
			#endregion

			bizCustomerSession customerSession = BizCustomerService.InitiateCustomerSession(agentSessionId, alloyId, context);
			CustomerSessionDTO sessionDTO = new CustomerSessionDTO();
			sessionDTO.CustomerSessionId = customerSession.CustomerSessionId;
			sessionDTO.CardPresent = customerSession.CardPresent;
			sessionDTO.Customer = Mapper.Map<bizCustomer, Customer>(customerSession.Customer);
			if (customerSession.Customer.ID != null)
				sessionDTO.Customer.ID = Mapper.Map<bizIdentification, Identification>(customerSession.Customer.ID);
			if (customerSession.Customer.Employment != null)
			{
				sessionDTO.Customer.Employment.Occupation = customerSession.Customer.Employment.Occupation;
				sessionDTO.Customer.Employment.Employer = customerSession.Customer.Employment.Employer;
				sessionDTO.Customer.Employment.EmployerPhone = customerSession.Customer.Employment.EmployerPhone;
			}
			sessionDTO.ProfileStatus = customerSession.Customer.Profile.ProfileStatus;
			// AL-591: This is introduced to update IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
			// Developed by: Sunil Shetty || 03/07/2015
			// we are commeneting below line as we are calling it twice one gets called in MGI.Channel.DMS.Server.Impl.InitiateCustomerSession and other one here.
			//getRevisedFeeForParkedTransactions(long.Parse(sessionDTO.CustomerSessionId));


			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<bizCustomerSession>(0, customerSession, "InitiateCustomerSession", AlloyLayerName.SERVICE,
				ModuleName.Customer, "End InitiateCustomerSession - MGI.Channel.Shared.Server.Impl.CustomerEngine",
				context);
			#endregion

			return sessionDTO;
		}

		public Customer GetCustomerByCardNumber(long agentSessionId, string CardNumber, MGIContext context)
		{
			bizCustomer bizCustomer = BizCustomerService.GetCustomerForCardNumber(agentSessionId, CardNumber, context);

			return Mapper.Map<bizCustomer, Customer>(bizCustomer);
		}

		private void getRevisedFeeForParkedTransactions(long customerSessionId)
		{
			MGI.Common.Util.MGIContext mgiContext = new MGI.Common.Util.MGIContext();

			MGI.Biz.Partner.Data.ShoppingCart shoppingcart = BIZShoppingCartService.Get(customerSessionId, mgiContext);

			if (shoppingcart != null)
			{

				var availableChecks = shoppingcart.Checks;

				foreach (var check in availableChecks)
				{
					CPEngineService.Resubmit(customerSessionId, check.Id, mgiContext);
				}

				var availableMoneyOrders = shoppingcart.MoneyOrders;

				foreach (var moneyorder in availableMoneyOrders)
				{
					MoneyOrderEngineService.Resubmit(customerSessionId, moneyorder.Id, mgiContext);
				}

			}
		}

		#endregion

	}
}

