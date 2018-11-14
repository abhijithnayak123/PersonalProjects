using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Common.Data
{
    public abstract class ZeoModel
    {
        public long Id { get; set; }
        public DateTime DTTerminalCreate { get; set; }
        public Nullable<DateTime> DTTerminalLastModified { get; set; }
        public DateTime DTServerCreate { get; set; }
        public Nullable<DateTime> DTServerLastModified { get; set; }
    }
}
