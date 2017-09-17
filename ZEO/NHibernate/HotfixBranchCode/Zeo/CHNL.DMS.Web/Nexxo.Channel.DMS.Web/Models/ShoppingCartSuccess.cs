using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class ShoppingCartSuccess : BaseModel
    {       
        public decimal CashToCustomer { get; set; }
        public decimal LoadToCard { get; set; }
        public decimal NewAccountBalance { get; set; }
        public List<string> Receipts { get; set; }
        public string PrintData { get; set; }
        public bool CheckOutResult { get; set; }
        public int ReceiptCount { get; set; }
        public string ReceiptType { get; set; }

		//US1421 Changes
		public bool FrankCheck { get; set; }
		public int CheckCount { get; set; }
		public string[] CheckData { get; set; } 
    }
}