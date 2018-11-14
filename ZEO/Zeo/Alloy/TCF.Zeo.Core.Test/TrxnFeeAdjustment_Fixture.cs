using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class TrxnFeeAdjustment_Fixture
    {
        ITrxnFeeAdjustmentService trxnFeeAdjustment = new TrxnFeeAdjustmentService();
        ZeoContext alloycontext = new ZeoContext();
        [TestCase]
        public void Can_Create_TrxnFeeAdj()
        {
            TransactionFeeAdjustment trxnFeeAdj = new TransactionFeeAdjustment()
            {
                TransactionId = 1000000000,
                IsActive = true,
                //FeeAdjustmentId = 10000000,
                DTTerminalCreate = DateTime.Today,
                DTServerCreate = DateTime.Now
            };

            bool isCreated = trxnFeeAdjustment.Create(trxnFeeAdj, alloycontext);

            Assert.IsTrue(isCreated);
        }

        [TestCase]
        public void Can_Update_TrxnFeeAdj()
        {
            TransactionFeeAdjustment trxnFeeAdj = new TransactionFeeAdjustment()
            {
                TransactionId = 1000000000,
                IsActive = false,
                //FeeAdjustmentId = 10000000,
                DTTerminalLastModified = DateTime.Today,
                TransactionType = TransactionType.ProcessCheck
        };

            bool isUpdated = trxnFeeAdjustment.Update(trxnFeeAdj, new Common.Data.ZeoContext() {ChannelPartnerId = 33 });

            Assert.IsTrue(isUpdated);
        }
    }
}
