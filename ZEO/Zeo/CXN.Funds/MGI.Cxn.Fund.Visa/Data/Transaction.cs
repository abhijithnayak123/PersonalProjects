using MGI.Common.DataAccess.Data;
using MGI.Cxn.Fund.Data;
using System;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class Transaction : NexxoModel
	{
		public virtual Account Account { get; set; }
		public virtual TransactionType TransactionType { get; set; }
		public virtual decimal Amount { get; set; }
		public virtual decimal? Fee { get; set; }
		public virtual string Description { get; set; }
		public virtual TransactionState Status { get; set; }
		public virtual string ConfirmationId { get; set; }
		public virtual long ChannelPartnerID { get; set; }
		public virtual decimal Balance { get; set; }
		public virtual string ErrorCode { get; set; }
		public virtual string ErrorMsg { get; set; }
		public virtual string PromoCode { get; set; }
		public virtual long LocationNodeId { get; set; }
		public virtual Nullable<DateTime> DTTransmission { get; set; }
	}
}
