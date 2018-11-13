using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Contract;
using System.Collections.Generic;
using MGI.Biz.Events.Contract;
using MGI.Cxn.Customer.CCIS.Data;
using MGI.Cxn.Customer.Contract;
using MGI.Common.Util;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using CxeCustomerService = MGI.Core.CXE.Contract.ICustomerService;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;
using AutoMapper;
using MGI.Common.DataAccess.Contract;
using IPtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using System;

namespace MGI.Biz.Carver.Impl
{
	public class CarverCustomerRegistrationEventListener : INexxoBizEventListener
	{
		public IClientCustomerService CXNClientCustomerSvc { private get; set; }
		public CxeCustomerService CxeCustomerService { private get; set; }

		public IRepository<CCISAccount> CCISAccountRepo { private get; set; }

		public ICustomerRepository ClientCustomerSvc { private get; set; }

		private IPtnrCustomerService PTNRCustomerService;
		public IPtnrCustomerService PartnerCustomerService { set { PTNRCustomerService = value; } }

		public void Notify(NexxoBizEvent BizEvent)
		{
			CustomerRegistrationEvent EventData = (CustomerRegistrationEvent)BizEvent;
			Mapper.CreateMap<CustomerProfile, CxnCustomer>();
			Register(EventData);
		}

		private void Register(CustomerRegistrationEvent RegistrationEvent)
		{
			long cxnAccountId = PushCustomertoClient(RegistrationEvent.profile, RegistrationEvent.mgiContext, RegistrationEvent.profile.ProfileStatus);
		}


		private long PushCustomertoClient(CustomerProfile customer, MGIContext mgiContext, ProfileStatus CXEProfileStatus)
		{
			mgiContext.SSN = customer.SSN;

			CxnCustomer cxnCustomer = Mapper.Map<CxnCustomer>(customer);
			long cxnAccountId = 0;

			if (mgiContext.Context.ContainsKey("FetchedFromCustomerLookUp") && mgiContext.Context["FetchedFromCustomerLookUp"] != null)
			{
				//Get the details from the Customer Look up Search Result (Based on the customer chosen by Teller)
				cxnCustomer.PartnerAccountNumber = (string)mgiContext.Context["CustomerLookUpPartnerAccountNumber"];
				cxnCustomer.RelationshipAccountNumber = (string)mgiContext.Context["CustomerLookUpRelationshipAccountNumber"];
				cxnCustomer.BankId = (string)mgiContext.Context["CustomerLookUpBankId"];
				cxnCustomer.BranchId = (string)mgiContext.Context["CustomerLookUpBranchId"];
			}

			cxnAccountId = CXNClientCustomerSvc.AddCXNAccount(cxnCustomer, mgiContext);

			//partner account details
			MGI.Core.Partner.Data.Customer ptnrCustomer = (MGI.Core.Partner.Data.Customer)mgiContext.Context["PTNRCustomer"];
			ptnrCustomer.AddAccount((int)mgiContext.ProviderId, mgiContext.CXECustomerId, cxnAccountId);

			// to get the ProfileStataus for the CCIS Account
			ProfileStatus cxnProfileStatus = CXNClientCustomerSvc.GetClientProfileStatus(cxnAccountId, mgiContext);
			CXECustomer cxeCustomer = CxeCustomerService.Lookup(ptnrCustomer.Id);
			PTNRProspect ptnrProspect = PTNRCustomerService.LookupProspect(ptnrCustomer.Id);
			// to get the ClientCustomerId for the CCIS Account
			string clientCustId = CXNClientCustomerSvc.GetClientCustID(cxnAccountId, mgiContext);

			if (cxnProfileStatus == ProfileStatus.Active && !string.IsNullOrWhiteSpace(clientCustId))
			{
				// to get the ClientCustomerId for the CCIS Account
				ptnrCustomer.CustomerProfileStatus = ProfileStatus.Active;
				cxeCustomer.ProfileStatus = ProfileStatus.Active;
				//Assigning ClientCustId to cxeCustomer
				cxeCustomer.ClientID = clientCustId;
				//Assigning ClientCustId to ptnrProspect
				ptnrProspect.ClientID = clientCustId;
				ptnrProspect.ProfileStatus = ProfileStatus.Active;
				cxeCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				ptnrProspect.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				//saving the cxeCustomer with updated ClientId into Database
				CxeCustomerService.Save(cxeCustomer);
				//saving the ptnProspect with updated ClientId into Database
				PTNRCustomerService.SaveProspect(ptnrProspect);
			}
			ptnrCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
			PTNRCustomerService.Update(ptnrCustomer);

			return cxnAccountId;

		}
	}
}
