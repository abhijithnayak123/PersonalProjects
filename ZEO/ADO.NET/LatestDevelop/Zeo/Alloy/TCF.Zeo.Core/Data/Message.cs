using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Data
{
    public class Message
    {
        public string MessageKey { get; set; }
        public Helper.Language Language { get; set; }
        public string Content { get; set; }
        public string AddlDetails { get; set; }
        public string Processor { get; set; }
        public long Partner { get; set; }
        public int ErrorType { get; set; }
    }
}
