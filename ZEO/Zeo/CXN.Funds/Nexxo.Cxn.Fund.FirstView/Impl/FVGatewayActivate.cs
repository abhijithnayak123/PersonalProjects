using MGI.Common.DataProtection.Contract;
using MGI.Common.Util;
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.FirstView.Contract;
using MGI.Cxn.Fund.FirstView.Data;
using System;
using System.Collections.Generic;
using TransactionMapping = MGI.Cxn.Fund.FirstView.Data.FirstViewTransaction;

namespace MGI.Cxn.Fund.FirstView.Impl
{
	public class FVGatewayActivate : IFundProcessor, IFirstViewProcessor
	{
		public FVGatewayActivate()
		{
			AutoMapper.Mapper.CreateMap<FirstViewCard, CardAccount>();
			AutoMapper.Mapper.CreateMap<CardAccount, FirstViewCard>();
			AutoMapper.Mapper.CreateMap<TransactionMapping, FundTrx>();
		}

		#region Public Properties

		public FundsProcessorImpl FundProcessorDAL { get; set; }
		public IDataProtectionService DataProtection { private get; set; }

		#endregion

		#region Public Methods
		public long Authenticate(string cardNumber, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			string timezone = mgiContext.TimeZone;

			if (string.IsNullOrEmpty(timezone))
				throw new Exception("Time zone not provided in the context");

			long cxnAccountId = 0L;

			CardResponse response = GetCardInfo(cardNumber, mgiContext, out processorResult, true);

			HandleProcessorResult(response, processorResult, ErrorCode.Could_Not_Authenticate_Card, () =>
				cxnAccountId = FundProcessorDAL.Get(response.BSAccountNumber.Trim()).Id);

			FirstViewCard fvcard = FundProcessorDAL.Get(response.BSAccountNumber);

			UpdateReplacedCardNumber(fvcard, response.CARD_NUMBER, timezone);

			return cxnAccountId;
		}

		public Fund.Data.CardInfo GetBalance(long accountId, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

			if (string.IsNullOrEmpty(timezone))
				throw new Exception("Time zone not provided in the context");

			Fund.Data.CardInfo cardBalance = new CardInfo();
			CheckContext(mgiContext);

			int errorCode = 0;
			string errorMessage = string.Empty;
			FirstViewIO fvIO = null;

			try
			{
				errorCode = FundException.FIRSTVIEW_CREDENTIALS_NOTFOUND;
				errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", mgiContext.ChannelPartnerId);

				var credential = FundProcessorDAL.GetCredentials(mgiContext.ChannelPartnerId);

				if (credential == null)
					throw new FundException(errorCode, errorMessage);

				// get the CXN account information.
				errorCode = FundException.ACCOUNT_NOT_FOUND;
				errorMessage = string.Format("Error while retrieving account for Id: {0}", accountId.ToString());
				FirstViewCard fvCard = FundProcessorDAL.Get(accountId);
				errorCode = FundException.CARD_NOT_ACTIVATED;
				errorMessage = string.Format("Card or account is not activated for Id: {0}", accountId.ToString());

				if (fvCard.IsActive == false)
					throw new FundException(errorCode, errorMessage);

				//The only way to get the balance from first view without card number
				// is by posting a clearing load transaction with 0
				//Refer clearing API document to see the details
				var request = new TransactionRequest()
				{
					DbbServiceName = "svcClearing",
					ServiceUrl = credential.ServiceUrl,
					Application = credential.Application,
					dbbSystemExtLogin = credential.SystemExtLogin,
					Password = credential.Password,
					User = credential.User,

					deClrSvcCustNbr = Convert.ToInt64(DecryptAccountNumber(fvCard.BSAccountNumber)),
					deClrSvcTranType = TransactionType.Load,
					deClrSvcTransactionAmount = 0.00m,
					//deClrSvcCardAcceptorTerminalID = mgiContext.LocationRowGuid
				};

				errorCode = FundException.RETREIVE_CARD_BALANCE_ERROR;
				errorMessage = "Error while retrieving card balance from FirstView for account";

				fvIO = new FirstViewIO();
				TransactionResponse response = fvIO.Clearing(request);
				//AddMapping(request, response, GetProcessorId(context), fvCard);

				processorResult = new ProcessorResult(errorCode, response.PostingNote, response.PostingFlag == "1", TransactionType.Load.ToString(), response.TRANSACTION_ID);
				if (response.CurrentBalance != null && response.CurrentBalance.Trim().Length != 0)
					cardBalance.Balance = Convert.ToDecimal(response.CurrentBalance);
				else
					throw new FundException(FundException.PROVIDER_ERROR, new FirstViewProviderException(Convert.ToString(FundProcessorDAL.GetExceptionMinorCode(response)), response.PostingNote));

				if (response.PrimaryAccountNumber != null)
					UpdateReplacedCardNumber(fvCard, response.PrimaryAccountNumber, timezone);//Need to pass timezone
			}
			catch (Exception ex)
			{
				throw new FundException(errorCode, errorMessage, ex);
			}

			return cardBalance;
		}

