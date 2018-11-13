// MS libraries
using System;
using System.Collections.Generic;

// 3rd party
using AutoMapper;

// Nexxo
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using bizProspectService = MGI.Biz.Partner.Contract.ICustomerProspectService;

using ProspectDTO = MGI.Channel.Shared.Server.Data.Prospect;
using CustomerDTO = MGI.Channel.Shared.Server.Data.Customer;
using IdentificationDTO = MGI.Channel.Shared.Server.Data.Identification;
using SessionContextDTO = MGI.Channel.DMS.Server.Data.SessionContext;
using SharedSessionContextDTO = MGI.Channel.Shared.Server.Data.SessionContext;
using CustomerSearchCriteriaDTO = MGI.Channel.DMS.Server.Data.CustomerSearchCriteria;
using CustomerSearchResultDTO = MGI.Channel.DMS.Server.Data.CustomerSearchResult;
using CustomerSessionDTO = MGI.Channel.Shared.Server.Data.CustomerSession;
using CustomerPersonalInformationDTO = MGI.Channel.Shared.Server.Data.PersonalInformation;
using FundsAccountDTO = MGI.Channel.Shared.Server.Data.FundsProcessorAccount;
using PurseDTO = MGI.Channel.DMS.Server.Data.Purse;

using TransactionHistoryDTO = MGI.Channel.DMS.Server.Data.TransactionHistory;

// biz customer 
using bizCustomerService = MGI.Biz.Customer.Contract.ICustomerService;
using bizProspect = MGI.Biz.Partner.Data.Prospect;
using bizProspectSessionContext = MGI.Biz.Partner.Data.SessionContext;
using bizSessionContext = MGI.Biz.Customer.Data.SessionContext;
using bizIdentification = MGI.Biz.Customer.Data.Identification;
using bizCustomer = MGI.Biz.Customer.Data.Customer;
using bizCustomerProfile = MGI.Biz.Customer.Data.CustomerProfile;
using bizEmployment = MGI.Biz.Customer.Data.EmploymentDetails;
using bizCustomerSearchCriteria = MGI.Biz.Customer.Data.CustomerSearchCriteria;
using bizCustomerSearchResult = MGI.Biz.Customer.Data.CustomerSearchResult;
using bizCustomerSession = MGI.Biz.Customer.Data.CustomerSession;

using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;

