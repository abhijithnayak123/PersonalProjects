using System.Collections.Generic;
namespace MGI.Cxn.MoneyTransfer.Data
{
	public class ValidateRequest
	{
		public long TransactionId { get; set; }
		public MoneyTransferType TransferType { get; set; }

		public string DateOfBirth { get; set; }
		public string Occupation { get; set; }
		public string CountryOfBirth { get; set; }
		public string CountryOfBirthAbbr2 { get; set; }
		public string CountryOfBirthAbbr3 { get; set; }

		public string PrimaryIdType { get; set; }
		public string PrimaryIdCountryOfIssue { get; set; }
		public string PrimaryIdPlaceOfIssue { get; set; }
		public string PrimaryCountryOfIssue { get; set; }
		public string PrimaryCountryCodeOfIssue { get; set; }
		public string PrimaryIdNumber { get; set; }

		public string SecondIdNumber { get; set; }
		public string SecondIdType { get; set; }
		public string SecondIdCountryOfIssue { get; set; }

		public string DeliveryService { get; set; }
		public string ReceiveCurrency { get; set; }
		public string PersonalMessage { get; set; }

		public string State { get; set; }

		public long ReceiverId { get; set; }
		public string ReceiverFirstName { get; set; }
		public string ReceiverLastName { get; set; }
		public string ReceiverMiddleName { get; set; }
		public string ReceiverSecondLastName { get; set; }

		public string IdentificationQuestion { get; set; }
		public string IdentificationAnswer { get; set; }

		public Dictionary<string, object> MetaData { get; set; }
		public Dictionary<string, string> FieldValues { get; set; }

	}
}
