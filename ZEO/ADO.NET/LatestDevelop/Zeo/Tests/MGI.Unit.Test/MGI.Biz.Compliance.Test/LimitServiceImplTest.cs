using NUnit.Framework;
using MGI.Biz.Compliance.Data;
using MGI.Common.Util;
using MGI.Biz.Compliance.Contract;

namespace MGI.Unit.Test
{
	[TestFixture]
	public class LimitServiceImplTest : BaseClass_Fixture
	{
		public ILimitService BIZLimitService { get; set; }

		[Test]
        public void CalculateTransactionMaximumLimitTest()
		{
			MGIContext context = new MGIContext();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 1;
			context.ShouldIncludeShoppingCartItems = true;

			decimal amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.Check, context);
			Assert.AreNotEqual(amount, 0);

			amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.ActivateGPR, context);
			Assert.AreNotEqual(amount, 0);

			amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.BillPay, context);
			Assert.AreNotEqual(amount, 0);

			amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.CashWithdrawal, context);
			Assert.AreNotEqual(amount, 0);

			amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.DebitGPR, context);
			Assert.AreNotEqual(amount, 0);

			amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.Funds, context);
			Assert.AreNotEqual(amount, 0);

			amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.LoadToGPR, context);
			Assert.AreNotEqual(amount, 0);

			amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.MoneyOrder, context);
			Assert.AreNotEqual(amount, 0);

			amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.MoneyTransfer, context);
			Assert.AreNotEqual(amount, 0);

			amount = BIZLimitService.CalculateTransactionMaximumLimit(1000000000, "TCFCompliance", TransactionTypes.Cash, context);
			Assert.AreNotEqual(amount, 0);
		}

        [Test]
        public void GetProductMinimumTest()
        {
            MGIContext context = new MGIContext();
            context.ProcessorId = 13;
            context.AgentId = 500021;
            context.TimeZone = "Eastern Standard Time";
            context.WUCounterId = "990000402";
            context.LocationName = "TCF Service Desk";
            context.ChannelPartnerId = 1;
            context.CheckUserName = "test";
			Assert.AreEqual(10, BIZLimitService.GetProductMinimum("test", TransactionTypes.Check, context));
        }

		[Test]
		[ExpectedException(typeof(BizComplianceException))]
		public void Can_Get_Product_Minimum_With_Wrong_Compliance_Name()
		{
			MGIContext context = new MGIContext();
			BIZLimitService.GetProductMinimum("TestCompliance", TransactionTypes.Check, context);
		}
	}
}
