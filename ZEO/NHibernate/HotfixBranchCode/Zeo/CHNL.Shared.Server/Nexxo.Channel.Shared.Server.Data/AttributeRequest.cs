using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class AttributeRequest
	{
		[DataMember]
		public string ReceiveCountry { get; set; }
		[DataMember]
		public string ReceiveCurrencyCode { get; set; }
		[DataMember]
		public string ReceiveAgentId { get; set; }
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public DeliveryService DeliveryService { get; set; }
        [DataMember]
        public TransferType TransferType { get; set; }
	}
}