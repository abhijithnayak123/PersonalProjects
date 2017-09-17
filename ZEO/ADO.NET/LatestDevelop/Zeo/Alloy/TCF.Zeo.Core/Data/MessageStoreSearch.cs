using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Data
{
    public class MessageStoreSearch
    {
        public string ErrorCode { get; set; }
        public Helper.Language Language { get; set; }
        public string ProviderCode { get; set; }
        public string ProductCode { get; set; }
    }
}
