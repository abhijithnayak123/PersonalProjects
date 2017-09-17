using System;
using System.Collections.Generic;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using System.Linq;
using MGI.Integration.Test.Data;
using MGI.Channel.DMS.Web.Models;

namespace MGI.Integration.Test
{

	[TestFixture]
	public partial class AlloyIntegrationTestFixture : BaseFixture
	{
        public decimal SendMoneyAmount { get; set; }
		#region SendMoney Integration Test cases
		[TestCase("Synovus")]
		[TestCase("Carver")]
		public void DoSendMoneyIT(string channelPartnerName)
		{
			var tranHistory = DoSendMoney(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		public void SendMoneyPark(string channelPartnerName)
		{
			bool isSendMoneyParked = false;
			isSendMoneyParked = ParkSenMoney(channelPartnerName);
			Assert.That(isSendMoneyParked, Is.True);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		public void SendMoneyParkUnPark(string channelPartnerName)
		{
			var tranHistory = DoSendMoney(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		public void SendMoneyRemove(string channelPartnerName)
		{
			bool isSendMoneyRemoved = false;
			isSendMoneyRemoved = RemoveSenMoney(channelPartnerName);
			Assert.That(isSendMoneyRemoved, Is.True);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		public void SendMoneyModify(string channelPartnerName)
		{
			ShoppingCartCheckoutStatus cartStatus = ModifySendMoney(channelPartnerName);

			Assert.AreEqual(cartStatus, ShoppingCartCheckoutStatus.Completed);
		}


		[TestCase("Synovus")]
		[TestCase("Carver")]
		public void SendMoneyRefund(string channelPartnerName)
		{
			ShoppingCartCheckoutStatus cartStatus = RefundSendMoney(channelPartnerName);
			Assert.AreEqual(cartStatus, ShoppingCartCheckoutStatus.Completed);
		}

		#endregion

		#region Private Methods
		
		private List<TransactionHistory> DoSendMoney(string channelPartnerName)
		{
            Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			MGI.Channel.DMS.Server.Data.MGIContext context;
			PerformSendMoney(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), out context);

            var status = client.Checkout(customerSession.CustomerSessionId, GetRandomAmount(), customerSession.CardPresent ? customerSession.Customer.Fund.CardNumber : string.Empty, ShoppingCartCheckoutStatus.InitialCheckout, context);
			var items = client.GetTransactionHistory(Convert.ToInt64(customerSession.CustomerSessionId), customerSession.Customer.CIN, string.Empty, string.Empty, DateTime.Now.AddDays(-1), context);
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return items;
		}

		private void PerformSendMoney(string channelPartnerName, long CustomerSessionId, out MGI.Channel.DMS.Server.Data.MGIContext context)
		{
            Desktop client = new Desktop();
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);

            SendMoneyAmount = GetRandomAmount();

			context = new Channel.DMS.Server.Data.MGIContext();

			Receiver receiver = GetReceiver(Convert.ToInt64(CustomerSessionId), channelPartnerName);

			FeeRequest feeRequest = GetMoneyTransferFeeRequest(Convert.ToInt64(CustomerSessionId), receiver, context);

			FeeResponse feeResponse = client.GetMoneyTransferFee(CustomerSessionId, feeRequest, context);

			ValidateRequest validateRequest = GetValidateRequest(feeResponse, feeRequest, receiver);

			ValidateResponse response = client.ValidateTransfer(CustomerSessionId, validateRequest, context);
		}

		private Receiver GetReceiver(long customerSessionId, string channelPartnerName)
		{
            Desktop client = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext context = new Channel.DMS.Server.Data.MGIContext();

			List<Receiver> receivers = client.GetFrequentReceivers(customerSessionId, context);

			Receiver receiver = new Receiver();
			if (receivers.Count == 0)
			{
				receiver = IntegrationTestData.GetReceiverData(channelPartnerName);
				client.SaveReceiver(customerSessionId, receiver, context);

				receivers = client.GetFrequentReceivers(customerSessionId, context);
			}
			receiver = receivers.FirstOrDefault();
			return receiver;
		}

		private FeeRequest GetMoneyTransferFeeRequest(long customerSessionId, Receiver receiver, MGI.Channel.DMS.Server.Data.MGIContext context)
        {
            Desktop client = new Desktop();
			string currencyCode = client.GetCurrencyCode(customerSessionId, receiver.PickupCountry, context);
			FeeRequest feeRequest = new FeeRequest();

			feeRequest.Amount = SendMoneyAmount;
			feeRequest.ReceiveAmount = SendMoneyAmount;
			feeRequest.ReceiveCountryCode = receiver.PickupCountry;
			feeRequest.ReceiveCountryCurrency = currencyCode;
			feeRequest.IsDomesticTransfer = false;
			feeRequest.PromoCode = string.Empty;
			feeRequest.ReceiverFirstName = receiver.FirstName;
			feeRequest.ReceiverLastName = receiver.LastName;
			feeRequest.ReceiverSecondLastName = receiver.SecondLastName;
			feeRequest.ReceiverId = receiver.Id;
			feeRequest.DeliveryService = new DeliveryService()
			{
				Code = receiver.DeliveryOption != null ? receiver.DeliveryOption : receiver.DeliveryMethod,
				Name = receiver.DeliveryOption != null ? receiver.DeliveryOption : receiver.DeliveryMethod
			};
			feeRequest.PersonalMessage = "";
			feeRequest.MetaData = new Dictionary<string, object>()
						{
							{"CityCode",  receiver.PickupCity},
							{"CityName",  receiver.City},
							{"StateCode",  receiver.PickupState_Province},
							{"StateName",  receiver.State_Province},
							{"TestQuestionOption", "Test Question Option"},
							{"TestQuestion", "Test Question"},
							{"TestAnswer", "Test Answer"},
							{"IsFixedOnSend", "false"}		
						};
			feeRequest.ReferenceNo = "12323412";

			return feeRequest;

		}

		private ValidateRequest GetValidateRequest(FeeResponse feeResponse, FeeRequest feeRequest, Receiver receiver)
		{
			FeeInformation feeInformation = new FeeInformation();
			ValidateRequest validateRequest = new ValidateRequest();
			if (feeResponse.FeeInformations.Count > 0)
			{
				feeInformation = feeResponse.FeeInformations.FirstOrDefault();
				validateRequest.TransferType = TransferType.SendMoney;
				validateRequest.TransactionId = feeResponse.TransactionId;

				validateRequest.Amount = feeInformation.Amount;
				validateRequest.Fee = feeInformation.Fee;
				validateRequest.Tax = feeInformation.Tax;
				validateRequest.OtherFee = feeInformation.OtherFee;
				validateRequest.MessageFee = feeInformation.OtherFee;

				validateRequest.PersonalMessage = string.Empty;
				validateRequest.PromoCode = string.Empty;
				validateRequest.DeliveryService = feeRequest.DeliveryService.Code;
				validateRequest.State = receiver.PickupState_Province;

				validateRequest.ReceiverFirstName = feeRequest.ReceiverFirstName;
				validateRequest.ReceiverLastName = feeRequest.ReceiverFirstName;
				validateRequest.ReceiverSecondLastName = feeRequest.ReceiverLastName;

				validateRequest.MetaData = new Dictionary<string, object>()
				{
					{"ExpectedPayoutCity",receiver.PickupCity}, 
					{"ExpectedPayoutStateCode",receiver.PickupState_Province}, 
					{"ProceedWithLPMTError","false"},
					{"ReceiveAgentAbbr", string.Empty},
				};
			}
			return validateRequest;
		}

		private bool ParkSenMoney(string channelPartnerName)
		{
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			MGI.Channel.DMS.Server.Data.MGIContext context;
			bool isSendMoneyParked = false;
			Desktop client = new Desktop();
			PerformSendMoney(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), out context);

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);

			client.ParkMoneyTransfer(Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(cart.MoneyTransfers.FirstOrDefault().Id));

			cart = client.ShoppingCart(customerSession.CustomerSessionId);

			if (cart.MoneyTransfers.Count == 0)
			{
				isSendMoneyParked = true;
			}
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return isSendMoneyParked;
		}

		private bool RemoveSenMoney(string channelPartnerName)
		{
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			MGI.Channel.DMS.Server.Data.MGIContext context;
			bool isSendMoneyRemoved = false;
			Desktop client = new Desktop();
			PerformSendMoney(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), out context);

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);

			client.RemoveMoneyTransfer(Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(cart.MoneyTransfers.FirstOrDefault().Id));

			cart = client.ShoppingCart(customerSession.CustomerSessionId);

			if (cart.MoneyTransfers.Count == 0)
			{
				isSendMoneyRemoved = true;
			}
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return isSendMoneyRemoved;
		}

