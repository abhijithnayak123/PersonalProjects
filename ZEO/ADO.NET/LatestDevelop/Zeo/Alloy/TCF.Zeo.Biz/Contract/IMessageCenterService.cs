using TCF.Channel.Zeo.Data;
using System.Collections.Generic;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
    public interface IMessageCenterService
    {

        /// <summary>
        /// This method is to fetch the collection of agent messages
        /// </summary>

        /// <param name="alloyContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Collection of agent messages</returns>
        List<AgentMessage> GetMessagesByAgentID(commonData.ZeoContext context);

        /// <summary>
        /// This method is to get the agent message by transaction id
        /// </summary>
        /// <param name="transactionId">This is transaction id</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Agent message</returns>
        AgentMessage GetMessageDetails(long transactionId, commonData.ZeoContext context);
    }
}
