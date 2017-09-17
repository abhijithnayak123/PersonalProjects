using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.Customer.CCIS.Data;
using MGI.Cxn.Customer.Contract;
using CCISConnectData = MGI.Cxn.Customer.CCIS.Data.CCISConnect;
using MGI.Common.Util;

namespace MGI.Cxn.Customer.CCIS.Impl
{
   public class ProcessorDAL
    {
        #region Repositories
        public IRepository<Data.CCISConnect> CCISConnectRepo { private get; set; }
        public IRepository<Data.CCISAccount> CCISAccountRepo { private get; set; }
        #endregion
		
        #region ProcessorDAL Public Methods
        public NLoggerCommon NLogger { get; set; }
       /// <summary>
        /// Get Cutomers form CCISConnectDB
       /// </summary>
       /// <param name="customerLookUpCriteria"></param>
       /// <param name="cxnContext"></param>
       /// <returns></returns>
        public List<CCISConnectData> LookUp(Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
        {
            DateTime? dOB = null;
            string phoneNumber = "", zipCode = "", lastName = "";

			if (customerLookUpCriteria.ContainsKey("DateOfBirth")) dOB = Convert.ToDateTime(customerLookUpCriteria["DateOfBirth"]);
            if (customerLookUpCriteria.ContainsKey("PhoneNumber")) phoneNumber = Convert.ToString(customerLookUpCriteria["PhoneNumber"]);
            if (customerLookUpCriteria.ContainsKey("ZipCode")) zipCode = Convert.ToString(customerLookUpCriteria["ZipCode"]);
            if (customerLookUpCriteria.ContainsKey("LastName")) lastName = Convert.ToString(customerLookUpCriteria["LastName"]);

            try
            {
                NLogger.Info(string.Format("Start - CCIS LookUp for SQL to LINQ-nHibernate"));
				List<CCISConnectData> ccisConnectList = CCISConnectRepo.FilterBy(t => ((!dOB.HasValue) || t.DateOfBirth == dOB)
                    && (string.IsNullOrEmpty(phoneNumber) || t.PrimaryPhoneNumber == phoneNumber)
                    && (string.IsNullOrEmpty(zipCode) || t.ZipCode == zipCode)
                    && (string.IsNullOrEmpty(lastName) || t.LastName == lastName)
                    ).ToList();
                NLogger.Info(string.Format("End - CCIS LookUp for SQL to LINQ-nHibernate"));
                return ccisConnectList;
            }
            catch(Exception ex) 
            {
                throw new ClientCustomerException(ClientCustomerException.CCIS_LOOKUP_ERROR, "",ex);
            }
        }
       
        public long AddAccount(CCISAccount customer, MGIContext mgiContext)
        {
            NLogger.Info("CCIS AddAccount Start");
			customer.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
            customer.DTServerCreate = DateTime.Now;
            try
            {
                CCISAccountRepo.AddWithFlush(customer);
                
                if (customer.Id <= 0)
                 throw new ClientCustomerException(ClientCustomerException.CREATE_ACCOUNT_FAILED, "");
            }
            catch(ClientCustomerException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                NLogger.Error(String.Format("Exception: {0}, Trace : {1}", ex.Message, ex.StackTrace));
                throw new ClientCustomerException(ClientCustomerException.CREATE_ACCOUNT_FAILED, "Could not create account for the customer",ex);
            }
            NLogger.Info("CCIS AddAccount End");
            return customer.Id;
        }

		public void UpdateCxnAccount(string cxnAccountId, Customer.Data.CustomerProfile account, MGIContext mgiContext)
		{
			// Get the CCIS account details to be updated from database
			CCISAccount ccisAccountDetails = CCISAccountRepo.FindBy(x => x.Id == Convert.ToInt64(cxnAccountId));

			Mapper.CreateMap<Customer.Data.CustomerProfile, CCISAccount>()
				 .ForMember(x => x.PartnerAccountNumber, opt => opt.Ignore())
				 .ForMember(x => x.RelationshipAccountNumber, opt => opt.Ignore());

			// Map the edited customer profile values to CCIS account details
			ccisAccountDetails = Mapper.Map<Customer.Data.CustomerProfile, CCISAccount>(account, ccisAccountDetails);

			ccisAccountDetails.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			ccisAccountDetails.DTServerLastModified =DateTime.Now;
			// Call Repository update method to push changes
			CCISAccountRepo.UpdateWithFlush(ccisAccountDetails);
		}

        #endregion

        #region Private region
        private object GetDictionaryValue(Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key) == false)
                throw new Exception(String.Format("{0} not provided in dictionary", key));
            return dictionary[key];
        }

       /// <summary>
        /// Implemented Method to get the Client Profile Status fron database
       /// </summary>
       /// <param name="cxnAccountId"></param>
       /// <param name="context"></param>
       /// <returns>Status of the Customer Profile</returns>
		public ProfileStatus GetClientProfileStatus(long cxnAccountId, MGIContext context)
        {
            //Call Repository to get the CCISAccount details
            CCISAccount account = CCISAccountRepo.FindBy(x => x.Id == cxnAccountId);
			ProfileStatus status = (account == null) ? ProfileStatus.Inactive : account.ProfileStatus;
            return status;
        }
        /// <summary>
        /// Implemented Method to get the ClientCustomerId fron database
        /// </summary>
        /// <param name="cxnAccountId"></param>
        /// <param name="mgiContext"></param>
        /// <returns>Client Customer Id</returns>
        public string GetClientCustID(long cxnAccountId, MGIContext mgiContext)
        {
            //Call Repository to get the CCISAccount details
            CCISAccount account = CCISAccountRepo.FindBy(x => x.Id == cxnAccountId);
            string clientCustID = (account != null) ? account.PartnerAccountNumber : null;
            return clientCustID;
        }

        #endregion
    }
}
