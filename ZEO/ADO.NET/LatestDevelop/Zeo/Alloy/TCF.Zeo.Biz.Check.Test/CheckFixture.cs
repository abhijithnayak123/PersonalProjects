using TCF.Zeo.Biz.Check.Impl;
using commonData = TCF.Zeo.Common.Data;
using NUnit.Framework;
using TCF.Zeo.Biz.Check.Contract;
using TCF.Channel.Zeo.Data;

namespace TCF.Zeo.Biz.Check.Test
{
    public class CheckFixture
    {

        [Test]
        public void Can_GetStatus()
        {
            ICPService _checkProcess = new CPServiceImpl();           
            long transactionId = 1000000001;
            bool includeImage = true;
            commonData.ZeoContext context = GetContext();
            TCF.Channel.Zeo.Data.Check status = _checkProcess.GetStatus(transactionId, includeImage, context);
            Assert.IsNotNullOrEmpty(status.ToString());
        }
        [Test]
        public void can_getFrankData()
        {
            ICPService _CheckfranchData = new CPServiceImpl();
            commonData.ZeoContext context = GetContext();
            long transactionId = 32411122344;
            var frankdata = _CheckfranchData.GetCheckFrankingData(transactionId, context);
            Assert.IsNotNull(frankdata);

        }

        private commonData.ZeoContext GetContext()
        {
            commonData.ZeoContext context = new commonData.ZeoContext()
            {
                ChannelPartnerId = 34,
                CheckUserName = "INGO",
                URL = "https://proxy.ic.local/ingo/webservice/",
                IngoBranchId = 12345,
                CompanyToken = "simulator",
                EmployeeId = 12345,
                ChannelPartnerName = "Synovus",
                CustomerSessionId = 1234567890,
                AgentId = 1234567890,
                ProviderId = 34,
            };
            return context;
        }

    }
}
