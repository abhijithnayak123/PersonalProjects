using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class PricingCluster_Fixture
    {
        IPricingCluster pricingCluster = new ZeoCoreImpl();
        ZeoContext alloycontext = new ZeoContext();
        [TestCase(33)]
        [TestCase(34)]
        [TestCase(28)]
        public void Can_Get_BaseFee_CheckProcessing(long channelPartnerId)
        {
            List<Pricing> pricings = pricingCluster.GetBaseFee(Common.Util.Helper.TransactionType.ProcessCheck, channelPartnerId, 0, "1", alloycontext);

            Assert.AreNotEqual(0, pricings.Count);
        }

        [TestCase(33)]
        [TestCase(34)]
        [TestCase(28)]
        public void Can_Get_BaseFee_MoneyOrder(long channelPartnerId)
        {
            List<Pricing> pricings = pricingCluster.GetBaseFee(Common.Util.Helper.TransactionType.MoneyOrder, channelPartnerId, 0, null, alloycontext);

            Assert.AreNotEqual(0, pricings.Count);
        }
    }
}
