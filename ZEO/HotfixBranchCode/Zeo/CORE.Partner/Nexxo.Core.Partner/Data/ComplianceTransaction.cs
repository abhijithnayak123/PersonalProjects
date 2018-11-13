using MGI.Common.DataAccess.Data;
using System;

namespace MGI.Core.Partner.Data
{
	public class ComplianceTransaction : NexxoModel
	{
    	public virtual long TransactionId { get; set; }
		public virtual long CustomerId { get; set; }
		public virtual int TransactionType { get; set; }
		public virtual decimal Amount { get; set; }
		public virtual decimal Fee { get; set; }
		public virtual int State { get; set; }
		public virtual int ProviderId { get; set; }
		public virtual long? xRecipientId { get; set; }
		public virtual decimal? xRate { get; set; }
		public virtual long? bpProductId { get; set; }
		public virtual string bpAccountNumber { get; set; }
		public virtual long? ShoppingCartId { get; set; }
	}
}
