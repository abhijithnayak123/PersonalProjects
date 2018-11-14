using System;
using System.Collections.Generic;
using MGI.Alloy.Common.Data;
using MGI.Alloy.CXN.Customer.Contract;
using MGI.Alloy.CXN.Customer.Data;
using P3Net.Data.Common;
using P3Net.Data;
using System.Data;
using MGI.Alloy.Common.Util;

namespace MGI.Alloy.CXN.Customer.FIS.Impl
{
    public class FISGateway : IClientCustomerService
    {
        internal FISIOImpl FisIO { get; set; }
        internal string EnvironmentIndicator { private get; set; }

        public FISGateway()
        {
            FisIO = GetFisIO();
        }

        //Added for User Story - AL-3715
        //Description - For checking the MISC account created successfully or not.
        public bool? IsCNECTSuccess { private get; set; }
        public bool? IsPREPDSuccess { private get; set; }

        #region LegacyCode - Use For Reference Only
        //public CustomerProfile Fetch(MGIContext search)
        //{
        //	NLogger.Info("Calling fetch to search based on SSN");
        //	FISAccount fisDetails = FisIO.SearchCustomerBySSN(search);
        //	NLogger.Info("Complete calling fetch");
        //	return Mapper.Map<FISAccount, CustomerProfile>(fisDetails);
        //}

        //public long Add(CustomerProfile customer, MGIContext mgiContext)
        //{
        //	FISAccount customerAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(customer);
        //	FISAccount fisAccountDetails = FISAccountRepo.FindBy(x => x.Id == mgiContext.CxnAccountId);

        //	customerAccountDetails.BranchId = (string.IsNullOrWhiteSpace(fisAccountDetails.BranchId) && string.IsNullOrWhiteSpace(fisAccountDetails.BankId)) && !string.IsNullOrWhiteSpace(mgiContext.BranchId) ? mgiContext.BranchId : fisAccountDetails.BranchId;
        //	customerAccountDetails.BankId = string.IsNullOrWhiteSpace(fisAccountDetails.BankId) && mgiContext.BankId != null ? mgiContext.BankId : fisAccountDetails.BankId;

        //	string fisPartnerAccountNo = (fisAccountDetails != null) ? fisAccountDetails.PartnerAccountNumber : string.Empty;

        //	if (string.IsNullOrWhiteSpace(fisPartnerAccountNo))
        //	{
        //		fisPartnerAccountNo = FisIO.CreateFISCustomer(customerAccountDetails, mgiContext);
        //		fisAccountDetails.PartnerAccountNumber = fisPartnerAccountNo;
        //	}

        //	if (!string.IsNullOrEmpty(fisPartnerAccountNo))
        //	{
        //		fisAccountDetails.ProfileStatus = ProfileStatus.Active;

        //		//Added for User Story - AL-3715
        //		//Description - When the CIS account is created make the flag - "IsCISSuccess" in tFIS_Account table to true.
        //		fisAccountDetails.IsCISSuccess = true;
        //		//Update CXN. The profile status doesn't depend on Misc account creation. Hence it's updated before the Misc account creation.
        //		FISAccountRepo.UpdateWithFlush(fisAccountDetails);
        //		//throw new ClientCustomerException(ClientCustomerException.CREATE_ACCOUNT_FAILED, "Could not create account for the customer");    
        //	}

        //	if (!mgiContext.Context.ContainsKey("AccountType"))
        //	{
        //		// This needs to be moved to gpr event handler.
        //		mgiContext.Context.Add("AccountType", CxnFISEnum.ConnectionsType.CNECT.ToString());
        //	}
        //	//instead of doing a mapping again from fisaccount to account use account directly.
        //	//if we are going to use gpr event handler, this is not required here.
        //	customer.PartnerAccountNumber = fisAccountDetails.PartnerAccountNumber;

        //	// This needs to be moved to gpr event handler.
        //	customer = Mapper.Map<FISAccount, CustomerProfile>(fisAccountDetails);
        //	AddAccount(customer, mgiContext);

