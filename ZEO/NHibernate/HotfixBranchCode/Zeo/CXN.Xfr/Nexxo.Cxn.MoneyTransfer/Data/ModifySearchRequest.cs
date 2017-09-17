using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
    public class ModifySearchRequest
    {
        public PaymentTransaction paymentTransaction { get; set; }
        public string referenceNo { get; set; }
    }
}
