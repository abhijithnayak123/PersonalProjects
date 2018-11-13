using AutoMapper;
using MGI.Biz.MoneyTransfer.Contract;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;
using System.Collections.Generic;
using bizAccount = MGI.Biz.MoneyTransfer.Data.Account;
using bizMasterData = MGI.Biz.MoneyTransfer.Data.MasterData;
using bizModifySendMoney = MGI.Biz.MoneyTransfer.Data.ModifyRequest;
using bizReason = MGI.Biz.MoneyTransfer.Data.Reason;
using BizMoneyTransferData = MGI.Biz.MoneyTransfer.Data;
using bizPaymentDetails = MGI.Biz.MoneyTransfer.Data.PaymentDetails;
using bizReceiver = MGI.Biz.MoneyTransfer.Data.Receiver;
using dmsMasterData = MGI.Channel.Shared.Server.Data.XferMasterData;
using dmsModifySendMoney = MGI.Channel.Shared.Server.Data.ModifySendMoneyRequest;
using dmsPaymentDetails = MGI.Channel.Shared.Server.Data.XferPaymentDetails;
using dmsReceiver = MGI.Channel.Shared.Server.Data.Receiver;
using ISharedMoneyTransferService = MGI.Channel.Shared.Server.Contract.IMoneyTransferService;
using ISharedMoneyTransferSetupService = MGI.Channel.Shared.Server.Contract.IMoneyTransferSetupService;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;
using System;

namespace MGI.Channel.Shared.Server.Impl
{
    public partial class SharedEngine : ISharedMoneyTransferSetupService, ISharedMoneyTransferService
    {
        #region Injected Services & Data Mapper

        public IMoneyTransferEngine bizMoneyTransferEngine { get; set; }

        /// <summary>
        /// Method for initialise map
        /// </summary>
        internal static void MoneyTransferConverter()
        {

            Mapper.CreateMap<bizAccount, Account>();
            Mapper.CreateMap<bizReceiver, dmsReceiver>();
            Mapper.CreateMap<dmsReceiver, bizReceiver>();
            Mapper.CreateMap<bizReason, SharedData.MoneyTransferReason>();
            Mapper.CreateMap<dmsPaymentDetails, bizPaymentDetails>()
                .ForMember(d => d.RecieverFirstName, o => o.MapFrom(s => s.ReceiverFirstName))
                .ForMember(d => d.RecieverLastName, o => o.MapFrom(s => s.ReceiverLastName))
                .ForMember(d => d.RecieverSecondLastName, o => o.MapFrom(s => s.ReceiverSecondLastName));
            Mapper.CreateMap<bizPaymentDetails, dmsPaymentDetails>()
                .ForMember(d => d.ReceiverFirstName, o => o.MapFrom(s => s.RecieverFirstName))
                .ForMember(d => d.ReceiverLastName, o => o.MapFrom(s => s.RecieverLastName))
                .ForMember(d => d.ReceiverSecondLastName, o => o.MapFrom(s => s.RecieverSecondLastName));
            Mapper.CreateMap<bizMasterData, dmsMasterData>();

            Mapper.CreateMap<BizMoneyTransferData.DeliveryService, SharedData.DeliveryService>();
            Mapper.CreateMap<SharedData.DeliveryService, BizMoneyTransferData.DeliveryService>();
            Mapper.CreateMap<SharedData.DeliveryServiceRequest, BizMoneyTransferData.DeliveryServiceRequest>();
            Mapper.CreateMap<BizMoneyTransferData.DeliveryServiceRequest, SharedData.DeliveryServiceRequest>();
            Mapper.CreateMap<dmsModifySendMoney, bizModifySendMoney>();
            Mapper.CreateMap<bizModifySendMoney, dmsModifySendMoney>();

            Mapper.CreateMap<BizMoneyTransferData.FeeRequest, FeeRequest>();
            Mapper.CreateMap<FeeRequest, BizMoneyTransferData.FeeRequest>();
            Mapper.CreateMap<BizMoneyTransferData.FeeResponse, FeeResponse>();
            Mapper.CreateMap<FeeResponse, BizMoneyTransferData.FeeResponse>();
            Mapper.CreateMap<FeeInformation, BizMoneyTransferData.FeeInformation>();
            Mapper.CreateMap<BizMoneyTransferData.FeeInformation, FeeInformation>();

            Mapper.CreateMap<BizMoneyTransferData.ValidateRequest, ValidateRequest>();
            Mapper.CreateMap<ValidateRequest, BizMoneyTransferData.ValidateRequest>();

            Mapper.CreateMap<BizMoneyTransferData.ValidateResponse, ValidateResponse>();
            Mapper.CreateMap<ValidateResponse, BizMoneyTransferData.ValidateResponse>();
            Mapper.CreateMap<BizMoneyTransferData.MoneyTransferTransaction, MoneyTransferTransaction>();

            Mapper.CreateMap<AttributeRequest, BizMoneyTransferData.AttributeRequest>();
            Mapper.CreateMap<BizMoneyTransferData.Field, Field>();
            Mapper.CreateMap<BizMoneyTransferData.SearchResponse, SharedData.SendMoneySearchResponse>();

            Mapper.CreateMap<RefundSendMoneyRequest, MGI.Biz.MoneyTransfer.Data.RefundRequest>()
                .ForMember(d => d.ReasonCode, s => s.MapFrom(c => c.CategoryCode))
                .ForMember(d => d.ReasonDesc, s => s.MapFrom(c => c.CategoryDescription));

            Mapper.CreateMap<ReasonRequest, MGI.Biz.MoneyTransfer.Data.ReasonRequest>();
        }
        #endregion

