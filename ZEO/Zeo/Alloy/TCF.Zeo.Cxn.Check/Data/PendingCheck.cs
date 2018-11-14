using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Check.Data
{
    public class PendingCheck
    {
        public long TransactionId { get; set; }
        public long CustomerSessionId { set; get; }
        public string CheckUserName { set; get; }
        public string CheckPassword { set; get; }
        public string DiscountName { get; set; }
        public bool IsSystemApplied { get; set; }
        public int ChannelPartnerId { set; get; }
        public string LocationTimeZone { set; get; }
        public long CxnTrasactionId { set; get; }
        public string ChannelPartnerName { set; get; }

        public long CustomerId { get; set; }
        public string LocationIdentifier { set; get; }



    }
}
