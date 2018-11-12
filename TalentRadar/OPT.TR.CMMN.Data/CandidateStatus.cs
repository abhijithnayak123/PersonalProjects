using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public class CandidateStatus : BaseModel
    {
        public string DeclineReason { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
        public bool IsNewCandidate { get; set; }
    }
}
