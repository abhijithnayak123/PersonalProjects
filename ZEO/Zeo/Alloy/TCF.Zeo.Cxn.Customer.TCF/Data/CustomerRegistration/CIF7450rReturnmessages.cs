using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7450rReturnmessages
    {
        [JsonProperty(PropertyName = "CIF7450r_rtrn_msg_cnt")]
        public int CIF7450rrtrnmsgcnt { get; set; } // min = 0 max = 99999
        [JsonProperty(PropertyName = "items")]
        public Item[] items { get; set; }
    }
}
