using System;

namespace MGI.Biz.Partner.Data
{
	public class Identification
	{
		public string Country { get; set; }
		public string IDType { get; set; }
		public string State { get; set; }
		public string GovernmentId { get; set; }
        public Nullable<DateTime> IssueDate { get; set; }
		public Nullable<DateTime> ExpirationDate { get; set; }
		public string CountryOfBirth { get; set; }
	}
}
