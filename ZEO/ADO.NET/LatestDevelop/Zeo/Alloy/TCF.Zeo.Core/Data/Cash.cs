using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Data
{
    public class Cash : Transaction
    {
        public Helper.CashType CashType { get; set; }
    }
}
