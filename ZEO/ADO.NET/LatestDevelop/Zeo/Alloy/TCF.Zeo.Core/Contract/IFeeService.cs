﻿using System;
using System.Collections.Generic;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Contract
{
    public interface IFeeService : IDisposable
    {
        List<FeeAdjustment> GetChannelPartnerFeeAdj(TransactionType transactionType, long channelPartnerId, ZeoContext context);

        bool ValidatePromoCode(TransactionType transactionType, long channelPartnerId, string promotionCode, ZeoContext context);

        List<TransactionFeeAdjustment> GetAuthorizedTransaction(TransactionType transactionType, long customerId, ZeoContext context);

        int GetTransactionCount(TransactionType transactionType, long transactionId, long customerId, ZeoContext context);

        int GetTransactionsCountByPromoCode(ref string promoCode, TransactionType transactionType, long transactionId, bool systemApplied, DateTime dTStart, ZeoContext context);
    }
}
