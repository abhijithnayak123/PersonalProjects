using System;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class MoneyOrderPurchase
	{
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public decimal Fee { get; set; }
		[DataMember]
		public string PromotionCode { get; set; }
		[DataMember]
		public bool IsSystemApplied { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Amount = ", Amount, "\r\n");
			str = string.Concat(str, "Fee = ", Fee, "\r\n");
			return str;
		}
	}
}
