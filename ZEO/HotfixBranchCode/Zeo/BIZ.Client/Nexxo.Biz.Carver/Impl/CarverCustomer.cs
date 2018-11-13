using AutoMapper;
using MGI.Biz.Customer.Contract;
using MGI.Biz.Customer.Data;
using MGI.Cxn.Common.Processor.Contract;
using MGI.Cxn.Customer.Contract;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CxnCustomerData = MGI.Cxn.Customer.Data;
using CCISConnectData = MGI.Cxn.Customer.CCIS.Data;
using BizCustomerProfile = MGI.Biz.Customer.Data;


namespace MGI.Biz.Carver.Impl
{
	public class CarverCustomer : ICustomerRepository, IProcessor
    {
        public IClientCustomerService CxnClientCustomerService { private get; set; }

        public CarverCustomer()
        {
			Mapper.CreateMap<CxnCustomerData.CustomerProfile, BizCustomerProfile.CustomerProfile>()
				.ForMember(x => x.ClientID,  o => o.MapFrom(s => s.PartnerAccountNumber))
				.ForMember(x => x.PartnerAccountNumber, opt => opt.Ignore());
        }

        #region ICustomerRepository Implementation

        /// <summary>
        /// Search the customers by DOB,PhoneNumber,Zipcode,LastName from Carver
        /// </summary>
        /// <param name="context"> The context should have the following:
        /// 1. ChannelPartnerId
        /// 2. BankId
        /// </param>
        /// <returns></returns>
		public List<BizCustomerProfile.Customer> FetchAll(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
        {
            List<CxnCustomerData.CustomerProfile> customerProfiles = null;
            customerProfiles = CxnClientCustomerService.FetchAll(customerLookUpCriteria, cxnContext);
			List<BizCustomerProfile.Customer> customers = new List<BizCustomerProfile.Customer>();

            if (customerProfiles != null && customerProfiles.Count > 0)
            {
                foreach (var customerProfile in customerProfiles)
                {
					BizCustomerProfile.Customer mappingRecord = new BizCustomerProfile.Customer();
					mappingRecord.Profile = Mapper.Map<CxnCustomerData.CustomerProfile, BizCustomerProfile.CustomerProfile>(customerProfile);
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
        /// Validate Customer against CCIS
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

        public bool ValidateCustomerRequiredFields(long agentSessionId, BizCustomerProfile.Customer customer, MGIContext context)
        {
            return false;
        }
        #endregion
    }
}
