using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Data
{
    public class ProductProcessor : ZeoModel
    {
        public Processor Processor { get; set; }
        public Product Product { get; set; }
        public bool IsSSNRequired { get; set; }
        public long Code { get; set; }
        public bool IsSWBRequired { get; set; }
        public bool CanParkReceiveMoney { get; set; }
        public int ReceiptCopies { get; set; }
        public int ReceiptReprintCopies { get; set; }
    }
}
