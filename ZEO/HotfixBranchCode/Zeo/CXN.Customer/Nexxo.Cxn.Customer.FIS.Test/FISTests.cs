using System;
using System.Collections.Generic;
using NUnit.Framework;
using MGI.Cxn.Customer.FIS.Test.FISService;
using Spring.Testing.NUnit;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using MGI.Cxn.Customer.FIS.Impl;
using MGI.Cxn.Customer.FIS.Data;
using CxnCustomerData = MGI.Cxn.Customer.Data;
using ConnImpl = MGI.Cxn.Customer.FIS.Impl;
using ConnData = MGI.Cxn.Customer.FIS.Data;
using FISConnect = MGI.Cxn.Customer.FIS.Data.FISConnect;


namespace MGI.Cxn.Customer.FIS.Test
{
	[TestFixture]
	public class FISTests : AbstractTransactionalSpringContextTests
	{
		public FISIOImpl FISIO { private get; set; }
		public ConnImpl.FISConnect FISConn { private get; set; }
		private MGI.Common.Util.MGIContext MgiContext;

		#region Spring Assembly

		protected override string[] ConfigLocations
		{ get { return new string[] { "assembly://MGI.Cxn.Customer.FIS.Test/MGI.Cxn.Customer.FIS.Test/Cxn.Customer.FIS.Test.Spring.xml" }; } }
		# endregion

		#region Private Properties

		private Dictionary<string, object> customerLookUpCriteria;

		#endregion

		[TestFixtureSetUp]
		public void fixtSetup()
		{
			MgiContext = new Common.Util.MGIContext()
			{
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				BankId = "300",
				SSN = "442770002",
				ChannelPartnerId = 33
			};
		}

		/// <summary>
		/// This test method is added for SQL Injection US#1789
		/// </summary>
		[Test]
		public void TestfetchFromConnectsDB()
		{
			string SSN = "111111111";

			ConnData.FISConnect FISCustomer = new ConnData.FISConnect();

			FISCustomer = FISConn.GetSSNForCustomer(SSN);

			Assert.IsNotNull(FISCustomer);

			Assert.AreEqual(FISCustomer.CustomerTaxNumber, "111111111");

		}

		/// <summary>
		/// This test method for FISConnectDB US1931 
		/// </summary>
		[Test]
		public void TestFISConnectCustomerLookUp()
		{
			List<ConnData.FISConnect> objFisConnects = new List<FISConnect>();
			customerLookUpCriteria = new Dictionary<string, object>();
			customerLookUpCriteria.Add("SSN", "111111111");
			customerLookUpCriteria.Add("PhoneNumber", "0706568112");
			customerLookUpCriteria.Add("ZipCode", "31909");
			objFisConnects = FISConn.FISConnectCustomerLookUp(customerLookUpCriteria);
			Assert.IsNotNull(objFisConnects);
			foreach (var objFisConnect in objFisConnects)
			{
				Assert.AreEqual(objFisConnect.CustomerTaxNumber, "111111111");
			}

			if (objFisConnects.Count > 0)
			{
				Console.WriteLine(" {0} FisAccounts Found in the FISConnectDB", objFisConnects.Count);
			}
			else
			{
				Console.WriteLine(" No FisAccount Found in the FISConnectDB");
			}

		}

		[Test]
		public void TestSearchCustomerBySSN()
		{
			FIS.Data.FISAccount customerprofile = null;
			customerprofile = new FISAccount();
			customerprofile = FISIO.SearchCustomerBySSN(MgiContext);
			Assert.AreEqual(customerprofile.SSN, "666014830");
		}

		/// <summary>
		/// This test method  for FetchFromFIS by CustomerLookUp US1931 
		/// </summary>
		[Test]
		public void TestFISFetchAll()
		{
			List<FIS.Data.FISAccount> customerprofiles = new List<FISAccount>();
			customerLookUpCriteria = new Dictionary<string, object>();
			customerLookUpCriteria.Add("SSN", "442770002");
			customerprofiles = FISIO.FetchAll(customerLookUpCriteria, MgiContext);
			foreach (var customerprofile in customerprofiles)
			{
				Assert.AreEqual(customerprofile.SSN, "442770002");
			}
			if (customerprofiles.Count > 0)
			{
				Console.WriteLine(" {0} FisAccounts Found in the FetchFIS", customerprofiles.Count);
			}
			else
			{
				Console.WriteLine(" No FisAccount Found in the FetchFIS");
			}


		}

