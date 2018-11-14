using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data
{
    public class ChannelPartnerProductProvider : ZeoModel
    {
        public string ProductName { get; set; }
        public string ProcessorName { get; set; }
        public long ProcessorId { get; set; }
        public int Sequence { get; set; }
        public bool IsSSNRequired { get; set; }
        public bool IsSWBRequired { get; set; }
        public bool IsTnCForcePrintRequired { get; set; }
        public virtual bool CanParkReceiveMoney { get; set; }
        public int ReceiptCopies { get; set; }
        public int ReceiptReprintCopies { get; set; }
        public CheckEntryTypes CheckEntryType { get; set; }
        public int MinimumTransactAge { get; set; }
    }
}
