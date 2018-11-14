using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Customer.TCF.Impl;

namespace TCF.Zeo.Cxn.Customer.TCF.Test
{
    [TestFixture]
    public class PostFlushTest
    {
        [Test]
        public void PostFlush()
        {
            ZeoContext context = GetContext();
            FlushProcessorImpl postFlush = new FlushProcessorImpl();
            postFlush.PostFlush(100, context);

            Assert.IsTrue(true);
        }

        [Test]
        public void TellerFinalCommit()
        {
            ZeoContext context = GetContext();
            FlushProcessorImpl postFlush = new FlushProcessorImpl();
            postFlush.TellerFinalCommitCalls(context);

            Assert.IsTrue(true);
        }

        private ZeoContext GetContext()
        {
            Dictionary<string, object> _context = new Dictionary<string, object>();
            _context.Add("UserID", 150001);
            _context.Add("timezone", TimeZone.CurrentTimeZone.StandardName);
            _context.Add("BranchNum", "00003");
            _context.Add("BankNum", "099");
            _context.Add("BusinessDate", "2018-04-03");
            _context.Add("TellerNum", "10099");
            _context.Add("LawsonID", "000104");
            _context.Add("LU", "HERA2352");
            _context.Add("CashDrawer", "678");
            _context.Add("AmPmInd", "A");

            ZeoContext context = new ZeoContext()
            {
                ChannelPartnerId = 34,
                CheckUserName = "INGO",
                URL = "https://proxy.ic.local/ingo/webservice/",
                IngoBranchId = 12345,
                CompanyToken = "simulator",
                EmployeeId = 12345,
                AgentId = 123344455,
                ProviderId = 34,
                CustomerId = 2251473180545873,
                CustomerSessionId = 1000000030,
                SSOAttributes = _context
            };
            return context;
        }
    }
}
