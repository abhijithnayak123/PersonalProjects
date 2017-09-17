using System;
using MGI.Common.Util;

namespace MGI.Cxn.Customer.Data
{
	public class CustomerProfile
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string LastName2 { get; set; }
		public string MothersMaidenName { get; set; }
		public Nullable<DateTime> DateOfBirth { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string Phone1 { get; set; }
		public string Phone2 { get; set; }
		public string Email { get; set; }
		public string SSN { get; set; }
		public int ChannelPartnerId { get; set; }
		public string Gender { get; set; }
		public string PartnerAccountNumber { get; set; }
		public string RelationshipAccountNumber { get; set; }
		public ProfileStatus ProfileStatus { get; set; }
		public string BankId { get; set; }
		public string BranchId { get; set; }
		public string ProgramId { get; set; }
		public string Phone1Type { get; set; }
		public string PIN { get; set; }

		public string EmployerName { get; set; }
		public string EmployerPhone { get; set; }
		public string Occupation { get; set; }
		public string OccupationDescription { get; set; }
		public string GovernmentIDType { get; set; }
		public string GovernmentId { get; set; }
		public string IDIssuingCountry { get; set; }
		public string IDIssuingState { get; set; }
		public Nullable<DateTime> IDIssueDate { get; set; }
		public DateTime IDExpirationDate { get; set; }
		public string ClientID { get; set; }
		public string LegalCode { get; set; }
		public string PrimaryCountryCitizenShip { get; set; }
		public string SecondaryCountryCitizenShip { get; set; }
		public string CountryOfBirth { get; set; }

		public string Phone1Provider { get; set; }
		public string Phone2Type { get; set; }
		public string Phone2Provider { get; set; }
		public bool MailingAddressDifferent { get; set; }
		public string MailingAddress1 { get; set; }
		public string MailingAddress2 { get; set; }
		public string MailingCity { get; set; }
		public string MailingState { get; set; }
		public string MailingZipCode { get; set; }
		public bool CustInd { get; set; }
		public string IDCode { get; set; }
	}
}
