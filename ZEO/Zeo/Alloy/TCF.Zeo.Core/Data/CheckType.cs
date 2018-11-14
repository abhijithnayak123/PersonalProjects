using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data
{
	public class CheckType
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public ProviderId ProductProviderCode { get; set; }
    }
}
