using System.Collections.Generic;
using System.Linq;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;
using ITransactionHistoryService = MGI.Channel.Consumer.Server.Contract.ITransactionHistoryService;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.Consumer.Server.Impl
{
    public partial class ConsumerEngine : ITransactionHistoryService
    {
        [Transaction(ReadOnly = true)]
        public List<SharedData.PastTransaction> GetPastTransactions(long agentSessionId, long customerSessionId, long customerId, string transactionType, MGIContext mgiContext)
        {
			return SharedEngine.GetPastTransactions(agentSessionId, customerSessionId, customerId, transactionType, mgiContext);
        }

        [Transaction(ReadOnly = true)]
        public SharedData.BillPayTransaction GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			BillPayTransaction billPayTransaction = SharedEngine.GetBillPayTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);

            billPayTransaction.MetaData = AppendTimeZoneAbbr(billPayTransaction.MetaData, "ModifiedDate");

            return billPayTransaction;
        }
    }
}
