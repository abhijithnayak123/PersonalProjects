using AutoMapper;
using MGI.Biz.Customer.Contract;
using MGI.Biz.Customer.Data;
using MGI.Cxn.Common.Processor.Contract;
using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.FIS.Contract;
using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CxnCustomerData = MGI.Cxn.Customer.Data;
using FISConnectData = MGI.Cxn.Customer.FIS.Data;
using BizCustomerProfile = MGI.Biz.Customer.Data;
using MGI.Common.Util;


namespace MGI.Biz.Synovus.Impl
{
	public class SynovusCustomer : ICustomerRepository, IProcessor
    {
        CustomerProfile CustomerProfileRepo { get; set; }
        //private DbProvider _dbProvider; // Need To Remove
        public NLoggerCommon NLogger { get; set; }
        //public DbProvider DBProvider { set { _dbProvider = value; } }

        public IClientCustomerService CxnClientCustomerService { private get; set; }

        // Added SQL Injection. US#1789
        public IFISConnect CxnFISCustomer { private get; set; }
        // Added for SQL Injection Complete. US#1789


        public SynovusCustomer()
        {
            Mapper.CreateMap<CxnCustomerData.CustomerProfile, BizCustomerProfile.CustomerProfile>();
             #region Mapping FisConnect Data to Account Data (SQL Injection User Story US#1789)
            // Mapping FisConnect Data to Account Data to return Account Type - User Story # 1789.
            AutoMapper.Mapper.CreateMap<FISConnectData.FISConnect, CxnCustomerData.CustomerProfile>()
            .ForMember(x => x.Phone1, s => s.MapFrom(c => c.PrimaryPhoneNumber))
            .ForMember(x => x.Phone2, s => s.MapFrom(c => c.Secondaryphone))
            .ForMember(x => x.SSN, s => s.MapFrom(c => c.CustomerTaxNumber))
            .ForMember(x => x.LastName, s => s.MapFrom(c => c.LastName))
            .ForMember(x => x.FirstName, s => s.MapFrom(c => c.FirstName))
            .ForMember(x => x.MiddleName, s => s.MapFrom(c => c.MiddleName))
            .ForMember(x => x.Address1, s => s.MapFrom(c => c.AddressStreet))
            .ForMember(x => x.City, s => s.MapFrom(c => c.AddressCity))
            .ForMember(x => x.State, s => s.MapFrom(c => c.AddressState))
            .ForMember(x => x.ZipCode, s => s.MapFrom(c => c.ZipCode))
			.ForMember(x => x.DateOfBirth, s => s.MapFrom(c => c.DateOfBirth))
            .ForMember(x => x.GovernmentId, s => s.MapFrom(c => c.DriversLicenseNumber))
            .ForMember(x => x.MothersMaidenName, s => s.MapFrom(c => c.MothersMaidenName))
            .ForMember(x => x.Gender, s => s.MapFrom(c => c.Gender))
            .ForMember(x => x.MothersMaidenName, s => s.MapFrom(c => c.MothersMaidenName))
            .ForMember(x => x.BankId, s => s.MapFrom(c => c.MetBankNumber))
            .ForMember(x => x.PartnerAccountNumber, s => s.MapFrom(c => c.CustomerNumber))
            .ForMember(x => x.RelationshipAccountNumber, s => s.MapFrom(c => c.ExternalKey))
            .ForMember(x => x.ProgramId, s => s.MapFrom(c => c.ProgramId));
            #endregion
        }
        #region ICustomerRepository Implementation
   	
