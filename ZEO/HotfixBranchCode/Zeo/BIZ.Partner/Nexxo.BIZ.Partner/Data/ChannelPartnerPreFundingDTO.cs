using System;

namespace MGI.Biz.Partner.Data
{
    public class ChannelPartnerPreFundingDTO
    {
        public Guid Id { get; set; }
        public int ChannelPartnerId { get; set; }
        public decimal Amount { get; set; }
        public float Threshold { get; set; }
        //public DateTime DTCreate { get; set; }
        //public DateTime DTLastMod { get; set; }
    }
}
