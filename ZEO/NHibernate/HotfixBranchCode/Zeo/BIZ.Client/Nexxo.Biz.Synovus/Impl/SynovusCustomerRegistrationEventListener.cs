using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Contract;
using MGI.Cxn.Customer.FIS.Contract;
using MGI.Cxn.Customer.FIS.Data;
using MGI.Biz.FundsEngine.Data;
using MGI.Biz.FundsEngine.Contract;
using System.Collections.Generic;
using MGI.Biz.Events.Contract;
using MGI.Cxn.Customer.Contract;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using CxeCustomerService = MGI.Core.CXE.Contract.ICustomerService;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;
using AutoMapper;
using MGI.Core.Partner.Data;
using MGI.Common.DataAccess.Contract;
using IPtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using System;
using MGI.Common.Util;
using MGI.Core.Partner.Contract;
using System.Linq;

namespace MGI.Biz.Synovus.Impl
{
	public class SynovusCustomerRegistrationEventListener : INexxoBizEventListener
	{
		public IClientCustomerService CXNClientCustomerSvc { private get; set; }
		public IFISConnect CXNCustomerSvc { private get; set; }
		public CxeCustomerService CxeCustomerService { private get; set; }

		public IFundsEngine BIZFundsEngineSvc { private get; set; }

		public IRepository<FISAccount> FISAccountRepo { private get; set; }

		public ICustomerRepository ClientCustomerSvc { private get; set; }

		private IPtnrCustomerService PTNRCustomerService;
		public IPtnrCustomerService PartnerCustomerService { set { PTNRCustomerService = value; } }

		public void Notify(NexxoBizEvent BizEvent)
		{
			CustomerRegistrationEvent EventData = (CustomerRegistrationEvent)BizEvent;
			Mapper.CreateMap<CustomerProfile, CxnCustomer>();
			#region Mapping FisConnect Data to Account Data (SQL Injection User Story US#1789)
			// Mapping FisConnect Data to Account Data to return Account Type - User Story # 1789.
			AutoMapper.Mapper.CreateMap<FISConnect, CustomerProfile>()
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
			.ForMember(x => x.Gender, s => s.MapFrom(c => c.Gender))
			.ForMember(x => x.MothersMaidenName, s => s.MapFrom(c => c.MothersMaidenName))
			.ForMember(x => x.BankId, s => s.MapFrom(c => c.MetBankNumber))
			.ForMember(x => x.PartnerAccountNumber, s => s.MapFrom(c => c.CustomerNumber))
			.ForMember(x => x.RelationshipAccountNumber, s => s.MapFrom(c => c.ExternalKey))
			.ForMember(x => x.ProgramId, s => s.MapFrom(c => c.ProgramId));
			#endregion
			Mapper.CreateMap<FISAccount, FundsAccount>()
				.ForMember(x => x.AccountNumber, s => s.MapFrom(y => y.RelationshipAccountNumber)); // This should have the TSys card kit # for customers fetced from connects db.

			Register(EventData);
		}

		private void Register(CustomerRegistrationEvent RegistrationEvent)
		{
			// the profile is present in the event. We can now do anything.
			// 2. call FIS through CXN.
			// 2.2 call FIS to the following:
			// 2.3 Open Misc Account and get the external client id.
			// 2.4 Update the external client id to NexxoCustomer account in CXN Funds?
			// 
			// 1. Lookup the ConnectDb and if GPR, register with Nexxo (Link GPR usecase)
			// 1.1 Do the fetch again and see if the customer profile has external key, if so associate the card # directly to the newly created profile.
			// 3 . anything else??? 

			MGI.Core.Partner.Data.Customer ptnrCustomer = (MGI.Core.Partner.Data.Customer)RegistrationEvent.mgiContext.Context["PTNRCustomer"];

			//Update the partner profile status as false. This will be updated, if the FIS insertion is successful.
			ptnrCustomer.CustomerProfileStatus = ProfileStatus.Inactive;
			PTNRCustomerService.Update(ptnrCustomer);


			long cxnAccountId = PushCustomertoClient(RegistrationEvent.profile, RegistrationEvent.mgiContext, RegistrationEvent.profile.ProfileStatus);

			FISAccount account = FISAccountRepo.FindBy(x => x.Id == cxnAccountId);

			// By now the core fis account should have been created and corresponding accounts in CXE, CXN and PTNR should also have been done.
			// Let's create the funds account if the customer has an existing GPR card. 
			// we can get the CXN profile from tFIS_Account using the FIS Repo, populate the funds account object and prepare context.
			// call Biz.FundsEngine.Add, this in turn will create a funds account and will also trigger the gpr events.

			FundsAccount fundsaccount = Mapper.Map<FISAccount, FundsAccount>(account);
			Dictionary<string, object> context = RegistrationEvent.mgiContext.Context;

			//Customer.Data.Customer existingfiscustomer = ClientCustomerSvc.Fetch(context);

			//Fetch only from connectsDB
			//FISConnect objConnect = CXNCustomerSvc.GetSSNForCustomer(RegistrationEvent.profile.SSN);
			//Cxn.Customer.Data.CustomerProfile existingfiscustomer = AutoMapper.Mapper.Map<Cxn.Customer.FIS.Data.FISConnect, Cxn.Customer.Data.CustomerProfile>(objConnect);

			string RelationshipAccountNumber = string.Empty;
			string ProgramId = string.Empty;

			if (context.ContainsKey("FetchedFromCustomerLookUp") && (bool)context["FetchedFromCustomerLookUp"] == true)
			{
				if (context.ContainsKey("CustomerLookUpRelationshipAccountNumber") && context["CustomerLookUpRelationshipAccountNumber"] != null)
					RelationshipAccountNumber = context["CustomerLookUpRelationshipAccountNumber"].ToString();

				if (context.ContainsKey("CustomerLookUpProgramId") && context["CustomerLookUpProgramId"] != null)
					ProgramId = context["CustomerLookUpProgramId"].ToString();
			}

			if (!string.IsNullOrEmpty(RelationshipAccountNumber))
			{
				fundsaccount.AccountNumber = RelationshipAccountNumber;
				//AL-2460 (JIRA Id for TAS-175)
				//This is required if the customer was pulled from connects db, in that case we must use Program id from connectsdb.
				if (!string.IsNullOrWhiteSpace(ProgramId))
				{ 
					RegistrationEvent.mgiContext.TSysPartnerId = ProgramId;
				}

				RegistrationEvent.mgiContext.IsExistingAccount = true;

				BIZFundsEngineSvc.Add(-1, fundsaccount, RegistrationEvent.mgiContext);
			}
		}

