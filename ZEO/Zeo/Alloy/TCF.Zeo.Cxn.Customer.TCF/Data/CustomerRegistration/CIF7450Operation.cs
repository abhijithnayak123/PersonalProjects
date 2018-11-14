using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7450Operation
    {
        [JsonProperty(PropertyName = "cif7450i_request_data")]
        public CIF7450iRequestData CIF7450irequestdata { get; set; }
        [JsonProperty(PropertyName = "cif7450i_filler")]
        public string CIF7450iFiller { get; set; } // length = 8767
    }
}
