using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Biz.Common.Data;
using static TCF.Zeo.Common.Util.Helper;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Common.Contract
{
    public interface IComplianceService
    {
        Limit GetTransactionLimit(TransactionType transactionType, commonData.ZeoContext context);

        /// <summary>
        /// While activating gpr cart we are checking the minimum initial load.
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        decimal GetTransactionMinimumLimit(TransactionType transactionType, commonData.ZeoContext context);
    }
}
