using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using TCF.Channel.Zeo.Data;
using CoreData = TCF.Zeo.Core.Data;
using TCF.Zeo.Biz.MoneyTransfer.Contract;
using CxnService = TCF.Zeo.Cxn.MoneyTransfer;
using TCF.Zeo.Processor;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Biz.Common.Data;
using BizCommon = TCF.Zeo.Biz.Common;
using TCF.Zeo.Common.Util;
using AutoMapper;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Impl;
using TCF.Zeo.Core.Contract;

namespace TCF.Zeo.Biz.MoneyTransfer.Impl
{
    public class MoneyTransferEngineImpl : IMoneyTransferEngine
    {
        #region Properties

        private bool IsHardCodedCounterId { get; } = Convert.ToBoolean(ConfigurationManager.AppSettings["IsHardCodedCounterId"]);

        #endregion

        #region Depedencies 

        ProcessorRouter processorRouter = new ProcessorRouter();
        public IMoneyTransferService CoreMoneyTransferService { get; set; }
        public CxnService.Contract.IMoneyTransferService CxnMoneyTransferService { get; set; }
        public BizCommon.Contract.IComplianceService complianceService { get; set; }

        public ILocationCounterIdService LocationCounterIdService { get; set; }
        public IMapper Mapper { get; set; }

        #endregion


        public MoneyTransferEngineImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CxnService.Data.CardDetails, CardDetails>();
                cfg.CreateMap<CardDetails, CxnService.Data.CardDetails>();
                cfg.CreateMap<CxnService.Data.MasterData, MasterData>();
                cfg.CreateMap<MasterData, CxnService.Data.MasterData>();
                cfg.CreateMap<DeliveryService, CxnService.Data.DeliveryService>();
                cfg.CreateMap<CxnService.Data.DeliveryService, DeliveryService>();
                cfg.CreateMap<DeliveryServiceRequest, CxnService.Data.DeliveryServiceRequest>();
                cfg.CreateMap<ZeoContext, commonData.ZeoContext>();
                cfg.CreateMap<Receiver, CxnService.Data.Receiver>();
                cfg.CreateMap<CxnService.Data.Receiver, Receiver>();
                cfg.CreateMap<CxnService.Data.FeeRequest, FeeRequest>();
                cfg.CreateMap<FeeRequest, CxnService.Data.FeeRequest>();
                cfg.CreateMap<FeeResponse, CxnService.Data.FeeResponse>();
                cfg.CreateMap<CxnService.Data.FeeResponse, FeeResponse>();
                cfg.CreateMap<ValidateResponse, CxnService.Data.ValidateResponse>();
                cfg.CreateMap<CxnService.Data.ValidateResponse, ValidateResponse>();
                cfg.CreateMap<CxnService.Data.FeeInformation, FeeInformation>();
                cfg.CreateMap<FeeInformation, CxnService.Data.FeeInformation>();
                cfg.CreateMap<ValidateRequest, CxnService.Data.ValidateRequest>()
                    .ForMember(x => x.PromotionsCode, s => s.MapFrom(c => c.PromoCode));

