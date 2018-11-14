using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7450rRequestData
    {
        [JsonProperty(PropertyName = "CIF7450r_req_type")]
        public string CIF7450rreqtype { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_oper_id")]
        public string CIF7450rreqoperid { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_cust_inst")]
        public int CIF7450rreqcustinst { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_cust_no")]
        public long CIF7450rreqcustno { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_verify_tin")]
        public int CIF7450rreqverifytin { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_verify_dob")]
        public int CIF7450rreqverifydob { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_ofac_scan")]
        public string CIF7450rreqofacscan { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_risk_source")]
        public string CIF7450rreqrisksource { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_risk_template")]
        public string CIF7450rreqrisktemplate { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_risk_score_cust")]
        public int CIF7450rreqriskscorecust { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_risk_score_zeosvc")]
        public int CIF7450rreqriskscorezeosvc { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_legal_cd")]
        public string CIF7450rreqlegalcd { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_ctzn_ctry")]
        public string CIF7450rreqctznctry { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_ctzn_ctry_2")]
        public string CIF7450rreqctznctry2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_id_type")]
        public string CIF7450rreqidtype { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_id_issuer_st")]
        public string CIF7450rreqidissuerst { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_id_issuer_ctry")]
        public string CIF7450rreqidissuerctry { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_id_number")]
        public string CIF7450rreqidnumber { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_id_issue_dt")]
        public int CIF7450rreqidissuedt { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_id_expire_dt")]
        public int CIF7450rreqidexpiredt { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_acct_inst_1")]
        public int CIF7450rreqacctinst1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_acct_brch_1")]
        public int CIF7450rreqacctbrch1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_rel_1")]
        public int CIF7450rreqrel1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_risk_score_acct_1")]
        public int CIF7450rreqriskscoreacct1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_acct_inst_2")]
        public int CIF7450rreqacctinst2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_acct_brch_2")]
        public int CIF7450rreqacctbrch2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_rel_2")]
        public int CIF7450rreqrel2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_risk_score_acct_2")]
        public int CIF7450rreqriskscoreacct2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_prodcode_2")]
        public string CIF7450rreqprodcode2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_acct_2")]
        public string CIF7450rreqacct2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_cip_ov_opt")]
        public string CIF7450rreqcipovopt { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_prodcode_1")]
        public string CIF7450rreqprodcode1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_appl_cd_1")]
        public string CIF7450rreqapplcd1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_acct_1")]
        public string CIF7450rreqacct1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_req_appl_cd_2")]
        public string CIF7450rreqapplcd2 { get; set; }
    }
}
