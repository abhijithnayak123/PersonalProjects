using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public class CandidateDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string JobDescription { get; set; }
        public DateTime InterviewDate { get; set; }
        public string InterviewTime { get; set; }
        public string Token { get; set; }
        public bool IsNewCandidate { get; set; }
        public CandidateStatusEnum CandidateStatus { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PositionName { get; set; }
        public string PracticeName { get; set; }
        public string InterviewStageName { get; set; }
        public string InterviewTypeName { get; set; }
        public int NumberCandidate { get; set; }
    }
}
