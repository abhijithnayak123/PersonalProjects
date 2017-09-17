using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using MGI.Core.CXE.Data;

using Spring.Data.Core;

using NUnit.Framework;

namespace MGI.Biz.Compliance.Test
{
	[TestFixture]
	public class GetCustomersTest : AbstractBizComplianceTests
	{
		private IRepository<Customer> _customerRepo;
		public IRepository<Customer> CustomerRepo { set { _customerRepo = value; } }

		[Test]
		public void SetupTest()
		{
			long cpId = 27;

			List<Customer> cs = _customerRepo.FilterByChildCriteria( "Profile", "ChannelPartnerId", cpId );

			foreach ( Customer c in cs )
			{
				Console.WriteLine( c.Id + " : " + c.FirstName );
			}
		}
	}
}
