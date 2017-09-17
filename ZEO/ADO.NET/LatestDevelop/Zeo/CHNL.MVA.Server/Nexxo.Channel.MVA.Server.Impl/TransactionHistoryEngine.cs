using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.MVA.Server.Impl
{
	public partial class MVAEngine : ITransactionHistoryService
	{
		public List<MGI.Channel.Shared.Server.Data.PastTransaction> GetPastTransactions(long customerSessionId, string transactionType)
		{
			long agentSessionId = 0L;
			MGI.Common.Util.MGIContext context = Self.GetCustomerContext(customerSessionId);
			return ConsumerEngine.GetPastTransactions(agentSessionId, customerSessionId, context.AlloyId, transactionType, context);
		}

		public BillPayTransaction GetBillPayTransaction(long customerSessionId, long transactionId)
		{
			long agentSessionId = 0L;
			return ConsumerEngine.GetBillPayTransaction(agentSessionId, customerSessionId, transactionId, Self.GetCustomerContext(customerSessionId));
		}
	}
}
