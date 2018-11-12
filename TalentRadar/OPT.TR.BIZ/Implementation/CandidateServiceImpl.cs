using OPT.TalentRadar.BIZ.Contract;
using System.Collections.Generic;
using OPT.TalentRadar.Common.Data;
using OPT.TalentRadar.DAL.Contract;
using OPT.TalentRadar.DAL.Implementation;
using System;
using System.Linq;

namespace OPT.TalentRadar.BIZ.Implementation
{
    public class CandidateServiceImpl : ICandidateService
    {
        ICandidateRepository candidateRepo;
        IEmailService emailSvc;

        public CandidateServiceImpl()
        {
            candidateRepo = new CandidateRepositoryImpl();
        }

        public List<Candidate> GetAllCandidates()
        {
            return candidateRepo.GetCandidates();
        }

        public Candidate GetCandidate(string token)
        {
            Candidate candidate = candidateRepo.GetCandidate(token);
            candidate.Notifications = candidate.Notifications.OrderByDescending(x => x.DTCreate).ToList();
            return candidate;
        }

        public bool NewCandidate(Candidate candidate)
        {
            candidate = mapCandidate(candidate);
            string id = candidateRepo.AddCandidate(candidate);
            emailSvc = new EmailServiceImpl();
            emailSvc.CandidateMail(new Mail() { Id = id, CandidateStage = candidate.Stage.Name, FromEmailId = candidate.Recruiter.Email, ToEmailId = candidate.Email, CandidateName = candidate.FirstName, RecruiterName = candidate.Recruiter.FullName, RecruiterPhoneNumber = candidate.Recruiter.MobileNo });

            return true;
        }

        private Candidate mapCandidate(Candidate candidate)
        {
            IMasterDataRepositoy masterRepo = new MasterDataRepositoryImpl();

            if (candidate.Position != null)
            {
                candidate.Position = masterRepo.GetPostion(candidate.Position.Id);
            }
            if (candidate.Practice != null)
            {
                candidate.Practice = masterRepo.GetPractice(candidate.Practice.Id);
            }

            if (candidate.Stage != null)
            {
                candidate.Stage = masterRepo.GetInterviewStage(candidate.Stage.Id);
                candidate.CandidateStatus = (CandidateStatusEnum)int.Parse(candidate.Stage.Id.ToString());
            }
            if (candidate.InterviewType != null)
            {
                candidate.InterviewType = masterRepo.GetInterviewType(candidate.InterviewType.Id);
            }
            if (candidate.Notifications == null)
            {
                candidate.Notifications = new List<Notification>();
            }
            if (candidate.Id == 0)
            {
                candidate.IsNewCandidate = true;
                Notification notification = new Notification();
                notification.InterviewDate = candidate.InterviewDate == DateTime.MinValue ? DateTime.Now : candidate.InterviewDate;
                notification.InterviewTime = candidate.InterviewTime ?? string.Empty;
                notification.Stage = candidate.Stage;
                notification.DTCreate = DateTime.Now;
                candidate.Notifications.Add(notification);
            }
            else
            {
                Candidate existingCandidate = GetCandidate(candidate.Token); //GetCandidateById(candidate.Id);

                if (existingCandidate.Notifications.Find(x => x.Stage.Id == candidate.Stage.Id) == null)
                {
                    var masterData = new MasterDataServiceImpl();
                    Notification notification = new Notification()
                    {
                        Stage = masterData.GetInterviewStage(candidate.Stage.Id),
                        DTCreate = DateTime.Now
                    };
                    existingCandidate.Notifications.Add(notification);
                }
                if (existingCandidate.Stage.Name != candidate.Stage.Name || (candidate.InterviewType != null && (
                    existingCandidate.InterviewType.Name != candidate.InterviewType.Name ||
                    existingCandidate.InterviewDate != candidate.InterviewDate ||
                    existingCandidate.InterviewTime != candidate.InterviewTime)))
                {
                    emailSvc = new EmailServiceImpl();
                    emailSvc.CandidateMail(new Mail()
                    {
                        Id = candidate.Token,
                        CandidateName = string.Concat(candidate.FirstName, " ", candidate.LastName),
                        CandidateStage = candidate.Stage.Name,
                        FromEmailId = candidate.Recruiter.Email,
                        ToEmailId = candidate.Email
                    });

                    existingCandidate.IsActive = true;
                }
                candidate.IsActive = existingCandidate.IsActive;
                candidate.IsNewCandidate = existingCandidate.IsNewCandidate;
                candidate.Notifications = existingCandidate.Notifications;
                candidate.FeedBackComment = existingCandidate.FeedBackComment;
            }
            return candidate;
        }