        #region IMoneyTransferSetupService Impl

        public List<dmsMasterData> GetXfrCountries(long customerSessionId, MGIContext mgiContext)
        {
            return Mapper.Map<List<bizMasterData>, List<dmsMasterData>>(bizMoneyTransferEngine.GetCountries(customerSessionId, mgiContext));
        }

        public List<dmsMasterData> GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext)
        {
            return Mapper.Map<List<bizMasterData>, List<dmsMasterData>>(bizMoneyTransferEngine.GetStates(customerSessionId, countryCode, mgiContext));
        }

        public List<dmsMasterData> GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext)
        {
            return Mapper.Map<List<bizMasterData>, List<dmsMasterData>>(bizMoneyTransferEngine.GetCities(customerSessionId, stateCode, mgiContext));
        }

        public List<dmsMasterData> GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext)
        {
            return Mapper.Map<List<bizMasterData>, List<dmsMasterData>>(bizMoneyTransferEngine.GetCurrencyCodeList(customerSessionId, countryCode, mgiContext));
        }

        #endregion

        #region IMoneyTransferService Impl

        #region Xfr Receiver Methods

        public long AddReceiver(long customerSessionId, dmsReceiver receiver, MGIContext mgiContext)
        {
			#region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<dmsReceiver>(customerSessionId, receiver, "AddReceiver", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin AddReceiver-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
			var bizReceiver = Mapper.Map<dmsReceiver, bizReceiver>(receiver);

           #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<bizReceiver>(customerSessionId, bizReceiver, "AddReceiver", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End AddReceiver-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
		   #endregion
			return bizMoneyTransferEngine.AddReceiver(customerSessionId, bizReceiver, mgiContext);
        }

        public long EditReceiver(long customerSessionId, dmsReceiver receiver, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<dmsReceiver>(customerSessionId, receiver, "EditReceiver", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin EditReceiver-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            var bizReceiver = Mapper.Map<dmsReceiver, bizReceiver>(receiver);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<bizReceiver>(customerSessionId, bizReceiver, "EditReceiver", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End EditReceiver-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return bizMoneyTransferEngine.EditReceiver(customerSessionId, bizReceiver, mgiContext);
        }


		public void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext)
		{
			bizMoneyTransferEngine.DeleteFavoriteReceiver(customerSessionId, receiverId, mgiContext);
		}


        public IList<dmsReceiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext)
        {
            return Mapper.Map<IList<bizReceiver>, IList<dmsReceiver>>(bizMoneyTransferEngine.GetFrequentReceivers(customerSessionId, mgiContext));
        }

        public dmsReceiver GetReceiver(long customerSessionId, long Id, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(Id), "GetReceiver", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin GetReceiver-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
			var receiver = bizMoneyTransferEngine.GetReceiver(customerSessionId, Id, mgiContext);

           #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<MGI.Biz.MoneyTransfer.Data.Receiver>(customerSessionId, receiver, "GetReceiver", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End GetReceiver-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
		   #endregion
			return Mapper.Map<bizReceiver, dmsReceiver>(receiver);
        }
        public MoneyTransferTransaction GetReceiverLastTransaction(long customerSessionId, long receiverId, MGIContext mgiContext)
        {
            var transaction = bizMoneyTransferEngine.GetReceiverLastTransaction(customerSessionId, receiverId, mgiContext);
            return Mapper.Map<MGI.Biz.MoneyTransfer.Data.MoneyTransferTransaction, MoneyTransferTransaction>(transaction);
        }

        #endregion

        #region Xfr trx Methods



        public FeeResponse GetXfrFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<FeeRequest>(customerSessionId, feeRequest, "GetXfrFee", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin GetXfrFee- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            var bizFeeRequest = Mapper.Map<BizMoneyTransferData.FeeRequest>(feeRequest);

            BizMoneyTransferData.FeeResponse bizFeeResponse = bizMoneyTransferEngine.GetFee(customerSessionId, bizFeeRequest, mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<BizMoneyTransferData.FeeResponse>(customerSessionId, bizFeeResponse, "GetXfrFee", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End GetXfrFee- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return Mapper.Map<SharedData.FeeResponse>(bizFeeResponse);
        }

        public ValidateResponse ValidateXfr(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ValidateRequest>(customerSessionId, validateRequest, "ValidateXfr", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
                                      "Begin ValidateXfr- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            ValidateResponse serverResponse = new ValidateResponse();

            var bizValidateRequest = Mapper.Map<BizMoneyTransferData.ValidateRequest>(validateRequest);
            BizMoneyTransferData.ValidateResponse response = bizMoneyTransferEngine.Validate(customerSessionId, bizValidateRequest, mgiContext);

            if (response.HasLPMTError)
            {
                serverResponse = new ValidateResponse() { HasLPMTError = true };
            }
            else
            {
                serverResponse = new ValidateResponse() { TransactionId = response.TransactionId, HasLPMTError = false};
                // add to ShoppingCart
                BIZShoppingCartService.AddMoneyTransfer(customerSessionId, response.TransactionId, mgiContext);
			}

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<ValidateResponse>(customerSessionId, serverResponse, "ValidateXfr", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
                                      "End ValidateXfr- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return serverResponse;
        }

        //public int CommitXfr(long customerSessionId, long ptnrTransactionId, MGIContext mgiContext)
        //{
        //	return bizMoneyTransferEngine.Commit(customerSessionId, ptnrTransactionId, mgiContext);
        //}

        //public int CommitXfrModify(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
        //{
        //	bizModifySendMoney modifySendMoney = Mapper.Map<dmsModifySendMoney, bizModifySendMoney>(moneyTransferModify);

        //	return bizMoneyTransferEngine.Modify(customerSessionId, modifySendMoney, mgiContext);
        //}

        public MoneyTransferTransaction GetXfrTransaction(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "GetXfrTransaction", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "Begin GetXfrTransaction- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            BizMoneyTransferData.TransactionRequest request = new BizMoneyTransferData.TransactionRequest()
            {
                PTNRTransactionId = transactionId
            };

            BizMoneyTransferData.MoneyTransferTransaction moneyTransferTransaction = bizMoneyTransferEngine.Get(customerSessionId, request, mgiContext);
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<BizMoneyTransferData.MoneyTransferTransaction>(customerSessionId, moneyTransferTransaction, "GetXfrTransaction", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "End GetXfrTransaction- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return Mapper.Map<MoneyTransferTransaction>(moneyTransferTransaction);
        }

        #endregion

        #endregion

        public List<DeliveryService> GetXfrDeliveryServices(long customerSessionId, DeliveryServiceRequest request, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<DeliveryServiceRequest>(customerSessionId, request, "GetXfrDeliveryServices", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin GetXfrDeliveryServices- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            BizMoneyTransferData.DeliveryServiceRequest deliveryServiceRequest = Mapper.Map<BizMoneyTransferData.DeliveryServiceRequest>(request);

            List<BizMoneyTransferData.DeliveryService> deliveryServices = bizMoneyTransferEngine.GetDeliveryServices(customerSessionId, deliveryServiceRequest, mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.ListInfo<BizMoneyTransferData.DeliveryService>(customerSessionId, deliveryServices, "GetXfrDeliveryServices", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End GetXfrDeliveryServices- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return Mapper.Map<List<SharedData.DeliveryService>>(deliveryServices);
        }

        public List<Field> GetXfrProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext)
        {
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<AttributeRequest>(customerSessionId, attributeRequest, "GetXfrProviderAttributes", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "Begin GetXfrProviderAttributes- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion

            BizMoneyTransferData.AttributeRequest bizAttributeRequest = Mapper.Map<BizMoneyTransferData.AttributeRequest>(attributeRequest);

            List<BizMoneyTransferData.Field> fields = bizMoneyTransferEngine.GetProviderAttributes(customerSessionId, bizAttributeRequest, mgiContext);
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.ListInfo<BizMoneyTransferData.Field>(customerSessionId, fields, "GetXfrProviderAttributes", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "End GetXfrProviderAttributes- MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return Mapper.Map<List<Field>>(fields);
        }

        public SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<SendMoneySearchRequest>(customerSessionId, request, "SendMoneySearch", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
                                      "Begin SendMoneySearch-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            BizMoneyTransferData.SearchRequest bizRequest = new BizMoneyTransferData.SearchRequest()
            {
                ConfirmationNumber = request.ConfirmationNumber,
                SearchRequestType = request.SearchRequestType == SharedData.SearchRequestType.Modify ? BizMoneyTransferData.SearchRequestType.Modify : BizMoneyTransferData.SearchRequestType.Refund
            };
            BizMoneyTransferData.SearchResponse response = bizMoneyTransferEngine.Search(customerSessionId, bizRequest, mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<BizMoneyTransferData.SearchResponse>(customerSessionId, response, "SendMoneySearch", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
                                      "End SendMoneySearch-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return Mapper.Map<BizMoneyTransferData.SearchResponse, SharedData.SendMoneySearchResponse>(response);
        }


        public ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ModifySendMoneyRequest>(customerSessionId, moneyTransferModify, "StageModifySendMoney", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin StageModifySendMoney-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            bizModifySendMoney modifySendMoney = Mapper.Map<ModifySendMoneyRequest, bizModifySendMoney>(moneyTransferModify);
            MGI.Biz.MoneyTransfer.Data.ModifyResponse modifySendmoneyResponse = bizMoneyTransferEngine.StageModify(customerSessionId, modifySendMoney, mgiContext);
            SharedData.ModifySendMoneyResponse dmsModifySendMoneyResponse = new SharedData.ModifySendMoneyResponse()
            {
                CancelTransactionId = modifySendmoneyResponse.CancelTransactionId,
                ModifyTransactionId = modifySendmoneyResponse.ModifyTransactionId
            };

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<SharedData.ModifySendMoneyResponse>(customerSessionId, dmsModifySendMoneyResponse, "StageModifySendMoney", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End StageModifySendMoney-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return dmsModifySendMoneyResponse;
        }

        public ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoneyRequest, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ModifySendMoneyRequest>(customerSessionId, modifySendMoneyRequest, "AuthorizeModifySendMoney", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin AuthorizeModifySendMoney-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            MGI.Biz.MoneyTransfer.Data.ModifyRequest bizModifySendMoneyRequest = new MGI.Biz.MoneyTransfer.Data.ModifyRequest()
            {
                CancelTransactionId = modifySendMoneyRequest.CancelTransactionId,
                ModifyTransactionId = modifySendMoneyRequest.ModifyTransactionId
            };

            bizMoneyTransferEngine.AuthorizeModify(customerSessionId, bizModifySendMoneyRequest, mgiContext);

            BIZShoppingCartService.AddMoneyTransfer(customerSessionId, modifySendMoneyRequest.CancelTransactionId, mgiContext);
            BIZShoppingCartService.AddMoneyTransfer(customerSessionId, modifySendMoneyRequest.ModifyTransactionId, mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "AuthorizeModifySendMoney", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End AuthorizeModifySendMoney-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return new SharedData.ModifySendMoneyResponse()
            {
                CancelTransactionId = modifySendMoneyRequest.CancelTransactionId,
                ModifyTransactionId = modifySendMoneyRequest.ModifyTransactionId
            };

        }


        public List<MoneyTransferReason> GetRefundReasons(long customerSessionId, ReasonRequest request, MGIContext mgiContext)
        {
            MGI.Biz.MoneyTransfer.Data.ReasonRequest reasonRequest = Mapper.Map<ReasonRequest, MGI.Biz.MoneyTransfer.Data.ReasonRequest>(request);
            return Mapper.Map<List<bizReason>, List<SharedData.MoneyTransferReason>>(bizMoneyTransferEngine.GetRefundReasons(customerSessionId, reasonRequest, mgiContext));
        }

        public long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<RefundSendMoneyRequest>(customerSessionId, moneyTransferRefund, "StageRefundSendMoney", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "Begin StageRefundSendMoney-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            MGI.Biz.MoneyTransfer.Data.RefundRequest refundSendMoney = Mapper.Map<RefundSendMoneyRequest, MGI.Biz.MoneyTransfer.Data.RefundRequest>(moneyTransferRefund);
            long ptnrTrxId = bizMoneyTransferEngine.StageRefund(customerSessionId, refundSendMoney, mgiContext);

            BIZShoppingCartService.AddMoneyTransfer(customerSessionId, ptnrTrxId, mgiContext);
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(ptnrTrxId), "StageRefundSendMoney", AlloyLayerName.SERVICE, ModuleName.SendMoney,
                                      "End StageRefundSendMoney-MGI.Channel.Shared.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return ptnrTrxId;
        }
       
    }
}
