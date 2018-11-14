using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7454OperationResponse
    {
        [JsonProperty(PropertyName = "cif7454r_request_data")]
        public CIF7454rRequestData CIF7454rrequestdata { get; set; }
        [JsonProperty(PropertyName = "cif7454r_return_codes")]
        public CIF7454rReturnCodes CIF7454rreturncodes { get; set; }
        [JsonProperty(PropertyName = "cif7454r_return_svc_msgs")]
        public CIF7454rReturnsvcmsgs CIF7454rreturnsvcmsgs { get; set; }
        [JsonProperty(PropertyName = "cif7454r_return_data")]
        public CIF7454rReturndata CIF7454rreturndata { get; set; }
        [JsonProperty(PropertyName = "cif7454r_return_messages")]
        public CIF7454rReturnmessages CIF7454rreturnmessages { get; set; }
    }
}
