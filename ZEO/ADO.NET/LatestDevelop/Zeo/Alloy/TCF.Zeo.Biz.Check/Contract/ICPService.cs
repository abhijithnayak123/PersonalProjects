using System;
using System.Collections.Generic;
using TCF.Zeo.Common.Util;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Check.Contract
{
    public interface ICPService
    {
        CheckLogin GetChexarSessions(commonData.ZeoContext context);

        TCF.Channel.Zeo.Data.Check Submit(CheckSubmission check, commonData.ZeoContext context);

        TCF.Channel.Zeo.Data.Check GetStatus(long transactionId, bool includeImage, commonData.ZeoContext context);

        bool Cancel(long transactionId, commonData.ZeoContext context);

        void Commit(long transactionId, commonData.ZeoContext context);

        List<CheckType> GetCheckTypes(commonData.ZeoContext context);

        TransactionFee GetFee(CheckSubmission check, commonData.ZeoContext context);

        CheckTransactionDetails GetTransaction(long transactionId, commonData.ZeoContext context);

        CheckFrankingDetails GetCheckFrankingData(long transactionId, commonData.ZeoContext context);

        CheckProcessorInfo GetCheckProcessorInfo(commonData.ZeoContext context);

        void UpdateCheckTransactionFranked(long transactionId, commonData.ZeoContext context);

        bool Resubmit(long transactionId, commonData.ZeoContext context);
    }
}
