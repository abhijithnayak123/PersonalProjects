using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class ModifySendMoneyRequest
    {
        public WUTransaction Transaction { get; set; }
        public WUAccount Account { get; set; }
        public Receiver Receiver { get; set; }
    }
}
