using System;

namespace MGI.Biz.Partner.Data.Transactions
{
    public class PastTransaction : Transaction
    {
        public string TransactionType { get; set; }
        public string BillerCode { get; set; }
        public string AccountNumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string BillerName { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public DateTime DTLastMod { get; set; }
    }
}
