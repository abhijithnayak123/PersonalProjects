namespace TCF.Zeo.Core.Data
{
    public class TransactionFee
    {
        public decimal BaseFee { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal NetFee { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionDescription { get; set; }
        public bool IsSystemApplied { get; set; }
        public decimal AdditionalFee { get; set; }
        public long PromotionId { get; set; }
        public bool IsOverridable { get; set; }
        public int Priority { get; set; }
        public bool IsGroupPromo { get; set; }
        public long ProvisionId { get; set; }
    }
}
