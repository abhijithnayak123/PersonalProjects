using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
    public class PaymentTransaction
    {
        public string mtcn { set; get; }
        public Sender sender { get; set; }
        public Receiver receiver { get; set; }
	    public PaymentDetails paymentdetails { get; set; }
	    public Financials financials { get; set; }
    }
}


