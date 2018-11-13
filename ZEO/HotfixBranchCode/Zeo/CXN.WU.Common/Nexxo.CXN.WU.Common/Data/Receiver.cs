using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.WU.Common.Data
{
	public class Receiver 
	{
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string SecondLastName { get; set; }
        public virtual string Status { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Country { get; set; }
		public virtual string CountryCode { get; set; }
		public virtual string CurrencyCode { get; set; }
        public virtual string City { get; set; }
        public virtual string State_Province { get; set; }
        public virtual string ZipCode { get; set; }
		public virtual string ContactPhone { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PickupCountry { get; set; }
        public virtual string PickupState_Province { get; set; }
        public virtual string PickupCity { get; set; }
        public virtual string DeliveryMethod { get; set; }
        public virtual string DeliveryOption { get; set; }
        public virtual string Occupation { get; set; }
		public virtual System.Nullable<System.DateTime> DateOfBirth { get; set; }
        public virtual string CountryOfBirth { get; set; }
        public virtual System.Nullable<long> CustomerId { get; set; }
		public virtual string MiddleName { get; set; }
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
