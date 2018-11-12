using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public class Candidate : BaseModel
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Mobile { get; set; }

        public Recruiter Recruiter { get; set; }

        public List<Document> Documents { get; set; }

        public Position Position { get; set; }

        public Practice Practice { get; set; }

        public InterviewStage Stage { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }

        //public ObjectId Token { get; set; }

        public List<Notification> Notifications { get; set; }

        public string JobDescription { get; set; }

        public string Comment { get; set; }

        public DateTime InterviewDate { get; set; }

        public string InterviewTime { get; set; }

        public InterviewType InterviewType { get; set; }

        public string DeclineReason { get; set; }

        public string Token { get; set; }

        public string OnBoardDescription { get; set; }

        public bool IsNewCandidate { get; set; }

        public CandidateStatusEnum CandidateStatus { get; set; }

        public string FeedBackComment { get; set; }

    }
}
