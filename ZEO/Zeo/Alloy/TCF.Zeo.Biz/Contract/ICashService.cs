using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
    public interface ICashService
    {
        bool CashIn(decimal amount, commonData.ZeoContext context);

        bool UpdateOrCancelCashIn(decimal amount, commonData.ZeoContext context);

        CashTransaction GetCashTransaction(long transactionId, commonData.ZeoContext context);

        bool RemoveCashIn(commonData.ZeoContext context);

    }
}
