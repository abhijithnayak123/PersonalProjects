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
    public interface IComplianceService
    {
        Limit GetTransactionLimit(TransactionType transactionType, long channelPartnerId, ZeoContext context);

        decimal GetComplianceTransactionTotal(long customerSessionId, TransactionType transactionType, bool shouldIncludeShoppingCartItems, int period, ZeoContext context);
    }
}
