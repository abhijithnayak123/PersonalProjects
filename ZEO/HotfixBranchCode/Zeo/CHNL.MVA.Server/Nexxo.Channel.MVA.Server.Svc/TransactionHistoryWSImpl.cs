using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MGI.Channel.MVA.Server.Contract;

namespace MGI.Channel.MVA.Server.Svc
{
    public partial class MVAWSImpl : ITransactionHistoryService
    {
        public List<MGI.Channel.Shared.Server.Data.PastTransaction> GetPastTransactions(long customerSessionId, string transactionType)
        {
            return MVAEngine.GetPastTransactions(customerSessionId, transactionType);
        }

        public MGI.Channel.Shared.Server.Data.BillPayTransaction GetBillPayTransaction(long customerSessionId, long transactionId)
        {
            return MVAEngine.GetBillPayTransaction(customerSessionId, transactionId);
        }
    }
}