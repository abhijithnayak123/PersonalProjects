using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.WU.Common.Data;    

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
	public class ModifySendMoneySearchResponse
	{
        public PaymentTransaction paymentTransaction {get;set;}
		public DeliveryServices deliveryServices { get; set; }
 		public string fusion_status { set; get; }

        public string database_key { get; set; }
        public string mtcn { set; get; }
        public string paystatusdescription { get; set; }

        public Sender sender { get; set; }
        public Receiver receiver { get; set; }
	}
}
