using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class FeeRequest
	{
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public decimal ReceiveAmount { get; set; }
		[DataMember]
		public FeeRequestType FeeRequestType { get; set; }

		[DataMember]
		public string ReceiveCountryCode { get; set; }
		[DataMember]
		public string ReceiveCountryCurrency { get; set; }
		[DataMember]
		public string PromoCode { get; set; }
		[DataMember]
		public string PersonalMessage { get; set; }
		[DataMember]
		public string ReferenceNo { get; set; }

		[DataMember]
		public long AccountId { get; set; }
		[DataMember]
		public long ReceiverId { get; set; }
		[DataMember]
		public long TransactionId { get; set; }
		[DataMember]
		public bool IsDomesticTransfer { get; set; }

		[DataMember]
		public string ReceiverFirstName { get; set; }
		[DataMember]
		public string ReceiverMiddleName { get; set; }
		[DataMember]
		public string ReceiverLastName { get; set; }
		[DataMember]
		public string ReceiverSecondLastName { get; set; }

		[DataMember]
		public DeliveryService DeliveryService { get; set; }

		[DataMember]
		public string ReceiveAgentId { get; set; }

		[DataMember]
		public Dictionary<string, object> MetaData { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Amount = ", Amount, "\r\n");
			str = string.Concat(str, "ReceiveAmount = ", ReceiveAmount, "\r\n");
			str = string.Concat(str, "ReceiveCountryCode = ", ReceiveCountryCode, "\r\n");
			str = string.Concat(str, "ReceiveCountryCurrency = ", ReceiveCountryCurrency, "\r\n");
			str = string.Concat(str, "PromoCode = ", PromoCode, "\r\n");
			str = string.Concat(str, "PersonalMessage = ", PersonalMessage, "\r\n");
			str = string.Concat(str, "ReferenceNo = ", ReferenceNo, "\r\n");
			str = string.Concat(str, "AccountId = ", AccountId, "\r\n");
			str = string.Concat(str, "ReceiverId = ", ReceiverId, "\r\n");
			str = string.Concat(str, "TransactionId = ", TransactionId, "\r\n");
			str = string.Concat(str, "IsDomesticTransfer = ", IsDomesticTransfer, "\r\n");
			str = string.Concat(str, "ReceiverFirstName = ", ReceiverFirstName, "\r\n");
			str = string.Concat(str, "ReceiverLastName = ", ReceiverLastName, "\r\n");
			str = string.Concat(str, "ReceiverSecondLastName = ", ReceiverSecondLastName, "\r\n");
			str = string.Concat(str, "ReceiverMiddleName = ", ReceiverMiddleName, "\r\n");
			str = string.Concat(str, "DeliveryService = ", DeliveryService, "\r\n");
			str = string.Concat(str, "ReceiveAgentId = ", ReceiveAgentId, "\r\n");

			if (MetaData != null)
			{
				foreach (KeyValuePair<string, object> meta in MetaData)
				{
					str = string.Concat(str, meta.Key + "=", meta.Value, "\r\n");
				}
			}
			return str;
		}
	}
}
