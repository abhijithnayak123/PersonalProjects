using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.MoneyTransfer.MG.Data
{
	public class Country : NexxoModel
	{
		public virtual string Code { get; set; }
		public virtual string Name { get; set; }
		public virtual string Legacycode { get; set; }
		public virtual bool Sendactive { get; set; }
		public virtual bool Receiveactive { get; set; }
		public virtual bool Directedsendcountry { get; set; }
		public virtual bool Mgdirectedsendcountry { get; set; }
	}
}
