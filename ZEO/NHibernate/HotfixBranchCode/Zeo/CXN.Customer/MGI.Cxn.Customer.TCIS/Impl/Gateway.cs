using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MGI.Cxn.Customer.TCIS.Data;
using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Cxn.Customer.TCIS.Contract;

namespace MGI.Cxn.Customer.TCIS.Impl
{
	public class Gateway : IClientCustomerService
	{
		public IO IO { private get; set; }
		public NLoggerCommon NLogger { get; set; }
		public IRepository<Account> TCISAccountRepo { private get; set; }

		public Gateway()
		{
			Mapper.CreateMap<Account, CustomerProfile>()
			.ForMember(x => x.CustInd, o => o.MapFrom(s => s.TcfCustInd));
			Mapper.CreateMap<CustomerProfile, Account>()
				  .ForMember(x => x.ProfileStatus, opt => opt.Ignore())
				  .ForMember(x => x.TcfCustInd, o => o.MapFrom(s => s.CustInd))
				  .ForMember(x => x.PartnerAccountNumber, o => o.Ignore())
				  .AfterMap((s, d) =>
				  {
					  if (ValidateClientCustId(s.ClientID))
						  d.PartnerAccountNumber = s.ClientID;
				  })
				  .ForMember(x => x.RelationshipAccountNumber, opt => opt.Ignore());
		}

		public CustomerProfile Fetch(MGIContext mgiContext)
		{
			NLogger.Info("Calling fetchAll to search based on CustomerLookUp");

			CustomerProfile customerProfile = GetAccountById(mgiContext.CxnAccountId);

			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();

			if (customerProfile != null && !string.IsNullOrEmpty(customerProfile.PartnerAccountNumber))
				customerLookUpCriteria.Add("ClientPAN", NexxoUtil.SafeSQLString(customerProfile.PartnerAccountNumber.Replace("-", ""), true));
			try
			{
				Account customerDetail = IO.FetchAll(customerLookUpCriteria, mgiContext).FirstOrDefault();

				// Keep Alloy Customer name detailsas it is(Tcf don't want customer name updated from RCIF at session authentication)//AL-626
				customerDetail.FirstName = customerProfile.FirstName;
				customerDetail.MiddleName = customerProfile.MiddleName;
				customerDetail.LastName = customerProfile.LastName;
				customerDetail.LastName2 = customerProfile.LastName2;

				NLogger.Info("Complete calling fetch");
				return Mapper.Map<Account, CustomerProfile>(customerDetail);
			}
			catch (TCIFProviderException ex)
			{
				throw new ClientCustomerException(ClientCustomerException.TCIS_FIND_CUSTOMER_FAILED, ex.Message, ex);
			}
		}

