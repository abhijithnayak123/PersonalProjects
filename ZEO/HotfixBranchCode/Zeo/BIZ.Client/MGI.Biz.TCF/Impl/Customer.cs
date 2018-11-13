using AutoMapper;
using MGI.Biz.Customer.Contract;
using MGI.Biz.Customer.Data;
using MGI.Core.Partner.Contract;
using MGI.Cxn.Common.Processor.Contract;
using MGI.Cxn.Customer.Contract;
using MGI.Common.Util;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BizCustomerData = MGI.Biz.Customer.Data;
using CxnCustomerData = MGI.Cxn.Customer.Data;
using PtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using IPtnrDataStructureService = MGI.Core.Partner.Contract.INexxoDataStructuresService;
using MGI.Core.Partner.Data;

namespace MGI.Biz.TCF.Impl
{
	public class Customer : ICustomerRepository, IProcessor
	{

		public IClientCustomerService CxnClientCustomerService { private get; set; }

		public INexxoDataStructuresService PTNRIdTypeService { private get; set; }
				
		public Customer()
		{
			Mapper.CreateMap<CxnCustomerData.CustomerProfile, BizCustomerData.CustomerProfile>()
				.ForMember(x => x.ClientID, o => o.MapFrom(s => s.ClientID));
			Mapper.CreateMap<CxnCustomerData.CustomerProfile, BizCustomerData.Identification>()
				.ForMember(x => x.GovernmentId, o => o.MapFrom(s => s.GovernmentId))
				.ForMember(x => x.IDType, o => o.MapFrom(s => s.GovernmentIDType))
				.ForMember(x => x.Country, o => o.MapFrom(s => s.IDIssuingCountry))
				.ForMember(x => x.State, o => o.MapFrom(s => s.IDIssuingState))
				.ForMember(x => x.IssueDate, o => o.MapFrom(s => s.IDIssueDate))
				.ForMember(x => x.ExpirationDate, o => o.MapFrom(s => s.IDExpirationDate))
				.ForMember(x => x.CountryOfBirth, o => o.MapFrom(s => s.CountryOfBirth));

			Mapper.CreateMap<CxnCustomerData.CustomerProfile, BizCustomerData.EmploymentDetails>()
				.ForMember(x => x.Occupation, opt => opt.MapFrom(r => r.Occupation))
				.ForMember(x => x.OccupationDescription, opt => opt.MapFrom(r => r.OccupationDescription))
				.ForMember(x => x.Employer, opt => opt.MapFrom(r => r.EmployerName))
				.ForMember(x => x.EmployerPhone, opt => opt.MapFrom(r => r.EmployerPhone));

		}
		#region ICustomerRepository Implementation
		
