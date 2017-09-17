using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class ChannelPartnerRemittanceTransaction
    {
        public Guid Id { get; set; }
        public int ChannelPartnerId { get; set; }
        public DateTime DTPayment { get; set; }
        public decimal PaymentAggregate { get; set; }
        public DateTime DTServerCreate { get; set; }
        public DateTime DTServerLastModified { get; set; }
    }
}
