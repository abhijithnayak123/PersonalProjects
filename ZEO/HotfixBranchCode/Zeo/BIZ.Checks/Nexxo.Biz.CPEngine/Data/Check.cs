using System;

namespace MGI.Biz.CPEngine.Data
{
    public class Check
    {
        public string Id { get; set; }
        public DateTime SubmissionDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string StatusMessage { get; set; }
        public string SelectedType { get; set; }
        public string ValidatedType { get; set; }
        public decimal SelectedFee { get; set; }
        public decimal ValidatedFee { get; set; }
        public decimal BaseFee { get; set; }
        public decimal DiscountApplied { get; set; }
        public string DiscountName { get; set; }
        public byte[] Image { get; set; }
        public string DmsDeclineMessage { get; set; }
		public int DeclineCode { get; set; }
    }
}
