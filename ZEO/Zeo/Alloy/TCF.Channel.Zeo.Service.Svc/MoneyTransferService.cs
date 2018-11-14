using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IMoneyTransferService
    {
        public Response AddReceiver(Receiver receiver, ZeoContext context)
        {
            return serviceEngine.AddReceiver(receiver, context);
        }

        public Response EditReceiver(Receiver receiver, ZeoContext context)
        {
            return serviceEngine.EditReceiver(receiver, context);
        }

        public Response GetDeliveryServices(DeliveryServiceRequest request, ZeoContext context)
        {
            return serviceEngine.GetDeliveryServices(request, context);
        }

        public Response GetFeeMoneyTransfer(FeeRequest feeRequest, ZeoContext context)
        {
            return serviceEngine.GetFeeMoneyTransfer(feeRequest, context);
        }

        public Response GetXfrCities(string stateCode, ZeoContext context)
        {
            return serviceEngine.GetXfrCities(stateCode, context);
        }

        public Response GetXfrCountries(ZeoContext context)
        {
            return serviceEngine.GetXfrCountries(context);
        }

        public Response GetXfrStates(string countryCode, ZeoContext context)
        {
            return serviceEngine.GetXfrStates(countryCode, context);
        }

        public Response Validate(ValidateRequest validateRequest, ZeoContext context)
        {
            return serviceEngine.Validate(validateRequest, context);
        }

        public Response WUCardEnrollment(ZeoContext context)
        {
            return serviceEngine.WUCardEnrollment(context);
        }
        public Response GetFrequentReceivers(ZeoContext context)
        {
            return serviceEngine.GetFrequentReceivers(context);
        }

        public Response GetReceiver(long receiverId, ZeoContext context)
        {
            return serviceEngine.GetReceiver(receiverId, context);
        }

        public Response DeleteFavoriteReceiver(long receiverId, ZeoContext context)
        {
            return serviceEngine.DeleteFavoriteReceiver(receiverId, context);
        }

        public Response AddPastReceivers(string cardNumber, ZeoContext context)
        {
            return serviceEngine.AddPastReceivers(cardNumber, context);
        }

        public Response WUCardLookup(CardLookupDetails lookupDetails, ZeoContext context)
        {
            return serviceEngine.WUCardLookup(lookupDetails, context);
        }

        public Response UpdateWUAccount(string WUGoldCardNumber, ZeoContext context)
        {
            return serviceEngine.UpdateWUAccount(WUGoldCardNumber, context);
        }

        public Response GetCurrencyCodeList(string countryCode, ZeoContext context)
        {
            return serviceEngine.GetCurrencyCodeList(countryCode, context);
        }

        public Response CancelXfer(long transactionId, ZeoContext context)
        {
            return serviceEngine.CancelXfer(transactionId, context);
        }

        public Response WUGetAgentBannerMessage(long agentSessionId, ZeoContext context)
        {
            return serviceEngine.WUGetAgentBannerMessage(agentSessionId, context);
        }

        public Response GetCurrencyCode(string countryCode, ZeoContext context)
        {
            return serviceEngine.GetCurrencyCode(countryCode, context);
        }

        public Response StageModify(ModifyRequest modifySendMoney, ZeoContext context)
        {
            return serviceEngine.StageModify(modifySendMoney, context);
        }

        public Response Search(SearchRequest request, ZeoContext context)
        {
            return serviceEngine.Search(request, context);
        }

        public Response GetRefundReasons(ReasonRequest request, ZeoContext context)
        {
            return serviceEngine.GetRefundReasons(request, context);
        }

        public Response GetTransaction(long transactionId, ZeoContext context)
        {
            return serviceEngine.GetTransaction(transactionId, context);
        }

        public Response GetStatus(string mtcn, ZeoContext context)
        {
            return serviceEngine.GetStatus(mtcn, context);
        }

        public Response IsSWBStateXfer(ZeoContext context)
        {
            return serviceEngine.IsSWBStateXfer(context);
        }

        public Response AuthorizeModifySendMoney(ModifyRequest modifySendMoney, ZeoContext context)
        {
            return serviceEngine.AuthorizeModifySendMoney(modifySendMoney, context);
        }

        public Response ReceiveMoneySearch(ReceiveMoneyRequest receiveMoneyRequest, ZeoContext context)
        {
            return serviceEngine.ReceiveMoneySearch(receiveMoneyRequest, context);
        }

        public Response SendMoneyRefund(SendMoneyRefundRequest moneyTransferRefund, ZeoContext context)
        {
            return serviceEngine.SendMoneyRefund(moneyTransferRefund, context);
        }

        public Response CancelSendMoneyModify(long modifyTransactionId, long cancelTransactionId, ZeoContext context)
        {
            return serviceEngine.CancelSendMoneyModify(modifyTransactionId, cancelTransactionId, context);
        }

        public Response GetFraudLimit(string countryCode, ZeoContext context)
        {
            return serviceEngine.GetFraudLimit(countryCode, context);
        }

        public Response GetDestinationAmount(FeeRequest feeRequest, ZeoContext context)
        {
            return serviceEngine.GetDestinationAmount(feeRequest, context);
        }

        public Response GetBlockedUnBlockedCountries(ZeoContext context)
        {
            return serviceEngine.GetBlockedUnBlockedCountries(context);
        }

        public Response SaveBlockedCountries(List<string> blockedCountries, ZeoContext context)
        {
            return serviceEngine.SaveBlockedCountries(blockedCountries, context);
        }
    }
}