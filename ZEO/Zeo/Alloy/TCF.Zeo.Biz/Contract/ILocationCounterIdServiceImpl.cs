using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
    public interface ILocationCounterIdService
    {
        string GetLocationCounterID(long locationId, int providerId, commonData.ZeoContext context);
        string CreateCustomerSessionCounterId(long productProviderId, long locationId, long customerSessionId, string timeZone, commonData.ZeoContext context);
        bool UpdateLocationCounterID(commonData.ZeoContext context);
    }
}
