using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : INpsTerminalService
    {
        public Response CreateNpsTerminal(NpsTerminal npsTerminal, ZeoContext context)
        {
            return serviceEngine.CreateNpsTerminal(npsTerminal,context);
        }

        public Response GetNpsTerminalBylocationId(string locationId, ZeoContext context)
        {
            return serviceEngine.GetNpsTerminalBylocationId(locationId, context);
        }

        public Response GetNpsTerminalByName(string name, long channelPartnerId, ZeoContext context)
        {
            return serviceEngine.GetNpsTerminalByName(name, channelPartnerId, context);
        }

        public Response GetNpsterminalByTerminalId(long npsTerminalId, ZeoContext context)
        {
            return serviceEngine.GetNpsterminalByTerminalId(npsTerminalId, context);
        }

        public Response UpdateNpsTerminal(NpsTerminal npsTerminal, ZeoContext context)
        {
            return serviceEngine.UpdateNpsTerminal(npsTerminal, context);
        }
    }
}