using BizMOData = MGI.Biz.MoneyOrderEngine.Data;
using CXEData = MGI.Core.CXE.Data;
using NUnit.Framework;
using MGI.Common.Util;
using MGI.Biz.MoneyOrderEngine.Contract;
using MGI.Unit.Test;
using Moq;
using MGI.Biz.Compliance.Contract;

namespace MGI.Biz.MoneyOrder.Test
{
	[TestFixture]
	public class MoneyOrderEngineServiceImplTest : BaseClass_Fixture
	{
        public IMoneyOrderEngineService MoneyOrderEngineService { get; set; }

		[Test]
		public void Can_Get_Money_Order_Fee()
		{
			long customerSessionId = 1000000002;
			BizMOData.MoneyOrderData mOrderData = new BizMOData.MoneyOrderData()
			{
				Amount = 100,
				IsSystemApplied = true,
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.Amount = mOrderData.Amount;
			mgiContext.IsSystemApplied = mOrderData.IsSystemApplied;

			Common.Data.TransactionFee transactionFee = MoneyOrderEngineService.GetMoneyOrderFee(customerSessionId, mOrderData, mgiContext);

			Assert.IsNotNull(transactionFee);
		}

		[Test]
		public void Can_Add_MoneyOrder_Trxn()
		{
			long customerSessionId = 1000000103;
			BizMOData.MoneyOrderPurchase moneyOrderPurchase = new BizMOData.MoneyOrderPurchase()
			{
				Amount = 100,
				IsSystemApplied = true,
				Fee = 1,
				PromotionCode = string.Empty
			};
			MGIContext mgiContext = new MGIContext();

			BizMOData.MoneyOrder moneyOrder = MoneyOrderEngineService.Add(customerSessionId, moneyOrderPurchase, mgiContext);

			Assert.IsNotNull(moneyOrder);
            CxeMoneyOrderSvc.Verify(moq => moq.Create(It.IsAny<CXEData.Transactions.Stage.MoneyOrder>(), It.IsAny<string>()), Times.AtLeastOnce());
            CxeMoneyOrderSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Update_MoneyOrder_TrxnStatus()
		{
			long customerSessionId = 1000000002;
			long moId = 1000000000;
			MGIContext mgiContext = new MGIContext();
			MoneyOrderEngineService.UpdateMoneyOrderStatus(customerSessionId, moId, 1, mgiContext);
			CxeMoneyOrderSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Update_MoneyOrder_Trxn()
		{
			long customerSessionId = 1000000002;
			long moId = 1000000002;
			MGIContext mgiContext = new MGIContext();
			BizMOData.MoneyOrder moneyOrder = new BizMOData.MoneyOrder() { };

			MoneyOrderEngineService.UpdateMoneyOrder(customerSessionId, moneyOrder, moId, mgiContext);

			CxeMoneyOrderSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Commit_MoneyOrder_Trxn()
		{
			long customerSessionId = 1000000002;
			long moId = 1000000002;
			MGIContext mgiContext = new MGIContext();

			MoneyOrderEngineService.Commit(customerSessionId, moId, mgiContext);

			CxeMoneyOrderSvc.Verify(moq => moq.Commit(It.IsAny<long>()), Times.AtLeastOnce());
			CxeMoneyOrderSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_MoneyOrder_Check()
		{
			long customerSessionId = 1000000002;
            long moId = 1000000001;
			MGIContext mgiContext = new MGIContext();

			BizMOData.MoneyOrderCheckPrint moCheckPrint = MoneyOrderEngineService.GetMoneyOrderCheck(customerSessionId, moId, mgiContext);

			Assert.IsNotNull(moCheckPrint);
		}

		[Test]
		public void Can_Get_MoneyOrder_Diagnostics()
		{
			long agentSessionId = 1000000002;
			MGIContext mgiContext = new MGIContext();

			BizMOData.MoneyOrderCheckPrint moneyOrderCheckPrint = MoneyOrderEngineService.GetMoneyOrderDiagnostics(agentSessionId, mgiContext);

			Assert.IsNotNull(moneyOrderCheckPrint);
		}

		[Test]
		public void Can_Get_Stage_MoneyOrder()
		{
			long customerSessionId = 1000000002;
			long moId = 1000000002;
			MGIContext mgiContext = new MGIContext();

			BizMOData.MoneyOrder moneyOrder = MoneyOrderEngineService.GetMoneyOrderStage(customerSessionId, moId, mgiContext);

			Assert.IsNotNull(moneyOrder);
			CxeMoneyOrderSvc.Verify(moq => moq.GetStage(It.IsAny<long>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Resubmit_MoneyOrder()
		{
			long customerSessionId = 1000000002;
            long moId = 1000000001;
			MGIContext mgiContext = new MGIContext();

			bool can = MoneyOrderEngineService.Resubmit(customerSessionId, moId, mgiContext);

			Assert.True(can);
			CxeMoneyOrderSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Not_Update_Duplicate_MoneyOrder()
		{
			long customerSessionId = 1000000002;
			long moId = 1000000003;
			MGIContext mgiContext = new MGIContext();
			BizMOData.MoneyOrder moneyOrder = new BizMOData.MoneyOrder() { };
			
			MoneyOrderEngineService.UpdateMoneyOrder(customerSessionId, moneyOrder, moId, mgiContext);

			CxeMoneyOrderSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce());
		}
        [Test]
        [ExpectedException(typeof(BizComplianceLimitException))]
        public void Can_Validate_Compliance_Limit_Lower()
        {
            long customerSessionId = 1000000002;
            BizMOData.MoneyOrderPurchase moneyOrderPurchase = new BizMOData.MoneyOrderPurchase()
            {
                Amount = 0,
                IsSystemApplied = true,
                Fee = 1,
                PromotionCode = string.Empty
            };
            MGIContext mgiContext = new MGIContext();

            BizMOData.MoneyOrder moneyOrder = MoneyOrderEngineService.Add(customerSessionId, moneyOrderPurchase, mgiContext);
        }

        [Test]
        [ExpectedException(typeof(BizComplianceLimitException))]
        public void Can_Validate_Compliance_Limit_High()
        {
            long customerSessionId = 1000000002;
            BizMOData.MoneyOrderPurchase moneyOrderPurchase = new BizMOData.MoneyOrderPurchase()
            {
                Amount = 200,
                IsSystemApplied = true,
                Fee = 1,
                PromotionCode = string.Empty
            };
            MGIContext mgiContext = new MGIContext();

            BizMOData.MoneyOrder moneyOrder = MoneyOrderEngineService.Add(customerSessionId, moneyOrderPurchase, mgiContext);
        }

        [Test]
        public void Can__UpdateMoneyOrderImage_MoneyOrder()
        {
            long customerSessionId = 1000000002;
            long moId = 1000000003;
            MGIContext mgiContext = new MGIContext();
            BizMOData.MoneyOrder moneyOrder = new BizMOData.MoneyOrder() { };

            MoneyOrderEngineService.UpdateMoneyOrder(customerSessionId, moneyOrder, moId, mgiContext);

            CxeMoneyOrderSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Test]
        public void Can_Add_GetProviderId_MoneyOrder_Trxn()
        {
            long customerSessionId = 10000000016;

            BizMOData.MoneyOrderPurchase moneyOrderPurchase = new BizMOData.MoneyOrderPurchase()
            {
                Amount = 100,
                IsSystemApplied = true,
                Fee = 1,
                PromotionCode = string.Empty
            };
            MGIContext mgiContext = new MGIContext();

            BizMOData.MoneyOrder moneyOrder = MoneyOrderEngineService.Add(customerSessionId, moneyOrderPurchase, mgiContext);

            Assert.IsNotNull(moneyOrder);
            CxeMoneyOrderSvc.Verify(moq => moq.Create(It.IsAny<CXEData.Transactions.Stage.MoneyOrder>(), It.IsAny<string>()), Times.AtLeastOnce());
            CxeMoneyOrderSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Test]
        public void Can_Add_GetProviderIds_MoneyOrder_Trxn()
        {
			long customerSessionId = 1000000102;

            BizMOData.MoneyOrderPurchase moneyOrderPurchase = new BizMOData.MoneyOrderPurchase()
            {
                Amount = 100,
                IsSystemApplied = true,
                Fee = 1,
                PromotionCode = string.Empty
            };
            MGIContext mgiContext = new MGIContext();

            BizMOData.MoneyOrder moneyOrder = MoneyOrderEngineService.Add(customerSessionId, moneyOrderPurchase, mgiContext);

            Assert.IsNotNull(moneyOrder);
            CxeMoneyOrderSvc.Verify(moq => moq.Create(It.IsAny<CXEData.Transactions.Stage.MoneyOrder>(), It.IsAny<string>()), Times.AtLeastOnce());
            CxeMoneyOrderSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()), Times.AtLeastOnce());
        }
        
	}
}
