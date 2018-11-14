using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Data
{
    public class TipsAndOffers: ZeoModel
    {
        public string ViewName { get; set; }
        public string ChannelPartnerName { get; set; }
        public string TipsAndOffersEn { get; set; }
        public string TipsAndOffersEs { get; set; }
        public string OptionalFilter { get; set; }
        public string TipsAndOffersValue { get; set; }
    }
}
