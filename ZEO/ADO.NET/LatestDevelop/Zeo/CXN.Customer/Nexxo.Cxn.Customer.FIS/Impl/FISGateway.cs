using System;
using System.Collections.Generic;
using MGI.Common.Util;
using MGI.Cxn.Customer.Data;
using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.FIS.Contract;
using MGI.Cxn.Customer.FIS.Data;
using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Data;
using AutoMapper;
using System.Diagnostics;

namespace MGI.Cxn.Customer.FIS.Impl
{
	public class FISGateway : IClientCustomerService
	{
		public FISIOImpl FisIO { get; set; }

		public IRepository<FISAccount> FISAccountRepo { private get; set; }
		public NLoggerCommon NLogger { get; set; }

		public string EnvironmentIndicator { private get; set; }

		//Added for User Story - AL-3715
		//Description - For checking the MISC account created successfully or not.
		public bool? IsCNECTSuccess { private get; set; }
		public bool? IsPREPDSuccess { private get; set; }

		public FISGateway()
		{
			Mapper.CreateMap<FISAccount, CustomerProfile>();
			Mapper.CreateMap<CustomerProfile, FISAccount>();
			Mapper.CreateMap<CustomerProfile, FISAccount>()
				.ForMember(d => d.IsCISSuccess, opt => opt.Ignore())
				.ForMember(d => d.IsCNECTSuccess, opt => opt.Ignore())
				.ForMember(d => d.IsPREPDSuccess, opt => opt.Ignore())
				.ForMember(d => d.BankId, opt => opt.Ignore())
				.ForMember(d => d.BranchId, opt => opt.Ignore())
                .ForMember(d => d.PartnerAccountNumber, opt => opt.Ignore())
				.ForMember(d => d.RelationshipAccountNumber, opt => opt.Ignore());
		}

		public CustomerProfile Fetch(MGIContext search)
		{
			try
            {
                NLogger.Info("Calling fetch to search based on SSN");
                FISAccount fisDetails = FisIO.SearchCustomerBySSN(search);
                NLogger.Info("Complete calling fetch");
                return Mapper.Map<FISAccount, CustomerProfile>(fisDetails);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.SEARCH_CUSTOMER_FAILED, ex);
            }
		}

