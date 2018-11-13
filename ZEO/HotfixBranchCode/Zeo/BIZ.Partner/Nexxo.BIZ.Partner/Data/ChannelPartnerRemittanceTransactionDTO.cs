using System;

namespace MGI.Biz.Partner.Data
{
    public class ChannelPartnerRemittanceTransactionDTO
    {
        public Guid Id { get; set; }
        public int ChannelPartnerId { get; set; }
        public DateTime DTPayment { get; set; }
        public decimal PaymentAggregate { get; set; }
        //public DateTime DTCreate { get; set; }
        //public DateTime DTLastMod { get; set; }
    }
}
