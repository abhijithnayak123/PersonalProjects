using System;
using System.Collections.Generic;

namespace MGI.CXN.MG.Common.Data
{
	public class CTResponse
	{
		public string Version { get; set; }
		public DateTime TimeStamp { get; set; }
		public List<StateProvince> StateProvinces { get; set; }
		public List<Country> Countries { get; set; }
		public List<CountryCurrency> CountryCurrencies { get; set; }
		public List<Currency> Currencies { get; set; }
		public List<DeliveryOption> DeliveryOptions { get; set; } 
	}
}
