using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class Occupation : ZeoModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