        //	fisAccountDetails.RelationshipAccountNumber = GetFISRelationShipNumber(mgiContext);

        //	//Added for User Story - AL-3715
        //	//Description - When the MISC accounts are created (CNECT,PREPD), make the flag - "IsCNECTSuccess" and "IsPREPDSuccess" in tFIS_Account table to true.
        //	fisAccountDetails.IsCNECTSuccess = IsCNECTSuccess;

        //	//During customer creation update the IsPREPDSuccess flag to null as there won't be any card account assosiated to the customer.
        //	fisAccountDetails.IsPREPDSuccess = IsPREPDSuccess;

        //	//update cxn account
        //	FISAccountRepo.UpdateWithFlush(fisAccountDetails);

        //	return fisAccountDetails.Id;
        //}

        //public void Update(string id, CustomerProfile customerprofile, MGIContext mgiContext)
        //{
        //	//Updates CXN account
        //	UpdateCxnAccount(id, customerprofile, mgiContext);
        //}

        //public long AddAccount(CustomerProfile account, MGIContext mgiContext)
        //{
        //	FISAccount fisAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(account);
        //	string partneraccoutnumber = account.PartnerAccountNumber;
        //	fisAccountDetails.PartnerAccountNumber = partneraccoutnumber;

        //	NLogger.Info("Inside add account checking for cxn id.");
        //	if (mgiContext.Context.ContainsKey("CXNId"))
        //	{
        //		//partneraccoutnumber = FISAccountRepo.FindBy(x => x.Id == Convert.ToInt64(context["CXNId"])).PartnerAccountNumber;
        //		FISAccount fisAccountRepo = FISAccountRepo.FindBy(x => x.Id == Convert.ToInt64(mgiContext.Context["CXNId"]));
        //		fisAccountDetails.PartnerAccountNumber = fisAccountRepo.PartnerAccountNumber;
        //		fisAccountDetails.BankId = fisAccountRepo.BankId;
        //		fisAccountDetails.BranchId = fisAccountRepo.BranchId;
        //		NLogger.Info("After checking for cxn id. {0}", mgiContext.Context["CXNId"].ToString());
        //	}

        //	fisAccountDetails.RelationshipAccountNumber = GetFISRelationShipNumber(mgiContext);
        //	fisAccountDetails.BankId = !string.IsNullOrEmpty(fisAccountDetails.BankId) ? fisAccountDetails.BankId : account.BankId;
        //	fisAccountDetails.BranchId = !string.IsNullOrEmpty(fisAccountDetails.BranchId) ? fisAccountDetails.BranchId : account.BranchId;
        //	NLogger.Info("Before calling CreateMiscAccount");

        //	bool isSuccess = FisIO.CreateMiscAccount(fisAccountDetails, mgiContext);

        //	//Added for User Story - AL-3715
        //	//Description - When the MISC accounts are created - CNECT, make the flag - "IsCNECTSuccess" in tFIS_Account table to true.
        //	if (isSuccess && mgiContext.Context["AccountType"].ToString() == "CNECT")
        //	{
        //		IsCNECTSuccess = true;
        //	}
        //	else if (!isSuccess && mgiContext.Context["AccountType"].ToString() == "CNECT")
        //	{
        //		IsCNECTSuccess = false;
        //	}

        //	//Added for User Story - AL-3715
        //	//Description - When the MISC accounts are created - PREPD, make the flag - "IsPREPDSuccess" in tFIS_Account table to true.
        //	if (isSuccess && mgiContext.Context["AccountType"].ToString() == "PREPD")
        //	{
        //		IsPREPDSuccess = true;
        //	}
        //	else if (!isSuccess && mgiContext.Context["AccountType"].ToString() == "PREPD")
        //	{
        //		IsPREPDSuccess = false;
        //	}

        //	//Added for User Story - AL-3715
        //	//Update the IsPREPDSuccess flag to true when PREPD MISC account creation is success. 
        //	UpdateFISAccountForFund(mgiContext);

