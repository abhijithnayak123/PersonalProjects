using System;
using System.Collections.Generic;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;

namespace MGI.Common.DataAccess.Test
{
	[TestFixture]
	public class OFACAlertSPTest : DataAccessAbstractTesting
	{
		private IOFACRepository _ofacStatus;
		public IOFACRepository OFACStatusRepository { set { _ofacStatus = value; } }

		[Test]
		public void TestAlert()
		{
			// uses dummy SP in Compliance DB

			Dictionary<string, object> alert = _ofacStatus.GetStatus( "whee" );

			foreach ( var x in alert )
				Console.WriteLine( x.Key + ": " + x.Value.ToString() );

			Assert.IsTrue( alert["@EntityErrorMessage"].Equals( DBNull.Value ) );
		}

		[Test]
		public void TestResults()
		{
			// uses dummy DefaultClient_Runs in ComplianceDB

			Dictionary<string, object> results = _ofacStatus.GetBatchResults( "D:\\BridgerQ408\\Batches\\OFAC-DMS_CUSTOMER" );

			foreach ( var thing in results )
				Console.WriteLine( thing.Key + " : " + thing.Value );
		}
	}
}
