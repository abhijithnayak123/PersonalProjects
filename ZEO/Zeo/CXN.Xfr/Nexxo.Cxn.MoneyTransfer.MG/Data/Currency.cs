using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.MoneyTransfer.MG.Data
{
	public class Currency : NexxoModel
	{
		public virtual string Code { get; set; }
		public virtual string Name { get; set; }
		public virtual string Currencyprecision { get; set; }
	}
}
