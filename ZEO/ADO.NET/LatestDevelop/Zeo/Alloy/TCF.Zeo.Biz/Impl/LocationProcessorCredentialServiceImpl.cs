using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Zeo References
using TCF.Channel.Zeo.Data;
using CoreImpl = TCF.Zeo.Core.Impl;
using CoreContract = TCF.Zeo.Core.Contract;
using coreData = TCF.Zeo.Core.Data;
using TCF.Zeo.Biz.Contract;
using commonData = TCF.Zeo.Common.Data;

#endregion

#region External References
using AutoMapper;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Zeo.Biz.Impl
{
    public class LocationProcessorCredentialServiceImpl : ILocationProcessorCredentialService
    {
        IMapper mapper;
        CoreContract.ILocationProcessorCredentialService _locProcessorCredentials;

        public LocationProcessorCredentialServiceImpl()
        {
            #region Mapping
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<coreData.LocationProcessorCredentials, LocationProcessorCredentials>().ReverseMap();
                cfg.CreateMap<coreData.LocationProcessorCredentials, LocationProcessorCredentials>();
            });
            mapper = config.CreateMapper();
            #endregion
        }

        public bool SaveLocationProcessorCredentials(LocationProcessorCredentials locProcessorCredential, commonData.ZeoContext context)
        {
            try
            {
                using (_locProcessorCredentials = new CoreImpl.ZeoCoreImpl())
                {
                    coreData.LocationProcessorCredentials coreLocProcessorCredential = mapper.Map<LocationProcessorCredentials, coreData.LocationProcessorCredentials>(locProcessorCredential);
                    return _locProcessorCredentials.SaveLocationProcessorCredentials(coreLocProcessorCredential, context.TimeZone, context);
                }
            }
            catch (Exception ex)
            {
                if(ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_PROCESSOR_CREDENTIALS_CREATE_FAILED, ex);
            }
        }

        public List<LocationProcessorCredentials> GetLocationProcessorCredentials(long locationId, commonData.ZeoContext context)
        {
            try
            {
                using (_locProcessorCredentials = new CoreImpl.ZeoCoreImpl())
                {
                    List<coreData.LocationProcessorCredentials> coreLocProcessorCredential = _locProcessorCredentials.GetLocationProcessorCredentials(locationId, context);
                    return mapper.Map<List<coreData.LocationProcessorCredentials>, List<LocationProcessorCredentials>>(coreLocProcessorCredential);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_PROCESSOR_CREDENTIALS_GET_FAILED, ex);
            }
        }

    }
}