		private void DoAddReceiver(string channelPartnerName)
		{
            Desktop client = new Desktop();
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			Receiver receiver = IntegrationTestData.GetReceiverData(channelPartnerName);
			client.SaveReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiver, context);
		}

		private ShoppingCartCheckoutStatus ModifySendMoney(string channelPartnerName)
		{
			Channel.DMS.Server.Data.MGIContext context;
			Desktop client = new Desktop();
			CustomerSession customerSession;
			TransactionHistory trx;
			MoneyTransferTransaction moneytransferTransaction;

			SendMoneyFetchMTCN(channelPartnerName, out context, out customerSession, out trx, out moneytransferTransaction);

			SendMoneySearchRequest request = new SendMoneySearchRequest()
			{
				ConfirmationNumber = moneytransferTransaction.ConfirmationNumber,
				SearchRequestType = SearchRequestType.Modify
			};

			SendMoneySearchResponse moneyTransferModify = client.SendMoneySearch(long.Parse(customerSession.CustomerSessionId), request, context);

			ModifySendMoneyRequest moneyTransferRequest = new Channel.Shared.Server.Data.ModifySendMoneyRequest()
			{
				FirstName = moneyTransferModify.FirstName,
				LastName = moneyTransferModify.LastName,
				SecondLastName = moneyTransferModify.SecondLastName,
				TransactionId = Convert.ToString(trx.TransactionId),
				ConfirmationNumber = moneyTransferModify.ConfirmationNumber,
				TestQuestion = moneyTransferModify.TestQuestion,
				TestAnswer = moneyTransferModify.TestAnswer
			};

			ModifySendMoneyResponse response = client.StageModifySendMoney(long.Parse(customerSession.CustomerSessionId), moneyTransferRequest, context);

			ShoppingCartCheckoutStatus cartStatus = Client.Checkout(customerSession.CustomerSessionId, GetRandomAmount(), customerSession.CardPresent ? customerSession.Customer.Fund.CardNumber : string.Empty, ShoppingCartCheckoutStatus.InitialCheckout, context);

			client.UpdateCounterId(long.Parse(customerSession.CustomerSessionId), context);

			return cartStatus;
		}

