using System;

using MGI.Common.DataAccess.Data;
using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.FIS.ISO8583;

namespace MGI.Cxn.Fund.FIS.Data
{
    public class FISTrx : NexxoModel
	{
        public virtual FISAccount Account { get; set; }
        public virtual FISTransactionType TransactionType { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal Fee { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime? DTLocalTransaction { get; set; }
        public virtual DateTime? DTTransmission { get; set; }
        public virtual string ConfirmationId { get; set; }
        public virtual string ErrorCode { get; set; }
        public virtual string ErrorMsg { get; set; }
        public virtual FISTransactionStatus Status { get; set; }
        public virtual decimal Balance { get; set; }
        public virtual Nullable<long> ChannelPartnerID { get; set; }
	}
}
