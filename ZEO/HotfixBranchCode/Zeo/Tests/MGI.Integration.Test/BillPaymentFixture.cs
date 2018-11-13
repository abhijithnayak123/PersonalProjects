using System;
using System.Collections.Generic;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using System.Linq;
using MGI.Integration.Test.Data;


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

		[TestCase("Synovus")]
		//[TestCase("TCF")]
		[TestCase("Carver")]
		public void DoBillpay(string channelPartnerName)
		{
			var tranHistory = DOBillpay(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

		[TestCase("Synovus")]
		//[TestCase("TCF")]
		[TestCase("Carver")]
		public void BillPayRemove(string channelPartnerName)
		{
			bool isBillPayRemoved = RemoveBillPay(channelPartnerName);
			Assert.That(isBillPayRemoved, Is.True);
		}

		[TestCase("Synovus")]
		//[TestCase("TCF")]
		[TestCase("Carver")]
		public void BillPayPark(string channelPartnerName)
		{
			bool isBillPayParked = ParkBillPay(channelPartnerName);
			Assert.That(isBillPayParked, Is.True);
		}

		[TestCase("Synovus")]
		//[TestCase("TCF")]
		[TestCase("Carver")]
		public void BillPayParkUnPark(string channelPartnerName)
		{
			bool isBillPayParked = UnParkBillPay(channelPartnerName);
			Assert.That(isBillPayParked, Is.True);
		}

		private bool UnParkBillPay(string channelPartnerName)
		{
			Desktop client = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext context = new Channel.DMS.Server.Data.MGIContext();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

			bool isBillPayRemoved = false;

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);
			if (cart.Bills.Count == 1)
			{
				client.RemoveBill(Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(cart.Bills.FirstOrDefault().Id));
			}
			cart = client.ShoppingCart(customerSession.CustomerSessionId);
			if (cart.Bills.Count == 0)
			{
				isBillPayRemoved = true;
			}
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return isBillPayRemoved;
		}

		[TestCase("Synovus")]
		//[TestCase("TCF")]
		[TestCase("Carver")]
		public void WUCardEnroll(string channelPartnerName)
		{
			CustomerSession = InitiateCustomerSession(channelPartnerName);
			bool isWuGoldCardCustomer = CustomerSession.Customer.IsWUGoldCard;
			if (!isWuGoldCardCustomer)
			{
				//enroll WU Card if not registered
				isWuGoldCardCustomer = WUCardEnrollment(CustomerSession, MgiContext);
			}
			Assert.That(isWuGoldCardCustomer, Is.True);
		}

		#endregion

		#region Private
		private CustomerSession InitiateCustomerSession(string channelPartnerName)
		{
			Desktop client = new Desktop();
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			Channel.DMS.Server.Data.MGIContext context = new Channel.DMS.Server.Data.MGIContext();
			CustomerSearchCriteria searchCriteria = IntegrationTestData.GetSearchCriteria(channelPartner);
			CustomerSearchResult[] searchResult = client.SearchCustomers(agentSession.SessionId, searchCriteria, context);
			var customer = searchResult.Where(c => c.ProfileStatus == "Active").FirstOrDefault();
			CustomerSession customerSession = client.InitiateCustomerSession(agentSession.SessionId, Convert.ToInt64(customer.AlloyID), 3, context);
			return customerSession;
		}

		private List<TransactionHistory> DOBillpay(string channelPartnerName)
		{
			Desktop client = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext context;
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			PerformBillPay(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), out context);
			ShoppingCartCheckoutStatus status = client.Checkout(customerSession.CustomerSessionId, Convert.ToDecimal(150), string.Empty, ShoppingCartCheckoutStatus.FinalCheckout, context);
		
			TransactionHistoryRequest request = new TransactionHistoryRequest()
			{
				DateRange = 1
			};

			var items = client.GetTransactionHistory(Convert.ToInt64(customerSession.CustomerSessionId), customerSession.Customer.CIN, string.Empty, string.Empty, DateTime.Now.AddDays(-1), context);
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return items;
		}

		private void PerformBillPay(string channelPartnerName, long CustomerSessionId, out MGI.Channel.DMS.Server.Data.MGIContext context)
		{
			Desktop client = new Desktop();

			BillpayAmount = GetRandomAmount();

			context = new Channel.DMS.Server.Data.MGIContext();
			ChannelPartner channelPartner = GetChannelPartner(channelPartnerName);
			BillPayLocation billerLocation = client.GetLocations(CustomerSessionId, "REGIONAL ACCEPTANCE", "1234561830", BillpayAmount, context);
			BillFee fee = client.GetFee(CustomerSessionId, "REGIONAL ACCEPTANCE", "1234561830", BillpayAmount, billerLocation.BillerLocation.First(), context);
			MGI.Channel.Shared.Server.Data.Product product = client.GetBiller(CustomerSessionId, channelPartner.Id, "REGIONAL ACCEPTANCE", context);

			BillPayment billpayment = IntegrationTestData.GetBillerInformation(channelPartner);
			billpayment.PaymentAmount = BillpayAmount;
			billpayment.Fee = fee.DeliveryMethods.First().FeeAmount;
			billpayment.BillerId = product.Id;
			billpayment.MetaData = new Dictionary<string, object>()
                            {
                                {"DeliveryCode", fee.DeliveryMethods.First().Code},
                                {"Location", billerLocation.BillerLocation.First().Name},
                                {"SessionCookie", fee.SessionCookie},
                                {"Reference",null},
                                {"AailableBalance",fee.AvailableBalance},
                                {"AccountHolder",fee.AccountHolderName},
                                {"Attention",null},
                                {"DateOfBirth",null}
            };
			context.TrxId = fee.TransactionId;
			long id = client.ValidateBillPayment(CustomerSessionId, billpayment, context);
			context.RequestType = RequestType.HOLD.ToString();
			client.StageBillPayment(CustomerSessionId, id, context);
		}

		private bool RemoveBillPay(string channelPartnerName)
		{
			Desktop client = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext context;
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			PerformBillPay(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), out context);

			bool isBillPayRemoved = false;

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);

			client.RemoveBill(Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(cart.Bills.FirstOrDefault().Id));
			cart = client.ShoppingCart(customerSession.CustomerSessionId);
			if (cart.Bills.Count == 0)
			{
				isBillPayRemoved = true;
			}
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return isBillPayRemoved;
		}

		private bool ParkBillPay(string channelPartnerName)
		{
			Desktop client = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext context;
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			PerformBillPay(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), out context);

			bool isBillPayParked = false;

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);

			client.ParkBillPay(Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(cart.Bills.FirstOrDefault().Id));
			cart = client.ShoppingCart(customerSession.CustomerSessionId);

			if (cart.Bills.Count == 0)
			{
				isBillPayParked = true;
			}
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return isBillPayParked;

		}

		private bool WUCardEnrollment(CustomerSession customerSession, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			CardLookupDetails cardlookupreq = new CardLookupDetails()
			{
				firstname = customerSession.Customer.PersonalInformation.FName,
				lastname = customerSession.Customer.PersonalInformation.LName
			};

			List<WUCustomerGoldCardResult> customers = new List<WUCustomerGoldCardResult>();

			//this is work around till the WU fixes the issue with WUCardlookup by name from there end
			try
			{
				customers = Client.WUCardLookup(Convert.ToInt64(customerSession.CustomerSessionId), cardlookupreq, mgiContext).ToList();
			}
			catch (Exception) { }

			string wuGoldCardNumber;
			bool isUpdated;
			if (customers.Count() != 0)
			{
				wuGoldCardNumber = customers.Select(x => x.WUGoldCardNumber).FirstOrDefault();

				isUpdated = Client.UpdateCustomerProfile(Convert.ToInt64(customerSession.CustomerSessionId), wuGoldCardNumber, mgiContext);
			}
			else
			{
				XferPaymentDetails paymentDetails = new XferPaymentDetails();
				paymentDetails.DestinationCountryCode = "US";
				paymentDetails.DestinationCurrencyCode = "USD";

				paymentDetails.OriginatingCountryCode = "US";
				paymentDetails.OriginatingCurrencyCode = Client.GetCurrencyCode(Convert.ToInt64(customerSession.CustomerSessionId), "US", mgiContext);

				CardDetails cardDetails = Client.WUCardEnrollment(Convert.ToInt64(customerSession.CustomerSessionId), paymentDetails, mgiContext);
				isUpdated = !string.IsNullOrEmpty(cardDetails.AccountNumber);
			}
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return isUpdated;
		}
		#endregion
	}
}
