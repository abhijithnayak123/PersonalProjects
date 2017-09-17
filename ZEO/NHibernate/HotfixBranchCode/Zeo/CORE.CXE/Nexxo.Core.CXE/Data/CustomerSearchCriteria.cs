using System;
using System.Text;

namespace MGI.Core.CXE.Data
{
	public class CustomerSearchCriteria
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public Nullable<DateTime> DateOfBirth { get; set; }
		public string Cardnumber { get; set; }
		public string GovernmentId { get; set; }
		public long AlloyID { get; set; }
		public string SSN { get; set; }
		public bool IsIncludeClosed { get; set; }
	}
}
