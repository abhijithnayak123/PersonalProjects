using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : ILocationCounterIdService
    {
        public Response CreateCustomerSessionCounterId(long productProviderId, long locationId, ZeoContext context)
        {
            return serviceEngine.CreateCustomerSessionCounterId(productProviderId,locationId, context);
        }

        public Response GetLocationCounterID(long locationId, int providerId,ZeoContext context)
        {
            return serviceEngine.GetLocationCounterID(locationId, providerId,context);

        }

        public Response UpdateCounterId(ZeoContext context)
        {
            return serviceEngine.UpdateCounterId(context);
        }
    }
}
