namespace MGI.Biz.MoneyTransfer.Data
{
	public class AttributeRequest
	{
		public string ReceiveCountry { get; set; }
		public string ReceiveCurrencyCode { get; set; }
		public string ReceiveAgentId { get; set; }
		public decimal Amount { get; set; }
		public DeliveryService DeliveryService { get; set; }
        public TransferType TransferType { get; set; }
	}
}