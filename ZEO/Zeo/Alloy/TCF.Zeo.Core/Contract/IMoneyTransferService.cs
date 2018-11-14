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
    public interface IMoneyTransferService : IDisposable
    {
        long CreateTransaction(MoneyTransfer moneyTransfer, ZeoContext context);

        void UpdateTransaction(MoneyTransfer moneyTransfer, ZeoContext context);

        MoneyTransfer GetTransaction(long transactionId, ZeoContext context);

        void UpdateTransactionState(long transactionId, int state, DateTime dtTerminalDate, DateTime dtServerDate, ZeoContext context);

        ModifyResponse AddModifyandRefundTransactions(MoneyTransfer moneyTransfer, ZeoContext context);

        void UpdateTransactionStates(long cancelTranId, int state, DateTime dtTerminalDate, DateTime dtServerDate, ZeoContext context);

        bool UpdatePTNRTransactionStates(long transactionId, int state, DateTime dtTerminalDate, DateTime dtServerDate, ZeoContext context);

        void UpdateModifyorRefundTransactions(ModifyResponse modifyResp, long wuCancelTrxId, long wuModifyTrxId, DateTime dtTerminalDate, DateTime dtServerDate, ZeoContext context);

        WUCountry GetBlockedUnBlockedCountries(ZeoContext context);

        bool SaveBlockedCountries(List<string> blockedCountries, ZeoContext context);
    }
}
