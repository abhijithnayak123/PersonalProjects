using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class TransactionFinancials
    {
        public float ExchangeRate { get; set; }
        public long OriginatorsPrincipalAmount { get; set; }
        public long DestinationPrincipalAmount { get; set; }
        public long GrossTotalAmount { get; set; }
        public long Charges { get; set; }
        public long TaxAmount { get; set; }
        public string Mtcn { get; set; }
        public bool Status { get; set; }   
    }
}
