using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class PaymentTranasction
    {
        public string DatabaseKey { set; get; }
        public string MTCN { set; get; }
        public string PayStatus { get; set; }
        public Account sender { get; set; }
        public Receiver receiver { get; set; }
        public PaymentDetails paymentDetails { get; set; }
    }

}
