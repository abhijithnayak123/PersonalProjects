using TCF.Zeo.Cxn.Check.Data;
using TCF.Zeo.Common.Data;
using System.Collections.Generic;

namespace TCF.Zeo.Cxn.Check.Contract
{
    public interface ICheckProcessor
    {
        CheckLogin GetCheckSessions(ZeoContext context);

        CheckTransaction Submit(long accountId, CheckInfo check, ZeoContext context);

        void Commit(long trxId, ZeoContext context);

        CheckTransaction Get(long trxId);

        CheckTransaction Status(long transactionId, ZeoContext context);

        bool Cancel(long trxId, ZeoContext context);

        List<PendingCheck> GetPendingChecks();

        CheckProcessorInfo GetCheckProcessorInfo(long agentSessionId, string locationId);

        void UpdateTransactionFranked(long trxId, bool isCheckFranked);

        long GetAccount(ZeoContext context);
    }
}
