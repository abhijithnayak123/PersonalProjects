using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class LoadResponse
    {
        public long TransactionKey { get; set; }
        public long ReloadAliasId { get; set; }
        public string TransationId { get; set; }
    }
}
