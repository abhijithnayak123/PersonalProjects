using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class BillPay : Transaction
    {
        public long CustomerSessionId { set; get; }
        public long ProviderAccountId { set; get; }
        public int ProviderId { set; get; }
        public string AccountNumber { get; set; }
        public long ProductId { get; set; }
        public string BillerNameOrCode { get; set; }
    }
}
