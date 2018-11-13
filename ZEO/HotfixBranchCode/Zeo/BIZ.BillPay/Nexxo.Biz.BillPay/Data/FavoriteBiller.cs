using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.BillPay.Data
{
    public class FavoriteBiller
    {
        public string BillerName { get; set; }
        public string AccountNumber { get; set; }
        public int ChannelPartnerId { get; set; }
        public int ProviderId { get; set; }
        public string BillerId { get; set; }
        public string ProviderName { get; set; }
        public string TenantId { get; set; }
        public string BillerCode { get; set; }
    }
}
