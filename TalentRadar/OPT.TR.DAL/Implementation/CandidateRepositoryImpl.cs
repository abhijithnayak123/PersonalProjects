using OPT.TalentRadar.DAL.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using OPT.TalentRadar.Common.Data;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace OPT.TalentRadar.DAL.Implementation
{
    public class CandidateRepositoryImpl : ICandidateRepository
    {
        IRepository repo;
        public CandidateRepositoryImpl()
        {
            repo = new RepositoryImpl(TRConstants.Candidate);
        }

        public string AddCandidate(Candidate candidate)
        {
            ObjectId accessToken = ObjectId.GenerateNewId();
            long id = getNextId();
            candidate.Id = id;
            candidate.IsActive = false;
            //candidate.Token = accessToken;
            candidate.DTCreate = DateTime.UtcNow;
            candidate.Token = accessToken.ToString();
            repo.Save(candidate);
            return accessToken.ToString();
        }

        public Candidate GetCandidate(long candidateId)
        {
            return repo.Fetch<Candidate>(candidateId);
        }
        public Candidate GetCandidate(string filter)
        {
            return filterCandidate(filter);
        }

        public List<Candidate> GetCandidates()
        {
            return repo.FetchAll<Candidate>().ToList<Candidate>().ToList();
        }

        //public List<Candidate> GetCandidateBasedOnEmailAndMobile(Candidate candidate)
        //{
        //    IDictionary<string, string> filters = new Dictionary<string, string>()
        //    {
        //        { "Email", candidate.Email },
        //        { "Mobile", candidate.Mobile }
        //    };

        //    return repo.DocumentsMatchEqFieldValue<Candidate>(filters);
        //}

        public bool UpdateCandidate(Candidate candidate)
        {
            var filter = Builders<Candidate>.Filter.Eq("_id", candidate.Id);
            repo.UpdateDocument<Candidate>(filter, candidate);
            return true;
        }

        public CandidateDetails GetCandidateDetails(CandidateLogin login)
        {
            IDictionary<string, string> filters = new Dictionary<string, string>()
            {
                { "Email", login.Email },
                { "Mobile", login.PhoneNumber }
            };

            Candidate candidate = repo.DocumentsMatchEqFieldValue<Candidate>(filters);

            return new CandidateDetails()
            {
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                Email = candidate.Email,
                Mobile = candidate.Mobile,
                IsActive = candidate.IsActive,
                IsNewCandidate = candidate.IsNewCandidate,
                InterviewDate = candidate.InterviewDate,
                InterviewTime = candidate.InterviewTime,
                Token = candidate.Token,
                JobDescription = candidate.JobDescription,
                CandidateStatus = candidate.CandidateStatus,
                PositionName = candidate.Position.Name
            };
        }

        public List<CandidateDetails> GetListCandidate(CandidateDetails candidateDetail, int filter, int  numberOfRecord)
        {
            List<CandidateDetails> candidatesDetails = new List<CandidateDetails>();

            CandidateDetails candidate = null;

            List<Candidate> candidates = repo.DocumentsMatchFieldValue(candidateDetail).OrderByDescending(x => x.Id).ToList();

            int numberOfCandidate = candidates.Count;

           List<Candidate> topTenCandidate = candidates.GetRange(filter, numberOfCandidate - filter > numberOfRecord ? numberOfRecord : numberOfCandidate - filter);

            foreach (var item in topTenCandidate)
            {
                candidate = new CandidateDetails()
                {
                    Token = item.Token,
                    PracticeName = item.Practice.Name,
                    InterviewTypeName = item.InterviewType?.Name,
                    CandidateStatus = item.CandidateStatus,
                    Email = item.Email,
                    Mobile = item.Mobile,
                    PositionName = item.Position.Name,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    InterviewStageName = item.Stage.Name,
                    NumberCandidate = numberOfCandidate,
                };
                candidatesDetails.Add(candidate);
            }


            return candidatesDetails;

        }

        private long getNextId()
        {
            Candidate profie = repo.IdGenerater<Candidate>();
            long id = 1;
            if (profie != null)
            {
                id = profie.Id + 1;
            }
            return id;
        }
        private Candidate filterCandidate(string filter)
        {
            Candidate candidate = null;
            ObjectId objId = new ObjectId();
            if (isValidEmail(filter))
            {
                candidate = repo.Fetch<Candidate>("Email", filter);
            }
            else if (ObjectId.TryParse(filter, out objId))
            {
                candidate = repo.Fetch<Candidate>("Token", filter);
            }
            else
            {
                candidate = repo.Fetch<Candidate>("Mobile", filter);
            }

            if (candidate != null && candidate.Notifications != null)
            {
                candidate.Notifications = candidate.Notifications.OrderByDescending(x => x.DTCreate).ToList();
            }

            return candidate;
        }

        private bool isValidEmail(string input)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(input);
                return addr.Address == input;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateCandidateStatus(CandidateStatus status)
        {
            try
            {
                Candidate candidate = filterCandidate(status.Token);
                var filter = Builders<Candidate>.Filter.Eq("Token", status.Token);
                if (string.IsNullOrWhiteSpace(status.DeclineReason) && status.IsActive)
                {
                    candidate.IsActive = status.IsActive;
                }
                else
                {
                    candidate.DeclineReason = status.DeclineReason;
                    candidate.IsActive = status.IsActive;
                }
                candidate.IsNewCandidate = status.IsNewCandidate;
                candidate.DTUpdate = DateTime.Now;
                repo.UpdateDocument<Candidate>(filter, candidate);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateFeedBack(FeedBack feedBack)
        {
            try
            {
                Candidate candidate = filterCandidate(feedBack.CandidateToken);
                candidate.FeedBackComment = feedBack.FeedBackComment;
                var filter = Builders<Candidate>.Filter.Eq("Token", feedBack.CandidateToken);
                repo.UpdateDocument<Candidate>(filter, candidate);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
