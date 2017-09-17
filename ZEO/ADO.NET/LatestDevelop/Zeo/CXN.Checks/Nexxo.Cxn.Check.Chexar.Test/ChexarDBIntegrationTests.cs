using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NUnit.Framework;
using Moq;
using Spring.Testing.NUnit;

using MGI.Common.DataAccess.Contract;

using MGI.Cxn.Check.Chexar.Contract;
using MGI.Cxn.Check.Chexar.Data;

namespace MGI.Cxn.Check.Chexar.Test
{
	[TestFixture]
	public class ChexarDBIntegrationTests : AbstractTransactionalDbProviderSpringContextTests
	{
		private IChexarPartnerConfigurator _checkDB;
		public IChexarPartnerConfigurator ChexarDB { set { _checkDB = value; } }

		private IRepository<ChexarSession> _chexarSession;
		public IRepository<ChexarSession> ChxrSession { set { _chexarSession = value; } }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.Check.Chexar.Test/MGI.Cxn.Check.Chexar.Test/springTestSim.xml" }; }
		}

		private void CreateRecords()
		{
			//Guid customerRowguid = Guid.NewGuid();
			//AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tCustomers(rowguid,id,cxeid,dtcreate) values('{0}',222,222,getdate())", customerRowguid));
			//AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tAccounts(rowguid,id,cxeid,cxnid,customerpk,dtcreate) values(newid(),555,555,555,'{0}',getdate())", customerRowguid));
		}

		[Test]
		public void PartnerTest()
		{
			ChexarPartner p = new ChexarPartner
			{
				Id = 1,
				Name = "Justin Test",
				URL = "http://nowhere/",
				DTServerCreate = DateTime.Now
			};

			_checkDB.SetupPartner(p);

			SetComplete();
			EndTransaction();

			int count = (int)AdoTemplate.ExecuteScalar(CommandType.Text, "select count(*) from tChxr_Partner");
			Assert.IsTrue(count > 0);
		}

		[Test]
		public void SessionTest()
		{
			ChexarPartner p = new ChexarPartner
			{
				Id = 1,
				Name = "Justin Test",
				URL = "http://nowhere/",
				DTServerCreate = DateTime.Now
			};

			_checkDB.SetupPartner(p);

			ChexarSession session = new ChexarSession
			{
				BranchId = 0,
				CompanyToken = "test",
				EmployeeId = 0,
				Location = "test",
				Partner = p,
				DTTerminalCreate = DateTime.Now
			};

			_chexarSession.AddWithFlush(session);
		}

		//[Test]
		//public void LocationLookupTest()
		//{
		//    Guid partnerRowGuid = Guid.NewGuid();
		//    AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChxr_Partner(rowguid,Name,Url,dtcreate) values('{0}','Justin Test','http://test',getdate())", partnerRowGuid));

		//    ChexarIdentity i = _checkDB.("test location");

		//    Assert.AreEqual(i.rowguid, identityRowGuid);
		//}

		[TearDown]
		public void teardown()
		{
			AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from tChxr_Session");
			AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from tChxr_Partner");
		}
	}
}
