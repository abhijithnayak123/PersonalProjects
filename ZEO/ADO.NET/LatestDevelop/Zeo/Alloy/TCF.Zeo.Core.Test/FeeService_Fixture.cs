using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class FeeService_Fixture
    {
        IFeeService feeService = new ZeoCoreImpl();

        [TestCase(33)]
        [TestCase(34)]
        [TestCase(28)]
        public void Can_GetListOfFeeAdjForMO(long channelPartnerId)
        {
            ZeoContext alloycontext = new ZeoContext();
            List<FeeAdjustment> feeAdjs = feeService.GetChannelPartnerFeeAdj(Common.Util.Helper.TransactionType.MoneyOrder, channelPartnerId, alloycontext);

            Assert.AreNotEqual(0, feeAdjs.Count);
        }

        [TestCase(33)]
        [TestCase(34)]
        [TestCase(28)]
        public void Can_GetListOfFeeAdjForCheckProcessing(long channelPartnerId)
        {
            ZeoContext alloycontext = new ZeoContext();
            List<FeeAdjustment> feeAdjs = feeService.GetChannelPartnerFeeAdj(Common.Util.Helper.TransactionType.ProcessCheck, channelPartnerId, alloycontext);

            Assert.AreNotEqual(0, feeAdjs.Count);
        }
    }
}
