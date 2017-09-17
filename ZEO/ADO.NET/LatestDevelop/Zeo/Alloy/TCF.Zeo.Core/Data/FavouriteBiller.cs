using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class FavouriteBiller: ZeoModel
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
