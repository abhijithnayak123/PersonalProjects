using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using System.Collections.Generic;
using System.ServiceModel;

namespace MGI.Channel.MVA.Server.Contract
{
	[ServiceContract]
	public interface IMoneyTransferService
	{
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		long AddReceiver(long customerSessionId, Receiver receiver);

		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		long EditReceiver(long customerSessionId, Receiver receiver);

		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		IList<Receiver> GetFrequentReceivers(long customerSessionId);

		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Receiver GetReceiver(long customerSessionId, long Id);

		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		FeeResponse GetXfrFee(long customerSessionId, FeeRequest feeRequest);

		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		ValidateResponse ValidateXfr(long customerSessionId, ValidateRequest validateRequest);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<Field> GetXfrProviderAttributes(long customerSessionId, long transactionId, AttributeRequest attributeRequest);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<XferMasterData> GetXfrCountries(string channelPartnerName, long customerSessionId=0);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<XferMasterData> GetXfrStates(string countryCode, string channelPartnerName);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<XferMasterData> GetXfrCities(string stateCode, string channelPartnerName);

	    [OperationContract]
	    [FaultContract(typeof (NexxoSOAPFault))]
	    MoneyTransferTransaction GetReceiverLastTransaction(long customerSessionId, long receiverId);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        MoneyTransferTransaction GetXfrTransaction(long customerSessionId, long transactionId );

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney);
        
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<MoneyTransferReason> GetRefundReasons(long customerSessionId, ReasonRequest request);


	    [OperationContract]
	    [FaultContract(typeof (NexxoSOAPFault))]
	    long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund);

       



	}
}