		public CardAccount Lookup(long accountId)
		{
			try
			{
				FirstViewCard fvCard = FundProcessorDAL.Get(accountId);
				if (fvCard.IsActive == false)
					return null;
				//throw new FundException(FundException.CARD_NOT_ACTIVATED,"Card or account is not activated");
				return AutoMapper.Mapper.Map<FirstViewCard, CardAccount>(fvCard);
			}
			catch (Exception ex)
			{
				throw new FundException(FundException.ACCOUNT_NOT_FOUND, ex.Message, ex);
			}
		}

		public CardAccount LookupCardAccount(long accountId, bool isCardAccountActivated = false)
		{
			return Lookup(accountId);
		}

		public virtual long Register(CardAccount cardAccount, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			return Update(cardAccount, mgiContext, out processorResult);
		}

		public void Commit(long transactionId, MGIContext mgiContext, out ProcessorResult processorResult, string cardNumber = "")
		{

			string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

			if (string.IsNullOrEmpty(timezone))
				throw new Exception("Time zone not provided in the context");

			int errorCode = 0;
			string errorMessage = string.Empty;
			FirstViewIO fvIO = null;

			errorCode = FundException.FIRSTVIEW_CREDENTIALS_NOTFOUND;
			errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", mgiContext.ChannelPartnerId);

			var credential = FundProcessorDAL.GetCredentials(mgiContext.ChannelPartnerId);

			if (credential == null)
				throw new FundException(errorCode, errorMessage);

			// get the CXN stage information.
			errorCode = FundException.ACCOUNT_NOT_FOUND;
			errorMessage = string.Format("Error while retreiving transaction for Id:{0}", transactionId);

			FirstViewTransaction transaction = FundProcessorDAL.GetTransactionMapping(transactionId);

			if (transaction == null)
				throw new FundException(FundException.TRANSACTION_NOT_FOUND, string.Format("No transaction found for id:{0}", transactionId));

			var request = new TransactionRequest()
			{
				DbbServiceName = "svcClearing",
				ServiceUrl = credential.ServiceUrl,
				Application = credential.Application,
				dbbSystemExtLogin = credential.SystemExtLogin,
				Password = credential.Password,
				User = credential.User,

				deClrSvcCustNbr = Convert.ToInt64(DecryptAccountNumber(transaction.Account.BSAccountNumber)),
				deClrSvcTranType = transaction.TransactionType,
				deClrSvcTransactionAmount = transaction.TransactionAmount,
				//deClrSvcCardAcceptorTerminalID = GetLocationId(mgiContext)
			};

			//if (transaction.TransactionType == MGI.Cxn.Fund.FirstView.Data.TransactionType.Load)
			//    errorCode = FundException.POST_DEBIT_TRANSACTION_ERROR;
			//else if (transaction.TransactionType == MGI.Cxn.Fund.FirstView.Data.TransactionType.Unload)
			//    errorCode = FundException.POST_CREDIT_TRANSACTION_ERROR;
			//else if (transaction.TransactionType == MGI.Cxn.Fund.FirstView.Data.TransactionType.None)
			//    errorCode = FundException.CARD_ACTIVATION_FAILED;

			errorCode = FundException.PROVIDER_ERROR;

			if (transaction.TransactionType != TransactionType.None)
			{
				CardInfo balance = GetBalance(transaction.Account.Id, mgiContext, out processorResult);
				decimal prevCardBalance = balance.Balance;

				transaction.PreviousCardBalance = prevCardBalance;

				FundProcessorDAL.UpdateTransaction(transaction);
			}

			if (transaction.TransactionType != MGI.Cxn.Fund.FirstView.Data.TransactionType.None)
			{
				fvIO = new FirstViewIO();
				TransactionResponse response = fvIO.Clearing(request);
				if (response.PostingFlag != "1")
					throw new FundException(errorCode, new FirstViewProviderException(Convert.ToString(FundProcessorDAL.GetExceptionMinorCode(response)), response.PostingNote));
				AddMapping(request, response, Convert.ToInt16(mgiContext.ProcessorId), transaction.Account, transaction.PreviousCardBalance, mgiContext, timezone);
				processorResult = new ProcessorResult(errorCode, response.PostingNote, response.PostingFlag == "1", (string)transaction.TransactionType, response.TRANSACTION_ID);
			}
			else
			{
				errorMessage = string.Format("Error while activating card:{0}", transaction.Account.CardNumber);
				this.UpdateCommit(transaction.Account.Id, mgiContext, out processorResult, cardNumber);
				processorResult = new ProcessorResult(0, true);
			}
		}

