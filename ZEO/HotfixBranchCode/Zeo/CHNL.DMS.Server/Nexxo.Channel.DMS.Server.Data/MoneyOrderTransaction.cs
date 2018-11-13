using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class MoneyOrderTransaction
	{
		[DataMember]
		public decimal Amount;
		[DataMember]
		public decimal Fee { get; set; }
		[DataMember]
		public string CheckNumber { get; set; }
		[DataMember]
		public byte[] FrontImage { get; set; }
		[DataMember]
		public byte[] BackImage { get; set; }
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
		public string AccountNumber { get; set; }
		[DataMember]
		public string RoutingNumber { get; set; }
		[DataMember]
		public string MICR { get; set; }
		[DataMember]
		public string PromotionCode { get; set; }
		[DataMember]
		public bool IsSystemApplied { get; set; }

		override public string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("MoneyOrder Transaction:");
			sb.AppendLine(string.Format("	Amount: {0:C}", Amount));
			sb.AppendLine(string.Format("	Fee: {0:C}", Fee));
			sb.AppendLine(" ProviderId: " + ProviderId.ToString());
			sb.AppendLine(" CheckNumber: " + CheckNumber);
			sb.AppendLine("AccountNumber:" + AccountNumber);
			sb.AppendLine("RoutingNumber: " + RoutingNumber);
			sb.AppendLine("MICR: " + MICR);
			return sb.ToString();
		}
	}
}
