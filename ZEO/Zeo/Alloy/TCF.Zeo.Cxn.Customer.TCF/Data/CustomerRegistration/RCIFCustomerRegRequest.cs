using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class RCIFCustomerRegRequest
    {
        [JsonProperty(PropertyName = "CIF7450Operation")]
        public CIF7450Operation CIF7450Operation { get; set; }
    }
}
