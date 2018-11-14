using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Common.Data
{
    public class Limit
    {
        public Nullable<decimal> PerTransactionMaximum { get; set; }
        public Nullable<decimal> PerTransactionMinimum { get; set; }
    }
}
