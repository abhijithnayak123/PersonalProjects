using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using System.Runtime.Remoting.Contexts;

namespace MGI.Channel.DMS.Web.ServiceClient.Test
{
    [TestFixture]
    public class ShoppingCartClientTest
    {
        public Desktop DeskTop { get; set; }
        private ShoppingCart cart = null;
		int cardPresentedType = 0;

        [SetUp]
        public void Setup()
        {
            DeskTop = new Desktop();
        }

        [Test]
		public void GetShoppingCartTest()
        {
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "34", "TCF");
			//long PAN = 1000000000000010;
			//CustomerSession customerSession
			//		= DeskTop.InitiateCustomerSession(Session.SessionId, PAN, cardPresentedType);
			//cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
			//Assert.AreNotEqual(cart,null);
        }

        [Test]
        public void AddCheckTest()
        {
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "34", "TCF");
			//long PAN = 1000000000000010;
			//CustomerSession customerSession
			//		= DeskTop.InitiateCustomerSession(Session.SessionId, PAN, cardPresentedType);
			//ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId );

			//int originalCheckCount = cart.Checks.Count;
			//decimal originalCheckTotal = cart.CheckTotal;

			//Check check = DeskTop.GetCheckStatus(customerSession.CustomerSessionId, "64C8E371-140A-4F14-B5FC-E9012E687BEE");
			////cart.AddCheque(check);

