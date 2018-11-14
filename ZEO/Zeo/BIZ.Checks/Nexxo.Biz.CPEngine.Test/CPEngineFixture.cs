using System;
using System.Collections.Generic;
using NUnit.Framework;
using Spring.Testing.NUnit;
using MGI.Biz.CPEngine.Data;
using MGI.Biz.CPEngine.Contract;
using MGI.Biz.CPEngine.Impl;
using Spring.Data.Core;
using Spring.Context;
using Spring.Context.Support;


namespace MGI.Biz.CPEngine.Test
{
	[TestFixture]
	public class CPEngineFixture : AbstractTransactionalSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get
			{
				return new string[] { "assembly://MGI.Biz.CPEngine.Test/MGI.Biz.CPEngine.Test/Biz.CPEngine.Test.xml" };
			}
		}
		public ICPEngineService BIZCPEngine
		{
			get { return BIZCPEngine; }
			set { BIZCPEngine = value; }
		}

		[Test]
		public void GetSessionTest()
		{
			Dictionary<string, object> _context = new Dictionary<string, object>();
			_context.Add("ChannelPartnerId", 28);
			_context.Add("timezone", TimeZone.CurrentTimeZone.StandardName);
			_context.Add("CheckUserName", "9900004");
			_context.Add("CheckPassword", "9900004");
			_context.Add("location", "test");

			BIZCPEngine.GetChexarSessions(_context);

		}

	}
}


