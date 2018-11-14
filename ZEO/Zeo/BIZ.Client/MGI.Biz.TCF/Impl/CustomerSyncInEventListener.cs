using AutoMapper;
using MGI.Biz.Customer.Data;
using MGI.Biz.Events.Contract;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using CxeCustomerService = MGI.Core.CXE.Contract.ICustomerService;
using PtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using CxeCustomer = MGI.Core.CXE.Data.Customer;
using CxnCustomerData = MGI.Cxn.Customer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.Customer.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.Partner.Data;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using NexxoIdTypeDto = MGI.Core.Partner.Data.NexxoIdType;
using MGI.Common.DataAccess.Contract;
using System.Text.RegularExpressions;
using MGI.Common.Util;

namespace MGI.Biz.TCF.Impl
{
	public class CustomerSyncInEventListener : INexxoBizEventListener
	{
		public IClientCustomerService CxnClientCustomerService { private get; set; }

		public CxeCustomerService CxeCustomerService { private get; set; }

		public PtnrCustomerService PartnerCustomerService { private get; set; }

		public INexxoDataStructuresService PtnrIdTypeService { private get; set; }

		public IRepository<MGI.Cxn.Customer.TCIS.Data.Account> TCISAccountRepo { private get; set; }

		public void Notify(NexxoBizEvent bizEvent)
		{
			CustomerSyncInEvent EventData = (CustomerSyncInEvent)bizEvent;

			SyncIn(EventData);
		}

		private NexxoIdTypeDto GetIdType(long channelPartnerId, Identification prospectID)
		{
			NexxoIdTypeDto idType = null;
			if (prospectID != null && !string.IsNullOrEmpty(prospectID.Country) && !string.IsNullOrEmpty(prospectID.IDType))
			{
				idType = PtnrIdTypeService.Find(channelPartnerId, prospectID.IDType, prospectID.Country, prospectID.State);
			}
			return idType;
		}

		private void SyncIn(CustomerSyncInEvent SyncInEvent)
		{
			SyncInEvent.mgiContext.CxnAccountId = SyncInEvent.cxnCustomerId;

			CxnCustomerData.CustomerProfile cxnCustomerProfiles = null;

			try
			{
				cxnCustomerProfiles = CxnClientCustomerService.Fetch(SyncInEvent.mgiContext);
			}
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCustomerException(BizCustomerException.CUSTOMER_SYNC_IN_FAILED, ex);
            }

	
			if (cxnCustomerProfiles != null)
			{
				cxnCustomerProfiles.IDIssuingState = PtnrIdTypeService.GetIDState(cxnCustomerProfiles.IDIssuingCountry, cxnCustomerProfiles.IDIssuingState);

				PtnrCustomer ptnrCustomer = (PtnrCustomer)SyncInEvent.mgiContext.Context["PTNRCustomer"];

				CxeCustomer cxeCustomer = CxeCustomerService.Lookup(ptnrCustomer.Id);
				Prospect ptnrProspect = PartnerCustomerService.LookupProspect(ptnrCustomer.Id);

				cxeCustomer = MapToCxeCustomer(cxeCustomer, cxnCustomerProfiles);

				Identification prospectID = new Identification
				{
 					Country = cxnCustomerProfiles.IDIssuingCountry,
					State = cxnCustomerProfiles.IDIssuingState,
					IDType = cxnCustomerProfiles.GovernmentIDType
				};

				ptnrProspect = MapToProspect(ptnrProspect, cxeCustomer, prospectID);
				MGI.Cxn.Customer.TCIS.Data.Account tcisAccountDetails = TCISAccountRepo.FindBy(x => x.Id == Convert.ToInt64(SyncInEvent.mgiContext.CxnAccountId));
				cxnCustomerProfiles = MapToCxnCustomerProfile(tcisAccountDetails, cxnCustomerProfiles, cxeCustomer);

				CxnClientCustomerService.Update(Convert.ToString(SyncInEvent.cxnCustomerId), cxnCustomerProfiles, SyncInEvent.mgiContext);

				CxeCustomerService.Save(cxeCustomer);

				PartnerCustomerService.SaveProspect(ptnrProspect);
				
			}
		}

