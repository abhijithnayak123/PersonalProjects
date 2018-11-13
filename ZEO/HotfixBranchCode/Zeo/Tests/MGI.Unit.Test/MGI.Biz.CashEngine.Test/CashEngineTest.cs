using NUnit.Framework;
using CXEData = MGI.Core.CXE.Data;
using MGI.Common.Util;
using MGI.Biz.CashEngine.Contract;
using MGI.Unit.Test;
using Moq;
using MGI.Biz.CashEngine.Data;

namespace MGI.Biz.CashEngine.Test
{
    [TestFixture]
	public class CashEngineTest : BaseClass_Fixture
    {
		MGIContext context = new MGIContext();
		public ICashEngine CashEngine { private get; set; }

        [Test]
        public void Can_Make_CashIn_Trxn()
        {
			long returnId = CashEngine.CashIn(1000000000, 100, context);
			Assert.AreNotEqual(returnId, 0);
        }

        [Test]
        public void Can_Make_CashOut_Trxn()
        {
			long returnId = CashEngine.CashOut(1000000000, 100, context);
			Assert.AreNotEqual(returnId, 0);
        }

        [Test]
        public void Can_Commit_Cash_Trxn()
        {
			int returnId = CashEngine.Commit(1000000000, 1000000000, context);
			Assert.AreEqual(returnId, (int)CXEData.TransactionStates.Committed);
        }

		[Test]
		[ExpectedException(typeof(BizCashEngineException))]
		public void Can_Not_Make_CashIn_Trxn()
		{
			long cxeTxnId = CashEngine.CashIn(0, 100, context);
		}

		[Test]
		public void Can_Cancel_CashIn_Trxn()
		{
			CashEngine.Cancel(1000000000, 1000000000, context);

			CashService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<Core.CXE.Data.TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());

		}

		[Test] //AL-2729 for update cash-in transaction
		public void Can_Update_CashIn_Trxn()
		{
			//Update(long customerSessionId, long trxId, decimal amount, MGIContext mgiContext)
			long transactionId = CashEngine.Update(1000000000, 1000000000, 100, context);
			Assert.AreNotEqual(transactionId, 0);
			CashService.Verify(moq => moq.UpdateAmount(It.IsAny<long>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.AtLeastOnce());
		}
    }
}
