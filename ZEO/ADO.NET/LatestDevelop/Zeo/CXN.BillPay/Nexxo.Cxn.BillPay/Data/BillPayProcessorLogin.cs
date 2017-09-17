using System;

namespace MGI.Cxn.BillPay.Data
{
    public class BillPayProcessorLogin
    {
        public virtual Guid ID { get; set; }
		public virtual string ServiceURL { get; set; }
		public virtual int AgentID { get; set; }
		public virtual int ProcessorID { get; set; }
		public virtual string ServiceNamespace { get; set; }
		public virtual string ClerkID { get; set; }
		public virtual string SecureKey { get; set; }
		public virtual string TerminalID { get; set; }
        public virtual string ServicePartnerId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
    }
}