		/// <summary>
		/// Search the customers by SSN, Account # or Card # from TCF
		/// </summary>
		/// <param name="context"> The context should have the following:
		/// 1. ChannelPartnerId
		/// 2. BankId
		/// </param>
		/// <returns></returns>
		public List<BizCustomerData.Customer> FetchAll(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
		{
			List<CxnCustomerData.CustomerProfile> cxnCustomerProfiles = CxnClientCustomerService.FetchAll(customerLookUpCriteria, cxnContext); ;
			List<MGI.Biz.Customer.Data.Customer> customers = new List<MGI.Biz.Customer.Data.Customer>();

			if (cxnCustomerProfiles != null && cxnCustomerProfiles.Count > 0)
			{
				foreach (var customerProfile in cxnCustomerProfiles)
				{
					BizCustomerData.Customer mappingRecord = new BizCustomerData.Customer();
					mappingRecord.Profile = Mapper.Map<BizCustomerData.CustomerProfile>(customerProfile);
					mappingRecord.ID = Mapper.Map<BizCustomerData.Identification>(customerProfile);
					mappingRecord.Employment = Mapper.Map<BizCustomerData.EmploymentDetails>(customerProfile);
					mappingRecord.ID.State = PTNRIdTypeService.GetIDState(mappingRecord.ID.Country, mappingRecord.ID.State);

					customers.Add(mappingRecord);
				}

			}
			else
			{
				customers = new List<BizCustomerData.Customer>();
			}
			return customers;
		}

		/// <summary>
		/// Validate Customer against TCIS
		/// </summary>
		/// <param name="SSN"></param>
		/// <param name="context"></param>
		public void ValidateCustomerStatus(long agentSessionId, long CXNId, MGIContext context)
		{
			CxnClientCustomerService.ValidateCustomerStatus(CXNId, context);
		}

		/// <summary>
		/// Get Client Profile status
		/// </summary>
		/// <param name="cxnAccountId"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public ProfileStatus GetClientProfileStatus(long agentSessionId, long cxnAccountId, MGIContext context)
		{
			return CxnClientCustomerService.GetClientProfileStatus(cxnAccountId, context);
		}

		public bool ValidateCustomerRequiredFields(long agentSessionId, BizCustomerData.Customer customer, MGIContext context)
		{
			Regex nameRgx = new Regex("[^A-Za-z\\-' ]");

			if (string.IsNullOrEmpty(customer.Profile.FirstName) || nameRgx.IsMatch(customer.Profile.FirstName ?? ""))
				return false;
			if (string.IsNullOrEmpty(customer.Profile.LastName) || nameRgx.IsMatch(customer.Profile.LastName ?? ""))
				return false;
			if (string.IsNullOrEmpty(customer.Profile.MothersMaidenName) || nameRgx.IsMatch(customer.Profile.MothersMaidenName ?? ""))
				return false;
			if (string.IsNullOrEmpty(customer.Profile.Gender))
				return false;
			if (string.IsNullOrEmpty(customer.Profile.Phone1) && customer.Profile.Phone1.Length < 10)
				return false;
			if (string.IsNullOrEmpty(customer.Profile.Phone1Type))
				return false;
			else if (customer.Profile.Phone1Type == "CELL" && !string.IsNullOrEmpty(customer.Profile.Phone1Provider))
				return false;

			if (!string.IsNullOrEmpty(customer.Profile.Phone2) && customer.Profile.Phone2.Length < 10)
				return false;
			else if (!string.IsNullOrEmpty(customer.Profile.Phone2Type) && customer.Profile.Phone2Type == "CELL" && string.IsNullOrEmpty(customer.Profile.Phone2Provider))
				return false;

			Regex cityRgx = new Regex("[^A-Za-z ]");
			Regex zipRgx = new Regex("\\d{5}");

			if (string.IsNullOrEmpty(customer.Profile.Address1))
				return false;
			if (string.IsNullOrEmpty(customer.Profile.City) || cityRgx.IsMatch(customer.Profile.City.ToUpper() ?? ""))
				return false;
			if (string.IsNullOrEmpty(customer.Profile.ZipCode) || !zipRgx.IsMatch(customer.Profile.ZipCode ?? ""))
				return false;

			if (customer.Profile.MailingAddressDifferent == true)
			{
				if (string.IsNullOrEmpty(customer.Profile.MailingAddress1))
					return false;
				if (string.IsNullOrEmpty(customer.Profile.MailingState))
					return false;
				if (string.IsNullOrEmpty(customer.Profile.MailingCity) || nameRgx.IsMatch(customer.Profile.MailingCity.ToUpper() ?? ""))
					return false;
				if (string.IsNullOrEmpty(customer.Profile.MailingZipCode) || !zipRgx.IsMatch(customer.Profile.MailingZipCode ?? ""))
					return false;
			}

			if (string.IsNullOrEmpty(customer.ID.CountryOfBirth))
				return false;

			if (customer.Profile.DateOfBirth == null)
				return false;

			if (string.IsNullOrEmpty(customer.ID.Country))
				return false;

			if (string.IsNullOrEmpty(customer.ID.IDType))
				return false;
			else if (customer.ID.IDType == "DRIVER'S LICENSE" && (string.IsNullOrEmpty(customer.ID.State) || customer.ID.IssueDate == null))
				return false;
			else if (customer.ID.IDType == "U.S. STATE IDENTITY CARD" && string.IsNullOrEmpty(customer.ID.State))
				return false;

			if (string.IsNullOrEmpty(customer.ID.GovernmentId))
				return false;

			if (customer.ID.ExpirationDate == null)
				return false;

			if (string.IsNullOrEmpty(customer.Profile.LegalCode))
				return false;

			if (string.IsNullOrEmpty(customer.Profile.PrimaryCountryCitizenship))
				return false;

			if (string.IsNullOrEmpty(customer.Employment.Occupation))
				return false;

			if (string.IsNullOrEmpty(customer.Profile.PIN))
				return false;

			return true;
		}
		#endregion
				

	}
}