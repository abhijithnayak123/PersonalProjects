using System;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class MoneyOrder
	{
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public DateTime PurchaseDate { get; set; }
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public decimal BaseFee { get; set; }
		[DataMember]
		public decimal DiscountApplied { get; set; }
        [DataMember]
		public decimal Fee { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
		[DataMember]
		public string AccountNumber { get; set; }
		[DataMember]
		public string RoutingNumber { get; set; }
        [DataMember]
		public string Status { get; set; }
		[DataMember]
		public string StatusDescription { get; set; }
		[DataMember]
		public string DiscountName { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
            str = string.Concat(str, "PurchaseDate = ", PurchaseDate, "\r\n");
			str = string.Concat(str, "Amount = ", Amount, "\r\n");
			str = string.Concat(str, "BaseFee = ", BaseFee, "\r\n");
			str = string.Concat(str, "DiscountApplied = ", DiscountApplied, "\r\n");
			str = string.Concat(str, "Fee = ", Fee, "\r\n");
			str = string.Concat(str, "DiscountName = ", DiscountName, "\r\n");
            str = string.Concat(str, "CheckNumber = ", CheckNumber, "\r\n");
			str = string.Concat(str, "AccountNumber = ", AccountNumber, "\r\n");
			str = string.Concat(str, "RoutingNumber = ", RoutingNumber, "\r\n");
            str = string.Concat(str, "Status = ", Status, "\r\n");
			str = string.Concat(str, "StatusDescription = ", StatusDescription, "\r\n");

			return str;
		}
	}
}
