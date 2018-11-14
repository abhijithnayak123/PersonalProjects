using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class TEL7770OperationResponse
    {
        [JsonProperty(PropertyName = "telnexxc_tran_return")]
        public TELNexxcTranReturn TelnexxTranReturn { get; set; }
    }
}
