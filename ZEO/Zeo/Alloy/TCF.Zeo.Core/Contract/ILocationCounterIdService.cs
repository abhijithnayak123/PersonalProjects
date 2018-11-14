using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TCF.Zeo.Core.Contract
{
    public interface ILocationCounterIdService : IDisposable
    {
        string GetLocationCounterID(long locationId, int providerId, ZeoContext context);
        string CreateCustomerSessionCounterId(long productProviderId, long locationId, long customerSessionId, string timeZone, ZeoContext context);
        bool UpdateLocationCounterID(ZeoContext context);
    }
}
