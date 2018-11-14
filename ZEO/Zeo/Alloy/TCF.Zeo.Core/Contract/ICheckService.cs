using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Contract
{
    public interface ICheckService : IDisposable
    {
        long CreateCheckTransaction(Check check, CheckImages checkImage, ZeoContext context);

        bool UpdateCheckTransaction(Check check, ZeoContext context);

        Check GetCheckTransaction(long transactionId, ZeoContext context);

        void CommitTransaction(long transactionId, int status, long customerId, string timeZone, ZeoContext context);

        List<CheckType> GetCheckTypes(ZeoContext context);

        void UpdateCheckTransactionState(long transactionId, TransactionStates transactionStates, string timeZone, ZeoContext context);

        bool CancelCheckTransaction(long transactionId, TransactionStates transactionStates, string timeZone, ZeoContext context);

        long GetCheckCxnTransactionId(long transactionId);

        CheckProviderDetails GetCheckProvider(MICRDetails micrDetails, ZeoContext context);
    }
}
