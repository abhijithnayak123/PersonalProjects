using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using MGI.Integration.Test.Data;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using static TCF.Zeo.Common.Util.Helper;

namespace MGI.Integration.Test
{
    [TestFixture]
	public partial class AlloyIntegrationTestFixture : BaseFixture
	{
		#region Members
		private BillerLocation BillerLocations { get; set; }
		private decimal BillpayAmount { get; set; }
		private decimal CashCollected { get; set; }
		private string AccountNumber { get; set; }
		private string BillerNameOrCode { get; set; }
		private decimal CashToCustomer { get; set; }
		#endregion

		#region SetUp
		[SetUp]
		public void Setup()
		{

		}
        #endregion

        #region BillPay Intergration Test Cases

        [TestCase("TCF")]
        public void DoBillpay(string channelPartnerName)
		{
            var tranHistory = DOBillpay(channelPartnerName,zeoContext);
			Assert.That(tranHistory, Is.Not.Null);
		}

        [TestCase("TCF")]
        public void BillPayRemove(string channelPartnerName)
		{
			bool isBillPayRemoved = RemoveBillPay(channelPartnerName);
			Assert.That(isBillPayRemoved, Is.True);
		}

        [TestCase("TCF")]
        public void BillPayPark(string channelPartnerName)
		{
			bool isBillPayParked = ParkBillPay(channelPartnerName);
			Assert.That(isBillPayParked, Is.True);
		}

        [TestCase("TCF")]
        public void BillPayParkUnPark(string channelPartnerName)
		{
			bool isBillPayParked = UnParkBillPay(channelPartnerName);
			Assert.That(isBillPayParked, Is.True);
		}

		private bool UnParkBillPay(string channelPartnerName)
		{
		    CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

			bool isBillPayRemoved = false;

            Response cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId, zeoContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
			if (cart.Bills.Count == 1)
			{
                client.RemoveBillPay(Convert.ToInt64(cart.Bills.FirstOrDefault().Id), zeoContext);
			}
			cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId, zeoContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            cart = cartResponse.Result as ShoppingCart;
			if (cart.Bills.Count == 0)
			{
				isBillPayRemoved = true;
			}
            //context.IsAvailable = true;
            client.UpdateCounterId(zeoContext);
			return isBillPayRemoved;
		}

		#endregion

		#region Private
	
		private List<TransactionHistory> DOBillpay(string channelPartnerName, ZeoContext zeoContext)
		{
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

			PerformBillPay(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), zeoContext);
            Response response = client.GetZeoContextForCustomer(Convert.ToInt64(customerSession.CustomerSessionId), zeoContext);
            zeoContext = response.Result as ZeoContext;
            Response statusResponse = client.ShoppingCartCheckout(Convert.ToDecimal(150), HelperShoppingCartCheckoutStatus.FinalCheckout, zeoContext);
            if (VerifyException(statusResponse)) throw new Exception(statusResponse.Error.Details);
            HelperShoppingCartCheckoutStatus status = (HelperShoppingCartCheckoutStatus)statusResponse.Result;

       

            TransactionHistoryRequest request = new TransactionHistoryRequest()
			{
				DateRange = 1
			};
            TransactionHistorySearchCriteria creteria = new TransactionHistorySearchCriteria()
            {
                DatePeriod = DateTime.Now,
                TransactionType = "BillPay",
                AgentId = zeoContext.AgentId,
                CustomerId = zeoContext.CustomerId,
                LocationName = zeoContext.LocationName,
                //TransactionId = zeoContext.id TrxId,
            };

			response = client.GetCustomerTransactions(creteria, zeoContext);
			var items = response.Result as List<TransactionHistory>;
            client.UpdateCounterId(zeoContext);
			return items;
		}

