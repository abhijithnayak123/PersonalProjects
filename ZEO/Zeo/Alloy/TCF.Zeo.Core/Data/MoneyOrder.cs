using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Data
{
    public class MoneyOrder : ExtendedFeeTransaction
    {
        public long CustomerSessionId { get; set; }
        public string MICR { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string CheckNumber { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
    }
}