		public long Withdraw(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			return Validate(accountId, fundRequest, RequestType.Debit, mgiContext, out processorResult);
		}

		public long Load(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			return Validate(accountId, fundRequest, RequestType.Credit, mgiContext, out processorResult);
		}

		public long Activate(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			return Validate(accountId, fundRequest, RequestType.None, mgiContext, out processorResult);
		}

		public TransactionMapping GetTransactionMapping(long transactionId)
		{
			return FundProcessorDAL.GetTransactionMapping(transactionId);
		}

		public FundTrx Get(long transactionId, MGIContext mgiContext)
		{
			return AutoMapper.Mapper.Map<TransactionMapping, FundTrx>(GetTransactionMapping(transactionId));
		}

		public long GetPanForCardNumber(string cardNumber, MGIContext mgiContext)
		{
			ProcessorResult processorResult = new ProcessorResult();
			long cxnAccountId = Authenticate(cardNumber, mgiContext, out processorResult);

			return cxnAccountId;
		}

		public long UpdateAmount(long cxnFundTrxId, FundRequest fundRequest, string timezone)
		{
			FirstViewTransaction fundTrx = FundProcessorDAL.GetTransactionMapping(cxnFundTrxId);
			fundTrx.TransactionAmount = fundRequest.Amount;
			//Changes for timestamp
			fundTrx.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			fundTrx.DTServerLastModified = DateTime.Now;
			FundProcessorDAL.UpdateTransaction(fundTrx);
			return fundTrx.Id;
		}

		public void UpdateRegistrationDetails(long cxnAccountId, string cardNumber, string timezone)//Need to pass timezone
		{
			FirstViewCard fvCard = FundProcessorDAL.Get(cxnAccountId);
			UpdateReplacedCardNumber(fvCard, cardNumber, timezone);
		}
		#endregion

		#region Private Methods

		//private int GetProcessorId(MGIContext context)
		//{
		//	return (int)context["ProcessorID"];
		//}

		//private string GetLocationId(MGIContext context)
		//{
		//	return (string)context["LocationId"];
		//}

		//private long GetChannelPartnerId(MGIContext context)
		//{
		//	return (long)context["ChannelPartnerId"];
		//}


		protected void CheckContext(MGIContext context)
		{
			if (context == null)
			{
				throw new FundException(FundException.CONTEXT_NOT_FOUND, "Context object can not be empty");
			}
		}

		private CardResponse GetCardInfo(long accountId, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			FirstViewCard fundAccount = FundProcessorDAL.Get(accountId);
			string accountIdentifier = DecryptAccountNumber(fundAccount.BSAccountNumber);

			return GetCardInfo(accountIdentifier, mgiContext, out processorResult, false);
		}

		private void UpdateReplacedCardNumber(FirstViewCard fvCard, string cardNumber, string timezone)
		{
			if (!cardNumber.EndsWith(fvCard.CardNumber))
			{
				fvCard.CardNumber = cardNumber.Substring(cardNumber.Length - 4);
				//Changes for timestamp
				fvCard.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
				fvCard.DTServerLastModified = DateTime.Now;
				FundProcessorDAL.UpdateCard(fvCard);
			}
		}

		protected CardResponse GetCardInfo(string accountIdentifier, MGIContext mgiContext, out ProcessorResult processorResult, bool IsRegistration)
		{
			CardResponse response = null;
			processorResult = null;
			FirstViewIO fvIO = null;

			int errorCode = 0;
			string errorMessage = string.Empty;

			try
			{
				errorCode = FundException.FIRSTVIEW_CREDENTIALS_NOTFOUND;
				errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", mgiContext.ChannelPartnerId);
				var credential = FundProcessorDAL.GetCredentials(mgiContext.ChannelPartnerId);

				if (credential != null)
				{
					var cardRequest = new CardRequest()
					{
						DbbServiceName = "svcValidateCard",
						ServiceUrl = credential.ServiceUrl,
						Application = credential.Application,
						dbbSystemExtLogin = credential.SystemExtLogin,
						Password = credential.Password,
						User = credential.User,
					};

					if (IsRegistration)
						cardRequest.deTCIVRPrimaryAccountNumber = Convert.ToInt64(accountIdentifier);
					else
						cardRequest.deBSAccountNumber = accountIdentifier;

					fvIO = new FirstViewIO();
					errorCode = FundException.PROVIDER_ERROR;
					errorMessage = string.Format("Card activation failed for card:{0}", accountIdentifier);
					response = fvIO.ValidateCard(cardRequest);
					if (response.ERR_NUMBER != "0")
						throw new FundException(errorCode, new FirstViewProviderException(Convert.ToString(FundProcessorDAL.GetExceptionMinorCode(response)), response.ERRMSG));
				}
				else
					throw new FundException(errorCode, errorMessage);

				processorResult = new ProcessorResult(errorCode, errorMessage, response.ERR_NUMBER == "0", "", "");
			}
			catch (Exception ex)
			{
				throw new FundException(errorCode, errorMessage, ex);
			}
			return response;
		}

