using OPT.TalentRadar.BIZ.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPT.TalentRadar.Common.Data;
using OPT.TalentRadar.DAL.Contract;
using OPT.TalentRadar.DAL.Implementation;
namespace OPT.TalentRadar.BIZ.Implementation
{
    public class MasterDataServiceImpl : IMasterDataService
    {
        IMasterDataRepositoy repo;
        public MasterDataServiceImpl()
        {
            repo = new MasterDataRepositoryImpl();
        }
        public List<InterviewStage> GetAllInterviewStages()
        {
            return repo.GetAllInterviewStages();
        }

        public List<InterviewType> GetAllInterviewTypes()
        {
            return repo.GetAllInterviewTypes();
        }

        public List<Position> GetAllPostions()
        {
            return repo.GetAllPostions();
        }

        public List<Practice> GetAllPractices()
        {
            return repo.GetAllPractices();
        }

        public InterviewStage GetInterviewStage(long id)
        {
            return repo.GetInterviewStage(id);
        }

        public InterviewType GetInterviewType(long id)
        {
            return repo.GetInterviewType(id);
        }

        public Position GetPostion(long id)
        {
            return repo.GetPostion(id);
        }

        public Practice GetPractice(long id)
        {
            return repo.GetPractice(id);
        }
    }
}
