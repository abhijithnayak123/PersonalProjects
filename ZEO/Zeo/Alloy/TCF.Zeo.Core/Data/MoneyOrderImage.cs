using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Data
{
    public class MoneyOrderImage : ZeoModel
    {
        public virtual long TransactionId { get; set; }
        public virtual byte[] CheckFrontImage { get; set; }
        public virtual byte[] CheckBackImage { get; set; }
    }
}
