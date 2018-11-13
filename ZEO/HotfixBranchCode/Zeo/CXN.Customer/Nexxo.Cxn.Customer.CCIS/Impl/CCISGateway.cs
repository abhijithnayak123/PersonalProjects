using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.Customer.CCIS.Data;
using MGI.Cxn.Customer.Contract;
using CCISConnectData = MGI.Cxn.Customer.CCIS.Data.CCISConnect;
using CxnCustomerData = MGI.Cxn.Customer.Data.CustomerProfile;
using MGI.Common.Util;
namespace MGI.Cxn.Customer.CCIS.Impl
{
    public class CCISGateway : IClientCustomerService
    {
        #region Public Properties

        public ProcessorDAL CCISProcessorDAL { get; set; }
		public IRepository<CCISAccount> CCISAccountRepo { private get; set; }

        #endregion


        CCISGateway()
        {
          #region Mapping 
            AutoMapper.Mapper.CreateMap<CCISConnectData, CxnCustomerData>()
            .ForMember(x => x.Phone1, s => s.MapFrom(c => c.PrimaryPhoneNumber))
            .ForMember(x => x.Phone2, s => s.MapFrom(c => c.SecondaryPhone ))
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
        }
       
        #region IClientCustomerService

        #region CCISProcessorImplementation
        /// <summary>
        /// Get the Customers by customerLookUpCriteria
        /// </summary>
        /// <param name="customerLookUpCriteria"></param>
        /// <param name="cxnContext"></param>
        /// <returns></returns>
        public List<Customer.Data.CustomerProfile> FetchAll(Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
        {
            List<CCISConnectData> ccisConnectList = CCISProcessorDAL.LookUp(customerLookUpCriteria, cxnContext);

            List<CxnCustomerData> cxnCustomerProfileList = AutoMapper.Mapper.Map<List<CCISConnectData>, List<CxnCustomerData>>(ccisConnectList);

            return cxnCustomerProfileList;
        }

        public void ValidateCustomerStatus(long CXNId, MGIContext context)
        {
            return;

        }

        /// <summary>
        /// Implemented Method  to get the ClientProfileStatus
        /// </summary>
        /// <param name="cxnAccountId"></param>
        /// <param name="context"></param>
        /// <returns>the status of Customer Profile status</returns>
		public ProfileStatus GetClientProfileStatus(long cxnAccountId, MGIContext context)
        {
			ProfileStatus status = ProfileStatus.Inactive;
            status = CCISProcessorDAL.GetClientProfileStatus(cxnAccountId, context);
            return status;                    
        }

        #endregion

        public long Add(Customer.Data.CustomerProfile customer, MGIContext mgiContext)
        {
            throw new NotImplementedException();
        }

        public void Update(string id, Customer.Data.CustomerProfile customer, MGIContext mgiContext)
        {
			CCISProcessorDAL.UpdateCxnAccount(id, customer, mgiContext);
        }

        public long AddAccount(Customer.Data.CustomerProfile account, MGIContext mgiContext)
        {
            throw new NotImplementedException();
        }

       public Customer.Data.CustomerProfile Fetch(MGIContext search)
        {
            throw new NotImplementedException();
        }

        public long AddCXNAccount(Customer.Data.CustomerProfile customer, MGIContext mgiContext)
        {
			Mapper.CreateMap<CxnCustomerData, CCISAccount>();				

            CCISAccount ccisAccountDetails = Mapper.Map<CxnCustomerData, CCISAccount>(customer);

			ccisAccountDetails.ProfileStatus = ProfileStatus.Active;

			long accountId = CCISProcessorDAL.AddAccount(ccisAccountDetails, mgiContext);

            if (accountId <= 0)
                throw new ClientCustomerException(ClientCustomerException.CREATE_ACCOUNT_FAILED, "Could not create account for the customer");

            return accountId;
        }

        /// <summary>
        /// Implemented Method  to get the ClientCustomerId
        /// </summary>
        /// <param name="cxnAccountId"></param>
        /// <param name="mgiContext"></param>
        /// <returns>Client Id </returns>
		public string GetClientCustID(long cxnAccountId, MGIContext mgiContext)
		{
           string clientId = CCISProcessorDAL.GetClientCustID(cxnAccountId, mgiContext);
           return clientId;
		}

		public bool GetCustInd(long cxnAccountId, MGIContext context)
		{
			throw new NotImplementedException();
		}
        #endregion

    }
}
