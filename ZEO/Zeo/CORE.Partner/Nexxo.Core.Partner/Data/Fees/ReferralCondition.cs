using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public class ReferralCondition : IFeeCondition
	{
		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
		/// Referral Condition  added to check customer has any Referral Code
		/// </summary>
		/// <param name="session"></param>
		/// <param name="transactions"></param>
		/// <param name="otherData"></param>
		/// <returns></returns>
		public override bool MeetsCondition(CustomerSession session, List<Transactions.Transaction> transactions, MGIContext mgiContext)
		{			
			if (!string.IsNullOrEmpty(session.Customer.ReferralCode))
					return true;

			return false;
		}
	}
}