		private FirstViewCard BuildGPRCard(AccountRequest request, AccountResponse response, bool IsActive, string timezone)
		{
			return new FirstViewCard()
			{
				rowguid = Guid.NewGuid(),
				CardNumber = request.deCIAPrimaryAccountNumber.ToString().Substring(request.deCIAPrimaryAccountNumber.ToString().Length - 4),
				AccountNumber = response.ACCOUNTNUMBER,
				BSAccountNumber = response.ACCOUNTNUMBER,
				NameAsOnCard = "",
				FirstName = request.deCIAFirstName,
				MiddleName = request.deCIAMiddleName,
				LastName = request.deCIALastName,
				DateOfBirth = FormatFVDateToDateTime(request.deCIADateOfBirth),
				SSN = string.IsNullOrWhiteSpace(request.deCIASSNNumber) ? null : (long?)long.Parse(request.deCIASSNNumber),
				GovernmentId = request.deCIAGovernmentID,
				IDNumber = request.deCIAIDNumber,
				GovtIdExpirationDate = FormatFVDateToDateTime(request.deCIAIDExpirationDate),
				GovtIDIssueCountry = request.deCIAIDIssueCountry,
				GovtIDIssueDate = FormatFVDateToDateTime(request.deCIAIDIssueDate),
				GovtIDIssueState = request.deCIAIDIssueState,
				AddressLine1 = request.deCIAAddressLine1,
				AddressLine2 = request.deCIAAddressLine2,
				City = request.deCIACity,
				State = request.deCIAState,
				PostalCode = request.deCIAPostalCode,
				HomePhoneNumber = request.deCIAHomePhoneNumber,
				ShippingContactName = request.deCIAShipContactName,
				ShippingAddressLine1 = request.deCIAShipAddress1,
				ShippingAddressLine2 = request.deCIAShipAddress2,
				ShippingCity = request.deCIAShipToCity,
				ShippingState = request.deCIAShipToState,
				ShippingZipCode = request.deCIAShipToZipCode,
				IsActive = IsActive,
				//Changes for timestamp
				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
				DTServerCreate = DateTime.Now
			};
		}

		private DateTime? FormatFVDateToDateTime(string date)
		{
			if (string.IsNullOrWhiteSpace(date))
				return null;
			return DateTime.ParseExact(date, "MMddyyyy", System.Globalization.CultureInfo.InvariantCulture);
		}

		private void AddMapping(CardRequest request, CardResponse response, int processorId, string timezone)
		{
			FirstViewTransaction mapping = new TransactionMapping()
			{
				ProcessorId = processorId,
				PrimaryAccountNumber = request.deTCIVRPrimaryAccountNumber,
				CreditPlanMaster = int.MinValue,
				AccountNumber = response.ACCOUNT_NO,
				CardBalance = Convert.ToDecimal(response.CURRENT_CARD_BALANCE),
				ErrorCode = response.ERR_NUMBER,
				ErrorMsg = response.ERRMSG,
				//Changes for timestamp
				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
				DTServerCreate = DateTime.Now,
				CardStatus = response.CARD_STATUS,
				ActivationRequired = response.ACTIVATION_REQUIRED
			};

			FundProcessorDAL.Create(mapping);
		}

