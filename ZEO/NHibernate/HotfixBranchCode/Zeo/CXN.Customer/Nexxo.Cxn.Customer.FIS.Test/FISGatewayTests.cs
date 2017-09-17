using MGI.Common.Util;
using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.Data;
using MGI.Cxn.Customer.FIS.Impl;
using NUnit.Framework;
using Spring.Testing.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Customer.FIS.Test
{
	[TestFixture]
	public class FISGatewayTests : AbstractTransactionalSpringContextTests
	{
		public FISGateway Gateway { private get; set; }
		public FISIOImpl FISIO { private get; set; }
		private MGI.Common.Util.MGIContext MgiContext;

		[Test]
		[ExpectedException(typeof(ClientCustomerException))]
		public void AddFailureScenario()
		{
			MgiContext = new MGIContext()
			{
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				BankId = "460",
				SSN = "252043062",
				ChannelPartnerId = 33,
				CxnAccountId = 2000000030
			};

			CustomerProfile customer = new CustomerProfile()
			{
				Address1 = "MyAddr",
				BankId = "460",
				BranchId = "200",
				City = "San Bruno",
				ChannelPartnerId = 33,
				EmployerName = "TestEmp",
				FirstName = "Fname",
				LastName = "Lname",
				LegalCode = "TestLegalCode",
				Occupation = "Student",
				PIN = "1234",
				PrimaryCountryCitizenShip = "US",
				SSN = "252043062",
				Phone1 = "9204932493",
				IDCode = "I"
			};

			long id = Gateway.Add(customer, MgiContext);
		}

		[Test]
		public void AddAccountSuccessScenario()
		{
			MgiContext = new MGIContext()
			{
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				BankId = "460",
				SSN = "252043062",
				ChannelPartnerId = 33,
				CxnAccountId = 2000000030,
				Context = new Dictionary<string, object>() { {"AccountType", "CNECT"}}
			};

			CustomerProfile customer = new CustomerProfile()
			{
				Address1 = "MyAddr",
				BankId = "460",
				BranchId = "200",
				City = "San Bruno",
				ChannelPartnerId = 33,
				EmployerName = "TestEmp",
				FirstName = "Fname",
				LastName = "Lname",
				LegalCode = "TestLegalCode",
				Occupation = "Student",
				PIN = "1234",
				PrimaryCountryCitizenShip = "US",
				SSN = "252043062",
				Phone1 = "9204932493",
				IDCode = "I"
			};

			long id = Gateway.AddAccount(customer, MgiContext);

			Assert.True(id == 0);
		}

		protected override string[] ConfigLocations
		{ get { return new string[] { "assembly://MGI.Cxn.Customer.FIS.Test/MGI.Cxn.Customer.FIS.Test/Cxn.Customer.FIS.Test.Spring.xml" }; } }
	}
}
