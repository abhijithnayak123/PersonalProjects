using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7450rReturndata
    {
        [JsonProperty(PropertyName = "CIF7450r_rtrn_new_cust_no")]
        public long CIF7450rrtrnnewcustno { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rtrn_acct_1")]
        public string CIF7450rrtrnacct1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rtrn_acct_2")]
        public string CIF7450rrtrnacct2 { get; set; } 
    }
}
