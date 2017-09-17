using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class AccountHistory
    {
        public DateTime TransactionDate { get; set; }

        public string TransactionType { get; set; }

        public string AdditionalDetails { get; set; }
    }
}
