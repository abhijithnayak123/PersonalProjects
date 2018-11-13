using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Impl;
using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NHibernate;
using NHibernate.Context;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class LedgerServiceTest
	{
		[Test]
		public void CreateLedgerTransactionTest()
		{
			//LedgerTransaction tx = new LedgerTransaction();

			//Guid entryId = tx.AddEntry( Guid.NewGuid(), 10 );
			//Assert.IsTrue( entryId != Guid.Empty );
			//Assert.IsTrue( entryId == tx.LedgerEntries[0].rowguid );

			//Guid entryId2 = tx.AddEntry( Guid.NewGuid(), -10 );
			//Assert.IsTrue( entryId2 != Guid.Empty );
			//Assert.IsTrue( entryId2 == tx.LedgerEntries[1].rowguid );
		}

		//[Test]
		//public void PersistLedgerTransactionTest()
		//{
		//    NHibRepository<LedgerTransaction> repo = new NHibRepository<LedgerTransaction>();
		//    repo.SessionFactory = NHibernateHelper.SessionFactory;

		//    LedgerServiceImpl ldgSvc = new LedgerServiceImpl();
		//    ldgSvc.LedgerTransactionRepo = repo;

		//    LedgerTransaction tx = new LedgerTransaction();

		//    Guid entryId = tx.AddEntry( new Guid("7517995A-059F-48C1-A328-DB3D1E4611C2"), 10 );

		//    Guid entryId2 = tx.AddEntry( new Guid("82D06317-0F9A-490E-8603-2FB9615AC6A6"), -10 );

		//    using ( ISession session = NHibernateHelper.OpenSession() )
		//    {
		//        CallSessionContext.Bind( session );
		//        using ( ITransaction txn = session.BeginTransaction() )
		//        {
		//            ldgSvc.RecordLedgerTransation( tx );

		//            txn.Commit();
		//        }
		//    }
		//}
	}
}