		private void AddMapping(AccountRequest request, AccountResponse response, int processorId, string timezone)
		{
			FirstViewTransaction mapping = new TransactionMapping()
			{
				ProcessorId = processorId,
				PrimaryAccountNumber = request.deCIAPrimaryAccountNumber,
				AccountNumber = response.ACCOUNTNUMBER,
				ErrorCode = response.ResErrorCode,
				ErrorMsg = response.ResErrorMsg,
				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
				DTServerCreate = DateTime.Now //Added for Timestamp
			};

			FundProcessorDAL.Create(mapping);
		}
		//Added context for getting channelpartnerPK
		private long AddMapping(TransactionRequest request, TransactionResponse response, int processorId, FirstViewCard firstViewAccount, MGIContext mgiContext, string timezone)
		{
			FirstViewTransaction tranMap = new TransactionMapping()
			{
				Account = firstViewAccount,
				ProcessorId = processorId,
				PrimaryAccountNumber = Convert.ToInt64(request.deClrSvcPrimaryAccountNumber),
				CardAcceptorBusinessCode = request.deClrSvcCardAcceptorBusinessCode,
				CardAcceptorIdCode = request.deClrSvcCardAcceptorIdCode,
				CardAcceptorTerminalID = request.deClrSvcCardAcceptorTerminalID,
				CreditPlanMaster = request.deClrSvcCreditPlanMaster,
				DTLocalTransaction = request.deClrSvcDateTimeLocalTransaction,
				DTTransmission = request.deClrSvcTransmissionDateTime,
				TransactionAmount = request.deClrSvcTransactionAmount,
				TransactionCurrencyCode = request.deClrSvcTransactionCurrencyCode,
				TransactionType = request.deClrSvcTranType,
				TransactionDescription = request.deClrSvcTransactionDescription,
				TransactionID = response.TRANSACTION_ID,
				CardBalance = Convert.ToDecimal(response.CurrentBalance),
				ErrorCode = response.PostingFlag,
				ErrorMsg = response.PostingNote,
				//timestamp changes
				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
				DTServerCreate = DateTime.Now,
				ChannelPartnerID = mgiContext.ChannelPartnerId
			};

			return FundProcessorDAL.Create(tranMap);
		}
		//Added context for getting channelpartnerPK
		private long AddMapping(TransactionRequest request, TransactionResponse response, int processorId, FirstViewCard firstViewAccount, decimal? previousCardBalance, MGIContext mgiContext, string timezone)
		{
			FirstViewTransaction tranMap = new TransactionMapping()
			{
				Account = firstViewAccount,
				ProcessorId = processorId,
				PrimaryAccountNumber = Convert.ToInt64(request.deClrSvcPrimaryAccountNumber),
				CardAcceptorBusinessCode = request.deClrSvcCardAcceptorBusinessCode,
				CardAcceptorIdCode = request.deClrSvcCardAcceptorIdCode,
				CardAcceptorTerminalID = request.deClrSvcCardAcceptorTerminalID,
				CreditPlanMaster = request.deClrSvcCreditPlanMaster,
				DTLocalTransaction = request.deClrSvcDateTimeLocalTransaction,
				DTTransmission = request.deClrSvcTransmissionDateTime,
				TransactionAmount = request.deClrSvcTransactionAmount,
				TransactionCurrencyCode = request.deClrSvcTransactionCurrencyCode,
				TransactionType = request.deClrSvcTranType,
				TransactionDescription = request.deClrSvcTransactionDescription,
				TransactionID = response.TRANSACTION_ID,
				CardBalance = Convert.ToDecimal(response.CurrentBalance),
				PreviousCardBalance = previousCardBalance,
				ErrorCode = response.PostingFlag,
				ErrorMsg = response.PostingNote,
				//timestamp changes
				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
				DTServerCreate = DateTime.Now,
				ChannelPartnerID = mgiContext.ChannelPartnerId
			};

			return FundProcessorDAL.Create(tranMap);
		}

		private string GetFormattedDate(DateTime? date)
		{
			if (date != null || date != (DateTime.MinValue))
			{
				DateTime dateConvert = (DateTime)date;
				return dateConvert.Month.ToString().PadLeft(2, '0') + dateConvert.Day.ToString().PadLeft(2, '0') + dateConvert.Year.ToString();
			}
			else
				return null;
		}

		private void _ValidateRegisterRequest(CardAccount customer, MGIContext mgiContext)
		{
			if (customer == null)
			{
				throw new FundException(FundException.INVALID_CUSTOMER_DETAILS, "Customer object can not be empty");
			}
			else if (string.IsNullOrWhiteSpace(customer.CardNumber))
			{
				throw new FundException(FundException.INVALID_CARD_NUMBER, "Account identifier should not be empty");
			}
			else if (string.IsNullOrWhiteSpace(customer.MailingAddress1)
				|| string.IsNullOrWhiteSpace(customer.MailingCity)
				|| string.IsNullOrWhiteSpace(customer.MailingState)
				|| string.IsNullOrWhiteSpace(customer.MailingZipCode))
			{
				throw new FundException(FundException.INVALID_MAILINGADDRESS, "Mailing address is not provided or incomplete");
			}
			else if (string.IsNullOrWhiteSpace(customer.GovernmentId) || string.IsNullOrWhiteSpace(customer.GovtIDCountry))
			{
				throw new FundException(FundException.INVALID_GOVTID_DETAILS, "Government ID number or issue country not provided");
			}
			else if (customer.GovtIDIssueDate != null)
			{
				if (customer.GovtIDIssueDate == DateTime.MinValue)
					throw new FundException(FundException.INVALID_IDISSUE_DATE, "Government ID issue date is not provided");
			}
			else if (customer.GovtIDIssueDate == null)
			{
				throw new FundException(FundException.INVALID_IDISSUE_DATE, "Government ID issue date is not provided");
			}
			else if (customer.GovtIDExpiryDate != null)
			{
				if (customer.GovtIDExpiryDate == DateTime.MinValue)
					throw new FundException(FundException.INVALID_IDEXPIRY_DATE, "Government ID Expiry date is not provided");
			}
			else if (customer.GovtIDExpiryDate == null)
			{
				throw new FundException(FundException.INVALID_IDEXPIRY_DATE, "Government ID Expiry date is not provided");
			}

			FirstViewIdTypes fvIdTypes = FundProcessorDAL.GetIdTypes(customer.IdTypeId);
			if (fvIdTypes == null)
				throw new FundException(FundException.GOVT_IDTYPE_NOT_FOUND, "Government ID Type not found");

			if (fvIdTypes.IdCode == "02" && string.IsNullOrWhiteSpace(customer.GovtIDIssueState))
				throw new FundException(FundException.INVALID_GOVTID_DETAILS, "Government ID issue state cannot be empty for this ID Type");

			//check if card is already associated to different customer
			//need to think about a different way of doing this.
			//long cxnAccountId = GetPanForCardNumber(customer.CardNumber, context);
			//if (cxnAccountId > 0)
			//    throw new FundException(FundException.CARD_ALREADY_REGISTERED, string.Format("Card number {0} is already registered", customer.CardNumber));
		}

