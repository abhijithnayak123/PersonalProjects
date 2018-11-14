using System;
using System.Collections.Generic;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Contract
{
    public interface IFeeService : IDisposable
    {
        List<FeeAdjustment> GetChannelPartnerFeeAdj(TransactionType transactionType, long channelPartnerId, ZeoContext context);

        bool ValidatePromoCode(TransactionType transactionType, long channelPartnerId, string promotionCode, ZeoContext context);

        List<TransactionFeeAdjustment> GetAuthorizedTransaction(TransactionType transactionType, long customerId, ZeoContext context);

        int GetTransactionCount(TransactionType transactionType, long transactionId, long customerId, ZeoContext context);

        int GetTransactionsCountByPromoCode(ref string promoCode, TransactionType transactionType, long transactionId, bool systemApplied, DateTime dTStart, ZeoContext context);
        int GetCommittedTrxCountByPromoCode(string name, TransactionType transactionType, long transactionId, bool systemApplied, DateTime dTStart, ZeoContext context);


        #region New Promo Engine code
        List<TransactionFee> GetPromotionEligibility(decimal baseFee, decimal amount, int checkTypeId,
                long transactionId, TransactionType transactionType, ZeoContext context);

        #endregion
    }
}
