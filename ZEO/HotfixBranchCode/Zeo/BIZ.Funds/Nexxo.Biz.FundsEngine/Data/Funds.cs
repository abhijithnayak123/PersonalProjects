namespace MGI.Biz.FundsEngine.Data
{
    public class Funds
    {
        public FundsAccount Account { get; set; }
        public decimal Amount { get; set; }
		public decimal BaseFee { get; set; }
		public decimal DiscountApplied { get; set; }
		public string DiscountName { get; set; }
        public decimal Fee { get; set; }
        public FundType FundsType { get; set; }
        public string FundDescription { get; set; }
		public string PromoCode { get; set; }
    }
}
