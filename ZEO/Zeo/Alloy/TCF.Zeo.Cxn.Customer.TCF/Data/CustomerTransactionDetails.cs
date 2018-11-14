using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CustomerTransactionDetails
    {
        public CustomerDetails Customer { get; set; }
        public List<Transaction> Transactions { get; set; }

    }
}
