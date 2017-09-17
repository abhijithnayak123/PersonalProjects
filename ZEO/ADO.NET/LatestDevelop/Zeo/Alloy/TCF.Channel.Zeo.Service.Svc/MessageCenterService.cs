using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IMessageCenterService
    {
        public Response GetAgentMessages(ZeoContext context)
        {
            return serviceEngine.GetAgentMessages(context);
        }

        public Response GetMessageDetails(long transactionId, ZeoContext context)
        {
            return serviceEngine.GetMessageDetails(transactionId, context);
        }

    }
}