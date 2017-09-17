using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class FeeAdjustment : ZeoModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DTStart { get; set; }

        public DateTime? DTEnd { get; set; }

        public bool SystemApplied { get; set; }

        public decimal AdjustmentRate { get; set; }

        public decimal AdjustmentAmount { get; set; }

        public string PromotionType { get; set; }

        public List<FeeAdjustmentConditions> Conditions { get; set; }
        
    }
}
