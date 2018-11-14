using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7450OperationResponse
    {
        [JsonProperty(PropertyName = "CIF7450r_request_data")]
        public CIF7450rRequestData CIF7450rrequestdata { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_return_codes")]
        public CIF7450rReturnCodes CIF7450rreturncodes { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_return_svc_msgs")]
        public CIF7450rReturnsvcmsgs CIF7450rreturnsvcmsgs { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_return_data")]
        public CIF7450rReturndata CIF7450rreturndata { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_return_messages")]
        public CIF7450rReturnmessages CIF7450rreturnmessages { get; set; }
    }
}