			//cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
			//Assert.AreEqual(originalCheckCount + 1,cart.Checks.Count);
			//Assert.AreEqual(cart.CheckTotal, originalCheckTotal + check.Amount);
        }

        [Test]
        private void RemoveCheckTest()
        {
			//AgentSession Session = DeskTop.AuthenticateAgent("anil", "Anil@123", "centris", "Anil");
			//long PAN = 1000000008519520;
			//CustomerSession customerSession
			//		= DeskTop.InitiateCustomerSession(Session.SessionId, PAN, cardPresentedType);

			//ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
			//int originalCheckCount = cart.Checks.Count;
			//decimal originalCheckTotal = cart.CheckTotal;

			//Check check = DeskTop.GetCheckStatus(customerSession.CustomerSessionId, "64C8E371-140A-4F14-B5FC-E9012E687BEE");
			////cart.RemoveCheque(long.Parse(customerSession.CustomerSessionId),check);

			//cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
			//Assert.AreEqual(originalCheckCount - 1, cart.Checks.Count);
			//Assert.AreEqual(cart.CheckTotal, originalCheckTotal - check.Amount);
        }

        [Test]
        private void RemoveCartTest()
        {
			//AgentSession Session = DeskTop.AuthenticateAgent("anil", "Anil@123", "centris", "Anil");
			//long PAN = 1000000008519520;
			//CustomerSession customerSession
			//		= DeskTop.InitiateCustomerSession(Session.SessionId, PAN, cardPresentedType);

			//ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);

			//long originalCartId = cart.Id;
			//Assert.AreNotEqual(cart, null);

			////DeskTop.RemoveShoppingCart(cart.ShoppingCartId);

			//cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
			//Assert.AreNotEqual(cart.Id,originalCartId);
        }

        [Test]
        private void AddBillTest()
        {
			//AgentSession Session = DeskTop.AuthenticateAgent("anil", "Anil@123", "centris", "Anil");
			//long PAN = 1000000008519520;
			//CustomerSession customerSession
			//		= DeskTop.InitiateCustomerSession(Session.SessionId, PAN, cardPresentedType);

			//cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
			//int originalBillCount = cart.Bills.Count;
			//decimal originalBillTotal = cart.BillTotal;

			//List<Product> billers = DeskTop.GetFrequentBillers(PAN);
			//Product biller = DeskTop.GetBiller(34, billers[2].ProductName);
			//Bill bill = new Bill();
			//bill.BillerId = biller.Id;
			//bill.PrimaryAuth = "123";
			//bill.SecondaryAuth = "234";
			//bill.Amount = 100;
			//bill.AcceptedFee = false;
			//bill.Fee = DeskTop.GetCustomerBillFee(biller.Id);
			//bill.BillTotal = bill.Amount + bill.Fee;
			//bill.Id = Guid.NewGuid().ToString();
			//bill.Status = "";
			////cart.AddBill(bill);

			//cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
			//Assert.AreEqual(originalBillCount + 1, cart.Bills.Count);
			//Assert.AreEqual(cart.BillTotal, originalBillTotal + bill.Amount);
        }

        [Test]
        private void RemoveBillTest()
        {
		  //  AgentSession Session = DeskTop.AuthenticateAgent("anil", "Anil@123", "centris", "Anil");
		  //  long PAN = 1000000008519520;
		  //  CustomerSession customerSession
		  //		  = DeskTop.InitiateCustomerSession(Session.SessionId, PAN, cardPresentedType);

		  //  ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
		  //  int originalBillCount = cart.Bills.Count;
		  //  decimal originalBillTotal = cart.BillTotal;

		  //  Bill bill = cart.Bills[0];
		  ////  cart.RemoveBill(long.Parse(customerSession.CustomerSessionId),bill);

		  //  cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
		  //  Assert.AreEqual(originalBillCount - 1, cart.Bills.Count);
		  //  Assert.AreEqual(cart.BillTotal, originalBillTotal - bill.Amount);
        }

        [Test]
        public void TestCartAndItems()
        {
			//GetShoppingCartTest();
			//AddCheckTest();
			//AddBillTest();
			//RemoveCheckTest();
			//RemoveBillTest();
			//RemoveCartTest();
        }

        [Test]
        public void CheckLimitTest()
        {
	   //	 AgentSession Session = DeskTop.AuthenticateAgent("anil", "Anil@123", "centris", "Anil");
	   //	 long PAN = 1000000008519520;
	   //	 CustomerSession customerSession
	   //			 = DeskTop.InitiateCustomerSession(Session.SessionId, PAN, cardPresentedType);
	   //	 Context context = new Context();
	   // //    BillPayCustomer billPayCustomer = new BillPayCustomer();
	   //	 cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
	   //	 int originalBillCount = cart.Bills.Count;
	   //	 decimal originalBillTotal = cart.BillTotal;

	   //	 List<Product> billers = DeskTop.GetFrequentBillers(PAN);
	   //	 Product biller = DeskTop.GetBiller(34, billers[2].ProductName);
	   //	 Bill bill = new Bill();
	   //	 bill.BillerId = biller.Id;
	   //	 bill.PrimaryAuth = "123";
	   //	 bill.SecondaryAuth = "234";
	   //	 bill.Amount = 497.5m;
	   //	 bill.AcceptedFee = false;
	   //	 bill.Fee = DeskTop.GetCustomerBillFee(biller.Id);
	   //	 bill.BillTotal = bill.Amount + bill.Fee;
	   //	 bill.Id = Guid.NewGuid().ToString();
	   //	 bill.Status = "";
	   //	 //BillPayResult billValidate = DeskTop.BillPaymentValidate(bill.Amount,bill.Fee,bill.BillerId,billPayCustomer,bill.BillerZip ,context);
	   //   //  Assert.GreaterOrEqual(billValidate.MaximumLoadAmount, bill.Amount + bill.Fee);
	   //	 //cart.AddBill(bill);


	   //	 bill = new Bill();
	   //	 bill.BillerId = biller.Id;
	   //	 bill.PrimaryAuth = "123";
	   //	 bill.SecondaryAuth = "234";
	   //	 bill.Amount = 497.5m;
	   //	 bill.AcceptedFee = false;
	   //	 bill.Fee = DeskTop.GetCustomerBillFee(biller.Id);
	   //	 bill.BillTotal = bill.Amount + bill.Fee;
	   //	 bill.Id = Guid.NewGuid().ToString();
	   //	 bill.Status = "";
	   // //    billValidate = billValidate = DeskTop.BillPaymentValidate(bill.Amount, bill.Fee, bill.BillerId, billPayCustomer, bill.BillerZip, context);
	   ////     Assert.GreaterOrEqual(billValidate.MaximumLoadAmount, bill.Amount + bill.Fee);
	   // //    cart.AddBill(bill);

	   //	 bill = new Bill();
	   //	 bill.BillerId = biller.Id;
	   //	 bill.PrimaryAuth = "123";
	   //	 bill.SecondaryAuth = "234";
	   //	 bill.Amount = 497.5m;
	   //	 bill.AcceptedFee = false;
	   //	 bill.Fee = DeskTop.GetCustomerBillFee(biller.Id);
	   //	 bill.BillTotal = bill.Amount + bill.Fee;
	   //	 bill.Id = Guid.NewGuid().ToString();
	   //	 bill.Status = "";
         //   billValidate = billValidate = DeskTop.BillPaymentValidate(bill.Amount, bill.Fee, bill.BillerId, billPayCustomer, bill.BillerZip , context);
         //   Assert.AreEqual(billValidate.MaximumLoadAmount, 0);
        }
    }
}