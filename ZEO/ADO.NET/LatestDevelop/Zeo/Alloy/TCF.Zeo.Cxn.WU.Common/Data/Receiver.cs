namespace TCF.Zeo.Cxn.WU.Common.Data
{
	public class Receiver 
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
		public string CountryCode { get; set; }
		public string CurrencyCode { get; set; }
        public string City { get; set; }
        public string State_Province { get; set; }
        public string ZipCode { get; set; }
		public string ContactPhone { get; set; }
        public string PhoneNumber { get; set; }
        public string PickupCountry { get; set; }
        public string PickupState_Province { get; set; }
        public string PickupCity { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryOption { get; set; }
        public string Occupation { get; set; }
		public System.Nullable<System.DateTime> DateOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public System.Nullable<long> CustomerId { get; set; }
		public string MiddleName { get; set; }
		public WUEnums.name_type NameType { get; set; }
		public Address Address { get; set; }

        //This is added for User Story # US1645 and # US1646.

        public string BusinessName { get; set; }
        public string Attention { get; set; }
        public string given_name { get; set; }
        public string paternal_name { get; set; }
        public string maternal_name { get; set; }
        public string ReceiverIndexNumber { get; set; }
        public string Type { get; set; }
    }
	
}
