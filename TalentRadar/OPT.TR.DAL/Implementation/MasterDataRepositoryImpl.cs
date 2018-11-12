using OPT.TalentRadar.DAL.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPT.TalentRadar.Common.Data;

namespace OPT.TalentRadar.DAL.Implementation
{
    public class MasterDataRepositoryImpl : IMasterDataRepositoy
    {
        IRepository repo;

        public List<InterviewStage> GetAllInterviewStages()
        {
            repo = new RepositoryImpl(TRConstants.InterviewStage);
            return repo.FetchAll<InterviewStage>();
        }

        public List<InterviewType> GetAllInterviewTypes()
        {
            repo = new RepositoryImpl(TRConstants.InterviewType);
            return repo.FetchAll<InterviewType>();
        }

        public List<Position> GetAllPostions()
        {
            repo = new RepositoryImpl(TRConstants.Position);
            return repo.FetchAll<Position>();
        }

        public List<Practice> GetAllPractices()
        {
            repo = new RepositoryImpl(TRConstants.Practice);
            return repo.FetchAll<Practice>();
        }

        public InterviewStage GetInterviewStage(long id)
        {
            repo = new RepositoryImpl(TRConstants.InterviewStage);
            return repo.Fetch<InterviewStage>(id);
        }

        public InterviewType GetInterviewType(long id)
        {
            repo = new RepositoryImpl(TRConstants.InterviewType);
            return repo.Fetch<InterviewType>(id);
        }

        public Position GetPostion(long id)
        {
            repo = new RepositoryImpl(TRConstants.Position);
            return repo.Fetch<Position>(id);
        }

        public Practice GetPractice(long id)
        {
            repo = new RepositoryImpl(TRConstants.Practice);
            return repo.Fetch<Practice>(id);
        }
    }
}
