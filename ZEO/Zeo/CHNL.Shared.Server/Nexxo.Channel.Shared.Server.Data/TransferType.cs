using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    public enum TransferType : int
    {
        SendMoney = 1,
        RecieveMoney = 2,
        Refund = 3
    }

    public enum MTReleaseStatus : int
    {
        Hold = 1,
        Release = 2,
        Cancel = 3
    }

    public enum TransactionSubType : int
    {
        Cancel = 1,
        Modify = 2,
        Refund = 3
    }

    public enum RefundStatus 
    {
        F,
        N
    }
}
