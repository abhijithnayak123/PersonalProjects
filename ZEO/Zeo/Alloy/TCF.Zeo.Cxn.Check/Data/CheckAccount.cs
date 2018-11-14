using System;

namespace TCF.Zeo.Cxn.Check.Data
{
	public class CheckAccount
	{
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string SecondLastName { get; set; }
		public string ITIN { get; set; }
		public string SSN { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Phone { get; set; }
		public string Occupation { get; set; }
		public string Employer { get; set; }
		public string EmployerPhone { get; set; }
		public string GovernmentId { get; set; }
		public string IDType { get; set; }
		public string IDCountry { get; set; }
		public string IDState { get; set; }
		public byte[] IDImage { get; set; }
		public Nullable<DateTime> IDExpireDate { get; set; }
		public long CardNumber { get; set; }
		public int CustomerScore { get; set; }
		public Nullable<DateTime> IDIssueDate { get; set; }
		public string IDCode { get; set; }
	}
}