		public long Add(CustomerProfile customer, MGIContext mgiContext)
		{
            try
            {
                FISAccount customerAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(customer);
                FISAccount fisAccountDetails = FISAccountRepo.FindBy(x => x.Id == mgiContext.CxnAccountId);

                customerAccountDetails.BranchId = (string.IsNullOrWhiteSpace(fisAccountDetails.BranchId) && string.IsNullOrWhiteSpace(fisAccountDetails.BankId)) && !string.IsNullOrWhiteSpace(mgiContext.BranchId) ? mgiContext.BranchId : fisAccountDetails.BranchId;
                customerAccountDetails.BankId = string.IsNullOrWhiteSpace(fisAccountDetails.BankId) && mgiContext.BankId != null ? mgiContext.BankId : fisAccountDetails.BankId;

                string fisPartnerAccountNo = (fisAccountDetails != null) ? fisAccountDetails.PartnerAccountNumber : string.Empty;

                if (string.IsNullOrWhiteSpace(fisPartnerAccountNo))
                {
                    fisPartnerAccountNo = FisIO.CreateFISCustomer(customerAccountDetails, mgiContext);
                    fisAccountDetails.PartnerAccountNumber = fisPartnerAccountNo;
                }

                if (!string.IsNullOrEmpty(fisPartnerAccountNo))
                {
                    fisAccountDetails.ProfileStatus = ProfileStatus.Active;

                    //Added for User Story - AL-3715
                    //Description - When the CIS account is created make the flag - "IsCISSuccess" in tFIS_Account table to true.
                    fisAccountDetails.IsCISSuccess = true;
                    //Update CXN. The profile status doesn't depend on Misc account creation. Hence it's updated before the Misc account creation.
                    FISAccountRepo.UpdateWithFlush(fisAccountDetails);
                    //throw new ClientCustomerException(ClientCustomerException.CREATE_ACCOUNT_FAILED, "Could not create account for the customer");    
                }

                if (!mgiContext.Context.ContainsKey("AccountType"))
                {
                    // This needs to be moved to gpr event handler.
                    mgiContext.Context.Add("AccountType", CxnFISEnum.ConnectionsType.CNECT.ToString());
                }
                //instead of doing a mapping again from fisaccount to account use account directly.
                //if we are going to use gpr event handler, this is not required here.
                customer.PartnerAccountNumber = fisAccountDetails.PartnerAccountNumber;

                // This needs to be moved to gpr event handler.
                customer = Mapper.Map<FISAccount, CustomerProfile>(fisAccountDetails);
                AddAccount(customer, mgiContext);

                fisAccountDetails.RelationshipAccountNumber = GetFISRelationShipNumber(mgiContext);

                //Added for User Story - AL-3715
                //Description - When the MISC accounts are created (CNECT,PREPD), make the flag - "IsCNECTSuccess" and "IsPREPDSuccess" in tFIS_Account table to true.
                fisAccountDetails.IsCNECTSuccess = IsCNECTSuccess;

                //During customer creation update the IsPREPDSuccess flag to null as there won't be any card account assosiated to the customer.
                fisAccountDetails.IsPREPDSuccess = IsPREPDSuccess;

                //update cxn account
                FISAccountRepo.UpdateWithFlush(fisAccountDetails);

                return fisAccountDetails.Id;
            }
            catch(Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.UPDATE_ACCOUNT_FAILED, ex);
            }
		}

		public void Update(string id, CustomerProfile customerprofile, MGIContext mgiContext)
		{
			try
            {
                //Updates CXN account
                UpdateCxnAccount(id, customerprofile, mgiContext);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.UPDATE_ACCOUNT_FAILED, ex);
            }
		}

		public long AddAccount(CustomerProfile account, MGIContext mgiContext)
		{
            try
            {
                FISAccount fisAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(account);
                string partneraccoutnumber = account.PartnerAccountNumber;
                fisAccountDetails.PartnerAccountNumber = partneraccoutnumber;

                NLogger.Info("Inside add account checking for cxn id.");
                if (mgiContext.Context.ContainsKey("CXNId"))
                {
                    //partneraccoutnumber = FISAccountRepo.FindBy(x => x.Id == Convert.ToInt64(context["CXNId"])).PartnerAccountNumber;
                    FISAccount fisAccountRepo = FISAccountRepo.FindBy(x => x.Id == Convert.ToInt64(mgiContext.Context["CXNId"]));
                    fisAccountDetails.PartnerAccountNumber = fisAccountRepo.PartnerAccountNumber;
                    fisAccountDetails.BankId = fisAccountRepo.BankId;
                    fisAccountDetails.BranchId = fisAccountRepo.BranchId;
                    NLogger.Info("After checking for cxn id. {0}", mgiContext.Context["CXNId"].ToString());
                }

                fisAccountDetails.RelationshipAccountNumber = GetFISRelationShipNumber(mgiContext);
                fisAccountDetails.BankId = !string.IsNullOrEmpty(fisAccountDetails.BankId) ? fisAccountDetails.BankId : account.BankId;
                fisAccountDetails.BranchId = !string.IsNullOrEmpty(fisAccountDetails.BranchId) ? fisAccountDetails.BranchId : account.BranchId;
                NLogger.Info("Before calling CreateMiscAccount");

                bool isSuccess = FisIO.CreateMiscAccount(fisAccountDetails, mgiContext);

                //Added for User Story - AL-3715
                //Description - When the MISC accounts are created - CNECT, make the flag - "IsCNECTSuccess" in tFIS_Account table to true.
                if (isSuccess && mgiContext.Context["AccountType"].ToString() == "CNECT")
                {
                    IsCNECTSuccess = true;
                }
                else if (!isSuccess && mgiContext.Context["AccountType"].ToString() == "CNECT")
                {
                    IsCNECTSuccess = false;
                }

                //Added for User Story - AL-3715
                //Description - When the MISC accounts are created - PREPD, make the flag - "IsPREPDSuccess" in tFIS_Account table to true.
                if (isSuccess && mgiContext.Context["AccountType"].ToString() == "PREPD")
                {
                    IsPREPDSuccess = true;
                }
                else if (!isSuccess && mgiContext.Context["AccountType"].ToString() == "PREPD")
                {
                    IsPREPDSuccess = false;
                }

                //Added for User Story - AL-3715
                //Update the IsPREPDSuccess flag to true when PREPD MISC account creation is success. 
                UpdateFISAccountForFund(mgiContext);

                //DE1971 - Fixed as part of the defect, when GPR card is added it is not updating the relationshipaccount number.
                //FISAccountRepo.SaveOrUpdate(fisAccountDetails);
                return 0;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.ADD_ACCOUNT_FAILED, ex);
            }
		}

		private void UpdateFISAccountForFund(MGIContext mgiContext)
		{
			FISAccount fisAccountDetails = FISAccountRepo.FindBy(x => x.Id == mgiContext.CxnAccountId);
			fisAccountDetails.IsPREPDSuccess = IsPREPDSuccess;

			FISAccountRepo.SaveOrUpdate(fisAccountDetails);
		}

		private string GetFISRelationShipNumber(MGIContext mgiContext)
		{
			string accountnumber = string.Empty;

			//This is CXECustomerId, will be used in CNECT for E16055
			if (mgiContext.CXECustomerId != 0)
				accountnumber = EnvironmentIndicator + mgiContext.CXECustomerId;

			//This is card kit #, this will be used in PREPD for E16055
			if (mgiContext.Context.ContainsKey("AccountNumber") && mgiContext.Context["AccountNumber"] != null)
				accountnumber = mgiContext.Context["AccountNumber"].ToString();

			return accountnumber;
		}

		public void UpdateAccount(CustomerProfile account, MGIContext mgiContext)
		{
			try
            {
                FISAccount fisAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(account);

                Update(account.PartnerAccountNumber, account, mgiContext);

                FISAccountRepo.SaveOrUpdate(fisAccountDetails);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.UPDATE_ACCOUNT_FAILED, ex);
            }
		}

		private void UpdateCxnAccount(string cxnAccountId, CustomerProfile account, MGIContext mgiContext)
		{
			// Get the FIS account details to be updated from database
			FISAccount fisAccountDetails = FISAccountRepo.FindBy(x => x.Id == Convert.ToInt64(cxnAccountId));
			ProfileStatus cxnStatus = fisAccountDetails.ProfileStatus;
			// Map the edited customer profile values to FIS account details
			fisAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(account, fisAccountDetails);
			fisAccountDetails.ProfileStatus = cxnStatus;

			fisAccountDetails.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			fisAccountDetails.DTServerLastModified = DateTime.Now;
			// Call FIS account repository update method to push changes
			FISAccountRepo.UpdateWithFlush(fisAccountDetails);
		}

		public List<CustomerProfile> FetchAll(Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
		{
			try
            {
                NLogger.Info("Calling fetchAll to search based on CustomerLookUp");

                List<FISAccount> fisDetails = FisIO.FetchAll(customerLookUpCriteria, cxnContext);

                NLogger.Info("Complete calling fetch");
                return Mapper.Map<List<FISAccount>, List<CustomerProfile>>(fisDetails);
            }
            catch(Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.SEARCH_CUSTOMER_FAILED, ex);
            }

		}

		public void ValidateCustomerStatus(long CXNId, MGIContext context)
		{
            try
            {
                var FISAccount = FISAccountRepo.FindBy(x => x.Id == CXNId);

                //FISProviderException fisValidateCustomerExp = new FISProviderException(FISProviderException.PROVIDER_VALIDATECUSTOMER_ERROR, "", null);
                if (FISAccount == null || FISAccount.ProfileStatus == ProfileStatus.Inactive)
                    throw new CustomerException(CustomerException.VALIDATE_CUSTOMER_STATUS_FAILED, null);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.VALIDATE_CUSTOMER_STATUS_FAILED, ex);
            }
		}

		public long AddCXNAccount(CustomerProfile customer, MGIContext mgiContext)
		{
            try
            {
                var partnerAccountNumber = customer.PartnerAccountNumber != null ? customer.PartnerAccountNumber : string.Empty;
                FISAccount fisAccountDetails = Mapper.Map<CustomerProfile, FISAccount>(customer);

                fisAccountDetails.BranchId = (string.IsNullOrWhiteSpace(fisAccountDetails.BranchId) && string.IsNullOrWhiteSpace(fisAccountDetails.BranchId)) && !string.IsNullOrWhiteSpace(mgiContext.BranchId) ? mgiContext.BranchId : fisAccountDetails.BranchId;
                fisAccountDetails.BankId = string.IsNullOrWhiteSpace(fisAccountDetails.BankId) && mgiContext.BankId != null ? mgiContext.BankId : fisAccountDetails.BankId;

                NLogger.Info("Calling Add to register a new customer in FIS");

                fisAccountDetails.ProfileStatus = ProfileStatus.Inactive;

                fisAccountDetails.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
                fisAccountDetails.DTServerCreate = DateTime.Now;
                fisAccountDetails.PartnerAccountNumber = partnerAccountNumber;

                FISAccountRepo.AddWithFlush(fisAccountDetails);

                if (fisAccountDetails.Id <= 0)
                    throw new CustomerException(CustomerException.ADD_ACCOUNT_FAILED, null);

                return fisAccountDetails.Id;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.ADD_ACCOUNT_FAILED, ex);
            }
		}

		public ProfileStatus GetClientProfileStatus(long cxnAccountId, MGIContext context)
		{
            try
            {
                FISAccount account = FISAccountRepo.FindBy(x => x.Id == cxnAccountId);
                ProfileStatus status = (account == null) ? ProfileStatus.Inactive : account.ProfileStatus;
                return status;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.FIND_CLIENT_PROFILESTATUS_FAILED, ex);
            }
		}
		/// <summary>
		/// implemeted Method to get the ClientCustomerId
		/// </summary>
		/// <param name="cxnAccountId"></param>
		/// <param name="mgiContext"></param>
		/// <returns>return ClientCustomerId</returns>
		public string GetClientCustID(long cxnAccountId, MGIContext mgiContext)
		{
            try
            {
                FISAccount account = FISAccountRepo.FindBy(x => x.Id == cxnAccountId);
                string clientCustID = (account != null) ? account.PartnerAccountNumber : null;
                return clientCustID;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.FIND_CLIENT_CUSTIND_FAILED, ex);
            }
		}

		public bool GetCustInd(long cxnAccountId, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}
	}
}
