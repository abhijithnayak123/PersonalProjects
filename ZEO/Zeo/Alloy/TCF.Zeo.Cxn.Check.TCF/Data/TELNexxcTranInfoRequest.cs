using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Check.TCF.Data
{
    public class TELNexxcTranInfoRequest
    {
        public string telnexxc_teller_bk { get; set; }
        public string telnexxc_teller_br { get; set; }
        public string telnexxc_teller { get; set; }
        public string telnexxc_ampm { get; set; }
        public string telnexxc_drawer { get; set; }
        public string telnexxc_lu { get; set; }
        public string telnexxc_lawson { get; set; }
        public string telnexxc_nexxo_acct { get; set; }
        public string telnexxc_tran { get; set; }
        public string telnexxc_type { get; set; }
        public string telnexxc_acct { get; set; }
        public string telnexxc_check_no { get; set; }
        //public string telnexxc_card { get; set; }
        public decimal telnexxc_amt { get; set; }
        public decimal telnexxc_fee { get; set; }
        public string telnexxc_tcfcard { get; set; }
        public string telnexxc_appl { get; set; }
        public string telnexxc_zeo_acct { get; set; }
        public string telnexxc_initial { get; set; }
        public string telnexxc_routing_no { get; set; }
        public string telnexxc_mgi_tranid { get; set; }
    }
}
