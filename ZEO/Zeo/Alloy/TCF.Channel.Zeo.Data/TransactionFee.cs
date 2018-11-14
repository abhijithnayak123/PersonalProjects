using System;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Channel.Zeo.Data
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
		public string PromotionCode { get; set; }
		[DataMember]
		public string PromotionDescription { get; set; }
		//US1799 Targeted promotions for check cashing and money order
		[DataMember]
		public bool IsSystemApplied { get; set; }
        [DataMember]
        public decimal AdditionalFee { get; set; }
        [DataMember]
        public long PromotionId { get; set; }
        [DataMember]
        public bool IsOverridable { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public bool IsGroupPromo { get; set; }
        [DataMember]
        public long ProvisionId { get; set; }


        public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Transaction Fee:");
			sb.AppendLine(string.Format("	BaseFee: {0:c}", BaseFee));
			sb.AppendLine(string.Format("	DiscountApplied: {0:c}", DiscountApplied));
			sb.AppendLine(string.Format("	NetFee: {0:c}", NetFee));
			sb.AppendLine(string.Format("	PromotionCode: {0}", PromotionCode));
			sb.AppendLine(string.Format("	PromotionDescription: {0}", PromotionDescription));
            sb.AppendLine(string.Format("	IsSystemApplied: {0}", IsSystemApplied));
            sb.AppendLine(string.Format("	AdditionalFee: {0}", AdditionalFee));
            sb.AppendLine(string.Format("	FeeAdjustmentId: {0}", PromotionId));
            sb.AppendLine(string.Format("	CanOverridePromo: {0}", IsOverridable));
            return sb.ToString();
		}
	}
}
