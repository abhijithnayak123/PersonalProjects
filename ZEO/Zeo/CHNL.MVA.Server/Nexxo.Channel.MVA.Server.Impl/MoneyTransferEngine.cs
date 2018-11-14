using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using dmsMasterData = MGI.Channel.Shared.Server.Data.XferMasterData;
using MGI.Common.Util;

namespace MGI.Channel.MVA.Server.Impl
{
    public partial class MVAEngine : IMoneyTransferService
    {

        #region IMoneyTransferService Impl

        public List<dmsMasterData> GetXfrCountries(string channelPartnerName, long customerSessionId = 0)
        {
            List<dmsMasterData> xfrCountries = ConsumerEngine.GetXfrCountries(customerSessionId, Self.GetPartnerContext(channelPartnerName)).OrderBy(x => x.Name).ToList();
            if (customerSessionId > 0)
            {
                List<dmsMasterData> frequestMasterData = new List<XferMasterData>();

                IList<Receiver> frequentReceivers = ConsumerEngine.GetFrequentReceivers(customerSessionId, Self.GetCustomerContext(customerSessionId));

                var frequentCountries = frequentReceivers.GroupBy(item => item.PickupCountry).ToList();

                foreach (var frequentCountry in frequentCountries)
                {
                    List<dmsMasterData> filteredcountry = xfrCountries.Where(x => x.Code.ToUpper() == frequentCountry.Key).ToList();

                    if (filteredcountry.Count > 0)
                    {
                        frequestMasterData.Add(filteredcountry.FirstOrDefault());
                    }
                }
                frequestMasterData = frequestMasterData.Concat(xfrCountries).ToList();

                return frequestMasterData;
            }
            else
                return xfrCountries;
        }

        public List<dmsMasterData> GetXfrStates(string countryCode, string channelPartnerName)
        {
            long customerSessionId = 0;
            return ConsumerEngine.GetXfrStates(customerSessionId, countryCode, Self.GetPartnerContext(channelPartnerName));
        }

        public List<dmsMasterData> GetXfrCities(string stateCode, string channelPartnerName)
        {
            long customerSessionId = 0;
            return ConsumerEngine.GetXfrCities(customerSessionId, stateCode, Self.GetPartnerContext(channelPartnerName));
        }

        #region Xfr Receiver Methods

        public long AddReceiver(long customerSessionId, Receiver receiver)
        {
            return ConsumerEngine.AddReceiver(customerSessionId, receiver, Self.GetCustomerContext(customerSessionId));
        }

        public long EditReceiver(long customerSessionId, Receiver receiver)
        {
            return ConsumerEngine.EditReceiver(customerSessionId, receiver, Self.GetCustomerContext(customerSessionId));
        }

        public IList<Receiver> GetFrequentReceivers(long customerSessionId)
        {
            return ConsumerEngine.GetFrequentReceivers(customerSessionId, Self.GetCustomerContext(customerSessionId));
        }

        public Receiver GetReceiver(long customerSessionId, long Id)
        {
            return ConsumerEngine.GetReceiver(customerSessionId, Id, Self.GetCustomerContext(customerSessionId));
        }

        public MoneyTransferTransaction GetReceiverLastTransaction(long customerSessionId , long receiverId)
        {
            return ConsumerEngine.GetReceiverLastTransaction(customerSessionId, receiverId, Self.GetCustomerContext(customerSessionId));
        }

        #endregion

        #region Xfer trx Methods

        public FeeResponse GetXfrFee(long customerSessionId, FeeRequest feeRequest)
        {
            FeeResponse fee = ConsumerEngine.GetXfrFee(customerSessionId, feeRequest, Self.GetCustomerContext(customerSessionId));

            List<FeeInformation> feeInformations = fee.FeeInformations;

            if (feeRequest.DeliveryService != null && !string.IsNullOrEmpty(feeRequest.DeliveryService.Code))
            {
                feeInformations = feeInformations.Where(x => x.DeliveryService.Code == feeRequest.DeliveryService.Code).ToList();
            }
            if (!string.IsNullOrEmpty(feeRequest.ReceiveCountryCurrency))
            {
                feeInformations = feeInformations.Where(x => x.ReceiveCurrencyCode == feeRequest.ReceiveCountryCurrency).ToList();
            }
            if (!string.IsNullOrEmpty(feeRequest.ReceiveAgentId))
            {
                feeInformations = feeInformations.Where(x => x.ReceiveAgentId == feeRequest.ReceiveAgentId).ToList();
            }
            
            fee.FeeInformations = feeInformations;

            return fee;
        }

        public ValidateResponse ValidateXfr(long customerSessionId, ValidateRequest validateRequest)
        {
            return ConsumerEngine.ValidateXfr(customerSessionId, validateRequest, Self.GetCustomerContext(customerSessionId));
        }

        public List<Field> GetXfrProviderAttributes(long customerSessionId, long transactionId, AttributeRequest attributeRequest)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);
            context.TrxId = transactionId;
            return ConsumerEngine.GetXfrProviderAttributes(customerSessionId, attributeRequest, context);
        }

        public MoneyTransferTransaction GetXfrTransaction(long customerSessionId, long transactionId)
        {
            return ConsumerEngine.GetXfrTransaction(customerSessionId, transactionId, Self.GetCustomerContext(customerSessionId));
        }


        public SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);
            return ConsumerEngine.SendMoneySearch(customerSessionId, request, context);
        }


        public ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);
            return ConsumerEngine.StageModifySendMoney(customerSessionId, modifySendMoney, context);
        }


        public ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);
            return ConsumerEngine.AuthorizeModifySendMoney(customerSessionId, modifySendMoney, context);
        }


        public List<MoneyTransferReason> GetRefundReasons(long customerSessionId, ReasonRequest request)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);
            return ConsumerEngine.GetRefundReasons(customerSessionId, request, context);
        }

        public long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);
            return ConsumerEngine.StageRefundSendMoney(customerSessionId, moneyTransferRefund, context);
        }


        #endregion

        #endregion
    }
}
