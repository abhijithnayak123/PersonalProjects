using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Data.Transactions;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
	public interface IMessageCenterService
	{
        /// <summary>
        /// To update the Agent message when check transaction park.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="agentMessage">A transient instance of AgentMessage[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Updated status of Agent Message</returns>
		bool Update(long customerSessionId, AgentMessage agentMessage, MGIContext mgiContext);

        /// <summary>
        /// This method is to fetch the collection of agent messages
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier of agent session</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Collection of agent messages</returns>
        List<AgentMessage> GetMessagesByAgentID(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method is to get the agent message by transaction id
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier of agent session</param>
        /// <param name="transactionId">This is transaction id</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Agent message</returns>
        AgentMessage GetMessageDetails(long agentSessionId, long transactionId, MGIContext mgiContext);
	}
}
