using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
   public class PaymentTransaction
    {
        public string database_key { get; set; }
        public string mtcn { set; get; }
        //public string new_mtcn { get; set; }
       // public string money_transfer_key { get; set; }
        public string paystatusdescription { get; set; }
        public Account sender { get; set; }
        public Receiver receiver { get; set; }
        public PaymentDetails paymentDetails { get; set; }
    }
}