		private long PushCustomertoClient(CustomerProfile customer, MGIContext mgiContext, ProfileStatus CXEProfileStatus)
		{
			mgiContext.SSN = customer.SSN;

			CxnCustomer cxnCustomer = Mapper.Map<CxnCustomer>(customer);

			//If the customer details are not populated using Customer Look up
			if (mgiContext.Context.ContainsKey("FetchedFromCustomerLookUp") && (bool)mgiContext.Context["FetchedFromCustomerLookUp"] == true)
			{
				//Get the details from the Customer Look up Search Result (Based on the customer chosen by Teller)
				cxnCustomer.PartnerAccountNumber = (string)mgiContext.Context["CustomerLookUpPartnerAccountNumber"];
				cxnCustomer.RelationshipAccountNumber = (string)mgiContext.Context["CustomerLookUpRelationshipAccountNumber"];
				cxnCustomer.BankId = (string)mgiContext.Context["CustomerLookUpBankId"];
				cxnCustomer.BranchId = (string)mgiContext.Context["CustomerLookUpBranchId"];
			}

			long cxnAccountId = CXNClientCustomerSvc.AddCXNAccount(cxnCustomer, mgiContext);

			//partner account details
			MGI.Core.Partner.Data.Customer ptnrCustomer = (MGI.Core.Partner.Data.Customer)mgiContext.Context["PTNRCustomer"];
			ptnrCustomer.AddAccount((mgiContext.ProviderId), (mgiContext.CXECustomerId), cxnAccountId);

			try
			{
				//Insert into FIS
				mgiContext.CxnAccountId = cxnAccountId;
				CXNClientCustomerSvc.Add(cxnCustomer, mgiContext);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				// to get the client Profile Status
				ProfileStatus cxnProfileStatus = CXNClientCustomerSvc.GetClientProfileStatus(cxnAccountId, mgiContext);
				//FISAccount account = FISAccountRepo.FindBy(x => x.Id == cxnAccountId);
				CXECustomer cxeCustomer = CxeCustomerService.Lookup(ptnrCustomer.Id);
				PTNRProspect ptnrProspect = PTNRCustomerService.LookupProspect(ptnrCustomer.Id);
				// to get the ClientCustomerId for the FIS Account
				string clientCustId = CXNClientCustomerSvc.GetClientCustID(cxnAccountId, mgiContext);

				ptnrCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				cxeCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				ptnrProspect.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);

				if (cxnProfileStatus == ProfileStatus.Active && !string.IsNullOrWhiteSpace(clientCustId))
				{
					// to get the ClientCustomerId for the FIS Account
					ptnrCustomer.CustomerProfileStatus = ProfileStatus.Active;
					cxeCustomer.ProfileStatus = ProfileStatus.Active;
					//Assigning ClientCustId to cxeCustomer
					cxeCustomer.ClientID = clientCustId;
					//Assigning ClientCustId to ptnrProspect
					ptnrProspect.ClientID = clientCustId;
					ptnrProspect.ProfileStatus = ProfileStatus.Active;

					//saving cxeCustomer with update ClientId into database
					CxeCustomerService.Save(cxeCustomer);
					//saving the ptnProspect with updated ClientId into Database
					PTNRCustomerService.SaveProspect(ptnrProspect);
				}
				else if (cxnProfileStatus == ProfileStatus.Inactive && string.IsNullOrWhiteSpace(clientCustId))
				{
					ptnrCustomer.CustomerProfileStatus = ProfileStatus.Inactive;
					cxeCustomer.ProfileStatus = ProfileStatus.Inactive;
					ptnrProspect.ProfileStatus = ProfileStatus.Inactive;
					//saving cxeCustomer with update ClientId into database
					CxeCustomerService.Save(cxeCustomer);
					//saving the ptnProspect with updated ClientId into Database
					PTNRCustomerService.SaveProspect(ptnrProspect);

				}
				PTNRCustomerService.Update(ptnrCustomer);
			}

			return cxnAccountId;

		}

	}
}
