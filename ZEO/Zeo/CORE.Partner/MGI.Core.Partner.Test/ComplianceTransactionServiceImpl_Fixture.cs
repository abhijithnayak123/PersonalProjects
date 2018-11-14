using System;
using System.Collections.Generic;
using System.Data;

using Iesi.Collections.Generic;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
//using Moq;

using Spring.Testing.NUnit;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class ComplianceTransactionServiceImpl_Fixture:AbstractPartnerTest
	{
		private IComplianceTransactionService _txnSvc;
		public IComplianceTransactionService ComplianceTransactionService { set { _txnSvc = value; } }

		//private ITransactionHistoryService _txnHistSvc;
		//public ITransactionHistoryService TransactionHistoryService { set { _txnHistSvc = value; } }

		[Test]
		public void TestGettingTxns()
		{
			//List<TransactionHistory> txns = _txnHistSvc.Get( 1000000000000130 );

			//Console.WriteLine( txns.Count );

			List<ComplianceTransaction> txns = _txnSvc.Get( 1000000000000130 );

			Console.WriteLine( txns.Count );
			
		}

		[Test]
		public void TestGetTxnsWithRecipientId()
		{
			List<ComplianceTransaction> txns = _txnSvc.Get( 1000000000000130, 898922 );
			Console.WriteLine( txns.Count );
		}
	}
}