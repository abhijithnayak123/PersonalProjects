using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Contract
{
    public interface IFundService : IDisposable
    {
        long CreateFundTransaction(Funds funds, ZeoContext context);

        bool UpdateFundTransaction(Funds funds, ZeoContext context);

        Funds GetFundTransaction(long transactionId, ZeoContext context);

        void CommitFundTransaction(long transactionId, int status, long customerSessionId, ZeoContext context);

        void UpdateFundTransactionState(long transactionId, TransactionStates transactionStates, ZeoContext context);

        void UpdateFundTransactionAmount(long transactionId, decimal amount, ZeoContext context);

        long GetCXNTransactionId(long transactionId, ZeoContext context);

        void UpdateCXNTransctionId(long transactionId, long cxnTransactionId, ZeoContext context);
    }
}
