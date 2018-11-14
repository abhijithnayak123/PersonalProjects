using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    public class PromotionSearchCriteria 
    {
        public string PromotionName { get; set; }

        public MasterData Product { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool ShowExpired { get; set; }
    }
}