        //	//DE1971 - Fixed as part of the defect, when GPR card is added it is not updating the relationshipaccount number.
        //	//FISAccountRepo.SaveOrUpdate(fisAccountDetails);
        //	return 0;
        //}

        //private void UpdateFISAccountForFund(MGIContext mgiContext)
        //{
        //	FISAccount fisAccountDetails = FISAccountRepo.FindBy(x => x.Id == mgiContext.CxnAccountId);
        //	fisAccountDetails.IsPREPDSuccess = IsPREPDSuccess;

        //	FISAccountRepo.SaveOrUpdate(fisAccountDetails);
        //}

        //private string GetFISRelationShipNumber(MGIContext mgiContext)
        //{
        //	string accountnumber = string.Empty;

        //	//This is CXECustomerId, will be used in CNECT for E16055
        //	if (mgiContext.CXECustomerId != 0)
        //		accountnumber = EnvironmentIndicator + mgiContext.CXECustomerId;

        //	//This is card kit #, this will be used in PREPD for E16055
        //	if (mgiContext.Context.ContainsKey("AccountNumber") && mgiContext.Context["AccountNumber"] != null)
        //		accountnumber = mgiContext.Context["AccountNumber"].ToString();

        //	return accountnumber;
        //}


        //public void UpdateAccount(CustomerProfile account, MGIContext mgiContext)
        //{
        //	FISAccount fisAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(account);

        //	Update(account.PartnerAccountNumber, account, mgiContext);

        //	FISAccountRepo.SaveOrUpdate(fisAccountDetails);
        //}


        //private void UpdateCxnAccount(string cxnAccountId, CustomerProfile account, MGIContext mgiContext)
        //{
        //	// Get the FIS account details to be updated from database
        //	FISAccount fisAccountDetails = FISAccountRepo.FindBy(x => x.Id == Convert.ToInt64(cxnAccountId));
        //	ProfileStatus cxnStatus = fisAccountDetails.ProfileStatus;
        //	// Map the edited customer profile values to FIS account details
        //	fisAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(account, fisAccountDetails);
        //	fisAccountDetails.ProfileStatus = cxnStatus;

        //	fisAccountDetails.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
        //	fisAccountDetails.DTServerLastModified = DateTime.Now;
        //	// Call FIS account repository update method to push changes
        //	FISAccountRepo.UpdateWithFlush(fisAccountDetails);
        //}

        //public List<CustomerProfile> FetchAll(Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
        //{
        //	NLogger.Info("Calling fetchAll to search based on CustomerLookUp");

        //	List<FISAccount> fisDetails = FisIO.FetchAll(customerLookUpCriteria, cxnContext);

        //	NLogger.Info("Complete calling fetch");
        //	return Mapper.Map<List<FISAccount>, List<CustomerProfile>>(fisDetails);


        //}

        //public void ValidateCustomerStatus(long CXNId, MGIContext context)
        //{
        //	var FISAccount = FISAccountRepo.FindBy(x => x.Id == CXNId);

        //	FISProviderException fisValidateCustomerExp = new FISProviderException(FISProviderException.PROVIDER_VALIDATECUSTOMER_ERROR, "");
        //	if (FISAccount == null || FISAccount.ProfileStatus == ProfileStatus.Inactive)
        //		throw new ClientCustomerException(ClientCustomerException.PROVIDER_ERROR, fisValidateCustomerExp);
        //}

        //public long AddCXNAccount(CustomerProfile customer, MGIContext mgiContext)
        //{
        //          var partnerAccountNumber = customer.PartnerAccountNumber != null ? customer.PartnerAccountNumber : string.Empty;
        //	FISAccount fisAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(customer);

