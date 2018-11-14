using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7454iRequestData
    {
        [JsonProperty(PropertyName = "cif7454i_req_type")]
        public string CIF7454ireqtype { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_oper_id")]
        public string CIF7454ireqoperid { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_cust_inst")]
        public int CIF7454ireqcustinst { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_cust_no")]
        public long CIF7454ireqcustno { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_verify_tin")]
        public int CIF7454ireqverifytin { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_verify_dob")]
        public int CIF7454ireqverifydob { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_ofac_scan")]
        public string CIF7454ireqofacscan { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_risk_source")]
        public string CIF7454ireqrisksource { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_risk_template")]
        public string CIF7454ireqrisktemplate { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_risk_score_cust")]
        public string CIF7454ireqriskscorecust { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_risk_score_zeosvc")]
        public string CIF7454ireqriskscorezeosvc { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_legal_cd")]
        public string CIF7454ireqlegalcd { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_ctzn_ctry")]
        public string CIF7454ireqctznctry { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_ctzn_ctry_2")]
        public string CIF7454ireqctznctry2 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_id_type")]
        public string CIF7454ireqidtype { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_id_issuer_st")]
        public string CIF7454ireqidissuerst { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_id_issuer_ctry")]
        public string CIF7454ireqidissuerctry { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_id_number")]
        public string CIF7454ireqidnumber { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_id_issue_dt")]
        public int CIF7454ireqidissuedt { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_id_expire_dt")]
        public int CIF7454ireqidexpiredt { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_cip_ov_opt")]
        public string CIF7454ireqcipovopt { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_acct_inst_1")]
        public string CIF7454ireqacctinst1 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_acct_brch_1")]
        public string CIF7454ireqacctbrch1 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_prodcode_1")]
        public string CIF7454ireqprodcode1 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_appl_cd_1")]
        public string CIF7454ireqapplcd1 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_acct_1")]
        public string CIF7454ireqacct1 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_rel_1")]
        public string CIF7454ireqrel1 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_risk_score_acct_1")]
        public string CIF7454ireqriskscoreacct1 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_acct_inst_2")]
        public string CIF7454ireqacctinst2 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_acct_brch_2")]
        public string CIF7454ireqacctbrch2 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_rel_2")]
        public string CIF7454ireqrel2 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_risk_score_acct_2")]
        public string CIF7454ireqriskscoreacct2 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_prodcode_2")]
        public string CIF7454ireqprodcode2 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_appl_cd_2")]
        public string CIF7454ireqapplcd2 { get; set; }
        [JsonProperty(PropertyName = "cif7454i_req_acct_2")]
        public string CIF7454ireqacct2 { get; set; }
    }
}
