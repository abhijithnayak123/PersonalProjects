using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Fund.TSys.Data
{
	public class TSysTransaction : NexxoModel
	{
		public virtual TSysAccount Account { get; set; }
		public virtual TSysTransactionType TransactionType { get; set; }
		public virtual decimal Amount { get; set; }
		public virtual decimal Fee { get; set; }
		public virtual string Description { get; set; }
		public virtual DateTime? DTLocalTransaction { get; set; }
		public virtual DateTime? DTTransmission { get; set; }
		public virtual string ConfirmationId { get; set; }
		public virtual string ErrorCode { get; set; }
		public virtual string ErrorMsg { get; set; }
		public virtual TSysTransactionStatus Status { get; set; }
		public virtual decimal Balance { get; set; }
        public virtual Nullable<long> ChannelPartnerID { get; set; }
	}
}
