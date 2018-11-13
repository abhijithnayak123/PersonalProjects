using System.Collections.Generic;
using NUnit.Framework;
using MGI.Common.Util;
using MGI.Unit.Test;
using System;

namespace MGI.Biz.TCF.Test
{
    [TestFixture]
    public class CustomerTest : BaseClass_Fixture
    {
		public MGI.Biz.TCF.Impl.Customer TCFClientCustomerService { get; set; }

		[Test]
		public void Can_Fetch_Customer()
		{
			long agentSessionId = 1000000001;
			Dictionary<string, object> searchContext = new Dictionary<string, object>() { };
			MGIContext mgiContext = new MGIContext() { };

			List<Customer.Data.Customer> customers = TCFClientCustomerService.FetchAll(agentSessionId, searchContext, mgiContext);

			Assert.AreNotEqual(customers.Count, 0);
		}

		[Test]
		public void Can_Get_Client_Profile_Status()
		{
			long agentSessionId = 1000000001;
			long trxnId = 1000000001;
			MGIContext mgiContext = new MGIContext() { };

			ProfileStatus status = TCFClientCustomerService.GetClientProfileStatus(agentSessionId, trxnId, mgiContext);

			Assert.AreEqual(status, ProfileStatus.Active);
		}

		[Test]
		public void Can_Validate_Customer_Field()
		{
			long agentSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { };

			Customer.Data.Customer customer = new Customer.Data.Customer() 
			{
				Profile = new Customer.Data.CustomerProfile(),
				ID = new Customer.Data.Identification(),
				Employment = new Customer.Data.EmploymentDetails(),
				Groups = new List<string>()
			};

			bool status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.FirstName = "Nitish";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.LastName = "Biradar";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.MothersMaidenName = "Testing";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.Gender = "Male";
			customer.Profile.Phone1 = string.Empty;
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.Phone1 = "1594887591";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.Phone1Type = "CELL";
			customer.Profile.Phone1Provider = "Test";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.Phone2 = "1594878591";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.Phone1Type = "Home";
			customer.Profile.Phone1Provider = "";
			customer.Profile.Phone2Type = "CELL";
			customer.Profile.Phone2Provider = "Test";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.Phone2Type = "";
			customer.Profile.Phone2Provider = "";
			customer.Profile.Address1 = "Testing";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.City = "Test";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.ZipCode = "12345";
			customer.Profile.MailingAddressDifferent = true;
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.MailingAddress1 = "Test";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.MailingState = "State";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.MailingCity = "City";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.MailingZipCode = "14598";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.ID.CountryOfBirth = "United States";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.DateOfBirth = new DateTime(1990, 10, 10);
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.ID.Country = "United States";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.ID.IDType = "DRIVER'S LICENSE";
			customer.ID.State = "CA";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.ID.IDType = "U.S. STATE IDENTITY CARD";
			customer.ID.State = "";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.ID.State = "CA";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.ID.GovernmentId = "Test";
			customer.ID.ExpirationDate = new DateTime(2020, 10, 10);
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.LegalCode = "15948";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.PrimaryCountryCitizenship = "United States";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Employment.Occupation = "Student";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.IsFalse(status);

			customer.Profile.PIN = "12340";
			customer.Profile.Phone1Type = "Home";
			customer.Profile.Phone2Type = "Home";
			status = TCFClientCustomerService.ValidateCustomerRequiredFields(agentSessionId, customer, mgiContext);
			Assert.True(status);
		}
    }
}
