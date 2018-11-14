using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using commonData = TCF.Zeo.Common.Data;
namespace TCF.Zeo.Biz.Contract
{
    public interface IManageNpsTerminal
    {
        /// <summary>
        /// This method is to create the NPS terminal
        /// </summary>
        /// <param name="npsTerminal">This is NPS terminal details</param>
        /// <returns>Created status of NPS terminal</returns>
        bool CreateNpsTerminal(NpsTerminal npsTerminal, commonData.ZeoContext context);

        /// <summary>
        /// This method is to update the NPS terminal
        /// </summary>
        /// <param name="npsTerminal">This is NPS terminal details</param>
        /// <returns>Updated status of NPS terminal</returns>
        bool UpdateNpsTerminal(NpsTerminal npsTerminal, commonData.ZeoContext context);

        /// <summary>
        /// This method is to get the NPS terminal details by Id
        /// </summary>
        /// <param name="Id">This is unique identifier of NPS terminal</param>
        /// <returns>NPS terminal details</returns>
        List<NpsTerminal> GetNpsterminalByTerminalId(long terminalId, commonData.ZeoContext context);

        /// <summary>
        /// This method is to get the NPS terminal details by IP adddress
        /// </summary>
        /// <param name="ipAddress">This is IP address of system</param>
        /// <returns>NPS terminal details</returns>
        List<NpsTerminal> GetNpsTerminalBylocationId(string locationId, commonData.ZeoContext context);

        /// <summary>
        /// This method is to get the NPS terminal details by terminal name and channel partner details
        /// </summary>
        /// <param name="name">This terminal name</param>
        /// <param name="channelPartner">This is channel partner details</param>
        /// <returns>NPS terminal details</returns>
        NpsTerminal GetNpsTerminalByName(string name, long channelPartnerId, commonData.ZeoContext context);
    }
}
