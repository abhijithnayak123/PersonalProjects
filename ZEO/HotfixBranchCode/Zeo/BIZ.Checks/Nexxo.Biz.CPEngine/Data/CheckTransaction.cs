using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.CPEngine.Data
{
    public class CheckTransaction
    {
        public string Id { get; set; }
        public string CheckNumber { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public decimal BaseFee { get; set; }
        public decimal DiscountApplied { get; set; }
        public string DiscountName { get; set; }
        public string DiscountDescription { get; set; }
        public decimal Fee { get; set; }
        public byte[] ImageFront { get; set; }
        public byte[] ImageBack { get; set; }
        public string ConfirmationNumber { get; set; }
        public string CheckType { get; set; }
        public int ProviderId { get; set; }
        public string DeclineMessage { get; set; }
        public string Status { get; set; }
        public string DmsDeclineMessage { get; set; }
		public int DeclineErrorCode { get; set; }
    }
}
