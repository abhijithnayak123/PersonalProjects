using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Customer.FIS.Data 
{
    public class FISCredential : NexxoModel
    {
        public virtual string User { get; set; }
        public virtual string Password { get; set; }
        public virtual string Applicationkey { get; set; }
        public virtual string ChannelKey { get; set; }
        public virtual long ChannelPartnerId { get; set; }
		public virtual string BankId { get; set; }
		public virtual string MetBankNumber { get; set; }
        public virtual string RACFUserId { get; set; }
        public virtual string RACFPassword { get; set; }
    }
}