using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Data
{
	public class ChannelPartnerProductProvider
	{
		public string ProductName { get; set; }
		public string ProcessorName { get; set; }
		public string ProcessorId { get; set; }
		public int Sequence { get; set; }
		public bool IsSSNRequired { get; set; }
		public bool IsSWBRequired { get; set; }
		public bool IsTnCForcePrintRequired { get; set; }
		public virtual bool CanParkReceiveMoney { get; set; }
		public int ReceiptCopies { get; set; }
		public int ReceiptReprintCopies { get; set; }
		public CheckEntryTypes CheckEntryType { get; set; }
		public int MinimumTransactAge { get; set; }
	}
}
