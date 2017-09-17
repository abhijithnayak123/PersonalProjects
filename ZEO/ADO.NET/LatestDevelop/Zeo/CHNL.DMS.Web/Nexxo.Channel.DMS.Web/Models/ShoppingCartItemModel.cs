using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class ShoppingCartItemModel : BaseModel
    {
        public string CartId { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public int Transactions { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public decimal Fee { get; set; }
        public decimal NetAmount { get; set; }
        public string TransactionType { get; set; }
        public string CheckID { get; set; }
        public string BillPayee { get; set; }
        public string AccountNumber { get; set; }
        public string SummaryTitle { get; set; }
        public string checkImage { get; set; }      
        public string Service { get; set; }

		public decimal BaseFee { get; set; }
		public decimal DiscountApplied { get; set; }
		public decimal NetFee { get; set; }
		public string DiscountName { get; set; }
		public string DiscountDescription { get; set; }
    }
}