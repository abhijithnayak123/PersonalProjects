using OPT.TalentRadar.BIZ.Implementation;
using OPT.TalentRadar.Common.Data;
using OPT.TalentRadar.DAL.Contract;
using OPT.TalentRadar.DAL.Implementation;
using System;
using System.Collections.Generic;

namespace OPT.TalentRadar.MasterDataUtility
{
    public class Program
    {
        static void Main(string[] args)
        {
            //SendMail();
            PopulatePositions();
            PopulatePractice();
            PopulateInterviewType();
            PopulateInterviewStage();
            AddCandidate();
        }

        //private static void SendMail()
        //{
        //    var emailSvc = new EmailServiceImpl();

        //    emailSvc.SendMail("ksakala@opteamix.com", "Recruiter", "Hi Congratulations, Your resume is shortlisted.");
        //}

        private static void PopulateInterviewStage()
        {
            IRepository repo = new RepositoryImpl(TRConstants.InterviewStage);
            repo.DeleteAll<InterviewStage>();

            List<InterviewStage> stages = new List<InterviewStage>();
            stages.Add(new InterviewStage() { Id = 1, Name = "Shortlisted", DTCreate = DateTime.Now });
            stages.Add(new InterviewStage() { Id = 2, Name = "Interview", DTCreate = DateTime.Now });
            stages.Add(new InterviewStage() { Id = 3, Name = "Offer", DTCreate = DateTime.Now });
            stages.Add(new InterviewStage() { Id = 4, Name = "OnBoarding", DTCreate = DateTime.Now });
            stages.Add(new InterviewStage() { Id = 5, Name = "Rejected", DTCreate = DateTime.Now });

            repo.SaveAll<InterviewStage>(stages);
        }

        private static void PopulateInterviewType()
        {
            IRepository repo = new RepositoryImpl(TRConstants.InterviewType);
            repo.DeleteAll<InterviewType>();

            List<InterviewType> types = new List<InterviewType>();
            types.Add(new InterviewType() { Id = 1, Name = "Face To Face", DTCreate = DateTime.Now });
            types.Add(new InterviewType() { Id = 2, Name = "Telephonic", DTCreate = DateTime.Now });
            types.Add(new InterviewType() { Id = 3, Name = "Skype Call", DTCreate = DateTime.Now });

            repo.SaveAll<InterviewType>(types);
        }

        private static void PopulatePractice()
        {
            IRepository repo = new RepositoryImpl(TRConstants.Practice);
            repo.DeleteAll<Practice>();

            List<Practice> practices = new List<Practice>();
            practices.Add(new Practice() { Id = 1, Name = "BA", DTCreate = DateTime.Now });
            practices.Add(new Practice() { Id = 2, Name = "Emerging Trends", DTCreate = DateTime.Now });
            practices.Add(new Practice() { Id = 3, Name = "Java", DTCreate = DateTime.Now });
            practices.Add(new Practice() { Id = 4, Name = "Microsoft", DTCreate = DateTime.Now });
            practices.Add(new Practice() { Id = 5, Name = "Mobility", DTCreate = DateTime.Now });
            practices.Add(new Practice() { Id = 6, Name = "PMO", DTCreate = DateTime.Now });
            practices.Add(new Practice() { Id = 7, Name = "UI/UX", DTCreate = DateTime.Now });
            practices.Add(new Practice() { Id = 8, Name = "V & V", DTCreate = DateTime.Now });
            practices.Add(new Practice() { Id = 9, Name = "Shared Service", DTCreate = DateTime.Now });

            repo.SaveAll<Practice>(practices);
        }

        private static void PopulatePositions()
        {
            IRepository repo = new RepositoryImpl(TRConstants.Position);
            repo.DeleteAll<Position>();

            List<Position> positions = new List<Position>();
            positions.Add(new Position() { Id = 1, Name = "Analyst", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 2, Name = "Associate", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 3, Name = "Developer", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 4, Name = "Lead Analyst", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 5, Name = "Lead Associate", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 6, Name = "Manager", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 7, Name = "QA Engineer", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 8, Name = "Senior QA Engineer", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 9, Name = "QA Lead", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 10, Name = "QA Manager", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 11, Name = "Senior Analyst", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 12, Name = "Senior Associate", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 13, Name = "Senior Developer", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 14, Name = "Team Lead", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 15, Name = "Technical Lead", DTCreate = DateTime.Now });
            positions.Add(new Position() { Id = 16, Name = "Technical Manager", DTCreate = DateTime.Now });

            repo.SaveAll<Position>(positions);
        }
        private static void AddCandidate()
        {
            Candidate can = new Candidate()
            {
                FirstName = "Kaushik",
                LastName = "Sakala",
                Mobile = "9731111606",
                Email = "ksakala@opteamix.com",
                Recruiter = new Recruiter()
                {
                    FullName = "Chandrashekhar Patil",
                    Email = "cpatil@opteamix.com"
                },
                Position = new Position()
                {
                    Id = 1,
                    Name = "Developer"
                },
                Practice = new Practice()
                {
                    Id = 4,
                    Name = "Microsoft"
                },
                InterviewDate = DateTime.Now,
                IsActive = true,
                Stage = new InterviewStage()
                {
                    Id = 1,
                    Name = "Shortlisted"
                }
            };

            ICandidateRepository repo = new CandidateRepositoryImpl();
            repo.AddCandidate(can);
        }

    }
}