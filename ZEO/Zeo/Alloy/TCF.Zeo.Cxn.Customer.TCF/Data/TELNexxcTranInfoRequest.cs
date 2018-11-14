using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class TELNexxcTranInfoRequest
    {
        [JsonProperty(PropertyName = "telnexxc_teller_bk")]
        public string Telnexxctellerbk { get; set; }
        [JsonProperty(PropertyName = "telnexxc_teller_br")]
        public string Telnexxctellerbr { get; set; }
        [JsonProperty(PropertyName = "telnexxc_teller")]
        public string Telnexxcteller { get; set; }
        [JsonProperty(PropertyName = "telnexxc_ampm")]
        public string Telnexxcampm { get; set; }
        [JsonProperty(PropertyName = "telnexxc_drawer")]
        public string Telnexxcdrawer { get; set; }
        [JsonProperty(PropertyName = "telnexxc_lu")]
        public string Telnexxclu { get; set; }
        [JsonProperty(PropertyName = "telnexxc_lawson")]
        public string Telnexxclawson { get; set; }
        [JsonProperty(PropertyName = "telnexxc_nexxo_acct")]
        public string Telnexxcnexxoacct { get; set; }
        [JsonProperty(PropertyName = "telnexxc_tran")]
        public string Telnexxctran { get; set; }
        [JsonProperty(PropertyName = "telnexxc_type")]
        public string Telnexxctype { get; set; }
        [JsonProperty(PropertyName = "telnexxc_acct")]
        public string Telnexxcacct { get; set; }
        [JsonProperty(PropertyName = "telnexxc_check_no")]
        public string Telnexxccheckno { get; set; }
        [JsonProperty(PropertyName = "telnexxc_amt")]
        public decimal Telnexxcamt { get; set; }
        [JsonProperty(PropertyName = "telnexxc_fee")]
        public decimal Telnexxcfee { get; set; }
        [JsonProperty(PropertyName = "telnexxc_tcfcard")]
        public string Telnexxctcfcard { get; set; }
        [JsonProperty(PropertyName = "telnexxc_appl")]
        public string Telnexxcappl { get; set; }
        [JsonProperty(PropertyName = "telnexxc_zeo_acct")]
        public string Telnexxczeoacct { get; set; }
        [JsonProperty(PropertyName = "telnexxc_initial")]
        public string Telnexxcinitial { get; set; }
        [JsonProperty(PropertyName = "telnexxc_routing_no")]
        public string Telnexxcroutingno { get; set; }
        [JsonProperty(PropertyName = "telnexxc_mgi_tranid")]
        public string Telnexxcmgitranid { get; set; }
    }
}
