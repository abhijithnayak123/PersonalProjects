using System.Collections.Generic;

namespace MGI.Biz.MoneyTransfer.Data
{
	public class DeliveryServiceRequest
	{
		public DeliveryServiceType Type { get; set; }
		public string CountryCode { get; set; }
		public string CountryCurrency { get; set; }
		public long CustomerSessionId { get; set; }
		public Dictionary<string, object> MetaData { get; set; }
	}
}