		private long Update(CardAccount customer, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

			if (string.IsNullOrEmpty(timezone))
				throw new Exception("Time zone not provided in the context");

			string accountNumber = string.Empty;
			processorResult = null;
			long cxnAccountId = 0;

			_ValidateRegisterRequest(customer, mgiContext);

			int errorCode = 0;
			string errorMessage = string.Empty;
			processorResult = new ProcessorResult(0, false);

			try
			{
				errorCode = FundException.FIRSTVIEW_CREDENTIALS_NOTFOUND;
				errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", mgiContext.ChannelPartnerId);

				var credential = FundProcessorDAL.GetCredentials(mgiContext.ChannelPartnerId);

				if (credential != null)
				{
					errorCode = FundException.GOVT_IDTYPE_NOT_FOUND;
					errorMessage = string.Format("Error while retrieving government id type details for: {0}", customer.IdTypeId);

					FirstViewIdTypes fvIdTypes = FundProcessorDAL.GetIdTypes(customer.IdTypeId);

					var request = new AccountRequest()
					{
						DbbServiceName = "svcCardUpdate",
						ServiceUrl = credential.ServiceUrl,
						Application = credential.Application,
						dbbSystemExtLogin = credential.SystemExtLogin,
						Password = credential.Password,
						User = credential.User,
						deCIAClientID = credential.CIAClientID,

						deCIAPrimaryAccountNumber = Convert.ToInt64(customer.CardNumber),
						deCIAFirstName = customer.FirstName,
						deCIAMiddleName = customer.MiddleName,
						deCIALastName = customer.LastName,
						deCIADateOfBirth = GetFormattedDate(customer.DateOfBirth),
						deCIASSNNumber = string.IsNullOrWhiteSpace(customer.SSN) ? null : customer.SSN,
						deCIAGovernmentID = fvIdTypes.IdCode,
						deCIAIDNumber = (customer.GovtIDType == GovernmentIDType.SSN) ? customer.SSN : customer.GovernmentId,
						deCIAIDExpirationDate = GetFormattedDate(customer.GovtIDExpiryDate),
						deCIAIDIssueCountry = customer.GovtIDCountry,
						deCIAAddressLine1 = customer.Address1,
						deCIAAddressLine2 = customer.Address2,
						deCIACity = customer.City,
						deCIAState = customer.State,
						deCIACountryOfIssue = customer.GovtIDCountry,
						deCIAPostalCode = string.IsNullOrWhiteSpace(customer.ZipCode) ? null : customer.ZipCode,
						deCIAHomePhoneNumber = string.IsNullOrWhiteSpace(customer.Phone) ? null : customer.Phone,
						deCIAIDIssueDate = GetFormattedDate(customer.GovtIDIssueDate),
						deCIAIDIssueState = customer.GovtIDIssueState,
						deCIAShipContactName = customer.FirstName + " " + customer.LastName,
						deCIAShipAddress1 = customer.MailingAddress1,
						deCIAShipAddress2 = customer.MailingAddress2,
						deCIAShipToCity = customer.MailingCity,
						deCIAShipToState = customer.MailingState,
						deCIAShipToZipCode = customer.MailingZipCode
					};

					AccountResponse response = new AccountResponse();
					cxnAccountId = FundProcessorDAL.AddGprCard(BuildGPRCard(request, response, false, timezone));//Need to pass timezone
					processorResult = new ProcessorResult(0, true);
				}
				else
				{
					throw new FundException(errorCode, errorMessage);
				}
			}
			catch (Exception ex)
			{
				throw new FundException(errorCode, errorMessage, ex);
			}
			return cxnAccountId;
		}

