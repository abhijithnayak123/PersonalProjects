using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class TransactionSubTypes
    {
        public enum MTType : int
        {
            Cancel = 1,
            Modify = 2,
            Refund = 3
        }
    }
}