        //	fisAccountDetails.BranchId = (string.IsNullOrWhiteSpace(fisAccountDetails.BranchId) && string.IsNullOrWhiteSpace(fisAccountDetails.BankId)) && !string.IsNullOrWhiteSpace(mgiContext.BranchId) ? mgiContext.BranchId : fisAccountDetails.BranchId;
        //	fisAccountDetails.BankId = string.IsNullOrWhiteSpace(fisAccountDetails.BankId) && mgiContext.BankId != null ? mgiContext.BankId : fisAccountDetails.BankId;

        //	NLogger.Info("Calling Add to register a new customer in FIS");

        //	fisAccountDetails.ProfileStatus = ProfileStatus.Inactive;

        //	fisAccountDetails.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
        //	fisAccountDetails.DTServerCreate = DateTime.Now;
        //          fisAccountDetails.PartnerAccountNumber = partnerAccountNumber;

        //	FISAccountRepo.AddWithFlush(fisAccountDetails);

        //	if (fisAccountDetails.Id <= 0)
        //		throw new ClientCustomerException(ClientCustomerException.CREATE_ACCOUNT_FAILED, "Could not create account for the customer");

        //	return fisAccountDetails.Id;
        //}

        //public MGI.Common.Util.ProfileStatus GetClientProfileStatus(long cxnAccountId, MGIContext context)
        //{
        //	FISAccount account = FISAccountRepo.FindBy(x => x.Id == cxnAccountId);
        //	ProfileStatus status = (account == null) ? ProfileStatus.Inactive : account.ProfileStatus;
        //	return status;
        //}
        ///// <summary>
        ///// implemeted Method to get the ClientCustomerId
        ///// </summary>
        ///// <param name="cxnAccountId"></param>
        ///// <param name="mgiContext"></param>
        ///// <returns>return ClientCustomerId</returns>
        //public string GetClientCustID(long cxnAccountId, MGIContext mgiContext)
        //{
        //	FISAccount account = FISAccountRepo.FindBy(x => x.Id == cxnAccountId);
        //	string clientCustID = (account != null) ? account.PartnerAccountNumber : null;
        //	return clientCustID;
        //}

        //public bool GetCustInd(long cxnAccountId, MGIContext mgiContext)
        //{
        //	throw new NotImplementedException();
        //      }

        #endregion
        #region Public Methods

        /// <summary>
        /// This method is used to search the customer in provider.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria criteria, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="AlloyContext"></param>
        /// <returns></returns>
        public long Add(CustomerProfile customer, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="AlloyContext"></param>
        /// <returns></returns>
        public long AddCXNAccount(CustomerProfile customer, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to get client account data.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerProfile GetAccountByCustomerId(ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to get client account data.
        /// </summary>
        /// <param name="cxnAccountId"></param>
        /// <returns></returns>
        public CustomerProfile GetAccountById(long cxnAccountId, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to search the customer with card number.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCustomerWithCardNumber(string cardNumber, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// This method is used to validate the customer.
        ///// </summary>
        ///// <param name="alloyId"></param>
        ///// <param name="channelpartnerId"></param>
        //public void ValidateCustomerStatus(long alloyId, long channelpartnerId, AlloyContext context)
        //{
        //    StoredProcedure customerProcedure = new StoredProcedure("usp_ValidateCustomerStatus");

        //    customerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelpartnerId));
        //    customerProcedure.WithParameters(InputParameter.Named("alloyId").WithValue(alloyId));

        //    IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(customerProcedure);

        //    while (datareader.Read())
        //    {
        //        string status = datareader.GetStringOrDefault("ProfileStatus");

        //        if (string.IsNullOrWhiteSpace(status) || Helper.GetProfileStatus(status) == Helper.ProfileStatus.Inactive)
        //            throw new Exception("Error occurred while validating the customer status");

        //    }
        //}

        /// <summary>
        /// This method is used to sync in the customer details with TCF.
        /// </summary>
        /// <param name="custProfile"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerProfile CustomerSyncInFromClient(CustomerProfile custProfile, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Private Methods

        private FISIOImpl GetFisIO()
        {
            if (FisIO == null)
            { return new FISIOImpl(); }

            return FisIO;
        }



        #endregion


    }
}
