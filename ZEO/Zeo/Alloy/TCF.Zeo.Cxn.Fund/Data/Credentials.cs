using TCF.Zeo.Cxn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public class Credential : BaseRequest
    {
        public virtual string ServiceUrl { get; set; }
        public virtual string CertificateName { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual long ClientNodeId { get; set; }
        public virtual long CardProgramNodeId { get; set; }
        public virtual long SubClientNodeId { get; set; }
        public virtual string StockId { get; set; }
        public virtual long VisaLocationNodeId { get; set; }
    }
}
