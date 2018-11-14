using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class PromotionSearchCriteria
    {
        public string PromotionName { get; set; }

        public Product Product { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool ShowExpired { get; set; }
    }
}
