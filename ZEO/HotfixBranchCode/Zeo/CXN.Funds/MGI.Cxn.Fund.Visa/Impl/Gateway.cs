using AutoMapper;
using MGI.Cxn.Fund.Visa.Data;
using MGI.Common.DataAccess.Contract;
using MGI.Common.DataProtection.Contract;
using MGI.Common.Util;
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using MGI.Cxn.Fund.Visa.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.Fund.Visa.Impl
{
	public class Gateway : IFundProcessor
	{
		private Credential _credential = null;

		public Gateway()
		{
			#region Mappings

			Mapper.CreateMap<CardAccount, Account>()
				.ForMember(x => x.CardAliasId, s => s.MapFrom(c => c.AccountNumber))
				.ForMember(x => x.Address1, s => s.MapFrom(c => c.MailingAddress1))
				.ForMember(x => x.Address2, s => s.MapFrom(c => c.MailingAddress2))
				.ForMember(x => x.City, s => s.MapFrom(c => c.MailingCity))
				.ForMember(x => x.State, s => s.MapFrom(c => c.MailingState))
				.ForMember(x => x.ZipCode, s => s.MapFrom(c => c.MailingZipCode))
				.ForMember(x => x.Country, s => s.MapFrom(c => c.CountryCode))
				.ForMember(x => x.Email, s => s.MapFrom(c => c.Email))
				.ForMember(x => x.MothersMaidenName, s => s.MapFrom(c => c.MothersMaidenName));

			Mapper.CreateMap<Account, CardAccount>()
				.ForMember(x => x.AccountNumber, s => s.MapFrom(c => c.CardAliasId))
				.ForMember(x => x.MailingAddress1, s => s.MapFrom(c => c.Address1))
				.ForMember(x => x.MailingAddress2, s => s.MapFrom(c => c.Address2))
				.ForMember(x => x.MailingCity, s => s.MapFrom(c => c.City))
				.ForMember(x => x.MailingState, s => s.MapFrom(c => c.State))
				.ForMember(x => x.MailingZipCode, s => s.MapFrom(c => c.ZipCode))
				.ForMember(x => x.CountryCode, s => s.MapFrom(c => c.Country))
				.ForMember(x => x.Email, s => s.MapFrom(c => c.Email))
				.ForMember(x => x.MothersMaidenName, s => s.MapFrom(c => c.MothersMaidenName))
				.AfterMap((s, d) =>
				{
					d.IsCardActive = true;
				});

			Mapper.CreateMap<Transaction, FundTrx>()
				.ForMember(x => x.TransactionAmount, s => s.MapFrom(c => c.Amount))
				.ForMember(x => x.TransactionID, s => s.MapFrom(c => c.ConfirmationId))
				.ForMember(x => x.PreviousCardBalance, s => s.MapFrom(c => c.Balance))
				.ForMember(x => x.Account, s => s.MapFrom(c => c.Account))
				.ForMember(x => x.TransactionType, s => s.MapFrom(c => c.TransactionType.ToString()))
				.ForMember(x => x.PromoCode, s => s.MapFrom(c => c.PromoCode.ToString()))
				.AfterMap((s, d) =>
				{
					d.Account.ExpirationDate = string.Format("{0}/{1}", s.Account.ExpirationMonth, s.Account.ExpirationYear);
				});
			Mapper.CreateMap<Account, Account>();
			Mapper.CreateMap<Data.CardBalance, MGI.Cxn.Fund.Data.CardInfo>();

			#endregion
		}

		#region Dependencies
		public IRepository<Account> AccountRepo { private get; set; }
		public IRepository<Transaction> TransactionRepo { private get; set; }
		public IDataProtectionService DataProtectionService { private get; set; }

		public IIO IO { private get; set; }
		public IRepository<Credential> CredentialRepo { private get; set; }
		public IRepository<CardClass> CardClassRepo { private get; set; }
		public IRepository<VisaFee> VisaFeeRepo { private get; set; }
		public IRepository<VisaShippingFee> VisaShippingFeeRepo { private get; set; }

		public IRepository<ChannelPartnerShippingTypeMapping> ChannelPartnerShippingTypeRepo { private get; set; }
		public IRepository<ChannelPartnerFeeTypeMapping> ChannelPartnerFeeTypeRepo { private get; set; }
		public IRepository<VisaFeeTypes> VisaFeeTypeRepo { private get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }

		#endregion


		public long Register(CardAccount cardAccount, MGIContext mgiContext, out ProcessorResult processorResult)
		{

			ValidateContext(cardAccount, mgiContext.ChannelPartnerId);

			string cardNumber = string.Empty;
			Data.CardInfo cardInformation = new Data.CardInfo();
			string pseudoDDA = string.Empty;

			Account account = Mapper.Map<Account>(cardAccount);
			GetCredential(mgiContext.ChannelPartnerId);
			try
			{
				cardInformation = IO.GetCardInfoByProxyId(account.ProxyId, _credential);

				pseudoDDA = IO.GetPsedoDDAFromAliasId(cardInformation.AliasId, _credential);
			}
			catch (VisaProviderException vpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<CardAccount>(cardAccount, "Register", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, "Failure while retrieving card information", vpe);
			}

			string expiryDate = string.Format("{0}/{1}", cardInformation.ExpirationMonth, cardInformation.ExpirationYear);
			cardAccount.ExpirationDate = cardAccount.ExpirationDate.TrimStart('0');

			if (expiryDate != cardAccount.ExpirationDate)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<CardAccount>(cardAccount, "Register", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", "Expiration Date doesn't match", string.Empty);

				throw new FundException(FundException.INVALID_EXPIRATION_DATE, "Expiration Date doesn't match");
			}

			if (!string.IsNullOrEmpty(account.PseudoDDA))
			{
				if (account.PseudoDDA != pseudoDDA)
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.Error<CardAccount>(cardAccount, "Register", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", "Pseudo DDA is not matching", string.Empty);

					throw new FundException(FundException.PSEUDO_DDA_MISMATCH, "Pseudo DDA is not matching");
				}
			}
			else if (!string.IsNullOrEmpty(account.CardNumber) && account.CardNumber != cardInformation.CardNumber)
			{
				throw new FundException(FundException.PAN_NUMBER_MISMATCH, "Card number is not matching");
			}

			if (cardInformation != null && !string.IsNullOrWhiteSpace(cardInformation.CardNumber))
			{
				if (cardInformation.Status != "1")
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.Error<Data.CardInfo>(cardInformation, "Register", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", "This card has already been issued to another customer", string.Empty);

					throw new FundException(FundException.CARD_ALREADY_ISSUED, "This card has already been issued to another customer");
				}
				cardNumber = EncryptCardNumber(cardInformation.CardNumber);
				var existingCardAccounts = AccountRepo.FilterBy(a => a.CardNumber == cardNumber);

				if (existingCardAccounts != null && existingCardAccounts.Count() > 0)
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.ListError<Account>(existingCardAccounts.ToList(), "Register", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", "Card already registered with another customer", string.Empty);

					throw new FundException(FundException.CARD_ALREADY_REGISTERED, "Card already registered with another customer");
				}
			}


			try
			{
				account.CardAliasId = Convert.ToString(cardInformation.AliasId);
				account.PrimaryCardAliasId = Convert.ToString(cardInformation.AliasId);
				account.ProxyId = cardInformation.ProxyId;
				account.PseudoDDA = pseudoDDA;
				account.CardNumber = EncryptCardNumber(cardInformation.CardNumber);
				account.ExpirationMonth = cardInformation.ExpirationMonth;
				account.ExpirationYear = cardInformation.ExpirationYear;
				account.SubClientNodeId = cardInformation.SubClientNodeId;
				account.ActivatedLocationNodeId = mgiContext.VisaLocationNodeId;

				account.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				account.DTServerCreate = DateTime.Now;

				AccountRepo.AddWithFlush(account);

				processorResult = new ProcessorResult(true);
			}
			catch (VisaProviderException vpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<Account>(account, "Register", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, "Failure registering new card account", vpe);
			}

			return account.Id;
		}

		private void ValidateContext(CardAccount cardAccount, long channelPartnerId)
		{
			if (cardAccount == null)
			{
				throw new ArgumentException("cardAccount parameter can not be null");
			}
			else if (string.IsNullOrWhiteSpace(cardAccount.ProxyId))
			{
				throw new ArgumentException("ProxyId can not be empty");
			}
			else if (string.IsNullOrWhiteSpace(cardAccount.ExpirationDate))
			{
				throw new ArgumentException("Expiration date can not be empty");
			}
			GetCredential(channelPartnerId);
		}

		public long Authenticate(string cardNumber, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			throw new NotImplementedException();
		}

		public MGI.Cxn.Fund.Data.CardInfo GetBalance(long accountId, MGIContext mgiContext, out ProcessorResult processorResult)
		{

			Fund.Data.CardInfo cardInfo = new Fund.Data.CardInfo();
			Account account = AccountRepo.FindBy(a => a.Id == accountId && a.Activated == true);

			if (account != null)
			{
				try
				{
					string aliasId = account.CardAliasId;
					if (!string.IsNullOrWhiteSpace(aliasId))
					{
						long cardAliasId = Convert.ToInt64(aliasId);

						GetCredential(mgiContext.ChannelPartnerId);

						Data.CardBalance cardBalance = IO.GetBalance(cardAliasId, _credential);
						cardInfo = Mapper.Map<Fund.Data.CardInfo>(cardBalance);
						cardInfo.ClosureDate = account.DTAccountClosed;
						if (string.IsNullOrWhiteSpace(account.PrimaryCardAliasId))
						{
							UpdatePrimaryAliasID(account, mgiContext);
						}
						if (!string.IsNullOrWhiteSpace(cardBalance.NewCardNumber) && DecryptCardNumber(account.CardNumber) != cardBalance.NewCardNumber)
						{
							UpdateRegistrationDetails(accountId, cardBalance.NewCardNumber, mgiContext.TimeZone);
						}
						cardInfo.MetaData = new Dictionary<string, object>();
						if (!string.IsNullOrEmpty(account.PrimaryCardAliasId) && !string.IsNullOrEmpty(account.CardAliasId)
																		&& account.PrimaryCardAliasId == account.CardAliasId)
						{
							cardInfo.MetaData.AddOrUpdate("IsPrimaryCardHolder", true);
						}
						else
						{
							cardInfo.MetaData.AddOrUpdate("IsPrimaryCardHolder", false);
						}
					}
				}
				catch (VisaProviderException vpe)
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.Error<Fund.Data.CardInfo>(cardInfo, "GetBalance", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

					throw new FundException(FundException.PROVIDER_ERROR, "Failure while retrieving card balance", vpe);
				}
			}

			processorResult = new ProcessorResult(true);

			return cardInfo;
		}

		public long Load(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			ValidateFundTransaction(accountId, fundRequest, mgiContext);

			string promoCode = string.Empty;

			Transaction transaction = StageTransaction(accountId, TransactionType.Credit, fundRequest.Amount, promoCode, mgiContext);
			processorResult = new ProcessorResult(true);

			return transaction.Id;
		}

		public long Withdraw(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			ValidateFundTransaction(accountId, fundRequest, mgiContext);
			string promoCode = string.Empty;
			Transaction transaction = StageTransaction(accountId, TransactionType.Debit, fundRequest.Amount, promoCode, mgiContext);

			processorResult = new ProcessorResult(true);

			return transaction.Id;
		}

		public void Commit(long transactionId, MGIContext mgiContext, out ProcessorResult processorResult, string cardNumber = "")
		{
			Transaction transaction = GetTransaction(transactionId);

			long cardAliasId = Convert.ToInt64(GetAliasId(transaction.Account));
			double transactionAmount = Convert.ToDouble(transaction.Amount);
			mgiContext.CardClass = GetCardClass(mgiContext);

			if (transaction.TransactionType == TransactionType.Activation)
				transaction.Description = "Account activation";
			else
				transaction.Description = string.Format("Teller {0} at {1}", (transaction.TransactionType == TransactionType.Credit) ? "load"
					: (transaction.TransactionType == TransactionType.Debit) ? "withdraw" : "Order Companion Card", mgiContext.LocationName);

			try
			{
				string confirmationId = string.Empty;

				GetCredential(mgiContext.ChannelPartnerId);
				_credential.VisaLocationNodeId = mgiContext.VisaLocationNodeId;

				if (transaction.TransactionType == TransactionType.AddOnCard)
				{
					CardPurchaseResponse issueCardResponse = new CardPurchaseResponse();

					try
					{
						CardAccount cardAccount = (CardAccount)mgiContext.Context["FundsAccount"];
						Account account = Mapper.Map<Account>(cardAccount);
						account.SubClientNodeId = transaction.Account.SubClientNodeId;

						if (mgiContext.CardExpiryPeriod > 0)
						{
							DateTime today = DateTime.Now;
							if (today.Date.Day > 15)
							{
								mgiContext.CardExpiryPeriod += 1;
							}
						}
						else
						{
							throw new FundException(FundException.PROVIDER_ERROR, "Card expiry period should be more than zero");
						}
						issueCardResponse = IO.CompanianCardOrder(cardAliasId, _credential, account, mgiContext);
						if (issueCardResponse != null && !string.IsNullOrWhiteSpace(issueCardResponse.ConfirmationNumber))
						{
							confirmationId = issueCardResponse.ConfirmationNumber;
						}
					}
					catch (VisaProviderException vpe)
					{
						throw new FundException(FundException.PROVIDER_ERROR, vpe.Message, vpe);
					}
				}
				else if (transaction.TransactionType == TransactionType.Activation)
				{
					CardPurchaseResponse issueCardResponse = ActivateAccount(transaction, transactionAmount, mgiContext.TimeZone, _credential);

					if (issueCardResponse != null && !string.IsNullOrWhiteSpace(issueCardResponse.ConfirmationNumber))
					{
						confirmationId = issueCardResponse.ConfirmationNumber;
					}
				}
				else if (transaction.TransactionType == TransactionType.Credit)
				{
					LoadResponse response = IO.Load(cardAliasId, transactionAmount, _credential);
					confirmationId = Convert.ToString(response.TransactionKey);
				}
				else if (transaction.TransactionType == TransactionType.Debit)  // Debit
				{
					IO.Withdraw(cardAliasId, transactionAmount, _credential);
				}

				transaction.ConfirmationId = confirmationId;
				transaction.DTTransmission = DateTime.Now;
				transaction.Status = TransactionState.Committed;

				processorResult = new ProcessorResult(true);
				processorResult.ConfirmationNumber = transaction.ConfirmationId;
			}
			catch (VisaProviderException vpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<Transaction>(transaction, "Commit", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

				transaction.Status = TransactionState.Failed;
				transaction.ErrorCode = vpe.Code;
				transaction.ErrorMsg = vpe.Message;

				processorResult = new ProcessorResult(FundException.PROVIDER_ERROR, vpe.Message, false, vpe);


			}
			transaction.LocationNodeId = mgiContext.VisaLocationNodeId;

			transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			transaction.DTServerLastModified = DateTime.Now;

			TransactionRepo.UpdateWithFlush(transaction);
		}

		public long Activate(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			ValidateFundTransaction(accountId, fundRequest, mgiContext);
			TransactionType transactionType = (TransactionType)Enum.Parse(typeof(TransactionType), fundRequest.RequestType);

			Transaction transaction = StageTransaction(accountId, transactionType, fundRequest.Amount, fundRequest.PromoCode, mgiContext);
			processorResult = new ProcessorResult(true);

			return transaction.Id;
		}

		public CardAccount Lookup(long accountId)
		{
			Account account = new Account();
			try
			{
				account = GetAccount(accountId);
			}
			catch (VisaProviderException vpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<Account>(account, "Commit", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, vpe.Message, vpe);
			}
			CardAccount cardAccount = Mapper.Map<CardAccount>(account);
			return cardAccount;
		}

		public CardAccount LookupCardAccount(long accountId, bool isCardAccountActivated = false)
		{
			//AL-5906 : Synovus 6.1- Getting date of birth below minimum age for card 1 error after correcting DOB
			//This change is to update tVisa_Account against the card activation.
			//Start
			if (isCardAccountActivated)
			{
				Account account = new Account();
				account = GetExistingAccount(accountId);

				CardAccount cardAccount = Mapper.Map<CardAccount>(account);

				return cardAccount;
			}
			//End
			return Lookup(accountId);
		}

		public long GetPanForCardNumber(string cardNumber, MGIContext mgiContext)
		{
			long accountId = 0L;

			cardNumber = EncryptCardNumber(cardNumber);
			var accounts = AccountRepo.FilterBy(a => a.CardNumber == cardNumber && a.Activated == true);
			if (accounts != null && accounts.Count() > 0)
			{
				var account = accounts.FirstOrDefault();
				if (account != null)
				{
					accountId = account.Id;
				}
			}
			return accountId;
		}

		public long UpdateAmount(long transactionId, FundRequest fundRequest, string timezone)
		{
			Transaction transaction = GetTransaction(transactionId);

			if (fundRequest.RequestType == "Activation")
			{
				transaction.PromoCode = fundRequest.PromoCode;
			}

			transaction.Amount = fundRequest.Amount;
			transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			transaction.DTServerLastModified = DateTime.Now;

			TransactionRepo.UpdateWithFlush(transaction);

			return transaction.Id;
		}

		public FundTrx Get(long transactionId, MGIContext mgiContext)
		{
			Transaction transaction = GetTransaction(transactionId);
			FundTrx fundTrx = Mapper.Map<FundTrx>(transaction);
			string cardAliasId = transaction.Account.CardAliasId;

			if (!string.IsNullOrWhiteSpace(cardAliasId))
			{
				long aliasId = Convert.ToInt64(cardAliasId);

				GetCredential(mgiContext.ChannelPartnerId);
				Data.CardBalance cardBalance = new Data.CardBalance();
				try
				{
					cardBalance = IO.GetBalance(aliasId, _credential);
				}
				catch (VisaProviderException vpe)
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.Error<Transaction>(transaction, "Get", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

					throw new FundException(FundException.PROVIDER_ERROR, "Failure while retrieving balance", vpe);
				}

				fundTrx.CardBalance = Convert.ToDecimal(cardBalance.Balance);
				string cardNumber = string.Empty;
				if (!string.IsNullOrWhiteSpace(transaction.Account.CardNumber))
				{
					cardNumber = transaction.Account.CardNumber;
					fundTrx.Account.CardNumber = cardNumber.Length > 4 ? cardNumber.Substring(cardNumber.Length - 4) : cardNumber;
					fundTrx.Account.FullCardNumber = DecryptCardNumber(transaction.Account.CardNumber);
				}
			}
			return fundTrx;
		}

		public void UpdateRegistrationDetails(long cxnAccountId, string cardNumber, string timezone)
		{
			Account visaAccount = GetAccount(cxnAccountId);

			if (visaAccount != null)
			{
				if (!string.IsNullOrWhiteSpace(cardNumber))
				{
					visaAccount.CardNumber = EncryptCardNumber(cardNumber);
				}
				else if (!string.IsNullOrWhiteSpace(visaAccount.CardNumber))
				{
					visaAccount.CardNumber = EncryptCardNumber(visaAccount.CardNumber);
				}
				visaAccount.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
				visaAccount.DTServerLastModified = DateTime.Now;
				AccountRepo.Merge(visaAccount);
			}
		}

		public void UpdateRegistrationDetails(CardAccount cardAccount, MGIContext mgiContext)
		{
			ValidateContext(cardAccount, mgiContext.ChannelPartnerId);
			Data.CardInfo cardInformation = new Data.CardInfo();
			string pseudoDDA = string.Empty;
			GetCredential(mgiContext.ChannelPartnerId);
			try
			{
				cardInformation = IO.GetCardInfoByProxyId(cardAccount.ProxyId, _credential);
				pseudoDDA = IO.GetPsedoDDAFromAliasId(cardInformation.AliasId, _credential);
			}
			catch (VisaProviderException vpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<CardAccount>(cardAccount, "UpdateRegistrationDetails", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, "Failure while retrieving card information", vpe);
			}

			string expiryDate = string.Format("{0}/{1}", cardInformation.ExpirationMonth, cardInformation.ExpirationYear);
			cardAccount.ExpirationDate = cardAccount.ExpirationDate.TrimStart('0');

			if (expiryDate != cardAccount.ExpirationDate)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<CardAccount>(cardAccount, "UpdateRegistrationDetails", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", "Expiration Date doesn't match", string.Empty);

				throw new FundException(FundException.INVALID_EXPIRATION_DATE, "Expiration Date doesn't match");
			}

			if (!string.IsNullOrEmpty(cardAccount.PseudoDDA))
			{
				if (cardAccount.PseudoDDA != pseudoDDA)
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.Error<CardAccount>(cardAccount, "UpdateRegistrationDetails", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", "Pseudo DDA is not matching", string.Empty);

					throw new FundException(FundException.PSEUDO_DDA_MISMATCH, "Pseudo DDA is not matching");
				}
			}
			else if (!string.IsNullOrEmpty(cardAccount.CardNumber) && cardAccount.CardNumber != cardInformation.CardNumber)
			{
				throw new FundException(FundException.PAN_NUMBER_MISMATCH, "Card number is not matching");
			}

			Account account = new Account();
			try
			{
				account = GetExistingAccount(cardAccount.Id);

				if (account != null)
				{
					account.CardAliasId = Convert.ToString(cardInformation.AliasId);
					account.PrimaryCardAliasId = Convert.ToString(cardInformation.AliasId);
					account.ProxyId = cardInformation.ProxyId;
					account.PseudoDDA = pseudoDDA;
					account.CardNumber = EncryptCardNumber(cardInformation.CardNumber);
					account.ExpirationMonth = cardInformation.ExpirationMonth;
					account.ExpirationYear = cardInformation.ExpirationYear;
					account.SubClientNodeId = cardInformation.SubClientNodeId;
					account.IDCode = cardAccount.IDCode;
					account.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
					account.DTServerLastModified = DateTime.Now;
					account.ActivatedLocationNodeId = mgiContext.VisaLocationNodeId;

					AccountRepo.Merge(account);
				}
			}
			catch (VisaProviderException vpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<Account>(account, "UpdateRegistrationDetails", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, "Failure while updating card account", vpe);
			}
		}

		public void Cancel(long accountId, MGIContext mgiContext)
		{
			Account account = GetExistingAccount(accountId);

			if (account != null)
			{
				account.CardAliasId = null;
				account.PrimaryCardAliasId = null;
				account.CardNumber = null;
				account.ProxyId = null;
				account.PseudoDDA = null;
				account.ExpirationMonth = 0;
				account.ExpirationYear = 0;
				account.SubClientNodeId = 0;

				account.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				account.DTServerLastModified = DateTime.Now;

				AccountRepo.Merge(account);
			}
		}

		public long UpdateAccount(CardAccount cardAccount, MGIContext mgiContext)
		{
			Account account = GetExistingAccount(cardAccount.Id);
			long accountId = 0;

			if (account != null)
			{
				account.FirstName = cardAccount.FirstName;
				account.LastName = cardAccount.LastName;
				account.MiddleName = cardAccount.MiddleName;
				account.Phone = cardAccount.Phone;
				account.SSN = cardAccount.SSN;
				account.State = cardAccount.State;
				account.ZipCode = cardAccount.ZipCode;
				account.Address1 = cardAccount.Address1;
				account.Address2 = cardAccount.Address2;
				account.City = cardAccount.City;
				account.Country = cardAccount.CountryCode;
				account.DateOfBirth = cardAccount.DateOfBirth;
				account.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				account.DTServerLastModified = DateTime.Now;
				account.CardNumber = string.IsNullOrEmpty(cardAccount.CardNumber) ? "" : EncryptCardNumber(cardAccount.CardNumber);
				account.IDCode = cardAccount.IDCode;

				AccountRepo.Merge(account);

				accountId = account.Id;
			}

			return accountId;
		}

		public List<TransactionHistory> GetTransactionHistory(long accountId, TransactionHistoryRequest request, MGIContext mgiContext)
		{
			List<TransactionHistory> transactionHistoryList = new List<TransactionHistory>();

			Account account = GetAccount(accountId);
			if (account != null)
			{
				GetCredential(mgiContext.ChannelPartnerId);
				string aliasId = account.CardAliasId;
				if (request != null && account != null && !string.IsNullOrWhiteSpace(aliasId))
				{
					request.AliasId = Convert.ToInt64(aliasId);
				}

				try
				{
					transactionHistoryList = IO.GetTransactionHistory(request, _credential);
				}
				catch (VisaProviderException vpe)
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.Error<Account>(account, "GetTransactionHistory", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

					throw new FundException(FundException.PROVIDER_ERROR, "Failure retrieving card transaction history", vpe);
				}
			}

			return transactionHistoryList;
		}

		public bool CloseAccount(long accountId, MGIContext mgiContext)
		{
			long aliasId = 0L;
			bool couldCloseAccount = false;

			Account account = GetAccount(accountId);

			if (account != null)
			{
				GetCredential(mgiContext.ChannelPartnerId);
				string cardAliasId = account.CardAliasId;
				if (account != null && !string.IsNullOrWhiteSpace(cardAliasId))
				{
					aliasId = Convert.ToInt64(cardAliasId);
				}

				try
				{
					couldCloseAccount = IO.CloseAccount(aliasId, _credential);
					account.CardNumber = string.IsNullOrWhiteSpace(account.CardNumber) ? "" : EncryptCardNumber(account.CardNumber);
					account.DTAccountClosed = DateTime.Now;
					AccountRepo.Merge(account);
				}
				catch (VisaProviderException vpe)
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.Error<Account>(account, "CloseAccount", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

					throw new FundException(FundException.PROVIDER_ERROR, "Failure while closing card account", vpe);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}

			return couldCloseAccount;
		}

		public bool UpdateCardStatus(long accountId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			long aliasId = GetAccountDetail(accountId, mgiContext);
			bool couldUpdateStatus = false;
			GetCredential(mgiContext.ChannelPartnerId);
			try
			{
				couldUpdateStatus = IO.UpdateCardStatus(aliasId, cardMaintenanceInfo.CardStatus, _credential);
			}
			catch (VisaProviderException vpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<long>(aliasId, "UpdateCardStatus", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, "Failure while updating card status", vpe);
			}

			return couldUpdateStatus;
		}

		private long GetAccountDetail(long accountId, MGIContext mgiContext)
		{
			long aliasId = 0L;

			Account account = GetAccount(accountId);

			GetCredential(mgiContext.ChannelPartnerId);
			string cardAliasId = account.CardAliasId;
			if (account != null && !string.IsNullOrWhiteSpace(cardAliasId))
			{
				aliasId = Convert.ToInt64(cardAliasId);
			}
			return aliasId;
		}

		public bool ReplaceCard(long accountId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			long aliasId = 0L;
			string proxyId = string.Empty;
			bool couldReplaceCard = false;


			Account account = GetAccount(accountId);
			if (account != null)
			{

				GetCredential(mgiContext.ChannelPartnerId);

				cardMaintenanceInfo.CardClass = GetCardClass(mgiContext);
				int cardStatus = Convert.ToInt32(cardMaintenanceInfo.SelectedCardStatus);
				int shippingType = Convert.ToInt32(cardMaintenanceInfo.ShippingType);

				if ((cardStatus == (int)CardStatus.Lost || cardStatus == (int)CardStatus.Stolen) && shippingType == (int)ShippingFeeType.ReplaceInstantIssue)
					cardMaintenanceInfo.ShippingType = Convert.ToString((int)ShippingFeeType.InstantIssueReplaceLostOrStolen);
				VisaShippingFee visashippingfee = GetVisaShippingFee(cardMaintenanceInfo.ShippingType, mgiContext.ChannelPartnerId);
				cardMaintenanceInfo.ShippingFee = visashippingfee.Fee;
				cardMaintenanceInfo.ShippingFeeCode = visashippingfee.FeeCode;

				int visaFeeType = (cardStatus == (int)CardStatus.Lost || cardStatus == (int)CardStatus.Stolen) ? Convert.ToInt32(FundFeeType.CardReplacementFee) : Convert.ToInt32(FundFeeType.MailOrderFee);
				VisaFee visaFee = GetVisaFee(mgiContext.ChannelPartnerId, visaFeeType);
				cardMaintenanceInfo.ReplacementFee = visaFee.Fee;
				cardMaintenanceInfo.ReplacementFeeCode = visaFee.FeeCode;
				cardMaintenanceInfo.StockId = visaFee.StockId;
				string cardAliasId = account.CardAliasId;
				if (account != null && !string.IsNullOrWhiteSpace(cardAliasId))
				{
					aliasId = Convert.ToInt64(cardAliasId);
					proxyId = account.ProxyId;
				}

				Data.CardInfo cardInfo = IO.GetCardInfoByProxyId(proxyId, _credential);

				DateTime today = DateTime.Now;
				//if (today.Day > 15 && cardMaintenanceInfo.CardStatus == "14")
				//{
				//	mgiContext.CardExpiryPeriod += 1;
				//}
				cardMaintenanceInfo.ExpiryYear = today.AddMonths(mgiContext.CardExpiryPeriod).Year;
				cardMaintenanceInfo.ExpiryMonth = today.AddMonths(mgiContext.CardExpiryPeriod).Month;

				try
				{
					couldReplaceCard = IO.ReplaceCard(aliasId, cardMaintenanceInfo, _credential);
					UpdateInstantIssueCard(accountId, cardMaintenanceInfo, mgiContext, couldReplaceCard);
				}
				catch (VisaProviderException vpe)
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.Error<Account>(account, "ReplaceCard", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.Visa.Impl.Gateway", vpe.Message, vpe.StackTrace);

					throw new FundException(FundException.PROVIDER_ERROR, "Failure while replace card request", vpe);
				}
			}

			return couldReplaceCard;
		}

		public List<MGI.Cxn.Fund.Data.ShippingTypes> GetShippingTypes(long channelPartnerId)
		{
			var shippingTypes = ChannelPartnerShippingTypeRepo.FilterBy(c => c.ChannelPartnerId == channelPartnerId && c.ShippingTypes.Active == true)
				.Select(c => new MGI.Cxn.Fund.Data.ShippingTypes { Name = c.ShippingTypes.Name, Code = c.ShippingTypes.Code }).ToList();
			return shippingTypes;
		}

		public long AssociateCard(CardAccount cardAccount, MGIContext mgiContext, bool isNewCard = false)
		{
			GetCredential(mgiContext.ChannelPartnerId);
			MGI.Cxn.Fund.Visa.Data.CardInfo cardInformation = new Data.CardInfo();
			MGI.Cxn.Fund.Visa.Data.CardInfo latestCardInformation = new Data.CardInfo();
			try
			{
				cardInformation = IO.GetCardInfoByCardNumber(cardAccount.CardNumber, _credential);
				latestCardInformation = IO.GetCardHolderInfo(cardInformation.AliasId, _credential);
			}
			catch (VisaProviderException vpe)
			{
				throw new FundException(FundException.PROVIDER_ERROR, vpe);
			}
			string latestCardNumber = (latestCardInformation != null && !string.IsNullOrWhiteSpace(latestCardInformation.CardNumber)) ? latestCardInformation.CardNumber : cardAccount.CardNumber;
			Account account = Mapper.Map<Account>(cardAccount);
			//AL-1955 : As Synovus, Handle VISA cards update for existing customers in Alloy
			// isNewcard is used for enrolling new Visa DPS Card for exsisting Card Customer, if its not used then it will use associate card functionality
			if (isNewCard)
			{
				return UpdateCardExsistingAccount(cardInformation.AliasId, cardInformation.SSN, cardInformation.LastName, latestCardNumber, mgiContext);
			}
			if (!string.IsNullOrWhiteSpace(cardInformation.SSN) && !string.IsNullOrWhiteSpace(cardAccount.SSN)
						&& !string.IsNullOrWhiteSpace(cardAccount.LastName) && !string.IsNullOrWhiteSpace(cardInformation.LastName))
			{
				if (((cardInformation.SSN.Substring(cardInformation.SSN.Length - 4, 4) == cardAccount.SSN.Substring(cardAccount.SSN.Length - 4, 4)))
										&& (string.Compare(cardInformation.LastName, cardAccount.LastName, true) == 0))
				{
					if (account.Id != 0)
						account = AccountRepo.FindBy(x => x.Id == account.Id);

					account.CardAliasId = Convert.ToString(cardInformation.AliasId);
					account.ProxyId = cardInformation.ProxyId;
					account.PseudoDDA = cardInformation.PsedoDDA;
					account.CardNumber = EncryptCardNumber(latestCardNumber);
					account.ExpirationMonth = cardInformation.ExpirationMonth;
					account.ExpirationYear = cardInformation.ExpirationYear;
					account.SubClientNodeId = cardInformation.SubClientNodeId;
					account.Activated = true;
					if (account.Id != 0)
					{
						account.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
						account.DTServerLastModified = DateTime.Now;
						AccountRepo.Merge(account);
					}
					else
					{
						account.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
						account.DTServerCreate = DateTime.Now;
						AccountRepo.AddWithFlush(account);
					}
				}
				else
				{
					throw new FundException(FundException.CARD_ASSOCIATION_ERROR, "The SSN/ITIN and/or name on the card does not match that on the account. Please verify card belongs to customer");
				}
			}
			else
			{
				throw new FundException(FundException.INVALID_CUSTOMER_DETAILS, "SS# Mismatch. Please verify SS#");

			}
			return account.Id;
		}

		#region Private Methods

		private Account GetAccount(long accountId)
		{
			Account account = AccountRepo.FindBy(a => a.Id == accountId && a.Activated == true);
			Account newAccount = null;

			if (account != null)
			{
				newAccount = Mapper.Map<Account>(account);

				if (!string.IsNullOrWhiteSpace(account.CardNumber))
				{
					newAccount.CardNumber = DecryptCardNumber(newAccount.CardNumber);
				}

			}

			return newAccount;
		}

		private Account GetExistingAccount(long accountId)
		{
			Account account = AccountRepo.FindBy(a => a.Id == accountId);
			Account existingAccount = null;

			if (account != null)
			{
				existingAccount = Mapper.Map<Account>(account);

				if (!string.IsNullOrWhiteSpace(account.CardNumber))
				{
					existingAccount.CardNumber = DecryptCardNumber(existingAccount.CardNumber);
				}
			}

			return existingAccount;
		}

		private Transaction StageTransaction(long accountId, TransactionType transactionType, decimal amount, string PromoCode, MGIContext mgiContext)
		{
			long channelPartnerId = 0L;
			Transaction transaction = null;
			Account account = GetExistingAccount(accountId);
			ProcessorResult processorResult = null;
			Fund.Data.CardInfo balance = GetBalance(account.Id, mgiContext, out processorResult);

			account.CardNumber = DataProtectionService.Encrypt(account.CardNumber, 0);

			// stage transaction
			transaction = new Transaction
			{
				Account = account,
				Amount = amount,
				TransactionType = transactionType,
				Status = TransactionState.Staged,
				Description = string.Empty,
				Balance = balance.Balance,
				ChannelPartnerID = channelPartnerId,

				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
				DTServerCreate = DateTime.Now,
				PromoCode = PromoCode,
				LocationNodeId = mgiContext.VisaLocationNodeId
			};

			TransactionRepo.AddWithFlush(transaction);
			return transaction;
		}

		private static void ValidateFundTransaction(long accountId, FundRequest fundRequest, MGIContext mgiContext)
		{
			if (accountId <= 0)
			{
				throw new ArgumentException("Invalid accountId");
			}

			if (fundRequest == null)
			{
				throw new ArgumentException("fundRequest parameter can not be null");
			}
		}

		private Transaction GetTransaction(long transactionId)
		{
			Transaction transaction = TransactionRepo.FindBy(x => x.Id == transactionId);

			if (transaction == null)
				throw new FundException(FundException.TRANSACTION_NOT_FOUND, string.Format("Could not find transaction Id {0}", transactionId));

			return transaction;
		}

		private CardPurchaseResponse ActivateAccount(Transaction transaction, double initialLoadAmount, string timezone, Credential credential)
		{
			Account account = transaction.Account;
			account.ActivatedLocationNodeId = credential.VisaLocationNodeId;

			Data.CardInfo cardInformation = new Data.CardInfo()
			{
				//This line is specific to Synovus Visa, we need to send LocationNodeId to Visa. 
				//For TCF it's always -1. Hence the changes
				SubClientNodeId = credential.VisaLocationNodeId == -1 ? account.SubClientNodeId : credential.VisaLocationNodeId,
				ExpirationMonth = account.ExpirationMonth,
				ExpirationYear = account.ExpirationYear,
				ProxyId = account.ProxyId,
				PromotionCode = transaction.PromoCode

			};
			CardPurchaseResponse activateResponse = new CardPurchaseResponse();

			try
			{
				activateResponse = IO.IssueCard(account, initialLoadAmount, cardInformation, credential);
			}
			catch (VisaProviderException vpe)
			{
				throw new FundException(FundException.PROVIDER_ERROR, vpe.Message, vpe);
			}

			account.Activated = true;
			account.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			account.DTServerLastModified = DateTime.Now;

			AccountRepo.UpdateWithFlush(account);

			return activateResponse;
		}

		private bool IsFirstLoadWithActivation(long accountId)
		{
			List<Transaction> accountTransactions = TransactionRepo.FilterBy(t => t.Account.Id == accountId).ToList();

			// check for activation record AND no other loads
			return accountTransactions.Where(t => t.TransactionType == TransactionType.Activation).Count() > 0 &&
					accountTransactions.Where(t => t.TransactionType == TransactionType.Credit && t.Status == TransactionState.Committed).Count() == 0;
		}

		private string DecryptCardNumber(string encryptedCardNumber)
		{
			return DataProtectionService.Decrypt(encryptedCardNumber, 0);
		}

		private string EncryptCardNumber(string cardNumber)
		{
			return DataProtectionService.Encrypt(cardNumber, 0);
		}

		private void GetCredential(long channelPartnerId)
		{
			/*AL-4638
			Since _credential is global variable, It will only re-initialize when the app domain recycles so it 
			contains previous channelPartner visa_credential details and Its not re-intializing per request, 
			so I have changed the below condition to get channel partner associated Prepaid Card Credentials.  */
			_credential = CredentialRepo.FindBy(c => c.ChannelPartnerId == channelPartnerId);
		}

		private int GetCardClass(MGIContext mgiContext)
		{

			string locationStateCode = mgiContext.LocationStateCode;
			int cardClass = 1;

			if (!string.IsNullOrWhiteSpace(locationStateCode))
			{
				CardClass visaCardClass = CardClassRepo.FindBy(c => c.StateCode == locationStateCode && c.ChannelPartnerId == mgiContext.ChannelPartnerId);
				if (visaCardClass != null)
				{
					cardClass = visaCardClass.Class;
				}
			}

			return cardClass;
		}

		public double GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			double shippingFee = 0;

			//TODO : AL-1641: we are avoiding fee for instant issue, this will be made accessible to instant issue fee once its made dynamic by adding to shippingfee table according AL-1639
			if (!string.IsNullOrWhiteSpace(cardMaintenanceInfo.ShippingType))
			{
				string shippingType = cardMaintenanceInfo.ShippingType;
				var visaShippingFee = GetVisaShippingFee(shippingType, mgiContext.ChannelPartnerId);
				shippingFee = visaShippingFee.Fee;
				return shippingFee;
			}

			return shippingFee;
		}

		private VisaShippingFee GetVisaShippingFee(string shippingType, long channelPartnerId)
		{
			VisaShippingFee visaShippingFee = new VisaShippingFee();

			if (!string.IsNullOrWhiteSpace(shippingType))
			{
				visaShippingFee = VisaShippingFeeRepo.FindBy(s => s.ChannelPartnerShippingTypeMapping.ChannelPartnerId == channelPartnerId && s.ChannelPartnerShippingTypeMapping.ShippingTypes.Code == shippingType);
			}
			return visaShippingFee;
		}

		public double GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			int feeType = 0;
			int cardStatus = Convert.ToInt32(cardMaintenanceInfo.CardStatus);
			if (cardStatus == (int)CardStatus.Lost || cardStatus == (int)CardStatus.Stolen)
				feeType = (int)FundFeeType.CardReplacementFee;
			else if (cardStatus == (int)CardStatus.ReplaceCard)
				feeType = (int)FundFeeType.MailOrderFee;
			VisaFee visaFee = GetVisaFee(mgiContext.ChannelPartnerId, feeType);

			double fee = 0.0;
			if (visaFee != null)
			{
				fee = visaFee.Fee;
			}
			return fee;
		}

		private VisaFee GetVisaFee(long channelPartnerId, int visaFeeType)
		{
			VisaFee visaFee = new VisaFee();

			visaFee = VisaFeeRepo.FilterBy(s => s.ChannelPartnerFeeTypeMapping.VisaFeeTypes.Id == visaFeeType && s.ChannelPartnerFeeTypeMapping.ChannelPartnerID == channelPartnerId).FirstOrDefault();
			return visaFee;
		}

		private void UpdateInstantIssueCard(long accountId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext, bool couldReplaceCard)
		{
			if (couldReplaceCard)
			{
				UpdateRegistrationDetails(accountId, cardMaintenanceInfo.CardNumber, mgiContext.TimeZone);
			}
		}

		//AL-1955:As Synovus, Handle VISA cards update for existing customers in Alloy
		private long UpdateCardExsistingAccount(long aliasId, string SSN, string lastname, string cardNumber, MGIContext mgiContext)
		{
			//Searches the Alias ID in the tVisa_Account
			Account account = GetAccountByAlias(aliasId);

			if (account != null)
			{
				account.CardNumber = string.IsNullOrWhiteSpace(cardNumber) ? "" : EncryptCardNumber(cardNumber);
				account.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				account.DTServerLastModified = DateTime.Now;
				AccountRepo.UpdateWithFlush(account);
				return account.Id;
			}
			else
				return 0;
		}

		//AL-1955:As Synovus, Handle VISA cards update for existing customers in Alloy
		private Account GetAccountByAlias(long aliasId)
		{
			Account account = AccountRepo.FindBy(a => a.CardAliasId == aliasId.ToString() && a.Activated == true);
			return account;
		}

		//AL-1955:As Synovus, Handle VISA cards update for existing customers in Alloy
		private Account GetAccountBySSN(string SSN, string lastname)
		{
			Account account = new Account();
			if (!string.IsNullOrWhiteSpace(SSN) && SSN.Length > 4)
			{
				return AccountRepo.FindBy(a => a.SSN.EndsWith(SSN.Substring(SSN.Count() - 4, 4)) && a.LastName == lastname && a.Activated == true);
			}
			return null;
		}

		private void UpdatePrimaryAliasID(Account account, MGIContext mgiContext)
		{
			GetCredential(mgiContext.ChannelPartnerId);
			Data.CardInfo cardInformation = IO.GetCardHolderInfo(Convert.ToInt64(account.CardAliasId), _credential);
			if (cardInformation.PrimaryAliasId > 0)
			{
				account.PrimaryCardAliasId = Convert.ToString(cardInformation.PrimaryAliasId);
			}
			else if (cardInformation.PrimaryAliasId == 0)
			{
				account.PrimaryCardAliasId = account.CardAliasId;
			}
			account.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			account.DTServerLastModified = DateTime.Now;
			AccountRepo.UpdateWithFlush(account);
		}

		private string GetAliasId(Account account)
		{
			string aliasId = string.Empty;
			if (!string.IsNullOrEmpty(account.PrimaryCardAliasId))
				aliasId = account.PrimaryCardAliasId;
			else
				aliasId = account.CardAliasId;

			return aliasId;
		}
		#endregion
	}
}

