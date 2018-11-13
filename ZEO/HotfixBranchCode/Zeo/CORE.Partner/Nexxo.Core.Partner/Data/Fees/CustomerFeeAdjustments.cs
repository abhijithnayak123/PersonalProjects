using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{

	/// <summary>
	/// US1800 Referral promotions – Free check cashing to referrer and referee 
	/// Added CustomerFeeAdjustment class
	/// </summary>
	public class CustomerFeeAdjustments
	{
		public virtual Guid rowguid { get; set; }
		public virtual FeeAdjustment feeAdjustment { get; set; }
		public virtual long Id { get; set; }
		public virtual long CustomerID { get; set; }
		public virtual bool IsAvailed { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual DateTime? DTServerLastModified { get; set; }
		public virtual DateTime DTTerminalCreate { get; set; }
		public virtual DateTime? DTTerminalLastModified { get; set; }
	}
}
