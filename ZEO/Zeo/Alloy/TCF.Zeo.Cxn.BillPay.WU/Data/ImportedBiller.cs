using TCF.Zeo.Common.Data;
using System;

namespace TCF.Zeo.Cxn.BillPay.WU.Data
{
    public class ImportedBiller : ZeoModel
    {
        public string BillerName { get; set; }
        public string AccountNumber { get; set; }
        public string CardNumber { get; set; }
        public string WUIndex { get; set; }
        public long WUAccountId { get; set; }
        public long AgentSessionId { get; set; }
        public long CustomerSessionId { get; set; }
    }
}
