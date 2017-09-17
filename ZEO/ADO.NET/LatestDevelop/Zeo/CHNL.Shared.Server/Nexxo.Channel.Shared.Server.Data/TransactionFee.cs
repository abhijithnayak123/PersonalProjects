using System;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class TransactionFee
	{
		[DataMember]
		public decimal BaseFee { get; set; }
		[DataMember]
		public decimal DiscountApplied { get; set; }
		[DataMember]
		public decimal NetFee { get; set; }
		[DataMember]
		public string DiscountName { get; set; }
		[DataMember]
		public string DiscountDescription { get; set; }
		//US1799 Targeted promotions for check cashing and money order
		[DataMember]
		public bool IsSystemApplied { get; set; }
		

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Transaction Fee:");
			sb.AppendLine(string.Format("	BaseFee: {0:c}", BaseFee));
			sb.AppendLine(string.Format("	DiscountApplied: {0:c}", DiscountApplied));
			sb.AppendLine(string.Format("	NetFee: {0:c}", NetFee));
			sb.AppendLine(string.Format("	DiscountName: {0}", DiscountName));
			sb.AppendLine(string.Format("	DiscountDescription: {0}", DiscountDescription));
			return sb.ToString();
		}
	}
}
