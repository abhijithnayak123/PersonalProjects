using System;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public class TransactionHistory
    {
        public TransactionHistory() { }

		[DataMember]
		public long CustomerId { get; set; }
		[DataMember]
		public string CustomerName { get; set; }
		[DataMember]
		public DateTime TransactionDate { get; set; }
		[DataMember]
		public string Teller { get; set; }
		[DataMember]
		public long SessionId { get; set; }
		[DataMember]
		public long TransactionId { get; set; }
		[DataMember]
		public string Location { get; set; }
		[DataMember]
		public string TransactionType { get; set; }
		[DataMember]
		public string TransactionDetail { get; set; }
		[DataMember]
		public string TransactionStatus { get; set; }
		[DataMember]
		public decimal TotalAmount { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "CustomerId = ", CustomerId, "\r\n");
			str = string.Concat(str, "CustomerName = ", CustomerName, "\r\n");
			str = string.Concat(str, "TransactionDate = ", TransactionDate, "\r\n");
			str = string.Concat(str, "Teller = ", Teller, "\r\n");
			str = string.Concat(str, "SessionId = ", SessionId, "\r\n");
			str = string.Concat(str, "TransactionId = ", TransactionId, "\r\n");
			str = string.Concat(str, "Location = ", Location, "\r\n");
			str = string.Concat(str, "TransactionType = ", TransactionType, "\r\n");
			str = string.Concat(str, "TransactionDetail = ", TransactionDetail, "\r\n");
			str = string.Concat(str, "TransactionStatus = ", TransactionStatus, "\r\n");
			str = string.Concat(str, "TotalAmount = ", TotalAmount, "\r\n");
			return str;
		}
	}
}
