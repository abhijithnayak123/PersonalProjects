using OPT.TalentRadar.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.BIZ.Contract
{
    public interface IMasterDataService
    {
        #region InterViewStage
        InterviewStage GetInterviewStage(long id);

        List<InterviewStage> GetAllInterviewStages();
        #endregion

        #region Position
        Position GetPostion(long id);

        List<Position> GetAllPostions();
        #endregion

        #region Practice
        Practice GetPractice(long id);

        List<Practice> GetAllPractices();
        #endregion

        #region InterviewTypes
        InterviewType GetInterviewType(long id);

        List<InterviewType> GetAllInterviewTypes();

        #endregion
    }
}
