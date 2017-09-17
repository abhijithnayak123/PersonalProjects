using System;
using System.Diagnostics;
using System.Linq.Expressions;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

using MGI.Common.DataAccess.Contract;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Impl
{
	public class NexxoDataStructuresServiceImpl : MGI.Biz.Partner.Contract.INexxoDataStructuresService
	{

        private INexxoDataStructuresService _nexxoDataStructuresService;
        public INexxoDataStructuresService NexxoDataStructuresService
        {
            set { _nexxoDataStructuresService = value; }
        }

		public NexxoDataStructuresServiceImpl()
		{
			Mapper.CreateMap<NexxoIdType, Data.NexxoIdType>();
			Mapper.CreateMap<LegalCode, Data.LegalCode>();
			Mapper.CreateMap<Occupation, Data.Occupation>();
			Mapper.CreateMap<MasterCountry, Data.MasterCountry>();
		}

		public Data.NexxoIdType Find(long agentSessionId, long channelPartnerId, string name, string country, string state, MGIContext mgiContext)
		{
			var idType = _nexxoDataStructuresService.Find(channelPartnerId, name, country, state);
			return AutoMapper.Mapper.Map<NexxoIdType, Data.NexxoIdType>(idType);
		}

		public List<string> Countries(long agentSessionId, MGIContext mgiContext)
        {
            return _nexxoDataStructuresService.Countries();
        }

		public List<string> States(long agentSessionId, string country, MGIContext mgiContext)
        {
            return _nexxoDataStructuresService.States(country);
        }

		public List<string> USStates(long agentSessionId, MGIContext mgiContext)
        {
            return _nexxoDataStructuresService.USStates();
        }

		public List<string> IdCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
        {
            return _nexxoDataStructuresService.IdCountries(channelPartnerId);
        }

		public List<string> IdStates(long agentSessionId, long channelPartnerId, string country, string idType, MGIContext mgiContext)
        {
            return _nexxoDataStructuresService.IdStates(channelPartnerId, country, idType);
        }

		public List<string> IdTypes(long agentSessionId, long channelPartnerId, string country, MGIContext mgiContext)
        {
            return _nexxoDataStructuresService.IdTypes(channelPartnerId, country);
        }

		public List<Data.LegalCode> GetLegalCodes(long agentSessionId, MGIContext mgiContext)
		{
			var legalCodes = _nexxoDataStructuresService.GetLegalCodes();
			return Mapper.Map<List<Data.LegalCode>>(legalCodes);
		}

		public List<Data.Occupation> GetOccupations(long agentSessionId, MGIContext mgiContext)
		{
			var occupations = _nexxoDataStructuresService.GetOccupations();
			return Mapper.Map <List<Data.Occupation>>(occupations);
		}

		public List<string> PhoneTypes(long agentSessionId, MGIContext mgiContext)
        {
            return _nexxoDataStructuresService.PhoneTypes();
        }

		public List<string> MobileProviders(long agentSessionId, MGIContext mgiContext)
        {
            return _nexxoDataStructuresService.MobileProviders();
        }

		public List<Biz.Partner.Data.MasterCountry> MasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
			List<MGI.Core.Partner.Data.MasterCountry> _masterCountry = _nexxoDataStructuresService.MasterCountries(channelPartnerId);
			return AutoMapper.Mapper.Map<List<Biz.Partner.Data.MasterCountry>>(_masterCountry);
		}

		public Biz.Partner.Data.MasterCountry GetMasterCountryByCode(long agentSessionId, string masterCountryAbbr2, MGIContext mgiContext)
		{
			MGI.Core.Partner.Data.MasterCountry _masterCountry = _nexxoDataStructuresService.GetMasterCountryByCode(masterCountryAbbr2);
			return AutoMapper.Mapper.Map<Biz.Partner.Data.MasterCountry>(_masterCountry);
		}
	}
}
