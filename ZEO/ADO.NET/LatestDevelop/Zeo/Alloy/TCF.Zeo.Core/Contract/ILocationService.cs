using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface ILocationService : IDisposable
    {
        List<Location> GetLocationsByChannelPartnerId(long channelpartnerId, ZeoContext context);

        List<Location> GetLocationById(long locationId, ZeoContext context);

        long CreateLocation(Location createlocation, ZeoContext context);

        bool UpdateLocation(Location updateloc, ZeoContext context);

        int ValidateLocation(Location validateLocation, ZeoContext context);
    }
}
