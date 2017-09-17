using System;
using System.Collections.Generic;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
using Moq;

using Spring.Testing.NUnit;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class AccountDBIntegrationTests: AbstractTransactionalDbProviderSpringContextTests
	{
		private IAccountService _acctSvc;
		public IAccountService AccountService { set { _acctSvc = value; } }

		private ICustomerService _custSvc;
		public ICustomerService CustomerService { set { _custSvc = value; } }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Core.Partner.Test/MGI.Core.Partner.Test/springPartnerTest.xml" }; }
		}

		[Test]
		public void CreateAccountTest()
		{
			Customer customer = _custSvc.Create( 222 );

			long cxeId = 10101;

			Account account = _acctSvc.Create( customer, cxeId, cxeId );

			Console.WriteLine( account.rowguid );

			Account acctCheck = _acctSvc.Lookup( 10101 );

			Console.WriteLine( acctCheck.rowguid );
		}
	}
}
