using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Fund.FirstView.Data
{
    public class FirstViewCredentials : NexxoModel
    {
        public virtual string ServiceUrl { get; set; }
        public virtual string User { get; set; }
        public virtual string Password { get; set; }
        public virtual string Application { get; set; }
		public virtual long ChannelPartnerId { get; set; }
    	public virtual string CIAClientID { get; set; }
        public virtual int SystemExtLogin { get; set; }
    }
}
