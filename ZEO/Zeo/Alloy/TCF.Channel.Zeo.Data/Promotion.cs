using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Data
{
    public class Promotion
    {
        public PromotionDetail PromotionDetail { get; set; }

        public List<Qualifier> Qualifiers { get; set; }

        public List<Provision> Provisions { get; set; }
    }
}
