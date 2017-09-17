using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Fund.FIS.Data
{
    public enum FISTransactionStatus
    {
        Staged = 1,
        Committed = 2,
        Failed = 3
    }
}
