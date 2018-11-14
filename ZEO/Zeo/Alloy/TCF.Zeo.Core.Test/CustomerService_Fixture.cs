using TCF.Zeo.Core.Impl;
using TCF.Zeo.Core.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class CustomerService_Fixture
    {
        ICustomerService custService = new ZeoCoreImpl();

        [Test]
        public void SSNTest()
        {
            ZeoContext context = new ZeoContext();
            context.SSN = "121212452";
            context.ChannelPartnerId = 34;
            context.CustomerId = 1000000000000030;
            bool isValid = custService.ValidateSSN(context);

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void CreateCustomerTest()
        {

            CustomerProfile customerProfile = new CustomerProfile();
            customerProfile.SSN = "12452166";
            customerProfile.FirstName = "Fname";
            customerProfile.LastName = "Lname";
            customerProfile.Address = new Address()
            {
                Address1 = "Address",
                Address2 = "Address",
                City = "Los Angeles",
                State = "CA",
                ZipCode = "90009"
            };
            //customerProfile.ChannelPartnerId = 34;//TCF
            customerProfile.DateOfBirth = DateTime.ParseExact("10/10/1990", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            customerProfile.MothersMaidenName = "maiden";
            customerProfile.PrimaryCountryCitizenShip = "USA";
            customerProfile.IdNumber = "110";
            customerProfile.PrimaryCountryCitizenShip = "US";
            customerProfile.SecondaryCountryCitizenShip = "US";
            customerProfile.LegalCode = "U";
            customerProfile.Gender = Gender.MALE;
            customerProfile.Phone1 = new Phone()
            {
                Number = "6504754017",
                Type = "Home",
                Provider = "AT&T"
            };
            customerProfile.PIN = "1234";
            customerProfile.ReferralCode = "Hello";
            customerProfile.SMSEnabled = false;
            customerProfile.ReceiptLanguage = "EN";
            customerProfile.Occupation = "Student";
            customerProfile.OccupationDescription = "nothing";
            customerProfile.IdType = "110";
            customerProfile.IdNumber = "s1230123";
            //customerProfile.AgentSessionId = 1000000006;
            customerProfile.DTServerCreate = DateTime.Now;
            customerProfile.DTTerminalCreate = DateTime.Now;
            customerProfile.IDCode = TaxIDCode.I;

            ZeoContext context = new ZeoContext();
            context.AgentSessionId = 1000000006;
            context.ChannelPartnerId = 34;

            long customerID = custService.InsertCustomer(customerProfile, context);

            Assert.That(customerID, Is.GreaterThan(0));
        }

        [Test]
        public void UpdateCustomerDetailsTest()
        {
            CustomerProfile customerProfile = new CustomerProfile();
            customerProfile.Id = 1000000000000140;
            customerProfile.SSN = "12452166";
            customerProfile.FirstName = "Test";
            customerProfile.LastName = "Customer";
            customerProfile.Address = new Address()
            {
                Address1 = "Address",
                Address2 = "Address",
                City = "Los Angeles",
                State = "CA",
                ZipCode = "90009"
            };
            //customerProfile.ChannelPartnerId = 34;//TCF
            customerProfile.DateOfBirth = DateTime.ParseExact("17/11/1991", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            customerProfile.MothersMaidenName = "maiden";
            customerProfile.PrimaryCountryCitizenShip = "USA";
            customerProfile.IdNumber = "110";
            customerProfile.PrimaryCountryCitizenShip = "US";
            customerProfile.SecondaryCountryCitizenShip = "US";
            customerProfile.LegalCode = "U";
            customerProfile.Gender = Gender.MALE;
            customerProfile.Phone1 = new Phone()
            {
                Number = "6504754017",
                Type = "Home",
                Provider = "AT&T"
            };
            customerProfile.PIN = "1234";
            customerProfile.ReferralCode = "Hello";
            customerProfile.SMSEnabled = false;
            customerProfile.ReceiptLanguage = "EN";
            customerProfile.Occupation = "Student";
            customerProfile.OccupationDescription = "nothing";
            customerProfile.IdType = "110";
            customerProfile.IdNumber = "s1230123";
            //customerProfile.AgentSessionId = 1000000006;
            customerProfile.DTServerLastModified = DateTime.Now;
            customerProfile.DTTerminalLastModified = DateTime.Now;
            customerProfile.IDCode = TaxIDCode.I;
            customerProfile.ClientCustomerId = "154214122";

            ZeoContext context = new ZeoContext();
            context.AgentSessionId = 1000000006;
            context.ChannelPartnerId = 34;

            bool result = custService.UpdateCustomer(customerProfile, context);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetCustomerByCustomerIdTest()
        {
            ZeoContext context = new ZeoContext();
            CustomerProfile customer = custService.GetCustomer(context);
            Assert.That(customer, Is.Not.Null);
        }

        [Test]
        public void IsValidSSNTest()
        {
            ZeoContext context = new ZeoContext();
            context.SSN = "121212452";
            context.ChannelPartnerId = 34;
            context.CustomerId = 1000000000000030;

            bool isValid = custService.ValidateSSN(context);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GetNoCustomerByCustomerIdTest()
        {
            ZeoContext context = new ZeoContext(); 
            CustomerProfile customer = custService.GetCustomer(context);
            Assert.That(customer, Is.Null);
        }


        [Test]
        public void SearchCustomerTest()
        {
            ZeoContext context = new ZeoContext();
            context.ChannelPartnerId = 34;
            CustomerSearchCriteria criteria = new CustomerSearchCriteria()
            {
                LastName = "SAKALA",
                DateOfBirth = Convert.ToDateTime("10/10/1950")
            };

            var customers = custService.SearchCustomer(criteria, context);
            Assert.IsTrue(customers.Count > 0);
        }


        [Test]
        public void CreateCustomerSessionTest()
        {
            ZeoContext context = new ZeoContext();
            CustomerSession customerSession = new CustomerSession()
            {
                //AgentSessionId = 1000000006
                //, ChannelPartnerId = 34
                CustomerId = 1000000000000010
                ,
                TimezoneID = "CA"
                ,
                CardPresent = false
                ,
                DTServerCreate = DateTime.Now
                ,
                DTTerminalCreate = DateTime.Now
            };
            var custSession = custService.CreateCustomerSession(customerSession, context);
            Assert.IsNotNull(custSession);
            CustomerSession coreCustomerSession = custService.CreateCustomerSession(customerSession, context);
            Assert.IsTrue(coreCustomerSession.Id > 0);
        }

    }
}