		[Test]
		public void TestUpdateCustomerProfile()
		{
			FIS.Data.FISAccount customerprofile1 = null;
			customerprofile1 = new FISAccount();
			MgiContext.Context = new Dictionary<string, object>();
			MgiContext.Context.Add("SSN", "121245125");
			customerprofile1 = FISIO.SearchCustomerBySSN(MgiContext);
			//building the request object
			FIS.Data.FISAccount customerprofile = new FISAccount();
			customerprofile.FirstName = "Janet";
			customerprofile.MiddleName = "M";
			customerprofile.LastName = "NexxoTest";
			customerprofile.Address1 = "111 Anza Blvd";
			customerprofile.City = "Burlingame";
			customerprofile.State = "CA";
			customerprofile.ZipCode = "94010";
			customerprofile.Phone1 = "9740065379";
			customerprofile.PartnerAccountNumber = customerprofile1.PartnerAccountNumber;

			CustNameAddrMaintRes response = new CustNameAddrMaintRes();
			FISIO.UpdateCustomerProfile(customerprofile, MgiContext);

			////checking for the bank number need to check for other values. 
			//Assert.AreEqual(response.CISCustomerBankNum, 300);
		}

		[Test]
		public void TestCreateFISCustomer()
		{
			FIS.Data.FISAccount customer = new FISAccount();

			customer.DateOfBirth = DateTime.Parse("3/9/1984");
			customer.FirstName = "Test";
			customer.LastName = "MGI";
			customer.Address1 = "111 ANZA Blvd";
			customer.City = "Burlingame";
			customer.State = "CA";
			customer.ZipCode = "94010";
			customer.MiddleName = "M";
			customer.Phone1 = "7859657854";
			//customer.Phone2 = "9740065378";
			customer.State = "CA"; // State codes can be only 2 characters, //"SOUTH CAROLINA";
			customer.ZipCode = "94065";
			customer.SSN = "121245125";
			customer.Gender = "F";
			customer.IDCode = "S";
			customer.MothersMaidenName = "NexxoTest";
			customer.IDIssueDate = DateTime.Parse("10/10/2000");
			customer.IDExpirationDate = DateTime.Parse("10/10/2020");
			customer.BankId = "300";
			customer.BranchId = "120";
			string custCustomerNumber = FISIO.CreateFISCustomer(customer, MgiContext);

			Assert.That(custCustomerNumber,Is.Not.Null);
		}

		[Test]
		public void TestCreateFISCustomerByITIN()
		{
			FIS.Data.FISAccount customer = new FISAccount();

			customer.DateOfBirth = DateTime.Parse("3/9/1984");
			customer.FirstName = "Test";
			customer.LastName = "MGI";
			customer.Address1 = "111 ANZA Blvd";
			customer.City = "Burlingame";
			customer.State = "CA";
			customer.ZipCode = "94010";
			customer.MiddleName = "M";
			customer.Phone1 = "7859657854";
			customer.State = "CA";
			customer.ZipCode = "94065";
			customer.SSN = "998988982";
			customer.Gender = "F";
			customer.IDCode = "I";
			customer.MothersMaidenName = "NexxoTest";
			customer.IDIssueDate = DateTime.Parse("10/10/2000");
			customer.IDExpirationDate = DateTime.Parse("10/10/2020");
			customer.BankId = "300";
			customer.BranchId = "120";
			string custCustomerNumber = FISIO.CreateFISCustomer(customer, MgiContext);

			Assert.That(custCustomerNumber, Is.Not.Null);
		}

		[Test]
		public void TestCreateMiscAccount()
		{
			FISAccount baseprofile = FISIO.SearchCustomerBySSN(MgiContext);
			FIS.Data.FISAccount customerprofile = new FISAccount();

			customerprofile.FirstName = baseprofile.FirstName;
			customerprofile.MiddleName = baseprofile.MiddleName;
			customerprofile.LastName = baseprofile.LastName;
			customerprofile.Address1 = baseprofile.Address1;
			customerprofile.City = baseprofile.City;
			customerprofile.State = baseprofile.State;
			customerprofile.ZipCode = baseprofile.ZipCode;
			customerprofile.PartnerAccountNumber = baseprofile.PartnerAccountNumber;
			try
			{
				FISIO.CreateMiscAccount(customerprofile, MgiContext);
				Assert.True(true);
			}
			catch
			{
				Assert.True(false);
			}
		}


	}
}