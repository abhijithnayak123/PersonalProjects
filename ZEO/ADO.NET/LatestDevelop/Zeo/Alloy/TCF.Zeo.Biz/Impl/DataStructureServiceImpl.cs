#region Using References

using TCF.Zeo.Biz.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreData = TCF.Zeo.Core;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;
#endregion

#region External References
using AutoMapper;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;
#endregion

namespace TCF.Zeo.Biz.Impl
{
    public class DataStructureServiceImpl : IDataStructuresService
    {

        CoreData.Contract.IDataStructuresService dataStructureService;

        IMapper mapper;

        public DataStructureServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoreData.Data.LegalCode, LegalCode>();
                cfg.CreateMap<CoreData.Data.Occupation, Occupation>();
                cfg.CreateMap<CoreData.Data.MasterCountry, MasterCountry>();
                cfg.CreateMap<CoreData.Data.IdType, IdType>();
                cfg.CreateMap<CoreData.Data.KeyValuePair, KeyValuePair>();
            });

            mapper = config.CreateMapper();
        }

        public List<string> GetPhoneTypes(commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return dataStructureService.PhoneTypes(context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_PHONETYPES_FAILED, ex);
            }
        }

        public List<string> GetMobileProviders(commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return dataStructureService.MobileProviders(context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_MOBILEPROVIDERS_FAILED, ex);
            }
        }

        public List<string> GetStates(string country, commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return dataStructureService.States(country, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_STATES_FAILED, ex);
            }
        }

        public List<string> GetUSStates(commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return dataStructureService.USStates(context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_USSTATES_FAILED, ex);
            }
        }

        public List<LegalCode> GetLegalCodes(commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                List<CoreData.Data.LegalCode> corelegalCodes = dataStructureService.GetLegalCodes(context);
                List<LegalCode> legalCodes = mapper.Map<List<CoreData.Data.LegalCode>, List<LegalCode>>(corelegalCodes);

                return legalCodes;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_LEGALCODES_FAILED, ex);
            }
        }

        public List<Occupation> GetOccupations(commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                List<CoreData.Data.Occupation> coreOccupations = dataStructureService.GetOccupations(context);
                List<Occupation> occupations = mapper.Map<List<CoreData.Data.Occupation>, List<Occupation>>(coreOccupations);

                return occupations;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_OCCUPATIONS_FAILED, ex);
            }
        }

        public List<string> GetIdCountries(commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return dataStructureService.IdCountries(context.ChannelPartnerId, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_IDCOUNTRIES_FAILED, ex);
            }
        }

        public List<string> GetIdStates(string idType, string country, commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return dataStructureService.IdStates(context.ChannelPartnerId, country, idType, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_IDSTATES_FAILED, ex);
            }
        }

        public List<string> GetIdTypes(string country, commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return dataStructureService.IdTypes(context.ChannelPartnerId, country, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_IDTYPES_FAILED, ex);
            }
        }

        public List<MasterCountry> GetMasterCountries(commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                List<CoreData.Data.MasterCountry> coreMasterCountries = dataStructureService.MasterCountries(context.ChannelPartnerId, context);
                List<MasterCountry> masterCountries = mapper.Map<List<CoreData.Data.MasterCountry>, List<MasterCountry>>(coreMasterCountries);

                return masterCountries;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_MASTERCOUNTRIES_FAILED, ex);
            }
        }

        public List<string> GetCountries(commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return dataStructureService.Countries(context.ChannelPartnerId, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_COUNTRIES_FAILED, ex);
            }
        }

        public IdType GetIdType(string idType, string country, string stateName, commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return mapper.Map<IdType>(dataStructureService.GetIdType(idType, country, stateName, context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_IDTYPES_FAILED, ex);
            }
        }

        public string GetStateNameByCode(string stateCode, string countryCode, commonData.ZeoContext context)
        {
            try
            {
                dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
                return dataStructureService.GetStateNameByCode(stateCode, countryCode, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_STATENAME_FAILED, ex);
            }
        }

        public List<KeyValuePair> GetBINDetails(commonData.ZeoContext context)
        {
            dataStructureService = new CoreData.Impl.DataStructureServiceImpl();
            return mapper.Map<List<KeyValuePair>>(dataStructureService.GetBINDetails(context));

        }
    }
}
