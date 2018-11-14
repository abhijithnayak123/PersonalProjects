using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class IdType
    {
        public virtual string Name { get; set; }
        public virtual string Mask { get; set; }
        public virtual bool HasExpirationDate { get; set; }
    }
}
