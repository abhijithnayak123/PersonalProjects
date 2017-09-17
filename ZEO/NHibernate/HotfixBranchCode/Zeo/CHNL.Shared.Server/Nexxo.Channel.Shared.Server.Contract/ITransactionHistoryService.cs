using System.Collections.Generic;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;


namespace MGI.Channel.Shared.Server.Contract
{
	public interface ITransactionHistoryService
	{
		MoneyTransferTransaction GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

		List<PastTransaction> GetPastTransactions(long agentSessionId, long customerSessionId, long customerId, string transactionType, MGIContext mgiContext);

		BillPayTransaction GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);
	}
}
