using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class CIF7450iRequestData
    {
        [JsonProperty(PropertyName = "cif7450i_req_type")]
        public string CIF7450ireqtype { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_oper_id")]
        public string CIF7450ireqoperid { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_cust_inst")]
        public int CIF7450ireqcustinst { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_cust_brch")]
        public int CIF7450ireqcustbrch { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_cust_no")]
        public long CIF7450ireqcustno { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_acct_inst")]
        public int CIF7450ireqacctinst { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_prodcode")]
        public string CIF7450ireqprodcode { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_appl_cd")]
        public string CIF7450ireqapplcd { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_acct")]
        public string CIF7450ireqacct { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_rel")]
        public int CIF7450ireqrel { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_source")]
        public string CIF7450ireqsource { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_name")]
        public string CIF7450ireqname { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_tin")]
        public int CIF7450ireqtin { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_dob")]
        public int CIF7450ireqdob { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_sex")]
        public string CIF7450ireqsex { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_legal_cd")]
        public string CIF7450ireqlegalcd { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_ctzn_ctry")]
        public string CIF7450ireqctznctry { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_ctzn_ctry_2")]
        public string CIF7450ireqctznctry2 { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_occup_cd")]
        public string CIF7450ireqoccupcd { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_occup_desc")]
        public string CIF7450ireqoccupdesc { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_curr_empl")]
        public string CIF7450ireqcurrempl { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_maiden_name")]
        public string CIF7450ireqmaidenname { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_addr_line_1")]
        public string CIF7450ireqaddrline1 { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_addr_line_2")]
        public string CIF7450ireqaddrline2 { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_addr_line_3")]
        public string CIF7450ireqaddrline3 { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_city")]
        public string CIF7450ireqcity { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_state")]
        public string CIF7450ireqstate { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_zip5")]
        public int CIF7450ireqzip5 { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_phone1_type")]
        public string CIF7450ireqphone1type { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_phone1")]
        public long CIF7450ireqphone1 { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_phone2_type")]
        public string CIF7450ireqphone2type { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_phone2")]
        public long CIF7450ireqphone2 { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_email_addr")]
        public string CIF7450ireqemailaddr { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_id_type")]
        public string CIF7450ireqidtype { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_id_issuer_st")]
        public string CIF7450ireqidissuerst { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_id_issuer_ctry")]
        public string CIF7450ireqidissuerctry { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_id_number")]
        public string CIF7450ireqidnumber { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_id_issue_dt")]
        public int CIF7450ireqidissuedt { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_id_expire_dt")]
        public int CIF7450ireqidexpiredt { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_ofac_scan")]
        public string CIF7450ireqofacscan { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_risk_source")]
        public string CIF7450ireqrisksource { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_risk_template")]
        public string CIF7450ireqrisktemplate { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_risk_score_cust")]
        public int CIF7450ireqriskscorecust { get; set; }
        [JsonProperty(PropertyName = "cif7450i_req_risk_score_acct")]
        public int CIF7450ireqriskscoreacct { get; set; }
        //[JsonProperty(PropertyName = "cif7450i_filler")]
        //public string CIF7450ireqfiller { get; set; }
    }
}
