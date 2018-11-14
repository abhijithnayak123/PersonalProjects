using System.Collections.Generic;
using AutoMapper;
using TCF.Channel.Zeo.Service.Contract;
using System.EnterpriseServices;
using TCF.Channel.Zeo.Data;
using System;
using TCF.Zeo.Biz.Impl;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : ILocationCounterIdService
    {
        public TCF.Zeo.Biz.Contract.ILocationCounterIdService LocationCounterIdService { private get; set; }

        public Response CreateCustomerSessionCounterId(long productProviderId, long locationId, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                LocationCounterIdService = new LocationCounterIdServiceImpl();
                response.Result = LocationCounterIdService.CreateCustomerSessionCounterId(productProviderId, locationId, context.CustomerSessionId, context.TimeZone, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GetLocationCounterID(long locationId, int providerId, ZeoContext context)
        {

            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            LocationCounterIdService = new LocationCounterIdServiceImpl();
            response.Result = LocationCounterIdService.GetLocationCounterID(locationId, providerId, commonContext);

            return response;
        }

        public Response UpdateCounterId(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);


            Response response = new Response();

            LocationCounterIdService = new LocationCounterIdServiceImpl();
            response.Result = LocationCounterIdService.UpdateLocationCounterID(commonContext);

            return response;
        }
    }
}
