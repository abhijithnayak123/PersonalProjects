using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NUnit.Framework;
using Spring.Testing.NUnit;

using MGI.Common.DataAccess.Contract;

using MGI.Cxn.Fund.TSys.Data;
using MGI.Cxn.Fund.TSys.Contract;

namespace MGI.Cxn.Fund.TSys.Test
{
	[TestFixture]
	public class TSysDBIntegrationTests : AbstractTransactionalDbProviderSpringContextTests
	{
		private IRepository<TSysAccount> _tSysAccountRepo;
		public IRepository<TSysAccount> TSysAccountRepo { set { _tSysAccountRepo = value; } }

		private IRepository<TSysTransaction> _tSysTrxRepo;
		public IRepository<TSysTransaction> TSysTrxRepo { set { _tSysTrxRepo = value; } }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.Fund.TSys.Test/MGI.Cxn.Fund.TSys.Test/springTSysTest.xml" }; }
		}

		private void CreateRecords()
		{
			//Guid customerRowguid = Guid.NewGuid();
			//AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tCustomers(rowguid,id,cxeid,dtcreate) values('{0}',222,222,getdate())", customerRowguid));
			//AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tAccounts(rowguid,id,cxeid,cxnid,customerpk,dtcreate) values(newid(),555,555,555,'{0}',getdate())", customerRowguid));
		}

		[Test]
        //Pass Test Case
		public void AccountTest()
		{
			TSysAccount profile = getProfile();
			_tSysAccountRepo.AddWithFlush(profile);

			SetComplete();
			EndTransaction();

			int count = (int)AdoTemplate.ExecuteScalar(CommandType.Text, "select count(*) from tTSys_Account");
			Assert.IsTrue(count == 1);
		}

		[Test]
        //Pass Test Case
		public void TransactionTest()
		{
			TSysAccount profile = getProfile();
			_tSysAccountRepo.AddWithFlush(profile);

			TSysTransaction trx = new TSysTransaction
			{
				Account = profile,
				Amount = 10m,
				DTTerminalCreate = DateTime.Now,
				TransactionType = TSysTransactionType.Credit,
				Status = TSysTransactionStatus.Staged,
				Description = string.Empty,
				Balance = 100m,
				ChannelPartnerID = 33L
			};

			_tSysTrxRepo.AddWithFlush(trx);

			SetComplete();
			EndTransaction();

			int count = (int)AdoTemplate.ExecuteScalar(CommandType.Text, "select count(*) from tTSys_Account");
			Assert.IsTrue(count == 1);
			count = (int)AdoTemplate.ExecuteScalar(CommandType.Text, "select count(*) from tTSys_Trx");
			Assert.IsTrue(count == 1);
		}

		[TearDown]
		public void teardown()
		{
			AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from tTSys_Trx");
			AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from tTSys_Account");
		}

		private TSysAccount getProfile()
		{
			return new TSysAccount
			{
				AccountId = 188135700,
				UserId = 102354369,
				ExternalKey = "extkeynexxo2",
				ProgramId = 13140417,
				FirstName = "Justin",
				MiddleName = "Dev",
				LastName = "Test",
				Address1 = "111 Anza Blvd",
				Address2 = "Suite 200",
				City = "Burlingame",
				State = "CA",
				ZipCode = "94010",
				Country = "USA",
				DateOfBirth = new DateTime(1990, 1, 1),
				Phone = "650-685-5702",
				PhoneType = "cell",
				SSN = "260-53-5522",
				CardNumber = "11111111111111",
				Activated = false,
				FraudScore = 10,
				FraudResolution = "dunno",
				DTTerminalCreate = DateTime.Now
			};
		}
	}
}
