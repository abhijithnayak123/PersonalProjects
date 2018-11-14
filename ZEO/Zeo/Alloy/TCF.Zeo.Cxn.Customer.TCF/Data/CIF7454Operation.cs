using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7454Operation
    {
        [JsonProperty(PropertyName = "cif7454i_request_data")]
        public CIF7454iRequestData CIF7454irequestdata { get; set; }
        [JsonProperty(PropertyName = "cif7454i_filler")]
        public string CIF7454iFiller { get; set; } // length = 8767
    }
}
