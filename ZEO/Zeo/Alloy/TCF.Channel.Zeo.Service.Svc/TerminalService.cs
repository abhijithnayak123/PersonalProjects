using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : ITerminalService
    {
        public Response CreateTerminal(Terminal terminal, ZeoContext context)
        {
            return serviceEngine.CreateTerminal(terminal, context);
        }

        public Response GetNpsdiagnosticInfo(long terminalId, ZeoContext context)
        {
            return serviceEngine.GetNpsdiagnosticInfo(terminalId, context);
        }

        public Response GetTerminalById(long terminalId, ZeoContext context)
        {
            return serviceEngine.GetTerminalById(terminalId, context);
        }

        public Response GetTerminalByName(string terminalName, ZeoContext context)
        {
            return serviceEngine.GetTerminalByName(terminalName, context);
        }

        public Response UpdateTerminal(Terminal terminal, ZeoContext context)
        {
            return serviceEngine.UpdateTerminal(terminal, context);
        }
    }
}