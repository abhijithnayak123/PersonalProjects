using System;
using System.Collections.Generic;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.Consumer.Server.Contract
{
    public interface ITransactionHistoryService
    {
		/// <summary>
		/// This method is used for getting past transactions
		/// </summary>
		/// <param name="agentSessionId">unique identifier for agent session</param>
		/// <param name="customerSessionId">unique identifier for customer session</param>
		/// <param name="customerId">unique identifier for customer</param>
		/// <param name="transactionType">transaction type</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>list of past transactions</returns>
        List<PastTransaction> GetPastTransactions(long agentSessionId, long customerSessionId, long customerId, string transactionType, MGIContext mgiContext);

		/// <summary>
		/// This method is used for getting bill pay transactions
		/// </summary>
		/// <param name="agentSessionId">unique identifier for agent session</param>
		/// <param name="customerSessionId">unique identifier for customer session</param>
		/// <param name="transactionId">transaction id</param>
		/// <param name="mgiContext">>This is the common class parameter used to pass supplimental information</param>
		/// <returns>billpay transaction details</returns>
        BillPayTransaction GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);
    }
}
