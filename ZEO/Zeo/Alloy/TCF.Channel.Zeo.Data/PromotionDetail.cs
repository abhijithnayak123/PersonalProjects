using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Channel.Zeo.Data
{
    public class PromotionDetail
    {
        public long PromotionId { get; set; }

        public string PromotionName { get; set; }

        public string PromotionDescription { get; set; }

        public MasterData Product { get; set; }

        public MasterData Provider { get; set; }

        public int? Priority { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsSystemApplied { get; set; }

        public bool IsOverridable { get; set; }

        public bool IsPrintable { get; set; }

        public bool IsNextCustomerSession { get; set; }

        public PromotionStatus PromotionStatus { get; set; }

        public int? FreeTxnCount { get; set; }

        public bool Stackable { get; set; }

        public bool IsPromotionHidden { get; set; }
    }
}
