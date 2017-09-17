using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data
{
    public class TransactionFeeAdjustment : ZeoModel
    {
        public long TransactionId { get; set; }

        public long FeeAdjustmentId { get; set; }
        
        // AL-591: This is introduced to update IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
        public bool IsActive { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}
