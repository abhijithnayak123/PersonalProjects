using System;
using System.Collections.Generic;
using NHibernate.Engine;
using MGI.Biz.MoneyOrderEngine.Contract;
using MGI.Biz.MoneyOrderEngine.Data;
using MGI.Biz.MoneyOrderEngine.Impl;
using Spring.Testing.NUnit;
using CXEContract = MGI.Core.CXE.Contract;
using CXEData = MGI.Core.CXE.Data;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;
using CustomerSessionSvc = MGI.Core.Partner.Contract.ICustomerSessionService;
using CoreCustomerSession = MGI.Core.Partner.Data.CustomerSession;
using NHibernate;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using MGI.Core.CXE.Contract;
using MoneyOrderCommit = MGI.Core.CXE.Data.Transactions.Commit.MoneyOrder;
using MoneyOrderStage = MGI.Core.CXE.Data.Transactions.Stage.MoneyOrder;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Common.Util;

namespace MGI.Biz.MoneyOrderEngine.Test
{
	[TestFixture]
	public class MoneyOrderEngineTest : AbstractTransactionalSpringContextTests
	{
		// public IMoneyOrderEngineService MoneyOrderEngineServiceTest { get; set; }        
		public IMoneyOrderEngineService BIZMoneyOrderEngine { get; set; }
		public CustomerSessionSvc CustomerSessionSvc { get; set; }
		public IMoneyOrderService MoneyOrderServiceTest { get; set; }
		public PTNRContract.ITransactionService<PTNRData.Transactions.MoneyOrder> PartnerMoneyOrderService { private get; set; }

		private CXEContract.IMoneyOrderService _cxeMoneyOrderSvc;
		public CXEContract.IMoneyOrderService CxeMoneyOrderSvc
		{
			get { return _cxeMoneyOrderSvc; }
			set { _cxeMoneyOrderSvc = value; }
		}

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Biz.MoneyOrderEngine.Test/MGI.Biz.MoneyOrderEngine.Test/Biz.MoneyOrderEngine.Test.xml" }; }
		}

		//private int var = 0;
		MGIContext context;
		long sessionId = 1000004571;
		CoreCustomerSession customerSession = new CoreCustomerSession()
		{

			Id = 1000004571
		};
		private decimal amount = 100m;
		//private string promotionCode = "123456789";
		bool isSystemApplied = false;
		MGI.Biz.Common.Data.TransactionFee transFee;


		[Test]
		public void CanGetMoneyOrderFee()
		{
			MoneyOrderData mOrderData = new MoneyOrderData()
			{
				Amount = amount,
				IsSystemApplied = isSystemApplied,
			};
			context = new MGIContext();
			context.Amount = mOrderData.Amount;
			context.IsSystemApplied = mOrderData.IsSystemApplied;
			transFee = BIZMoneyOrderEngine.GetMoneyOrderFee(sessionId, mOrderData, context);
			Assert.IsNotNull(transFee);
		}

