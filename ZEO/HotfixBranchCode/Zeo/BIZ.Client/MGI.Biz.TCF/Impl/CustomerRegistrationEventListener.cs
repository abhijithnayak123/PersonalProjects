using AutoMapper;
using MGI.Biz.Customer.Data;
using MGI.Biz.Events.Contract;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using PtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using CxeCustomerService = MGI.Core.CXE.Contract.ICustomerService;
using IPtnrDataStructureService = MGI.Core.Partner.Contract.INexxoDataStructuresService;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGI.Cxn.Customer.TCIS.Data;
using MGI.Cxn.Customer.Contract;
using MGI.Common.Util;

namespace MGI.Biz.TCF.Impl
{
	public class CustomerRegistrationEventListener : INexxoBizEventListener
	{
		public IClientCustomerService CxnClientCustomerService { private get; set; }

		public PtnrCustomerService PartnerCustomerService { private get; set; }

		public CxeCustomerService CxeCustomerService { private get; set; }

		public IPtnrDataStructureService PTNRDataStructureService { private get; set; }

		public void Notify(NexxoBizEvent bizEvent)
		{
			CustomerRegistrationEvent EventData = (CustomerRegistrationEvent)bizEvent;
			Mapper.CreateMap<CustomerProfile, CxnCustomer>();
			Register(EventData);
		}

		private void Register(CustomerRegistrationEvent registrationEvent)
		{
			// the profile is present in the event. We can now do anything.
			// 2. call TCIS through CXN.
			// 2.2 call TCIS to the following:
			// 2.3 Open Misc Account and get the external client id.
			// 2.4 Update the external client id to NexxoCustomer account in CXN Funds?
			// 
			// 1. Lookup the ConnectDb and if GPR, register with Nexxo (Link GPR usecase)
			// 1.1 Do the fetch again and see if the customer profile has external key, if so associate the card # directly to the newly created profile.
			// 3 . anything else??? 

			long cxnAccountId = PushCustomertoClient(registrationEvent.profile, registrationEvent.mgiContext, registrationEvent.profile.ProfileStatus);
		}

		private long PushCustomertoClient(CustomerProfile customer, MGIContext mgiContext, ProfileStatus profileStatus)
		{
			mgiContext.SSN = customer.SSN;

			//CxnCustomer existingTcisCustomer = null;
			CxnCustomer cxnCustomer = Mapper.Map<CxnCustomer>(customer);

			cxnCustomer.EmployerName = customer.Employer;
			cxnCustomer.IDIssuingState = customer.IDIssuingStateAbbr;

			if (customer.GovernmentIDType.ToUpper() == "PASSPORT")
			{
				cxnCustomer.IDIssuingState = customer.IDIssuingCountryId;
			}
			else
			{
				cxnCustomer.IDIssuingState = customer.IDIssuingStateAbbr;
			}

			cxnCustomer.PrimaryCountryCitizenShip = BaseClass.GetMasterCountryName(customer.PrimaryCountryCitizenship);
			cxnCustomer.SecondaryCountryCitizenShip = BaseClass.GetMasterCountryName(customer.SecondaryCountryCitizenship);

			Dictionary<string, string> fullName = BaseClass.TruncateFullName(cxnCustomer.FirstName, cxnCustomer.MiddleName, cxnCustomer.LastName, cxnCustomer.LastName2);

			cxnCustomer.FirstName = fullName["FirstName"];
			cxnCustomer.MiddleName = fullName["MiddleName"];
			cxnCustomer.LastName = fullName["LastName"];
			cxnCustomer.LastName2 = fullName["SecondLastName"];

			//For new customer registration TCF expect ClientCustID as 0
			if (string.IsNullOrWhiteSpace(cxnCustomer.ClientID))
			{
				cxnCustomer.ClientID = "0";
			}

			long cxnAccountId;
			PtnrCustomer ptnrCustomer = (PtnrCustomer)mgiContext.Context["PTNRCustomer"];

			//If the customer details are not populated using Customer Look up
			cxnAccountId = CxnClientCustomerService.AddCXNAccount(cxnCustomer, mgiContext);


			ptnrCustomer.AddAccount((mgiContext.ProviderId), (mgiContext.CXECustomerId), cxnAccountId);
			PartnerCustomerService.Update(ptnrCustomer);

			try
			{
				//Insert into TCIS
				mgiContext.CxnAccountId = cxnAccountId;
				CxnClientCustomerService.Add(cxnCustomer, mgiContext);
			}
			catch (ClientCustomerException ex)
			{
				throw ex;
			}
			finally
			{
				ProfileStatus cxnProfileStatus = CxnClientCustomerService.GetClientProfileStatus(cxnAccountId, mgiContext);

				CXECustomer cxeCustomer = CxeCustomerService.Lookup(ptnrCustomer.Id);
				PTNRProspect ptnrProspect = PartnerCustomerService.LookupProspect(ptnrCustomer.Id);
				cxeCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				ptnrProspect.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);

				//UPDATE CUSTOMER PROFILE STATUS IN PARTNER CUSTOMER BASED ON THE PROFILE STATUS IN TCIS ACCOUNT
				if (cxnProfileStatus == ProfileStatus.Active)
				{
					string clientCustId = CxnClientCustomerService.GetClientCustID(cxnAccountId, mgiContext);
					if (clientCustId != "0")
					{
						ptnrCustomer.CustomerProfileStatus = ProfileStatus.Active;
						cxeCustomer.ProfileStatus = ProfileStatus.Active;
						cxeCustomer.ClientID = clientCustId;
						ptnrProspect.ClientID = clientCustId;
						ptnrProspect.ProfileStatus = ProfileStatus.Active;
					}
					else
					{
						ptnrCustomer.CustomerProfileStatus = ProfileStatus.Inactive;
						cxeCustomer.ProfileStatus = ProfileStatus.Inactive;
						ptnrProspect.ProfileStatus = ProfileStatus.Inactive;
					}
				}
				else
				{
					ptnrCustomer.CustomerProfileStatus = ProfileStatus.Inactive;
					cxeCustomer.ProfileStatus = ProfileStatus.Inactive;
					ptnrProspect.ProfileStatus = ProfileStatus.Inactive;

				}

				PartnerCustomerService.Update(ptnrCustomer);
				CxeCustomerService.Save(cxeCustomer);
				PartnerCustomerService.SaveProspect(ptnrProspect);
			}

			return cxnAccountId;

		}


	}
}
