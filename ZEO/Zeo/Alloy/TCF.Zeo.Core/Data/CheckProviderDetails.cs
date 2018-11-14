using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Data
{
    public class CheckProviderDetails
    {
        public int CheckTypeId { get; set; }

        public Helper.ProviderId ProductProviderCode { get; set; }
    }
}
