using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IZeoContext
    {
        public Response GetZeoContextForAgent(long agentSessionId,ZeoContext context)
        {
            return serviceEngine.GetZeoContextForAgent(agentSessionId, context);
        }

        public Response GetZeoContextForCustomer(long customerSessionId,ZeoContext context)
        {
            return serviceEngine.GetZeoContextForCustomer(customerSessionId, context);
        }
    }
}