		private ShoppingCartCheckoutStatus RefundSendMoney(string channelPartnerName)
		{
			Channel.DMS.Server.Data.MGIContext context;
			Desktop client = new Desktop();
			CustomerSession customerSession;
			TransactionHistory trx;
			MoneyTransferTransaction moneytransferTransaction;

			SendMoneyFetchMTCN(channelPartnerName, out context, out customerSession, out trx, out moneytransferTransaction);

			SendMoneySearchRequest searchRequest = new SendMoneySearchRequest()
			{
				ConfirmationNumber = moneytransferTransaction.ConfirmationNumber,
				SearchRequestType = SearchRequestType.Refund
			};
			MoneyTransferTransactionDetailsViewModel tranDetails = new MoneyTransferTransactionDetailsViewModel();

			SendMoneySearchResponse response = client.SendMoneySearch(long.Parse(customerSession.CustomerSessionId), searchRequest, context);

			string RefundFlag = response.RefundStatus;

			string transactiontype = String.Empty;

			if (RefundFlag == "F")
			{
				transactiontype = "REFUND,F";
				tranDetails.RefundStatusDesc = "FULL REFUND AVAILABLE";
			}
			else if (RefundFlag == "N")
			{
				transactiontype = "REFUND,N";
				tranDetails.RefundStatusDesc = "PRINCIPAL REFUND AVAILABLE";
			}
			else if (RefundFlag == RefundType.FullAmount.ToString())
			{
				tranDetails.RefundStatusDesc = "FULL REFUND AVAILABLE";
			}
			else if (RefundFlag == RefundType.PrincipalAmount.ToString())
			{
				tranDetails.RefundStatusDesc = "PRINCIPAL REFUND AVAILABLE";
			}

			tranDetails.FeeRefund = response.FeeRefund;

			ReasonRequest request = new ReasonRequest()
			{
				TransactionType = transactiontype
			};

			tranDetails.RefundCategory = client.GetRefundReasons(long.Parse(customerSession.CustomerSessionId), request, context);
			tranDetails.RefundStatus = RefundFlag;

			RefundSendMoneyRequest refundRequest = new RefundSendMoneyRequest()
			{
				TransactionId = Convert.ToString(trx.TransactionId),
				ConfirmationNumber = moneytransferTransaction.ConfirmationNumber,
				CategoryCode = tranDetails.RefundCategory.Where(x => x.Value != string.Empty).FirstOrDefault().Value,
				CategoryDescription = tranDetails.RefundCategory.Where(x => x.Text.ToLower() != "select").FirstOrDefault().Text,
				Reason = "Need Money",
				RefundStatus = RefundFlag
			};

			var TrxId = client.MoneyTransferRefund(long.Parse(customerSession.CustomerSessionId), refundRequest, context);

			ShoppingCartCheckoutStatus cartStatus = ReceiveMoneyCheckout(context, client, customerSession, moneytransferTransaction); client.UpdateCounterId(long.Parse(customerSession.CustomerSessionId), context);

			return cartStatus;
		}

