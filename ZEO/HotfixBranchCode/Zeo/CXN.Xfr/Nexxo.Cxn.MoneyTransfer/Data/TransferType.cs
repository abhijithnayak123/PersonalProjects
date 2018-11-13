using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
    public enum MoneyTransferType : int
    {
        Send = 1,
        Receive = 2,
        Refund = 3
    }

    public enum TransactionSubType : int
    {
        Cancel = 1,
        Modify = 2,
        Refund = 3
    }

    public enum SendType
    {
        Fxd,
        Estd
    }
}
