using System;
using System.Diagnostics;
using System.Linq.Expressions;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

using MGI.Common.DataAccess.Contract;
using System.Collections.Generic;
using System.Linq;

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

        private IRepository<MasterCountry> _masterCountriesRepo;
        public IRepository<MasterCountry> MasterCountriesRepo
        {
            set { _masterCountriesRepo = value; }
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

        public NexxoIdType Find(long channelPartnerId, string name, string country, string state)
        {
            Expression<Func<ChannelPartnerIdTypesMapping, bool>> findExp;

            if (string.IsNullOrEmpty(state))
                findExp = x => x.ChannelPartner.Id == channelPartnerId && x.NexxoIdType.Name == name && x.NexxoIdType.CountryId.Name == country && x.NexxoIdType.StateId == null && x.NexxoIdType.IsActive == true;
            else
                findExp = x => x.ChannelPartner.Id == channelPartnerId && x.NexxoIdType.Name == name && x.NexxoIdType.CountryId.Name == country && x.NexxoIdType.StateId.Name == state && x.NexxoIdType.IsActive == true;

            var ids = _idTypeRepo.FindBy(findExp);

            return ids == null ? null : ids.NexxoIdType;
        }

        public NexxoIdType Find(long channelPartnerId, long idTypeId)
        {
            // 2080 Changes
            ChannelPartnerIdTypesMapping _channelPartnerIdTypesMapping = _idTypeRepo.FindBy(c => c.ChannelPartner.Id == channelPartnerId && c.NexxoIdType.Id == idTypeId);
            if (_channelPartnerIdTypesMapping != null)
            {
                return _channelPartnerIdTypesMapping.NexxoIdType;
            }
            else
                return null;
        }

        public List<string> Countries()
        {
            List<string> countries = _countryRepo.All().Select(c => c.Name).ToList();
            return countries;
        }

        public List<string> States(string country)
        {
            MasterCountry _country = _masterCountriesRepo.FindBy(c => c.Name == country);
            string countryCode = _country.Abbr2.ToString();

            List<string> states = new List<string>();
            var statesList = _stateRepo.FilterBy(c => c.CountryCode == countryCode);
            states = statesList.Select(c => c.Name).ToList();
            return states;
        }

        public List<string> USStates()
        {
            var usStates = _stateRepo.FilterBy(c => c.CountryCode == "US").Select(c => c.Abbr);
            return usStates.ToList();
        }

        public List<string> IdCountries(long channelPartnerId)
        {
            var idCountries = _idTypeRepo.FilterBy(x => x.ChannelPartner.Id == channelPartnerId && x.IsActive == true).Select(c => c.NexxoIdType.CountryId.Name).Distinct();
            return idCountries.ToList();
        }

        public List<string> IdStates(long channelPartnerId, string country, string idType)
        {
            var idStates = _idTypeRepo.FilterBy(c => c.ChannelPartner.Id == channelPartnerId && c.NexxoIdType.CountryId.Name == country && c.NexxoIdType.Name == idType && c.IsActive == true).Select(c => c.NexxoIdType.StateId.Name);
            return idStates.ToList();
        }

        public List<string> IdTypes(long channelPartnerId, string country)
        {
            List<string> idTypes = new List<string>();
            var queryableIdTypes = _idTypeRepo.FilterBy(c => c.ChannelPartner.Id == channelPartnerId && c.NexxoIdType.CountryId.Name == country && c.IsActive == true).Select(c => c.NexxoIdType.Name);
            var distinctIdTypes = queryableIdTypes.Distinct();
            if (distinctIdTypes != null)
            {
                idTypes = distinctIdTypes.ToList();
            }
            return idTypes;
        }

        public List<string> PhoneTypes()
        {
            var queryablephoneTypes = _contactTypeRepo.FilterBy(c => c.Id < 5);
            return queryablephoneTypes.Select(c => c.Type).ToList();
        }

        public List<string> MobileProviders()
        {
            var mobileProviders = _notificationHostRepo.All().Select(c => c.Name);
            return mobileProviders.ToList();
        }

        public string GetIDState(string countryName, string stateAbbr)
        {
            if (!string.IsNullOrWhiteSpace(countryName))
            {
                string state = string.Empty;
                var country = _masterCountriesRepo.FilterBy(c => c.Name == countryName).FirstOrDefault();
                var selectedState = _stateRepo.FilterBy(s => s.CountryCode == country.Abbr2.ToString() && s.Abbr == stateAbbr).FirstOrDefault();
                if (selectedState != null)
                {
                    state = selectedState.Name;
                }
                return state;
            }
            return stateAbbr;
        }

        public string GetCountry(string countryAbbr)
        {
            string country = string.Empty;
            var selectedCountry = _countryRepo.FilterBy(c => c.Abbr2 == countryAbbr).FirstOrDefault();

            if (selectedCountry != null)
            {
                country = selectedCountry.Name;
            }
            return country;
        }

        public List<LegalCode> GetLegalCodes()
        {
            var legalCodes = LegalCodeRepo.All().ToList();
            return legalCodes;
        }

        public List<Occupation> GetOccupations()
        {
            var occupations = OccupationRepo.All().Where(a => a.IsActive == true).OrderBy(a => a.Name).ToList();
            return occupations;
        }


        public List<MasterCountry> MasterCountries(long channelPartnerId)
        {
            var mastercountries = _masterCountryRepo.FilterBy(c => c.ChannelPartner.Id == channelPartnerId).OrderBy(c => c.MasterCountry.Name)
                .Select(c => new MasterCountry { Name = c.MasterCountry.Name, Abbr2 = c.MasterCountry.Abbr2, Abbr3 = c.MasterCountry.Abbr3 }).ToList();
            return mastercountries;
        }

        public MasterCountry GetMasterCountryByCode(string masterCountryAbbr2)
        {
            var masterCountry = _masterCountryRepo.FilterBy(c => c.MasterCountry.Abbr2 == masterCountryAbbr2)
                .Select(c => new MasterCountry { Name = c.MasterCountry.Name, Abbr2 = c.MasterCountry.Abbr2, Abbr3 = c.MasterCountry.Abbr3 }).FirstOrDefault();
            return masterCountry;
        }
    }
}