		private CxnCustomerData.CustomerProfile MapToCxnCustomerProfile(MGI.Cxn.Customer.TCIS.Data.Account tcisAccountDetails, CxnCustomer cxnCustomerProfiles, CxeCustomer cxeCustomer)
		{
			cxnCustomerProfiles.FirstName = cxeCustomer.FirstName;
			cxnCustomerProfiles.MiddleName = cxeCustomer.MiddleName;
			cxnCustomerProfiles.LastName = cxeCustomer.LastName;
			cxnCustomerProfiles.LastName2 = cxeCustomer.LastName2;
			cxnCustomerProfiles.Address1 = cxeCustomer.Address1;
			cxnCustomerProfiles.Address2 = cxeCustomer.Address2;
			cxnCustomerProfiles.City = cxeCustomer.City;
			cxnCustomerProfiles.State = cxeCustomer.State;
			cxnCustomerProfiles.ZipCode = cxeCustomer.ZipCode;
			cxnCustomerProfiles.Phone1 = cxeCustomer.Phone1;
			cxnCustomerProfiles.Phone1Provider = cxeCustomer.Phone1Provider;
			cxnCustomerProfiles.Phone2 = cxeCustomer.Phone2;
			cxnCustomerProfiles.Phone2Provider = cxeCustomer.Phone2Provider;
			cxnCustomerProfiles.Phone2Type = cxeCustomer.Phone2Type;
			cxnCustomerProfiles.SSN = cxeCustomer.SSN;
			cxnCustomerProfiles.CountryOfBirth = cxeCustomer.CountryOfBirth;
			cxnCustomerProfiles.DateOfBirth = cxeCustomer.DateOfBirth;
			cxnCustomerProfiles.Gender = cxeCustomer.Gender;
			cxnCustomerProfiles.MothersMaidenName = cxeCustomer.MothersMaidenName;
			cxnCustomerProfiles.ProfileStatus = cxeCustomer.ProfileStatus;
			cxnCustomerProfiles.PartnerAccountNumber = cxeCustomer.ClientID;
			cxnCustomerProfiles.CustInd = tcisAccountDetails.TcfCustInd;
            cxnCustomerProfiles.IDCode = cxeCustomer.IDCode;
			return cxnCustomerProfiles;
		}


