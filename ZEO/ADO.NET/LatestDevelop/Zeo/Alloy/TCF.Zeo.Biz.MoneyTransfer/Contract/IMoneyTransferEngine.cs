using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.MoneyTransfer.Contract
{
    public interface IMoneyTransferEngine
    {
        long AddReceiver(Receiver receiver, commonData.ZeoContext context);

        long UpdateReceiver(Receiver receiver, commonData.ZeoContext context);

        bool DeleteFavoriteReceiver(long receiverId, commonData.ZeoContext context);

        List<Receiver> GetFrequentReceivers(commonData.ZeoContext context);

        Receiver GetReceiver(long receiverId, commonData.ZeoContext context);

        List<MasterData> GetXfrCountries(commonData.ZeoContext context);

        List<MasterData> GetXfrStates(string countryCode, commonData.ZeoContext context);

        List<MasterData> GetXfrCities(string stateCode, commonData.ZeoContext context);

        ValidateResponse Validate(ValidateRequest validateRequest, commonData.ZeoContext context);

        int Commit(long ptnrTransactionId, commonData.ZeoContext context);

        FeeResponse GetFee(FeeRequest feeRequest, commonData.ZeoContext context);

	    CardDetails WUCardEnrollment(commonData.ZeoContext context);

        List<DeliveryService> GetDeliveryServices(DeliveryServiceRequest request, commonData.ZeoContext context);
		
		string GetBannerMsgs(long agentSessionId, commonData.ZeoContext context);

        void AddPastReceivers(string cardNumber, commonData.ZeoContext context);

        CardLookupDetails WUCardLookup(CardLookupDetails lookupDetails, commonData.ZeoContext context);

        bool UpdateAccount(string WUGoldCardNumber, commonData.ZeoContext context);

		List<MasterData> GetCurrencyCodeList(string countryCode, commonData.ZeoContext context);

		SearchResponse Search(SearchRequest request, commonData.ZeoContext context);

        void Cancel(long transactionId, commonData.ZeoContext context);

        string GetCurrencyCode(string countryCode, commonData.ZeoContext context);

		ModifyResponse StageModify(ModifyRequest modifySendMoney, commonData.ZeoContext context);

        int Modify(long transactionId, commonData.ZeoContext context);

		List<Reason> GetRefundReasons(ReasonRequest request, commonData.ZeoContext context);

        MoneyTransferTransaction GetTransaction(long transactionId, commonData.ZeoContext context);

        bool IsSendMoneyModifyRefundAvailable(long transactionId, commonData.ZeoContext context);

        string GetStatus(string confirmationNumber, commonData.ZeoContext context);

        bool IsSWBStateXfer(commonData.ZeoContext context);

        void AuthorizeModify(ModifyRequest modifySendMoney, commonData.ZeoContext context);

        MoneyTransferTransaction GetReceiveMoney(ReceiveMoneyRequest request, commonData.ZeoContext context);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="refundSendMoney"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        string Refund(SendMoneyRefundRequest refundSendMoney, commonData.ZeoContext context);

        void CancelSendMoneyModify(long modifyTransactionId, long cancelTransactionId, commonData.ZeoContext context);

        void CancelSendMoney(long transactionId, commonData.ZeoContext context);
    }
}