		private void UpdateCommit(long cxnAccountId, MGIContext mgiContext, out ProcessorResult processorResult, string cardNumber = "")
		{
			string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

			if (string.IsNullOrEmpty(timezone))
				throw new Exception("Time zone not provided in the context");

			string accountNumber = string.Empty;
			processorResult = null;

			int errorCode = 0;
			string errorMessage = string.Empty;

			FirstViewIO fvIO = null;
			AccountResponse response = null;
			FirstViewCard customer = null;
			errorCode = FundException.FIRSTVIEW_CREDENTIALS_NOTFOUND;
			errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", mgiContext.ChannelPartnerId);

			var credential = FundProcessorDAL.GetCredentials(mgiContext.ChannelPartnerId);

			if (credential != null)
			{
				customer = FundProcessorDAL.Get(cxnAccountId);

				var request = new AccountRequest()
				{
					DbbServiceName = "svcCardUpdate",
					ServiceUrl = credential.ServiceUrl,
					Application = credential.Application,
					dbbSystemExtLogin = credential.SystemExtLogin,
					Password = credential.Password,
					User = credential.User,
					deCIAClientID = credential.CIAClientID,

					deCIAPrimaryAccountNumber = Convert.ToInt64(cardNumber),
					deCIAFirstName = customer.FirstName,
					deCIAMiddleName = customer.MiddleName,
					deCIALastName = customer.LastName,
					deCIADateOfBirth = GetFormattedDate((DateTime)customer.DateOfBirth),
					deCIASSNNumber = customer.SSN == null ? null : customer.SSN.ToString(),
					deCIAGovernmentID = customer.GovernmentId,
					deCIAIDNumber = (customer.GovernmentId == GovernmentIDType.SSN) ? customer.SSN.ToString() : customer.IDNumber,
					deCIAIDExpirationDate = GetFormattedDate((DateTime)customer.GovtIdExpirationDate),
					deCIAIDIssueCountry = customer.GovtIDIssueCountry,
					deCIAAddressLine1 = customer.AddressLine1,
					deCIAAddressLine2 = customer.AddressLine2,
					deCIACity = customer.City,
					deCIAState = customer.State,
					deCIAPostalCode = string.IsNullOrWhiteSpace(customer.PostalCode) ? null : customer.PostalCode,
					deCIAHomePhoneNumber = string.IsNullOrWhiteSpace(customer.HomePhoneNumber) ? null : customer.HomePhoneNumber,
					deCIAIDIssueDate = GetFormattedDate((DateTime)customer.GovtIDIssueDate),
					deCIAIDIssueState = customer.GovtIDIssueState,
					deCIAShipContactName = customer.FirstName + " " + customer.LastName,
					deCIAShipAddress1 = customer.ShippingAddressLine1,
					deCIAShipAddress2 = customer.ShippingAddressLine2,
					deCIAShipToCity = customer.ShippingCity,
					deCIAShipToState = customer.ShippingState,
					deCIAShipToZipCode = customer.ShippingZipCode
				};

				errorCode = FundException.PROVIDER_ERROR;
				errorMessage = string.Format("Error while activating card '{0}' in FirstView", customer.CardNumber);

				fvIO = new FirstViewIO();

				response = fvIO.UpdateCardInfo(request);

				if (string.Compare(response.ResErrorCode, "msgcu01", true) == 0)
				{
					processorResult = new ProcessorResult(0, true);
					customer.AccountNumber = response.ACCOUNTNUMBER;
					customer.BSAccountNumber = response.ACCOUNTNUMBER;
					customer.IsActive = true;
					//Changes for timestamp
					customer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
					customer.DTServerLastModified = DateTime.Now; //Added for TimeStamp
					FundProcessorDAL.UpdateCard(customer);
				}
				else
				{
					throw new FundException(errorCode, new FirstViewProviderException(Convert.ToString(FundProcessorDAL.GetExceptionMinorCode(response)), response.ResErrorMsg));
				}
			}
			else
			{
				throw new FundException(errorCode, errorMessage);
			}
		}

