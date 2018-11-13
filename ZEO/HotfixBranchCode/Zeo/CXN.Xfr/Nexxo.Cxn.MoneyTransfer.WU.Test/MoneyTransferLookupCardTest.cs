using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Testing.NUnit;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using NHibernate;
using NHibernate.Context;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.WU.Common.Data;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.MoneyTransfer.WU.Impl;
using MGI.Common.Util;
using System.Security.Cryptography.X509Certificates;

namespace MGI.Cxn.MoneyTransfer.WU.Test
{
    [TestFixture]
    public class MoneyTransferLookupCardTest : AbstractTransactionalSpringContextTests
	{
		public IO WUIO { private get; set; }
		public WUGateway WUGateway { private get; set; }

        public IMoneyTransfer moneytransfer { get; set; }
		public MGIContext mgiContext { get; set; }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.MoneyTransfer.WU.Test/MGI.Cxn.MoneyTransfer.WU.Test/CXNTestSpring.xml" }; }
		}

        [SetUp]
        public void Setup()
        {
			mgiContext = new MGIContext() {
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				ChannelPartnerId = 34,
				ProcessorId = 14,
				ProviderId = 304,
				SMTrxType = MTReleaseStatus.Hold.ToString()
			};
        }

		[Test]
		public void Can_Get_LookupCardDetails()
		{
			long accountId = 1000000183;

			Account acc = new Account();
			acc.Id = accountId;
			acc.rowguid = new Guid("58B24E06-D127-4031-91C0-F9EC9DA09C0F");
			acc.Address = "Test Address";
			acc.City = "Test Bengaluru";
			acc.ContactPhone = "9848384737";
			acc.Email = "test@test.com";
			acc.FirstName = "Test Firstname";
			acc.LastName = "Test Lastname";
			acc.LevelCode = "Test Level";
			acc.LoyaltyCardNumber = "94939399393";
			acc.MobilePhone = "8483737373";
			acc.PostalCode = "83858";
			acc.SmsNotificationFlag = "false";
			acc.State = "CA";

			var isupdated = moneytransfer.UpdateAccount(acc, mgiContext);
			Assert.IsTrue(isupdated > 0);
		}

        [Test]
        [ExpectedException]
        public void Cannot_Get_LookupCardDetails()
        {
            long accountId = 1000000016;

			Account acc = new Account();
			acc.Id = accountId;

			var isupdated = moneytransfer.UpdateAccount(acc, mgiContext);
			Assert.IsTrue(isupdated > 0);  
        }

        [Test]
        [ExpectedException]
        public void Cannot_Get_LookupCardDetailswithInvalidAccountId()
        {
            long accountId = 324324;

			Account acc = new Account();
			acc.Id = accountId;

			var isupdated = moneytransfer.UpdateAccount(acc, mgiContext);
			Assert.IsTrue(isupdated > 0);  
        }

        [Test]
        [ExpectedException]
        public void Cannot_Get_LookupCardDetailswithInvalidCardNo()
        {
            long accountId = 1000000016;

			Account acc = new Account();
			acc.Id = accountId;

			var isupdated = moneytransfer.UpdateAccount(acc, mgiContext);
			Assert.IsTrue(isupdated > 0);  
        }
    }
}

