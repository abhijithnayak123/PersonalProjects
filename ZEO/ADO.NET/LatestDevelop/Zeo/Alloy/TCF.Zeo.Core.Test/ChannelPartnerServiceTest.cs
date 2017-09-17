using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System.Collections.Generic;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class ChannelPartnerServiceTest
    {

        [Test]
        public void TestGroups()
        {
            ZeoContext context = new ZeoContext();
            ChannelPartnerServiceImpl cpService = new ChannelPartnerServiceImpl();

            long channelPartnerId = 34;
            List<string> groups = cpService.GetGroups(channelPartnerId, context);

            Assert.IsTrue(groups.Count > 0);
        }
    }
}
