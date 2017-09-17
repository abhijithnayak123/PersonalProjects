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
	//[TestFixture]
	//public class TransactionHistoryServiceTests// : AbstractTransactionalDbProviderSpringContextTests
	//{
	//    //public ITransactionHistoryService CXETransactionHistoryService { get; set; }

	//    //protected override string[] ConfigLocations
	//    //{
	//    //    //get { return new string[] { "assembly://MGI.Core.CXE.Impl/MGI.Core.CXE.Impl/CXESpring.xml" }; }
	//    //    get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/CXETestSpring.xml" }; }
	//    //}

	//    //[Test]
	//    //public void GetTransactionTest()
	//    //{
	//    //    var transactionHistory = CXETransactionHistoryService.Get(1000000000000290);
	//    //    Assert.That(transactionHistory.Count == 8, "Get transaction history failed");
	//    //}

	//    //[Test]
	//    //public void GetTransactionInvalidPANTest()
	//    //{
	//    //    var transactionHistory = CXETransactionHistoryService.Get(1000000000000);
	//    //    Assert.That(transactionHistory.Count == 0, "Get transaction history failed");
	//    //}
	//}
}
