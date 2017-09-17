using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class CardDetails
    {
        public string AccountNumber { get; set; }
        public string ForiegnSystemId { get; set; }
        public string ForiegnRefNum { get; set; }
        public string CounterId { get; set; }
    }
}
