using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    public class Receipt
    {
            public List<Receipt> receipts { get; set; }

            public List<string> receiptType { get; set; }

            public string Name { get; set; }

            public string PrintData { get; set; }

            public int NumberOfCopies { get; set; }
        }
}