using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7450rReturnCodes
    {
        [JsonProperty(PropertyName = "CIF7450r_rtrn_cd")]
        public string CIF7450rrtrncd { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_cust_inst")]
        public string CIF7450rrccustinst { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_cust_no")]
        public string CIF7450rrccustno { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_verify_tin")]
        public string CIF7450rrcverifytin { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_verify_dob")]
        public string CIF7450rrcverifydob { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_ofac_scan")]
        public string CIF7450rrcofacscan { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_risk_source")]
        public string CIF7450rrcrisksource { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_risk_template")]
        public string CIF7450rrcrisktemplate { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_risk_score_cust")]
        public string CIF7450rrcriskscorecust { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_legal_cd")]
        public string CIF7450rrclegalcd { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_ctzn_ctry")]
        public string CIF7450rrcctznctry { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_ctzn_ctry_2")]
        public string CIF7450rrcctznctry2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_id_type")]
        public string CIF7450rrcidtype { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_id_issuer_st")]
        public string CIF7450rrcidissuerst { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_id_issuer_ctry")]
        public string CIF7450rrcidissuerctry { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_id_number")]
        public string CIF7450rrcidnumber { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_id_issue_dt")]
        public string CIF7450rrcidissuedt { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_id_expire_dt")]
        public string CIF7450rrcidexpiredt { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_cip_ov_opt")]
        public string CIF7450rrccipovopt { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_acct_inst_1")]
        public string CIF7450rrcacctinst1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_acct_brch_1")]
        public string CIF7450rrcacctbrch1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_prodcode_1")]
        public string CIF7450rrcprodcode1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_appl_cd_1")]
        public string CIF7450rrcapplcd1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_acct_1")]
        public string CIF7450rrcacct1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_rel_1")]
        public string CIF7450rrcrel1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_risk_score_acct_1")]
        public string CIF7450rrcriskscoreacct1 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_acct_inst_2")]
        public string CIF7450rrcacctinst2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_acct_brch_2")]
        public string CIF7450rrcacctbrch2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_prodcode_2")]
        public string CIF7450rrcprodcode2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_appl_cd_2")]
        public string CIF7450rrcapplcd2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_acct_2")]
        public string CIF7450rrcacct2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_rel_2")]
        public string CIF7450rrcrel2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_risk_score_acct_2")]
        public string CIF7450rrcriskscoreacct2 { get; set; }
        [JsonProperty(PropertyName = "CIF7450r_rc_rtrn_new_cust_no")]
        public string CIF7450rrcrtrnnewcustno { get; set; }
    }
}
