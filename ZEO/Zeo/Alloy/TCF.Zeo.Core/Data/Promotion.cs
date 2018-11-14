using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Data
{
    public class Promotion : ZeoModel
    {
        public PromotionDetail PromotionDetail { get; set; }

        public List<Qualifier> Qualifiers { get; set; }

        public List<Provision> Provisions { get; set; }
    }
}
