using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;


namespace MGI.Channel.DMS.Web.ServiceClient.Test
{

	[TestFixture]
	class MoneyOrderTest
	{
		public Desktop DeskTop { get; set; }
		//AgentSession Session;
		CustomerSession customerSession;
		//long alloyId = 1000000000003880;
		long alloyId = 1000000000000010;
		MGIContext mgiContext = new MGIContext();
		MoneyOrderData moneyOrderData = new MoneyOrderData()
		{

			Amount = 500m,
			IsSystemApplied = false
		};
		[SetUp]
		public void Setup()
		{
			DeskTop = new Desktop();
            var agentsso = new AgentSSO();
            agentsso.UserName = "testagent";
            agentsso.Role = new UserRole();
            agentsso.Role.Id = 1;
            AgentSession session = DeskTop.AuthenticateSSO(agentsso, "28", "Carver", mgiContext);
            customerSession = DeskTop.InitiateCustomerSession(session.SessionId, alloyId, 3, mgiContext);
	
		}
		[Test]
		public void GetMoneyOrderFeeTest()
		{
			TransactionFee fee = DeskTop.GetMoneyOrderFee(long.Parse(customerSession.CustomerSessionId), moneyOrderData, mgiContext);
			Assert.AreEqual(5.0m, fee.NetFee);

		}
		[Test]
		public void IssMinimumMoneyOrderAmountTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 9.5m,
				Fee = 2m
			};
			Exception ex = Assert.Throws<Exception>(() => DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext));
			Assert.AreEqual("MGiAlloy|1008.6010|MoneyOrder Amount Less Than Minimum Amount|MoneyOrder Amount Less Than Minimum Amount", ex.Message);
		}
		[Test]
		public void IssMaximumMoneyOrderAmountTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 1000.1m,
				Fee = 2m
			};
			Exception ex = Assert.Throws<Exception>(() => DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext));
			Assert.AreEqual("MGiAlloy|1008.6005|Exceeded MGiAlloy Limit Check|The customer has reached their Money Order limit.", ex.Message);
		}
		[Test]
		public void PurchaseMoneyOrderTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 12.01m,
				Fee = 2m
			};
			MoneyOrder moneyOrder = DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);
			Assert.AreEqual(moneyOrder.Amount, moneyOrderPurchase.Amount);
			//Assert.AreEqual(moneyOrder.Fee, moneyOrderPurchase.Fee);
			Assert.AreEqual(moneyOrder.Status, "2");

			ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);

			bool PatnetMOPresent = cart.MoneyOrders.Where(x => x.Id == moneyOrder.Id
				&& x.Status == moneyOrder.Status
				&& x.Amount == moneyOrder.Amount
				&& x.Fee == moneyOrder.Fee).Any();
			Assert.AreEqual(false, PatnetMOPresent);
		}
		[Test]
		public void UpdateMoneyOrderStatusTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 12.02m,
				Fee = 2m
			};

			MoneyOrder moneyOrder = DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);

			int newMoneyOrderStatus = 9;//Proccessing State
			DeskTop.UpdateMoneyOrderStatus(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), newMoneyOrderStatus, mgiContext);

			MoneyOrder moneyOrderStage = DeskTop.GetMoneyOrderStage(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), mgiContext);
			bool updatedMOStagePresent = (moneyOrderStage.Status == newMoneyOrderStatus.ToString()) ? true : false;
			Assert.AreEqual(true, updatedMOStagePresent);

			ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);

			bool updatedMOPresent = cart.MoneyOrders.Where(x => x.Id == moneyOrder.Id && x.Status == newMoneyOrderStatus.ToString()).Any();
			Assert.AreEqual(true, updatedMOPresent);
		}
		[Test]
		public void AddMoneyOrderCheckNumberTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 12.03m,
				Fee = 2m,
			};
			MoneyOrder moneyOrder = DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);

			string moneyOrderCheckNumber = "0987654321";
			//	 DeskTop.AddMoneyOrderCheckNumber(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), moneyOrderCheckNumber);
			MoneyOrderTransaction moneyOrderTrans = new MoneyOrderTransaction()
			{
				AccountNumber = moneyOrder.AccountNumber,
				CheckNumber = "0987654321",
				BaseFee = moneyOrder.BaseFee,
				Fee = moneyOrder.Fee,
				RoutingNumber = moneyOrder.RoutingNumber
			};
			DeskTop.UpdateMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderTrans, long.Parse(moneyOrder.Id), mgiContext);
			MoneyOrder moneyOrderStage = DeskTop.GetMoneyOrderStage(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), mgiContext);

			bool updatedMOStagePresent = (moneyOrderStage.CheckNumber == moneyOrderCheckNumber) ? true : false;
			Assert.AreEqual(true, updatedMOStagePresent);
			//Assert.DoesNotThrow(() => DeskTop.CommitMoneyOrder(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id)));
		}
		[Test]
		public void UpdateMoneyOrderCheckNumberTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 12.04m,
				Fee = 2m
			};

			MoneyOrder moneyOrder = DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);

			string newMoneyOrderCheckNumber = "0987654321";
			//  DeskTop.UpdateMoneyOrderCheckNumber(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), newMoneyOrderCheckNumber);
			MoneyOrderTransaction moneyOrderTrans = new MoneyOrderTransaction()
			{
				AccountNumber = moneyOrder.AccountNumber,
				CheckNumber = "0987654321",
				BaseFee = moneyOrder.BaseFee,
				Fee = moneyOrder.Fee,
				RoutingNumber = moneyOrder.RoutingNumber
			};
			DeskTop.UpdateMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderTrans, long.Parse(moneyOrder.Id), mgiContext);
			MoneyOrder moneyOrderStage = DeskTop.GetMoneyOrderStage(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), mgiContext);

			bool updatedMOStagePresent = (moneyOrderStage.CheckNumber == newMoneyOrderCheckNumber) ? true : false;
			Assert.AreEqual(true, updatedMOStagePresent);
			//Assert.DoesNotThrow(() => DeskTop.CommitMoneyOrder(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id)));
		}
		[Test]
		public void GenerateCheckPrintForMoneyOrderTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 12.05m,
				Fee = 2m
			};

			MoneyOrder moneyOrder = DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);

			//string MoneyOrderCheckNumber = "1234567890";
			// DeskTop.AddMoneyOrderCheckNumber(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), MoneyOrderCheckNumber);
			MoneyOrderTransaction moneyOrderTrans = new MoneyOrderTransaction()
			{
				AccountNumber = moneyOrder.AccountNumber,
				CheckNumber = moneyOrder.CheckNumber,
				BaseFee = moneyOrder.BaseFee,
				Fee = moneyOrder.Fee,
				RoutingNumber = moneyOrder.RoutingNumber
			};
			DeskTop.UpdateMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderTrans, long.Parse(moneyOrder.Id), mgiContext);

			CheckPrint checkPrint = DeskTop.GenerateCheckPrintForMoneyOrder(long.Parse(moneyOrder.Id), long.Parse(customerSession.CustomerSessionId), mgiContext);

			Assert.IsTrue(checkPrint.Lines.Count() > 0);
		}
		[Test]
		public void CommitMoneyOrderTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 12.06m,
				Fee = 2m
			};

			MoneyOrder moneyOrder = DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);

			//string moneyOrderCheckNumber = "1234567890";
			//  DeskTop.AddMoneyOrderCheckNumber(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), moneyOrderCheckNumber);

			//DeskTop.CommitMoneyOrder(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id));

			ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);
			string moneyOrderCommitStatus = "4";

			bool updatedMOPresent = cart.MoneyOrders.Where(x => x.Id == moneyOrder.Id
				&& x.Status == moneyOrderCommitStatus
				&& x.Amount == moneyOrderPurchase.Amount
				&& x.Fee == moneyOrderPurchase.Fee).Any();
			Assert.AreEqual(false, updatedMOPresent);
		}
		[Test]
		public void GetMoneyOrderTransactionTest()
		{

			long agentSessionId = 1000002070;
			DeskTop = new Desktop();
			//Session = DeskTop.AuthenticateAgent("sysadmin", "Password@123", "28", "Carver");
			customerSession = DeskTop.InitiateCustomerSession(agentSessionId.ToString(), alloyId, 3, mgiContext);
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 12.07m,
				Fee = 5m
			};

			MoneyOrder moneyOrder = DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);

			//string moneyOrderCheckNumber = "1234567890";
			MoneyOrderTransaction moneyOrderTrans = new MoneyOrderTransaction()
			{
				AccountNumber = moneyOrder.AccountNumber,
				CheckNumber = "1234567890",
				BaseFee = moneyOrder.BaseFee,
				Fee = moneyOrder.Fee,
				RoutingNumber = moneyOrder.RoutingNumber
			};
			DeskTop.UpdateMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderTrans, long.Parse(moneyOrder.Id), mgiContext);
			//  DeskTop.AddMoneyOrderCheckNumber(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), moneyOrderCheckNumber);

			// DeskTop.CommitMoneyOrder(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id));

			MoneyOrderTransaction moneyOrderTransaction = DeskTop.GetMoneyOrderTransaction(agentSessionId, long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), mgiContext);

			//Assert.IsTrue(moneyOrderTransaction.Amount == moneyOrderPurchase.Amount &&
			//	moneyOrderTransaction.Fee == moneyOrderPurchase.Fee &&
			//	moneyOrderTransaction.CheckNumber == moneyOrderCheckNumber);
			Assert.IsTrue(moneyOrderTransaction.Amount == moneyOrderPurchase.Amount &&
				moneyOrderTransaction.Fee == moneyOrderPurchase.Fee);
		}
		[Test]
		public void GetMoneyOrderReceiptTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 12.08m,
				Fee = 2m
			};

			//long agentSessionId = 0;
			///bool isReprint;
			MoneyOrder moneyOrder = DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);

			//string moneyOrderCheckNumber = "1234567890";
			//  DeskTop.AddMoneyOrderCheckNumber(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), moneyOrderCheckNumber);

			//            DeskTop.CommitMoneyOrder(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id));
			//public List<ReceiptData> GetMoneyOrderReceipt(, long transactionId, bool isReprint)

			//Receipt receipt =  DeskTop.GetMoneyOrderReceipt(agentSessionId,long.Parse(moneyOrder.Id), isReprint);

			//Assert.IsTrue(receipt.Lines.Count() > 0);
		}

		[Test]
		public void GetMoneyOrderStageTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 12.09m,
				Fee = 2m
			};

			MoneyOrder moneyOrder = DeskTop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);

			string moneyOrderCheckNumber = "1234567890";
			//  DeskTop.AddMoneyOrderCheckNumber(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), moneyOrderCheckNumber);
			MoneyOrderTransaction moneyOrderTrans = new MoneyOrderTransaction()
			{
				AccountNumber = moneyOrder.AccountNumber,
				CheckNumber = "1234567890",
				BaseFee = moneyOrder.BaseFee,
				Fee = moneyOrder.Fee,
				RoutingNumber = moneyOrder.RoutingNumber
			};
			DeskTop.UpdateMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderTrans, long.Parse(moneyOrder.Id), mgiContext);


			MoneyOrder moneyOrderStage = DeskTop.GetMoneyOrderStage(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), mgiContext);

			bool updatedMOStagePresent = (moneyOrderStage.CheckNumber == moneyOrderCheckNumber
				&& moneyOrderStage.Amount == moneyOrderPurchase.Amount
				&& moneyOrderStage.Fee == moneyOrderPurchase.Fee
				&& moneyOrderStage.Status == "2") ? true : false;
			Assert.AreEqual(true, updatedMOStagePresent);
		}
	}
}
