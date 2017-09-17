using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class ChannelPartnerPreFunding
    {
        public Guid Id { get; set; }
        public int ChannelPartnerId { get; set; }
        public decimal Amount { get; set; }
        public float Threshold { get; set; }
        public DateTime DTTerminalCreate { get; set; }
        public DateTime DTTerminalLastModified { get; set; }
        public virtual Nullable<DateTime> DTServerCreate { get; set; }
        public virtual Nullable<DateTime> DTServerLastModified { get; set; }
    }
}
