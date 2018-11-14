using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.DMS.Server.Contract;

using NUnit.Framework;

using Spring.Context;
using Spring.Context.Support;
using MGI.Channel.Shared.Server.Data;
using Spring.Testing.NUnit;

namespace MGI.Channel.DMS.Server.Test
{
	[TestFixture]
	public class BillPayEngineTest : AbstractTransactionalSpringContextTests
	{

		public MGIContext mgiContext { get; set; }
		private static string DESKTOP_ENGINE = "DesktopEngine";

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }

		}

		IDesktopService DeskTopTest { get; set; }
		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);
		}

		[Test]
		public void BillerSearchTest()
		{
			//string agentSessionId = "1000005342";
			//CustomerAuthentication customer = new CustomerAuthentication();
			//customer.AlloyID = 1000000000004890;
			long channelPartnerID = 34;
			string searchTerm = "AT";
			//customer.PAN = "1000000008519520";
			//CustomerSession customerSession
			//		= DeskTopTest.InitiateCustomerSession(agentSessionId, customer, context);
            Response response = DeskTopTest.GetBillers(0, channelPartnerID, searchTerm, mgiContext);
			Assert.AreNotEqual(response.Result, null);
			Assert.Less((response.Result as List<string>).Count, 7);
		}

		[Test]
		public void BillerSearchTest2()
		{
			//AgentSession Session = DeskTopTest.Authenticate("anil", "Anil@123", "centris", "Anil");

			//CustomerAuthentication customer = new CustomerAuthentication();

			////customer.PAN = "1000000008519520";

			//CustomerSession customerSession
			//		= DeskTopTest.InitiateCustomerSession(Session.SessionId, customer, context);

			//List<string> BillerList = DeskTopTest.GetBillers("AT", customerSession.CustomerSessionId);

			//Assert.AreEqual(100, BillerList.Count, BillerList[0]);
		}

		[Test]
		public void FrequentBillerTest()
		{
			Dictionary<string, string> _context = new Dictionary<string, string>();
			CustomerAuthentication customer = new CustomerAuthentication();
			customer.AlloyID = 1000000000001010;
			//string agentSessionId = "1000005342";
			//long customerSessionId = 1000000000000240;

            List<Product> billerList = null;//DeskTopTest.GetFrequentBillers(0, customer.AlloyID, mgiContext);
			Assert.AreNotEqual(billerList, null);
			Assert.Greater(billerList.Count, 0, "Frequent biller available for the alloyId");

			//Product biller = DeskTopTest.GetBiller(Convert.ToInt32(billerList[0].BillerCode));
			//Assert.AreNotEqual(biller, null, string.Format("Biller could not be retrieved for {0}", billerList[0].ProductName));
			//CustomerSession customerSession
			//		= DeskTopTest.InitiateCustomerSession(agentSessionId, customer, context);
			//Purse purse = DeskTopTest.GetPurse(customerSession.CustomerSessionId, customer.AlloyID, agentSessionId, _context);
			//BillPayment billPayment = new BillPayment();
			//var validatebillPayment = DeskTopTest.ValidateBillPayment(Convert.ToInt32(customerSessionId), billPayment, context);
			//BillPayValidation billPayment = DeskTopTest.ValidateBillPayment(biller.Id, biller.AccountAuth, biller.SecondaryAuth, customerSession.CustomerSessionId, customerSession.Customer, purse);

			//			Assert.AreNotEqual(billPayment.AccountName, "", "Account not found");
		}

		[Test]
		public void GetCustomerBillFeeTest()
		{
            //decimal retrievedFee = 0;

            //CustomerAuthentication customer = new CustomerAuthentication();

            //long agentSessionId = 1000002437;

            ////CustomerSession customerSession
            ////	  = DeskTopTest.InitiateCustomerSession(agentSessionId, customer, context);
            //string providerName = "WesternUnion";
            ////int productId = 125;
			//retrievedFee = DeskTopTest.GetBillPayFee(agentSessionId, providerName);
			//retrievedFee = DeskTopTest.GetCustomerBillFee(customerSession.CustomerSessionId, productId);
			//Assert.Greater(retrievedFee, 0);
		}

		[Test]
		public void CheckLimitTest()
		{
			// AgentSession Session = DeskTopTest.Authenticate("anil", "Anil@123", "centris", "Anil");

			CustomerAuthentication customer = new CustomerAuthentication();

			//customer.PAN = "1000000008519520";

			//    CustomerSession customerSession = DeskTopTest.InitiateCustomerSession(Session.SessionId, customer, context);

			int productId = 125;

			Bill bill = new Bill();

			bill.Id = Guid.NewGuid().ToString();
			bill.AcceptedFee = false;
			bill.Amount = 497.5m;
			bill.BillerId = productId;
			// bill.Fee = DeskTopTest.GetCustomerBillFee(customerSession.CustomerSessionId, productId);
			bill.BillTotal = bill.Amount + bill.Fee;
			bill.PrimaryAuth = "123";
			bill.SecondaryAuth = "234";
			bill.Status = "";

			// Purse purse = DeskTopTest.GetPurse(customer.PAN, customerSession.CustomerSessionId, Session.SessionId);
			//BillPayValidation billValidate = DeskTopTest.ValidateBillPayment(bill.BillerId.ToString(), bill.PrimaryAuth, bill.SecondaryAuth, customerSession.CustomerSessionId, customerSession.Customer, purse);
			// Assert.Greater(billValidate.MaximumLoadAmount, 0);
			bill.AcceptedFee = true;
			//ShoppingCart cart = DeskTopTest.ShoppingCart(customerSession.CustomerSessionId, Session.SessionId);
			// DeskTopTest.AddBill(cart.ShoppingCartId, bill, bill.AcceptedFee);

			bill.Id = Guid.NewGuid().ToString();
			bill.AcceptedFee = false;
			bill.Amount = 497.5m;
			bill.BillerId = productId;
			//bill.Fee = DeskTopTest.GetCustomerBillFee(customerSession.CustomerSessionId, productId);
			bill.BillTotal = bill.Amount + bill.Fee;
			bill.PrimaryAuth = "123";
			bill.SecondaryAuth = "234";
			bill.Status = "";

			// purse = DeskTopTest.GetPurse(customer.PAN, customerSession.CustomerSessionId, Session.SessionId);
			//billValidate = DeskTopTest.ValidateBillPayment(bill.BillerId.ToString(), bill.PrimaryAuth, bill.SecondaryAuth, customerSession.CustomerSessionId, customerSession.Customer, purse);
			// Assert.Greater(billValidate.MaximumLoadAmount, 0);
			bill.AcceptedFee = true;
			// cart = DeskTopTest.ShoppingCart(customerSession.CustomerSessionId, Session.SessionId);
			//DeskTopTest.AddBill(cart.ShoppingCartId, bill, bill.AcceptedFee);

			bill.Id = Guid.NewGuid().ToString();
			bill.AcceptedFee = false;
			bill.Amount = 497.5m;
			bill.BillerId = productId;
			// bill.Fee = DeskTopTest.GetCustomerBillFee(customerSession.CustomerSessionId, productId);
			bill.BillTotal = bill.Amount + bill.Fee;
			bill.PrimaryAuth = "123";
			bill.SecondaryAuth = "234";
			bill.Status = "";

			// purse = DeskTopTest.GetPurse(customer.PAN, customerSession.CustomerSessionId, Session.SessionId);
			//billValidate = DeskTopTest.ValidateBillPayment(bill.BillerId.ToString(), bill.PrimaryAuth, bill.SecondaryAuth, customerSession.CustomerSessionId, customerSession.Customer, purse);
			//Assert.AreEqual(billValidate.MaximumLoadAmount, 0);
		}
	}
}