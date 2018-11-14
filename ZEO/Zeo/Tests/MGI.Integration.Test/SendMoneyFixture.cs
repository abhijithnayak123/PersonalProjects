using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using MGI.Integration.Test.Data;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using static TCF.Zeo.Common.Util.Helper;
using System.Web.Mvc;

namespace MGI.Integration.Test
{

	[TestFixture]
	public partial class AlloyIntegrationTestFixture : BaseFixture
	{
        public decimal SendMoneyAmount { get; set; }

        #region SendMoney Integration Test cases
        [TestCase("TCF")]
		public void DoSendMoneyIT(string channelPartnerName)
		{
			var tranHistory = DoSendMoney(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

        [TestCase("TCF")]
        public void SendMoneyPark(string channelPartnerName)
        {
            bool isSendMoneyParked = false;
            isSendMoneyParked = ParkSenMoney(channelPartnerName);
            Assert.That(isSendMoneyParked, Is.True);
        }

        [TestCase("TCF")]
        public void SendMoneyParkUnPark(string channelPartnerName)
        {
            var tranHistory = DoSendMoney(channelPartnerName);
            Assert.That(tranHistory, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void SendMoneyRemove(string channelPartnerName)
        {
            bool isSendMoneyRemoved = false;
            isSendMoneyRemoved = RemoveSenMoney(channelPartnerName);
            Assert.That(isSendMoneyRemoved, Is.True);
        }

        [TestCase("TCF")]
        public void SendMoneyModify(string channelPartnerName)
        {
            ShoppingCartCheckoutStatus cartStatus = ModifySendMoney(channelPartnerName);

            Assert.AreEqual(cartStatus, ShoppingCartCheckoutStatus.Completed);
        }


        [TestCase("TCF")]
        public void SendMoneyRefund(string channelPartnerName)
        {
            ShoppingCartCheckoutStatus cartStatus = RefundSendMoney(channelPartnerName);
            Assert.AreEqual(cartStatus, ShoppingCartCheckoutStatus.Completed);
        }

        #endregion

        #region Private Methods

        private List<TransactionHistory> DoSendMoney(string channelPartnerName)
		{
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			PerformSendMoney(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(customerSession.CustomerId), out zeoContext);
            var status = client.ShoppingCartCheckout(GetRandomAmount(),HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);
            DateTime dateRange = DateTime.Now.AddDays(-60);
            var customerZeoContext = client.GetZeoContextForCustomer(Convert.ToInt64(customerSession.CustomerSessionId), zeoContext);
            zeoContext = customerZeoContext.Result as ZeoContext;
            TransactionHistorySearchCriteria searchCriteria = new TransactionHistorySearchCriteria()
            {
                DatePeriod = dateRange,
                TransactionType = "SendMoney",
                CustomerId = customerSession.CustomerId,
                LocationName = "IT_TCF"
            };
            var items = client.GetCustomerTransactions(searchCriteria,zeoContext);
			client.UpdateCounterId(zeoContext);
			return items.Result as List<TransactionHistory>;
		}

		private void PerformSendMoney(string channelPartnerName, long CustomerSessionId,long CustomerId, out ZeoContext zeoContext)
		{
			Response serviceResponse = new Response();
            zeoContext = new ZeoContext();
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName,zeoContext);
            SendMoneyAmount = GetRandomAmount();
			Receiver receiver = GetReceiver(Convert.ToInt64(CustomerSessionId), Convert.ToInt64(CustomerId), channelPartnerName);
            zeoContext.CustomerId = CustomerId;
            zeoContext.ChannelPartnerName = channelPartnerName;
            zeoContext.CustomerSessionId = CustomerSessionId;
            zeoContext.ChannelPartnerId = channelPartner.Id;
            FeeRequest feeRequest = GetMoneyTransferFeeRequest(Convert.ToInt64(CustomerSessionId), receiver, zeoContext);
			serviceResponse = client.GetFeeMoneyTransfer(feeRequest, zeoContext);
			FeeResponse feeResponse = serviceResponse.Result as FeeResponse;
			ValidateRequest validateRequest = GetValidateRequest(feeResponse, feeRequest, receiver);
			serviceResponse = client.Validate(validateRequest, zeoContext);
			ValidateResponse response = serviceResponse.Result as ValidateResponse;
		}

		private Receiver GetReceiver(long customerSessionId,long CustomerId, string channelPartnerName)
		{
			Response response = new Response();
            zeoContext.CustomerId = CustomerId;
            zeoContext.ChannelPartnerName = channelPartnerName;
            response = client.GetFrequentReceivers(zeoContext);
			List<Receiver> receivers = response.Result as List<Receiver>;
			Receiver receiver = new Receiver();
			if (receivers.Count == 0)
			{
				receiver = IntegrationTestData.GetReceiverData(channelPartnerName);
				client.AddReceiver(receiver, zeoContext);
				response = client.GetFrequentReceivers(zeoContext);
				receivers = response.Result as List<Receiver>;
			}
			receiver = receivers.FirstOrDefault();
            receiver.DeliveryMethod = "000";
            return receiver;
		}

		private FeeRequest GetMoneyTransferFeeRequest(long customerSessionId, Receiver receiver, ZeoContext zeoContext)
        {
			Response response = new Response();
			response = client.GetCurrencyCode(receiver.PickupCountry, zeoContext);
			string currencyCode = response.Result as string;
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
                validateRequest.TransferType = HelperMoneyTransferType.Send;
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
            bool isSendMoneyParked = false;
            PerformSendMoney(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(customerSession.CustomerId), out zeoContext);
            Response cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId,zeoContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
            client.ParkShoppingCartTransaction(Convert.ToInt64(cart.MoneyTransfers.FirstOrDefault().Id), (int)ProductType.SendMoney,zeoContext);
            cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId,zeoContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            cart = cartResponse.Result as ShoppingCart;
            if (cart.MoneyTransfers.Count == 0)
            {
                isSendMoneyParked = true;
            }
            zeoContext.CustomerSessionId = customerSession.CustomerSessionId;
            client.UpdateCounterId(zeoContext);
            return isSendMoneyParked;
        }

        private bool RemoveSenMoney(string channelPartnerName)
        {
            CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
            bool isSendMoneyRemoved = false;
            PerformSendMoney(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(customerSession.CustomerId), out zeoContext);
            Response cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId,zeoContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
            client.RemoveMoneyTransfer(Convert.ToInt64(cart.MoneyTransfers.FirstOrDefault().Id), (int)ProductType.SendMoney,zeoContext);
            cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId,zeoContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            cart = cartResponse.Result as ShoppingCart;
            if (cart.MoneyTransfers.Count == 0)
            {
                isSendMoneyRemoved = true;
            }
            client.UpdateCounterId(zeoContext);
            return isSendMoneyRemoved;
        }

        private ShoppingCartCheckoutStatus ModifySendMoney(string channelPartnerName)
        {
            Response serviceResponse = new Response();
            CustomerSession customerSession;
            TransactionHistory trx;
            MoneyTransferTransaction moneytransferTransaction;
            SendMoneyFetchMTCN(channelPartnerName, out zeoContext, out customerSession, out trx, out moneytransferTransaction);

            SearchRequest request = new SearchRequest()
            {
                ConfirmationNumber = moneytransferTransaction.MTCN,
                SearchRequestType = (int)SearchRequestType.Modify
            };
            serviceResponse = client.Search(request, zeoContext);
            SearchResponse modifySearch = serviceResponse.Result as SearchResponse;

            ModifyRequest moneyTransferRequest = new ModifyRequest()
            {
                FirstName = modifySearch.FirstName,
                LastName = modifySearch.LastName,
                SecondLastName = modifySearch.SecondLastName,
                TransactionId = trx.TransactionId,
                ConfirmationNumber = moneytransferTransaction.MTCN,
                TestQuestion = modifySearch.TestQuestion,
                TestAnswer = modifySearch.TestAnswer
            };
            serviceResponse = client.StageModify(moneyTransferRequest,zeoContext);
            ModifyResponse response = serviceResponse.Result as ModifyResponse;
            Response cartStatusResponse = client.ShoppingCartCheckout(GetRandomAmount(), HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);
            if (VerifyException(cartStatusResponse)) throw new Exception(cartStatusResponse.Error.Details);
            ShoppingCartCheckoutStatus cartStatus = (ShoppingCartCheckoutStatus)cartStatusResponse.Result;
            client.UpdateCounterId(zeoContext);
            return cartStatus;
        }

        private ShoppingCartCheckoutStatus RefundSendMoney(string channelPartnerName)
        {
            Response serviceResponse = new Response();
            CustomerSession customerSession;
            TransactionHistory trx;
            MoneyTransferTransaction moneytransferTransaction;
            SendMoneyFetchMTCN(channelPartnerName, out zeoContext, out customerSession, out trx, out moneytransferTransaction);
            SearchRequest searchRequest = new SearchRequest()
            {
                ConfirmationNumber = moneytransferTransaction.MTCN,
                SearchRequestType = (int)SearchRequestType.Modify
            };
            serviceResponse = client.Search(searchRequest, zeoContext);
            SearchResponse modifySearch = serviceResponse.Result as SearchResponse;
            string RefundFlag = modifySearch.RefundStatus;
            string transactiontype = String.Empty;
            string RefundStatusDesc = string.Empty;

            if (RefundFlag == "F")
            {
                transactiontype = "REFUND,F";
                RefundStatusDesc = "FULL REFUND AVAILABLE";
            }
            else if (RefundFlag == "N")
            {
                transactiontype = "REFUND,N";
                RefundStatusDesc = "PRINCIPAL REFUND AVAILABLE";
            }
            else if (RefundFlag == RefundType.FullAmount.ToString())
            {
                RefundStatusDesc = "FULL REFUND AVAILABLE";
            }
            else if (RefundFlag == RefundType.PrincipalAmount.ToString())
            {
                RefundStatusDesc = "PRINCIPAL REFUND AVAILABLE";
            }
            ReasonRequest request = new ReasonRequest()
            {
                TransactionType = transactiontype
            };
            serviceResponse = client.GetRefundReasons(request, zeoContext);
            List<Reason> pickupOptionsResult = serviceResponse.Result as List<Reason>;
            List<SelectListItem> pickupOptions = new List<SelectListItem>();
            if (pickupOptionsResult.Count > 0)
            {
                foreach (var item in pickupOptionsResult)
                {
                    pickupOptions.Add(new SelectListItem() { Value = item.Code, Text = item.Name });
                }
            }
            SendMoneyRefundRequest refundRequest = new SendMoneyRefundRequest()
            {
                TransactionId = Convert.ToString(trx.TransactionId),
                ConfirmationNumber = moneytransferTransaction.MTCN,
                CategoryCode = pickupOptions.Where(x => x.Value != string.Empty).FirstOrDefault().Value,
                CategoryDescription = pickupOptions.Where(x => x.Text.ToLower() != "select").FirstOrDefault().Text,
                Reason = "Need Money",
                RefundStatus = RefundFlag
            };

            var TrxId = client.SendMoneyRefund(refundRequest, zeoContext);

            ShoppingCartCheckoutStatus cartStatus = ReceiveMoneyCheckout(zeoContext, customerSession, moneytransferTransaction);
            client.UpdateCounterId(zeoContext);

            return cartStatus;
        }

        private ShoppingCartCheckoutStatus ReceiveMoneyCheckout(ZeoContext zeoContext, CustomerSession customerSession, MoneyTransferTransaction moneytransferTransaction)
        {
            Response response = new Response();
            ReceiveMoneyRequest rmRequest = new ReceiveMoneyRequest()
            {
                ConfirmationNumber = moneytransferTransaction.MTCN
            };
            response = client.ReceiveMoneySearch( rmRequest, zeoContext);
            MoneyTransferTransaction transaction = response.Result as MoneyTransferTransaction;
            ValidateRequest validateRequest = new ValidateRequest()
            {
                TransactionId = transaction.TransactionId,
                TransferType = HelperMoneyTransferType.Receive,
                Amount = transaction.DestinationPrincipalAmount
            };
            response = client.Validate(validateRequest, zeoContext);
            ValidateResponse validateResponse = response.Result as ValidateResponse;
            Response cartStatusResponse = client.ShoppingCartCheckout(GetRandomAmount(), HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);
            if (VerifyException(cartStatusResponse)) throw new Exception(cartStatusResponse.Error.Details);
            ShoppingCartCheckoutStatus cartStatus = (ShoppingCartCheckoutStatus)cartStatusResponse.Result;
            return cartStatus;
        }

        private void SendMoneyFetchMTCN(string channelPartnerName, out ZeoContext zeoContext, out CustomerSession customerSession, out TransactionHistory trx, out MoneyTransferTransaction moneytransferTransaction)
        {
            zeoContext = new ZeoContext();
            DateTime dateRange = DateTime.Now.AddDays(-60);
            customerSession = InitiateCustomerSession(channelPartnerName);

            PerformSendMoney(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(customerSession.CustomerId), out zeoContext);
            Response cartStatusResponse = client.ShoppingCartCheckout(GetRandomAmount(), HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);
            if (VerifyException(cartStatusResponse)) throw new Exception(cartStatusResponse.Error.Details);
            ShoppingCartCheckoutStatus cartStatus = (ShoppingCartCheckoutStatus)cartStatusResponse.Result;
            TransactionHistorySearchCriteria searchCriteria = new TransactionHistorySearchCriteria()
            {
                DatePeriod = dateRange,
                TransactionType = "SendMoney",
                CustomerId = customerSession.CustomerId,
                LocationName = "TCF Service Desk"
            };
            Response response = client.GetCustomerTransactions(searchCriteria, zeoContext);
            List<TransactionHistory> transactions = response.Result as List<TransactionHistory>;
            trx = transactions.Where(x => x.TransactionType.ToLower() == "sendmoney").FirstOrDefault();
            response = client.GetMoneyTransferTransaction(Convert.ToInt64(trx.TransactionId), zeoContext);
            moneytransferTransaction = response.Result as MoneyTransferTransaction;
        }

        #endregion
    }
}