		private ShoppingCartCheckoutStatus ReceiveMoneyCheckout(Channel.DMS.Server.Data.MGIContext context, Desktop client, CustomerSession customerSession, MoneyTransferTransaction moneytransferTransaction)
		{
			ReceiveMoneyRequest rmRequest = new ReceiveMoneyRequest()
			{
				ConfirmationNumber = moneytransferTransaction.ConfirmationNumber
			};

			MoneyTransferTransaction transaction = client.GetReceiveTransaction(long.Parse(customerSession.CustomerSessionId), rmRequest, context);

			ValidateRequest validateRequest = new ValidateRequest()
			{
				TransactionId = long.Parse(transaction.TransactionID),
				TransferType = TransferType.RecieveMoney,
				Amount = transaction.DestinationPrincipalAmount
			};

			ValidateResponse validateResponse = client.ValidateTransfer(long.Parse(customerSession.CustomerSessionId), validateRequest, context);

			ShoppingCartCheckoutStatus cartStatus = Client.Checkout(customerSession.CustomerSessionId, GetRandomAmount(), customerSession.CardPresent ? customerSession.Customer.Fund.CardNumber : string.Empty, ShoppingCartCheckoutStatus.InitialCheckout, context);

			return cartStatus;
		}

		private void SendMoneyFetchMTCN(string channelPartnerName, out Channel.DMS.Server.Data.MGIContext context, out CustomerSession customerSession, out TransactionHistory trx, out MoneyTransferTransaction moneytransferTransaction)
		{
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);

			CustomerSearchCriteria searchCriteria = IntegrationTestData.GetSearchCriteria(channelPartner);
			client = new Desktop();
			context = new Channel.DMS.Server.Data.MGIContext() { IsAvailable = true };

			CustomerSearchResult[] searchResult = client.SearchCustomers(agentSession.SessionId, searchCriteria, context);
			var customer = searchResult.Where(c => c.ProfileStatus == "Active").FirstOrDefault();
			customerSession = client.InitiateCustomerSession(agentSession.SessionId, Convert.ToInt64(customer.AlloyID), 3, context);

			PerformSendMoney(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), out context);

			ShoppingCartCheckoutStatus status = Client.Checkout(customerSession.CustomerSessionId, GetRandomAmount(), customerSession.CardPresent ? customerSession.Customer.Fund.CardNumber : string.Empty, ShoppingCartCheckoutStatus.InitialCheckout, context);

			List<TransactionHistory> transactions = client.GetTransactionHistory(Convert.ToInt64(customerSession.CustomerSessionId), customerSession.Customer.CIN, string.Empty, string.Empty, DateTime.Now.AddDays(-1), context);

			trx = transactions.Where(x => x.TransactionType.ToLower() == "sendmoney").FirstOrDefault();

			moneytransferTransaction = client.GetMoneyTransferTransaction(Convert.ToInt64(agentSession.SessionId), Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(trx.TransactionId), context);
		}

		protected void UpdateCounterId(long customerSessionId, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
            Desktop client = new Desktop();
			context.IsAvailable = true;
			client.UpdateCounterId(customerSessionId, context);
		}

		#endregion
	}
}
