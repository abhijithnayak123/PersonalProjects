using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.WU.Common.Data;

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
    public class SearchResponse : PaymentTransaction
    {
        public string database_key { set; get; }
        public string money_transfer_key { set; get; }
	    public string NewMtcn { get; set; }
        public string paystatusdescription { get; set; }
		public string RefundStatus { get; set; }

    }
}
