using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IMessageStoreService
    {
        public Response GetMessage(string messageKey, ZeoContext context)
        {
            return serviceEngine.GetMessage(messageKey, context);
        }

        public Response LookUp(MessageStoreSearch search, ZeoContext context)
        {
            return serviceEngine.LookUp(search, context);
        }
    }
}