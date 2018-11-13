using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    //public class TipsAndOffers
    //{
    //    public virtual string TipsandOffers { get; set; }
    //    public virtual string OptionalFilter { get; set; }
    //}

    public class TipsAndOffers
    {
        public virtual int Id { get; set; }
        public virtual string ViewName { get; set; }
        public virtual string ChannelPartnerName { get; set; }
        public virtual string TipsAndOffersEn { get; set; }
        public virtual string TipsAndOffersEs { get; set; }
        public virtual string OptionalFilter { get; set; }
        public virtual DateTime DTServerCreate { get; set; }
        public virtual Nullable<DateTime> DTServerLastModified { get; set; }
        
        public string TipsAndOffersValue { get; set; }
    }
}
