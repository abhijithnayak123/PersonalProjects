using System;
using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
	[DataContract]
	public class CheckTransactionDetails
	{
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string CheckNumber { get; set; }
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public string Type { get; set; }
		[DataMember]
		public decimal Fee { get; set; }
		[DataMember]
		public byte[] ImageFront { get; set; }
		[DataMember]
		public byte[] ImageBack { get; set; }
		[DataMember]
		public string ConfirmationNumber { get; set; }
		[DataMember]
		public string CheckType { get; set; }
		[DataMember]
		public int ProviderId { get; set; }
		[DataMember]
		public decimal BaseFee { get; set; }
		[DataMember]
		public decimal DiscountApplied { get; set; }
		[DataMember]
		public string DiscountName { get; set; }
		[DataMember]
		public string DiscountDescription { get; set; }
		[DataMember]
		public string DeclineMessage { get; set; }
		[DataMember]
		public int DeclineErrorCode { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "CheckNumber = ", CheckNumber, "\r\n");
			str = string.Concat(str, "Amount = ", Amount, "\r\n");
			str = string.Concat(str, "Type = ", Type, "\r\n");
			str = string.Concat(str, "Fee = ", Fee, "\r\n");
			str = string.Concat(str, "ConfirmationNumber = ", ConfirmationNumber, "\r\n");
			str = string.Concat(str, "CheckType = ", CheckType, "\r\n");
			str = string.Concat(str, "ProviderName = ", ProviderId, "\r\n");

			return str;
		}
	}
}
