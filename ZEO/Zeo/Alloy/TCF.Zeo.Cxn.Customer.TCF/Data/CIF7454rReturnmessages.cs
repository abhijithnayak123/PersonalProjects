using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7454rReturnmessages
    {
        [JsonProperty(PropertyName = "cif7454r_rtrn_msg_cnt")]
        public int CIF7454rrtrnmsgcnt { get; set; } // min = 0 max = 99999
        [JsonProperty(PropertyName = "items")]
        public Item[] items { get; set; }
    }
}
