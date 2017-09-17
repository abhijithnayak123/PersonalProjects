using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient.DMSService;

namespace MGI.Channel.DMS.Web.ServiceClient.Test
{
	[TestFixture]
	public class CheckOutProcessTest
	{
		public Desktop DeskTop { get; set; }
		public IDesktopService DesktopEngine { get; set; }
		//int cardPresentedType = 0;

		[SetUp]
		public void Setup()
		{
			DeskTop = new Desktop();
		}

		//long agentSessionId = 1000002070;
		//[Test]
		//public void CheckOutTest()
		//{
		//	System.Collections.Generic.Dictionary<string, object> context = new Dictionary<string, object>();
		//	//Session = DeskTop.AuthenticateAgent("ZeoMGI", "Sysadmin@123", "34", "OPT-LAP-0128");
		//	//AgentSession Session = DeskTop.AuthenticateAgent("sysadmin", "Password@123", "28", "Carver");
		//	long alloyId = 1000000000000030;
		//	CustomerSession customerSession
		//			= DeskTop.InitiateCustomerSession(agentSessionId.ToString(), alloyId, cardPresentedType);
		//	ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
		//	Check check = DesktopEngine.GetCheckStatus(customerSession.CustomerSessionId, "C68302CD-C662-4621-99CC-4890242C1549", context);
		//	//cart.AddCheque(check);


		//	List<Product> billers = DeskTop.GetFrequentBillers(alloyId);

		//	Product biller = DeskTop.GetBiller(34, billers[2].ProductName);
		//	Bill bill = new Bill();
		//	bill.BillerId = biller.Id;
		//	bill.PrimaryAuth = "523";
		//	bill.SecondaryAuth = "234";
		//	bill.Amount = 5;
		//	bill.Status = "validated";
		//	bill.AcceptedFee = true;
		//	bill.Fee = DeskTop.GetCustomerBillFee(biller.Id);
		//	bill.Id = Guid.NewGuid().ToString();
		//	// cart. AddBill(bill);

		//	bill = new Bill();
		//	bill.BillerId = biller.Id;
		//	bill.PrimaryAuth = "556";
		//	bill.SecondaryAuth = "678";
		//	bill.Amount = 5000;
		//	bill.Status = "validated";
		//	bill.AcceptedFee = true;
		//	bill.Fee = DeskTop.GetCustomerBillFee(biller.Id);
		//	bill.Id = Guid.NewGuid().ToString();
		//	//    cart.AddBill(bill);

		//	cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
		//	Assert.Greater(cart.Bills.Count, 0);
		//	Assert.Greater(cart.BillTotal, 0);

		//	//    cart.CashCollected = 100;


		//	int channelPartnerId = 27;
		//	//Checkout(string customerSessionId, decimal cashToCustomer, decimal cashCollected, decimal priorCashCollected, string cardNumber, SharedData.ShoppingCartCheckoutStatus shoppingCartStatus, Dictionary<string,object> context)
		//	//Checkout(string customerSessionId, decimal cashToCustomer, decimal cashCollected, decimal priorCashCollected, string cardNumber, SharedData.ShoppingCartCheckoutStatus shoppingCartStatus, Dictionary<string,object> context)
		//	//r		int receipts = DeskTop.Checkout(alloyId.ToString(), cart, long.Parse(customerSession.CustomerSessionId), channelPartnerId, customerSession.Customer. CardBalance);

		//	//            int receipts = DeskTop.Checkout(alloyId.ToString(),cart,long.Parse(customerSession.CustomerSessionId),channelPartnerId,customerSession.Customer.Profile.CardBalance);
		//	// Assert.GreaterOrEqual(receipts.ReceiptData.ReceiptData.Count, 1);
		//	//Assert.GreaterOrEqual(receipts,1);
		//	string originalCartId = cart.Id.ToString();
		//	DeskTop.RemoveShoppingCart(originalCartId);
		//	cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
		//	Assert.AreNotEqual(originalCartId, cart.Id);
		//}

		//[Test]
		//public void CalculateTotalsTest()
		//{
		//	long alloyId = 1000000000000920;
		//	//long alloyId = 1000000008519520;
		//	CustomerSession customerSession
		//			= DeskTop.InitiateCustomerSession(agentSessionId.ToString(), alloyId, cardPresentedType);
		//	ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);

		//	List<Product> billers = DeskTop.GetFrequentBillers(alloyId);
		//	Product biller = DeskTop.GetBiller(34, billers[0].ProductName);
		//	biller.Fee = DeskTop.GetCustomerBillFee(biller.Id);
		//	Bill bill = new Bill();
		//	bill.Id = Guid.NewGuid().ToString();
		//	bill.BillerId = biller.Id;
		//	bill.PrimaryAuth = "123";
		//	bill.SecondaryAuth = "234";
		//	bill.Amount = 5;
		//	bill.Status = "validated";
		//	bill.AcceptedFee = true;
		//	bill.Fee = (decimal)biller.Fee;
		//	bill.BillTotal = bill.Amount + bill.Fee;
		//	//cart.AddBill(bill);

		//	Assert.AreEqual(cart.BillTotal, bill.Amount + bill.Fee);
		//	cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
		//	Assert.AreEqual(cart.BillTotal, bill.Amount + biller.Fee);
		//}
	}
}