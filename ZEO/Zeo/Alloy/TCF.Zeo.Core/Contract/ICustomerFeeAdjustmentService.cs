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
    public interface ICustomerFeeAdjustmentService : IDisposable
    {
        /// <summary>
		/// This method is to update the customer fee adjustments
		/// </summary>
		/// <param name="customerId">This is customer id</param>
		/// <param name="adjustment">This is fee adjustment details</param>
		/// <returns>Customer fee adjustments details</returns>
		List<FeeAdjustment> LookupCustomerFeeAdjustments(TransactionType transactionType, long customerId, ZeoContext context);

    }
}
