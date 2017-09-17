using System;

namespace MGI.Biz.FundsEngine.Data
{
	public class FundsAccount
	{
		public string CardNumber { get; set; }
		public string AccountNumber { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string CountryCode { get; set; }
		public decimal CardBalance { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string SSN { get; set; }
		public string Phone { get; set; }
		public string GovtIDType { get; set; }
		public string GovtIDCountry { get; set; }
		public string GovernmentId { get; set; }
		public DateTime GovtIDExpiryDate { get; set; }
		public DateTime GovtIDIssueDate { get; set; }
		public string GovtIDIssueState { get; set; }
		public string MailingAddress1 { get; set; }
		public string MailingAddress2 { get; set; }
		public string MailingCity { get; set; }
		public string MailingState { get; set; }
		public string MailingZipCode { get; set; }
		public int IdTypeId { get; set; }
		public string Resolution { get; set; }
		public int FraudScore { get; set; }

		public string ProxyId { get; set; }
		public string PseudoDDA { get; set; }
		public string ExpirationDate { get; set; }
		public string FullCardNumber { get; set; }
		public string IDCode { get; set; }
		//AL-2999 Changes
		public string Email { get; set; }
		//AL-3054
		public string MothersMaidenName { get; set; }
	}
}
