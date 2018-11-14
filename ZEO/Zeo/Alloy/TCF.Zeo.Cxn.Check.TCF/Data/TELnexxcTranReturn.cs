using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Check.TCF.Data
{
    public class TELNexxcTranReturn
    {
        public string telnexxc_trace { get; set; }
        public string telnexxc_reject { get; set; }
        public string telnexxc_sysdwn { get; set; }
        public string telnexxc_nsf { get; set; }
        public string telnexxc_stops { get; set; }
        public string telnexxc_caution { get; set; }
        public string telnexxc_nopost { get; set; }
        public string telnexxc_nodebits { get; set; }
        public string telnexxc_noacct { get; set; }
        public string telnexxc_closed { get; set; }
        public string telnexxc_dormant { get; set; }
        public decimal telnexxc_availbal { get; set; }
        public decimal telnexxc_curbal { get; set; }
        public string telnexxc_message { get; set; }
    }
}