        /// <summary>
        /// Search the customers by SSN,PhoneNumber,Zipcode from Synovus
        /// </summary>
        /// <param name="context"> The context should have the following:
        /// 1. ChannelPartnerId
        /// 2. BankId
        /// </param>
        /// <returns></returns>
		public List<BizCustomerProfile.Customer> FetchAll(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
        {
            List<CxnCustomerData.CustomerProfile> customerProfiles = GetCustomers(agentSessionId, customerLookUpCriteria, cxnContext);
			List<BizCustomerProfile.Customer> customers = new List<BizCustomerProfile.Customer>();

            if (customerProfiles != null && customerProfiles.Count > 0)
            {
                foreach (var customerProfile in customerProfiles)
                {
					BizCustomerProfile.Customer mappingRecord = new BizCustomerProfile.Customer();
					mappingRecord.Profile = Mapper.Map<CxnCustomerData.CustomerProfile, BizCustomerProfile.CustomerProfile>(customerProfile);
                    mappingRecord.Profile.PartnerAccountNumber = customerProfile.PartnerAccountNumber;
					mappingRecord.ID = new Identification()
                                           {
                                               GovernmentId = customerProfile.GovernmentId,
                                               IDType = customerProfile.GovernmentIDType,
                                               Country = customerProfile.IDIssuingCountry,
                                               State = customerProfile.IDIssuingState,
                                               IssueDate = customerProfile.IDIssueDate,
                                               ExpirationDate = customerProfile.IDExpirationDate,

                                           };
                    mappingRecord.Employment = new EmploymentDetails()
                                                   {
                                                       Occupation = customerProfile.Occupation,
                                                       Employer = customerProfile.EmployerName
                                                   };

                    customers.Add(mappingRecord);
                }
                
            }
            else
            {
				customers = new List<BizCustomerProfile.Customer>();
            }
            return customers;
        }

        /// <summary>
        /// Validate Customer against FIS
        /// </summary>
        /// <param name="SSN"></param>
        /// <param name="context"></param>
        public void ValidateCustomerStatus(long agentSessionId, long CXNId, MGIContext context)
        {
            CxnClientCustomerService.ValidateCustomerStatus(CXNId, context);
        }

        /// <summary>
        /// Get Client Profile status
        /// </summary>
        /// <param name="cxnAccountId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
		public ProfileStatus GetClientProfileStatus(long agentSessionId, long cxnAccountId, MGIContext context)
        {
            return CxnClientCustomerService.GetClientProfileStatus(cxnAccountId, context);
        }
        #endregion
        /// <summary>
        /// Get Customers from FIS
        /// </summary>
        /// <param name="customerLookUpCriteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<CxnCustomerData.CustomerProfile> GetCustomers(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
        {
            List<CxnCustomerData.CustomerProfile> _customerProfiles = null;
            _customerProfiles = FetchListFromConnectsDB(customerLookUpCriteria, cxnContext);
            
            if (_customerProfiles.Count <=0)
            {
                List<CxnCustomerData.CustomerProfile> customerProfiles = FetchListFromFIS(customerLookUpCriteria, cxnContext);
                return customerProfiles;

            }
            return _customerProfiles;
        }

        /// <summary>
        ///  get the customer profiles data from FIS using FISIO
        /// </summary>
        /// <param name="customerLookUpCriteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<CxnCustomerData.CustomerProfile> FetchListFromFIS(Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
        {
            List<CxnCustomerData.CustomerProfile> customerprofiles = CxnClientCustomerService.FetchAll(customerLookUpCriteria, cxnContext);
            return customerprofiles;
        }
       
        /// <summary>
        ///  get the customer profiles data from FIS ConnectDB  table in NexxoDB
        /// </summary>
        /// <param name="customerLookUpCriteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<CxnCustomerData.CustomerProfile> FetchListFromConnectsDB(Dictionary<string, object> customerLookUpCriteria, MGIContext context)
        {
           
          try
            {
              
                List<FISConnectData.FISConnect> objConnects = CxnFISCustomer.FISConnectCustomerLookUp(customerLookUpCriteria);
                List<CxnCustomerData.CustomerProfile> objCustomerProfiles = AutoMapper.Mapper.Map<List<FISConnectData.FISConnect>, List<CxnCustomerData.CustomerProfile>>(objConnects);

               return objCustomerProfiles;
            }
            catch (Exception ex)
            {
                NLogger.Error(string.Format("Error while fetching data from ConnectsDB Database") + ex.Message);
                throw new Exception("Error while fetching data from ConnectsDB Database",ex);
            }
        }

        private CxnCustomerData.CustomerProfile getCustomer(MGIContext searchParam)
        {
            CxnCustomerData.CustomerProfile _customerProfile = null;

            _customerProfile = fetchFromConnectsDB(searchParam);

            if (_customerProfile == null)
            {
                CxnCustomerData.CustomerProfile customerprofile = fetchFromFIS(searchParam);
                return customerprofile;
            }
            return _customerProfile;
        }

        // get the customer profile data from FIS using FISIO
        private CxnCustomerData.CustomerProfile fetchFromFIS(MGIContext searchParam)
        {
            CxnCustomerData.CustomerProfile customerprofiles = CxnClientCustomerService.Fetch(searchParam);

            return customerprofiles;
        }

        // get the customer profile data from ConnectsDB table in NexxoDB
        private CxnCustomerData.CustomerProfile fetchFromConnectsDB(MGIContext searchParam)
        {
            string ssn = searchParam.Context["SSN"].ToString();
            //AdoTemplate adoTemplate = new AdoTemplate(_dbProvider);

            try
            {
                #region Code Changes for SQL Injection US#1789

                FISConnectData.FISConnect objConnect = CxnFISCustomer.GetSSNForCustomer(ssn);
                CxnCustomerData.CustomerProfile objAccount = AutoMapper.Mapper.Map<FISConnectData.FISConnect, CxnCustomerData.CustomerProfile>(objConnect);

                #endregion

                #region Old Code SQL.

                //TODO: Get the actual table name later 
                //string sql = string.Format("Select * from tFISConnectsDb Where CustomerTaxNumber = '{0}'", ssn);

                // This code has been removed for SQL Injection User Story. US#1789
                //IList<CxnCustomerData.Account> customers = adoTemplate.QueryWithRowMapper(System.Data.CommandType.Text, sql, new CustomerRowMapper<CxnCustomerData.Account>());

                //if (customers.Any())
                //	return customers.FirstOrDefault();
                //else
                //	return null; 
                #endregion

                return objAccount;
            }
            catch (Exception ex)
            {                
                NLogger.Error(string.Format("Error while fetching data from ConnectsDB Database") + ex.Message);
                throw new Exception("Error while fetching data from ConnectsDB Database",ex);
            }

        }

       private void IList<T1>(FISConnectData.FISConnect objConnect)
		{
			throw new NotImplementedException();
		}

       public bool ValidateCustomerRequiredFields(long agentSessionId, BizCustomerProfile.Customer customer, MGIContext context)
       {
           return false;
       }
    }
}
