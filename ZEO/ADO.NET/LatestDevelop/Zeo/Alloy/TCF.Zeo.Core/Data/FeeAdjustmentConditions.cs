using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class FeeAdjustmentConditions
    {
        public long FeeAdjustmentId { get; set; }

        public int ConditionType { get; set; }

        public int CompareType { get; set; }

        public string ConditionValue { get; set; }
    }
}
