using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Contract;
using MGI.Core.Partner.Contract;
using MGI.Cxn.Customer.FIS.Contract;
using MGI.Cxn.Customer.FIS.Data;
using MGI.Biz.FundsEngine.Data;
using MGI.Biz.FundsEngine.Contract;
using System.Collections.Generic;
using MGI.Biz.Events.Contract;
using MGI.Cxn.Customer.Contract;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using PTNRCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using CxeCustomerService = MGI.Core.CXE.Contract.ICustomerService;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using AutoMapper;
using MGI.Core.Partner.Data;
using MGI.Common.DataAccess.Contract;
using System;
using MGI.Common.Util;

namespace MGI.Biz.Synovus.Impl
{
	public class SynovusCustomerEditEventListener : INexxoBizEventListener
	{
		public IClientCustomerService CXNClientCustomerSvc { private get; set; }
		public IFISConnect CXNCustomerSvc { private get; set; }

		public CxeCustomerService CxeCustomerService { private get; set; }

		public IFundsEngine BIZFundsEngineSvc { private get; set; }

		public IRepository<FISAccount> FISAccountRepo { private get; set; }

		public ICustomerRepository ClientCustomerSvc { private get; set; }
		public PTNRCustomerService PTNRCustomerService { private get; set; }

		public void Notify(NexxoBizEvent BizEvent)
		{
			CustomerEditEvent EventData = (CustomerEditEvent)BizEvent;
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
			.ForMember(x => x.MothersMaidenName, s => s.MapFrom(c => c.MothersMaidenName))
			.ForMember(x => x.Gender, s => s.MapFrom(c => c.Gender))
			.ForMember(x => x.MothersMaidenName, s => s.MapFrom(c => c.MothersMaidenName))
			.ForMember(x => x.BankId, s => s.MapFrom(c => c.MetBankNumber))
			.ForMember(x => x.PartnerAccountNumber, s => s.MapFrom(c => c.CustomerNumber))
			.ForMember(x => x.RelationshipAccountNumber, s => s.MapFrom(c => c.ExternalKey))
			.ForMember(x => x.ProgramId, s => s.MapFrom(c => c.ProgramId));
			#endregion
			Mapper.CreateMap<FISAccount, FundsAccount>()
				.ForMember(x => x.AccountNumber, s => s.MapFrom(y => y.RelationshipAccountNumber)); // This should have the TSys card kit # for customers fetced from connects db.

			Update(EventData);
		}

		private void Update(CustomerEditEvent EditEvent)
		{
			PtnrCustomer ptnrCustomer = (PtnrCustomer)EditEvent.mgiContext.Context["PTNRCustomer"];

			long cxnAccountId = (ptnrCustomer.GetAccount(EditEvent.mgiContext.ProviderId) == null) ? 0 : ptnrCustomer.GetAccount(EditEvent.mgiContext.ProviderId).CXNId;

			FISAccount account = FISAccountRepo.FindBy(x => x.Id == cxnAccountId);

			//If core accout creation is not successful 
			if (account == null || account.ProfileStatus == ProfileStatus.Inactive && ptnrCustomer.CustomerProfileStatus != ProfileStatus.Closed)
			{
				//Update the partner profile status as false. This will be updated, if the FIS insertion is successful.
				ptnrCustomer.CustomerProfileStatus = ProfileStatus.Inactive;
				PTNRCustomerService.Update(ptnrCustomer);

				cxnAccountId = PushCustomertoClient(EditEvent.profile, EditEvent.mgiContext, EditEvent.profile.ProfileStatus);
			}
			else
			{
				CustomerProfile customerProfile = EditEvent.profile;
				//Map Biz customer profile to CXN customer profile
				CxnCustomer cxnCustomer = Mapper.Map<CxnCustomer>(customerProfile);

				try
				{
					//Update FIS and CXN by calling CXN client service
					CXNClientCustomerSvc.Update(cxnAccountId.ToString(), cxnCustomer, EditEvent.mgiContext);
				}
				catch (Exception ex)
				{
                    if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                    throw new BizCustomerException(BizCustomerException.CUSTOMER_UPDATE_FAILED, ex);
				}

				ptnrCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(EditEvent.mgiContext.TimeZone);

				ProfileStatus cxeProfileStatus = EditEvent.profile.ProfileStatus;

				//UPDATE CUSTOMER PROFILE STATUS IN PARTNER CUSTOMER BASED ON THE PROFILE STATUS IN FIS ACCOUNT & CXE
				if (account.ProfileStatus == ProfileStatus.Active && cxeProfileStatus == ProfileStatus.Active)
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

			long cxnAccountId;
			MGI.Core.Partner.Data.Customer ptnrCustomer = (MGI.Core.Partner.Data.Customer)mgiContext.Context["PTNRCustomer"];

			var partnerAccount = ptnrCustomer.GetAccount((int)ProviderIds.FIS);

			if (partnerAccount != null)
			{
				cxnAccountId = partnerAccount.CXNId;
				CXNClientCustomerSvc.Update(cxnAccountId.ToNullSafeString(), cxnCustomer, mgiContext);
			}
			else
			{
				cxnAccountId = CXNClientCustomerSvc.AddCXNAccount(cxnCustomer, mgiContext);
				ptnrCustomer.AddAccount((mgiContext.ProviderId), (mgiContext.CXECustomerId), cxnAccountId);
			}
			try
			{
				//Insert into FIS
				mgiContext.CxnAccountId = cxnAccountId;
				CXNClientCustomerSvc.Add(cxnCustomer, mgiContext);
			}
			catch (Exception ex)
			{
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_UPDATE_FAILED, ex);
			}
			finally
			{
				CXECustomer cxeCustomer = CxeCustomerService.Lookup(ptnrCustomer.Id);
				PTNRProspect ptnrProspect = PTNRCustomerService.LookupProspect(ptnrCustomer.Id);
				// to get the Client Profile Status
				ProfileStatus cxnProfileStatus = CXNClientCustomerSvc.GetClientProfileStatus(cxnAccountId, mgiContext);
				// to get the ClientCustomerId for the FIS Account
				string clientCustId = CXNClientCustomerSvc.GetClientCustID(cxnAccountId, mgiContext);
				//TODO: unreachable code need to do refactoring
				if (cxnProfileStatus == ProfileStatus.Active && !string.IsNullOrWhiteSpace(clientCustId))
				{
					// to get the ClientCustomerId for the FIS Account
					ptnrCustomer.CustomerProfileStatus = ProfileStatus.Active;
					cxeCustomer.ProfileStatus = ProfileStatus.Active;
					//Assigning ClientCustId to cxeCustomer
					cxeCustomer.ClientID = clientCustId;
					//Assigning ClientId to ptnrProspect
					ptnrProspect.ClientID = clientCustId;
					ptnrProspect.ProfileStatus = ProfileStatus.Active;
					ptnrCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
					cxeCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
					ptnrProspect.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
					PTNRCustomerService.Update(ptnrCustomer);
					//saving the CxeCustomer with Updated ClientId into database
					CxeCustomerService.Save(cxeCustomer);
					//saving the ptnProspect with updated ClientId into Database
					PTNRCustomerService.SaveProspect(ptnrProspect);
				}
			}
			return cxnAccountId;
		}

	}
}
