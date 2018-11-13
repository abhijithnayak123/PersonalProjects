using MGI.Cxn.Customer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Impl;
//using MGI.Cxn.Customer.Contract;
using CxnCustomerData = MGI.Cxn.Customer.Data;
using CCISConnectData = MGI.Cxn.Customer.CCIS.Data;
//using CCISConnectData = MGI.Cxn.Customer.CCIS.Data.CCISConnect;
//using CxnCustomerData = MGI.Cxn.Customer.Data.CustomerProfile;
using MGI.Common.DataAccess.Contract;
using AutoMapper;
using MGI.Common.Util;
using MGI.Cxn.Customer.CCIS.Impl;
using BizCustomerProfile = MGI.Biz.Customer.Data;

namespace MGI.Unit.Test.MockData
{
    class MockClientCustomerServiceCarver : IClientCustomerService
    {
        List<Cxn.Customer.Data.CustomerProfile> cstmrs = new List<Cxn.Customer.Data.CustomerProfile>();
        
        public MockClientCustomerServiceCarver()
        {
            #region Mapping
            AutoMapper.Mapper.CreateMap<CCISConnectData.CCISConnect, CxnCustomerData.CustomerProfile>()
            .ForMember(x => x.Phone1, s => s.MapFrom(c => c.PrimaryPhoneNumber))
            .ForMember(x => x.Phone2, s => s.MapFrom(c => c.SecondaryPhone))
            .ForMember(x => x.SSN, s => s.MapFrom(c => c.CustomerTaxNumber))
            .ForMember(x => x.LastName, s => s.MapFrom(c => c.LastName))
            .ForMember(x => x.FirstName, s => s.MapFrom(c => c.FirstName))
            .ForMember(x => x.MiddleName, s => s.MapFrom(c => c.MiddleName))
                // .ForMember(x => x., s => s.MapFrom(c => c.MiddleName2))
            .ForMember(x => x.Address1, s => s.MapFrom(c => c.AddressStreet))
            .ForMember(x => x.City, s => s.MapFrom(c => c.AddressCity))
            .ForMember(x => x.State, s => s.MapFrom(c => c.AddressState))
            .ForMember(x => x.ZipCode, s => s.MapFrom(c => c.ZipCode))
            .ForMember(x => x.DateOfBirth, s => s.MapFrom(c => c.DateOfBirth))
            .ForMember(x => x.MothersMaidenName, s => s.MapFrom(c => c.MothersMaidenName))
            .ForMember(x => x.Gender, s => s.MapFrom(c => c.Gender))
            .ForMember(x => x.GovernmentId, s => s.MapFrom(c => c.DriversLicenseNumber))
            .ForMember(x => x.BankId, s => s.MapFrom(c => c.MetBankNumber))
            .ForMember(x => x.PartnerAccountNumber, s => s.MapFrom(c => c.CustomerNumber))
            .ForMember(x => x.RelationshipAccountNumber, s => s.MapFrom(c => c.ExternalKey))
            .ForMember(x => x.ProgramId, s => s.MapFrom(c => c.ProgramId));
            #endregion
            Mapper.CreateMap<CxnCustomerData.CustomerProfile, BizCustomerProfile.CustomerProfile>()
                .ForMember(x => x.ClientID, o => o.MapFrom(s => s.PartnerAccountNumber))
                .ForMember(x => x.PartnerAccountNumber, opt => opt.Ignore());

        }
        public Cxn.Customer.Data.CustomerProfile Fetch(Dictionary<string, object> search)
        {
            throw new NotImplementedException();
        }

        public List<Cxn.Customer.Data.CustomerProfile> FetchAll(Dictionary<string, object> customerLookUpCriteria, Dictionary<string, object> cxnContext)
        {
           // List<MGI.Cxn.Customer.CCIS.Data.CCISConnect> ccisConnectList = new List<MGI.Cxn.Customer.CCIS.Data.CCISConnect>();
            List<CCISConnectData.CCISConnect> ccisConList = new List<CCISConnectData.CCISConnect>();//here avoided namespace used like a type' error
            CCISConnectData.CCISConnect Cconnect = new Cxn.Customer.CCIS.Data.CCISConnect()
            {
                CustomerNumber = "00044700",
                CustomerTaxNumber = "741741123",
                PrimaryPhoneNumber = "2124567895",
                SecondaryPhone="",
                LastName = "BLANEY",
                FirstName = "AMANDA",
                MiddleName="",
                MiddleName2="",
                AddressStreet = "14410 125TH ST",
                AddressCity = "NEW YORK",
                AddressState = "NY",
                ZipCode = "10035",
                DateOfBirth = new System.DateTime(1974, 01, 30),
                MothersMaidenName = "CANDYSONA",
                DriversLicenseNumber = "789753789",
                ExternalKey="",
                MetBankNumber="",
                ProgramId="",
                Gender="Male"
            };
            ccisConList.Add(Cconnect);
            List<CxnCustomerData.CustomerProfile> cxnCustomerProfileList = AutoMapper.Mapper.Map<List<CCISConnectData.CCISConnect>, List<CxnCustomerData.CustomerProfile>>(ccisConList);
            return cxnCustomerProfileList;      
        }

        public long Add(Cxn.Customer.Data.CustomerProfile customer, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public void Update(string id, Cxn.Customer.Data.CustomerProfile customer, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public long AddAccount(Cxn.Customer.Data.CustomerProfile account, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public void ValidateCustomerStatus(long CXNId, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public long AddCXNAccount(Cxn.Customer.Data.CustomerProfile customer, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public Common.Util.ProfileStatus GetClientProfileStatus(long cxnAccountId, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public string GetClientCustID(long cxnAccountId, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public bool GetCustInd(long cxnAccountId, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }
    }
}
