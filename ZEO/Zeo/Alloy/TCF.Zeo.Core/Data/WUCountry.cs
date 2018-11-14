using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class WUCountry
    {
        public List<Country> UnblockedCountries { get; set; }
        public List<Country> BlockedCountries { get; set; }
    }
}
