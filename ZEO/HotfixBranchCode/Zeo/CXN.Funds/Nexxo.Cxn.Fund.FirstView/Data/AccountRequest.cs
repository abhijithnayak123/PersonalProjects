using System;

namespace MGI.Cxn.Fund.FirstView.Data
{
	public class AccountRequest : Request
	{
		public long? deCIAPrimaryAccountNumber { get; set; }
		public string deCIAClientID { get; set; }
		public string deCIAFirstName { get; set; }
		public string deCIAMiddleName { get; set; }
		public string deCIALastName { get; set; }
		public string deCIADateOfBirth { get; set; }
		public string deCIASSNNumber { get; set; }
		public string deCIAGovernmentID { get; set; }
		public string deCIAAddressLine1 { get; set; }
		public string deCIAAddressLine2 { get; set; }
		public string deCIACity { get; set; }
		public string deCIAState { get; set; }
		public string deCIACountryOfIssue { get; set; }
		public string deCIAPostalCode { get; set; }
		public string deCIAHomePhoneNumber { get; set; }
		public string deCIAIDNumber { get; set; }

		public string deCIAIDIssueDate { get; set; }
		public string deCIAIDExpirationDate { get; set; }
		public string deCIAIDIssueCountry { get; set; }
		public string deCIAIDIssueState { get; set; }
		public string deCIAEmailAddress { get; set; }
		public string deCIAField1 { get; set; }
		public string deCIAField2 { get; set; }
		public string deCIAField3 { get; set; }
		public string deCIAField4 { get; set; }
		public string deCIAField5 { get; set; }
		public string Reissue { get; set; }
		
		public long? deCIASHomePhoneCCode { get; set; }
		public long? deCIASHomePhoneExt { get; set; }
		public string deCIAShipCmpnyName { get; set; }
		public string deCIAShipContactName { get; set; }
		public string deCIAShipAddress1 { get; set; }
		public string deCIAShipAddress2 { get; set; }
		public string deCIAShipToCity { get; set; }
		public string deCIAShipToState { get; set; }
		public string deCIAShipToZipCode { get; set; }
	}
}
