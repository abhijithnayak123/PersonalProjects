using TCF.Zeo.Cxn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    public class TCISAccount : BaseRequest
    {
        public string PartnerAccountNumber { get; set; }
        public string RelationshipAccountNumber { get; set; }
        public bool TcfCustInd { get; set; }
        public ProfileStatus ProfileStatus { get; set; }
        public long CustomerID { get; set; }
        public int CustomerRevisionNo { get; set; }
    }
}
