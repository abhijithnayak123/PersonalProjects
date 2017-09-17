using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Zeo References
using TCF.Zeo.Biz.Contract;
using CoreImpl = TCF.Zeo.Core.Impl;
using TCF.Channel.Zeo.Data;
using CoreContract = TCF.Zeo.Core.Contract;
using coreData = TCF.Zeo.Core.Data;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;
#endregion

#region External References
using AutoMapper;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Zeo.Biz.Impl
{
    public class LocationServiceImpl : ILocationService
    {
        IMapper mapper;
        CoreContract.ILocationService _LocationserviceImpl;

        public LocationServiceImpl()
        {
            #region Mapping
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<coreData.Location, Location>().ReverseMap();
            });
            mapper = config.CreateMapper();
            #endregion
        }

        public List<Location> GetLocationById(long locationId, commonData.ZeoContext context)
        {
            try
            {
                using (_LocationserviceImpl = new CoreImpl.ZeoCoreImpl())
                {
                    List<coreData.Location> coreGetLocationById = _LocationserviceImpl.GetLocationById(locationId, context);
                    return mapper.Map<List<coreData.Location>, List<Location>>(coreGetLocationById);

                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_GET_FAILED, ex);
            }
        }


        public long CreateLocation( Location location, commonData.ZeoContext context)
        {
            try
            {
                using (_LocationserviceImpl = new CoreImpl.ZeoCoreImpl())
                {
                    coreData.Location coreLocation = mapper.Map<Location, coreData.Location>(location);
                    return _LocationserviceImpl.CreateLocation(coreLocation, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_CREATE_FAILED, ex);
            }
        }

        public bool UpdateLocation(Location location, commonData.ZeoContext context)
        {
            try
            {
                using (_LocationserviceImpl = new CoreImpl.ZeoCoreImpl())
                {
                    coreData.Location coreLocation = mapper.Map<Location, coreData.Location>(location);
                    return _LocationserviceImpl.UpdateLocation(coreLocation, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_UPDATE_FAILED, ex);
            }
        }

        public bool ValidateLocation( Location location, commonData.ZeoContext context)
        {
            try
            {
                using (_LocationserviceImpl = new CoreImpl.ZeoCoreImpl())
                {
                    coreData.Location coreLocation = mapper.Map<Location, coreData.Location>(location);
                    int errorCode = _LocationserviceImpl.ValidateLocation(coreLocation, context);


                    if (errorCode == 0)
                        return true;

                    Helper.LocationErrorCode locError = (Helper.LocationErrorCode)Enum.ToObject(typeof(Helper.LocationErrorCode), errorCode);
                    Exception ex = new Exception();
                    switch (locError)
                    {
                        case Helper.LocationErrorCode.LOCATIONNAME_ALREADY_EXIST:
                            throw new LocationException(LocationException.LOCATION_NAME_ALREADY_EXIST, ex);

                        case Helper.LocationErrorCode.BANK_BRANCH_ID_ALREADY_EXIST:
                            throw new LocationException(LocationException.LOCATION_BANK_OR_BRANCH_ID_ALREADY_EXIST, ex);

                        case Helper.LocationErrorCode.LOCATIONIDENTIFIER_ALREADY_EXIST:
                            throw new LocationException(LocationException.LOCATION_IDENTIFIER_ALREADY_EXIST, ex);
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_ALREADY_EXISTS, ex);
            }
        }

        public List<Location> GetLocationsByChannelPartnerId(long channelpartnerId, commonData.ZeoContext context)
        {
            try
            {
                using (_LocationserviceImpl = new CoreImpl.ZeoCoreImpl())
                {
                    List<coreData.Location> coreGetLocationByChannelPartnerId = _LocationserviceImpl.GetLocationsByChannelPartnerId(channelpartnerId, context);
                    return mapper.Map<List<coreData.Location>, List<Location>>(coreGetLocationByChannelPartnerId);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_GET_FAILED, ex);
            }
        }
    }
}
