using AutoMapper;
using MGI.Biz.MoneyTransfer.Data;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;
using System.Collections.Generic;
using bizAccount = MGI.Biz.MoneyTransfer.Data.Account;
using bizCardDetails = MGI.Biz.MoneyTransfer.Data.CardDetails;
using bizCardLookupDetails = MGI.Biz.MoneyTransfer.Data.CardLookupDetails;
using BizData = MGI.Biz.MoneyTransfer.Data;
using bizMasterData = MGI.Biz.MoneyTransfer.Data.MasterData;
using bizModifySendMoney = MGI.Biz.MoneyTransfer.Data.ModifyRequest;
using bizPaymentDetails = MGI.Biz.MoneyTransfer.Data.PaymentDetails;
using bizReason = MGI.Biz.MoneyTransfer.Data.Reason;

using bizReceiver = MGI.Biz.MoneyTransfer.Data.Receiver;
using bizRefundSendMoney = MGI.Biz.MoneyTransfer.Data.RefundRequest;
using bizSendMoneyTransaction = MGI.Biz.MoneyTransfer.Data.MoneyTransferTransaction;
using dmsAccount = MGI.Channel.Shared.Server.Data.Account;
using dmsCardDetails = MGI.Channel.Shared.Server.Data.CardDetails;
using dmsCardLookupDetails = MGI.Channel.Shared.Server.Data.CardLookupDetails;
using dmsMasterData = MGI.Channel.Shared.Server.Data.XferMasterData;
using dmsModifySendMoney = MGI.Channel.Shared.Server.Data.ModifySendMoneyRequest;
using dmsPaymentDetails = MGI.Channel.Shared.Server.Data.XferPaymentDetails;
using dmsReason = MGI.Channel.Shared.Server.Data.MoneyTransferReason;
using dmsReceiver = MGI.Channel.Shared.Server.Data.Receiver;
using dmsSendMoneyTransaction = MGI.Channel.Shared.Server.Data.MoneyTransferTransaction;
using SharedData = MGI.Channel.Shared.Server.Data;
using dmsCashierResponse = MGI.Channel.DMS.Server.Data.CashierDetails;
using coreCustomerSession = MGI.Core.Partner.Data.CustomerSession;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IMoneyTransferService, IMoneyTransferSetupService
	{
		/// <summary>
		/// Method for initialise map
		/// </summary>
		internal static void MoneyTransferConverter()
		{
			Mapper.CreateMap<bizReceiver, dmsReceiver>();
			//Mapper.CreateMap<dmsReceiver, bizReceiver>();
			Mapper.CreateMap<dmsPaymentDetails, bizPaymentDetails>()
				.ForMember(d => d.RecieverFirstName, o => o.MapFrom(s => s.ReceiverFirstName))
				.ForMember(d => d.RecieverLastName, o => o.MapFrom(s => s.ReceiverLastName))
				.ForMember(d => d.RecieverSecondLastName, o => o.MapFrom(s => s.ReceiverSecondLastName));
			Mapper.CreateMap<bizPaymentDetails, dmsPaymentDetails>()
				.ForMember(d => d.ReceiverFirstName, o => o.MapFrom(s => s.RecieverFirstName))
				.ForMember(d => d.ReceiverLastName, o => o.MapFrom(s => s.RecieverLastName))
				.ForMember(d => d.ReceiverSecondLastName, o => o.MapFrom(s => s.RecieverSecondLastName));
			Mapper.CreateMap<bizMasterData, dmsMasterData>();
			Mapper.CreateMap<dmsCardDetails, bizCardDetails>();
			Mapper.CreateMap<dmsCardLookupDetails, bizCardLookupDetails>();
			Mapper.CreateMap<MGI.Biz.MoneyTransfer.Data.CardInfo, SharedData.CardInfo>();
			Mapper.CreateMap<dmsSendMoneyTransaction, bizSendMoneyTransaction>();
			Mapper.CreateMap<bizSendMoneyTransaction, dmsSendMoneyTransaction>();
			Mapper.CreateMap<bizAccount, dmsAccount>();
			Mapper.CreateMap<dmsAccount, bizAccount>();
			Mapper.CreateMap<dmsModifySendMoney, bizModifySendMoney>();
			Mapper.CreateMap<bizModifySendMoney, dmsModifySendMoney>();
			Mapper.CreateMap<BizData.DeliveryService, SharedData.DeliveryService>();
			Mapper.CreateMap<SharedData.DeliveryService, BizData.DeliveryService>();
			Mapper.CreateMap<SharedData.DeliveryServiceRequest, BizData.DeliveryServiceRequest>();
			Mapper.CreateMap<BizData.DeliveryServiceRequest, SharedData.DeliveryServiceRequest>();
			Mapper.CreateMap<bizReason, dmsReason>();
			Mapper.CreateMap<SharedData.ReasonRequest, BizData.ReasonRequest>();
			Mapper.CreateMap<SharedData.SendMoneySearchResponse, BizData.SearchResponse>();
			Mapper.CreateMap<BizData.SearchResponse, SharedData.SendMoneySearchResponse>();

			Mapper.CreateMap<RefundSendMoneyRequest, RefundRequest>();
		}

		#region IMoneyTransferService Impl

		#region Shared Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="recipient"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Add receiver")]
		public long AddReceiver(long customerSessionId, dmsReceiver receiver, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.AddReceiver(customerSessionId, receiver, context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="recipient"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Edit receiver")]
		public long EditReceiver(long customerSessionId, dmsReceiver receiver, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.EditReceiver(customerSessionId, receiver, context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="AlloyID"></param>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Get frequent receivers")]
		public IList<dmsReceiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetFrequentReceivers(customerSessionId, context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="beneficiaryId"></param>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Get receiver by ID")]
		public dmsReceiver GetReceiver(long customerSessionId, long Id, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetReceiver(customerSessionId, Id, context);
		}

		/// <summary>
		///  AL-3502
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="receiverId"></param>
		/// <param name="mgiContext"></param>
		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Delete favorite receiver")]
		public void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			SharedEngine.DeleteFavoriteReceiver(customerSessionId, receiverId, context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="paymentDetails"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Get MT fee")]
		public SharedData.FeeResponse GetFee(long customerSessionId, SharedData.FeeRequest feeRequest, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetXfrFee(customerSessionId, feeRequest, context);
			//These implementation is moved to Shared engine 
			//var bizFeeRequest = Mapper.Map<BizData.FeeRequest>(feeRequest);
			//MGIContext context = _GetContext(customerSessionId);
			//BizData.FeeResponse bizFeeResponse = bizMoneyTransferEngine.GetFee(customerSessionId, bizFeeRequest, context);
			//return Mapper.Map<SharedData.FeeResponse>(bizFeeResponse);
		}

		/// <summary>
		/// US2054
		/// </summary>
		/// <param name="agentSessionId"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "GetAgentXfer")]
		public dmsCashierResponse GetAgentXfer(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			string agentSessionid = agentSessionId.ToString();
			return GetAgentDetails(agentSessionid, context);
		}

		/// <summary>
		/// US2054
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="locationState"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "IsSWBStateXfer")]
		public bool IsSWBStateXfer(long customerSessionId, string locationState, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return bizMoneyTransferEngine.IsSWBState(customerSessionId, locationState, context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="beneficiary"></param>
		/// <param name="paymentDetails"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Begin MT transaction")]
		public SharedData.ValidateResponse ValidateTransfer(long customerSessionId, SharedData.ValidateRequest validateRequest, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.ValidateXfr(customerSessionId, validateRequest, context);
		}

		[Transaction(ReadOnly = true)]
		public List<SharedData.DeliveryService> GetDeliveryServices(long customerSessionId, SharedData.DeliveryServiceRequest request, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetXfrDeliveryServices(customerSessionId, request, context);
		}

		[Transaction(ReadOnly = true)]
		public List<SharedData.Field> GetXfrProviderAttributes(long customerSessionId, SharedData.AttributeRequest attributeRequest, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetXfrProviderAttributes(customerSessionId, attributeRequest, context);
		}

		[Transaction()]
		public dmsSendMoneyTransaction GetMoneyTransferDetailsTransaction(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetXfrTransaction(customerSessionId, transactionId, context);
		}

		#endregion

		#region DMS Methods

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Cancel")]
		public void Cancel(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			bizMoneyTransferEngine.Cancel(customerSessionId, transactionId, context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lastName"></param>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Get receivers by last name")]
		public IList<dmsReceiver> GetReceivers(long customerSessionId, string lastName, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, lastName, "GetReceivers", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin GetReceivers- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			#endregion
			var receivers = bizMoneyTransferEngine.GetReceivers(customerSessionId, lastName, context);

			#region AL-3370 Transactional Log User Story
            MongoDBLogger.ListInfo<bizReceiver>(customerSessionId, receivers, "GetReceivers", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End GetReceivers- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			#endregion
			return Mapper.Map<IList<bizReceiver>, IList<dmsReceiver>>(receivers);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="beneficiaryId"></param>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Get receiver by name")]
		public dmsReceiver GetReceiver(long customerSessionId, string fullName, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId,fullName, "GetReceiver", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin GetReceiver- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			#endregion
			var receiver = bizMoneyTransferEngine.GetReceiver(customerSessionId, fullName, context);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<bizReceiver>(customerSessionId, receiver, "GetReceiver", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End GetReceiver- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			#endregion
			return Mapper.Map<bizReceiver, dmsReceiver>(receiver);
		}

		[Transaction]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Get receive transaction")]
		public SharedData.MoneyTransferTransaction Get(long customerSessionId, SharedData.ReceiveMoneyRequest request, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<SharedData.ReceiveMoneyRequest>(customerSessionId, request, "Get", AlloyLayerName.SERVICE, ModuleName.ReceiveMoney,
                                      "Begin Get- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			#endregion
			BizData.ReceiveMoneyRequest receiveMoneyRequest = new BizData.ReceiveMoneyRequest()
			{
				ConfirmationNumber = request.ConfirmationNumber
			};
			BizData.MoneyTransferTransaction receivetransaction = bizMoneyTransferEngine.Get(customerSessionId, receiveMoneyRequest, context);

           #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<BizData.MoneyTransferTransaction>(customerSessionId, receivetransaction, "Get", AlloyLayerName.SERVICE, ModuleName.ReceiveMoney,
                                      "End Get- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
		   #endregion
			return Mapper.Map<BizData.MoneyTransferTransaction, SharedData.MoneyTransferTransaction>(receivetransaction);
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Update WU account")]
		public bool UpdateWUAccount(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			 #region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, WUGoldCardNumber, "UpdateWUAccount", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "Begin UpdateWUAccount- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			 #endregion
			bool isUpdated = bizMoneyTransferEngine.UpdateAccount(customerSessionId, WUGoldCardNumber, context);
			long cxeAccountId = BizBillPayService.UpdateWUCardDetails(customerSessionId, WUGoldCardNumber, context);
			if (cxeAccountId > 0)
				isUpdated = true;

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, isUpdated.ToString(), "UpdateWUAccount", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "End UpdateWUAccount- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			#endregion
			return isUpdated;
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Enroll WU card")]
		public SharedData.CardDetails WUCardEnrollment(long customerSessionId, dmsPaymentDetails paymentDetails, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			 #region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<dmsPaymentDetails>(customerSessionId, paymentDetails, "WUCardEnrollment", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "Begin WUCardEnrollment- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			 #endregion
			bizPaymentDetails bizpayment = Mapper.Map<dmsPaymentDetails, bizPaymentDetails>(paymentDetails);

			bizCardDetails bizCardDtls = new bizCardDetails();

			bizCardDtls = bizMoneyTransferEngine.WUCardEnrollment(customerSessionId, bizpayment, context);
			SharedData.CardDetails cardDetails = new dmsCardDetails();

			long cxeAccountId = BizBillPayService.UpdateWUCardDetails(customerSessionId, bizCardDtls.AccountNumber, context);

			cardDetails = EnrollCardResponseMapper(bizCardDtls, cardDetails);

			 #region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<SharedData.CardDetails>(customerSessionId, cardDetails, "WUCardEnrollment", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "End WUCardEnrollment- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			 #endregion
			return cardDetails;
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "WU card lookup")]
		public List<WUCustomerGoldCardResult> WUCardLookup(long customerSessionId, SharedData.CardLookupDetails wucardlookupreq, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			 #region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<SharedData.CardLookupDetails>(customerSessionId, wucardlookupreq, "WUCardLookup", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "Begin WUCardLookup- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			 #endregion
			bizCardLookupDetails LookupDetails = Mapper.Map<SharedData.CardLookupDetails, bizCardLookupDetails>(wucardlookupreq);
			LookupDetails = bizMoneyTransferEngine.WUCardLookup(customerSessionId, LookupDetails, context);
			List<WUCustomerGoldCardResult> goldCardResult = new List<WUCustomerGoldCardResult>();

			WUCustomerGoldCardResult carddetail = null;

			foreach (MGI.Biz.MoneyTransfer.Data.Account account in LookupDetails.Sender)
			{
				carddetail = new WUCustomerGoldCardResult()
				{
					Address = account.Address,
					FullName = string.Format("{0} {1}", account.FirstName, account.LastName),
					ZipCode = account.PostalCode,
					WUGoldCardNumber = account.LoyaltyCardNumber,
					PhoneNumber = account.MobilePhone
				};
				goldCardResult.Add(carddetail);
			}

			 #region AL-3370 Transactional Log User Story
			MongoDBLogger.ListInfo<WUCustomerGoldCardResult>(customerSessionId, goldCardResult, "WUCardLookup", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "End WUCardLookup- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			 #endregion
			return goldCardResult;
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Get WU account")]
		public bool GetWUCardAccount(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return bizMoneyTransferEngine.GetAccount(customerSessionId, context);
		}

		private SharedData.CardDetails EnrollCardResponseMapper(bizCardDetails bizCardDtls, SharedData.CardDetails cardDetails)
		{
			cardDetails = new SharedData.CardDetails()
			{
				AccountNumber = bizCardDtls.AccountNumber,
				ForiegnSystemId = bizCardDtls.ForiegnSystemId,
				ForiegnRefNum = bizCardDtls.ForiegnRefNum,
				CounterId = bizCardDtls.CounterId
			};
			return cardDetails;
		}

		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Get sender details")]
		public SharedData.Account DisplayWUCardAccountInfo(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			 #region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "DisplayWUCardAccountInfo", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "Begin DisplayWUCardAccountInfo- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			 #endregion
			MGI.Biz.MoneyTransfer.Data.Account senderDetails = bizMoneyTransferEngine.DisplayWUCardAccountInfo(customerSessionId, context);

			SharedData.Account account = new SharedData.Account()
			{
				LoyaltyCardNumber = senderDetails.LoyaltyCardNumber
			};
			 #region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<SharedData.Account>(customerSessionId, account, "DisplayWUCardAccountInfo", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "End DisplayWUCardAccountInfo- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			 #endregion
			return account;
		}


		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyTransfer, "Get card info")]
		public SharedData.CardInfo GetCardInfoXfer(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			MGI.Biz.MoneyTransfer.Data.CardInfo cardInfo = bizMoneyTransferEngine.GetCardInfo(customerSessionId, context);
			return Mapper.Map<SharedData.CardInfo>(cardInfo);
		}

		[Transaction]
		public void AddPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			bizMoneyTransferEngine.AddPastReceivers(customerSessionId, cardNumber, context);
		}

		#region  Send Money Modify

		[Transaction()]
		public string GetStatus(long customerSessionId, string confirmationNumber, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return bizMoneyTransferEngine.GetStatus(customerSessionId, confirmationNumber, context);
		}

		[Transaction()]
		public SharedData.SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.SendMoneySearch(customerSessionId, request, context);
		}

		[Transaction()]
		public SharedData.ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, SharedData.ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<SharedData.ModifySendMoneyRequest>(customerSessionId, moneyTransferModify, "StageModifySendMoney", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "Begin StageModifySendMoney- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			#endregion
			ModifyRequest modifySendMoney = Mapper.Map<dmsModifySendMoney, bizModifySendMoney>(moneyTransferModify);

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<ModifyRequest>(customerSessionId, modifySendMoney, "StageModifySendMoney", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "End StageModifySendMoney- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
			#endregion

			return SharedEngine.StageModifySendMoney(customerSessionId, moneyTransferModify, context);
		}

		[Transaction()]
		public SharedData.ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId, SharedData.ModifySendMoneyRequest modifySendMoneyRequest, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.AuthorizeModifySendMoney(customerSessionId, modifySendMoneyRequest, context);
		}

		#endregion

		#region  Send Money Refund

		[Transaction(ReadOnly = true)]
		public List<MoneyTransferReason> GetRefundReasons(long customerSessionId, SharedData.ReasonRequest request, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetRefundReasons(customerSessionId, request, context);
		}


		[Transaction()]
		public long StageRefundSendMoney(long customerSessionId, SharedData.RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.StageRefundSendMoney(customerSessionId, moneyTransferRefund, context);
		}

		[Transaction()]
		public string SendMoneyRefund(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<RefundSendMoneyRequest>(customerSessionId, moneyTransferRefund, "SendMoneyRefund", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin SendMoneyRefund- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine",context);
			#endregion
			BizData.SearchRequest request = new BizData.SearchRequest()
			{
				ConfirmationNumber = moneyTransferRefund.ConfirmationNumber,
				SearchRequestType = BizData.SearchRequestType.RefundWithStage,
				TransactionId = long.Parse(moneyTransferRefund.TransactionId)
			};

			BizData.SearchResponse response = bizMoneyTransferEngine.Search(customerSessionId, request, context);

			if (response.RefundStatus.ToUpper() == RefundStatus.F.ToString() || response.RefundStatus.ToUpper() == RefundStatus.N.ToString())
			{
				BIZShoppingCartService.AddMoneyTransfer(customerSessionId, response.CancelTransactionId, context);
				BIZShoppingCartService.AddMoneyTransfer(customerSessionId, response.RefundTransactionId, context);

				bizRefundSendMoney bizRefundSendMoney = new bizRefundSendMoney()
				{
					TransactionId = long.Parse(moneyTransferRefund.TransactionId),
					ReasonCode = moneyTransferRefund.CategoryCode,
					ReasonDesc = moneyTransferRefund.CategoryDescription,
					Comments = moneyTransferRefund.Reason,
					ConfirmationNumber = moneyTransferRefund.ConfirmationNumber,
					RefundStatus = moneyTransferRefund.RefundStatus,
					CancelTransactionId = response.CancelTransactionId,
					RefundTransactionId = response.RefundTransactionId
				};

				#region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<bizRefundSendMoney>(customerSessionId, bizRefundSendMoney, "SendMoneyRefund", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                          "End SendMoneyRefund- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);
				#endregion
				return bizMoneyTransferEngine.Refund(customerSessionId, bizRefundSendMoney, context);
			}

            //AL-3370 Transactional Log User Story
            MongoDBLogger.Info<BizData.SearchResponse>(customerSessionId, response, "SendMoneyRefund", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End SendMoneyRefund- MGI.Channel.DMS.Server.Impl.MoneyTransferEngine", context);

			return string.Empty;
		}


		#endregion

		#endregion

		#endregion

		#region IMoneyTransferSetupService Impl

		#region Shared Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		public List<dmsMasterData> GetXfrCountries(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetXfrCountries(customerSessionId, context);
			//return Mapper.Map<List<bizMasterData>, List<dmsMasterData>>(bizMoneyTransferEngine.GetCountries(context));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="countryCode"></param>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		public List<dmsMasterData> GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetXfrStates(customerSessionId, countryCode, context);
			//return Mapper.Map<List<bizMasterData>, List<dmsMasterData>>(bizMoneyTransferEngine.GetStates(countryCode, context));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stateCode"></param>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		public List<dmsMasterData> GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetXfrCities(customerSessionId, stateCode, context);
			//return Mapper.Map<List<bizMasterData>, List<dmsMasterData>>(bizMoneyTransferEngine.GetCities(stateCode, context));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		public List<dmsMasterData> GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GetCurrencyCodeList(customerSessionId, countryCode, context);
			//return Mapper.Map<List<bizMasterData>, List<dmsMasterData>>(bizMoneyTransferEngine.GetCurrencyCodeList(countryCode, context));
		}

		#endregion

		#region DMS Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		public string GetCurrencyCode(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return bizMoneyTransferEngine.GetCurrencyCode(customerSessionId, countryCode, context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		public List<dmsMasterData> WUGetAgentBannerMessage(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return Mapper.Map<List<bizMasterData>, List<dmsMasterData>>(bizMoneyTransferEngine.GetBannerMsgs(agentSessionId, context));
		}




		#endregion

		#endregion
	}
}
