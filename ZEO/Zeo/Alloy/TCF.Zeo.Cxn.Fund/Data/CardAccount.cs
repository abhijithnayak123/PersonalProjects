using TCF.Zeo.Cxn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class CardAccount : BaseRequest
    {
        public string ProxyId { get; set; }
        public string PseudoDDA { get; set; }
        public string CardNumber { get; set; }
        public string CardAliasId { get; set; }
        public long SubClientNodeId { get; set; }
        public bool IsCardActive { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public string FullCardNumber { get; set; }
        public DateTime? DTAccountClosed { get; set; }
        public string PrimaryCardAliasId { get; set; }
        public long ActivatedLocationNodeId { get; set; }
        public string ExpirationDate { get; set; }
        public bool IsFraud { get; set; }
    }
}
