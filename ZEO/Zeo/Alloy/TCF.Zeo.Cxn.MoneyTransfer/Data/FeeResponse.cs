using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class FeeResponse
    {
        public List<FeeInformation> FeeInformations { get; set; }
        public long TransactionId { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
        public long WuTrxId { get; set; }
    }
}
