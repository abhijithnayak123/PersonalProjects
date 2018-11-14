using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class CustomerSession : ZeoModel
    {
        public long CustomerId { get; set; }
        public bool CardPresent { get; set; }
        public string TimezoneID { get; set; }
        public bool IsGPRCustomer { get; set; }
        public string CardNumber { get; set; }
        public Helper.ProfileStatus ProfileStatus { get; set; }
        public Helper.CardSearchType CardSearchType { get; set; }
        public long CartId { get; set; }
    }
}
