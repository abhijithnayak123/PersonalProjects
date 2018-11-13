using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.MG.Data
{
    public enum TransferType : int
    {
        SendMoney = 1,
        ReceiveMoney,
        Refund
    }

    public enum MTReleaseStatus : int
    {
        Release = 2,
        Cancel = 3
    }

    public enum SendMoneyTransactionSubType : int
    {
        Cancel = 1,
        Modify = 2
    }

    public enum SendReversalType
    {
        C = 1,
        R = 2,
    }
}
