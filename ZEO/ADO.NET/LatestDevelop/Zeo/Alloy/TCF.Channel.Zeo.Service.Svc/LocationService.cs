using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : ILocationService
    {
        public Response GetLocationsByChannelPartnerId(long channelPartnerId,ZeoContext context)
        {
            return serviceEngine.GetLocationsByChannelPartnerId(channelPartnerId, context);
        }
        public Response CreateLocation(Location location,ZeoContext context)
        {

            return serviceEngine.CreateLocation(location, context);
        }

        public Response GetLocationById(long locationId, ZeoContext context)
        {
            return serviceEngine.GetLocationById(locationId, context);
        }

        public Response UpdateLocation(Location location,ZeoContext context)
        {
            return serviceEngine.UpdateLocation(location, context);
        }

        public Response ValidateLocation(Location location, ZeoContext context)
        {
            return serviceEngine.ValidateLocation(location, context);
        }
    }
}
