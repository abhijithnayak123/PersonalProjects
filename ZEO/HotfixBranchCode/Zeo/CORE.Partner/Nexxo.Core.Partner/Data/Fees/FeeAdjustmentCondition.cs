using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public class FeeAdjustmentCondition
	{
		public virtual Guid rowguid { get; set; }
		public virtual FeeAdjustment feeAdjustment { get; set; }
		public virtual string Description { get; set; }
		public virtual int ConditionType { get; set; }
		public virtual int CompareType { get; set; }
		public virtual string ConditionValue { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual DateTime? DTServerLastModified { get; set; }
	}
}
