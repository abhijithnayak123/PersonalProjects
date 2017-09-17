using System;

namespace MGI.Biz.Customer.Data
{
	public class CustomerSearchResult
	{
		public string AlloyID { get; set; }
		public string FullName { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string MothersMaidenName { get; set; }
		public string GovernmentId { get; set; }
		public string CardNumber { get; set; }
		public string SSN { get; set; }
		public string ProfileStatus { get; set; }
	}
}
