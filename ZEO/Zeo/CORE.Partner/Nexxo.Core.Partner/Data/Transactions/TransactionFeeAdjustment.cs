using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data.Fees;
using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data.Transactions
{
	public class TransactionFeeAdjustment : NexxoModel
	{
		public virtual Transaction transaction { get; set; }
		public virtual FeeAdjustment feeAdjustment { get; set; }
		// AL-591: This is introduced to update IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
		// Developed by: Sunil Shetty || 03/07/2015
		public virtual bool IsActive { get; set; }
	}
}