        public bool UpdateCandidate(Candidate candidate)
        {
            candidate = mapCandidate(candidate);

            return candidateRepo.UpdateCandidate(candidate);
        }

        public Candidate FindCandidate(string filter)
        {
            return candidateRepo.GetCandidate(filter);
        }

        public bool UpdateCandidateStatus(CandidateStatus status)
        {
            Candidate candidate = FindCandidate(status.Token);
            candidateRepo.UpdateCandidateStatus(status);
            emailSvc = new EmailServiceImpl();
            return emailSvc.RecruiterMail(new Mail() { ToEmailId = candidate.Recruiter.Email, FromEmailId = candidate.Email, CandidateName = candidate.FirstName, DeclineReason = status.DeclineReason, CandidateStage = candidate.Stage.Name });
        }

        public string ValidateCandidate(CandidateValidation candidate)
        {
            string isValid = string.Empty;

            if (!string.IsNullOrWhiteSpace(candidate.Mobile))
            {
                Candidate existCandidate = candidateRepo.GetCandidate(candidate.Mobile);

                if (existCandidate != null && existCandidate.Token != candidate.Token)
                {
                    isValid = "The mobile number already has been used";
                }
            }
            else if (!string.IsNullOrWhiteSpace(candidate.Email))
            {
                Candidate existCandidate = candidateRepo.GetCandidate(candidate.Email);

                if (existCandidate != null && existCandidate.Token != candidate.Token)
                {
                    isValid = "The email address already has been used";
                }
            }

            return isValid;
        }

        public CandidateDetails GetCandidateDetails(CandidateLogin candidateLogin)
        {
            return candidateRepo.GetCandidateDetails(candidateLogin);
        }

        public CandidateNotification GetNotifications(string filter)
        {
            CandidateNotification candNotification = new CandidateNotification();

            Candidate cand = candidateRepo.GetCandidate(filter);

            candNotification.Notifications = cand.Notifications.OrderByDescending(x => x.DTCreate).ToList();

            candNotification.CandidateStatus = cand.CandidateStatus.ToString();

            return candNotification;
        }

        public Recruiter GetRecuiterDetails(string filter)
        {
            return candidateRepo.GetCandidate(filter).Recruiter;
        }

        public CandidateDetails GetCandidateBasedOnToken(string filter)
        {
            Candidate candidate = candidateRepo.GetCandidate(filter);

            return new CandidateDetails()
            {
                CandidateStatus = candidate.CandidateStatus,
                Email = candidate.Email,
                FirstName = candidate.FirstName,
                InterviewDate = candidate.InterviewDate,
                InterviewTime = candidate.InterviewTime,
                IsActive = candidate.IsActive,
                IsNewCandidate = candidate.IsNewCandidate,
                JobDescription = candidate.JobDescription,
                LastName = candidate.LastName,
                Mobile = candidate.Mobile,
                PositionName = candidate.Position.Name
            };
        }

        public List<CandidateDetails> GetListCandidate(CandidateDetails candidateDetail, int filter, int numberOfRecord)
        {
            return candidateRepo.GetListCandidate(candidateDetail, filter, numberOfRecord);
        }

        public bool UpdateFeedBack(FeedBack feedBack)
        {
            return candidateRepo.UpdateFeedBack(feedBack);
        }
    }
}
