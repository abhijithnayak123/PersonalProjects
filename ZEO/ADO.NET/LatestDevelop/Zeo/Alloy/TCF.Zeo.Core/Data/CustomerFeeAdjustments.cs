using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class CustomerFeeAdjustments : ZeoModel
    {
        public long FeeAdjustmentId { get; set; }

        public long CustomerID { get; set; }

        public bool IsAvailed { get; set; }

        public long TransactionId { get; set; }
    }
}
