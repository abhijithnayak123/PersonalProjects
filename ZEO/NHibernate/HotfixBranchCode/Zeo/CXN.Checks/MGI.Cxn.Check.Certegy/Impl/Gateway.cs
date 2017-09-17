using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.Check.Contract;
using MGI.Cxn.Check.Data;
using MGI.Cxn.Check.Certegy.Data;
using MGI.Cxn.Check.Certegy.Certegy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Util;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using MGI.Common.Logging;
using MGI.Cxn.Check.Certegy.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.Check.Certegy.Impl
{
	public class Gateway : ICheckProcessor
	{
		public Gateway()
		{
			#region Mappings

			Mapper.CreateMap<CheckAccount, Account>()
				.ForMember(x => x.Idcardnumber, s => s.MapFrom(c => c.GovernmentId))
				.ForMember(x => x.IdState, s => s.MapFrom(c => c.IDState))
				.ForMember(x => x.DTServerCreate, s => s.UseValue<DateTime>(DateTime.Now));

			Mapper.CreateMap<Account, CheckAccount>();

			Mapper.CreateMap<Transaction, CheckTrx>()
				.ForMember(x => x.ReturnFee, s => s.MapFrom(c => c.CertegyFee))
				.ForMember(x => x.Status, s => s.MapFrom(c => c.CheckStatus))
				.ForMember(x => x.DeclineCode, s => s.MapFrom(c => c.ResponseCode))
				.ForMember(x => x.ConfirmationNumber, s => s.MapFrom(c => c.ApprovalNumber))
				.ForMember(x => x.CheckNumber, s => s.MapFrom(c => c.CheckNumber))
				.ForMember(x => x.SubmitType, s => s.MapFrom(c => (CheckType)c.AlloySubmitType))
				.ForMember(x => x.ReturnType, s => s.MapFrom(c => (CheckType)c.AlloyReturnType))
				.AfterMap((s, d) =>
							  {
								  d.MetaData = new Dictionary<string, object>()
                                                   {
                                                       {"CertegyUID", s.CertegyUID},
                                                       {"AccountNumber", s.AccountNumber},
                                                       {"CheckNo", s.CheckNumber}
                                                   };
							  });
			Mapper.CreateMap<CheckTrx, Transaction>()
				.ForMember(x => x.IdType, opt => opt.Ignore());

			#endregion
		}

		#region Dependencies

		private Credential _credential = null;
		public NLoggerCommon NLogger = new NLoggerCommon();

		public IRepository<Account> AccountRepo { private get; set; }
		public IRepository<Credential> CredentialRepo { private get; set; }
		public IRepository<Transaction> TransactionRepo { private get; set; }
		public IRepository<CheckImage> ImageRepo { private get; set; }
		public IReadOnlyRepository<CheckTypeMapping> CheckTypeMappingRepo { private get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

		public IIO IO { private get; set; }

		#endregion

		#region ICheckProcessor Implementation

		/// <summary>
		/// Submits the check for processing with Certegy
		/// </summary>
		/// <param name="trxId">Transaction Id</param>
		/// <param name="accountId">Acclount Id</param>
		/// <param name="check">Check information</param>
		/// <param name="mgiContext">Dictionary object</param>
		/// <returns></returns>
		public CheckStatus Submit(long transactionId, long accountId, CheckInfo check, MGIContext mgiContext)
		{
			if (check == null)
				throw new ArgumentException("check Info parameter can not be null");

			ValidateContext(mgiContext);
			var timezone = mgiContext.TimeZone;

			if (check.MicrEntryType == (int)CheckEntryTypes.Manual)
				check.Micr = BuildMicrFromCheckInfo(check);

			LogCheckInformation(check);

			Account account = GetCertegyAccount(accountId);

			GetCredential(mgiContext);

			Transaction transaction = SetUpTransactionData(transactionId, account, check, _credential, mgiContext);
			TransactionRepo.AddWithFlush(transaction);

			if (check.MicrEntryType == (int)CheckEntryTypes.ScanWithImage)
				AddCheckImage(check, timezone, transaction);

			string erroReason = string.Empty;
			authorizeResponse response = null;
			reverseResponse reverseResponse = null;

			try
			{
				/* Web Service Call for Check AuthorizeSubmit */
				response = IO.AuthorizeCheck(transaction, _credential, mgiContext);
				transaction = UpdateTransactionData(transaction, response, mgiContext);
			}
			catch (CertegyProviderException ex)
			{
				transaction.CheckStatus = CheckStatus.Failed;

				//AL-3371 Transactional Log User Story(Process check)
				MongoDBLogger.Error<CheckInfo>(check, "Submit", AlloyLayerName.CXN, ModuleName.ProcessCheck,
						"Error in Submit - MGI.Cxn.Check.Certegy.Impl.Gateway", ex.Message, ex.StackTrace);
				
				throw new CheckException(CheckException.PROVIDER_ERROR, "Check Authorization Failed", ex);
			}
			catch (Exception ex)
			{
				transaction.CheckStatus = CheckStatus.Failed;
				reverseResponse = IO.ReverseCheck(transaction, _credential, mgiContext);

				//AL-3371 Transactional Log User Story(Process check)
				MongoDBLogger.Error<CheckInfo>(check, "Submit", AlloyLayerName.CXN, ModuleName.ProcessCheck,
						"Error in Submit - MGI.Cxn.Check.Certegy.Impl.Gateway", ex.Message, ex.StackTrace);
				
				throw ex;
			}
			finally
			{
				TransactionRepo.Merge(transaction);
			}

			return transaction.CheckStatus;
		}

		private string BuildMicrFromCheckInfo(CheckInfo check)
		{
			string micr = string.Empty;
			ValidateCheckInfo(check);
			micr = string.Format("T{0}A{1}C{2}", check.RoutingNumber.PadLeft(9, '9'), check.AccountNumber.PadRight(23), check.CheckNumber.PadLeft(15, '0'));
			return micr;
		}

		private static void ValidateCheckInfo(CheckInfo check)
		{
			if (string.IsNullOrWhiteSpace(check.RoutingNumber))
			{
				throw new ArgumentException("RoutingNumber can not be null");
			}

			if (string.IsNullOrWhiteSpace(check.AccountNumber))
			{
				throw new ArgumentException("AccountNumber can not be null");
			}

			if (string.IsNullOrWhiteSpace(check.CheckNumber))
			{
				throw new ArgumentException("CheckNumber can not be null");
			}
		}

		private void LogCheckInformation(CheckInfo check)
		{
			//Validate check image data
			if (check.MicrEntryType == (int)CheckEntryTypes.ScanWithImage)
			{
				NLogger.Info("Logging Check Information for scan with image option");

				int frontImageTIFFLength = check.FrontImageTIF == null ? 0 : check.FrontImageTIF.Length;
				int backImageTIFFLength = check.BackImageTIF == null ? 0 : check.BackImageTIF.Length;

				NLogger.Info(string.Format("Front Image Size of TIFF is {0}", frontImageTIFFLength.ToString()));
				NLogger.Info(string.Format("Back Image Size of TIFF is {0}", backImageTIFFLength.ToString()));

				NLogger.Info(LogImageDetails(check.FrontImageTIF, "TIFF Front Image"));

				NLogger.Info(LogImageDetails(check.BackImageTIF, "TIFF Back Image"));
				NLogger.Info(LogImageDetails(check.FrontImage, "JPEG Front Image"));
				NLogger.Info(LogImageDetails(check.BackImage, "JPEG Back Image"));

				NLogger.Info(string.Format("Routing Number: {0}", check.RoutingNumber));
				NLogger.Info(string.Format("Account Number: {0}", check.AccountNumber));
				NLogger.Info(string.Format("Check Number: {0}", check.CheckNumber));
			}

			if (check.MicrEntryType == (int)CheckEntryTypes.ScanWithoutImage)
			{
				NLogger.Info("Logging Check Information for scan without image entry option");
				NLogger.Info(string.Format("Routing Number: {0}", check.RoutingNumber));
				NLogger.Info(string.Format("Account Number: {0}", check.AccountNumber));
				NLogger.Info(string.Format("Check Number: {0}", check.CheckNumber));
				NLogger.Info(string.Format("MICR: {0}", check.Micr));
			}

			if (check.MicrEntryType == (int)CheckEntryTypes.Manual)
			{
				NLogger.Info("Logging Check Information for manual entry option");
				NLogger.Info(string.Format("Routing Number: {0}", check.RoutingNumber));
				NLogger.Info(string.Format("Account Number: {0}", check.AccountNumber));
				NLogger.Info(string.Format("Check Number: {0}", check.CheckNumber));
				NLogger.Info(string.Format("MICR: {0}", check.Micr));
			}

		}

		private Transaction UpdateTransactionData(Transaction transaction, authorizeResponse response, MGIContext mgiContext)
		{
			string timezone = GetTimeZone(mgiContext);
			int responsCode = int.Parse(response.ResponseCode);

			if (responsCode == 0)
			{
				transaction.CheckStatus = CheckStatus.Approved;
				transaction.ResponseCode = responsCode;
				transaction.SettlementID = response.SettlementID;
				transaction.ApprovalNumber = response.ApprovalNumber;

				if (!string.IsNullOrWhiteSpace(response.CertegyUID)) transaction.CertegyUID = response.CertegyUID;
				if (!string.IsNullOrWhiteSpace(response.CheckABA)) transaction.RoutingNumber = response.CheckABA;
				if (!string.IsNullOrWhiteSpace(response.CheckAcct)) transaction.AccountNumber = response.CheckAcct;
				if (!string.IsNullOrWhiteSpace(response.CheckNumber)) transaction.CheckNumber = response.CheckNumber;

				if (!string.IsNullOrWhiteSpace(response.CheckType))
				{
					transaction.CertegyReturnType = response.CheckType;
					//Check to see if check is reclassified
					if (transaction.CertegySubmitType.Trim() != transaction.CertegyReturnType)
						transaction.AlloyReturnType = GetAlloyCheckType(response.CheckType);
				}
				//validate before reading value
				transaction.CertegyFee = response.Fee;
			}

			else
			{
				transaction.CheckStatus = CheckStatus.Declined;
				transaction.ResponseCode = responsCode;

				if (!string.IsNullOrWhiteSpace(response.CertegyUID)) transaction.CertegyUID = response.CertegyUID;
				if (!string.IsNullOrWhiteSpace(response.CheckABA)) transaction.RoutingNumber = response.CheckABA;
				if (!string.IsNullOrWhiteSpace(response.CheckAcct)) transaction.AccountNumber = response.CheckAcct;
				if (!string.IsNullOrWhiteSpace(response.CheckNumber)) transaction.CheckNumber = response.CheckNumber;
			}

			transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			transaction.DTServerLastModified = DateTime.Now;

			return transaction;
		}

		/// <summary>
		/// Gets Certegy account from Database
		/// </summary>
		/// <param name="accountId">Account Id</param>
		/// <returns></returns>
		private Account GetCertegyAccount(long accountId)
		{
			Account account = AccountRepo.FindBy(x => x.Id == accountId);

			if (account == null)
				throw new CheckException(CheckException.ACCOUNT_NOT_FOUND, string.Format("could not find account {0}", accountId));

			return account;
		}

		/// <summary>
		/// Adds Check Image to tCertegy_CheckImages
		/// </summary>
		/// <param name="check">Check information to be added</param>
		/// <param name="timezone">Customer time zone</param>
		/// <param name="transaction">Certegy Transaction</param>
		private void AddCheckImage(CheckInfo check, string timezone, Transaction transaction)
		{
			CheckImage certegyImages = ConvertToCheckImage(check);

			certegyImages.DTServerCreate = DateTime.Now;
			certegyImages.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(Convert.ToString(timezone));
			certegyImages.CertegyTrx = transaction;

			ImageRepo.AddWithFlush(certegyImages);
		}

		/// <summary>
		/// Commits the transaction
		/// </summary>
		/// <param name="trxId">Transaction Id to commit</param>
		/// <param name="timezone">Customer time zone</param>
		/// <param name="context"></param>
		public void Commit(long transactionId, string timezone, MGIContext mgiContext)
		{

			ValidateContext(mgiContext);

			Transaction transaction = GetCertegyTransaction(transactionId);

			transaction.CheckStatus = CheckStatus.Cashed;
			transaction.DTServerLastModified = DateTime.Now;
			transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);

			TransactionRepo.Merge(transaction);
		}

		/// <summary>
		/// Gets Check Transaction by Transaction Id
		/// </summary>
		/// <param name="trxId"></param>
		/// <returns></returns>
		public CheckTrx Get(long transactionId)
		{
			Transaction transaction = GetCertegyTransaction(transactionId);

			CheckTrx checkTransaction = Mapper.Map<Transaction, CheckTrx>(transaction);

			checkTransaction.SubmitType = getCheckType(transaction.AlloySubmitType);
			checkTransaction.ReturnType = getCheckType(transaction.AlloyReturnType);

			return checkTransaction;
		}

		/// <summary>
		/// Gets the Certegy transaction status
		/// </summary>
		/// <param name="trxId">Certegy Transaction Id</param>
		/// <param name="timezone">Customer time zone</param>
		/// <param name="context">Dictionary object</param>
		/// <returns></returns>
		public CheckStatus Status(long transactionId, string timezone, MGIContext mgiContext)
		{
			Transaction transaction = GetCertegyTransaction(transactionId);

			return transaction.CheckStatus;
		}

		/// <summary>
		/// Cancel the Certegy Check transaction
		/// </summary>
		/// <param name="trxId">Tansaction Id</param>
		/// <param name="timezone">Customer time zome</param>
		/// <param name="mgiContext">Dictionary object </param>
		public bool Cancel(long transactionId, string timezone, MGIContext mgiContext)
		{
			Transaction transaction = GetCertegyTransaction(transactionId);
			reverseResponse response = null;
			int responseCode = 0;
			bool checkCancelStatus = false;

			GetCredential(mgiContext);
			try
			{
				//Proceed to cancel the transaction. This method call will issue Check reversal IO request Certegy
				response = IO.ReverseCheck(transaction, _credential, mgiContext);
			}
			catch (CertegyProviderException ex)
			{
				//AL-3371 Transactional Log User Story(Process check)
				MongoDBLogger.Error<Transaction>(transaction, "Cancel", AlloyLayerName.CXN, ModuleName.ProcessCheck,
						"Error in Cancel - MGI.Cxn.Check.Certegy.Impl.Gateway", ex.Message, ex.StackTrace);
				
				throw new CheckException(CheckException.PROVIDER_ERROR, "Check Reversal Failed", ex);
			}

			responseCode = int.Parse(response.ResponseCode);

			//Response code 13 is a successful Check reversal
			if (responseCode == 13)
			{
				transaction.CheckStatus = CheckStatus.Canceled;
				transaction.ResponseCode = responseCode;
				checkCancelStatus = true;
			}

			TransactionRepo.Merge(transaction);
			return checkCancelStatus;
		}

		public List<CheckTrx> PendingChecks()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This will register an user account for Certegy
		/// </summary>
		/// <param name="account">Check Account</param>
		/// <param name="mgiContext">class object</param>
		/// <param name="timezone">Customer Timezone</param>
		public long Register(CheckAccount account, MGIContext mgiContext, string timezone)
		{
			//Call method to validate incoming context object
			if (account == null)
				throw new ArgumentException("checkAccount parameter can not be null");

			ValidateContext(mgiContext);
			Account certegyAccount = Mapper.Map<Account>(account);
			certegyAccount.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			AccountRepo.AddWithFlush(certegyAccount);

			return certegyAccount.Id;
		}

		/// <summary>
		/// Updates the Certegy user account table tCertegy_Account with the updated data
		/// </summary>
		/// <param name="account">Check Account with updated data</param>
		/// <param name="mgiContext">class object</param>
		public void Update(CheckAccount account, MGIContext mgiContext)
		{
			object timeZone = mgiContext.TimeZone;

			Account certegyAccount = GetCertegyAccount(account.Id);

			certegyAccount = Mapper.Map<CheckAccount, Account>(account, certegyAccount);
			certegyAccount.DTServerLastModified = DateTime.Now;
			certegyAccount.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(Convert.ToString(timeZone));
			AccountRepo.Merge(certegyAccount);

		}

		/// <summary>
		/// Updates the Certegy Transaction table tCertegy_Trx
		/// </summary>
		/// <param name="checkTrx">Check Transaction</param>
		/// <param name="mgiContext">Dictionary object</param>
		public void Update(CheckTrx checkTransaction, MGIContext mgiContext)
		{
			Transaction certegyTransaction = GetCertegyTransaction(checkTransaction.Id);

			certegyTransaction = Mapper.Map<CheckTrx, Transaction>(checkTransaction, certegyTransaction);

			TransactionRepo.Merge(certegyTransaction);
		}

		public CheckProcessorInfo GetCheckProcessorInfo(string locationId)
		{
			return new CheckProcessorInfo();
		}

		/// <summary>
		/// Updates the Transaction's check frank flag to true on check franking
		/// </summary>
		/// <param name="trxId">Transaction Id</param>
		public void UpdateTransactionFranked(long transactionId)
		{
			Transaction transaction = GetCertegyTransaction(transactionId);
			transaction.IsCheckFranked = true;
			TransactionRepo.Merge(transaction);
		}

		/// <summary>
		/// Gets Check Account by account Id 
		/// </summary>
		/// <param name="accountId">Check Account Id</param>
		/// <returns></returns>
		public CheckAccount GetAccount(long accountId)
		{
			Account certegyAccount = AccountRepo.FindBy(a => a.Id == accountId);

			if (certegyAccount == null)
				throw new CheckException(CheckException.ACCOUNT_NOT_FOUND, string.Format("Could not find Certegy account {0}", accountId));

			return Mapper.Map<CheckAccount>(certegyAccount);
		}

		public CheckLogin GetCheckSessions(MGIContext mgiContext)
		{
			return null;
		}

		#endregion

		#region Private Methods

		private Transaction GetCertegyTransaction(long transactionId)
		{
			Transaction transaction = TransactionRepo.FindBy(x => x.Id == transactionId);
			//transaction.CheckStatus = (CheckStatus)Enum.Parse(typeof(CheckStatus), transaction.CertegyStatus, true); ;

			if (transaction == null)
				throw new CheckException(CheckException.TRANSACTION_NOT_FOUND, string.Format("Could not find Id {0}", transactionId));

			return transaction;
		}

		private CheckType getCheckType(int alloyType)
		{
			CheckType checkType = CheckType.TwoParty;
			CheckTypeMapping certegyCheckTypeMap = CheckTypeMappingRepo.FindBy(t => (int)t.CheckType == alloyType);

			if (certegyCheckTypeMap != null)
				checkType = certegyCheckTypeMap.CheckType;

			return checkType;
		}

		private int GetAlloyCheckType(string certegyType)
		{
			switch (certegyType.ToLower())
			{
				case "y":
					return (int)CheckType.Cashier;

				case "g":
					return (int)CheckType.GovtUSOther;

				default:
					return (int)CheckType.TwoParty;
			}
		}

		private string getCertegyType(CheckType checkType)
		{
			List<CheckTypeMapping> certegyCheckTypes = CheckTypeMappingRepo.FilterBy(t => t.CheckType == checkType).ToList();
			return certegyCheckTypes.OrderBy(t => t.CheckType).First().Name;
		}

		private string LogImageDetails(byte[] imgByte, string imageType)
		{
			String logStr = "Image Properties : " + imageType + "\n ";

			using (MemoryStream ms = new MemoryStream(imgByte))
			{
				Image chkImage = Image.FromStream(ms);
				int compressionTagIndex = Array.IndexOf(chkImage.PropertyIdList, 0x103);
				if (compressionTagIndex >= 0)
				{
					PropertyItem compressionTag = chkImage.PropertyItems[compressionTagIndex];
					if (BitConverter.ToInt16(compressionTag.Value, 0) == 1)
						logStr += "Compression:Uncompressed TIFF 6.0 ";
					else if (BitConverter.ToInt16(compressionTag.Value, 0) == 2)
						logStr += "Compression:CCITT Group 3(CCITT 1D) TIFF 6.0 ";
					else if (BitConverter.ToInt16(compressionTag.Value, 0) == 3)
						logStr += "Compression:CCITT T.4 bi-level encoding(CCITT Group 3 2D)  TIFF 6.0 ";
					else if (BitConverter.ToInt16(compressionTag.Value, 0) == 4)
						logStr += "Compression:CCITT T.6 bi-level encoding (CCITT Group 4 fax encoding) TIFF 6.0  ";
					else if (BitConverter.ToInt16(compressionTag.Value, 0) == 5)
						logStr += "LZW  TIFF 6.0 ";
					else if (BitConverter.ToInt16(compressionTag.Value, 0) > 5)
						logStr += "Compression:Other ";
				}
				else
					logStr += "Compression:Other ";

				logStr += "Image Length: " + imgByte.Length + " ";
				logStr += "Pixel Format: " + chkImage.PixelFormat + " ";
				logStr += "Image Width: " + chkImage.PhysicalDimension.Width + " ";
				logStr += "Image Height: " + chkImage.PhysicalDimension.Height + " ";
				logStr += "Image Resolution: " + chkImage.HorizontalResolution + " X " + chkImage.VerticalResolution;
			}
			return logStr;
		}

		private Transaction SetUpTransactionData(long transactionId, Account account, CheckInfo check, Credential credential, MGIContext mgiContext)
		{
			string timezone = GetTimeZone(mgiContext);
			string siteId = mgiContext.CertegySiteId;
			string terminalName = mgiContext.TerminalName;

			string certegyType = getCertegyType(check.Type);
			long channelPartnerId = PartnerId(mgiContext);
			string certegyIdType = GetCertegyIdType(account.IDType, account.IdState);
			//Micr Entry Types M: Manual, S: Scanned
			string micrEntryType = check.MicrEntryType == 3 ? "M" : "S";

			return new Transaction
			{
				Id = transactionId,
				CheckAmount = check.Amount,
				Micr = check.Micr,
				CheckDate = check.IssueDate,
				AccountNumber = check.AccountNumber,
				RoutingNumber = check.RoutingNumber,
				CheckNumber = check.CheckNumber,
				SiteID = siteId,
				IdType = certegyIdType,
				Version = credential.Version,
				DeviceId = terminalName,
				DeviceIP = credential.DeviceIP,
				DeviceType = credential.DeviceType,
				MicrEntryType = micrEntryType,
				AlloySubmitType = (int)check.Type,
				AlloyReturnType = (int)check.Type,
				CertegySubmitType = certegyType,
				CertegyReturnType = certegyType,
				CertegyAccount = account,
				CheckStatus = CheckStatus.Approved,
				ChannelPartnerID = channelPartnerId,
				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
				DTServerCreate = DateTime.Now
			};
		}

		private static string GetTimeZone(MGIContext mgiContext)
		{
			string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

			if (string.IsNullOrEmpty(timezone))
				throw new Exception("Time zone not provided in the context");
			return timezone;
		}

		private long PartnerId(MGIContext mgiContext)
		{
			if (mgiContext.ChannelPartnerId == 0)
				throw new CheckException(CheckException.PARTNER_NOT_SET, "PartnerId not set in the context");

			return mgiContext.ChannelPartnerId;
		}

		private CheckImage ConvertToCheckImage(CheckInfo check)
		{
			if (check.FrontImage.Length == 0 || check.BackImage.Length == 0 || check.FrontImageTIF.Length == 0 || check.BackImageTIF.Length == 0)
				throw new CheckException(CheckException.MISSING_IMAGE, "One or more check images missing");

			return new CheckImage
			{
				Front = check.FrontImage,
				Back = check.BackImage,
				Format = check.ImageFormat,
				FrontTIF = check.FrontImageTIF,
				BackTIF = check.BackImageTIF
			};
		}

		private void ValidateContext(MGIContext mgiContext)
		{
			if (mgiContext == null)
			{
				throw new ArgumentException("context parameter can not be null");
			}

			if (string.IsNullOrEmpty(mgiContext.TimeZone))
			{
				throw new ArgumentException("TimeZone key unavailable in context parameter");
			}

			if (mgiContext.ChannelPartnerId == 0)
			{
				throw new ArgumentException("ChannelPartnerId key unavailable in context parameter");
			}

			if (string.IsNullOrEmpty(mgiContext.CertegySiteId))
			{
				throw new ArgumentException("SiteId key unavailable in context parameter");
			}

			if (string.IsNullOrEmpty(mgiContext.TerminalName))
			{
				throw new ArgumentException("Terminal Name key unavailable in context parameter");
			}
		}

		private void GetCredential(MGIContext mgiContext)
		{
			if (_credential == null)
			{
				long channelPartnerId = mgiContext.ChannelPartnerId;
				_credential = CredentialRepo.FindBy(c => c.ChannelPartnerId == channelPartnerId);
			}

		}

		public string GetCertegyIdType(string idType, string idState)
		{
			Dictionary<string, string> govtIdTypeMapping = new Dictionary<string, string>()
			{
				{"DRIVER'S LICENSE", idState},
				{"EMPLOYMENT AUTHORIZATION CARD (EAD)", "AD"},
				{"GREEN CARD / PERMANENT RESIDENT CARD", "AD"},
				{"MILITARY ID", "CZ"},
				{"PASSPORT", "PP"},
				{"U.S. STATE IDENTITY CARD", idState},
				{"MATRICULA CONSULAR", "MX"},
				{"NEW YORK BENEFITS ID", idState},
				{"NEW YORK CITY ID",idState}
			};
			if (!govtIdTypeMapping.ContainsKey(idType))
			{
				throw new ArgumentException("ID Type Not Supported by Certegy");
			}
			return govtIdTypeMapping[idType];
		}
		#endregion
	}
}
