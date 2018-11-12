using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public class Recruiter : BaseModel
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string MobileNo { get; set; }

        public string ContactNo { get; set; }
    }
}
