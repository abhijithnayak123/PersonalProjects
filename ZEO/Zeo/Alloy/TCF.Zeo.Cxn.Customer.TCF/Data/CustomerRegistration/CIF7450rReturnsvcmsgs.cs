using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7450rReturnsvcmsgs
    {
        [JsonProperty(PropertyName = "CIF7450r_rtrn_msg_ind_1")]
        public string CIF7450rrtrnmsgind1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rtrn_msg_text_1")]
        public string CIF7450rrtrnmsgtext1 { get; set; } 
    }
}
