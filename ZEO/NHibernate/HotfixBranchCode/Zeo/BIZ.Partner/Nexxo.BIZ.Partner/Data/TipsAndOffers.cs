using System;

namespace MGI.Biz.Partner.Data
{
 public class TipsAndOffers
    {
        public int Id { get; set; }
        public string ViewName { get; set; }
        public string ChannelPartnerName { get; set; }
        public string TipsAndOffersEn { get; set; }
        public string TipsAndOffersEs { get; set; }
        public string OptionalFilter { get; set; }
        //public DateTime DTCreate { get; set; }
        //public Nullable<DateTime> DTLastMod { get; set; }
        public string TipsAndOffersValue { get; set; }
    }
}
