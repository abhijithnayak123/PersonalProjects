using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class CustomerFeeAdjustment_Fixture
    {
        ICustomerFeeAdjustmentService customerFeeAdjustment = new ZeoCoreImpl();
        [TestCase]
        public void Can_GetCustomerMOFeeAdj()
        {
            ZeoContext alloycontext = new ZeoContext();
            long customerId = 1000000000;

            List<FeeAdjustment> feeAdj = customerFeeAdjustment.LookupCustomerFeeAdjustments(Common.Util.Helper.TransactionType.MoneyOrder, customerId, alloycontext);

            Assert.AreNotEqual(0, feeAdj.Count);
        }

        [TestCase]
        public void Can_GetCustomerCPFeeAdj()
        {
            ZeoContext alloycontext = new ZeoContext();
            long customerId = 1000000000;

            List<FeeAdjustment> feeAdj = customerFeeAdjustment.LookupCustomerFeeAdjustments(Common.Util.Helper.TransactionType.ProcessCheck, customerId, alloycontext);

            Assert.AreNotEqual(0, feeAdj.Count);
        }

    }
}
