using System;

namespace TCF.Channel.Zeo.Data
{
    public class TransactionHistory
    {
        public long CustomerId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Teller { get; set; }
        public long TellerId { get; set; }
        public long SessionID { get; set; }
        public long TransactionId { get; set; }
        public string Location { get; set; }
        public string TransactionType { get; set; }
        public decimal TotalAmount { get; set; }
        public string TransactionDetail { get; set; }
        public string CustomerName { get; set; }
        public string TransactionStatus { get; set; }
    }
}
