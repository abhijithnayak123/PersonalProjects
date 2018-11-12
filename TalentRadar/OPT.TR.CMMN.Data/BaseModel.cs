using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public class BaseModel
    {
        public long Id { get; set; }

        public DateTime DTCreate { get; set; }

        public DateTime DTUpdate { get; set; }
    }
}
