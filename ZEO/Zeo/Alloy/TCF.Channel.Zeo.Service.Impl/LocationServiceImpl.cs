using TCF.Channel.Zeo.Data;
using bizContract = TCF.Zeo.Biz.Contract;
using TCF.Zeo.Biz.Impl;
using TCF.Channel.Zeo.Service.Contract;
using commonData = TCF.Zeo.Common.Data;
using System;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : ILocationService
    {

        public bizContract.ILocationService LocationService { get; set; }

        public Response GetLocationsByChannelPartnerId(long channelPartnerId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);


            Response response = new Response();

            LocationService = new LocationServiceImpl();
            response.Result = LocationService.GetLocationsByChannelPartnerId(channelPartnerId, commonContext);

            return response;
        }

        public Response CreateLocation(Location location, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            LocationService = new LocationServiceImpl();
            response.Result = LocationService.CreateLocation(location, commonContext);

            return response;
        }

        public Response GetLocationById(long locationId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            LocationService = new LocationServiceImpl();
            response.Result = LocationService.GetLocationById(locationId, commonContext);

            return response;
        }

        public Response UpdateLocation(Location location, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            LocationService = new LocationServiceImpl();
            response.Result = LocationService.UpdateLocation(location, commonContext);

            return response;
        }

        public Response ValidateLocation(Location location, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            LocationService = new LocationServiceImpl();
            response.Result = LocationService.ValidateLocation(location, commonContext);

            return response;
        }

        public Response GetStateNamesAndIdByChannelPartnerId(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);


            Response response = new Response();

            LocationService = new LocationServiceImpl();

            response.Result = LocationService.GetStateNamesAndIdByChannelPartnerId(commonContext);

            return response;
        }
    }
}
