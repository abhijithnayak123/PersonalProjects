using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.MoneyTransfer.MG.Data
{
	public class DeliveryOption : NexxoModel
	{
		public virtual string OptionId { get; set; }
		public virtual string Deliveryoption { get; set; }
		public virtual string Name { get; set; }
	}
}
