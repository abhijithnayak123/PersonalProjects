using System.Collections.Generic;
namespace MGI.Biz.MoneyTransfer.Data
{
	public class ValidateRequest
	{
		public decimal Amount { get; set; }
		public decimal ReceiveAmount { get; set; }

		public decimal Fee { get; set; }
		public decimal Tax { get; set; }
		public decimal ExchangeRate { get; set; }
		public decimal OtherFee { get; set; }
		public decimal OtherTax { get; set; }
		public decimal MessageFee { get; set; }
		public decimal Discount { get; set; }
		public decimal TotalAmount { get; set; }
		public long TransactionId { get; set; }
		public TransferType TransferType { get; set; }
		public long ReceiverId { get; set; }

		public decimal DateOfBirth { get; set; }
		public string Occupation { get; set; }
		public string CountryOfBirth { get; set; }

		public string PrimaryIdType { get; set; }
		public string PrimaryIdCountryOfIssue { get; set; }
		public string PrimaryIdPlaceOfIssue { get; set; }
		public string PrimaryIdNumber { get; set; }
		public string PrimaryCountryOfIssue { get; set; }

		public string SecondIdNumber { get; set; }

		public string PersonalMessage { get; set; }

		public bool ProceedWithLPMTError { get; set; }

		public string State { get; set; }
		public string ReferenceNumber { get; set; }
		public string PromoCode { get; set; }
		public string DeliveryService { get; set; }
		public string ReceiveCurrency { get; set; }
		public string IdentificationQuestion { get; set; }
		public string IdentificationAnswer { get; set; }

		public string ReceiverFirstName { get; set; }
		public string ReceiverLastName { get; set; }
		public string ReceiverSecondLastName { get; set; }

		public Dictionary<string, object> MetaData { get; set; }
		public Dictionary<string, string> FieldValues { get; set; }
	}
}