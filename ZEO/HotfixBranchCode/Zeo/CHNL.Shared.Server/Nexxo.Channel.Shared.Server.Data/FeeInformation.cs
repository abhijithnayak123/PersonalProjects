using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class FeeInformation
	{
		[DataMember]
		public decimal Fee { get; set; }
		[DataMember]
		public decimal Tax { get; set; }
		[DataMember]
		public decimal ExchangeRate { get; set; }
		[DataMember]
		public decimal OtherFee { get; set; }
		[DataMember]
		public decimal OtherTax { get; set; }
		[DataMember]
		public decimal MessageFee { get; set; }

		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public decimal ReceiveAmount { get; set; }
		[DataMember]
		public decimal Discount { get; set; }
		[DataMember]
		public decimal TotalAmount { get; set; }
		[DataMember]
		public string ReferenceNumber { get; set; }
		[DataMember]
		public string ReceiveCurrencyCode { get; set; }
		[DataMember]
		public DeliveryService DeliveryService { get; set; }
		[DataMember]
		public string ReceiveAgentId { get; set; }
		[DataMember]
		public string ReceiveAgentName { get; set; }
		[DataMember]
		public string ReceiveAgentAbbreviation { get; set; }

		[DataMember]
		public Dictionary<string, object> MetaData { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Fee = ", Fee, "\r\n");
			str = string.Concat(str, "Tax = ", Tax, "\r\n");
			str = string.Concat(str, "ExchangeRate = ", ExchangeRate, "\r\n");
			str = string.Concat(str, "OtherFee = ", OtherFee, "\r\n");
			str = string.Concat(str, "OtherTax = ", OtherTax, "\r\n");
			str = string.Concat(str, "MessageFee = ", MessageFee, "\r\n");
			str = string.Concat(str, "Amount = ", Amount, "\r\n");
			str = string.Concat(str, "ReceiveAmount = ", ReceiveAmount, "\r\n");
			str = string.Concat(str, "Discount = ", Discount, "\r\n");
			str = string.Concat(str, "TotalAmount = ", TotalAmount, "\r\n");
			str = string.Concat(str, "ReferenceNumber = ", ReferenceNumber, "\r\n");
			str = string.Concat(str, "ReceiveCurrencyCode = ", ReceiveCurrencyCode, "\r\n");
			str = string.Concat(str, "DeliveryService = ", DeliveryService, "\r\n");
			str = string.Concat(str, "ReceiveAgentId = ", ReceiveAgentId, "\r\n");
			str = string.Concat(str, "ReceiveAgentName = ", ReceiveAgentName, "\r\n");
			str = string.Concat(str, "ReceiveAgentAbbreviation = ", ReceiveAgentAbbreviation, "\r\n");
			if (MetaData != null)
			{
				foreach (KeyValuePair<string, object> meta in MetaData)
				{
					str = string.Concat(str, meta.Key + " = ", meta.Value, "\r\n");
				}
			}
			return str;
		}
	}
}