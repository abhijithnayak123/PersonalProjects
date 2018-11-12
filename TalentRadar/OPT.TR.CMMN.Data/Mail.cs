using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public class Mail
    {
        public string Id { get; set; }
        public string FromEmailId { get; set; }
        public string ToEmailId { get; set; }
        public string CandidateStage { get; set; }
        public string DeclineReason { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string  CandidateName { get; set; }
        public string RecruiterPhoneNumber { get; set; }
        public string RecruiterName { get; set; }
    }
}
