using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public class CandidateNotification 
    {
        public string CandidateStatus { get; set; }

        public List<Notification> Notifications { get; set; }
    }
}
