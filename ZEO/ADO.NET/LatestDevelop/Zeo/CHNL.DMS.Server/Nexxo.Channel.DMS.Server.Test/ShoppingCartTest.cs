using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Contract;
using Spring.Context;

using MGI.Channel.Shared.Server.Data;
using MGI.Channel.DMS.Server.Data;
using Spring.Testing.NUnit;


namespace MGI.Channel.DMS.Server.Test
{
	[TestFixture]
	public class ShoppingCartTest : AbstractTransactionalSpringContextTests
	{

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }

		}
		IDesktopService DeskTopTest { get; set; }
		private static string DESKTOP_ENGINE = "DesktopEngine";
        public MGIContext mgiContext { get; set; }

		long customerSessionId = 1000002065;
		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);

			mgiContext.ChannelPartnerId = 34;
			//mgiContext.Language ="EN";
		}

		//[Test]
		//public void GetShoppingCartItemsTest()
		//{
		//	AgentSession agentSession = null; //DeskTopTest.Authenticate( "manoj", "Anil@123", "centris", "Anil" );

		//	CustomerAuthentication customer = new CustomerAuthentication();

		//	//	customer.AlloyID = "1000000008582551";

		//	CustomerSession customerSession = DeskTopTest.InitiateCustomerSession(agentSession.SessionId, customer, _context);
		//	ShoppingCart shoppingCart = new ShoppingCart();
		//	shoppingCart = DeskTopTest.ShoppingCart(Convert.ToInt32(customerSession.CustomerSessionId));
		//	//shoppingCart = DeskTopTest.ShoppingCart(customerSession.CustomerSessionId, agentSession.SessionId, long.Parse(customer.AlloyID));
		//	CartId = Convert.ToString(shoppingCart.Id);
		//	//CartId = shoppingCart.ShoppingCartId;
		//	Assert.AreEqual("1000000008582551", shoppingCart.AlloyID.ToString());
		//}

		[Test]
		public void ShoppingCartAddCheckTest()
		{
			//DeskTopTest.AddCheque(CartId, "458BA478-A252-48B0-B063-FFDA73E6472E", true);

			long checkId = 0;
            DeskTopTest.AddCheck(customerSessionId, checkId, mgiContext);
			Assert.Pass("Check added");
		}

		[Test]
		public void ShoppingCartAddBillTest()
		{
			//Data.Bill bill = new Data.Bill { BillerId = 125, Id = "458BA478-A252-48B0-B063-FFDA73E6472E", Amount = 25, Fee = 5, PrimaryAuth = "1234", SecondaryAuth = "4321", Status = "validated" };
			long billPayId = 0;
            DeskTopTest.AddBillPay(customerSessionId, billPayId, mgiContext);// AddBill(CartId, bill, true);
			Assert.Pass("Bill added");
		}

		[Test]
		public void ShoppingCartAddMoneyTransferTest()
		{
			//Data.MoneyTransfer mt = new Data.MoneyTransfer { ReceiverId = new Guid("4589fbbd-9e2c-45fb-9823-69f2cd3ecbb1"), Id = "10000000596", Amount = 75, Fee = 15, Status = "validated" };
			long moneyTransferId = 0;
            DeskTopTest.AddMoneyTransfer(customerSessionId, moneyTransferId, mgiContext); // CartId, mt);
			Assert.Pass("Money transfer added");
		}

		[Test]
		public void ShoppingCartAddGprActivateTest()
		{
			//Data.GprCard gpc = new Data.GprCard { ActivationFee = 10, CardNumber = 4444, LoadAmount = 0, LoadFee = 0, WithdrawAmount = 0, WithdrawFee = 0 };

			//DeskTopTest.AddGprCard(CartId, gpc);
			//Assert.Pass("GPR card added");
		}

		[Test]
		public void ShoppingCartAddGprLoadTest()
		{
			//Data.GprCard gpc = new Data.GprCard { ActivationFee = 0, CardNumber = 4444, LoadAmount = 100, LoadFee = 10, WithdrawAmount = 0, WithdrawFee = 0 };

			//DeskTopTest.AddGprCard(CartId, gpc);
			//Assert.Pass("GPR card added");
		}

		[Test]
		public void ShoppingCartAddGprWithdrawTest()
		{
			//Data.GprCard gpc = new Data.GprCard { ActivationFee = 0, CardNumber = 4444, LoadAmount = 0, LoadFee = 0, WithdrawAmount = 80, WithdrawFee = 5 };

			//DeskTopTest.AddGprCard(CartId, gpc);
			//Assert.Pass("GPR card added");
		}

		[Test]
		public void ShoppingCartRemoveGprTest()
		{
			//DeskTopTest.RemoveGprCard("93A546CD-7AF5-4B7E-A8B4-D502CED7D459");
			//DeskTopTest.RemoveGprCard("04CC5BBD-CD61-4349-899E-F8D506C9206F");
			//DeskTopTest.RemoveGprCard("42EB5390-8281-4601-AB3C-EE73100EC3A2");
			//Assert.Pass("GPR card added");
		}
	}
}
