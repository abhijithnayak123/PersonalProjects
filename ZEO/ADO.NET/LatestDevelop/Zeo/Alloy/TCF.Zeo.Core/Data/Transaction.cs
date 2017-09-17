using TCF.Zeo.Common.Data;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data
{
    public abstract class Transaction : ZeoModel
    {        
        public TransactionType Type { get; set; }
        public TransactionBehavior Behavior { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Description { get; set; }
        public int State { set; get; }
        public string ConfirmationNumber { get; set; }
        public long TransactionId { set; get; }
    }
}
