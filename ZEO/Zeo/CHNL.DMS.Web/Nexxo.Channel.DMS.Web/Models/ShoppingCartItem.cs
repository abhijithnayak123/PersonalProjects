using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class ShoppingCartItem
    {
        public long  TxnId { get; set; }
        public string Product { get; set; }
        public string Details { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string TxnType { get; set; }
	public string SummaryTitle { get; set; }
	public string DiscountName { get; set; }
	public decimal BaseFee { get; set; }
	public decimal DiscountApplied { get; set; }
	public decimal NetFee { get; set; }
        public string TxnSubType { get; set; }
        public string DmsStatusMessage { get; set; }
    }
}
