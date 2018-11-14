using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7454rReturnCodes
    {
        [JsonProperty(PropertyName = "cif7454r_rtrn_cd")]
        public string CIF7454rrtrncd { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_cust_inst")]
        public string CIF7454rrccustinst { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_cust_no")]
        public string CIF7454rrccustno { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_verify_tin")]
        public string CIF7454rrcverifytin { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_verify_dob")]
        public string CIF7454rrcverifydob { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_ofac_scan")]
        public string CIF7454rrcofacscan { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_risk_source")]
        public string CIF7454rrcrisksource { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_risk_template")]
        public string CIF7454rrcrisktemplate { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_risk_score_cust")]
        public string CIF7454rrcriskscorecust { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_legal_cd")]
        public string CIF7454rrclegalcd { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_ctzn_ctry")]
        public string CIF7454rrcctznctry { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_ctzn_ctry_2")]
        public string CIF7454rrcctznctry2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_id_type")]
        public string CIF7454rrcidtype { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_id_issuer_st")]
        public string CIF7454rrcidissuerst { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_id_issuer_ctry")]
        public string CIF7454rrcidissuerctry { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_id_number")]
        public string CIF7454rrcidnumber { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_id_issue_dt")]
        public string CIF7454rrcidissuedt { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_id_expire_dt")]
        public string CIF7454rrcidexpiredt { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_cip_ov_opt")]
        public string CIF7454rrccipovopt { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_acct_inst_1")]
        public string CIF7454rrcacctinst1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_acct_brch_1")]
        public string CIF7454rrcacctbrch1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_prodcode_1")]
        public string CIF7454rrcprodcode1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_appl_cd_1")]
        public string CIF7454rrcapplcd1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_acct_1")]
        public string CIF7454rrcacct1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_rel_1")]
        public string CIF7454rrcrel1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_risk_score_acct_1")]
        public string CIF7454rrcriskscoreacct1 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_acct_inst_2")]
        public string CIF7454rrcacctinst2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_acct_brch_2")]
        public string CIF7454rrcacctbrch2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_prodcode_2")]
        public string CIF7454rrcprodcode2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_appl_cd_2")]
        public string CIF7454rrcapplcd2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_acct_2")]
        public string CIF7454rrcacct2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_rel_2")]
        public string CIF7454rrcrel2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_risk_score_acct_2")]
        public string CIF7454rrcriskscoreacct2 { get; set; }
        [JsonProperty(PropertyName = "cif7454r_rc_rtrn_new_cust_no")]
        public string CIF7454rrcrtrnnewcustno { get; set; }
    }
}
