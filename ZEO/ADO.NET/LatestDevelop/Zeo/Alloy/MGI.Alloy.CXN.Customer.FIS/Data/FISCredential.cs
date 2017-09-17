
using MGI.Alloy.CXN.Common;

namespace MGI.Alloy.CXN.Customer.FIS.Data 
{
    public class FISCredential : BaseRequest
    {
        public virtual string User { get; set; }
        public virtual string Password { get; set; }
        public virtual string Applicationkey { get; set; }
        public virtual string ChannelKey { get; set; }
		public virtual string BankId { get; set; }
		public virtual string MetBankNumber { get; set; }
        public virtual string RACFUserId { get; set; }
        public virtual string RACFPassword { get; set; }
    }
}