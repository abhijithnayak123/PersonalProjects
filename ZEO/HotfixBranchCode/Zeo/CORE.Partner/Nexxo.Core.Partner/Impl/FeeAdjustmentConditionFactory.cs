using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data.Fees;
using MGI.Core.Partner.Contract;

namespace MGI.Core.Partner.Impl
{
	internal class FeeAdjustmentConditionFactory : IFeeAdjustmentConditionFactory
	{
		public IFeeCondition GetFeeCondition(FeeAdjustmentCondition f)
		{
			IFeeCondition adjustmentCondition;

			switch ((ConditionTypes)f.ConditionType)
			{
				case ConditionTypes.CheckType:
					adjustmentCondition = new CheckTypeCondition { Condition = f };
					break;
				case ConditionTypes.Group:
					adjustmentCondition = new GroupCondition { Condition = f };
					break;
				case ConditionTypes.Location:
					adjustmentCondition = new LocationCondition { Condition = f };
					break;
				case ConditionTypes.RegistrationDate:
					adjustmentCondition = new RegistrationDateCondition { Condition = f };
					break;
				case ConditionTypes.DaysSinceRegistration:
					adjustmentCondition = new DaysSinceRegistrationCondition { Condition = f };
					break;
				case ConditionTypes.TransactionAmount:
					adjustmentCondition = new TransactionAmountCondition { Condition = f };
					break;
				case ConditionTypes.TransactionCount:
					adjustmentCondition = new TransactionCountCondition { Condition = f };
					break;
				case ConditionTypes.Referral: // US1800 Referral & Referree Promotions
					adjustmentCondition = new ReferralCondition { Condition = f };
					break;
				case ConditionTypes.Code: //US1799 Targeted promotions for check cashing and money order 
					adjustmentCondition = new CodeCondition { Condition = f };
					break;
				default:
					adjustmentCondition = new DefaultCondition { Condition = f };
					break;
			}
			return adjustmentCondition;
		}
	}
}
