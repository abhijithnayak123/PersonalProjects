using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data.Transactions;
using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data.Fees
{
	public class FeeAdjustment : NexxoModel
	{
		public virtual ChannelPartner channelPartner { get; set; }
		public virtual IList<FeeAdjustmentCondition> Conditions { get; set; }
		public virtual FeeAdjustmentTransactionType TransactionType { get; set; }
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
		public virtual DateTime DTStart { get; set; }
		public virtual DateTime? DTEnd { get; set; }
		public virtual bool SystemApplied { get; set; }
		public virtual decimal AdjustmentRate { get; set; }
		public virtual decimal AdjustmentAmount { get; set; }
		//US1799 & US1800 Promotions Changes
		public virtual string PromotionType {get; set;}		
	}
}
