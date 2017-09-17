using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.WU.Common.Data;

namespace MGI.Cxn.BillPay.WU.Data
{
	public class PaymentTransaction
	{
        public Sender MyProperty { get; set; }
        public QpCompany Biller { get; set; }
        public Financials Financial { get; set; }
        public PaymentDetails PaymentDetail { get; set; }
        public DeliveryServices DeliveryService { get; set; }
		public string Conv_Session_Cookie { get; set; }

        public string FillingDate { get; set; }
        public string FillingTime { get; set; }
        public string MTCN { get; set; }
        public string NewMTCN { get; set; }

        public DfFields DfFields { get; set; }
	}
}
