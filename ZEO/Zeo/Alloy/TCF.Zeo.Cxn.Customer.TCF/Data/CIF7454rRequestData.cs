using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7454rRequestData
    {
        [JsonProperty(PropertyName = "cif7454r_req_type")]
        public string CIF7454rreqtype { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_oper_id")]
        public string CIF7454rreqoperid { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_cust_inst")]
        public int CIF7454rreqcustinst { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_cust_no")]
        public long CIF7454rreqcustno { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_verify_tin")]
        public int CIF7454rreqverifytin { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_verify_dob")]
        public int CIF7454rreqverifydob { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_ofac_scan")]
        public string CIF7454rreqofacscan { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_risk_source")]
        public string CIF7454rreqrisksource { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_risk_template")]
        public string CIF7454rreqrisktemplate { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_risk_score_cust")]
        public int CIF7454rreqriskscorecust { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_risk_score_zeosvc")]
        public int CIF7454rreqriskscorezeosvc { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_legal_cd")]
        public string CIF7454rreqlegalcd { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_ctzn_ctry")]
        public string CIF7454rreqctznctry { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_ctzn_ctry_2")]
        public string CIF7454rreqctznctry2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_id_type")]
        public string CIF7454rreqidtype { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_id_issuer_st")]
        public string CIF7454rreqidissuerst { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_id_issuer_ctry")]
        public string CIF7454rreqidissuerctry { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_id_number")]
        public string CIF7454rreqidnumber { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_id_issue_dt")]
        public int CIF7454rreqidissuedt { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_id_expire_dt")]
        public int CIF7454rreqidexpiredt { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_acct_inst_1")]
        public int CIF7454rreqacctinst1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_acct_brch_1")]
        public int CIF7454rreqacctbrch1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_rel_1")]
        public int CIF7454rreqrel1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_risk_score_acct_1")]
        public int CIF7454rreqriskscoreacct1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_acct_inst_2")]
        public int CIF7454rreqacctinst2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_acct_brch_2")]
        public int CIF7454rreqacctbrch2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_rel_2")]
        public int CIF7454rreqrel2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_risk_score_acct_2")]
        public int CIF7454rreqriskscoreacct2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_prodcode_2")]
        public string CIF7454rreqprodcode2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_acct_2")]
        public string CIF7454rreqacct2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_cip_ov_opt")]
        public string CIF7454rreqcipovopt { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_prodcode_1")]
        public string CIF7454rreqprodcode1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_appl_cd_1")]
        public string CIF7454rreqapplcd1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_acct_1")]
        public string CIF7454rreqacct1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_req_appl_cd_2")]
        public string CIF7454rreqapplcd2 { get; set; }
    }
}
