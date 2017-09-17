using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;
using System.Collections.Generic;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Channel.DMS.Server.Data;
using System;
using System.ServiceModel;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IMoneyTransferService, IMoneyTransferSetupService
	{

        public Response GetFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetFee(customerSessionId, feeRequest, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetReceivers(long customerSessionId, string lastName, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetReceivers(customerSessionId, lastName, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetReceiver(long customerSessionId, long Id, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetReceiver(customerSessionId, Id, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetReceiver(long customerSessionId, string fullName, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetReceiver(customerSessionId, fullName, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response AddReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.AddReceiver(customerSessionId, receiver, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response EditReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.EditReceiver(customerSessionId, receiver, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetFrequentReceivers(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetFrequentReceivers(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				DesktopEngine.DeleteFavoriteReceiver(customerSessionId, receiverId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response ValidateTransfer(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.ValidateTransfer(customerSessionId, validateRequest, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetAgentXfer(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetAgentXfer(agentSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response IsSWBStateXfer(long customerSessionId, string locationState, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.IsSWBStateXfer(customerSessionId, locationState, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetXfrCountries(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetXfrCountries(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetXfrStates(customerSessionId, countryCode, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetXfrCities(customerSessionId, stateCode, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetCurrencyCode(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCurrencyCode(customerSessionId, countryCode, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCurrencyCodeList(customerSessionId, countryCode, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response Get(long customerSessionId, ReceiveMoneyRequest request, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.Get(customerSessionId, request, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response WUGetAgentBannerMessage(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.WUGetAgentBannerMessage(agentSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
        public Response UpdateWUAccount(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.UpdateWUAccount(customerSessionId, WUGoldCardNumber, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
		public Response WUCardEnrollment(long customerSessionId, XferPaymentDetails paymentDetails, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.WUCardEnrollment(customerSessionId, paymentDetails, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
		public Response WUCardLookup(long customerSessionId, CardLookupDetails wucardlookupreq, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.WUCardLookup(customerSessionId, wucardlookupreq, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}


        public Response GetWUCardAccount(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetWUCardAccount(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response DisplayWUCardAccountInfo(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.DisplayWUCardAccountInfo(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response Cancel(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				DesktopEngine.Cancel(customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetCardInfoXfer(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCardInfoXfer(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		/// <summary>
		/// This method will publish the methods in the DMS Service Operation for Past Receivers - User Story # US1645.
		/// </summary>
		/// <param name="customerSessionId">customerSessionId</param>
		/// <param name="cardNumber">WUGoldCardNum</param>
		/// <param name="mgiContext">mgiContext</param>
		public Response AddPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				DesktopEngine.AddPastReceivers(customerSessionId, cardNumber, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetDeliveryServices(long customerSessionId, DeliveryServiceRequest request, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetDeliveryServices(customerSessionId, request, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetXfrProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetXfrProviderAttributes(customerSessionId, attributeRequest, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		#region  Send Money Modify

        public Response GetStatus(long customerSessionId, string confirmationNumber, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetStatus(customerSessionId, confirmationNumber, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.SendMoneySearch(customerSessionId, request, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.StageModifySendMoney(customerSessionId, moneyTransferModify, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetMoneyTransferDetailsTransaction(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetMoneyTransferDetailsTransaction(customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response AuthorizeModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.AuthorizeModifySendMoney(customerSessionId, moneyTransferModify, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		#endregion

		#region  Send Money Refund

        public Response GetRefundReasons(long customerSessionId, ReasonRequest request, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetRefundReasons(customerSessionId, request, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
        {
			Response response = new Response();
			try
			{
				response = DesktopEngine.StageRefundSendMoney(customerSessionId, moneyTransferRefund, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
        }

        public Response SendMoneyRefund(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.SendMoneyRefund(customerSessionId, moneyTransferRefund, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		#endregion

	}
}
