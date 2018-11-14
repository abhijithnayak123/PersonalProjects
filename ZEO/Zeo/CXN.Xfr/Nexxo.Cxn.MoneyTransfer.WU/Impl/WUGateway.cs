using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.WU.Impl;
using MGI.Cxn.MoneyTransfer.WU.ModifySendMoney;
using MGI.Cxn.MoneyTransfer.WU.ReceiveMoneyPay;
using MGI.Cxn.MoneyTransfer.WU.ReceiveMoneySearch;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyStore;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyValidation;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Account = MGI.Cxn.MoneyTransfer.Data.Account;
using CxnData = MGI.Cxn.MoneyTransfer.Data;
using PaymentDetails = MGI.Cxn.MoneyTransfer.Data.PaymentDetails;
using Receiver = MGI.Cxn.MoneyTransfer.Data.Receiver;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.MoneyTransfer.WU.Impl
{
	public partial class WUGateway : IMoneyTransfer
	{
		private const string CountryName = "United States";
		private const string CountryCode = "US";
		private const string CountryCurrencyCode = "USD";
		private const string Language = "es";
		public NLoggerCommon NLogger { get; set; }
		public bool IsHardCodedCounterId { get; set; }
		public IWUCommonIO WuCommon { get; set; }

		private IRepository<WUReceiver> _WUReceiverRepo;
		public IRepository<CountryTransalation> WUCountryTransalationRepo { get; set; }
		public IRepository<DeliveryServiceTransalation> WUDeliveryServiceTransalationRepo { get; set; }

		public IRepository<WUReceiver> WUReceiverRepo
		{
			set { _WUReceiverRepo = value; }
		}

		private IRepository<WUTransaction> _wuTransactionLogRepo;

		public IRepository<WUTransaction> WUTransactionLogRepo
		{
			set { _wuTransactionLogRepo = value; }
		}

		public TLoggerCommon MongoDBLogger { private get; set; }

		public IRepository<WUAccount> WUAccountRepo { private get; set; }

		public IIO WUIO { private get; set; }

		//private string ErrorMessage;

		public string AllowDuplicateTrxWU { private get; set; }

		public WUGateway()
		{

			#region Auto Mapper

			AutoMapper.Mapper.CreateMap<Sender, WUTransaction>();

			AutoMapper.Mapper.CreateMap<PaymentDetails, WUTransaction>()
				.ForMember(x => x.ExpectedPayoutCityName, s => s.MapFrom(c => c.ExpectedPayoutLocCity))
				.ForMember(x => x.ExpectedPayoutStateCode, s => s.MapFrom(c => c.ExpectedPayoutStateCode))
				.ForMember(x => x.Charges, s => s.MapFrom(c => c.Fee))
				.ForMember(x => x.DeliveryServiceName, s => s.MapFrom(c => c.DeliveryMethod))
				.ForMember(x => x.plus_charges_amount, s => s.MapFrom(c => c.OtherFees))
				.ForMember(x => x.PromotionDiscount, s => s.MapFrom(c => c.PromotionDiscount / 100m));

			AutoMapper.Mapper.CreateMap<WUTransaction, Sender>();

			//US1685
			AutoMapper.Mapper.CreateMap<Transaction, WUTransaction>()
				.ForMember(x => x.WUnionAccount, s => s.MapFrom(c => c.Account))
				.ForMember(x => x.WUnionRecipient, s => s.MapFrom(c => c.Receiver))
				.ForMember(x => x.MTCN, s => s.MapFrom(c => c.ConfirmationNumber))
				.ForMember(x => x.Charges, s => s.MapFrom(c => c.Fee))
				.ForMember(x => x.OriginatorsPrincipalAmount, s => s.MapFrom(c => c.TransactionAmount))
				.ForMember(x => x.TranascationType, s => s.MapFrom(c => c.TransactionType));

			#region Receiver

			AutoMapper.Mapper.CreateMap<Receiver, WUReceiver>();
			AutoMapper.Mapper.CreateMap<WUReceiver, Receiver>();

			AutoMapper.Mapper.CreateMap<Account, MGI.Cxn.WU.Common.Data.Sender>();
			AutoMapper.Mapper.CreateMap<MGI.Cxn.WU.Common.Data.Sender, Account>();

			AutoMapper.Mapper.CreateMap<WUAccount, Account>()
				.ForMember(x => x.LoyaltyCardNumber, s => s.MapFrom(c => c.PreferredCustomerAccountNumber))
				.ForMember(x => x.LevelCode, s => s.MapFrom(c => c.PreferredCustomerLevelCode));

			AutoMapper.Mapper.CreateMap<Account, WUAccount>()
				.ForMember(x => x.PreferredCustomerAccountNumber, s => s.MapFrom(c => c.LoyaltyCardNumber))
				.ForMember(x => x.PreferredCustomerLevelCode, s => s.MapFrom(c => c.LevelCode));

			#endregion

			#region CardDetails
			AutoMapper.Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.PaymentDetails, MGI.Cxn.WU.Common.Data.CardDetails>()
			   .AfterMap((paymentDetail, CardDetails) => CardDetails.paymentDetails = new MGI.Cxn.WU.Common.Data.PaymentDetails()
			   {
				   destination_country_currency = new MGI.Cxn.WU.Common.Data.CountryCurrencyInfo() { country_code = paymentDetail.DestinationCountryCode, currency_code = paymentDetail.DestinationCurrencyCode },
				   originating_country_currency = new MGI.Cxn.WU.Common.Data.CountryCurrencyInfo() { country_code = paymentDetail.OriginatingCountryCode, currency_code = paymentDetail.OriginatingCurrencyCode },
				   recording_country_currency = new MGI.Cxn.WU.Common.Data.CountryCurrencyInfo() { country_code = paymentDetail.RecordingcountrycurrencyCountryCode, currency_code = paymentDetail.RecordingcountrycurrencyCurrencyCode }
			   });
			AutoMapper.Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.Account, MGI.Cxn.WU.Common.Data.CardDetails>()
				.AfterMap((sender, CardDetails) => CardDetails.sender = new MGI.Cxn.WU.Common.Data.Sender()
				{
					FirstName = sender.FirstName,
					LastName = sender.LastName,
					AddressAddrLine1 = sender.Address,
					AddressCity = sender.City,
					AddressState = sender.State,
					AddressPostalCode = sender.PostalCode,
					ContactPhone = sender.ContactPhone,
				});
			AutoMapper.Mapper.CreateMap<MGI.Cxn.WU.Common.Data.CardDetails, MGI.Cxn.MoneyTransfer.Data.CardDetails>();
			AutoMapper.Mapper.CreateMap<MGI.Cxn.WU.Common.Data.CardInfo, MGI.Cxn.MoneyTransfer.Data.CardInfo>();
			#endregion
			#region Mapping for response to CardDetails output object
			AutoMapper.Mapper.CreateMap<MGI.Cxn.WU.Common.Data.CardDetails, MGI.Cxn.MoneyTransfer.Data.CardDetails>();
			#endregion
			#region CardLookupRequest
			AutoMapper.Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.CardLookUpRequest, MGI.Cxn.WU.Common.Data.CardLookUpRequest>();
			#endregion
			#region Mapping for response to CardLookupDetails output object
			AutoMapper.Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.CardLookupDetails, MGI.Cxn.WU.Common.Data.CardLookupDetails>();
			#endregion

			AutoMapper.Mapper.CreateMap<WUTransaction, WUTransaction>()
					.ForMember(x => x.Id, o => o.Ignore())
					.ForMember(x => x.rowguid, o => o.Ignore());

			//US2054
			AutoMapper.Mapper.CreateMap<SwbFlaInfo, FeeInquiry.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			AutoMapper.Mapper.CreateMap<GeneralName, FeeInquiry.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			AutoMapper.Mapper.CreateMap<SwbFlaInfo, SendMoneyValidation.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			AutoMapper.Mapper.CreateMap<GeneralName, SendMoneyValidation.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			AutoMapper.Mapper.CreateMap<SwbFlaInfo, SendMoneyStore.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			AutoMapper.Mapper.CreateMap<GeneralName, SendMoneyStore.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			AutoMapper.Mapper.CreateMap<SwbFlaInfo, SendMoneyRefund.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			AutoMapper.Mapper.CreateMap<GeneralName, SendMoneyRefund.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			AutoMapper.Mapper.CreateMap<SwbFlaInfo, ModifySendMoney.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			AutoMapper.Mapper.CreateMap<GeneralName, ModifySendMoney.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			AutoMapper.Mapper.CreateMap<SwbFlaInfo, ReceiveMoneySearch.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			AutoMapper.Mapper.CreateMap<GeneralName, ReceiveMoneySearch.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			AutoMapper.Mapper.CreateMap<SwbFlaInfo, ReceiveMoneyPay.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			AutoMapper.Mapper.CreateMap<GeneralName, ReceiveMoneyPay.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));
			#endregion
		}

		private Receiver DBReceiver = new Receiver();

		private bool blnNoChange = false;

		/// <summary>
		/// US2054
		/// </summary>
		/// <param name="locationState"></param>
		/// <returns></returns>
		public bool IsSWBState(string locationState)
		{
			try
			{
				return WuCommon.IsSWBState(locationState);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(locationState, "IsSWBState", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in IsSWBState - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_ISSWBSTATE_FAILED, ex);
			}
		}

		public Receiver GetReceiver(long Id)
		{
			try
			{
				WUReceiver wuReceiver = _WUReceiverRepo.FindBy(x => x.Id == Id);

				Receiver receiver = AutoMapper.Mapper.Map<WUReceiver, Receiver>(wuReceiver);

				return receiver;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(Id), "GetReceiver", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetReceiver - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETRECEIVER_FAILED,  ex);
			}
		}

		public Receiver GetActiveReceiver(long Id)
		{
			try
			{
				WUReceiver wuReceiver = _WUReceiverRepo.FindBy(x => x.Id == Id && x.Status == "Active");

				Receiver receiver = AutoMapper.Mapper.Map<WUReceiver, Receiver>(wuReceiver);

				return receiver;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(Id), "GetActiveReceiver", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetActiveReceiver - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETACTIVERECEIVER_FAILED, ex);
			}
		}

		public Receiver GetReceiver(long customerId, string fullName)
		{
			try
			{
				IQueryable<WUReceiver> wuReceivers = _WUReceiverRepo.FilterBy(c => (c.FirstName + " " + c.LastName).ToLower() == fullName.ToLower() && c.CustomerId == customerId && c.Status == "Active");
				WUReceiver wuReceiver = null;
				if (wuReceivers != null)
					wuReceiver = wuReceivers.FirstOrDefault();
				Receiver receiver = AutoMapper.Mapper.Map<WUReceiver, Receiver>(wuReceiver);

				return receiver;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("CustomerID:" + Convert.ToString(customerId));
				details.Add("FullName:" + fullName);
				MongoDBLogger.ListError<string>(details, "GetReceiver", AlloyLayerName.CXE, ModuleName.SendMoney,
				 "Error in GetReceiver - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETRECEIVER_FAILED, ex);
			}
		}

		public List<Receiver> GetFrequentReceivers(long CustomerId)
		{
			List<Receiver> finalReceivers = new List<Receiver>();

			try
			{
				List<WUReceiver> freReceivers = _WUReceiverRepo.FilterBy(c => c.CustomerId == CustomerId && c.Status == "Active").ToList();

				finalReceivers.AddRange(AutoMapper.Mapper.Map<List<WUReceiver>, List<Receiver>>(freReceivers));
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(CustomerId), "GetFrequentReceivers", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetFrequentReceivers - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETFREQUENTRECEIVERS_FAILED, ex);
			}

			return finalReceivers;
		}

		public List<Receiver> GetReceivers(long CustomerId, string lastName)
		{
			List<Receiver> finalReceivers = new List<Receiver>();
			try
			{

				List<WUReceiver> receivers = _WUReceiverRepo.FilterBy(c => c.CustomerId == CustomerId && c.LastName.ToLower().Contains(lastName.ToLower())).ToList();

				finalReceivers.AddRange(AutoMapper.Mapper.Map<List<WUReceiver>, List<Receiver>>(receivers));
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("CustomerID:" + Convert.ToString(CustomerId));
				details.Add("LastName:" + lastName);
				MongoDBLogger.ListError<string>(details, "GetReceivers", AlloyLayerName.CXE, ModuleName.SendMoney,
				 "Error in GetReceivers - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETRECEIVERS_FAILED, ex);
			}

			return finalReceivers;
		}

		/// <summary>
		/// This is method to ADD new records from either UI/View OR from WU Card Look Up Service.
		/// </summary>
		/// <param name="receiver">Receiver Object</param>
		/// <param name="timezone">TimeZone</param>
		/// <returns></returns>
		public long SaveReceiver(Receiver receiver, MGIContext mgiContext)
		{
			try
			{
				WUReceiver wuReceiver = AutoMapper.Mapper.Map<Receiver, WUReceiver>(receiver);

				#region This if condition is added for User Story # US1645.

				// This if condition is added only if the receiver data is from WU Service for User Story # US1645. IMPORTANT METHOD TO SAVE/MODIFY tWUnion_Receiver Table.
				if (!String.IsNullOrEmpty(receiver.ReceiverIndexNo))
				{
					//If record is NOT existing.
					if (!_isReceiverIndexExist(receiver) && blnNoChange == false)
					{
						wuReceiver.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
						wuReceiver.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
						wuReceiver.DTServerLastModified = DateTime.Now;
						wuReceiver.DTServerCreate = DateTime.Now;

						wuReceiver.rowguid = Guid.NewGuid();
						if (wuReceiver.DateOfBirth == DateTime.MinValue)
							wuReceiver.DateOfBirth = null;

						_WUReceiverRepo.AddWithFlush(wuReceiver);
						blnNoChange = false;
						return wuReceiver.Id;
					}

					//If receiver record is existing. Merge the existing receiver through nHibernate.
					else
					{
						if (DBReceiver != null && blnNoChange == false)
						{
							Receiver existingReceiver = GetReceiver(DBReceiver.Id);
							wuReceiver.ReceiverIndexNo = existingReceiver.ReceiverIndexNo;

							wuReceiver.rowguid = existingReceiver.rowguid;
							wuReceiver.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
							wuReceiver.DTServerLastModified = DateTime.Now;

							if (DBReceiver.FirstName == wuReceiver.FirstName)
								wuReceiver.FirstName = (!String.IsNullOrEmpty(DBReceiver.FirstName.ToString())) ? DBReceiver.FirstName.Trim() : String.Empty;

							if (DBReceiver.LastName == wuReceiver.LastName)
								wuReceiver.LastName = (!String.IsNullOrEmpty(DBReceiver.LastName.ToString())) ? DBReceiver.LastName.Trim() : String.Empty;

							if (DBReceiver.Country != null && DBReceiver.Country == wuReceiver.Country)
								wuReceiver.Country = (!String.IsNullOrEmpty(DBReceiver.Country.ToString())) ? DBReceiver.Country.Trim() : String.Empty;

							if (DBReceiver.SecondLastName != null && DBReceiver.SecondLastName == wuReceiver.SecondLastName)
								wuReceiver.SecondLastName = (!String.IsNullOrEmpty(DBReceiver.SecondLastName.ToString())) ? DBReceiver.SecondLastName.Trim() : String.Empty;

							_WUReceiverRepo.Merge(wuReceiver);
							blnNoChange = false;
							return wuReceiver.Id;

						}

						blnNoChange = false;
						return 0;
					}

				}

				#endregion

				// This else condition is based on the same old codebase where receiver information comes from UI/Web Layer.
				else
				{
					Receiver existingReceiver = GetReceiver(receiver.Id);
					if (existingReceiver != null)
					{
						bool receiverExisted = _isReceiverExisting(receiver, existingReceiver.CustomerId ?? 0);
						if (!receiverExisted)
						{
							if (!String.IsNullOrEmpty(existingReceiver.ReceiverIndexNo))
								wuReceiver.ReceiverIndexNo = existingReceiver.ReceiverIndexNo;

							wuReceiver.rowguid = existingReceiver.rowguid;
							wuReceiver.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
							wuReceiver.DTServerLastModified = DateTime.Now;
							_WUReceiverRepo.Merge(wuReceiver);
							return wuReceiver.Id;
						}
						else
							throw new MoneyTransferException(MoneyTransferException.RECEIVER_ALREADY_EXISTED);
					}

					wuReceiver.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
					wuReceiver.DTServerCreate = DateTime.Now;
					wuReceiver.rowguid = Guid.NewGuid();
					//if (wuReceiver.DOB == DateTime.MinValue) // IS this to be handled in diff way?
					//    wuReceiver.DOB = null;

					bool alreadyReceiverExisted = _isReceiverExisting(receiver, receiver.CustomerId ?? 0);
					if (!alreadyReceiverExisted)
					{
						_WUReceiverRepo.AddWithFlush(wuReceiver);
						return wuReceiver.Id;
					}
					else
						throw new MoneyTransferException(MoneyTransferException.RECEIVER_ALREADY_EXISTED);
				}
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<Receiver>(receiver, "SaveReceiver", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in SaveReceiver - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SAVERECEIVER_FAILED, ex);
			}
		}

		public bool Commit(long transactionId, MGIContext mgiContext)
		{

			try
			{
				CheckCounterId(mgiContext);
				WUReceiver receiver = null;

				if (string.IsNullOrEmpty(mgiContext.TimeZone))
					throw new MoneyTransferException(MoneyTransferException.TIME_ZONE_NOT_PROVIDE);

				WUTransaction transaction = Get(transactionId);
				receiver = transaction.WUnionRecipient;
				if (string.IsNullOrEmpty(mgiContext.ReferenceNumber))
				{
					mgiContext.ReferenceNumber = transaction.ReferenceNo;
				}

				if (transaction.TranascationType == ((int)TransferType.receiveMoney).ToString())
				{

					if (mgiContext.RMTrxType == (MTReleaseStatus.Release).ToString())
						mgiContext.RMTrxType = SendMoneyStore.mt_requested_status.RELEASE.ToString();
					else if (mgiContext.RMTrxType == (MTReleaseStatus.Cancel).ToString())
						mgiContext.RMTrxType = SendMoneyStore.mt_requested_status.CANCEL.ToString();

					ReceiveMoneyPay.receivemoneypayrequest receivemoneypayrequest = PopulateReceiveMoneyPayRequest(transaction, mgiContext);

					ReceiveMoneyPay.receivemoneypayreply reply = WUIO.ReceiveMoneyPay(receivemoneypayrequest, mgiContext);

					transaction.MTCN = reply.mtcn;
					transaction.PaidDateTime = reply.paid_date_time;

					transaction.MessageArea = string.Concat(reply.host_message_set1, reply.host_message_set2, reply.host_message_set3);

					_wuTransactionLogRepo.SaveOrUpdate(transaction);
					_wuTransactionLogRepo.Flush();
					return true;
				}
				else if (transaction.TranascationType == ((int)TransferType.sendMoney).ToString())
				{

					bool hasLPMTError;
					sendmoneystorerequest sendMoneyStoreRequest = PopulateSendMoneyStoreRequest(transaction, mgiContext);

					sendmoneystorereply sendMoneyStoreReply = WUIO.SendMoneyStore(sendMoneyStoreRequest, mgiContext, out hasLPMTError);

					if (hasLPMTError)
					{
						return true;
					}

					string totalPointsEarned = string.Empty;

					if (!string.IsNullOrWhiteSpace(transaction.GCNumber))
					{
						totalPointsEarned = GetCardPoints(transaction.GCNumber, mgiContext);
					}

					UpdateTrx(transactionId, sendMoneyStoreReply, totalPointsEarned, mgiContext);

					return false;
				}
				return false;
			}
			catch (Exception ex)
			{
				if (!string.IsNullOrEmpty(mgiContext.SMTrxType) && mgiContext.SMTrxType.ToLower() == (MTReleaseStatus.Cancel).ToString().ToLower())
				{
					if (ex.Message.ToUpper().Trim().Equals("U9767 - MT NOT FOUND OR ALREADY BEEN CANCELLED"))
						return false;
				}
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);

				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Commit", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Commit - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_COMMIT_FAILED , ex);
			}
		}

		/// <summary>
		/// AL-491 While printing receipts updating Gold card points
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="totalPointsEarned"></param>
		/// <param name="context"></param>
		public void UpdateGoldCardPoints(long transactionId, string totalPointsEarned, MGIContext mgiContext)
		{
			try
			{
				WUTransaction transaction = Get(transactionId);

				transaction.WuCardTotalPointsEarned = totalPointsEarned;
				transaction.DTServerLastModified = DateTime.Now;
				transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);

				_wuTransactionLogRepo.UpdateWithFlush(transaction);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "UpdateGoldCardPoints", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in UpdateGoldCardPoints - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATEGOLDCARDPOINTS_FAILED, ex);
			}
		}

		private void UpdateTrx(long transactionId, sendmoneystorereply sendMoneyStoreReply, string totalPointsEarned, MGIContext mgiContext)
		{

			WUTransaction transaction = Get(transactionId);

			transaction.MTCN = sendMoneyStoreReply.mtcn;
			transaction.TempMTCN = sendMoneyStoreReply.new_mtcn;
			if (sendMoneyStoreReply.df_fields != null)
			{
				transaction.AmountToReceiver = Convert.ToDecimal(sendMoneyStoreReply.df_fields.amount_to_receiver);
				transaction.AvailableForPickup = sendMoneyStoreReply.df_fields.available_for_pickup;
				if (!string.IsNullOrWhiteSpace(sendMoneyStoreReply.df_fields.available_for_pickup))
				{
					transaction.DTAvailableForPickup = ParseDate(sendMoneyStoreReply.df_fields.available_for_pickup);
				}
				transaction.DfTransactionFlag = sendMoneyStoreReply.df_fields.df_transaction_flag == SendMoneyStore.yes_no.Y;
				transaction.PdsRequiredFlag = sendMoneyStoreReply.df_fields.pds_required_flag == SendMoneyStore.yes_no.Y;
				transaction.PaySideCharges = Convert.ToDecimal(sendMoneyStoreReply.df_fields.pay_side_charges);
				transaction.PaySideTax = Convert.ToDecimal(sendMoneyStoreReply.df_fields.pay_side_tax);
				transaction.DelayHours = sendMoneyStoreReply.df_fields.delay_hours;
				transaction.DeliveryServiceName = sendMoneyStoreReply.df_fields.delivery_service_name;

				if (sendMoneyStoreReply.df_fields.consumer_bureau_info != null && sendMoneyStoreReply.df_fields.consumer_bureau_info.state != null)
				{
					transaction.AgencyName = sendMoneyStoreReply.df_fields.consumer_bureau_info.state.agencyname;
					transaction.Url = sendMoneyStoreReply.df_fields.consumer_bureau_info.state.url1;
					transaction.PhoneNumber = sendMoneyStoreReply.df_fields.consumer_bureau_info.state.phonenumber1;
				}
			}

			if (sendMoneyStoreReply.pin_text_message_set_2 != null && sendMoneyStoreReply.promo_text_message != null && sendMoneyStoreReply.auto_enroll_text != null)
			{
				string messageArea = string.Empty;

				messageArea = GetStringFromArray(sendMoneyStoreReply.pin_text_message_set_2);
				messageArea += GetStringFromArray(sendMoneyStoreReply.promo_text_message);
				messageArea += GetStringFromArray(sendMoneyStoreReply.auto_enroll_text);

				transaction.MessageArea = messageArea;
			}

			transaction.FilingDate = sendMoneyStoreReply.filing_date;
			transaction.FilingTime = sendMoneyStoreReply.filing_time;
			transaction.WuCardTotalPointsEarned = totalPointsEarned;

			transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			transaction.DTServerLastModified = DateTime.Now;
			_wuTransactionLogRepo.UpdateWithFlush(transaction);
		}

		public long AddAccount(Account account, MGIContext mgiContext)
		{
			try
			{
				WUAccount wuAccount = new WUAccount()
					{
						rowguid = Guid.NewGuid(),
						DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
						DTServerCreate = DateTime.Now,
						Address = account.Address,
						City = account.City,
						ContactPhone = account.ContactPhone,
						Email = account.Email,
						FirstName = account.FirstName,
						LastName = account.LastName,
						MiddleName = account.MiddleName,
						SecondLastName = account.SecondLastName,
						MobilePhone = account.MobilePhone,
						NameType = "D",
						PostalCode = account.PostalCode,
						PreferredCustomerAccountNumber = account.LoyaltyCardNumber,
						PreferredCustomerLevelCode = account.LevelCode,
						State = account.State,
						SmsNotificationFlag = account.SmsNotificationFlag
					};

				WUAccountRepo.AddWithFlush(wuAccount);
				return wuAccount.Id;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<Account>(account, "AddAccount", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in AddAccount - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_ADDACCOUNT_FAILED, ex);
			}
		}

		public long UpdateAccount(Account account, MGIContext mgiContext)
		{
			try
			{
				WUAccount wuAccount = WUAccountRepo.FindBy(x => x.rowguid == account.rowguid);

				wuAccount.rowguid = account.rowguid;
				wuAccount.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				wuAccount.DTServerLastModified = DateTime.Now;
				wuAccount.Address = account.Address;
				wuAccount.City = account.City;
				wuAccount.ContactPhone = account.ContactPhone;
				wuAccount.Email = account.Email;
				wuAccount.FirstName = account.FirstName;
				wuAccount.LastName = account.LastName;
				wuAccount.MiddleName = account.MiddleName;
				wuAccount.SecondLastName = account.SecondLastName;
				wuAccount.MobilePhone = account.MobilePhone;
				wuAccount.PostalCode = account.PostalCode;
				wuAccount.State = account.State;
				wuAccount.SmsNotificationFlag = account.SmsNotificationFlag;
				wuAccount.PreferredCustomerAccountNumber = account.LoyaltyCardNumber;
				WUAccountRepo.Update(wuAccount);
				return wuAccount.Id;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<Account>(account, "UpdateAccount", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in UpdateAccount - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATEACCOUNT_FAILED, ex);
			}
		}

		public Account GetAccount(long cxnAccountId, MGIContext mgiContext)
		{
			try
			{
				return AutoMapper.Mapper.Map<WUAccount, Account>(WUAccountRepo.FindBy(x => x.Id == cxnAccountId));
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(cxnAccountId), "GetAccount", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetAccount - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETACCOUNT_FAILED, ex);
			}
		}

		private Transaction GetReceiveTransaction(TransactionRequest transactionRequest, string confirmationId, MGIContext mgiContext)
		{
			CheckCounterId(mgiContext);
			// Initiate transaction
			long transactionId = CreateReceiveMoneyTransaction(transactionRequest.AccountId, mgiContext);

			WUTransaction transaction = Get(transactionId);

			receivemoneysearchrequest receiveMoneySearchRequest = PopulateReceiveMoneySearchRequest(transactionRequest);

			receiveMoneySearchRequest.swb_fla_info = AutoMapper.Mapper.Map<SwbFlaInfo, ReceiveMoneySearch.swb_fla_info>(WuCommon.BuildSwbFlaInfo(mgiContext));
			receiveMoneySearchRequest.swb_fla_info.fla_name = AutoMapper.Mapper.Map<GeneralName, ReceiveMoneySearch.general_name>(WuCommon.BuildGeneralName(mgiContext));
			if (string.IsNullOrEmpty(mgiContext.ReferenceNumber))
			{
				mgiContext.ReferenceNumber = transaction.ReferenceNo;
			}

			receivemoneysearchreply response = WUIO.SearchReceiveMoney(receiveMoneySearchRequest, mgiContext);

			// Update transaction

			UpdateReceiveMoneyTransaction(transactionId, response, mgiContext);

			Transaction trx = TransactionMapper(response, transaction);


			trx.TransactionID = transactionId.ToString();

			return trx;
		}

		private receivemoneysearchrequest PopulateReceiveMoneySearchRequest(TransactionRequest transactionRequest)
		{
			var receiveMoneySearchRequest = new receivemoneysearchrequest()
			{
				payment_transaction = new ReceiveMoneySearch.payment_transaction()
				{
					mtcn = transactionRequest.ConfirmationNumber
				}
			};

			return receiveMoneySearchRequest;
		}

		/// <summary>
		/// This is the method created to split the messaage string into 69 words and list<string> will be returned by Enumerable.
		/// </summary>
		/// <param name="strMessage">Message from the View/UI</param>
		/// <returns>List<string>This returns the results after splitting the string into 69 words.</string></returns>
		private IEnumerable<string> MessageBlockSplit(string strMessage)
		{
			var length = 69;
			for (int i = 0; i < strMessage.Length; i += length)
			{
				yield return strMessage.Substring(i, Math.Min((strMessage.Length - i), Math.Min(length, strMessage.Length)));
			}
		}

		public List<MoneyTransfer.Data.MasterData> GetBannerMsgs(MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				List<MoneyTransfer.Data.MasterData> BannerMsgs = new List<MoneyTransfer.Data.MasterData>();
				List<AgentBanners> response = new List<AgentBanners>();

				response = WuCommon.GetWUAgentBannerMsgs(mgiContext);

				BannerMsgs = response.Select(i => new MoneyTransfer.Data.MasterData() { Code = i.ERR_CODE.ToString(), Name = i.ERR_MESSAGE }).ToList();
				return BannerMsgs;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<MGIContext>(mgiContext, "GetBannerMsgs", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetBannerMsgs - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETBANNERMSGS_FAILED, ex);
			}
		}

		public MGI.Cxn.MoneyTransfer.Data.CardInfo GetCardInfo(string cardNumber, MGIContext mgiContext)
		{
            try
            {
                MGI.Cxn.WU.Common.Data.CardLookUpRequest request = new MGI.Cxn.WU.Common.Data.CardLookUpRequest();
                //Assigning the Gold Card Number. 
                MGI.Cxn.WU.Common.Data.Sender sender = new MGI.Cxn.WU.Common.Data.Sender();
                sender.PreferredCustomerAccountNumber = cardNumber;
                request.sender = sender;

                MGI.Cxn.WU.Common.Data.CardInfo response = new MGI.Cxn.WU.Common.Data.CardInfo();
                response = WuCommon.GetCardInfo(request, mgiContext);

                MGI.Cxn.MoneyTransfer.Data.CardInfo cardInfo = AutoMapper.Mapper.Map<MGI.Cxn.MoneyTransfer.Data.CardInfo>(response);

                return cardInfo;
            }
            catch(Exception ex)
            {
				MongoDBLogger.Error<string>(cardNumber, "GetCardInfo", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetCardInfo - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCARDINFO_FAILED, ex);
            }
		}

		private void UpdateReceiveMoneyTransaction(long transactionId, receivemoneysearchreply reply, MGIContext mgiContext)
		{

			ReceiveMoneySearch.payment_transaction paymentTransaction = reply.payment_transactions.payment_transaction.FirstOrDefault();

			if (!string.IsNullOrEmpty(paymentTransaction.pay_status_description) &&
				paymentTransaction.pay_status_description.ToLower() == "paid")
			{
				throw new MoneyTransferException(MoneyTransferException.TRANSACTION_ALREADY_PAID);
			}

			var receiverNameType = paymentTransaction.receiver.name.name_type;
			string originatingCity = string.Empty;
			string expectedLocationStateCode = string.Empty;
			string expectedLocationCityCode = string.Empty;
			string securityQuestion = string.Empty;
			string securityAnswer = string.Empty;
			string senderName = string.Format("{0} {1} {2}", paymentTransaction.sender.name.first_name, paymentTransaction.sender.name.middle_name, paymentTransaction.sender.name.last_name);
			var senderNameType = paymentTransaction.sender.name.name_type;

			if (senderNameType == ReceiveMoneySearch.name_type.M)
			{
				senderName = string.Format("{0} {1} {2}", paymentTransaction.sender.name.given_name, paymentTransaction.sender.name.paternal_name, paymentTransaction.sender.name.maternal_name);
			}

			if (reply.delivery_services != null && reply.delivery_services.identification_question != null)
			{
				securityQuestion = reply.delivery_services.identification_question.question;
				securityAnswer = reply.delivery_services.identification_question.answer;
			}

			if (paymentTransaction.payment_details.expected_payout_location != null)
			{
				expectedLocationStateCode = paymentTransaction.payment_details.expected_payout_location.state_code;
				expectedLocationCityCode = paymentTransaction.payment_details.expected_payout_location.city;
			}

			var personalMessageBuilder = new StringBuilder();

			if (reply.delivery_services != null && reply.delivery_services.message != null
				&& reply.delivery_services.message.message_details1 != null)
			{
				foreach (string message in reply.delivery_services.message.message_details1.text)
				{
					personalMessageBuilder.Append(message);
				}
			}

			if (paymentTransaction.payment_details.originating_city != null)
			{
				originatingCity = paymentTransaction.payment_details.originating_city;
			}

			WUTransaction transaction = Get(transactionId);

			transaction.Charges = ConvertLongToDecimal(paymentTransaction.financials.charges);
			transaction.DestinationPrincipalAmount = ConvertLongToDecimal(paymentTransaction.financials.principal_amount);
			transaction.ExchangeRate = Convert.ToDecimal(paymentTransaction.payment_details.exchange_rate);
			transaction.GrossTotalAmount = ConvertLongToDecimal(paymentTransaction.financials.gross_total_amount);
			transaction.AmountToReceiver = ConvertLongToDecimal(paymentTransaction.financials.pay_amount);

			transaction.MoneyTransferKey = paymentTransaction.money_transfer_key;
			transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			transaction.DTServerLastModified = DateTime.Now;
			transaction.TempMTCN = paymentTransaction.new_mtcn;
			transaction.MTCN = paymentTransaction.mtcn;
			transaction.OriginatingCurrencyCode = paymentTransaction.payment_details.originating_country_currency.iso_code.currency_code;
			transaction.DestinationCurrencyCode = paymentTransaction.payment_details.destination_country_currency.iso_code.currency_code;
			transaction.SenderName = senderName;
			transaction.OriginatingCountryCode = paymentTransaction.payment_details.originating_country_currency.iso_code.country_code;
			transaction.DestinationCountryCode = paymentTransaction.payment_details.destination_country_currency.iso_code.country_code;
			transaction.TestQuestion = securityQuestion;
			transaction.TestAnswer = securityAnswer;
			transaction.PersonalMessage = personalMessageBuilder.ToString();
			transaction.ExpectedPayoutStateCode = expectedLocationStateCode;
			transaction.ExpectedPayoutCityName = expectedLocationCityCode;
			transaction.AmountToReceiver = ConvertLongToDecimal(paymentTransaction.financials.pay_amount);
			transaction.GrossTotalAmount = ConvertLongToDecimal(paymentTransaction.financials.gross_total_amount);
			transaction.OriginalDestinationCountryCode = paymentTransaction.payment_details.original_destination_country_currency.iso_code.country_code;
			transaction.OriginalDestinationCurrencyCode = paymentTransaction.payment_details.original_destination_country_currency.iso_code.currency_code;
			transaction.originating_city = originatingCity;
			transaction.RecieverFirstName = string.IsNullOrEmpty(paymentTransaction.receiver.name.given_name) ? paymentTransaction.receiver.name.first_name : paymentTransaction.receiver.name.given_name;
			transaction.RecieverLastName = string.IsNullOrEmpty(paymentTransaction.receiver.name.paternal_name) ? paymentTransaction.receiver.name.last_name : paymentTransaction.receiver.name.paternal_name;
			transaction.RecieverSecondLastName = receiverNameType == ReceiveMoneySearch.name_type.M ? paymentTransaction.receiver.name.maternal_name : string.Empty;

			_wuTransactionLogRepo.Update(transaction);

			return;
		}

		private Transaction TransactionMapper(receivemoneysearchreply reply, WUTransaction transaction)
		{
			ReceiveMoneySearch.payment_transaction paymentTransaction = reply.payment_transactions.payment_transaction.FirstOrDefault();

			decimal netAmount = ConvertLongToDecimal(paymentTransaction.financials.principal_amount);
			string senderStateCode = paymentTransaction.payment_details != null ? paymentTransaction.payment_details.originating_city : string.Empty;

			string receiverName = string.Format("{0} {1} {2}", paymentTransaction.receiver.name.first_name, paymentTransaction.receiver.name.middle_name, paymentTransaction.receiver.name.last_name);
			var nameType = paymentTransaction.receiver.name.name_type;
			if (nameType == ReceiveMoneySearch.name_type.M)
			{
				receiverName = string.Format("{0} {1} {2}",
					paymentTransaction.receiver.name.given_name,
					paymentTransaction.receiver.name.paternal_name,
					paymentTransaction.receiver.name.maternal_name);
			}

			Transaction trx = new Transaction
			{
				ConfirmationNumber = transaction.MTCN,
				TestQuestion = transaction.TestQuestion,
				TestAnswer = transaction.TestAnswer,
				DestinationCurrencyCode = transaction.DestinationCurrencyCode,
				OriginatingCurrencyCode = transaction.OriginatingCurrencyCode,
				GrossTotalAmount = transaction.GrossTotalAmount,
				Fee = transaction.Charges,
				ExchangeRate = transaction.ExchangeRate,
				OriginatingCountryCode = transaction.OriginatingCountryCode,
				DestinationPrincipalAmount = transaction.AmountToReceiver,
				PersonalMessage = transaction.PersonalMessage,
				SenderName = transaction.SenderName,
				Receiver = new Receiver()
				{
					FirstName = transaction.RecieverFirstName,
					LastName = transaction.RecieverLastName,
					SecondLastName = transaction.RecieverSecondLastName
				},
				ReceiverFirstName = string.IsNullOrEmpty(transaction.RecieverFirstName) ? paymentTransaction.receiver.name.given_name : transaction.RecieverFirstName,
				ReceiverLastName = string.IsNullOrEmpty(transaction.RecieverLastName) ? paymentTransaction.receiver.name.paternal_name : transaction.RecieverLastName,
				ReceiverSecondLastName = transaction.RecieverSecondLastName,
				MetaData = new Dictionary<string, object>()
				{
					{"ReceiverName", receiverName},
					{"NetAmount", netAmount},													 
					{"SenderStateCode", senderStateCode},
				}
			};
			return trx;
		}

		#region PRIVATE METHODS

		public WUTransaction Get(long Id)
		{
			try
			{
				return _wuTransactionLogRepo.FindBy(x => x.Id == Id);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(Id), "Get", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Get - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GET_FAILED, ex);
			}
		}

		public Transaction GetTransaction(TransactionRequest request, MGIContext mgiContext)
		{
			try
			{
				long Id = request.TransactionId;

				if (request.TransactionRequestType == TransactionRequestType.ReceiveTransaction)
				{
					return GetReceiveTransaction(request, request.ConfirmationNumber, mgiContext);
				}

				WUTransaction wutransaction = _wuTransactionLogRepo.FindBy(x => x.Id == Id);

				if (wutransaction.TransactionSubType == Convert.ToString((int)SendMoneyTransactionSubType.Refund))
				{
					wutransaction = _wuTransactionLogRepo.All().FirstOrDefault(x => x.MTCN == wutransaction.MTCN && x.TransactionSubType == Convert.ToString((int)SendMoneyTransactionSubType.Cancel));
					wutransaction.Id = Id;
				}
				Transaction transaction = new Transaction()
				{
					Account = AutoMapper.Mapper.Map<WUAccount, Account>(wutransaction.WUnionAccount),
					Receiver = AutoMapper.Mapper.Map<WUReceiver, Receiver>(wutransaction.WUnionRecipient),
					Fee = Convert.ToDecimal(wutransaction.Charges),
					TransactionAmount = wutransaction.TranascationType == ((int)TransferType.sendMoney).ToString() ? Convert.ToDecimal(wutransaction.OriginatorsPrincipalAmount) : Convert.ToDecimal(wutransaction.DestinationPrincipalAmount),
					PromotionsCode = wutransaction.PromotionsCode,
					TransactionID = wutransaction.Id.ToString(),
					TransactionType = wutransaction.TranascationType,
					ConfirmationNumber = wutransaction.MTCN,
					DestinationCountryCode = wutransaction.DestinationCountryCode,
					DestinationCurrencyCode = wutransaction.DestinationCurrencyCode,
					DestinationPrincipalAmount = Convert.ToDecimal(wutransaction.DestinationPrincipalAmount),
					DestinationState = wutransaction.DestinationState,
					ExchangeRate = Convert.ToDecimal(wutransaction.ExchangeRate),
					PromotionDiscount = Convert.ToDecimal(wutransaction.PromotionDiscount),
					TaxAmount = Convert.ToDecimal(wutransaction.TaxAmount),
					OriginatingCountryCode = wutransaction.OriginatingCountryCode,
					OriginatingCurrencyCode = wutransaction.OriginatingCurrencyCode,
					IsDomesticTransfer = wutransaction.IsDomesticTransfer,
					GrossTotalAmount = Convert.ToDecimal(wutransaction.GrossTotalAmount),
					SenderName = wutransaction.SenderName,
					ExpectedPayoutStateCode = wutransaction.ExpectedPayoutStateCode,
					TestQuestion = wutransaction.TestQuestion,
					DeliveryServiceName = wutransaction.DeliveryServiceName,
					DTAvailableForPickup = wutransaction.DTAvailableForPickup,
					ReceiverFirstName = wutransaction.RecieverFirstName,
					ReceiverLastName = wutransaction.RecieverLastName,
					ReceiverSecondLastName = wutransaction.RecieverSecondLastName,
					AmountToReceiver = wutransaction.AmountToReceiver,
					PersonalMessage = wutransaction.PersonalMessage,
					DeliveryServiceDesc = wutransaction.DeliveryServiceDesc,
					ReferenceNo = wutransaction.ReferenceNo,
					OriginalTransactionID = wutransaction.OriginalTransactionID,
					ProviderId = wutransaction.ProviderId,
					ChannelPartnerId = wutransaction.ChannelPartnerId,
					TestAnswer = wutransaction.TestAnswer,
					IsModifiedOrRefunded = _wuTransactionLogRepo.All().Any(x => x.OriginalTransactionID == Id) ? true : false,
					TransactionSubType = wutransaction.TransactionSubType,
					LoyaltyCardNumber = wutransaction.GCNumber,
					LoyaltyCardPoints = wutransaction.WuCardTotalPointsEarned,
					MetaData = new Dictionary<string, object>()

				};


				transaction.MetaData.Add("AdditionalCharges", wutransaction.AdditionalCharges);
				transaction.MetaData.Add("MessageCharge", wutransaction.message_charge);
				transaction.MetaData.Add("PaySideCharges", wutransaction.PaySideCharges);
				transaction.MetaData.Add("OtherCharges", wutransaction.OtherCharges);
				transaction.MetaData.Add("IsFixOnSend", wutransaction.IsFixedOnSend);
				transaction.MetaData.Add("PlusChargesAmount", wutransaction.plus_charges_amount);
				transaction.MetaData.Add("DeliveryOption", wutransaction.DeliveryOption);
				transaction.MetaData.Add("DeliveryOptionDesc", wutransaction.DeliveryOptionDesc);
				transaction.MetaData.Add("FilingDate", wutransaction.FilingDate);
				transaction.MetaData.Add("FilingTime", wutransaction.FilingTime);
				transaction.MetaData.Add("PaidDateTime", wutransaction.PaidDateTime);
				transaction.MetaData.Add("PhoneNumber", wutransaction.PhoneNumber);
				transaction.MetaData.Add("Url", wutransaction.Url);
				transaction.MetaData.Add("AgencyName", wutransaction.AgencyName);
				transaction.MetaData.Add("ExpectedPayoutCity", wutransaction.ExpectedPayoutCityName);
				transaction.MetaData.Add("TransferTax", wutransaction.municipal_tax + wutransaction.state_tax + wutransaction.county_tax);
				transaction.MetaData.Add("PaySideTax", wutransaction.PaySideTax);
				transaction.MetaData.Add("TransalatedDeliveryServiceName", wutransaction.TransalatedDeliveryServiceName);
				transaction.MetaData.Add("MessageArea", wutransaction.MessageArea);
				return transaction;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<TransactionRequest>(request, "GetTransaction", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Get - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETTRANSACTION_FAILED, ex);
			}
		}

		private long CreateReceiveMoneyTransaction(long accountId, MGIContext mgiContext)
		{
			try
			{
				var transaction = new WUTransaction();
				long transactionId;

				if (string.IsNullOrEmpty(mgiContext.TimeZone))
					throw new MoneyTransferException(MoneyTransferException.TIME_ZONE_NOT_PROVIDE);


				transaction.rowguid = Guid.NewGuid();
				transaction.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				transaction.DTServerCreate = DateTime.Now;
				transaction.WUnionAccount = WUAccountRepo.FindBy(x => x.Id == accountId);
				transaction.MTCN = mgiContext.RMMTCN;
				transaction.TranascationType = ((int)TransferType.receiveMoney).ToString();
				transaction.GCNumber = transaction.WUnionAccount.PreferredCustomerAccountNumber;
				transaction.ReferenceNo = DateTime.Now.ToString("yyyyMMddhhmmssff");
				transaction.ChannelPartnerId = mgiContext.ChannelPartnerId;
				transaction.ProviderId = mgiContext.ProviderId;
				_wuTransactionLogRepo.AddWithFlush(transaction);

				transactionId = transaction.Id;

				return transactionId;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(accountId), "CreateReceiveMoneyTransaction", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_CREATERECEIVEMONEYTRANSACTION_FAILED, ex);
			}
		}

		private void UpdateTrx(FeeRequest feeRequest, FeeInquiry.feeinquiryrequest feeInquiryRequest, MGIContext mgiContext)
		{
			try
			{
				WUTransaction transaction = Get(feeRequest.TransactionId);

				transaction.RecieverFirstName = feeInquiryRequest.receiver.name.first_name;
				transaction.RecieverLastName = feeInquiryRequest.receiver.name.last_name;

				transaction.DestinationPrincipalAmount = ConvertLongToDecimal(feeInquiryRequest.financials.destination_principal_amount);
				transaction.OriginatorsPrincipalAmount = ConvertLongToDecimal(feeInquiryRequest.financials.originators_principal_amount);

				transaction.DestinationCountryCode = feeInquiryRequest.payment_details.destination_country_currency.iso_code.country_code;
				transaction.DestinationCurrencyCode = feeInquiryRequest.payment_details.destination_country_currency.iso_code.currency_code;

				transaction.recordingCountryCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.country_code;
				transaction.recordingCurrencyCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.currency_code;
				transaction.IsFixedOnSend = feeInquiryRequest.payment_details.fix_on_send == FeeInquiry.yes_no.Y;
				transaction.DeliveryOption = feeInquiryRequest.delivery_services.code;

				transaction.PersonalMessage = feeRequest.PersonalMessage;
				transaction.PromotionsCode = feeInquiryRequest.promotions.coupons_promotions;
				transaction.GCNumber = feeInquiryRequest.preferred_customer_no;

				transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				transaction.DTServerLastModified = DateTime.Now;
				transaction.GrossTotalAmount = transaction.OriginatorsPrincipalAmount + transaction.Charges + transaction.plus_charges_amount + transaction.message_charge + transaction.OtherCharges;
				transaction.TaxAmount = transaction.county_tax + transaction.municipal_tax + transaction.state_tax;
				_wuTransactionLogRepo.UpdateWithFlush(transaction);
				return;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<FeeRequest>(feeRequest, "UpdateTrx", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATETRX_FAILED, ex);
			}
		}

		private bool _isReceiverExisting(Receiver receiver, long customerId)
		{
			WUReceiver eReceiver = _WUReceiverRepo.FindBy(c => c.Id == receiver.Id);
			if (eReceiver != null)
			{
				if (_WUReceiverRepo.FilterBy(c => c.FirstName.ToLower() == receiver.FirstName.ToLower() && c.LastName.ToLower() == receiver.LastName.ToLower() && c.CustomerId == customerId && c.Id != receiver.Id && c.Status == "Active").Count() > 0)
				{ return true; }
			}
			else
			{
				if (_WUReceiverRepo.FilterBy(c => c.FirstName.ToLower() == receiver.FirstName.ToLower() && c.LastName.ToLower() == receiver.LastName.ToLower() && c.CustomerId == customerId && c.Status == "Active").Count() > 0)
				{ return true; }
			}

			return false;
		}

		private SendMoneyValidation.sendmoneyvalidationrequest MapSendMoneyValidateRequest(ValidateRequest validateRequest, WUTransaction transaction)
		{
			WUAccount account = transaction.WUnionAccount;
			WUReceiver receiver = transaction.WUnionRecipient;

			string expectedPayoutStateCode = Convert.ToString(NexxoUtil.GetDictionaryValue(validateRequest.MetaData, "ExpectedPayoutStateCode"));
			string expectedPayoutCity = Convert.ToString(NexxoUtil.GetDictionaryValue(validateRequest.MetaData, "ExpectedPayoutCity"));

			SendMoneyValidation.sendmoneyvalidationrequest request = new SendMoneyValidation.sendmoneyvalidationrequest();
			SendMoneyValidation.country_currency_info country = new SendMoneyValidation.country_currency_info()
			{
				iso_code = new SendMoneyValidation.iso_code() { country_code = CountryCode, currency_code = CountryCurrencyCode }
			};

			if (account != null)
			{
				SendMoneyValidation.sender sender = new SendMoneyValidation.sender()
				{
					preferred_customer = new SendMoneyValidation.preferred_customer()
					{
						account_nbr = account.PreferredCustomerAccountNumber
					},
					address = new SendMoneyValidation.address()
					{
						addr_line1 = string.IsNullOrEmpty(account.Address) ? string.Empty : NexxoUtil.MassagingValue(account.Address),
						city = string.IsNullOrEmpty(account.City) ? string.Empty : NexxoUtil.MassagingValue(account.City),
						state = string.IsNullOrEmpty(account.State) ? string.Empty : NexxoUtil.MassagingValue(account.State),
						postal_code = string.IsNullOrEmpty(account.PostalCode) ? string.Empty : NexxoUtil.MassagingValue(account.PostalCode),
						Item = country
					},
					contact_phone = string.IsNullOrEmpty(account.ContactPhone) ? string.Empty : NexxoUtil.MassagingValue(account.ContactPhone),
					sms_notification_flag = (account.SmsNotificationFlag == WUEnums.yes_no.Y.ToString()) ? SendMoneyValidation.sms_notification.Y : SendMoneyValidation.sms_notification.N,
					sms_notification_flagSpecified = (account.SmsNotificationFlag == WUEnums.yes_no.Y.ToString()) ? true : false
				};

				SendMoneyValidation.general_name name = null;
				if (!string.IsNullOrEmpty(account.SecondLastName))
				{

					name = new SendMoneyValidation.general_name()
					{
						given_name = string.IsNullOrWhiteSpace(account.FirstName) ? string.Empty : NexxoUtil.MassagingValue(account.FirstName),
						paternal_name = string.IsNullOrWhiteSpace(account.LastName) ? string.Empty : NexxoUtil.MassagingValue(account.LastName),
						maternal_name = string.IsNullOrWhiteSpace(account.SecondLastName) ? string.Empty : NexxoUtil.MassagingValue(account.SecondLastName),
						name_type = SendMoneyValidation.name_type.M,
						name_typeSpecified = true
					};
				}
				else
				{
					name = new SendMoneyValidation.general_name()
					{
						first_name = string.IsNullOrEmpty(account.FirstName) ? string.Empty : NexxoUtil.MassagingValue(account.FirstName),
						last_name = string.IsNullOrEmpty(account.LastName) ? string.Empty : NexxoUtil.MassagingValue(account.LastName),
						middle_name = string.IsNullOrEmpty(account.MiddleName) ? string.Empty : NexxoUtil.MassagingValue(account.MiddleName),
						name_type = SendMoneyValidation.name_type.D,
						name_typeSpecified = true
					};
				}
				sender.name = name;
				request.sender = sender;
			}
			if (receiver != null)
			{
				SendMoneyValidation.receiver requestReciever = new SendMoneyValidation.receiver();
				SendMoneyValidation.general_name recievername = null;
				if (!string.IsNullOrWhiteSpace(validateRequest.ReceiverSecondLastName))
				{
					recievername = new SendMoneyValidation.general_name()
					{
						given_name = string.IsNullOrEmpty(validateRequest.ReceiverFirstName) ? string.Empty : NexxoUtil.MassagingValue(validateRequest.ReceiverFirstName),
						maternal_name = string.IsNullOrEmpty(validateRequest.ReceiverSecondLastName) ? string.Empty : NexxoUtil.MassagingValue(validateRequest.ReceiverSecondLastName),
						paternal_name = string.IsNullOrEmpty(validateRequest.ReceiverLastName) ? string.Empty : NexxoUtil.MassagingValue(validateRequest.ReceiverLastName),
						name_type = SendMoneyValidation.name_type.M
					};
				}
				else
				{
					recievername = new SendMoneyValidation.general_name()
					{
						last_name = string.IsNullOrEmpty(validateRequest.ReceiverLastName) ? string.Empty : NexxoUtil.MassagingValue(validateRequest.ReceiverLastName),
						first_name = string.IsNullOrEmpty(validateRequest.ReceiverFirstName) ? string.Empty : NexxoUtil.MassagingValue(validateRequest.ReceiverFirstName),
						middle_name = string.IsNullOrEmpty(validateRequest.ReceiverMiddleName) ? string.Empty : NexxoUtil.MassagingValue(validateRequest.ReceiverMiddleName),
						name_type = SendMoneyValidation.name_type.D
					};
				}
				recievername.name_typeSpecified = true;
				requestReciever.name = recievername;
				request.receiver = requestReciever;

			}

			if (!string.IsNullOrWhiteSpace(transaction.PromotionsCode))
			{
				request.promotions = new SendMoneyValidation.promotions()
				{
					coupons_promotions = transaction.PromotionsCode,
				};
			}

			SendMoneyValidation.payment_details paymentdetails = new SendMoneyValidation.payment_details()
			{
				expected_payout_location = new SendMoneyValidation.expected_payout_location()
				{
					state_code = string.IsNullOrEmpty(expectedPayoutStateCode) ? string.Empty : expectedPayoutStateCode,
					city = string.IsNullOrEmpty(expectedPayoutCity) ? string.Empty : expectedPayoutCity
				},
				recording_country_currency = country,
				originating_country_currency = country,
				destination_country_currency = new SendMoneyValidation.country_currency_info()
				{
					iso_code = new SendMoneyValidation.iso_code()
					{
						country_code = transaction.DestinationCountryCode,
						currency_code = transaction.DestinationCurrencyCode
					}
				},
				transaction_type = SendMoneyValidation.transaction_type.WMN,
				transaction_typeSpecified = true,
				payment_type = SendMoneyValidation.payment_type.Cash,
				payment_typeSpecified = true,
				duplicate_detection_flag = AllowDuplicateTrxWU,
			};
			request.payment_details = paymentdetails;

			if (!string.IsNullOrWhiteSpace(validateRequest.DeliveryService))
			{
				request.delivery_services = new SendMoneyValidation.delivery_services()
				{
					code = validateRequest.DeliveryService
				};

				if (!string.IsNullOrWhiteSpace(validateRequest.IdentificationQuestion))
				{
					request.delivery_services.identification_question = new SendMoneyValidation.identification_question()
					{
						question = validateRequest.IdentificationQuestion,
						answer = validateRequest.IdentificationAnswer,
					};
				}
			}
			if (!string.IsNullOrWhiteSpace(validateRequest.PersonalMessage))
			{
				string[] personalMessages = MessageBlockSplit(validateRequest.PersonalMessage).ToArray();

				int msgcnt = personalMessages.Length;
				SendMoneyValidation.message_details msgs = new SendMoneyValidation.message_details()
				{
					message_details1 = new SendMoneyValidation.messages()
					{
						text = personalMessages,
						context = msgcnt.ToString()
					}
				};
				request.delivery_services.message = msgs;
			}

			request.financials = new SendMoneyValidation.financials()
			{
				originators_principal_amount = ConvertDecimalToLong(transaction.OriginatorsPrincipalAmount),
				originators_principal_amountSpecified = transaction.DestinationPrincipalAmount > 0,
			};

			//When a Passport is used as the form of ID in the customer profile
			//In order for WU transaction to work correctly with Passports the id_country_of_issue field should use country name instead of country code 	

			string issueCountry = validateRequest.PrimaryCountryCodeOfIssue;
			if (validateRequest.PrimaryIdType != null)
			{
				if (validateRequest.PrimaryIdType.Equals("PASSPORT")
					|| validateRequest.PrimaryIdType.Equals("EMPLOYMENT AUTHORIZATION CARD (EAD)")
					|| validateRequest.PrimaryIdType.Equals("GREEN CARD / PERMANENT RESIDENT CARD")
					|| validateRequest.PrimaryIdType.Equals("MILITARY ID"))
				{
					issueCountry = GetCountryName(validateRequest.PrimaryCountryCodeOfIssue);
				}
			}
			request.sender.compliance_details = new SendMoneyValidation.compliance_details()
			{
				template_id = ComplianceTemplate.SEND_MONEY,
				id_details = new SendMoneyValidation.id_details()
				{
					id_type = !string.IsNullOrEmpty(validateRequest.PrimaryIdType) ? WuCommon.GetGovtIDType(validateRequest.PrimaryIdType) : string.Empty,
					id_number = validateRequest.PrimaryIdNumber,
					id_country_of_issue = issueCountry,
					id_place_of_issue = !string.IsNullOrEmpty(validateRequest.PrimaryIdPlaceOfIssue) ? GetStateCode(validateRequest.PrimaryIdPlaceOfIssue) : string.Empty
				},
				Current_address = new SendMoneyValidation.compliance_address()
				{
					addr_line1 = NexxoUtil.MassagingValue(account.Address),
					city = NexxoUtil.MassagingValue(account.City),
					state_code = NexxoUtil.MassagingValue(account.State),
					postal_code = NexxoUtil.MassagingValue(account.PostalCode),
					country = CountryCode
				},
				date_of_birth = validateRequest.DateOfBirth,
				occupation = WuCommon.TrimOccupation(NexxoUtil.MassagingValue(validateRequest.Occupation)),
				Country_of_Birth = validateRequest.CountryOfBirthAbbr2,
				ack_flag = "X",
				third_party_details = new SendMoneyValidation.third_party_details() { flag_pay = "N" }
			};

			if (!string.IsNullOrEmpty(validateRequest.SecondIdNumber))
			{
				request.sender.compliance_details.second_id = new SendMoneyValidation.id_details()
				{
					id_type = WuCommon.GetGovtIDType(validateRequest.SecondIdType),
					id_number = validateRequest.SecondIdNumber,
					id_country_of_issue = "United States"//sendMoneyValidateRequest.SecondIdCountryOfIssue
				};
			}

			if (account.SmsNotificationFlag == "Y")
			{
				request.sender.mobile_phone = new SendMoneyValidation.mobile_phone()
				{
					phone_number = new SendMoneyValidation.international_phone_number()
					{
						country_code = "1",
						national_number = account.MobilePhone
					}
				};
			}

			if (request.sender.compliance_details.id_details != null && request.sender.compliance_details.id_details.id_country_of_issue != null)
			{
				if (validateRequest.PrimaryIdType != null)
					if (request.sender.compliance_details.id_details.id_country_of_issue.Equals("US")
						&& (validateRequest.PrimaryIdType.Equals("DRIVER'S LICENSE") || validateRequest.PrimaryIdType.Equals("U.S. STATE IDENTITY CARD")
						|| validateRequest.PrimaryIdType.Equals("NEW YORK CITY ID") || validateRequest.PrimaryIdType.Equals("NEW YORK BENEFITS ID")))
					{
						request.sender.compliance_details.id_details.id_country_of_issue = string.Format("{0}/{1}", GetCountryCode(validateRequest.PrimaryIdCountryOfIssue), GetStateCode(validateRequest.PrimaryIdPlaceOfIssue));
					}
					else if (request.sender.compliance_details.id_details.id_country_of_issue.Equals("MX"))
					{
						request.sender.compliance_details.id_details.id_country_of_issue = "Mexico";
					}
			}

			return request;
		}

		private sendmoneystorerequest PopulateSendMoneyStoreRequest(WUTransaction transaction, MGIContext mgiContext)
		{
			WUAccount account = transaction.WUnionAccount;
			WUReceiver receiver = transaction.WUnionRecipient;

			receiver.FirstName = NexxoUtil.MassagingValue(receiver.FirstName);
			receiver.LastName = NexxoUtil.MassagingValue(receiver.LastName);
			receiver.SecondLastName = NexxoUtil.MassagingValue(transaction.RecieverSecondLastName);
			receiver.City = NexxoUtil.MassagingValue(receiver.City);
			receiver.PickupCountry = NexxoUtil.MassagingValue(receiver.PickupCountry);
			account.Address = NexxoUtil.MassagingValue(account.Address);
			account.City = NexxoUtil.MassagingValue(account.City);
			account.State = NexxoUtil.MassagingValue(account.State);
			account.FirstName = NexxoUtil.MassagingValue(account.FirstName);
			account.LastName = NexxoUtil.MassagingValue(account.LastName);
			account.MiddleName = NexxoUtil.MassagingValue(account.MiddleName);
			account.SecondLastName = NexxoUtil.MassagingValue(account.SecondLastName);

			var requestedStatus = SendMoneyStore.mt_requested_status.HOLD;

			if (mgiContext.SMTrxType == (MTReleaseStatus.Release).ToString())
				requestedStatus = SendMoneyStore.mt_requested_status.RELEASE;
			else if (mgiContext.SMTrxType == (MTReleaseStatus.Cancel).ToString())
				requestedStatus = SendMoneyStore.mt_requested_status.CANCEL;
			else if (mgiContext.SMTrxType == (MTReleaseStatus.Hold).ToString())
				requestedStatus = SendMoneyStore.mt_requested_status.HOLD;

			SendMoneyStore.general_name receiverName = null;
			SendMoneyStore.general_name senderName = null;

			var countryinfo = new SendMoneyStore.country_currency_info()
			{
				iso_code = new SendMoneyStore.iso_code()
				{
					country_code = CountryCode,
					currency_code = CountryCurrencyCode
				}
			};

			if (!string.IsNullOrWhiteSpace(receiver.SecondLastName))
			{
				receiverName = new SendMoneyStore.general_name()
				{
					given_name = receiver.FirstName,
					paternal_name = receiver.LastName,
					maternal_name = receiver.SecondLastName,
					name_type = SendMoneyStore.name_type.M,
					name_typeSpecified = true
				};
			}
			else
			{
				receiverName = new SendMoneyStore.general_name()
				{
					first_name = receiver.FirstName,
					last_name = receiver.LastName,
					name_type = SendMoneyStore.name_type.D,
					name_typeSpecified = true
				};
			}
			if (!string.IsNullOrWhiteSpace(account.SecondLastName))
			{
				senderName = new SendMoneyStore.general_name()
				{
					given_name = account.FirstName,
					paternal_name = account.LastName,
					maternal_name = account.SecondLastName,
					name_type = SendMoneyStore.name_type.M,
					name_typeSpecified = true
				};
			}
			else
			{
				senderName = new SendMoneyStore.general_name()
				{
					first_name = account.FirstName,
					last_name = account.LastName,
					middle_name = account.MiddleName,
					name_type = SendMoneyStore.name_type.D,
					name_typeSpecified = true
				};
			}


			var sendMoneyStoreRequest = new sendmoneystorerequest()
			{
				sender = new SendMoneyStore.sender
				{
					name = senderName,
					preferred_customer = new SendMoneyStore.preferred_customer()
					{
						account_nbr = account.PreferredCustomerAccountNumber,
					},
					address = new SendMoneyStore.address()
						{
							addr_line1 = account.Address,
							city = account.City,
							state = account.State,
							postal_code = account.PostalCode,
							Item = countryinfo
						},
					contact_phone = account.ContactPhone,
					compliance_details = new SendMoneyStore.compliance_details()
						{
							compliance_data_buffer = transaction.SenderComplianceDetailsComplianceDataBuffer
						},
					sms_notification_flag = (account.SmsNotificationFlag == WUEnums.yes_no.Y.ToString()) ? SendMoneyStore.sms_notification.Y : SendMoneyStore.sms_notification.N,
					sms_notification_flagSpecified = (account.SmsNotificationFlag == WUEnums.yes_no.Y.ToString()) ? true : false
				},
				receiver = new SendMoneyStore.receiver()
					{
						name = receiverName,
						contact_phone = receiver.PhoneNumber
					},
				promotions = new SendMoneyStore.promotions()
					{
						promo_sequence_no = transaction.PromotionSequenceNo,
						coupons_promotions = transaction.PromotionsCode ?? string.Empty,
						promo_code_description = transaction.PromoCodeDescription ?? string.Empty,
						promo_name = transaction.PromoName ?? string.Empty,
						promo_discount_amount = ConvertDecimalToLong(transaction.PromotionDiscount),
						sender_promo_code = transaction.PromotionsCode ?? string.Empty,
						promo_message = transaction.PromoMessage ?? string.Empty,
						promo_discount_amountSpecified = transaction.PromotionDiscount > 0
					},
				payment_details = new SendMoneyStore.payment_details()
					{
						expected_payout_location = new SendMoneyStore.expected_payout_location()
						{
							city = transaction.ExpectedPayoutCityName,
							state_code = transaction.ExpectedPayoutStateCode
						},
						originating_country_currency = countryinfo,
						recording_country_currency = countryinfo,
						destination_country_currency = new SendMoneyStore.country_currency_info()
						{
							iso_code = new SendMoneyStore.iso_code()
							{
								country_code = transaction.DestinationCountryCode,
								currency_code = transaction.DestinationCurrencyCode
							}
						},
						transaction_type = SendMoneyStore.transaction_type.WMN,
						transaction_typeSpecified = true,
						payment_type = SendMoneyStore.payment_type.Cash,
						payment_typeSpecified = true,
						duplicate_detection_flag = AllowDuplicateTrxWU,
						mt_requested_status = requestedStatus,
						mt_requested_statusSpecified = true
					},
				delivery_services = new SendMoneyStore.delivery_services()
					{
						code = string.IsNullOrEmpty(transaction.DeliveryOption) ? transaction.DeliveryServiceName : transaction.DeliveryOption,
						identification_question = new SendMoneyStore.identification_question
						{
							question = transaction.TestQuestion,
							answer = transaction.TestAnswer
						}
					},
				financials = new SendMoneyStore.financials()
					{
						taxes = new SendMoneyStore.taxes()
						{
							municipal_tax = ConvertDecimalToLong(transaction.municipal_tax),
							municipal_taxSpecified = true,
							state_tax = ConvertDecimalToLong(transaction.state_tax),
							state_taxSpecified = true,
							county_tax = ConvertDecimalToLong(transaction.county_tax),
							county_taxSpecified = true,
						},
						gross_total_amount = ConvertDecimalToLong(transaction.GrossTotalAmount),
						gross_total_amountSpecified = transaction.GrossTotalAmount > 0,
						plus_charges_amount = ConvertDecimalToLong(transaction.plus_charges_amount),
						plus_charges_amountSpecified = transaction.plus_charges_amount > 0,
						charges = ConvertDecimalToLong(transaction.Charges),
						chargesSpecified = transaction.Charges > 0
					},
				mtcn = transaction.MTCN,
				new_mtcn = transaction.TempMTCN,
				df_fields = new SendMoneyStore.df_fields()
				{
					amount_to_receiver = Convert.ToDouble(transaction.AmountToReceiver),
					amount_to_receiverSpecified = transaction.AmountToReceiver > 0,
					pay_side_charges = Convert.ToDouble(transaction.PaySideCharges),
					pay_side_chargesSpecified = transaction.PaySideCharges > 0,
					pay_side_tax = Convert.ToDouble(transaction.PaySideTax),
					pay_side_taxSpecified = transaction.PaySideTax > 0,
					delivery_service_name = transaction.DeliveryServiceDesc ?? string.Empty,
					pds_required_flag = transaction.PdsRequiredFlag ? SendMoneyStore.yes_no.Y : SendMoneyStore.yes_no.N,
					pds_required_flagSpecified = true,
					df_transaction_flag = transaction.DfTransactionFlag ? SendMoneyStore.yes_no.Y : SendMoneyStore.yes_no.N,
					df_transaction_flagSpecified = true
				}
			};

			if (transaction.message_charge > 0)
			{
				sendMoneyStoreRequest.financials.message_charge = ConvertDecimalToLong(transaction.message_charge);
				sendMoneyStoreRequest.financials.message_chargeSpecified = true;
			}

			if (transaction.OriginatorsPrincipalAmount > 0)
			{
				sendMoneyStoreRequest.financials.originators_principal_amount = ConvertDecimalToLong(transaction.OriginatorsPrincipalAmount);
				sendMoneyStoreRequest.financials.originators_principal_amountSpecified = true;
			}

			if (transaction.DestinationPrincipalAmount > 0)
			{
				sendMoneyStoreRequest.financials.destination_principal_amount = ConvertDecimalToLong(transaction.DestinationPrincipalAmount);
				sendMoneyStoreRequest.financials.destination_principal_amountSpecified = true;
			}

			if (transaction.total_discount > 0)
			{
				sendMoneyStoreRequest.financials.total_discount = ConvertDecimalToLong(transaction.total_discount);
				sendMoneyStoreRequest.financials.total_discountSpecified = true;
			}

			if (transaction.total_discounted_charges > 0)
			{
				sendMoneyStoreRequest.financials.total_discounted_charges = ConvertDecimalToLong(transaction.total_discounted_charges);
				sendMoneyStoreRequest.financials.total_discounted_chargesSpecified = true;
			}

			if (transaction.total_undiscounted_charges > 0)
			{
				sendMoneyStoreRequest.financials.total_undiscounted_charges = ConvertDecimalToLong(transaction.total_undiscounted_charges);
				sendMoneyStoreRequest.financials.total_undiscounted_chargesSpecified = true;
			}


			if (account.SmsNotificationFlag == "Y")
			{
				sendMoneyStoreRequest.sender.mobile_phone = new SendMoneyStore.mobile_phone()
				{
					phone_number = new SendMoneyStore.international_phone_number()
					{
						country_code = "1",
						national_number = account.MobilePhone
					}
				};
			}

			if (!string.IsNullOrWhiteSpace(transaction.TestQuestion))
			{
				sendMoneyStoreRequest.delivery_services.identification_question = new SendMoneyStore.identification_question()
				{
					question = transaction.TestQuestion,
					answer = transaction.TestAnswer
				};
			}

			if (!string.IsNullOrWhiteSpace(transaction.PersonalMessage))
			{
				string[] personalMessages = MessageBlockSplit(transaction.PersonalMessage).ToArray();

				int msgcnt = personalMessages.Length;
				var msgs = new SendMoneyStore.message_details()
				{
					message_details1 = new SendMoneyStore.messages()
					{
						text = personalMessages,
						context = msgcnt.ToString()
					}
				};
				sendMoneyStoreRequest.delivery_services.message = msgs;
			}

			if (!string.IsNullOrWhiteSpace(transaction.instant_notification_addl_service_charges))
			{
				sendMoneyStoreRequest.instant_notification = new SendMoneyStore.instant_notification()
				{
					addl_service_charges = transaction.instant_notification_addl_service_charges
				};
			}
			sendMoneyStoreRequest.swb_fla_info = AutoMapper.Mapper.Map<SwbFlaInfo, SendMoneyStore.swb_fla_info>(WuCommon.BuildSwbFlaInfo(mgiContext));
			sendMoneyStoreRequest.swb_fla_info.fla_name = AutoMapper.Mapper.Map<GeneralName, SendMoneyStore.general_name>(WuCommon.BuildGeneralName(mgiContext));

			return sendMoneyStoreRequest;
		}


		/// <summary>
		/// This is the final method in the CXN Gateway to add to get "Past Receivers" for User Story # US1645.
		/// </summary>
		/// <param name="accountID">Account ID</param>
		/// <param name="cardNumber">Gold Card Number</param>
		/// <param name="context">Context</param>
		/// <returns>True if returns card look up is populated from WU Service </returns>
		public bool GetPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				string goldCardNumber = String.Empty;
				List<MGI.Cxn.WU.Common.Data.Receiver> cardReceiver = new List<MGI.Cxn.WU.Common.Data.Receiver>();

				if (!string.IsNullOrEmpty(cardNumber.ToString()))
					goldCardNumber = cardNumber.ToString();
				else
					goldCardNumber = String.Empty;

				MGI.Cxn.WU.Common.Data.CardLookUpRequest cxncardlookupreq = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
				{
					sender = new MGI.Cxn.WU.Common.Data.Sender()
					{
						PreferredCustomerAccountNumber = goldCardNumber,
					}
				};

				// cardlookupdetails collection will contain combination of Past Biller and Receivers Collection. User Story # US1645 and # US1646.
				cardReceiver = WuCommon.WUPastBillersReceivers(customerSessionId, cxncardlookupreq, mgiContext);
				if (cardReceiver != null)
				{
					foreach (MGI.Cxn.WU.Common.Data.Receiver receiverFromWU in cardReceiver)
					{

						if ((receiverFromWU.NameType.ToString().ToUpper() == "D" || receiverFromWU.NameType.ToString().ToUpper() == "M") && receiverFromWU.FirstName != string.Empty && receiverFromWU.LastName != string.Empty && receiverFromWU.SecondLastName != string.Empty && receiverFromWU.Address != null && receiverFromWU.Address.item.country_code != string.Empty)
						{

							Receiver receiver = new Receiver();
							if (!string.IsNullOrWhiteSpace(receiverFromWU.FirstName) && !string.IsNullOrWhiteSpace(receiverFromWU.LastName))
							{
								receiver.FirstName = receiverFromWU.FirstName;
								receiver.LastName = receiverFromWU.LastName;
								if (!string.IsNullOrWhiteSpace(receiverFromWU.SecondLastName))
								{
									receiver.SecondLastName = receiverFromWU.SecondLastName;
								}
								receiver.ReceiverIndexNo = receiverFromWU.ReceiverIndexNumber;
								receiver.Country = receiverFromWU.Address.item.country_code;
								receiver.PickupCountry = receiverFromWU.Address.item.country_code;
								receiver.Status = "Active";
								receiver.CustomerId = customerSessionId;
								receiver.GoldCardNumber = goldCardNumber;
								SaveReceiver(receiver, mgiContext);
							}
						}
					}
				}

				return cardReceiver != null;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Customer Session Id:" + Convert.ToString(customerSessionId));
				details.Add("Card Number:" + cardNumber);
				MongoDBLogger.ListError<string>(details, "GetPastReceivers", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetPastReceivers - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETPASTRECEIVERS_FAILED, ex);
			}
		}

		/// <summary>
		/// This is an new method that will check whether the record from WU should be ADDED / MODIFIED / NO Change in the current DMS DB's for User Story # US1645.
		/// </summary>
		/// <param name="receiver">Receiver Object</param>
		/// <returns>True : If record is there / False : If No Records</returns>
		private bool _isReceiverIndexExist(Receiver receiver)
		{
			WUReceiver pastReceiver;
			if (receiver.SecondLastName != null)
			{
				pastReceiver = _WUReceiverRepo.FindBy(y => y.CustomerId == receiver.CustomerId && y.FirstName == receiver.FirstName.ToUpper().Trim() && y.LastName == receiver.LastName.ToUpper().Trim() && y.SecondLastName == receiver.SecondLastName.ToUpper().Trim());
			}
			else
			{
				pastReceiver = _WUReceiverRepo.FindBy(y => y.CustomerId == receiver.CustomerId && y.PickupCountry.ToUpper().Trim() == receiver.PickupCountry.ToUpper().Trim() && y.FirstName == receiver.FirstName.ToUpper().Trim() && y.LastName == receiver.LastName.ToUpper().Trim());
			}
			if (pastReceiver != null)
			{
				//If FirstName, LastName or Country from WU Changed then same change to be implemented in DMS.
				if (receiver.FirstName != null && receiver.LastName != null)
				{
					if (_WUReceiverRepo.FilterBy(c => (c.FirstName.ToUpper().Trim() != receiver.FirstName.ToUpper().Trim() || c.LastName.ToUpper().Trim() != receiver.LastName.ToUpper().Trim() || c.Country.ToUpper().Trim() != receiver.Country.ToUpper().Trim()) && c.CustomerId == receiver.CustomerId).Count() > 0)
					{
						DBReceiver.FirstName = (!String.IsNullOrEmpty(receiver.FirstName.ToString())) ? receiver.FirstName : string.Empty;
						DBReceiver.LastName = (!String.IsNullOrEmpty(receiver.LastName.ToString())) ? receiver.LastName : string.Empty;
						if (receiver.SecondLastName != null)
							DBReceiver.SecondLastName = (!String.IsNullOrEmpty(receiver.SecondLastName.ToString())) ? receiver.SecondLastName : string.Empty;
						if (receiver.Country != null)
							DBReceiver.Country = (!String.IsNullOrEmpty(receiver.Country.ToString())) ? receiver.Country : string.Empty;
						DBReceiver.Id = pastReceiver.Id;
						return true;
					}
					else
					{
						blnNoChange = true;
						return false;
					}
				}
				else
				{
					blnNoChange = true;
					return false;
				}
			}
			else
				return false;
		}

		#endregion

		public bool UseGoldcard(long accountId, string WUGoldCardNumber, MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				MGI.Cxn.WU.Common.Data.CardLookupDetails cardlookupdetails = new MGI.Cxn.WU.Common.Data.CardLookupDetails();
				MGI.Cxn.WU.Common.Data.CardLookUpRequest cxncardlookupreq = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
				{
					sender = new MGI.Cxn.WU.Common.Data.Sender()
					{
						PreferredCustomerAccountNumber = WUGoldCardNumber,
					}
				};
				cardlookupdetails = WuCommon.WUCardLookupForCardNumber(cxncardlookupreq, mgiContext);
				MGI.Cxn.MoneyTransfer.Data.CardLookupDetails cxncardlookupDetails = new MGI.Cxn.MoneyTransfer.Data.CardLookupDetails();

				cxncardlookupDetails.Sender = new Account[1];

				cxncardlookupDetails.Sender[0] = new MGI.Cxn.MoneyTransfer.Data.Account()
				{
					Address = cardlookupdetails.Sender[0].AddressAddrLine1,
					FirstName = cardlookupdetails.Sender[0].FirstName,
					LastName = cardlookupdetails.Sender[0].LastName,
					LoyaltyCardNumber = cardlookupdetails.Sender[0].PreferredCustomerAccountNumber,
					MobilePhone = cardlookupdetails.Sender[0].MobilePhone,
					PostalCode = cardlookupdetails.Sender[0].AddressPostalCode
				};

				WUAccount wuAccount = WUAccountRepo.FindBy(x => x.Id == accountId);

				if (!(cxncardlookupDetails.Sender[0].FirstName.Trim().ToLower().Equals(wuAccount.FirstName.Trim().ToLower())
					&& cxncardlookupDetails.Sender[0].LastName.Trim().ToLower().Equals(wuAccount.LastName.Trim().ToLower())))
					throw new WUCommonException(MGI.Common.Util.NexxoUtil.GetProductCode(mgiContext.ProductType), MoneyTransferException.CUSTOMER_NAME_NOT_MATCH, null);

				wuAccount.PreferredCustomerAccountNumber = WUGoldCardNumber;
				return WUAccountRepo.UpdateWithFlush(wuAccount);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Customer Session Id:" + Convert.ToString(accountId));
				details.Add("My WU Number:" + WUGoldCardNumber);
				MongoDBLogger.ListError<string>(details, "UseGoldcard", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in UseGoldcard - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_USEGOLDCARD_FAILED, ex);
			}
		}

		public MGI.Cxn.MoneyTransfer.Data.CardDetails WUCardEnrollment(Account account, PaymentDetails paymentDetails, MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				MGI.Cxn.WU.Common.Data.CardDetails carddetails = new MGI.Cxn.WU.Common.Data.CardDetails();
				paymentDetails.RecordingcountrycurrencyCountryCode = CountryCode; //Currently hardcoding as the app is used from US
				paymentDetails.RecordingcountrycurrencyCurrencyCode = CountryCurrencyCode;//Currently hardcoding as the app is used from US
				MGI.Cxn.WU.Common.Data.PaymentDetails cxnPaymentDetails = new MGI.Cxn.WU.Common.Data.PaymentDetails();
				MGI.Cxn.WU.Common.Data.PaymentDetails cxndetails = Mapper(paymentDetails, cxnPaymentDetails);
				MGI.Cxn.WU.Common.Data.Sender cxnSenderDetails = new MGI.Cxn.WU.Common.Data.Sender();
				MGI.Cxn.WU.Common.Data.Sender cxnSender = Mapper(account, cxnSenderDetails);
				carddetails = WuCommon.WUCardEnrollment(cxnSender, cxndetails, mgiContext);
				MGI.Cxn.MoneyTransfer.Data.CardDetails cxncarddDetails = AutoMapper.Mapper.Map<MGI.Cxn.WU.Common.Data.CardDetails, MGI.Cxn.MoneyTransfer.Data.CardDetails>(carddetails);

				return cxncarddDetails;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<PaymentDetails>(paymentDetails, "WUCardEnrollment", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in WUCardEnrollment - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_WUCARDENROLLMENT_FAILED, ex);
			}
		}

		public MGI.Cxn.MoneyTransfer.Data.CardLookupDetails WUCardLookup(long customerAccountId, MGI.Cxn.MoneyTransfer.Data.CardLookupDetails LookupDetails, MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				MGI.Cxn.MoneyTransfer.Data.Account senderinfo = GetAccount(customerAccountId, mgiContext);
				MGI.Cxn.WU.Common.Data.CardLookupDetails cardlookupdetails = new MGI.Cxn.WU.Common.Data.CardLookupDetails();
				MGI.Cxn.WU.Common.Data.CardLookUpRequest cxncardlookupreq = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
				{
					ForiegnSystemId = LookupDetails.ForiegnSystemId,
					ForiegnRefNum = LookupDetails.ForiegnRefNum,
					CounterId = LookupDetails.CounterId,
					AccountNumber = LookupDetails.AccountNumber,
					firstname = LookupDetails.firstname,
					lastname = LookupDetails.lastname,
					midname = LookupDetails.midname,
					postalcode = LookupDetails.countrycode,
					currencycode = LookupDetails.currencycode,
					levelcode = LookupDetails.levelcode,
					sender = new MGI.Cxn.WU.Common.Data.Sender()
					{
						AddressAddrLine1 = LookupDetails.AddressAddrLine1,
						AddressCity = LookupDetails.AddressCity,
						AddressPostalCode = LookupDetails.AddressPostalCode,
						AddressState = LookupDetails.AddressState,
						ContactPhone = LookupDetails.ContactPhone,
						PreferredCustomerAccountNumber = LookupDetails.AccountNumber //senderinfo.PreferredCustomerAccountNumber,
					}
				};
				cardlookupdetails = WuCommon.WUCardLookup(cxncardlookupreq, mgiContext);
				MGI.Cxn.MoneyTransfer.Data.CardLookupDetails cxncardlookupDetails = new MGI.Cxn.MoneyTransfer.Data.CardLookupDetails();

				int cnt = 0;
				cxncardlookupDetails.Sender = new Account[cardlookupdetails.Sender.Count()];
				foreach (MGI.Cxn.WU.Common.Data.Sender sender in cardlookupdetails.Sender)
				{
					cxncardlookupDetails.Sender[cnt] = new MGI.Cxn.MoneyTransfer.Data.Account()
					{
						Address = sender.AddressAddrLine1,
						FirstName = sender.FirstName,
						LastName = sender.LastName,
						LoyaltyCardNumber = sender.PreferredCustomerAccountNumber,
						MobilePhone = sender.MobilePhone,
						PostalCode = sender.AddressPostalCode
					};
					cnt++;
				}

				return cxncardlookupDetails;
			}
			catch (Exception ex)
			{
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);

				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<MGI.Cxn.MoneyTransfer.Data.CardLookupDetails>(LookupDetails, "WUCardLookup", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in WUCardLookup - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_WUCARDLOOKUP_FAILED, ex);
			}
		}

		private MGI.Cxn.WU.Common.Data.PaymentDetails Mapper(PaymentDetails paymentDetails, MGI.Cxn.WU.Common.Data.PaymentDetails cxnPaymentDetails)
		{
			//return new MGI.Cxn.WU.Common.Data.PaymentDetails()
			cxnPaymentDetails = new MGI.Cxn.WU.Common.Data.PaymentDetails()
			{
				destination_country_currency = new MGI.Cxn.WU.Common.Data.CountryCurrencyInfo() { country_code = paymentDetails.DestinationCountryCode, currency_code = paymentDetails.DestinationCurrencyCode },
				originating_country_currency = new MGI.Cxn.WU.Common.Data.CountryCurrencyInfo() { country_code = paymentDetails.OriginatingCountryCode, currency_code = paymentDetails.OriginatingCurrencyCode },
				recording_country_currency = new MGI.Cxn.WU.Common.Data.CountryCurrencyInfo() { country_code = paymentDetails.RecordingcountrycurrencyCountryCode, currency_code = paymentDetails.RecordingcountrycurrencyCurrencyCode }
			};
			return cxnPaymentDetails;

		}

		private MGI.Cxn.WU.Common.Data.Sender Mapper(MGI.Cxn.MoneyTransfer.Data.Account SenderDetails, MGI.Cxn.WU.Common.Data.Sender cxnSenderDetails)
		{
			cxnSenderDetails = new MGI.Cxn.WU.Common.Data.Sender()
			{
				FirstName = SenderDetails.FirstName,
				LastName = SenderDetails.LastName,
				AddressAddrLine1 = SenderDetails.Address,
				AddressCity = SenderDetails.City,
				AddressState = SenderDetails.State,
				AddressPostalCode = SenderDetails.PostalCode,
				ContactPhone = SenderDetails.ContactPhone,
				Email = SenderDetails.Email
			};
			return cxnSenderDetails;

		}

		public bool GetWUCardAccount(long cxnAccountId)
		{
			try
			{
				var result = WUAccountRepo.FindBy(x => x.Id == cxnAccountId);

				if (result != null && !string.IsNullOrEmpty(result.PreferredCustomerAccountNumber))
				{
					return true;
				}
				else
				{
					return false;
				}

			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(cxnAccountId), "GetWUCardAccount", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetWUCardAccount - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETWUCARDACCOUNT_FAILED, ex);
			}
		}

		public MGI.Cxn.MoneyTransfer.Data.Account DisplayWUCardAccountInfo(long cxnAccountId)
		{
			try
			{
				WUAccount WUAccountDetails = WUAccountRepo.FindBy(x => x.Id == cxnAccountId);
				MGI.Cxn.MoneyTransfer.Data.Account WUAccountInfo = new Account()
				{
					rowguid = WUAccountDetails.rowguid,
					Id = WUAccountDetails.Id,
					LoyaltyCardNumber = WUAccountDetails.PreferredCustomerAccountNumber
				};
				return WUAccountInfo;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(cxnAccountId), "DisplayWUCardAccountInfo", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in DisplayWUCardAccountInfo - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_DISPLAYWUCARDACCOUNTINFO_FAILED, ex);
			}
		}

		private string GetCardPoints(string cardNumber, MGIContext mgiContext)
		{
			RequestType requestStatus = (RequestType)Enum.Parse(typeof(RequestType), mgiContext.SMTrxType, true);
			if (requestStatus == RequestType.RELEASE)
			{
				MGI.Cxn.WU.Common.Data.CardLookUpRequest cxncardlookupreq = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
				{
					sender = new MGI.Cxn.WU.Common.Data.Sender()
					{
						PreferredCustomerAccountNumber = cardNumber,
					}
				};
				MGI.Cxn.WU.Common.Data.CardLookupDetails cardlookupdetails = WuCommon.WUCardLookupForCardNumber(cxncardlookupreq, mgiContext);
				return cardlookupdetails.WuCardTotalPointsEarned;
			}
			return null;
		}

		/// <summary>
		/// Initiate Send Money Modify - US1685
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="modifySendMoney"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public Cxn.MoneyTransfer.Data.ModifyResponse StageModify(Cxn.MoneyTransfer.Data.ModifyRequest modifySendMoney, MGIContext mgiContext)
		{

			Cxn.MoneyTransfer.Data.ModifyResponse modifySendMoneyResponse = new Cxn.MoneyTransfer.Data.ModifyResponse();

			try
			{
				CheckCounterId(mgiContext);
				
				WUTransaction trx = Get(modifySendMoney.TransactionId); // old transaction ID				

				WUTransaction cancelTrx = AutoMapper.Mapper.Map<WUTransaction, WUTransaction>(trx);

				mgiContext.TrxSubType = (int)SendMoneyTransactionSubType.Cancel;
				modifySendMoneyResponse.CancelTransactionId = CreateSendMoneyTransaction(cancelTrx, mgiContext);

				WUTransaction modifyTrx = AutoMapper.Mapper.Map<WUTransaction, WUTransaction>(trx);

				if (modifySendMoney != null)
				{
					modifyTrx.RecieverFirstName = modifySendMoney.FirstName;
					modifyTrx.RecieverSecondLastName = modifySendMoney.SecondLastName;
					modifyTrx.RecieverLastName = modifySendMoney.LastName;
					modifyTrx.TestQuestion = modifySendMoney.TestQuestion;
					modifyTrx.TestAnswer = modifySendMoney.TestAnswer;
				}


				mgiContext.TrxSubType = (int)SendMoneyTransactionSubType.Modify;

				modifySendMoneyResponse.ModifyTransactionId = CreateSendMoneyTransaction(modifyTrx, mgiContext);
			}
			catch (Exception ex)
			{
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);

				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<Cxn.MoneyTransfer.Data.ModifyResponse>(modifySendMoneyResponse, "StageModify", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in StageModify - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_STAGEMODIFY_FAILED, ex);
			}

			return modifySendMoneyResponse;
		}

		/// <summary>
		/// Get PayStatus - US1685
		/// </summary>
		/// <param name="confirmationNumber"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public string GetStatus(string confirmationNumber, MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				SendMoneyPayStatus.paystatusinquiryrequestdata searchRequest = new SendMoneyPayStatus.paystatusinquiryrequestdata()
				{
					mtcn = confirmationNumber
				};

				SendMoneyPayStatus.paystatusinquiryreply searchResponse = WUIO.GetPayStatus(searchRequest, mgiContext);
				return searchResponse.payment_transactions.payment_transaction[0].pay_status_description;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(confirmationNumber, "GetStatus", AlloyLayerName.CXN, ModuleName.ReceiveMoney,
					"Error in GetStatus - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETSTATUS_FAILED, ex);
			}
		}


		/// <summary>
		/// Commit Send Money Modify Transaction
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="modifySendMoneySearch"></param>
		/// <param name="context"></param>
		public void Modify(long transactionId, MGIContext mgiContext)
		{	
			try
			{
				CheckCounterId(mgiContext);
				string timezone = GetTimeZone(mgiContext);
				WUTransaction trx = Get(transactionId); // New transaction ID

				CxnData.SearchRequest searchRequest = new CxnData.SearchRequest();
				searchRequest.ConfirmationNumber = trx.MTCN;
				searchRequest.ReferenceNumber = trx.ReferenceNo;
				ModifySendMoneySearch.modifysendmoneysearchreply modifySendMoneySearchResponse = GetModifySearchResponse(searchRequest, mgiContext);

				UpdateSendMoneySearchTransaction(transactionId, modifySendMoneySearchResponse, mgiContext);

				string status = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].fusion != null ? modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].fusion.fusion_status : string.Empty;

				if (status.Equals("W/C"))
				{
					modifysendmoneyrequest modifySendMoneyRequest = PopulateModifySendMoneyRequest(trx, mgiContext);
					mgiContext.ReferenceNumber = trx.ReferenceNo;
					modifysendmoneyreply response = WUIO.Modify(modifySendMoneyRequest, mgiContext);
					if (response != null)
					{
						UpdateSendMoneyModifyTransaction(transactionId, response, mgiContext);
					}
				}
				else if (status.Equals("HOLD"))
				{
					throw new MoneyTransferException(MoneyTransferException.MODIFY_TRANSACTION_NOT_ALLOWED);
				}
				else
				{
					throw new MoneyTransferException(MoneyTransferException.TRANSACTION_ALREADY_PAID);
				}
			}
			catch (Exception ex)
			{
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);

				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Modify", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Modify - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_MODIFY_FAILED, ex);
			}
		}


		/// <summary>
		/// Send Money Search Modify - US1685
		/// </summary>
		/// <param name="searchRequest"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public CxnData.SearchResponse Search(CxnData.SearchRequest searchRequest, MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				if (searchRequest.SearchRequestType == SearchRequestType.Modify)
				{
					return SearchModify(searchRequest, mgiContext);
				}
				else
				{
					return SearchRefund(searchRequest, mgiContext);
				}
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<CxnData.SearchRequest>(searchRequest, "Search", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SEARCH_FAILED, ex);
			}
		}

		//US1685
		private CxnData.SearchResponse SearchModify(CxnData.SearchRequest searchRequest, MGIContext mgiContext)
		{

			try
			{
				ModifySendMoneySearch.modifysendmoneysearchreply modifySendMoneySearchResponse = GetModifySearchResponse(searchRequest, mgiContext);

				var receiver = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].receiver;

				MoneyTransfer.Data.SearchResponse searchResponse = new MoneyTransfer.Data.SearchResponse();
				if (receiver.name.name_type == ModifySendMoneySearch.name_type.D)
				{
					searchResponse.FirstName = string.IsNullOrEmpty(receiver.name.first_name) ? string.Empty : receiver.name.first_name.ToString();
					searchResponse.SecondLastName = string.IsNullOrEmpty(receiver.name.middle_name) ? string.Empty : receiver.name.middle_name.ToString();
					searchResponse.LastName = string.IsNullOrEmpty(receiver.name.last_name) ? string.Empty : receiver.name.last_name.ToString();
				}
				else
				{
					searchResponse.FirstName = string.IsNullOrEmpty(receiver.name.given_name) ? string.Empty : receiver.name.given_name.ToString();
					searchResponse.SecondLastName = string.IsNullOrEmpty(receiver.name.maternal_name) ? string.Empty : receiver.name.maternal_name.ToString();
					searchResponse.LastName = string.IsNullOrEmpty(receiver.name.paternal_name) ? string.Empty : receiver.name.paternal_name.ToString();
				}

				if (modifySendMoneySearchResponse.delivery_services != null)
				{
					searchResponse.TestQuestionAvailable = modifySendMoneySearchResponse.delivery_services.test_question_available;

					if (modifySendMoneySearchResponse.delivery_services.identification_question != null)
					{
						searchResponse.TestQuestion = modifySendMoneySearchResponse.delivery_services.identification_question.question;
						searchResponse.TestAnswer = modifySendMoneySearchResponse.delivery_services.identification_question.answer;
					}
				}
				return searchResponse;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<CxnData.SearchRequest>(searchRequest, "SearchModify", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SEARCHMODIFY_FAILED, ex);
			}
		}


		//US1685
		private ModifySendMoneySearch.modifysendmoneysearchreply GetModifySearchResponse(Cxn.MoneyTransfer.Data.SearchRequest searchRequest, MGIContext mgiContext)
		{
			ModifySendMoneySearch.modifysendmoneysearchrequest modifySearchSendRequest = new ModifySendMoneySearch.modifysendmoneysearchrequest();
			ModifySendMoneySearch.payment_transaction transaction = new ModifySendMoneySearch.payment_transaction();
			transaction.mtcn = searchRequest.ConfirmationNumber;

			modifySearchSendRequest.payment_transaction = transaction;

			ModifySendMoneySearch.modifysendmoneysearchreply sendMoneyModifySearchResponse = WUIO.ModifySearch(modifySearchSendRequest, mgiContext);
			return sendMoneyModifySearchResponse;
		}

		private modifysendmoneyrequest PopulateModifySendMoneyRequest(WUTransaction transaction, MGIContext mgiContext)
		{
			WUAccount account = transaction.WUnionAccount;
			WUReceiver receiver = transaction.WUnionRecipient;

			receiver.FirstName = NexxoUtil.MassagingValue(transaction.RecieverFirstName);
			receiver.LastName = NexxoUtil.MassagingValue(transaction.RecieverLastName);
			receiver.SecondLastName = NexxoUtil.MassagingValue(transaction.RecieverSecondLastName);
			receiver.City = NexxoUtil.MassagingValue(receiver.City);
			receiver.PickupCountry = NexxoUtil.MassagingValue(receiver.PickupCountry);
			account.Address = NexxoUtil.MassagingValue(account.Address);
			account.City = NexxoUtil.MassagingValue(account.City);
			account.State = NexxoUtil.MassagingValue(account.State);
			account.FirstName = NexxoUtil.MassagingValue(account.FirstName);
			account.LastName = NexxoUtil.MassagingValue(account.LastName);
			account.MiddleName = NexxoUtil.MassagingValue(account.MiddleName);
			account.SecondLastName = NexxoUtil.MassagingValue(account.SecondLastName);

			ModifySendMoney.general_name receiverName = null;

			var countryinfo = new ModifySendMoney.country_currency_info()
			{
				iso_code = new ModifySendMoney.iso_code()
				{
					country_code = CountryCode,
					currency_code = CountryCurrencyCode
				}
			};

			if (!string.IsNullOrWhiteSpace(transaction.RecieverSecondLastName))
			{
				receiverName = new ModifySendMoney.general_name()
				{
					given_name = receiver.FirstName,
					paternal_name = receiver.LastName,
					maternal_name = receiver.SecondLastName,
					name_type = ModifySendMoney.name_type.M,
					name_typeSpecified = true
				};
			}
			else
			{
				receiverName = new ModifySendMoney.general_name()
				{
					first_name = receiver.FirstName,
					last_name = receiver.LastName,
					name_type = ModifySendMoney.name_type.D,
					name_typeSpecified = true
				};
			}

			var modifysendmoneyrequest = new modifysendmoneyrequest()
			{


				sender = new ModifySendMoney.sender
				{
					preferred_customer = new ModifySendMoney.preferred_customer()
					{
						account_nbr = account.PreferredCustomerAccountNumber,
					},
					name = new ModifySendMoney.general_name
					{
						first_name = account.FirstName,
						last_name = account.LastName,
						name_type = ModifySendMoney.name_type.D,
						name_typeSpecified = true
					},
					address = new ModifySendMoney.address()
					{
						addr_line1 = account.Address,
						city = account.City,
						state = account.State,
						postal_code = account.PostalCode,
						Item = countryinfo,

					},
					contact_phone = account.ContactPhone,
					compliance_details = new ModifySendMoney.compliance_details()
					{
						compliance_data_buffer = transaction.SenderComplianceDetailsComplianceDataBuffer
					},

					unv_buffer = transaction.Sender_unv_Buffer
				},
				receiver = new ModifySendMoney.receiver()
				{
					name = receiverName,
					contact_phone = receiver.PhoneNumber,
					unv_buffer = transaction.Receiver_unv_Buffer,
					address = new ModifySendMoney.address()
					{
						city = receiver.City
					}
				},
				promotions = new ModifySendMoney.promotions()
				{
					promo_sequence_no = transaction.PromotionSequenceNo,
					coupons_promotions = transaction.PromotionsCode ?? string.Empty,
					promo_code_description = transaction.PromoCodeDescription ?? string.Empty,
					promo_name = transaction.PromoName ?? string.Empty,
					promo_discount_amount = Convert.ToInt64(transaction.PromotionDiscount),
					sender_promo_code = transaction.PromotionsCode ?? string.Empty,
					promo_message = transaction.PromoMessage ?? string.Empty,
					promo_discount_amountSpecified = transaction.PromotionDiscount > 0
				},
				payment_details = new ModifySendMoney.payment_details()
				{
					expected_payout_location = new ModifySendMoney.expected_payout_location()
					{
						city = transaction.ExpectedPayoutCityName,
						state_code = transaction.ExpectedPayoutStateCode
					},

					originating_country_currency = countryinfo,

					recording_country_currency = countryinfo,
					destination_country_currency = new ModifySendMoney.country_currency_info()
					{
						iso_code = new ModifySendMoney.iso_code()
						{
							country_code = transaction.DestinationCountryCode,
							currency_code = transaction.DestinationCurrencyCode
						}
					},
					originating_city = transaction.originating_city,
					originating_state = transaction.originating_state,
					transaction_type = ModifySendMoney.transaction_type.WMN,
					transaction_typeSpecified = true,
					payment_type = ModifySendMoney.payment_type.Cash,
					payment_typeSpecified = true,
					duplicate_detection_flag = AllowDuplicateTrxWU,
					exchange_rate = Convert.ToInt64(transaction.ExchangeRate),
					money_transfer_type = ModifySendMoney.money_transfer_type.WMN,
					original_destination_country_currency = new ModifySendMoney.country_currency_info()
					{
						iso_code = new ModifySendMoney.iso_code()
						{
							currency_code = transaction.OriginalDestinationCurrencyCode,
							country_code = transaction.OriginalDestinationCountryCode
						}
					},

				},

				delivery_services = new ModifySendMoney.delivery_services()
				{
					code = string.IsNullOrEmpty(transaction.DeliveryOption) ? transaction.DeliveryServiceName : transaction.DeliveryOption,
				},
				financials = new ModifySendMoney.financials()
				{
					taxes = new ModifySendMoney.taxes()
					{
						municipal_tax = ConvertDecimalToLong(transaction.municipal_tax),
						municipal_taxSpecified = true,
						state_tax = ConvertDecimalToLong(transaction.state_tax),
						state_taxSpecified = true,
						county_tax = ConvertDecimalToLong(transaction.county_tax),
						county_taxSpecified = true,
					},
					gross_total_amount = ConvertDecimalToLong(transaction.GrossTotalAmount),
					gross_total_amountSpecified = transaction.GrossTotalAmount > 0,
					plus_charges_amount = ConvertDecimalToLong(transaction.plus_charges_amount),
					plus_charges_amountSpecified = transaction.plus_charges_amount > 0,
					charges = ConvertDecimalToLong(transaction.Charges),
					chargesSpecified = transaction.Charges > 0,
					principal_amount = ConvertDecimalToLong(transaction.Principal_Amount),
					principal_amountSpecified = true
				},
				mtcn = transaction.MTCN,
				new_mtcn = transaction.TempMTCN,
				money_transfer_key = transaction.MoneyTransferKey,
				confirmed_id = modifysendmoneyrequestConfirmed_id.Y,
				confirmed_idSpecified = true,


				df_fields = new ModifySendMoney.df_fields()
				{
					amount_to_receiver = Convert.ToDouble(transaction.AmountToReceiver),
					amount_to_receiverSpecified = transaction.AmountToReceiver > 0,
					pay_side_charges = Convert.ToDouble(transaction.PaySideCharges),
					pay_side_chargesSpecified = transaction.PaySideCharges > 0,
					pay_side_tax = Convert.ToDouble(transaction.PaySideTax),
					pay_side_taxSpecified = transaction.PaySideTax > 0,
					delivery_service_name = transaction.DeliveryServiceDesc ?? string.Empty,
					pds_required_flag = transaction.PdsRequiredFlag ? ModifySendMoney.yes_no.Y : ModifySendMoney.yes_no.N,
					pds_required_flagSpecified = true,
					df_transaction_flag = transaction.DfTransactionFlag ? ModifySendMoney.yes_no.Y : ModifySendMoney.yes_no.N,
					df_transaction_flagSpecified = true,
					available_for_pickup = transaction.AvailableForPickup,
					available_for_pickup_est = transaction.AvailableForPickupEST
				}
			};

			if (transaction.message_charge > 0)
			{
				modifysendmoneyrequest.financials.message_charge = ConvertDecimalToLong(transaction.message_charge);
				modifysendmoneyrequest.financials.message_chargeSpecified = true;
			}

			if (transaction.OriginatorsPrincipalAmount > 0)
			{
				modifysendmoneyrequest.financials.originators_principal_amount = ConvertDecimalToLong(transaction.OriginatorsPrincipalAmount);
				modifysendmoneyrequest.financials.originators_principal_amountSpecified = true;
			}

			if (transaction.DestinationPrincipalAmount > 0)
			{
				modifysendmoneyrequest.financials.destination_principal_amount = ConvertDecimalToLong(transaction.DestinationPrincipalAmount);
				modifysendmoneyrequest.financials.destination_principal_amountSpecified = true;
			}

			if (transaction.total_discount > 0)
			{
				modifysendmoneyrequest.financials.total_discount = ConvertDecimalToLong(transaction.total_discount);
				modifysendmoneyrequest.financials.total_discountSpecified = true;
			}

			if (transaction.total_discounted_charges > 0)
			{
				modifysendmoneyrequest.financials.total_discounted_charges = ConvertDecimalToLong(transaction.total_discounted_charges);
				modifysendmoneyrequest.financials.total_discounted_chargesSpecified = true;
			}

			if (transaction.total_undiscounted_charges > 0)
			{
				modifysendmoneyrequest.financials.total_undiscounted_charges = ConvertDecimalToLong(transaction.total_undiscounted_charges);
				modifysendmoneyrequest.financials.total_undiscounted_chargesSpecified = true;
			}

			if (account.SmsNotificationFlag == "Y")
			{
				modifysendmoneyrequest.sender.mobile_phone = new ModifySendMoney.mobile_phone()
				{
					phone_number = new ModifySendMoney.international_phone_number()
					{
						country_code = "1",
						national_number = account.MobilePhone
					}
				};
			}

			if (!string.IsNullOrWhiteSpace(transaction.TestQuestion))
			{
				modifysendmoneyrequest.delivery_services.identification_question = new ModifySendMoney.identification_question()
				{
					question = transaction.TestQuestion,
					answer = transaction.TestAnswer
				};
			}

			if (!string.IsNullOrWhiteSpace(transaction.PersonalMessage))
			{
				string[] personalMessages = MessageBlockSplit(transaction.PersonalMessage).ToArray();

				int msgcnt = personalMessages.Length;
				var msgs = new ModifySendMoney.message_details()
				{
					message_details1 = new ModifySendMoney.messages()
					{
						text = personalMessages,
						context = msgcnt.ToString()
					}
				};
				modifysendmoneyrequest.delivery_services.message = msgs;
			}

			if (!string.IsNullOrWhiteSpace(transaction.instant_notification_addl_service_charges))
			{
				modifysendmoneyrequest.instant_notification = new ModifySendMoney.instant_notification()
				{
					addl_service_charges = transaction.instant_notification_addl_service_charges
				};
			}

			modifysendmoneyrequest.swb_fla_info = AutoMapper.Mapper.Map<SwbFlaInfo, ModifySendMoney.swb_fla_info>(WuCommon.BuildSwbFlaInfo(mgiContext));
			modifysendmoneyrequest.swb_fla_info.fla_name = AutoMapper.Mapper.Map<GeneralName, ModifySendMoney.general_name>(WuCommon.BuildGeneralName(mgiContext));

			return modifysendmoneyrequest;
		}

		/// <summary>
		/// AL-3502
		/// </summary>
		/// <param name="receiver"></param>
		/// <param name="mgiContext"></param>
		/// <returns></returns>
		public bool DeleteFavoriteReceiver(Receiver receiver, MGIContext mgiContext)
		{
			try
			{
				WUReceiver wuReceiver = _WUReceiverRepo.FindBy(x => x.Id == receiver.Id);
				wuReceiver.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				wuReceiver.Status = receiver.Status;
				wuReceiver.DTServerLastModified = DateTime.Now;
				return _WUReceiverRepo.UpdateWithFlush(wuReceiver);
			}
			catch (Exception ex)
			{
				//Transactional Log User Story
				MongoDBLogger.Error<Receiver>(receiver, "DeleteFavoriteReceiver", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in DeleteFavoriteReceiver - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_DELETEFAVORITERECEIVER_FAILED, ex);
			}
		}

		//US1685
		private void UpdateSendMoneyModifyTransaction(long transactionId, modifysendmoneyreply response, MGIContext mgiContext)
		{
			WUTransaction WUTrxlog = Get(transactionId);
			WUTrxlog.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			WUTrxlog.DTServerLastModified = DateTime.Now;
			WUTrxlog.MTCN = response.mtcn;
			_wuTransactionLogRepo.UpdateWithFlush(WUTrxlog);

			WUReceiver receiver = _WUReceiverRepo.FindBy(x => x.Id == WUTrxlog.WUnionRecipient.Id);
			receiver.FirstName = WUTrxlog.RecieverFirstName;
			receiver.LastName = WUTrxlog.RecieverLastName;
			receiver.SecondLastName = WUTrxlog.RecieverSecondLastName;
			_WUReceiverRepo.UpdateWithFlush(receiver);
		}

		//US1685
		private void UpdateSendMoneySearchTransaction(long transactionId, ModifySendMoneySearch.modifysendmoneysearchreply modifySendMoneySearchResponse, MGIContext mgiContext)
		{
			WUTransaction WUTrxlog = Get(transactionId);
			WUTrxlog.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			WUTrxlog.DTServerLastModified = DateTime.Now;
			WUTrxlog.MoneyTransferKey = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].money_transfer_key;
			WUTrxlog.Sender_unv_Buffer = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].sender.unv_buffer;
			WUTrxlog.Receiver_unv_Buffer = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].receiver.unv_buffer;
			WUTrxlog.Principal_Amount = ConvertLongToDecimal(modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].financials.principal_amount);
			_wuTransactionLogRepo.UpdateWithFlush(WUTrxlog);
		}

		/// <summary>
		/// Search Send Money Refund - US1686
		/// </summary>
		/// <param name="transactionID"></param>
		/// <param name="searchRefundRequest"></param>
		/// <param name="RefundStatus"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		private CxnData.SearchResponse SearchRefund(CxnData.SearchRequest searchRefundRequest, MGIContext mgiContext)
		{
			try
			{
				WUTransaction trx;
				searchRefundRequest.ReferenceNumber = DateTime.Now.ToString("yyyyMMddhhmmssff");

				CxnData.SearchResponse searchResponse = new CxnData.SearchResponse();
				Search.searchreply response = new Search.searchreply();
				if (searchRefundRequest.SearchRequestType == SearchRequestType.RefundWithStage)
				{
					trx = Get(searchRefundRequest.TransactionId);
					mgiContext.TrxSubType = (int)SendMoneyTransactionSubType.Cancel;

					//Create Cancel Record
					long cancelTranId = CreateSendMoneyRefundTransaction(trx, mgiContext);

					trx.ReasonCode = searchRefundRequest.ReasonCode;
					trx.ReasonDescription = searchRefundRequest.ReasonDesc;
					trx.Comments = searchRefundRequest.Comments;
					trx.ReferenceNo = searchRefundRequest.ReferenceNumber;

					mgiContext.TrxSubType = (int)SendMoneyTransactionSubType.Refund;
					//Create Refund Record
					long refundTranId = CreateSendMoneyRefundTransaction(trx, mgiContext);

					response = GetSearchRefundResponse(searchRefundRequest, mgiContext);

					UpdateSendMoneyRefundSearchTransaction(refundTranId, response, mgiContext);

					searchResponse.CancelTransactionId = cancelTranId;
					searchResponse.RefundTransactionId = refundTranId;
					searchResponse.RefundStatus = response.refund_cancel_flag;
				}
				else
				{
					response = GetSearchRefundResponse(searchRefundRequest, mgiContext);
					searchResponse.RefundStatus = response.refund_cancel_flag;
				}

				return searchResponse;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<CxnData.SearchRequest>(searchRefundRequest, "SearchRefund", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SEARCHREFUND_FAILED, ex);
			}
		}

		//US1685
		private Search.searchreply GetSearchRefundResponse(Cxn.MoneyTransfer.Data.SearchRequest searchRequest, MGIContext mgiContext)
		{
			Search.searchrequest searchReq = new Search.searchrequest();
			searchReq.payment_transaction = new Search.payment_transaction();
			searchReq.payment_transaction.mtcn = searchRequest.ConfirmationNumber;
			searchReq.search_flag = MGI.Cxn.MoneyTransfer.WU.Search.agentcsc_flags.REFUND;
			searchReq.search_flagSpecified = true;
			searchReq.device = new Search.gwp_gbs_device();
			searchReq.device.type = MGI.Cxn.MoneyTransfer.WU.Search.gwp_gbs_device_type.AGENT;
			searchReq.device.typeSpecified = true;
			mgiContext.ReferenceNumber = searchRequest.ReferenceNumber;

			Search.searchreply reply = WUIO.Search(searchReq, mgiContext);

			return reply;
		}


		/// <summary>
		/// Update Transaction - US1686
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="searchSendMoneyResponse"></param>
		/// <param name="context"></param>
		private void UpdateSendMoneyRefundSearchTransaction(long transactionId, Search.searchreply searchSendMoneyResponse, MGIContext mgiContext)
		{
			WUTransaction WUTrxlog = Get(transactionId);
			WUTrxlog.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			WUTrxlog.DTServerLastModified = DateTime.Now;

			var item = searchSendMoneyResponse.payment_transactions.payment_transaction[0];

			WUTrxlog.recordingCountryCode = item.payment_details.originating_country_currency.iso_code.country_code;
			WUTrxlog.recordingCurrencyCode = item.payment_details.originating_country_currency.iso_code.currency_code;
			WUTrxlog.OriginatorsPrincipalAmount = item.financials.principal_amount / 100;
			WUTrxlog.GrossTotalAmount = item.financials.gross_total_amount / 100;
			WUTrxlog.DestinationPrincipalAmount = item.financials.pay_amount / 100;
			WUTrxlog.Charges = item.financials.charges / 100;
			WUTrxlog.MoneyTransferKey = item.money_transfer_key;
			WUTrxlog.TempMTCN = item.new_mtcn;

			_wuTransactionLogRepo.UpdateWithFlush(WUTrxlog);
		}


		/// <summary>
		/// Send Money Refund - US1686
		/// </summary>
		/// <param name="transactionID"></param>
		/// <param name="refundRequest"></param>
		/// <param name="context"></param>
		public string Refund(MGI.Cxn.MoneyTransfer.Data.RefundRequest refundRequest, MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				WUTransaction trx = Get(refundRequest.TransactionId);

				//context.Add("ReferenceNo", trx.ReferenceNo);

				SendMoneyRefund.refundrequest request = PopulateRefundSendMoneyRequest(trx, refundRequest);
				request.swb_fla_info = AutoMapper.Mapper.Map<SwbFlaInfo, SendMoneyRefund.swb_fla_info>(WuCommon.BuildSwbFlaInfo(mgiContext));
				request.swb_fla_info.fla_name = AutoMapper.Mapper.Map<GeneralName, SendMoneyRefund.general_name>(WuCommon.BuildGeneralName(mgiContext));

				SendMoneyRefund.refundreply response = WUIO.Refund(request, mgiContext);
				if (response != null)
				{
					UpdateSendMoneyRefundTransaction(refundRequest.TransactionId, response, mgiContext);

				}
				return response.mtcn;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<MGI.Cxn.MoneyTransfer.Data.RefundRequest>(refundRequest, "Refund", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Refund - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_REFUND_FAILED, ex);
			}
		}

		//US1687
		public List<Reason> GetRefundReasons(ReasonRequest request, MGIContext mgiContext)
		{
			try
			{
				return WUIO.GetRefundReasons(request, mgiContext);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<ReasonRequest>(request, "GetRefundReasons", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetRefundReasons - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETREFUNDREASONS_FAILED, ex);
			}
		}

		public List<DeliveryService> GetDeliveryServices(DeliveryServiceRequest request, MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				var deliveryServices = new List<DeliveryService>();

				string state = string.Empty;
				string stateCode = string.Empty;
				string city = string.Empty;
				string deliveryService = string.Empty;

				ValidateDeliveryServices(request, mgiContext);

				state = Convert.ToString(request.MetaData["State"]);
				stateCode = Convert.ToString(request.MetaData["StateCode"]);
				city = Convert.ToString(request.MetaData["City"]);

				if (request.Type == DeliveryServiceType.Option)
				{
					deliveryService = Convert.ToString(request.MetaData["DeliveryService"]);
				}

				deliveryServices = WUIO.GetDeliveryServices(request, state, stateCode, city, deliveryService, mgiContext);
				return deliveryServices;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<DeliveryServiceRequest>(request, "GetDeliveryServices", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetDeliveryServices - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETDELIVERYSERVICES_FAILED, ex);
			}
		}

		/// <summary>
		/// Mapper SearchRequest - US1686
		/// </summary>
		/// <param name="transactionID"></param>
		/// <param name="refundRequest"></param>
		private SendMoneyRefund.refundrequest PopulateRefundSendMoneyRequest(WUTransaction transaction, MGI.Cxn.MoneyTransfer.Data.RefundRequest refundRequest)
		{
			WUAccount account = transaction.WUnionAccount;
			WUReceiver receiver = transaction.WUnionRecipient;

			receiver.FirstName = NexxoUtil.MassagingValue(transaction.RecieverFirstName);
			receiver.LastName = NexxoUtil.MassagingValue(transaction.RecieverLastName);
			receiver.SecondLastName = NexxoUtil.MassagingValue(transaction.RecieverSecondLastName);
			receiver.City = NexxoUtil.MassagingValue(receiver.City);
			receiver.PickupCountry = NexxoUtil.MassagingValue(receiver.PickupCountry);
			account.Address = NexxoUtil.MassagingValue(account.Address);
			account.City = NexxoUtil.MassagingValue(account.City);
			account.State = NexxoUtil.MassagingValue(account.State);
			account.FirstName = NexxoUtil.MassagingValue(account.FirstName);
			account.LastName = NexxoUtil.MassagingValue(account.LastName);

			SendMoneyRefund.general_name receiverName = null;

			var countryinfo = new SendMoneyRefund.country_currency_info()
			{
				iso_code = new SendMoneyRefund.iso_code()
				{
					country_code = CountryCode,
					currency_code = CountryCurrencyCode
				}
			};

			if (!string.IsNullOrWhiteSpace(transaction.RecieverSecondLastName))
			{
				receiverName = new SendMoneyRefund.general_name()
				{
					given_name = receiver.FirstName,
					paternal_name = receiver.LastName,
					maternal_name = receiver.SecondLastName,
					name_type = SendMoneyRefund.name_type.M,
					name_typeSpecified = true
				};
			}
			else
			{
				receiverName = new SendMoneyRefund.general_name()
				{
					first_name = receiver.FirstName,
					last_name = receiver.LastName,
					name_type = SendMoneyRefund.name_type.D,
					name_typeSpecified = true
				};
			}

			var refundrequest = new SendMoneyRefund.refundrequest()
			{
				sender = new SendMoneyRefund.sender
				{
					preferred_customer = new SendMoneyRefund.preferred_customer()
					{
						account_nbr = account.PreferredCustomerAccountNumber,
					},
					name = new SendMoneyRefund.general_name
					{
						first_name = account.FirstName,
						last_name = account.LastName,
						middle_name = account.MiddleName,
						name_type = SendMoneyRefund.name_type.D,
						name_typeSpecified = true
					},
					address = new SendMoneyRefund.address()
					{
						addr_line1 = account.Address,
						city = account.City,
						state = account.State,
						postal_code = account.PostalCode,
						Item = countryinfo
					},
					contact_phone = account.ContactPhone,
					compliance_details = new SendMoneyRefund.compliance_details()
					{
						compliance_data_buffer = transaction.SenderComplianceDetailsComplianceDataBuffer
					}
				},
				receiver = new SendMoneyRefund.receiver()
				{
					name = receiverName,
					contact_phone = receiver.PhoneNumber
				},
				payment_details = new SendMoneyRefund.payment_details()
				  {
					  expected_payout_location = new SendMoneyRefund.expected_payout_location()
					  {
						  city = transaction.ExpectedPayoutCityName,
						  state_code = transaction.ExpectedPayoutStateCode
					  },
					  originating_country_currency = countryinfo,
					  recording_country_currency = countryinfo,
					  destination_country_currency = new SendMoneyRefund.country_currency_info()
					  {
						  iso_code = new SendMoneyRefund.iso_code()
						  {
							  country_code = transaction.DestinationCountryCode,
							  currency_code = transaction.DestinationCurrencyCode
						  }
					  },
					  transaction_type = SendMoneyRefund.transaction_type.WMN,
					  transaction_typeSpecified = true,
					  payment_type = SendMoneyRefund.payment_type.Refund,
					  payment_typeSpecified = true,
					  duplicate_detection_flag = AllowDuplicateTrxWU
				  },

				financials = new SendMoneyRefund.financials()
				{
					originators_principal_amount = ConvertDecimalToLong(transaction.OriginatorsPrincipalAmount),
					originators_principal_amountSpecified = transaction.OriginatorsPrincipalAmount > 0,
					destination_principal_amount = ConvertDecimalToLong(transaction.DestinationPrincipalAmount),
					destination_principal_amountSpecified = transaction.DestinationPrincipalAmount > 0,
					gross_total_amount = ConvertDecimalToLong(transaction.GrossTotalAmount),
					gross_total_amountSpecified = transaction.GrossTotalAmount > 0,
					pay_amount = ConvertDecimalToLong(transaction.AmountToReceiver),
					pay_amountSpecified = transaction.AmountToReceiver > 0,
					principal_amount = ConvertDecimalToLong(transaction.OriginatorsPrincipalAmount),
					principal_amountSpecified = transaction.OriginatorsPrincipalAmount > 0,
					plus_charges_amount = ConvertDecimalToLong(transaction.plus_charges_amount),
					plus_charges_amountSpecified = transaction.plus_charges_amount > 0,
					charges = ConvertDecimalToLong(transaction.Charges),
					chargesSpecified = transaction.Charges > 0
				},
				mtcn = transaction.MTCN,
				new_mtcn = transaction.TempMTCN,
				money_transfer_key = transaction.MoneyTransferKey,
				encompass_reason_code = refundRequest.ReasonDesc == null ? "" : refundRequest.ReasonDesc.Substring(0, 3),
				comments = refundRequest.Comments,
				refund_cancel_flag = refundRequest.RefundStatus
			};

			return refundrequest;
		}

		//US1685
		private long CreateSendMoneyTransaction(WUTransaction WUTrxlog, MGIContext mgiContext)
		{
			try
			{
				bool receiverExisted = _isReceiverExisting(new Receiver() { Id = WUTrxlog.WUnionRecipient.Id, FirstName = WUTrxlog.RecieverFirstName, LastName = WUTrxlog.RecieverLastName }, WUTrxlog.WUnionRecipient.CustomerId ?? 0);

				if (receiverExisted)
					throw new MoneyTransferException(MoneyTransferException.RECEIVER_ALREADY_EXISTED);

				WUTrxlog.rowguid = Guid.NewGuid();
				WUTrxlog.DTServerCreate = DateTime.Now;
				WUTrxlog.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				WUTrxlog.OriginalTransactionID = WUTrxlog.Id;
				WUTrxlog.TransactionSubType = mgiContext.TrxSubType.ToString();
				WUTrxlog.ReferenceNo = DateTime.Now.ToString("yyyyMMddhhmmssff");
				_wuTransactionLogRepo.AddWithFlush(WUTrxlog);
				return WUTrxlog.Id;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<WUTransaction>(WUTrxlog, "CreateSendMoneyTransaction", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_CREATESENDMONEYTRANSACTION_FAILED, ex);
			}
		}

		/// <summary>
		/// Create Transaction - US1686
		/// </summary>
		/// <param name="trx"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		private long CreateSendMoneyRefundTransaction(WUTransaction trx, MGIContext mgiContext)
		{
			try
			{
				WUTransaction WUTrxlog = new WUTransaction();
				//Mapping Transaction
				AutoMapper.Mapper.Map<WUTransaction, WUTransaction>(trx, WUTrxlog);
				WUTrxlog.rowguid = Guid.NewGuid();
				WUTrxlog.DTServerCreate = DateTime.Now;
				WUTrxlog.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				WUTrxlog.OriginalTransactionID = trx.Id;
				WUTrxlog.TransactionSubType = mgiContext.TrxSubType.ToString();
				WUTrxlog.CounterId = mgiContext.WUCounterId;
				_wuTransactionLogRepo.AddWithFlush(WUTrxlog);

				return WUTrxlog.Id;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<WUTransaction>(trx, "CreateSendMoneyRefundTransaction", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_CREATESENDMONEYREFUNDTRANSACTION_FAILED, ex);
			}
		}

		public long StageRefund(RefundRequest refundRequest, MGIContext mgiContext)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Update Transaction Refund Search - US1686
		/// </summary>
		/// <param name="transactionId"></param>
		/// <param name="refundsendresponse"></param>
		/// <param name="context"></param>
		private void UpdateSendMoneyRefundTransaction(long transactionId, SendMoneyRefund.refundreply refundsendresponse, MGIContext mgiContext)
		{
			try
			{
				WUTransaction WUTrxlog = Get(transactionId);

				if (refundsendresponse != null)
				{
					WUTrxlog.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
					WUTrxlog.DTServerLastModified = DateTime.Now;
					WUTrxlog.OriginatorsPrincipalAmount = refundsendresponse.financials.principal_amount / 100;
					WUTrxlog.DestinationPrincipalAmount = refundsendresponse.financials.pay_amount / 100;
					WUTrxlog.GrossTotalAmount = refundsendresponse.financials.gross_total_amount / 100;
					WUTrxlog.Charges = refundsendresponse.financials.charges / 100;
					_wuTransactionLogRepo.UpdateWithFlush(WUTrxlog);
				}
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<SendMoneyRefund.refundreply>(refundsendresponse, "UpdateSendMoneyRefundTransaction", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATESENDMONEYREFUNDTRANSACTION_FAILED, ex);
			}
		}

		private string GetTimeZone(MGIContext mgiContext)
		{
			if (string.IsNullOrEmpty(mgiContext.TimeZone))
			{
				throw new MoneyTransferException(MoneyTransferException.TIME_ZONE_NOT_PROVIDE);
			}
			return mgiContext.TimeZone;
		}

		private string GetCountryCode(string countyName)
		{
			string countryCode = string.Empty;

			if (!string.IsNullOrWhiteSpace(countyName))
			{
				var country = WUCountryRepo.FindBy(c => c.Name == countyName);
				if (country != null)
				{
					countryCode = country.CountryCode;
				}
			}

			return countryCode;
		}

		private string GetCountryName(string countyCode)
		{
			string countryName = string.Empty;

			if (!string.IsNullOrWhiteSpace(countyCode))
			{
				var country = WUCountryRepo.FindBy(c => c.CountryCode == countyCode);
				if (country != null)
				{
					countryName = country.Name;
				}
			}

			return countryName;
		}

		private string GetStateCode(string stateName)
		{
			string stateCode = string.Empty;

			if (!string.IsNullOrWhiteSpace(stateName))
			{
				var state = WUStateRepo.FindBy(c => c.Name == stateName);
				if (state != null)
				{
					stateCode = state.StateCode;
				}
			}

			return stateCode;
		}

		private static void ValidateDeliveryServices(DeliveryServiceRequest request, MGIContext mgiContext)
		{
			if (string.IsNullOrWhiteSpace(request.CountryCode) || string.IsNullOrWhiteSpace(request.CountryCurrency))
			{
				throw new MoneyTransferException(MoneyTransferException.DESTINATION_COUNTRY_CODE_NOT_FOUND);
			}

			if (!request.MetaData.ContainsKey("State"))
			{
				throw new MoneyTransferException(MoneyTransferException.DESTINATION_STATE_NOT_FOUND);
			}

			if (!request.MetaData.ContainsKey("StateCode"))
			{
				throw new MoneyTransferException(MoneyTransferException.DESTINATION_STATE_CODE_NOT_FOUND);
			}

			if (!request.MetaData.ContainsKey("City"))
			{
				throw new MoneyTransferException(MoneyTransferException.DESTINATION_CITY_NOT_FOUND);
			}

			if (request.Type == DeliveryServiceType.Option)
			{
				if (!request.MetaData.ContainsKey("DeliveryService"))
				{
					throw new MoneyTransferException(MoneyTransferException.DELIVERYSERVICE_NOT_FOUND);
				}
			}
		}

		private long CreateTrx(FeeInquiry.feeinquiryrequest feeInquiryRequest, FeeRequest feeRequest, MGIContext mgiContext)
		{
			long transactionId = 0;
			WUTransaction transaction = new WUTransaction()
			{
				DestinationPrincipalAmount = ConvertLongToDecimal(feeInquiryRequest.financials.destination_principal_amount),
				OriginatorsPrincipalAmount = ConvertLongToDecimal(feeInquiryRequest.financials.originators_principal_amount),

				DestinationCountryCode = feeInquiryRequest.payment_details.destination_country_currency.iso_code.country_code,
				DestinationCurrencyCode = feeInquiryRequest.payment_details.destination_country_currency.iso_code.currency_code,

				OriginatingCountryCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.country_code,
				OriginatingCurrencyCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.currency_code,

				recordingCountryCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.country_code,
				recordingCurrencyCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.currency_code,
				IsFixedOnSend = feeInquiryRequest.payment_details.fix_on_send == FeeInquiry.yes_no.Y,

				DeliveryOption = feeInquiryRequest.delivery_services.code,
				PersonalMessage = feeRequest.PersonalMessage,
				PromotionsCode = feeInquiryRequest.promotions.coupons_promotions,
				GCNumber = feeInquiryRequest.preferred_customer_no,
				IsDomesticTransfer = feeRequest.IsDomesticTransfer,
				ReferenceNo = feeRequest.ReferenceNo
			};

			if (!string.IsNullOrWhiteSpace(feeRequest.ReceiverSecondLastName))
			{
				transaction.RecieverFirstName = feeInquiryRequest.receiver.name.given_name;
				transaction.RecieverLastName = feeInquiryRequest.receiver.name.paternal_name;
				transaction.RecieverSecondLastName = feeRequest.ReceiverSecondLastName;
			}
			else
			{
				transaction.RecieverFirstName = feeInquiryRequest.receiver.name.first_name;
				transaction.RecieverLastName = feeInquiryRequest.receiver.name.last_name;
			}
			
			transaction.rowguid = Guid.NewGuid();
			//Changes for timestamp
			transaction.DTServerCreate = DateTime.Now;
			transaction.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			transaction.WUnionRecipient = _WUReceiverRepo.FindBy(x => x.Id == feeRequest.ReceiverId);
			transaction.WUnionAccount = WUAccountRepo.FindBy(x => x.Id == feeRequest.AccountId);
			transaction.TranascationType = ((int)TransferType.sendMoney).ToString();
			transaction.ChannelPartnerId = mgiContext.ChannelPartnerId;
			transaction.ProviderId = mgiContext.ProviderId;
			_wuTransactionLogRepo.AddWithFlush(transaction);

			transactionId = transaction.Id;

			return transactionId;
		}

		public FeeResponse GetFee(FeeRequest feeRequest, MGIContext mgiContext)
		{

			try
			{
				CheckCounterId(mgiContext);
				FeeInquiry.feeinquiryrequest feeInquiryRequest = BuildFeeEnquiryRequest(feeRequest);
				feeInquiryRequest.swb_fla_info = AutoMapper.Mapper.Map<SwbFlaInfo, FeeInquiry.swb_fla_info>(WuCommon.BuildSwbFlaInfo(mgiContext));
				feeInquiryRequest.swb_fla_info.fla_name = AutoMapper.Mapper.Map<GeneralName, FeeInquiry.general_name>(WuCommon.BuildGeneralName(mgiContext));
				FeeResponse feeResponse = null;
				decimal transferAmount = feeRequest.Amount > 0 ? feeRequest.Amount : feeRequest.ReceiveAmount;
				long transactionId = 0L;
				if (feeRequest.TransactionId == 0)
				{
					// Create new MoneyTransfer transaction
					transactionId = CreateTrx(feeInquiryRequest, feeRequest, mgiContext);
					feeRequest.TransactionId = transactionId;
				}
				else
				{
					UpdateTrx(feeRequest, feeInquiryRequest, mgiContext);
					transactionId = feeRequest.TransactionId;
				}

				FeeInquiry.feeinquiryreply reply = WUIO.FeeInquiry(feeInquiryRequest, mgiContext);
				feeResponse = MapfeeEnquiryResponse(reply);
				feeResponse.TransactionId = transactionId;

				UpdateTrx(feeRequest, feeResponse, mgiContext);
				
				return feeResponse;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<FeeRequest>(feeRequest, "GetFee", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetFee - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);

				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETFEE_FAILED, ex);
			}

		}

		private void UpdateTrx(FeeRequest feeRequest, FeeResponse feeResponse, MGIContext mgiContext)
		{
			WUTransaction transaction = Get(feeRequest.TransactionId);

			string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

			if (string.IsNullOrEmpty(mgiContext.TimeZone))
				throw new MoneyTransferException(MoneyTransferException.TIME_ZONE_NOT_PROVIDE);

			FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();
			if (feeInformation != null)
			{
				transaction.GrossTotalAmount = feeInformation.TotalAmount;
				transaction.DestinationPrincipalAmount = feeInformation.ReceiveAmount;
				transaction.OriginatorsPrincipalAmount = feeInformation.Amount;
				transaction.message_charge = feeInformation.MessageFee;
				transaction.Charges = feeInformation.Fee;
				transaction.ExchangeRate = feeInformation.ExchangeRate;
				transaction.plus_charges_amount = Convert.ToDecimal(feeInformation.MetaData["PlusCharges"]);
				transaction.municipal_tax = Convert.ToDecimal(feeInformation.MetaData["MunicipalTax"]);
				transaction.state_tax = Convert.ToDecimal(feeInformation.MetaData["StateTax"]);
				transaction.county_tax = Convert.ToDecimal(feeInformation.MetaData["CountyTax"]);
				transaction.TaxAmount = Convert.ToDecimal(feeInformation.MetaData["TransferTax"]);
			}
			transaction.DestinationState = Convert.ToString(feeRequest.MetaData["StateName"]);
			transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			transaction.DTServerLastModified = DateTime.Now;

			//transaction.GrossTotalAmount = transaction.OriginatorsPrincipalAmount + transaction.Charges
			//	+ transaction.plus_charges_amount + transaction.message_charge + transaction.OtherCharges;
			// This is already calculated and comes from WU in gross_total_amount property of fee-inquiry-reply 
			transaction.GrossTotalAmount = Convert.ToDecimal(feeInformation.TotalAmount);
			transaction.TestQuestionAvaliable = feeResponse.MetaData["TestQuestionOption"].ToString();
			_wuTransactionLogRepo.UpdateWithFlush(transaction);
			
			return;
		}

		private FeeInquiry.feeinquiryrequest BuildFeeEnquiryRequest(FeeRequest feeRequest)
		{

			FeeInquiry.iso_code isoCode = new FeeInquiry.iso_code() { country_code = CountryCode, currency_code = CountryCurrencyCode };

			FeeInquiry.iso_code destinationIsoCode = new FeeInquiry.iso_code()
			{
				country_code = feeRequest.ReceiveCountryCode,
				currency_code = feeRequest.ReceiveCountryCurrency
			};

			WUAccount account = WUAccountRepo.FilterBy(c => c.Id == feeRequest.AccountId).SingleOrDefault();

			FeeInquiry.feeinquiryrequest feeInquiryRequest = new FeeInquiry.feeinquiryrequest
			{
				financials = new FeeInquiry.financials()
				{
					originators_principal_amount = ConvertDecimalToLong(feeRequest.Amount),
					originators_principal_amountSpecified = feeRequest.Amount > 0,
					destination_principal_amount = ConvertDecimalToLong(feeRequest.ReceiveAmount),
					destination_principal_amountSpecified = feeRequest.ReceiveAmount > 0
				},
				payment_details = new FeeInquiry.payment_details()
				{
					originating_country_currency = new FeeInquiry.country_currency_info() { iso_code = isoCode },
					destination_country_currency = new FeeInquiry.country_currency_info() { iso_code = destinationIsoCode },
					recording_country_currency = new FeeInquiry.country_currency_info() { iso_code = isoCode },
					transaction_type = feeRequest.Amount > 0 ? FeeInquiry.transaction_type.WMN : FeeInquiry.transaction_type.WMF,
					transaction_typeSpecified = true,
					payment_type = FeeInquiry.payment_type.Cash,
					payment_typeSpecified = true,
					fix_on_send = feeRequest.Amount > 0 ? FeeInquiry.yes_no.Y : FeeInquiry.yes_no.N
				},
				delivery_services = new FeeInquiry.delivery_services()
				{
					code = feeRequest.DeliveryService != null ? feeRequest.DeliveryService.Code : string.Empty
				},
				promotions = new FeeInquiry.promotions()
				{
					coupons_promotions = !string.IsNullOrWhiteSpace(feeRequest.PromoCode) ? feeRequest.PromoCode : string.Empty
				}
			};

			if (!string.IsNullOrWhiteSpace(feeRequest.ReceiverSecondLastName))
			{
				feeInquiryRequest.receiver = new FeeInquiry.receiver()
				{
					name = new FeeInquiry.general_name()
						{
							given_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverFirstName)
									? string.Empty
									: NexxoUtil.MassagingValue(feeRequest.ReceiverFirstName),
							paternal_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverLastName)
									? string.Empty
									: NexxoUtil.MassagingValue(feeRequest.ReceiverLastName),
							maternal_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverSecondLastName)
									? string.Empty
									: NexxoUtil.MassagingValue(feeRequest.ReceiverSecondLastName),
							name_type = FeeInquiry.name_type.M,
							name_typeSpecified = true

						}
				};
			}
			else
			{
				feeInquiryRequest.receiver = new FeeInquiry.receiver()
					{
						name = new FeeInquiry.general_name()
							{
								first_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverFirstName)
											? string.Empty
											: NexxoUtil.MassagingValue(feeRequest.ReceiverFirstName),
								last_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverLastName)
											? string.Empty
											: NexxoUtil.MassagingValue(feeRequest.ReceiverLastName),
								middle_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverMiddleName)
											? string.Empty
											: NexxoUtil.MassagingValue(feeRequest.ReceiverMiddleName),
								name_type = FeeInquiry.name_type.D,
								name_typeSpecified = true
							}
					};
			}
			if (account != null && !string.IsNullOrWhiteSpace(account.PreferredCustomerAccountNumber))
			{
				feeInquiryRequest.preferred_customer_no = account.PreferredCustomerAccountNumber;
			}


			string[] personalMessages = null;

			if (!string.IsNullOrWhiteSpace(feeRequest.PersonalMessage))
			{
				personalMessages = MessageBlockSplit(feeRequest.PersonalMessage).ToArray();
			}

			if (personalMessages != null)
			{
				int messageIndex = 0;

				FeeInquiry.messages messages = new FeeInquiry.messages();
				string[] text = new string[personalMessages.Length];
				foreach (string msg in personalMessages)
				{
					text[messageIndex] = msg;
					messages.text = text;
					messageIndex++;
				}

				feeInquiryRequest.delivery_services.message = new FeeInquiry.message_details()
				{
					message_details1 = messages
				};
			}

			return feeInquiryRequest;
		}

		private FeeResponse MapfeeEnquiryResponse(FeeInquiry.feeinquiryreply feeInqResponse)
		{
			var feeInformations = new List<FeeInformation>
			{
				new FeeInformation()
				{
					Amount = ConvertLongToDecimal(feeInqResponse.financials.originators_principal_amount),
					ReceiveAmount = ConvertLongToDecimal(feeInqResponse.financials.destination_principal_amount),
					TotalAmount = ConvertLongToDecimal(feeInqResponse.financials.gross_total_amount),
					Fee = ConvertLongToDecimal(feeInqResponse.financials.charges),
					MetaData = new Dictionary<string, object>()
					{
						{"PlusCharges", ConvertLongToDecimal(feeInqResponse.financials.plus_charges_amount)},
						{"PayAmount", ConvertLongToDecimal(feeInqResponse.financials.pay_amount)},
						{"Tolls", ConvertLongToDecimal(feeInqResponse.financials.tolls)},
						{"CanadianDollarExchangeFee", ConvertLongToDecimal(feeInqResponse.financials.canadian_dollar_exchange_fee)},
						{"BaseMessageCharge", feeInqResponse.fee_inquiry_message.base_message_charge},
						{"TotalDiscount", ConvertLongToDecimal(feeInqResponse.financials.total_discount)},
                        {"MunicipalTax", ConvertLongToDecimal(feeInqResponse.financials.taxes.municipal_tax)},
                        {"StateTax", ConvertLongToDecimal(feeInqResponse.financials.taxes.state_tax)},
                        {"CountyTax", ConvertLongToDecimal(feeInqResponse.financials.taxes.county_tax)},
                        {"TransferTax", ConvertLongToDecimal(feeInqResponse.financials.taxes.municipal_tax)+ConvertLongToDecimal(feeInqResponse.financials.taxes.state_tax)+ConvertLongToDecimal(feeInqResponse.financials.taxes.county_tax)}
					},
					ExchangeRate = Convert.ToDecimal(feeInqResponse.payment_details.exchange_rate),
					MessageFee = ConvertLongToDecimal(feeInqResponse.financials.message_charge),
					Discount = ConvertLongToDecimal(feeInqResponse.promotions.promo_discount_amount),
				}
			};

			var feeResponse = new FeeResponse()
			{
				FeeInformations = feeInformations,
				MetaData = new Dictionary<string, object>()
				{
					{"TestQuestionOption", feeInqResponse.delivery_services != null ? feeInqResponse.delivery_services.test_question_available : string.Empty},
					{"IsFixedOnSend", feeInqResponse.payment_details.fix_on_send == FeeInquiry.yes_no.Y}
				}
			};

			return feeResponse;
		}

		private long ConvertDecimalToLong(decimal amount)
		{
			return Convert.ToInt64(amount * 100);
		}

		private decimal ConvertLongToDecimal(long amount)
		{
			return Convert.ToDecimal(amount / 100m);
		}

		public ValidateResponse Validate(ValidateRequest validateRequest, MGIContext mgiContext)
		{
			try
			{
				CheckCounterId(mgiContext);
				if (string.IsNullOrEmpty(mgiContext.TimeZone))
					throw new MoneyTransferException(MoneyTransferException.TIME_ZONE_NOT_PROVIDE);
				WUTransaction transaction = Get(validateRequest.TransactionId);
				bool hasLPMTError = false;
				if (transaction != null)
				{
					if (string.IsNullOrEmpty(mgiContext.ReferenceNumber))
					{
						mgiContext.ReferenceNumber = transaction.ReferenceNo;
					}

					if (validateRequest.TransferType == MoneyTransferType.Receive)
					{

						if (string.IsNullOrEmpty(mgiContext.RMTrxType))
						{
							mgiContext.RMTrxType = ReceiveMoneyPay.mt_requested_status.HOLD.ToString();
						}

						ReceiveMoneyPay.receivemoneypayrequest receivemoneypayrequest = PopulateReceiveMoneyPayRequest(validateRequest, mgiContext);

						ReceiveMoneyPay.receivemoneypayreply reply = WUIO.ReceiveMoneyPay(receivemoneypayrequest, mgiContext);

						transaction.PaidDateTime = reply.paid_date_time;

						transaction.MTCN = reply.mtcn;
						transaction.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P.ToString();
						transaction.SenderComplianceDetailsComplianceDataBuffer = reply.receiver.compliance_details.compliance_data_buffer;

						_wuTransactionLogRepo.SaveOrUpdate(transaction);
						_wuTransactionLogRepo.Flush();
					}
					if (validateRequest.TransferType == MoneyTransferType.Send)
					{
						bool proceedWithLPMTError = Convert.ToBoolean(NexxoUtil.GetDictionaryValue(validateRequest.MetaData, "ProceedWithLPMTError"));

						if (!proceedWithLPMTError)
						{
							SendMoneyValidation.sendmoneyvalidationrequest sendMoneyValidationRequest = MapSendMoneyValidateRequest(validateRequest, transaction);
							if (sendMoneyValidationRequest != null)
							{
								sendMoneyValidationRequest.swb_fla_info = AutoMapper.Mapper.Map<SwbFlaInfo, SendMoneyValidation.swb_fla_info>(WuCommon.BuildSwbFlaInfo(mgiContext));
								sendMoneyValidationRequest.swb_fla_info.fla_name = AutoMapper.Mapper.Map<GeneralName, SendMoneyValidation.general_name>(WuCommon.BuildGeneralName(mgiContext));
								SendMoneyValidation.sendmoneyvalidationreply response = WUIO.SendMoneyValidate(sendMoneyValidationRequest, mgiContext);
								if (response != null)
								{
									UpdateTrx(validateRequest.TransactionId, response, validateRequest, mgiContext.TimeZone);

								}
							}
						}

						mgiContext.SMTrxType = SendMoneyStore.mt_requested_status.HOLD.ToString();

						hasLPMTError = Commit(validateRequest.TransactionId, mgiContext);
					}
				}
				return new ValidateResponse() { TransactionId = validateRequest.TransactionId, HasLPMTError = hasLPMTError };

			}
			catch (Exception ex)
			{
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);

				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<ValidateRequest>(validateRequest, "Validate", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Validate - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_VALIDATE_FAILED, ex);
			}
		}

		private receivemoneypayrequest PopulateReceiveMoneyPayRequest(ValidateRequest validateRequest, MGIContext mgiContext)
		{
			WUTransaction transaction = Get(validateRequest.TransactionId);
			WUAccount account = transaction.WUnionAccount;

			account.Address = NexxoUtil.MassagingValue(account.Address);
			account.City = NexxoUtil.MassagingValue(account.City);
			account.FirstName = NexxoUtil.MassagingValue(account.FirstName);
			account.LastName = NexxoUtil.MassagingValue(account.LastName);
			account.State = NexxoUtil.MassagingValue(account.State);
			validateRequest.Occupation = WuCommon.TrimOccupation(NexxoUtil.MassagingValue(validateRequest.Occupation));


			string issueCountry = validateRequest.PrimaryCountryCodeOfIssue;
			if (validateRequest.PrimaryIdType != null)
				if (validateRequest.PrimaryIdType.Equals("PASSPORT")
					|| validateRequest.PrimaryIdType.Equals("EMPLOYMENT AUTHORIZATION CARD (EAD)")
					|| validateRequest.PrimaryIdType.Equals("GREEN CARD / PERMANENT RESIDENT CARD")
					|| validateRequest.PrimaryIdType.Equals("MILITARY ID"))
				{
					issueCountry = GetCountryName(validateRequest.PrimaryCountryCodeOfIssue);
				}

			var countryCurrencyInfo = new ReceiveMoneyPay.country_currency_info()
			{
				iso_code = new ReceiveMoneyPay.iso_code()
				{
					country_code = CountryCode,
					currency_code = CountryCurrencyCode
				},
				country_name = CountryName
			};

			var receiveMoneyPayRequest = new receivemoneypayrequest
			{
				mtcn = transaction.MTCN,
				new_mtcn = transaction.TempMTCN,
				money_transfer_key = transaction.MoneyTransferKey,
				receiver = new ReceiveMoneyPay.receiver
				{
					name = new ReceiveMoneyPay.general_name()
					{
						first_name = account.FirstName,
						last_name = account.LastName,
						name_type = ReceiveMoneyPay.name_type.D,
						name_typeSpecified = true
					},
					address = new ReceiveMoneyPay.address
					{
						addr_line1 = account.Address,
						city = account.City,
						state = account.State,
						postal_code = account.PostalCode,
						Item = countryCurrencyInfo
					},
					compliance_details = new ReceiveMoneyPay.compliance_details()
					{
						template_id = ComplianceTemplate.RECEIVE_MONEY,
						id_details = new ReceiveMoneyPay.id_details()
						{
							id_type = !string.IsNullOrEmpty(validateRequest.PrimaryIdType) ? WuCommon.GetGovtIDType(validateRequest.PrimaryIdType) : string.Empty,
							id_number = validateRequest.PrimaryIdNumber,
							id_country_of_issue = issueCountry,
							id_place_of_issue = !string.IsNullOrEmpty(validateRequest.PrimaryIdPlaceOfIssue) ? GetStateCode(validateRequest.PrimaryIdPlaceOfIssue) : string.Empty
						},
						Current_address = new ReceiveMoneyPay.compliance_address()
						{
							addr_line1 = account.Address,
							city = account.City,
							state_code = account.State,
							postal_code = account.PostalCode,
							country = CountryCode
						},
						date_of_birth = validateRequest.DateOfBirth,
						occupation = WuCommon.TrimOccupation(NexxoUtil.MassagingValue(validateRequest.Occupation)),
						contact_phone = account.ContactPhone,
						Country_of_Birth = validateRequest.CountryOfBirthAbbr2,
						ack_flag = "X",
						third_party_details = new ReceiveMoneyPay.third_party_details() { flag_pay = "N" }
					}
				},
				delivery_services = new ReceiveMoneyPay.delivery_services()
				{
					identification_question = new ReceiveMoneyPay.identification_question()
					{
						question = transaction.TestQuestion,
						answer = transaction.TestAnswer
					}
				},
				financials = new ReceiveMoneyPay.financials()
				{
					gross_total_amount = ConvertDecimalToLong(transaction.GrossTotalAmount),
					gross_total_amountSpecified = true,
					pay_amount = ConvertDecimalToLong(transaction.AmountToReceiver),
					pay_amountSpecified = true,
					principal_amount = ConvertDecimalToLong(transaction.DestinationPrincipalAmount),
					principal_amountSpecified = true,
					charges = ConvertDecimalToLong(transaction.Charges),
					chargesSpecified = true
				},
				payment_details = new ReceiveMoneyPay.payment_details
				{
					destination_country_currency = new ReceiveMoneyPay.country_currency_info()
					{
						iso_code = new ReceiveMoneyPay.iso_code()
						{
							country_code = transaction.DestinationCountryCode,
							currency_code = transaction.DestinationCurrencyCode
						}
					},
					originating_country_currency = countryCurrencyInfo,
					original_destination_country_currency = countryCurrencyInfo,
					expected_payout_location = new ReceiveMoneyPay.expected_payout_location()
					{
						state_code = transaction.ExpectedPayoutStateCode,
						city = transaction.ExpectedPayoutCityName
					},
					exchange_rate = Convert.ToDouble(transaction.ExchangeRate),
					exchange_rateSpecified = true
				}
			};
			//Appending second_id tag if the customer has SSN/ITIN 
			if (!string.IsNullOrWhiteSpace(validateRequest.SecondIdNumber))
			{
				receiveMoneyPayRequest.receiver.compliance_details.second_id = new ReceiveMoneyPay.id_details()
				{
					id_type = !string.IsNullOrEmpty(validateRequest.SecondIdType) ? WuCommon.GetGovtIDType(validateRequest.SecondIdType) : string.Empty,
					id_number = validateRequest.SecondIdNumber,
					id_country_of_issue = !string.IsNullOrEmpty(CountryName) ? CountryName : string.Empty
				};
			}



			if (validateRequest.PrimaryIdType != null)
				if (receiveMoneyPayRequest.receiver.compliance_details.id_details != null &&
					receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue != null)
				{
					if (receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue.Equals("US")
						&&
						(validateRequest.PrimaryIdType.Equals("DRIVER'S LICENSE") ||
						 validateRequest.PrimaryIdType.Equals("U.S. STATE IDENTITY CARD")))
					{
						receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue =
						  GetCountryCode(validateRequest.PrimaryIdCountryOfIssue) + "/" + GetStateCode(validateRequest.PrimaryIdPlaceOfIssue);
					}
					else if (receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue.Equals("MX"))
					{
						receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue = "Mexico";
					}
				}

			if (!string.IsNullOrWhiteSpace(transaction.originating_city))
			{
				receiveMoneyPayRequest.payment_details.originating_city = transaction.originating_city;
			}

			if (!string.IsNullOrEmpty(mgiContext.RMTrxType) && mgiContext.RMTrxType == ReceiveMoneyPay.mt_requested_status.HOLD.ToString())
			{
				receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.HOLD;
				receiveMoneyPayRequest.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P;
			}
			if (!string.IsNullOrEmpty(mgiContext.RMTrxType) && (mgiContext.RMTrxType == ReceiveMoneyPay.mt_requested_status.RELEASE.ToString()))
			{
				receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.RELEASE;
				receiveMoneyPayRequest.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P;
			}
			else if (!string.IsNullOrEmpty(mgiContext.RMTrxType) && mgiContext.RMTrxType == ReceiveMoneyPay.mt_requested_status.CANCEL.ToString())
			{
				receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.CANCEL;
			}

			receiveMoneyPayRequest.payment_details.mt_requested_statusSpecified = true;

			receiveMoneyPayRequest.swb_fla_info = AutoMapper.Mapper.Map<SwbFlaInfo, ReceiveMoneyPay.swb_fla_info>(WuCommon.BuildSwbFlaInfo(mgiContext));
			receiveMoneyPayRequest.swb_fla_info.fla_name = AutoMapper.Mapper.Map<GeneralName, ReceiveMoneyPay.general_name>(WuCommon.BuildGeneralName(mgiContext));
			return receiveMoneyPayRequest;
		}

		private receivemoneypayrequest PopulateReceiveMoneyPayRequest(WUTransaction transaction, MGIContext mgiContext)
		{
			WUAccount account = transaction.WUnionAccount;

			var countryCurrencyInfo = new ReceiveMoneyPay.country_currency_info()
			{
				iso_code = new ReceiveMoneyPay.iso_code()
				{
					country_code = CountryCode,
					currency_code = CountryCurrencyCode
				},
				country_name = CountryName
			};

			var receiveMoneyPayRequest = new receivemoneypayrequest
			{
				mtcn = transaction.MTCN,
				new_mtcn = transaction.TempMTCN,
				money_transfer_key = transaction.MoneyTransferKey,
				receiver = new ReceiveMoneyPay.receiver
				{
					name = new ReceiveMoneyPay.general_name()
					{
						first_name = account.FirstName,
						last_name = account.LastName,
						name_type = ReceiveMoneyPay.name_type.D,
						name_typeSpecified = true
					},
					address = new ReceiveMoneyPay.address
					{
						addr_line1 = account.Address,
						city = account.City,
						state = account.State,
						postal_code = account.PostalCode,
						Item = countryCurrencyInfo
					},
					compliance_details = new ReceiveMoneyPay.compliance_details()
					{
						compliance_data_buffer = transaction.SenderComplianceDetailsComplianceDataBuffer
					}
				},
				delivery_services = new ReceiveMoneyPay.delivery_services()
				{
					identification_question = new ReceiveMoneyPay.identification_question()
					{
						question = transaction.TestQuestion,
						answer = transaction.TestAnswer
					}
				},
				financials = new ReceiveMoneyPay.financials()
				{
					gross_total_amount = ConvertDecimalToLong(transaction.GrossTotalAmount),
					gross_total_amountSpecified = true,
					pay_amount = ConvertDecimalToLong(transaction.AmountToReceiver),
					pay_amountSpecified = true,
					principal_amount = ConvertDecimalToLong(transaction.DestinationPrincipalAmount),
					principal_amountSpecified = true,
					charges = ConvertDecimalToLong(transaction.Charges),
					chargesSpecified = true
				},
				payment_details = new ReceiveMoneyPay.payment_details
				{
					destination_country_currency = new ReceiveMoneyPay.country_currency_info()
					{
						iso_code = new ReceiveMoneyPay.iso_code()
						{
							country_code = transaction.DestinationCountryCode,
							currency_code = transaction.DestinationCurrencyCode
						}
					},
					originating_country_currency = countryCurrencyInfo,
					original_destination_country_currency = countryCurrencyInfo,
					expected_payout_location = new ReceiveMoneyPay.expected_payout_location()
					{
						state_code = transaction.ExpectedPayoutStateCode,
						city = transaction.ExpectedPayoutCityName
					},
					exchange_rate = Convert.ToDouble(transaction.ExchangeRate),
					exchange_rateSpecified = true
				}
			};

			if (!string.IsNullOrWhiteSpace(transaction.originating_city))
			{
				receiveMoneyPayRequest.payment_details.originating_city = transaction.originating_city;
			}

			if (!string.IsNullOrEmpty(mgiContext.RMTrxType) && mgiContext.RMTrxType == ReceiveMoneyPay.mt_requested_status.HOLD.ToString())
			{
				receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.HOLD;
				receiveMoneyPayRequest.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P;
			}
			if (!string.IsNullOrEmpty(mgiContext.RMTrxType) && mgiContext.RMTrxType == ReceiveMoneyPay.mt_requested_status.RELEASE.ToString())
			{
				receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.RELEASE;
				receiveMoneyPayRequest.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P;
			}
			else if (!string.IsNullOrEmpty(mgiContext.RMTrxType) && mgiContext.RMTrxType == ReceiveMoneyPay.mt_requested_status.CANCEL.ToString())
			{
				receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.CANCEL;
			}

			receiveMoneyPayRequest.payment_details.mt_requested_statusSpecified = true;

			receiveMoneyPayRequest.swb_fla_info = AutoMapper.Mapper.Map<SwbFlaInfo, ReceiveMoneyPay.swb_fla_info>(WuCommon.BuildSwbFlaInfo(mgiContext));
			receiveMoneyPayRequest.swb_fla_info.fla_name = AutoMapper.Mapper.Map<GeneralName, ReceiveMoneyPay.general_name>(WuCommon.BuildGeneralName(mgiContext));
			return receiveMoneyPayRequest;
		}

		private void UpdateTrx(long transactionId, sendmoneyvalidationreply reply, ValidateRequest validateRequest, string timezone)
		{
			WUTransaction transaction = Get(transactionId);

			transaction.MTCN = reply.mtcn;
			transaction.TempMTCN = reply.new_mtcn;

			NLogger.Info(string.Format("'{0}'", transaction.DeliveryServiceDesc), "Delivery service from DB");
			if (reply.df_fields != null)
			{
				NLogger.Info(string.Format("'{0}'", reply.df_fields.delivery_service_name), "Delivery service from WU reply");
				transaction.DeliveryServiceDesc = (!string.IsNullOrWhiteSpace(reply.df_fields.delivery_service_name)) ? reply.df_fields.delivery_service_name.Trim() : transaction.DeliveryServiceDesc;
				transaction.PdsRequiredFlag = reply.df_fields.pds_required_flag == SendMoneyValidation.yes_no.Y;
				transaction.DfTransactionFlag = reply.df_fields.df_transaction_flag == SendMoneyValidation.yes_no.Y;
				//AL-571 added (PaySideTax,PaySideCharges)
				transaction.PaySideTax = Convert.ToDecimal(reply.df_fields.pay_side_tax);
				transaction.PaySideCharges = Convert.ToDecimal(reply.df_fields.pay_side_charges);

				transaction.AmountToReceiver = Convert.ToDecimal(reply.df_fields.amount_to_receiver);
			}

			transaction.municipal_tax = ConvertLongToDecimal(reply.financials.taxes.municipal_tax);
			transaction.state_tax = ConvertLongToDecimal(reply.financials.taxes.state_tax);
			transaction.county_tax = ConvertLongToDecimal(reply.financials.taxes.county_tax);
			transaction.plus_charges_amount = ConvertLongToDecimal(reply.financials.plus_charges_amount);
			transaction.message_charge = ConvertLongToDecimal(reply.financials.message_charge);
			transaction.total_discount = ConvertLongToDecimal(reply.financials.total_discount);
			transaction.total_discounted_charges = ConvertLongToDecimal(reply.financials.total_discounted_charges);
			transaction.total_undiscounted_charges = ConvertLongToDecimal(reply.financials.total_undiscounted_charges);
			transaction.PromoCodeDescription = (reply.promotions.promo_code_description);
			transaction.PromoName = (reply.promotions.promo_name);
			transaction.PromoMessage = (reply.promotions.promo_message);
			transaction.PromotionDiscount = ConvertLongToDecimal(reply.promotions.promo_discount_amount);
			transaction.PromotionsCode = (reply.promotions.sender_promo_code);
			transaction.PromotionSequenceNo = (reply.promotions.promo_sequence_no);
			transaction.GrossTotalAmount = ConvertLongToDecimal(reply.financials.gross_total_amount);

			transaction.SMSNotificationFlag = transaction.WUnionAccount.SmsNotificationFlag;

			transaction.AdditionalCharges = transaction.plus_charges_amount + transaction.message_charge;

			if (!string.IsNullOrWhiteSpace(validateRequest.PersonalMessage))
			{
				transaction.PersonalMessage = validateRequest.PersonalMessage;
			}

			transaction.FilingDate = reply.filing_date;
			transaction.FilingTime = reply.filing_time;

			transaction.instant_notification_addl_service_charges = (reply.instant_notification != null) ? reply.instant_notification.addl_service_charges : string.Empty;

			if (reply.payment_details != null)
			{
				transaction.originating_city = reply.payment_details.originating_city;
				transaction.originating_state = reply.payment_details.originating_state;
				transaction.ExpectedPayoutCityName = reply.payment_details.expected_payout_location.city;
				transaction.ExpectedPayoutStateCode = reply.payment_details.expected_payout_location.state_code;
				transaction.IsFixedOnSend = reply.payment_details.fix_on_send == SendMoneyValidation.yes_no.Y;
			}

			if (validateRequest.DeliveryService != null)
			{
				transaction.DeliveryOption = validateRequest.DeliveryService;
				NLogger.Info(string.Format("'{0}'", transaction.DeliveryServiceDesc), "Delivery services to get translations");
				transaction.TransalatedDeliveryServiceName = GetDeliveryServiceTransalation(transaction.DeliveryServiceDesc, Language);
			}

			transaction.SenderComplianceDetailsComplianceDataBuffer = reply.sender.compliance_details.compliance_data_buffer;
			transaction.TaxAmount = transaction.county_tax + transaction.municipal_tax + transaction.state_tax;
			transaction.TestQuestion = validateRequest.IdentificationQuestion;
			transaction.TestAnswer = validateRequest.IdentificationAnswer;

			_wuTransactionLogRepo.UpdateWithFlush(transaction);
		}

		public Transaction GetReceiverLastTransaction(long receiverId, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public List<Field> GetProviderAttributes(AttributeRequest attributeRequest, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		private DateTime ParseDate(string dateString)
		{
			return DateTime.ParseExact(dateString, "MMddyyyy", System.Globalization.CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// AL-90
		/// </summary>
		/// <param name="context"></param>
		private void CheckCounterId(MGIContext mgiContext)
		{
			if (!IsHardCodedCounterId)
			{
				if (string.IsNullOrEmpty(mgiContext.WUCounterId))
					throw new MoneyTransferException(MoneyTransferException.COUNTERID_NOT_FOUND);
			}
		}

		public string GetDeliveryServiceTransalation(string serviceName, string language)
		{
			try
			{
				NLogger.Info(string.Format("'{0}'", serviceName), "Delivery services to get translations");
				string transalatedName = string.Empty;

				List<DeliveryServiceTransalation> deliveryServices = WUDeliveryServiceTransalationRepo.FilterBy(x => x.EnglishName == serviceName && x.Language == language).ToList();
				NLogger.Info(string.Format("'{0}'", deliveryServices), "Delivery services");
				foreach (var deliveryService in deliveryServices)
				{
					NLogger.Info(string.Format("'{0}'", deliveryService), "Delivery services translation");
				}
				if (deliveryServices.Count() > 0)
				{
					var deliveryService = deliveryServices.FirstOrDefault();

					if (deliveryService != null)
					{
						transalatedName = deliveryService.Name;
					}
				}
				else
				{
					throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETDELIVERYSERVICETRANSALATION_FAILED);
				}
				return transalatedName;
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Service Name:" + serviceName);
				details.Add("Language:" + language);
				MongoDBLogger.ListError<string>(details, "GetDeliveryServiceTransalation", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetDeliveryServiceTransalation - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETDELIVERYSERVICETRANSALATION_FAILED, ex);
			}
		}

		public string GetCountryTransalation(string countryCode, string language)
		{
			string transalatedName = string.Empty;
			try
			{
				CountryTransalation country = WUCountryTransalationRepo.FindBy(x => x.CountryCode == countryCode && x.Language == language);
				if (country == null)
					throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCOUNTRYTRANSALATION_FAILED);

				transalatedName = country.Name;
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("County Code:" + countryCode);
				details.Add("Language:" + language);
				MongoDBLogger.ListError<string>(details, "GetCountryTransalation", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetCountryTransalation - MGI.Cxn.MoneyTransfer.WU.Impl.WUGateway", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCOUNTRYTRANSALATION_FAILED, ex);
			}
			return transalatedName;
		}

		private string GetStringFromArray(string[] stringArray)
		{
			StringBuilder message = new StringBuilder();
			foreach (var item in stringArray)
			{
				if (!string.IsNullOrWhiteSpace(item))
				{
					message.Append(item);
				}
			}
			return message.ToString();
		}
	}
}
