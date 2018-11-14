#region External References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

#region Zeo References

using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.BillPay.Contract;
using CXNData = TCF.Zeo.Cxn.BillPay.Data;
using CoreData = TCF.Zeo.Core.Data;
using AutoMapper;
using TCF.Zeo.Processor;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.BillPay.Contract;
using commonData = TCF.Zeo.Common.Data;
using Core = TCF.Zeo.Core;
using TCF.Zeo.Common.DataProtection.Contract;
using TCF.Zeo.Common.DataProtection.Impl;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Biz.Common.Data;
using TCF.Zeo.Biz.Common.Impl;
using static TCF.Zeo.Common.Util.Helper;
using System.Configuration;
#endregion


namespace TCF.Zeo.Biz.BillPay.Impl
{
    public class BillPayServiceImpl : IBillPayService
    {

        #region Dependencies

        ProcessorRouter processorRouter = new ProcessorRouter();
        IBillPayProcessor billPayProcessor { set; get; }
        Core.Contract.IBillPayService coreBillPayService { set; get; }
        public IDataProtectionService BPDataProtectionSvc { private get; set; }
        public IComplianceService complianceService { get; set; }

        IMapper mapper;

        #endregion

        public BillPayServiceImpl()
        {
            #region Mapping

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CXNData.Location, BillerLocation>();
                cfg.CreateMap<BillerLocation, CXNData.Location>();
                cfg.CreateMap<CXNData.Fee, BillPayFee>();
                cfg.CreateMap<CXNData.Field, Field>();
                cfg.CreateMap<CXNData.DeliveryMethod, DeliveryMethod>();
                cfg.CreateMap<CoreData.FavouriteBiller, FavoriteBiller>();
                cfg.CreateMap<CXNData.BillPayRequest, CoreData.BillPayRequest>();
                cfg.CreateMap<CXNData.BillPayTransaction, BillPayTransaction>();
                cfg.CreateMap<CXNData.BillerInfo, BillerInfo>();
                cfg.CreateMap<CXNData.BillPayValidateResponse, BillPayValidateResponse>();
                cfg.CreateMap<BillPayment, CXNData.BillPayment>();
                cfg.CreateMap<CXNData.CardInfo, CardInfo>();
            });

            mapper = config.CreateMapper();

