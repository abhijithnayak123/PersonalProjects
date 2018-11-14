using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Cxn.Check.Chexar.Data
{
    public class ChexarSimInvoice : ZeoModel
    {
        public virtual int ChxrSimInvoiceId { get; set; }
        public virtual int TicketId { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal Fee { get; set; }
        public virtual int CheckType { get; set; }
        public virtual string Status { get; set; }
        public virtual string WaitTime { get; set; }
        public virtual int DeclineId { get; set; }
        public virtual string DeclineReason { get; set; }
        public virtual long ChexarSimAccountId { get; set; }
        public virtual int BadgeId { set; get; }
    }
}
