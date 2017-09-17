	namespace MGI.Cxn.WU.Common.Data
	{
		public class PaymentDetails
		{
			public string expectedPayoutLoc_StateCode { get; set; }
			public string expectedPayoutLoc_City { get; set; }			
			public CountryCurrencyInfo destination_country_currency  { get; set; }
			public CountryCurrencyInfo originating_country_currency { get; set; }
			public CountryCurrencyInfo recording_country_currency { get; set; }
			public WUEnums.Transaction_type transaction_type { set; get; }
			public WUEnums.Payment_type Payment_type { get; set; }
			public bool Transaction_TypeSpecified { get; set; }
			public bool Payment_typeSpecified { get; set; }
			public WUEnums.yes_no Fix_on_send { get; set; }
			public double Exchange_Rate { get; set; }
			public string duplicate_detection_flag { get; set; }
			public string Originating_city { get; set; }
			public string Originating_state { get; set; }
		}

	}

