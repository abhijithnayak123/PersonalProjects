using MGI.Biz.Events.Contract;
using MGI.Cxn.Customer.Contract;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using PtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using CxeCustomerService = MGI.Core.CXE.Contract.ICustomerService;
using IPtnrDataStructureService = MGI.Core.Partner.Contract.INexxoDataStructuresService;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using System.Collections.Generic;
using MGI.Biz.Customer.Data;
using AutoMapper;
using MGI.Core.Partner.Contract;
using MGI.Common.Util;
using System;

namespace MGI.Biz.TCF.Impl
{
	public class CustomerEditEventListener : INexxoBizEventListener
	{
		public IClientCustomerService CxnClientCustomerService { private get; set; }

		public PtnrCustomerService PartnerCustomerService { private get; set; }

		public CxeCustomerService CxeCustomerService { private get; set; }

		public IPtnrDataStructureService PTNRDataStructureService { private get; set; }

		public void Notify(NexxoBizEvent BizEvent)
		{
			CustomerEditEvent EventData = (CustomerEditEvent)BizEvent;
			Mapper.CreateMap<CustomerProfile, CxnCustomer>();
			Update(EventData);
		}

		private void Update(CustomerEditEvent EditEvent)
		{
			PtnrCustomer ptnrCustomer = (PtnrCustomer)EditEvent.mgiContext.Context["PTNRCustomer"];

			ptnrCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(EditEvent.mgiContext.TimeZone);
			PartnerCustomerService.Update(ptnrCustomer);

			long cxnAccountId = PushCustomertoClient(EditEvent.profile, EditEvent.mgiContext, EditEvent.profile.ProfileStatus);

		}

		private long PushCustomertoClient(CustomerProfile customer, MGIContext mgiContext, ProfileStatus cxeProfileStatus)
		{
			mgiContext.SSN = customer.SSN;

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

			if (string.IsNullOrWhiteSpace(cxnCustomer.ClientID))
			{
				cxnCustomer.ClientID = "0";
			}

			long cxnAccountId;
			PtnrCustomer ptnrCustomer = (PtnrCustomer)mgiContext.Context["PTNRCustomer"];

			var partnerAccount = ptnrCustomer.GetAccount((int)ProviderIds.TCISCustomer);


			if (partnerAccount != null)
			{
				cxnAccountId = partnerAccount.CXNId;

				ProfileStatus cxnProfileStatus = CxnClientCustomerService.GetClientProfileStatus(cxnAccountId, mgiContext);
				CXECustomer cxeCustomer = CxeCustomerService.Lookup(ptnrCustomer.Id);

				if (cxnProfileStatus == ProfileStatus.Inactive && ptnrCustomer.CustomerProfileStatus != ProfileStatus.Closed && cxeCustomer.ProfileStatus != ProfileStatus.Closed)
				{
					try
					{
						//Insert into TCIS
						mgiContext.CxnAccountId = cxnAccountId;
						CxnClientCustomerService.Add(cxnCustomer, mgiContext);
					}
					catch (Exception ex)
					{
						if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                        throw new BizCustomerException(BizCustomerException.CUSTOMER_UPDATE_FAILED, ex);
					}
					finally
					{

						cxnProfileStatus = CxnClientCustomerService.GetClientProfileStatus(cxnAccountId, mgiContext);

						PTNRProspect ptnrProspect = PartnerCustomerService.LookupProspect(ptnrCustomer.Id);
						cxeCustomer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
						ptnrProspect.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);

						//UPDATE CUSTOMER PROFILE STATUS IN PARTNER CUSTOMER BASED ON THE PROFILE STATUS IN TCIS ACCOUNT
						if (cxnProfileStatus == ProfileStatus.Active)
						{
							string clientCustId = CxnClientCustomerService.GetClientCustID(cxnAccountId, mgiContext);
							if (clientCustId != null)
							{
								cxeCustomer.ClientID = clientCustId;
								ptnrProspect.ClientID = clientCustId;
								ptnrCustomer.CustomerProfileStatus = ProfileStatus.Active;
								cxeCustomer.ProfileStatus = ProfileStatus.Active;
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
				}
				else
				{
					bool tcfCustInd = CxnClientCustomerService.GetCustInd(cxnAccountId, mgiContext);
					cxnCustomer.CustInd = tcfCustInd;
					cxnCustomer.ProfileStatus = cxnProfileStatus;
					CxnClientCustomerService.Update(cxnAccountId.ToString(), cxnCustomer, mgiContext);
				}
			}
			else
			{
				cxnAccountId = CxnClientCustomerService.AddCXNAccount(cxnCustomer, mgiContext);
				ptnrCustomer.AddAccount((mgiContext.ProviderId), (mgiContext.CXECustomerId), cxnAccountId);
				PartnerCustomerService.Update(ptnrCustomer);
			}
			return cxnAccountId;
		}

	}
}
