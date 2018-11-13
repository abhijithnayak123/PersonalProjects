using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data.Transactions.Stage;
using MGI.Core.CXE.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;
using MGI.Core.CXE.Data;

using NHibernate;
using NHibernate.Context;

using Spring.Context;
using Spring.Context.Support;
using Spring.Testing.NUnit;

namespace MGI.Core.CXE.Test
{
    [TestFixture]
    public class FundServiceTest : AbstractTransactionalSpringContextTests
    {
       
        public IFundsService CXEFundsService { get; set; }
		public ICustomerService CXECustomerService { get; set; }

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/CXETestSpring.xml" }; }
        }


        [SetUp]
        public void Setup()
        {
            //IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            //FundsService = (IFundsService)ctx.GetObject("FundService");
            //session = (ISession)ctx.GetObject("session");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void GetTransactionTest()
        {
            MGI.Core.CXE.Data.Transactions.Commit.Funds funds = new Data.Transactions.Commit.Funds();
			funds = CXEFundsService.Get(1000000035);

			Assert.AreEqual(funds.Id, 1000000035);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateTest()
        {
			Customer customer = CXECustomerService.Lookup(1000000000003880);
			//fund.Account.Customer = customer;
			Account account = customer.GetAccount(1000001018);

            Funds fund = new Funds
            {
				Account = account,
                Amount = 100,
                Fee = 10,
                Status = 0,
                Type = 1,
                DTTerminalCreate = DateTime.Now,
                DTTerminalLastModified = DateTime.Now,
				DTServerCreate = DateTime.Now,
				DTServerLastModified = DateTime.Now
            };

            long trnid = CXEFundsService.Create(fund);
            Assert.IsTrue(trnid >= 1);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CommitTest()
        {
            try
            {
                //1000000013 exist in table tTxn_Funds_Stage
                CXEFundsService.Commit(1000000013,"");
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
           
        }
    }
}
