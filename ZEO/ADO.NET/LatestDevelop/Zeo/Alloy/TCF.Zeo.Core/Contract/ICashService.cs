using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;

namespace TCF.Zeo.Core.Contract
{
    public interface ICashService
    {
        bool CashIn(long customerSessionId, decimal amount, string timeZone, ZeoContext context);

        bool UpdateOrCancelCash(long customerSessionId, decimal amount, string timeZone, ZeoContext context);

        CashTransaction GetCashTransaction(long transactionId, ZeoContext context);

        bool RemoveCashIn(long transactionId, string timeZone, ZeoContext context);
    }
}
