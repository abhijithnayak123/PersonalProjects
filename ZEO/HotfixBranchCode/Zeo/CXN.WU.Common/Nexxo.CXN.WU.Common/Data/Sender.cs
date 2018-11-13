namespace MGI.Cxn.WU.Common.Data
{
    
	public class Sender
	{
		public WUEnums.name_type NameType { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }

		public string AddressAddrLine1 { get; set; }
		public string AddressAddrLine2 { get; set; }
		public string AddressCity { get; set; }
		public string AddressState { get; set; }
		public string AddressPostalCode { get; set; }
		public string AddressStreet { get; set; }
		public string AddressStateZip { get; set; }

		public string PreferredCustomerAccountNumber { get; set; }
		public string PreferredCustomerLevelCode { get; set; }

		public string Email { get; set; }
		public string ContactPhone { get; set; }
		public string MobilePhone { get; set; }
		public string SmsNotificationFlag { get; set; }
		public string CountryName { get; set; }
		public string CountryCode { get; set; }
		public string CurrencyCode { get; set; }

		public string SenderComplianceDetailsComplianceDataBuffer { get; set; }
        	public string SMSNotificationFlag { get; set; }
		public string PreferredCustomerPermanentChange{ get; set; }
	}
}
