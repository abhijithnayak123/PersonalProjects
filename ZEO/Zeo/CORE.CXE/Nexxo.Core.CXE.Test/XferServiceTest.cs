using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NUnit.Framework;
using Moq;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.CXE.Data.Transactions.Stage;
using MGI.Core.CXE.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NHibernate;
using NHibernate.Context;

using Spring.Context;
using Spring.Context.Support;
using Spring.Testing.NUnit;

namespace MGI.Core.CXE.Test
{
    [TestFixture]
    public class XferServiceTest : AbstractTransactionalDbProviderSpringContextTests
    {

        public IMoneyTransferService CXEMoneyTransferService { get; set; }
        public IAccountService AccountsService { get; set; }
        public ICustomerService CXECustomerService { get; set; }

        protected override string[] ConfigLocations
        {
            //get { return new string[] { "assembly://MGI.Core.CXE.Impl/MGI.Core.CXE.Impl/CXESpring.xml" }; }
            get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/CXETestSpring.xml" }; }
        }

        [SetUp]
        public void Setup()
        {
            //IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            //FundsService = (IFundsService)ctx.GetObject("FundService");
            //AccountService = (IAccountService)ctx.GetObject("AccountService");
            //SessionFactory = (ISessionFactory)ctx.GetObject("SessionFactoryCXE");
            //Guid _customerPK = Guid.NewGuid();
            //AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("INSERT tCustomers(rowguid,Id,DTCreate) VALUES('{0}',100999,getdate())", _customerPK));
            //AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("INSERT tCustomerEmploymentDetails(CustomerPK,Occupation,DTCreate) VALUES('{0}','joker',getdate())", _customerPK));
            //Guid idtypePK = (Guid)AdoTemplate.ExecuteScalar(CommandType.Text, "select rowguid from tIdTypes where Id=110");
            //AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("INSERT tCustomerGovernmentIdDetails(CustomerPK,IdTypePK,DTCreate) VALUES('{0}','{1}',getdate())", _customerPK, idtypePK));
            //AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("INSERT tCustomerAccounts(rowguid,type,provider,dtcreate,customerpk) values(newid(),6,8,getdate(),'{0}')", _customerPK));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void GetTransactionTest()
        {
            var fundsTransaction = CXEMoneyTransferService.Get(1000000013);
            Assert.That(fundsTransaction.Id == 1000000013, "funds get method failed");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateTest()
        {

			Customer cust = CXECustomerService.Lookup(1000000000003880);
			Account account = cust.GetAccount(1000001018);

            MoneyTransfer fund = new MoneyTransfer
            {
                Account = account,
                Amount = 33,
                Fee = 5,
                Status = 4,
                 DTTerminalCreate = DateTime.Now,
                DTTerminalLastModified = DateTime.Now,
				DTServerCreate = DateTime.Now,
				DTServerLastModified = DateTime.Now,
                ReceiverName = "test test",
                Destination = "IN",
                DestinationAmount = 100000
            };

            long trnid = CXEMoneyTransferService.Create(fund);

            SetComplete();
            EndTransaction();

            Assert.That(trnid >= 0, "funds add method failed");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CommitTest()
        {
            CXEMoneyTransferService.Commit(1000000013);
            var fundsTransaction = CXEMoneyTransferService.Get(1000000013);
            // SetComplete();
            Assert.That(fundsTransaction.Id == 1000000013, "funds add method failed");
        }
    }
}
