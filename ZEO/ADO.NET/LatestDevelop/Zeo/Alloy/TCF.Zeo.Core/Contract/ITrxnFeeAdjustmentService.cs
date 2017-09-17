using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface ITrxnFeeAdjustmentService
    {
        /// <summary>
		/// This method is to create the customer fee adjustments
		/// </summary>
		/// <param name="trxFeeAdjustment">This is customer fee adjustment details</param>
		/// <returns>Boolean value which is the customer is created or not</returns>
		bool Create(TransactionFeeAdjustment trxFeeAdjustment, ZeoContext context);

        /// <summary>
        /// This method is to update the customer fee adjustments
        /// </summary>
        /// <param name="trxFeeAdjustment">This is customer fee adjustment details</param>
        /// <returns>Boolean value which is the customer is updated or not</returns>
        bool Update(TransactionFeeAdjustment trxFeeAdjustment, ZeoContext context);
    }
}
