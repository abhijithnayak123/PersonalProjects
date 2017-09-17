using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data.Fees;

namespace MGI.Core.Partner.Contract
{
	public interface IFeeAdjustmentConditionFactory
	{

		/// <summary>
		/// This method is to get the fee conndition details
		/// </summary>
		/// <param name="f">This is fee adjustment condition details</param>
		/// <returns>Fee condition details</returns>
		IFeeCondition GetFeeCondition(FeeAdjustmentCondition f);
	}
}
