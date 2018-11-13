using System.Collections.Generic;

namespace MGI.Biz.MoneyTransfer.Data
{
	public class FeeRequest
	{
		public decimal Amount { get; set; }
		public decimal ReceiveAmount { get; set; }
		public FeeRequestType FeeRequestType { get; set; }

		public string ReceiveCountryCode { get; set; }
		public string ReceiveCountryCurrency { get; set; }
		public string PromoCode { get; set; }
		public string PersonalMessage { get; set; }
		public string ReferenceNo { get; set; }

		public long AccountId { get; set; }
		public long ReceiverId { get; set; }
		public long TransactionId { get; set; }
		public bool IsDomesticTransfer { get; set; }

		public string ReceiverFirstName { get; set; }
		public string ReceiverLastName { get; set; }
		public string ReceiverSecondLastName { get; set; }
		public string ReceiverMiddleName { get; set; }

		public DeliveryService DeliveryService { get; set; }
		public Dictionary<string, object> MetaData { get; set; }
	}
}
