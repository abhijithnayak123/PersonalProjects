using System;
using System.Collections.Generic;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Fund.Contract;
using CxnFundsAccount = TCF.Zeo.Cxn.Fund.Data.CardAccount;
using TCF.Zeo.Processor;
using ZeoData = TCF.Channel.Zeo.Data;
using CXNData = TCF.Zeo.Cxn.Fund.Data;
using TCF.Zeo.Common.Util;
using BizCore = TCF.Zeo.Biz.Common;
using TCF.Zeo.Biz.Fund.Contract;
using AutoMapper;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Impl;
using CoreData = TCF.Zeo.Core.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using TCF.Zeo.Cxn.Fund.Visa.Data.Exceptions;

namespace TCF.Zeo.Biz.Fund.Impl
{
    public class FundsEngineImpl : IFundsEngine
    {
        BizCore.Contract.IComplianceService complianceService = new BizCore.Impl.ComplianceServiceImpl();
        public IFundService CoreFundsService { private get; set; }

        ProcessorRouter processorRouter = new ProcessorRouter();
        IMapper mapper;
        public FundsEngineImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ZeoData.FundsAccount, CxnFundsAccount>().ReverseMap();

                cfg.CreateMap<CXNData.CardMaintenanceInfo, ZeoData.CardMaintenanceInfo>();
                cfg.CreateMap<ZeoData.CardMaintenanceInfo, CXNData.CardMaintenanceInfo>();
                cfg.CreateMap<CXNData.CardBalanceInfo, ZeoData.CardBalanceInfo>().ReverseMap();
                cfg.CreateMap<CXNData.ShippingTypes, ZeoData.ShippingTypes>().ReverseMap();
                cfg.CreateMap<CXNData.TransactionHistory, ZeoData.VisaTransactionHistory>();
                cfg.CreateMap<ZeoData.TransactionHistoryRequest, CXNData.TransactionHistoryRequest>();
                cfg.CreateMap<CXNData.FundTrx, ZeoData.Funds>()
                .ForMember(d => d.Amount, s => s.MapFrom(c => c.TransactionAmount))
                .AfterMap((s, d) =>
                {
                    d.FundsType = (Helper.FundType)Enum.Parse(typeof(Helper.FundType), s.TransactionType, true);
                });
                cfg.CreateMap<CoreData.CustomerProfile, CXNData.CustomerInfo>()
                    .ForMember(d => d.Phone, s => s.MapFrom(c => c.Phone1.Number))
                    .ForMember(d => d.Email, s => s.MapFrom(c => c.Email))
                    .ForMember(d => d.Address1, s => s.MapFrom(c => c.Address.Address1))
                    .ForMember(d => d.Address2, s => s.MapFrom(c => c.Address.Address2))
                    .ForMember(d => d.City, s => s.MapFrom(c => c.Address.City))
                    .ForMember(d => d.State, s => s.MapFrom(c => c.Address.State))
                    .ForMember(d => d.ZipCode, s => s.MapFrom(c => c.Address.ZipCode))
                    .ForMember(d => d.IDCode, s => s.MapFrom(c => Convert.ToString(c.IDCode)));
            });
            mapper = config.CreateMapper();
        }

        #region  Public Methods

        public long Activate(ZeoData.Funds funds, commonData.ZeoContext context)
        {
            try
            {
                return Transact(funds, Helper.FundType.Activation, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_ACTIVATE_FAILED, ex);
            }
        }

        public long Add(ZeoData.FundsAccount fundsAccount, commonData.ZeoContext context)
        {
            try
            {
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);

                int providerId = (int)_GetFundProvider(context.ChannelPartnerName);

                CxnFundsAccount account = cxnFundsProcessor.Lookup(context);

                long cxnFundId = 0;
                if (account == null)
                {
                    cxnFundId = _AddCXNAccount(fundsAccount, cxnFundsProcessor, context);
                }
                else
                {
                    CxnFundsAccount cxnFundsAccount = mapper.Map<ZeoData.FundsAccount, CxnFundsAccount>(fundsAccount);
                    cxnFundsProcessor.UpdateRegistrationDetails(cxnFundsAccount, context);
                }

                return cxnFundId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.ADD_FUNDS_FAILED, ex);
            }
        }

        public long AssociateCard(ZeoData.FundsAccount fundsAccount, commonData.ZeoContext context)
        {
            try
            {
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);

                int providerId = (int)_GetFundProvider(context.ChannelPartnerName);

                long cxnFundId = 0;

                CxnFundsAccount cxnFundsAccount = mapper.Map<ZeoData.FundsAccount, CxnFundsAccount>(fundsAccount);
                CxnFundsAccount account = cxnFundsProcessor.Lookup(context);
                ICustomerService customerSvc = new ZeoCoreImpl();

                CoreData.CustomerProfile customer = customerSvc.GetCustomer(context);

                CXNData.CustomerInfo fundCust = mapper.Map<CXNData.CustomerInfo>(customer);

                if (account == null)
                {
                    cxnFundId = cxnFundsProcessor.AssociateCard(cxnFundsAccount, fundCust, context).CustomerId;
                }
                else
                {
                    cxnFundsAccount.Id = account.Id;
                    cxnFundsProcessor.AssociateCard(cxnFundsAccount, fundCust, context);
                }

                return cxnFundId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_CARD_ASSOCIATION_FAILED, ex);
            }
        }

        public void Cancel(long fundsId, bool hasFundsAccount, commonData.ZeoContext context)
        {
            try
            {
                CoreData.Funds fund;
                using (CoreFundsService = new ZeoCoreImpl())
                {
                    fund = CoreFundsService.GetFundTransaction(fundsId, context);
                }

                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);

                bool isActive = fund.FundsType == Helper.FundType.Activation ? false : true; // Why do we need this?

                validateCXNAccountExists(null, context);

                if (fund.FundsType == Helper.FundType.Activation && !hasFundsAccount)
                {
                    cxnFundsProcessor.Cancel(context);
                }

                using (CoreFundsService = new ZeoCoreImpl())
                {
                    CoreFundsService.UpdateFundTransactionState(fundsId, Helper.TransactionStates.Canceled, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_CANCEL_TRANSACTION_FAILED, ex);
            }
        }

        public bool CloseAccount(commonData.ZeoContext context)
        {
            try
            {
                validateCXNAccountExists(true, context);

                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);
                return cxnFundsProcessor.CloseAccount(context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_ACCOUNT_CLOSURE_FAILED, ex);
            }
        }

        public int Commit(long TrxId, commonData.ZeoContext context)
        {
            try
            {
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);

                ICustomerService customerSvc = new ZeoCoreImpl();

                CoreData.CustomerProfile customer = customerSvc.GetCustomer(context);

                CXNData.CustomerInfo fundCust = mapper.Map<CXNData.CustomerInfo>(customer);

                if (context.Context == null)
                    context.Context = new Dictionary<string, object>();

                long cxnId;
                using (CoreFundsService = new ZeoCoreImpl())
                {
                    cxnId = CoreFundsService.GetCXNTransactionId(TrxId, context);
                }

                cxnFundsProcessor.Commit(cxnId, fundCust, context);

                using (CoreFundsService = new ZeoCoreImpl())
                {
                    CoreFundsService.CommitFundTransaction(TrxId, (int)Helper.TransactionStates.Committed, context.CustomerSessionId, context);
                }

                return (int)Helper.TransactionStates.Committed;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.COMMIT_FAILED, ex);
            }
        }

        public ZeoData.Funds Get(long Id, bool isEditTransaction, commonData.ZeoContext context)
        {
            try
            {
                long cxnId;
                using (CoreFundsService = new ZeoCoreImpl())
                {
                    cxnId = CoreFundsService.GetCXNTransactionId(Id, context);
                }
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);
                CXNData.FundTrx fundTrx = cxnFundsProcessor.Get(cxnId, isEditTransaction, context);

                //Added this code to show the latest balance of the card in New Card Balance field of the Trx History.
                decimal cardBalance = (decimal)fundTrx.PreviousCardBalance;
                if (fundTrx.TransactionType == Helper.FundType.Debit.ToString() && fundTrx.Status == TransactionStates.Committed)
                    cardBalance = (decimal)(fundTrx.PreviousCardBalance - fundTrx.TransactionAmount);
                else if (fundTrx.TransactionType == Helper.FundType.Credit.ToString() && fundTrx.Status == TransactionStates.Committed)
                    cardBalance = (decimal)(fundTrx.PreviousCardBalance + fundTrx.TransactionAmount);

                ZeoData.Funds bizFundTrx = mapper.Map<ZeoData.Funds>(fundTrx);

                bizFundTrx.CardBalance = cardBalance;

                return bizFundTrx;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_TRANSACTION_RETRIVEL_FAILED, ex);
            }
        }

        public ZeoData.FundsAccount GetAccount(commonData.ZeoContext context)
        {
            try
            {
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);
                //CXNData.CardBalanceInfo cardInfo = new CXNData.CardBalanceInfo();

                //try
                //{
                //    cardInfo = cxnFundsProcessor.GetBalance(context);
                //}
                //catch (Exception ex)
                //{
                //    //TODO
                //    var exp = ex as commonData.ProviderException;
                //    if (exp != null && exp.ProductCode == VisaProviderException.ProductCode && exp.ProviderErrorCode == commonData.ProviderException.PROVIDER_COMMUNICATION_ERROR)
                //    {
                //        cardInfo.Balance = decimal.MinValue;
                //    }
                //    else
                //    {
                //        if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                //        throw new FundsException(FundsException.ADD_FUNDS_FAILED, ex);
                //    }
                //}
                CxnFundsAccount cardAccount = cxnFundsProcessor.Lookup(context);
                if (cardAccount == null)
                    throw new FundsException(FundsException.FUNDS_ACCOUNT_NOT_FOUND);
                return new ZeoData.FundsAccount()
                {
                    CardNumber = cardAccount.CardNumber,
                    //CardBalance = cardInfo.Balance,
                    ProxyId = cardAccount.ProxyId,
                    PseudoDDA = cardAccount.PseudoDDA,
                    FullCardNumber = cardAccount.FullCardNumber,
                    CardAliasId = cardAccount.CardAliasId,
                    PrimaryCardAliasId = cardAccount.PrimaryCardAliasId,
                    SubClientNodeId = cardAccount.SubClientNodeId,
                    Activated = cardAccount.IsCardActive,
                    DTAccountClosed = cardAccount.DTAccountClosed,
                    ExpirationYear = cardAccount.ExpirationYear,
                    ExpirationMonth = cardAccount.ExpirationMonth,
                    ActivatedLocationNodeId = cardAccount.ActivatedLocationNodeId,
                    FraudScore = Convert.ToInt32(cardAccount.IsFraud)
                    //FirstName = cardAccount.FirstName,
                    //LastName = cardAccount.LastName
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ZeoData.CardBalanceInfo GetBalance(commonData.ZeoContext context)
        {
            try
            {
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);
                CXNData.CardBalanceInfo cardInfo = cxnFundsProcessor.GetBalance(context);
                return mapper.Map<ZeoData.CardBalanceInfo>(cardInfo);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_BALANCE_RETRIVEL_FAILED, ex);
            }

        }

        public ZeoData.TransactionFee GetFee(decimal amount, Helper.FundType fundsType, commonData.ZeoContext context)
        {
            throw new NotImplementedException();
        }

        public double GetFundFee(ZeoData.CardMaintenanceInfo cardMaintenanceInfo, commonData.ZeoContext context)
        {
            try
            {
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);
                CXNData.CardMaintenanceInfo cxnCardMaintenanceInfo = mapper.Map<CXNData.CardMaintenanceInfo>(cardMaintenanceInfo);
                double fundfee = cxnFundsProcessor.GetFundFee(cxnCardMaintenanceInfo, context);
                return fundfee;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_CARDMAINTENANCE_FEE_RETRIVEL_FAILED, ex);
            }
        }

        public decimal GetMinimumLoadAmount(bool initialLoad, commonData.ZeoContext context)
        {
            try
            {
                decimal minLoadAmt = 0.00M;
                // pass the complaince program 	
                if (initialLoad)
                {
                    minLoadAmt = complianceService.GetTransactionMinimumLimit(Helper.TransactionType.ActivateGPR, context);
                }
                else
                {
                    minLoadAmt = complianceService.GetTransactionMinimumLimit(Helper.TransactionType.LoadToGPR, context);
                }
                return minLoadAmt;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_GETMINIMUM_FAILED, ex);
            }
        }

        public double GetShippingFee(ZeoData.CardMaintenanceInfo cardMaintenanceInfo, commonData.ZeoContext context)
        {
            try
            {
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);
                TCF.Zeo.Cxn.Fund.Data.CardMaintenanceInfo cxnCardMaintenanceInfo = mapper.Map<TCF.Zeo.Cxn.Fund.Data.CardMaintenanceInfo>(cardMaintenanceInfo);
                double shippingfee = cxnFundsProcessor.GetShippingFee(cxnCardMaintenanceInfo, context);

                return shippingfee;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_SHIPPING_FEE_RETRIVEL_FAILED, ex);
            }
        }

        public List<ZeoData.ShippingTypes> GetShippingTypes(commonData.ZeoContext context)
        {
            try
            {
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);

                List<CXNData.ShippingTypes> cardShippingTypes = cxnFundsProcessor.GetShippingTypes((long)context.ChannelPartnerId, context);
                return mapper.Map<List<ZeoData.ShippingTypes>>(cardShippingTypes);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_SHIPPING_TYPE_RETRIVEL_FAILED, ex);
            }
        }

        public List<ZeoData.VisaTransactionHistory> GetTransactionHistory(ZeoData.TransactionHistoryRequest request, commonData.ZeoContext context)
        {
            try
            {
                validateCXNAccountExists(true, context);

                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);

                CXNData.TransactionHistoryRequest cxnTransactionHistoryRequest = mapper.Map<CXNData.TransactionHistoryRequest>(request);

                List<CXNData.TransactionHistory> transactionHistoryList = cxnFundsProcessor.GetTransactionHistory(cxnTransactionHistoryRequest, context);

                return mapper.Map<List<ZeoData.VisaTransactionHistory>>(transactionHistoryList);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_TRANSACTION_HISTORY_RETRIVEL_FAILED, ex);
            }

        }

        public long IssueAddOnCard(ZeoData.Funds funds, commonData.ZeoContext context)
        {
            try
            {
                return Transact(funds, Helper.FundType.AddOnCard, context);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long Load(ZeoData.Funds funds, commonData.ZeoContext context)
        {
            try
            {
                return Transact(funds, Helper.FundType.Credit, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_ORDER_COMPANION_FAILED, ex);
            }
        }

        public bool ReplaceCard(ZeoData.CardMaintenanceInfo cardMaintenanceInfo, commonData.ZeoContext context)
        {
            try
            {
                validateCXNAccountExists(true, context);

                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);
                CXNData.CardMaintenanceInfo cxnCardMaintenanceInfo = mapper.Map<CXNData.CardMaintenanceInfo>(cardMaintenanceInfo);

                return cxnFundsProcessor.ReplaceCard(cxnCardMaintenanceInfo, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_CARD_REPLACE_FAILED, ex);
            }

        }

        public long UpdateAmount(long fundTrxId, decimal amount, Helper.FundType fundType, string gprPromoCode, commonData.ZeoContext context)
        {
            try
            {
                Helper.FundType requestType = Helper.FundType.Activation;

                if (fundType == Helper.FundType.Credit)
                    requestType = Helper.FundType.Credit;
                else if (fundType == Helper.FundType.Debit)
                    requestType = Helper.FundType.Debit;

                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);

                if (requestType == Helper.FundType.Activation || requestType == Helper.FundType.Credit || requestType == Helper.FundType.Debit)
                {
                    CXNData.FundRequest fundRequest = new CXNData.FundRequest()
                    {
                        RequestType = GetRequestType(Convert.ToString(requestType)),
                        PromoCode = gprPromoCode,
                        Amount = amount
                    };

                    long cxnTrxId = 0;
                    using (CoreFundsService = new ZeoCoreImpl())
                    {
                        CoreFundsService.UpdateFundTransactionAmount(fundTrxId, amount, context);
                        cxnTrxId = CoreFundsService.GetCXNTransactionId(fundTrxId, context);
                    }
                    //update the amount of the transaction in cxn
                    cxnFundsProcessor.UpdateAmount(cxnTrxId, fundRequest, context.TimeZone, context);
                }

                return fundTrxId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_UPDATE_FAILED, ex);
            }
        }

        public bool UpdateCardStatus(ZeoData.CardMaintenanceInfo cardMaintenanceInfo, commonData.ZeoContext context)
        {
            try
            {
                validateCXNAccountExists(true, context);
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);
                CXNData.CardMaintenanceInfo cxnCardMaintenanceInfo = mapper.Map<CXNData.CardMaintenanceInfo>(cardMaintenanceInfo);
                return cxnFundsProcessor.UpdateCardStatus(cxnCardMaintenanceInfo, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_UPDATE_STATUS_FAILED, ex);
            }

        }

        public long Withdraw(ZeoData.Funds funds, commonData.ZeoContext context)
        {
            try
            {
                return Transact(funds, Helper.FundType.Debit, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_WITHDRAW_FAILED, ex);
            }
        }

        public string GetPrepaidCardNumber(commonData.ZeoContext context)
        {
            try
            {
                IFundProcessor cxnFundsProcessor = _GetProcessor(context.ChannelPartnerName);

                return cxnFundsProcessor.GetPrepaidCardNumber(context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundsException(FundsException.FUNDS_CARD_RETRIVEL_FAILED, ex);
            }
        }


        public long UpdateAccount(ZeoData.FundsAccount fundsAccount, commonData.ZeoContext context)
        {
            IFundProcessor processor = _GetProcessor(context.ChannelPartnerName);

            CxnFundsAccount cxnAccount = new CxnFundsAccount()
            {

                CardNumber = fundsAccount.CardNumber,
                //CardBalance = cardInfo.Balance,
                ProxyId = fundsAccount.ProxyId,
                PseudoDDA = fundsAccount.PseudoDDA,
                FullCardNumber = fundsAccount.FullCardNumber,
                CardAliasId = fundsAccount.CardAliasId,
                PrimaryCardAliasId = fundsAccount.PrimaryCardAliasId,
                SubClientNodeId = fundsAccount.SubClientNodeId,
                IsCardActive = fundsAccount.Activated,
                DTAccountClosed = fundsAccount.DTAccountClosed,
                ExpirationYear = fundsAccount.ExpirationYear,
                ExpirationMonth = fundsAccount.ExpirationMonth,
                ActivatedLocationNodeId = fundsAccount.ActivatedLocationNodeId,
                IsFraud = Convert.ToBoolean(fundsAccount.FraudScore)
            };
            long id = processor.UpdateAccount(cxnAccount, context);
            return id;
        }

        #endregion

        #region Private Methods

        private long Transact(ZeoData.Funds bizFunds, Helper.FundType requestType, commonData.ZeoContext context)
        {
            _ValidateFundsRequest(bizFunds, requestType);

            _ValidateLimits(requestType, context, bizFunds.Amount);

            IFundProcessor processor = _GetProcessor(context.ChannelPartnerName);

            Helper.FundType type = (Helper.FundType)Enum.Parse(typeof(Helper.FundType), requestType.ToString());

            long coreFundsId = 0;

            if (bizFunds.TransactionId == 0)
            {
                CoreData.Funds coreFund = new CoreData.Funds()
                {
                    Amount = bizFunds.Amount,
                    Fee = bizFunds.Fee,
                    Description = bizFunds.Description,
                    ConfirmationNumber = bizFunds.ConfirmationNumber,
                    DiscountName = bizFunds.DiscountName,
                    PromoCode = bizFunds.PromoCode,
                    ProviderId = (int)processorRouter.GetFundProviders(context.ChannelPartnerName),
                    State = (int)TransactionStates.Authorized,
                    FundsType = type,
                    AddOnCustomerId = context.AddOnCustomerId,
                    CustomerSessionId = context.CustomerSessionId,
                    DTServerCreate = DateTime.Now,
                    DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone)
                };
                using (CoreFundsService = new ZeoCoreImpl())
                {
                    coreFundsId = CoreFundsService.CreateFundTransaction(coreFund, context);
                }

                IFundProcessor cxnFundsProcessor = processorRouter.GetFundProvider(context.ChannelPartnerName);
                long cxnId = _SendCxnFunds(cxnFundsProcessor, bizFunds, requestType, context);

                using (CoreFundsService = new ZeoCoreImpl())
                {
                    CoreFundsService.UpdateCXNTransctionId(coreFundsId, cxnId, context);
                }
            }
            else
            {
                coreFundsId = UpdateAmount(bizFunds.TransactionId, bizFunds.Amount, Helper.FundType.Activation, bizFunds.PromoCode, context);
            }
            return coreFundsId;
        }

        private void _ValidateFundsRequest(ZeoData.Funds bizFunds, Helper.FundType requestType)
        {
            if (bizFunds.Amount <= 0 && (requestType == Helper.FundType.Credit || requestType == Helper.FundType.Debit))
                throw new FundsException(FundsException.FUNDS_TRANSACTION_INVALID);
            if (bizFunds.Amount <= bizFunds.Fee && (requestType == Helper.FundType.Credit || requestType == Helper.FundType.Debit))
                throw new FundsException(FundsException.FUNDS_TRANSACTION_INVALID);
        }

        private void _ValidateLimits(Helper.FundType fundType, commonData.ZeoContext context, decimal amount)
        {
            Helper.TransactionType transactionType = Helper.TransactionType.DebitGPR;
            if ((fundType == Helper.FundType.Credit || fundType == Helper.FundType.Debit))
            {
                if (fundType == Helper.FundType.Credit)
                {
                    transactionType = Helper.TransactionType.LoadToGPR;
                }
                BizCore.Data.Limit limit = complianceService.GetTransactionLimit(transactionType, context);

                if (amount > limit.PerTransactionMaximum)
                {
                    throw new BizComplianceLimitException(FundsException.FundsProductCode, BizComplianceLimitException.MAXIMUM_LIMIT_FAILED, limit.PerTransactionMaximum.ToString(), Convert.ToDecimal(limit.PerTransactionMaximum));
                }
            }
        }
        private IFundProcessor _GetProcessor(string channelPartner)
        {
            // get the fund processor for the channel partner.
            return (IFundProcessor)processorRouter.GetFundProvider(channelPartner);
        }
        private ProviderId _GetFundProvider(string channelPartner)
        {
            // get the fund provider for the channel partner.
            return processorRouter.GetFundProviders(channelPartner);
        }

        private void validateCXNAccountExists(bool? isActive, commonData.ZeoContext context)
        {
            IFundProcessor cxnFundsProcessor = processorRouter.GetFundProvider(context.ChannelPartnerName);

            bool exists = cxnFundsProcessor.AccountExists(context.CustomerId, isActive, context);

            if (!exists)
                throw new FundsException(FundsException.FUNDS_ACCOUNT_NOT_FOUND);
            ;
        }

        private long _SendCxnFunds(IFundProcessor cxnFundsProcessor, ZeoData.Funds bizFunds, Helper.FundType fundType, commonData.ZeoContext context)
        {
            CXNData.FundRequest cxnFunds = new CXNData.FundRequest
            {
                Amount = bizFunds.Amount,
                PromoCode = bizFunds.PromoCode,
            };

            long cxnTransactionId = 0L;

            if (fundType == Helper.FundType.Debit)
            {
                cxnFunds.RequestType = Helper.FundType.Debit.ToString();
                cxnFunds.Amount += bizFunds.Fee;
                cxnTransactionId = cxnFundsProcessor.Withdraw(cxnFunds, context);
            }
            else if (fundType == Helper.FundType.Credit)
            {
                cxnFunds.RequestType = Helper.FundType.Credit.ToString();
                cxnFunds.Amount -= bizFunds.Fee;
                cxnTransactionId = cxnFundsProcessor.Load(cxnFunds, context);
            }
            else if (fundType == Helper.FundType.Activation)
            {
                cxnFunds.RequestType = "Activation";
                cxnTransactionId = cxnFundsProcessor.Activate(cxnFunds, context);
            }
            else
            {
                cxnFunds.RequestType = Helper.FundType.AddOnCard.ToString();
                cxnTransactionId = cxnFundsProcessor.Activate(cxnFunds, context);
            }
            return cxnTransactionId;
        }

        private long _AddCXNAccount(ZeoData.FundsAccount fundsAccount, IFundProcessor cxnFundsProcessor, commonData.ZeoContext context)
        {
            CxnFundsAccount cxnFundsAccount = mapper.Map<ZeoData.FundsAccount, CxnFundsAccount>(fundsAccount);

            long accountNumber = cxnFundsProcessor.Register(cxnFundsAccount, context);

            return accountNumber;
        }

        private string GetRequestType(string requestType)
        {
            switch (requestType)
            {
                case "None":
                    requestType = "Activation";
                    break;
                case "Debit":
                    requestType = Convert.ToString((Helper.FundType.Debit));
                    break;
                case "Credit":
                    requestType = Convert.ToString((Helper.FundType.Credit));
                    break;
            }
            return requestType;
        }

        #endregion
    }
}

