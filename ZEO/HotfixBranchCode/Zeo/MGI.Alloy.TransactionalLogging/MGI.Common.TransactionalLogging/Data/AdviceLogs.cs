using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Common.TransactionalLogging.Data
{
    public class AdviceLogs
    {
        List<TraceAroundAdviceLog> _TraceAroundAdviceLog  {get; set; }
        List<ActivityAdvice> _ActivityAdvice { get; set; }
        List<ThrowsFaultAdvice> _ThrowsFaultAdvice { get; set; }
    }

    public class ThrowsFaultAdvice
    {
 
    }

    public class ActivityAdvice
    {

    }

    public class TraceAroundAdviceLog
    {
        public string GovernmentIdNumber { get; set; }
        public string SSN { get; set; }
        public string CardNumber { get; set; }
        public string AlloyId { get; set; }
        public string AccountNumber { get; set; }
    }
}
