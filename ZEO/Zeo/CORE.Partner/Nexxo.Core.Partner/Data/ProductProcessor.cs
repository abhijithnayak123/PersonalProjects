using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class ProductProcessor : NexxoModel
	{
		public virtual Processor Processor { get; set; }
		public virtual Product Product { get; set; }
		public virtual bool IsSSNRequired { get; set; }
		public virtual long Code { get; set; }
		public virtual bool IsSWBRequired { get; set; }
		public virtual bool CanParkReceiveMoney { get; set; }
		public virtual int ReceiptCopies { get; set; }
		public virtual int ReceiptReprintCopies { get; set; }		
	}
}
