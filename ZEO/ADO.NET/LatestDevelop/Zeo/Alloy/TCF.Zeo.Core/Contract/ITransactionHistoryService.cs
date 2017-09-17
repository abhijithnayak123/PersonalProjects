using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface ITransactionHistoryService
    {
        List<TransactionHistory> GetCustomerTransactionHistory(TransactionSearchCriteria searchCriteria, ZeoContext context);

        List<TransactionHistory> GetAgentTransactionHistory(TransactionSearchCriteria searchCriteria, ZeoContext context);

        List<string> GetCustomerTransactionLocation(DateTime dateRange, ZeoContext context);

        long GetCustomerSessionId(long transactionId, int transactionTypeValue, ZeoContext context);
    }
}