		private long Validate(long cxnAccountId, FundRequest fundRequest,
			RequestType type, MGIContext mgiContext, out ProcessorResult processorResult)
		{

			string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

			if (string.IsNullOrEmpty(timezone))
				throw new Exception("Time zone not provided in the context");

			int errorCode = 0;
			string errorMessage = string.Empty;

			try
			{
				string transactionType = "";
				if (type == RequestType.Credit)
					transactionType = TransactionType.Load;
				else if (type == RequestType.Debit)
					transactionType = TransactionType.Unload;
				else
					transactionType = TransactionType.None;

				//For catch, ErrorCode Setting
				errorCode = FundException.FIRSTVIEW_CREDENTIALS_NOTFOUND;
				errorMessage = string.Format("Error while retrieving the context for channel partner: {0}", mgiContext.ChannelPartnerId);

				var credential = FundProcessorDAL.GetCredentials(mgiContext.ChannelPartnerId);

				errorCode = FundException.ACCOUNT_NOT_FOUND;
				errorMessage = string.Format("Error while retrieving the account details for Id:{0}", cxnAccountId);
				FirstViewCard cardAccount = FundProcessorDAL.Get(cxnAccountId);

				if (cardAccount == null)
				{
					cardAccount = new FirstViewCard();
					cardAccount.BSAccountNumber = string.Empty;
				}

				//if (transactionType == MGI.Cxn.Fund.FirstView.Data.TransactionType.Load)
				//    errorCode = FundException.POST_DEBIT_TRANSACTION_ERROR;
				//else if (transactionType == MGI.Cxn.Fund.FirstView.Data.TransactionType.Unload)
				//    errorCode = FundException.POST_CREDIT_TRANSACTION_ERROR;
				//else if (transactionType == MGI.Cxn.Fund.FirstView.Data.TransactionType.None)
				//    errorCode = FundException.CARD_ACTIVATION_FAILED;

				errorCode = FundException.PROVIDER_ERROR;

				var request = new TransactionRequest()
				{
					DbbServiceName = "svcClearing",
					ServiceUrl = credential.ServiceUrl,
					Application = credential.Application,
					dbbSystemExtLogin = credential.SystemExtLogin,
					Password = credential.Password,
					User = credential.User,

					deClrSvcCustNbr = Convert.ToInt64(DecryptAccountNumber(cardAccount.BSAccountNumber)),
					deClrSvcTranType = transactionType,
					deClrSvcTransactionAmount = fundRequest.Amount,
					//deClrSvcCardAcceptorTerminalID = GetLocationId(mgiContext)
				};

				var response = new TransactionResponse();

				processorResult = new ProcessorResult(0, true);

				return AddMapping(request, response, Convert.ToInt16(mgiContext.ProcessorId), cardAccount, mgiContext, timezone);
			}
			catch (Exception ex)
			{
				processorResult = new ProcessorResult(errorCode, ex.Message, false, ex);
				return 0;
			}
		}

		private void HandleProcessorResult(CardResponse response, ProcessorResult processorResult, ErrorCode contextualErrorCode, Action function)
		{
			if (processorResult.Exception != null)
			{
				int errorCode = 0;
				int.TryParse(processorResult.ErrorCode.ToString(), out errorCode);
				throw new FundException(FundException.PROVIDER_ERROR, new FirstViewProviderException(Convert.ToString(FundProcessorDAL.GetExceptionMinorCode(response)), processorResult.ErrorMessage));
			}

			if (response.ERR_NUMBER.Equals("0") && string.Compare(response.CARD_STATUS, "active", true) == 0)
			{
				processorResult = new ProcessorResult(int.Parse(response.ERR_NUMBER), true);
				function();
			}
			else
			{
				int errorCode = 0;
				int.TryParse(processorResult.ErrorCode.ToString(), out errorCode);
				throw new FundException(FundException.PROVIDER_ERROR, new FirstViewProviderException(Convert.ToString(FundProcessorDAL.GetExceptionMinorCode(response)), processorResult.ErrorMessage));
			}
		}

		private string DecryptAccountNumber(string accountNumber)
		{
			if (accountNumber != null)
				return DataProtection.Decrypt(accountNumber, 0);
			else
				return null;
		}
		#endregion




		public void UpdateRegistrationDetails(CardAccount cardAccount, MGIContext context)
		{

		}


		public void Cancel(long accountId, MGIContext context)
		{

		}

		public long UpdateAccount(CardAccount cardAccount, MGIContext context)
		{
			return long.MinValue;
		}


		public List<TransactionHistory> GetTransactionHistory(long accountId, TransactionHistoryRequest request, MGIContext mgiContext)
		{
			return new List<TransactionHistory>();
		}


		public bool CloseAccount(long accountId, MGIContext mgiContext)
		{
			return true;
		}


		public bool UpdateCardStatus(long accountId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			return true;
		}


		public bool ReplaceCard(long accountId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			return true;
		}

		public List<MGI.Cxn.Fund.Data.ShippingTypes> GetShippingTypes(long channelPartnerId)
		{
			return null;
		}

		public double GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			return 0;
		}

		public double GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			return 0;
		}

		public long AssociateCard(CardAccount cardAccount, MGIContext mgiContext, bool isNewCard = false)
		{
			return -1;
		}
	}
}
