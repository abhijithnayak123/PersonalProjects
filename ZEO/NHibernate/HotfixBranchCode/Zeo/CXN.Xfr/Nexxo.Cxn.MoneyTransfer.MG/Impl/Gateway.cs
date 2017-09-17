using AutoMapper;
using Newtonsoft.Json;
using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.MG.AgentConnectService;
using MGI.CXN.MG.Common.Data;
using MGI.CXN.MG.Common.Impl;
using MGI.TimeStamp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using MGCommonService = MGI.CXN.MG.Common.AgentConnectService;
using MGData = MGI.Cxn.MoneyTransfer.MG.Data;
using MGI.Cxn.MoneyTransfer.MG.Contract;
using MGI.CXN.MG.Common.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.MoneyTransfer.MG.Impl
{
	public partial class Gateway : IMoneyTransfer
	{

		#region Dependencies

		public TLoggerCommon MongoDBLogger { private get; set; }

		public IRepository<MGData.Account> MGAccountRepo { private get; set; }

		public IRepository<MGData.Transaction> MGTransactionLogRepo { private get; set; }

		public IRepository<MGData.Receiver> ReceiverRepo { private get; set; }

		public IMGCommonIO MoneyGramCommonIO { private get; set; }

		public IIO IO { private get; set; }

		#endregion

		#region Private Members
		private const string OriginatingCountry = "USA";
		private const string OriginatingCurrency = "USD";
		private const string ReceivingCountry = "USA";
		public NLoggerCommon NLogger { get; set; }
		#endregion

		public Gateway()
		{
			Mapper.CreateMap<MGData.Receiver, Receiver>()
				.ForMember(x => x.PickupState_Province, s => s.MapFrom(c => c.PickupState))
				.ForMember(x => x.State_Province, s => s.MapFrom(c => c.State))
				.ForMember(x => x.Status, s => s.MapFrom(c => (c.IsActive == true) ? "Active" : ""));

			Mapper.CreateMap<Receiver, MGData.Receiver>()
				.ForMember(x => x.PickupState, s => s.MapFrom(c => c.PickupState_Province))
				.ForMember(x => x.State, s => s.MapFrom(c => c.State_Province))
				.ForMember(x => x.IsActive, s => s.MapFrom(c => (c.Status.ToLower() == "active")));

			Mapper.CreateMap<photoIdType, PhotoIDType>();

			Mapper.CreateMap<FeeInfo, FeeInformation>()
				.ForMember(d => d.ExchangeRate, s => s.MapFrom(c => c.validExchangeRate))
				.ForMember(d => d.TotalAmount, s => s.MapFrom(c => c.totalAmount))
				.ForMember(d => d.ReferenceNumber, s => s.MapFrom(c => c.mgiTransactionSessionID))
				.ForMember(d => d.ReceiveAmount, s => s.MapFrom(c => c.receiveAmounts.totalReceiveAmount))
				.ForMember(d => d.Amount, s => s.MapFrom(c => c.sendAmounts.sendAmount))
				.ForMember(d => d.Fee, s => s.MapFrom(c => c.sendAmounts.totalSendFees))
				.ForMember(d => d.Tax, s => s.MapFrom(c => c.sendAmounts.totalSendTaxes))
				.ForMember(d => d.ReceiveCurrencyCode, s => s.MapFrom(c => c.receiveAmounts.validCurrencyIndicator == true ? c.validReceiveCurrency : c.estimatedReceiveCurrency))
				.ForMember(d => d.ReceiveAgentId, s => s.MapFrom(c => c.receiveAgentID))
				.ForMember(d => d.ReceiveAgentName, s => s.MapFrom(c => c.receiveAgentName))
				.ForMember(d => d.ReceiveAgentAbbreviation, s => s.MapFrom(c => c.receiveAgentAbbreviation))
				.ForMember(d => d.Discount, s => s.MapFrom(c => c.sendAmounts.totalDiscountAmount))

				.AfterMap((s, d) =>
				 {
					 d.MetaData = new Dictionary<string, object>()
                    {
                        {"DeliveryOptionName", s.deliveryOptDisplayName},
                        {"ReceiveCountry", s.receiveCountry},
                        {"PayoutCurrencyCode", s.receiveAmounts.payoutCurrency},
						{"ValidCurrencyIndicator", s.receiveAmounts.validCurrencyIndicator},
						{"EstimatedReceiveCurrency", s.estimatedReceiveCurrency},
						{"EstimatedExchangeRate", s.estimatedExchangeRate},
						{"OtherFees", s.receiveAmounts.totalReceiveFees},
						{"OtherTaxes", s.receiveAmounts.totalReceiveTaxes},
                        {"ReceiveTaxesAreEstimated", s.receiveAmounts.receiveTaxesAreEstimated},
                        {"ReceiveFeesAreEstimated", s.receiveAmounts.receiveFeesAreEstimated},
                    };

					 d.DeliveryService = new DeliveryService()
					 {
						 Code = s.deliveryOption,
						 Name = s.deliveryOptDisplayName
					 };
				 });

			Mapper.CreateMap<FeeLookupRequest, MGCommonService.FeeLookupRequest>();
			Mapper.CreateMap<MGCommonService.FeeLookupResponse, FeeLookupResponse>();
			Mapper.CreateMap<MGCommonService.FeeInfo, FeeInfo>();
			Mapper.CreateMap<MGCommonService.SendAmountInfo, SendAmountInfo>();
			Mapper.CreateMap<MGCommonService.PromotionInfo, PromotionInfo>();
			Mapper.CreateMap<MGCommonService.EstimatedReceiveAmountInfo, EstimatedReceiveAmountInfo>();
			Mapper.CreateMap<MGCommonService.AmountInfo, AmountInfo>();
			Mapper.CreateMap<MGCommonService.TextTranslation, AgentConnectService.TextTranslation>();

			Mapper.CreateMap<BaseRequest, MGData.Transaction>();
			Mapper.CreateMap<ReferenceNumberResponse, MGData.Transaction>()
				.ForMember(d => d.ExpectedDateOfDelivery, s => s.MapFrom(c => c.expectedDateOfDelivery == DateTime.MinValue ? (DateTime?)null : c.expectedDateOfDelivery))
				.ForMember(d => d.ReceiverLastName2, s => s.MapFrom(c => c.receiverLastName2 == "/" ? null : c.receiverLastName2));

			Mapper.CreateMap<BaseRequest, ReferenceNumberRequest>();

			Mapper.CreateMap<ReferenceNumberResponse, Transaction>()
				.ForMember(d => d.SenderName, s => s.MapFrom(c => c.senderFirstName + (String.IsNullOrEmpty(c.senderMiddleName) ? "" : " " + c.senderMiddleName) + " " + c.senderLastName + " " + c.senderLastName2))
				.ForMember(d => d.DestinationPrincipalAmount, s => s.MapFrom(c => c.receiveAmount))
				.ForMember(d => d.DestinationCurrencyCode, s => s.MapFrom(c => c.receiveCurrency))
				.ForMember(d => d.OriginatingCountryCode, s => s.MapFrom(c => c.originatingCountry))
				.ForMember(d => d.ReferenceNo, s => s.MapFrom(c => c.referenceNumber))
				.ForMember(d => d.ReceiverSecondLastName, s => s.MapFrom(c => c.receiverLastName2 == "/" ? "" : c.receiverLastName2))
				.ForMember(d => d.ReceiverFirstName, s => s.MapFrom(c => c.receiverFirstName + (c.receiverMiddleName == "/" ? "" : " " + c.receiverMiddleName)));

			Mapper.CreateMap<MGData.Transaction, Account>();
			Mapper.CreateMap<MGData.Account, Account>();

			Mapper.CreateMap<Transaction, MGData.Transaction>()
				.ForMember(x => x.Recipient, s => s.MapFrom(c => c.Receiver));


			Mapper.CreateMap<MGData.Transaction, Receiver>();

			Mapper.CreateMap<MGData.Transaction, MGData.Transaction>()
				//.ForMember(x => x.Id, o => o.Ignore())
					.ForMember(x => x.rowguid, o => o.Ignore());

			Mapper.CreateMap<ReceiveValidationRequest, MGData.Transaction>();
			Mapper.CreateMap<BaseRequest, ReceiveValidationRequest>();
			Mapper.CreateMap<MGData.Transaction, ReceiveValidationRequest>();
			Mapper.CreateMap<SendReversalRequest, MGData.Transaction>();
			Mapper.CreateMap<SendReversalResponse, MGData.Transaction>();
		}

		public bool IsSWBState(string locationState)
		{
			return false;
		}

		public FeeResponse GetFee(FeeRequest request, MGIContext mgiContext)
		{

			string errorMessage = string.Empty;

			try
			{
				long transactionId = 0L;
				if (request.TransactionId == 0)
				{
					// Create new MoneyTransfer transaction
                    transactionId = _CreateTrx(request, mgiContext);
					request.TransactionId = transactionId;
				}
				else
				{
					// Updating existing MoneyTransfer transaction with new request
                    transactionId = UpdateTrx(request, mgiContext);
				}

				FeeLookupRequest feeLookupRequest = BuildFeeLookupRequest(request);

				errorMessage = string.Format("Error while get fee for {0}", request.Amount <= 0
						? request.ReceiveAmount : request.Amount);

				var commonFeeLookupRequest = Mapper.Map<MGCommonService.FeeLookupRequest>(feeLookupRequest);

                MGCommonService.FeeLookupResponse commonResponse = MoneyGramCommonIO.GetFee(commonFeeLookupRequest, mgiContext);

				var feeLookupResponse = Mapper.Map<FeeLookupResponse>(commonResponse);

				// Updating existing MoneyTransfer transaction with response
                transactionId = UpdateTrx(transactionId, feeLookupResponse, mgiContext);

				// Converting to return type
				FeeResponse feeResponse = FeeResponseMapper(feeLookupResponse);
				feeResponse.TransactionId = transactionId;

				return feeResponse;
			}
			catch (MGramProviderException ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<FeeRequest>(request, "GetFee", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetFee - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);	

				throw new MoneyTransferException(MoneyTransferException.PROVIDER_ERROR, ex.Message, ex);
			}
			catch (TimeoutException timeoutEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<FeeRequest>(request, "GetFee", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetFee - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", timeoutEx.Message, timeoutEx.StackTrace);	

				throw new Exception(errorMessage, timeoutEx);
			}
			catch (WebException webEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<FeeRequest>(request, "GetFee", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetFee - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", webEx.Message, webEx.StackTrace);	

				throw new Exception(errorMessage, webEx);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<FeeRequest>(request, "GetFee", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetFee - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);	

				errorMessage = string.Format("{0}. {1}", errorMessage, ex.Message);
				throw new Exception(errorMessage, ex);
			}
		}

		private FeeLookupRequest BuildFeeLookupRequest(FeeRequest feeRequest)
		{
			var baseRequest = PopulateBaseRequest();
			var feeLookupRequest = new FeeLookupRequest()
			{
				agentID = baseRequest.AgentID,
				agentSequence = baseRequest.AgentSequence,
				token = baseRequest.Token,
				apiVersion = baseRequest.ApiVersion,
				clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
				timeStamp = baseRequest.TimeStamp,
				allOptions = true,
				productType = productType.SEND,
				receiveCountry = feeRequest.ReceiveCountryCode,
			};

			if (!string.IsNullOrWhiteSpace(feeRequest.PromoCode))
			{
				feeLookupRequest.promoCodeValues = new string[] { feeRequest.PromoCode };
			}

			if (feeRequest.FeeRequestType == FeeRequestType.AmountExcludingFee)
			{
				feeLookupRequest.Item = feeRequest.Amount;
				feeLookupRequest.ItemElementName = ItemChoiceType.amountExcludingFee;
			}
			else if (feeRequest.FeeRequestType == FeeRequestType.ReceiveAmount)
			{
				feeLookupRequest.Item = feeRequest.ReceiveAmount;
				feeLookupRequest.ItemElementName = ItemChoiceType.receiveAmount;
			}


			return feeLookupRequest;
		}

		private FeeResponse FeeResponseMapper(FeeLookupResponse feeResponse)
		{
			var fee = new FeeResponse()
			{
				FeeInformations = Mapper.Map<List<FeeInformation>>(feeResponse.feeInfo)
			};

			return fee;
		}

		private long _CreateTrx(FeeRequest request, MGIContext mgiContext)
		{
			long transactionId = 0;
           
			string deliveryOption = string.Empty;

			MGData.Transaction mgTransaction = new MGData.Transaction();

			MGData.Account account = MGAccountRepo.FindBy(x => x.Id == request.AccountId);

			MGData.Receiver receiver = ReceiverRepo.FindBy(r => r.Id == request.ReceiverId);

			if (request.DeliveryService != null)
			{
				deliveryOption = request.DeliveryService.Name;
			}

			try
			{
				mgTransaction.rowguid = Guid.NewGuid();
				mgTransaction.DTServerCreate = DateTime.Now;
                mgTransaction.DTTerminalCreate = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
                mgTransaction.ChannelPartnerId = mgiContext.ChannelPartnerId;
				mgTransaction.ProviderId = mgiContext.ProviderId;
				mgTransaction.TransactionType = ((int)MoneyTransferType.Send).ToString();

				mgTransaction.ReceiverFirstName = request.ReceiverFirstName;
				mgTransaction.ReceiverLastName = request.ReceiverLastName;
				mgTransaction.ReceiverMiddleName = request.ReceiverMiddleName;
				mgTransaction.ReceiverLastName2 = request.ReceiverSecondLastName;
				mgTransaction.ReceiverCountry = request.ReceiveCountryCode;
				if (receiver != null)
				{
					mgTransaction.ReceiverState = receiver.State;
					mgTransaction.ReceiverCity = receiver.City;
					mgTransaction.ReceiverAddress = receiver.Address;
					mgTransaction.ReceiverPhone = receiver.PhoneNumber;
					mgTransaction.ReceiverZipCode = receiver.ZipCode;
					mgTransaction.TestQuestion = receiver.SecurityQuestion;
					mgTransaction.TestAnswer = receiver.SecurityAnswer;
					mgTransaction.IsReceiverHasPhotoId = receiver.IsReceiverHasPhotoId;
				}
				mgTransaction.SenderFirstName = account.FirstName;
				mgTransaction.SenderLastName = account.LastName;
				mgTransaction.SenderAddress = account.Address;
				mgTransaction.SenderCity = account.City;
				mgTransaction.SenderState = account.State;
				mgTransaction.SenderZipCode = account.PostalCode;
				mgTransaction.SenderHomePhone = account.ContactPhone;

				mgTransaction.Amount = request.Amount;
				mgTransaction.ReceiveAmount = request.ReceiveAmount;

				mgTransaction.DeliveryOption = deliveryOption;
				mgTransaction.DestinationCountry = request.ReceiveCountryCode;
				mgTransaction.DestinationState = NexxoUtil.GetDictionaryValueIfExists(request.MetaData, "State");

				mgTransaction.SenderCountry = OriginatingCountry;
				mgTransaction.PrimaryReceiptLanguage = ReceiptLanguage.ENG.ToString();
				mgTransaction.SecondaryReceiptLanguage = ReceiptLanguage.SPA.ToString();

				mgTransaction.ReceiveCurrency = request.ReceiveCountryCurrency;
				mgTransaction.SendCurrency = OriginatingCurrency;

				mgTransaction.AccountId = request.AccountId;
				mgTransaction.ReceiverId = request.ReceiverId;

				mgTransaction.IsDomesticTransfer = request.IsDomesticTransfer;

				MGTransactionLogRepo.AddWithFlush(mgTransaction);

				transactionId = mgTransaction.Id;
				request.TransactionId = transactionId;
			}
			catch (Exception ex)
			{
				throw new Exception("Error in money gram Transaction Create", ex);
			}

			return transactionId;
		}

		private long UpdateTrx(FeeRequest request, MGIContext mgiContext)
		{
			MGData.Transaction MGTrxlog = GetMGTransaction(request.TransactionId);
			try
			{
				MGTrxlog.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				MGTrxlog.DTServerLastModified = DateTime.Now;

				MGTrxlog.Amount = request.Amount;
				MGTrxlog.ReceiveAmount = request.ReceiveAmount;
				MGTrxlog.PromoCodeValues_PromoCode = request.PromoCode;
				MGTrxlog.IsDomesticTransfer = request.IsDomesticTransfer;
				MGTrxlog.DeliveryOption = (request.DeliveryService != null) ? request.DeliveryService.Name : string.Empty;

				MGTrxlog.RequestResponseType = (int)RequestResponseType.FeeRequest;

				MGTransactionLogRepo.UpdateWithFlush(MGTrxlog);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update", ex.InnerException);
			}
			return request.TransactionId;
		}

		public bool Commit(long transactionId, MGIContext mgiContext)
		{
			string errorMessage = "Error while committing MGI send money";
			string receiverFirstName = "";

            if (((!string.IsNullOrEmpty(mgiContext.SMTrxType) && mgiContext.SMTrxType == (MGData.MTReleaseStatus.Cancel).ToString())) || 
                (!string.IsNullOrEmpty(mgiContext.RMTrxType) && mgiContext.RMTrxType == (MGData.MTReleaseStatus.Cancel).ToString()))
            {           
				return true; // Has to revisit again once Cancel Functionality Clear.
			}
           
			try
			{
				MGData.Transaction transaction = Get(transactionId);

				if (transaction.TransactionType == ((int)MGData.TransferType.SendMoney).ToString()) //SEND MONEY
				{
					receiverFirstName = transaction.ReceiverFirstName;

					CommitTransactionRequest commitRequest = BuildCommitRequest(transactionId, mgiContext.TimeZone);

                    CommitTransactionResponse commitResponse = IO.SendCommit(commitRequest, mgiContext);

                    _UpdateTrx(transactionId, commitResponse, mgiContext.TimeZone);
				}
				else if (transaction.TransactionType == ((int)MGData.TransferType.Refund).ToString()) //SEND MONEY REFUND
				{
					errorMessage = string.Format("Error while send money refund for {0}", transaction.Amount);

					RefundRequest refundRequest = new RefundRequest();

					refundRequest.ReasonCode = transaction.SendReversalReason.ToString();
					refundRequest.RefundStatus = transaction.SendReversalType.ToString();
					refundRequest.TransactionId = transaction.Id;

                    Refund(refundRequest, mgiContext);
				}
				else  //RECEIVE MONEY
				{
					//Validation should be called before Commit as Per API Document

                    ReceiveValidationRequest validationRequest = BuildValidationRequestReceiveDuringCommit(transactionId, mgiContext.TimeZone);

					errorMessage = string.Format("Error while receive money validate for {0}", validationRequest.agentCheckAmount);

					ReceiveValidationResponse validationResponse = IO.ReceiveValidation(validationRequest);

                    _UpdateTrx(transactionId, validationResponse, mgiContext.TimeZone, true);

					if (!validationResponse.readyForCommit)
					{
						throw new MGramProviderException(string.Empty, "Additional fields Info Has to be Collected for Receive Validate");
					}

                    CommitTransactionRequest commitRequest = BuildCommitRequest(transactionId, mgiContext.TimeZone);

					CommitTransactionResponse commitResponse;

                    commitResponse = IO.ReceiveCommit(commitRequest, mgiContext);

					if (commitResponse == null)
					{
						//If commit is empty, then call Commit once more
                        commitResponse = IO.ReceiveCommit(commitRequest, mgiContext);
					}

                    _UpdateTrx(transactionId, commitResponse, mgiContext.TimeZone);
				}
			}
			catch (MGData.MGramProviderException ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Commit", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Commit - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);	

				throw new MoneyTransferException(MoneyTransferException.PROVIDER_ERROR, ex.Message, ex);
			}
			catch (TimeoutException timeoutEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Commit", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Commit - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", timeoutEx.Message, timeoutEx.StackTrace);	
				
				throw new Exception(errorMessage, timeoutEx);
			}
			catch (WebException webEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Commit", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Commit - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", webEx.Message, webEx.StackTrace);	

				throw new Exception(errorMessage, webEx);
			}
			catch (Exception ex)
			{
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);

				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Commit", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Commit - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);	

				string statusMsg = "";
                statusMsg = (!string.IsNullOrEmpty(mgiContext.SMTrxType))
                                ? mgiContext.SMTrxType
                                : (!string.IsNullOrEmpty(mgiContext.RMTrxType) ? mgiContext.RMTrxType : "");

				errorMessage = string.Format("Error in Commit Money Transfer Transaction for {0} with Status {1} : {2}", receiverFirstName, statusMsg, ex.Message);
				throw new Exception(errorMessage, ex);
			}

			return false;
		}

		public long SaveReceiver(Receiver receiver, MGIContext mgiContext)
		{
			try
			{
				MGData.Receiver moneyGramReceiver = Mapper.Map<MGData.Receiver>(receiver);

				Receiver existingReceiver = GetReceiver(receiver.Id);

				if (_isReceiverExisting(receiver))
				{
					throw new MoneyTransferException(MoneyTransferException.RECEIVER_ALREADY_EXISTED,
						"Receiver with first name and last name combination exists.");
				}
				if (existingReceiver != null)
				{
					moneyGramReceiver.rowguid = existingReceiver.rowguid;
					moneyGramReceiver.DTTerminalCreate = existingReceiver.DTTerminalCreate;
					moneyGramReceiver.DTServerCreate = existingReceiver.DTServerCreate;
                    moneyGramReceiver.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
					moneyGramReceiver.DTServerLastModified = DateTime.Now;
					ReceiverRepo.Merge(moneyGramReceiver);
				}
				else
				{
                    moneyGramReceiver.DTTerminalCreate = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
					moneyGramReceiver.DTServerCreate = DateTime.Now;
					moneyGramReceiver.rowguid = Guid.NewGuid();
					ReceiverRepo.AddWithFlush(moneyGramReceiver);
				}

				return moneyGramReceiver.Id;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<Receiver>(receiver, "SaveReceiver", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in SaveReceiver - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.SAVE_RECEIVER_FAILED, ex.Message, ex);
			}
		}

		public List<Receiver> GetFrequentReceivers(long customerId)
		{
			try
			{
				var receivers = ReceiverRepo.FilterBy(r => r.CustomerId == customerId && r.IsActive);
				receivers = receivers.OrderByDescending(x => x.DTTerminalLastModified);
				return Mapper.Map<List<Receiver>>(receivers);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerId), "GetFrequentReceivers", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetFrequentReceivers - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.UNKNOWN, ex);
			}
		}

		public List<Receiver> GetReceivers(long customerId, string lastName)
		{
			try
			{
				var receivers = ReceiverRepo.FilterBy(r => r.CustomerId == customerId && r.LastName.ToLower().Contains(lastName.ToLower()));
				return Mapper.Map<List<Receiver>>(receivers);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerId), "GetReceivers", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetReceivers - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.UNKNOWN, ex);
			}
		}

		public Receiver GetReceiver(long Id)
		{
			try
			{
				var moneyGramReceiver = ReceiverRepo.FindBy(r => r.Id == Id);
				Receiver receiver = Mapper.Map<Receiver>(moneyGramReceiver);
				return receiver;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(Id), "GetReceiver", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetReceiver - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.RECEIVER_NOT_EXISTED, ex);
			}
		}

		public Receiver GetReceiver(long customerId, string fullName)
		{
			try
			{
				var moneyGramReceiver = ReceiverRepo.FindBy(r => r.CustomerId == customerId
					&& fullName.ToLower() == (r.FirstName + " " + r.LastName).ToLower());
				Receiver receiver = Mapper.Map<Receiver>(moneyGramReceiver);
				return receiver;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerId), "GetReceiver", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetReceiver - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.RECEIVER_NOT_EXISTED, ex);
			}
		}

		public Receiver GetActiveReceiver(long Id)
		{
			try
			{
				var moneyGramReceiver = ReceiverRepo.FindBy(r => r.Id == Id && r.IsActive == true);
				Receiver receiver = Mapper.Map<Receiver>(moneyGramReceiver);
				return receiver;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(Id), "GetActiveReceiver", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetActiveReceiver - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.RECEIVER_NOT_EXISTED, ex);
			}
		}

		public bool DeleteFavoriteReceiver(Receiver receiver, MGIContext mgiContext)
		{
			try
			{
				var moneyGramReceiver = ReceiverRepo.FindBy(r => r.Id == receiver.Id);
				moneyGramReceiver.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				moneyGramReceiver.IsActive = moneyGramReceiver.IsActive = receiver.Status.ToLower() == "active" ? true : false;
				moneyGramReceiver.DTServerLastModified = DateTime.Now;
				return ReceiverRepo.UpdateWithFlush(moneyGramReceiver);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<Receiver>(receiver, "DeleteFavoriteReceiver", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in DeleteFavoriteReceiver - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);
				throw new MoneyTransferException(MoneyTransferException.RECEIVER_NOT_EXISTED, ex);
			}
		}

		public long AddAccount(Account account, MGIContext mgiContext)
		{
			MGData.Account mgAccount = new MGData.Account()
			{
				rowguid = Guid.NewGuid(),
                DTTerminalCreate = Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
				DTServerCreate = DateTime.Now,
				Address = account.Address,
				City = account.City,
				ContactPhone = account.ContactPhone,
				Email = account.Email,
				FirstName = account.FirstName,
				LastName = account.LastName,
				MobilePhone = account.MobilePhone,
				PostalCode = account.PostalCode,
				LoyaltyCardNumber = account.LoyaltyCardNumber,
				State = account.State,
				SmsNotificationFlag = account.SmsNotificationFlag
			};

			MGAccountRepo.AddWithFlush(mgAccount);
			return mgAccount.Id;
		}

		public ValidateResponse Validate(ValidateRequest validateRequest, MGIContext mgiContext)
		{
			string errorMessage = string.Empty;

			try
			{
				ValidateResponse response = new ValidateResponse();

				if (validateRequest.TransferType == MoneyTransferType.Receive)
				{
                    ReceiveValidationRequest validationRequest = BuildValidationRequestReceive(validateRequest, mgiContext);

					errorMessage = string.Format("Error while receive money validate for {0}", validationRequest.agentCheckAmount);

					ReceiveValidationResponse validationResponse = IO.ReceiveValidation(validationRequest);

                    _UpdateTrx(validateRequest.TransactionId, validationResponse, mgiContext.TimeZone);

					if (!validationResponse.readyForCommit)
					{
						throw new MGramProviderException(string.Empty, "Additional fields Info Has to be Collected for Receive Validate");
					}
					response.TransactionId = validateRequest.TransactionId;
				}
				else
				{
                    SendValidationRequest validationRequest = BuildValidationRequest(validateRequest, mgiContext);

					errorMessage = string.Format("Error while send money validate for {0}", validationRequest.amount);

                    SendValidationResponse validationResponse = IO.SendValidation(validationRequest, mgiContext);

                    _UpdateTrx(validateRequest.TransactionId, validationResponse, mgiContext.TimeZone);

					if (!validationResponse.readyForCommit)
					{
						throw new MGramProviderException(string.Empty, "Additional fields Info Has to be Collected for Send Validate");
					}
					response.TransactionId = validateRequest.TransactionId;
				}

				return response;
			}
			catch (MGData.MGramProviderException ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<ValidateRequest>(validateRequest, "Validate", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Validate - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.PROVIDER_ERROR, ex.Message, ex);
			}
			catch (TimeoutException timeoutEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<ValidateRequest>(validateRequest, "Validate", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Validate - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", timeoutEx.Message, timeoutEx.StackTrace);

				throw new Exception(errorMessage, timeoutEx);
			}
			catch (WebException webEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<ValidateRequest>(validateRequest, "Validate", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Validate - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", webEx.Message, webEx.StackTrace);

				throw new Exception(errorMessage, webEx);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<ValidateRequest>(validateRequest, "Validate", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Validate - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				errorMessage = string.Format("{0}. {1}", errorMessage, ex.Message);
				throw new Exception(errorMessage, ex);
			}
		}

		private SendValidationRequest BuildValidationRequest(ValidateRequest validateRequest, MGIContext mgiContext)
		{
			MGData.Transaction transaction = GetMGTransaction(validateRequest.TransactionId);

			if (string.IsNullOrWhiteSpace(transaction.ReceiverFirstName) || validateRequest.ReceiverId > 0)
			{
				MGData.Receiver receiver = ReceiverRepo.FindBy(x => x.Id == validateRequest.ReceiverId);
				transaction.ReceiverId = validateRequest.ReceiverId;

				transaction.ReceiverFirstName = receiver.FirstName;
				transaction.ReceiverLastName = receiver.LastName;
				transaction.ReceiverMiddleName = receiver.MiddleName;
				transaction.ReceiverLastName2 = receiver.SecondLastName;
				transaction.ReceiverNickName = receiver.NickName;
				transaction.ReceiverState = receiver.State;
				transaction.ReceiverCity = receiver.City;
				transaction.ReceiverAddress = receiver.Address;
				transaction.ReceiverPhone = receiver.PhoneNumber;
				transaction.ReceiverZipCode = receiver.ZipCode;
				transaction.TestQuestion = receiver.SecurityQuestion;
				transaction.TestAnswer = receiver.SecurityAnswer;
				transaction.IsReceiverHasPhotoId = receiver.IsReceiverHasPhotoId;
			}

			var baseRequest = PopulateBaseRequest();

			var request = new SendValidationRequest()
			{
				agentID = baseRequest.AgentID,
				agentSequence = baseRequest.AgentSequence,
				token = baseRequest.Token,
				apiVersion = baseRequest.ApiVersion,
				clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
				timeStamp = baseRequest.TimeStamp,

				amount = transaction.Amount,
				destinationCountry = transaction.DestinationCountry,
				destinationState = validateRequest.State,
				deliveryOption = validateRequest.DeliveryService,
				receiveCurrency = validateRequest.ReceiveCurrency,
				mgiTransactionSessionID = transaction.MgiTransactionSessionId,

				consumerId = "0",
				formFreeStaging = false,
				primaryReceiptLanguage = transaction.PrimaryReceiptLanguage,
				secondaryReceiptLanguage = transaction.SecondaryReceiptLanguage,

				// GetAccount
				senderFirstName = transaction.SenderFirstName,
				senderLastName = transaction.SenderLastName,
				senderAddress = transaction.SenderAddress,
				senderCity = transaction.SenderCity,
				senderState = transaction.SenderState,
				senderZipCode = transaction.SenderZipCode,
				senderCountry = transaction.SenderCountry,
				senderHomePhone = transaction.SenderHomePhone,
				sendCurrency = transaction.SendCurrency,

				// Compliance Information
				senderDOB = Convert.ToDateTime(validateRequest.DateOfBirth),
				senderDOBSpecified = true,
				senderOccupation = validateRequest.Occupation,
				senderBirthCountry = validateRequest.CountryOfBirthAbbr3,

				// GetReceiver
				receiverFirstName = transaction.ReceiverFirstName,
				receiverLastName = transaction.ReceiverLastName,
				receiverLastName2 = string.IsNullOrWhiteSpace(transaction.ReceiverLastName2) ? "/" : transaction.ReceiverLastName2,
				receiverCountry = transaction.ReceiverCountry,
				receiverMiddleName = transaction.ReceiverMiddleName,

				// Promo Code 	 
				promoCodeValues = !string.IsNullOrEmpty(transaction.PromoCodeValues_PromoCode) ? new string[] { transaction.PromoCodeValues_PromoCode } : new string[] { string.Empty },

				// Compliance Information
				senderPhotoIdType = GetPhotoIdType(validateRequest.PrimaryIdType),
				senderPhotoIdTypeSpecified = true,
				senderPhotoIdCountry = GetCountryCode(validateRequest.PrimaryIdCountryOfIssue),
				senderPhotoIdNumber = validateRequest.PrimaryIdNumber
			};
			request.senderPhotoIdState = GetStateCode(validateRequest.PrimaryIdPlaceOfIssue, request.senderPhotoIdCountry);

			if (transaction.IsTestQusAndAnsRequired == "REQ")
			{
				request.testQuestion = transaction.TestQuestion;
				request.testAnswer = transaction.TestAnswer;
			}
			else if (transaction.IsTestQusAndAnsRequired == "OPT")
			{

				if (transaction.TestQuestion != null && transaction.TestAnswer != null)
				{
					request.testQuestion = transaction.TestQuestion;
					request.testAnswer = transaction.TestAnswer;
				}
			}

			if (!string.IsNullOrWhiteSpace(validateRequest.SecondIdNumber))
			{
				// Compliance Information
				request.senderLegalIdType = legalIdType.SSN;
				request.senderLegalIdTypeSpecified = true;
				request.senderLegalIdNumber = validateRequest.SecondIdNumber;
			}

			if (!string.IsNullOrWhiteSpace(validateRequest.PersonalMessage))
			{
				List<string> messages = validateRequest.PersonalMessage.Split(40).ToList();

				if (messages.Count > 0)
				{
					request.messageField1 = messages[0];
					if (messages.Count > 1)
					{
						request.messageField2 = messages[1];
					}
				}
			}

			if (validateRequest.MetaData != null)
			{
				foreach (var item in validateRequest.MetaData)
				{
					if (request.GetType().GetProperty(item.Key) != null)
					{
						System.Reflection.PropertyInfo propertyInfo = request.GetType().GetProperty(item.Key);
						Type propertyType = propertyInfo.PropertyType;

						var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

						propertyInfo.SetValue(request, Convert.ChangeType((Convert.ChangeType(item.Value, targetType)), propertyType), null);
					}
				}

				if (validateRequest.FieldValues != null)
				{
					if (validateRequest.FieldValues != null && validateRequest.FieldValues.Count > 0)
					{
						var dynamicFields = validateRequest.FieldValues.Select(item => new KeyValuePair()
						{
							xmlTag = item.Key,
							fieldValue = item.Value
						}).ToArray();

						request.fieldValues = dynamicFields;
					}
				}
			}

			if (request.senderAddress.Length > 30)
				request.senderAddress = request.senderAddress.Substring(0, 29);

            string deliveryServiceDisplayName = GetDeliveryServiceName(mgiContext, request.deliveryOption);

			try
			{
                transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				transaction.DTServerLastModified = DateTime.Now;
				transaction.AgentID = request.agentID;
				transaction.AgentSequence = request.agentSequence;
				transaction.Token = request.token;
				transaction.ApiVersion = request.apiVersion;
				transaction.ClientSoftwareVersion = request.clientSoftwareVersion;
				transaction.RequestTimeStamp = request.timeStamp;

				transaction.Amount = request.amount;
				transaction.DestinationCountry = request.destinationCountry;
				transaction.DestinationState = request.destinationState;
				transaction.DeliveryOption = request.deliveryOption;
				transaction.DeliveryOptionDesc = deliveryServiceDisplayName;
				transaction.ReceiveCurrency = request.receiveCurrency;
				transaction.MgiTransactionSessionId = request.mgiTransactionSessionID;

				transaction.FeeAmount = request.feeAmount;
				transaction.AccountNumber = request.accountNumber;

				transaction.ConsumerId = request.consumerId;
				transaction.FormFreeStaging = request.formFreeStaging;
				transaction.PrimaryReceiptLanguage = request.primaryReceiptLanguage;
				transaction.SecondaryReceiptLanguage = request.secondaryReceiptLanguage;

				if (validateRequest.MetaData.ContainsKey("ReceiveAgentAbbreviation"))
					transaction.ReceiveAgentAbbreviation = validateRequest.MetaData["ReceiveAgentAbbreviation"].ToString();

				transaction.SenderFirstName = request.senderFirstName;
				transaction.SenderMiddleName = request.senderMiddleName;
				transaction.SenderLastName = request.senderLastName;
				transaction.SenderLastName2 = request.senderLastName2;
				transaction.SenderAddress = request.senderAddress;
				transaction.SenderAddress2 = request.senderAddress2;
				transaction.SenderAddress3 = request.senderAddress3;
				transaction.SenderCity = request.senderCity;
				transaction.SenderState = request.senderState;
				transaction.SenderZipCode = request.senderZipCode;
				transaction.SenderCountry = request.senderCountry;
				transaction.SenderHomePhone = request.senderHomePhone;

				// GetReceiver
				transaction.ReceiverFirstName = request.receiverFirstName;
				transaction.ReceiverMiddleName = request.receiverMiddleName;
				transaction.ReceiverLastName = request.receiverLastName;
				transaction.ReceiverLastName2 = request.receiverLastName2 == "/" ? null : request.receiverLastName2;
				transaction.ReceiverAddress2 = request.receiverAddress2;
				transaction.ReceiverAddress3 = request.receiverAddress3;
				transaction.Direction1 = request.direction1;
				transaction.Direction2 = request.direction2;
				transaction.Direction3 = request.direction3;
				transaction.ReceiverCountry = request.receiverCountry;

				transaction.TestQuestion = request.testQuestion;
				transaction.TestAnswer = request.testAnswer;
				transaction.MessageField1 = request.messageField1;
				transaction.MessageField2 = request.messageField2;
				transaction.ReceiveAgentID = request.receiveAgentID;
				// Promo Code 
				if (request.promoCodeValues != null && request.promoCodeValues.Count() > 0)
					transaction.PromoCodeValues_PromoCode = request.promoCodeValues[0].ToString();

				transaction.SenderPhotoIdType = request.senderPhotoIdType.ToString();
				transaction.SenderPhotoIdNumber = request.senderPhotoIdNumber;
				transaction.SenderPhotoIdState = request.senderPhotoIdState;
				transaction.SenderPhotoIdCountry = request.senderPhotoIdCountry;
				transaction.SenderLegalIdType = request.senderLegalIdType.ToString();
				transaction.SenderLegalIdNumber = request.senderLegalIdNumber;

				transaction.SenderDOB = request.senderDOB;
				transaction.SenderOccupation = request.senderOccupation;
				transaction.SenderBirthCity = request.senderBirthCity;
				transaction.SenderBirthCountry = request.senderBirthCountry;
				transaction.SenderPassportIssueDate = Convert.ToString(request.senderPassportIssueDate);
				transaction.SenderPassportIssueCity = request.senderPassportIssueCity;
				transaction.SenderPassportIssueCountry = request.senderPassportIssueCountry;
				transaction.SenderLegalIdIssueCountry = request.senderLegalIdIssueCountry;
				transaction.SenderEmailAddress = request.senderEmailAddress;
				transaction.SenderMobilePhone = request.senderMobilePhone;
				transaction.MarketingOptIn = Convert.ToString(request.marketingOptIn);
				transaction.PcTerminalNumber = request.pcTerminalNumber;

				transaction.SendCurrency = request.sendCurrency;
				transaction.SenderNationalityCountry = request.senderNationalityCountry;
				transaction.SenderNationalityAtBirthCountry = request.senderNationalityAtBirthCountry;
				transaction.AgentTransactionId = request.agentTransactionId;

				transaction.RequestResponseType = (int)RequestResponseType.ValidationRequest;

				if (request.fieldValues != null)
				{
					transaction.DynamicFields = JsonConvert.SerializeObject(request.fieldValues);
				}

				MGTransactionLogRepo.UpdateWithFlush(transaction);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update", ex.InnerException);
			}

			return request;
		}

		private ReceiveValidationRequest BuildValidationRequestReceive(ValidateRequest validateRequest, MGIContext mgiContext)
		{
			MGData.Transaction transaction = GetMGTransaction(validateRequest.TransactionId);

			var baseRequest = PopulateBaseRequestReceive();

			var request = new ReceiveValidationRequest()
			{
				agentID = baseRequest.AgentID,
				agentSequence = baseRequest.AgentSequence,
				token = baseRequest.Token,
				apiVersion = baseRequest.ApiVersion,
				clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
				timeStamp = baseRequest.TimeStamp,

				referenceNumber = transaction.ReceiveMoneySearchRefNo,
				receiveCurrency = validateRequest.ReceiveCurrency,
				agentCheckAmount = transaction.ReceiveAmount,
				agentCheckAmountSpecified = true,

				//Commented since these will be in Meta data
				//receiverAddress = transaction.ReceiverAddress,
				//receiverCity = transaction.ReceiverCity,
				//receiverState = transaction.DestinationState,
				//receiverZipCode = transaction.ReceiverZipCode,
				receiverCountry = ReceivingCountry,

				receiverPhotoIdType = GetPhotoIdType(validateRequest.PrimaryIdType),
				receiverPhotoIdTypeSpecified = true,
				receiverPhotoIdCountry = GetCountryCode(validateRequest.PrimaryIdCountryOfIssue),
				receiverPhotoIdNumber = validateRequest.PrimaryIdNumber,

				// Compliance Information
				receiverDOB = Convert.ToDateTime(validateRequest.DateOfBirth),
				receiverDOBSpecified = true,
				receiverOccupation = validateRequest.Occupation,
				receiverBirthCountry = GetCountryCode(validateRequest.CountryOfBirth),

				consumerId = "0",
				mgiTransactionSessionID = transaction.MgiTransactionSessionId,
				formFreeStaging = false,
			};

			request.receiverPhotoIdState = GetStateCode(validateRequest.PrimaryIdPlaceOfIssue, request.receiverPhotoIdCountry);

			if (!string.IsNullOrWhiteSpace(validateRequest.SecondIdNumber))
			{
				// Compliance Information
				request.receiverLegalIdType = legalIdType.SSN;
				request.receiverLegalIdTypeSpecified = true;
				request.receiverLegalIdNumber = validateRequest.SecondIdNumber;
			}

			if (validateRequest.MetaData != null)
			{
				foreach (var item in validateRequest.MetaData)
				{
					if (request.GetType().GetProperty(item.Key) != null)
					{
						System.Reflection.PropertyInfo propertyInfo = request.GetType().GetProperty(item.Key);
						Type propertyType = propertyInfo.PropertyType;

						var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

						propertyInfo.SetValue(request, Convert.ChangeType((Convert.ChangeType(item.Value, targetType)), propertyType), null);
					}
				}

				if (validateRequest.FieldValues != null)
				{
					if (validateRequest.FieldValues != null && validateRequest.FieldValues.Count > 0)
					{
						var dynamicFields = validateRequest.FieldValues.Select(item => new KeyValuePair()
						{
							xmlTag = item.Key,
							fieldValue = item.Value
						}).ToArray();

						request.fieldValues = dynamicFields;
					}
				}
			}

			if (request.receiverAddress.Length > 30)
				request.receiverAddress = request.receiverAddress.Substring(0, 29);

			try
			{
				Mapper.Map(request, transaction);

				transaction.RequestResponseType = (int)RequestResponseType.ValidationRequest;

                transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				transaction.DTServerLastModified = DateTime.Now;
				transaction.RequestTimeStamp = request.timeStamp;

				if (request.fieldValues != null)
				{
					transaction.DynamicFields = JsonConvert.SerializeObject(request.fieldValues);
				}

				MGTransactionLogRepo.UpdateWithFlush(transaction);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update", ex.InnerException);
			}

			return request;
		}

		private ReceiveValidationRequest BuildValidationRequestReceiveDuringCommit(long transactionId, string timezone)
		{
			ReceiveValidationRequest validationRequest = new ReceiveValidationRequest();
			MGData.Transaction transaction = Get(transactionId);

			Mapper.Map(transaction, validationRequest);
			validationRequest.agentCheckAmountSpecified = true;
			validationRequest.timeStamp = DateTime.Now;

			transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
			transaction.DTServerLastModified = DateTime.Now;
			transaction.RequestTimeStamp = validationRequest.timeStamp;
			transaction.RequestResponseType = (int)RequestResponseType.ValidationRequestDuringCommit;

			MGTransactionLogRepo.UpdateWithFlush(transaction);

			return validationRequest;
		}

		private string GetDeliveryServiceName(MGIContext mgiContext, string deliveryOptionCode)
		{
			string deliveryServiceDisplayName = string.Empty;

			if (!string.IsNullOrWhiteSpace(deliveryOptionCode))
			{
				DeliveryServiceRequest deliveryServiceRequest = new DeliveryServiceRequest();

                var deliveryServices = GetDeliveryServices(deliveryServiceRequest, mgiContext);

				if (deliveryServices != null && deliveryServices.Count > 0)
				{
					var deliveryService = deliveryServices.FirstOrDefault(d => d.Code == deliveryOptionCode);
					if (deliveryService != null)
					{
						deliveryServiceDisplayName = deliveryService.Name;
					}
				}
			}

			return deliveryServiceDisplayName;
		}

		private static bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
		}

		public Transaction GetTransaction(TransactionRequest transactionRequest, MGIContext mgiContext)
		{
			bool cashToAccountTrans = false;

			long transactionId = transactionRequest.TransactionId;

			if (transactionRequest.TransactionRequestType == TransactionRequestType.ReceiveTransaction)
			{
                return GetReceiveMoneyTransaction(transactionRequest, mgiContext);
			}

			MGData.Transaction mgtransaction = MGTransactionLogRepo.FindBy(x => x.Id == transactionId);


			//Get the reversal reason desc for Refund
			string reversalReason = "", reversalReasonCode = "";


			if (mgtransaction.TransactionType == ((int)MoneyTransferType.Refund).ToString() && mgtransaction.SendReversalType != (int)Data.SendReversalType.C)
			{
				reversalReasonCode = Enum.GetName(typeof(sendReversalReasonCode), mgtransaction.SendReversalReason);

				if (!string.IsNullOrEmpty(reversalReasonCode))
				{
					ReasonRequest reasonRequest = new ReasonRequest();
					reasonRequest.TransactionType = MoneyTransferType.Refund.ToString().ToUpper();

                    List<Reason> reasons = GetRefundReasons(reasonRequest, mgiContext);
					if (reasons.FirstOrDefault(t => t.Code == reversalReasonCode) != null)
					{
						reversalReason = reasons.FirstOrDefault(t => t.Code == reversalReasonCode).Name;
					}
				}
			}

			if (mgtransaction.DeliveryOption != null)
			{
				if (mgtransaction.DeliveryOption == "HDS_USD" || mgtransaction.DeliveryOption == "HDS_LOCAL" || mgtransaction.DeliveryOption == "HOME_DELIVERY"
						|| mgtransaction.DeliveryOption == "CARD_DEPOSIT" || mgtransaction.DeliveryOption == "BANK_DEPOSIT")
					cashToAccountTrans = true;
			}

			Transaction transaction = new Transaction()
			{
				Account = new Account()
				{
					FirstName = mgtransaction.SenderFirstName,
					LastName = mgtransaction.SenderLastName,
					Address = mgtransaction.SenderAddress,
					City = mgtransaction.SenderCity,
					State = mgtransaction.SenderState,
					ContactPhone = mgtransaction.SenderHomePhone,
					PostalCode = mgtransaction.SenderZipCode
				},
				Receiver = new Receiver()
				{
					FirstName = mgtransaction.ReceiverFirstName,
					LastName = mgtransaction.ReceiverLastName,
					MiddleName = mgtransaction.ReceiverMiddleName,
					SecondLastName = mgtransaction.ReceiverLastName2,
					NickName = mgtransaction.ReceiverNickName,
					Country = mgtransaction.ReceiverCountry,
					Id = mgtransaction.ReceiverId,
					City = mgtransaction.ReceiverCity,
					State_Province = mgtransaction.ReceiverState,
					ZipCode = mgtransaction.ReceiverZipCode,
					SecurityQuestion = mgtransaction.TestQuestion,
					SecurityAnswer = mgtransaction.TestAnswer,
					PhoneNumber = mgtransaction.ReceiverPhone,
					IsReceiverHasPhotoId = mgtransaction.IsReceiverHasPhotoId
				},
				Fee = mgtransaction.TransactionType == ((int)MGData.TransferType.Refund).ToString() ? mgtransaction.FeeAmount : mgtransaction.TotalSendFees,
				TransactionAmount = mgtransaction.TransactionType == ((int)MGData.TransferType.ReceiveMoney).ToString() ? mgtransaction.ReceiveAmount : mgtransaction.Amount,
				PromotionsCode = mgtransaction.PromoCodeValues_PromoCode,
				TransactionID = mgtransaction.Id.ToString(),
				TransactionType = mgtransaction.TransactionType,
				ConfirmationNumber = mgtransaction.ReferenceNumber,
				DestinationCountryCode = mgtransaction.DestinationCountry,
				DestinationCurrencyCode = mgtransaction.ReceiveCurrency,
				DestinationPrincipalAmount = mgtransaction.ReceiveAmount,
				DestinationState = mgtransaction.DestinationState,
				ExchangeRate = mgtransaction.ExchangeRate,
				PromotionDiscount = Convert.ToDecimal(mgtransaction.PromotionDiscountAmount),
				TaxAmount = Convert.ToDecimal(mgtransaction.TaxAmount),

				OriginatingCountryCode = mgtransaction.TransactionType ==
					 ((int)MGData.TransferType.SendMoney).ToString() || mgtransaction.TransactionType == ((int)MGData.TransferType.Refund).ToString() ?
					 mgtransaction.SenderCountry : mgtransaction.OriginatingCountry,

				OriginatingCurrencyCode = mgtransaction.SendCurrency,
				IsDomesticTransfer = mgtransaction.IsDomesticTransfer,
				GrossTotalAmount = mgtransaction.TransactionType == ((int)MGData.TransferType.Refund).ToString() ? mgtransaction.RefundTotalAmount : mgtransaction.TotalAmountToCollect,
				SenderName = mgtransaction.SenderFirstName + ' ' + mgtransaction.SenderLastName,
				ExpectedPayoutStateCode = mgtransaction.DestinationState,
				TestQuestion = mgtransaction.TestQuestion,
				DeliveryServiceName = mgtransaction.DeliveryOption,
				DTAvailableForPickup = mgtransaction.ExpectedDateOfDelivery,
				ReceiverFirstName = mgtransaction.ReceiverFirstName,
				ReceiverLastName = mgtransaction.ReceiverLastName,
				AmountToReceiver = mgtransaction.ReceiveAmount,
				PersonalMessage = mgtransaction.MessageField1 + (string.IsNullOrEmpty(mgtransaction.MessageField2) ? "" : mgtransaction.MessageField2),
				DeliveryServiceDesc = mgtransaction.DeliveryOptionDesc,
				ReferenceNo = mgtransaction.ReferenceNumber,
				ProviderId = mgtransaction.ProviderId,
				ChannelPartnerId = mgtransaction.ChannelPartnerId,
				TestAnswer = mgtransaction.TestAnswer,
				ReceiverSecondLastName = mgtransaction.ReceiverLastName2,
				ReceiveAgentID = mgtransaction.ReceiveAgentID,
				SenderState = mgtransaction.SenderState,
				TransactionSubType = mgtransaction.TransactionSubType,
				OriginalTransactionID = mgtransaction.OriginalTransactionID,
				IsModifiedOrRefunded = MGTransactionLogRepo.All().Any(x => x.OriginalTransactionID == transactionId) ? true : false,
				MetaData = new Dictionary<string, object>()
				{
					{"TollFreePhoneNumber", mgtransaction.TollFreePhoneNumber},
					{"FreePhoneCallPIN", mgtransaction.FreePhoneCallPIN},
					{"IsFixOnSend", false},
					{"AccountNumberLastFour", mgtransaction.AccountNumberLastFour},
					{"AccountNickName", mgtransaction.AccountNickname},
					{"PayoutCurrency", mgtransaction.PayoutCurrency},
					{"CustomerReceiveNumber", mgtransaction.CustomerReceiveNumber},
					{"PartnerConfirmationNumber", mgtransaction.PartnerConfirmationNumber},
					{"ReceiveFeeDisclosureText", mgtransaction.ReceiveFeeDisclosureText},
					{"ValidCurrencyIndicator", mgtransaction.ValidCurrencyIndicator},
					{"DisclosureTextPrimaryLanguage", mgtransaction.DisclosureText_PrimaryLanguage},
					{"TransactionDate",String.Format("{0:MMM dd, yyyy HH:mm tt}", mgtransaction.DTTerminalLastModified)},
					{"OtherFees", mgtransaction.TotalReceiveFees},
					{"OtherTaxes", mgtransaction.TotalReceiveTaxes},
                    {"TotalReceiveAmount", mgtransaction.TotalReceiveAmount},
                    {"ReceiverAddress", mgtransaction.ReceiverAddress + (string.IsNullOrEmpty(mgtransaction.ReceiverAddress2) ? "" : mgtransaction.ReceiverAddress2)}, 
                    {"ReceiverDOB", Convert.ToDateTime(mgtransaction.ReceiverDOB).ToShortDateString()},
                    {"ReceiverOccupation", mgtransaction.ReceiverOccupation},
                    {"ReceiverPhotoIdType", mgtransaction.ReceiverPhotoIdType},
                    {"ReceiverPhotoIdNumber", mgtransaction.ReceiverPhotoIdNumber},
                    {"ReceiverPhotoIdState", mgtransaction.ReceiverPhotoIdState},
                    {"ReceiverPhotoIdCountry", mgtransaction.ReceiverPhotoIdCountry},
                    {"ReceiverLegalIdType", mgtransaction.ReceiverLegalIdType},
                    {"ReceiverLegalIdNumber", mgtransaction.ReceiverLegalIdNumber},
                    {"TextTranslationPrimary", mgtransaction.ReceiptTextInfo_PrimaryLanguage},
                    {"TextTranslationSecondary", mgtransaction.ReceiptTextInfo_SecondaryLanguage},
                    {"DateTimeSent", mgtransaction.DateTimeSent},
                    {"SendReversalReason", reversalReason},
                    {"OperatorName", mgtransaction.OperatorName},
                    {"RefundFaceAmount", mgtransaction.RefundFaceAmount},
                    {"RefundFeeAmount", mgtransaction.RefundFeeAmount},
                    {"RefundTotalAmount", mgtransaction.RefundTotalAmount},
                    {"SenderPhotoIdType", mgtransaction.SenderPhotoIdType},
                    {"SenderPhotoIdNumber", mgtransaction.SenderPhotoIdNumber},
                    {"SenderPhotoIdState", mgtransaction.SenderPhotoIdState},
                    {"SenderPhotoIdCountry", mgtransaction.SenderPhotoIdCountry},
                    {"SenderLegalIdType", mgtransaction.SenderLegalIdType},
                    {"SenderLegalIdNumber", mgtransaction.SenderLegalIdNumber},
                    {"SenderDOB", mgtransaction.SenderDOB},
                    {"SenderOccupation", mgtransaction.SenderOccupation},
                    {"ReceiveTaxesAreEstimated", mgtransaction.ReceiveTaxesAreEstimated},
                    {"ReceiveFeesAreEstimated", mgtransaction.ReceiveFeesAreEstimated},
                    {"ReceiveAgentName", mgtransaction.ReceiveAgentName}, 
                    {"ReceiveAgentAbbreviation", mgtransaction.ReceiveAgentAbbreviation},
                    {"SenderMiddleName", mgtransaction.SenderMiddleName},
                    {"SenderLastName2", mgtransaction.SenderLastName2},
                    {"CashToAccountTrans", cashToAccountTrans}
				},
			};

			//Dodd-Frank Information
			if (mgtransaction.TransactionType == ((int)MoneyTransferType.Send).ToString())
			{
				string regulatorInfoStateCode = string.Empty;
				if (!string.IsNullOrEmpty(mgiContext.RegulatorInfoStateCode))
				{
					regulatorInfoStateCode = mgiContext.RegulatorInfoStateCode.ToString();
				}
				else
				{
					regulatorInfoStateCode = mgtransaction.SenderState;
				}

				var stateRegulators = StateRegulatorRepo.FilterBy(s => s.Dfjurisdiction == regulatorInfoStateCode);
				if (stateRegulators != null)
				{
					var doddFrankStateRegulator = stateRegulators.FirstOrDefault();
					if (doddFrankStateRegulator != null)
					{
						transaction.MetaData.Add("StateRegulatorName", doddFrankStateRegulator.Translation);
						transaction.MetaData.Add("StateRegulatorPhone", doddFrankStateRegulator.Stateregulatorphone);
						transaction.MetaData.Add("StateRegulatorURL", doddFrankStateRegulator.Stateregulatorurl);
					}
				}

			}

			return transaction;
		}

		public Transaction GetReceiverLastTransaction(long receiverId, MGIContext mgiContext)
		{
			var mgtransactions = MGTransactionLogRepo.FilterBy(r => r.ReceiverId == receiverId && r.ReferenceNumber != null).OrderByDescending(t => t.DTTerminalCreate).FirstOrDefault();

			if (mgtransactions == null)
				return null;

			TransactionRequest transactionRequest = new TransactionRequest()
			{
				TransactionId = mgtransactions.Id
			};

			Transaction mgtransaction = GetTransaction(transactionRequest, mgiContext);
			MGData.Receiver cxnReceiver = ReceiverRepo.FindBy(x => x.Id == mgtransaction.Receiver.Id);
			Receiver receiver = new Receiver()
							  {
								  FirstName = cxnReceiver.FirstName,
								  LastName = cxnReceiver.LastName,
								  Country = cxnReceiver.Country,
								  Id = cxnReceiver.Id,
								  SecondLastName = cxnReceiver.SecondLastName,
								  City = cxnReceiver.City,
								  State_Province = cxnReceiver.State,
								  ZipCode = cxnReceiver.ZipCode,
								  SecurityQuestion = cxnReceiver.SecurityQuestion,
								  SecurityAnswer = cxnReceiver.SecurityAnswer,
								  IsReceiverHasPhotoId = cxnReceiver.IsReceiverHasPhotoId,
								  NickName = cxnReceiver.NickName,
								  PhoneNumber = cxnReceiver.PhoneNumber,
								  MiddleName = cxnReceiver.MiddleName
							  };
			mgtransaction.Receiver = receiver;
			return mgtransaction;

		}

		public void UpdateGoldCardPoints(long transactionId, string totalPointsEarned, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public long UpdateAccount(Account account, MGIContext mgiContext)
		{
   
			MGData.Account wuAccount = MGAccountRepo.FindBy(x => x.rowguid == account.rowguid);

			wuAccount.rowguid = account.rowguid;
			wuAccount.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			wuAccount.DTServerLastModified = DateTime.Now;
			wuAccount.Address = account.Address;
			wuAccount.City = account.City;
			wuAccount.ContactPhone = account.ContactPhone;
			wuAccount.Email = account.Email;
			wuAccount.FirstName = account.FirstName;
			wuAccount.LastName = account.LastName;
			wuAccount.MobilePhone = account.MobilePhone;
			wuAccount.PostalCode = account.PostalCode;
			wuAccount.State = account.State;
			wuAccount.SmsNotificationFlag = account.SmsNotificationFlag;
			wuAccount.LoyaltyCardNumber = account.LoyaltyCardNumber;
			MGAccountRepo.Update(wuAccount);
			return wuAccount.Id;
		}

        public List<Field> GetProviderAttributes(AttributeRequest attributeRequest, MGIContext mgiContext)
        {
            string errorMessage = "Error while retreiving dynamic fields";
            var fields = new List<Field>();
            var fields2 = new List<Field>();
            

			List<string> standardFields;

			if (attributeRequest.TransferType == MoneyTransferType.Receive)
			{
				standardFields = new List<string>
                {
                    "operatorName", "referenceNumber", "pin", "receiveCurrency", "agentCheckNumber", "agentCheckType", "agentCheckAmount", "customerCheckNumber",
                    "customerCheckType", "customerCheckAmount", "receiverAddress",  "receiverCity", "receiverState", "receiverZipCode", "receiverCountry",  
                    "receiverPhotoIdType",  "receiverPhotoIdNumber", "receiverPhotoIdState", "reeceiverPhotoIdCountry", "receiverLegalIdType", "receiverLegalIdNumber", 
                    "receiverDOB",  "receiverOccupation", "receiverBirthCountry", "cardExpirationMonth", "cardExpirationYear", "cardSwiped", "consumerId", 
                    "mgiTransactionSessionID", "formFreeStaging", "receiverPhone"
                };
			}
			else
			{
				standardFields = new List<string>
			    {
				    "amount", "destinationCountry", "destinationState", "deliveryOption", "receiveCurrency", "receiveAgentID", "senderFirstName", "senderLastName", 
                    "senderAddress", "senderCity", "senderState", "senderZipCode", "senderCountry", "senderHomePhone", "receiverFirstName", "receiverLastName", 
                    "receiverLastName2", "receiverCountry", "senderPhotoIdType", "senderPhotoIdNumber", "senderPhotoIdState", "senderPhotoIdCountry", 
                    "senderLegalIdType", "senderLegalIdNumber", "senderDOB", "senderOccupation", "senderBirthCountry ", "sendCurrency", "consumerId", 
                    "mgiTransactionSessionID", "formFreeStaging", "primaryReceiptLanguage", "secondaryReceiptLanguage", "promoCode", "senderMiddleName", 
                    "receiverMiddleName", "senderAddress2", "senderLastName2", "feeAmount",  "messageField1", "messageField2", "testQuestion", "testAnswer",
                    "customerReceiveNumber","IsTestQusAndAnsRequired"
			    };
			}

			try
			{
				CXN.MG.Common.AgentConnectService.GetFieldsForProductRequest getFieldsForProductRequest;

				if (attributeRequest.TransferType == MoneyTransferType.Receive) //Identify whether it's receive or send money
				{
					getFieldsForProductRequest = BuildGetFieldsForProductRequestForReceive(attributeRequest);
				}
				else
				{
					getFieldsForProductRequest = BuildGetFieldsForProductRequest(attributeRequest);
				}

				CXN.MG.Common.AgentConnectService.GetFieldsForProductResponse response = MoneyGramCommonIO.GetFieldsForProduct(getFieldsForProductRequest, mgiContext);

				fields = (from productField in response.productFieldInfo
						  where productField.visibility == CXN.MG.Common.AgentConnectService.ProductFieldInfoVisibility.REQ
								  || productField.visibility == CXN.MG.Common.AgentConnectService.ProductFieldInfoVisibility.OPT
						  where !standardFields.Contains(productField.xmlTag)
						  select new Field
						  {
							  Label = productField.fieldLabel,
							  IsMandatory = productField.visibility == MGCommonService.ProductFieldInfoVisibility.REQ,
							  TagName = productField.xmlTag,
							  DataType = productField.enumerated ? "Dropdown" : "TextBox",
							  IsDynamic = productField.dynamic,
							  MaxLength = Convert.ToInt32(productField.fieldMax),
							  Values = productField.enumerated ? productField.enumeratedValues.ToDictionary(e => e.label, e => e.value) : null,
							  RegularExpression = productField.validationRegEx
						  }).ToList();

				if (attributeRequest.TransferType == MoneyTransferType.Send) //Identify whether it's receive or send money
				{
					var productValue = (from productField in response.productFieldInfo
										where productField.visibility == CXN.MG.Common.AgentConnectService.ProductFieldInfoVisibility.REQ
												|| productField.visibility == CXN.MG.Common.AgentConnectService.ProductFieldInfoVisibility.OPT
										where standardFields.Contains(productField.xmlTag) && productField.xmlTag == "testQuestion"
										select new
										{
											Label = productField.visibility == MGCommonService.ProductFieldInfoVisibility.REQ ? "REQ" : "OPT",
											IsMandatory = productField.visibility == MGCommonService.ProductFieldInfoVisibility.REQ,
											TagName = productField.xmlTag,

										}).ToList();

					if (productValue.Count != 0 && productValue[0].TagName == "testQuestion")
					{
                        Cxn.MoneyTransfer.MG.Data.Transaction transaction = MGTransactionLogRepo.FindBy(x => x.Id == mgiContext.ChannelPartnerId);
						transaction.IsTestQusAndAnsRequired = null;
						transaction.IsTestQusAndAnsRequired = productValue[0].Label;
						MGTransactionLogRepo.UpdateWithFlush(transaction);
					}


				}
			}
			catch (MGData.MGramProviderException ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<AttributeRequest>(attributeRequest, "GetProviderAttributes", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetProviderAttributes - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.PROVIDER_ERROR, ex.Message, ex);
			}
			catch (TimeoutException timeoutEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<AttributeRequest>(attributeRequest, "GetProviderAttributes", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetProviderAttributes - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", timeoutEx.Message, timeoutEx.StackTrace);

				throw new Exception(errorMessage, timeoutEx);
			}
			catch (WebException webEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<AttributeRequest>(attributeRequest, "GetProviderAttributes", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetProviderAttributes - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", webEx.Message, webEx.StackTrace);

				throw new Exception(errorMessage, webEx);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<AttributeRequest>(attributeRequest, "GetProviderAttributes", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in GetProviderAttributes - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				errorMessage = string.Format("{0}. {1}", errorMessage, ex.Message);
				throw new Exception(errorMessage, ex);
			}

			return fields;
		}

		private CXN.MG.Common.AgentConnectService.GetFieldsForProductRequest BuildGetFieldsForProductRequest(AttributeRequest attributeRequest)
		{
			var baseRequest = PopulateBaseRequest();
			var request = new MGI.CXN.MG.Common.AgentConnectService.GetFieldsForProductRequest
			{
				agentID = baseRequest.AgentID,
				agentSequence = baseRequest.AgentSequence,
				token = baseRequest.Token,
				apiVersion = baseRequest.ApiVersion,
				clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
				timeStamp = baseRequest.TimeStamp,

				receiveCountry = attributeRequest.ReceiveCountry,
				deliveryOption = attributeRequest.DeliveryService.Code,
				thirdPartyType = CXN.MG.Common.AgentConnectService.thirdPartyType.NONE,
				receiveCurrency = attributeRequest.ReceiveCurrencyCode,
				amount = attributeRequest.Amount,
				sendCurrency = OriginatingCurrency,
				productType = CXN.MG.Common.AgentConnectService.productType.SEND,
				consumerId = "0",
			};

			if (!string.IsNullOrWhiteSpace(attributeRequest.ReceiveAgentId))
			{
				request.receiveAgentID = attributeRequest.ReceiveAgentId;
			}

			return request;
		}

		private CXN.MG.Common.AgentConnectService.GetFieldsForProductRequest BuildGetFieldsForProductRequestForReceive(AttributeRequest attributeRequest)
		{
			var baseRequest = PopulateBaseRequestReceive();

			var request = new MGI.CXN.MG.Common.AgentConnectService.GetFieldsForProductRequest
			{
				agentID = baseRequest.AgentID,
				agentSequence = baseRequest.AgentSequence,
				token = baseRequest.Token,
				apiVersion = baseRequest.ApiVersion,
				clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
				timeStamp = baseRequest.TimeStamp,

				receiveCountry = ReceivingCountry,
				thirdPartyType = CXN.MG.Common.AgentConnectService.thirdPartyType.NONE,
				receiveCurrency = attributeRequest.ReceiveCurrencyCode,
				amount = attributeRequest.Amount,
				productType = CXN.MG.Common.AgentConnectService.productType.RCV,
				consumerId = "0",
			};

			return request;
		}

		public Account GetAccount(long cxnAccountId, MGIContext mgiContext)
		{
			return Mapper.Map<MGData.Account, Account>(MGAccountRepo.FindBy(x => x.Id == cxnAccountId));
		}

		public CardDetails WUCardEnrollment(Account sender, PaymentDetails paymentDetails, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public CardLookupDetails WUCardLookup(long customerAccountId, CardLookupDetails LookupDetails, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public bool GetWUCardAccount(long customerAccountId)
		{
			return false;
		}

		public Account DisplayWUCardAccountInfo(long cxnAccountId)
		{
			throw new NotImplementedException();
		}

		public bool GetPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public bool UseGoldcard(long accountId, string WUGoldCardNumber, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public CardInfo GetCardInfo(string cardNumber, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public string GetStatus(string confirmationNumber, MGIContext mgiContext)
		{
			return string.Empty; // TO DO. This has to be changed, While code refactoring
		}

		public SearchResponse Search(SearchRequest searchRequest, MGIContext mgiContext)
		{
			var baseRequest = PopulateBaseRequest();
			DetailLookupRequest detailLookupRequest = new DetailLookupRequest()
			{
				agentID = baseRequest.AgentID,
				agentSequence = baseRequest.AgentSequence,
				token = baseRequest.Token,
				apiVersion = baseRequest.ApiVersion,
				clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
				timeStamp = baseRequest.TimeStamp,
				referenceNumber = searchRequest.ConfirmationNumber,
				includeUseData = false
			};
			DetailLookupResponse lookUpResponse = new DetailLookupResponse();
			try
			{
				lookUpResponse = IO.DetailLookupRequest(detailLookupRequest);
			}
			catch (MGramProviderException ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<SearchRequest>(searchRequest, "Search", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Search - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.PROVIDER_ERROR, ex.Message, ex);
			}

			//Set the time zone information to CST
			TimeZoneInfo timeZoneInfo;
			DateTime dateTimeSentCST, timeStampCST;
			timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
			dateTimeSentCST = TimeZoneInfo.ConvertTime(lookUpResponse.dateTimeSent, timeZoneInfo);
			timeStampCST = TimeZoneInfo.ConvertTime(lookUpResponse.timeStamp, timeZoneInfo);

			//day diff
			TimeSpan DateSpan = timeStampCST.Date.Subtract(dateTimeSentCST.Date);

			//time diff
			TimeSpan TimeSpan = timeStampCST.Subtract(dateTimeSentCST);

			string strReversalType;
			string strFeeRefund = "";

			if (DateSpan.Days == 0)
			{
				strReversalType = RefundType.FullAmount.ToString();
			}
			else
			{
				if (DateSpan.Days == 1 && TimeSpan.TotalMinutes < 30)
				{
					strReversalType = RefundType.FullAmount.ToString();
					strFeeRefund = "Y";
				}
				else
				{
					strReversalType = RefundType.PrincipalAmount.ToString();
				}
			}

			SearchResponse searchResponse = new SearchResponse()
			 {
				 FirstName = lookUpResponse.receiverFirstName,
				 LastName = lookUpResponse.receiverLastName,
				 MiddleName = lookUpResponse.receiverMiddleName,
				 SecondLastName = lookUpResponse.receiverLastName2 == "/" ? null : lookUpResponse.receiverLastName2,
				 TransactionStatus = Convert.ToString(lookUpResponse.transactionStatus),
				 RefundStatus = strReversalType,
				 FeeRefund = strFeeRefund
			 };
			return searchResponse;
		}

		public Cxn.MoneyTransfer.Data.ModifyResponse StageModify(Cxn.MoneyTransfer.Data.ModifyRequest modifySendMoney, MGIContext mgiContext)
		{

			Cxn.MoneyTransfer.Data.ModifyResponse modifySendMoneyResponse = new Cxn.MoneyTransfer.Data.ModifyResponse();

			try
			{
				MGData.Transaction trx = Get(modifySendMoney.TransactionId); // old transaction ID				

				MGData.Transaction cancelTrx = AutoMapper.Mapper.Map<MGData.Transaction, MGData.Transaction>(trx);

				mgiContext.TrxSubType =  (int)MGData.SendMoneyTransactionSubType.Cancel;

				modifySendMoneyResponse.CancelTransactionId = CreateSendMoneyTransaction(cancelTrx, mgiContext);

				MGData.Transaction modifyTrx = AutoMapper.Mapper.Map<MGData.Transaction, MGData.Transaction>(trx);

				if (modifySendMoney != null)
				{
					modifyTrx.ReceiverFirstName = modifySendMoney.FirstName;
					modifyTrx.ReceiverLastName2 = modifySendMoney.SecondLastName;
					modifyTrx.ReceiverLastName = modifySendMoney.LastName;
					modifyTrx.ReceiverMiddleName = modifySendMoney.MiddleName;
				}

                mgiContext.TrxSubType = (int)MGData.SendMoneyTransactionSubType.Modify;

				modifySendMoneyResponse.ModifyTransactionId = CreateSendMoneyTransaction(modifyTrx, mgiContext);
			}
			catch (Exception ex)
			{
				string ErrorMessage;
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);
				
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<Cxn.MoneyTransfer.Data.ModifyRequest>(modifySendMoney, "StageModify", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in StageModify - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				ErrorMessage = string.Format("Error while Initiate Send Money Modify Transaction : {0}", ex.Message);
				throw new Exception(ErrorMessage, ex);
			}

			return modifySendMoneyResponse;
		}

		public void Modify(long transactionId, MGIContext mgiContext)
		{
			string errorMessage = "Error while AmendTransaction MGI send money";
			try
			{
				AmendTransactionRequest amendTransactionRequest = BuildAmendTransactionRequest(transactionId, mgiContext);
				AmendTransactionResponse amendTransactionResponse = IO.AmendTransaction(amendTransactionRequest);

				_UpdateTrx(transactionId, amendTransactionResponse, mgiContext.TimeZone);
			}
			catch (MGData.MGramProviderException ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Modify", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Modify - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new MoneyTransferException(MoneyTransferException.PROVIDER_ERROR, ex.Message, ex);
			}
			catch (TimeoutException timeoutEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Modify", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Modify - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", timeoutEx.Message, timeoutEx.StackTrace);

				throw new Exception(errorMessage, timeoutEx);
			}
			catch (WebException webEx)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Modify", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Modify - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", webEx.Message, webEx.StackTrace);

				throw new Exception(errorMessage, webEx);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionId), "Modify", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Modify - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				errorMessage = string.Format("{0}. {1}", errorMessage, ex.Message);
				throw new Exception(errorMessage, ex);
			}
		}

		public long StageRefund(RefundRequest refundRequest, MGIContext mgiContext)
		{
			try
			{
				MGData.Transaction trx = Get(refundRequest.TransactionId);

				sendReversalReasonCode reasonCode;

				MGData.Transaction transaction = AutoMapper.Mapper.Map<MGData.Transaction, MGData.Transaction>(trx);

				transaction.rowguid = Guid.NewGuid();
				transaction.OriginalTransactionID = transaction.Id;

				if (refundRequest.RefundStatus == RefundType.PrincipalAmount.ToString() && string.IsNullOrEmpty(refundRequest.FeeRefund))
				{
					Enum.TryParse(refundRequest.ReasonCode, out reasonCode);
					transaction.SendReversalReason = Convert.ToInt16(reasonCode);
					transaction.SendReversalType = (int)RefundType.PrincipalAmount;
					transaction.FeeRefund = "N";
				}
				else if (refundRequest.RefundStatus == RefundType.FullAmount.ToString() && string.IsNullOrEmpty(refundRequest.FeeRefund)) //Fees + Principal amount will be refunded
				{
					transaction.Amount = transaction.Amount + transaction.TotalSendFees;
					transaction.SendReversalReason = null;
					transaction.SendReversalType = (int)RefundType.FullAmount;
					transaction.FeeRefund = "N";
				}
				else if (refundRequest.RefundStatus == RefundType.FullAmount.ToString() && refundRequest.FeeRefund == "Y")
				{
					transaction.Amount = transaction.Amount + transaction.TotalSendFees;
					Enum.TryParse(refundRequest.ReasonCode, out reasonCode);
					transaction.SendReversalReason = Convert.ToInt16(reasonCode);
					transaction.SendReversalType = (int)RefundType.FullAmount;
					transaction.FeeRefund = "Y";
				}

				transaction.FeeAmount = 0; //Fees for the product imposed by Nexxo, which is 0 in this case
				transaction.ReferenceNumber = refundRequest.ReferenceNumber;
                transaction.OperatorName = mgiContext.AgentName;
				transaction.RequestResponseType = (int)RequestResponseType.Stage;
				transaction.TransactionType = ((int)MGData.TransferType.Refund).ToString();
				transaction.TransactionSubType = null;

				transaction.DTTerminalCreate = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				transaction.DTServerCreate = DateTime.Now;

				MGTransactionLogRepo.AddWithFlush(transaction);

				return transaction.Id;
			}
			catch (Exception ex)
			{
				string ErrorMessage;
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);

				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<RefundRequest>(refundRequest, "StageRefund", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in StageRefund - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				ErrorMessage = string.Format("Error while Staging Send Money Refund Transaction : {0}", ex.Message);
				throw new Exception(ErrorMessage, ex);
			}
		}

		public string Refund(RefundRequest refundRequest, MGIContext mgiContext)
		{
			string errorMessage = "Error in Commit MGI Refund Transaction.";
			DetailLookupResponse lookUpResponse = new DetailLookupResponse();

			try
			{
				MGData.Transaction trx = Get(refundRequest.TransactionId);

				//Detail Look up should be called before sending the reversal request
				var baseRequest = PopulateBaseRequest();
				DetailLookupRequest detailLookupRequest = new DetailLookupRequest()
				{
					agentID = baseRequest.AgentID,
					agentSequence = baseRequest.AgentSequence,
					token = baseRequest.Token,
					apiVersion = baseRequest.ApiVersion,
					clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
					timeStamp = baseRequest.TimeStamp,
					referenceNumber = trx.ReferenceNumber,
					includeUseData = false
				};

				lookUpResponse = IO.DetailLookupRequest(detailLookupRequest);

                SendReversalRequest request = BuildSendReversalRequest(lookUpResponse, refundRequest, mgiContext, "");

				SendReversalResponse response = IO.SendReversal(request, mgiContext);

				UpdateSendMoneyReversalTransaction(refundRequest.TransactionId, response, mgiContext);

				return refundRequest.TransactionId.ToString();
			}
			catch (MGData.MGramProviderException ex)
			{
				if (ex.Code == "605.3401")
				{
					NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);
					
					//AL-3370 Transactional Log User Story
					MongoDBLogger.Error<RefundRequest>(refundRequest, "Refund", AlloyLayerName.CXN, ModuleName.SendMoney,
						"Error in Refund - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

					throw new MGData.MGramProviderException(ex.Code, ex.Message);
				}
				else
				{
					NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);
					
					//AL-3370 Transactional Log User Story
					MongoDBLogger.Error<RefundRequest>(refundRequest, "Refund", AlloyLayerName.CXN, ModuleName.SendMoney,
						"Error in Refund - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

					throw new MoneyTransferException(MoneyTransferException.PROVIDER_ERROR, ex.Message, ex);
				}
			}
			catch (Exception ex)
			{
				NLogger.Error("Error :" + ex.Message + " Stack Trace:" + ex.StackTrace);

				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<RefundRequest>(refundRequest, "Refund", AlloyLayerName.CXN, ModuleName.SendMoney,
					"Error in Refund - MGI.Cxn.MoneyTransfer.MG.Impl.Gateway", ex.Message, ex.StackTrace);

				throw new Exception(errorMessage, ex);
			}
		}

		private SendReversalRequest BuildSendReversalRequest(DetailLookupResponse lookUpResponse, RefundRequest refundRequest, MGIContext mgiContext, string reversalType)
		{
			SendReversalRequest sendReversalRequest = new SendReversalRequest();
			
			string agentName = GetAgentName(mgiContext);

			var baseRequest = PopulateBaseRequest();
			sendReversalRequest.agentID = baseRequest.AgentID;
			sendReversalRequest.agentSequence = baseRequest.AgentSequence;
			sendReversalRequest.token = baseRequest.Token;
			sendReversalRequest.apiVersion = baseRequest.ApiVersion;
			sendReversalRequest.clientSoftwareVersion = baseRequest.ClientSoftwareVersion;
			sendReversalRequest.timeStamp = baseRequest.TimeStamp;

			sendReversalRequest.sendAmount = lookUpResponse.sendAmounts.sendAmount;
			sendReversalRequest.feeAmount = lookUpResponse.sendAmounts.totalSendFees;
			sendReversalRequest.sendCurrency = lookUpResponse.sendAmounts.sendCurrency;
			sendReversalRequest.referenceNumber = lookUpResponse.referenceNumber;
			sendReversalRequest.operatorName = agentName;
			sendReversalRequest.timeStamp = DateTime.Now;

			MGData.Transaction transaction = Get(refundRequest.TransactionId);

			sendReversalReasonCode reasonCode = 0;
			Enum.TryParse(refundRequest.ReasonCode, out reasonCode);

			if (transaction.SendReversalType == (int)RefundType.PrincipalAmount && transaction.FeeRefund == "N")
			{
				sendReversalRequest.sendReversalReason = reasonCode;
				sendReversalRequest.sendReversalReasonSpecified = true;
				sendReversalRequest.reversalType = sendReversalType.R;
				sendReversalRequest.feeRefund = "N";
			}
			else if (transaction.SendReversalType == (int)RefundType.FullAmount && transaction.FeeRefund == "N")
			{
				sendReversalRequest.reversalType = sendReversalType.C;
				sendReversalRequest.feeRefund = "Y";
			}
			else if (transaction.SendReversalType == (int)RefundType.FullAmount && transaction.FeeRefund == "Y")
			{
				sendReversalRequest.reversalType = sendReversalType.R;
				sendReversalRequest.sendReversalReason = reasonCode;
				sendReversalRequest.sendReversalReasonSpecified = true;
				sendReversalRequest.feeRefund = "Y";

				transaction.SendReversalType = (int)RefundType.PrincipalAmount;
			}
			else if (transaction.SendReversalType == (int)RefundType.FullAmount && string.IsNullOrEmpty(transaction.FeeRefund))
			{
				sendReversalRequest.reversalType = sendReversalType.R;
				sendReversalRequest.feeRefund = "N";
			}

			Mapper.Map(sendReversalRequest, transaction);

			TimeZoneInfo timeZoneInfo;
			DateTime dateTimeSentCST;
			timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
			dateTimeSentCST = TimeZoneInfo.ConvertTime(lookUpResponse.dateTimeSent, timeZoneInfo);

			transaction.DateTimeSent = dateTimeSentCST;
			transaction.FeeAmount = 0;
            transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			transaction.DTServerLastModified = DateTime.Now;
			transaction.RequestTimeStamp = sendReversalRequest.timeStamp;
			transaction.RequestResponseType = (int)RequestResponseType.ReversalRequest;

			if (sendReversalRequest.sendReversalReasonSpecified)
			{
				transaction.SendReversalReason = Convert.ToInt16(reasonCode);
			}
			else
			{
				transaction.SendReversalReason = null;
			}

			MGTransactionLogRepo.UpdateWithFlush(transaction);

			return sendReversalRequest;
		}

		private void UpdateSendMoneyReversalTransaction(long transactionId, SendReversalResponse sendReversalResponse, MGIContext mgiContext)
		{
			decimal Amount = 0, FeeAmount = 0, TotalAmt = 0;

			MGData.Transaction transaction = GetMGTransaction(transactionId);
			transaction = Mapper.Map<SendReversalResponse, MGData.Transaction>(sendReversalResponse, transaction);

			if (transaction.FeeRefund == "Y")// && !sendReversalResponse.refundFeeAmountSpecified) //Fee is being refunded
			{
				FeeAmount = transaction.TotalSendFees;
				Amount = transaction.Amount - FeeAmount; //transaction.Amount includes Fees
			}
			else
			{
				FeeAmount = 0;
			}

			if (sendReversalResponse.refundFaceAmountSpecified)
				Amount = sendReversalResponse.refundFaceAmount;

			TotalAmt = Amount + FeeAmount;

			transaction.RefundFeeAmount = FeeAmount;
			transaction.RefundTotalAmount = TotalAmt;
			transaction.RefundFaceAmount = Amount;

			transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			transaction.DTServerLastModified = DateTime.Now;
			transaction.RequestResponseType = (int)RequestResponseType.ReversalResponse;

			MGTransactionLogRepo.UpdateWithFlush(transaction);
		}

		public List<MasterData> GetBannerMsgs(MGIContext mgiContext)
		{
			return new List<MasterData>();
		}

		public List<Reason> GetRefundReasons(ReasonRequest request, MGIContext mgiContext)
		{
			List<Reason> reasons = new List<Reason>();
			reasons.Add(new Reason() { Code = "NO_RCV_LOC", Name = "No Receive Location" });
			reasons.Add(new Reason() { Code = "WRONG_SERVICE", Name = "Wrong Delivery Option" });
			reasons.Add(new Reason() { Code = "NO_TQ", Name = "Missing Test Question and Answer" });
			reasons.Add(new Reason() { Code = "INCORRECT_AMT", Name = "Incorrect Amount" });
			reasons.Add(new Reason() { Code = "MS_NOT_USED", Name = "MoneyGram Rewards not used" });

			return reasons;
		}

		public List<DeliveryService> GetDeliveryServices(DeliveryServiceRequest request, MGIContext mgiContext)
		{
			var deliveryservices = new List<DeliveryService>();
			var moneyGramDeliveryOptions = DeliveryOptionRepo.All();

			if (moneyGramDeliveryOptions != null)
			{
				deliveryservices = moneyGramDeliveryOptions.Select(c => new DeliveryService() { Code = c.Deliveryoption, Name = c.Name }).ToList();
			}

			return deliveryservices;
		}

		#region Private methods

		private long GetTransactionId(MGIContext mgiContext)
		{
			long transactionId = 0;
			if (mgiContext.CxnTransactionId!=0)
			{
				transactionId = mgiContext.CxnTransactionId;
			}
			return transactionId;
		}


		private MGData.Transaction Get(long Id)
		{
			return MGTransactionLogRepo.FindBy(x => x.Id == Id);
		}

		private MGData.Transaction GetMGTransaction(long Id)
		{
			return MGTransactionLogRepo.FindBy(x => x.Id == Id);
		}



		// Update transaction with FeeResponse
		private long UpdateTrx(long transactionId, FeeLookupResponse feeResponse, MGIContext mgiContext)
		{
			MGData.Transaction transaction = GetMGTransaction(transactionId);

			try
			{
				transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				transaction.DTServerLastModified = DateTime.Now;

				transaction.DoCheckIn = feeResponse.doCheckIn;
				transaction.ResponseTimeStamp = feeResponse.timeStamp;
				transaction.Flags = feeResponse.flags;

				var feeInfo = feeResponse.feeInfo.FirstOrDefault();

				if (feeInfo != null)
				{
					transaction.ReceiveAmount = feeInfo.validReceiveAmount;
					transaction.ReceiveCurrency = feeInfo.receiveAmounts.validCurrencyIndicator == true ? feeInfo.validReceiveCurrency : feeInfo.estimatedReceiveCurrency;
					transaction.ExchangeRate = feeInfo.validExchangeRate;
					transaction.EstimatedReceiveAmount = feeInfo.estimatedReceiveAmount;
					transaction.EstimatedExchangeRate = feeInfo.estimatedExchangeRate;
					transaction.GrossTotalAmount = feeInfo.totalAmount;
					transaction.DestinationCountry = feeInfo.receiveCountry;
					transaction.DeliveryOption = feeInfo.deliveryOption;
					transaction.ReceiveAmountAltered = feeInfo.receiveAmountAltered;
					transaction.RevisedInformationalFee = feeInfo.revisedInformationalFee;
					transaction.DeliveryOptionDesc = feeInfo.deliveryOptDisplayName;
					transaction.MgiTransactionSessionId = feeInfo.mgiTransactionSessionID;
					transaction.SendAmountAltered = feeInfo.sendAmountAltered;

					if (feeInfo.promotionInfo != null && feeInfo.promotionInfo.FirstOrDefault() != null
						&& feeInfo.promotionInfo.FirstOrDefault().promotionCode != null)
					{
						var promotionInfo = feeInfo.promotionInfo.FirstOrDefault();
						transaction.PromoCodeValues_PromoCode = promotionInfo.promotionCode;
						transaction.PromotionDiscountId = promotionInfo.promotionDiscountId;
						transaction.PromotionCategoryId = promotionInfo.promotionCategoryId;
						transaction.PromotionDiscount = promotionInfo.promotionDiscount;
						transaction.PromotionDiscountAmount = promotionInfo.promotionDiscountAmount;
					}

					transaction.Amount = feeInfo.sendAmounts.sendAmount;
					transaction.SendCurrency = feeInfo.sendAmounts.sendCurrency;
					transaction.TotalSendFees = feeInfo.sendAmounts.totalSendFees;
					transaction.TotalDiscountAmount = feeInfo.sendAmounts.totalDiscountAmount;
					transaction.TotalSendTaxes = feeInfo.sendAmounts.totalSendTaxes;
					transaction.TotalAmountToCollect = feeInfo.sendAmounts.totalAmountToCollect;

					var sendAmountDetails = feeInfo.sendAmounts.detailSendAmounts;
					transaction.DetailSendAmounts_nonMgiSendTax = sendAmountDetails.Where(x => x.amountType == "nonMgiSendTax").FirstOrDefault().amount;
					transaction.DetailSendAmounts_nonMgiSendFee = sendAmountDetails.Where(x => x.amountType == "nonMgiSendFee").FirstOrDefault().amount;
					transaction.DetailSendAmounts_mgiNonDiscountedSendFee = sendAmountDetails.Where(x => x.amountType == "mgiNonDiscountedSendFee").FirstOrDefault().amount;
					transaction.DetailSendAmounts_totalNonDiscountedFees = sendAmountDetails.Where(x => x.amountType == "totalNonDiscountedFees").FirstOrDefault().amount;
					transaction.DetailSendAmounts_discountedMgiSendFee = sendAmountDetails.Where(x => x.amountType == "discountedMgiSendFee").FirstOrDefault().amount;
					transaction.DetailSendAmounts_mgiSendTax = sendAmountDetails.Where(x => x.amountType == "mgiSendTax").FirstOrDefault().amount;
					transaction.DetailSendAmounts_totalMgiCollectedFeesAndTaxes = sendAmountDetails.Where(x => x.amountType == "totalMgiCollectedFeesAndTaxes").FirstOrDefault().amount;
					transaction.DetailSendAmounts_totalAmountToMgi = sendAmountDetails.Where(x => x.amountType == "totalAmountToMgi").FirstOrDefault().amount;
					transaction.DetailSendAmounts_totalSendFeesAndTaxes = sendAmountDetails.Where(x => x.amountType == "totalSendFeesAndTaxes").FirstOrDefault().amount;
					transaction.DetailSendAmounts_totalNonMgiSendFeesAndTaxes = sendAmountDetails.Where(x => x.amountType == "totalNonMgiSendFeesAndTaxes").FirstOrDefault().amount;

					var receiveAmounts = feeInfo.receiveAmounts;
					transaction.ReceiveAmount = receiveAmounts.receiveAmount;
					transaction.ValidCurrencyIndicator = receiveAmounts.validCurrencyIndicator;
					transaction.PayoutCurrency = receiveAmounts.payoutCurrency;
					transaction.TotalReceiveFees = receiveAmounts.totalReceiveFees;
					transaction.TotalReceiveTaxes = receiveAmounts.totalReceiveTaxes;
					transaction.TotalReceiveAmount = receiveAmounts.totalReceiveAmount;
					transaction.ReceiveFeesAreEstimated = receiveAmounts.receiveFeesAreEstimated;
					transaction.ReceiveTaxesAreEstimated = receiveAmounts.receiveTaxesAreEstimated;

					if (receiveAmounts.detailEstimatedReceiveAmounts.Any())
					{
						var detailEstimatedReceiveAmounts = receiveAmounts.detailEstimatedReceiveAmounts;
						transaction.DetailEstimatedReceiveAmounts_mgiReceiveFee = detailEstimatedReceiveAmounts.Where(x => x.amountType == "mgiReceiveFee").FirstOrDefault().amount;
						transaction.DetailEstimatedReceiveAmounts_mgiReceiveTax = detailEstimatedReceiveAmounts.Where(x => x.amountType == "mgiReceiveTax").FirstOrDefault().amount;
						transaction.DetailEstimatedReceiveAmounts_nonMgiReceiveFee = detailEstimatedReceiveAmounts.Where(x => x.amountType == "nonMgiReceiveFee").FirstOrDefault().amount;
						transaction.DetailEstimatedReceiveAmounts_nonMgiReceiveTax = detailEstimatedReceiveAmounts.Where(x => x.amountType == "nonMgiReceiveTax").FirstOrDefault().amount;
					}
				}

				transaction.RequestResponseType = (int)RequestResponseType.FeeResponse;

				MGTransactionLogRepo.UpdateWithFlush(transaction);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update", ex.InnerException);
			}
			return transactionId;
		}

		// Update transaction with Send ValidationResponse
		private long _UpdateTrx(long transactionId, SendValidationResponse response, string timezone)
		{
			MGData.Transaction transaction = GetMGTransaction(transactionId);

			try
			{
				transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
				transaction.DTServerLastModified = DateTime.Now;

				transaction.DoCheckIn = response.doCheckIn;
				transaction.ResponseTimeStamp = response.timeStamp;
				transaction.Flags = response.flags;

				if (response.promotionInfo != null && response.promotionInfo.FirstOrDefault().promotionCode != null)
				{
					var PromoInfo = response.promotionInfo.FirstOrDefault();
					transaction.PromoCodeValues_PromoCode = PromoInfo.promotionCode;
					transaction.PromotionDiscountId = PromoInfo.promotionDiscountId;
					transaction.PromotionCategoryId = PromoInfo.promotionCategoryId;
					transaction.PromotionDiscount = PromoInfo.promotionDiscount;
					transaction.PromotionDiscountAmount = PromoInfo.promotionDiscountAmount;

					if (PromoInfo.promotionErrorCode != null)
					{
						transaction.PromotionErrorCode = PromoInfo.promotionErrorCode;
						if (PromoInfo.promotionErrorMessage != null && PromoInfo.promotionErrorMessage.Count() > 0)
						{
							transaction.PromotionErrorCodeMessage_PrimaryLanguage = PromoInfo.promotionErrorMessage[0].textTranslation.ToString();
							if (PromoInfo.promotionErrorMessage.Count() > 1)
								transaction.PromotionErrorCodeMessage_SecondaryLanguage = PromoInfo.promotionErrorMessage[1].textTranslation.ToString();
						}
					}
				}

				if (response.promotionalMessage != null && response.promotionalMessage.Count() > 0)
				{
					AgentConnectService.TextTranslation[] trans = new AgentConnectService.TextTranslation[response.promotionalMessage.Count()];

					if (trans[0] != null)
						transaction.PromotionalMessage_PrimaryLanguage = trans[0].textTranslation.ToString();

					if (trans[1] != null)
						transaction.PromotionalMessage_SecondaryLanguage = trans[1].textTranslation.ToString();
				}

				transaction.ReadyForCommit = response.readyForCommit;
				transaction.Amount = response.sendAmounts.sendAmount;
				transaction.SendCurrency = response.sendAmounts.sendCurrency;
				transaction.TotalSendFees = response.sendAmounts.totalSendFees;
				transaction.TotalDiscountAmount = response.sendAmounts.totalDiscountAmount;
				transaction.TotalSendTaxes = response.sendAmounts.totalSendTaxes;
				transaction.TotalAmountToCollect = response.sendAmounts.totalAmountToCollect;

				transaction.DetailSendAmounts_nonMgiSendTax = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "nonMgiSendTax").FirstOrDefault().amount;
				transaction.DetailSendAmounts_nonMgiSendFee = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "nonMgiSendFee").FirstOrDefault().amount;
				transaction.DetailSendAmounts_mgiNonDiscountedSendFee = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "mgiNonDiscountedSendFee").FirstOrDefault().amount;
				transaction.DetailSendAmounts_totalNonDiscountedFees = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "totalNonDiscountedFees").FirstOrDefault().amount;
				transaction.DetailSendAmounts_discountedMgiSendFee = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "discountedMgiSendFee").FirstOrDefault().amount;
				transaction.DetailSendAmounts_mgiSendTax = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "mgiSendTax").FirstOrDefault().amount;
				transaction.DetailSendAmounts_totalMgiCollectedFeesAndTaxes = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "totalMgiCollectedFeesAndTaxes").FirstOrDefault().amount;
				transaction.DetailSendAmounts_totalAmountToMgi = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "totalAmountToMgi").FirstOrDefault().amount;
				transaction.DetailSendAmounts_totalSendFeesAndTaxes = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "totalSendFeesAndTaxes").FirstOrDefault().amount;
				transaction.DetailSendAmounts_totalNonMgiSendFeesAndTaxes = response.sendAmounts.detailSendAmounts.Where(x => x.amountType == "totalNonMgiSendFeesAndTaxes").FirstOrDefault().amount;

				transaction.ReceiveAmount = response.receiveAmounts.receiveAmount;
				transaction.ReceiveCurrency = response.receiveAmounts.payoutCurrency; //Irrespective of validCurrencyIndicator, always use the Payout currency as the Receive Currency. (As per Hue's mail)
				transaction.ValidCurrencyIndicator = response.receiveAmounts.validCurrencyIndicator;
				transaction.PayoutCurrency = response.receiveAmounts.payoutCurrency;
				transaction.TotalReceiveFees = response.receiveAmounts.totalReceiveFees;
				transaction.TotalReceiveTaxes = response.receiveAmounts.totalReceiveTaxes;
				transaction.TotalReceiveAmount = response.receiveAmounts.totalReceiveAmount;
				transaction.ReceiveFeesAreEstimated = response.receiveAmounts.receiveFeesAreEstimated;
				transaction.ReceiveTaxesAreEstimated = response.receiveAmounts.receiveTaxesAreEstimated;

				if (response.receiveAmounts.detailReceiveAmounts.Any())
				{
					transaction.DetailReceiveAmounts_mgiReceiveFee = response.receiveAmounts.detailReceiveAmounts.Where(x => x.amountType == "mgiReceiveFee").FirstOrDefault().amount;
					transaction.DetailReceiveAmounts_mgiReceiveTax = response.receiveAmounts.detailReceiveAmounts.Where(x => x.amountType == "mgiReceiveTax").FirstOrDefault().amount;
					transaction.DetailReceiveAmounts_nonMgiReceiveFee = response.receiveAmounts.detailReceiveAmounts.Where(x => x.amountType == "nonMgiReceiveFee").FirstOrDefault().amount;
					transaction.DetailReceiveAmounts_nonMgiReceiveTax = response.receiveAmounts.detailReceiveAmounts.Where(x => x.amountType == "nonMgiReceiveTax").FirstOrDefault().amount;
				}

				transaction.ExchangeRate = response.exchangeRateApplied;
				transaction.ExchangeRateApplied = response.exchangeRateApplied;
				transaction.ReceiveFeeDisclosureText = response.receiveFeeDisclosureText;
				transaction.ReceiveTaxDisclosureText = response.receiveTaxDisclosureText;
				transaction.CustomerReceiveNumber = response.customerReceiveNumber;

				transaction.AccountNumberLastFour = response.accountNumberLastFour;
				transaction.AccountNickname = response.accountNickname;
				transaction.ReceiveAgentName = response.receiveAgentName;
				transaction.CustomerServiceMessage = response.customerServiceMessage;

				if (response.disclosureText != null && response.disclosureText.Count() > 0)
				{
					transaction.DisclosureText_PrimaryLanguage = response.disclosureText[0].textTranslation.ToString();
					if (response.disclosureText.Count() > 1)
						transaction.DisclosureText_SecondaryLanguage = response.disclosureText[1].textTranslation.ToString();
				}

				transaction.RequestResponseType = (int)RequestResponseType.ValidationResponse;

				MGTransactionLogRepo.UpdateWithFlush(transaction);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update", ex.InnerException);
			}
			return transaction.Id;
		}

		// Update transaction with Receive ValidationResponse
		private long _UpdateTrx(long transactionId, ReceiveValidationResponse response, string timezone, bool duringCommitYN = false)
		{
			MGData.Transaction transaction = GetMGTransaction(transactionId);

			try
			{
				transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
				transaction.DTServerLastModified = DateTime.Now;

				transaction.DoCheckIn = response.doCheckIn;
				transaction.ResponseTimeStamp = response.timeStamp;
				transaction.Flags = response.flags;

				transaction.ReadyForCommit = response.readyForCommit;

				if (duringCommitYN)
					transaction.RequestResponseType = (int)RequestResponseType.ValidationResponseDuringCommit;
				else
					transaction.RequestResponseType = (int)RequestResponseType.ValidationResponse;

				MGTransactionLogRepo.UpdateWithFlush(transaction);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update after receive validate", ex.InnerException);
			}
			return transaction.Id;
		}

		// Update transaction with CommitResponse
		private void _UpdateTrx(long transactionId, CommitTransactionResponse commitResponse, string timezone)
		{
			MGData.Transaction transaction = GetMGTransaction(transactionId);

			try
			{
				transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
				transaction.DTServerLastModified = DateTime.Now;
				transaction.DoCheckIn = commitResponse.doCheckIn;
				transaction.ResponseTimeStamp = commitResponse.timeStamp;
				transaction.Flags = commitResponse.flags;
				transaction.ReferenceNumber = commitResponse.referenceNumber;
				transaction.PartnerConfirmationNumber = commitResponse.partnerConfirmationNumber;
				transaction.FreePhoneCallPIN = commitResponse.freePhoneCallPIN;
				transaction.TollFreePhoneNumber = commitResponse.tollFreePhoneNumber;
				if (commitResponse.expectedDateOfDeliverySpecified)
					transaction.ExpectedDateOfDelivery = commitResponse.expectedDateOfDelivery;
				transaction.TransactionDateTime = commitResponse.transactionDateTime;
				transaction.RequestResponseType = (int)RequestResponseType.CommitResponse;

				transaction.PartnerName = commitResponse.partnerName;
				if (commitResponse.receiptTextInfo != null && commitResponse.receiptTextInfo.Any())
				{
					transaction.ReceiptTextInfo_PrimaryLanguage = commitResponse.receiptTextInfo[0].textTranslation.ToString();

					if (commitResponse.receiptTextInfo.Count() > 1)
						transaction.ReceiptTextInfo_SecondaryLanguage = commitResponse.receiptTextInfo[1].textTranslation.ToString();
				}

				MGTransactionLogRepo.UpdateWithFlush(transaction);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update", ex.InnerException);
			}
		}

		// Update transaction with AmendTransactionResponse
		private void _UpdateTrx(long transactionId, AmendTransactionResponse response, string timezone)
		{
			MGData.Transaction transaction = GetMGTransaction(transactionId);

			try
			{
				transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
				transaction.DTServerLastModified = DateTime.Now;
				transaction.RequestResponseType = (int)RequestResponseType.AmendTransactionResponse;
				transaction.TransactionSucceeded = response.transactionSucceeded;

				MGTransactionLogRepo.UpdateWithFlush(transaction);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update", ex.InnerException);
			}
		}

		private AmendTransactionRequest BuildAmendTransactionRequest(long transactionId, MGIContext mgiContext)
		{
			MGData.Transaction trx = GetMGTransaction(transactionId);
			string agentName = GetAgentName(mgiContext);
			var baseRequest = PopulateBaseRequest();

			AmendTransactionRequest amendTransactionRequest = new AmendTransactionRequest()
			{
				agentID = baseRequest.AgentID,
				agentSequence = baseRequest.AgentSequence,
				token = baseRequest.Token,
				apiVersion = baseRequest.ApiVersion,
				clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
				timeStamp = baseRequest.TimeStamp,
				referenceNumber = trx.ReferenceNumber,
				receiverFirstName = trx.ReceiverFirstName,
				receiverLastName = trx.ReceiverLastName,
				receiverLastName2 = string.IsNullOrWhiteSpace(trx.ReceiverLastName2) ? "/" : trx.ReceiverLastName2,
				receiverMiddleName = trx.ReceiverMiddleName,

				operatorName = agentName
			};

			try
			{
				trx.RequestResponseType = (int)RequestResponseType.AmendTransactionRequest;

				trx.DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				trx.DTServerLastModified = DateTime.Now;
				trx.RequestTimeStamp = amendTransactionRequest.timeStamp;

				MGTransactionLogRepo.UpdateWithFlush(trx);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update", ex.InnerException);
			}

			return amendTransactionRequest;
		}

		private CommitTransactionRequest BuildCommitRequest(long transactionId, string timezone)
		{
			MGData.Transaction transaction = GetMGTransaction(transactionId);
			var baseRequest = PopulateBaseRequest();

			var request = new CommitTransactionRequest()
			{
				agentID = baseRequest.AgentID,
				agentSequence = baseRequest.AgentSequence,
				token = baseRequest.Token,
				apiVersion = baseRequest.ApiVersion,
				clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
				timeStamp = baseRequest.TimeStamp,

				productType = transaction.TransactionType == ((int)MoneyTransferType.Receive).ToString() ? productType.RCV : productType.SEND,
				mgiTransactionSessionID = transaction.MgiTransactionSessionId
			};

			try
			{
				transaction.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
				transaction.DTServerLastModified = DateTime.Now;
				transaction.AgentID = request.agentID;
				transaction.AgentSequence = request.agentSequence;
				transaction.Token = request.token;
				transaction.ApiVersion = request.apiVersion;
				transaction.ClientSoftwareVersion = request.clientSoftwareVersion;
				transaction.RequestTimeStamp = request.timeStamp;
				transaction.TransactionType = request.productType == productType.SEND ? ((int)MoneyTransferType.Send).ToString() : ((int)MoneyTransferType.Receive).ToString();
				transaction.MgiTransactionSessionId = request.mgiTransactionSessionID;
				transaction.RequestResponseType = (int)RequestResponseType.CommitRequest;
				MGTransactionLogRepo.UpdateWithFlush(transaction);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in MGI Transaction update during validate", ex.InnerException);
			}

			return request;
		}


		private string GetAgentName(MGIContext mgiContext)
		{
			return mgiContext.AgentName.Substring(0, 7);
		}


		private photoIdType GetPhotoIdType(string idType)
		{
			return Mapper.Map<photoIdType>(MoneyGramCommonIO.GetPhotoIdType(idType));
		}

		private BaseRequest PopulateBaseRequest()
		{
			BaseRequest baserequest = new BaseRequest();

			baserequest.AgentID = "30042575";
			baserequest.AgentSequence = "1";
			baserequest.Token = "TEST";
			baserequest.ApiVersion = "1305";
			baserequest.ClientSoftwareVersion = "10.2";
			baserequest.TimeStamp = DateTime.Now;

			return baserequest;
		}

		private BaseRequest PopulateBaseRequestReceive()
		{
			BaseRequest baserequest = new BaseRequest();

			baserequest.AgentID = "30014930";
			baserequest.AgentSequence = "15";
			baserequest.Token = "TEST";
			baserequest.ApiVersion = "1305";
			baserequest.ClientSoftwareVersion = "10.2";
			baserequest.TimeStamp = DateTime.Now;

			return baserequest;
		}

		private Transaction GetReceiveMoneyTransaction(TransactionRequest transactionRequest, MGIContext mgiContext)
		{
			string errorMessage = "Error while receive money transaction";

			try
			{
				// Initiate transaction
				long transactionId = CreateReceiveMoneyTransaction(transactionRequest, mgiContext);

				ReferenceNumberRequest referenceNumberRequest = BuildReferenceNumberRequest(transactionRequest);
				ReferenceNumberResponse response = IO.RequestReferenceNumber(referenceNumberRequest);
				ValidateReferenceNumberResponse(response);

				// Update transaction
				UpdateReceiveMoneyTransaction(transactionId, response, mgiContext);

				Transaction transaction = new Transaction();

				Mapper.Map(response, transaction);

				transaction.MetaData = new Dictionary<string, object>() { };
				transaction.TransactionID = transactionId.ToString();

				return transaction;
			}
			catch (MGramProviderException ex)
			{
				throw new MoneyTransferException(MoneyTransferException.PROVIDER_ERROR, ex.Message, ex);
			}
			catch (MoneyTransferException ex)
			{
				throw ex;
			}
			catch (TimeoutException timeoutEx)
			{
				throw new Exception(errorMessage, timeoutEx);
			}
			catch (WebException webEx)
			{
				throw new Exception(errorMessage, webEx);
			}
			catch (Exception ex)
			{
				errorMessage = string.Format("{0}. {1}", errorMessage, ex.Message);
				throw new Exception(errorMessage, ex);
			}

		}

		private long CreateReceiveMoneyTransaction(TransactionRequest transactionRequest, MGIContext mgiContext)
		{
			MGData.Transaction transaction = new MGData.Transaction();
			var baseRequest = PopulateBaseRequestReceive();

			Mapper.Map(baseRequest, transaction);
			transaction.rowguid = new Guid();
			transaction.ReceiveMoneySearchRefNo = transactionRequest.ConfirmationNumber;
			transaction.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			transaction.DTServerCreate = DateTime.Now;
			transaction.TransactionType = ((int)MGData.TransferType.ReceiveMoney).ToString();
			transaction.RequestResponseType = (int)RequestResponseType.ReferenceNumberRequest;
			transaction.ChannelPartnerId = mgiContext.ChannelPartnerId;
			transaction.ProviderId = mgiContext.ProviderId;
			MGTransactionLogRepo.AddWithFlush(transaction);

			return transaction.Id;
		}

		private void UpdateReceiveMoneyTransaction(long transactionId, ReferenceNumberResponse referenceNumberResponse, MGIContext mgiContext)
		{
			MGData.Transaction transaction = GetMGTransaction(transactionId);
			transaction = Mapper.Map<ReferenceNumberResponse, MGData.Transaction>(referenceNumberResponse, transaction);
			transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			transaction.DTServerLastModified = DateTime.Now;
			transaction.RequestResponseType = (int)RequestResponseType.ReferenceNumberResponse;
			MGTransactionLogRepo.UpdateWithFlush(transaction);
		}

        private void ValidateReferenceNumberResponse(ReferenceNumberResponse referenceNumberResponse)
        {
            if (referenceNumberResponse.transactionStatus != transactionStatus.AVAIL)
            {
                switch (referenceNumberResponse.transactionStatus)
                {
                    case transactionStatus.CANCL:
                        throw new MoneyTransferException(MoneyTransferException.TRANSACTION_STATUS_CANCL, "Reference number was cancelled, advise customer to contact MoneyGram International.");
                        
                    case transactionStatus.RECVD:
                        throw new MoneyTransferException(MoneyTransferException.TRANSACTION_STATUS_RECVD, "Reference number already paid, advise customer to contact MoneyGram International.");
                        
                    case transactionStatus.REFND:
                        throw new MoneyTransferException(MoneyTransferException.TRANSACTION_STATUS_REFND, "Reference number already refunded, advise customer to contact MoneyGram International.");
                        
                }
            }
            else
            {
                if (!referenceNumberResponse.okForAgent)
                    throw new MoneyTransferException(MoneyTransferException.OK_FOR_AGENT, "This transaction is not available for this agent.");
                else if (!referenceNumberResponse.okForPickup)
                    throw new MoneyTransferException(MoneyTransferException.OK_FOR_PICKUP, referenceNumberResponse.notOkForPickupReasonDescription);
            }
        }

		private ReferenceNumberRequest BuildReferenceNumberRequest(TransactionRequest transactionRequest)
		{
			ReferenceNumberRequest referenceNumberRequest = new ReferenceNumberRequest();
			var baseRequest = PopulateBaseRequestReceive();
			Mapper.Map(baseRequest, referenceNumberRequest);
			referenceNumberRequest.referenceNumber = transactionRequest.ConfirmationNumber;
			return referenceNumberRequest;
		}

		private long CreateSendMoneyTransaction(MGData.Transaction Trxlog, MGIContext mgiContext)
		{

			bool receiverExisted = _isReceiverExisting(new Receiver()
			{
				Id = Trxlog.ReceiverId,
				FirstName = Trxlog.ReceiverFirstName,
				LastName = Trxlog.ReceiverLastName
			});

			if (receiverExisted)
				throw new MoneyTransferException(MoneyTransferException.RECEIVER_ALREADY_EXISTED,
					"Receiver already existed with first name and last name combination.");

			try
			{
				Trxlog.rowguid = Guid.NewGuid();
				Trxlog.DTServerCreate = DateTime.Now;
				Trxlog.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				Trxlog.OriginalTransactionID = Trxlog.Id;
				Trxlog.TransactionSubType =  mgiContext.TrxSubType.ToString();
				Trxlog.RequestResponseType = (int)RequestResponseType.Stage;
				Trxlog.TransactionSucceeded = false;

				MGTransactionLogRepo.AddWithFlush(Trxlog);
			}
			catch (Exception ex)
			{
				throw new Exception("Error in Moneygram Create Modify Transaction", ex);
			}
			return Trxlog.Id;
		}

		private bool _isReceiverExisting(Receiver receiver)
		{
			MGData.Receiver eReceiver = ReceiverRepo.FindBy(c => c.Id == receiver.Id);

			if (eReceiver != null)
			{
				if (ReceiverRepo.FilterBy(c => c.FirstName.ToLower() == receiver.FirstName
					&& c.LastName.ToLower() == receiver.LastName.ToLower()
					&& c.CustomerId == eReceiver.CustomerId && c.Id != receiver.Id).Any())
				{ return true; }
			}
			else
			{
				if (ReceiverRepo.FilterBy(c => c.FirstName.ToLower() == receiver.FirstName &&
					c.LastName.ToLower() == receiver.LastName.ToLower() && c.IsActive == true
					&& c.CustomerId == receiver.CustomerId).Any())
				{ return true; }
			}

            return false;
        }
        #endregion


        public string GetDeliveryServiceTransalation(string serviceName, string language)
        {
            return string.Empty;
        }

        public string GetCountryTransalation(string countryCode, string language)
        {
            return string.Empty;
        }
    }
}
