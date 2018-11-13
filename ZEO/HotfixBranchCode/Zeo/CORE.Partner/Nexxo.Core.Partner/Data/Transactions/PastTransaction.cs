using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data.Transactions
{
    public class PastTransaction :NexxoModel
    {
        public virtual long CXEId { get; set; }
        public virtual long CXNId { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal Fee { get; set; }
        public virtual string TransactionType { get; set; }
        public virtual string BillerCode { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string Country { get; set; }
        public virtual string State { get; set; }
        public virtual long CustomerId { get; set; }
        public virtual string BillerName { get; set; }
        public virtual string ReceiverFirstName { get; set; }
        public virtual string ReceiverLastName { get; set; }

    }
}
