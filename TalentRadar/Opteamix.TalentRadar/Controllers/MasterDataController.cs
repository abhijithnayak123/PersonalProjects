using OPT.TalentRadar.BIZ.Contract;
using OPT.TalentRadar.BIZ.Implementation;
using OPT.TalentRadar.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Opteamix.TalentRadar.Controllers
{
    [RoutePrefix("api/masterdata")]
    public class MasterDataController : ApiController
    {

        IMasterDataService service;

        [Route("positions")]
        public List<Position> GetPositions()
        {
            service = new MasterDataServiceImpl();
            return service.GetAllPostions();  
        }

        [Route("interviewstages")]
        public List<InterviewStage> GetStages()
        {
            service = new MasterDataServiceImpl();

            return service.GetAllInterviewStages();
        }

        [Route("interviewtypes")]
        public List<InterviewType> GetTypes()
        {
            service = new MasterDataServiceImpl();

            return service.GetAllInterviewTypes();
        }

        [Route("practices")]
        public List<Practice> GetPractice()
        {
            service = new MasterDataServiceImpl();

            return service.GetAllPractices();
        }

    }
}
