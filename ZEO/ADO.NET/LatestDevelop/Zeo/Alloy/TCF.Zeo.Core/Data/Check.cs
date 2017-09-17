using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class Check : ExtendedFeeTransaction
    {
        public long CustomerSessionId { set; get; }
        public long ProviderAccountId { set; get; }
        public int ProviderId { set; get; }
        public string DmsDeclineMessage { get; set; }
        public string CheckType { get; set; }
        public string MICR { get; set; }
        public string FrankData { get; set; }
        public bool IsPendingCheckApprovedOrDeclined { get; set; } = false;
        public long CxnTransactionId { get; set;}

    }
}