		private CxeCustomer MapToCxeCustomer(CxeCustomer cxeCustomer, CxnCustomer cxnCustomerProfiles)
		{

			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.FirstName))
			{
				cxeCustomer.FirstName = cxnCustomerProfiles.FirstName;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.MiddleName))
			{
				cxeCustomer.MiddleName = cxnCustomerProfiles.MiddleName;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.LastName))
			{
				cxeCustomer.LastName = cxnCustomerProfiles.LastName;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Address1))
			{
				cxeCustomer.Address1 = cxnCustomerProfiles.Address1;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Address2))
			{
				cxeCustomer.Address2 = cxnCustomerProfiles.Address2;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.City))
			{
				cxeCustomer.City = cxnCustomerProfiles.City;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.State))
			{
				cxeCustomer.State = cxnCustomerProfiles.State;
			}
			if (BaseClass.IsValidZipCode(cxnCustomerProfiles.ZipCode))
			{
				cxeCustomer.ZipCode = cxnCustomerProfiles.ZipCode;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone1) && cxnCustomerProfiles.Phone1.Length == 10 && PhoneNumberCheck(cxnCustomerProfiles.Phone1) > 1)
			{
				cxeCustomer.Phone1 = cxnCustomerProfiles.Phone1;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone1Provider))
			{
				cxeCustomer.Phone1Provider = cxnCustomerProfiles.Phone1Provider;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone1Type))
			{
				cxeCustomer.Phone1Type = cxnCustomerProfiles.Phone1Type;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone2))
			{
				cxeCustomer.Phone2Provider = cxnCustomerProfiles.Phone2;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone2) && cxnCustomerProfiles.Phone2.Length == 10 && PhoneNumberCheck(cxnCustomerProfiles.Phone2) > 1)
			{
				cxeCustomer.Phone2Type = cxnCustomerProfiles.Phone2;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.SSN) && cxnCustomerProfiles.SSN.Length == 9)
			{
				cxeCustomer.SSN = cxnCustomerProfiles.SSN;
			}
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.IDCode))
            {
                cxeCustomer.IDCode = cxnCustomerProfiles.IDCode;
            }
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.CountryOfBirth))
			{
				cxeCustomer.CountryOfBirth = cxnCustomerProfiles.CountryOfBirth;
			}
			if (cxnCustomerProfiles.DateOfBirth != null && cxnCustomerProfiles.DateOfBirth != DateTime.MinValue)
			{
				cxeCustomer.DateOfBirth = cxnCustomerProfiles.DateOfBirth;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Email))
			{
				cxeCustomer.Email = cxnCustomerProfiles.Email;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Gender))
			{
				cxeCustomer.Gender = cxnCustomerProfiles.Gender;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.MothersMaidenName))
			{
				cxeCustomer.MothersMaidenName = cxnCustomerProfiles.MothersMaidenName;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.PrimaryCountryCitizenShip))
			{
				cxeCustomer.PrimaryCountryCitizenShip = cxnCustomerProfiles.PrimaryCountryCitizenShip;
			}
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.LegalCode))
            {
                cxeCustomer.LegalCode = cxnCustomerProfiles.LegalCode;
            }
			if (cxnCustomerProfiles.GovernmentIDType != null && cxnCustomerProfiles.IDIssuingCountry != null)
			{
				NexxoIdType idType = PtnrIdTypeService.Find(cxeCustomer.ChannelPartnerId, cxnCustomerProfiles.GovernmentIDType, cxnCustomerProfiles.IDIssuingCountry, cxnCustomerProfiles.IDIssuingState);
				if (idType != null)
				{
					cxeCustomer.GovernmentId.IdTypeId = idType.Id;

					if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.GovernmentId) && cxnCustomerProfiles.GovernmentId != "0")
					{
						cxeCustomer.GovernmentId.Identification = cxnCustomerProfiles.GovernmentId;
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.EmployerName))
			{
				cxeCustomer.EmploymentDetails.Employer = cxnCustomerProfiles.EmployerName;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.EmployerPhone))
			{
				cxeCustomer.EmploymentDetails.EmployerPhone = cxnCustomerProfiles.EmployerPhone;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Occupation))
			{
				cxeCustomer.EmploymentDetails.Occupation = cxnCustomerProfiles.Occupation;
			}
			if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.OccupationDescription))
			{
				cxeCustomer.EmploymentDetails.OccupationDescription = cxnCustomerProfiles.OccupationDescription;
			}

			return cxeCustomer;
		}

		private Prospect MapToProspect(Prospect ptnrProspect, CxeCustomer cxeCustomer, Identification prospectID)
		{
			ptnrProspect.FirstName = cxeCustomer.FirstName;
			ptnrProspect.MiddleName = cxeCustomer.MiddleName;
			ptnrProspect.LastName = cxeCustomer.LastName;
			ptnrProspect.Address1 = cxeCustomer.Address1;
			ptnrProspect.Address2 = cxeCustomer.Address2;
			ptnrProspect.City = cxeCustomer.City;
			ptnrProspect.State = cxeCustomer.State;
			ptnrProspect.ZipCode = cxeCustomer.ZipCode;
			ptnrProspect.Phone1 = cxeCustomer.Phone1;
			ptnrProspect.Phone1Provider = cxeCustomer.Phone1Provider;
			ptnrProspect.Phone1Type = cxeCustomer.Phone1Type;
			ptnrProspect.Phone2Type = cxeCustomer.Phone2;
			ptnrProspect.Phone1Provider = cxeCustomer.Phone2Provider;
			ptnrProspect.Phone2Type = cxeCustomer.Phone2Type;
			ptnrProspect.SSN = cxeCustomer.SSN;
			ptnrProspect.Email = cxeCustomer.Email;
			ptnrProspect.CountryOfBirth = cxeCustomer.CountryOfBirth;
			ptnrProspect.DateOfBirth = cxeCustomer.DateOfBirth;
			ptnrProspect.Gender = cxeCustomer.Gender;
			ptnrProspect.MothersMaidenName = cxeCustomer.MothersMaidenName;
			ptnrProspect.PrimaryCountryCitizenShip = cxeCustomer.PrimaryCountryCitizenShip;
			NexxoIdTypeDto prospectId = GetIdType(cxeCustomer.ChannelPartnerId, prospectID);
            ptnrProspect.LegalCode = cxeCustomer.LegalCode;
			ptnrProspect.GovernmentId.IdType = prospectId != null ? prospectId : ptnrProspect.GovernmentId.IdType;
			ptnrProspect.GovernmentId.Identification = cxeCustomer.GovernmentId.Identification;
			ptnrProspect.EmploymentDetails.Employer = cxeCustomer.EmploymentDetails.Employer;
			ptnrProspect.EmploymentDetails.EmployerPhone = cxeCustomer.EmploymentDetails.EmployerPhone;
			ptnrProspect.EmploymentDetails.Occupation = cxeCustomer.EmploymentDetails.Occupation;
			ptnrProspect.EmploymentDetails.OccupationDescription = cxeCustomer.EmploymentDetails.OccupationDescription;
			ptnrProspect.ProfileStatus = cxeCustomer.ProfileStatus;
			ptnrProspect.IDCode = cxeCustomer.IDCode;
			return ptnrProspect;
		}

		public int PhoneNumberCheck(string phoneNumber)
		{
			int count = 0;
			if (!string.IsNullOrWhiteSpace(phoneNumber))
			{
				count = Regex.Matches(phoneNumber, @"[1-9]").Count;
			}
			return count;
		}

	}
}
