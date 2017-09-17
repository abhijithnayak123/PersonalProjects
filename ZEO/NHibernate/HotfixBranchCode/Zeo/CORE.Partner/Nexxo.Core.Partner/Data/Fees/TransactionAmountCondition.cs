using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MGI.Common.Util;

namespace MGI.Core.Partner.Data.Fees
{
	public class TransactionAmountCondition : IFeeCondition
	{
		public override bool MeetsCondition(CustomerSession session, List<Transactions.Transaction> transactions, MGIContext mgiContext)
		{
			return meetsCondition<decimal>(mgiContext.Amount);
		}
	}
}
