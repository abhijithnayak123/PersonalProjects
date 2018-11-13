using System;

using MGI.Common.DataAccess.Data;
using MGI.Cxn.Check.Data;

namespace MGI.Cxn.Check.Chexar.Data
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
	public class ChexarTrx : NexxoModel
    {
        public virtual decimal Amount { get; set; }
		public virtual decimal ChexarAmount { get; set; }
		public virtual decimal ChexarFee { get; set; }

        public virtual DateTime? CheckDate { get; set; }
		
        public virtual string CheckNumber { get; set; }

        public virtual string RoutingNumber { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual string Micr { get; set; }

        public virtual double Latitude { get; set; }

        public virtual double Longitude { get; set; }

        public virtual int InvoiceId { get; set; }

        public virtual int TicketId { get; set; }

        public virtual string WaitTime { get; set; }

        public virtual CheckStatus Status { get; set; }
        public virtual string ChexarStatus { get; set; }

		public virtual int SubmitType { get; set; }
		public virtual int ReturnType { get; set; }

        public virtual int DeclineCode { get; set; }

        public virtual string Message { get; set; }

        public virtual string Location { get; set; }

        public virtual ChexarAccount Account { get; set; }

        public virtual Nullable<long> ChannelPartnerID { get; set; }

		public virtual bool IsCheckFranked { get; set; }
    }
}
 