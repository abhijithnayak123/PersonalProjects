using System;
using System.Linq;
using MGI.Core.Partner.Data;
using System.Collections.Generic;
using MGI.Core.Partner.Data.Transactions;

namespace MGI.Core.Partner.Contract
{
	public interface IMessageCenter
	{

		/// <summary>
		/// This method is to create the agent message
		/// </summary>
		/// <param name="agentMessage">This is agent message details</param>
		/// <param name="TimeZone">This is time zone</param>
		/// <returns>Created status of agent message</returns>
		bool Create(AgentMessage agentMessage, string TimeZone);

		/// <summary>
		/// This method is to update the agent message
		/// </summary>
		/// <param name="agentMessage">This is agent message details to be updated</param>
		/// <returns>Updated status of agent message</returns>
		bool Update(AgentMessage agentMessage);

		/// <summary>
		/// This method is to update the agaent message
		/// </summary>
		/// <param name="agentMessage">This is agent message details to be updated</param>
		/// <param name="TimeZone">This is passed to update the transaction date time based on the Time Zone selected</param>
		/// <returns></returns>
		bool UpdateStatus(AgentMessage agentMessage, string TimeZone);

		/// <summary>
		/// This method is to delete the transaction
		/// </summary>
		/// <param name="transaction">This is transaction details</param>
		/// <returns>Deleted status of transaction</returns>
		bool Delete(Transaction transaction);

		/// <summary>
		/// This method is to get the agent message by transaction details
		/// </summary>
		/// <param name="transaction">This is transaction details</param>
		/// <returns>This is Agent message details</returns>
		AgentMessage Lookup(Transaction transaction);

		/// <summary>
		/// This method is to get the collection of agent messages by agent id
		/// </summary>
		/// <param name="AgentId">This is Agent id</param>
		/// <returns>This is Collection of agent message details</returns>
		List<AgentMessage> GetByAgentID(long AgentId);

		/// <summary>
		/// This method is used to deleted the chat history from message centre
		/// </summary>
		/// <returns></returns>
		bool DeleteAllMessages();
	}
}
