using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MGI.Cxn.Customer.TCIS.Impl;
using Spring.Testing.NUnit;
using MGI.Cxn.Customer.Contract;
using MGI.Common.Util;
using MGI.Cxn.Customer.Data;

namespace MGI.Cxn.Customer.TCIS.Test
{
	[TestFixture]
	public class Gateway_Fixture : AbstractTransactionalSpringContextTests
	{
		public MGIContext MgiContext = new MGIContext();
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.Customer.TCIS.Test/MGI.Cxn.Customer.TCIS.Test/MGI.Cxn.Customer.TCIS.Test.Spring.xml" }; }
		}

		public IClientCustomerService TCISGateway { get; set; }

		#region Positive Register test case

		[Test]
		public void FetchAllCustomerBySSN()
		{
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();

			customerLookUpCriteria.Add("SSN", "718111072");
			customerLookUpCriteria.Add("DateofBirth", "01/03/1975");


			List<MGI.Cxn.Customer.Data.CustomerProfile> customer = TCISGateway.FetchAll(customerLookUpCriteria, GetContext());

			Assert.NotNull(customer);
			Assert.Greater(customer.Count(), 0);
		}

		[Test]
		public void FetchAllCustomerByCardNumber()
		{
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();

			customerLookUpCriteria.Add("DateofBirth", "01/03/1975");
			customerLookUpCriteria.Add("CardNumber", "57222720001572648");
			customerLookUpCriteria.Add("AccountType", "SAV");

			List<MGI.Cxn.Customer.Data.CustomerProfile> customer = TCISGateway.FetchAll(customerLookUpCriteria, GetContext());

			Assert.NotNull(customer);
			Assert.Greater(customer.Count, 0);
		}



		[Test]
		public void FetchAllCustomerByAccountNumber()
		{
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();

			customerLookUpCriteria.Add("DateofBirth", "01/03/1975");
			customerLookUpCriteria.Add("AccountNumber", "3000109081");
			customerLookUpCriteria.Add("AccountType", "SAV");

			List<MGI.Cxn.Customer.Data.CustomerProfile> customer = TCISGateway.FetchAll(customerLookUpCriteria, GetContext());

			Assert.NotNull(customer);
			Assert.Greater(customer.Count, 0);
		}


		[Test]
		public void CustomerRegistration()
		{

			MGI.Cxn.Customer.Data.CustomerProfile customerProfile = new MGI.Cxn.Customer.Data.CustomerProfile();

			customerProfile.FirstName = "Angelique";
			customerProfile.MiddleName = "";
			customerProfile.LastName = "Fraser";
			customerProfile.Address1 = "13500 SOUTH ST";
			customerProfile.Address2 = "";
			customerProfile.City = "MINNETONKA";
			customerProfile.State = "MN";
			customerProfile.ZipCode = "55345";
			customerProfile.Phone1 = "6126617539";
			customerProfile.Phone1Type = "Home";
			customerProfile.Phone2 = "";
			customerProfile.Phone2Type = "";
			customerProfile.Phone2Provider = "";
			customerProfile.SSN = "718111073";
			customerProfile.Gender = "M";
			customerProfile.DateOfBirth = Convert.ToDateTime("01/03/1976");
			customerProfile.MothersMaidenName = "MOM";
			customerProfile.GovernmentIDType = "D";
			customerProfile.LegalCode = "U";
			customerProfile.IDIssuingState = "MN";
			customerProfile.IDIssuingCountry = "United Status";
			customerProfile.GovernmentId = "K3236245";
			customerProfile.IDIssueDate = Convert.ToDateTime("10/10/2010");
			customerProfile.IDExpirationDate = Convert.ToDateTime("10/10/2020");
			customerProfile.PrimaryCountryCitizenShip = "UNITED STATES";
			customerProfile.SecondaryCountryCitizenShip = "UNITED STATES";
			customerProfile.Occupation = "16";
			customerProfile.EmployerName = "MGI";

			long id = TCISGateway.Add(customerProfile, GetContext());

			Assert.Greater(id, 0);
			// Assert.Greater(customer.Count(), 0);
		}

		[Test]
		public void CanCustomerSynIn()
		{
			CustomerProfile customerProfile = TCISGateway.Fetch(GetContext());
			customerProfile.ClientID = null;
			TCISGateway.Update("2000000000", customerProfile, GetContext());
		}

		private MGIContext GetContext()
		{
			Dictionary<string, object> ssoAttributes = new Dictionary<string, object>()
			{
				{"UserID", "ZeoMGI"},
				{"TellerNum","98001"},
				{"BranchNum","00043"},
				{"BankNum", "099"},
				{"LawsonID","000104"},
				{"LU","23B7"},
				{"CashDrawer", "980"},
				{"AmPmInd","A"},
				{"MachineName","001-MGIw7"},
				{"BusinessDate", "20150601"}
			};
			MGIContext mgiContext = new MGIContext()
			{
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				ChannelPartnerId = 34,
				BankId = "099",
				CxnAccountId = 2000000000,
				Context = ssoAttributes
			};

			return mgiContext;
		}


		#endregion
	}
}