		private void PerformBillPay(string channelPartnerName, long CustomerSessionId, ZeoContext context)
		{

			BillpayAmount = GetRandomAmount();
            Response response = client.GetZeoContextForCustomer(CustomerSessionId,zeoContext);
            context = response.Result as ZeoContext;

            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            var billerName = "REGIONAL ACCEPTANCE";
            List<BillerLocation> billerLocation = GetMockLocationDetails(billerName);
             Response feeResponse = client.GetBillPayFee(0, "REGIONAL ACCEPTANCE", "1234561830", BillpayAmount, billerLocation.FirstOrDefault(), context);
            BillPayFee fee = feeResponse.Result as BillPayFee;
            Response billerResponse = client.GetBillerDetails("REGIONAL ACCEPTANCE", context);
            FavoriteBiller product = billerResponse.Result as FavoriteBiller;

			BillPayment billpayment = IntegrationTestData.GetBillerInformation(channelPartner);
			billpayment.PaymentAmount = BillpayAmount;
			billpayment.Fee = fee.DeliveryMethods.First().FeeAmount;
			//billpayment.BillerId =product.BillerId;
			billpayment.MetaData = new Dictionary<string, object>()
                            {
                                {"DeliveryCode", fee.DeliveryMethods.First().Code},
                                {"Location", billerLocation.First().Name},
                                {"SessionCookie", fee.SessionCookie},
                                {"Reference",null},
                                {"AailableBalance",fee.AvailableBalance},
                                {"AccountHolder",fee.AccountHolderName},
                                {"Attention",null},
                                {"DateOfBirth",null}
            };
			//context.TrxId = fee.TransactionId;
            Response validateBillpayResponse = client.ValidateBillPayment(fee.TransactionId, billpayment, context);
            BillPayValidateResponse validate = (validateBillpayResponse.Result as BillPayValidateResponse);
            long id = validate.TransactionId;
			context.RequestType = RequestType.Hold.ToString();
			client.StageBillPayment(fee.TransactionId, context);
		}

		private bool RemoveBillPay(string channelPartnerName)
		{
			//MGI.Channel.DMS.Server.Data.MGIContext context;
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
            PerformRemoveBillPay(channelPartnerName, customerSession.CustomerSessionId, zeoContext);

            Response response = client.GetZeoContextForCustomer(customerSession.CustomerSessionId, zeoContext);
            ZeoContext context = response.Result as ZeoContext;
            bool isBillPayRemoved = false;

			Response cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId, context);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;

