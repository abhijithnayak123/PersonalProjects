using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.TSys.Data
{
	public class TSysIOProfile
	{
		public long ProgramId { get; set; }
		public string ExternalKey { get; set; }
		public long CardNumber { get; set; }
		public long AccountId { get; set; }
		public long UserId { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string SSN { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string PhoneType { get; set; }
	}
}
