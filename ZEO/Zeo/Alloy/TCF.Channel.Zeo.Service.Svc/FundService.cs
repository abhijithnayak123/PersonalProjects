using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IFundsProcessorService
    {
        public Response ActivateGPRCard(Funds funds, ZeoContext context)
        {
            return serviceEngine.ActivateGPRCard(funds, context);
        }

        public Response Add(FundsAccount fundsAccount, ZeoContext context)
        {
            return serviceEngine.Add(fundsAccount, context);
        }

        public Response AssociateCard(FundsAccount fundsAccount, ZeoContext context)
        {
            return serviceEngine.AssociateCard(fundsAccount, context);
        }

        public Response CloseAccount(ZeoContext context)
        {
            return serviceEngine.CloseAccount(context);
        }

        public Response GetBalance(ZeoContext context)
        {
            return serviceEngine.GetBalance(context);
        }

        public Response GetCardTransactionHistory(TransactionHistoryRequest request, ZeoContext context)
        {
            return serviceEngine.GetCardTransactionHistory(request, context);
        }

        public Response GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            return serviceEngine.GetFundFee(cardMaintenanceInfo, context);
        }

        public Response GetMinimumLoadAmount(bool initialLoad, ZeoContext context)
        {
            return serviceEngine.GetMinimumLoadAmount(initialLoad, context);
        }

        public Response GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            return serviceEngine.GetShippingFee(cardMaintenanceInfo, context);
        }

        public Response GetShippingTypes(ZeoContext context)
        {
            return serviceEngine.GetShippingTypes(context);
        }

        public Response IssueAddOnCard(Funds funds, ZeoContext context)
        {
            return serviceEngine.IssueAddOnCard(funds, context);
        }

        public Response Load(Funds funds, ZeoContext context)
        {
            return serviceEngine.Load(funds, context);
        }

        public Response LookupForPAN(ZeoContext context)
        {
            return serviceEngine.LookupForPAN(context);
        }

        public Response ReplaceCard(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            return serviceEngine.ReplaceCard(cardMaintenanceInfo, context);
        }

        public Response UpdateCardStatus(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            return serviceEngine.UpdateCardStatus(cardMaintenanceInfo, context);
        }

        public Response UpdateFundAmount(long cxnFundTrxId, decimal amount, Helper.FundType fundType, ZeoContext context)
        {
            return serviceEngine.UpdateFundAmount(cxnFundTrxId, amount, fundType, context);
        }

        public Response Withdraw(Funds funds, ZeoContext context)
        {
            return serviceEngine.Withdraw(funds, context);
        }

        public Response GetPrepaidActions(string cardStatus, ZeoContext context)
        {
            return serviceEngine.GetPrepaidActions(cardStatus, context);
        }

        public Response GetFundTransaction(long transactionId, bool isEditTransaction, ZeoContext context)
        {
            return serviceEngine.GetFundTransaction(transactionId, isEditTransaction, context);
        }

        public Response UpdateGPRAccount(FundsAccount fundsAccount, Data.ZeoContext context)
        {
            return serviceEngine.UpdateGPRAccount(fundsAccount, context);
        }
    }
}