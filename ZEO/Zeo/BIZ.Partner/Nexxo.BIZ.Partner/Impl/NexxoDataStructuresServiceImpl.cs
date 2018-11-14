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
using MGI.Biz.Partner.Data;
using CoreData = MGI.Core.Partner.Data;

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
			Mapper.CreateMap<CoreData.NexxoIdType, Data.NexxoIdType>();
			Mapper.CreateMap<CoreData.LegalCode, Data.LegalCode>();
			Mapper.CreateMap<CoreData.Occupation, Data.Occupation>();
			Mapper.CreateMap<CoreData.MasterCountry, Data.MasterCountry>();
		}

		public Data.NexxoIdType Find(long agentSessionId, long channelPartnerId, string name, string country, string state, MGIContext mgiContext)
		{
			try
			{
                CoreData.NexxoIdType idType = _nexxoDataStructuresService.Find(channelPartnerId, name, country, state);
                return AutoMapper.Mapper.Map<CoreData.NexxoIdType, Data.NexxoIdType>(idType);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.ERROR_IN_FIND_IDTYPE, ex);
			}
			
		}

		public List<string> Countries(long agentSessionId, MGIContext mgiContext)
        {
			try
			{
                List<string> countries = _nexxoDataStructuresService.Countries();
                return countries;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_COUNTRIES_FAILED, ex);
			}
        }

		public List<string> States(long agentSessionId, string country, MGIContext mgiContext)
        {
			try
			{
                List<string> states = _nexxoDataStructuresService.States(country);
                return states;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_STATES_FAILED, ex);
			}
        }

		public List<string> USStates(long agentSessionId, MGIContext mgiContext)
        {
			try
			{
                List<string> USstates = _nexxoDataStructuresService.USStates();
                return USstates;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_USSTATES_FAILED, ex);
			}
        }

		public List<string> IdCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
        {
			try
			{
                List<string> idCountries = _nexxoDataStructuresService.IdCountries(channelPartnerId);
                return idCountries;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_IDCOUNTRIES_FAILED, ex);
			}
        }

		public List<string> IdStates(long agentSessionId, long channelPartnerId, string country, string idType, MGIContext mgiContext)
        {
			try
			{
				List<string> idStates =_nexxoDataStructuresService.IdStates(channelPartnerId, country, idType);
                return idStates;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_IDSTATES_FAILED, ex);
			}
        }

		public List<string> IdTypes(long agentSessionId, long channelPartnerId, string country, MGIContext mgiContext)
        {
			try
			{
                List<string> idTypes = _nexxoDataStructuresService.IdTypes(channelPartnerId, country);
                return idTypes;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_IDTYPES_FAILED, ex);
			}
        }

		public List<Data.LegalCode> GetLegalCodes(long agentSessionId, MGIContext mgiContext)
		{
			try
			{
                List<CoreData.LegalCode> legalCodes = _nexxoDataStructuresService.GetLegalCodes();
                return Mapper.Map<List<Data.LegalCode>>(legalCodes);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_LEGALCODES_FAILED, ex);
			}
		}

		public List<Data.Occupation> GetOccupations(long agentSessionId, MGIContext mgiContext)
		{
			try
			{
                List<CoreData.Occupation> occupations = _nexxoDataStructuresService.GetOccupations();
                return Mapper.Map<List<Data.Occupation>>(occupations);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_OCCUPATIONS_FAILED, ex);
			}
		}

		public List<string> PhoneTypes(long agentSessionId, MGIContext mgiContext)
        {
			try
			{
				List<string> phoneTypes = _nexxoDataStructuresService.PhoneTypes();
                return phoneTypes;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_PHONETYPES_FAILED, ex);
			}
        }

		public List<string> MobileProviders(long agentSessionId, MGIContext mgiContext)
        {
			try
			{
                List<string> mobileProviders = _nexxoDataStructuresService.MobileProviders();
                return mobileProviders;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_MOBILEPROVIDERS_FAILED, ex);
			}
        }

		public List<Biz.Partner.Data.MasterCountry> MasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{		
			
			try
			{
				List<CoreData.MasterCountry> _masterCountry = _nexxoDataStructuresService.MasterCountries(channelPartnerId);
				List<Biz.Partner.Data.MasterCountry> masterCountries = AutoMapper.Mapper.Map<List<Biz.Partner.Data.MasterCountry>>(_masterCountry);
                return masterCountries;
            }
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_MASTERCOUNTRIES_FAILED, ex);
			}			
		}

		public Biz.Partner.Data.MasterCountry GetMasterCountryByCode(long agentSessionId, string masterCountryAbbr2, MGIContext mgiContext)
		{
			try
			{
				CoreData.MasterCountry _masterCountry = _nexxoDataStructuresService.GetMasterCountryByCode(masterCountryAbbr2);
				Data.MasterCountry masterCountry  = AutoMapper.Mapper.Map<Biz.Partner.Data.MasterCountry>(_masterCountry);
                return masterCountry;
            }
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizCustomerException(BizCustomerException.GET_MASTER_COUNTRY_BY_CODE_FAILED, ex);
			}			
		}
	}
}
