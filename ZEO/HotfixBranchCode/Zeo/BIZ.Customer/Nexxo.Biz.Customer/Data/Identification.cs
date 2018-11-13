using System;

namespace MGI.Biz.Customer.Data
{
	public class Identification
	{
		public string Country { get; set; }
		public string CountryOfBirth { get; set; }
		public string IDType { get; set; }
		public string State { get; set; }
		public string GovernmentId { get; set; }
        public Nullable<DateTime> IssueDate { get; set; }
        public Nullable<DateTime> ExpirationDate { get; set; }
        //Added to populate ID Type Desc from NexxoIDTypes for Get Customer for KIOSK
        public string IDTypeName { get; set; } 
	}
}