            #endregion
        }

        #region IBillPayService methods

        #region Bill Pay


        public BillPayFee GetFee(long transactionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, commonData.ZeoContext context)
        {
            try
            {
                billPayProcessor = GetProcessor(context.ChannelPartnerName);

                CoreData.BillPay billPay = new CoreData.BillPay()
                {
                    Amount = amount,
                    AccountNumber = accountNumber,
                    BillerNameOrCode = billerNameOrCode,
                    State = (int)Helper.TransactionStates.Pending,
                    CustomerSessionId = context.CustomerSessionId,
                    Fee = 0M // Dummy Fee
                };

                CreateOrUpdateBillPayment(ref transactionId, billPay, ref context);

                CXNData.Location cxnLocation = mapper.Map<CXNData.Location>(location);

                long cxnTransactionId = GetBillPayCXNTransactionID(transactionId);

                CXNData.Fee fee = billPayProcessor.GetFee(cxnTransactionId, billerNameOrCode, accountNumber, amount, cxnLocation, context);

                if (fee.TransactionId > 0)
                {
                    GetCoreBillPayService().UpdateTransactionwithCXNID(fee.TransactionId, billPay, context);

                    // Update customer preferred product and transaction state
                    GetCoreBillPayService().UpdatePreferredProductsAndState(transactionId, (int)Helper.TransactionStates.Initiated, context.TimeZone, context);

                    //overwriting the TransactionId to PTNR transactionId, as this is CXNId can be derived from PTNR whenever is required. But otherway is wrong.
                    fee.TransactionId = transactionId;
                }

                return mapper.Map<BillPayFee>(fee);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLPAY_GETFEE_FAILED, ex);
            }
        }

        public BillPayLocation GetLocations(long transactionId, string billerName, string accountNumber, decimal amount, commonData.ZeoContext context)
        {
            try
            {
                billPayProcessor = GetProcessor(context.ChannelPartnerName);

                CoreData.BillPay billPay = new CoreData.BillPay()
                {
                    Amount = amount,
                    AccountNumber = accountNumber,
                    BillerNameOrCode = billerName,
                    State = (int)Helper.TransactionStates.Pending,
                    CustomerSessionId = context.CustomerSessionId
                };

                CreateOrUpdateBillPayment(ref transactionId, billPay, ref context);

                List<CXNData.Location> locations = billPayProcessor.GetLocations(billerName, accountNumber, amount, context);

                BillPayLocation billPayLocation = new BillPayLocation()
                {
                    BillerLocation = mapper.Map<List<BillerLocation>>(locations),
                    TransactionId = transactionId
                };

                return billPayLocation;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLERLOCATION_GET_FAILED, ex);
            }
        }

        public BillPayValidateResponse Validate(long transactionId, BillPayment billPayment, commonData.ZeoContext context)
        {
            try
            {
                complianceService = new ComplianceServiceImpl();

                Limit limit = complianceService.GetTransactionLimit(Helper.TransactionType.BillPayment, context);

                if (billPayment.PaymentAmount < limit.PerTransactionMinimum)
                {
                    throw new BizComplianceLimitException(BillPayException.BillPayProductCode, BizComplianceLimitException.MINIMUM_LIMIT_FAILED, limit.PerTransactionMinimum.ToString(), Convert.ToDecimal(limit.PerTransactionMinimum));

                }
                if (billPayment.PaymentAmount > limit.PerTransactionMaximum)
                {
                    throw new BizComplianceLimitException(BillPayException.BillPayProductCode, BizComplianceLimitException.MAXIMUM_LIMIT_FAILED, limit.PerTransactionMaximum.ToString(), Convert.ToDecimal(limit.PerTransactionMaximum));
                }

                billPayProcessor = GetProcessor(context.ChannelPartnerName);

                CoreData.BillPay billPay = new CoreData.BillPay()
                {
                    Amount = billPayment.PaymentAmount,
                    AccountNumber = billPayment.AccountNumber,
                    BillerNameOrCode = billPayment.BillerName,
                    State = (int)Helper.TransactionStates.Pending,
                    CustomerSessionId = context.CustomerSessionId,
                    Fee = billPayment.Fee
                };

                context.Context = billPayment.MetaData;

                CXNData.BillPayment cxnBillPayment = mapper.Map<CXNData.BillPayment>(billPayment);
                CreateOrUpdateBillPayment(ref transactionId, billPay, ref context);

                long cxnTransactionId = GetBillPayCXNTransactionID(transactionId);

                CXNData.BillPayValidateResponse cxnValidateResponse = billPayProcessor.Validate(cxnTransactionId, cxnBillPayment, context);

                //overwriting the TransactionId to PTNR transactionId, as this is CXNId can be derived from PTNR whenever is required. But otherway is wrong.
                cxnValidateResponse.TransactionId = transactionId;

                GetCoreBillPayService().UpdateBillPayTransactionFee(cxnValidateResponse.TransactionId, (int)Helper.TransactionStates.Authorized, cxnValidateResponse.Fee, cxnValidateResponse.Amount, cxnValidateResponse.ConfirmationNumber, context.TimeZone, context);

                return mapper.Map<BillPayValidateResponse>(cxnValidateResponse);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.TRANSACTION_VALIDATE_FAILED, ex);
            }

        }

        public void Submit(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                context.RequestType = Helper.RequestType.Hold.ToString();
                CommitTransaction(transactionId, context);

                if (context.PromotionId > 0) // Update transaction fee adjustment if the promo name is configured in ZEO
                {
                    IFeeService feeService = new FeeServiceImpl();
                    feeService.CreateOrUpdateTransactionFeeAdjustment(context.PromotionId, TransactionType.BillPayment, transactionId, context);
                }

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.TRANSACTION_SUBMIT_FAILED, ex);
            }
        }

        public void Commit(long transactionId, commonData.ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope(TransactionScopeOptions.RequiresNew))
            {
                try
                {
                    context.RequestType = Helper.RequestType.Release.ToString();

                    CXNData.BillPayTransaction cxnTrx = CommitTransaction(transactionId, context);

                    GetCoreBillPayService().UpdateBillPayTransactionFee(transactionId, (int)Helper.TransactionStates.Committed, cxnTrx.Fee, cxnTrx.Amount, cxnTrx.ConfirmationNumber, context.TimeZone, context);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                    throw new BillPayException(BillPayException.TRANSACTION_COMMIT_FAILED, ex);
                }
            }
        }

        public void UpdateTransactionState(long transactionId, int state, commonData.ZeoContext context)
        {
            try
            {
                GetCoreBillPayService().UpdateBillPayTransactionState(transactionId, state, context.TimeZone, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.TRANSACTION_UPDATE_FAILED, ex);
            }
        }

        public void Cancel(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                context.RequestType = Helper.RequestType.Cancel.ToString();
                CommitTransaction(transactionId, context);
                GetCoreBillPayService().UpdateBillPayTransactionState(transactionId, (int)Helper.TransactionStates.Canceled, context.TimeZone, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.TRANSACTION_CANCEL_FAILED, ex);
            }
        }

        public BillPayTransaction GetTransaction(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                billPayProcessor = GetProcessor(context.ChannelPartnerName);
                long cxnTransactionId = GetBillPayCXNTransactionID(transactionId);

                return mapper.Map<BillPayTransaction>(billPayProcessor.GetTransaction(cxnTransactionId, context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.TRANSACTION_GET_FAILED, ex);
            }
        }

        public List<Field> GetProviderAttributes(string billerName, string location, commonData.ZeoContext context)
        {
            try
            {
                List<CXNData.Field> fields = GetProcessor(context.ChannelPartnerName).GetProviderAttributes(billerName, location, context);

                return mapper.Map<List<Field>>(fields);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.PROVIDERATTRIBUTES_GET_FAILED, ex);
            }
        }

        #endregion

        #region Billers

        public FavoriteBiller GetBillerDetails(string billerName, commonData.ZeoContext context)
        {
            try
            {
                coreBillPayService = GetCoreBillPayService();

                FavoriteBiller biller = mapper.Map<FavoriteBiller>(coreBillPayService.GetBillerDetails(billerName, context.CustomerId, Convert.ToInt16(context.ChannelPartnerId), context));

                biller.ProviderName = ((Helper.ProviderId)biller.ProviderId).ToString();

                return biller;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLERDETAILS_GET_FAILED, ex);
            }
        }

        public List<FavoriteBiller> DeleteFavouriteBiller(long billerID, commonData.ZeoContext context)
        {
            try
            {
                coreBillPayService = GetCoreBillPayService();
                return mapper.Map<List<FavoriteBiller>>(coreBillPayService.DeleteFavouriteBiller(billerID, context.CustomerId, context.TimeZone, context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.FAVORITEBILLER_DELETE_FAILED, ex);
            }
        }

        public BillerInfo GetBillerInfo(string billerNameOrCode, commonData.ZeoContext context)
        {
            try
            {
                billPayProcessor = GetProcessor(context.ChannelPartnerName);

                return mapper.Map<BillerInfo>(billPayProcessor.GetBillerInfo(billerNameOrCode, context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLERMESSAGE_GET_FAILED, ex);
            }
        }

        public List<FavoriteBiller> GetFrequentBillers(commonData.ZeoContext context)
        {
            try
            {
                coreBillPayService = GetCoreBillPayService();
                return mapper.Map<List<FavoriteBiller>>(coreBillPayService.GetFrequentBillers(context.CustomerId, context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.FAVORITEBILLER_GET_FAILED, ex);
            }
        }

        public List<string> GetBillers(string searchTerm, int channelPartnerID, commonData.ZeoContext context)
        {
            try
            {
                coreBillPayService = GetCoreBillPayService();
                return (coreBillPayService.GetBillers(searchTerm, channelPartnerID, context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLERDETAILS_GET_FAILED, ex);
            }
        }

        public List<FavoriteBiller> AddPastBillers(string cardNumber, commonData.ZeoContext context)
        {
            try
            {
                billPayProcessor = GetProcessor(context.ChannelPartnerName);

                billPayProcessor.GetPastBillers(cardNumber, context);

                return GetFrequentBillers(context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.PASTBILLER_ADD_FAILED, ex);
            }
        }

        #endregion

        public CardInfo GetCardInfo(commonData.ZeoContext context)
        {
            try
            {
                billPayProcessor = GetProcessor(context.ChannelPartnerName);
                return mapper.Map<CardInfo>(billPayProcessor.GetCardInfo(context.WUCardNumber, context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.CARDINFO_GET_FAILED, ex);
            }
        }


        #endregion

        #region Private Methods

        private CXNData.BillPayTransaction CommitTransaction(long transactionId, commonData.ZeoContext context)
        {
            billPayProcessor = GetProcessor(context.ChannelPartnerName);

            context.ProviderId = (int)processorRouter.GetBillPayProvider(context.ChannelPartnerName);

            long cxnTransactionId = GetBillPayCXNTransactionID(transactionId);

            return billPayProcessor.Commit(cxnTransactionId, context);
        }

        private IBillPayProcessor GetProcessor(string channelPartnerName)
        {
            return processorRouter.GetBillPayCXNProcessor(channelPartnerName);
        }

        private Core.Contract.IBillPayService GetCoreBillPayService()
        {
            if (coreBillPayService == null)
                return coreBillPayService = new Core.Impl.ZeoCoreImpl();
            else
                return coreBillPayService;
        }

        private void CreateOrUpdateBillPayment(ref long transactionId, CoreData.BillPay billPay, ref commonData.ZeoContext context)
        {
            context.ProviderId = billPay.ProviderId = (int)processorRouter.GetBillPayProvider(context.ChannelPartnerName);

            long providerAccountId = billPayProcessor.GetBillPayAccountId(context.CustomerSessionId, context.TimeZone, context);

            context.CXNAccountId = billPay.ProviderAccountId = providerAccountId;

            billPay.AccountNumber = Encrypt(billPay.AccountNumber);

            coreBillPayService = GetCoreBillPayService();

            if (transactionId == 0)
            {
                billPay.DTServerCreate = DateTime.Now;
                billPay.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);
                transactionId = coreBillPayService.CreateBillPayTransaction(billPay, context);
            }
            else
            {
                billPay.Id = transactionId;
                billPay.DTServerLastModified = DateTime.Now;
                billPay.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                coreBillPayService.UpdateBillPayTransaction(billPay, context.ChannelPartnerId, context);
            }
        }

        private string Encrypt(string plainString)
        {
            string type = (ConfigurationManager.AppSettings["DataProtectionService"]);
            if (type == "Simulator")
            {
                BPDataProtectionSvc = new DataProtectionSimulator();
            }
            else
            {
                BPDataProtectionSvc = new DataProtectionService();
            }

            if (!string.IsNullOrWhiteSpace(plainString) && plainString.IsCreditCardNumber())
            {
                return BPDataProtectionSvc.Encrypt(plainString, 0);
            }
            return plainString;
        }

        private long GetBillPayCXNTransactionID(long transactionId)
        {
            long cxnId = 0;
            coreBillPayService = GetCoreBillPayService();
            return cxnId = coreBillPayService.GetBillPayCxnTransactionId(transactionId);
        }
        #endregion
    }
}