		[Test]
		public void AddMoneyOrder()
		{
			decimal amount = 100m;
			decimal fee = 5m;
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase()
			{
				Amount = amount,
				Fee = fee,
				IsSystemApplied = isSystemApplied,

			};

			context = new MGIContext();
			context.Amount = moneyOrderPurchase.Amount;
			context.Fee = moneyOrderPurchase.Fee;
			context.TimeZone = "Eastern Standard Time";
			MoneyOrder moneyOrder = BIZMoneyOrderEngine.Add(sessionId, moneyOrderPurchase, context);
			Assert.AreEqual(moneyOrderPurchase.Amount, moneyOrder.Amount);
			Assert.AreEqual(moneyOrderPurchase.Fee, moneyOrder.Fee);
		}
		[Test]
		public void IsMinimumMoneyOrderAmountTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 9.9m,
				Fee = 2m
			};
			context = new MGIContext();
			context.Amount = moneyOrderPurchase.Amount;
			context.Fee = moneyOrderPurchase.Fee;
			context.TimeZone = "Eastern Standard Time";
			BizComplianceLimitException ex = Assert.Throws<BizComplianceLimitException>(() => BIZMoneyOrderEngine.Add(sessionId, moneyOrderPurchase, context));
			Assert.AreEqual(1008, ex.MajorCode);
			Assert.AreEqual(6010, ex.MinorCode);
		}
		[Test]
		public void IsMaximumMoneyOrderAmountTest()
		{
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
			{
				Amount = 1000.1m,
				Fee = 2m
			};
			context = new MGIContext();
			context.Amount = moneyOrderPurchase.Amount;
			context.Fee = moneyOrderPurchase.Fee;
			context.TimeZone = "Eastern Standard Time";
			BizComplianceLimitException ex = Assert.Throws<BizComplianceLimitException>(() => BIZMoneyOrderEngine.Add(sessionId, moneyOrderPurchase, context));
			Assert.AreEqual(1008, ex.MajorCode);
			Assert.AreEqual(6005, ex.MinorCode);
		}


		[Test]
		public void Should_MoneyOrder_Limits_Pass()
		{
			decimal amount = 1000M;
			decimal fee = 1M;
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase()
			{
				Amount = amount,
				Fee = fee,
				IsSystemApplied = isSystemApplied,

			};

			context = new MGIContext();
			context.Amount = moneyOrderPurchase.Amount;
			context.Fee = moneyOrderPurchase.Fee;
			context.TimeZone = "Eastern Standard Time";
			try
			{
				MoneyOrder moneyOrder = BIZMoneyOrderEngine.Add(sessionId, moneyOrderPurchase, context);

				Assert.AreEqual(moneyOrderPurchase.Amount, moneyOrder.Amount);
				Assert.Pass("Limits Test Passed");
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}

		}


		[Test]
		public void Should_MoneyOrder_Limits_Fail()
		{
			decimal amount = 5000M;
			decimal fee = 1M;
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase()
			{
				Amount = amount,
				Fee = fee,
				IsSystemApplied = isSystemApplied,

			};

			context = new MGIContext();
			context.Amount = moneyOrderPurchase.Amount;
			context.Fee = moneyOrderPurchase.Fee;
			context.TimeZone = "Eastern Standard Time";
			try
			{
				MoneyOrder moneyOrder = BIZMoneyOrderEngine.Add(sessionId, moneyOrderPurchase, context);

				Assert.AreEqual(moneyOrderPurchase.Amount, moneyOrder.Amount);
				Assert.Pass("Limits Test Passed");
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}

		}
		//[Test]
		//public void UpdateMoneyOrderStatusTest()
		//{
		//	MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
		//	{
		//		Amount = 100m,
		//		Fee = 1m,
		//		IsSystemApplied = isSystemApplied,

		//	};
		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add("Amnt", moneyOrderPurchase.Amount);
		//	context.Add("FeeCharge", moneyOrderPurchase.Fee);
		//	context.Add("TimeZone", "Eastern Standard Time");
		//	MoneyOrder moneyOrder = BIZMoneyOrderEngine.Add(sessionId, moneyOrderPurchase, context);

		//	//  SetComplete();
		//	EndTransaction();
		//	StartNewTransaction();
		//	//customerSession.Id = 1000004571;
		//	int newMoneyOrderStatus = 9;//Proccessing State
		//	BIZMoneyOrderEngine.UpdateMoneyOrderStatus(customerSession.Id, long.Parse(moneyOrder.Id), newMoneyOrderStatus, context);

		//	//  SetComplete();
		//	EndTransaction();
		//	StartNewTransaction();

		//	PTNRData.Transactions.MoneyOrder moneyOrderTrx = PartnerMoneyOrderService.Lookup(long.Parse(moneyOrder.Id));

		//	Assert.AreEqual(moneyOrderPurchase.Amount, moneyOrderTrx.Amount);
		//	Assert.AreEqual(moneyOrderPurchase.Fee, moneyOrderTrx.Fee);
		//	Assert.AreEqual(9, moneyOrderTrx.CXEState);
		//}
		//[Test]
		//public void AddMoneyOrderCheckNumberTest()
		//{
		//    MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
		//    {
		//        Amount = 12.03m,
		//        Fee = 2m
		//    };
		//    Dictionary<string, object> context = new Dictionary<string, object>();
		//    context.Add("Amntt", moneyOrderPurchase.Amount);
		//    context.Add("FeeChargee", moneyOrderPurchase.Fee);

		//    MoneyOrder moneyOrder = BIZMoneyOrderEngine.Add(customerSession.Id, moneyOrderPurchase, context);


		//    EndTransaction();
		//    StartNewTransaction();

		//    string moneyOrderCheckNumber = "1234567890";
		//    BIZMoneyOrderEngine.AddMoneyOrderCheckNumber(customerSession.Id, long.Parse(moneyOrder.Id), moneyOrderCheckNumber, context);

		//    SetComplete();
		//    EndTransaction();
		//    StartNewTransaction();

		//    MoneyOrderStage moneyOrderStage = CxeMoneyOrderSvc.GetStage(long.Parse(moneyOrder.Id));
		//    Assert.IsTrue(moneyOrderPurchase.Amount == moneyOrderStage.Amount && moneyOrderPurchase.Fee == moneyOrderStage.Fee && moneyOrderCheckNumber == moneyOrderStage.MoneyOrderCheckNumber);
		//}
		//[Test]
		//public void UpdateMoneyOrderCheckNumberTest()
		//{
		//    MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
		//    {
		//        Amount = 12.03m,
		//        Fee = 2m
		//    };
		//    MoneyOrder moneyOrder = MoneyOrderEngineServiceTest.Add(customerSession.Id, moneyOrderPurchase, context);

		//    SetComplete();
		//    EndTransaction();
		//    StartNewTransaction();

		//    string newMoneyOrderCheckNumber = "1234567891";
		//    MoneyOrderEngineServiceTest.UpdateMoneyOrderCheckNumber(customerSession.Id, long.Parse(moneyOrder.Id), newMoneyOrderCheckNumber, context);

		//    SetComplete();
		//    EndTransaction();
		//    StartNewTransaction();

		//    MoneyOrderStage moneyOrderStage = CxeMoneyOrderSvc.GetStage(long.Parse(moneyOrder.Id));
		//    Assert.IsTrue(moneyOrderPurchase.Amount == moneyOrderStage.Amount && moneyOrderPurchase.Fee == moneyOrderStage.Fee && newMoneyOrderCheckNumber == moneyOrderStage.MoneyOrderCheckNumber);


		//}
		//  [Test]
		//public void CommitMoneyOrderTest()
		//{
		//    MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
		//    {
		//        Amount = 12.06m,s
		//        Fee = 2m
		//    };

		//    MoneyOrder moneyOrder = MoneyOrderEngineServiceTest.Add(customerSession.Id, moneyOrderPurchase, context);

		//    SetComplete();
		//    EndTransaction();
		//    StartNewTransaction();

		//    string moneyOrderCheckNumber = "1234567890";
		//    MoneyOrderEngineServiceTest.AddMoneyOrderCheckNumber(customerSession.Id, long.Parse(moneyOrder.Id), moneyOrderCheckNumber, context);

		//    SetComplete();
		//    EndTransaction();
		//    StartNewTransaction();

		//    MoneyOrderEngineServiceTest.Commit(customerSession.Id, long.Parse(moneyOrder.Id), context);

		//    SetComplete();
		//    EndTransaction();
		//    StartNewTransaction();

		//    int moneyOrderCommitStatus = 4;

		//    PTNRData.Transactions.MoneyOrder moneyOrderTrx = PartnerMoneyOrderService.Lookup(long.Parse(moneyOrder.Id));
		//    Assert.IsTrue(moneyOrderPurchase.Amount == moneyOrderTrx.Amount 
		//        && moneyOrderPurchase.Fee == moneyOrderTrx.Fee 
		//        && moneyOrderCommitStatus == moneyOrderTrx.CXEState);

		//    MoneyOrderCommit moneyOrderCommit = MoneyOrderServiceTest.Get(long.Parse(moneyOrder.Id));

		//    Assert.IsTrue(moneyOrderPurchase.Amount == moneyOrderCommit.Amount 
		//        && moneyOrderPurchase.Fee == moneyOrderCommit.Fee 
		//        && moneyOrderCheckNumber == moneyOrderCommit.MoneyOrderCheckNumber 
		//        && moneyOrderCommitStatus == moneyOrderCommit.Status);
		//}
	}
}
