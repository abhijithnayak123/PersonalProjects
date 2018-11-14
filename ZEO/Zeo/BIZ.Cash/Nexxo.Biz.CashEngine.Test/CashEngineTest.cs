using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using MGI.Core.CXE.Data.Transactions.Commit;
using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Impl;

using MGI.Biz.CashEngine.Contract;
using MGI.Biz.CashEngine.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using Spring.Testing.NUnit;

namespace MGI.Biz.CashEngine.Test
{
    [TestFixture]
    public class CashEngineTest : AbstractTransactionalSpringContextTests
    {
        public ICashEngine CashService { get; set; }

        [Test]
        public void RecordCashTransactionTest()
        {
			long sessionId = 1000004571;
            decimal amount = 2;
			long id = CashService.CashOut(sessionId, amount, new MGI.Common.Util.MGIContext());          
           // SetComplete();
            Assert.That(id, Is.GreaterThan(0));
        }		

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Biz.CashEngine.Test/MGI.Biz.CashEngine.Test/Biz.Cash.Test.xml" }; }
        }

    }
}
