using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
    public interface IManageTerminal
    {
        Terminal GetTerminalById(long terminalId, commonData.ZeoContext context);

        Terminal GetTerminalByName( string terminalName, commonData.ZeoContext context);

        bool CreateTerminal(Terminal terminal, commonData.ZeoContext alloycontext);

        bool UpdateTerminal(Terminal terminal, commonData.ZeoContext context);
        Terminal GetNpsdiagnosticInfo(long terminalId, commonData.ZeoContext context);
    }
}
