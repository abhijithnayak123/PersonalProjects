using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using System.Collections.Generic;

namespace MGI.Channel.MVA.Server.Svc
{
    public partial class MVAWSImpl : IMoneyTransferService
    {
        #region IMoneyTransferService

        public List<XferMasterData> GetXfrCountries(string channelPartnerName, long customerSessionId = 0)
        {
            return MVAEngine.GetXfrCountries(channelPartnerName, customerSessionId);
        }

        public List<XferMasterData> GetXfrStates(string countryCode, string channelPartnerName)
        {
            return MVAEngine.GetXfrStates(countryCode, channelPartnerName);
        }

        public List<XferMasterData> GetXfrCities(string stateCode, string channelPartnerName)
        {
            return MVAEngine.GetXfrCities(stateCode, channelPartnerName);
        }

        public long AddReceiver(long customerSessionId, Receiver receiver)
        {
            return MVAEngine.AddReceiver(customerSessionId, receiver);
        }

        public long EditReceiver(long customerSessionId, Receiver receiver)
        {
            return MVAEngine.EditReceiver(customerSessionId, receiver);
        }

        public IList<Receiver> GetFrequentReceivers(long customerSessionId)
        {
            return MVAEngine.GetFrequentReceivers(customerSessionId);
        }

        public Receiver GetReceiver(long customerSessionId, long Id)
        {
            return MVAEngine.GetReceiver(customerSessionId, Id);
        }

        public FeeResponse GetXfrFee(long customerSessionId, FeeRequest feeRequest)
        {
            return MVAEngine.GetXfrFee(customerSessionId, feeRequest);
        }

        public ValidateResponse ValidateXfr(long customerSessionId, ValidateRequest validateRequest)
        {
            return MVAEngine.ValidateXfr(customerSessionId, validateRequest);
        }

        public List<Field> GetXfrProviderAttributes(long customerSessionId, long transactionId, AttributeRequest attributeRequest)
        {
            return MVAEngine.GetXfrProviderAttributes(customerSessionId, transactionId, attributeRequest);
        }

        public MoneyTransferTransaction GetReceiverLastTransaction(long customerSessionId, long receiverId)
        {
            return MVAEngine.GetReceiverLastTransaction(customerSessionId, receiverId );
        }

        public MoneyTransferTransaction GetXfrTransaction(long customerSessionId, long transactionId)
        {
            return MVAEngine.GetXfrTransaction( customerSessionId, transactionId );
        }

        public SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request)
        {
            return MVAEngine.SendMoneySearch(customerSessionId, request);
        }

        public ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney)
        {
            return MVAEngine.StageModifySendMoney(customerSessionId, modifySendMoney);
        }

        public ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney)
        {
            return MVAEngine.AuthorizeModifySendMoney(customerSessionId, modifySendMoney);
        }

        #region Refund


        public long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund)
        {
            return MVAEngine.StageRefundSendMoney(customerSessionId, moneyTransferRefund);
        }

        public List<MoneyTransferReason> GetRefundReasons(long customerSessionId, ReasonRequest request)
        {
            return MVAEngine.GetRefundReasons(customerSessionId, request);
        }

        #endregion Refund

        #endregion






      
    }
}
