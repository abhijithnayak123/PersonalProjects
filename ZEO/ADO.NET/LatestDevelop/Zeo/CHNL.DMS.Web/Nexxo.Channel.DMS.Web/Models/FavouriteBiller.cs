using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class FavouriteBiller
    {
        public string BillerName { get; set; }
        public string AccountNumber { get; set; }
        public int ChannelPartnerId { get; set; }
        public int ProviderId { get; set; }
        public string ProductId { get; set; }
        public string ProviderName { get; set; }
        public string TenantId { get; set; }
        public string BillerCode { get; set; }
    }
}