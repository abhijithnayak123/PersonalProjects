using MGI.Common.DataAccess.Contract;
using MGI.Common.DataProtection.Contract;
using MGI.Common.Util;
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.TSys.Contract;
using MGI.Cxn.Fund.TSys.Data;
using MGI.TimeStamp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.Fund.TSys.Impl
{
	public class TSysGateway : IFundProcessor, ITSysProcessor
	{
		private const string PREPAID_CARD_NOT_ACTIVE = "BLOCKED/INACTIVE";
		#region Dependencies
		private IIO _TSysIO;
		public IIO TSysIO { set { _TSysIO = value; } }

		private IDataProtectionService _dataProtectionSvc;
		public IDataProtectionService DataProtectionSvc { set { _dataProtectionSvc = value; } }

		private IRepository<TSysAccount> _tSysAccountRepo;
		public IRepository<TSysAccount> TSysAccountRepo { set { _tSysAccountRepo = value; } }

		private IRepository<TSysTransaction> _tSysTrxRepo;
		public IRepository<TSysTransaction> TSysTrxRepo { set { _tSysTrxRepo = value; } }
		public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }

		#endregion

		#region IFundProcessor Members

		public long Register(CardAccount cardAccount, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			// convert to a TSysAccount entity
			TSysAccount tSysAccount = TSysMapper.ToTSysAccount(cardAccount);

			bool existingAccount = mgiContext.IsExistingAccount;

			try
			{
				TSysIONewUser newUser;
				if (existingAccount)
					newUser = _TSysIO.ValidateExistingCardAccount(long.Parse(mgiContext.TSysPartnerId), cardAccount.AccountNumber);
				else
					newUser = _TSysIO.ValidateNewCardAccount(long.Parse(mgiContext.TSysPartnerId), cardAccount.AccountNumber, long.Parse(cardAccount.CardNumber));

				tSysAccount.ProgramId = long.Parse(mgiContext.TSysPartnerId);
				tSysAccount.CardNumber = encryptCardNumber(newUser.CardId.ToString()); // encrypt card before storing
				tSysAccount.AccountId = newUser.AccountId;
				tSysAccount.UserId = newUser.UserId;
				tSysAccount.PhoneType = "mobile";
				tSysAccount.Activated = existingAccount;
				//timestamp changes
				tSysAccount.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);//Need to pass timezone
				tSysAccount.DTServerCreate = DateTime.Now;

				_tSysAccountRepo.AddWithFlush(tSysAccount);

				processorResult = new ProcessorResult(true);
			}
			catch (TSysProviderException tex)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysAccount>(tSysAccount, "Register", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.TSys.Impl.TSysGateway", tex.Message, tex.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, "Failure validating new card account", tex);
			}

			return tSysAccount.Id;
		}

		public long Activate(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			TSysTransaction newTransaction = stageTransaction(accountId, TSysTransactionType.Activation, fundRequest.Amount, mgiContext);

			processorResult = new ProcessorResult(true);

			return newTransaction.Id;
		}

		public long Authenticate(string cardNumber, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			string encryptedCard = encryptCardNumber(cardNumber);
			// get account by encrypted card number
			TSysAccount tSysAccount = _tSysAccountRepo.FindBy(a => a.CardNumber == encryptedCard && a.Activated);

			long accountId = 0;
			if (tSysAccount == null)
				processorResult = new ProcessorResult(FundException.ACCOUNT_NOT_FOUND, false);
			else
			{
				accountId = tSysAccount.Id;

				try
				{
					_TSysIO.ValidateCard(tSysAccount.UserId, tSysAccount.AccountId, long.Parse(cardNumber));
					processorResult = new ProcessorResult(true);
				}
				catch (TSysProviderException tex)
				{
					//AL-3372 Transactional Log User Story
					MongoDBLogger.Error<TSysAccount>(tSysAccount, "Authenticate", AlloyLayerName.CXN, ModuleName.Funds,
						"Error in  -MGI.Cxn.Fund.TSys.Impl.TSysGateway", tex.Message, tex.StackTrace);

					processorResult = new ProcessorResult(FundException.PROVIDER_ERROR, tex.Message, false, tex);
				}
			}

			return accountId;
		}

		public CardInfo GetBalance(long accountId, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			TSysAccount tSysAccount = new TSysAccount();
			try
			{
				CardInfo cardBalance = new CardInfo();
				tSysAccount = getAccount(accountId);
				decimal balance = _TSysIO.GetBalance(tSysAccount.AccountId);
				cardBalance.Balance = balance;
				processorResult = new ProcessorResult(true);

				return cardBalance;
			}
			catch (TSysProviderException tpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysAccount>(tSysAccount, "GetBalance", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.TSys.Impl.TSysGateway", tpe.Message, tpe.StackTrace);
				throw new FundException(FundException.PROVIDER_ERROR, tpe.Message, tpe);
			}

		}

		public long Load(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			// get account
			TSysTransaction newTransaction = stageTransaction(accountId, TSysTransactionType.Credit, fundRequest.Amount, mgiContext);

			processorResult = new ProcessorResult(true);

			return newTransaction.Id;
		}

		public long Withdraw(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult)
		{
			TSysTransaction newTransaction = stageTransaction(accountId, TSysTransactionType.Debit, fundRequest.Amount, mgiContext);

			processorResult = new ProcessorResult(true);

			return newTransaction.Id;
		}

		public void Commit(long transactionId, MGIContext mgiContext, out ProcessorResult processorResult, string cardNumber = "")
		{
			TSysTransaction stagedTrx = getTSysTrx(transactionId);

			if (stagedTrx.TransactionType == TSysTransactionType.Activation)
				stagedTrx.Description = "Account activation";
			else
				stagedTrx.Description = string.Format("Teller {0} at {1}", stagedTrx.TransactionType == TSysTransactionType.Credit ? "load" : "withdraw", mgiContext.LocationName);

			try
			{
				string confirmationId = string.Empty;

				if (stagedTrx.TransactionType == TSysTransactionType.Activation)
					activateAccount(stagedTrx.Account, mgiContext.TimeZone);
				else if (stagedTrx.TransactionType == TSysTransactionType.Credit)
				{
					string detokenzidedCardNumber = decryptCardNumber(stagedTrx.Account.CardNumber);
					confirmationId = _TSysIO.Load(decryptCardNumber(stagedTrx.Account.CardNumber), stagedTrx.Amount, stagedTrx.Description);

					// special code for Synovus - call applyFee silently for the activation fee if this is the first credit
					if (firstLoadWithActivation(stagedTrx.Account.Id))
					{
						NLogger.Info(string.Format("First credit: calling applyFee silently for TSys account Id {0}", stagedTrx.Account.AccountId));
						string feeId = _TSysIO.ApplyFee(stagedTrx.Account.AccountId, 4m, "Account Opening Fee");
						NLogger.Info(string.Format("success. Fee confirmationId: {0}", feeId));
					}
				}
				else  // Debit
					confirmationId = _TSysIO.Withdraw(stagedTrx.Account.AccountId, stagedTrx.Amount, stagedTrx.Description);

				stagedTrx.ConfirmationId = confirmationId;
				stagedTrx.DTTransmission = DateTime.Now;
				stagedTrx.Status = TSysTransactionStatus.Committed;

				processorResult = new ProcessorResult(true);
				processorResult.ConfirmationNumber = stagedTrx.ConfirmationId;
			}
			catch (TSysProviderException tex)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysTransaction>(stagedTrx, "Commit", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.TSys.Impl.TSysGateway", tex.Message, tex.StackTrace);

				stagedTrx.Status = TSysTransactionStatus.Failed;
				stagedTrx.ErrorCode = tex.Code;
				stagedTrx.ErrorMsg = tex.Message;

				processorResult = new ProcessorResult(FundException.PROVIDER_ERROR, tex.Message, false, tex);
			}

			stagedTrx.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			stagedTrx.DTServerLastModified = DateTime.Now;

			_tSysTrxRepo.UpdateWithFlush(stagedTrx);
		}

		public CardAccount Lookup(long accountId)
		{
			TSysAccount tSysAcct = getAccount(accountId);

			CardAccount cardAccount = null;
			long cardNumber = 0L;
			if (tSysAcct.Activated == false)
				return null;

			try
			{
				cardNumber = _TSysIO.GetActiveCard(tSysAcct.UserId, tSysAcct.AccountId);
			}
			catch (TSysProviderException ex)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysAccount>(tSysAcct, "Lookup", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.TSys.Impl.TSysGateway", ex.Message, ex.StackTrace);

				if (ex.Code == FundException.NO_ACTIVE_CARD.ToString())
				{
					cardAccount = TSysMapper.ToCardAccount(tSysAcct);
					cardAccount.CardNumber = PREPAID_CARD_NOT_ACTIVE;
					cardAccount.CardBalance = -1.00M;
					return cardAccount;
				}
			}

			string encryptedCard = encryptCardNumber(cardNumber.ToString());
			if (encryptedCard != tSysAcct.CardNumber)
			{
				NLogger.Info(string.Format("active card has changed to {0}", ISOCard.EncodeCardNumber(cardNumber)));

				tSysAcct.CardNumber = encryptedCard;
				_tSysAccountRepo.UpdateWithFlush(tSysAcct);
			}

			cardAccount = TSysMapper.ToCardAccount(tSysAcct);
			cardAccount.CardNumber = cardNumber.ToString();
			//try
			//{
			//	cardAccount.CardBalance = _TSysIO.GetBalance(tSysAcct.AccountId);
			//}
			//catch (TSysProviderException tpe)
			//{
			//	throw new FundException(FundException.PROVIDER_ERROR, tpe.Message, tpe);
			//}

			return cardAccount;
		}

		public CardAccount LookupCardAccount(long accountId, bool isCardAccountActivated = false)
		{
			TSysAccount tSysAcct = getAccount(accountId);
			string detokenzidedCardNumber = decryptCardNumber(tSysAcct.CardNumber);
			tSysAcct.CardNumber = decryptCardNumber(tSysAcct.CardNumber);
			CardAccount cardAcct = TSysMapper.ToCardAccount(tSysAcct);

			return cardAcct;
		}

		public long GetPanForCardNumber(string cardNumber, MGIContext mgiContext)
		{
			ProcessorResult processorResult;
			return Authenticate(cardNumber, mgiContext, out processorResult);
		}

		public long UpdateAmount(long cxnFundTrxId, FundRequest fundRequest, string timezone)
		{
			TSysTransaction tSysTrx = getTSysTrx(cxnFundTrxId);

			tSysTrx.Amount = fundRequest.Amount;
			tSysTrx.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			tSysTrx.DTServerLastModified = DateTime.Now;

			_tSysTrxRepo.UpdateWithFlush(tSysTrx);

			return tSysTrx.Id;
		}

		public void UpdateRegistrationDetails(long cxnAccountId, string cardNumber, string timezone)
		{
			throw new NotImplementedException();
		}

		public void UpdateRegistrationDetails(CardAccount cardAccount, MGIContext mgiContext)
		{
			string timeZone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

			TSysAccount tSysAccount = getAccount(cardAccount.Id);

			tSysAccount.CardNumber = encryptCardNumber(cardAccount.CardNumber);
			tSysAccount.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timeZone);
			tSysAccount.DTServerLastModified = DateTime.Now;

			_tSysAccountRepo.UpdateWithFlush(tSysAccount);
		}

		public FundTrx Get(long cxnFundTrxId, MGIContext mgiContext)
		{
			TSysTransaction tSysTrx = getTSysTrx(cxnFundTrxId);

			FundTrx fundTrx = TSysMapper.ToFundTrx(tSysTrx);
			try
			{
				fundTrx.CardBalance = _TSysIO.GetBalance(tSysTrx.Account.AccountId);
			}
			catch (TSysProviderException tpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysTransaction>(tSysTrx, "Get", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.TSys.Impl.TSysGateway", tpe.Message, tpe.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, tpe.Message, tpe);
			}

			string detokenzidedCardNumber = decryptCardNumber(tSysTrx.Account.CardNumber);
			fundTrx.Account.FullCardNumber = decryptCardNumber(tSysTrx.Account.CardNumber);
			fundTrx.Account.CardNumber = fundTrx.Account.CardNumber.Length > 4 ? fundTrx.Account.CardNumber.Substring(fundTrx.Account.CardNumber.Length - 4) : fundTrx.Account.CardNumber;
			return fundTrx;
		}
		#endregion

		#region private methods

		private TSysAccount getAccount(long accountId)
		{
			TSysAccount tSysAcct = _tSysAccountRepo.FindBy(a => a.Id == accountId);

			if (tSysAcct == null)
				throw new FundException(FundException.ACCOUNT_NOT_FOUND, string.Format("Could not find funds account {0}", accountId));

			return tSysAcct;
		}

		private TSysTransaction getTSysTrx(long trxId)
		{
			TSysTransaction tSysTrx = _tSysTrxRepo.FindBy(x => x.Id == trxId);

			if (tSysTrx == null)
				throw new FundException(FundException.TRANSACTION_NOT_FOUND, string.Format("Could not find transaction Id {0}", trxId));

			return tSysTrx;
		}

		private TSysTransaction stageTransaction(long accountId, TSysTransactionType transactionType, decimal amount, MGIContext mgiContext)
		{
			TSysAccount tSysAccount = getAccount(accountId);

			// stage transaction
			try
			{
				TSysTransaction newTransaction = new TSysTransaction
				{
					Account = tSysAccount,
					Amount = amount,
					DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
					DTServerCreate = DateTime.Now,
					TransactionType = transactionType,
					Status = TSysTransactionStatus.Staged,
					Description = string.Empty,
					Balance = _TSysIO.GetBalance(tSysAccount.AccountId),
					ChannelPartnerID = mgiContext.ChannelPartnerId
				};

				_tSysTrxRepo.AddWithFlush(newTransaction);

				return newTransaction;
			}
			catch (TSysProviderException tpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysAccount>(tSysAccount, "stageTransaction", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.TSys.Impl.TSysGateway", tpe.Message, tpe.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, tpe.Message, tpe);
			}
		}

		private void activateAccount(TSysAccount tSysAccount, string timezone)
		{
			TSysIOProfile tSysProfile = TSysMapper.ToTSysIOProfile(tSysAccount);

			// decrypt card number to activate with TSys			
			tSysProfile.CardNumber = long.Parse(decryptCardNumber(tSysAccount.CardNumber));

			// update account details and activate
			try
			{
				_TSysIO.UpdateCardAccount(tSysProfile);
				_TSysIO.ActivateCardAccount(tSysProfile);
			}
			catch (TSysProviderException tpe)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysIOProfile>(tSysProfile, "activateAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in  -MGI.Cxn.Fund.TSys.Impl.TSysGateway", tpe.Message, tpe.StackTrace);

				throw new FundException(FundException.PROVIDER_ERROR, tpe.Message, tpe);
			}

			tSysAccount.Activated = true;
			tSysAccount.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			tSysAccount.DTServerLastModified = DateTime.Now;

			_tSysAccountRepo.UpdateWithFlush(tSysAccount);
		}

		private string decryptCardNumber(string encryptedCard)
		{
			return _dataProtectionSvc.Decrypt(encryptedCard, 0);
		}

		private string encryptCardNumber(string cardNumber)
		{
			return _dataProtectionSvc.Encrypt(cardNumber, 0);
		}

		private bool firstLoadWithActivation(long accountId)
		{
			List<TSysTransaction> accountTransactions = _tSysTrxRepo.FilterBy(t => t.Account.Id == accountId).ToList();

			// check for activation record AND no other loads
			return accountTransactions.Where(t => t.TransactionType == TSysTransactionType.Activation).Count() > 0 &&
					accountTransactions.Where(t => t.TransactionType == TSysTransactionType.Credit && t.Status == TSysTransactionStatus.Committed).Count() == 0;
		}
		#endregion

		public void Cancel(long accountId, MGIContext mgiContext)
		{

		}

		public long UpdateAccount(CardAccount cardAccount, MGIContext mgiContext)
		{
			string timezone = mgiContext.TimeZone;

			TSysAccount tSysAccount = getAccount(cardAccount.Id);
			tSysAccount.FirstName = cardAccount.FirstName;
			tSysAccount.MiddleName = cardAccount.MiddleName;
			tSysAccount.LastName = cardAccount.LastName;
			tSysAccount.Address1 = cardAccount.Address1;
			tSysAccount.Address2 = cardAccount.Address2;
			tSysAccount.City = cardAccount.City;
			tSysAccount.State = cardAccount.State;
			tSysAccount.ZipCode = cardAccount.ZipCode;
			tSysAccount.Country = cardAccount.CountryCode;
			tSysAccount.DateOfBirth = cardAccount.DateOfBirth;
			tSysAccount.SSN = cardAccount.SSN;
			tSysAccount.Phone = cardAccount.Phone;
			tSysAccount.PhoneType = cardAccount.PhoneType;
			tSysAccount.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
			tSysAccount.DTServerLastModified = DateTime.Now;
			tSysAccount.CardNumber = encryptCardNumber(tSysAccount.CardNumber);
			tSysAccount.IDCode = cardAccount.IDCode;
			_tSysAccountRepo.Update(tSysAccount);

			return tSysAccount.Id;
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
