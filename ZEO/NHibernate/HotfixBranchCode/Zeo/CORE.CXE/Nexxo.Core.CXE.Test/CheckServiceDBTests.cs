using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.CXE.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
using Moq;

using Spring.Testing.NUnit;

namespace MGI.Core.CXE.Test
{
	[TestFixture]
	public class CheckServiceDBTests : AbstractTransactionalDbProviderSpringContextTests
	{
		private ICustomerService _custSvc;
		public ICustomerService CustomerService { set { _custSvc = value; } }

		private ICheckService _chkSvc;
		public ICheckService CheckService { set { _chkSvc = value; } }

		private long customerId = 1000000000003880; //100999;
		private long accountId = 1000001018; //1000000200;
		//private Guid accountPK;
        private string timezone = string.Empty;

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/springCustTest.xml" }; }
		}

		[SetUp]
		public void Setup()
		{
			////setup a check account to use
			//Guid customerPK = Guid.NewGuid();
			//accountPK = Guid.NewGuid();
			//AdoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "insert tCustomers(rowguid,id,DTCreate) values('{0}',{1},getdate())", customerPK, customerId ) );
			//AdoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "insert tCustomerAccounts(rowguid,type,dtcreate,customerpk) values('{0}',3,getdate(),'{1}')", accountPK, customerPK ) );
		}

		[Test]
		public void StageACheck()
		{
			long id = SetupACheck(100m);
			Console.WriteLine( "check transaction id: " + id );
			Assert.IsTrue( id > 0 );
		}

		private long SetupACheck(decimal amount)
		{
			Customer c = _custSvc.Lookup(customerId);
			Account chkAcct = c.GetAccount(accountId);

			Data.Transactions.Stage.Check check = new Data.Transactions.Stage.Check
			{
				Account = chkAcct,
				Amount = 100m,
				CheckType = 1,
				Fee = 1m,
				IssueDate = DateTime.Today,
				MICR = "12345",
				Status = 1
			};
			check.AddImages(Convert.FromBase64String("0xFFD8FFE000104A4649"), Convert.FromBase64String("0xFFD8FFE000104A4649"), "jpg");

            return _chkSvc.Create(check, timezone);			
		}

		[Test]
		public void CommitAndGetACheck()
		{
			decimal amt = 100m;

			long chkid = SetupACheck(amt);

			_chkSvc.Commit( chkid );
			StopStartTransation();

			Data.Transactions.Commit.Check chk = _chkSvc.Get( chkid );

			Assert.IsTrue( chk.Amount == amt );
			Assert.IsTrue( chk.rowguid != Guid.Empty );
		}

		[Test]
		public void UpdateCheckStatus()
		{
			long chkid = SetupACheck(100m);

			int newStatus = 2;
            _chkSvc.Update(chkid, newStatus, timezone);
			_chkSvc.Commit( chkid );
			StopStartTransation();

			Data.Transactions.Commit.Check chk = _chkSvc.Get( chkid );

			Assert.IsTrue( chk.Status==newStatus );
		}

		public void StopStartTransation()
		{
			SetComplete();
			EndTransaction();
			StartNewTransaction();
		}


	}
}
