using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Fund.FIS.Data
{
    public class FISIOProfile
	{
		public long ProgramId { get; set; }
		public string ExternalKey { get; set; }
		public string CardNumber { get; set; }
		public long AccountId { get; set; }
		public long UserId { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public DateTime DOB { get; set; }
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
