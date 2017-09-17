using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Biz.Common.Data;
using TCF.Zeo.Biz.Common.Impl;
using TCF.Zeo.Common.Data;
using NUnit.Framework;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Biz.Test
{
    [TestFixture]
    public class Compliance_Fixture
    {
        IComplianceService compliance = new ComplianceServiceImpl();

        [TestCase]
        public void Can_Get_Transaction_Limits()
        {
            ZeoContext context = new ZeoContext() { ChannelPartnerId = 34, CustomerSessionId = 100000000, ShouldIncludeShoppingCartItems = false};
            Limit limit = compliance.GetTransactionLimit(TransactionType.MoneyOrder, context);
            Assert.IsNotNull(limit);
        }
    }
}