using Spring.Transaction.Interceptor;
using ICustomerService = MGI.Channel.DMS.Server.Contract.ICustomerService;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : ICustomerService
	{

		public bizCustomerService BizCustomerService { get; set; }
		public bizProspectService BizProspectService { get; set; }
		public MGI.Biz.MoneyTransfer.Contract.IMoneyTransferEngine bizMoneyTransferEngine { get; set; }

		public ILimitService LimitService { get; set; }

		internal static void CustomerConverter()
		{
			Mapper.CreateMap<SessionContextDTO, SharedSessionContextDTO>();
			Mapper.CreateMap<ProspectDTO, bizProspect>();
			Mapper.CreateMap<IdentificationDTO, MGI.Biz.Partner.Data.Identification>();
			Mapper.CreateMap<SessionContextDTO, bizSessionContext>();
			Mapper.CreateMap<SessionContextDTO, bizProspectSessionContext>();
			Mapper.CreateMap<ProspectDTO, bizCustomerProfile>();

			Mapper.CreateMap<TransactionHistory, TransactionHistoryDTO>();

			Mapper.CreateMap<Identification, IdentificationDTO>();

			Mapper.CreateMap<IdentificationDTO, bizIdentification>();
			Mapper.CreateMap<bizIdentification, IdentificationDTO>();
			Mapper.CreateMap<SessionContextDTO, bizSessionContext>();

			Mapper.CreateMap<CustomerSearchCriteriaDTO, bizCustomerSearchCriteria>();
			Mapper.CreateMap<bizCustomerSearchResult, CustomerSearchResultDTO>();

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

			Mapper.CreateMap<bizCustomer, CustomerDTO>()
				.AfterMap((s, d) =>
				{
					if (s.Profile != null)
					{
						d.Email = s.Profile.Email;
						d.SSN = s.Profile.SSN;
						d.MailingAddressDifferent = s.Profile.MailingAddressDifferent;
						d.PersonalInformation = Mapper.Map<PersonalInformation>(s.Profile);
						d.Address = new Address()
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

			Mapper.CreateMap<bizProspect, ProspectDTO>()
				 .AfterMap((s, d) =>
				 {
					 if (s.ID != null)
					 {
						 d.ID = new IdentificationDTO()
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
						 d.ID.CountryOfBirth = s.ID != null ? s.ID.CountryOfBirth : string.Empty;
				 })
	.ForMember(x => x.ID, o => o.Ignore());

			Mapper.CreateMap<bizIdentification, IdentificationDTO>();

			Mapper.CreateMap<bizCustomer, ProspectDTO>()
				.ForMember(d => d.Address1, o => o.MapFrom(s => s.Profile.Address1))
				.ForMember(d => d.Address2, o => o.MapFrom(s => s.Profile.Address2))
				.ForMember(d => d.City, o => o.MapFrom(s => s.Profile.City))
				.ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.Profile.DateOfBirth))
				.ForMember(d => d.DoNotCall, o => o.MapFrom(s => s.Profile.DoNotCall))
				.ForMember(d => d.Email, o => o.MapFrom(s => s.Profile.Email))
				.ForMember(d => d.FName, o => o.MapFrom(s => s.Profile.FirstName))
				.ForMember(d => d.Gender, o => o.MapFrom(s => s.Profile.Gender))
				.ForMember(d => d.IsAccountHolder, o => o.MapFrom(s => s.Profile.IsPartnerAccountHolder))
				.ForMember(d => d.LName, o => o.MapFrom(s => s.Profile.LastName))
				.ForMember(d => d.LName2, o => o.MapFrom(s => s.Profile.LastName2))
				.ForMember(d => d.MailingAddress1, o => o.MapFrom(s => s.Profile.MailingAddress1))
				.ForMember(d => d.MailingAddress2, o => o.MapFrom(s => s.Profile.MailingAddress2))
				.ForMember(d => d.MailingAddressDifferent, o => o.MapFrom(s => s.Profile.MailingAddressDifferent))
				.ForMember(d => d.MailingCity, o => o.MapFrom(s => s.Profile.MailingCity))
				.ForMember(d => d.MailingState, o => o.MapFrom(s => s.Profile.MailingState))
				.ForMember(d => d.MailingZipCode, o => o.MapFrom(s => s.Profile.MailingZipCode))
				.ForMember(d => d.MName, o => o.MapFrom(s => s.Profile.MiddleName))
				.ForMember(d => d.MoMaName, o => o.MapFrom(s => s.Profile.MothersMaidenName))
				.ForMember(d => d.Phone1, o => o.MapFrom(s => s.Profile.Phone1))
				.ForMember(d => d.Phone1Type, o => o.MapFrom(s => s.Profile.Phone1Type))
				.ForMember(d => d.Phone1Provider, o => o.MapFrom(s => s.Profile.Phone1Provider))
				.ForMember(d => d.Phone2, o => o.MapFrom(s => s.Profile.Phone2))
				.ForMember(d => d.Phone2Type, o => o.MapFrom(s => s.Profile.Phone2Type))
				.ForMember(d => d.Phone2Provider, o => o.MapFrom(s => s.Profile.Phone2Provider))
				.ForMember(d => d.PIN, o => o.MapFrom(s => s.Profile.PIN))
				.ForMember(d => d.ReferralCode, o => o.MapFrom(s => s.Profile.ReferralCode))
				.ForMember(d => d.WUSMSNotification, o => o.MapFrom(s => s.Profile.SMSEnabled))
				.ForMember(d => d.SSN, o => o.MapFrom(s => s.Profile.SSN))
				.ForMember(d => d.State, o => o.MapFrom(s => s.Profile.State))
				.ForMember(d => d.PostalCode, o => o.MapFrom(s => s.Profile.ZipCode))
				.ForMember(d => d.ProfileStatus, o => o.MapFrom(s => s.Profile.ProfileStatus))
				.ForMember(d => d.ReceiptLanguage, o => o.MapFrom(s => s.Profile.ReceiptLanguage))
				.ForMember(d => d.Employer, o => o.MapFrom(s => s.Employment.Employer))
				.ForMember(d => d.EmployerPhone, o => o.MapFrom(s => s.Employment.EmployerPhone))
				.ForMember(d => d.Occupation, o => o.MapFrom(s => s.Employment.Occupation))
				.ForMember(d => d.PartnerAccountNumber, o => o.MapFrom(s => s.Profile.PartnerAccountNumber))
				.ForMember(d => d.RelationshipAccountNumber, o => o.MapFrom(s => s.Profile.RelationshipAccountNumber))
				.ForMember(d => d.BankId, o => o.MapFrom(s => s.Profile.BankId))
				.ForMember(d => d.BranchId, o => o.MapFrom(s => s.Profile.BranchId))
				.ForMember(d => d.ProgramId, o => o.MapFrom(s => s.Profile.ProgramId))
				.ForMember(d => d.LegalCode, o => o.MapFrom(s => s.Profile.LegalCode))
				.ForMember(d => d.PrimaryCountryCitizenShip, o => o.MapFrom(s => s.Profile.PrimaryCountryCitizenship))
				.ForMember(d => d.SecondaryCountryCitizenShip, o => o.MapFrom(s => s.Profile.SecondaryCountryCitizenship))
				.ForMember(d => d.ClientID, o => o.MapFrom(s => s.Profile.ClientID))
				.AfterMap((s, d) =>
				{
					if (s.ID != null)
					{
						d.ID = new Identification()
						{
							Country = s.ID.Country,
							ExpirationDate = s.ID.ExpirationDate == null ? DateTime.MinValue : Convert.ToDateTime(s.ID.ExpirationDate), //s.ID.ExpirationDate,
							GovernmentId = s.ID.GovernmentId,
							IDType = s.ID.IDType,
							IssueDate = s.ID.IssueDate == null ? DateTime.MinValue : Convert.ToDateTime(s.ID.IssueDate),
							State = s.ID.State,
							CountryOfBirth = s.ID.CountryOfBirth
						};
					}
				});

			Mapper.CreateMap<ProspectDTO, bizCustomer>()
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
						LegalCode = s.LegalCode,
						PrimaryCountryCitizenship = s.PrimaryCountryCitizenShip,
						SecondaryCountryCitizenship = s.SecondaryCountryCitizenShip,
						ClientID = s.ClientID,
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
		}


		#region Shared Methods

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Save initial registration data")]
		public String Save(long agentSessionId, ProspectDTO prospectDto, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			SharedSessionContextDTO contextDTO = Mapper.Map<SessionContextDTO, SharedSessionContextDTO>(GetSessionContext(Convert.ToString(agentSessionId)));
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			long alloyId = SharedEngine.CreateProspect(agentSessionId, prospectDto, contextDTO, context);

			return alloyId.ToString();

		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Save registration")]
		public void Save(long agentSessionId, long alloyId, ProspectDTO prospectDto, MGIContext mgiContext)
		{

			SharedSessionContextDTO contextDTO = Mapper.Map<SessionContextDTO, SharedSessionContextDTO>(GetSessionContext(Convert.ToString(agentSessionId)));
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			SharedEngine.SaveProspect(agentSessionId, alloyId, prospectDto, contextDTO, context);

		}

		[Transaction]
		public void NexxoActivate(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			SharedSessionContextDTO contextDTO = Mapper.Map<SessionContextDTO, SharedSessionContextDTO>(GetSessionContext(Convert.ToString(agentSessionId)));
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			SharedEngine.NexxoActivate(agentSessionId, alloyId, contextDTO, context);
		}

		[Transaction(Spring.Transaction.TransactionPropagation.NotSupported)]
		public void ClientActivate(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext serverContext = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			SharedEngine.ClientActivate(agentSessionId, alloyId, serverContext);

		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Update customer profile")]
		public void UpdateCustomer(long agentSessionId, long alloyId, ProspectDTO prospectDTO, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			SharedEngine.UpdateCustomer(agentSessionId, alloyId, prospectDTO, null);
		}

		[Transaction(Spring.Transaction.TransactionPropagation.NotSupported)]
		public void UpdateCustomerToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			SharedEngine.UpdateCustomerToClient(agentSessionId, alloyId, context);
		}

		[Transaction(Spring.Transaction.TransactionPropagation.NotSupported)]
		public void CustomerSyncInFromClient(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			SharedEngine.CustomerSyncInFromClient(agentSessionId, alloyId, context);
		}


		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Begin customer session")]
		public CustomerSessionDTO InitiateCustomerSession(long agentSessionId, CustomerAuthentication authentication, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<CustomerAuthentication>(0, authentication, "InitiateCustomerSession", AlloyLayerName.SERVICE,
				ModuleName.Customer, "Begin InitiateCustomerSession - MGI.Channel.DMS.Server.Impl.CustomerEngine",
				context);
			#endregion

			CustomerSessionDTO sessionDTO = SharedEngine.InitiateCustomerSession(agentSessionId, authentication.AlloyID, context);

			if (_IsAnonymous(sessionDTO.Customer.PersonalInformation.FName, sessionDTO.Customer.PersonalInformation.LName))
			{
				sessionDTO.Customer.IsAnonymous = true;
				return sessionDTO;
			}
			FundsAccountDTO fundAccount = LookupForPAN(Convert.ToInt64(sessionDTO.CustomerSessionId), mgiContext);

			if (fundAccount != null && !string.IsNullOrWhiteSpace(fundAccount.CardNumber))
			{
				sessionDTO.Customer.Fund.CardNumber = fundAccount.CardNumber;
				sessionDTO.Customer.Fund.AccountNumber = fundAccount.AccountNumber;
				sessionDTO.Customer.Fund.CardBalance = fundAccount.CardBalance;
				sessionDTO.Customer.Fund.IsGPRCard = true;
			}

			sessionDTO.Customer.IsWUGoldCard = GetWUCardAccount(long.Parse(sessionDTO.CustomerSessionId), mgiContext);
			sessionDTO.CardPresent = sessionDTO.CardPresent;

			// AL-591: This is introduced to update IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
			// Developed by: Sunil Shetty || 03/07/2015
			//We are calling it twice one gets called in above line(line no:- 370).so we commented in MGI.Channel.Shared.Server.Impl.InitiateCustomerSession
			getRevisedFeeForParkedTransactions(long.Parse(sessionDTO.CustomerSessionId), context);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<CustomerSessionDTO>(0, sessionDTO, "InitiateCustomerSession", AlloyLayerName.SERVICE,
				ModuleName.Customer, "End InitiateCustomerSession - MGI.Channel.DMS.Server.Impl.CustomerEngine",
				context);
			#endregion

			return sessionDTO;
		}

		private void getRevisedFeeForParkedTransactions(long customerSessionId, MGI.Common.Util.MGIContext mgiContext)
		{
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

		#region DMS Methods

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Get customer profile")]
		public CustomerDTO Lookup(long customerSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			bizCustomer bizCustomer = BizCustomerService.GetCustomer(customerSessionId, alloyId, context);
			CustomerDTO customer = Mapper.Map<bizCustomer, CustomerDTO>(bizCustomer);

			if (bizCustomer.ID != null)
				customer.ID = Mapper.Map<bizIdentification, IdentificationDTO>(bizCustomer.ID);
			if (bizCustomer.Employment != null)
			{
				customer.Employment.Occupation = bizCustomer.Employment.Occupation;
				customer.Employment.Employer = bizCustomer.Employment.Employer;
				customer.Employment.EmployerPhone = bizCustomer.Employment.EmployerPhone;
			}
			//customer.Purse = new PurseDTO { ProcessorAccountId = alloyId.ToString() };

			if (_IsAnonymous(customer.PersonalInformation.FName, customer.PersonalInformation.LName))
			{
				customer.IsAnonymous = true;
				return customer;
			}

			FundsAccountDTO fundAccount = LookupForPAN(Convert.ToInt64(customerSessionId), mgiContext);

			if (fundAccount != null && !string.IsNullOrWhiteSpace(fundAccount.CardNumber))
			{
				customer.Fund.CardNumber = fundAccount.CardNumber;
				customer.Fund.AccountNumber = fundAccount.AccountNumber;
				customer.Fund.CardBalance = fundAccount.CardBalance;
				customer.Fund.IsGPRCard = true;
			}

			customer.IsWUGoldCard = GetWUCardAccount(customerSessionId, mgiContext);

			return customer;
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Customer search")]
		public List<CustomerSearchResultDTO> SearchCustomers(long agentSessionId, CustomerSearchCriteriaDTO searchCriteriaDTO, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			bizCustomerSearchCriteria searchCriteria = Mapper.Map<CustomerSearchCriteriaDTO, bizCustomerSearchCriteria>(searchCriteriaDTO);

			List<bizCustomerSearchResult> results = BizCustomerService.Search(agentSessionId, searchCriteria, context);

			return Mapper.Map<List<bizCustomerSearchResult>, List<CustomerSearchResultDTO>>(results);
		}

		/// <summary>
		/// GetProspect
		/// </summary>
		/// <param name="agentSessionId"></param>
		/// <param name="alloyId"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Get registration data")]
		public ProspectDTO GetProspect(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			SessionContextDTO contextDTO = GetSessionContext(Convert.ToString(agentSessionId));

			bizProspectSessionContext sessionContext = Mapper.Map<SessionContextDTO, bizProspectSessionContext>(contextDTO);

			bizProspect prospect = BizProspectService.GetProspect(agentSessionId, sessionContext, alloyId, context);

			ProspectDTO prospectDTO = Mapper.Map<bizProspect, ProspectDTO>(prospect);

			prospectDTO.ClientProfileStatus = BizCustomerService.GetClientProfileStatus(agentSessionId, alloyId, context);
			prospectDTO.PartnerAccountNumber = alloyId.ToString();
			return prospectDTO;
		}

		/// <summary>
		/// Recording Identification Confirmation
		/// </summary>
		/// <param name="agentId"></param>
		/// <param name="customerSessionId"></param>
		/// <param name="IdentificationStatus"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "ID confirmation")]
		public string RecordIdentificationConfirmation(long customerSessionId, string agentId, bool IdentificationStatus, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return BizProspectService.ConfirmIdentity(customerSessionId, long.Parse(agentId), IdentificationStatus, context);
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Get customer by phone+pin")]
		public CustomerDTO Get(long agentSessionId, string Phone, string PIN, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			bizCustomer customer = BizCustomerService.Get(agentSessionId, Phone, PIN, context);
			return GetCustomerMapper(customer);
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Validate SSN")]
		public bool ValidateSSN(long agentSessionId, string SSN, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			return BizCustomerService.ValidateSSN(agentSessionId, SSN, context);
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Validate Customer RequiredFields")]
		public bool ValidateCustomer(long agentSessionId, ProspectDTO prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			bizCustomer bizCustomer = Mapper.Map<Prospect, bizCustomer>(prospect);

			if (bizCustomer == null)
				return false;

			return BizCustomerService.ValidateCustomer(agentSessionId, bizCustomer, context);

		}

		private Customer GetCustomerMapper(bizCustomer customer)
		{
			CustomerDTO customerDTO = null;
			if (customer != null)
			{
				customerDTO = Mapper.Map<bizCustomer, CustomerDTO>(customer);
				if (customer.ID != null)
					customerDTO.ID = Mapper.Map<bizIdentification, IdentificationDTO>(customer.ID);
				if (customer.Employment != null)
				{
					if (customer.Employment.Employer != null)
						customerDTO.Employment.Employer = customer.Employment.Employer;
					if (customer.Employment.EmployerPhone != null)
						customerDTO.Employment.EmployerPhone = customer.Employment.EmployerPhone;
					if (customer.Employment.Occupation != null)
						customerDTO.Employment.Occupation = customer.Employment.Occupation;
				}
			}
			return customerDTO;
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Get anonymous PAN")]
		public long GetAnonymousUserPAN(long agentSessionId, long channelPartnerId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			return BizCustomerService.GetAnonymousUserPAN(agentSessionId, channelPartnerId, "unregistered", "customer", context);

		}

		/// <summary>
		/// Search Customers by Customer LookUp
		/// </summary>
		/// <param name="sessionId"></param>
		/// <param name="customerLookUpCriteria"></param>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		public List<ProspectDTO> CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			List<bizCustomer> bizCustomers = BizCustomerService.CustomerLookUp(agentSessionId, customerLookUpCriteria, context);
			if (bizCustomers == null)
				return null;
			return Mapper.Map<List<bizCustomer>, List<ProspectDTO>>(bizCustomers);
		}

		[Transaction(ReadOnly = true)]
		public void ValidateCustomerStatus(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizCustomerService.ValidateCustomerStatus(agentSessionId, alloyId, context);

		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.Customer, "Check profile status change permission")]
		public bool CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return BizCustomerService.CanChangeProfileStatus(agentSessionId, userId, profileStatus, context);
		}

		#endregion

		//[Transaction()]
		//public Boolean Activate(string sessionId, long alloyId, Dictionary<string, string> context)
		//{

		//    SessionContextDTO contextDTO = GetSessionContext(sessionId);
		//    bizSessionContext sessionContext = Mapper.Map<SessionContextDTO, bizSessionContext>(contextDTO);

		//    BizCustomerService.Register(sessionContext, alloyId);
		//    ComplianceService.ClearNewCustomer(alloyId, context);

		//    return true;
		//}




	}
}

