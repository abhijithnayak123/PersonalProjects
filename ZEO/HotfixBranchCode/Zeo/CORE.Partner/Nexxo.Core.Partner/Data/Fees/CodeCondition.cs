using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public class CodeCondition : IFeeCondition
	{
		/// <summary>
		/// US1799 Targeted promotions for check cashing and money order
		/// Code Condition for checking Promotion Codes manually entered
		/// </summary>
		/// <param name="session"></param>
		/// <param name="transactions"></param>
		/// <param name="otherData"></param>
		/// <returns></returns>
		public override bool MeetsCondition(CustomerSession session, List<Transactions.Transaction> transactions, MGIContext mgiContext)
		{
			return meetsStringCondition(mgiContext.PromotionCode);			
		}
	}
}
