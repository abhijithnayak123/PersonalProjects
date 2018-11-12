using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public class Notification : BaseModel
    {
        public DateTime InterviewDate { get; set; }

        public InterviewStage Stage { get; set; }

        public string InterviewTime { get; set; }

    }
}
