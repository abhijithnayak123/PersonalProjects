using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Contract;
using MGI.Core.Partner.Contract;
using System.Collections.Generic;
using MGI.Biz.Events.Contract;
using MGI.Cxn.Customer.CCIS.Data;
using MGI.Cxn.Customer.Contract;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using CxeCustomerService = MGI.Core.CXE.Contract.ICustomerService;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;
using PTNRCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using AutoMapper;
using MGI.Core.Partner.Data;
using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using System;

namespace MGI.Biz.Carver.Impl
{
	public class CarverCustomerEditEventListener : INexxoBizEventListener
	{
		public IClientCustomerService CXNClientCustomerSvc { private get; set; }
		public CxeCustomerService CxeCustomerService { private get; set; }
		public IRepository<CCISAccount> CCISAccountRepo { private get; set; }

		public ICustomerRepository ClientCustomerSvc { private get; set; }
		public PTNRCustomerService PTNRCustomerService { private get; set; }

		public void Notify(NexxoBizEvent BizEvent)
		{
			CustomerEditEvent EventData = (CustomerEditEvent)BizEvent;
			Mapper.CreateMap<CustomerProfile, CxnCustomer>();
			Update(EventData);
		}

		private void Update(CustomerEditEvent EditEvent)
		{
			PtnrCustomer ptnrCustomer = (PtnrCustomer)EditEvent.mgiContext.Context["PTNRCustomer"];

			long cxnAccountId = (ptnrCustomer.GetAccount(EditEvent.mgiContext.ProviderId) == null ? 0 : ptnrCustomer.GetAccount((int)EditEvent.mgiContext.ProviderId).CXNId);

			CCISAccount account = CCISAccountRepo.FindBy(x => x.Id == cxnAccountId);

			if (account == null)
			{
				//Update the partner profile status as false. This will be updated, if the CCIS insertion is successful.
				ptnrCustomer.CustomerProfileStatus = ProfileStatus.Inactive;
				PTNRCustomerService.Update(ptnrCustomer);
				cxnAccountId = PushCustomertoClient(EditEvent.profile, EditEvent.mgiContext, EditEvent.profile.ProfileStatus);
			}
            else
            {
			CustomerProfile customerProfile = EditEvent.profile;
			//Map Biz customer profile to CXN customer profile
			CxnCustomer cxnCustomer = Mapper.Map<CxnCustomer>(customerProfile);

			//Update CXN by calling CXN client service
			CXNClientCustomerSvc.Update(cxnAccountId.ToString(), cxnCustomer, EditEvent.mgiContext);

			ptnrCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone((string)EditEvent.mgiContext.TimeZone);

			ProfileStatus cxeProfileStatus = EditEvent.profile.ProfileStatus;

			//UPDATE CUSTOMER PROFILE STATUS IN PARTNER CUSTOMER BASED ON THE PROFILE STATUS IN FIS ACCOUNT & CXE
			if (account != null && account.ProfileStatus == ProfileStatus.Active && cxeProfileStatus == ProfileStatus.Active)
			{
				ptnrCustomer.CustomerProfileStatus = ProfileStatus.Active;
				PTNRCustomerService.Update(ptnrCustomer);
			}
            }

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
			ptnrCustomer.AddAccount((mgiContext.ProviderId), ((long)mgiContext.CXECustomerId), cxnAccountId);

			// to get the ProfileStataus for the CCIS Account
			ProfileStatus cxnProfileStatus = CXNClientCustomerSvc.GetClientProfileStatus(cxnAccountId, mgiContext);
			CXECustomer cxeCustomer = CxeCustomerService.Lookup(ptnrCustomer.Id);
			PTNRProspect ptnrProspect = PTNRCustomerService.LookupProspect(ptnrCustomer.Id);
			// to get the ClientCustomerId for the CCIS Account
			string clientCustId = CXNClientCustomerSvc.GetClientCustID(cxnAccountId, mgiContext);

			if (cxnProfileStatus == ProfileStatus.Active && !string.IsNullOrWhiteSpace(clientCustId))
			{

				ptnrCustomer.CustomerProfileStatus = ProfileStatus.Active;
				cxeCustomer.ProfileStatus = ProfileStatus.Active;
				//Assigning ClientCustId to cxeCustomer
				cxeCustomer.ClientID = clientCustId;
				//Assigning ClientCustId to ptnrProspect
				ptnrProspect.ClientID = clientCustId;
				ptnrProspect.ProfileStatus = ProfileStatus.Active;
				ptnrCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				cxeCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				ptnrProspect.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				PTNRCustomerService.Update(ptnrCustomer);
				//saving the cxeCustomer with updated ClientId into Database
				CxeCustomerService.Save(cxeCustomer);
				//saving the ptnProspect with updated ClientId into Database
				PTNRCustomerService.SaveProspect(ptnrProspect);
			}

			return cxnAccountId;

		}
	}
}
