using System;
using System.Diagnostics;
using System.Linq.Expressions;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

using MGI.Common.DataAccess.Contract;
using System.Collections.Generic;
using System.Linq;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.Partner.Impl
{
	public class NexxoDataStructuresServiceImpl : INexxoDataStructuresService
	{
		private IRepository<ChannelPartnerIdTypesMapping> _idTypeRepo;
		public IRepository<ChannelPartnerIdTypesMapping> IdTypeRepo
		{
			set { _idTypeRepo = value; }
		}

		private IRepository<Country> _countryRepo;
		public IRepository<Country> CountryRepo
		{
			set { _countryRepo = value; }
		}

		private IRepository<State> _stateRepo;
		public IRepository<State> StateRepo
		{
			set { _stateRepo = value; }
		}

		private IRepository<ContactType> _contactTypeRepo;
		public IRepository<ContactType> ContactTypeRepo
		{
			set { _contactTypeRepo = value; }
		}

		private IRepository<NotificationHost> _notificationHostRepo;
		public IRepository<NotificationHost> NotificationHostRepo
		{
			set { _notificationHostRepo = value; }
		}

		public IRepository<LegalCode> LegalCodeRepo { private get; set; }

		public IRepository<Occupation> OccupationRepo { private get; set; }

		private IRepository<ChannelPartnerMasterCountryMapping> _masterCountryRepo;
		public IRepository<ChannelPartnerMasterCountryMapping> MasterCountryRepo
		{
			set { _masterCountryRepo = value; }
		}

		public TLoggerCommon MongoDBLogger { get; set; }

		public NexxoIdType Find(long channelPartnerId, string name, string country, string state)
		{
			try
			{
				Expression<Func<ChannelPartnerIdTypesMapping, bool>> findExp;

				if (string.IsNullOrEmpty(state))
					findExp = x => x.ChannelPartner.Id == channelPartnerId && x.NexxoIdType.Name == name && x.NexxoIdType.CountryId.Name == country && x.NexxoIdType.StateId == null && x.NexxoIdType.IsActive == true;
				else
					findExp = x => x.ChannelPartner.Id == channelPartnerId && x.NexxoIdType.Name == name && x.NexxoIdType.CountryId.Name == country && x.NexxoIdType.StateId.Name == state && x.NexxoIdType.IsActive == true;

				var ids = _idTypeRepo.FindBy(findExp);

				return ids == null ? null : ids.NexxoIdType;
			}
			catch (Exception ex)
			{
                throw new PartnerCustomerException(PartnerCustomerException.GET_NEXXOIDTYPE_FAILED, ex);
			}
		}

		public NexxoIdType Find(long channelPartnerId, long idTypeId)
		{
			// 2080 Changes
			try
			{
				ChannelPartnerIdTypesMapping _channelPartnerIdTypesMapping = _idTypeRepo.FindBy(c => c.ChannelPartner.Id == channelPartnerId && c.NexxoIdType.Id == idTypeId);
				if (_channelPartnerIdTypesMapping != null)
				{
					return _channelPartnerIdTypesMapping.NexxoIdType;
				}
				else
					return null;
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_NEXXOIDTYPE_FAILED, ex);
			}
		}

		public List<string> Countries()
		{
			try
			{
                List<string> countries = _countryRepo.All().Select(c => c.Name).ToList();
                return countries;
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_COUNTRIES_FAILED, ex);
			}
		}

		public List<string> States(string country)
		{
			try
			{
				var _country = _countryRepo.FindBy(c => c.Name == country);
				var countryCode = _country.Code.ToString();
				var statesList = _stateRepo.FilterBy(c => c.CountryCode == countryCode);
                List<string> states = statesList.Select(c => c.Name).ToList();
                return states;
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_STATES_FAILED, ex);
			}
		}

		public List<string> USStates()
		{
			try
			{
                var USstates = _stateRepo.FilterBy(c => c.CountryCode == "840").Select(c => c.Abbr);
                return USstates.ToList();
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_USSTATES_FAILED, ex);
			}
		}

		public List<string> IdCountries(long channelPartnerId)
		{
			try
			{
				var idCountries = _idTypeRepo.FilterBy(x => x.ChannelPartner.Id == channelPartnerId && x.IsActive == true).Select(c => c.NexxoIdType.CountryId.Name).Distinct();
                return idCountries.ToList();
			}
			catch (Exception ex)
			{				
				throw new PartnerCustomerException(PartnerCustomerException.GET_IDCOUNTRIES_FAILED, ex);
			}
		}

		public List<string> IdStates(long channelPartnerId, string country, string idType)
		{
			try
			{
				var idStates = _idTypeRepo.FilterBy(c => c.ChannelPartner.Id == channelPartnerId && c.NexxoIdType.CountryId.Name == country && c.NexxoIdType.Name == idType && c.IsActive == true).Select(c => c.NexxoIdType.StateId.Name);
                return idStates.ToList();
			}
			catch (Exception ex)
			{				
				throw new PartnerCustomerException(PartnerCustomerException.GET_IDSTATES_FAILED, ex);
			}
		}

		public List<string> IdTypes(long channelPartnerId, string country)
		{
			List<string> idTypes = new List<string>();
			try
			{
				var queryableIdTypes = _idTypeRepo.FilterBy(c => c.ChannelPartner.Id == channelPartnerId && c.NexxoIdType.CountryId.Name == country && c.IsActive == true).Select(c => c.NexxoIdType.Name);
				var distinctIdTypes = queryableIdTypes.Distinct();
				if (distinctIdTypes != null)
				{
					idTypes = distinctIdTypes.ToList();
				}
                return idTypes;
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_IDTYPES_FAILED, ex);
			}
		}

		public List<string> PhoneTypes()
		{
			try
			{
				var queryablephoneTypes = _contactTypeRepo.FilterBy(c => c.Id < 5);
                return queryablephoneTypes.Select(c => c.Type).ToList();
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_PHONETYPES_FAILED, ex);
			}
		}

		public List<string> MobileProviders()
		{
			try
			{
				var mobileProviders = _notificationHostRepo.All().Select(c => c.Name);
                return mobileProviders.ToList();
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_MOBILEPROVIDERS_FAILED, ex);
			}
		}

		public string GetIDState(string countryName, string stateAbbr)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(countryName))
				{
					string state = string.Empty;
					var country = _countryRepo.FilterBy(c => c.Name == countryName).FirstOrDefault();
					var selectedState = _stateRepo.FilterBy(s => s.CountryCode == country.Code.ToString() && s.Abbr == stateAbbr).FirstOrDefault();
					if (selectedState != null)
					{
						state = selectedState.Name;
					}
					return state;
				}

                return stateAbbr;
			}
			catch (Exception ex)
			{
				List<string> details = new List<string>();
				details.Add("Country Name :" + countryName);
				details.Add("State Code :" + stateAbbr);
				//MongoDBLogger.ListError<string>(details, "GetIDState", AlloyLayerName.CORE, ModuleName.Customer,
				//"Error in GetCountry -MGI.Core.Partner.Impl.NexxoDataStructuresServiceImpl", ex.Message, ex.StackTrace);
				throw new PartnerCustomerException(PartnerCustomerException.GET_ID_STATE_FAILED, ex);
			}
		}

		public string GetCountry(string countryAbbr)
		{
			string country = string.Empty;
			try
			{
				var selectedCountry = _countryRepo.FilterBy(c => c.Abbr2 == countryAbbr).FirstOrDefault();

				if (selectedCountry != null)
				{
					country = selectedCountry.Name;
				}

                return country;
			}
			catch (Exception ex)
			{
				//MongoDBLogger.Error<string>(countryAbbr, "GetCountry", AlloyLayerName.CORE, ModuleName.Customer,
				//"Error in GetCountry -MGI.Core.Partner.Impl.NexxoDataStructuresServiceImpl", ex.Message, ex.StackTrace);
				throw new PartnerCustomerException(PartnerCustomerException.GET_COUNTRY_BY_CODE_FAILED, ex);
			}
		}

		public List<LegalCode> GetLegalCodes()
		{
			try
			{
                List<LegalCode> legalCodes = LegalCodeRepo.All().ToList();
                return legalCodes;
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_LEGALCODES_FAILED, ex);
			}
		}

		public List<Occupation> GetOccupations()
		{
			try
			{
                List<Occupation> occupations = OccupationRepo.All().Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList();
                return occupations;
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_OCCUPATIONS_FAILED, ex);
			}
		}


		public List<MasterCountry> MasterCountries(long channelPartnerId)
		{
			try
			{
                List<MasterCountry> mastercountries = _masterCountryRepo.FilterBy(c => c.ChannelPartner.Id == channelPartnerId).OrderBy(c => c.MasterCountry.Name)
						.Select(c => new MasterCountry { Name = c.MasterCountry.Name, Abbr2 = c.MasterCountry.Abbr2, Abbr3 = c.MasterCountry.Abbr3 }).ToList();

                return mastercountries;
            }
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_MASTERCOUNTRIES_FAILED, ex);
			}
		}

		public MasterCountry GetMasterCountryByCode(string masterCountryAbbr2)
		{
			try
			{
                MasterCountry masterCountry = _masterCountryRepo.FilterBy(c => c.MasterCountry.Abbr2 == masterCountryAbbr2)
						.Select(c => new MasterCountry { Name = c.MasterCountry.Name, Abbr2 = c.MasterCountry.Abbr2, Abbr3 = c.MasterCountry.Abbr3 }).FirstOrDefault();

                return masterCountry;
            }
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.GET_MASTER_COUNTRY_BY_CODE_FAILED, ex);
			}
		}
	}
}