			client.RemoveBillPay( Convert.ToInt64(cart.Bills.FirstOrDefault().Id),context);
			cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId, context);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            cart = cartResponse.Result as ShoppingCart;

			if (cart.Bills.Count == 0)
			{
				isBillPayRemoved = true;
			}
			//context.IsAvailable = true;
			client.UpdateCounterId( context);
			return isBillPayRemoved;
		}

		private bool ParkBillPay(string channelPartnerName)
		{
            ZeoContext Context = new ZeoContext();
            CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			PerformBillPay(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId),  Context);

            Response response = client.GetZeoContextForAgent(customerSession.CustomerSessionId, Context);
            ZeoContext alloyContext = response.Result as ZeoContext;

            bool isBillPayParked = false;

			Response cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId,alloyContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
			cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId,alloyContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            cart = cartResponse.Result as ShoppingCart;

			if (cart.Bills.Count == 1)
			{
				isBillPayParked = true;
			}
			//context.IsAvailable = true;
			client.UpdateCounterId(alloyContext);
			return isBillPayParked;

		}

		private bool WUCardEnrollment(CustomerSession customerSession, ZeoContext alloyContext)
		{
			CardLookupDetails cardlookupreq = new CardLookupDetails()
			{
				FirstName = customerSession.Customer.FirstName,
				LastName = customerSession.Customer.LastName,
            };
			Response response = new Response();
			List<WUCustomerGoldCardResult> customers = new List<WUCustomerGoldCardResult>();

			try
			{
				response = Client.WUCardLookup( cardlookupreq, alloyContext);
				customers = response.Result as List<WUCustomerGoldCardResult>;
			}
			catch (Exception) { }

             response = client.GetZeoContextForAgent(customerSession.CustomerSessionId, alloyContext);
             ZeoContext Context = response.Result as ZeoContext;

            string wuGoldCardNumber;
			bool isUpdated;
            response = client.GetCustomer(alloyContext);
            CustomerProfile customer = response.Result as CustomerProfile;
			if (customers.Count() != 0)
			{
				wuGoldCardNumber = customers.Select(x => x.WUGoldCardNumber).FirstOrDefault();
				response = Client.UpdateCustomer(customer, Context);
				isUpdated = Convert.ToBoolean(response.Result);
			}
			else
			{
                MoneyTransferTransaction paymentDetails = new MoneyTransferTransaction();
				paymentDetails.DestinationCountryCode = "US";
				paymentDetails.DestinationCurrencyCode = "USD";

				paymentDetails.OriginatingCountryCode = "US";
				response = Client.GetCurrencyCode( "US", Context);
				paymentDetails.OriginatingCurrencyCode = response.Result as string;

				response = Client.WUCardEnrollment(Context);
				CardDetails cardDetails = response.Result as CardDetails;
				isUpdated = !string.IsNullOrEmpty(cardDetails.AccountNumber);
			}
            //Context.IsAvailable = true;
			client.UpdateCounterId(Context);
			return isUpdated;
		}

        private List<BillerLocation> GetMockLocationDetails(string billerName)
        {
            List<BillerLocation> locations = null;
            switch (billerName.ToUpper())
            {
                case "REGIONAL ACCEPTANCE":
                    locations = new List<BillerLocation>() {
                        new BillerLocation() { Id = "1", Name = "KINGFISH FL", Type = "03" },
                        new BillerLocation() { Id = "2", Name = "RACCHES VA", Type = "03" },
                        new BillerLocation() { Id = "3", Name = "RACWEST NC", Type = "03" },
                        new BillerLocation() { Id = "4", Name = "RECRCY NC", Type = "03" },
                        new BillerLocation() { Id = "5", Name = "REGA NC", Type = "03" },
                        new BillerLocation() { Id = "6", Name = "REGI NC", Type = "03" },
                        new BillerLocation() { Id = "7", Name = "REGIONALCREDIT TX", Type = "03" }
                    };
                    break;
                default:
                    locations = new List<BillerLocation>();
                    break;
            }
            return locations;
        }

        private void PerformRemoveBillPay(string channelPartnerName, long CustomerSessionId, ZeoContext context)
        {

            BillpayAmount = GetRandomAmount();
            Response response = client.GetZeoContextForCustomer(CustomerSessionId, zeoContext);
            context = response.Result as ZeoContext;

            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            var billerName = "REGIONAL ACCEPTANCE";
            List<BillerLocation> billerLocation = GetMockLocationDetails(billerName);
            Response feeResponse = client.GetBillPayFee(0, "REGIONAL ACCEPTANCE", "1234561830", BillpayAmount, billerLocation.FirstOrDefault(), context);
            BillPayFee fee = feeResponse.Result as BillPayFee;
            Response billerResponse = client.GetBillerDetails("REGIONAL ACCEPTANCE", context);
            FavoriteBiller product = billerResponse.Result as FavoriteBiller;

            BillPayment billpayment = IntegrationTestData.GetBillerInformation(channelPartner);
            billpayment.PaymentAmount = BillpayAmount;
            billpayment.Fee = fee.DeliveryMethods.First().FeeAmount;
            //billpayment.BillerId =product.BillerId;
            billpayment.MetaData = new Dictionary<string, object>()
                            {
                                {"DeliveryCode", fee.DeliveryMethods.First().Code},
                                {"Location", billerLocation.First().Name},
                                {"SessionCookie", fee.SessionCookie},
                                {"Reference",null},
                                {"AailableBalance",fee.AvailableBalance},
                                {"AccountHolder",fee.AccountHolderName},
                                {"Attention",null},
                                {"DateOfBirth",null}
            };
            //context.TrxId = fee.TransactionId;
            Response validateBillpayResponse = client.ValidateBillPayment(fee.TransactionId, billpayment, context);
            BillPayValidateResponse validate = (validateBillpayResponse.Result as BillPayValidateResponse);
            long id = validate.TransactionId;
            context.RequestType = RequestType.Hold.ToString();
            client.StageBillPayment(fee.TransactionId, context);
        }

        #endregion
    }
}
