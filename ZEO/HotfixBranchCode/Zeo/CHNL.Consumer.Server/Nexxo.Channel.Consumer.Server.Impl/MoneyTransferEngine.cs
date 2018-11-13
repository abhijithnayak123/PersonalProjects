using System.Collections.Generic;
using System.Linq;
using MGI.Biz.MoneyTransfer.Contract;
using MGI.Channel.Consumer.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;
using System;

namespace MGI.Channel.Consumer.Server.Impl
{
    public partial class ConsumerEngine : IMoneyTransferService
    {
        public IMoneyTransferEngine bizMoneyTransferEngine { get; set; }
        #region MoneyTransfer Data Mapper

        internal static void MoneyTransferConverter()
        {

        }

        #endregion

        #region Xfr SetupService Impl

        [Transaction(ReadOnly = true)]
        public List<SharedData.XferMasterData> GetXfrCountries(long customerSessionId, MGIContext mgiContext)
        {
            return SharedEngine.GetXfrCountries(customerSessionId, mgiContext);
        }

        [Transaction(ReadOnly = true)]
        public List<SharedData.XferMasterData> GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext)
        {
            return SharedEngine.GetXfrStates(customerSessionId, countryCode, mgiContext);
        }

        [Transaction(ReadOnly = true)]
        public List<SharedData.XferMasterData> GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext)
        {
			return SharedEngine.GetXfrCities(customerSessionId, stateCode, mgiContext);
        }

        [Transaction(ReadOnly = true)]
        public List<SharedData.XferMasterData> GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext)
        {
			return SharedEngine.GetCurrencyCodeList(customerSessionId, countryCode, mgiContext);
        }

        #endregion

        #region IMoneyTransferService Impl

        #region Xfr Receiver Methods

        [Transaction()]
        public long AddReceiver(long customerSessionId, SharedData.Receiver receiver, MGIContext mgiContext)
        {
			return SharedEngine.AddReceiver(customerSessionId, receiver, mgiContext);
        }

        [Transaction()]
        public long EditReceiver(long customerSessionId, SharedData.Receiver receiver, MGIContext mgiContext)
        {
			return SharedEngine.EditReceiver(customerSessionId, receiver, mgiContext);
        }

        [Transaction(ReadOnly = true)]
        public IList<SharedData.Receiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext)
        {
			return SharedEngine.GetFrequentReceivers(customerSessionId, mgiContext);
        }

        [Transaction(ReadOnly = true)]
        public SharedData.Receiver GetReceiver(long customerSessionId, long Id, MGIContext mgiContext)
        {
			return SharedEngine.GetReceiver(customerSessionId, Id, mgiContext);
        }

        [Transaction(ReadOnly = true)]
        public SharedData.MoneyTransferTransaction GetReceiverLastTransaction(long customerSessionId, long receiverId, MGIContext mgiContext)
        { 
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(receiverId), "GetReceiverLastTransaction", AlloyLayerName.SERVICE,
				ModuleName.MoneyTransfer, "Begin GetReceiverLastTransaction - MGI.Channel.Consumer.Server.Impl.MoneyTransferEngine", mgiContext);
            #endregion
			SharedData.MoneyTransferTransaction moneyTransferTransaction = SharedEngine.GetReceiverLastTransaction(customerSessionId, receiverId, mgiContext);

            if (moneyTransferTransaction != null)
            {
                if (!string.IsNullOrEmpty(moneyTransferTransaction.DestinationCountryCode))
                {
					List<XferMasterData> countries = SharedEngine.GetXfrCountries(customerSessionId, mgiContext).Where(
                        x => x.Code == moneyTransferTransaction.DestinationCountryCode).ToList();
                    if (countries != null && countries.Count > 0)
                        moneyTransferTransaction.ReceiverCountryName = countries.FirstOrDefault().Name;
                }

                if (!string.IsNullOrEmpty(moneyTransferTransaction.ExpectedPayoutStateCode))
                {
                    List<XferMasterData> states = SharedEngine.GetXfrStates(customerSessionId,
						moneyTransferTransaction.DestinationCountryCode, mgiContext).
                        Where(x => x.Code == moneyTransferTransaction.ExpectedPayoutStateCode).ToList();
                    if (states != null && states.Count > 0)
                        moneyTransferTransaction.ReceiverStateName = states.FirstOrDefault().Name;
                }
            }

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<SharedData.MoneyTransferTransaction>(customerSessionId, moneyTransferTransaction, "GetReceiverLastTransaction", AlloyLayerName.SERVICE,
				ModuleName.MoneyTransfer, "End GetReceiverLastTransaction - MGI.Channel.Consumer.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return moneyTransferTransaction;
        }

		[Transaction()]
		public void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext)
		{
			SharedEngine.DeleteFavoriteReceiver(customerSessionId, receiverId, mgiContext);
		}
        #endregion

        #region Xfr trx Methods

        [Transaction()]
        public SharedData.FeeResponse GetXfrFee(long customerSessionId, SharedData.FeeRequest feeRequest, MGIContext mgiContext)
        {
			return SharedEngine.GetXfrFee(customerSessionId, feeRequest, mgiContext);
        }

        [Transaction()]
        public SharedData.ValidateResponse ValidateXfr(long customerSessionId, SharedData.ValidateRequest validateRequest, MGIContext mgiContext)
        {
			return SharedEngine.ValidateXfr(customerSessionId, validateRequest, mgiContext);
        }

        [Transaction]
        public List<SharedData.Field> GetXfrProviderAttributes(long customerSessionId, SharedData.AttributeRequest attributeRequest, MGIContext mgiContext)
        {
			return SharedEngine.GetXfrProviderAttributes(customerSessionId, attributeRequest, mgiContext);
        }

        [Transaction]
        public MoneyTransferTransaction GetXfrTransaction(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "GetXfrTransaction", AlloyLayerName.SERVICE,
				ModuleName.MoneyTransfer, "Begin GetXfrTransaction - MGI.Channel.Consumer.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
			MoneyTransferTransaction moneyTransferTransaction = SharedEngine.GetXfrTransaction(customerSessionId, transactionId, mgiContext);

            if (!string.IsNullOrEmpty(moneyTransferTransaction.DestinationCountryCode))
            {
				List<XferMasterData> countries = SharedEngine.GetXfrCountries(customerSessionId, mgiContext).Where(
                    x => x.Code == moneyTransferTransaction.DestinationCountryCode).ToList();
                if (countries != null && countries.Count > 0)
                    moneyTransferTransaction.ReceiverCountryName = countries.FirstOrDefault().Name;
            }

            if (!string.IsNullOrEmpty(moneyTransferTransaction.ExpectedPayoutStateCode))
            {
                List<XferMasterData> states = SharedEngine.GetXfrStates(customerSessionId,
					moneyTransferTransaction.DestinationCountryCode, mgiContext).
                    Where(x => x.Code == moneyTransferTransaction.ExpectedPayoutStateCode).ToList();
                if (states != null && states.Count > 0)
                    moneyTransferTransaction.ReceiverStateName = states.FirstOrDefault().Name;
            }

            moneyTransferTransaction.MetaData = AppendTimeZoneAbbr(moneyTransferTransaction.MetaData, "TransactionDate");

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<MoneyTransferTransaction>(customerSessionId, moneyTransferTransaction, "GetXfrTransaction", AlloyLayerName.SERVICE,
				ModuleName.MoneyTransfer, "End GetXfrTransaction - MGI.Channel.Consumer.Server.Impl.MoneyTransferEngine", mgiContext);
			#endregion
            return moneyTransferTransaction;
        }

        [Transaction]
        public SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext)
        {
			return SharedEngine.SendMoneySearch(customerSessionId, request, mgiContext);
        }

        [Transaction]
        public ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
        {
			return SharedEngine.StageModifySendMoney(customerSessionId, moneyTransferModify, mgiContext);
        }

        [Transaction]
        public ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney, MGIContext mgiContext)
        {
			return SharedEngine.AuthorizeModifySendMoney(customerSessionId, modifySendMoney, mgiContext);
        }

        [Transaction]
        public List<MoneyTransferReason> GetRefundReasons(long customerSessionId, ReasonRequest request, MGIContext mgiContext)
        {
			return SharedEngine.GetRefundReasons(customerSessionId, request, mgiContext);
        }

        [Transaction]
        public long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
        {
			return SharedEngine.StageRefundSendMoney(customerSessionId, moneyTransferRefund, mgiContext);
        }

        #endregion

        #endregion

    }
}