                cfg.CreateMap<CxnService.Data.ValidateRequest, ValidateRequest>();
                cfg.CreateMap<CardLookupDetails, CxnService.Data.CardLookupDetails>();
                cfg.CreateMap<CxnService.Data.CardLookupDetails, CardLookupDetails>();
                cfg.CreateMap<CoreData.ModifyResponse, ModifyResponse>();
                cfg.CreateMap<CxnService.Data.SearchResponse, SearchResponse>();
                cfg.CreateMap<SearchResponse, CxnService.Data.SearchResponse>();
                cfg.CreateMap<CxnService.Data.Reason, Reason>();
                cfg.CreateMap<Reason, CxnService.Data.Reason>();
                cfg.CreateMap<CxnService.Data.ReasonRequest, ReasonRequest>();
                cfg.CreateMap<ReasonRequest, CxnService.Data.ReasonRequest>();
                cfg.CreateMap<MoneyTransferTransaction, CxnService.Data.WUTransaction>();
                cfg.CreateMap<CxnService.Data.WUTransaction, MoneyTransferTransaction>()
                    .ForMember(x => x.Fee, s => s.MapFrom(c => c.Charges));
                cfg.CreateMap<CxnService.Data.RefundRequest, SendMoneyRefundRequest>();
                cfg.CreateMap<SendMoneyRefundRequest, CxnService.Data.RefundRequest>();
                cfg.CreateMap<SendMoneyRefundRequest, CxnService.Data.RefundRequest>()
               .ForMember(d => d.ReasonCode, s => s.MapFrom(c => c.CategoryCode))
               .ForMember(d => d.ReasonDesc, s => s.MapFrom(c => c.CategoryDescription))
               .ForMember(d => d.Comments, s => s.MapFrom(c => c.Comments));
                cfg.CreateMap<CoreData.Country, Country>();
                cfg.CreateMap<CoreData.WUCountry, WUCountry>();
            });

            Mapper = config.CreateMapper();
        }

        public long AddReceiver(Receiver receiver, commonData.ZeoContext context)
        {
            try
            {
                CxnService.Data.Receiver cxnReceiver = Mapper.Map<Receiver, CxnService.Data.Receiver>(receiver);
                cxnReceiver.DTServerCreate = DateTime.Now;
                cxnReceiver.DTTerminalCreate = GetTimeZoneTime(context.TimeZone);
                cxnReceiver.CustomerId = context.CustomerId;
                return processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).AddReceiver(cxnReceiver);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_ADDRECEIVER_FAILED, ex);
            }
        }

        public long UpdateReceiver(Receiver receiver, commonData.ZeoContext context)
        {
            try
            {
                CxnService.Data.Receiver cxnReceiver = Mapper.Map<Receiver, CxnService.Data.Receiver>(receiver);

                cxnReceiver.DTServerLastModified = DateTime.Now;
                cxnReceiver.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
                cxnReceiver.CustomerId = context.CustomerId;
                return processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).UpdateReceiver(cxnReceiver);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                //throw new BizMoneyTransferException(BizMoneyTransferException, ex);
                throw ex;
            }
        }

        public Receiver GetReceiver(long receiverId, commonData.ZeoContext context)
        {
            try
            {
                CxnService.Data.Receiver cxnReceiver = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetReceiver(receiverId);

                return Mapper.Map<CxnService.Data.Receiver, Receiver>(cxnReceiver);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETRECEIVER_FAILED, ex);
            }
        }

        public bool DeleteFavoriteReceiver(long receiverId, commonData.ZeoContext context)
        {
            try
            {
                CxnService.Data.Receiver receiver = new CxnService.Data.Receiver()
                {
                    Id = receiverId,
                    Status = "Inactive",
                    DTServerLastModified = DateTime.Now,
                    DTTerminalLastModified = GetTimeZoneTime(context.TimeZone)
                };

                return processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).DeleteFavoriteReceiver(receiver);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_DELETEFAVORITERECEIVER_FAILED, ex);
            }
        }

        public List<Receiver> GetFrequentReceivers(commonData.ZeoContext context)
        {
            try
            {
                List<CxnService.Data.Receiver> receivers = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetFrequentReceivers(context.CustomerId);

                return Mapper.Map<List<CxnService.Data.Receiver>, List<Receiver>>(receivers);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETFREQUENTRECEIVERS_FAILED, ex);
            }
        }

        /// <summary>
        /// Returns Cities based on state code
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        public List<MasterData> GetXfrCities(string stateCode, commonData.ZeoContext context)
        {
            try
            {
                List<CxnService.Data.MasterData> Cities = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetXfrCities(stateCode);

                List<MasterData> Masterdata = Mapper.Map<List<CxnService.Data.MasterData>, List<MasterData>>(Cities);

                return Masterdata;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCITIES_FAILED, ex);
            }
        }

        /// <summary>
        /// returns Countries Master Data
        /// </summary>
        /// <returns>MasterData List</returns>
        public List<MasterData> GetXfrCountries(commonData.ZeoContext context)
        {
            try
            {
                List<CxnService.Data.MasterData> countries = new List<CxnService.Data.MasterData>();
                var processor = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName);

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["ConsiderBlockedCountries"]))
                    countries = processor.GetUnblockedCountries(context);
                else
                    countries = processor.GetXfrCountries();

                List<MasterData> Masterdata = Mapper.Map<List<CxnService.Data.MasterData>, List<MasterData>>(countries);

                return Masterdata;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCOUNTRIES_FAILED, ex);
            }
        }
        /// <summary>
        /// returns states based country code
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public List<MasterData> GetXfrStates(string countryCode, commonData.ZeoContext context)
        {
            try
            {
                List<CxnService.Data.MasterData> States = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetXfrStates(countryCode);

                List<MasterData> Masterdata = Mapper.Map<List<CxnService.Data.MasterData>, List<MasterData>>(States);

                return Masterdata;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETSTATES_FAILED, ex);
            }
        }

        public ValidateResponse Validate(ValidateRequest validateRequest, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();

                CxnService.Data.ValidateRequest cxnValidateRequest = Mapper.Map<ValidateRequest, CxnService.Data.ValidateRequest>(validateRequest);
                if (context.ProviderId == 0)
                {
                    context.ProviderId = processorRouter.GetMoneyTransferProvider(context.ChannelPartnerName);
                }
                if (validateRequest.TransferType == MoneyTransferType.Send)
                {
                    complianceService = new BizCommon.Impl.ComplianceServiceImpl();

                    Limit limit = complianceService.GetTransactionLimit(Helper.TransactionType.MoneyTransfer, context);

                    if (validateRequest.Amount < limit.PerTransactionMinimum)
                    {
                        throw new BizComplianceLimitException(MoneyTransferException.MoneyTransferProductCode, BizComplianceLimitException.MINIMUM_LIMIT_FAILED, limit.PerTransactionMinimum.ToString(), Convert.ToDecimal(limit.PerTransactionMinimum));
                    }
                    if (validateRequest.Amount > limit.PerTransactionMaximum)
                    {
                        throw new BizComplianceLimitException(MoneyTransferException.MoneyTransferProductCode, BizComplianceLimitException.MAXIMUM_LIMIT_FAILED, limit.PerTransactionMaximum.ToString(), Convert.ToDecimal(limit.PerTransactionMaximum));
                    }
                }

                //If we have PTNR Id available then we need to get the CXN Id of PTNR record and pass it to FeeRequest.
                CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(validateRequest.TransactionId, context);
                cxnValidateRequest.TransactionId = moneyTransferTran.WUTrxId;

                CxnService.Data.ValidateResponse cxnValidateResponse = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).Validate(cxnValidateRequest, context);
                cxnValidateResponse.TransactionId = validateRequest.TransactionId;

                return Mapper.Map<CxnService.Data.ValidateResponse, ValidateResponse>(cxnValidateResponse);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_VALIDATE_FAILED, ex);
            }

        }

        public int Commit(long ptnrTransactionId, commonData.ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope(TransactionScopeOptions.RequiresNew))
            {
                try
                {
                    CoreMoneyTransferService = new ZeoCoreImpl();

                    context.SMTrxType = (RequestType.Release).ToString();
                    ///While commiting receive money we need to send request release mode.
                    context.RMTrxType = (RequestType.Release).ToString();

                    CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(ptnrTransactionId, context);
                    processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).Commit(moneyTransferTran.WUTrxId, context);
                    using (CoreMoneyTransferService = new ZeoCoreImpl())
                    {
                        CoreMoneyTransferService.UpdateTransactionState(ptnrTransactionId, (int)Helper.TransactionStates.Committed, Helper.GetTimeZoneTime(context.TimeZone), DateTime.Now, context);
                    }

                    scope.Complete();

                    return 0;
                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                    throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_COMMIT_FAILED, ex);
                }
            }
              
        }

        public FeeResponse GetFee(FeeRequest feeRequest, commonData.ZeoContext context)
        {
            long tranId = 0;
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                FeeResponse feeResponse = new FeeResponse();

                //long transactionId = 0;
                if (context.ProviderId == 0)
                {
                    context.ProviderId = processorRouter.GetMoneyTransferProvider(context.ChannelPartnerName);
                }

                long cxnAccountId = CreateCXNAccount(context);
                feeRequest.AccountId = cxnAccountId;
                feeRequest.ReferenceNo = DateTime.Now.ToString("yyyyMMddhhmmssff");

                tranId = feeRequest.TransactionId;

                // If the PTNR is not exists, create a new record and while passing the FeeRequest to CXN it will pass, TransactionId as 0.
                if (tranId == 0)
                {
                    tranId = CreatePTNRTransaction(cxnAccountId, feeRequest, context);
                    //feeRequest.TransactionId = transactionId;
                }
                else
                {
                    //If we have PTNR Id available then we need to get the CXN Id of PTNR record and pass it to FeeRequest.
                    CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(feeRequest.TransactionId, context);
                    feeRequest.TransactionId = moneyTransferTran.WUTrxId;
                }

                CxnService.Data.FeeRequest cxnFeeRequest = Mapper.Map<CxnService.Data.FeeRequest>(feeRequest);

                CxnService.Data.FeeResponse cxnFeeResponse = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetFee(cxnFeeRequest, context);

                CxnService.Data.FeeInformation feeInfo = cxnFeeResponse.FeeInformations.FirstOrDefault();

                //Calculating Fee based on the Fee response and update the Fee value in PTNR table.
                decimal transferTax = Convert.ToDecimal(GetDecimalDictionaryValueIfExists(feeInfo.MetaData, "TransferTax"));
                decimal fee = feeInfo.Fee + transferTax;
                if (feeInfo.MetaData != null)
                {
                    decimal plusCharges = Convert.ToDecimal(GetDecimalDictionaryValueIfExists(feeInfo.MetaData, "PlusCharges"));
                    fee = fee + plusCharges + feeInfo.MessageFee;
                }

                CoreData.MoneyTransfer moneyTransfer = new CoreData.MoneyTransfer()
                {
                    WUTrxId = cxnFeeResponse.WuTrxId,
                    Fee = fee,
                    Amount = feeInfo.Amount,
                    Id = tranId,
                    DTServerLastModified = DateTime.Now,
                    DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone)
                };

                CoreMoneyTransferService.UpdateTransaction(moneyTransfer, context);

                feeResponse = Mapper.Map<CxnService.Data.FeeResponse, FeeResponse>(cxnFeeResponse);

                feeResponse.TransactionId = tranId;

                return feeResponse;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETFEE_FAILED, ex);
            }
        }

        public List<MasterData> GetCurrencyCodeList(string countryCode, commonData.ZeoContext context)
        {
            try
            {
                List<CxnService.Data.MasterData> masterData = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetCurrencyCodeList(countryCode);

                return Mapper.Map<List<CxnService.Data.MasterData>, List<MasterData>>(masterData);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCURRENCYCODELIST_FAILED, ex);
            }
        }

        public SearchResponse Search(SearchRequest request, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                CoreData.ModifyResponse refundResp = new CoreData.ModifyResponse();
                CoreData.MoneyTransfer moneyTransfer = new CoreData.MoneyTransfer();

                if (context.ProviderId == 0)
                {
                    context.ProviderId = processorRouter.GetMoneyTransferProvider(context.ChannelPartnerName);
                }

                CxnService.Data.SearchRequest searchRequest = new CxnService.Data.SearchRequest();
                CxnService.Data.SearchResponse searchResponse = new CxnService.Data.SearchResponse();

                if (request.SearchRequestType == Helper.SearchRequestType.Refund || request.SearchRequestType == Helper.SearchRequestType.RefundWithStage)
                {
                    searchRequest.ReasonCode = request.ReasonCode;
                    searchRequest.ReasonDesc = request.ReasonDesc;
                    searchRequest.Comments = request.Comments;

                    if (request.SearchRequestType == Helper.SearchRequestType.Refund)
                        searchRequest.SearchRequestType = Helper.SearchRequestType.Refund;
                    else
                    {
                        searchRequest.SearchRequestType = Helper.SearchRequestType.RefundWithStage;

                        moneyTransfer = CoreMoneyTransferService.GetTransaction(request.TransactionId, context);
                        searchRequest.TransactionId = moneyTransfer.WUTrxId;
                    }
                }
                else if (request.SearchRequestType == Helper.SearchRequestType.LookUp)
                {
                    searchRequest.SearchRequestType = Helper.SearchRequestType.LookUp;
                }
                else
                {
                    searchRequest.SearchRequestType = Helper.SearchRequestType.Modify;
                }


                if (request.SearchRequestType == Helper.SearchRequestType.RefundWithStage)
                {
                    moneyTransfer.Id = request.TransactionId;
                    moneyTransfer.OriginalTransactionID = request.TransactionId;
                    moneyTransfer.ProviderId = context.ProviderId;
                    moneyTransfer.CustomerSessionId = context.CustomerSessionId;
                    moneyTransfer.TransactionSubType = TransactionSubType.Refund;
                    moneyTransfer.MoneyTransferType = MoneyTransferType.Send;
                    moneyTransfer.DTServerCreate = DateTime.Now;
                    moneyTransfer.DTTerminalCreate = GetTimeZoneTime(context.TimeZone);
                    moneyTransfer.State = TransactionStates.Initiated;

                    refundResp = CoreMoneyTransferService.AddModifyandRefundTransactions(moneyTransfer, context);
                    searchRequest.CancelTransactionId = refundResp.CancelTransactionId;
                    searchRequest.ModifyOrRefundTransactionId = refundResp.ModifyTransactionId;
                }

                searchRequest.ConfirmationNumber = request.ConfirmationNumber;

                CxnService.Data.SearchResponse cxnResponse =
                    processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).Search(searchRequest, context);

                SearchResponse bizSearchResponse = Mapper.Map<CxnService.Data.SearchResponse, SearchResponse>(cxnResponse);

                //Update the PTNR records with the CXN Ids obtained from CXN.
                if (request.SearchRequestType == SearchRequestType.RefundWithStage)
                {
                    CoreMoneyTransferService.UpdateModifyorRefundTransactions(refundResp, cxnResponse.CancelTransactionId, cxnResponse.RefundTransactionId, Helper.GetTimeZoneTime(context.TimeZone)
                                        , DateTime.Now, context);
                }

                bizSearchResponse.TransactionId = Convert.ToString(searchRequest.ModifyOrRefundTransactionId);
                return bizSearchResponse;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SEARCH_FAILED, ex);
            }
        }

        public CardDetails WUCardEnrollment(commonData.ZeoContext context)
        {
            try
            {
                CxnService.Data.CardDetails cxnCardDetails = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).WUCardEnrollment(context);

                CardDetails cDetails = Mapper.Map<CxnService.Data.CardDetails, CardDetails>(cxnCardDetails);

                string WUGoldCardNumber = cDetails.AccountNumber;
                if (WUGoldCardNumber != string.Empty)
                {
                    CxnService.Data.Account account = new CxnService.Data.Account()
                    {
                        LoyaltyCardNumber = WUGoldCardNumber,
                        DTServerLastModified = DateTime.Now,
                        DTTerminalLastModified = GetTimeZoneTime(context.TimeZone)
                    };
                    processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).UpdateAccountWithCardNumber(context.CustomerSessionId, account);
                }

                return cDetails;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_WUCARDENROLLMENT_FAILED, ex);
            }
        }

        public List<DeliveryService> GetDeliveryServices(DeliveryServiceRequest request, commonData.ZeoContext context)
        {
            try
            {
                if (context.ProviderId == 0)
                {
                    context.ProviderId = processorRouter.GetMoneyTransferProvider(context.ChannelPartnerName);
                }
                CxnService.Data.DeliveryServiceRequest cxnDeliveryServiceRequest = Mapper.Map<CxnService.Data.DeliveryServiceRequest>(request);

                List<CxnService.Data.DeliveryService> cxnDeliveryServices = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetDeliveryServices(cxnDeliveryServiceRequest, context);

                List<DeliveryService> delivery = Mapper.Map<List<CxnService.Data.DeliveryService>, List<DeliveryService>>(cxnDeliveryServices);

                return delivery;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETDELIVERYSERVICES_FAILED, ex);
            }
        }

        public void AddPastReceivers(string cardNumber, commonData.ZeoContext context)
        {
            try
            {
                if (!String.IsNullOrEmpty(context.ProductType) && (context.ProductType.ToUpper() == "SENDMONEY"))
                {
                    processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetPastReceivers(context.CustomerId, cardNumber, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_ADDPASTRECEIVERS_FAILED, ex);
            }

        }

        public CardLookupDetails WUCardLookup(CardLookupDetails lookupDetails, commonData.ZeoContext context)
        {
            try
            {
                CxnService.Data.CardLookupDetails cxnCardLookupDetails = Mapper.Map<CardLookupDetails, CxnService.Data.CardLookupDetails>(lookupDetails);

                cxnCardLookupDetails = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).WUCardLookup(cxnCardLookupDetails, context);

                CardLookupDetails bizCardLookupDetails = new CardLookupDetails();

                bizCardLookupDetails.Sender = new Account[cxnCardLookupDetails.Sender.Count()];

                for (int i = 0; i < cxnCardLookupDetails.Sender.Length; i++)
                {
                    bizCardLookupDetails.Sender[i] = new Account()
                    {
                        Address = cxnCardLookupDetails.Sender[i].Address,
                        FirstName = cxnCardLookupDetails.Sender[i].FirstName,
                        LastName = cxnCardLookupDetails.Sender[i].LastName,
                        MiddleName = cxnCardLookupDetails.Sender[i].MiddleName,
                        SecondLastName = cxnCardLookupDetails.Sender[i].SecondLastName,
                        LoyaltyCardNumber = cxnCardLookupDetails.Sender[i].LoyaltyCardNumber,
                        MobilePhone = cxnCardLookupDetails.Sender[i].MobilePhone,
                        PostalCode = cxnCardLookupDetails.Sender[i].PostalCode
                    };
                }

                return bizCardLookupDetails;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_WUCARDLOOKUP_FAILED, ex);
            }
        }

        public void Cancel(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                CoreMoneyTransferService.UpdateTransactionState(transactionId, (int)Helper.TransactionStates.Canceled, GetTimeZoneTime(context.TimeZone), DateTime.Now, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_CANCEL_FAILED, ex);
            }
        }


        public bool UpdateAccount(string WUGoldCardNumber, commonData.ZeoContext context)
        {
            try
            {
                bool isUpdated = false;

                isUpdated = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).UseGoldcard(WUGoldCardNumber, context);

                return isUpdated;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATEACCOUNT_FAILED, ex);
            }

        }

        public string GetBannerMsgs(long agentSessionId, commonData.ZeoContext context)
        {
            try
            {

                List<CxnService.Data.MasterData> bannermsgs = new List<CxnService.Data.MasterData>();
                LocationCounterIdService = new ZeoCoreImpl();
                string channerPartnerName = context.ChannelPartnerName;
                string bannerMessage = string.Empty;
                string counterId = string.Empty;
                int providerId = processorRouter.GetMoneyTransferProvider(channerPartnerName);
                if (!string.IsNullOrWhiteSpace(context.LocationId))
                {
                    context.WUCounterId = LocationCounterIdService.GetLocationCounterID(Convert.ToInt64(context.LocationId), providerId, context);
                }
                if (processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName) != null)
                {
                    bannermsgs = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetBannerMsgs(context);
                    foreach (var item in bannermsgs)
                    {
                        bannerMessage += item.Name + " ";
                    }
                }
                return bannerMessage;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETBANNERMSGS_FAILED, ex);
            }
        }

        public int Modify(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                DateTime dtTerminalDate = GetTimeZoneTime(context.TimeZone);
                DateTime dtServerDate = DateTime.Now;

                CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(transactionId, context);

                processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).Modify(moneyTransferTran.WUTrxId, context);

                CoreMoneyTransferService = new ZeoCoreImpl();

                CoreMoneyTransferService.UpdateTransactionStates(transactionId,(int)Helper.TransactionStates.Committed, dtTerminalDate, dtServerDate, context);

                //Update the receiver details.
                UpdateWUReceiverDetails(transactionId, context);

                return (int)TransactionStates.Committed;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_MODIFY_FAILED, ex);
            }
        }

        public ModifyResponse StageModify(ModifyRequest modifySendMoney, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();

                CoreData.MoneyTransfer moneyTransfer = new CoreData.MoneyTransfer()
                {
                    Id = modifySendMoney.TransactionId,
                    CustomerSessionId = context.CustomerSessionId,
                    State = TransactionStates.Initiated,
                    TransactionSubType = TransactionSubType.Modify,
                    RecipientId = modifySendMoney.RecipientId,
                    OriginalTransactionID = modifySendMoney.TransactionId,
                    DTServerCreate = DateTime.Now,
                    DTTerminalCreate = GetTimeZoneTime(context.TimeZone)
                };

                CoreData.ModifyResponse modifyResp = CoreMoneyTransferService.AddModifyandRefundTransactions(moneyTransfer, context);

                CxnService.Data.ModifyRequest cxnModifyRequest = new CxnService.Data.ModifyRequest();
                cxnModifyRequest.FirstName = modifySendMoney.FirstName;
                cxnModifyRequest.SecondLastName = modifySendMoney.SecondLastName;
                cxnModifyRequest.MiddleName = modifySendMoney.MiddleName;
                cxnModifyRequest.LastName = modifySendMoney.LastName;
                cxnModifyRequest.TestQuestion = modifySendMoney.TestQuestion;
                cxnModifyRequest.TestAnswer = modifySendMoney.TestAnswer;

                //Passing WU Transaction Id from PTNR Id 
                CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(modifySendMoney.TransactionId, context);
                cxnModifyRequest.TransactionId = moneyTransferTran.WUTrxId;

                CxnService.Data.ModifyResponse modifyResponse = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).StageModify(cxnModifyRequest, context);

                //Update the PTNR records with the CXN Ids obtained from CXN.
                CoreMoneyTransferService.UpdateModifyorRefundTransactions(modifyResp, modifyResponse.CancelTransactionId, modifyResponse.ModifyTransactionId, Helper.GetTimeZoneTime(context.TimeZone)
                                    , DateTime.Now, context);

                return Mapper.Map<CoreData.ModifyResponse, ModifyResponse>(modifyResp);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_STAGEMODIFY_FAILED, ex);
            }
        }

        public List<Reason> GetRefundReasons(ReasonRequest request, commonData.ZeoContext context)
        {
            try
            {
                CxnService.Data.ReasonRequest cxnReasonRequest = Mapper.Map<ReasonRequest, CxnService.Data.ReasonRequest>(request);

                List<CxnService.Data.Reason> cxnReasons = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetRefundReasons(cxnReasonRequest, context);

                return Mapper.Map<List<CxnService.Data.Reason>, List<Reason>>(cxnReasons);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETREFUNDREASONS_FAILED, ex);
            }
        }

        public MoneyTransferTransaction GetTransaction(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();

                CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(transactionId, context);

                CxnService.Data.WUTransaction wuMoneyTransferTransaction = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetWUTransaction(moneyTransferTran.WUTrxId);

                MoneyTransferTransaction wuTran = Mapper.Map<MoneyTransferTransaction>(wuMoneyTransferTransaction);

                //Passing the PTNR transaction Id to the above layers as whenever we need to talk to WU, we will get CXN Id from PTNR Id.
                wuTran.TransactionId = transactionId;

                //Author : Abhijith
                //Description : Dont show the Fee component for Receive Money (Transfer type = 2) as fee should not be 
                // included for receive money in Transaction History.
                //Starts Here

                if (wuTran.TranascationType != Helper.MoneyTransferType.Receive.ToString())
                    wuTran.Fee = wuTran.Fee + wuTran.message_charge
                        + wuTran.plus_charges_amount
                        + wuTran.municipal_tax + wuTran.state_tax + wuTran.county_tax;
                //Ends Here

                wuTran.IsModifiedOrRefunded = IsSendMoneyModifyRefundAvailable(transactionId, context);

                wuTran.MetaData = new Dictionary<string, object>();
                wuTran.MetaData.Add("AdditionalCharges", wuTran.AdditionalCharges);
                wuTran.MetaData.Add("MessageCharge", wuTran.message_charge);
                wuTran.MetaData.Add("PaySideCharges", wuTran.PaySideCharges);
                wuTran.MetaData.Add("OtherCharges", wuTran.OtherCharges);
                wuTran.MetaData.Add("IsFixOnSend", wuTran.IsFixedOnSend);
                wuTran.MetaData.Add("PlusChargesAmount", wuTran.plus_charges_amount);
                wuTran.MetaData.Add("DeliveryOption", wuTran.DeliveryOption);
                wuTran.MetaData.Add("DeliveryOptionDesc", wuTran.DeliveryOptionDesc);
                wuTran.MetaData.Add("FilingDate", wuTran.FilingDate);
                wuTran.MetaData.Add("FilingTime", wuTran.FilingTime);
                wuTran.MetaData.Add("PaidDateTime", wuTran.PaidDateTime);
                wuTran.MetaData.Add("PhoneNumber", wuTran.PhoneNumber);
                wuTran.MetaData.Add("Url", wuTran.Url);
                wuTran.MetaData.Add("AgencyName", wuTran.AgencyName);
                wuTran.MetaData.Add("ExpectedPayoutCity", wuTran.ExpectedPayoutCityName);
                wuTran.MetaData.Add("TransferTax", wuTran.municipal_tax + wuTran.state_tax + wuTran.county_tax);
                wuTran.MetaData.Add("PaySideTax", wuTran.PaySideTax);
                wuTran.MetaData.Add("TransalatedDeliveryServiceName", wuTran.TransalatedDeliveryServiceName);
                wuTran.MetaData.Add("MessageArea", wuTran.MessageArea);
                wuTran.MetaData.Add("ConsumerFraudPromptQuestion", wuTran.ConsumerFraudPromptQuestion);

                return wuTran;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GET_FAILED, ex);
            }
        }

        public bool IsSendMoneyModifyRefundAvailable(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                return processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).IsSendMoneyModifyRefundAvailable(transactionId, context.CustomerId);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_ISSENDMONEYMODIFYREFUNDAVAILABLE_FAILED, ex);
            }
        }

        public string GetStatus(string confirmationNumber, commonData.ZeoContext context)
        {
            try
            {
                return processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetStatus(confirmationNumber, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETSTATUS_FAILED, ex);
            }
        }

        public bool IsSWBStateXfer(commonData.ZeoContext context)
        {
            try
            {
                bool IsSWBState = false;
                List<string> swbStates = new List<string>() { "AZ", "CA", "NM", "TX" };
                if (!string.IsNullOrWhiteSpace(context.StateCode))
                {
                    IsSWBState = swbStates.Contains(context.StateCode);
                }
                return IsSWBState;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_ISSWBSTATE_FAILED, ex);
            }
        }

        public string GetCurrencyCode(string countryCode, commonData.ZeoContext context)
        {
            try
            {
                return processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetCurrencyCode(countryCode);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCURRENCYCODE_FAILED, ex);
            }
        }

        public void AuthorizeModify(ModifyRequest modifySendMoney, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();

                CoreMoneyTransferService.UpdateTransactionStates(modifySendMoney.ModifyTransactionId, (int)TransactionStates.Authorized, GetTimeZoneTime(context.TimeZone), DateTime.Now, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_AUTHORIZEMODIFY_FAILED, ex);
            }
        }

        public MoneyTransferTransaction GetReceiveMoney(ReceiveMoneyRequest request, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                long transactionId = 0L;

                if (context.ProviderId == 0)
                {
                    context.ProviderId = processorRouter.GetMoneyTransferProvider(context.ChannelPartnerName);
                }
                context.ReferenceNumber = DateTime.Now.ToString("yyyyMMddhhmmssff");

                long cxnAccountId = CreateCXNAccount(context);

                //context.RMMTCN = request.ConfirmationNumber;

                transactionId = WritePTNRRMMoneyTransfer(cxnAccountId, request.ConfirmationNumber, context);

                CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(transactionId, context);

                CxnService.Data.WUTransaction cxnTransaction = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetReceiveTransaction(request.ConfirmationNumber, moneyTransferTran.WUTrxId, context);

                MoneyTransferTransaction recTrxn = new MoneyTransferTransaction();
                recTrxn.SenderName = cxnTransaction.SenderName;
                recTrxn.DestinationPrincipalAmount = cxnTransaction.AmountToReceiver;
                recTrxn.DestinationCurrencyCode = cxnTransaction.DestinationCurrencyCode;
                recTrxn.TestQuestion = cxnTransaction.TestQuestion;
                recTrxn.TestAnswer = cxnTransaction.TestAnswer;
                recTrxn.MetaData = new Dictionary<string, object>();
                recTrxn.MetaData["ReceiverName"] = cxnTransaction.MetaData["ReceiverName"];
                recTrxn.MetaData["SenderStateCode"] = cxnTransaction.MetaData["SenderStateCode"];
                recTrxn.MetaData["NetAmount"] = cxnTransaction.MetaData["NetAmount"];

                //Update the Transaction with the CXNId obtained from the provider.
                moneyTransferTran.Amount = cxnTransaction.AmountToReceiver;
                moneyTransferTran.WUTrxId = cxnTransaction.Id;
                moneyTransferTran.Fee = 0.00M;
                moneyTransferTran.Id = transactionId;
                moneyTransferTran.DTServerLastModified = DateTime.Now;
                moneyTransferTran.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
                CoreMoneyTransferService.UpdateTransaction(moneyTransferTran, context);

                //Changed from CXN Id to PTNR Id as it shud always have the PTNR Id. 
                recTrxn.TransactionId = transactionId;

                return recTrxn;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETRECEIVERLASTTRANSACTION_FAILED, ex);
            }

        }

        private long WritePTNRRMMoneyTransfer(long cxnAccountId, string confirmationNumber, commonData.ZeoContext context)
        {
            CoreMoneyTransferService = new ZeoCoreImpl();

            CoreData.MoneyTransfer moneyTransfer = new CoreData.MoneyTransfer();

            moneyTransfer.ConfirmationNumber = confirmationNumber;
            moneyTransfer.DTServerCreate = DateTime.Now;
            moneyTransfer.DTTerminalCreate = GetTimeZoneTime(context.TimeZone);
            moneyTransfer.State = TransactionStates.Initiated;
            moneyTransfer.ProviderId = context.ProviderId;
            moneyTransfer.CustomerSessionId = context.CustomerSessionId;
            moneyTransfer.MoneyTransferType = MoneyTransferType.Receive;
            moneyTransfer.ProviderAccountId = cxnAccountId;

            return CoreMoneyTransferService.CreateTransaction(moneyTransfer, context);

        }

        private long CreatePTNRTransaction(long cxnAccountId, FeeRequest feeRequest, commonData.ZeoContext context)
        {
            CoreMoneyTransferService = new ZeoCoreImpl();

            CoreData.MoneyTransfer moneyTransfer = new CoreData.MoneyTransfer();
            moneyTransfer.Amount = feeRequest.Amount;
            moneyTransfer.DTServerCreate = DateTime.Now;
            moneyTransfer.DTTerminalCreate = GetTimeZoneTime(context.TimeZone);
            moneyTransfer.MoneyTransferType = MoneyTransferType.Send;
            moneyTransfer.State = TransactionStates.Initiated;
            moneyTransfer.CustomerSessionId = context.CustomerSessionId;
            moneyTransfer.OriginalTransactionID = feeRequest.TransactionId;
            moneyTransfer.ProviderId = context.ProviderId;
            moneyTransfer.ProviderAccountId = cxnAccountId;
            moneyTransfer.RecipientId = feeRequest.ReceiverId;
            return CoreMoneyTransferService.CreateTransaction(moneyTransfer, context);
        }

        private long CreateCXNAccount(commonData.ZeoContext context)
        {
            CxnService.Data.Account account = new CxnService.Data.Account();
            account.CustomerID = context.CustomerId;
            account.CustomerSessionId = context.CustomerSessionId;
            account.DTServerCreate = DateTime.Now;
            account.DTTerminalCreate = GetTimeZoneTime(context.TimeZone);
            account.LoyaltyCardNumber = string.Empty;
            account.LevelCode = string.Empty;
            return processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).AddAccount(account, context);
        }

        private void UpdateWUReceiverDetails(long transactionId, commonData.ZeoContext context)
        {
            CoreMoneyTransferService = new ZeoCoreImpl();
            DateTime dtServerDate = DateTime.Now;
            DateTime dtTerminalDate = GetTimeZoneTime(context.TimeZone);

            CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(transactionId, context);

            processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName)
                .UpdateReceiverDetails(moneyTransferTran.WUTrxId, context.CustomerSessionId, dtTerminalDate, dtServerDate);
        }

        public string Refund(SendMoneyRefundRequest refundSendMoney, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                CoreData.MoneyTransfer moneyTransfer = new CoreData.MoneyTransfer();
                SendMoneyRefundRequest refundRequest = new SendMoneyRefundRequest()
                {
                    CategoryDescription = refundSendMoney.CategoryDescription,
                    CategoryCode = refundSendMoney.CategoryCode,
                    Reason = refundSendMoney.Reason,
                    Comments = refundSendMoney.Comments,
                    RefundStatus = refundSendMoney.RefundStatus,
                   // TransactionId = refundSendMoney.TransactionId
                };

                CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(long.Parse(refundSendMoney.TransactionId), context);
                refundRequest.TransactionId = Convert.ToString(moneyTransferTran.WUTrxId);

                CxnService.Data.RefundRequest cxnRefundRequest = Mapper.Map<SendMoneyRefundRequest, CxnService.Data.RefundRequest>(refundRequest);
                string confirmationNumber = processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).Refund(cxnRefundRequest, context);
                CoreMoneyTransferService.UpdateTransactionStates(long.Parse(refundSendMoney.TransactionId), (int)TransactionStates.Committed, GetTimeZoneTime(context.TimeZone), DateTime.Now, context);
                return confirmationNumber;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_REFUND_FAILED, ex);
            }
        }

        public void CancelSendMoneyModify(long modifyTransactionId, long cancelTransactionId, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                CoreMoneyTransferService.UpdateTransactionStates(modifyTransactionId, (int)TransactionStates.Canceled, GetTimeZoneTime(context.TimeZone), DateTime.Now, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_CANCEL_FAILED, ex);
            }
        }

        public void CancelSendMoney(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                bool isTrue = CoreMoneyTransferService.UpdatePTNRTransactionStates(transactionId, (int)Helper.TransactionStates.Canceled, GetTimeZoneTime(context.TimeZone), DateTime.Now, context);

                if (isTrue)
                {
                    context.SMTrxType = (RequestType.Cancel).ToString();
                    context.RMTrxType = (RequestType.Cancel).ToString();

                    CoreData.MoneyTransfer moneyTransferTran = CoreMoneyTransferService.GetTransaction(transactionId, context);
                    processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).Commit(moneyTransferTran.WUTrxId, context);
                }
            }
            catch (Exception)
            {
                CoreMoneyTransferService = new ZeoCoreImpl();

                CoreMoneyTransferService.UpdateTransactionState(transactionId, (int)Helper.TransactionStates.Failed, GetTimeZoneTime(context.TimeZone), DateTime.Now, context);
            }
        }

        public decimal GetFraudLimit(string countryCode, commonData.ZeoContext context)
        {
            try
            {
                return processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetFraudLimit(countryCode, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_FRAUDLIMIT_FAILED, ex);
            }
        }

        public decimal GetDestinationAmount(FeeRequest feeRequest, commonData.ZeoContext context)
        {
            try
            {
                CxnService.Data.FeeRequest cxnFeeRequest = Mapper.Map<CxnService.Data.FeeRequest>(feeRequest);
                return processorRouter.GetCXNMoneyTransferProcessor(context.ChannelPartnerName).GetDestinationAmount(cxnFeeRequest, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_DESTINATIONAMOUNT_FAILED, ex);
            }
        }

        public WUCountry GetBlockedUnBlockedCountries(commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                WUCountry countries = Mapper.Map<WUCountry>(CoreMoneyTransferService.GetBlockedUnBlockedCountries(context));
                return countries;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.GET_BLOCKED_UNBLOCKED_COUNTRIES_FAILED, ex);
            }
        }

        public bool SaveBlockedCountries(List<string> blockedCountries, commonData.ZeoContext context)
        {
            try
            {
                CoreMoneyTransferService = new ZeoCoreImpl();
                bool result = CoreMoneyTransferService.SaveBlockedCountries(blockedCountries, context);
                return result;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.SAVE_BLOCKED_COUNTRIES_FAILED, ex);
            }
        }
    }
}


