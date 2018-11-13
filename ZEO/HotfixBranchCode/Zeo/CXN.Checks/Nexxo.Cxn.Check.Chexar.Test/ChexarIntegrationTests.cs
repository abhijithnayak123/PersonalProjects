using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Diagnostics;

using NUnit.Framework;
using Moq;
using Spring.Testing.NUnit;

using MGI.Cxn.Check.Data;
using MGI.Cxn.Check.Contract;

using ChexarIO;
using MGI.Cxn.Check.Chexar.Contract;
using MGI.Cxn.Check.Chexar.Data;
using MGI.Cxn.Check.Chexar.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;

namespace MGI.Cxn.Check.Chexar.Test
{
	[TestFixture]
	public class ChexarIntegrationTests : AbstractTransactionalDbProviderSpringContextTests
	{
		public MGIContext MgiContext { get; set; }
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.Check.Chexar.Test/MGI.Cxn.Check.Chexar.Test/springTestChexar.xml" }; }
		}

		private ChexarGateway _chexarProcessor;

		public ChexarGateway ChexarProcessor
		{
			get { return _chexarProcessor; }
			set { _chexarProcessor = value; }
		}

		[SetUp]
		public void setup()
		{
			CreatePartner();
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			MgiContext = new MGI.Common.Util.MGIContext()
			{
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				ChannelPartnerId = 34,
				LocationName = "Test",
				CheckUserName = "9900004",
				CheckPassword = "9900004"
			};
		}

		private void CreatePartner()
		{
			ChexarPartner partner = new ChexarPartner
			{
				Id = 34,
				Name = "TCF National Bank",
				URL = "http://beta.chexar.net/webservice/",
				DTServerCreate = DateTime.Today
			};

			_chexarProcessor.SetupPartner(partner);
		}

		[Test]
		public void CreateAccount()
		{
			CheckAccount checkAcct = new CheckAccount
			{
				Id = 111222333,
				FirstName = "Justin",
				LastName = "Test",
				Address1 = "101 test st",
				City = "SF",
				State = "CA",
				Zip = "12345",
				DateOfBirth = DateTime.Today.AddYears(-20),
				Phone = "4151234567",
				SSN = "123659876"
			};

			_chexarProcessor.Register(checkAcct, MgiContext, TimeZone.CurrentTimeZone.StandardName);
		}

		private CheckInfo CreateCheck(decimal amount)
		{
			return new CheckInfo
			{
				Amount = amount,
				Latitude = 0,
				Longitude = 0,
				Micr = "fjdkafjdkslfajk",
				FrontImage = Convert.FromBase64String("0xFFD8FFE000104A4649"),
				BackImage = Convert.FromBase64String("0xFFD8FFE000104A4649"),
				FrontImageTIF = Convert.FromBase64String("0xFFD8FFE000104A4649"),
				BackImageTIF = Convert.FromBase64String("0xFFD8FFE000104A4649"),
				ImageFormat = "jpg",
				IssueDate = DateTime.Today,
				Type = Check.Data.CheckType.Cashier
			};
		}
	}
}
