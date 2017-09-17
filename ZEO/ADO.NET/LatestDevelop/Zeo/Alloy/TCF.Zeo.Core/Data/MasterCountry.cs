using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class MasterCountry : ZeoModel
    {
        public string Name { get; set; }
        public string Abbr2 { get; set; }
        public string Abbr3 { get; set; }
    }
}
