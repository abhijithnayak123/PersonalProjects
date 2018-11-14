using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyOrderEngine.Data
{    
    public class MoneyOrder
    {
        public string Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Amount { get; set; }
		public decimal BaseFee { get; set; }
		public decimal DiscountApplied { get; set; }
		public string DiscountName { get; set; }
        public decimal Fee { get; set; }
        public string CheckNumber { get; set; }
		public string AccountNumber { get; set; }
		public string RoutingNumber { get; set; }
		public string MICR { get; set; }
		public byte[] FrontImage { get; set; }
		public byte[] BackImage { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
    }
}
