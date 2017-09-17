using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public class GroupCondition : IFeeCondition
	{
		public override bool MeetsCondition(CustomerSession session, List<Transactions.Transaction> transactions, MGIContext mgiContext)
		{
			foreach (CustomerGroupSetting g in session.Customer.Groups)
				if (meetsStringCondition(g.channelPartnerGroup.Name))
					return true;

			return false;
		}
	}
}
