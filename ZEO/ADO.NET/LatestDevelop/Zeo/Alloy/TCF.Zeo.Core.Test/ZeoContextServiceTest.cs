using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;

namespace TCF.Zeo.Core.Test
{
    public class ZeoContextServiceTest
    {
        IZeoContext Context = new ZeoContextServiceImpl();

        [Test]
        public void ZeoContextForAgentTest()
        {
            long agentSessionId = 1000000006;
            ZeoContext context = new ZeoContext();
            ZeoContext getAgentContext = Context.GetZeoContextForAgent(agentSessionId, context);

            Assert.IsNotNull(getAgentContext);
        }


        [Test]
        public void ZeoContextForCustomerTest()
        {
            long customerSessionId = 1000000001;
            ZeoContext context = new ZeoContext();
            ZeoContext getCustomerContext = Context.GetZeoContextForCustomer(customerSessionId, context);

            Assert.IsNotNull(getCustomerContext);
        }
    }
}
