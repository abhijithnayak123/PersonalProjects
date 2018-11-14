using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class TELNexxcTranReturn
    {
        [JsonProperty(PropertyName = "telnexxc_trace")]
        public string Telnexxctrace { get; set; }
        [JsonProperty(PropertyName = "telnexxc_reject")]
        public string Telnexxcreject { get; set; }
        [JsonProperty(PropertyName = "telnexxc_sysdwn")]
        public string Telnexxcsysdwn { get; set; }
        [JsonProperty(PropertyName = "telnexxc_nsf")]
        public string Telnexxcnsf { get; set; }
        [JsonProperty(PropertyName = "telnexxc_stops")]
        public string Telnexxcstops { get; set; }
        [JsonProperty(PropertyName = "telnexxc_caution")]
        public string Telnexxccaution { get; set; }
        [JsonProperty(PropertyName = "telnexxc_nopost")]
        public string Telnexxcnopost { get; set; }
        [JsonProperty(PropertyName = "telnexxc_nodebits")]
        public string Telnexxcnodebits { get; set; }
        [JsonProperty(PropertyName = "telnexxc_noacct")]
        public string Telnexxcnoacct { get; set; }
        [JsonProperty(PropertyName = "telnexxc_closed")]
        public string Telnexxcclosed { get; set; }
        [JsonProperty(PropertyName = "telnexxc_dormant")]
        public string Telnexxcdormant { get; set; }
        [JsonProperty(PropertyName = "telnexxc_availbal")]
        public decimal Telnexxcavailbal { get; set; }
        [JsonProperty(PropertyName = "telnexxc_curbal")]
        public decimal Telnexxccurbal { get; set; }
        [JsonProperty(PropertyName = "telnexxc_message")]
        public string Telnexxcmessage { get; set; }
    }
}
