using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Unit.Test.MockData;
using MGI.Core.Partner.Contract;
using MGI.Biz.Customer.Contract;
using BizCustomerData = MGI.Biz.Customer.Data;
using CxnCustomerData = MGI.Cxn.Customer.Data;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Impl;
using MGI.Cxn.Customer.TCIS.Impl;
using MGI.Cxn.Customer.TCIS.Data;
using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Common.DataAccess.Data;


namespace MGI.Biz.TCF.Test
{
    [TestFixture]
    public class CustomerTest
    {
        MGI.Biz.TCF.Impl.Customer impl;
        private void Setup()
        {
            impl = new Impl.Customer();

            impl.CxnClientCustomerService = new MockClientCustomerServiceTCF();

            var nxoIdPtnr = new NexxoDataStructuresServiceImpl();
            var idtyperepo = new MockNexxoModelRepo<MGI.Core.Partner.Data.ChannelPartnerIdTypesMapping>();
            var cpIdtypemap = new MGI.Core.Partner.Data.ChannelPartnerIdTypesMapping();
            idtyperepo.Add(cpIdtypemap);
            nxoIdPtnr.IdTypeRepo = idtyperepo;
            var cpMcntryrepo = new MockNexxoModelRepo<MGI.Core.Partner.Data.ChannelPartnerMasterCountryMapping>();
            var country1 = new MGI.Core.Partner.Data.ChannelPartnerMasterCountryMapping();
            cpMcntryrepo.Add(country1);
            nxoIdPtnr.MasterCountryRepo = cpMcntryrepo;
            var occupationrepo = new MockNexxoModelRepo<MGI.Core.Partner.Data.Occupation>();
            var occ = new MGI.Core.Partner.Data.Occupation();
            occupationrepo.Add(occ);
            nxoIdPtnr.OccupationRepo = occupationrepo;
            var countryrepo=new MockNexxoModelRepo<MGI.Core.Partner.Data.Country>();
            var con = new MGI.Core.Partner.Data.Country() { Name = "UNITED STATES", Code=840 };
            countryrepo.Add(con);
            nxoIdPtnr.CountryRepo = countryrepo;
            var staterepo = new MockNexxoModelRepo<MGI.Core.Partner.Data.State>();
            var st = new MGI.Core.Partner.Data.State() { Name = "CALIFORNIA", CountryCode = "840" };
            staterepo.Add(st);
            nxoIdPtnr.StateRepo = staterepo;
            impl.PTNRIdTypeService = nxoIdPtnr;

        }
        [Test]
        public void FetchAllTest()
        {
            Setup();

            var cxncontext = new Dictionary<string, object>();
            cxncontext.Add("ChannelPartnerId", 34);
            
            var customerLookUpcriteria = new Dictionary<string, object>();

            var cust = impl.FetchAll(12, customerLookUpcriteria, cxncontext);

            Assert.NotNull(cust);
            Assert.Greater(cust.Count,0);
        }
        [Test]
        public void ValidateCustomerRequiredFields()
        {
            Setup();
            var context = new Dictionary<string, object>();
            MGI.Biz.Customer.Data.CustomerProfile customerProfile = new MGI.Biz.Customer.Data.CustomerProfile()
            {
                FirstName = "STEVE",
                MiddleName = "",
                LastName = "JOBS",
                LastName2 = "",
                MothersMaidenName = "MAIDEN NAME",
                DateOfBirth = new System.DateTime(1956, 10, 10),
                Address1 = "111 anza Blvd",
                Address2 = "Suite 200",
                City = "Burlingame",
                State = "CA",
                ZipCode = "94010",
                Phone1 = "2345678976",
                Phone1Type = "Home",
                Phone1Provider = "AT&T",
                Phone2 = "",
                Phone2Type = "",
                Phone2Provider = "",
                Email = "",
                SSN = "731486025",
                TaxpayerId = "",
                DoNotCall = true,
                SMSEnabled = false,
                MarketingSMSEnabled = false,
                ChannelPartnerId = 1,
                Gender = "MALE",
                MailingAddressDifferent = false,
                MailingAddress1 = "",
                MailingAddress2 = "",
                MailingCity = "",
                MailingState = "",
                MailingZipCode = "",
                IsPartnerAccountHolder = true,
                ReferralCode = "",
                PIN = "7777",
                CardNumber = "",
                AccountNumber = "",
                ReceiptLanguage = "",
                ProfileStatus = MGI.Common.Util.ProfileStatus.Active,
                Resolution = "",
                FraudScore = 0,
                PartnerAccountNumber = "",
                RelationshipAccountNumber = "",
                ProgramId = "",
                BankId = "",
                BranchId = "",
                GovernmentIDType = "DRIVER'S LICENSE",
                // GovernmentId = "K9482601",
                IDIssuingCountry = "UNITED STATES",
                IDIssuingCountryId = "1",
                IDIssuingState = "CALIFORNIA",
                IDIssuingStateAbbr = "",
                IDIssueDate = new System.DateTime(2010, 10, 10),
                IDExpirationDate = new System.DateTime(001, 1, 1),
                CIN = 0,
                ClientID = "00000318896",
                LegalCode = "444",
                PrimaryCountryCitizenship = "US",
                SecondaryCountryCitizenship = "",
                Notes = "",
                Occupation = "ENGINEER",
                OccupationDescription = "",
                Employer = "NEXXO",
                EmployerPhone = "2342342345"
            };

            MGI.Biz.Customer.Data.Identification identification = new MGI.Biz.Customer.Data.Identification()
            {
                Country = "UNITED STATES",
                CountryOfBirth = "US",
                IDType = "DRIVER'S LICENSE",
                State = "CALIFORNIA",
                GovernmentId = "K9482601",//
                IssueDate = new System.DateTime(2010, 10, 10),
                ExpirationDate = new System.DateTime(2020, 10, 10),
                IDTypeName = "DRIVER'S LICENSE"
            };

            MGI.Biz.Customer.Data.EmploymentDetails emptDetails = new MGI.Biz.Customer.Data.EmploymentDetails()
            {
                Occupation = "ENGINEER",
                OccupationDescription = "",
                Employer = "NEXXO",
                EmployerPhone = "2342342345"
            };

            List<string> groups = new List<string>() { "MO Discount" };


            MGI.Biz.Customer.Data.Customer custDTO = new MGI.Biz.Customer.Data.Customer()
            {
                Profile = customerProfile,
                ID = identification,
                Employment = emptDetails,
                Groups = groups
            };
            bool valReqfeilds = impl.ValidateCustomerRequiredFields(12,custDTO,context);
            Assert.IsTrue(valReqfeilds);

        }

    }
}
