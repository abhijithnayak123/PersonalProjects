using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Contract;
using CoreData = TCF.Zeo.Core;

namespace TCF.Zeo.Biz.Common.Impl
{
    public class ComplianceServiceImpl : IComplianceService
    {
        CoreData.Contract.IComplianceService compliance = new CoreData.Impl.ComplianceServiceImpl();
        public Common.Data.Limit GetTransactionLimit(Helper.TransactionType transactionType, ZeoContext context)
        {
            Common.Data.Limit limit = new Common.Data.Limit();

            CoreData.Data.Limit coreLimit = compliance.GetTransactionLimit(transactionType, context.ChannelPartnerId,context);

            limit.PerTransactionMinimum = coreLimit.PerTransactionMinimum.GetValueOrDefault(0.00M);

            limit.PerTransactionMaximum = GetMaximumLimit(context.CustomerSessionId, transactionType, coreLimit, context);

            limit.PerTransactionMaximum = limit.PerTransactionMaximum < limit.PerTransactionMinimum ? 0 : limit.PerTransactionMaximum;

            return limit;
        }

        public decimal GetTransactionMinimumLimit(Helper.TransactionType transactionType, ZeoContext context)
        {
            CoreData.Data.Limit coreLimit = compliance.GetTransactionLimit(transactionType, context.ChannelPartnerId, context);

            return coreLimit.PerTransactionMinimum.GetValueOrDefault(0.00M);
        }

        private decimal GetMaximumLimit(long customerSessionId, Helper.TransactionType transactionType, CoreData.Data.Limit limit, ZeoContext context)
        {
            decimal maxPossibleAmount = limit.PerTransactionMaximum.GetValueOrDefault(decimal.MaxValue);

            decimal xDayTrxsTotalAmount = decimal.MinValue;

            foreach (var xDayLimit in limit.NDaysLimit)
            {
                if (maxPossibleAmount != 0)
                {
                    xDayTrxsTotalAmount = compliance.GetComplianceTransactionTotal(customerSessionId, transactionType, context.ShouldIncludeShoppingCartItems, xDayLimit.Key, context);

                    var maxAmount = xDayLimit.Value - xDayTrxsTotalAmount < 0 ? 0 : xDayLimit.Value - xDayTrxsTotalAmount;

                    if (maxPossibleAmount > maxAmount)
                    {
                        maxPossibleAmount = maxAmount;
                    }
                }
            }
            if (maxPossibleAmount == 0)
            {
                maxPossibleAmount = decimal.MaxValue;
            }
            return maxPossibleAmount;
        }
    }
}
