using System;
using System.Collections.Generic;

namespace MGI.CXN.MG.Common.Data
{
	public class TranslationsResponse
	{
		public string Version { get; set; }
		public DateTime TimeStamp { get; set; }
		public List<CountryTranslation> Countries { get; set; }
		public List<CurrencyTranslation> Currencies { get; set; }
		public List<DeliveryOptionTranslation> DeliveryOptions { get; set; }
		public List<IndustryTranslation> Industries { get; set; }
		public List<FQDOTextTranslation> FQDOTexts { get; set; }
	}
}
