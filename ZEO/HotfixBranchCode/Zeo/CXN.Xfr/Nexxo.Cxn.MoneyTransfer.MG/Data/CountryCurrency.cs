using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.MoneyTransfer.MG.Data
{
	public class CountryCurrency : NexxoModel
	{
		public virtual string CountryCode { get; set; }
		public virtual string Basecurrency { get; set; }
		public virtual string Localcurrency { get; set; }
		public virtual string Receivecurrency { get; set; }
		public virtual bool Indicativerateavailable { get; set; }
		public virtual string Deliveryoption { get; set; }
	}
}
