using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class RCIFMainFrameRequest
    {
        [JsonProperty(PropertyName = "CIF7454Operation")]
        public CIF7454Operation CIF7454Operation { get; set; }
    }
}