		public List<CustomerProfile> FetchAll(Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
		{
			try
			{
				NLogger.Info("Calling fetchAll to search based on CustomerLookUp");

				List<Account> customerDetails = IO.FetchAll(customerLookUpCriteria, cxnContext);

				NLogger.Info("Complete calling fetch");
				return Mapper.Map<List<Account>, List<CustomerProfile>>(customerDetails);
			}
			catch (TCIFProviderException ex)
			{
				throw new ClientCustomerException(ClientCustomerException.TCIS_FIND_CUSTOMER_FAILED, ex.Message, ex);
			}
		}

		public CustomerProfile GetAccountById(long cxnAccountId)
		{
			NLogger.Info("Calling fetchAll to search based on CustomerLookUp");

			Account tcisAccountDetails = TCISAccountRepo.FindBy(x => x.Id == cxnAccountId);

			return Mapper.Map<Account, CustomerProfile>(tcisAccountDetails);
		}

		public long Add(CustomerProfile customer, MGIContext mgiContext)
		{
			Account tcisAccountDetails = TCISAccountRepo.FindBy(x => x.Id == mgiContext.CxnAccountId);
			tcisAccountDetails = Mapper.Map<CustomerProfile, Account>(customer, tcisAccountDetails);

			string partnerAccountNo = (tcisAccountDetails != null) ? tcisAccountDetails.PartnerAccountNumber : string.Empty;

			try
			{
				//TODO: undate response type to handle failure response indicator (error codes 1, 2).
				partnerAccountNo = IO.CreateCustomer(tcisAccountDetails, mgiContext);
			}
			catch (TCIFProviderException ex)
			{
				throw new ClientCustomerException(ClientCustomerException.TCIS_REGISTRATION_SOFTSTOP, ex.Message, ex);
			}

			if (ValidateClientCustId(partnerAccountNo))
			{
				tcisAccountDetails.ProfileStatus = ProfileStatus.Active;
				tcisAccountDetails.PartnerAccountNumber = partnerAccountNo;

				tcisAccountDetails.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				tcisAccountDetails.DTServerLastModified = DateTime.Now;
				//Update CXN.
				TCISAccountRepo.Merge(tcisAccountDetails);
			}

			return tcisAccountDetails.Id;
		}

		public void Update(string id, CustomerProfile customer, MGIContext mgiContext)
		{
			UpdateCxnAccount(id, customer, mgiContext);
		}

		public long AddAccount(CustomerProfile account, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public void ValidateCustomerStatus(long CXNId, MGIContext context)
		{
			return;
		}

		public long AddCXNAccount(CustomerProfile customer, MGIContext mgiContext)
		{
			Account tcisAccountDetails = Mapper.Map<CustomerProfile, Account>(customer);

			try
			{
				tcisAccountDetails.ProfileStatus = ProfileStatus.Inactive;
				tcisAccountDetails.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				tcisAccountDetails.DTServerCreate = DateTime.Now;

				TCISAccountRepo.AddWithFlush(tcisAccountDetails);
			}
			catch (Exception ex)
			{
				if (tcisAccountDetails.Id <= 0)
					throw new ClientCustomerException(ClientCustomerException.CREATE_ACCOUNT_FAILED, "Could not create account for the customer", ex);
			}
			return tcisAccountDetails.Id;
		}

		public ProfileStatus GetClientProfileStatus(long cxnAccountId, MGIContext context)
		{
			var TCISAccount = TCISAccountRepo.FindBy(x => x.Id == cxnAccountId);

			ProfileStatus status = (TCISAccount == null) ? ProfileStatus.Inactive : TCISAccount.ProfileStatus;

			return status;
		}

		public string GetClientCustID(long cxnAccountId, MGIContext context)
		{
			var TCISAccount = TCISAccountRepo.FindBy(x => x.Id == cxnAccountId);

			string clientCustID = (TCISAccount != null) ? TCISAccount.PartnerAccountNumber : null;

			return clientCustID;
		}

		public bool GetCustInd(long cxnAccountId, MGIContext mgiContext)
		{
			var TCISAccount = TCISAccountRepo.FindBy(x => x.Id == cxnAccountId);

			bool tcfCustInd = (TCISAccount != null) ? TCISAccount.TcfCustInd : false;

			return tcfCustInd;
		}

		private void UpdateCxnAccount(string cxnAccountId, CustomerProfile account, MGIContext mgiContext)
		{
			// Get the TCIS account details to be updated from database
			Account tcisAccountDetails = TCISAccountRepo.FindBy(x => x.Id == Convert.ToInt64(cxnAccountId));
			string bankId = tcisAccountDetails.BankId;
			string branchId = tcisAccountDetails.BranchId;

			// Map the edited customer profile values to TCIS account details
			tcisAccountDetails = Mapper.Map<CustomerProfile, Account>(account, tcisAccountDetails);
			tcisAccountDetails.BranchId = branchId;
			tcisAccountDetails.BankId = bankId;
			tcisAccountDetails.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			tcisAccountDetails.DTServerLastModified = DateTime.Now;
			// Call TCIS account repository update method to push changes
			TCISAccountRepo.UpdateWithFlush(tcisAccountDetails);
		}

		private bool ValidateClientCustId(string clientCustId)
		{
			long number = 0;
			Int64.TryParse(clientCustId, out number);
			return number > 0;
		}
	}
}
