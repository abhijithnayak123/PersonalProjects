using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using commonData = TCF.Zeo.Common.Data;

#region Zeo References
using TCF.Channel.Zeo.Data;
using CoreData = TCF.Zeo.Core;
using coreContract = TCF.Zeo.Core.Contract;
using TCF.Zeo.Biz.Contract;
#endregion

#region External References
using AutoMapper;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;
#endregion

namespace TCF.Zeo.Biz.Impl
{
    public class LocationCounterIdServiceImpl : ILocationCounterIdService
    {
        IMapper mapper;
        coreContract.ILocationCounterIdService _LocCounterIdServiceImpl;

        public LocationCounterIdServiceImpl()
        {
            #region Mapping
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoreData.Data.LocationCounterId, LocationCounterId>();

            });
            mapper = config.CreateMapper();
            #endregion
        }

        public string CreateCustomerSessionCounterId(long productProviderId, long locationId, long customerSessionId, string timeZone, commonData.ZeoContext context)
        {
            try
            {
                using (_LocCounterIdServiceImpl = new CoreData.Impl.ZeoCoreImpl())
                {

                    return _LocCounterIdServiceImpl.CreateCustomerSessionCounterId(productProviderId, locationId, customerSessionId, timeZone, context);
                }

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_COUNTERID_CUSTOMER_SESSION_FAILED, ex);
            }
        }

        public string GetLocationCounterID(long locationId, int providerId, commonData.ZeoContext context)
        {

            try
            {
                using (_LocCounterIdServiceImpl = new CoreData.Impl.ZeoCoreImpl())
                {
                    return _LocCounterIdServiceImpl.GetLocationCounterID(locationId, providerId, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_GET_FAILED, ex);
            }
        }

        public bool UpdateLocationCounterID(commonData.ZeoContext context)
        {
            try
            {
                using (_LocCounterIdServiceImpl = new CoreData.Impl.ZeoCoreImpl())
                {
                    return _LocCounterIdServiceImpl.UpdateLocationCounterID(context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new LocationException(LocationException.LOCATION_COUNTERID_STATUS_UPDATE_FAILED, ex);
            }
        }
    }
}
