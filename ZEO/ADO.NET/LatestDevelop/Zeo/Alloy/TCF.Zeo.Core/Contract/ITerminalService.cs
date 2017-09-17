using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Contract
{
    public interface ITerminalService:IDisposable
    {
        /// <summary>
        /// This method is to get the terminal details by id.
        /// </summary>
        /// <param name="Id">This is unique identifier of terminal</param>
        /// <returns>Terminal details</returns>
        Terminal GetTerminalById(long terminalId, ZeoContext context);

        /// <summary>
        /// This method is to get terminal by name
        /// </summary>
        /// <param name="terminalName"></param>
        /// <returns></returns>
        Terminal GetTerminalByName( string terminalName, ZeoContext context);

        /// <summary>
        /// This method is to create the terminal
        /// </summary>		
        /// <param name="terminal">This is terminal details</param>
        /// <returns>Id of terminal</returns>
        bool CreateTerminal(Terminal terminal,ZeoContext context);

        /// <summary>
        /// This method is to update the terminal
        /// </summary>		
        /// <param name="terminal">This is terminal details</param>
        /// <returns>Updated status of terminal</returns>
        bool UpdateTerminal(Terminal terminal, ZeoContext context);

        ChannelPartner GetChannelPartnerTim(long terminalId, ZeoContext context);

        Terminal GetNpsdiagnosticInfo(long terminalId, ZeoContext context);
    }
}
