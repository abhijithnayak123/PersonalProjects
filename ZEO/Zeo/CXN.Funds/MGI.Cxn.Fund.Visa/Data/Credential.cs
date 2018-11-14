using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class Credential : NexxoModel
	{
		public virtual string ServiceUrl { get; set; }
		public virtual string CertificateName { get; set; }
		public virtual string UserName { get; set; }
		public virtual string Password { get; set; }
		public virtual long ClientNodeId { get; set; }
		public virtual long CardProgramNodeId { get; set; }
		public virtual long SubClientNodeId { get; set; }
		public virtual string StockId { get; set; }
		public virtual long ChannelPartnerId { get; set; }
		public virtual long VisaLocationNodeId { get; set; }
	}
}
