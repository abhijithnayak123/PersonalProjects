using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public enum FeeAdjustmentTransactionType
	{
		Check = 1,
		FundsCredit = 2,
		FundsDebit = 3,
		FundsActivation = 4,
		MoneyOrder = 5
	}
}
