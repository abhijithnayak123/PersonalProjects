using System.Collections.Generic;

namespace MGI.Biz.MoneyTransfer.Data
{
	public class FeeInformation
	{
		public decimal Fee { get; set; }
		public decimal Tax { get; set; }
		public decimal ExchangeRate { get; set; }
		public decimal OtherFee { get; set; }
		public decimal OtherTax { get; set; }
		public decimal MessageFee { get; set; }

		public decimal Amount { get; set; }
		public decimal ReceiveAmount { get; set; }
		public decimal Discount { get; set; }
		public decimal TotalAmount { get; set; }
		public string ReferenceNumber { get; set; }
		public string ReceiveCurrencyCode { get; set; }
		public DeliveryService DeliveryService { get; set; }

		public string ReceiveAgentId { get; set; }
		public string ReceiveAgentName { get; set; }
		public string ReceiveAgentAbbreviation { get; set; }

		public Dictionary<string, object> MetaData { get; set; }
	}
}