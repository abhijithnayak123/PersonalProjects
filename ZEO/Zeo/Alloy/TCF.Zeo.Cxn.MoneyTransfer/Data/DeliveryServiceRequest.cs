using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class DeliveryServiceRequest
    {
        public DeliveryServiceType Type { get; set; }
        public string CountryCode { get; set; }
        public string CountryCurrency { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
    }
}
