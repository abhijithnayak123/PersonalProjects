using System;
using System.Collections.Generic;

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
	public class AccountDBIntegrationTests: AbstractTransactionalDbProviderSpringContextTests
	{
		private ICustomerService _custSvc;
		public ICustomerService CustomerService { set { _custSvc = value; } }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Core.Partner.Test/MGI.Core.Partner.Test/springPartnerTest.xml" }; }
		}
	}
}
