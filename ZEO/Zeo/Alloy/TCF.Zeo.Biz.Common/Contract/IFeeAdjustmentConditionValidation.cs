using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System.Collections.Generic;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Biz.Common.Contract
{
    public interface IFeeAdjustmentConditionValidation
    {
        bool CheckTypeCondition(int checkType, CompareTypes compareType, string conditionValue, ZeoContext context);

        bool CodeCondition(string promoCode, CompareTypes compareType, string conditionValue, ZeoContext context);

        bool GroupCondition(CompareTypes compareType, string conditionValue, ZeoContext context);

        bool LocationCondition(string locationId, CompareTypes compareType, string conditionValue, ZeoContext context);

        bool RegistrationDateCondition(CompareTypes compareType, string conditionValue, ZeoContext context);

        bool TransactionAmountCondition(decimal amount, CompareTypes compareType, string conditionValue, ZeoContext context);

        bool DaysSinceRegistrationCondition(CompareTypes compareType, string conditionValue, ZeoContext context);

        bool ReferralCondition(CompareTypes compareType, string conditionValue, ZeoContext context);

        bool TransactionCountCondition(TransactionType transactionType, long transactionId, CompareTypes compareType, string conditionValue, ZeoContext context);
       
        bool AggregateCondition(ref FeeAdjustment feeAdjustment, TransactionType transactionType, long transactionId, CompareTypes compareType, string conditionValue, ZeoContext context);
        bool CommittedTransactionCount(FeeAdjustment feeAdj, TransactionType transactionType, long transactionId, CompareTypes compareType, string conditionValue, ZeoContext context);
    }
}
