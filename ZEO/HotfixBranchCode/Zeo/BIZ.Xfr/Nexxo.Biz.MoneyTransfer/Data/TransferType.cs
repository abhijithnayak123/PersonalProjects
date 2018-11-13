using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public enum TransferType : int
    {
        SendMoney = 1,
        ReceiveMoney = 2,
        Refund = 3
    }

    public enum TransactionSubType : int
    {
        Cancel = 1,
        Modify = 2,
        Refund = 3
    }


}
