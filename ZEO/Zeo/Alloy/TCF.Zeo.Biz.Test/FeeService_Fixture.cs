using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Biz.Common.Impl;
using TCF.Channel.Zeo.Data;
using NUnit.Framework;
using static TCF.Zeo.Common.Util.Helper;
using CommonData = TCF.Zeo.Common.Data;
using System.Collections.Generic;

namespace TCF.Zeo.Biz.Partner.Test
{
    [TestFixture]
    public class FeeService_Fixture
    {
        IFeeService feeService = new FeeServiceImpl();
        [TestCase(33)]
        [TestCase(34)]
        [TestCase(28)]
        public void Can_GetMoneyOrderFee(long channelPartnerId)
        {
            CommonData.ZeoContext context = new CommonData.ZeoContext() { ChannelPartnerId = channelPartnerId };

            List<TransactionFee> transactionFee = feeService.GetFee(TransactionType.MoneyOrder, decimal.Zero, 0, context);

            Assert.IsTrue(transactionFee.Count > 0);
        }

        [TestCase(33)]
        [TestCase(34)]
        [TestCase(28)]
        public void Can_GetCheckProcessFee(long channelPartnerId)
        {
            CommonData.ZeoContext context = new CommonData.ZeoContext() { ChannelPartnerId = channelPartnerId };

            List<TransactionFee> transactionFee = feeService.GetFee(TransactionType.ProcessCheck, decimal.Zero, 1, context);

            Assert.IsTrue(transactionFee.Count > 0);
        }
    }
}
