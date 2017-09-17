using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public class DaysSinceRegistrationCondition : IFeeCondition
	{
		public override bool MeetsCondition(CustomerSession session, List<Transactions.Transaction> transactions, MGIContext mgiContext)
		{
			return meetsCondition<int>((DateTime.Today - session.Customer.DTServerCreate.Date).Days);
		}
	}
}
