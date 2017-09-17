using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.MoneyTransfer.MG.Data
{
	public class StateRegulator : NexxoModel
	{
		public virtual string Dfjurisdiction { get; set; }
		public virtual string Stateregulatorurl { get; set; }
		public virtual string Stateregulatorphone { get; set; }
		public virtual string Languagecode { get; set; }
		public virtual string Translation { get; set; }
	}
}
