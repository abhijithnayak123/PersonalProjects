using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using System.Collections.Generic;

namespace MGI.Channel.Shared.Server.Contract
{
	public interface IMoneyTransferService
	{
		#region Xfr Receiver

		long AddReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext);

		long EditReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext);

        IList<Receiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext);

        Receiver GetReceiver(long customerSessionId, long Id, MGIContext mgiContext);

		MoneyTransferTransaction GetReceiverLastTransaction(long customerSessionId, long receiverId, MGIContext mgiContext);

		void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext);

		#endregion

		#region Xfr Trx Methods


		//int CommitXfr(long customerSessionId, long ptnrTransactionId, MGIContext mgiContext);

		//int CommitXfrModify(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext);

		FeeResponse GetXfrFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext);

		ValidateResponse ValidateXfr(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext);

		List<DeliveryService> GetXfrDeliveryServices(long customerSessionId, DeliveryServiceRequest request, MGIContext mgiContext);

		List<Field> GetXfrProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext);

        MoneyTransferTransaction GetXfrTransaction(long customerSessionId, long transactionId, MGIContext mgiContext);

        SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext);

	    ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify,MGIContext mgiContext);

	    ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId,ModifySendMoneyRequest modifySendMoneyRequest, MGIContext mgiContext);

        long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext);

	    List<MoneyTransferReason> GetRefundReasons(long customerSessionId, ReasonRequest request, MGIContext mgiContext);


	    #endregion
	}
}
