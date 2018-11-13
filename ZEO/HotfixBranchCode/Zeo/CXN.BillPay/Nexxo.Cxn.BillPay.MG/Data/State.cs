using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.BillPay.MG.Data
{
	public class State : NexxoModel
	{
		public virtual string Code { get; set; }
		public virtual string Name { get; set; }
		public virtual string Countrycode { get; set; }
	}
}
