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
    public class AgentService_Fixture
    {
        IAgentService agentService = new ZeoCoreImpl();
        [Test]
        public void Can_Authenticate_SSO_Agent()
        {
            ZeoContext context = new ZeoContext();
            UserDetails userdetails = agentService.AuthenticateSSOAgent("systemadmin", "system","admin", "systemadmin", 4, "98001", null, 34, context);
            Assert.IsNotNull(userdetails);
        }

        [Test]
        public void Can_Create_Session()
        {
            ZeoContext context = new ZeoContext();

            AgentSession userdetails = agentService.CreateSession(500002,"98001",34,null,"2015-06-10", context);
            Assert.IsNotNull(userdetails);
        }
    }
}
