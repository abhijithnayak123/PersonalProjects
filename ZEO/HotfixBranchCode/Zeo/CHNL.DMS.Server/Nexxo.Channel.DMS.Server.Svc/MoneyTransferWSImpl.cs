using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;
using System.Collections.Generic;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IMoneyTransferService, IMoneyTransferSetupService
	{

        public FeeResponse GetFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext)
		{
			return DesktopEngine.GetFee(customerSessionId, feeRequest, mgiContext);
		}

        public IList<Receiver> GetReceivers(long customerSessionId, string lastName, MGIContext mgiContext)
		{
			return DesktopEngine.GetReceivers(customerSessionId, lastName, mgiContext);
		}

        public Receiver GetReceiver(long customerSessionId, long Id, MGIContext mgiContext)
		{
			return DesktopEngine.GetReceiver(customerSessionId, Id, mgiContext);
		}

        public Receiver GetReceiver(long customerSessionId, string fullName, MGIContext mgiContext)
		{
			return DesktopEngine.GetReceiver(customerSessionId, fullName, mgiContext);
		}

		public long AddReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext)
		{
			return DesktopEngine.AddReceiver(customerSessionId, receiver, mgiContext);
		}

		public long EditReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext)
		{
			return DesktopEngine.EditReceiver(customerSessionId, receiver, mgiContext);
		}

        public IList<Receiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetFrequentReceivers(customerSessionId, mgiContext);
		}

		public void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext)
		{
			DesktopEngine.DeleteFavoriteReceiver(customerSessionId, receiverId, mgiContext);
		}

        public ValidateResponse ValidateTransfer(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext)
		{
			return DesktopEngine.ValidateTransfer(customerSessionId, validateRequest, mgiContext);
		}

        public CashierDetails GetAgentXfer(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetAgentXfer(agentSessionId, mgiContext);
		}

        public bool IsSWBStateXfer(long customerSessionId, string locationState, MGIContext mgiContext)
		{
			return DesktopEngine.IsSWBStateXfer(customerSessionId, locationState, mgiContext);
		}

        public List<XferMasterData> GetXfrCountries(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetXfrCountries(customerSessionId, mgiContext);
		}

        public List<XferMasterData> GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			return DesktopEngine.GetXfrStates(customerSessionId, countryCode, mgiContext);
		}

        public List<XferMasterData> GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext)
		{
			return DesktopEngine.GetXfrCities(customerSessionId, stateCode, mgiContext);
		}

        public string GetCurrencyCode(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			return DesktopEngine.GetCurrencyCode(customerSessionId, countryCode, mgiContext);
		}

        public List<XferMasterData> GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			return DesktopEngine.GetCurrencyCodeList(customerSessionId, countryCode, mgiContext);
		}

		public MoneyTransferTransaction Get(long customerSessionId, ReceiveMoneyRequest request, MGIContext mgiContext)
		{
			return DesktopEngine.Get(customerSessionId, request, mgiContext);
		}

        public List<XferMasterData> WUGetAgentBannerMessage(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.WUGetAgentBannerMessage(agentSessionId, mgiContext);
		}
        public bool UpdateWUAccount(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext)
		{
			return DesktopEngine.UpdateWUAccount(customerSessionId, WUGoldCardNumber, mgiContext);
		}
		public SharedData.CardDetails WUCardEnrollment(long customerSessionId, XferPaymentDetails paymentDetails, MGIContext mgiContext)
		{
			return DesktopEngine.WUCardEnrollment(customerSessionId, paymentDetails, mgiContext);
		}
		public List<WUCustomerGoldCardResult> WUCardLookup(long customerSessionId, CardLookupDetails wucardlookupreq, MGIContext mgiContext)
		{
			return DesktopEngine.WUCardLookup(customerSessionId, wucardlookupreq, mgiContext);
		}


        public bool GetWUCardAccount(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetWUCardAccount(customerSessionId, mgiContext);
		}

        public Account DisplayWUCardAccountInfo(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.DisplayWUCardAccountInfo(customerSessionId, mgiContext);
		}

		public void Cancel(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			DesktopEngine.Cancel(customerSessionId, transactionId, mgiContext);
		}
		public SharedData.CardInfo GetCardInfoXfer(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCardInfoXfer(customerSessionId, mgiContext);
		}

		/// <summary>
		/// This method will publish the methods in the DMS Service Operation for Past Receivers - User Story # US1645.
		/// </summary>
		/// <param name="customerSessionId">customerSessionId</param>
		/// <param name="cardNumber">WUGoldCardNum</param>
		/// <param name="mgiContext">mgiContext</param>
		public void AddPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext)
		{
			DesktopEngine.AddPastReceivers(customerSessionId, cardNumber, mgiContext);
		}

		public List<DeliveryService> GetDeliveryServices(long customerSessionId, DeliveryServiceRequest request, MGIContext mgiContext)
		{
			return DesktopEngine.GetDeliveryServices(customerSessionId, request, mgiContext);
		}

		public List<Field> GetXfrProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext)
		{
			return DesktopEngine.GetXfrProviderAttributes(customerSessionId, attributeRequest, mgiContext);
		}

		#region  Send Money Modify

        public string GetStatus(long customerSessionId, string confirmationNumber, MGIContext mgiContext)
		{
			return DesktopEngine.GetStatus(customerSessionId, confirmationNumber, mgiContext);
		}

        public SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext)
		{
			return DesktopEngine.SendMoneySearch(customerSessionId, request, mgiContext);
		}

        public ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
		{
			return DesktopEngine.StageModifySendMoney(customerSessionId, moneyTransferModify, mgiContext);
		}

        public SharedData.MoneyTransferTransaction GetMoneyTransferDetailsTransaction(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetMoneyTransferDetailsTransaction(customerSessionId, transactionId, mgiContext);
		}

        public ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
		{
			return DesktopEngine.AuthorizeModifySendMoney(customerSessionId, moneyTransferModify, mgiContext);
		}

		#endregion

		#region  Send Money Refund

        public List<MoneyTransferReason> GetRefundReasons(long customerSessionId, ReasonRequest request, MGIContext mgiContext)
		{
			return DesktopEngine.GetRefundReasons(customerSessionId, request, mgiContext);
		}

        public long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
        {
            return DesktopEngine.StageRefundSendMoney(customerSessionId, moneyTransferRefund, mgiContext);
        }

        public string SendMoneyRefund(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
		{
			return DesktopEngine.SendMoneyRefund(customerSessionId, moneyTransferRefund, mgiContext);
		}

		#endregion

	}
}
