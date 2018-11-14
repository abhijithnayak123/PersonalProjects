using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class TellerMainFrameResponse
    {
        [JsonProperty(PropertyName = "TEL7770OperationResponse")]
        public TEL7770OperationResponse TEL7770OperationResponse { get; set; }
    }
}
