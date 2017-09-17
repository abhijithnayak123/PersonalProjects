using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Cxn.WU.Common.Data
{
	public class CardDetails
	{
		public string AccountNumber { get; set; }
		public string ForiegnSystemId { get; set; }
		public string ForiegnRefNum { get; set; }
        public string CounterId { get; set; }
        public PaymentDetails paymentDetails { get; set; }
        public Sender sender { get; set; }

	}
}
